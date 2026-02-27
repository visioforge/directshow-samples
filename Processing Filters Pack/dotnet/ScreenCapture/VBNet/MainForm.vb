' MainForm.vb
' VB.NET WinForms Sample Application for ScreenCaptureFilterDD
'
' Demonstrates how to use the VisioForge Screen Capture DD DirectShow filter
' in a VB.NET WinForms application with video preview.
'
' Prerequisites:
'   - Register the filter: regsvr32 VisioForge_Screen_Capture_DD_x64.ax
'   - .NET Framework 4.7.2+
'
' NuGet Packages:
'   - VisioForge.DirectShowAPI

Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports VisioForge.DirectShowAPI
Imports VisioForge.DirectShowLib
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports IStream = System.Runtime.InteropServices.ComTypes.IStream

Namespace ScreenCaptureSample

    Public Class MainForm
        Inherits Form

        ' Controls
        Private WithEvents cmbMode As ComboBox
        Private txtFPS As TextBox
        Private txtLeft As TextBox
        Private txtTop As TextBox
        Private txtRight As TextBox
        Private txtBottom As TextBox
        Private chkMouse As CheckBox
        Private WithEvents btnStart As Button
        Private WithEvents btnStop As Button
        Private WithEvents btnRefreshWindows As Button
        Private cmbWindow As ComboBox
        Private lblWindow As Label
        Private pnlVideo As Panel
        Private txtLog As TextBox
        Private ReadOnly windowHandles As New List(Of IntPtr)

        ' DirectShow objects
        Private filterGraph As IFilterGraph2
        Private mediaControl As IMediaControl
        Private videoWindow As IVideoWindow
        Private screenCaptureFilter As IBaseFilter
        Private isRunning As Boolean

        <DllImport("ole32.dll")>
        Private Shared Function CreateStreamOnHGlobal(hGlobal As IntPtr, fDeleteOnRelease As Boolean, <Out> ByRef ppstm As IStream) As Integer
        End Function

        Private Delegate Function EnumWindowsProc(hWnd As IntPtr, lParam As IntPtr) As Boolean

        <DllImport("user32.dll")>
        Private Shared Function EnumWindows(lpEnumFunc As EnumWindowsProc, lParam As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll")>
        Private Shared Function IsWindowVisible(hWnd As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Private Shared Function GetWindowText(hWnd As IntPtr, lpString As StringBuilder, nMaxCount As Integer) As Integer
        End Function

        <DllImport("user32.dll")>
        Private Shared Function GetWindowTextLength(hWnd As IntPtr) As Integer
        End Function

        Public Sub New()
            InitializeComponents()
        End Sub

        Private Sub InitializeComponents()
            Text = "ScreenCaptureFilterDD - VB.NET Sample"
            Width = 750
            Height = 560
            FormBorderStyle = FormBorderStyle.FixedSingle
            MaximizeBox = False

            ' Settings group
            Dim grpSettings As New GroupBox With {.Text = "Capture Settings", .Left = 10, .Top = 10, .Width = 240, .Height = 260}
            Controls.Add(grpSettings)

            grpSettings.Controls.Add(New Label With {.Text = "Mode:", .Left = 10, .Top = 25, .Width = 40})
            cmbMode = New ComboBox With {.Left = 55, .Top = 22, .Width = 170, .DropDownStyle = ComboBoxStyle.DropDownList}
            cmbMode.Items.AddRange({"Screen (DXGI)", "Picture", "Color", "Window"})
            cmbMode.SelectedIndex = 0
            AddHandler cmbMode.SelectedIndexChanged, AddressOf CmbMode_SelectedIndexChanged
            grpSettings.Controls.Add(cmbMode)

            grpSettings.Controls.Add(New Label With {.Text = "FPS:", .Left = 10, .Top = 55, .Width = 40})
            txtFPS = New TextBox With {.Left = 55, .Top = 52, .Width = 50, .Text = "15"}
            grpSettings.Controls.Add(txtFPS)

            grpSettings.Controls.Add(New Label With {.Text = "Rect:", .Left = 10, .Top = 85, .Width = 40})
            grpSettings.Controls.Add(New Label With {.Text = "L:", .Left = 55, .Top = 85, .Width = 15})
            txtLeft = New TextBox With {.Left = 70, .Top = 82, .Width = 45, .Text = "0"}
            grpSettings.Controls.Add(txtLeft)
            grpSettings.Controls.Add(New Label With {.Text = "T:", .Left = 125, .Top = 85, .Width = 15})
            txtTop = New TextBox With {.Left = 140, .Top = 82, .Width = 45, .Text = "0"}
            grpSettings.Controls.Add(txtTop)
            grpSettings.Controls.Add(New Label With {.Text = "R:", .Left = 55, .Top = 112, .Width = 15})
            txtRight = New TextBox With {.Left = 70, .Top = 109, .Width = 45, .Text = "1920"}
            grpSettings.Controls.Add(txtRight)
            grpSettings.Controls.Add(New Label With {.Text = "B:", .Left = 125, .Top = 112, .Width = 15})
            txtBottom = New TextBox With {.Left = 140, .Top = 109, .Width = 45, .Text = "1080"}
            grpSettings.Controls.Add(txtBottom)

            chkMouse = New CheckBox With {.Text = "Draw mouse cursor", .Left = 10, .Top = 140, .Width = 150, .Checked = True}
            grpSettings.Controls.Add(chkMouse)

            lblWindow = New Label With {.Text = "Window:", .Left = 10, .Top = 168, .Width = 50, .Visible = False}
            grpSettings.Controls.Add(lblWindow)
            cmbWindow = New ComboBox With {.Left = 10, .Top = 186, .Width = 180, .DropDownStyle = ComboBoxStyle.DropDownList, .Visible = False}
            grpSettings.Controls.Add(cmbWindow)
            btnRefreshWindows = New Button With {.Text = ChrW(&H21BB), .Left = 195, .Top = 186, .Width = 30, .Height = 22, .Visible = False}
            AddHandler btnRefreshWindows.Click, Sub(s, ev) RefreshWindowList()
            grpSettings.Controls.Add(btnRefreshWindows)

            btnStart = New Button With {.Text = "Start", .Left = 10, .Top = 218, .Width = 100, .Height = 30}
            grpSettings.Controls.Add(btnStart)

            btnStop = New Button With {.Text = "Stop", .Left = 120, .Top = 218, .Width = 100, .Height = 30, .Enabled = False}
            grpSettings.Controls.Add(btnStop)

            ' Video preview
            Dim grpVideo As New GroupBox With {.Text = "Video Preview", .Left = 260, .Top = 10, .Width = 470, .Height = 310}
            Controls.Add(grpVideo)
            pnlVideo = New Panel With {.Left = 6, .Top = 18, .Width = 458, .Height = 286, .BackColor = Drawing.Color.Black}
            grpVideo.Controls.Add(pnlVideo)

            ' Log
            Dim grpLog As New GroupBox With {.Text = "Log", .Left = 10, .Top = 335, .Width = 720, .Height = 180}
            Controls.Add(grpLog)
            txtLog = New TextBox With {
                .Left = 6, .Top = 18, .Width = 708, .Height = 155,
                .Multiline = True, .ScrollBars = ScrollBars.Vertical, .ReadOnly = True
            }
            grpLog.Controls.Add(txtLog)
        End Sub

        Private Sub Log(msg As String)
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}" & vbCrLf)
        End Sub

        Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
            If isRunning Then
                Log("Already running.")
                Return
            End If

            Try
                BuildGraph()

                Dim hr As Integer = mediaControl.Run()
                If hr >= 0 Then
                    isRunning = True
                    btnStart.Enabled = False
                    btnStop.Enabled = True
                    Log("Capture started.")
                Else
                    Log($"ERROR: Run failed (0x{hr:X8}).")
                    TearDownGraph()
                End If
            Catch ex As Exception
                Log($"ERROR: {ex.Message}")
                TearDownGraph()
            End Try
        End Sub

        Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
            If Not isRunning Then
                Log("Not running.")
                Return
            End If

            TearDownGraph()
            btnStart.Enabled = True
            btnStop.Enabled = False
            Log("Capture stopped.")
        End Sub

        Private Sub CmbMode_SelectedIndexChanged(sender As Object, e As EventArgs)
            Dim isWindowMode As Boolean = cmbMode.SelectedIndex = 3
            lblWindow.Visible = isWindowMode
            cmbWindow.Visible = isWindowMode
            btnRefreshWindows.Visible = isWindowMode
            If isWindowMode AndAlso cmbWindow.Items.Count = 0 Then
                RefreshWindowList()
            End If
        End Sub

        Private Sub RefreshWindowList()
            cmbWindow.Items.Clear()
            windowHandles.Clear()

            EnumWindows(Function(hWnd, lParam)
                            If Not IsWindowVisible(hWnd) Then Return True

                            Dim length As Integer = GetWindowTextLength(hWnd)
                            If length = 0 Then Return True

                            If hWnd = Handle Then Return True

                            Dim sb As New StringBuilder(length + 1)
                            GetWindowText(hWnd, sb, sb.Capacity)

                            windowHandles.Add(hWnd)
                            cmbWindow.Items.Add(sb.ToString())
                            Return True
                        End Function, IntPtr.Zero)

            If cmbWindow.Items.Count > 0 Then
                cmbWindow.SelectedIndex = 0
            End If

            Log($"Found {windowHandles.Count} windows.")
        End Sub

        Private Sub BuildGraph()
            ' Create filter graph
            filterGraph = DirectCast(New FilterGraph(), IFilterGraph2)
            mediaControl = DirectCast(filterGraph, IMediaControl)
            videoWindow = DirectCast(filterGraph, IVideoWindow)

            ' Create and add screen capture filter
            screenCaptureFilter = DSHelper.AddFilterFromClsid(
                DirectCast(filterGraph, IGraphBuilder), Consts.CLSID_VFScreenCaptureDD, "Screen Capture DD")
            If screenCaptureFilter Is Nothing Then
                Throw New Exception("ScreenCaptureFilterDD not registered. Run: regsvr32 VisioForge_Screen_Capture_DD_x64.ax")
            End If

            ' Configure via IVFScreenCapture3 (aggregated interface)
            Dim capture3 As IVFScreenCapture3 = TryCast(screenCaptureFilter, IVFScreenCapture3)
            If capture3 IsNot Nothing Then
                capture3.init()

                ' FPS
                Dim fps As Double
                If Not Double.TryParse(txtFPS.Text, fps) Then fps = 15.0
                fps = Math.Max(0.4, Math.Min(30.0, fps))
                capture3.set_fps(fps)
                Log($"FPS: {fps:F1}")

                ' Capture rectangle
                Dim rect As New VFRect()
                Dim tempVal As UInteger
                rect.Left = If(UInteger.TryParse(txtLeft.Text, tempVal), tempVal, 0UI)
                rect.Top = If(UInteger.TryParse(txtTop.Text, tempVal), tempVal, 0UI)
                rect.Right = If(UInteger.TryParse(txtRight.Text, tempVal), tempVal, 1920UI)
                rect.Bottom = If(UInteger.TryParse(txtBottom.Text, tempVal), tempVal, 1080UI)
                capture3.set_rect(rect)
                Log($"Rect: ({rect.Left},{rect.Top})-({rect.Right},{rect.Bottom})")

                ' Mouse cursor
                capture3.set_mouse(chkMouse.Checked)
                Log($"Mouse cursor: {If(chkMouse.Checked, "enabled", "disabled")}")

                ' Display index
                capture3.set_display_index(0)

                ' Capture mode
                capture3.set_mode(CType(cmbMode.SelectedIndex, VFScreenCaptureMode))
                Log($"Capture mode: {cmbMode.SelectedItem}")

                ' Picture mode: load image and set stream
                If cmbMode.SelectedIndex = 1 Then
                    LoadPictureStream(capture3, rect)
                End If

                ' Window mode: set target window handle and adjust rect
                If cmbMode.SelectedIndex = 3 AndAlso cmbWindow.SelectedIndex >= 0 _
                    AndAlso cmbWindow.SelectedIndex < windowHandles.Count Then
                    Dim hwnd As IntPtr = windowHandles(cmbWindow.SelectedIndex)
                    Dim ww, wh As Integer
                    capture3.get_window_size(hwnd, ww, wh)
                    ww -= ww Mod 4
                    wh -= wh Mod 4
                    rect.Left = 0
                    rect.Top = 0
                    rect.Right = CUInt(ww)
                    rect.Bottom = CUInt(wh)
                    capture3.set_rect(rect)
                    capture3.set_window_handle(hwnd)
                    Log($"Window: ""{cmbWindow.SelectedItem}"", handle: 0x{hwnd.ToInt64():X}, size: {ww}x{wh}")
                End If
            End If

            ' Check DXGI availability
            Dim captureDD As IVFScreenCaptureDD = TryCast(screenCaptureFilter, IVFScreenCaptureDD)
            If captureDD IsNot Nothing Then
                Dim hr As Integer = captureDD.dd_check(0)
                Log(If(hr = 0, "DXGI Desktop Duplication: available", "DXGI Desktop Duplication: not available"))
            End If

            ' Add video renderer and connect
            Dim videoRenderer As IBaseFilter = FilterGraphTools.AddFilterFromClsid(
                DirectCast(filterGraph, IGraphBuilder),
                New Guid("70E102B0-5556-11CE-97C0-00AA0055595A"),
                "Video Renderer")

            ' Connect pins
            Dim outPin As IPin = GetFirstPin(screenCaptureFilter)
            Dim inPin As IPin = GetFirstPin(videoRenderer)

            Try
                If outPin IsNot Nothing AndAlso inPin IsNot Nothing Then
                    Dim hr As Integer = DirectCast(filterGraph, IGraphBuilder).Connect(outPin, inPin)
                    DsError.ThrowExceptionForHR(hr)
                End If
            Finally
                If outPin IsNot Nothing Then Marshal.ReleaseComObject(outPin)
                If inPin IsNot Nothing Then Marshal.ReleaseComObject(inPin)
            End Try

            ' Set up video window
            videoWindow.put_Owner(pnlVideo.Handle)
            videoWindow.put_WindowStyle(WindowStyle.Child Or WindowStyle.ClipSiblings)
            videoWindow.SetWindowPosition(0, 0, pnlVideo.Width, pnlVideo.Height)
            videoWindow.put_Visible(OABool.True)

            Log("Graph built successfully.")
        End Sub

        Private Sub LoadPictureStream(capture3 As IVFScreenCapture3, rect As VFRect)
            Dim imagePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.jpg")
            If Not File.Exists(imagePath) Then
                Log("WARNING: image.jpg not found for Picture mode.")
                Return
            End If

            Dim width As Integer = CInt(rect.Right - rect.Left)
            Dim height As Integer = CInt(rect.Bottom - rect.Top)

            Using srcBitmap As New Bitmap(imagePath)
                Using targetBitmap As New Bitmap(width, height, PixelFormat.Format32bppArgb)
                    Using g As Graphics = Graphics.FromImage(targetBitmap)
                        g.DrawImage(srcBitmap, 0, 0, width, height)
                    End Using

                    Dim lockRect As New Rectangle(0, 0, width, height)
                    Dim bitmapData As BitmapData = targetBitmap.LockBits(lockRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)

                    Try
                        Dim bufSize As Integer = width * height * 4
                        Dim pixelData(bufSize - 1) As Byte

                        If bitmapData.Stride = width * 4 Then
                            Marshal.Copy(bitmapData.Scan0, pixelData, 0, bufSize)
                        Else
                            For y As Integer = 0 To height - 1
                                Marshal.Copy(IntPtr.Add(bitmapData.Scan0, y * bitmapData.Stride),
                                    pixelData, y * width * 4, width * 4)
                            Next
                        End If

                        Dim hGlobal As IntPtr = Marshal.AllocHGlobal(bufSize)
                        Marshal.Copy(pixelData, 0, hGlobal, bufSize)

                        Dim stream As IStream = Nothing
                        Dim hr As Integer = CreateStreamOnHGlobal(hGlobal, True, stream)
                        If hr = 0 Then
                            capture3.set_stream(stream, bufSize)
                            capture3.refresh_pic()
                            Log($"Picture loaded: {width}x{height}, {bufSize} bytes")
                        Else
                            Marshal.FreeHGlobal(hGlobal)
                            Log($"ERROR: CreateStreamOnHGlobal failed (0x{hr:X8}).")
                        End If
                    Finally
                        targetBitmap.UnlockBits(bitmapData)
                    End Try
                End Using
            End Using
        End Sub

        Private Function GetFirstPin(filter As IBaseFilter) As IPin
            Dim enumPins As IEnumPins = Nothing
            filter.EnumPins(enumPins)
            If enumPins Is Nothing Then Return Nothing

            Dim pins(0) As IPin
            enumPins.Next(1, pins, IntPtr.Zero)
            Marshal.ReleaseComObject(enumPins)

            Return pins(0)
        End Function

        Private Sub TearDownGraph()
            If mediaControl IsNot Nothing Then mediaControl.Stop()

            If videoWindow IsNot Nothing Then
                videoWindow.put_Visible(OABool.False)
                videoWindow.put_Owner(IntPtr.Zero)
            End If

            If filterGraph IsNot Nothing Then
                FilterGraphTools.RemoveAllFilters(filterGraph)
                Marshal.ReleaseComObject(filterGraph)
                filterGraph = Nothing
            End If

            screenCaptureFilter = Nothing
            videoWindow = Nothing
            mediaControl = Nothing
            isRunning = False
        End Sub

        Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
            TearDownGraph()
            MyBase.OnFormClosing(e)
        End Sub

        <STAThread>
        Public Shared Sub Main()
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New MainForm())
        End Sub

    End Class

End Namespace
