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
    /// FLAC is the Xiph encoder into the Dump filter.
    /// </summary>
    internal sealed class Session
    {
        public const int GraphNotifyMessage = 0x8000 + 1;

        private const string LicenseKey = "";

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

            if (kind == 2 && rate > 8)
            {
                rate = 8;
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
                var flac = AddFromClsid(DSFilterInitInfoConsts.VFXiphFLACEncoder.CLSID, "FLAC encoder");
                var dump = AddFromClsid(DSFilterInitInfoConsts.VFDump.CLSID, "Dump");
                ApplyLicense(flac);
                var sink = dump as IFileSinkFilter;
                if (sink == null)
                {
                    throw new InvalidOperationException("The Dump filter has no file sink.");
                }

                Check(sink.SetFileName(outputPath, null), "Set output path");
                Check(_capture.RenderStream(null, MediaType.Audio, _source, null, flac), "Connect FLAC");
                ConfigureFlac(flac, rate);
                Check(_capture.RenderStream(null, MediaType.Audio, flac, null, dump), "Connect the Dump filter");
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
                Check(_capture.RenderStream(null, MediaType.Audio, _source, aac, mux), "Connect AAC");
                ConfigureAac(aac, rate);
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

        private static void ConfigureFlac(IBaseFilter filter, int level)
        {
            var encoder = filter as IFLACEncodeSettings;
            if (encoder == null)
            {
                throw new InvalidOperationException("The FLAC encoder has no configuration interface.");
            }

            if (!encoder.setEncodingLevel((uint)level))
            {
                throw new InvalidOperationException("Set FLAC level failed.");
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
}
