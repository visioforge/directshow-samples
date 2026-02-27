// MainForm.cs
// C# WinForms Sample Application for ScreenCaptureFilterDD
//
// Demonstrates how to use the VisioForge Screen Capture DD DirectShow filter
// in a C# WinForms application with video preview.
//
// Prerequisites:
//   - Register the filter: regsvr32 VisioForge_Screen_Capture_DD_x64.ax
//   - .NET Framework 4.7.2+
//
// NuGet Packages:
//   - VisioForge.DirectShowAPI

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IStream = System.Runtime.InteropServices.ComTypes.IStream;

namespace ScreenCaptureSample
{
    public class MainForm : Form
    {
        // Controls
        private ComboBox cmbMode;
        private TextBox txtFPS;
        private TextBox txtLeft, txtTop, txtRight, txtBottom;
        private CheckBox chkMouse;
        private Button btnStart, btnStop, btnRefreshWindows;
        private ComboBox cmbWindow;
        private Label lblWindow;
        private Panel pnlVideo;
        private TextBox txtLog;
        private readonly List<IntPtr> windowHandles = new List<IntPtr>();

        // DirectShow objects
        private IFilterGraph2 filterGraph;
        private IMediaControl mediaControl;
        private IVideoWindow videoWindow;
        private IBaseFilter screenCaptureFilter;
        private bool isRunning;

        [DllImport("ole32.dll")]
        private static extern int CreateStreamOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, out IStream ppstm);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "ScreenCaptureFilterDD - C# WinForms Sample";
            Width = 750;
            Height = 560;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Settings group
            var grpSettings = new GroupBox { Text = "Capture Settings", Left = 10, Top = 10, Width = 240, Height = 260 };
            Controls.Add(grpSettings);

            grpSettings.Controls.Add(new Label { Text = "Mode:", Left = 10, Top = 25, Width = 40 });
            cmbMode = new ComboBox { Left = 55, Top = 22, Width = 170, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMode.Items.AddRange(new[] { "Screen (DXGI)", "Picture", "Color", "Window" });
            cmbMode.SelectedIndex = 0;
            cmbMode.SelectedIndexChanged += CmbMode_SelectedIndexChanged;
            grpSettings.Controls.Add(cmbMode);

            grpSettings.Controls.Add(new Label { Text = "FPS:", Left = 10, Top = 55, Width = 40 });
            txtFPS = new TextBox { Left = 55, Top = 52, Width = 50, Text = "15" };
            grpSettings.Controls.Add(txtFPS);

            grpSettings.Controls.Add(new Label { Text = "Rect:", Left = 10, Top = 85, Width = 40 });
            grpSettings.Controls.Add(new Label { Text = "L:", Left = 55, Top = 85, Width = 15 });
            txtLeft = new TextBox { Left = 70, Top = 82, Width = 45, Text = "0" };
            grpSettings.Controls.Add(txtLeft);
            grpSettings.Controls.Add(new Label { Text = "T:", Left = 125, Top = 85, Width = 15 });
            txtTop = new TextBox { Left = 140, Top = 82, Width = 45, Text = "0" };
            grpSettings.Controls.Add(txtTop);
            grpSettings.Controls.Add(new Label { Text = "R:", Left = 55, Top = 112, Width = 15 });
            txtRight = new TextBox { Left = 70, Top = 109, Width = 45, Text = "1920" };
            grpSettings.Controls.Add(txtRight);
            grpSettings.Controls.Add(new Label { Text = "B:", Left = 125, Top = 112, Width = 15 });
            txtBottom = new TextBox { Left = 140, Top = 109, Width = 45, Text = "1080" };
            grpSettings.Controls.Add(txtBottom);

            chkMouse = new CheckBox { Text = "Draw mouse cursor", Left = 10, Top = 140, Width = 150, Checked = true };
            grpSettings.Controls.Add(chkMouse);

            lblWindow = new Label { Text = "Window:", Left = 10, Top = 168, Width = 50, Visible = false };
            grpSettings.Controls.Add(lblWindow);
            cmbWindow = new ComboBox { Left = 10, Top = 186, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
            grpSettings.Controls.Add(cmbWindow);
            btnRefreshWindows = new Button { Text = "\u21BB", Left = 195, Top = 186, Width = 30, Height = 22, Visible = false };
            btnRefreshWindows.Click += (s, ev) => RefreshWindowList();
            grpSettings.Controls.Add(btnRefreshWindows);

            btnStart = new Button { Text = "Start", Left = 10, Top = 218, Width = 100, Height = 30 };
            btnStart.Click += BtnStart_Click;
            grpSettings.Controls.Add(btnStart);

            btnStop = new Button { Text = "Stop", Left = 120, Top = 218, Width = 100, Height = 30, Enabled = false };
            btnStop.Click += BtnStop_Click;
            grpSettings.Controls.Add(btnStop);

            // Video preview
            var grpVideo = new GroupBox { Text = "Video Preview", Left = 260, Top = 10, Width = 470, Height = 310 };
            Controls.Add(grpVideo);
            pnlVideo = new Panel { Left = 6, Top = 18, Width = 458, Height = 286, BackColor = System.Drawing.Color.Black };
            grpVideo.Controls.Add(pnlVideo);

            // Log
            var grpLog = new GroupBox { Text = "Log", Left = 10, Top = 335, Width = 720, Height = 180 };
            Controls.Add(grpLog);
            txtLog = new TextBox
            {
                Left = 6, Top = 18, Width = 708, Height = 155,
                Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true
            };
            grpLog.Controls.Add(txtLog);
        }

        private void Log(string msg)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Log("Already running.");
                return;
            }

