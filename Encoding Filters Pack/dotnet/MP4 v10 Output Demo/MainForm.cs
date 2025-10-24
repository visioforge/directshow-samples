using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using VisioForge.DirectShowLib.DES;
using VisioForge.DirectShowLib;
using VisioForge.DirectShowAPI;

namespace MP4_v10_Output_Demo
{
    public partial class MainForm : Form
    {
        // DirectShow Editing Services objects
        private IAMTimeline timeline;
        private IRenderEngine renderEngine;
        private IGraphBuilder graphBuilder;
        private IMediaControl mediaControl;
        private IMediaEventEx mediaEvent;
        private IFileSinkFilter fileSink;

        // UI controls
        private ListBox listBoxInputFiles;
        private Button btnAddVideoFile;
        private Button btnAddAudioFile;
        private Button btnRemoveFile;
        private Button btnClearFiles;
        private TextBox textBoxOutputFile;
        private Button btnBrowseOutput;
        private Button btnStart;
        private Button btnStop;
        private ProgressBar progressBar;
        private Label labelStatus;
        private GroupBox groupBoxInputFiles;
        private GroupBox groupBoxOutput;
        private GroupBox groupBoxProgress;
        private TextBox txtLog;

        // Progress + diagnostics
        private System.Windows.Forms.Timer progressTimer; // keep a reference so it isn't GC'ed
        private IMediaSeeking mediaSeeking; // cached seeking interface
        private long totalDuration100ns; // timeline duration in100-ns units
        private DateTime runStartUtc;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "MP4 v10 Output Demo - DirectShow";
            this.Size = new Size(800,700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Input Files Group
            groupBoxInputFiles = new GroupBox
            {
                Text = "Input Files",
                Location = new Point(10,10),
                Size = new Size(760,250)
            };

            listBoxInputFiles = new ListBox
            {
                Location = new Point(10,25),
                Size = new Size(640,180),
                SelectionMode = SelectionMode.One
            };

            btnAddVideoFile = new Button
            {
                Text = "Add Video File",
                Location = new Point(660,25),
                Size = new Size(90,30)
            };
            btnAddVideoFile.Click += BtnAddVideoFile_Click;

            btnAddAudioFile = new Button
            {
                Text = "Add Audio File",
                Location = new Point(660,60),
                Size = new Size(90,30)
            };
            btnAddAudioFile.Click += BtnAddAudioFile_Click;

            btnRemoveFile = new Button
            {
                Text = "Remove File",
                Location = new Point(660,95),
                Size = new Size(90,30)
            };
            btnRemoveFile.Click += BtnRemoveFile_Click;

            btnClearFiles = new Button
            {
                Text = "Clear All",
                Location = new Point(660,130),
                Size = new Size(90,30)
            };
            btnClearFiles.Click += BtnClearFiles_Click;

            groupBoxInputFiles.Controls.Add(listBoxInputFiles);
            groupBoxInputFiles.Controls.Add(btnAddVideoFile);
            groupBoxInputFiles.Controls.Add(btnAddAudioFile);
            groupBoxInputFiles.Controls.Add(btnRemoveFile);
            groupBoxInputFiles.Controls.Add(btnClearFiles);

            // Output Group
            groupBoxOutput = new GroupBox
            {
                Text = "Output Settings",
                Location = new Point(10,270),
                Size = new Size(760,100)
            };

            Label labelOutput = new Label
            {
                Text = "Output File:",
                Location = new Point(10,30),
                Size = new Size(80,20)
            };

            textBoxOutputFile = new TextBox
            {
                Location = new Point(100,27),
                Size = new Size(550,25),
                Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "output.mp4")
            };

            btnBrowseOutput = new Button
            {
                Text = "Browse...",
                Location = new Point(660,25),
                Size = new Size(90,30)
            };
            btnBrowseOutput.Click += BtnBrowseOutput_Click;

            Label labelInfo = new Label
            {
                Text = "Using VisioForge_MP4_Muxer_v10 DirectShow filter",
                Location = new Point(10,60),
                Size = new Size(400,20),
                ForeColor = Color.Gray
            };

            groupBoxOutput.Controls.Add(labelOutput);
            groupBoxOutput.Controls.Add(textBoxOutputFile);
            groupBoxOutput.Controls.Add(btnBrowseOutput);
            groupBoxOutput.Controls.Add(labelInfo);

            // Progress Group
            groupBoxProgress = new GroupBox
            {
                Text = "Progress",
                Location = new Point(10,380),
                Size = new Size(760,260)
            };

            btnStart = new Button
            {
                Text = "Start",
                Location = new Point(10,25),
                Size = new Size(100,40)
            };
            btnStart.Click += BtnStart_Click;

