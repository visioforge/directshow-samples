using System;
using System.Windows.Forms;

namespace VLC_Source_Demo
{
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Collections.Generic;
    using System.Linq;

    using MediaFoundation;
    using MediaFoundation.EVR;
    using MediaFoundation.Misc;
    using VisioForge.DirectShowAPI;
    using VisioForge.DirectShowLib;

    public partial class Form1 : Form
    {
        private volatile bool _seekingFlag;

        private const int WM_GRAPHNOTIFY = 0x8000 + 1;

        private IFilterGraph2 filterGraph;

        private ICaptureGraphBuilder2 captureGraph;

        private IMediaControl mediaControl;

        private IMediaSeeking mediaSeeking;

        private IVideoWindow videoWindow;

        private IMediaEventEx mediaEventEx;

        private IBaseFilter sourceFilter;

        private IBaseFilter videoRenderer;

        public Form1()
        {
            InitializeComponent();
        }

        private void AddGraph()
        {
            // An exception is thrown if cast fail
            filterGraph = (IFilterGraph2)new FilterGraph();
            captureGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControl = (IMediaControl)filterGraph;
            mediaSeeking = (IMediaSeeking)filterGraph;
            mediaEventEx = (IMediaEventEx)filterGraph;

            var hr = mediaEventEx.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraph.SetFiltergraph(filterGraph);
            DsError.ThrowExceptionForHR(hr);
        }

        private bool AddSource()
        {
            sourceFilter = FilterGraphTools.AddFilterFromClsid(filterGraph, Consts.CLSID_VFVLCSource, "VLC Source");
            if (sourceFilter == null)
            {
                MessageBox.Show(this, "Unable to create VLC source filter. Be sure that you've selected the correct CPU architecture for the project.");
                return false;
            }

            // Set custom command-line parameters if enabled
            if (cbUseCustomParams.Checked)
            {
                SetCustomCommandLineParameters();
            }

            // load file / network stream
            var sourceFilterIntf = sourceFilter as IFileSourceFilter;
            int hr = sourceFilterIntf.Load(edFilename.Text, null);
            DsError.ThrowExceptionForHR(hr);

            return true;
        }

        private void SetCustomCommandLineParameters()
        {
            // Try to get IVlcSrc3 interface first (latest), fallback to IVlcSrc2
            var vlcSrc3 = sourceFilter as IVlcSrc3;
            var vlcSrc2 = sourceFilter as IVlcSrc2;

            if (vlcSrc3 == null && vlcSrc2 == null)
            {
                MessageBox.Show(this, "VLC Source filter does not support custom command line parameters (IVlcSrc2/IVlcSrc3 interface not found).");
                return;
            }

            // Parse parameters from the text box
            var parametersText = edCustomParams.Text;
            if (string.IsNullOrWhiteSpace(parametersText))
            {
                return;
            }

            // Split by newlines and spaces, filter out empty entries
            var parameters = parametersText
                .Split(new[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim())
                .ToList();

            if (parameters.Count == 0)
            {
                return;
            }

            // Allocate native UTF-8 strings
            var nativeStrings = new IntPtr[parameters.Count];
            try
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    nativeStrings[i] = StringHelper.NativeUtf8FromString(parameters[i]);
                }

                // Call SetCustomCommandLine
                int hr;
                if (vlcSrc3 != null)
                {
                    hr = vlcSrc3.SetCustomCommandLine(nativeStrings, parameters.Count);
                }
                else
                {
                    hr = vlcSrc2.SetCustomCommandLine(nativeStrings, parameters.Count);
                }

                if (hr != 0)
                {
                    MessageBox.Show(this, $"Failed to set custom VLC parameters. HRESULT: 0x{hr:X8}");
                }
            }
            finally
            {
                // Free allocated native strings
                foreach (var ptr in nativeStrings)
                {
                    StringHelper.FreeNativeUtf8(ptr);
                }
            }
        }

