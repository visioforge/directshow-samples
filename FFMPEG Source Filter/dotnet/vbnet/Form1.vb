Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading

Imports MediaFoundation
Imports MediaFoundation.EVR
Imports MediaFoundation.Misc
Imports VisioForge.DirectShowAPI
Imports VisioForge.DirectShowLib

Namespace FFMPEG_Source_Demo
    Public Partial Class Form1
        Inherits Form

        Private _seekingFlag As Boolean

        Private Const WM_GRAPHNOTIFY As Integer = &H8000 + 1

        Private filterGraph As IFilterGraph2

        Private captureGraph As ICaptureGraphBuilder2

        Private mediaControl As IMediaControl

        Private mediaSeeking As IMediaSeeking

        Private videoWindow As IVideoWindow

        Private mediaEventEx As IMediaEventEx

        Private sourceFilter As IBaseFilter

        Private videoRenderer As IBaseFilter

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AddGraph()
            ' An exception is thrown if cast fail
            filterGraph = CType(New FilterGraph(), IFilterGraph2)
            captureGraph = CType(New CaptureGraphBuilder2(), ICaptureGraphBuilder2)
            mediaControl = CType(filterGraph, IMediaControl)
            mediaSeeking = CType(filterGraph, IMediaSeeking)
            'videoWindowSource = CType(filterGraphSource, IVideoWindow)
            mediaEventEx = CType(filterGraph, IMediaEventEx)

            Dim hr As Integer = mediaEventEx.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero)
            DsError.ThrowExceptionForHR(hr)

            ' Attach the filter graph to the capture graph
            hr = captureGraph.SetFiltergraph(filterGraph)
            DsError.ThrowExceptionForHR(hr)
        End Sub

        Private Sub AddSource()
            sourceFilter = FilterGraphTools.AddFilterFromClsid(filterGraph, Consts.CLSID_VFFFMPEGSource, "FFMPEG Source")

            ' Register purchased version
            ' Dim reg = TryCast(sourceFilter, IVFRegister)
            ' reg.SetLicenseKey("your license key")

            ' configure filter
            Dim filterConfig = TryCast(sourceFilter, IFFMPEGSourceSettings)
            If filterConfig IsNot Nothing Then
                Select Case cbBufferingMode.SelectedIndex
                    Case 0
                        filterConfig.SetBufferingMode(FFMPEG_SOURCE_BUFFERING_MODE.FFMPEG_SOURCE_BUFFERING_MODE_AUTO)
                    Case 1
                        filterConfig.SetBufferingMode(FFMPEG_SOURCE_BUFFERING_MODE.FFMPEG_SOURCE_BUFFERING_MODE_ON)
                    Case 2
                        filterConfig.SetBufferingMode(FFMPEG_SOURCE_BUFFERING_MODE.FFMPEG_SOURCE_BUFFERING_MODE_OFF)
                End Select

                filterConfig.SetHWAccelerationEnabled(cbUseGPU.Checked)
                filterConfig.SetLoadTimeOut(Convert.ToUInt32(edConnectionTimeOut.Text))
            End If

            ' load file / network stream
            Dim sourceFilterIntf = TryCast(sourceFilter, IFileSourceFilter)
            If sourceFilterIntf IsNot Nothing Then
                Dim hr As Integer = sourceFilterIntf.Load(edFilename.Text, Nothing)
                DsError.ThrowExceptionForHR(hr)
            Else
                Throw New Exception("Unable to query IFileSourceFilter interface.")
            End If
        End Sub

        Private Sub GetStreams()
            cbVideoStream.Items.Clear()
            cbAudioStream.Items.Clear()

            Dim streamSelect = TryCast(sourceFilter, IAMStreamSelect)
            If streamSelect IsNot Nothing Then
                Dim streamsCount As Integer
                streamSelect.Count(streamsCount)
                For i As Integer = 0 To streamsCount - 1
                    Dim mt As VisioForge.DirectShowLib.AMMediaType
                    Dim name As String
                    streamSelect.Info(i, mt, Nothing, Nothing, Nothing, name, Nothing, Nothing)
                    If mt.majorType = MediaType.Video Then
                        cbVideoStream.Items.Add(name)
                    ElseIf mt.majorType = MediaType.Audio Then
                        cbAudioStream.Items.Add(name)
                    End If
                Next
            End If

            If cbVideoStream.Items.Count > 0 Then
                cbVideoStream.SelectedIndex = 0
            End If

            If cbAudioStream.Items.Count > 0 Then
                cbAudioStream.SelectedIndex = 0
            End If
        End Sub

        Private Sub AddVideoRenderer()
            Dim CLSID_EVR As New Guid("FA10746C-9B63-4B6C-BC49-FC300EA5F256")
            videoRenderer = FilterGraphTools.AddFilterFromClsid(filterGraph, CLSID_EVR, "EVR")

            ' ReSharper disable once SuspiciousTypeConversion.Global
            Dim pConfig = CType(videoRenderer, IEVRFilterConfig)

            If pConfig IsNot Nothing Then
                pConfig.SetNumberOfStreams(1)
            Else
                Throw New Exception("Unable to query IEVRFilterConfig interface.")
            End If

            ' ReSharper disable once SuspiciousTypeConversion.Global
            Dim getService = CType(videoRenderer, MediaFoundation.IMFGetService)
            If getService IsNot Nothing Then
                ' FDS_pEVRFilterConfig.SetNumberOfStreams(dwNumStreams)
            Else
                Throw New Exception("Unable to query IMFGetService interface.")
            End If

            Dim videoDisplayControlObj As Object
            getService.GetService(MFServices.MR_VIDEO_RENDER_SERVICE, GetType(IMFVideoDisplayControl).GUID, videoDisplayControlObj)
            If videoDisplayControlObj IsNot Nothing Then
                Dim dsMFVideoDisplayControl = TryCast(videoDisplayControlObj, IMFVideoDisplayControl)
                If dsMFVideoDisplayControl IsNot Nothing Then
                    dsMFVideoDisplayControl.SetVideoWindow(Me.pnScreen.Handle)

                    Dim rectSrc As New MFVideoNormalizedRect(0, 0, 1, 1)
                    Dim rectDest As New MFRect(0, 0, pnScreen.Width, pnScreen.Height)

                    dsMFVideoDisplayControl.SetVideoPosition(rectSrc, rectDest)
                End If
            Else
                Throw New Exception("Unable to query IMFVideoDisplayControl interface.")
            End If

            ' Render the preview pin on the video capture filter
            Dim hr As Integer = captureGraph.RenderStream(Nothing, MediaType.Video, sourceFilter, Nothing, videoRenderer)
            DsError.ThrowExceptionForHR(hr)

            hr = captureGraph.RenderStream(Nothing, MediaType.Audio, sourceFilter, Nothing, Nothing)
            'DsError.ThrowExceptionForHR(hr)
        End Sub

        Private Sub btStart_Click(sender As Object, e As EventArgs)
            AddGraph()

            AddSource()

            GetStreams()

            AddVideoRenderer()

            'FilterGraphTools.SaveGraphFile(filterGraph, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\VisioForge\ffmpeg_source.grf")

            mediaControl.Run()

            tmProgress.Start()
        End Sub

        Private Sub btStop_Click(sender As Object, e As EventArgs)
            Try
                tmProgress.Stop()

                ' Stop previewing data
                If mediaControl IsNot Nothing Then
                    mediaControl.StopWhenReady()
                    mediaControl.Stop()
                End If

                ' Stop receiving events
                mediaEventEx?.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero)

                ' Relinquish ownership (IMPORTANT!) of the video window.
                ' Failing to call put_Owner can lead to assert failures within
                ' the video renderer, as it still assumes that it has a valid
                ' parent window.
                If videoWindow IsNot Nothing Then
                    videoWindow.put_Visible(OABool.False)
                    videoWindow.put_Owner(IntPtr.Zero)
                End If

                FilterGraphTools.RemoveAllFilters(filterGraph)

                ' Release DirectShow interfaces
                If mediaControl IsNot Nothing Then
                    Marshal.ReleaseComObject(mediaControl)
                    mediaControl = Nothing
                End If

                If mediaEventEx IsNot Nothing Then
                    Marshal.ReleaseComObject(mediaEventEx)
                    mediaEventEx = Nothing
                End If

                If videoWindow IsNot Nothing Then
                    Marshal.ReleaseComObject(videoWindow)
                    videoWindow = Nothing
                End If

                Marshal.ReleaseComObject(filterGraph)
                filterGraph = Nothing
                Marshal.ReleaseComObject(captureGraph)
                captureGraph = Nothing
                Marshal.ReleaseComObject(sourceFilter)
                sourceFilter = Nothing
                Marshal.ReleaseComObject(videoRenderer)
                videoRenderer = Nothing
            Catch
            End Try
        End Sub

        Private Function GetDuration() As TimeSpan
            If mediaSeeking IsNot Nothing Then
                Dim dur As Long
                mediaSeeking.GetDuration(dur)
                Return TimeSpan.FromMilliseconds(dur / 10000.0)
            End If

            Return TimeSpan.Zero
        End Function

        Private Function GetPosition() As TimeSpan
            Dim hr As Integer = mediaSeeking.SetTimeFormat(TimeFormat.MediaTime)
            If hr <> 0 Then
                Throw New Exception("Unable to set seeking time format to MediaTime")
            End If

            Dim pos As Long
            hr = mediaSeeking.GetCurrentPosition(pos)
            If hr <> 0 Then
                'Logger?.Error(Resources.Resources.Unable_to_get_current_position)
            End If

            Return TimeSpan.FromMilliseconds(pos / 10000.0)
        End Function

        ''' <summary>
        ''' Sets current position.
        ''' </summary>
        ''' <param name="position">
        ''' Time.
        ''' </param>
        ''' <returns>
        ''' The <see cref="bool"/>.
        ''' </returns>
        Public Function PositionSet(position As TimeSpan) As Boolean
            Dim result As Boolean

            If _seekingFlag Then
                Return True
            End If

            Try
                _seekingFlag = True

                Try
                    result = False

                    Dim pos As Long = CLng(position.TotalMilliseconds * 10000)

                    ' ReSharper disable ConvertToConstant.Local
                    Dim stopPos = 0
                    ' ReSharper restore ConvertToConstant.Local
                    If mediaSeeking IsNot Nothing Then
                        Dim hr As Integer = Me.mediaSeeking.SetTimeFormat(TimeFormat.MediaTime)
                        If hr <> 0 Then
                            Debug.WriteLine("Unable to set seeking time format to MediaTime")
                        End If

                        hr = mediaSeeking.SetPositions(pos, AMSeekingSeekingFlags.AbsolutePositioning, stopPos, AMSeekingSeekingFlags.NoPositioning)
                        If hr < 0 Then
                            Debug.WriteLine("Unable to set current position")
                        Else
                            result = True
                        End If

                        If Path.GetExtension(edFilename.Text)?.ToLowerInvariant() = ".wmv" OrElse Path.GetExtension(edFilename.Text)?.ToLowerInvariant() = ".asf" Then
                            Thread.Sleep(300)
                        End If
                    End If

                Catch
                    Return False
                End Try
            Finally
                _seekingFlag = False
            End Try

            Return result
        End Function

        Private Sub btResume_Click(sender As Object, e As EventArgs)
            mediaControl.Run()
        End Sub

        Private Sub btPause_Click(sender As Object, e As EventArgs)
            mediaControl.Pause()
        End Sub

        Private Sub Form1_Load(sender As Object, e As EventArgs)
            cbBufferingMode.SelectedIndex = 0
        End Sub

        Private Sub tmProgress_Tick(sender As Object, e As EventArgs)
            tmProgress.Tag = 1

            Dim duration = GetDuration()
            Dim position = GetPosition()
            tbTimeline.Maximum = CInt(duration.TotalSeconds)

            Dim value As Integer = CInt(position.TotalSeconds)
            If (value > 0) AndAlso (value < tbTimeline.Maximum) Then
                tbTimeline.Value = value
            End If

            lbTime.Text = position.ToString("hh\:mm\:ss") + "/" + duration.ToString("hh\:mm\:ss")

            tmProgress.Tag = 0
        End Sub

        Private Sub tbSpeed_Scroll(sender As Object, e As EventArgs)
            mediaSeeking.SetRate(tbSpeed.Value / 10.0)
        End Sub

        Private Sub tbTimeline_Scroll(sender As Object, e As EventArgs)
            If Convert.ToInt32(tmProgress.Tag) = 0 Then
                PositionSet(TimeSpan.FromSeconds(tbTimeline.Value))
            End If
        End Sub

        Private Sub btSelectFile_Click(sender As Object, e As EventArgs)
            If openFileDialog1.ShowDialog(Me) = DialogResult.OK Then
                edFilename.Text = openFileDialog1.FileName
            End If
        End Sub

        Private Sub cbVideoStream_SelectedIndexChanged(sender As Object, e As EventArgs)
            Dim streamSelect = TryCast(sourceFilter, IAMStreamSelect)
            If streamSelect IsNot Nothing Then

                Dim k As Integer = 0
                Dim streamsCount As Integer
                streamSelect.Count(streamsCount)
                For i As Integer = 0 To streamsCount - 1
                    Dim mt As VisioForge.DirectShowLib.AMMediaType
                    Dim name As String
                    streamSelect.Info(i, mt, Nothing, Nothing, Nothing, name, Nothing, Nothing)
                    If mt.majorType = MediaType.Video Then
                        If k = cbVideoStream.SelectedIndex Then
                            streamSelect.Enable(i, AMStreamSelectEnableFlags.Enable)
                        Else
                            streamSelect.Enable(i, AMStreamSelectEnableFlags.DisableAll)
                        End If

                        k += 1

                    End If
                Next
            End If
        End Sub

        Private Sub cbAudioStream_SelectedIndexChanged(sender As Object, e As EventArgs)
            Dim streamSelect = TryCast(sourceFilter, IAMStreamSelect)
            If streamSelect IsNot Nothing Then

                Dim k As Integer = 0
                Dim streamsCount As Integer
                streamSelect.Count(streamsCount)
                For i As Integer = 0 To streamsCount - 1
                    Dim mt As VisioForge.DirectShowLib.AMMediaType
                    Dim name As String
                    streamSelect.Info(i, mt, Nothing, Nothing, Nothing, name, Nothing, Nothing)
                    If mt.majorType = MediaType.Audio Then
                        If k = cbAudioStream.SelectedIndex Then
                            streamSelect.Enable(i, AMStreamSelectEnableFlags.Enable)
                        Else
                            streamSelect.Enable(i, AMStreamSelectEnableFlags.DisableAll)
                        End If

                        k += 1

                    End If
                Next
            End If
        End Sub
    End Class
End Namespace 