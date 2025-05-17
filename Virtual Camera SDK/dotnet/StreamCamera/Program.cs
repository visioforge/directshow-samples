using System;
using VisioForge.DirectShowLib;

namespace StreamCamera
{
    internal class Program
    {
        private IFilterGraph2 filterGraph;

        private ICaptureGraphBuilder2 captureGraph;

        private IMediaControl mediaControl;

        private IMediaEventEx mediaEventEx;

        private IBaseFilter sourceVideo;

        private IBaseFilter virtualCamera;

        static void Main(string[] args)
        {
            Console.WriteLine("Enumerate video source devices:");

            // Get all video / audio input devices
            var videoCaptureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);


        }
    }
}