        private void AddVideoRenderer()
        {
            Guid CLSID_EVR = new Guid("FA10746C-9B63-4B6C-BC49-FC300EA5F256");
            videoRenderer = FilterGraphTools.AddFilterFromClsid(filterGraph, CLSID_EVR, "EVR");

            // ReSharper disable once SuspiciousTypeConversion.Global
            var pConfig = (IEVRFilterConfig)videoRenderer;

            if (pConfig != null)
            {
                pConfig.SetNumberOfStreams(1);
            }
            else
            {
                throw new Exception("Unable to query IEVRFilterConfig interface.");
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            var getService = (MediaFoundation.IMFGetService)videoRenderer;
            if (getService != null)
            {
                // FDS_pEVRFilterConfig.SetNumberOfStreams(dwNumStreams);
            }
            else
            {
                throw new Exception("Unable to query IMFGetService interface.");
            }

            getService.GetService(
                MFServices.MR_VIDEO_RENDER_SERVICE,
                typeof(IMFVideoDisplayControl).GUID,
                out var videoDisplayControlObj);
            if (videoDisplayControlObj != null)
            {
                var dsMFVideoDisplayControl = videoDisplayControlObj as IMFVideoDisplayControl;
                dsMFVideoDisplayControl?.SetVideoWindow(this.pnScreen.Handle);

                var rectSrc = new MFVideoNormalizedRect(0, 0, 1, 1);
                var rectDest = new MFRect(0, 0, pnScreen.Width, pnScreen.Height);

                dsMFVideoDisplayControl.SetVideoPosition(rectSrc, rectDest);
            }
            else
            {
                throw new Exception("Unable to query IMFVideoDisplayControl interface.");
            }

            // Render the preview pin on the video capture filter
            int hr = captureGraph.RenderStream(null, MediaType.Video, sourceFilter, null, videoRenderer);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraph.RenderStream(null, MediaType.Audio, sourceFilter, null, null);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            AddGraph();

            if (!AddSource())
            {
                return;
            }
            
            AddVideoRenderer();

            mediaControl.Run();

            tmProgress.Start();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            try
            {
                tmProgress.Stop();

                // Stop previewing data
                if (mediaControl != null)
                {
                    mediaControl.StopWhenReady();
                    mediaControl.Stop();
                }

                // Stop receiving events
                mediaEventEx?.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);

                // Relinquish ownership (IMPORTANT!) of the video window.
                // Failing to call put_Owner can lead to assert failures within
                // the video renderer, as it still assumes that it has a valid
                // parent window.
                if (videoWindow != null)
                {
                    videoWindow.put_Visible(OABool.False);
                    videoWindow.put_Owner(IntPtr.Zero);
                }

                FilterGraphTools.RemoveAllFilters(filterGraph);

                // Release DirectShow interfaces
                if (mediaControl != null)
                {
                    Marshal.ReleaseComObject(mediaControl);
                    mediaControl = null;
                }

                if (mediaEventEx != null)
                {
                    Marshal.ReleaseComObject(mediaEventEx);
                    mediaEventEx = null;
                }

                if (videoWindow != null)
                {
                    Marshal.ReleaseComObject(videoWindow);
                    videoWindow = null;
                }

                Marshal.ReleaseComObject(filterGraph);
                filterGraph = null;
                Marshal.ReleaseComObject(captureGraph);
                captureGraph = null;
                Marshal.ReleaseComObject(sourceFilter);
                sourceFilter = null;
                Marshal.ReleaseComObject(videoRenderer);
                videoRenderer = null;
            }
            catch
            {
            }
        }

        private TimeSpan GetDuration()
        {
            if (mediaSeeking != null)
            {
                mediaSeeking.GetDuration(out var dur);
                return TimeSpan.FromMilliseconds(dur / 10000.0);
            }

            return TimeSpan.Zero;
        }

        private TimeSpan GetPosition()
        {
            int hr = mediaSeeking.SetTimeFormat(TimeFormat.MediaTime);
            if (hr != 0)
            {
                throw new Exception("Unable to set seeking time format to MediaTime");
            }

            hr = mediaSeeking.GetCurrentPosition(out var pos);
            if (hr != 0)
            {
                //Logger?.Error(Resources.Resources.Unable_to_get_current_position);
            }

            return TimeSpan.FromMilliseconds(pos / 10000.0);
        }

        /// <summary>
        /// Sets current position.
        /// </summary>
        /// <param name="position">
        /// Time.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool PositionSet(TimeSpan position)
        {
            bool result;

            if (_seekingFlag)
            {
                return true;
            }

            try
            {
                _seekingFlag = true;

                try
                {
                    result = false;

                    var pos = (long)(position.TotalMilliseconds * 10000);

                    // ReSharper disable ConvertToConstant.Local
                    var stopPos = 0;
                    // ReSharper restore ConvertToConstant.Local
                    if (mediaSeeking != null)
                    {
                        var hr = this.mediaSeeking.SetTimeFormat(TimeFormat.MediaTime);
                        if (hr != 0)
                        {
                            Debug.WriteLine("Unable to set seeking time format to MediaTime");
                        }

                        hr = mediaSeeking.SetPositions(
                            pos,
                            AMSeekingSeekingFlags.AbsolutePositioning,
                            stopPos,
                            AMSeekingSeekingFlags.NoPositioning);
                        if (hr < 0)
                        {
                            Debug.WriteLine("Unable to set current position");
                        }
                        else
                        {
                            result = true;
                        }

                        if (Path.GetExtension(edFilename.Text)?.ToLowerInvariant() == ".wmv" ||
                            Path.GetExtension(edFilename.Text)?.ToLowerInvariant() == ".asf")
                        {
                            Thread.Sleep(300);
                        }
                    }

                }
                catch
                {
                    return false;
                }
            }
            finally
            {
                _seekingFlag = false;
            }

            return result;
        }

        private void btResume_Click(object sender, EventArgs e)
        {
            mediaControl.Run();
        }

        private void btPause_Click(object sender, EventArgs e)
        {
            mediaControl.Pause();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void tmProgress_Tick(object sender, EventArgs e)
        {
            tmProgress.Tag = 1;

            var duration = GetDuration();
            var position = GetPosition();
            tbTimeline.Maximum = (int)duration.TotalSeconds;

            int value = (int)position.TotalSeconds;
            if ((value > 0) && (value < tbTimeline.Maximum))
            {
                tbTimeline.Value = value;
            }

            lbTime.Text = position.ToString(@"hh\:mm\:ss")  + "/" + duration.ToString(@"hh\:mm\:ss");

            tmProgress.Tag = 0;
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
            mediaSeeking.SetRate(tbSpeed.Value / 10.0);
        }

        private void tbTimeline_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(tmProgress.Tag) == 0)
            {
                PositionSet(TimeSpan.FromSeconds(tbTimeline.Value));
            }
        }

        private void btSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                edFilename.Text = openFileDialog1.FileName;
            }
        }
    }
}
