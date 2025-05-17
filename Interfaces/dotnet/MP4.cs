using System;
using System.Collections.Generic;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    using DirectShowLib;

    /// <summary>
    /// NVENC profiles.
    /// </summary>
    public enum NVENCProfile
    {
        /// <summary>
        /// Auto.
        /// </summary>
        Auto,

        /// <summary>
        /// H264 Baseline.
        /// </summary>
        H264_Baseline,

        /// <summary>
        /// H264 Main.
        /// </summary>
        H264_Main,

        /// <summary>
        /// H264 High.
        /// </summary>
        H264_High,

        /// <summary>
        /// H264 High 4:4:4.
        /// </summary>
        H264_High444,

        /// <summary>
        /// H264 Stereo.
        /// </summary>
        H264_Stereo,

        /// <summary>
        /// H264 SVC Temporal Scalabilty.
        /// </summary>
        H264_SVCTemporalScalabilty,

        /// <summary>
        /// H264 Progressive High.
        /// </summary>
        H264_ProgressiveHigh,

        /// <summary>
        /// H264 Constrained High.
        /// </summary>
        H264_ConstrainedHigh,

        /// <summary>
        /// HEVC Main. 
        /// </summary>
        HEVC_Main
    }

    /// <summary>
    /// NVENC rate control.
    /// </summary>
    public enum VFNVENCRateControlMode
    {
        /// <summary>
        /// Constant QP mode.
        /// </summary>
        CONST_QP = 0x0,

        /// <summary>
        /// Variable bitrate mode.
        /// </summary>
        VBR = 0x1,

        /// <summary>
        /// Constant bitrate mode.
        /// </summary>     
        CBR = 0x2,
    }

    /// <summary>
    /// NVENC codec.
    /// </summary>
    public enum VFNVENCCodec
    {
        /// <summary>
        /// H264.
        /// </summary>
        H264 = 0,

        /// <summary>
        /// HEVC.
        /// </summary>
        HEVC = 1,
    };

    /// <summary>
    /// NVENC profile level for H264/HEVC.
    /// </summary>
    public enum VFNVENCLevel
    {
        /// <summary>
        /// Auto.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// H264 1.0.
        /// </summary>
        H264_1 = 10,

        /// <summary>
        /// H264 1.1.
        /// </summary>
        H264_11 = 11,

        /// <summary>
        /// H264 1.2.
        /// </summary>
        H264_12 = 12,

        /// <summary>
        /// H264 1.3.
        /// </summary>
        H264_13 = 13,

        /// <summary>
        /// H264 2.0.
        /// </summary>
        H264_2 = 20,

        /// <summary>
        /// H264 2.1.
        /// </summary>
        H264_21 = 21,

        /// <summary>
        /// H264 2.2.
        /// </summary>
        H264_22 = 22,

        /// <summary>
        /// H264 3.0.
        /// </summary>
        H264_3 = 30,

        /// <summary>
        /// H264 3.1.
        /// </summary>
        H264_31 = 31,

        /// <summary>
        /// H264 3.2.
        /// </summary>
        H264_32 = 32,

        /// <summary>
        /// H264 4.0.
        /// </summary>
        H264_4 = 40,

        /// <summary>
        /// H264 4.1.
        /// </summary>
        H264_41 = 41,

        /// <summary>
        /// H264 4.2.
        /// </summary>
        H264_42 = 42,

        /// <summary>
        /// H264 5.0.
        /// </summary>
        H264_5 = 50,

        /// <summary>
        /// H264 5.1.
        /// </summary>
        H264_51 = 51,

        /// <summary>
        /// H264 5.2.
        /// </summary>
        H264_52 = 52,

        /// <summary>
        /// HEVC 1.0.
        /// </summary>
        HEVC_1 = 30,

        /// <summary>
        /// HEVC 2.0.
        /// </summary>
        HEVC_2 = 60,

        /// <summary>
        /// HEVC 2.1.
        /// </summary>
        HEVC_21 = 63,

        /// <summary>
        /// HEVC 3.0.
        /// </summary>
        HEVC_3 = 90,

        /// <summary>
        /// HEVC 3.1.
        /// </summary>
        HEVC_31 = 93,

        /// <summary>
        /// HEVC 4.0.
        /// </summary>
        HEVC_4 = 120,

        /// <summary>
        /// HEVC 4.1.
        /// </summary>
        HEVC_41 = 123,

        /// <summary>
        /// HEVC 5.0.
        /// </summary>
        HEVC_5 = 150,

        /// <summary>
        /// HEVC 5.1.
        /// </summary>
        HEVC_51 = 153,

        /// <summary>
        /// HEVC 5.2.
        /// </summary>
        HEVC_52 = 156,

        /// <summary>
        /// HEVC 6.0.
        /// </summary>
        HEVC_6 = 180,

        /// <summary>
        /// HEVC 6.1.
        /// </summary>
        HEVC_61 = 183,

        /// <summary>
        /// HEVC 6.2.
        /// </summary>
        HEVC_62 = 186,

        /// <summary>
        /// Tier HEVC Main.
        /// </summary>
        TIER_HEVC_MAIN = 0,

        /// <summary>
        /// Tier HEVC High.
        /// </summary>
        TIER_HEVC_HIGH = 1
    }
    
    
    
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("09FA2EA3-4773-41a8-90DC-9499D4061E9F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IH264Encoder
    {
        [PreserveSig]
        int get_Bitrate([Out] out int plValue);

        [PreserveSig]
        int put_Bitrate([In] int lValue);

        [PreserveSig]
        int get_RateControl([Out] out int pValue);

        [PreserveSig]
        int put_RateControl([In] int value);

        [PreserveSig]
        int get_MbEncoding([Out] out int pValue);

        [PreserveSig]
        int put_MbEncoding([In] int value);

        [PreserveSig]
        int get_GOP([Out] [MarshalAs(UnmanagedType.Bool)] out bool pValue);

        [PreserveSig]
        int put_GOP([In] [MarshalAs(UnmanagedType.Bool)] bool value);

        [PreserveSig]
        int get_AutoBitrate([Out] [MarshalAs(UnmanagedType.Bool)] out bool pValue);

        [PreserveSig]
        int put_AutoBitrate([In] [MarshalAs(UnmanagedType.Bool)] bool value);

        [PreserveSig]
        int get_Profile([Out] out int pValue);

        [PreserveSig]
        int put_Profile([In] int value);

        [PreserveSig]
        int get_Level([Out] out int pValue);

        [PreserveSig]
        int put_Level([In] int value);

        [PreserveSig]
        int get_Usage([Out] out int pValue);

        [PreserveSig]
        int put_Usage([In] int value);

        [PreserveSig]
        int get_SequentialTiming([Out] out int pValue);

        [PreserveSig]
        int put_SequentialTiming([In] int value);

        [PreserveSig]
        int get_SliceIntervals([Out] out int piIDR, [Out] out int piP);

        [PreserveSig]
        int put_SliceIntervals([In] ref int piIDR, [In] ref int piP);

        [PreserveSig]
        int get_MaxBitrate([Out] out int plValue);

        [PreserveSig]
        int put_MaxBitrate([In] int lValue);

        [PreserveSig]
        int get_MinBitrate([Out] out int plValue);

        [PreserveSig]
        int put_MinBitrate([In] int lValue);
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("99DC9BE5-0AFA-45d4-8370-AB021FB07CF4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMP4MuxerConfig
    {
        [PreserveSig]
        int get_SingleThread([Out] [MarshalAs(UnmanagedType.Bool)] out bool pValue);

        [PreserveSig]
        int put_SingleThread([In] [MarshalAs(UnmanagedType.Bool)] bool value);

        [PreserveSig]
        int get_CorrectTiming([Out] [MarshalAs(UnmanagedType.Bool)] out bool pValue);

        [PreserveSig]
        int put_CorrectTiming([In] [MarshalAs(UnmanagedType.Bool)] bool value);
    }

    public enum IntelVideoEncoderProfile // profile values
    {
        PF_AUTOSELECT = 0,

        // H.264 values
        PF_H264_BASELINE = 66,
        PF_H264_MAIN = 77,
        PF_H264_HIGH = 100,
        PF_H264_HIGH10 = 110,
        PF_H264_HIGH422 = 122,

        // MPEG2 values
        PF_MPEG2_SIMPLE = 80,
        PF_MPEG2_MAIN = 64,
        PF_MPEG2_SNR = 3,
        PF_MPEG2_SPATIAL = 2,
        PF_MPEG2_HIGH = 16
    }

    public enum IntelVideoEncoderLevel // level values
    {
        LL_AUTOSELECT = 0,

        // H.264 values
        LL_1 = 10,
        LL_1b = 9,
        LL_11 = 11,
        LL_12 = 12,
        LL_13 = 13,
        LL_2 = 20,
        LL_21 = 21,
        LL_22 = 22,
        LL_3 = 30,
        LL_31 = 31,
        LL_32 = 32,
        LL_4 = 40,
        LL_41 = 41,
        LL_42 = 42,
        LL_5 = 50,
        LL_51 = 51,

        // MPEG2 values
        LL_LOW = 10,
        LL_MAIN = 8,
        LL_HIGH1440 = 6,
        LL_HIGH = 4
    }

    /* TargetUsages: from 1 to 7 inclusive */
    public enum IntelVideoEncoderTargetUsage
    {
        TARGETUSAGE_1 = 1,
        TARGETUSAGE_2 = 2,
        TARGETUSAGE_3 = 3,
        TARGETUSAGE_4 = 4,
        TARGETUSAGE_5 = 5,
        TARGETUSAGE_6 = 6,
        TARGETUSAGE_7 = 7,

        TARGETUSAGE_UNKNOWN = 0,
        TARGETUSAGE_BEST_QUALITY = TARGETUSAGE_1,
        TARGETUSAGE_BALANCED = TARGETUSAGE_4,
        TARGETUSAGE_BEST_SPEED = TARGETUSAGE_7
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntelVideoEncoderPSControl // picture sequence control
    {
        public uint GopPicSize;           // I-frame interval in frames
        public uint GopRefDist;           // Distance between I- or P- key frames;If GopRefDist = 1, there are no B-frames used
        public uint NumSlice;             // Number of slices
        public uint BufferSizeInKB;       // vbv buffer size
    }

    public enum IntelVideoEncoderPCControl // picture coding control
    {
        PC_FRAME = 1,
        PC_FIELD_TFF = 2,
        PC_FIELD_BFF = 4,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntelVideoEncoderFrameControl
    {
        public uint width;         // output frame width
        public uint height;        // output frame height
    }

    // See Requirement 34
    public enum IntelVideoEncoderThrottlePolicy
    {
        TT_NA,              // no throttle handling
        TT_AUTO,            // auto throttling
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntelVideoEncoderThrottleControl
    {
        public IntelVideoEncoderThrottlePolicy throttle_policy;

        public uint bitrate_up;       // range of bitrate increase adjustment.
        public uint bitrate_down;     // range of bitrate decrease adjustment.
        public uint qp_up;            // range of qp increase adjustment
        public uint qp_down;          // range of qp decrease adjustment
    }

    public enum IntelVideoEncoderRCMethod
    {
        RC_CBR = 1,                      // Constant Bitrate
        RC_VBR = 2,                      // Variable Bitrate
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntelVideoEncoderRCControl // rate control
    {
        public IntelVideoEncoderRCMethod rc_method;

        public uint bitrate;               // specify bit rate in bps
    }

    public enum IntelVideoEncoderPreset
    {
        // undefined preset
        PRESET_UNKNOWN = -1,

        // User Defined (by setting encoder parameters)
        PRESET_USER_DEFINED,

        // Optimal speed and quality
        PRESET_BALANCED,

        // Best quality setting
        PRESET_BEST_QUALITY,

        // Fast encoding with reasonable quality
        PRESET_FAST,

        PRESET_LOW_LATENCY
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntelVideoEncoderParams
    {
        public IntelVideoEncoderProfile profile_idc;

        public IntelVideoEncoderLevel level_idc;

        public IntelVideoEncoderPSControl ps_control;

        public IntelVideoEncoderPCControl pc_control;

        public IntelVideoEncoderFrameControl frame_control;

        public IntelVideoEncoderThrottleControl throttle_control;

        public IntelVideoEncoderRCControl rc_control;

        public IntelVideoEncoderPreset preset;

        public IntelVideoEncoderTargetUsage target_usage;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntelVideoEncoderStatistics
    {
        public uint struct_size;          // size of the Statistics structure
        public uint width;                // frame width
        public uint height;               // frame height
        public uint frame_rate;           // frame rate

        //struct aspect_ratio
        //{
        public uint horizontal;        // horizontal pixel aspect ratio
        public uint vertical;          // vertical pixel aspect ratio
        //}  

        // aspect ratio
        public uint real_bitrate;         // average bitrate
        public uint frames_encoded;       // number of frames encoded
        public uint requested_bitrate;    // requested bit rate
        public uint frames_received;      // number of frames received
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("2A483975-C5B2-4D1F-8DBB-23A653AF5E39")]
    //[Guid("6BABAF70-864B-486b-B471-CC4E9AFF931B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IIntelConfigureVideoEncoder
    {
        [PreserveSig]
        int SetParams([In] IntelVideoEncoderParams parameters);

        [PreserveSig]
        int GetParams([Out] out IntelVideoEncoderParams parameters);

        [PreserveSig]
        int GetRunTimeStatistics([Out] out IntelVideoEncoderStatistics statistics);

        [PreserveSig]
        int SetParams2([In] IntelVideoEncoderParams parameters);
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("299332CF-1791-4301-B043-F06FAF847C52")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IIntelConfigureVideoEncoderV9
    {
        [PreserveSig]
        int SetParams([In] IntelVideoEncoderParams parameters);

        [PreserveSig]
        int GetParams([Out] out IntelVideoEncoderParams parameters);

        [PreserveSig]
        int GetRunTimeStatistics([Out] out IntelVideoEncoderStatistics statistics);

        [PreserveSig]
        int SetParams2([In] IntelVideoEncoderParams parameters);
    }

    public static class NVEncProfiles
    {
        public static readonly Guid Auto = new Guid("BFD6F8E7-233C-4341-8B3E-4818523803F4");

        public static readonly Guid H264_Baseline = new Guid("0727BCAA-78C4-4c83-8C2F-EF3DFF267C6A");

        public static readonly Guid H264_Main = new Guid("60B5C1D4-67FE-4790-94D5-C4726D7B6E6D");

        public static readonly Guid H264_High = new Guid("E7CBC309-4F7A-4b89-AF2A-D537C92BE310");

        public static readonly Guid H264_High444 = new Guid("7AC663CB-A598-4960-B844-339B261A7D52");

        public static readonly Guid H264_Stereo = new Guid("40847BF5-33F7-4601-9084-E8FE3C1DB8B7");

        public static readonly Guid H264_SVC_Temporal_Scalabilty = new Guid("CE788D20-AAA9-4318-92BB-AC7E858C8D36");

        public static readonly Guid H264_Progressive_High = new Guid("B405AFAC-F32B-417B-89C4-9ABEED3E5978");

        public static readonly Guid H264_Constrained_High = new Guid("AEC1BD87-E85B-48f2-84C3-98BCA6285072");

        public static readonly Guid HEVC_Main = new Guid("B514C39A-B55B-40fa-878F-F1253B4DFDEC");
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("9A2AC42C-3E3D-4E6A-84E5-D097292D496B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INVEncConfig : IAMVideoCompression
    {
        [PreserveSig]
        new int put_KeyFrameRate([In] int KeyFrameRate);

        [PreserveSig]
        new int get_KeyFrameRate([Out] out int pKeyFrameRate);

        [PreserveSig]
        new int put_PFramesPerKeyFrame([In] int PFramesPerKeyFrame);

        [PreserveSig]
        new int get_PFramesPerKeyFrame([Out] out int pPFramesPerKeyFrame);

        [PreserveSig]
        new int put_Quality([In] double Quality);

        [PreserveSig]
        new int get_Quality([Out] out double pQuality);

        [PreserveSig]
        new int put_WindowSize([In] long WindowSize);

        [PreserveSig]
        new int get_WindowSize([Out] out long pWindowSize);

        [PreserveSig]
        new int GetInfo(
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszVersion, // WCHAR *
            [Out] out int pcbVersion,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDescription, // LPWSTR
            [Out] out int pcbDescription,
            [Out] out int pDefaultKeyFrameRate,
            [Out] out int pDefaultPFramesPerKey,
            [Out] out double pDefaultQuality,
            [Out] out CompressionCaps pCapabilities
            );

        [PreserveSig]
        new int OverrideKeyFrame([In] int FrameNumber);

        [PreserveSig]
        new int OverrideFrameSize(
            [In] int FrameNumber,
            [In] int Size
            );

        [PreserveSig]
        int SetDeviceType(int v);

        [PreserveSig]
        int GetDeviceType([Out] out int v);

        [PreserveSig]
        int SetPictureStructure(int v);
        [PreserveSig]
        int GetPictureStructure([Out] out int v);

        [PreserveSig]
        int SetNumBuffers(int v);

        [PreserveSig]
        int GetNumBuffers([Out] out int v);

        [PreserveSig]
        int SetRateControl(VFNVENCRateControlMode v);

        [PreserveSig]
        int GetRateControl([Out] VFNVENCRateControlMode v);

        [PreserveSig]
        int SetPreset(Guid v);

        [PreserveSig]
        int GetPreset([Out] out Guid v);

        [PreserveSig]
        int SetQp(int v);

        [PreserveSig]
        int GetQp([Out] out int v);

        [PreserveSig]
        int SetBFrames(int v);

        [PreserveSig]
        int GetBFrames([Out] out int v);

        [PreserveSig]
        int SetGOP(int v);

        [PreserveSig]
        int GetGOP([Out] out int v);

        [PreserveSig]
        int SetBitrate(int v);

        [PreserveSig]
        int GetBitrate([Out] out int v);

        [PreserveSig]
        int SetVbvBitrate(int v);

        [PreserveSig]
        int GetVbvBitrate([Out] out int v);

        [PreserveSig]
        int SetVbvSize(int v);

        [PreserveSig]
        int GetVbvSize([Out] out int v);

        [PreserveSig]
        int SetProfile(Guid v);

        [PreserveSig]
        int GetProfile([Out] out Guid v);

        [PreserveSig]
        int SetLevel(VFNVENCLevel v);

        [PreserveSig]
        int GetLevel([Out] out VFNVENCLevel v);

        [PreserveSig]
        int SetCodec(VFNVENCCodec v);

        [PreserveSig]
        int GetCodec([Out] out VFNVENCCodec v);
    }


    /// <summary>
    /// AAC info.
    /// </summary>
    public struct AACInfo
    {
        public int samplerate;
        public int channels;
        public int frame_size;
        public Int64 frames_done;
    }

    /// <summary>
    /// AAC config.
    /// </summary>
    public struct AACConfig
    {
        public int version;
        public int object_type;
        public int output_type;
        public int bitrate;
    }

    /// <summary>
    /// Monogram AAC encoder interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("B2DE30C0-1441-4451-A0CE-A914FD561D7F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMonogramAACEncoder
    {
        [PreserveSig]
        int GetConfig(ref AACConfig config);

        [PreserveSig]
        int SetConfig(ref AACConfig config);
    }

    /// <summary>
    /// AAC encoder interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("0BEF7533-39E6-42a5-863F-E087FAB5D84F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFAACEncoder
    {
        /// <summary>
        /// Force specified sample rate. 0 to use any.
        /// </summary>
        [PreserveSig]
        int SetInputSampleRate(uint ulSampleRate);

        /// <summary>
        /// Get fixed sample rate. 0 if not fixed.
        /// </summary>
        [PreserveSig]
        int GetInputSampleRate(out uint pulSampleRate);

        /// <summary>
        /// Set number of channels to use.
        /// </summary>
        [PreserveSig]
        int SetInputChannels(short nChannels);

        /// <summary>
        /// Returns number of channels to use.
        /// </summary>
        [PreserveSig]
        int GetInputChannels(out short pnChannels);

        /// <summary>
        /// Force specified bit rate. -1 to use maximal.
        /// </summary>
        [PreserveSig]
        int SetBitRate(uint ulBitRate);

        /// <summary>
        /// Get bit rate to use. -1 to use maximal.
        /// </summary>
        [PreserveSig]
        int GetBitRate(out uint pulBitRate);

        /// <summary>
        /// Set profile type.
        /// </summary>
        [PreserveSig]
        int SetProfile(uint uProfile);

        /// <summary>
        /// Returns current profile.
        /// </summary>
        [PreserveSig]
        int GetProfile(out uint puProfile);

        /// <summary>
        /// Set format of output.
        /// </summary>
        [PreserveSig]
        int SetOutputFormat(uint uFormat);

        /// <summary>
        /// Returns format of output.
        /// </summary>
        [PreserveSig]
        int GetOutputFormat(out uint puFormat);

        /// <summary>
        /// Set time shift.
        /// </summary>
        [PreserveSig]
        int SetTimeShift(int timeShift);

        /// <summary>
        /// Returns format of output.
        /// </summary>
        [PreserveSig]
        int GetTimeShift(out int ptimeShift);

        /// <summary>
        /// LFE control.
        /// </summary>
        [PreserveSig]
        int SetLFE(uint lfe);

        /// <summary>
        /// Return LFE state.
        /// </summary>
        [PreserveSig]
        int GetLFE(out uint p);

        /// <summary>
        /// TNS control.
        /// </summary>
        [PreserveSig]
        int SetTNS(uint tns);

        /// <summary>
        /// Return TNS state.
        /// </summary>
        [PreserveSig]
        int GetTNS(out uint p);

        /// <summary>
        /// Mid-side control.
        /// </summary>
        [PreserveSig]
        int SetMidSide(uint v);

        /// <summary>
        /// Return TNS state.
        /// </summary>
        [PreserveSig]
        int GetMidSide(out uint p);
    }
   
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("2A741FB6-6DE1-460B-8FCA-76DB478C9357")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INVEncConfig2
    {
        [PreserveSig]
        int CheckNVENCAvailable([Out, MarshalAs(UnmanagedType.Bool)] out bool result, out NVENCErrorCode errorCode);
    }

    /// <summary>
    /// MP4 v10 muxer flags.
    /// </summary>
    [Flags]
    public enum MP4V10Flags
    {
        /// <summary>
        /// Default.
        /// </summary>
        None = 0,

        /// <summary>
        /// Time override.
        /// </summary>
        TimeOverride = 0x00000001,

        /// <summary>
        /// Time adjust.
        /// </summary>
        TimeAdjust = 0x00000002
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("9E26CE8B-6708-4535-AAA4-23F9A97C7937")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMP4V10MuxerConfig
    {
        [PreserveSig]
        int SetFlags([In] uint value);

        [PreserveSig]
        int GetFlags([Out] out uint pValue);

        [PreserveSig]
        int SetLiveDisabled([MarshalAs(UnmanagedType.Bool)] bool liveDisabled);
    }

    /// <summary>
    /// NVENC error codes.
    /// </summary>
    public enum NVENCErrorCode
    {
        /// <summary>
        /// This indicates that API call returned with no errors.
        /// </summary>
        Success,

        /// <summary>
        /// This indicates that no encode capable devices were detected.
        /// </summary>
        NoEncodeDevice,

        /// <summary>
        /// This indicates that devices pass by the client is not supported.
        /// </summary>
        UnsupportedDevice,

        /// <summary>
        /// This indicates that the encoder device supplied by the client is not valid.
        /// </summary>
        InvalidEncoderDevice,

        /// <summary>
        /// This indicates that device passed to the API call is invalid.
        /// </summary>
        InvalidDevice,

        /// <summary>
        /// This indicates that device passed to the API call is no longer available and 
        /// needs to be reinitialized. The clients need to destroy the current encoder  
        /// session by freeing the allocated input output buffers and destroying the device 
        /// and create a new encoding session.
        /// </summary>
        DeviceNotExist,

        /// <summary>
        /// This indicates that one or more of the pointers passed to the API call is invalid.
        /// </summary>
        InvalidPointer,

        /// <summary>
        /// This indicates that completion event passed in ::NvEncEncodePicture() call is invalid.
        /// </summary>
        InvalidEvent,

        /// <summary>
        /// This indicates that one or more of the parameter passed to the API call is invalid.
        /// </summary>
        InvalidParameter,

        /// <summary>
        /// This indicates that an API call was made in wrong sequence/order.
        /// </summary>
        InvalidCall,

        /// <summary>
        /// This indicates that the API call failed because it was unable to allocate enough memory to perform the requested operation.
        /// </summary>
        OutOfMemory,

        /// <summary>
        /// This indicates that the encoder has not been initialized with ::NvEncInitializeEncoder() or that initialization has failed.
        /// The client cannot allocate input or output buffers or do any encoding related operation before successfully initializing the encoder.
        /// </summary>
        EncoderNotInitialized,

        /// <summary>
        /// This indicates that an unsupported parameter was passed by the client.
        /// </summary>
        UnsupportedParameter,

        /// <summary>
        /// This indicates that the ::NvEncLockBitstream() failed to lock the output buffer. 
        /// </summary>
        LockBusy,

        /// <summary>
        /// This indicates that the size of the user buffer passed by the client is insufficient for the requested operation.
        /// </summary>
        NotEnoughBuffer,

        /// <summary>
        /// This indicates that an invalid struct version was used by the client.
        /// </summary>
        InvalidVersion,

        /// <summary>
        /// This indicates that ::NvEncMapInputResource() API failed to map the client provided input resource.
        /// </summary>
        MapFailed,

        /// <summary>
        /// This indicates encode driver requires more input buffers to produce an output
        ///  bitstream. 
        /// </summary>
        NeedMoreInput,

        /// <summary>
        ///  This indicates that the HW encoder is busy encoding and is unable to encode  
        /// the input. The client should call ::NvEncEncodePicture() again after few milliseconds.
        /// </summary>
        EncoderBusy,

        /// <summary>
        /// This indicates that the completion event passed in ::NvEncEncodePicture()
        /// API has not been registered with encoder driver using ::NvEncRegisterAsyncEvent().
        /// </summary>
        EventNotRegistered,

        /// <summary>
        /// This indicates that an unknown internal error has occurred.
        /// </summary>
        GenericError,

        /// <summary>
        /// This indicates that the client is attempting to use a feature that is not available for the license type for the current system.
        /// </summary>
        IncompatibleClientKey,

        /// <summary>
        /// This indicates that the client is attempting to use a feature that is not implemented for the current version.
        /// </summary>
        Unimplemeted,

        /// <summary>
        /// This indicates that the ::NvEncRegisterResource API failed to register the resource.
        /// </summary>
        ResourceRegisterFailder,

        /// <summary>
        /// This indicates that the client is attempting to unregister a resource that has not been successfully registered.
        /// </summary>
        ResourceNotRegistered,

        /// <summary>
        /// This indicates that the client is attempting to unmap a resource that has not been successfully mapped.
        /// </summary>
        ResourceNotMapped,
    }


}
