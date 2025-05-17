using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VideoEffects
{
    public partial class Form1 : Form, IAMGraphBuilderCallback
    {
        private IFilterGraph2 filterGraph;

        private ICaptureGraphBuilder2 captureGraph;

        private IMediaControl mediaControl;

        private VideoRendererEVR videoRenderer;

        private IBaseFilter source;

        private IBaseFilter videoEffects;

        private IBaseFilter videoResize;

        private IVFResize videoResizeIntf;

        private IBaseFilter videoRotate;

        private IVFResize videoRotateIntf;

        private IVFEffectsPro videoEffectsIntf2;

        private IVFEffects45 videoEffectsIntf;

        private IBaseFilter sourcePoint;

        private const int ID_TEXT_LOGO = 0;

        private const int ID_IMAGE_LOGO = 1;

        private const int ID_VFLIP = 2;

        private const int ID_HFLIP = 3;

        private const int ID_VMIRROR = 4;

        private const int ID_HMIRROR = 5;

        private const int ID_GRAYSCALE = 6;

        private const int ID_INVERT = 7;

        // Blacklisting very bad and ugly Microsoft decoder. Hope LAV available.
        private string[] blackList = { "Microsoft DTV-DVD Video Decoder" };

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds the video effects filter.
        /// </summary>
        private void AddVideoEffectsFilter()
        {
            videoEffects = DSHelper.AddFilterFromClsid(
                filterGraph,
                Consts.CLSID_VFVideoEffectsPro,
                "VisioForge Video Effects");

            int hr = captureGraph.RenderStream(null, MediaType.Video, sourcePoint, null, videoEffects);
            DsError.ThrowExceptionForHR(hr);
            sourcePoint = videoEffects;

            videoEffectsIntf2 = videoEffects as IVFEffectsPro;
            videoEffectsIntf2?.set_enabled(true, false, false, false);

            videoEffectsIntf = videoEffects as IVFEffects45;
            videoEffectsIntf?.clear_effects();

            UpdateTextLogo();
            UpdateImageLogo();
            UpdateHFlip();
            UpdateVFlip();
            UpdateHMirror();
            UpdateVMirror();
            UpdateGrayscale();
            UpdateInvert();
        }

        /// <summary>
        /// Adds the video resize / crop filter.
        /// </summary>
        private void AddVideoResizeCrop()
        {
            videoResize = DSHelper.AddFilterFromClsid(
                filterGraph,
                Consts.CLSID_VFResizer_4,
                "VisioForge Video Resize");

            int hr = captureGraph.RenderStream(null, MediaType.Video, sourcePoint, null, videoResize);
            DsError.ThrowExceptionForHR(hr);
            sourcePoint = videoResize;

            videoResizeIntf = videoResize as IVFResize;
            videoResizeIntf.put_FilterMode(VFResizeFilterMode.Resize);

            var cropLeft = Convert.ToUInt32(edCropLeft.Text);
            var cropTop = Convert.ToUInt32(edCropTop.Text);
            var cropRight = Convert.ToUInt32(edCropRight.Text);
            var cropBottom = Convert.ToUInt32(edCropBottom.Text);

            var newWidth = Convert.ToUInt32(edResizeWidth.Text);
            var newHeight = Convert.ToUInt32(edResizeHeight.Text);

            if (!cbResize.Checked)
            {
                // crop only
                videoResizeIntf.put_ResizeMode(VFResizeMode.CropOnly, false);

                // get source video size
                var inPin = DsFindPin.ByDirection(videoResize, PinDirection.Input, 0);

                var mt = new AMMediaType();
                hr = inPin.ConnectionMediaType(mt);
                if (hr == 0)
                {
                    var info = Marshal.PtrToStructure<VideoInfoHeader>(mt.formatPtr);

                    videoResizeIntf.put_Resolution((uint)(info.BmiHeader.Width - cropRight - cropLeft), (uint)(info.BmiHeader.Height - cropTop - cropBottom));
                }
            }
            else
            {
                videoResizeIntf.put_ResizeMode(VFResizeMode.Bilinear, false);
                videoResizeIntf.put_Resolution((uint)Width, (uint)Height);
            }

            if (cbCrop.Checked)
            {
                videoResizeIntf.put_Crop(cropLeft, cropTop, cropRight, cropBottom);
            }
        }

        /// <summary>
        /// Adds the video rotate filter (second resize filter used in rotate mode).
        /// </summary>
        private void AddVideoRotate()
        {
            videoRotate = DSHelper.AddFilterFromClsid(
                filterGraph,
                Consts.CLSID_VFResizer_4,
                "VisioForge Video Rotate");

            int hr = captureGraph.RenderStream(null, MediaType.Video, sourcePoint, null, videoRotate);
            DsError.ThrowExceptionForHR(hr);
            sourcePoint = videoRotate;

            videoRotateIntf = videoRotate as IVFResize;
            videoRotateIntf.put_FilterMode(VFResizeFilterMode.Rotate);

            if (cbRotate.SelectedIndex != 0)
            {
                videoRotateIntf.put_RotateMode((VFRotateMode)cbRotate.SelectedIndex);
            }
        }

        /// <summary>
        /// Adds the LAV source. We use LAV filters that support most video formats. You can use any other source filters.
        /// </summary>
        private void AddLAVSource()
        {
            var CLSID_LAVSplitterSource = new Guid("{B98D13E7-55DB-4385-A33D-09FD1BA26338}");
            source = DSHelper.AddFilterFromClsid(
                  filterGraph,
                  CLSID_LAVSplitterSource,
                  "LAV Splitter Source");
            sourcePoint = source;

            var fileSource = source as IFileSourceFilter;
            if (fileSource != null)
            {
                int hr = fileSource.Load(edFilename.Text, null);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            int hr = 0;

            ClearDS();

            filterGraph = (IFilterGraph2)new FilterGraph();
            captureGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            captureGraph.SetFiltergraph(filterGraph);

            SetCallback(filterGraph);

            mediaControl = filterGraph as IMediaControl;

            // add sources
            if (!string.IsNullOrEmpty(edFilename.Text) && File.Exists(edFilename.Text))
            {
                AddLAVSource();
            }
            else
            {
                MessageBox.Show("Source file not found.");
                return;
            }

            // add video resize / crop 
            if (cbResize.Checked || cbCrop.Checked)
            {
                AddVideoResizeCrop();
            }

            // add rotate
            if (cbRotate.SelectedIndex != 0)
            {
                AddVideoRotate();
            }

            // add video effects
            AddVideoEffectsFilter();

            // add video renderer
            videoRenderer = new VideoRendererEVR
            {
                ScreenHandle = pnScreen.Handle
            };

            videoRenderer.Init(filterGraph);
            videoRenderer.Update(filterGraph, pnScreen.Width, pnScreen.Height);

            // render
            hr = captureGraph.RenderStream(null, MediaType.Video, sourcePoint, null, videoRenderer.GetFilter());
            DsError.ThrowExceptionForHR(hr);
            sourcePoint = null;

            hr = mediaControl.Run();
            if (hr < 0)
            {
                DsError.ThrowExceptionForHR(hr);
            }

            videoRenderer.Update(filterGraph, pnScreen.Width, pnScreen.Height);

            //DSHelper.SaveGraphFile(filterGraph, Path.Combine(Application.StartupPath, "fin.grf"));
        }

        private void btSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                edFilename.Text = openFileDialog.FileName;
            }
        }

        private void SetCallback(IFilterGraph2 filterGraph)
        {
            try
            {
                IObjectWithSite obj = filterGraph as IObjectWithSite;
                if (obj != null)
                {
                    obj.SetSite(this);
                }
            }
            catch
            {
            }
        }

        private void ClearCallback(IFilterGraph2 filterGraph)
        {
            try
            {
                IObjectWithSite obj = filterGraph as IObjectWithSite;
                if (obj != null)
                {
                    obj.SetSite(null);
                }
            }
            catch
            {
            }
        }

        private void UpdateTextLogo()
        {
            if (cbTextLogo.Checked)
            {
                var eff = new VFVideoEffectSimple();
                eff.Type = (int)VFVideoEffectType.TextLogo;
                eff.ID = ID_TEXT_LOGO;
                eff.Enabled = true;
                eff.TextLogo = new VFTextLogo();
                eff.TextLogo.X = eff.TextLogo.Y = 10;
                eff.TextLogo.Text = "SAMPLE TEXT LOGO";
                eff.TextLogo.FontSize = 48;
                eff.TextLogo.FontName = "Arial";
                eff.TextLogo.FontBold = true;
                eff.TextLogo.TransparentBg = true;
                eff.TextLogo.FontColor = Color.White.ToArgb();

                videoEffectsIntf?.add_effect(eff);
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_TEXT_LOGO);
            }
        }

        private void UpdateImageLogo()
        {
            if (cbImageLogo.Checked)
            {
                var eff = new VFVideoEffectSimple();
                eff.Type = (int)VFVideoEffectType.GraphicalLogo;
                eff.ID = ID_IMAGE_LOGO;
                eff.Enabled = true;
                eff.GraphicalLogo = new VFGraphicalLogo();
                eff.GraphicalLogo.X = 10;
                eff.GraphicalLogo.Y = 100;
                eff.GraphicalLogo.Filename = Path.Combine(Application.StartupPath, "icon.png");
                eff.GraphicalLogo.StretchMode = (int)VFVideoEffectStretchMode.None;

                videoEffectsIntf?.add_effect(eff);
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_IMAGE_LOGO);
            }
        }

        private void UpdateHFlip()
        {
            if (cbHorizontalFlip.Checked)
            {
                videoEffectsIntf?.add_effect(CreateEffect(ID_HFLIP, VFVideoEffectType.FlipDown));
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_HFLIP);
            }
        }

        private void UpdateVFlip()
        {
            if (cbVerticalFlip.Checked)
            {
                videoEffectsIntf?.add_effect(CreateEffect(ID_VFLIP, VFVideoEffectType.FlipRight));
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_VFLIP);
            }
        }

        private void UpdateHMirror()
        {
            if (cbHorizontalMirror.Checked)
            {
                videoEffectsIntf?.add_effect(CreateEffect(ID_HMIRROR, VFVideoEffectType.MirrorDown));
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_HMIRROR);
            }
        }

        private void UpdateVMirror()
        {
            if (cbVerticalMirror.Checked)
            {
                videoEffectsIntf?.add_effect(CreateEffect(ID_VMIRROR, VFVideoEffectType.MirrorRight));
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_VMIRROR);
            }
        }

        private void UpdateGrayscale()
        {
            if (cbGrayscale.Checked)
            {
                videoEffectsIntf?.add_effect(CreateEffect(ID_GRAYSCALE, VFVideoEffectType.Greyscale));
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_GRAYSCALE);
            }
        }

        private void UpdateInvert()
        {
            if (cbInvert.Checked)
            {
                videoEffectsIntf?.add_effect(CreateEffect(ID_INVERT, VFVideoEffectType.Invert));
            }
            else
            {
                videoEffectsIntf?.remove_effect(ID_INVERT);
            }
        }

        private VFVideoEffectSimple CreateEffect(int id, VFVideoEffectType type_)
        {
            var eff = new VFVideoEffectSimple();
            eff.Type = (int)type_;
            eff.ID = id;
            eff.Enabled = true;

            return eff;
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            mediaControl?.Stop();

            ClearDS();

            pnScreen.Invalidate();
        }

        private void ClearDS()
        {
            if (filterGraph == null)
            {
                return;
            }

            DSHelper.RemoveAllFilters(filterGraph, true);

            if (videoRenderer != null)
            {
                videoRenderer.Dispose();
                videoRenderer = null;
            }

            if (videoResize != null)
            {
                Marshal.FinalReleaseComObject(videoResize);
                videoResize = null;
            }

            if (videoRotate != null)
            {
                Marshal.FinalReleaseComObject(videoRotate);
                videoRotate = null;
            }

            if (videoEffects != null)
            {
                Marshal.FinalReleaseComObject(videoEffects);
                videoEffects = null;
            }

            if (source != null)
            {
                Marshal.FinalReleaseComObject(source);
                source = null;
            }

            DSHelper.RemoveAllFilters(filterGraph, true);

            if (captureGraph != null)
            {
                Marshal.FinalReleaseComObject(captureGraph);
                captureGraph = null;
            }

            if (filterGraph != null)
            {
                ClearCallback(filterGraph);

                Marshal.FinalReleaseComObject(filterGraph);
                filterGraph = null;
            }

            mediaControl = null;

            videoEffectsIntf = null;
            videoEffectsIntf2 = null;
            videoResizeIntf = null;
            videoRotateIntf = null;

            GC.Collect(0);
            GC.WaitForPendingFinalizers();
            GC.Collect(0);
            GC.WaitForPendingFinalizers();
        }

        public int SelectedFilter(IMoniker moniker)
        {
            const string PP_FriendlyName = "FriendlyName";
            const int E_FAIL = unchecked((int)0x80004005);

            object name = null;
            try
            {
                if (moniker == null)
                {
                    return 0;
                }

                Guid iid = typeof(IPropertyBag).GUID;

                moniker.BindToStorage(null, null, ref iid, out var ppvObj);
                IPropertyBag propBag = ppvObj as IPropertyBag;

                if (propBag != null)
                {
                    propBag.Read(PP_FriendlyName, out name, null);
                }

                // check
                if (name != null && blackList.ToList().IndexOf(name.ToString()) != -1)
                {
                    return E_FAIL;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public int CreatedFilter(IBaseFilter pFil)
        {
            return 0;
        }

        private void cbTextLogo_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTextLogo();
        }

        private void cbImageLogo_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImageLogo();
        }

        private void cbHorizontalFlip_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHFlip();
        }

        private void cbVerticalFlip_CheckedChanged(object sender, EventArgs e)
        {
            UpdateVFlip();
        }

        private void cbVerticalMirror_CheckedChanged(object sender, EventArgs e)
        {
            UpdateVMirror();
        }

        private void cbHorizontalMirror_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHMirror();
        }

        private void cbGrayscale_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGrayscale();
        }

        private void cbInvert_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInvert();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbRotate.SelectedIndex = 0;
        }
    }
}
