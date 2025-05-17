// ReSharper disable SuspiciousTypeConversion.Global

using System.Threading.Tasks;

namespace Main_Demo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Security.Permissions;
    using System.Windows.Forms;
    using VisioForge.DirectShowAPI;
    using VisioForge.DirectShowLib;

    public partial class Form1 : Form
    {
        /// <summary>
        /// Use your license key received after purchase.
        /// </summary>
        private const string SDK_LICENSE_KEY = "";

        private const int WM_GRAPHNOTIFY = 0x8000 + 1;

        #region Sources code

        private DsDevice[] _videoCaptureDevices;

        private DsDevice[] _audioCaptureDevices;

        private IFilterGraph2 _filterGraphSource;

        private ICaptureGraphBuilder2 _captureGraphSource;

        private IMediaControl _mediaControlSource;

        private IMediaEventEx _mediaEventExSource;

        private IBaseFilter _sourceVideoFilter;

        private IBaseFilter _sourceAudioFilter;

        private IBaseFilter _videoEncFilter;

        private IBaseFilter _rgb2yuvFilter;

        private IBaseFilter _audioEncFilter;

        private IBaseFilter _muxFilter;

        private IBaseFilter _fileWriterFilter;

        private FiltersAvailableInfo _gpuEncoders;

        private IBaseFilter FindSourceVideoCaptureDevice()
        {
            // Take the first device
            DsDevice device = (DsDevice)_videoCaptureDevices[cbVideoCaptureSource.SelectedIndex];

            // Bind Moniker to a filter object
            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out var source);

            // An exception is thrown if cast fail
            return (IBaseFilter)source;
        }

        private IBaseFilter FindSourceAudioCaptureDevice()
        {
            object source;

            // Take the first device
            DsDevice device = (DsDevice)_audioCaptureDevices[cbAudioCaptureSource.SelectedIndex];

            // Bind Moniker to a filter object
            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out source);

            // An exception is thrown if cast fail
            return (IBaseFilter)source;
        }

        // Set the frame rate, and video size
        private static bool SetConfigParms(
            ICaptureGraphBuilder2 capGraph,
            IPin videoPin,
            double frameRate,
            List<VFVideoCaptureFormat> formats,
            string format)
        {
            IAMStreamConfig streamConfig = videoPin as IAMStreamConfig;

            if (formats == null)
            {
                formats = new List<VFVideoCaptureFormat>();
            }

            if (frameRate.CompareTo(0) == 0)
            {
                return false;
            }

            bool result = true;
            int k = -1;
            for (int i = 0; i < formats.Count; i++)
            {
                if (formats[i].Name == format)
                {
                    k = i;
                    break;
                }
            }

            if ((k == -1) || (!DSHelper.ApplyVideoFormat(streamConfig, formats[k], frameRate)))
            {
                result = false;
                MessageBox.Show("Bad video capture format");
            }

            return result;
        }

        private void SetFilterLicense(IBaseFilter filter)
        {
            if (string.IsNullOrEmpty(SDK_LICENSE_KEY))
            {
                return;
            }
            
            var register = filter as IVFRegister;
            if (register != null)
            {
                register.SetLicenseKey(SDK_LICENSE_KEY);
            }
        }

        private void AddMP4v10Output()
        {
            // add colorspace converter
            _rgb2yuvFilter = FilterGraphTools.AddFilterFromClsid(
                _filterGraphSource,
                DSFilterInitInfoConsts.VFRGB2YUV.CLSID,
                "VisioForge RGB2YUV");

            if (_rgb2yuvFilter == null)
            {
                MessageBox.Show("Unable to create VisioForge RGB2YUV filter.");
                return;
            }

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            var hr = _captureGraphSource.RenderStream(null, null, _sourceVideoFilter, null, _rgb2yuvFilter);
            DsError.ThrowExceptionForHR(hr);
            
            // add video encoder
            _videoEncFilter = FilterGraphTools.AddFilterFromClsid(
                    _filterGraphSource,
                    DSFilterInitInfoConsts.VFH264EncoderV10.CLSID,
                    "VisioForge H264 Encoder v10");

            if (_videoEncFilter == null)
            {
                MessageBox.Show("Unable to create VisioForge H264 encoder v10.");
                return;
            }

            IIntelConfigureVideoEncoder videoConfig = this._videoEncFilter as IIntelConfigureVideoEncoder;
            if (videoConfig != null)
            {
                IntelVideoEncoderParams pm;
                videoConfig.GetParams(out pm);

                // apply sample parameters
                pm.target_usage = IntelVideoEncoderTargetUsage.TARGETUSAGE_BEST_SPEED;
                pm.level_idc = IntelVideoEncoderLevel.LL_5;
                pm.profile_idc = IntelVideoEncoderProfile.PF_H264_HIGH;

                hr = videoConfig.SetParams2(pm);
                DsError.ThrowExceptionForHR(hr);
            }

            // add muxer
            _muxFilter = FilterGraphTools.AddFilterFromClsid(
                _filterGraphSource,
                DSFilterInitInfoConsts.VFMP4DestV10.CLSID,
                "VisioForge MP4 Muxer v10");

            if (_muxFilter == null)
            {
                MessageBox.Show("Unable to create VisioForge MP4 v10 filter.");
                return;
            }

            SetFilterLicense(_muxFilter);

            IFileSinkFilter sink = this._muxFilter as IFileSinkFilter;
            sink?.SetFileName(this.edOutput.Text, null);
            
            // connect colorspace converter, video encoder and muxer
            hr = _captureGraphSource.RenderStream(null, MediaType.Video, _rgb2yuvFilter, _videoEncFilter, _muxFilter);
            DsError.ThrowExceptionForHR(hr);

            // add audio encoder
            _audioEncFilter = FilterGraphTools.AddFilterFromClsid(
                _filterGraphSource,
                DSFilterInitInfoConsts.VFAACEncoderV10.CLSID,
                "VisioForge AAC Encoder v10");

            if (_audioEncFilter == null)
            {
                MessageBox.Show("Unable to create VisioForge AAC encoder v10.");
                return;
            }

            IVFAACEncoder audioConfig = this._audioEncFilter as IVFAACEncoder;
            if (audioConfig != null)
            {
                audioConfig.SetBitRate(192000);
            }

            hr = _captureGraphSource.RenderStream(null, null, _sourceAudioFilter, _audioEncFilter, _muxFilter);
            DsError.ThrowExceptionForHR(hr);
        }


        private void AddMP4HWOutput()
        {
            // add colorspace converter
            _rgb2yuvFilter = FilterGraphTools.AddFilterFromClsid(
                _filterGraphSource,
                DSFilterInitInfoConsts.VFRGB2YUV.CLSID,
                "VisioForge RGB2YUV");

            if (_rgb2yuvFilter == null)
            {
                MessageBox.Show("Unable to create VisioForge RGB2YUV filter.");
                return;
            }

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            var hr = _captureGraphSource.RenderStream(null, null, _sourceVideoFilter, null, _rgb2yuvFilter);
            DsError.ThrowExceptionForHR(hr);

            // add muxer
            _muxFilter = FilterGraphTools.AddFilterFromClsid(
                _filterGraphSource,
                DSFilterInitInfoConsts.VFMFMux.CLSID,
                "VisioForge MF Muxer");

            if (_muxFilter == null)
            {
                MessageBox.Show("Unable to create VisioForge MF Mux filter.");
                return;
            }

            SetFilterLicense(_muxFilter);

            var muxConfig = _muxFilter as IMFMuxConfig;
            if (muxConfig != null)
            {
                var videoSettings = new VFMFVideoEncoderSettings();
                SettingsHelper.SetDefaults(ref videoSettings);

                if (_gpuEncoders.AMD_H264)
                {
                    videoSettings.Encoder = VFMFVideoEncoder.AMD_H264;
                }
                else if (_gpuEncoders.NVENC_H264)
                {
                    videoSettings.Encoder = VFMFVideoEncoder.NVENC_H264;
                }
                else if (_gpuEncoders.QSV_H264)
                {
                    videoSettings.Encoder = VFMFVideoEncoder.QSV_H264;
                }
                else if (_gpuEncoders.AMD_H265)
                {
                    videoSettings.Encoder = VFMFVideoEncoder.AMD_H265;
                }
                else if (_gpuEncoders.NVENC_H265)
                {
                    videoSettings.Encoder = VFMFVideoEncoder.NVENC_H265;
                }
                else if (_gpuEncoders.QSV_H265)
                {
                    videoSettings.Encoder = VFMFVideoEncoder.QSV_H265;
                }

                muxConfig.SetVideoEncoderSettings(videoSettings);
            }
                

            IFileSinkFilter sink = this._muxFilter as IFileSinkFilter;
            sink?.SetFileName(this.edOutput.Text, null);

            // connect video
            hr = _captureGraphSource.RenderStream(null, null, _rgb2yuvFilter, null, _muxFilter);
            DsError.ThrowExceptionForHR(hr);

            // connect audio
            hr = _captureGraphSource.RenderStream(null, null, _sourceAudioFilter, null, _muxFilter);
            DsError.ThrowExceptionForHR(hr);
        }


        private void AddSink()
        {
            if (rbMP4v10.Checked)
            {
                AddMP4v10Output();
            }
            else
            {
                AddMP4HWOutput();
            }
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        private void SourceInitCamera()
        {
            int hr = 0;

            // An exception is thrown if cast fail
            _filterGraphSource = (IFilterGraph2)new FilterGraph();
            _captureGraphSource = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            _mediaControlSource = (IMediaControl)_filterGraphSource;
            _mediaEventExSource = (IMediaEventEx)_filterGraphSource;

            hr = _mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = _captureGraphSource.SetFiltergraph(_filterGraphSource);
            DsError.ThrowExceptionForHR(hr);

            // video source
            _sourceVideoFilter = FindSourceVideoCaptureDevice();
            hr = _filterGraphSource.AddFilter(
                _sourceVideoFilter,
                _videoCaptureDevices[cbVideoCaptureSource.SelectedIndex].Name);

            IPin outputPin = FilterHelper.GetFreePinWithMediaType(_sourceVideoFilter, PinDirection.Output, MediaType.Video);

            if (outputPin == null)
            {
                Marshal.ReleaseComObject(_sourceVideoFilter);
                _sourceVideoFilter = null;

                return;
            }

            List<string> videoFormats = new List<string>();
            List<string> frameRates = new List<string>();
            List<VFVideoCaptureFormat> videoFormatsObj = new List<VFVideoCaptureFormat>();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            enumerator.GetVideoFormatsAndFrameRates(outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);

            SetConfigParms(
                _captureGraphSource,
                outputPin,
                float.Parse(cbSourceFrameRate.Text),
                videoFormatsObj,
                cbSourceVideoFormat.Text);

            // audio source
            _sourceAudioFilter = FindSourceAudioCaptureDevice();
            hr = _filterGraphSource.AddFilter(
                _sourceAudioFilter,
                _audioCaptureDevices[cbAudioCaptureSource.SelectedIndex].Name);

            // sinks
           this.AddSink();     

            if (cbDebugMode.Checked)
            {
                FilterGraphHelper.SaveGraphFile(
                    _filterGraphSource,
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    + "\\VisioForge\\virtual_camera_source.grf");
            }

            hr = _mediaControlSource.Run();
        }

        /// <summary>
        /// Converts HEX string to byte array.
        /// </summary>
        /// <param name="hexString">
        /// HEX string.
        /// </param>
        /// <returns>
        /// Byte array.
        /// </returns>
        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        private void SourceInitFile()
        {
            // An exception is thrown if cast fail
            _filterGraphSource = (IFilterGraph2)new FilterGraph();
            _captureGraphSource = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            _mediaControlSource = (IMediaControl)_filterGraphSource;
            _mediaEventExSource = (IMediaEventEx)_filterGraphSource;

            var hr = this._mediaEventExSource.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = _captureGraphSource.SetFiltergraph(_filterGraphSource);
            DsError.ThrowExceptionForHR(hr);

            _filterGraphSource.AddSourceFilter(edSourceFile.Text, "Source file", out _sourceVideoFilter);
            _sourceAudioFilter = this._sourceVideoFilter;

            // sinks
            AddSink();

            if (cbDebugMode.Checked)
            {
                FilterGraphHelper.SaveGraphFile(_filterGraphSource, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\virtual_camera_source.grf");
            }

            // Set the graph clock.
            IMediaFilter mediaFilter = _filterGraphSource as IMediaFilter;
            if (mediaFilter != null)
            {
                hr = mediaFilter.SetSyncSource(null);
            }

            hr = _mediaControlSource.Run();
        }

        private void SourceFree()
        {
            try
            {
                // Stop previewing data
                if (_mediaControlSource != null)
                {
                    _mediaControlSource.StopWhenReady();
                }

                // Stop receiving events
                if (_mediaEventExSource != null)
                {
                    _mediaEventExSource.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);
                }

                // Release DirectShow interfaces
                if (_mediaControlSource != null)
                {
                    Marshal.ReleaseComObject(_mediaControlSource);
                    _mediaControlSource = null;
                }

                if (_mediaEventExSource != null)
                {
                    Marshal.ReleaseComObject(_mediaEventExSource);
                    _mediaEventExSource = null;
                }

                if (_filterGraphSource != null)
                {
                    Marshal.ReleaseComObject(_filterGraphSource);
                    _filterGraphSource = null;
                }

                if (_captureGraphSource != null)
                {
                    Marshal.ReleaseComObject(_captureGraphSource);
                    _captureGraphSource = null;
                }

                if (_sourceVideoFilter != null)
                {
                    Marshal.ReleaseComObject(_sourceVideoFilter);
                    _sourceVideoFilter = null;
                }

                if (_sourceAudioFilter != null)
                {
                    Marshal.ReleaseComObject(_sourceAudioFilter);
                    _sourceAudioFilter = null;
                }

                if (_videoEncFilter != null)
                {
                    Marshal.ReleaseComObject(_videoEncFilter);
                    _videoEncFilter = null;
                }

                if (_audioEncFilter != null)
                {
                    Marshal.ReleaseComObject(_audioEncFilter);
                    _audioEncFilter = null;
                }

                if (_rgb2yuvFilter != null)
                {
                    Marshal.ReleaseComObject(_rgb2yuvFilter);
                    _rgb2yuvFilter = null;
                }

                if (_muxFilter != null)
                {
                    Marshal.ReleaseComObject(_muxFilter);
                    _muxFilter = null;
                }

                if (_fileWriterFilter != null)
                {
                    Marshal.ReleaseComObject(_fileWriterFilter);
                    _fileWriterFilter = null;
                }
            }
            catch
            {
            }
        }

        private void EnumSourceCameraFormats()
        {
            cbSourceVideoFormat.Items.Clear();
            cbSourceFrameRate.Items.Clear();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            // DsDevice device = videoCaptureDevices[cbVideoCaptureSource.SelectedIndex];

            IBaseFilter camera = FindSourceVideoCaptureDevice();
            if (camera == null)
            {
                return;
            }

            IPin outputPin = FilterHelper.GetFreePinWithMediaType(camera, PinDirection.Output, MediaType.Video);

            if (outputPin == null)
            {
                Marshal.ReleaseComObject(camera);
                // ReSharper disable once RedundantAssignment
                camera = null;

                return;
            }

            List<string> videoFormats = new List<string>();
            List<string> frameRates = new List<string>();
            List<VFVideoCaptureFormat> videoFormatsObj = new List<VFVideoCaptureFormat>();

            enumerator.GetVideoFormatsAndFrameRates(
                outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);


            foreach (var videoFormat in videoFormats)
            {
                cbSourceVideoFormat.Items.Add(videoFormat);
            }

            if (cbSourceVideoFormat.Items.Count > 0)
            {
                cbSourceVideoFormat.SelectedIndex = 0;
            }

            foreach (var item in frameRates)
            {
                cbSourceFrameRate.Items.Add(item);
            }

            if (cbSourceFrameRate.Items.Count > 0)
            {
                cbSourceFrameRate.SelectedIndex = 0;
            }

            Marshal.ReleaseComObject(outputPin);
            // ReSharper disable once RedundantAssignment
            outputPin = null;

            Marshal.ReleaseComObject(camera);

            // ReSharper disable once RedundantAssignment
            camera = null;
        }

        /// <summary>
        /// WndProc procedure.
        /// </summary>
        /// <param name="m">
        /// Message m.
        /// </param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WM_GRAPHNOTIFY:
                        {
                            HandleGraphEvent();
                            break;
                        }
                }

                base.WndProc(ref m);
            }
            catch
            {
            }
        }

        private delegate void CompleteDelegate();

        private void CompleteDelegateMethod()
        {
            if (rbFile.Checked)
            {
                MessageBox.Show("Completed!");
                btSourceStop.Enabled = false;
                btSourceStart.Enabled = true;

                SourceFree();
            }
        }

        /// <summary>
        /// Handles graph event.
        /// </summary>
        private void HandleGraphEvent()
        {
            try
            {
                EventCode eventCode;
                IntPtr param1;
                IntPtr param2;

                if (_mediaEventExSource == null)
                {
                    return;
                }

                while (_mediaEventExSource.GetEvent(out eventCode, out param1, out param2, 0) == 0)
                {
                    // Free event parameters to prevent memory leaks associated with
                    // event parameter data.  While this application is not interested
                    // in the received events, applications should always process them.

                    switch (eventCode)
                    {
                        case EventCode.Complete:
                            BeginInvoke(new CompleteDelegate(CompleteDelegateMethod));
                            break;
                    }

                    // ReSharper disable ConditionIsAlwaysTrueOrFalse
                    // ReSharper disable HeuristicUnreachableCode
                    if (_mediaEventExSource == null)
                    {
                        return;
                    }
                    // ReSharper restore HeuristicUnreachableCode
                    // ReSharper restore ConditionIsAlwaysTrueOrFalse

                    int hr = _mediaEventExSource.FreeEventParams(eventCode, param1, param2);
                    DsError.ThrowExceptionForHR(hr);

                    // Insert event processing code here, if desired
                }
            }
            catch
            {
            }
        }

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Get all video / audio input devices
            _videoCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            foreach (var device in _videoCaptureDevices)
            {
                cbVideoCaptureSource.Items.Add(device.Name);
            }

            if (cbVideoCaptureSource.Items.Count > 0)
            {
                cbVideoCaptureSource.SelectedIndex = 0;
            }

            _audioCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);

            foreach (var device in _audioCaptureDevices)
            {
                cbAudioCaptureSource.Items.Add(device.Name);
            }

            if (cbAudioCaptureSource.Items.Count > 0)
            {
                cbAudioCaptureSource.SelectedIndex = 0;
            }

            edOutput.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisioForge", "output.mp4");

            // get available GPU encoders
            Task.Run((() =>
            {
                _gpuEncoders = MFTFilterEnum.GetFiltersAvailable(out var _);

                Invoke((Action)(() =>
                {
                    edGPUAvailable.Text = _gpuEncoders.ToString();
                }));
            }));
        }

        private void btSourceStart_Click(object sender, EventArgs e)
        {
            btSourceStart.Enabled = false;
            btSourceStop.Enabled = true;

            if (rbFile.Checked)
            {
                SourceInitFile();
            }
            else
            {
                SourceInitCamera();
            }
        }

        private void btSourceStop_Click(object sender, EventArgs e)
        {
            btSourceStop.Enabled = false;
            btSourceStart.Enabled = true;

            SourceFree();
        }

        private void cbVideoCaptureSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumSourceCameraFormats();
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                edSourceFile.Text = openFileDialog1.FileName;
            }
        }

        private void btSelectOutput_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                edOutput.Text = saveFileDialog1.FileName;
            }
        }

        private void rbMP4v10_Click(object sender, EventArgs e)
        {
            edOutput.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisioForge", "output.mp4");
        }

        private void rbMP4v8_Click(object sender, EventArgs e)
        {
            edOutput.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisioForge", "output.mp4");
        }
    }
}
