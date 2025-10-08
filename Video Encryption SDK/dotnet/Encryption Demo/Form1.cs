// ReSharper disable SuspiciousTypeConversion.Global
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

        private DsDevice[] videoCaptureDevices;

        private DsDevice[] audioCaptureDevices;

        private IFilterGraph2 filterGraphSource;

        private ICaptureGraphBuilder2 captureGraphSource;

        private IMediaControl mediaControlSource;

        private IMediaEventEx mediaEventExSource;

        private IBaseFilter sourceVideoFilter;

        private IBaseFilter sourceAudioFilter;

        private IBaseFilter videoEncFilter;

        private IBaseFilter audioEncFilter;

        private IBaseFilter rgb2yuvFilter;

        private IBaseFilter muxFilter;

        private IBaseFilter fileWriterFilter;

        private IBaseFilter FindSourceVideoCaptureDevice()
        {
            object source;

            // Take the first device
            DsDevice device = (DsDevice)videoCaptureDevices[cbVideoCaptureSource.SelectedIndex];

            // Bind Moniker to a filter object
            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out source);

            // An exception is thrown if cast fail
            return (IBaseFilter)source;
        }

        private IBaseFilter FindSourceAudioCaptureDevice()
        {
            object source;

            // Take the first device
            DsDevice device = (DsDevice)audioCaptureDevices[cbAudioCaptureSource.SelectedIndex];

            // Bind Moniker to a filter object
            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out source);

            // An exception is thrown if cast fail
            return (IBaseFilter)source;
        }

        // Set the Framerate, and video size
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

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        private void SourceInitCamera()
        {
            int hr = 0;

            // An exception is thrown if cast fail
            filterGraphSource = (IFilterGraph2)new FilterGraph();
            captureGraphSource = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlSource = (IMediaControl)filterGraphSource;
            mediaEventExSource = (IMediaEventEx)filterGraphSource;

            hr = mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphSource.SetFiltergraph(filterGraphSource);
            DsError.ThrowExceptionForHR(hr);

            // video source
            sourceVideoFilter = FindSourceVideoCaptureDevice();
            hr = filterGraphSource.AddFilter(
                sourceVideoFilter,
                videoCaptureDevices[cbVideoCaptureSource.SelectedIndex].Name);

            IPin outputPin = FilterHelper.GetFreePinWithMediaType(sourceVideoFilter, PinDirection.Output, MediaType.Video);

            if (outputPin == null)
            {
                Marshal.ReleaseComObject(sourceVideoFilter);
                sourceVideoFilter = null;

                return;
            }

            List<string> videoFormats = new List<string>();
            List<string> frameRates = new List<string>();
            List<VFVideoCaptureFormat> videoFormatsObj = new List<VFVideoCaptureFormat>();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            enumerator.GetVideoFormatsAndFrameRates(outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);

            SetConfigParms(
                captureGraphSource,
                outputPin,
                float.Parse(cbSourceFrameRate.Text),
                videoFormatsObj,
                cbSourceVideoFormat.Text);

            // audio source
            sourceAudioFilter = FindSourceAudioCaptureDevice();
            hr = filterGraphSource.AddFilter(
                sourceAudioFilter,
                audioCaptureDevices[cbAudioCaptureSource.SelectedIndex].Name);

            // sinks
            AddEncryptor();

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            Guid CLSID_VFRGB2YUV = new Guid("3BDA461E-12DB-4C24-9815-B68D1AA4D34A"); // "VisioForge RGB2YUV"
            rgb2yuvFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, CLSID_VFRGB2YUV, "VisioForge RGB2YUV");

            if (rgb2yuvFilter == null)
            {
                MessageBox.Show(this, "Unable to create VisioForge RGB2YUV filter.");
                return;
            }

            hr = captureGraphSource.RenderStream(null, null, sourceVideoFilter, null, rgb2yuvFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, rgb2yuvFilter, videoEncFilter, muxFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, sourceAudioFilter, audioEncFilter, muxFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, muxFilter, null, fileWriterFilter);
            DsError.ThrowExceptionForHR(hr);

            if (cbDebugMode.Checked)
            {
                FilterGraphTools.SaveGraphFile(
                    filterGraphSource,
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisioForge", "video_encryption_source.grf"));
            }

            mediaControlSource.Run();
        }

        private void AddEncryptor()
        {
            if (rbEncryptionModeAES128.Checked)
            {
                muxFilter = FilterGraphTools.AddFilterFromClsid(
                    filterGraphSource,
                    new Guid(Consts.CLSID_VFVideoEncryptor8),
                    "VisioForge Encryptor v8");
            }
            else
            {
                muxFilter = FilterGraphTools.AddFilterFromClsid(
                    filterGraphSource,
                    new Guid(Consts.CLSID_VFVideoEncryptor9),
                    "VisioForge Encryptor v9");
            }

            videoEncFilter = FilterGraphTools.AddFilterFromClsid(
                filterGraphSource,
                DSFilterInitInfoConsts.VFH264EncoderV10.CLSID,
                "VisioForge H264 Encoder");
            audioEncFilter = FilterGraphTools.AddFilterFromClsid(
                filterGraphSource,
                DSFilterInitInfoConsts.VFAACEncoderV10.CLSID,
                "VisioForge AAC Encoder v10");

            Guid CLSID_FileWriter = new Guid("8596E5F0-0DA5-11d0-BD21-00A0C911CE86");
            fileWriterFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, CLSID_FileWriter, "Filewriter");
            IFileSinkFilter fileSink = fileWriterFilter as IFileSinkFilter;
            fileSink?.SetFileName(edOutput.Text, null);

            ApplyEncryptorSettings();
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

        /// <summary>
        /// Applies encryption settings.
        /// </summary>
        private void ApplyEncryptorSettings()
        {
            IVFRegister reg = muxFilter as IVFRegister;
            if (reg != null && !string.IsNullOrEmpty(SDK_LICENSE_KEY))
            {
                reg.SetLicenseKey(SDK_LICENSE_KEY);
            }

            IVFCryptoConfig cryptoConfig = muxFilter as IVFCryptoConfig;
            if (cryptoConfig != null)
            {
                if (rbEncryptionKeyString.Checked)
                {
                    string encryptionKey = edEncryptionKeyString.Text;

                    if (string.IsNullOrEmpty(encryptionKey))
                    {
                        MessageBox.Show(this, "Encryption error not set! 123 will be used!");
                        encryptionKey = "123";
                    }

                    cryptoConfig.ApplyString(encryptionKey);
                }
                else if (rbEncryptionKeyFile.Checked)
                {
                    if (!File.Exists(edEncryptionKeyFile.Text))
                    {
                        MessageBox.Show(this, "Unable to open file key for encryptor.");
                        return;
                    }

                    cryptoConfig.ApplyFile(edEncryptionKeyFile.Text);
                }
                else
                {
                    byte[] data = ConvertHexStringToByteArray(edEncryptionKeyHEX.Text); 
                    cryptoConfig.ApplyBinary(data);
                }
            }
            else
            {
                MessageBox.Show(this, "Unable to find encryptor filter interface.");
            }
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        private void SourceInitFile()
        {
            // An exception is thrown if cast fail
            filterGraphSource = (IFilterGraph2)new FilterGraph();
            captureGraphSource = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlSource = (IMediaControl)filterGraphSource;
            mediaEventExSource = (IMediaEventEx)filterGraphSource;

            var hr = this.mediaEventExSource.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphSource.SetFiltergraph(filterGraphSource);
            DsError.ThrowExceptionForHR(hr);

            filterGraphSource.AddSourceFilter(edSourceFile.Text, "Source file", out sourceVideoFilter);

            // sinks
            AddEncryptor();

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            Guid CLSID_VFRGB2YUV = new Guid("3BDA461E-12DB-4C24-9815-B68D1AA4D34A"); // "VisioForge RGB2YUV"
            rgb2yuvFilter = FilterGraphTools.AddFilterFromClsid(
                filterGraphSource,
                CLSID_VFRGB2YUV,
                "VisioForge RGB2YUV");

            if (rgb2yuvFilter == null)
            {
                MessageBox.Show(this, "Unable to create VisioForge RGB2YUV filter.");
                return;
            }

            hr = captureGraphSource.RenderStream(null, null, sourceVideoFilter, null, rgb2yuvFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, rgb2yuvFilter, videoEncFilter, muxFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, sourceVideoFilter, audioEncFilter, muxFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, muxFilter, null, fileWriterFilter);
            DsError.ThrowExceptionForHR(hr);

            if (cbDebugMode.Checked)
            {
                FilterGraphTools.SaveGraphFile(filterGraphSource, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisioForge", "video_encryption_source.grf"));
            }

            // Set the graph clock.
            IMediaFilter mediaFilter = filterGraphSource as IMediaFilter;
            if (mediaFilter != null)
            {
                hr = mediaFilter.SetSyncSource(null);
            }

            hr = mediaControlSource.Run();
        }

        private void SourceFree()
        {
            try
            {
                // Stop previewing data
                if (mediaControlSource != null)
                {
                    mediaControlSource.StopWhenReady();
                }

                // Stop receiving events
                if (mediaEventExSource != null)
                {
                    mediaEventExSource.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);
                }

                // Release DirectShow interfaces
                if (mediaControlSource != null)
                {
                    Marshal.ReleaseComObject(mediaControlSource);
                    mediaControlSource = null;
                }

                if (mediaEventExSource != null)
                {
                    Marshal.ReleaseComObject(mediaEventExSource);
                    mediaEventExSource = null;
                }

                if (filterGraphSource != null)
                {
                    Marshal.ReleaseComObject(filterGraphSource);
                    filterGraphSource = null;
                }

                if (captureGraphSource != null)
                {
                    Marshal.ReleaseComObject(captureGraphSource);
                    captureGraphSource = null;
                }

                if (sourceVideoFilter != null)
                {
                    Marshal.ReleaseComObject(sourceVideoFilter);
                    sourceVideoFilter = null;
                }

                if (sourceAudioFilter != null)
                {
                    Marshal.ReleaseComObject(sourceAudioFilter);
                    sourceAudioFilter = null;
                }

                if (videoEncFilter != null)
                {
                    Marshal.ReleaseComObject(videoEncFilter);
                    videoEncFilter = null;
                }

                if (audioEncFilter != null)
                {
                    Marshal.ReleaseComObject(audioEncFilter);
                    audioEncFilter = null;
                }

                if (muxFilter != null)
                {
                    Marshal.ReleaseComObject(muxFilter);
                    muxFilter = null;
                }

                if (fileWriterFilter != null)
                {
                    Marshal.ReleaseComObject(fileWriterFilter);
                    fileWriterFilter = null;
                }

                if (rgb2yuvFilter != null)
                {
                    Marshal.ReleaseComObject(rgb2yuvFilter);
                    rgb2yuvFilter = null;
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
                MessageBox.Show(this, "Completed!");
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

                if (mediaEventExSource == null)
                {
                    return;
                }

                while (mediaEventExSource.GetEvent(out eventCode, out param1, out param2, 0) == 0)
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
                    if (mediaEventExSource == null)
                    {
                        return;
                    }
                    // ReSharper restore HeuristicUnreachableCode
                    // ReSharper restore ConditionIsAlwaysTrueOrFalse

                    int hr = mediaEventExSource.FreeEventParams(eventCode, param1, param2);
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
            videoCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            foreach (var device in videoCaptureDevices)
            {
                cbVideoCaptureSource.Items.Add(device.Name);
            }

            if (cbVideoCaptureSource.Items.Count > 0)
            {
                cbVideoCaptureSource.SelectedIndex = 0;
            }

            audioCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);

            foreach (var device in audioCaptureDevices)
            {
                cbAudioCaptureSource.Items.Add(device.Name);
            }

            if (cbAudioCaptureSource.Items.Count > 0)
            {
                cbAudioCaptureSource.SelectedIndex = 0;
            }

            edOutput.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\" + "output.enc";

            //EnumCameraFormats1();
            //EnumCameraFormats2();
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

        private void btEncryptionOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                edEncryptionKeyFile.Text = openFileDialog1.FileName;
            }
        }
    }
}