            btnStop = new Button
            {
                Text = "Stop",
                Location = new Point(120,25),
                Size = new Size(100,40),
                Enabled = false
            };
            btnStop.Click += BtnStop_Click;

            progressBar = new ProgressBar
            {
                Location = new Point(10,75),
                Size = new Size(740,30)
            };

            labelStatus = new Label
            {
                Text = "Ready",
                Location = new Point(10,115),
                Size = new Size(740,20),
                AutoSize = false
            };

            txtLog = new TextBox
            {
                Location = new Point(10,140),
                Size = new Size(740,100),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            groupBoxProgress.Controls.Add(btnStart);
            groupBoxProgress.Controls.Add(btnStop);
            groupBoxProgress.Controls.Add(progressBar);
            groupBoxProgress.Controls.Add(labelStatus);
            groupBoxProgress.Controls.Add(txtLog);

            // Add all groups to form
            this.Controls.Add(groupBoxInputFiles);
            this.Controls.Add(groupBoxOutput);
            this.Controls.Add(groupBoxProgress);
        }

        private void Log(string message)
        {
            try
            {
                var ts = DateTime.Now.ToString("HH:mm:ss.fff");
                var line = $"[{ts}] {message}";
                labelStatus.Text = message;
                txtLog.AppendText(line + Environment.NewLine);
                Debug.WriteLine(line);
            }
            catch
            {
                // ignore logging errors
            }
        }

