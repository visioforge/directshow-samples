Imports System.Runtime.InteropServices
Imports VisioForge.DirectShowAPI
Imports VisioForge.DirectShowLib

' ReSharper disable InconsistentNaming
' ReSharper disable RedundantAssignment

Public Class Form1
    Dim WM_GRAPHNOTIFY As UInt32 = &H8000 + 1

#Region "Camera 1 code"

    Dim filterGraphCamera1 As IFilterGraph2

    Dim captureGraphCamera1 As ICaptureGraphBuilder2

    Dim mediaControlCamera1 As IMediaControl

    Dim videoWindowCamera1 As IVideoWindow

    Dim mediaEventExCamera1 As IMediaEventEx

    Dim cameraFilter1 As IBaseFilter

    Dim cameraEffectsFilter1 As IBaseFilter

    Dim audioFilter1 As IBaseFilter

    Dim videoCodec As IBaseFilter

    Dim audioCodec As IBaseFilter

    Dim muxer As IBaseFilter

    Dim fileWriter As IBaseFilter

    Dim audioRenderer As IBaseFilter

    Dim videoSmartTee As IBaseFilter

    Dim audioSmartTee As IBaseFilter

    Dim cameraEffectsPro1 As IVFEffectsPro

    Private Sub CameraInitPreview1()

        Dim hr = 0

        ' An exception is thrown if cast fail
        filterGraphCamera1 = New FilterGraph()
        captureGraphCamera1 = New CaptureGraphBuilder2()
        mediaControlCamera1 = filterGraphCamera1
        ' ReSharper disable once SuspiciousTypeConversion.Global
        videoWindowCamera1 = DirectCast(filterGraphCamera1, IVideoWindow)
        mediaEventExCamera1 = filterGraphCamera1

        hr = mediaEventExCamera1.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero)
        DsError.ThrowExceptionForHR(hr)

        ' Attach the filter graph to the capture graph
        hr = captureGraphCamera1.SetFiltergraph(filterGraphCamera1)
        DsError.ThrowExceptionForHR(hr)

        cameraFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualCameraSource, "VisioForge Virtual Camera")

        Dim outputPin As IPin = FilterHelper.GetFreePinWithMediaType(cameraFilter1, PinDirection.Output, MediaType.Video)

        If (outputPin Is Nothing) Then

            Marshal.ReleaseComObject(cameraFilter1)
            cameraFilter1 = Nothing

            Return

        End If

        Dim videoFormats = New List(Of String)
        Dim frameRates = New List(Of String)
        Dim videoFormatsObj = New List(Of VFVideoCaptureFormat)

        Dim enumerator = New MediaFormatsEnumerator()

        enumerator.GetVideoFormatsAndFrameRates(
            outputPin, videoFormats, videoFormatsObj, frameRates)

        SetConfigParms(outputPin, cbCameraFrameRate1.Text, videoFormatsObj, cbCameraVideoFormat1.Text)

        ' adding video effects filter
        Dim effectsGuid = Consts.CLSID_VFVideoEffectsPro
        cameraEffectsFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, effectsGuid, "VisioForge Video Effects")

        cameraEffectsPro1 = cameraEffectsFilter1

        If (Not IsNothing(cameraEffectsPro1)) Then

            cameraEffectsPro1.set_enabled(True, False, False, False)

        End If

        ' Render the preview pin on the video capture filter
        ' Use this instead of graphBuilder.RenderFile
        hr = captureGraphCamera1.RenderStream(PinCategory.Capture, MediaType.Video, cameraFilter1, cameraEffectsFilter1, Nothing)
        DsError.ThrowExceptionForHR(hr)

        ' Set the video window to be a child of the main window
        hr = videoWindowCamera1.put_Owner(pnScreen1.Handle)
        DsError.ThrowExceptionForHR(hr)

        hr = videoWindowCamera1.put_WindowStyle(WindowStyle.Child Or WindowStyle.ClipChildren)
        DsError.ThrowExceptionForHR(hr)

        ' Use helper function to position video window in client rect 
        ' of main application window
        ResizeVideoWindow1()

        ' Make the video window visible, now that it is properly positioned
        hr = videoWindowCamera1.put_Visible(OABool.True)
        DsError.ThrowExceptionForHR(hr)

        audioFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualAudioCardSource, "VisioForge Virtual Audio Card")
        Dim pin As IPin = FilterHelper.GetFreePinWithMediaType(audioFilter1, PinDirection.Output, MediaType.Audio)

        filterGraphCamera1.Render(pin)

        hr = mediaControlCamera1.Run()
    End Sub

    Private Sub CameraInitCapture1()

        Dim hr = 0

        ' An exception is thrown if cast fail
        filterGraphCamera1 = New FilterGraph()
        captureGraphCamera1 = New CaptureGraphBuilder2()
        mediaControlCamera1 = CType(filterGraphCamera1, IMediaControl)
        videoWindowCamera1 = CType(filterGraphCamera1, IVideoWindow)
        mediaEventExCamera1 = CType(filterGraphCamera1, IMediaEventEx)

        hr = mediaEventExCamera1.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero)
        DsError.ThrowExceptionForHR(hr)

        ' Attach the filter graph to the capture graph
        hr = captureGraphCamera1.SetFiltergraph(filterGraphCamera1)
        DsError.ThrowExceptionForHR(hr)

        cameraFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualCameraSource, "VisioForge Virtual Camera")

        Dim outputPin As IPin = FilterHelper.GetFreePinWithMediaType(cameraFilter1, PinDirection.Output, MediaType.Video)

        If (IsNothing(outputPin)) Then

            Marshal.ReleaseComObject(cameraFilter1)
            cameraFilter1 = Nothing

            Return

        End If

        Dim videoFormats As List(Of String) = New List(Of String)
        Dim frameRates As List(Of String) = New List(Of String)
        Dim videoFormatsObj As List(Of VFVideoCaptureFormat) = New List(Of VFVideoCaptureFormat)

        Dim enumerator As MediaFormatsEnumerator = New MediaFormatsEnumerator()

        enumerator.GetVideoFormatsAndFrameRates(
            outputPin, videoFormats, videoFormatsObj, frameRates)

        SetConfigParms(outputPin, cbCameraFrameRate1.Text, videoFormatsObj, cbCameraVideoFormat1.Text)

        ' adding video effects filter
        'Guid effectsGuid = new Guid(Consts.CLSID_VFVideoEffectsPro)
        'cameraEffectsFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, effectsGuid, "VisioForge Video Effects")
        'cameraEffectsPro1 = cameraEffectsFilter1 as IVFEffectsPro

        'if (cameraEffectsPro1 != Nothing)
        '{
        '     cameraEffectsPro1.set_enabled(true, false, false, false)
        ' }

        videoSmartTee = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, New Guid("CC58E280-8AA1-11D1-B3F1-00AA003761C5"), "Smart Tee")

        ' Render the preview pin on the video capture filter
        ' Use this instead of graphBuilder.RenderFile
        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Video, cameraFilter1, Nothing, videoSmartTee)
        DsError.ThrowExceptionForHR(hr)

        videoCodec = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, New Guid("B80AB0A0-7416-11D2-9EEB-006008039E37"), "mjpeg compressor")

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Video, videoSmartTee, Nothing, videoCodec)
        DsError.ThrowExceptionForHR(hr)

        muxer = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, New Guid("E2510970-F137-11CE-8B67-00AA00A3F1A6"), "AVI Mux")

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Video, videoCodec, Nothing, muxer)
        DsError.ThrowExceptionForHR(hr)

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Video, videoSmartTee, Nothing, Nothing)
        DsError.ThrowExceptionForHR(hr)

        ' Set the video window to be a child of the main window
        hr = videoWindowCamera1.put_Owner(pnScreen1.Handle)
        DsError.ThrowExceptionForHR(hr)

        hr = videoWindowCamera1.put_WindowStyle(WindowStyle.Child Or WindowStyle.ClipChildren)
        DsError.ThrowExceptionForHR(hr)

        ' Use helper function to position video window in client rect 
        ' of main application window
        ResizeVideoWindow1()

        ' Make the video window visible, now that it is properly positioned
        hr = videoWindowCamera1.put_Visible(OABool.True)
        DsError.ThrowExceptionForHR(hr)

        audioFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualAudioCardSource, "VisioForge Virtual Audio Card")

        audioSmartTee = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, New Guid("CC58E280-8AA1-11D1-B3F1-00AA003761C5"), "Smart Tee")

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Audio, audioFilter1, Nothing, audioSmartTee)
        DsError.ThrowExceptionForHR(hr)

        audioCodec = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, New Guid("6A08CF80-0E18-11CF-A24D-0020AFD79767"), "pcm")

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Audio, audioSmartTee, Nothing, audioCodec)
        DsError.ThrowExceptionForHR(hr)

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Audio, audioCodec, Nothing, muxer)
        DsError.ThrowExceptionForHR(hr)

        fileWriter = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, New Guid("8596E5F0-0DA5-11D0-BD21-00A0C911CE86"), "Filewriter")

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Stream, muxer, Nothing, fileWriter)
        DsError.ThrowExceptionForHR(hr)

        audioRenderer = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, New Guid("79376820-07D0-11CF-A24D-0020AFD79767"), "Default DirectSound Device")

        hr = captureGraphCamera1.RenderStream(Nothing, MediaType.Audio, audioSmartTee, Nothing, audioRenderer)
        DsError.ThrowExceptionForHR(hr)

        Dim sink As IFileSinkFilter = fileWriter
        If (Not IsNothing(sink)) Then

            sink.SetFileName("c:\vf\___123.avi", Nothing)

        End If

        'IPin pin = DSHelper.GetFreePinWithMediaType(audioSmartTee, PinDirection.Output, MediaType.Audio)

        'filterGraphCamera1.Render(pin)

        'Marshal.ReleaseComObject(sourceVideoFilter)
        'sourceVideoFilter = Nothing

        'FilterGraphTools.SaveGraphFile(filterGraphCamera1, "c:\vf\_____capture.grf")

        hr = mediaControlCamera1.Run()

    End Sub

    ' Set the Framerate, and video size
    ' ReSharper disable once UnusedMethodReturnValue.Local
    Private Function SetConfigParms(videoPin As IPin, frameRate As Double, formats As List(Of VFVideoCaptureFormat), format As String) As Boolean

        Dim streamConfig As IAMStreamConfig = videoPin

        If (IsNothing(formats)) Then

            formats = New List(Of VFVideoCaptureFormat)

        End If

        If (frameRate.CompareTo(0) = 0) Then

            Return False

        End If

        Dim result As Boolean = True
        Dim k = -1

        For i As Integer = 0 To formats.Count

            If (formats(i).Name = format) Then

                k = i
                Exit For

            End If

        Next

        If ((k = -1) Or (Not DSHelper.ApplyVideoFormat(streamConfig, formats(k), frameRate))) Then
            result = False
            MessageBox.Show("Bad video capture format")
        End If

        Return result

    End Function

    Public Sub ResizeVideoWindow1()

        ' Resize the video preview window to match owner window size
        If (Not IsNothing(videoWindowCamera1)) Then

            videoWindowCamera1.SetWindowPosition(0, 0, pnScreen1.Width, pnScreen1.Height)

        End If
    End Sub

    Private Sub EnumCameraFormats1()

        cbCameraVideoFormat1.Items.Clear()
        cbCameraFrameRate1.Items.Clear()

        Dim enumerator As MediaFormatsEnumerator = New MediaFormatsEnumerator()

        Dim camera As IBaseFilter = FindVirtualCaptureDevice()
        If (IsNothing(camera)) Then

            Return

        End If

        Dim outputPin As IPin = FilterHelper.GetFreePinWithMediaType(camera, PinDirection.Output, MediaType.Video)

        If (IsNothing(outputPin)) Then

            Marshal.ReleaseComObject(camera)
            camera = Nothing

            Return

        End If

        Dim videoFormats As List(Of String) = New List(Of String)
        Dim frameRates As List(Of String) = New List(Of String)
        Dim videoFormatsObj As List(Of VFVideoCaptureFormat) = New List(Of VFVideoCaptureFormat)

        enumerator.GetVideoFormatsAndFrameRates(
            outputPin, videoFormats, videoFormatsObj, frameRates)

        For Each videoFormat As String In videoFormats

            cbCameraVideoFormat1.Items.Add(videoFormat)

        Next


        If (cbCameraVideoFormat1.Items.Count > 0) Then

            cbCameraVideoFormat1.SelectedIndex = 0

        End If

        For Each item As String In frameRates

            cbCameraFrameRate1.Items.Add(item)

        Next

        If (cbCameraFrameRate1.Items.Count > 0) Then

            cbCameraFrameRate1.SelectedIndex = 0

        End If

        Marshal.ReleaseComObject(outputPin)
        outputPin = Nothing
        Marshal.ReleaseComObject(camera)
        camera = Nothing

    End Sub

    Private Sub CameraFree1()

        ' Stop previewing data
        If (Not IsNothing(mediaControlCamera1)) Then

            mediaControlCamera1.StopWhenReady()

        End If

        ' Stop receiving events
        If (Not IsNothing(mediaEventExCamera1)) Then

            mediaEventExCamera1.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero)

        End If

        ' Relinquish ownership (IMPORTANT!) of the video window.
        ' Failing to call put_Owner can lead to assert failures within
        ' the video renderer, as it still assumes that it has a valid
        ' parent window.
        If (Not IsNothing(videoWindowCamera1)) Then

            videoWindowCamera1.put_Visible(OABool.False)
            videoWindowCamera1.put_Owner(IntPtr.Zero)

        End If

        FilterGraphTools.RemoveAllFilters(filterGraphCamera1)

        ' Release DirectShow interfaces
        Marshal.ReleaseComObject(mediaControlCamera1)
        mediaControlCamera1 = Nothing
        Marshal.ReleaseComObject(mediaEventExCamera1)
        mediaEventExCamera1 = Nothing
        Marshal.ReleaseComObject(videoWindowCamera1)
        videoWindowCamera1 = Nothing
        Marshal.ReleaseComObject(filterGraphCamera1)
        filterGraphCamera1 = Nothing
        Marshal.ReleaseComObject(captureGraphCamera1)
        captureGraphCamera1 = Nothing

    End Sub

