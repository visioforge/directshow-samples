using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.Diagnostics.CodeAnalysis;


    public enum LOG_LEVEL
    {
        LL_DEBUG = 0,

        LL_ERROR = 1,
    }

    public delegate int ERRORCALLBACK([In] IntPtr handle, [In] uint handle_id, LOG_LEVEL level, [MarshalAs(UnmanagedType.LPWStr)] string text);

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("3D7A8BA3-AB22-44E2-9CF1-6F98F4CBACEC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMFMuxConfig
    {
        [PreserveSig]
        int SetVideoEncoderSettings(VFMFVideoEncoderSettings settings);

        [PreserveSig]
        int SetAudioEncoderSettings(VFMFAudioEncoderSettings settings);

        [PreserveSig]
        int SetErrorCallback([MarshalAs(UnmanagedType.FunctionPtr)] ERRORCALLBACK callback);

        [PreserveSig]
        int SetMuxer(VFMFMuxer muxer);

        [PreserveSig]
        int SetHLSMuxerSettings(VFHLSSettings settings);

        [PreserveSig]
        int SeparateCaptureSetMode([MarshalAs(UnmanagedType.Bool)] bool enable, [MarshalAs(UnmanagedType.Bool)] bool autostart);

        [PreserveSig]
        int SeparateCaptureStart();

        [PreserveSig]
        int SeparateCaptureStop();

        [PreserveSig]
        int SeparateCaptureStartNewFile([MarshalAs(UnmanagedType.LPWStr)] string filename);

        [PreserveSig]
        int SeparateCapturePause();

        [PreserveSig]
        int SeparateCaptureResume();
    }

    public static class SettingsHelper
    {
        public static void SetDefaults(ref VFMFVideoEncoderSettings settings)
        {
            settings.AvgBitrate = 2000;
            settings.Encoder = VFMFVideoEncoder.MS_H264;
            settings.AdaptiveMode = VFMFAdaptiveMode.None;
            settings.CustomAspectRatioX = 0;
            settings.CustomAspectRatioY = 0;
            settings.H264Profile = VFMFH264Profile.Main;
            settings.H264Level = VFMFH264Level.Level42;
            settings.InterlaceMode = VFMFVideoInterlaceMode.Progressive;
            settings.LowLatencyMode = false;
            settings.MaxBitrate = 3000;
            settings.MaxKeyFrameSpacing = 10;
            settings.Quality = 75;
            settings.CABAC = true;
            settings.QPUsed = false;
        }

        public static void SetDefaults(ref VFMFAudioEncoderSettings settings)
        {
            settings.Bitrate = 128;
            settings.Encoder = VFMFAudioEncoder.MS_AAC;
            settings.AmplifyLevel = 1.0f;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VFMFSourceSettings
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string videoCaptureDevice;
        public VFMFBaseVideoInfo videoInfo;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string audioCaptureDevice;

        public VFMFBaseAudioInfo audioInfo;
        public double audioVolume;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VFMFBaseAudioInfo
    {
        public int BPS;
        public int Channels;
        public int SampleRate;
        public VFMFAudioFormat Format;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VFMFBaseVideoInfo
    {
        public int Width;
        public int Height;
        public VFMFVideoColorSpace Colorspace;
        public int Stride;
        public int FrameRateNum;
        public int FrameRateDen;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct H264QP
    {
        public UInt16 DefaultQp;
        public UInt16 I;
        public UInt16 P;
        public UInt16 B;

        public static H264QP FromMFH264QP(VFMFH264QP qp)
        {
            var qp2 = new H264QP
            {
                B = qp.B,
                I = qp.I,
                P = qp.P,
                DefaultQp = qp.DefaultQp
            };

            return qp2;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VFMFEncodersAvailableInfo
    {
        [MarshalAs(UnmanagedType.Bool)]
        public bool WIN_H264;

        [MarshalAs(UnmanagedType.Bool)]
        public bool QSV_H264;

        [MarshalAs(UnmanagedType.Bool)]
        public bool QSV_H265;

        [MarshalAs(UnmanagedType.Bool)]
        public bool NVENC_H264;

        [MarshalAs(UnmanagedType.Bool)]
        public bool NVENC_H265;

        [MarshalAs(UnmanagedType.Bool)]
        public bool WIN_AAC;

        [MarshalAs(UnmanagedType.Bool)]
        public bool AMD_H264;

        [MarshalAs(UnmanagedType.Bool)]
        public bool AMD_H265;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VFMFVideoEncoderSettings
    {
        public VFMFVideoEncoder Encoder;

        // Specifies the average encoded bit rate, in bits per second. This property applies only to constant bit rate (CBR) and variable bit rate (VBR) encoding modes.
        public int AvgBitrate;

        // Specifies the maximum bit rate, in bits per second. This property applies only to constant bit rate (CBR) and variable bit rate (VBR) encoding modes.
        public int MaxBitrate;

        // Specifies the quality level for encoding. This property controls the quality level when the encoder is not using a constrained bit rate. 65, 0-100
        public int Quality;

        //Enables or disables CABAC (context-adaptive binary arithmetic coding) for H.264 entropy coding.
        [MarshalAs(UnmanagedType.Bool)]
        public bool CABAC;

        public VFMFH264Profile H264Profile;

        public VFMFH264Level H264Level;

        public VFMFCommonRateControlMode RateControlMode;

        public VFMFVideoInterlaceMode InterlaceMode;

        public int CustomAspectRatioX;

        public int CustomAspectRatioY;

        public VFMFAdaptiveMode AdaptiveMode;

        // Low-latency mode is useful for real-time communications or live capture, when latency should be minimized. However, low-latency mode might also reduce the decoding or encoding quality.
        // The encoder is expected to not add any sample delay due to frame reordering in encoding process, and one input sample shall produce one output sample.B slices / frames can be present 
        // as long as they do not introduce any frame re - ordering in the encoder.
        [MarshalAs(UnmanagedType.Bool)]
        public bool LowLatencyMode;

        public int MaxKeyFrameSpacing;

        // Specifies the tradeoff between encoding quality and speed. This property is valid in all rate control modes. 33, 0-100
        public int QualityVsSpeed;

        // Specifies the maximum number of pictures from one group-of-pictures (GOP) header to the next GOP header. 50
        public int MPVGOPSize;

        // TRUE is QP used
        [MarshalAs(UnmanagedType.Bool)]
        public bool QPUsed;

        // Specifies the quantization parameter (QP) for video encoding.
        public H264QP QP;

        // Specifies the minimum quantization parameter(QP) for video encoding in variable-QP mode. 0, 0-51
        public int MinQP;

        // Specifies the maximum QP supported by the encoder. 51, 0-51.
        public int MaxQP;

        // Specifies the frame types (I, P, or B) that the quantization parameter (QP) is applied to.
        // Range for I, P, B frames is [0, 51].
        public H264QP FrameTypeQP;

        // Specifies the maximum reference frames supported by the encoder. 2, [0, 16]
        public int MaxNumRefFrame;

        // Sets the maximum number of consecutive B frames in the output bitstream. Valid values are:
        // 0: Do not use B frames(default).
        // 1 : Use one B frame.
        // 2 : Use two B frames.
        public int DefaultBPictureCount;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VFMFAudioEncoderSettings
    {
        public VFMFAudioEncoder Encoder;
        public int Bitrate;
        public double AmplifyLevel;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VFMFOutputSettings
    {
        public VFMFVideoEncoderSettings Video;
        public VFMFAudioEncoderSettings Audio;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Filename;

        public VFMFMuxer Muxer;

        public VFHLSSettings HLSSettings;
    }

    /// <summary>
    /// HLS playlist type.
    /// </summary>
    public enum VFHLSPlaylistType
    {
        /// <summary>
        /// Live.
        /// </summary>
        Live,

        /// <summary>
        /// VOD.
        /// </summary>
        VOD,

        /// <summary>
        /// Event.
        /// </summary>
        Event
    }

    /// <summary>
    /// HLS settings.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VFHLSSettings
    {
        /// <summary>
        /// Playlist type.
        /// </summary>
        public VFHLSPlaylistType PlaylistType;

        /// <summary>
        /// Segment duration.
        /// </summary>
        public int SegmentDuration;

        /// <summary>
        /// Number of segments saved on disk. Set 0 to not remove.
        /// </summary>
        public int NumSegments;

        /// <summary>
        /// Playlist file name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string PlaylistName;

        /// <summary>
        /// Part name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string PartName;

        /// <summary>
        /// Sets default values.
        /// </summary>
        /// <param name="source">
        /// Source settings.
        /// </param>
        /// <returns>
        /// The <see cref="VFHLSSettings"/>.
        /// </returns>
        public static VFHLSSettings Create(VFHLSSettings source)
        {
            VFHLSSettings dest = new VFHLSSettings
            {
                SegmentDuration = source.SegmentDuration,
                NumSegments = source.NumSegments,
                PlaylistName = source.PlaylistName,
                PartName = source.PartName,
                PlaylistType = source.PlaylistType
            };

            return dest;
        }
    };

    /// <summary>
    /// RAW video frame.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWVideoFrame
    {
        /// <summary>
        /// Data.
        /// </summary>
        public IntPtr Data;

        /// <summary>
        /// Data size.
        /// </summary>
        public int DataSize;

        public VFMFBaseVideoInfo Info;

        public long Timestamp;

        public long Duration;
    }

    /// <summary>
    /// RAW audio frame.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWAudioFrame
    {
        public IntPtr Buffer;
        public int BufferSize;
        public VFMFBaseAudioInfo Info;
        public long Timestamp;
        public long Duration;
    }

    /// <summary>
    /// RAW image color space helper.
    /// </summary>
    public static class RAWImageColorSpaceHelper
    {
        /// <summary>
        /// Gets stride.
        /// </summary>
        /// <param name="cs">
        /// Color space.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Wrong arguments.
        /// </exception>
        public static int GetStride(this VFMFVideoColorSpace cs, int width)
        {
            switch (cs)
            {
                case VFMFVideoColorSpace.RGB:
                case VFMFVideoColorSpace.BGR:
                    return (width * 3 - 1) / 4 * 4 + 4;
                case VFMFVideoColorSpace.RGBA:
                    return (width * 4 - 1) / 4 * 4 + 4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cs), cs, null);
            }
        }
    }

    /// <summary>
    /// The RAW image color space.
    /// </summary>
    public enum VFMFVideoColorSpace
    {
        /// <summary>
        /// RGB.
        /// </summary>
        RGB = 0,

        /// <summary>
        /// BGR.
        /// </summary>
        BGR = 1,

        /// <summary>
        /// RGBA.
        /// </summary>
        RGBA = 2,

        NV12 = 3
    }

    public enum VFMFAudioFormat
    {
        PCM8,
        PCM16,
        PCM24,
        PCM32,
        IEEE32
    }

    public enum VFMFMPEG2Profile
    {
        VFMFMPEG2Profile_Simple = 1,
        VFMFMPEG2Profile_Main,
        VFMFMPEG2Profile_SNRScalable,
        VFMFMPEG2Profile_SpatiallyScalable,
        VFMFMPEG2Profile_High
    }

    public enum VFMFMPEG2Level
    {
        VFMFMPEG2Level_Low = 1,
        VFMFMPEG2Level_Main,
        VFMFMPEG2Level_High1440,
        VFMFMPEG2Level_High
    }


    public class RAWVideoFrameEventArgs : EventArgs
    {
        public RAWVideoFrame Frame { get; private set; }

        public RAWVideoFrameEventArgs(RAWVideoFrame frame)
        {
            this.Frame = frame;
        }
    }

    public class RAWAudioFrameEventArgs : EventArgs
    {
        public RAWAudioFrame Frame { get; private set; }

        public RAWAudioFrameEventArgs(RAWAudioFrame frame)
        {
            this.Frame = frame;
        }
    }

    //public class VideoFrameEventArgs : EventArgs
    //{
    //    public Bitmap Image { get; private set; }

    //    public VideoFrameEventArgs(Bitmap image)
    //    {
    //        this.Image = image;
    //    }
    //}

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void VideoFrameCallback(
        ref RAWVideoFrame frame,
        long timestamp);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void AudioFrameCallback(
        ref RAWAudioFrame frame,
        long timestamp);

    /// <summary>
    /// MF video encoder.
    /// </summary>
    public enum VFMFVideoEncoder
    {
        /// <summary>
        /// Microsoft H264.
        /// </summary>
        MS_H264,

        /// <summary>
        /// Intel QuickSync H264.
        /// </summary>
        QSV_H264,

        /// <summary>
        /// nVidia NVENC H264.
        /// </summary>
        NVENC_H264,

        /// <summary>
        /// AMD / ATI H264.
        /// </summary>
        AMD_H264,

        /// <summary>
        /// Microsoft H265.
        /// </summary>
        MS_H265,

        /// <summary>
        /// Intel QuickSync H265.
        /// </summary>
        QSV_H265,

        /// <summary>
        /// nVidia NVENC H265.
        /// </summary>
        NVENC_H265,

        /// <summary>
        /// AMD / ATI H265.
        /// </summary>
        AMD_H265,

        /// <summary>
        /// No video stream.
        /// </summary>
        None
    }

    /// <summary>
    /// MF audio encoder.
    /// </summary>
    public enum VFMFAudioEncoder
    {
        /// <summary>
        /// Microsoft AAC.
        /// </summary>
        MS_AAC
    }

    public enum VFMFCommonRateControlMode
    {
        /// <summary>
        /// Constant bit rate (CBR) encoding.
        /// </summary>
        CBR = 0,

        /// <summary>
        /// Constrained variable bit rate (VBR) encoding.
        /// </summary>
        PeakConstrainedVBR = 1,

        /// <summary>
        /// Unconstrained VBR encoding.
        /// </summary>
        UnconstrainedVBR = 2,

        /// <summary>
        /// Quality-based VBR encoding. The encoder selects the bit rate to match a specified quality level.
        /// </summary>
        Quality = 3,

        /// <summary>
        /// Low delay VBR encoding.H.264 extension. Requires Windows 8 for CPU encoder.
        /// </summary>
        LowDelayVBR = 4,

        /// <summary>
        /// Global VBR encoding.H.264 extension. Requires Windows 8 for CPU encoder.
        /// </summary>
        GlobalVBR = 5,

        /// <summary>
        /// Global low delay VBR encoding.H.264 extension. Requires Windows 8 for CPU encoder.
        /// </summary>
        GlobalLowDelayVBR = 6
    }

    /// <summary>
    /// MF video adaptive mode.
    /// </summary>
    public enum VFMFAdaptiveMode
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        Resolution = 1,
        FrameRate = 2
    }

    /// <summary>
    /// MF video interlace mode.
    /// </summary>
    public enum VFMFVideoInterlaceMode
    {
        Unknown = 0,
        Progressive = 2,
        FieldInterleavedUpperFirst = 3,
        FieldInterleavedLowerFirst = 4,
        FieldSingleUpper = 5,
        FieldSingleLower = 6,
        MixedInterlaceOrProgressive = 7
    }

    public enum VFMFH264Profile
    {
        Unknown = 0,
        Simple = 66,
        Base = 66,
        Main = 77,
        High = 100,
        High422 = 122,
        High10 = 110,
        High444 = 144,
        Extended = 88,
        ScalableBase = 83,
        ScalableHigh = 86,
        MultiviewHigh = 118,
        StereoHigh = 128,
        ConstrainedBase = 256,
        UCConstrainedHigh = 257,
        UCScalableConstrainedBase = 258,
        UCScalableConstrainedHigh = 259
    }

    /// <summary>
    /// H264 level.
    /// </summary>
    public enum VFMFH264Level
    {
        Level1 = 10,
        Level1b = 11,
        Level11 = 11,
        Level12 = 12,
        Level13 = 13,
        Level2 = 20,
        Level21 = 21,
        Level22 = 22,
        Level3 = 30,
        Level31 = 31,
        Level32 = 32,
        Level4 = 40,
        Level41 = 41,
        Level42 = 42,
        Level5 = 50,
        Level51 = 51,
        Level52 = 52
    }


    public struct VFMFH264QP
    {
        public UInt16 DefaultQp;
        public UInt16 I;
        public UInt16 P;
        public UInt16 B;
    }

    /// <summary>
    /// v11 output muxer.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum VFMFMuxer
    {
        /// <summary>
        /// MP4.
        /// </summary>
        MP4,

        /// <summary>
        /// Alternative MP4 output.
        /// </summary>
        MP4v2,

        /// <summary>
        /// MPEG-TS output.
        /// </summary>
        MPEG_TS,

        /// <summary>
        /// HLS output.
        /// </summary>
        HLS,

        /// <summary>
        /// Matroska.
        /// </summary>
        MKV,

        /// <summary>
        /// Apple MOV.
        /// </summary>
        MOV
    };
}

