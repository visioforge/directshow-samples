using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoMixerDemo
{
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security.Permissions;

    using VisioForge.DirectShowAPI;
    using VisioForge.DirectShowLib;

    public partial class Form1 : Form, IAMGraphBuilderCallback
    {
        private IFilterGraph2 filterGraph;

        private ICaptureGraphBuilder2 captureGraph;

        private IMediaControl mediaControl;

        private IMediaSeeking mediaSeeking;

        private VideoRendererEVR videoRenderer;

        private IBaseFilter audioRenderer;

        private VideoMixer videoMixer;

        private IBaseFilter audioMixer;

        private IBaseFilter source1;

        private IBaseFilter source2;

        private IBaseFilter source3;

        private IBaseFilter videoEffects;

        private IVFEffectsPro videoEffectsIntf2;

        private IVFEffects45 videoEffectsIntf;

        // Blacklisting very bad and ugly Microsoft decoder. Hope LAV available.
        private string[] blackList = { "Microsoft DTV-DVD Video Decoder" };

        public Form1()
        {
            InitializeComponent();
        }

        private void btSelectFile1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                edFilename1.Text = openFileDialog1.FileName;
            }
        }

        private void btSelectFile2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                edFilename2.Text = openFileDialog1.FileName;
            }
        }

        private void btSelectFile3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                edFilename3.Text = openFileDialog1.FileName;
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

                    // Marshal.ReleaseComObject(obj);
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

                    // Marshal.ReleaseComObject(obj);
                }
            }
            catch
            {
            }
        }

        private void AddVideoMixer()
        {
            videoMixer = new VideoMixer();
            videoMixer.Init(filterGraph);
            videoMixer.InitIntf();

            string bgImage = null;
            if (!string.IsNullOrEmpty(edBGImage.Text) && File.Exists(edBGImage.Text))
            {
                bgImage = edBGImage.Text;
            }

            videoMixer.SetOutputSettings(Convert.ToInt32(edOutputWidth.Text), Convert.ToInt32(edOutputHeight.Text), pnBGImage.BackColor, bgImage, 25, VFPIPResizeQuality.RQ_LINEAR);
        }

        private void ConfigureVideoMixer()
        {
            videoMixer.SetLayerPosition(0, Convert.ToInt32(edFile1X.Text), Convert.ToInt32(edFile1Y.Text), Convert.ToInt32(edFile1Width.Text), Convert.ToInt32(edFile1Height.Text));
            videoMixer.SetLayerSettings(0, 255, false, false, false);

            if (source2 != null)
            {
                videoMixer.SetLayerPosition(1, Convert.ToInt32(edFile2X.Text), Convert.ToInt32(edFile2Y.Text), Convert.ToInt32(edFile2Width.Text), Convert.ToInt32(edFile2Height.Text));
                videoMixer.SetLayerSettings(1, 255, false, false, false);
            }

            if (source3 != null)
            {
                videoMixer.SetLayerPosition(2, Convert.ToInt32(edFile3X.Text), Convert.ToInt32(edFile3Y.Text), Convert.ToInt32(edFile3Width.Text), Convert.ToInt32(edFile3Height.Text));
                videoMixer.SetLayerSettings(2, 255, false, false, false);
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
            mediaSeeking = filterGraph as IMediaSeeking;
            
            // add video mixer
            AddVideoMixer();

            // add sources
            if (!string.IsNullOrEmpty(edFilename1.Text) && File.Exists(edFilename1.Text))
            {
                source1 = DSHelper.AddFileSource(edFilename1.Text, filterGraph);
            }
            
            hr = captureGraph.RenderStream(null, MediaType.Video, source1, null, videoMixer.GetFilter());
            DsError.ThrowExceptionForHR(hr);
            
            if (!string.IsNullOrEmpty(edFilename2.Text) && File.Exists(edFilename2.Text))
            {
                source2 = DSHelper.AddFileSource(edFilename2.Text, filterGraph);
            }

            if (source2 != null)
            {
                hr = captureGraph.RenderStream(null, MediaType.Video, source2, null, videoMixer.GetFilter());
                DsError.ThrowExceptionForHR(hr);
            }

            if (!string.IsNullOrEmpty(edFilename3.Text) && File.Exists(edFilename3.Text))
            {
                source3 = DSHelper.AddFileSource(edFilename3.Text, filterGraph);
            }

            if (source3 != null)
            {
                hr = captureGraph.RenderStream(null, MediaType.Video, source3, null, videoMixer.GetFilter());
                DsError.ThrowExceptionForHR(hr);
            }

            // add video effectss
            if (cbTextLogo.Checked || cbImageLogo.Checked)
            {
                videoEffects = DSHelper.AddFilterFromClsid(
                    filterGraph,
                    Consts.CLSID_VFVideoEffectsPro,
                    "VisioForge Video Effects");

                videoEffectsIntf2 = videoEffects as IVFEffectsPro;
                videoEffectsIntf2?.set_enabled(true, false, false, false);

                videoEffectsIntf = videoEffects as IVFEffects45;
                videoEffectsIntf?.clear_effects();

                if (cbTextLogo.Checked)
                {
                    videoEffectsIntf?.add_effect(CreateTextLogo());
                }

                if (cbImageLogo.Checked)
                {
                    videoEffectsIntf?.add_effect(CreateImageLogo());
                }
            }

            // add video renderer
            videoRenderer = new VideoRendererEVR
                                {
                                    ScreenHandle = pnScreen.Handle
                                };
            videoRenderer.Init(filterGraph);
            videoRenderer.Update(filterGraph, pnScreen.Width, pnScreen.Height);

            hr = captureGraph.RenderStream(null, MediaType.Video, videoMixer.GetFilter(), videoEffects, videoRenderer.GetFilter());
            DsError.ThrowExceptionForHR(hr);

            // configure video mixer
            ConfigureVideoMixer();

            hr = mediaControl.Run();

            //Helper.SaveGraphFile(filterGraph, Path.Combine(Application.StartupPath, "fin.grf"));
        }

        private VFVideoEffectSimple CreateTextLogo()
        {
            var eff = new VFVideoEffectSimple();
            eff.Type = (int)VFVideoEffectType.TextLogo;
            eff.ID = 0;
            eff.Enabled = true;
            eff.TextLogo = new VFTextLogo();
            eff.TextLogo.X = eff.TextLogo.Y = 100;
            eff.TextLogo.Text = "SAMPLE TEXT LOGO";
            eff.TextLogo.FontSize = 48;
            eff.TextLogo.FontName = "Arial";
            eff.TextLogo.FontBold = true;
            eff.TextLogo.TransparentBg = true;
            eff.TextLogo.FontColor = Color.White.ToArgb();

            return eff;
        }

        private VFVideoEffectSimple CreateImageLogo()
        {
            var eff = new VFVideoEffectSimple();
            eff.Type = (int)VFVideoEffectType.GraphicalLogo;
            eff.ID = 1;
            eff.Enabled = true;
            eff.GraphicalLogo = new VFGraphicalLogo();
            eff.GraphicalLogo.X = 100;
            eff.GraphicalLogo.Y = 200;
            eff.GraphicalLogo.Filename = Path.Combine(Application.StartupPath, "icon.png");
            eff.GraphicalLogo.StretchMode = (int)VFVideoEffectStretchMode.None;

            return eff;
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            mediaControl?.Stop();

            ClearDS();

            pnScreen.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

            if (audioRenderer != null)
            {
                Marshal.FinalReleaseComObject(audioRenderer);
                audioRenderer = null;
            }

            if (videoMixer != null)
            {
                videoMixer.Dispose();
                videoMixer = null;
            }

            if (audioMixer != null)
            {
                Marshal.FinalReleaseComObject(audioMixer);
                audioMixer = null;
            }

            if (videoEffects != null)
            {
                Marshal.FinalReleaseComObject(videoEffects);
                videoEffects = null;
            }

            if (source1 != null)
            {
                Marshal.FinalReleaseComObject(source1);
                source1 = null;
            }

            if (source2 != null)
            {
                Marshal.FinalReleaseComObject(source2);
                source2 = null;
            }

            if (source3 != null)
            {
                Marshal.FinalReleaseComObject(source3);
                source3 = null;
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

            mediaSeeking = null;

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

                    // Marshal.ReleaseComObject(propBag);
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

        private void btSelectBGImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                edBGImage.Text = openFileDialog1.FileName;
            }
        }

        private void pnBGImage_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                pnBGImage.BackColor = colorDialog1.Color;
            }
        }
    }
}