        private void BtnAddVideoFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Video File";
                openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.wmv;*.mkv;*.mov;*.mpg;*.mpeg;*.m4v|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    listBoxInputFiles.Items.Add($"[VIDEO] {openFileDialog.FileName}");
                }
            }
        }

        private void BtnAddAudioFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Audio File";
                openFileDialog.Filter = "Audio Files|*.mp3;*.wav;*.aac;*.m4a;*.wma;*.ogg;*.flac|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    listBoxInputFiles.Items.Add($"[AUDIO] {openFileDialog.FileName}");
                }
            }
        }

        private void BtnRemoveFile_Click(object sender, EventArgs e)
        {
            if (listBoxInputFiles.SelectedIndex >=0)
            {
                listBoxInputFiles.Items.RemoveAt(listBoxInputFiles.SelectedIndex);
            }
        }

        private void BtnClearFiles_Click(object sender, EventArgs e)
        {
            listBoxInputFiles.Items.Clear();
        }

        private void BtnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Select Output File";
                saveFileDialog.Filter = "MP4 Files|*.mp4";
                saveFileDialog.DefaultExt = "mp4";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxOutputFile.Text = saveFileDialog.FileName;
                }
            }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (listBoxInputFiles.Items.Count ==0)
            {
                MessageBox.Show("Please add at least one input file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxOutputFile.Text))
            {
                MessageBox.Show("Please specify an output file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                progressBar.Value =0;
                totalDuration100ns =0;
                mediaSeeking = null;
                labelStatus.Text = "Initializing DirectShow Editing Services...";
                txtLog.Clear();
                Log("Initializing...");

                // Create timeline
                timeline = (IAMTimeline)new AMTimeline();
                Log("Timeline created.");

                // Create render engine
                renderEngine = (IRenderEngine)new RenderEngine();
                Log("RenderEngine created.");

                // Create filter graph
                graphBuilder = (IGraphBuilder)new FilterGraph();
                int hr = renderEngine.SetFilterGraph(graphBuilder);
                if (hr <0) { LogError("SetFilterGraph", hr, true); return; }
                Log("FilterGraph created and set on RenderEngine.");

                // Create group for video
                IAMTimelineObj groupObj;
                timeline.CreateEmptyNode(out groupObj, TimelineMajorType.Group);
                IAMTimelineGroup group = (IAMTimelineGroup)groupObj;

                // Set group media type to video (details will be specified after probing first source)
                AMMediaType mediaType = new AMMediaType();
                mediaType.majorType = MediaType.Video;
                group.SetMediaType(mediaType);
                DsUtils.FreeAMMediaType(mediaType);
                Log("Video group created.");

                // Add group to timeline
                timeline.AddGroup(groupObj);

                // Create track for video
                IAMTimelineObj trackObj;
                timeline.CreateEmptyNode(out trackObj, TimelineMajorType.Track);
                IAMTimelineTrack track = (IAMTimelineTrack)trackObj;

                // Cast group to composition to add tracks
                IAMTimelineComp comp = (IAMTimelineComp)group;
                comp.VTrackInsBefore(trackObj, -1);

                // Add video sources to timeline
                long currentTime =0;
                int videoCount =0;
                double? firstVideoFPS = null;
                int? firstVideoWidth = null;
                int? firstVideoHeight = null;
                foreach (string item in listBoxInputFiles.Items)
                {
                    if (item.StartsWith("[VIDEO]"))
                    {
                        string filePath = item.Substring(item.IndexOf("]") +2);
                        if (!File.Exists(filePath))
                        {
                            Log($"File not found: {filePath}");
                            continue;
                        }

                        IAMTimelineObj sourceObj;
                        timeline.CreateEmptyNode(out sourceObj, TimelineMajorType.Source);
                        IAMTimelineSrc source = (IAMTimelineSrc)sourceObj;

                        // Set source filename
                        source.SetMediaName(filePath);

                        // Get media length and fps using IMediaDet
                        IMediaDet mediaDet = (IMediaDet)new MediaDet();
                        mediaDet.put_Filename(filePath);
                        mediaDet.put_CurrentStream(0);
                        double length;
                        mediaDet.get_StreamLength(out length);
                        double fpsDet =0;
                        try { mediaDet.get_FrameRate(out fpsDet); } catch { fpsDet =0; }
                        if (!firstVideoFPS.HasValue && fpsDet >0.1) firstVideoFPS = fpsDet;

                        // Probe first video width/height
                        if (!firstVideoWidth.HasValue || !firstVideoHeight.HasValue)
                        {
                            try
                            {
                                AMMediaType srcType = new AMMediaType();
                                mediaDet.get_StreamMediaType(srcType);
                                if (srcType.formatType == FormatType.VideoInfo && srcType.formatPtr != IntPtr.Zero)
                                {
                                    var vihSrc = (VideoInfoHeader)Marshal.PtrToStructure(srcType.formatPtr, typeof(VideoInfoHeader));
                                    firstVideoWidth = vihSrc.BmiHeader.Width;
                                    firstVideoHeight = Math.Abs(vihSrc.BmiHeader.Height);
                                }
                                DsUtils.FreeAMMediaType(srcType);
                            }
                            catch { }
                        }

                        long lengthInUnits = (long)(length *10000000);

                        // Set timeline position
                        sourceObj.SetStartStop(currentTime, currentTime + lengthInUnits);
                        currentTime += lengthInUnits;

                        // Add source to track
                        track.SrcAdd(sourceObj);
                        videoCount++;
                        Log($"Added video source: {Path.GetFileName(filePath)} ({length:F2}s)");

                        Marshal.ReleaseComObject(mediaDet);
                        Marshal.ReleaseComObject(source);
                        Marshal.ReleaseComObject(sourceObj);
                    }
                }

                // Ensure group output has AvgTimePerFrame defined
                if (!firstVideoFPS.HasValue || firstVideoFPS.Value <=0.1)
                {
                    firstVideoFPS =30.0; // reasonable default
                    Log("No FPS detected, defaulting video group output to30.0 fps");
                }
                int setFpsHr = ((IAMTimelineGroup)group).SetOutputFPS(firstVideoFPS.Value);
                if (setFpsHr ==0)
                {
                    Log($"Video group output FPS set: {firstVideoFPS.Value:F3}");
                }
                else
                {
                    Log($"Warning: SetOutputFPS failed hr=0x{setFpsHr:X8}");
                }

                // If we have width/height, set full RGB24 VIDEOINFOHEADER on the group to avoid invalid media type
                if (firstVideoWidth.HasValue && firstVideoHeight.HasValue)
                {
                    try
                    {
                        int w = firstVideoWidth.Value;
                        int h = firstVideoHeight.Value;
                        double fps = firstVideoFPS.Value;

                        var vih = new VideoInfoHeader();
                        vih.SrcRect = new DsRect(0,0, w, h);
                        vih.TargetRect = new DsRect(0,0, w, h);
                        vih.BitRate =0;
                        vih.BitErrorRate =0;
                        vih.AvgTimePerFrame = (long)(10000000.0 / Math.Max(0.1, fps));

                        var bmi = new BitmapInfoHeader();
                        bmi.Size = Marshal.SizeOf(typeof(BitmapInfoHeader));
                        bmi.Width = w;
                        bmi.Height = h; // positive = bottom-up
                        bmi.Planes =1;
                        bmi.BitCount =24;
                        bmi.Compression =0; // BI_RGB
                        bmi.ImageSize = w * h *3;
                        bmi.ClrImportant =0;
                        vih.BmiHeader = bmi;

                        IntPtr pVih = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VideoInfoHeader)));
                        Marshal.StructureToPtr(vih, pVih, false);

                        AMMediaType mtGroup = new AMMediaType
                        {
                            majorType = MediaType.Video,
                            subType = MediaSubType.RGB24,
                            formatType = FormatType.VideoInfo,
                            fixedSizeSamples = true,
                            temporalCompression = false,
                            sampleSize = (int)bmi.ImageSize,
                            formatPtr = pVih,
                            formatSize = Marshal.SizeOf(typeof(VideoInfoHeader))
                        };

                        int mtHr = ((IAMTimelineGroup)group).SetMediaType(mtGroup);
                        DsUtils.FreeAMMediaType(mtGroup); // frees formatPtr

                        if (mtHr ==0)
                        {
                            Log($"DES video group output format set RGB24 {w}x{h} @{fps:F3}fps.");
                        }
                        else
                        {
                            Log($"Warning: SetMediaType RGB24 {w}x{h} failed hr=0x{mtHr:X8}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Warning: failed to set group VIDEOINFOHEADER: {ex.Message}");
                    }
                }
                else
                {
                    Log("Width/height not detected for first video; skipping explicit RGB24 format on group.");
                }

                // === Audio group and track ===
                int audioCount =0;
                IAMTimelineObj audioGroupObj = null;
                IAMTimelineGroup audioGroup = null;
                IAMTimelineObj audioTrackObj = null;
                IAMTimelineTrack audioTrack = null;
                long currentTimeAudio =0;

                // Create audio group if any audio files exist
                bool hasAudioItems = false;
                foreach (string item in listBoxInputFiles.Items)
                {
                    if (item.StartsWith("[AUDIO]")) { hasAudioItems = true; break; }
                }

                if (hasAudioItems)
                {
                    timeline.CreateEmptyNode(out audioGroupObj, TimelineMajorType.Group);
                    audioGroup = (IAMTimelineGroup)audioGroupObj;

                    // Set audio group media type to Audio only (let graph negotiate PCM)
                    AMMediaType audioMT = new AMMediaType { majorType = MediaType.Audio };
                    audioGroup.SetMediaType(audioMT);
                    DsUtils.FreeAMMediaType(audioMT);

                    timeline.AddGroup(audioGroupObj);

                    // Create audio track
                    timeline.CreateEmptyNode(out audioTrackObj, TimelineMajorType.Track);
                    audioTrack = (IAMTimelineTrack)audioTrackObj;
                    IAMTimelineComp compAudio = (IAMTimelineComp)audioGroup;
                    compAudio.VTrackInsBefore(audioTrackObj, -1);

                    // Add audio sources
                    foreach (string item in listBoxInputFiles.Items)
                    {
                        if (!item.StartsWith("[AUDIO]")) continue;

                        string filePath = item.Substring(item.IndexOf("]") +2);
                        if (!File.Exists(filePath))
                        {
                            Log($"Audio file not found: {filePath}");
                            continue;
                        }

                        IAMTimelineObj aSrcObj;
                        timeline.CreateEmptyNode(out aSrcObj, TimelineMajorType.Source);
                        IAMTimelineSrc aSrc = (IAMTimelineSrc)aSrcObj;
                        aSrc.SetMediaName(filePath);

                        // Length using IMediaDet (stream0 should be audio for pure audio files)
                        IMediaDet aDet = (IMediaDet)new MediaDet();
                        aDet.put_Filename(filePath);
                        aDet.put_CurrentStream(0);
                        double aLen;
                        aDet.get_StreamLength(out aLen);
                        long aLenUnits = (long)(aLen *10000000);

                        aSrcObj.SetStartStop(currentTimeAudio, currentTimeAudio + aLenUnits);
                        currentTimeAudio += aLenUnits;

                        audioTrack.SrcAdd(aSrcObj);
                        audioCount++;
                        Log($"Added audio source: {Path.GetFileName(filePath)} ({aLen:F2}s)");

                        Marshal.ReleaseComObject(aDet);
                        Marshal.ReleaseComObject(aSrc);
                        Marshal.ReleaseComObject(aSrcObj);
                    }
                }

                if (videoCount ==0)
                {
                    MessageBox.Show("Please add at least one video input file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    CleanupDirectShow();
                    return;
                }

                // Cache total duration for progress fallback
                long tlDur;
                if (timeline.GetDuration(out tlDur) ==0)
                {
                    totalDuration100ns = tlDur;
                    Log($"Timeline duration: {TimeSpan.FromTicks(totalDuration100ns)}");
                }

                Marshal.ReleaseComObject(track);
                Marshal.ReleaseComObject(trackObj);
                if (hasAudioItems)
                {
                    Marshal.ReleaseComObject(audioTrack);
                    Marshal.ReleaseComObject(audioTrackObj);
                    Marshal.ReleaseComObject(audioGroup);
                    Marshal.ReleaseComObject(audioGroupObj);
                }
                // Don't release comp separately - it's the same COM object as group
                Marshal.ReleaseComObject(group);
                Marshal.ReleaseComObject(groupObj);

                // Set timeline on render engine
                hr = renderEngine.SetTimelineObject(timeline);
                if (hr <0) { LogError("SetTimelineObject", hr, true); return; }

                // Build front end (sources and compositing)
                hr = renderEngine.ConnectFrontEnd();
                if (hr <0) { LogError("ConnectFrontEnd", hr, true); return; }
                Log("RenderEngine front-end connected.");

                // Add MP4 Muxer v10 and connect via H264 encoder
                IBaseFilter mp4Muxer = null;
                // H264 encoder (v10) CLSID
                Guid h264V10CLSID = new Guid("EA1FED6B-B876-4DB0-B7B1-778463E59978");
                // RGB2YUV converter CLSID
                Guid rgb2yuvCLSID = new Guid("3BDA461E-12DB-4C24-9815-B68D1AA4D34A");
                // MP4 v10 muxer CLSID
                Guid mp4MuxerCLSID = new Guid("0B0D654C-7AC1-441E-9C4D-3C29ABEDB6A8");

                // Create and add filters
                IBaseFilter rgb2yuv = null;
                IBaseFilter h264Enc = null;

                try
                {
                    // Create muxer
                    Type mp4MuxerType = Type.GetTypeFromCLSID(mp4MuxerCLSID);
                    if (mp4MuxerType != null)
                    {
                        mp4Muxer = (IBaseFilter)Activator.CreateInstance(mp4MuxerType);
                        int addHr = graphBuilder.AddFilter(mp4Muxer, "VisioForge MP4 Muxer v10");
                        if (addHr <0) { LogError("AddFilter(MP4 Muxer v10)", addHr, true); return; }
                        Log("MP4 Muxer v10 filter added.");

                        // Configure MP4 Muxer v10
                        IMP4V10MuxerConfig mp4Config = mp4Muxer as IMP4V10MuxerConfig;
                        if (mp4Config != null)
                        {
                            mp4Config.SetLiveDisabled(true);
                            Log("MP4 Muxer configured (LiveDisabled=true).");
                        }

                        // Set output filename via sink
                        fileSink = mp4Muxer as IFileSinkFilter;
                        if (fileSink != null)
                        {
                            int fsHr = fileSink.SetFileName(textBoxOutputFile.Text, null);
                            if (fsHr <0) { LogError("IFileSinkFilter.SetFileName", fsHr, true); return; }
                            Log($"Output file set: {textBoxOutputFile.Text}");
                        }
                    }
                    else
                    {
                        Log("MP4 Muxer v10 CLSID not found; cannot create muxer.");
                        return;
                    }

                    // Create RGB2YUV converter (helps encoder accept RGB sources)
                    Type rgb2yuvType = Type.GetTypeFromCLSID(rgb2yuvCLSID);
                    if (rgb2yuvType != null)
                    {
                        rgb2yuv = (IBaseFilter)Activator.CreateInstance(rgb2yuvType);
                        int addRgb = graphBuilder.AddFilter(rgb2yuv, "VisioForge RGB2YUV");
                        if (addRgb <0) { LogError("AddFilter(RGB2YUV)", addRgb, true); return; }
                        Log("RGB2YUV filter added.");
                    }

                    // Create H264 encoder v10
                    Type h264Type = Type.GetTypeFromCLSID(h264V10CLSID);
                    if (h264Type != null)
                    {
                        h264Enc = (IBaseFilter)Activator.CreateInstance(h264Type);
                        int addH264 = graphBuilder.AddFilter(h264Enc, "VisioForge H264 Encoder v10");
                        if (addH264 <0) { LogError("AddFilter(H264 Encoder v10)", addH264, true); return; }
                        Log("H264 Encoder v10 filter added.");
                    }
                    else
                    {
                        Log("H264 Encoder v10 CLSID not found; cannot create encoder.");
                        return;
                    }

                    // Connect graph: group output -> RGB2YUV -> H264 -> MP4 muxer
                    IPin groupOutPin;
                    hr = renderEngine.GetGroupOutputPin(0, out groupOutPin);
                    if (hr <0 || groupOutPin == null)
                    {
                        LogError("GetGroupOutputPin", hr, true);
                        return;
                    }

                    // Try connect to RGB2YUV first if present, else directly to encoder
                    IPin rgb2yuvIn = rgb2yuv != null ? FindFirstPin(rgb2yuv, PinDirection.Input) : null;
                    if (rgb2yuvIn != null)
                    {
                        int conn1 = graphBuilder.Connect(groupOutPin, rgb2yuvIn);
                        Marshal.ReleaseComObject(rgb2yuvIn);
                        if (conn1 <0)
                        {
                            Marshal.ReleaseComObject(groupOutPin);
                            LogError("Connect(groupOut -> RGB2YUV)", conn1, true);
                            return;
                        }
                        Log("Connected group output to RGB2YUV.");
                    }
                    else
                    {
                        Log("RGB2YUV filter not available; attempting direct connect to encoder.");
                    }

                    // Determine upstream pin for encoder input
                    IPin upstreamOut = null;
                    if (rgb2yuv != null)
                    {
                        upstreamOut = FindFirstPin(rgb2yuv, PinDirection.Output);
                    }
                    else
                    {
                        upstreamOut = groupOutPin;
                    }

                    // Connect upstream to H264 encoder input
                    IPin h264In = FindFirstPin(h264Enc, PinDirection.Input);
                    if (h264In == null)
                    {
                        if (!ReferenceEquals(upstreamOut, groupOutPin)) Marshal.ReleaseComObject(upstreamOut);
                        Marshal.ReleaseComObject(groupOutPin);
                        Log("Failed to find H264 encoder input pin.");
                        return;
                    }

                    int conn2 = -1;
                    // Prefer NV12/YV12/IYUV/YUY2 into H264 v10 using ConnectDirect if available
                    var graph2 = graphBuilder as IFilterGraph2;
                    if (graph2 != null)
                    {
                        Guid[] prefSubTypes = new[] { MediaSubType.NV12, MediaSubType.YV12, MediaSubType.IYUV, MediaSubType.YUY2 };
                        foreach (var st in prefSubTypes)
                        {
                            AMMediaType mtPref = new AMMediaType
                            {
                                majorType = MediaType.Video,
                                subType = st,
                                formatType = FormatType.VideoInfo
                            };
                            conn2 = graph2.ConnectDirect(upstreamOut, h264In, mtPref);
                            DsUtils.FreeAMMediaType(mtPref);
                            if (conn2 ==0)
                            {
                                break;
                            }
                        }
                    }

                    if (conn2 !=0)
                    {
                        // Fallback to graph auto-connect
                        conn2 = graphBuilder.Connect(upstreamOut, h264In);
                    }

                    Marshal.ReleaseComObject(h264In);
                    if (!ReferenceEquals(upstreamOut, groupOutPin)) Marshal.ReleaseComObject(upstreamOut);
                    Marshal.ReleaseComObject(groupOutPin);
                    if (conn2 <0)
                    {
                        LogError("Connect(upstream -> H264)", conn2, true);
                        return;
                    }
                    Log("Connected RGB2YUV/Group output to H264 encoder input.");

                    // Connect H264 encoder output to muxer input
                    IPin h264Out = FindFirstPin(h264Enc, PinDirection.Output);
                    IPin muxerIn = FindFirstPin(mp4Muxer, PinDirection.Input);
                    if (h264Out == null || muxerIn == null)
                    {
                        if (h264Out != null) Marshal.ReleaseComObject(h264Out);
                        if (muxerIn != null) Marshal.ReleaseComObject(muxerIn);
                        Log("Failed to find pins for H264->Muxer connection.");
                        return;
                    }

                    int conn3 = graphBuilder.Connect(h264Out, muxerIn);
                    Marshal.ReleaseComObject(h264Out);
                    Marshal.ReleaseComObject(muxerIn);

                    if (conn3 <0)
                    {
                        LogError("Connect(H264 -> MP4 muxer)", conn3, true);
                        return;
                    }
                    Log("Connected H264 encoder output to MP4 muxer input.");

                    // If we have audio items, insert AAC encoder and connect group1 -> AAC -> MP4 muxer
                    if (hasAudioItems)
                    {
                        try
                        {
                            Guid aacCLSID = new Guid("763CAC70-373C-4892-898B-AC80661B15F3"); // VFAACEncoderV10
                            IBaseFilter aacEnc = null;
                            Type aacType = Type.GetTypeFromCLSID(aacCLSID);
                            if (aacType != null)
                            {
                                aacEnc = (IBaseFilter)Activator.CreateInstance(aacType);
                                int addAac = graphBuilder.AddFilter(aacEnc, "VisioForge AAC Encoder v10");
                                if (addAac <0) { LogError("AddFilter(AAC Encoder v10)", addAac, true); return; }
                                Log("AAC Encoder v10 filter added.");

                                // Get group1 output pin (audio group)
                                IPin audioGroupOut;
                                int gha = renderEngine.GetGroupOutputPin(1, out audioGroupOut);
                                if (gha <0 || audioGroupOut == null)
                                {
                                    LogError("GetGroupOutputPin(audio)", gha, true);
                                    return;
                                }

                                // Connect audio group -> AAC
                                IPin aacIn = FindFirstPin(aacEnc, PinDirection.Input);
                                if (aacIn == null)
                                {
                                    Marshal.ReleaseComObject(audioGroupOut);
                                    Log("Failed to find AAC encoder input pin.");
                                    return;
                                }
                                int ca1 = graphBuilder.Connect(audioGroupOut, aacIn);
                                Marshal.ReleaseComObject(aacIn);
                                if (ca1 <0)
                                {
                                    Marshal.ReleaseComObject(audioGroupOut);
                                    LogError("Connect(audio group -> AAC)", ca1, true);
                                    return;
                                }
                                Log("Connected audio group output to AAC encoder input.");

                                // Connect AAC -> MP4 muxer
                                IPin aacOut = FindFirstPin(aacEnc, PinDirection.Output);
                                IPin mp4AudioIn = FindUnconnectedPin(mp4Muxer, PinDirection.Input);
                                if (aacOut == null || mp4AudioIn == null)
                                {
                                    if (aacOut != null) Marshal.ReleaseComObject(aacOut);
                                    if (mp4AudioIn != null) Marshal.ReleaseComObject(mp4AudioIn);
                                    Marshal.ReleaseComObject(audioGroupOut);
                                    Log("Failed to find pins for AAC->Muxer connection.");
                                    return;
                                }
                                int ca2 = graphBuilder.Connect(aacOut, mp4AudioIn);
                                Marshal.ReleaseComObject(aacOut);
                                Marshal.ReleaseComObject(mp4AudioIn);
                                Marshal.ReleaseComObject(audioGroupOut);
                                if (ca2 <0)
                                {
                                    LogError("Connect(AAC -> MP4 muxer)", ca2, true);
                                    return;
                                }
                                Log("Connected AAC encoder output to MP4 muxer input.");
                            }
                            else
                            {
                                Log("AAC Encoder v10 CLSID not found; cannot create encoder.");
                            }
                        }
                        catch (Exception aex)
                        {
                            Log($"Audio path setup error: {aex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"Filter graph setup error: {ex.Message}");
                    return;
                }

                // Optionally finalize graph
                int commitHr = renderEngine.Commit();
                if (commitHr <0)
                {
                    LogError("RenderEngine.Commit", commitHr, false);
                }

                // Get media control and event interfaces
                mediaControl = (IMediaControl)graphBuilder;
                mediaEvent = (IMediaEventEx)graphBuilder;
                mediaSeeking = graphBuilder as IMediaSeeking;

                Log("Starting conversion...");

                // Start the graph
                int runHr = mediaControl.Run();
                if (runHr <0)
                {
                    LogError("IMediaControl.Run", runHr, true);
                    return;
                }

                Log("Graph running.");
                runStartUtc = DateTime.UtcNow;
                labelStatus.Text = "Conversion in progress...";

                // Start a timer to monitor progress (keep a field reference)
                progressTimer = new System.Windows.Forms.Timer();
                progressTimer.Interval =500;
                progressTimer.Tick += (s, args) => UpdateProgress();
                progressTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting conversion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                labelStatus.Text = $"Error: {ex.Message}";
                Log($"Exception: {ex}");
                CleanupDirectShow();
            }
        }

        private async void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (mediaControl != null)
                {
                    mediaControl.Stop();
                    Log("Graph stopped by user.");
                }
                CleanupDirectShow();

                btnStart.Enabled = true;
                btnStop.Enabled = false;
                labelStatus.Text = "Conversion stopped by user.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping conversion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"Exception on stop: {ex}");
            }
        }

        private void UpdateProgress()
        {
            try
            {
                if (mediaEvent != null)
                {
                    EventCode eventCode;
                    IntPtr param1, param2;

                    while (mediaEvent.GetEvent(out eventCode, out param1, out param2,0) ==0)
                    {
                        mediaEvent.FreeEventParams(eventCode, param1, param2);

                        if (eventCode == EventCode.Complete)
                        {
                            // Conversion completed
                            btnStart.Enabled = true;
                            btnStop.Enabled = false;
                            progressBar.Value =100;
                            labelStatus.Text = "Conversion completed successfully!";
                            Log("EC_COMPLETE received. Conversion finished.");
                            MessageBox.Show("Conversion completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CleanupDirectShow();
                            return;
                        }
                        else if (eventCode == EventCode.ErrorAbort || eventCode == EventCode.UserAbort)
                        {
                            // Error occurred
                            btnStart.Enabled = true;
                            btnStop.Enabled = false;
                            labelStatus.Text = "Conversion failed or aborted.";
                            Log($"Graph event: {eventCode}");
                            MessageBox.Show("Conversion failed or was aborted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            CleanupDirectShow();
                            return;
                        }
                    }
                }

                // Update progress (approximate)
                long currentPos =0;
                long duration =0;

                if (mediaSeeking != null)
                {
                    if (mediaSeeking.GetDuration(out duration) ==0 && mediaSeeking.GetCurrentPosition(out currentPos) ==0)
                    {
                        if (duration >0)
                        {
                            int progress = (int)((currentPos *100) / duration);
                            progressBar.Value = Math.Min(Math.Max(progress,0),100);
                            labelStatus.Text = $"Progress: {progress}%";
                        }
                    }
                    else if (totalDuration100ns >0)
                    {
                        // Fallback using wall clock if seeking not available yet
                        var elapsed = (DateTime.UtcNow - runStartUtc).Ticks;
                        int progress = (int)Math.Min((elapsed *100) / totalDuration100ns,100);
                        progressBar.Value = Math.Min(Math.Max(progress,0),100);
                        labelStatus.Text = $"Progress: {progress}%";
                    }
                }
                else if (totalDuration100ns >0)
                {
                    // Fallback using wall clock if seeking unsupported
                    var elapsed = (DateTime.UtcNow - runStartUtc).Ticks;
                    int progress = (int)Math.Min((elapsed *100) / totalDuration100ns,100);
                    progressBar.Value = Math.Min(Math.Max(progress,0),100);
                    labelStatus.Text = $"Progress: {progress}%";
                }
            }
            catch (Exception ex)
            {
                // Ignore errors in progress update, but log once
                Log($"UpdateProgress error: {ex.Message}");
            }
        }

        private void CleanupDirectShow()
        {
            try
            {
                // Stop and dispose progress timer
                if (progressTimer != null)
                {
                    try { progressTimer.Stop(); } catch { }
                    progressTimer.Tick -= (s, e) => UpdateProgress();
                    progressTimer.Dispose();
                    progressTimer = null;
                }

                if (mediaControl != null)
                {
                    try { mediaControl.Stop(); } catch { }
                }

                if (mediaEvent != null)
                {
                    Marshal.ReleaseComObject(mediaEvent);
                    mediaEvent = null;
                }

                if (mediaControl != null)
                {
                    Marshal.ReleaseComObject(mediaControl);
                    mediaControl = null;
                }

                if (fileSink != null)
                {
                    Marshal.ReleaseComObject(fileSink);
                    fileSink = null;
                }

                if (graphBuilder != null)
                {
                    Marshal.ReleaseComObject(graphBuilder);
                    graphBuilder = null;
                }

                if (renderEngine != null)
                {
                    Marshal.ReleaseComObject(renderEngine);
                    renderEngine = null;
                }

                if (timeline != null)
                {
                    Marshal.ReleaseComObject(timeline);
                    timeline = null;
                }
            }
            catch (Exception ex)
            {
                Log($"Cleanup error: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CleanupDirectShow();
            }

            base.Dispose(disposing);
        }

        private void LogError(string operation, int hr, bool showMessageBox)
        {
            string text = DESError.GetErrorText(hr);
            string msg = $"{operation} failed: 0x{hr:X8} - {text}";
            Log(msg);
            if (showMessageBox)
            {
                MessageBox.Show(msg, "DirectShow Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static IPin FindFirstPin(IBaseFilter filter, PinDirection dir)
        {
            IEnumPins enumPins;
            int hr = filter.EnumPins(out enumPins);
            if (hr !=0 || enumPins == null)
            {
                return null;
            }

            try
            {
                IPin[] pins = new IPin[1];
                while (enumPins.Next(1, pins, IntPtr.Zero) ==0)
                {
                    pins[0].QueryDirection(out PinDirection pd);
                    if (pd == dir)
                    {
                        return pins[0]; // caller must Release
                    }
                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(enumPins);
            }
            return null;
        }

        private static IPin FindUnconnectedPin(IBaseFilter filter, PinDirection dir)
        {
            IEnumPins enumPins;
            if (filter.EnumPins(out enumPins) !=0 || enumPins == null) return null;
            try
            {
                IPin[] pins = new IPin[1];
                while (enumPins.Next(1, pins, IntPtr.Zero) ==0)
                {
                    pins[0].QueryDirection(out PinDirection pd);
                    if (pd == dir)
                    {
                        // Check connection state
                        pins[0].ConnectedTo(out IPin other);
                        if (other == null)
                        {
                            return pins[0]; // caller must Release
                        }
                        Marshal.ReleaseComObject(other);
                    }
                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(enumPins);
            }
            return null;
        }
    }
}
