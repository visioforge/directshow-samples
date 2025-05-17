using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace StreamCamera
{
    internal class Program
    {
        private static IFilterGraph2 filterGraph;
        private static ICaptureGraphBuilder2 captureGraph;
        private static IMediaControl mediaControl;
        private static IMediaEventEx mediaEventEx;
        private static IBaseFilter sourceVideo;
        private static IBaseFilter virtualCamera;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enumerating video source devices:");

            // Get all video capture devices
            var videoCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            if (videoCaptureDevices.Length == 0)
            {
                Console.WriteLine("No video capture devices found.");
                return;
            }

            // Print all available capture devices
            for (int i = 0; i < videoCaptureDevices.Length; i++)
            {
                Console.WriteLine($"{i}: {videoCaptureDevices[i].Name}");
            }

            // Default to first device
            int deviceIndex = 0;

            // If there are multiple devices, ask the user to select one
            if (videoCaptureDevices.Length > 1)
            {
                bool validSelection = false;
                while (!validSelection)
                {
                    Console.Write("\nSelect device number: ");
                    string input = Console.ReadLine().Trim();
                    
                    if (int.TryParse(input, out int selectedIndex) && 
                        selectedIndex >= 0 && selectedIndex < videoCaptureDevices.Length)
                    {
                        deviceIndex = selectedIndex;
                        validSelection = true;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid selection. Please enter a number between 0 and {videoCaptureDevices.Length - 1}.");
                    }
                }
            }

            Console.WriteLine($"\nSelected device: {deviceIndex}: {videoCaptureDevices[deviceIndex].Name}");

            // Initialize DirectShow filter graph
            try
            {
                InitializeGraph();
                
                // Find the specified capture device and add it to the graph
                bool deviceFound = AddCaptureDevice(videoCaptureDevices, deviceIndex);
                if (!deviceFound)
                {
                    Console.WriteLine("Failed to add capture device to graph.");
                    ReleaseResources();
                    return;
                }

                // Configure the device to use 1280x720 25fps if available
                bool formatSet = ConfigureVideoFormat();
                if (!formatSet)
                {
                    Console.WriteLine("Failed to set requested video format.");
                    // Continue anyway with default format
                }

                // Set up the sink (virtual camera)
                bool sinkSetup = SetupVirtualCameraSink();
                if (!sinkSetup)
                {
                    Console.WriteLine("Failed to set up virtual camera sink.");
                    ReleaseResources();
                    return;
                }

                // Connect everything
                bool connected = ConnectFilters();
                if (!connected)
                {
                    Console.WriteLine("Failed to connect filters.");
                    ReleaseResources();
                    return;
                }

                // Start streaming
                mediaControl.Run();

                Console.WriteLine("Streaming started. Press Enter to stop...");
                Console.ReadLine();

                // Stop streaming
                mediaControl.Stop();
                ReleaseResources();
                Console.WriteLine("Streaming stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ReleaseResources();
            }
        }

        private static void InitializeGraph()
        {
            // Create and initialize the filter graph
            filterGraph = (IFilterGraph2)new FilterGraph();
            captureGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            mediaControl = (IMediaControl)filterGraph;
            mediaEventEx = (IMediaEventEx)filterGraph;

            // Attach the filter graph to the capture graph
            var hr = captureGraph.SetFiltergraph(filterGraph);
            DsError.ThrowExceptionForHR(hr);

            Console.WriteLine("DirectShow filter graph initialized.");
        }

        private static bool AddCaptureDevice(DsDevice[] devices, int deviceIndex)
        {
            if (devices.Length == 0 || deviceIndex < 0 || deviceIndex >= devices.Length)
                return false;

            try
            {
                // Use the specified device
                var device = devices[deviceIndex];
                Console.WriteLine($"Using capture device: {device.Name}");

                // Bind Moniker to a filter object
                Guid iid = typeof(IBaseFilter).GUID;
                device.Mon.BindToObject(null, null, ref iid, out object source);

                // Cast to IBaseFilter
                sourceVideo = (IBaseFilter)source;

                // Add filter to graph
                var hr = filterGraph.AddFilter(sourceVideo, "Video Capture Device");
                DsError.ThrowExceptionForHR(hr);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding capture device: {ex.Message}");
                return false;
            }
        }

        private static bool ConfigureVideoFormat()
        {
            try
            {
                // Get the output pin of the capture device
                IPin outputPin = FilterHelper.GetFreePinWithMediaType(sourceVideo, PinDirection.Output, MediaType.Video);
                if (outputPin == null)
                {
                    Console.WriteLine("Couldn't find video output pin.");
                    return false;
                }

                // Query for IAMStreamConfig interface
                IAMStreamConfig streamConfig = outputPin as IAMStreamConfig;
                if (streamConfig == null)
                {
                    Console.WriteLine("Couldn't get IAMStreamConfig interface.");
                    Marshal.ReleaseComObject(outputPin);
                    return false;
                }

                // Get available video formats
                List<string> videoFormats = new List<string>();
                List<string> frameRates = new List<string>();
                List<VFVideoCaptureFormat> videoFormatsObj = new List<VFVideoCaptureFormat>();

                MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();
                enumerator.GetVideoFormatsAndFrameRates(outputPin, ref videoFormats, ref videoFormatsObj, ref frameRates);

                Console.WriteLine("Available video formats:");
                for (int i = 0; i < videoFormats.Count; i++)
                {
                    Console.WriteLine($"  {i}: {videoFormats[i]}");
                }

                Console.WriteLine("Available frame rates:");
                for (int i = 0; i < frameRates.Count; i++)
                {
                    Console.WriteLine($"  {i}: {frameRates[i]}");
                }

                // Look for 1280x720 at 25 fps
                bool found = false;
                VFVideoCaptureFormat targetFormat = null;
                float targetFrameRate = 25.0f;

                for (int i = 0; i < videoFormatsObj.Count; i++)
                {
                    var format = videoFormatsObj[i];
                    if (format.Width == 1280 && format.Height == 720)
                    {
                        targetFormat = format;
                        Console.WriteLine($"Found compatible format: {format.Name}");
                        found = true;
                        break;
                    }
                }

                if (found && targetFormat != null)
                {
                    // Apply the format
                    bool result = DSHelper.ApplyVideoFormat(streamConfig, targetFormat, targetFrameRate);
                    Console.WriteLine($"Setting format to 1280x720 @ 25fps: {(result ? "Success" : "Failed")}");
                    
                    Marshal.ReleaseComObject(outputPin);
                    return result;
                }
                else
                {
                    Console.WriteLine("1280x720 format not found, using default format.");
                    Marshal.ReleaseComObject(outputPin);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting video format: {ex.Message}");
                return false;
            }
        }

        private static bool SetupVirtualCameraSink()
        {
            try
            {
                // Create the virtual camera sink filter
                virtualCamera = FilterGraphTools.AddFilterFromClsid(filterGraph, Consts.CLSID_VFVirtualCameraSink, "Virtual Camera Sink");
                
                if (virtualCamera != null)
                {
                    // If the filter has a license interface, set it
                    var cameraSink = virtualCamera as IVFVirtualCameraSink;
                    if (cameraSink != null)
                    {
                        cameraSink.set_license("YOUR_LICENSE_KEY"); // Replace with your license if needed
                    }
                    
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to create virtual camera sink filter.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up virtual camera sink: {ex.Message}");
                return false;
            }
        }

        private static bool ConnectFilters()
        {
            try
            {
                // Connect source to the virtual camera sink
                var hr = captureGraph.RenderStream(null, MediaType.Video, sourceVideo, null, virtualCamera);
                DsError.ThrowExceptionForHR(hr);
                
                Console.WriteLine("Filters connected successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting filters: {ex.Message}");
                return false;
            }
        }

        private static void ReleaseResources()
        {
            try
            {
                if (mediaControl != null)
                {
                    mediaControl.Stop();
                }

                // Release DirectShow interfaces
                if (sourceVideo != null)
                {
                    Marshal.ReleaseComObject(sourceVideo);
                    sourceVideo = null;
                }

                if (virtualCamera != null)
                {
                    Marshal.ReleaseComObject(virtualCamera);
                    virtualCamera = null;
                }

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

                if (captureGraph != null)
                {
                    Marshal.ReleaseComObject(captureGraph);
                    captureGraph = null;
                }

                if (filterGraph != null)
                {
                    Marshal.ReleaseComObject(filterGraph);
                    filterGraph = null;
                }

                Console.WriteLine("Resources released.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error releasing resources: {ex.Message}");
            }
        }
    }
}
