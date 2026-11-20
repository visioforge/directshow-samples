using System;
using System.Runtime.InteropServices;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.FileEncode
{
    internal sealed class EncoderChoice
    {
        public EncoderChoice(string caption, VFMFVideoEncoder encoder, bool available)
        {
            Caption = available ? caption : caption + " (not installed)";
            Encoder = encoder;
            Available = available;
        }

        public string Caption { get; private set; }

        public VFMFVideoEncoder Encoder { get; private set; }

        public bool Available { get; private set; }

        public override string ToString()
        {
            return Caption;
        }
    }

    internal sealed class MuxChoice
    {
        public MuxChoice(string caption, VFMFMuxer muxer, string extension)
        {
            Caption = caption;
            Muxer = muxer;
            Extension = extension;
        }

        public string Caption { get; private set; }

        public VFMFMuxer Muxer { get; private set; }

        /// <summary>
        /// File extension including the dot. Null when the output is a folder (HLS).
        /// </summary>
        public string Extension { get; private set; }

        public override string ToString()
        {
            return Caption;
        }
    }

    /// <summary>
    /// File or camera, optional preview, into the MF muxer.
    /// Video goes through an Infinite Pin Tee so the preview and the encoder each get a pin.
    /// AvgBitrate is kilobits per second: the encoder multiplies it by 1024.
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
            EncoderChoice encoder,
            int videoBitrateKbps,
            VFMFCommonRateControlMode rateControl,
            MuxChoice mux,
            bool encodeAudio,
            int audioBitrateKbps,
            string outputPath)
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
            var muxFilter = AddFromClsid(DSFilterInitInfoConsts.VFMFMux.CLSID, "MF Mux");
            ApplyLicense(muxFilter);
            ConfigureMux(muxFilter, encoder, videoBitrateKbps, rateControl, mux, encodeAudio, audioBitrateKbps, outputPath, fileSource);

            _window = (IVideoWindow)renderer;
            Check(_window.put_Owner(preview), "Set preview owner");
            Check(_window.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren), "Set preview style");
            PlacePreview(previewWidth, previewHeight);
            Check(_window.put_Visible(OABool.True), "Show preview");

            Check(_capture.RenderStream(null, MediaType.Video, _source, null, tee), "Connect source video to tee");
            Check(_capture.RenderStream(null, MediaType.Video, tee, null, renderer), "Connect preview");
            Check(_capture.RenderStream(null, MediaType.Video, tee, rgb2yuv, muxFilter), "Connect encoder");

            if (encodeAudio)
            {
                if (_audioSource == null)
                {
                    throw new InvalidOperationException("Select an audio device, or turn audio off.");
                }

                Check(_capture.RenderStream(null, MediaType.Audio, _audioSource, null, muxFilter), "Connect audio");
            }

            if (fileSource)
            {
                var clock = (IMediaFilter)_graph;
                Check(clock.SetSyncSource(null), "Encode the file as fast as the machine allows");
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

        /// <summary>
        /// Reads queued graph events. Returns true when encoding has finished.
        /// </summary>
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

        private static void ConfigureMux(
            IBaseFilter muxFilter,
            EncoderChoice encoder,
            int videoBitrateKbps,
            VFMFCommonRateControlMode rateControl,
            MuxChoice mux,
            bool encodeAudio,
            int audioBitrateKbps,
            string outputPath,
            bool fileSource)
        {
            var config = muxFilter as IMFMuxConfig;
            if (config == null)
            {
                throw new InvalidOperationException("The MF Mux filter has no IMFMuxConfig interface.");
            }

            var video = new VFMFVideoEncoderSettings();
            SettingsHelper.SetDefaults(ref video);
            video.Encoder = encoder.Encoder;
            video.AvgBitrate = videoBitrateKbps;
            video.MaxBitrate = videoBitrateKbps * 2;
            video.RateControlMode = rateControl;
            video.LowLatencyMode = !fileSource;
            Check(config.SetVideoEncoderSettings(video), "Set video encoder");
            Check(config.SetMuxer(mux.Muxer), "Set container");

            if (encodeAudio)
            {
                var audio = new VFMFAudioEncoderSettings();
                SettingsHelper.SetDefaults(ref audio);
                audio.Bitrate = audioBitrateKbps;
                Check(config.SetAudioEncoderSettings(audio), "Set audio encoder");
            }

            if (mux.Muxer == VFMFMuxer.HLS)
            {
                var hls = new VFHLSSettings
                {
                    PlaylistType = fileSource ? VFHLSPlaylistType.VOD : VFHLSPlaylistType.Live,
                    SegmentDuration = 4,
                    NumSegments = fileSource ? 0 : 5,
                    PlaylistName = "stream.m3u8",
                    PartName = "segment"
                };
                Check(config.SetHLSMuxerSettings(hls), "Set HLS");
            }

            var sink = muxFilter as IFileSinkFilter;
            if (sink == null)
            {
                throw new InvalidOperationException("The MF Mux filter has no file sink.");
            }

            Check(sink.SetFileName(outputPath, null), "Set output path");
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
