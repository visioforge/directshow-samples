using System;
using System.Runtime.InteropServices;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.LegacyContainers
{
    internal sealed class FormatChoice
    {
        public FormatChoice(string caption, int format, string extension)
        {
            Caption = caption;
            Format = format;
            Extension = extension;
        }

        public string Caption { get; private set; }

        public int Format { get; private set; }

        public string Extension { get; private set; }

        public override string ToString()
        {
            return Caption;
        }
    }

    /// <summary>
    /// Legacy FFmpeg encoder. The settings struct is passed by value and stays blittable.
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
            int format,
            int width,
            int height,
            int videoBitrateKbps,
            bool encodeAudio,
            int audioBitrateKbps,
            int sampleRate,
            int channels,
            int tvSystem,
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
            ConfigureEncoder(
                encoder,
                outputPath,
                format,
                width,
                height,
                videoBitrateKbps * 1000,
                encodeAudio,
                audioBitrateKbps * 1000,
                sampleRate,
                channels,
                tvSystem);

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

        // VCD is 2, SVCD is 5, DVD is 6. Those discs are 4:3. Other containers follow the picture.
        private static void DisplayAspect(int format, int width, int height, out int ratioW, out int ratioH)
        {
            if (format == 2 || format == 5 || format == 6)
            {
                ratioW = 4;
                ratioH = 3;
                return;
            }

            if (width < 1)
            {
                width = 1;
            }

            if (height < 1)
            {
                height = 1;
            }

            var divisor = width;
            var rest = height;
            while (rest != 0)
            {
                var next = divisor % rest;
                divisor = rest;
                rest = next;
            }

            ratioW = width / divisor;
            ratioH = height / divisor;
        }

        private static void ConfigureEncoder(
            IBaseFilter filter,
            string outputPath,
            int format,
            int width,
            int height,
            int videoBitrate,
            bool encodeAudio,
            int audioBitrate,
            int sampleRate,
            int channels,
            int tvSystem)
        {
            var encoder = filter as IVFFFMPEGEncoder;
            if (encoder == null)
            {
                throw new InvalidOperationException("The FFmpeg encoder has no configuration interface.");
            }

            var filename = Marshal.StringToCoTaskMemUni(outputPath);
            int aspectW;
            int aspectH;
            DisplayAspect(format, width, height, out aspectW, out aspectH);
            try
            {
                var settings = new FfmpegSettings
                {
                    Filename = filename,
                    AudioAvailable = encodeAudio ? 1 : 0,
                    AudioBitrate = audioBitrate,
                    AudioSamplerate = sampleRate,
                    AudioChannels = channels,
                    VideoWidth = width,
                    VideoHeight = height,
                    AspectRatioW = aspectW,
                    AspectRatioH = aspectH,
                    VideoBitrate = videoBitrate,
                    VideoMaxRate = videoBitrate,
                    VideoMinRate = videoBitrate / 2,
                    VideoBufferSize = videoBitrate / 8,
                    Interlace = 0,
                    VideoGopSize = 18,
                    TVSystem = tvSystem,
                    OutputFormat = format
                };
                encoder.set_settings(settings);
            }
            finally
            {
                Marshal.FreeCoTaskMem(filename);
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

    [StructLayout(LayoutKind.Sequential)]
    internal struct FfmpegSettings
    {
        public IntPtr Filename;
        public int AudioAvailable;
        public int AudioBitrate;
        public int AudioSamplerate;
        public int AudioChannels;
        public int VideoWidth;
        public int VideoHeight;
        public int AspectRatioW;
        public int AspectRatioH;
        public int VideoBitrate;
        public int VideoMaxRate;
        public int VideoMinRate;
        public int VideoBufferSize;
        public int Interlace;
        public int VideoGopSize;
        public int TVSystem;
        public int OutputFormat;
    }

    [ComImport]
    [Guid("17B8FF7D-A67F-45CE-B425-0E4F607D8C60")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVFFFMPEGEncoder
    {
        [PreserveSig]
        void set_settings(FfmpegSettings settings);
    }
}
