using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Player_Demo
{
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using VisioForge.DirectShowAPI;
    using VisioForge.DirectShowLib;

    public partial class Form1 : Form
    {
        private IFilterGraph2 filterGraph;

        private ICaptureGraphBuilder2 captureGraph;

        private IBaseFilter decryptor;

        private IMediaControl mediaControl;

        private IBaseFilter videoRenderer;

        /// <summary>
        /// Use your license key received after purchase.
        /// </summary>
        private const string SDK_LICENSE_KEY = "";

        private const int WM_GRAPHNOTIFY = 0x8000 + 1;

        public Form1()
        {
            InitializeComponent();
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
        private void ApplyDecryptorSettings()
        {
            IVFRegister reg = decryptor as IVFRegister;
            if (reg != null && !string.IsNullOrEmpty(SDK_LICENSE_KEY))
            {
                reg.SetLicenseKey(SDK_LICENSE_KEY);
            }

            IVFCryptoConfig cryptoConfig = decryptor as IVFCryptoConfig;
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

        private int UpdateVdeoWindow()
        {
            //var basicVideo = this.filterGraph as IBasicVideo;
            //if (basicVideo == null)
            //{
            //    return 0;
            //}

            //// Read the default video size
            //int hr = basicVideo.GetVideoSize(out var lWidth, out var lHeight);
            //if (hr == DsResults.E_NoInterface)
            //{
            //    return 0;
            //}

            //ClientSize = new Size(lWidth, lHeight);
            //Application.DoEvents();

            //hr = this.videoWindow.SetWindowPosition(0, 0, lWidth, lHeight);

            var rectDest = new DsRect();
            rectDest.Left = 0;
            rectDest.Top = 0;
            rectDest.Right = this.pnScreen.Width;
            rectDest.Bottom = this.pnScreen.Height;

            var vmrWindowlessControl = this.videoRenderer as IVMRWindowlessControl9;
            vmrWindowlessControl.SetVideoPosition(null, rectDest);

            return 0;
        }

        private void CreateGraph()
        {
            filterGraph = (IFilterGraph2)new FilterGraph();
            captureGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            captureGraph.SetFiltergraph(filterGraph);

            mediaControl = filterGraph as IMediaControl;

            var CLSID_FileSourceAsync = new Guid("E436EBB5-524F-11CE-9F53-0020AF0BA770");

            var source = FilterGraphTools.AddFilterFromClsid(
                    filterGraph,
                    CLSID_FileSourceAsync,
                    "File source (async.)");
            var fileSource = source as IFileSourceFilter;

            int hr = fileSource.Load(edSourceFile.Text, null);
            if (hr != 0)
            {
                MessageBox.Show(this, $"Unable to open encrypted file: {edSourceFile.Text}");
                return;
            }

            if (rbEncryptionModeAES128.Checked)
            {
                decryptor = FilterGraphTools.AddFilterFromClsid(
                    filterGraph,
                    new Guid(Consts.CLSID_VFVideoDecryptor8),
                    "VisioForge Decryptor v8");
            }
            else
            {
                decryptor = FilterGraphTools.AddFilterFromClsid(
                    filterGraph,
                    new Guid(Consts.CLSID_VFVideoDecryptor9),
                    "VisioForge Decryptor v9");
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            IVFCryptoConfig cryptoConfig = decryptor as IVFCryptoConfig;
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

            Guid CLSID_VideoMixingRenderer9 = new Guid("51b4abf3-748f-4e3b-a276-c828330e926a");
            videoRenderer = FilterGraphTools.AddFilterFromClsid(
                filterGraph,
                CLSID_VideoMixingRenderer9,
                "VMR-9");

            // ReSharper disable once SuspiciousTypeConversion.Global
            IVMRFilterConfig9 vmrFilterConfig = videoRenderer as IVMRFilterConfig9;

            if (vmrFilterConfig == null)
            {
                return;
            }

            vmrFilterConfig.SetRenderingMode(VMR9Mode.Windowless);
            IVMRWindowlessControl9 vmrWindowlessControl = videoRenderer as IVMRWindowlessControl9;

            vmrWindowlessControl?.SetVideoClippingWindow(pnScreen.Handle);

            hr = captureGraph.RenderStream(null, null, source, null, decryptor);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraph.RenderStream(null, MediaType.Video, decryptor, null, videoRenderer);
            DsError.ThrowExceptionForHR(hr);

            UpdateVdeoWindow();

            if (cbDebugMode.Checked)
            {
                FilterGraphTools.SaveGraphFile(filterGraph, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\video_encryption_player.grf");
            }
        }

        private void ClearGraph()
        {
            if (mediaControl != null)
            {
                Marshal.ReleaseComObject(mediaControl);
                mediaControl = null;
            }

            if (decryptor != null)
            {
                Marshal.ReleaseComObject(decryptor);
                decryptor = null;
            }

            if (filterGraph != null)
            {
                Marshal.ReleaseComObject(filterGraph);
                filterGraph = null;
            }

            if (captureGraph != null)
            {
                Marshal.ReleaseComObject(captureGraph);
                captureGraph = null;
            }
        }

        private void btSourceStart_Click(object sender, EventArgs e)
        {
            btSourceStart.Enabled = false;
            btSourceStop.Enabled = true;

            CreateGraph();

            int hr = mediaControl.Run();
            DsError.ThrowExceptionForHR(hr);
        }

        private void btSourceStop_Click(object sender, EventArgs e)
        {
            btSourceStop.Enabled = false;
            btSourceStart.Enabled = true;

            mediaControl.Stop();

            ClearGraph();

            pnScreen.Refresh();
        }

        private void btEncryptionOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                edEncryptionKeyFile.Text = openFileDialog1.FileName;
            }
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                edSourceFile.Text = openFileDialog1.FileName;
            }
        }
    }
}