#End Region

#Region "Camera 2 code"

    Dim filterGraphCamera2 As IFilterGraph2

    Dim captureGraphCamera2 As ICaptureGraphBuilder2

    Dim mediaControlCamera2 As IMediaControl

    Dim videoWindowCamera2 As IVideoWindow

    Dim mediaEventExCamera2 As IMediaEventEx

    Dim cameraFilter2 As IBaseFilter

    Dim cameraEffectsFilter2 As IBaseFilter


    Dim cameraEffectsPro2 As IVFEffectsPro

    Dim audioFilter2 As IBaseFilter

    Private Sub CameraInit2()

        Dim hr

        ' An exception is thrown if cast fail
        filterGraphCamera2 = New FilterGraph()
        captureGraphCamera2 = New CaptureGraphBuilder2()
        mediaControlCamera2 = filterGraphCamera2
        videoWindowCamera2 = filterGraphCamera2
        mediaEventExCamera2 = filterGraphCamera2

        hr = mediaEventExCamera2.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero)
        DsError.ThrowExceptionForHR(hr)

        ' Attach the filter graph to the capture graph
        hr = captureGraphCamera2.SetFiltergraph(filterGraphCamera2)
        DsError.ThrowExceptionForHR(hr)

        'Guid cameraGuid = new Guid("AA4DA14E-644B-487A-A7CB-517A390B4BB8")
        cameraFilter2 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera2, Consts.CLSID_VFVirtualCameraSource, "VisioForge Virtual Camera")

        Dim outputPin = FilterHelper.GetFreePinWithMediaType(cameraFilter2, PinDirection.Output, MediaType.Video)

        If (IsNothing(outputPin)) Then

            Marshal.ReleaseComObject(cameraFilter2)
            cameraFilter2 = Nothing

            Return

        End If

        Dim videoFormats As List(Of String) = New List(Of String)
        Dim frameRates As List(Of String) = New List(Of String)
        Dim videoFormatsObj As List(Of VFVideoCaptureFormat) = New List(Of VFVideoCaptureFormat)

        Dim enumerator As MediaFormatsEnumerator = New MediaFormatsEnumerator()

        enumerator.GetVideoFormatsAndFrameRates(
            outputPin, videoFormats, videoFormatsObj, frameRates)

        SetConfigParms(outputPin, cbCameraFrameRate2.Text, videoFormatsObj, cbCameraVideoFormat2.Text)

        ' adding video effects filter
        Dim effectsGuid = Consts.CLSID_VFVideoEffectsPro
        cameraEffectsFilter2 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera2, effectsGuid, "VisioForge Video Effects")
        cameraEffectsPro2 = cameraEffectsFilter2

        If (Not IsNothing(cameraEffectsPro2)) Then

            cameraEffectsPro2.set_enabled(True, False, False, False)

        End If

        ' Render the preview pin on the video capture filter
        ' Use this instead of graphBuilder.RenderFile
        hr = captureGraphCamera2.RenderStream(PinCategory.Capture, MediaType.Video, cameraFilter2, cameraEffectsFilter2, Nothing)
        DsError.ThrowExceptionForHR(hr)

        ' Set the video window to be a child of the main window
        hr = videoWindowCamera2.put_Owner(pnScreen2.Handle)
        DsError.ThrowExceptionForHR(hr)

        hr = videoWindowCamera2.put_WindowStyle(WindowStyle.Child Or WindowStyle.ClipChildren)
        DsError.ThrowExceptionForHR(hr)

        ' Use helper function to position video window in client rect 
        ' of main application window
        ResizeVideoWindow2()

        ' Make the video window visible, now that it is properly positioned
        hr = videoWindowCamera2.put_Visible(OABool.True)
        DsError.ThrowExceptionForHR(hr)

        'Marshal.ReleaseComObject(sourceVideoFilter)
        'sourceVideoFilter = Nothing
        audioFilter2 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera2, Consts.CLSID_VFVirtualAudioCardSource, "VisioForge Virtual Audio Card")
        Dim pin = FilterHelper.GetFreePinWithMediaType(audioFilter2, PinDirection.Output, MediaType.Audio)

        filterGraphCamera2.Render(pin)

        mediaControlCamera2.Run()

    End Sub

    Public Sub ResizeVideoWindow2()

        ' Resize the video preview window to match owner window size
        If (Not IsNothing(videoWindowCamera2)) Then
            videoWindowCamera2.SetWindowPosition(0, 0, pnScreen2.Width, pnScreen2.Height)
        End If

    End Sub

    Private Sub EnumCameraFormats2()

        cbCameraVideoFormat2.Items.Clear()
        cbCameraFrameRate2.Items.Clear()

        Dim enumerator As MediaFormatsEnumerator = New MediaFormatsEnumerator()

        Dim camera As IBaseFilter = FindVirtualCaptureDevice()
        If (IsNothing(camera)) Then

            Return

        End If

        Dim outputPin As IPin = FilterHelper.GetFreePinWithMediaType(camera, PinDirection.Output, MediaType.Video)

        If (IsNothing(outputPin)) Then

            Marshal.ReleaseComObject(camera)
            camera = Nothing

            Return

        End If

        Dim videoFormats As List(Of String) = New List(Of String)
        Dim frameRates As List(Of String) = New List(Of String)
        Dim videoFormatsObj As List(Of VFVideoCaptureFormat) = New List(Of VFVideoCaptureFormat)

        enumerator.GetVideoFormatsAndFrameRates(
            outputPin, videoFormats, videoFormatsObj, frameRates)


        For Each videoFormat As String In videoFormats

            cbCameraVideoFormat2.Items.Add(videoFormat)

        Next


        If (cbCameraVideoFormat2.Items.Count > 0) Then

            cbCameraVideoFormat2.SelectedIndex = 0

        End If

        For Each item As String In frameRates

            cbCameraFrameRate2.Items.Add(item)

        Next

        If (cbCameraFrameRate2.Items.Count > 0) Then

            cbCameraFrameRate2.SelectedIndex = 0

        End If

        Marshal.ReleaseComObject(outputPin)
        outputPin = Nothing

        Marshal.ReleaseComObject(camera)
        camera = Nothing
    End Sub

    Private Sub CameraFree2()

        ' Stop previewing data
        If (Not IsNothing(mediaControlCamera2)) Then

            mediaControlCamera2.StopWhenReady()

        End If

        ' Stop receiving events
        If (Not IsNothing(mediaEventExCamera2)) Then

            mediaEventExCamera2.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero)

        End If

        ' Relinquish ownership (IMPORTANT!) of the video window.
        ' Failing to call put_Owner can lead to assert failures within
        ' the video renderer, as it still assumes that it has a valid
        ' parent window.
        If (Not IsNothing(videoWindowCamera2)) Then

            videoWindowCamera2.put_Visible(OABool.False)
            videoWindowCamera2.put_Owner(IntPtr.Zero)

        End If

        FilterGraphTools.RemoveAllFilters(filterGraphCamera2)

        ' Release DirectShow interfaces
        Marshal.ReleaseComObject(mediaControlCamera2)
        mediaControlCamera2 = Nothing
        Marshal.ReleaseComObject(mediaEventExCamera2)
        mediaEventExCamera2 = Nothing
        Marshal.ReleaseComObject(videoWindowCamera2)
        videoWindowCamera2 = Nothing
        Marshal.ReleaseComObject(filterGraphCamera2)
        filterGraphCamera2 = Nothing
        Marshal.ReleaseComObject(captureGraphCamera2)
        captureGraphCamera2 = Nothing

    End Sub

