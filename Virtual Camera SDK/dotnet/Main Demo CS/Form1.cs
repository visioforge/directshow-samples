// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable InlineOutVariableDeclaration

using System.Drawing;
using System.IO;
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable EmptyGeneralCatchClause

namespace Main_Demo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisioForge.DirectShowAPI;
    using VisioForge.DirectShowLib;

    public partial class Form1 : Form
    {
        private const int WM_GRAPHNOTIFY = 0x8000 + 1;

        #region Camera 1 code

        private IFilterGraph2 filterGraphCamera1;

        private ICaptureGraphBuilder2 captureGraphCamera1;

        private IMediaControl mediaControlCamera1;

        private IVideoWindow videoWindowCamera1;

        private IMediaEventEx mediaEventExCamera1;

        private IBaseFilter cameraFilter1;

        private IBaseFilter cameraEffectsFilter1;

        private IBaseFilter audioFilter1;

        private IBaseFilter videoCodec;

        private IBaseFilter audioCodec;

        private IBaseFilter muxer;

        private IBaseFilter fileWriter;

        private IBaseFilter audioRenderer;

        private IBaseFilter videoSmartTee;

        private IBaseFilter audioSmartTee;

        private IVFEffectsPro cameraEffectsPro1;

        private void CameraInitPreview1()
        {
            // An exception is thrown if cast fail
            filterGraphCamera1 = (IFilterGraph2)new FilterGraph();
            captureGraphCamera1 = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlCamera1 = (IMediaControl)filterGraphCamera1;
            videoWindowCamera1 = (IVideoWindow)filterGraphCamera1;
            mediaEventExCamera1 = (IMediaEventEx)filterGraphCamera1;

            var hr = mediaEventExCamera1.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphCamera1.SetFiltergraph(filterGraphCamera1);
            DsError.ThrowExceptionForHR(hr);

            cameraFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualCameraSource, "VisioForge Virtual Camera");

            IPin outputPin = FilterHelper.GetFreePinWithMediaType(cameraFilter1, PinDirection.Output, MediaType.Video);

            if (outputPin == null)
            {
                Marshal.ReleaseComObject(cameraFilter1);
                cameraFilter1 = null;

                return;
            }

            List<string> videoFormats = new List<string>();
            List<string> frameRates = new List<string>();
            List<VFVideoCaptureFormat> videoFormatsObj = new List<VFVideoCaptureFormat>();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            enumerator.GetVideoFormatsAndFrameRates(
                outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);

            SetConfigParms(outputPin, float.Parse(cbCameraFrameRate1.Text), videoFormatsObj, cbCameraVideoFormat1.Text);

            // adding video effects filter
            Guid effectsGuid = Consts.CLSID_VFVideoEffectsPro;
            cameraEffectsFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, effectsGuid, "VisioForge Video Effects");
            cameraEffectsPro1 = cameraEffectsFilter1 as IVFEffectsPro;

            cameraEffectsPro1?.set_enabled(true, false, false, false);

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            hr = captureGraphCamera1.RenderStream(PinCategory.Capture, MediaType.Video, cameraFilter1, cameraEffectsFilter1, null);
            DsError.ThrowExceptionForHR(hr);

            // Set the video window to be a child of the main window
            hr = videoWindowCamera1.put_Owner(pnScreen1.Handle);
            DsError.ThrowExceptionForHR(hr);

            hr = videoWindowCamera1.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(hr);

            // Use helper function to position video window in client rect 
            // of main application window
            ResizeVideoWindow1();

            // Make the video window visible, now that it is properly positioned
            hr = videoWindowCamera1.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);

            audioFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualAudioCardSource, "VisioForge Virtual Audio Card");
            IPin pin = FilterHelper.GetFreePinWithMediaType(audioFilter1, PinDirection.Output, MediaType.Audio);

            filterGraphCamera1.Render(pin);

            //Marshal.ReleaseComObject(sourceVideoFilter);
            //sourceVideoFilter = null;

            if (cbDebugMode.Checked)
            {
                FilterGraphHelper.SaveGraphFile(filterGraphSource, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\virtual_camera_camera1.grf");
            }

            mediaControlCamera1.Run();
        }

        private void CameraInitCapture1()
        {
            // An exception is thrown if cast fail
            filterGraphCamera1 = (IFilterGraph2)new FilterGraph();
            captureGraphCamera1 = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlCamera1 = (IMediaControl)filterGraphCamera1;
            videoWindowCamera1 = (IVideoWindow)filterGraphCamera1;
            mediaEventExCamera1 = (IMediaEventEx)filterGraphCamera1;

            var hr = mediaEventExCamera1.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphCamera1.SetFiltergraph(filterGraphCamera1);
            DsError.ThrowExceptionForHR(hr);

            cameraFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualCameraSource  , "VisioForge Virtual Camera");

            IPin outputPin = FilterHelper.GetFreePinWithMediaType(cameraFilter1, PinDirection.Output, MediaType.Video);

            if (outputPin == null)
            {
                Marshal.ReleaseComObject(cameraFilter1);
                cameraFilter1 = null;

                return;
            }

            List<string> videoFormats = new List<string>();
            List<string> frameRates = new List<string>();
            List<VFVideoCaptureFormat> videoFormatsObj = new List<VFVideoCaptureFormat>();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            enumerator.GetVideoFormatsAndFrameRates(
                outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);

            SetConfigParms(outputPin, float.Parse(cbCameraFrameRate1.Text), videoFormatsObj, cbCameraVideoFormat1.Text);

            // adding video effects filter
            //Guid effectsGuid = new Guid(Consts.CLSID_VFVideoEffectsPro);
            //cameraEffectsFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, effectsGuid, "VisioForge Video Effects");
            //cameraEffectsPro1 = cameraEffectsFilter1 as IVFEffectsPro;

            //if (cameraEffectsPro1 != null)
            //{
            //     cameraEffectsPro1.set_enabled(true, false, false, false);
            // }

            videoSmartTee = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, new Guid("CC58E280-8AA1-11D1-B3F1-00AA003761C5"), "Smart Tee");

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            hr = captureGraphCamera1.RenderStream(null, MediaType.Video, cameraFilter1, null, videoSmartTee);
            DsError.ThrowExceptionForHR(hr);

            videoCodec = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, new Guid("B80AB0A0-7416-11D2-9EEB-006008039E37"), "mjpeg compressor");

            hr = captureGraphCamera1.RenderStream(null, MediaType.Video, videoSmartTee, null, videoCodec);
            DsError.ThrowExceptionForHR(hr);

            muxer = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, new Guid("E2510970-F137-11CE-8B67-00AA00A3F1A6"), "AVI Mux");

            hr = captureGraphCamera1.RenderStream(null, MediaType.Video, videoCodec, null, muxer);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphCamera1.RenderStream(null, MediaType.Video, videoSmartTee, null, videoWindowCamera1 as IBaseFilter);
            DsError.ThrowExceptionForHR(hr);

            // Set the video window to be a child of the main window
            hr = videoWindowCamera1.put_Owner(pnScreen1.Handle);
            DsError.ThrowExceptionForHR(hr);

            hr = videoWindowCamera1.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(hr);

            // Use helper function to position video window in client rect 
            // of main application window
            ResizeVideoWindow1();

            // Make the video window visible, now that it is properly positioned
            hr = videoWindowCamera1.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);

            audioFilter1 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, Consts.CLSID_VFVirtualAudioCardSource, "VisioForge Virtual Audio Card");

            audioSmartTee = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, new Guid("CC58E280-8AA1-11D1-B3F1-00AA003761C5"), "Smart Tee");

            hr = captureGraphCamera1.RenderStream(null, MediaType.Audio, audioFilter1, null, audioSmartTee);
            DsError.ThrowExceptionForHR(hr);

            audioCodec = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, new Guid("6A08CF80-0E18-11CF-A24D-0020AFD79767"), "pcm");

            hr = captureGraphCamera1.RenderStream(null, MediaType.Audio, audioSmartTee, null, audioCodec);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphCamera1.RenderStream(null, MediaType.Audio, audioCodec, null, muxer);
            DsError.ThrowExceptionForHR(hr);

            fileWriter = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, new Guid("8596E5F0-0DA5-11D0-BD21-00A0C911CE86"), "Filewriter");

            hr = captureGraphCamera1.RenderStream(null, MediaType.Stream, muxer, null, fileWriter);
            DsError.ThrowExceptionForHR(hr);

            audioRenderer = FilterGraphTools.AddFilterFromClsid(filterGraphCamera1, new Guid("79376820-07D0-11CF-A24D-0020AFD79767"), "Default DirectSound Device");

            hr = captureGraphCamera1.RenderStream(null, MediaType.Audio, audioSmartTee, null, audioRenderer);
            DsError.ThrowExceptionForHR(hr);

            IFileSinkFilter sink = fileWriter as IFileSinkFilter;
            sink?.SetFileName(@"c:\vf\___123.avi", null);

            //IPin pin = DSHelper.GetFreePinWithMediaType(audioSmartTee, PinDirection.Output, MediaType.Audio);

            //filterGraphCamera1.Render(pin);

            //Marshal.ReleaseComObject(sourceVideoFilter);
            //sourceVideoFilter = null;

            FilterGraphTools.SaveGraphFile(filterGraphCamera1, @"c:\vf\_____capture.grf");

            mediaControlCamera1.Run();
        }

        // Set the Framerate, and video size
        private static bool SetConfigParms(IPin videoPin, double frameRate, List<VFVideoCaptureFormat> formats, string format)
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

        public void ResizeVideoWindow1()
        {
            // Resize the video preview window to match owner window size
            videoWindowCamera1?.SetWindowPosition(0, 0, pnScreen1.Width, pnScreen1.Height);
        }

        private void EnumCameraFormats1()
        {
            cbCameraVideoFormat1.Items.Clear();
            cbCameraFrameRate1.Items.Clear();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            IBaseFilter camera = FindVirtualCaptureDevice();
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
                cbCameraVideoFormat1.Items.Add(videoFormat);
            }

            if (cbCameraVideoFormat1.Items.Count > 0)
            {
                cbCameraVideoFormat1.SelectedIndex = 0;
            }

            foreach (var item in frameRates)
            {
                cbCameraFrameRate1.Items.Add(item);
            }

            if (cbCameraFrameRate1.Items.Count > 0)
            {
                cbCameraFrameRate1.SelectedIndex = 0;
            }

            Marshal.ReleaseComObject(outputPin);
            // ReSharper disable once RedundantAssignment
            outputPin = null;

            Marshal.ReleaseComObject(camera);
            // ReSharper disable once RedundantAssignment
            camera = null;
        }

        private void CameraFree1()
        {
            // Stop previewing data
            mediaControlCamera1?.StopWhenReady();

            // Stop receiving events
            mediaEventExCamera1?.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);

            // Relinquish ownership (IMPORTANT!) of the video window.
            // Failing to call put_Owner can lead to assert failures within
            // the video renderer, as it still assumes that it has a valid
            // parent window.
            if (videoWindowCamera1 != null)
            {
                videoWindowCamera1.put_Visible(OABool.False);
                videoWindowCamera1.put_Owner(IntPtr.Zero);
            }

            FilterGraphTools.RemoveAllFilters(filterGraphCamera1);

            // Release DirectShow interfaces
            if (mediaControlCamera1 != null)
            {
                Marshal.ReleaseComObject(mediaControlCamera1);
                mediaControlCamera1 = null;
            }

            if (mediaEventExCamera1 != null)
            {
                Marshal.ReleaseComObject(mediaEventExCamera1);
                mediaEventExCamera1 = null;
            }

            if (videoWindowCamera1 != null)
            {
                Marshal.ReleaseComObject(videoWindowCamera1);
                videoWindowCamera1 = null;
            }
            
            Marshal.ReleaseComObject(filterGraphCamera1);
            filterGraphCamera1 = null;
            Marshal.ReleaseComObject(captureGraphCamera1);
            captureGraphCamera1 = null;
        }

        #endregion

        #region Camera 2 code

        private IFilterGraph2 filterGraphCamera2;

        private ICaptureGraphBuilder2 captureGraphCamera2;

        private IMediaControl mediaControlCamera2;

        private IVideoWindow videoWindowCamera2;

        private IMediaEventEx mediaEventExCamera2;

        private IBaseFilter cameraFilter2;

        private IBaseFilter cameraEffectsFilter2;

        private IVFEffectsPro cameraEffectsPro2;

        // ReSharper disable once NotAccessedField.Local
        private IVFEffects45 cameraEffects2;

        private IBaseFilter audioFilter2;

        private void CameraInit2()
        {
            // An exception is thrown if cast fail
            filterGraphCamera2 = (IFilterGraph2)new FilterGraph();
            captureGraphCamera2 = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlCamera2 = (IMediaControl)filterGraphCamera2;
            videoWindowCamera2 = (IVideoWindow)filterGraphCamera2;
            mediaEventExCamera2 = (IMediaEventEx)filterGraphCamera2;

            var hr = mediaEventExCamera2.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphCamera2.SetFiltergraph(filterGraphCamera2);
            DsError.ThrowExceptionForHR(hr);

            //Guid cameraGuid = new Guid("AA4DA14E-644B-487A-A7CB-517A390B4BB8");
            cameraFilter2 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera2, Consts.CLSID_VFVirtualCameraSource, "VisioForge Virtual Camera");

            IPin outputPin = FilterHelper.GetFreePinWithMediaType(cameraFilter2, PinDirection.Output, MediaType.Video);

            if (outputPin == null)
            {
                Marshal.ReleaseComObject(cameraFilter2);
                cameraFilter2 = null;

                return;
            }

            List<string> videoFormats = new List<string>();
            List<string> frameRates = new List<string>();
            List<VFVideoCaptureFormat> videoFormatsObj = new List<VFVideoCaptureFormat>();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            enumerator.GetVideoFormatsAndFrameRates(
                outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);

            SetConfigParms(outputPin, float.Parse(cbCameraFrameRate2.Text), videoFormatsObj, cbCameraVideoFormat2.Text);

            // adding video effects filter
            Guid effectsGuid = Consts.CLSID_VFVideoEffectsPro;
            cameraEffectsFilter2 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera2, effectsGuid, "VisioForge Video Effects");
            cameraEffectsPro2 = cameraEffectsFilter2 as IVFEffectsPro;
            cameraEffects2 = cameraEffectsFilter2 as IVFEffects45;

            cameraEffectsPro2?.set_enabled(true, false, false, false);

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            hr = captureGraphCamera2.RenderStream(PinCategory.Capture, MediaType.Video, cameraFilter2, cameraEffectsFilter2, null);
            DsError.ThrowExceptionForHR(hr);

            // Set the video window to be a child of the main window
            hr = videoWindowCamera2.put_Owner(pnScreen2.Handle);
            DsError.ThrowExceptionForHR(hr);

            hr = videoWindowCamera2.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(hr);

            // Use helper function to position video window in client rect 
            // of main application window
            ResizeVideoWindow2();

            // Make the video window visible, now that it is properly positioned
            hr = videoWindowCamera2.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);

            //Marshal.ReleaseComObject(sourceVideoFilter);
            //sourceVideoFilter = null;
            audioFilter2 = FilterGraphTools.AddFilterFromClsid(filterGraphCamera2, Consts.CLSID_VFVirtualAudioCardSource, "VisioForge Virtual Audio Card");
            IPin pin = FilterHelper.GetFreePinWithMediaType(audioFilter2, PinDirection.Output, MediaType.Audio);

            filterGraphCamera2.Render(pin);

            if (cbDebugMode.Checked)
            {
                FilterGraphHelper.SaveGraphFile(filterGraphSource, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\virtual_camera_camera2.grf");
            }

            mediaControlCamera2.Run();
        }

        public void ResizeVideoWindow2()
        {
            // Resize the video preview window to match owner window size
            videoWindowCamera2?.SetWindowPosition(0, 0, pnScreen2.Width, pnScreen2.Height);
        }

        private void EnumCameraFormats2()
        {
            cbCameraVideoFormat2.Items.Clear();
            cbCameraFrameRate2.Items.Clear();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            IBaseFilter camera = FindVirtualCaptureDevice();
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
                cbCameraVideoFormat2.Items.Add(videoFormat);
            }

            if (cbCameraVideoFormat2.Items.Count > 0)
            {
                cbCameraVideoFormat2.SelectedIndex = 0;
            }

            foreach (var item in frameRates)
            {
                cbCameraFrameRate2.Items.Add(item);
            }

            if (cbCameraFrameRate2.Items.Count > 0)
            {
                cbCameraFrameRate2.SelectedIndex = 0;
            }

            Marshal.ReleaseComObject(outputPin);
            // ReSharper disable once RedundantAssignment
            outputPin = null;

            Marshal.ReleaseComObject(camera);
            // ReSharper disable once RedundantAssignment
            camera = null;
        }

        private void CameraFree2()
        {
            // Stop previewing data
            mediaControlCamera2?.StopWhenReady();

            // Stop receiving events
            mediaEventExCamera2?.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);

            // Relinquish ownership (IMPORTANT!) of the video window.
            // Failing to call put_Owner can lead to assert failures within
            // the video renderer, as it still assumes that it has a valid
            // parent window.
            if (videoWindowCamera2 != null)
            {
                videoWindowCamera2.put_Visible(OABool.False);
                videoWindowCamera2.put_Owner(IntPtr.Zero);
            }

            // Release DirectShow interfaces
            if (mediaControlCamera2 != null)
            {
                Marshal.ReleaseComObject(mediaControlCamera2);
                mediaControlCamera2 = null;
            }

            if (mediaEventExCamera2 != null)
            {
                Marshal.ReleaseComObject(mediaEventExCamera2);
                mediaEventExCamera2 = null;
            }

            if (videoWindowCamera2 != null)
            {
                Marshal.ReleaseComObject(videoWindowCamera2);
                videoWindowCamera2 = null;
            }

            Marshal.ReleaseComObject(filterGraphCamera2);
            filterGraphCamera2 = null;
            Marshal.ReleaseComObject(captureGraphCamera2);
            captureGraphCamera2 = null;
        }

        #endregion

        #region Sources code

        private DsDevice[] videoCaptureDevices;

        private DsDevice[] audioCaptureDevices;

        private IFilterGraph2 filterGraphSource;

        private ICaptureGraphBuilder2 captureGraphSource;

        private IMediaControl mediaControlSource;

        private IVideoWindow videoWindowSource;

        private IMediaEventEx mediaEventExSource;

        private IBaseFilter sourceVideoFilter;

        private IBaseFilter sourceAudioFilter;

        private IBaseFilter sinkVideoFilter;

        private IBaseFilter sinkAudioFilter;

        private IBaseFilter sourceEffectsFilter;

        private IVFEffectsPro sourceEffectsPro;

        private IVFEffects45 sourceEffects;

        private IVFVirtualCameraSink sourceSinkIntf;

        private IVFLiveVideoSource sourceVideoSourcePushSource;

        private FrameCache sourceVideoSourcePushSourceCache;

        private int sourceVideoSourceWidth;

        private int sourceVideoSourceHeight;

        private IBaseFilter FindSourceVideoCaptureDevice()
        {
            object source;

            // Take the first device
            DsDevice device = videoCaptureDevices[cbVideoCaptureSource.SelectedIndex];

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
            DsDevice device = audioCaptureDevices[cbAudioCaptureSource.SelectedIndex];

            // Bind Moniker to a filter object
            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out source);

            // An exception is thrown if cast fail
            return (IBaseFilter)source;
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        private void SourceInitCamera()
        {
            // An exception is thrown if cast fail
            filterGraphSource = (IFilterGraph2)new FilterGraph();
            captureGraphSource = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlSource = (IMediaControl)filterGraphSource;
            videoWindowSource = (IVideoWindow)filterGraphSource;
            mediaEventExSource = (IMediaEventEx)filterGraphSource;

            var hr = mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphSource.SetFiltergraph(filterGraphSource);
            DsError.ThrowExceptionForHR(hr);

            // video source
            sourceVideoFilter = FindSourceVideoCaptureDevice();
            filterGraphSource.AddFilter(sourceVideoFilter, videoCaptureDevices[cbVideoCaptureSource.SelectedIndex].Name);

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

            enumerator.GetVideoFormatsAndFrameRates(
                outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);

            SetConfigParms(outputPin, float.Parse(cbSourceFrameRate.Text), videoFormatsObj, cbSourceVideoFormat.Text);

            // audio source
            sourceAudioFilter = FindSourceAudioCaptureDevice();
            filterGraphSource.AddFilter(sourceAudioFilter, audioCaptureDevices[cbAudioCaptureSource.SelectedIndex].Name);

            // sinks
            sinkVideoFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualCameraSink, "VisioForge Virtual Camera Sink - Video");

            //sourceSinkIntf = videoSinkFilter as IVFVirtualCameraSink;
            //sourceSinkIntf.set_license("TRIAL");

            sinkAudioFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualAudioCardSink, "VisioForge Virtual Camera Sink - Audio");

            // adding video effects filter
            Guid effectsGuid = Consts.CLSID_VFVideoEffectsPro;
            sourceEffectsFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, effectsGuid, "VisioForge Video Effects");
            sourceEffectsPro = sourceEffectsFilter as IVFEffectsPro;
            sourceEffects = sourceEffectsFilter as IVFEffects45;

            sourceEffectsPro?.set_enabled(true, false, false, false);

            // Render the preview pin on the video capture filter
            // Use this instead of graphBuilder.RenderFile
            hr = captureGraphSource.RenderStream(null, null, sourceVideoFilter, sourceEffectsFilter, sinkVideoFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, sourceAudioFilter, null, sinkAudioFilter);
            DsError.ThrowExceptionForHR(hr);

            if (cbDebugMode.Checked)
            {
                FilterGraphHelper.SaveGraphFile(filterGraphSource, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\virtual_camera_source.grf");
            }

            mediaControlSource.Run();
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        private void SourceInitFile()
        {
            // An exception is thrown if cast fail
            filterGraphSource = (IFilterGraph2)new FilterGraph();
            captureGraphSource = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlSource = (IMediaControl)filterGraphSource;
            videoWindowSource = (IVideoWindow)filterGraphSource;
            mediaEventExSource = (IMediaEventEx)filterGraphSource;

            var hr = mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphSource.SetFiltergraph(filterGraphSource);
            DsError.ThrowExceptionForHR(hr);

            // Guid sinkGuid = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340");
            sinkVideoFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualCameraSink, "VisioForge Virtual Camera Sink - Video");

            sourceSinkIntf = sinkVideoFilter as IVFVirtualCameraSink;
            sourceSinkIntf?.set_license("TRIAL");

            sinkAudioFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualAudioCardSink, "VisioForge Virtual Camera Sink - Audio");

            filterGraphSource.AddSourceFilter(edSourceFile.Text, "Source file", out sourceVideoFilter);

            // adding video effects filter
            Guid effectsGuid = Consts.CLSID_VFVideoEffectsPro;
            sourceEffectsFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, effectsGuid, "VisioForge Video Effects");
            sourceEffectsPro = sourceEffectsFilter as IVFEffectsPro;
            sourceEffects = sourceEffectsFilter as IVFEffects45;

            sourceEffectsPro?.set_enabled(true, false, false, false);

            // Render the preview pin on the video capture filter
            hr = captureGraphSource.RenderStream(null, null, sourceVideoFilter, sourceEffectsFilter, sinkVideoFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphSource.RenderStream(null, null, sourceVideoFilter, null, sinkAudioFilter);
            DsError.ThrowExceptionForHR(hr);

            if (cbDebugMode.Checked)
            {
                FilterGraphHelper.SaveGraphFile(filterGraphSource, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\virtual_camera_source.grf");
            }

            mediaControlSource.Run();
        }

        private static string[] GetFiles(string folder, string[] exts)
        {
            var files2 = new List<string>();
            foreach (var ext in exts)
            {
                var files = Directory.GetFiles(folder, ext);
                foreach (var file in files)
                {
                    files2.Add(file);
                }
            }

            return files2.ToArray();
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        private void SourceInitFolderWithImages()
        {
            // An exception is thrown if cast fail
            filterGraphSource = (IFilterGraph2)new FilterGraph();
            captureGraphSource = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControlSource = (IMediaControl)filterGraphSource;
            videoWindowSource = (IVideoWindow)filterGraphSource;
            mediaEventExSource = (IMediaEventEx)filterGraphSource;

            var hr = mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            // Attach the filter graph to the capture graph
            hr = captureGraphSource.SetFiltergraph(filterGraphSource);
            DsError.ThrowExceptionForHR(hr);

            // Guid sinkGuid = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340");
            sinkVideoFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, Consts.CLSID_VFVirtualCameraSink, "VisioForge Virtual Camera Sink - Video");

            sourceSinkIntf = sinkVideoFilter as IVFVirtualCameraSink;
            sourceSinkIntf?.set_license("TRIAL");

            // add push source and sample grabber
            // ReSharper disable once InconsistentNaming
            Guid CLSID_VFVideoPushSource = new Guid("38D15306-BBC6-4D6C-A89C-9621604D9FC1");
            sourceVideoFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, CLSID_VFVideoPushSource, "VisioForge Video Push Source");

            if (sourceVideoFilter == null)
            {
                SourceFree();
                throw new Exception("Unable to create VisioForge Push Source filter.");
            }

            sourceVideoSourceWidth = Convert.ToInt32(edImagesWidth.Text);
            sourceVideoSourceHeight = Convert.ToInt32(edImagesHeight.Text);
            float frameRate = 10;

            sourceVideoSourcePushSource = sourceVideoFilter as IVFLiveVideoSource;
            if (sourceVideoSourcePushSource == null)
            {
                SourceFree();
                throw new Exception("Unable to get VisioForge Push Source Filter interface.");
            }

            var bmiHeader = new BitmapInfoHeader
            {
                BitCount = 24,
                Compression = 0,
                Width = sourceVideoSourceWidth,
                Height = sourceVideoSourceHeight,
                Planes = 1,
                Size = Marshal.SizeOf(typeof(BitmapInfoHeader)),
                ImageSize = ImageHelper.GetStrideRGB24(sourceVideoSourceWidth) * sourceVideoSourceHeight
            };

            sourceVideoSourcePushSource.SetBitmapInfo(bmiHeader);
            sourceVideoSourcePushSource.SetFrameRate(frameRate);

            sourceVideoSourcePushSourceCache = new FrameCache(sourceVideoSourceWidth, sourceVideoSourceHeight);
            var files = GetFiles(edImagesPath.Text, new [] { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif" } );

            long frameDuration = 1000 / Convert.ToInt32(edImagesPerSecond.Text);
            long timestamp = 0;
            foreach (var file in files)
            {
                var bmp = new Bitmap(file);
                sourceVideoSourcePushSourceCache.Add(bmp, timestamp, timestamp + frameDuration);
                timestamp += frameDuration;
            }

            // adding video effects filter
            var effectsGuid = Consts.CLSID_VFVideoEffectsPro;
            sourceEffectsFilter = FilterGraphTools.AddFilterFromClsid(filterGraphSource, effectsGuid, "VisioForge Video Effects");
            sourceEffectsPro = sourceEffectsFilter as IVFEffectsPro;
            sourceEffects = sourceEffectsFilter as IVFEffects45;

            sourceEffectsPro?.set_enabled(true, false, false, false);

            // Render the preview pin on the video capture filter
            hr = captureGraphSource.RenderStream(null, null, sourceVideoFilter, sourceEffectsFilter, sinkVideoFilter);
            DsError.ThrowExceptionForHR(hr);

            if (cbDebugMode.Checked)
            {
                FilterGraphHelper.SaveGraphFile(filterGraphSource, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\virtual_camera_source.grf");
            }

            mediaControlSource.Run();

            sourcePusherStartTimeStamp = 0;
            sourcePusherFrameDuration = (long)(1000 * 10000 / frameRate);
            tmPusher.Interval = (int)(1000 / frameRate); 
            tmPusher.Enabled = true;
        }

        private void SourceFree()
        {
            try
            {
                sourceVideoSourcePushSource = null;

                sourceVideoSourcePushSourceCache = null;

                // Stop previewing data
                if (mediaControlSource != null)
                {
                    mediaControlSource.StopWhenReady();
                    mediaControlSource.Stop();
                }

                // Stop receiving events
                mediaEventExSource?.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);

                // Relinquish ownership (IMPORTANT!) of the video window.
                // Failing to call put_Owner can lead to assert failures within
                // the video renderer, as it still assumes that it has a valid
                // parent window.
                if (videoWindowSource != null)
                {
                    videoWindowSource.put_Visible(OABool.False);
                    videoWindowSource.put_Owner(IntPtr.Zero);
                }
                
                FilterGraphTools.RemoveAllFilters(filterGraphSource);

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

                if (videoWindowSource != null)
                {
                    Marshal.ReleaseComObject(videoWindowSource);
                    videoWindowSource = null;
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

                if (sinkVideoFilter != null)
                {
                    Marshal.ReleaseComObject(sinkVideoFilter);
                    sinkVideoFilter = null;
                }

                if (sinkAudioFilter != null)
                {
                    Marshal.ReleaseComObject(sinkAudioFilter);
                    sinkAudioFilter = null;
                }
            }
            catch
            {
            }
        }

        private IBaseFilter FindVirtualCaptureDevice()
        {
            object source;

            DsDevice device = null;
            foreach (var captureDevice in videoCaptureDevices)
            {
                if (captureDevice.Name == "VisioForge Virtual Camera")
                {
                    device = captureDevice;

                    break;
                }
            }

            if (device == null)
            {
                return null;
            }

            // Bind Moniker to a filter object
            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out source);

            // An exception is thrown if cast fail
            return (IBaseFilter)source;
        }

        private void EnumSourceCameraFormats()
        {
            cbSourceVideoFormat.Items.Clear();
            cbSourceFrameRate.Items.Clear();

            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

            // DsDevice device = videoCaptureDevices[cbVideoCaptureSource.SelectedIndex];

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

        private void SourceAddTextLogo()
        {
            VFVideoEffectSimple videoEffect = new VFVideoEffectSimple
            {
                ID = 0,
                Enabled = true
            };

            videoEffect.StartTime = videoEffect.StopTime = 0;
            videoEffect.Type = (int)VFVideoEffectType.TextLogo;

            videoEffect.TextLogo.Text = edSourceTextLogo.Text;
            videoEffect.TextLogo.X = videoEffect.TextLogo.Y = 50;
            videoEffect.TextLogo.TransparentBg = true;
            videoEffect.TextLogo.FontSize = (int)fontDialog1.Font.Size;
            videoEffect.TextLogo.FontName = fontDialog1.Font.Name;
            videoEffect.TextLogo.FontBold = fontDialog1.Font.Bold;
            videoEffect.TextLogo.FontItalic = fontDialog1.Font.Italic;
            videoEffect.TextLogo.FontStrikeout = fontDialog1.Font.Strikeout;
            videoEffect.TextLogo.FontUnderline = fontDialog1.Font.Underline;

            sourceEffects.add_effect(videoEffect);
        }

        private void SourceAddImageLogo()
        {
            VFVideoEffectSimple videoEffect = new VFVideoEffectSimple
            {
                ID = 1,
                Enabled = true
            };

            videoEffect.StartTime = videoEffect.StopTime = 0;
            videoEffect.Type = (int)VFVideoEffectType.GraphicalLogo;

            videoEffect.GraphicalLogo.Filename = edSourceImageLogo.Text;
            videoEffect.GraphicalLogo.X = videoEffect.GraphicalLogo.Y = 50;

            sourceEffects.add_effect(videoEffect);
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

            EnumCameraFormats1();
            EnumCameraFormats2();
        }

        private void btCameraStart_Click(object sender, EventArgs e)
        {
            btCameraStartPreview1.Enabled = false;
            btCameraStartCapture1.Enabled = false;
            btCameraStop1.Enabled = true;

            CameraInitPreview1();
        }

        private void btCameraStop_Click(object sender, EventArgs e)
        {
            btCameraStartPreview1.Enabled = true;
            btCameraStartCapture1.Enabled = true;
            btCameraStop1.Enabled = false;

            CameraFree1();
        }

        private void btSourceStart_Click(object sender, EventArgs e)
        {
            btSourceStart.Enabled = false;
            btSourceStop.Enabled = true;

            if (rbFile.Checked)
            {
                SourceInitFile();
            }
            else if (rbCamera.Checked)
            {
                SourceInitCamera();
            }
            else
            {
                SourceInitFolderWithImages();
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

        private void btCameraStart2_Click(object sender, EventArgs e)
        {
            btCameraStart2.Enabled = false;
            btCameraStop2.Enabled = true;

            CameraInit2();
        }

        private void btCameraStop2_Click(object sender, EventArgs e)
        {
            btCameraStart2.Enabled = true;
            btCameraStop2.Enabled = false;

            CameraFree2();
        }

        private void btSourceAddTextLogo_Click(object sender, EventArgs e)
        {
            SourceAddTextLogo();
        }

        private void btSouceTextLogoFont_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
        }

        private void btSourceImageLogoAdd_Click(object sender, EventArgs e)
        {
            SourceAddImageLogo();
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                edSourceFile.Text = openFileDialog1.FileName;
            }
        }

        private void btCameraStartCapture1_Click(object sender, EventArgs e)
        {
            btCameraStartPreview1.Enabled = false;
            btCameraStartCapture1.Enabled = false;
            btCameraStop1.Enabled = true;

            CameraInitCapture1();
        }

        private void btOpenFolderWithImages_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                edImagesPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private long sourcePusherStartTimeStamp;

        private long sourcePusherFrameDuration;

        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr MemSet(IntPtr dest, int c, int byteCount);

        private void tmPusher_Tick(object sender, EventArgs e)
        {
            if (sourceVideoSourcePushSource == null)
            {
                return;
            }

            var frame = new AVFrameData
            {
                StartTime = sourcePusherStartTimeStamp
            };

            sourcePusherStartTimeStamp += sourcePusherFrameDuration;
            frame.StopTime = sourcePusherStartTimeStamp;

            frame.Size = ImageHelper.GetStrideRGB24(sourceVideoSourceWidth) * sourceVideoSourceHeight;

            frame.Data = sourceVideoSourcePushSourceCache.GetFrame(sourcePusherStartTimeStamp / 10000);
            
            if (frame.Data == IntPtr.Zero)
            {
                frame.Data = Marshal.AllocCoTaskMem(frame.Size);
                MemSet(frame.Data, 0, frame.Size);
            }

            sourceVideoSourcePushSource.AddFrame(frame);
        }
    }
}