            try
            {
                BuildGraph();

                int hr = mediaControl.Run();
                if (hr >= 0)
                {
                    isRunning = true;
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    Log("Capture started.");
                }
                else
                {
                    Log($"ERROR: Run failed (0x{hr:X8}).");
                    TearDownGraph();
                }
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
                TearDownGraph();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                Log("Not running.");
                return;
            }

            TearDownGraph();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            Log("Capture stopped.");
        }

        private void CmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isWindowMode = cmbMode.SelectedIndex == 3;
            lblWindow.Visible = isWindowMode;
            cmbWindow.Visible = isWindowMode;
            btnRefreshWindows.Visible = isWindowMode;
            if (isWindowMode && cmbWindow.Items.Count == 0)
            {
                RefreshWindowList();
            }
        }

        private void RefreshWindowList()
        {
            cmbWindow.Items.Clear();
            windowHandles.Clear();

            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                if (hWnd == Handle) return true;

                var sb = new StringBuilder(length + 1);
                GetWindowText(hWnd, sb, sb.Capacity);

                windowHandles.Add(hWnd);
                cmbWindow.Items.Add(sb.ToString());
                return true;
            }, IntPtr.Zero);

            if (cmbWindow.Items.Count > 0)
                cmbWindow.SelectedIndex = 0;

            Log($"Found {windowHandles.Count} windows.");
        }

        private void BuildGraph()
        {
            // Create filter graph
            filterGraph = (IFilterGraph2)new FilterGraph();
            mediaControl = (IMediaControl)filterGraph;
            videoWindow = (IVideoWindow)filterGraph;

            // Create and add screen capture filter
            screenCaptureFilter = DSHelper.AddFilterFromClsid(
                (IGraphBuilder)filterGraph, Consts.CLSID_VFScreenCaptureDD, "Screen Capture DD");
            if (screenCaptureFilter == null)
                throw new Exception("ScreenCaptureFilterDD not registered. Run: regsvr32 VisioForge_Screen_Capture_DD_x64.ax");

            // Configure via IVFScreenCapture3 (aggregated interface)
            var capture3 = screenCaptureFilter as IVFScreenCapture3;
            if (capture3 != null)
            {
                capture3.init();

                double fps = double.TryParse(txtFPS.Text, out double f) ? f : 15.0;
                fps = Math.Max(0.4, Math.Min(30.0, fps));
                capture3.set_fps(fps);
                Log($"FPS: {fps:F1}");

                var rect = new VFRect
                {
                    Left = uint.TryParse(txtLeft.Text, out uint l) ? l : 0,
                    Top = uint.TryParse(txtTop.Text, out uint t) ? t : 0,
                    Right = uint.TryParse(txtRight.Text, out uint r) ? r : 1920,
                    Bottom = uint.TryParse(txtBottom.Text, out uint b) ? b : 1080
                };
                capture3.set_rect(rect);
                Log($"Rect: ({rect.Left},{rect.Top})-({rect.Right},{rect.Bottom})");

                capture3.set_mouse(chkMouse.Checked);
                Log($"Mouse cursor: {(chkMouse.Checked ? "enabled" : "disabled")}");

                capture3.set_display_index(0);

                capture3.set_mode((VFScreenCaptureMode)cmbMode.SelectedIndex);
                Log($"Capture mode: {cmbMode.SelectedItem}");

                // Picture mode: load image and set stream
                if (cmbMode.SelectedIndex == 1)
                {
                    LoadPictureStream(capture3, rect);
                }

                // Window mode: set target window handle and adjust rect
                if (cmbMode.SelectedIndex == 3 && cmbWindow.SelectedIndex >= 0
                    && cmbWindow.SelectedIndex < windowHandles.Count)
                {
                    IntPtr hwnd = windowHandles[cmbWindow.SelectedIndex];
                    capture3.get_window_size(hwnd, out int ww, out int wh);
                    ww -= ww % 4;
                    wh -= wh % 4;
                    rect.Left = 0;
                    rect.Top = 0;
                    rect.Right = (uint)ww;
                    rect.Bottom = (uint)wh;
                    capture3.set_rect(rect);
                    capture3.set_window_handle(hwnd);
                    Log($"Window: \"{cmbWindow.SelectedItem}\", handle: 0x{hwnd.ToInt64():X}, size: {ww}x{wh}");
                }
            }

            // Check DXGI availability
            var captureDD = screenCaptureFilter as IVFScreenCaptureDD;
            if (captureDD != null)
            {
                int hr = captureDD.dd_check(0);
                Log(hr == 0 ? "DXGI Desktop Duplication: available" : "DXGI Desktop Duplication: not available");
            }

            // Add video renderer and connect
            var videoRenderer = FilterGraphTools.AddFilterFromClsid(
                (IGraphBuilder)filterGraph,
                new Guid("70E102B0-5556-11CE-97C0-00AA0055595A"), // CLSID_VideoRenderer
                "Video Renderer");

            // Connect pins
            IPin outPin = GetFirstPin(screenCaptureFilter);
            IPin inPin = GetFirstPin(videoRenderer);

            try
            {
                if (outPin != null && inPin != null)
                {
                    int hr = ((IGraphBuilder)filterGraph).Connect(outPin, inPin);
                    DsError.ThrowExceptionForHR(hr);
                }
            }
            finally
            {
                if (outPin != null) Marshal.ReleaseComObject(outPin);
                if (inPin != null) Marshal.ReleaseComObject(inPin);
            }

            // Set up video window
            videoWindow.put_Owner(pnlVideo.Handle);
            videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings);
            videoWindow.SetWindowPosition(0, 0, pnlVideo.Width, pnlVideo.Height);
            videoWindow.put_Visible(OABool.True);

            Log("Graph built successfully.");
        }

        private void LoadPictureStream(IVFScreenCapture3 capture3, VFRect rect)
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.jpg");
            if (!File.Exists(imagePath))
            {
                Log("WARNING: image.jpg not found for Picture mode.");
                return;
            }

            int width = (int)(rect.Right - rect.Left);
            int height = (int)(rect.Bottom - rect.Top);

            using (var srcBitmap = new Bitmap(imagePath))
            using (var targetBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (var g = Graphics.FromImage(targetBitmap))
                {
                    g.DrawImage(srcBitmap, 0, 0, width, height);
                }

                var lockRect = new Rectangle(0, 0, width, height);
                var bitmapData = targetBitmap.LockBits(lockRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                try
                {
                    int bufSize = width * height * 4;
                    byte[] pixelData = new byte[bufSize];

                    if (bitmapData.Stride == width * 4)
                    {
                        Marshal.Copy(bitmapData.Scan0, pixelData, 0, bufSize);
                    }
                    else
                    {
                        for (int y = 0; y < height; y++)
                        {
                            Marshal.Copy(IntPtr.Add(bitmapData.Scan0, y * bitmapData.Stride),
                                pixelData, y * width * 4, width * 4);
                        }
                    }

                    IntPtr hGlobal = Marshal.AllocHGlobal(bufSize);
                    Marshal.Copy(pixelData, 0, hGlobal, bufSize);

                    int hr = CreateStreamOnHGlobal(hGlobal, true, out IStream stream);
                    if (hr == 0)
                    {
                        capture3.set_stream(stream, bufSize);
                        capture3.refresh_pic();
                        Log($"Picture loaded: {width}x{height}, {bufSize} bytes");
                    }
                    else
                    {
                        Marshal.FreeHGlobal(hGlobal);
                        Log($"ERROR: CreateStreamOnHGlobal failed (0x{hr:X8}).");
                    }
                }
                finally
                {
                    targetBitmap.UnlockBits(bitmapData);
                }
            }
        }

        private IPin GetFirstPin(IBaseFilter filter)
        {
            filter.EnumPins(out IEnumPins enumPins);
            if (enumPins == null) return null;

            IPin[] pins = new IPin[1];
            enumPins.Next(1, pins, IntPtr.Zero);
            Marshal.ReleaseComObject(enumPins);

            return pins[0];
        }

        private void TearDownGraph()
        {
            mediaControl?.Stop();

            if (videoWindow != null)
            {
                videoWindow.put_Visible(OABool.False);
                videoWindow.put_Owner(IntPtr.Zero);
            }

            if (filterGraph != null)
            {
                FilterGraphTools.RemoveAllFilters(filterGraph);
                Marshal.ReleaseComObject(filterGraph);
                filterGraph = null;
            }

            screenCaptureFilter = null;
            videoWindow = null;
            mediaControl = null;
            isRunning = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            TearDownGraph();
            base.OnFormClosing(e);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