#End Region

#Region "Sources code"

    Dim videoCaptureDevices As DsDevice()

    Dim audioCaptureDevices As DsDevice()

    Dim filterGraphSource As IFilterGraph2

    Dim captureGraphSource As ICaptureGraphBuilder2

    Dim mediaControlSource As IMediaControl

    Dim videoWindowSource As IVideoWindow

    Dim mediaEventExSource As IMediaEventEx

    Dim sourceVideoFilter As IBaseFilter

    Dim sourceAudioFilter As IBaseFilter

    Dim sinkVideoFilter As IBaseFilter

    Dim sinkAudioFilter As IBaseFilter

    Dim sourceEffectsFilter As IBaseFilter

    Dim sourceEffectsPro As IVFEffectsPro

    Dim sourceEffects As IVFEffects45

    Dim sourceSinkIntf As IVFVirtualCameraSink

    Private Function FindSourceVideoCaptureDevice() As IBaseFilter

        Dim source

        ' Take the first device
        Dim device = videoCaptureDevices(cbVideoCaptureSource.SelectedIndex)

        ' Bind Moniker to a filter object
        Dim iid = GetType(IBaseFilter).GUID
        device.Mon.BindToObject(Nothing, Nothing, iid, source)

        ' An exception is thrown if cast fail
        Return source

    End Function

    Private Function FindSourceAudioCaptureDevice() As IBaseFilter

        Dim source

        ' Take the first device
        Dim device = audioCaptureDevices(cbAudioCaptureSource.SelectedIndex)

        ' Bind Moniker to a filter object
        Dim iid = GetType(IBaseFilter).GUID
        device.Mon.BindToObject(Nothing, Nothing, iid, source)

        ' An exception is thrown if cast fail
        Return source

    End Function

    Private Sub SourceInitCamera()

        Dim hr

        ' An exception is thrown if cast fail
        filterGraphSource = New FilterGraph()
        captureGraphSource = New CaptureGraphBuilder2()
        mediaControlSource = filterGraphSource
        videoWindowSource = filterGraphSource
        mediaEventExSource = filterGraphSource

        hr = mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero)
        DsError.ThrowExceptionForHR(hr)

        ' Attach the filter graph to the capture graph
        hr = captureGraphSource.SetFiltergraph(filterGraphSource)
        DsError.ThrowExceptionForHR(hr)

        ' video source
        sourceVideoFilter = FindSourceVideoCaptureDevice()
        filterGraphSource.AddFilter(sourceVideoFilter, videoCaptureDevices(cbVideoCaptureSource.SelectedIndex).Name)

        Dim outputPin = FilterHelper.GetFreePinWithMediaType(sourceVideoFilter, PinDirection.Output, MediaType.Video)

        If (IsNothing(outputPin)) Then

            Marshal.ReleaseComObject(sourceVideoFilter)
            sourceVideoFilter = Nothing

            Return

        End If

        Dim videoFormats As List(Of String) = New List(Of String)
        Dim frameRates As List(Of String) = New List(Of String)
        Dim videoFormatsObj As List(Of VFVideoCaptureFormat) = New List(Of VFVideoCaptureFormat)

        Dim enumerator As MediaFormatsEnumerator = New MediaFormatsEnumerator()

        enumerator.GetVideoFormatsAndFrameRates(
            outputPin, videoFormats, videoFormatsObj, frameRates)

        SetConfigParms(outputPin, cbSourceFrameRate.Text, videoFormatsObj, cbSourceVideoFormat.Text)

        ' audio source
        sourceAudioFilter = FindSourceAudioCaptureDevice()
        filterGraphSource.AddFilter(sourceAudioFilter, audioCaptureDevices(cbAudioCaptureSource.SelectedIndex).Name)

        ' sinks
        sinkVideoFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualCameraSink, "VisioForge Virtual Camera Sink - Video")

        'sourceSinkIntf = videoSinkFilter as IVFVirtualCameraSink
        'sourceSinkIntf.set_license("TRIAL")

        sinkAudioFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualAudioCardSink, "VisioForge Virtual Camera Sink - Audio")

        ' adding video effects filter
        Dim effectsGuid = Consts.CLSID_VFVideoEffectsPro
        sourceEffectsFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, effectsGuid, "VisioForge Video Effects")
        sourceEffectsPro = sourceEffectsFilter
        sourceEffects = sourceEffectsFilter

        If (IsNothing(sourceEffectsPro)) Then
            sourceEffectsPro.set_enabled(True, False, False, False)
        End If

        ' Render the preview pin on the video capture filter
        ' Use this instead of graphBuilder.RenderFile
        hr = captureGraphSource.RenderStream(Nothing, Nothing, sourceVideoFilter, sourceEffectsFilter, sinkVideoFilter)
        DsError.ThrowExceptionForHR(hr)

        hr = captureGraphSource.RenderStream(Nothing, Nothing, sourceAudioFilter, Nothing, sinkAudioFilter)
        DsError.ThrowExceptionForHR(hr)

        mediaControlSource.Run()

    End Sub

    Private Sub SourceInitFile()
        Dim hr

        ' An exception is thrown if cast fail
        filterGraphSource = New FilterGraph()
        captureGraphSource = New CaptureGraphBuilder2()
        mediaControlSource = filterGraphSource
        videoWindowSource = filterGraphSource
        mediaEventExSource = filterGraphSource

        hr = mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero)
        DsError.ThrowExceptionForHR(hr)

        ' Attach the filter graph to the capture graph
        hr = captureGraphSource.SetFiltergraph(filterGraphSource)
        DsError.ThrowExceptionForHR(hr)

        'Guid sinkGuid = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340")
        sinkVideoFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualCameraSink, "VisioForge Virtual Camera Sink - Video")

        sourceSinkIntf = sinkVideoFilter
        sourceSinkIntf.set_license("TRIAL")

        sinkAudioFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualAudioCardSink, "VisioForge Virtual Camera Sink - Audio")

        filterGraphSource.AddSourceFilter(edSourceFile.Text, "Source file", sourceVideoFilter)

        ' adding video effects filter
        Dim effectsGuid = Consts.CLSID_VFVideoEffectsPro
        sourceEffectsFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, effectsGuid, "VisioForge Video Effects")
        sourceEffectsPro = sourceEffectsFilter
        sourceEffects = sourceEffectsFilter

        If (Not IsNothing(sourceEffectsPro)) Then

            sourceEffectsPro.set_enabled(True, False, False, False)

        End If

        ' Render the preview pin on the video capture filter
        hr = captureGraphSource.RenderStream(Nothing, Nothing, sourceVideoFilter, sourceEffectsFilter, sinkVideoFilter)
        DsError.ThrowExceptionForHR(hr)

        hr = captureGraphSource.RenderStream(Nothing, Nothing, sourceVideoFilter, Nothing, sinkAudioFilter)
        DsError.ThrowExceptionForHR(hr)

        mediaControlSource.Run()
    End Sub

    Private Sub SourceFree()
        ' Stop previewing data
        If (Not IsNothing(mediaControlSource)) Then
            mediaControlSource.StopWhenReady()
        End If

        ' Stop receiving events
        If (Not IsNothing(mediaEventExSource)) Then
            mediaEventExSource.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero)
        End If

        ' Relinquish ownership (IMPORTANT!) of the video window.
        ' Failing to call put_Owner can lead to assert failures within
        ' the video renderer, as it still assumes that it has a valid
        ' parent window.
        If (Not IsNothing(videoWindowSource)) Then
            videoWindowSource.put_Visible(OABool.False)
            videoWindowSource.put_Owner(IntPtr.Zero)
        End If

        FilterGraphTools.RemoveAllFilters(filterGraphSource)

        ' Release DirectShow interfaces
        If mediaControlSource IsNot Nothing Then
            Marshal.ReleaseComObject(mediaControlSource)
            mediaControlSource = Nothing
        End If
        If mediaEventExSource IsNot Nothing Then
            Marshal.ReleaseComObject(mediaEventExSource)
            mediaEventExSource = Nothing
        End If
        If videoWindowSource IsNot Nothing Then
            Marshal.ReleaseComObject(videoWindowSource)
            videoWindowSource = Nothing
        End If
        If filterGraphSource IsNot Nothing Then
            Marshal.ReleaseComObject(filterGraphSource)
            filterGraphSource = Nothing
        End If
        If captureGraphSource IsNot Nothing Then
            Marshal.ReleaseComObject(captureGraphSource)
            captureGraphSource = Nothing
        End If
        If sourceVideoFilter IsNot Nothing Then
            Marshal.ReleaseComObject(sourceVideoFilter)
            sourceVideoFilter = Nothing
        End If
        If sourceAudioFilter IsNot Nothing Then
            Marshal.ReleaseComObject(sourceAudioFilter)
            sourceAudioFilter = Nothing
        End If
        If sinkVideoFilter IsNot Nothing Then
            Marshal.ReleaseComObject(sinkVideoFilter)
            sinkVideoFilter = Nothing
        End If
        If sinkAudioFilter IsNot Nothing Then
            Marshal.ReleaseComObject(sinkAudioFilter)
            sinkAudioFilter = Nothing
        End If
    End Sub

    Private Function FindVirtualCaptureDevice() As IBaseFilter

        Dim source

        Dim device As DsDevice = Nothing

        For Each captureDevice As DsDevice In videoCaptureDevices

            If (captureDevice.Name = "VisioForge Virtual Camera") Then

                device = captureDevice

                Exit For

            End If

        Next

        If (IsNothing(device)) Then

            Return Nothing

        End If

        ' Bind Moniker to a filter object
        Dim iid As Guid = GetType(IBaseFilter).GUID
        device.Mon.BindToObject(Nothing, Nothing, iid, source)

        ' An exception is thrown if cast fail
        Return source

    End Function

    Private Sub EnumSourceCameraFormats()

        cbSourceVideoFormat.Items.Clear()
        cbSourceFrameRate.Items.Clear()

        Dim enumerator = New MediaFormatsEnumerator()

        Dim camera = FindSourceVideoCaptureDevice()
        If (IsNothing(camera)) Then

            Return

        End If

        Dim outputPin = FilterHelper.GetFreePinWithMediaType(camera, PinDirection.Output, MediaType.Video)

        If (IsNothing(outputPin)) Then

            Marshal.ReleaseComObject(camera)
            camera = Nothing

            Return

        End If

        Dim videoFormats As List(Of String) = New List(Of String)
        Dim frameRates As List(Of String) = New List(Of String)
        Dim videoFormatsObj As List(Of VFVideoCaptureFormat) = New List(Of VFVideoCaptureFormat)

        enumerator.GetVideoFormatsAndFrameRates(
            outputPin, videoFormats, videoFormatsObj, frameRates)


        For Each videoFormat As Object In videoFormats

            cbSourceVideoFormat.Items.Add(videoFormat)

        Next

        If (cbSourceVideoFormat.Items.Count > 0) Then

            cbSourceVideoFormat.SelectedIndex = 0

        End If

        For Each item As Object In frameRates

            cbSourceFrameRate.Items.Add(item)

        Next

        If (cbSourceFrameRate.Items.Count > 0) Then

            cbSourceFrameRate.SelectedIndex = 0

        End If

        Marshal.ReleaseComObject(outputPin)
        outputPin = Nothing
        Marshal.ReleaseComObject(camera)
        camera = Nothing

    End Sub

    Private Sub SourceAddTextLogo()

        If sourceEffects Is Nothing Then
            MessageBox.Show("Start playback before adding effect")
            Return
        End If

        Dim videoEffect As VFVideoEffectSimple = New VFVideoEffectSimple()
        videoEffect.ID = 0
        videoEffect.Enabled = True
        videoEffect.StartTime = videoEffect.StopTime = 0
        videoEffect.Type = VFVideoEffectType.TextLogo

        videoEffect.TextLogo.Text = edSourceTextLogo.Text
        videoEffect.TextLogo.X = videoEffect.TextLogo.Y = 50
        videoEffect.TextLogo.TransparentBg = True
        videoEffect.TextLogo.FontSize = fontDialog1.Font.Size
        videoEffect.TextLogo.FontName = fontDialog1.Font.Name
        videoEffect.TextLogo.FontBold = fontDialog1.Font.Bold
        videoEffect.TextLogo.FontItalic = fontDialog1.Font.Italic
        videoEffect.TextLogo.FontStrikeout = fontDialog1.Font.Strikeout
        videoEffect.TextLogo.FontUnderline = fontDialog1.Font.Underline

        sourceEffects.add_effect(videoEffect)

    End Sub

    Private Sub SourceAddImageLogo()

        If sourceEffects Is Nothing Then
            MessageBox.Show("Start playback before adding effect")
            Return
        End If

        Dim videoEffect = New VFVideoEffectSimple()
        videoEffect.ID = 1
        videoEffect.Enabled = True
        videoEffect.StartTime = videoEffect.StopTime = 0
        videoEffect.Type = VFVideoEffectType.GraphicalLogo

        videoEffect.GraphicalLogo.Filename = edSourceImageLogo.Text
        videoEffect.GraphicalLogo.X = videoEffect.GraphicalLogo.Y = 50

        sourceEffects.add_effect(videoEffect)

    End Sub

