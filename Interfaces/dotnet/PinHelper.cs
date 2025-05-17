using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MediaFoundation;

namespace VisioForge.DirectShowAPI
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using DirectShowLib;

    public static class PinHelper
    {
        /// <summary>
        /// NLog logger.
        /// </summary>
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Disconnects filter pin.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        public static void DisconnectPin(this IPin pin)
        {
            if (pin == null)
            {
                return;
            }

            IPin pin2;
            pin.ConnectedTo(out pin2);
            if (pin2 != null)
            {
                pin.Disconnect();
                pin2.Disconnect();

                Marshal.ReleaseComObject(pin2);
                // ReSharper disable RedundantAssignment
                pin2 = null;
                // ReSharper restore RedundantAssignment
            }
        }

        /// <summary>
        /// Shows pin info while debugging.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        public static void DebugPinInfo(this IPin pin)
        {
            try
            {
                if (pin == null)
                {
                    return;
                }

                PinInfo pinInfo;
                pin.QueryPinInfo(out pinInfo);

                Debug.Print("Pin info:");
                Debug.Print("Name:" + pinInfo.name);

                if (pinInfo.dir == PinDirection.Input)
                {
                    Debug.Print("Direction: Input");
                }
                else
                {
                    Debug.Print("Direction: Output");
                }

                DsUtils.FreePinInfo(ref pinInfo);
            }
            catch
            {
            }
        }

        ///// <summary>
        ///// Gets filter output formats. 
        ///// </summary>
        ///// <param name="pin">
        ///// Pin.
        ///// </param>
        ///// <param name="mediaType">
        ///// Media type.
        ///// </param>
        ///// <param name="formats">
        ///// Formats (string list).
        ///// </param>
        ///// <param name="formatsObj">
        ///// Formats (objects).
        ///// </param>
        ///// <param name="videoFrameRates">
        ///// Video frame rates.
        ///// </param>
        //public static void GetFilterOutputFormats(
        //    this IPin pin,
        //    Guid mediaType,
        //    out List<string> formats,
        //    out List<VFVideoCaptureFormat> formatsObj,
        //    out List<string> videoFrameRates)
        //{
        //    formats = new List<string>();
        //    videoFrameRates = new List<string>();
        //    formatsObj = new List<VFVideoCaptureFormat>();

        //    if (pin == null)
        //    {
        //        Logger?.Error("Unable to get pin to read formats.");
        //    }
        //    else if (mediaType == MediaType.Video)
        //    {
        //        try
        //        {
        //            MediaFormatsEnumerator enumerator = new MediaFormatsEnumerator();

        //            enumerator.GetVideoFormatsAndFrameRates(pin, ref formats, ref formatsObj, ref videoFrameRates);
        //        }
        //        catch
        //        {
        //            Logger?.Error("Unable to read frame rates and video formats.");
        //        }
        //    }
        //    else if (mediaType == MediaType.Audio)
        //    {
        //        VFEnumMediaType audioMediaTypes = new VFEnumMediaType();
        //        audioMediaTypes.Assign(pin);

        //        formats.Clear();

        //        for (int i = 0; i < audioMediaTypes.Count; i++)
        //        {
        //            formats.Add(audioMediaTypes.MediaDescription(i));
        //        }
        //    }
        //}

        /// <summary>
        /// Gets pin connection status.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool IsPinConnected(this IPin pin)
        {
            IPin connectedPin;
            pin.ConnectedTo(out connectedPin);

            if (connectedPin != null)
            {
                Marshal.ReleaseComObject(connectedPin);
                return true;
            }

            return false;
        }

        ///// <summary>
        ///// Gets pin audio info.
        ///// </summary>
        ///// <param name="pin">
        ///// Pin.
        ///// </param>
        ///// <param name="channels">
        ///// Channels.
        ///// </param>
        ///// <param name="sampleRate">
        ///// Sample rate.
        ///// </param>
        ///// <param name="bps">
        ///// BPS.
        ///// </param>
        ///// <param name="ieee">
        ///// True if float.
        ///// </param>
        ///// <returns>
        ///// Returns true if operation successful.
        ///// </returns>
        //public static bool GetPinAudioInfo(
        //    this IPin pin,
        //    out int channels,
        //    out int sampleRate,
        //    out int bps,
        //    out bool ieee)
        //{
        //    channels = 0;
        //    bps = 0;
        //    sampleRate = 0;
        //    ieee = false;

        //    VFEnumMediaType mts = new VFEnumMediaType(pin);

        //    for (int i = 0; i < mts.Count; i++)
        //    {
        //        AMMediaType mt = mts.Items(i);

        //        if ((mt.formatSize > 0) && (mt.formatPtr != IntPtr.Zero))
        //        {
        //            if (mt.formatType == FormatType.WaveEx)
        //            {
        //                WaveFormatEx header = (WaveFormatEx)Marshal.PtrToStructure(mt.formatPtr, typeof(WaveFormatEx));
        //                sampleRate = header.nSamplesPerSec;
        //                bps = header.wBitsPerSample;
        //                channels = header.nChannels;

        //                if (mt.subType == MediaSubType.IEEE_FLOAT)
        //                {
        //                    ieee = true;
        //                }

        //                mts.Clear();

        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// Gets pin audio info.
        ///// </summary>
        ///// <param name="pin">
        ///// Pin.
        ///// </param>
        ///// <param name="channels">
        ///// Channels.
        ///// </param>
        ///// <param name="sampleRate">
        ///// Sample rate.
        ///// </param>
        ///// <param name="bps">
        ///// BPS.
        ///// </param>
        ///// <returns>
        ///// Returns true if operation successful.
        ///// </returns>
        //public static bool GetPinAudioInfo(this IPin pin, out int channels, out int sampleRate, out int bps)
        //{
        //    bool ieee;
        //    return GetPinAudioInfo(pin, out channels, out sampleRate, out bps, out ieee);
        //}

        /// <summary>
        /// Gets pin audio info.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="wave">
        /// Audio info header.
        /// </param>
        /// <returns>
        /// Returns true if operation successful.
        /// </returns>
        public static bool GetPinAudioInfo(this IPin pin, out WaveFormatEx wave)
        {
            wave = null;

            VFEnumMediaType mts = new VFEnumMediaType(pin);

            for (int i = 0; i < mts.Count; i++)
            {
                AMMediaType mt = mts.Items(i);

                if ((mt.formatSize > 0) && (mt.formatPtr != IntPtr.Zero))
                {
                    if (mt.formatType == FormatType.WaveEx)
                    {
                        wave = (WaveFormatEx)Marshal.PtrToStructure(mt.formatPtr, typeof(WaveFormatEx));

                        mts.Clear();

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets pin video info.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <returns>
        /// Returns true if operation successful.
        /// </returns>
        public static bool GetPinVideoInfo(this IPin pin, out int width, out int height)
        {
            float frameRate;
            return GetPinVideoInfo(pin, out width, out height, out frameRate);
        }

        /// <summary>
        /// Gets pin video info.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="frameRate">
        /// Frame rate.
        /// </param>
        /// <returns>
        /// Returns true if operation successful.
        /// </returns>
        public static bool GetPinVideoInfo(this IPin pin, out int width, out int height, out double frameRate)
        {
            var value = GetPinVideoInfo(pin, out width, out height, out float frameRate1);
            frameRate = frameRate1;
            return value;
        }

        /// <summary>
        /// Gets pin video info.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="frameRate">
        /// Frame rate.
        /// </param>
        /// <returns>
        /// Returns true if operation successful.
        /// </returns>
        public static bool GetPinVideoInfo(this IPin pin, out int width, out int height, out float frameRate)
        {
            width = 0;
            height = 0;
            frameRate = 0;

            var mts = new VFEnumMediaType(pin);

            for (int i = 0; i < mts.Count; i++)
            {
                var mt = mts.Items(i);

                if ((mt.formatSize > 0) && (mt.formatPtr != IntPtr.Zero))
                {
                    if (mt.formatType == FormatType.VideoInfo)
                    {
                        var header = (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));
                        width = header.BmiHeader.Width;
                        height = Math.Abs(header.BmiHeader.Height);
                        frameRate = (float)(10000000.0 / header.AvgTimePerFrame);

                        mts.Clear();

                        return true;
                    }
                }
            }

            return false;
        }

        ///// <summary>
        ///// Gets pin video info.
        ///// </summary>
        ///// <param name="pin">
        ///// Pin.
        ///// </param>
        ///// <param name="width">
        ///// Width.
        ///// </param>
        ///// <param name="height">
        ///// Height.
        ///// </param>
        ///// <param name="frameRateNum">
        ///// Frame rate num.
        ///// </param>
        ///// <param name="frameRateDen">
        ///// Frame rate den.
        ///// </param>
        ///// <param name="frameDuration">
        ///// Frame duration.
        ///// </param>
        ///// <returns>
        ///// Returns true if operation successful.
        ///// </returns>
        //public static bool GetPinVideoInfo(this IPin pin, out int width, out int height, out int frameRateNum, out int frameRateDen, out long frameDuration)
        //{
        //    width = 0;
        //    height = 0;
        //    frameRateNum = 0;
        //    frameRateDen = 0;
        //    frameDuration = 0;

        //    VFEnumMediaType mts = new VFEnumMediaType(pin);

        //    for (int i = 0; i < mts.Count; i++)
        //    {
        //        AMMediaType mt = mts.Items(i);

        //        if ((mt.formatSize > 0) && (mt.formatPtr != IntPtr.Zero))
        //        {
        //            if (mt.formatType == FormatType.VideoInfo)
        //            {
        //                VideoInfoHeader header =
        //                    (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));
        //                width = header.BmiHeader.Width;
        //                height = Math.Abs(header.BmiHeader.Height);

        //                frameDuration = header.AvgTimePerFrame;
        //                MFExtern.MFAverageTimePerFrameToFrameRate(header.AvgTimePerFrame, out frameRateNum, out frameRateDen); 

        //                mts.Clear();

        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        /// <summary>
        /// Gets pin video info.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="videoInfoHeader">
        /// Video info header.
        /// </param>
        /// <returns>
        /// Returns true if operation successful.
        /// </returns>
        public static bool GetPinVideoInfo(this IPin pin, out VideoInfoHeader videoInfoHeader)
        {
            videoInfoHeader = null;

            VFEnumMediaType mts = new VFEnumMediaType(pin);

            for (int i = 0; i < mts.Count; i++)
            {
                AMMediaType mt = mts.Items(i);

                if ((mt.formatSize > 0) && (mt.formatPtr != IntPtr.Zero))
                {
                    if (mt.formatType == FormatType.VideoInfo)
                    {
                        videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));

                        mts.Clear();

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets pin video info.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="mediaType">
        /// Media type.
        /// </param>
        /// <returns>
        /// Returns true if operation successful.
        /// </returns>
        public static bool GetPinVideoInfo(this IPin pin, out AMMediaType mediaType)
        {
            mediaType = null;

            if (pin == null)
            {
                return false;
            }

            IAMStreamConfig streamConfig = pin as IAMStreamConfig;
            if (streamConfig != null)
            {
                streamConfig.GetFormat(out mediaType);

                return mediaType != null;
            }
            else
            {
                VFEnumMediaType mte = new VFEnumMediaType(pin);
                if (mte.Count > 0)
                {
                    mediaType = mte.Items(0);
                    mte.ClearExceptFirst();
                }
            }

            return false;
        }


        /// <summary>
        /// Gets pin state.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <returns>
        /// Returns true if pin connected.
        /// </returns>
        public static bool PinConnected(this IPin pin)
        {
            if (pin == null)
            {
                return false;
            }

            pin.ConnectedTo(out var pin2);
            if (pin2 != null)
            {
                Marshal.ReleaseComObject(pin2);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets connected pin.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <returns>
        /// Returns connected pin.
        /// </returns>
        public static IPin PinConnectedTo(this IPin pin)
        {
            IPin pin2;

            if (pin == null)
            {
                return null;
            }

            pin.ConnectedTo(out pin2);
            return pin2;
        }

        /// <summary>
        /// Checks that pin have specified media type. 
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="mediaType">
        /// Media type.
        /// </param>
        /// <param name="subtype">
        /// Sub type.
        /// </param>
        /// <param name="ignoreSubtype">
        /// True to ignore subtype.
        /// </param>
        /// <returns>
        /// Returns true in pin have media type.
        /// </returns>
        public static bool PinHaveThisType(this IPin pin, Guid mediaType, Guid subtype, bool ignoreSubtype = false)
        {
            bool result;
            AMMediaType[] mt = new AMMediaType[1];

            if (pin == null)
            {
                return false;
            }

            try
            {
                result = false;

                IEnumMediaTypes mts;
                pin.EnumMediaTypes(out mts);

                if (mts == null)
                {
                    return false;
                }

                mts.Reset();

                if (subtype == Guid.Empty)
                {
                    while (mts.Next(1, mt, IntPtr.Zero) == 0)
                    {
                        if (mt[0] != null)
                        {
                            if (mt[0].majorType == mediaType)
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (mts.Next(1, mt, IntPtr.Zero) == 0)
                    {
                        if (mt[0] != null)
                        {
                            if ((mt[0].majorType == mediaType) && ((mt[0].subType == subtype) || ignoreSubtype))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }

                Marshal.ReleaseComObject(mts);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Checks that pin have specified media type. 
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="mediaType">
        /// Media type.
        /// </param>
        /// <returns>
        /// Returns true in pin have media type.
        /// </returns>
        public static bool PinHaveThisType(this IPin pin, Guid mediaType)
        {
            bool result;
            AMMediaType[] mt = new AMMediaType[1];

            if (pin == null)
            {
                return false;
            }

            try
            {
                result = false;

                IEnumMediaTypes mts;
                pin.EnumMediaTypes(out mts);

                if (mts == null)
                {
                    return false;
                }

                mts.Reset();

                while (mts.Next(1, mt, IntPtr.Zero) == 0)
                {
                    if (mt[0] != null)
                    {
                        if (mt[0].majorType == mediaType)
                        {
                            result = true;
                            break;
                        }
                    }
                }

                Marshal.ReleaseComObject(mts);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Checks that pin have specified media type. 
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        /// <param name="mediaType">
        /// Media type.
        /// </param>
        /// <returns>
        /// Returns true in pin have media type.
        /// </returns>
        public static bool PinHaveThisSubType(this IPin pin, Guid mediaType)
        {
            bool result;
            AMMediaType[] mt = new AMMediaType[1];

            if (pin == null)
            {
                return false;
            }

            try
            {
                result = false;

                IEnumMediaTypes mts;
                pin.EnumMediaTypes(out mts);

                if (mts == null)
                {
                    return false;
                }

                mts.Reset();

                while (mts.Next(1, mt, IntPtr.Zero) == 0)
                {
                    if (mt[0] != null)
                    {
                        if (mt[0].subType == mediaType)
                        {
                            result = true;
                            break;
                        }
                    }
                }

                Marshal.ReleaseComObject(mts);
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
