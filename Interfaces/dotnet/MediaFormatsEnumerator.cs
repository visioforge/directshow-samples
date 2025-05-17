using VisioForge.DirectShowLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Media formats enumerator.
    /// </summary>
    public class MediaFormatsEnumerator
    {
        /// <summary>
        /// Resolution list.
        /// </summary>
        private readonly List<Size> FResolutions;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFormatsEnumerator"/> class.
        /// </summary>
        public MediaFormatsEnumerator()
        {
            FResolutions = new List<Size>
                {
                    new Size(160, 120),
                    new Size(176, 144),
                    new Size(240, 176),
                    new Size(240, 180),
                    new Size(320, 240),
                    new Size(352, 240),
                    new Size(352, 288),
                    new Size(640, 288),
                    new Size(640, 480),
                    new Size(704, 576),
                    new Size(720, 240),
                    new Size(720, 288),
                    new Size(720, 480),
                    new Size(720, 576),
                    new Size(768, 576),
                    new Size(1280, 720),
                    new Size(1400, 1080),
                    new Size(1440, 1080),
                    new Size(1920, 1080)
                };
        }

        /// <summary>
        /// Adds frame rates to the list.
        /// </summary>
        /// <param name="caps">
        /// Stream config caps.
        /// </param>
        /// <param name="frameRates">
        /// Frame rates.
        /// </param>
        /// <param name="inverse">
        /// Inverse max/min frame rate.
        /// </param>
        private static void AddFrameRatesToList(VideoStreamConfigCaps caps, ref List<string> frameRates, bool inverse)
        {
            try
            {
                int max;
                int min;

                if (inverse)
                {
                    max = (int)Math.Round(1000.0 / (caps.MinFrameInterval / 10000.0));
                    min = (int)Math.Round(1000.0 / (caps.MaxFrameInterval / 10000.0));
                }
                else
                {
                    min = (int)Math.Round(1000.0 / (caps.MinFrameInterval / 10000.0));
                    max = (int)Math.Round(1000.0 / (caps.MaxFrameInterval / 10000.0));
                }

                if (min <= 0)
                {
                    min = 1;
                }

                if (max >= 100)
                {
                    max = 100;
                }

                if ((max <= 29) && (min >= 30))
                {
                    for (int j = min; j <= 29; j++)
                    {
                        frameRates.Add(j.ToString());
                    }

                    const double fr2997 = 29.97;
                    // ReSharper disable LocalizableElement
                    frameRates.Add(fr2997.ToString("F2"));
                    // ReSharper restore LocalizableElement
                    for (int j = 30; j <= max; j++)
                    {
                        if (!frameRates.Contains(j.ToString()))
                        {
                            frameRates.Add(j.ToString());
                        }
                    }
                }
                else
                {
                    for (int j = min; j <= max; j++)
                    {
                        if (!frameRates.Contains(j.ToString()))
                        {
                            frameRates.Add(j.ToString());
                        }
                    }
                }
            }
            catch
            {
                //Logger.Debug(Resources.Unable_to_get_frame_rate_list);
            }
        }

        /// <summary>
        /// Checks that video format supported.
        /// </summary>
        /// <param name="caps">
        /// Stream config caps.
        /// </param>
        /// <param name="width">
        /// Video width.
        /// </param>
        /// <param name="height">
        /// Video height.
        /// </param>
        /// <returns>
        /// Returns true if video format supported.
        /// </returns>
        private static bool VideoFormatSupported(VideoStreamConfigCaps caps, int width, int height)
        {
            try
            {
                if (width > caps.MaxOutputSize.Width || width < caps.MinOutputSize.Width ||
                    height > caps.MaxOutputSize.Height || height < caps.MinOutputSize.Height)
                {
                    return false;
                }

                //if (width % caps.OutputGranularityX != 0 || height % caps.OutputGranularityY != 0)
                //{
                //    return false;
                //}
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds video formats to the list.
        /// </summary>
        /// <param name="streamConfigCaps">
        /// Stream config caps.
        /// </param>
        /// <param name="mediaType">
        /// Media type.
        /// </param>
        /// <param name="videoFormatsObj">
        /// Video formats objects.
        /// </param>
        /// <param name="videoFormats">
        /// Video formats.
        /// </param>
        private void AddVideoFormatsToList(
            VideoStreamConfigCaps streamConfigCaps,
            AMMediaType mediaType,
            ref List<VFVideoCaptureFormat> videoFormatsObj,
            ref List<string> videoFormats)
        {
            try
            {
                if (mediaType.formatType == FormatType.VideoInfo)
                {
                    // default videoCaptureFormat

                    // ReSharper disable UseObjectOrCollectionInitializer
                    VFVideoCaptureFormat capx = new VFVideoCaptureFormat
                    {
                        MajorType = mediaType.majorType,
                        SubType = mediaType.subType,
                        FormatType = mediaType.formatType,
                        SampleSize = mediaType.sampleSize,
                        FixedSizeSamples = mediaType.fixedSizeSamples,
                        TemporalCompression = mediaType.temporalCompression
                    };
                    // ReSharper restore UseObjectOrCollectionInitializer

                    capx.VIH = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));

                    capx.Width = capx.VIH.BmiHeader.Width;
                    capx.Height = Math.Abs(capx.VIH.BmiHeader.Height);
                    capx.Name = MediaTypeHelper.GetMediaTypeDescription(capx);

                    videoFormatsObj.Add(capx);
                    videoFormats.Add(capx.Name);

                    //// other caps
                    //foreach (Size size in FResolutions)
                    //{
                    //    if (VideoFormatSupported(streamConfigCaps, size.Width, size.Height))
                    //    {
                    //        // ReSharper disable UseObjectOrCollectionInitializer
                    //        VFVideoCaptureFormat cap = new VFVideoCaptureFormat
                    //            {
                    //                MajorType = mediaType.majorType,
                    //                SubType = mediaType.subType,
                    //                FormatType = mediaType.formatType,
                    //                SampleSize = mediaType.sampleSize,
                    //                FixedSizeSamples = mediaType.fixedSizeSamples,
                    //                TemporalCompression = mediaType.temporalCompression
                    //            };
                    //        // ReSharper restore UseObjectOrCollectionInitializer

                    //        // bug

                    //        cap.VIH = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));
                    //        cap.VIH.BmiHeader.Width = size.Width;
                    //        cap.VIH.BmiHeader.Height = size.Height;



                    //        if (cap.VIH.SrcRect.Right >= 0)
                    //        {
                    //            cap.VIH.SrcRect.Right = size.Width;
                    //            cap.VIH.TargetRect.Right = size.Width;
                    //        }
                    //        else
                    //        {
                    //            cap.VIH.SrcRect.Right = -size.Width;
                    //            cap.VIH.TargetRect.Right = -size.Width;
                    //        }

                    //        if (cap.VIH.SrcRect.Bottom >= 0)
                    //        {
                    //            cap.VIH.SrcRect.Bottom = size.Height;
                    //            cap.VIH.TargetRect.Bottom = size.Height;
                    //        }
                    //        else
                    //        {
                    //            cap.VIH.SrcRect.Bottom = -size.Height;
                    //            cap.VIH.TargetRect.Bottom = -size.Height;
                    //        }

                    //        cap.Width = size.Width;
                    //        cap.Height = size.Height;
                    //        cap.Name = DSHelper.GetMediaTypeDescription(cap);

                    //        videoFormatsObj.Add(cap);
                    //        videoFormats.Add(cap.Name);
                    //    }
                    //}
                }
                //else if (videoCaptureFormat.FormatType == FormatType.VideoInfo2)
                //{
                //    videoCaptureFormat.VIH2 = (VideoInfoHeader2)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader2));
                //    videoCaptureFormat.Width = videoCaptureFormat.VIH2.BmiHeader.Width;
                //    videoCaptureFormat.Height = videoCaptureFormat.VIH2.BmiHeader.Height;
                //}
                //else if (videoCaptureFormat.FormatType == FormatType.DvInfo)
                //{
                //    videoCaptureFormat.DV = (DVInfo)Marshal.PtrToStructure(mediaType.formatPtr, typeof(DVInfo));
                //}
                //else if (videoCaptureFormat.FormatType == FormatType.MpegVideo)
                //{
                //    videoCaptureFormat.MPEG1 = (MPEG1VideoInfo)Marshal.PtrToStructure(mediaType.formatPtr, typeof(MPEG1VideoInfo));
                //    videoCaptureFormat.Width = videoCaptureFormat.MPEG1.HDR.BmiHeader.Width;
                //    videoCaptureFormat.Height = videoCaptureFormat.MPEG1.HDR.BmiHeader.Height;
                //}
                //else if (videoCaptureFormat.FormatType == FormatType.Mpeg2Video)
                //{
                //    videoCaptureFormat.MPEG2 = (MPEG2VideoInfo)Marshal.PtrToStructure(mediaType.formatPtr, typeof(MPEG2VideoInfo));
                //    videoCaptureFormat.Width = videoCaptureFormat.MPEG2.HDR.BmiHeader.Width;
                //    videoCaptureFormat.Height = videoCaptureFormat.MPEG2.HDR.BmiHeader.Height;
                //}
            }
            catch
            {
                //Logger.Debug(Resources.Unable_to_get_formats_list);
            }
        }

        /// <summary>
        /// gets video formats and frame rates.
        /// </summary>
        /// <param name="outputPin">
        /// Output pin.
        /// </param>
        /// <param name="videoFormats">
        /// Video formats.
        /// </param>
        /// <param name="videoFormatsObj">
        /// Video formats objects.
        /// </param>
        /// <param name="frameRates">
        /// Frame rates.
        /// </param>
        /// <returns>
        /// Returns true if the operation was successful.
        /// </returns>
        public bool GetVideoFormatsAndFrameRates(
            IPin outputPin,
            ref List<string> videoFormats,
            ref List<VFVideoCaptureFormat> videoFormatsObj,
            ref List<string> frameRates)
        {
            try
            {
                if (videoFormats == null)
                {
                    return false;
                }

                videoFormats.Clear();

                if (outputPin == null)
                {
                    return false;
                }

                IAMStreamConfig streamConfig = outputPin as IAMStreamConfig;

                if (streamConfig != null)
                {
                    int size;
                    int count;
                    streamConfig.GetNumberOfCapabilities(out count, out size);

                    if (Marshal.SizeOf(typeof(VideoStreamConfigCaps)) == size)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            IntPtr capsP = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VideoStreamConfigCaps)));

                            AMMediaType mt;

                            streamConfig.GetStreamCaps(i, out mt, capsP);

                            VideoStreamConfigCaps caps =
                                (VideoStreamConfigCaps)Marshal.PtrToStructure(capsP, typeof(VideoStreamConfigCaps));

                            AddFrameRatesToList(caps, ref frameRates, false);
                            AddFrameRatesToList(caps, ref frameRates, true);
                            AddVideoFormatsToList(caps, mt, ref videoFormatsObj, ref videoFormats);

                            Marshal.FreeCoTaskMem(capsP);
                        }

                        frameRates = DSHelper.RemoveDuplicateStrings(frameRates);
                        RemoveVideoFormatsDuplicates(ref videoFormats, ref videoFormatsObj);
                    }
                }
            }
            catch
            {
            }

            return true;
        }

        /// <summary>
        /// Remover duplicates from video formats list.
        /// </summary>
        /// <param name="videoFormats">
        /// Video formats.
        /// </param>
        /// <param name="videoFormatsObj">
        /// Video formats objects.
        /// </param>
        private static void RemoveVideoFormatsDuplicates(
            ref List<string> videoFormats, ref List<VFVideoCaptureFormat> videoFormatsObj)
        {
            Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
            List<string> videoFormats2 = new List<string>();
            List<VFVideoCaptureFormat> videoFormatsObj2 = new List<VFVideoCaptureFormat>();

            int i = 0;
            foreach (string currValue in videoFormats)
            {
                if (!uniqueStore.ContainsKey(currValue))
                {
                    uniqueStore.Add(currValue, 0);
                    videoFormats2.Add(videoFormats[i]);
                    videoFormatsObj2.Add(videoFormatsObj[i]);
                }

                i++;
            }

            videoFormats = videoFormats2;
            videoFormatsObj = videoFormatsObj2;
        }

        /// <summary>
        /// Removes all filters downstream from a filter from the graph.
        /// </summary>
        /// <param name="filterGraph">
        /// Filter Graph.
        /// </param>
        /// <param name="filter">
        /// Filter.
        /// </param>
        public static void RemoveDownstreamFilters(IFilterGraph2 filterGraph, IBaseFilter filter)
        {
            if (filter == null)
            {
                return;
            }

            // Get a pin enumerator off the filter
            IEnumPins pinEnum;
            int hr = filter.EnumPins(out pinEnum);

            if (pinEnum == null || hr == 0)
            {
                return;
            }

            pinEnum.Reset();

            // Loop through each pin
            IPin[] pins = new IPin[1];
            IntPtr f = IntPtr.Zero;

            do
            {
                // Get the next pin
                hr = pinEnum.Next(1, pins, f);

                if ((hr == 0) && (pins[0] != null))
                {
                    // Get the pin it is connected to
                    IPin pinTo;
                    pins[0].ConnectedTo(out pinTo);

                    if (pinTo != null)
                    {
                        // Is this an input pin?
                        PinInfo info;
                        hr = pinTo.QueryPinInfo(out info);

                        if ((hr == 0) && (info.dir == PinDirection.Input))
                        {
                            // Recurse down this branch
                            RemoveDownstreamFilters(filterGraph, info.filter);

                            // Disconnect 
                            filterGraph.Disconnect(pinTo);
                            filterGraph.Disconnect(pins[0]);

                            // Remove this filter
                            filterGraph.RemoveFilter(info.filter);
                        }

                        if (info.filter != null)
                        {
                            Marshal.ReleaseComObject(info.filter);
                        }

                        Marshal.ReleaseComObject(pinTo);
                    }

                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            while (hr == 0);

            // ReSharper disable RedundantAssignment
            if (pinEnum != null)
            {
                Marshal.ReleaseComObject(pinEnum);
                pinEnum = null;
            }
            // ReSharper restore RedundantAssignment
        }
    }
}
