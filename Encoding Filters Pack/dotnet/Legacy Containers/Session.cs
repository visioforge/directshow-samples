using System;
using System.Runtime.InteropServices;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.LegacyContainers
{
    internal sealed class FormatChoice
    {
        public FormatChoice(string caption, int format, int videoCodec, int audioCodec, string extension)
        {
            Caption = caption;
            Format = format;
            VideoCodec = videoCodec;
            AudioCodec = audioCodec;
            Extension = extension;
        }

        public string Caption { get; private set; }

        // FFMPEG_SINK_FORMAT value.
        public int Format { get; private set; }

        // FFMPEG_SINK_VIDEO_CODEC value; 0 is the container's default codec.
        public int VideoCodec { get; private set; }

        // FFMPEG_SINK_AUDIO_CODEC value.
        public int AudioCodec { get; private set; }

        public string Extension { get; private set; }

        public override string ToString()
        {
            return Caption;
        }
    }

    /// <summary>
    /// FFmpeg encoder. The container is set on the filter, the codecs and bitrates on each connected input pin.
    /// Video and audio bitrates are bits per second.
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
            FormatChoice format,
            int width,
            int height,
            int videoBitrateKbps,
            bool encodeAudio,
            int audioBitrateKbps,
            int sampleRate,
            int channels,
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
                if (audioDevice != null && encodeAudio)
                {
                    _audioSource = BindDevice(audioDevice);
                    _ownAudioSource = true;
                    Check(_graph.AddFilter(_audioSource, audioDevice.Name), "Add audio device");
                }
            }

            var tee = Add((IBaseFilter)new InfTee(), "Preview tee");
            var renderer = Add((IBaseFilter)new VideoRenderer(), "Preview");
            var encoder = AddFromClsid(DSFilterInitInfoConsts.VFFFMPEGEncoder.CLSID, "FFmpeg encoder");
            ApplyLicense(encoder);
            var sink = encoder as IFFmpegSinkSettings;
            if (sink == null)
            {
                throw new InvalidOperationException("The FFmpeg encoder has no configuration interface.");
            }

            Check(sink.SetFormat(format.Format), "Set container");
            Check(((IFileSinkFilter)encoder).SetFileName(outputPath, null), "Set output path");

            _window = (IVideoWindow)renderer;

            Check(_capture.RenderStream(null, MediaType.Video, _source, null, tee), "Connect source video to tee");
            Check(_capture.RenderStream(null, MediaType.Video, tee, null, renderer), "Connect preview");

            // The renderer has a window only once its input pin is connected.
            Check(_window.put_Owner(preview), "Set preview owner");
            Check(_window.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren), "Set preview style");
            PlacePreview(previewWidth, previewHeight);
            Check(_window.put_Visible(OABool.True), "Show preview");

            Check(_capture.RenderStream(null, MediaType.Video, tee, null, encoder), "Connect encoder");

            if (encodeAudio)
            {
                if (_audioSource == null)
                {
                    throw new InvalidOperationException("Select an audio device, or turn audio off.");
                }

                Check(_capture.RenderStream(null, MediaType.Audio, _audioSource, null, encoder), "Connect audio");
            }

            // The encoder creates a pin per connection; each pin takes the settings of its stream.
            ConfigurePins(encoder, format, width, height, videoBitrateKbps * 1000, audioBitrateKbps * 1000, sampleRate, channels);

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
                // _control and _events are casts of the graph's RCW; only _graph is released.
                _control.Stop();
                _control = null;
            }

            if (_events != null)
            {
                _events.SetNotifyWindow(IntPtr.Zero, 0, IntPtr.Zero);
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

        private static void ConfigurePins(
            IBaseFilter encoder,
            FormatChoice format,
            int width,
            int height,
            int videoBitrate,
            int audioBitrate,
            int sampleRate,
            int channels)
        {
            for (var index = 0; ; index++)
            {
                var pin = DsFindPin.ByDirection(encoder, PinDirection.Input, index);
                if (pin == null)
                {
                    return;
                }

                try
                {
                    var type = new AMMediaType();
                    if (pin.ConnectionMediaType(type) != 0)
                    {
                        continue;
                    }

                    var major = type.majorType;
                    DsUtils.FreeAMMediaType(type);
                    var settings = (IFFmpegSinkPinSettings)pin;
                    if (major == MediaType.Video)
                    {
                        Check(settings.SetVideoCodec(format.VideoCodec), "Set video codec");
                        Check(settings.SetVideoBitRate(videoBitrate), "Set video bitrate");
                        Check(settings.SetVideoWidth(width), "Set output width");
                        Check(settings.SetVideoHeight(height), "Set output height");
                    }
                    else if (major == MediaType.Audio)
                    {
                        Check(settings.SetAudioCodec(format.AudioCodec), "Set audio codec");
                        Check(settings.SetAudioBitRate(audioBitrate), "Set audio bitrate");
                        Check(settings.SetAudioSampleRate(sampleRate), "Set sample rate");

                        // SPEAKER_FRONT_CENTER for mono, SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT for stereo.
                        Check(settings.SetAudioChannelMask(channels == 1 ? 0x4u : 0x3u), "Set channels");
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(pin);
                }
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

    [ComImport]
    [Guid("CAEC87C3-24B5-404C-9985-FA18D2B78A4A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IFFmpegSinkSettings
    {
        [PreserveSig]
        int SetFormat(int format);
    }

    /// <summary>
    /// Methods stay in vtable order through SetAudioChannelMask.
    /// </summary>
    [ComImport]
    [Guid("496D3BA7-0630-499E-A730-577D4A049F9F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IFFmpegSinkPinSettings
    {
        [PreserveSig]
        int SetVideoCodec(int codec);

        [PreserveSig]
        int SetVideoBitRate(int bitRate);

        [PreserveSig]
        int SetVideoWidth(int width);

        [PreserveSig]
        int SetVideoHeight(int height);

        [PreserveSig]
        int SetVideoProfile(int profile);

        [PreserveSig]
        int SetVideoLevel(int level);

        [PreserveSig]
        int SetVideoGOPSize(int gopSize);

        [PreserveSig]
        int SetVideoMaxBFrames(int maxBFrames);

        [PreserveSig]
        int SetVideoThreadCount(int threadCount);

        [PreserveSig]
        int SetAudioCodec(int codec);

        [PreserveSig]
        int SetAudioBitRate(int bitRate);

        [PreserveSig]
        int SetAudioSampleRate(int sampleRate);

        [PreserveSig]
        int SetAudioChannelMask(uint channelMask);
    }
}