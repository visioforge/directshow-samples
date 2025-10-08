using System;
using System.Collections.Generic;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Speex encode mode.
    /// </summary>
    public enum SpeexEncodeMode
    {
        /// <summary>
        /// Auto.
        /// </summary>
        Auto,

        /// <summary>
        /// Narrowband.
        /// </summary>
        Narrowband,

        /// <summary>
        /// Wideband.
        /// </summary>
        Wideband,

        /// <summary>
        /// Ultra-wideband.
        /// </summary>
        UltraWideband,
    }

    /// <summary>
    /// Speex bitrate control.
    /// </summary>
    public enum SpeexBitrateControl
    {
        /// <summary>
        /// VBR quality.
        /// </summary>
        VBRQuality,

        /// <summary>
        /// VBR bitrate.
        /// </summary>
        VBRBitrate,

        /// <summary>
        /// CBR quality.
        /// </summary>
        CBRQuality,

        /// <summary>
        /// CBR bitrate.
        /// </summary>
        CBRBitrate,

        /// <summary>
        /// ABR.
        /// </summary>
        ABR
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("479038D2-57FF-41ee-B397-FB98199BF1E8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISpeexEncodeSettings
    {
        [PreserveSig]
        object getEncoderSettings();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setMode(SpeexEncodeMode inMode);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setComplexity(int inComplexity);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setupVBRQualityMode(int inQuality, int inVBRMaxBitrate);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setupVBRBitrateMode(int inBitrate, int inVBRMaxBitrate);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setupABR(int inABRBitrate);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setupCBRBitrateMode(int inCBRBitrate);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setupCBRQualityMode(int inQuality);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setEncodingFlags(
            [MarshalAs(UnmanagedType.Bool)] bool inUseDTX,
            [MarshalAs(UnmanagedType.Bool)] bool inUseVAD,
            [MarshalAs(UnmanagedType.Bool)] bool inUseAGC,
            [MarshalAs(UnmanagedType.Bool)] bool inUseDenoise);
    }
}
