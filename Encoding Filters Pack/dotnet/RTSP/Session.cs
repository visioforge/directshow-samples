using System;
using System.Runtime.InteropServices;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.Rtsp
{
    /// <summary>
    /// File or camera to H.264 + AAC, published by the RTSP sink.
    /// </summary>
    internal sealed class Session
    {
        public const int GraphNotifyMessage = 0x8000 + 1;

        private const string LicenseKey = "";

        private IFilterGraph2 _graph;
        private ICaptureGraphBuilder2 _capture;
        private IMediaControl _control;
        private IMediaEventEx _events;
        private IVideoWindow _window;
        private IBaseFilter _source;
        private IBaseFilter _audioSource;
        private bool _ownAudioSource;

        public void Start(
            IntPtr notifyWindow,
            IntPtr preview,
            int previewWidth,
            int previewHeight,
            bool fileSource,
            string sourceFile,
            DsDevice videoDevice,
            DsDevice audioDevice,
            int videoBitrateKbps,
            bool encodeAudio,
            int audioBitrateKbps,
            string url)
        {
            Stop();

            _graph = (IFilterGraph2)new FilterGraph();
            _capture = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            Check(_capture.SetFiltergraph(_graph), "Attach capture graph");

            _control = (IMediaControl)_graph;
            _events = (IMediaEventEx)_graph;
            Check(_events.SetNotifyWindow(notifyWindow, GraphNotifyMessage, IntPtr.Zero), "Set graph notify window");

            if (fileSource)
            {
                Check(_graph.AddSourceFilter(sourceFile, "Source", out _source), "Add source file");
                _audioSource = _source;
            }
            else
            {
                _source = BindDevice(videoDevice);
                Check(_graph.AddFilter(_source, videoDevice.Name), "Add camera");
                if (audioDevice != null)
                {
                    _audioSource = BindDevice(audioDevice);
                    _ownAudioSource = true;
                    Check(_graph.AddFilter(_audioSource, audioDevice.Name), "Add audio device");
                }
            }

            var tee = Add((IBaseFilter)new InfTee(), "Preview tee");
            var renderer = Add((IBaseFilter)new VideoRenderer(), "Preview");
            var rgb2yuv = AddFromClsid(DSFilterInitInfoConsts.VFRGB2YUV.CLSID, "RGB to YUV");
            var h264 = AddFromClsid(DSFilterInitInfoConsts.VFH264EncoderV10.CLSID, "H.264 encoder");
            var sink = AddFromClsid(DSFilterInitInfoConsts.VFRTSPSink.CLSID, "RTSP sink");
            ApplyLicense(h264);
            ApplyLicense(sink);
            ConfigureH264(h264, videoBitrateKbps);
            ConfigureSink(sink, url);

            IBaseFilter aac = null;
            if (encodeAudio)
            {
                aac = AddFromClsid(DSFilterInitInfoConsts.VFAACEncoderV10.CLSID, "AAC encoder");
                ApplyLicense(aac);
            }

            _window = (IVideoWindow)renderer;
            Check(_window.put_Owner(preview), "Set preview owner");
            Check(_window.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren), "Set preview style");
            PlacePreview(previewWidth, previewHeight);
            Check(_window.put_Visible(OABool.True), "Show preview");

            Check(_capture.RenderStream(null, MediaType.Video, _source, null, tee), "Connect source video to tee");
            Check(_capture.RenderStream(null, MediaType.Video, tee, null, renderer), "Connect preview");
            Check(_capture.RenderStream(null, MediaType.Video, tee, rgb2yuv, h264), "Connect H.264");
            Check(_capture.RenderStream(null, MediaType.Video, h264, null, sink), "Connect RTSP video");

            if (encodeAudio)
            {
                if (_audioSource == null)
                {
                    throw new InvalidOperationException("Select an audio device, or turn audio off.");
                }

                Check(_capture.RenderStream(null, MediaType.Audio, _audioSource, aac, sink), "Connect AAC");
                ConfigureAac(aac, audioBitrateKbps);
            }

            if (fileSource)
            {
                var clock = (IMediaFilter)_graph;
                Check(clock.SetSyncSource(null), "Send the file as fast as the machine allows");
            }

            Check(_control.Run(), "Run");
        }

        public void PlacePreview(int width, int height)
        {
            if (_window == null)
            {
                return;
            }

            _window.SetWindowPosition(0, 0, width, height);
        }

        public bool DrainEvents(out string error)
        {
            error = null;
            if (_events == null)
            {
                return false;
            }

            var finished = false;
            EventCode code;
            IntPtr param1;
            IntPtr param2;
            while (_events.GetEvent(out code, out param1, out param2, 0) == 0)
            {
                if (code == EventCode.Complete)
                {
                    finished = true;
                }
                else if (code == EventCode.ErrorAbort || code == EventCode.UserAbort)
                {
                    finished = true;
                    error = "The graph stopped with " + code + ".";
                }

                _events.FreeEventParams(code, param1, param2);
            }

            return finished;
        }

        public void Stop()
        {
            if (_window != null)
            {
                _window.put_Visible(OABool.False);
                _window.put_Owner(IntPtr.Zero);
                Marshal.ReleaseComObject(_window);
                _window = null;
            }

            if (_control != null)
            {
                _control.Stop();
                Marshal.ReleaseComObject(_control);
                _control = null;
            }

            if (_events != null)
            {
                _events.SetNotifyWindow(IntPtr.Zero, 0, IntPtr.Zero);
                Marshal.ReleaseComObject(_events);
                _events = null;
            }

            Release(ref _source);
            if (_ownAudioSource)
            {
                Release(ref _audioSource);
            }
            else
            {
                _audioSource = null;
            }

            _ownAudioSource = false;

            if (_graph != null)
            {
                Marshal.ReleaseComObject(_graph);
                _graph = null;
            }

            if (_capture != null)
            {
                Marshal.ReleaseComObject(_capture);
                _capture = null;
            }
        }

        private static void ConfigureH264(IBaseFilter filter, int videoBitrateKbps)
        {
            var encoder = filter as IIntelConfigureVideoEncoder;
            if (encoder == null)
            {
                throw new InvalidOperationException("The H.264 encoder has no configuration interface.");
            }

            IntelVideoEncoderParams parameters;
            Check(encoder.GetParams(out parameters), "Read H.264 settings");
            parameters.rc_control.rc_method = IntelVideoEncoderRCMethod.RC_CBR;
            parameters.rc_control.bitrate = (uint)(videoBitrateKbps * 1000);
            Check(encoder.SetParams2(parameters), "Set H.264 bitrate");
        }

        private static void ConfigureAac(IBaseFilter filter, int audioBitrateKbps)
        {
            var encoder = filter as IVFAACEncoder;
            if (encoder == null)
            {
                throw new InvalidOperationException("The AAC encoder has no configuration interface.");
            }

            Check(encoder.SetBitRate((uint)(audioBitrateKbps * 1000)), "Set AAC bitrate");
        }

        private static void ConfigureSink(IBaseFilter filter, string url)
        {
            var sink = filter as IRTSPSinkConfig;
            if (sink == null)
            {
                throw new InvalidOperationException("The RTSP sink has no configuration interface.");
            }

            Check(sink.set_URL(url), "Set RTSP URL");
        }

        private IBaseFilter Add(IBaseFilter filter, string name)
        {
            Check(_graph.AddFilter(filter, name), "Add " + name);
            return filter;
        }

        private IBaseFilter AddFromClsid(Guid clsid, string name)
        {
            var filter = FilterGraphTools.AddFilterFromClsid(_graph, clsid, name);
            if (filter == null)
            {
                throw new InvalidOperationException("The " + name + " filter is not registered. Install the Encoding Filters Pack.");
            }

            return filter;
        }

        private static IBaseFilter BindDevice(DsDevice device)
        {
            var iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out var source);
            var filter = source as IBaseFilter;
            if (filter == null)
            {
                throw new InvalidOperationException("Cannot open " + device.Name + ".");
            }

            return filter;
        }

        private static void ApplyLicense(IBaseFilter filter)
        {
            if (string.IsNullOrEmpty(LicenseKey))
            {
                return;
            }

            var register = filter as IVFRegister;
            if (register != null)
            {
                register.SetLicenseKey(LicenseKey);
            }
        }

        private static void Check(int hr, string step)
        {
            if (hr >= 0)
            {
                return;
            }

            string text;
            try
            {
                DsError.ThrowExceptionForHR(hr);
                text = "0x" + hr.ToString("X8");
            }
            catch (Exception ex)
            {
                text = ex.Message;
            }

            throw new InvalidOperationException(step + " failed. " + text);
        }

        private static void Release(ref IBaseFilter filter)
        {
            if (filter == null)
            {
                return;
            }

            Marshal.ReleaseComObject(filter);
            filter = null;
        }
    }
}
