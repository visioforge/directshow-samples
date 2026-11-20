using System;
using System.Runtime.InteropServices;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.WebM
{
    internal sealed class EndUsageChoice
    {
        public EndUsageChoice(string caption, int value)
        {
            Caption = caption;
            Value = value;
        }

        public string Caption { get; private set; }

        public int Value { get; private set; }

        public override string ToString()
        {
            return Caption;
        }
    }

    /// <summary>
    /// File or camera to VP8 + Vorbis in a WebM file.
    /// The muxer writes through a separate File Writer.
    /// </summary>
    internal sealed class Session
    {
        public const int GraphNotifyMessage = 0x8000 + 1;

        private const string LicenseKey = "";

        private static readonly Guid ClsidVideoConverter = new Guid("3A65D681-4840-4077-BCA6-381F2564BEBC");
        private static readonly Guid I420 = new Guid("30323449-0000-0010-8000-00AA00389B71");
        private static readonly Guid YV12 = new Guid("32315659-0000-0010-8000-00AA00389B71");

        private const int DeadlineBestQuality = 0;
        private const int DeadlineRealtime = 1;
        private const int EncoderKindVp8 = 0;

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
            int endUsage,
            bool encodeAudio,
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
            var converter = AddFromClsid(ClsidVideoConverter, "Video converter");
            var vp8 = AddFromClsid(DSFilterInitInfoConsts.VFWebMVP8Encoder.CLSID, "VP8 encoder");
            var mux = AddFromClsid(DSFilterInitInfoConsts.VFWebmMuxer.CLSID, "WebM muxer");
            var writer = Add((IBaseFilter)new FileWriter(), "File writer");
            ApplyLicense(vp8);
            ApplyLicense(mux);
            ConfigureConverter(converter);
            ConfigureVp8(vp8, videoBitrateKbps, endUsage, fileSource);

            var sink = writer as IFileSinkFilter;
            if (sink == null)
            {
                throw new InvalidOperationException("The file writer has no file sink.");
            }

            Check(sink.SetFileName(outputPath, null), "Set output path");

            IBaseFilter vorbis = null;
            if (encodeAudio)
            {
                vorbis = AddFromClsid(DSFilterInitInfoConsts.VFWebMVorbisEncoder.CLSID, "Vorbis encoder");
                ApplyLicense(vorbis);
                ConfigureVorbis(vorbis);
            }

            _window = (IVideoWindow)renderer;
            Check(_window.put_Owner(preview), "Set preview owner");
            Check(_window.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren), "Set preview style");
            PlacePreview(previewWidth, previewHeight);
            Check(_window.put_Visible(OABool.True), "Show preview");

            Check(_capture.RenderStream(null, MediaType.Video, _source, null, tee), "Connect source video to tee");
            Check(_capture.RenderStream(null, MediaType.Video, tee, null, renderer), "Connect preview");
            Check(_capture.RenderStream(null, MediaType.Video, tee, converter, vp8), "Connect VP8");
            ConnectFirstFree(_graph, vp8, mux, "Connect VP8 to the muxer");
            ConnectFirstFree(_graph, mux, writer, "Connect the muxer to the file writer");

            if (encodeAudio)
            {
                if (_audioSource == null)
                {
                    throw new InvalidOperationException("Select an audio device, or turn audio off.");
                }

                Check(_capture.RenderStream(null, MediaType.Audio, _audioSource, vorbis, mux), "Connect Vorbis");
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

        private static void ConfigureConverter(IBaseFilter converter)
        {
            var settings = converter as IVFVideoConverter;
            if (settings == null)
            {
                throw new InvalidOperationException("The video converter has no settings interface.");
            }

            Check(settings.SetOutputSubtypes(new[] { I420, YV12 }, 2), "Set I420 output");
        }

        private static void ConfigureVp8(IBaseFilter filter, int videoBitrateKbps, int endUsage, bool fileSource)
        {
            var kind = filter as IVPXEncoderKind;
            if (kind != null)
            {
                Check(kind.SetEncoderKind(EncoderKindVp8), "Select VP8");
            }

            var encoder = filter as IVP8Encoder;
            if (encoder == null)
            {
                throw new InvalidOperationException("The VP8 encoder has no IVP8Encoder interface.");
            }

            Check(encoder.SetThreadCount(Environment.ProcessorCount), "Set VP8 threads");
            Check(encoder.SetDeadline(fileSource ? DeadlineBestQuality : DeadlineRealtime), "Set VP8 deadline");
            Check(encoder.SetEndUsage(endUsage), "Set VP8 rate mode");
            if (!fileSource)
            {
                Check(encoder.SetLagInFrames(0), "Set VP8 lag");
            }

            Check(encoder.SetTargetBitrate(videoBitrateKbps), "Set VP8 bitrate");
        }

        private static void ConfigureVorbis(IBaseFilter filter)
        {
            var encoder = filter as IVorbisEncode;
            if (encoder == null)
            {
                return;
            }

            if (!encoder.SetQuality(80))
            {
                throw new InvalidOperationException("Set Vorbis quality failed.");
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

        private static void ConnectFirstFree(IFilterGraph2 graph, IBaseFilter from, IBaseFilter to, string step)
        {
            var output = FirstUnconnected(from, PinDirection.Output);
            var input = FirstUnconnected(to, PinDirection.Input);
            if (output == null || input == null)
            {
                throw new InvalidOperationException(step + " failed. A pin is missing.");
            }

            try
            {
                Check(graph.Connect(output, input), step);
            }
            finally
            {
                Marshal.ReleaseComObject(output);
                Marshal.ReleaseComObject(input);
            }
        }

        private static IPin FirstUnconnected(IBaseFilter filter, PinDirection direction)
        {
            IEnumPins pins;
            filter.EnumPins(out pins);
            if (pins == null)
            {
                return null;
            }

            var fetched = new IPin[1];
            IPin found = null;
            while (pins.Next(1, fetched, IntPtr.Zero) == 0)
            {
                PinDirection pinDirection;
                fetched[0].QueryDirection(out pinDirection);
                IPin peer;
                var connected = fetched[0].ConnectedTo(out peer);
                if (peer != null)
                {
                    Marshal.ReleaseComObject(peer);
                }

                if (pinDirection == direction && connected != 0)
                {
                    found = fetched[0];
                    break;
                }

                Marshal.ReleaseComObject(fetched[0]);
            }

            Marshal.ReleaseComObject(pins);
            return found;
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
    [Guid("25AB8F6C-2CB3-46A8-90E3-BB2F27A99CEA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVFVideoConverter
    {
        [PreserveSig]
        int SetOutputSubtypes([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Guid[] subtypes, int count);
    }

    /// <summary>
    /// x64 VP8 encoder. Methods stay in vtable order through SetTargetBitrate.
    /// SetTargetBitrate is kilobits per second.
    /// </summary>
    [ComImport]
    [Guid("ED3110FE-5211-11DF-94AF-0026B977EEAA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVP8Encoder
    {
        [PreserveSig]
        int ApplySettings();

        [PreserveSig]
        int ResetSettings();

        [PreserveSig]
        int SetDeadline(int deadline);

        [PreserveSig]
        int GetDeadline(out int deadline);

        [PreserveSig]
        int SetThreadCount(int threads);

        [PreserveSig]
        int GetThreadCount(out int threads);

        [PreserveSig]
        int SetErrorResilient(int value);

        [PreserveSig]
        int GetErrorResilient(out int value);

        [PreserveSig]
        int SetDropframeThreshold(int value);

        [PreserveSig]
        int GetDropframeThreshold(out int value);

        [PreserveSig]
        int SetResizeAllowed(int value);

        [PreserveSig]
        int GetResizeAllowed(out int value);

        [PreserveSig]
        int SetResizeUpThreshold(int value);

        [PreserveSig]
        int GetResizeUpThreshold(out int value);

        [PreserveSig]
        int SetResizeDownThreshold(int value);

        [PreserveSig]
        int GetResizeDownThreshold(out int value);

        [PreserveSig]
        int SetEndUsage(int endUsage);

        [PreserveSig]
        int GetEndUsage(out int endUsage);

        [PreserveSig]
        int SetLagInFrames(int frames);

        [PreserveSig]
        int GetLagInFrames(out int frames);

        [PreserveSig]
        int SetTokenPartitions(int value);

        [PreserveSig]
        int GetTokenPartitions(out int value);

        [PreserveSig]
        int SetTargetBitrate(int bitrateKbps);
    }

    [ComImport]
    [Guid("ED311152-5211-11DF-94AF-0026B977EEAB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVPXEncoderKind
    {
        [PreserveSig]
        int SetEncoderKind(int kind);

        [PreserveSig]
        int GetEncoderKind(out int kind);
    }

    [ComImport]
    [Guid("A4C6A887-7BD3-4B33-9A57-A3EB10924D3A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVorbisEncode
    {
        [PreserveSig]
        int GetEncoderSettings();

        [PreserveSig]
        bool SetQuality(int quality);
    }
}