#End Region

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Get all video / audio input devices
        videoCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice)

        For Each device As DsDevice In videoCaptureDevices

            cbVideoCaptureSource.Items.Add(device.Name)

        Next

        If (cbVideoCaptureSource.Items.Count > 0) Then

            cbVideoCaptureSource.SelectedIndex = 0

        End If

        audioCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice)

        For Each device As DsDevice In audioCaptureDevices

            cbAudioCaptureSource.Items.Add(device.Name)

        Next

        If (cbAudioCaptureSource.Items.Count > 0) Then

            cbAudioCaptureSource.SelectedIndex = 0

        End If

        EnumCameraFormats1()
        EnumCameraFormats2()

    End Sub

    Private Sub btCameraStartPreview1_Click(sender As Object, e As EventArgs) Handles btCameraStartPreview1.Click

        btCameraStartPreview1.Enabled = False
        btCameraStartCapture1.Enabled = False
        btCameraStop1.Enabled = True

        CameraInitPreview1()

    End Sub

    Private Sub btCameraStop1_Click(sender As Object, e As EventArgs) Handles btCameraStop1.Click

        btCameraStartPreview1.Enabled = True
        btCameraStartCapture1.Enabled = True
        btCameraStop1.Enabled = False

        CameraFree1()

    End Sub

    Private Sub btSourceStart_Click(sender As Object, e As EventArgs) Handles btSourceStart.Click

        btSourceStart.Enabled = False
        btSourceStop.Enabled = True

        If (rbFile.Checked) Then

            SourceInitFile()

        Else

            SourceInitCamera()

        End If

    End Sub

    Private Sub btSourceStop_Click(sender As Object, e As EventArgs) Handles btSourceStop.Click

        btSourceStop.Enabled = False
        btSourceStart.Enabled = True

        SourceFree()

    End Sub

    Private Sub cbVideoCaptureSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbVideoCaptureSource.SelectedIndexChanged

        EnumSourceCameraFormats()

    End Sub

    Private Sub btCameraStart2_Click(sender As Object, e As EventArgs) Handles btCameraStart2.Click

        btCameraStart2.Enabled = False
        btCameraStop2.Enabled = True

        CameraInit2()

    End Sub

    Private Sub btCameraStop2_Click(sender As Object, e As EventArgs) Handles btCameraStop2.Click

        btCameraStart2.Enabled = True
        btCameraStop2.Enabled = False

        CameraFree2()

    End Sub

    Private Sub btSourceAddTextLogo_Click(sender As Object, e As EventArgs) Handles btSourceAddTextLogo.Click

        SourceAddTextLogo()

    End Sub

    Private Sub btSouceTextLogoFont_Click(sender As Object, e As EventArgs) Handles btSouceTextLogoFont.Click

        fontDialog1.ShowDialog()

    End Sub

    Private Sub btSourceImageLogoAdd_Click(sender As Object, e As EventArgs) Handles btSourceImageLogoAdd.Click

        SourceAddImageLogo()

    End Sub

    Private Sub btOpenFile_Click(sender As Object, e As EventArgs) Handles btOpenFile.Click

        If (openFileDialog1.ShowDialog() = DialogResult.OK) Then

            edSourceFile.Text = openFileDialog1.FileName

        End If

    End Sub

    Private Sub btCameraStartCapture1_Click(sender As Object, e As EventArgs) Handles btCameraStartCapture1.Click

        btCameraStartPreview1.Enabled = False
        btCameraStartCapture1.Enabled = False
        btCameraStop1.Enabled = True

        CameraInitCapture1()

    End Sub
End Class
