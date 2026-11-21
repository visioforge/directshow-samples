using System;
using System.Runtime.InteropServices;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.AudioEncode
{
    internal sealed class FormatChoice
    {
        public FormatChoice(string caption, int kind, string extension)
        {
            Caption = caption;
            Kind = kind;
            Extension = extension;
        }

        public string Caption { get; private set; }

        /// <summary>0 is MP3, 1 is AAC, 2 is FLAC.</summary>
        public int Kind { get; private set; }

        public string Extension { get; private set; }

        public override string ToString()
        {
            return Caption;
        }
    }

    /// <summary>
    /// Audio-only file. MP3 is LAME into a File Writer. AAC is the v10 encoder into the v10 MP4 muxer.
    /// FLAC is the FFmpeg encoder writing Matroska audio.
    /// </summary>
    internal sealed class Session
    {
        public const int GraphNotifyMessage = 0x8000 + 1;

        private const string LicenseKey = "";

        private const int FormatMkv = 2;      // FFMPEG_SINK_FORMAT_MKV
        private const int AudioCodecFlac = 4; // FFMPEG_SINK_AUDIO_CODEC_FLAC

        private IFilterGraph2 _graph;
        private ICaptureGraphBuilder2 _capture;
        private IMediaControl _control;
        private IMediaEventEx _events;
        private IBaseFilter _source;
        private bool _ownSource;

        public void Start(
            IntPtr notifyWindow,
            bool fileSource,
            string sourceFile,
            DsDevice audioDevice,
            int kind,
            int rate,
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
            }
            else
            {
                _source = BindDevice(audioDevice);
                _ownSource = true;
                Check(_graph.AddFilter(_source, audioDevice.Name), "Add audio device");
            }


            if (kind == 0)
            {
                var lame = AddFromClsid(DSFilterInitInfoConsts.LAMEDShowFilter.CLSID, "LAME encoder");
                var writer = Add((IBaseFilter)new FileWriter(), "File writer");
                ApplyLicense(lame);
                var sink = writer as IFileSinkFilter;
                if (sink == null)
                {
                    throw new InvalidOperationException("The file writer has no file sink.");
                }

                Check(sink.SetFileName(outputPath, null), "Set output path");
                Check(_capture.RenderStream(null, MediaType.Audio, _source, lame, writer), "Connect LAME");
                ConfigureLame(lame, rate);
            }
            else if (kind == 2)
            {
                var encoder = AddFromClsid(DSFilterInitInfoConsts.VFFFMPEGEncoder.CLSID, "FFmpeg encoder");
                ApplyLicense(encoder);
                var settings = encoder as IFFmpegSinkSettings;
                if (settings == null)
                {
                    throw new InvalidOperationException("The FFmpeg encoder has no configuration interface.");
                }

                Check(settings.SetFormat(FormatMkv), "Set container");
                Check(((IFileSinkFilter)encoder).SetFileName(outputPath, null), "Set output path");
                Check(_capture.RenderStream(null, MediaType.Audio, _source, null, encoder), "Connect the FFmpeg encoder");

                // The encoder creates its input pin on connection; the pin takes the codec.
                var pin = DsFindPin.ByDirection(encoder, PinDirection.Input, 0);
                try
                {
                    Check(((IFFmpegSinkPinSettings)pin).SetAudioCodec(AudioCodecFlac), "Set FLAC");
                }
                finally
                {
                    Marshal.ReleaseComObject(pin);
                }
            }
            else
            {
                var aac = AddFromClsid(DSFilterInitInfoConsts.VFAACEncoderV10.CLSID, "AAC encoder");
                var mux = AddFromClsid(DSFilterInitInfoConsts.VFMP4DestV10.CLSID, "MP4 muxer");
                ApplyLicense(aac);
                ApplyLicense(mux);
                var live = mux as IMP4V10MuxerConfig;
                if (live != null && fileSource)
                {
                    Check(live.SetLiveDisabled(true), "Turn live mode off");
                }

                var sink = mux as IFileSinkFilter;
                if (sink == null)
                {
                    throw new InvalidOperationException("The MP4 muxer has no file sink.");
                }

                Check(sink.SetFileName(outputPath, null), "Set output path");
                // The AAC encoder takes its bitrate only before its output pin is connected.
                ConfigureAac(aac, rate);
                Check(_capture.RenderStream(null, MediaType.Audio, _source, aac, mux), "Connect AAC");
            }

            if (fileSource)
            {
                var clock = (IMediaFilter)_graph;
                Check(clock.SetSyncSource(null), "Encode the file as fast as the machine allows");
            }

            Check(_control.Run(), "Run");
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

            if (_ownSource)
            {
                Release(ref _source);
            }
            else
            {
                _source = null;
            }

            _ownSource = false;

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

        private static void ConfigureLame(IBaseFilter filter, int bitrateKbps)
        {
            var encoder = filter as IAudioEncoderProperties;
            if (encoder == null)
            {
                throw new InvalidOperationException("The LAME encoder has no configuration interface.");
            }

            Check(encoder.set_Bitrate(bitrateKbps), "Set MP3 bitrate");
        }

        private static void ConfigureAac(IBaseFilter filter, int bitrateKbps)
        {
            var encoder = filter as IVFAACEncoder;
            if (encoder == null)
            {
                throw new InvalidOperationException("The AAC encoder has no configuration interface.");
            }

            Check(encoder.SetBitRate((uint)(bitrateKbps * 1000)), "Set AAC bitrate");
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
    /// Methods stay in vtable order through SetAudioCodec.
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
    }
}