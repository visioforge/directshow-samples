using System;
using System.Collections.Generic;
using System.Text;

namespace VisioForge.DirectShowAPI  
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// FFMPEG output settings.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FFMPEGOutputSettings
    {
        /// <summary>
        /// File name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Filename;

        /// <summary>
        /// True to have audio stream.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool AudioAvailable;

        /// <summary>
        /// Audio bitrate.
        /// </summary>
        public int AudioBitrate;

        /// <summary>
        /// Audio sample rate.
        /// </summary>
        public int AudioSamplerate;

        /// <summary>
        /// Audio channels.
        /// </summary>
        public int AudioChannels;

        /// <summary>
        /// Video width.
        /// </summary>
        public int VideoWidth;

        /// <summary>
        /// Video height.
        /// </summary>
        public int VideoHeight;

        /// <summary>
        /// Aspect ratio (W).
        /// </summary>
        public int AspectRatioW;

        /// <summary>
        /// Aspect ratio (H).
        /// </summary>
        public int AspectRatioH;

        /// <summary>
        /// Video bitrate.
        /// </summary>
        public int VideoBitrate;

        /// <summary>
        /// Video max bitrate.
        /// </summary>
        public int VideoMaxRate;

        /// <summary>
        /// Video min bitrate.
        /// </summary>
        public int VideoMinRate;

        /// <summary>
        /// Video buffer size.
        /// </summary>
        public int VideoBufferSize;

        /// <summary>
        /// True to use interlacing.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool Interlace;

        /// <summary>
        /// Video GOP size.
        /// </summary>
        public int VideoGopSize;

        /// <summary>
        /// TV system.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public VFFFMPEGDLLTVSystem TVSystem;

        /// <summary>
        /// Output format.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public VFFFMPEGDLLOutputFormat OutputFormat;
    }

    /// <summary>
    /// FFMPEG encoder interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("17B8FF7D-A67F-45CE-B425-0E4F607D8C60")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFFFMPEGEncoder
    {
        /// <summary>
        /// Sets FFMPEG encoder settings.
        /// </summary>
        /// <param name="settings">
        /// Settings.
        /// </param>
        [PreserveSig]
        void set_settings([In] FFMPEGOutputSettings settings);
    }

    /// <summary>
    /// FFMPEG DLL output format.
    /// </summary>
    public enum VFFFMPEGDLLOutputFormat
    {
        /// <summary>
        /// Flash video.
        /// </summary>
        FLV,

        /// <summary>
        /// MPEG-1.
        /// </summary>
        MPEG1,

        /// <summary>
        /// MPEG-1 VCD.
        /// </summary>
        MPEG1VCD,

        /// <summary>
        /// MPEG-2.
        /// </summary>
        MPEG2,

        /// <summary>
        /// MPEG-2 Transport Stream.
        /// </summary>
        MPEG2TS,

        /// <summary>
        /// MPEG-2 SVCD.
        /// </summary>
        MPEG2SVCD,

        /// <summary>
        /// MPEG-2 DVD.
        /// </summary>
        MPEG2DVD,

        ///// <summary>
        ///// MPEG-4 + AAC.
        ///// </summary>
        // MPEG4AAC,

        ///// <summary>
        ///// MPEG-4 + MP3.
        ///// </summary>
        // MPEG4MP3
    }

    /// <summary>
    /// FFMPEG DLL TV System.
    /// </summary>
    public enum VFFFMPEGDLLTVSystem
    {
        /// <summary>
        /// Unknown or default.
        /// </summary>
        None,

        /// <summary>
        /// PAL.
        /// </summary>
        PAL,

        /// <summary>
        /// NTSC.
        /// </summary>
        NTSC,

        /// <summary>
        /// Film.
        /// </summary>
        Film
    }

}
