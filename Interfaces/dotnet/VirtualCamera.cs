namespace VisioForge.DirectShowAPI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    using DirectShowLib;

    // ReSharper disable InconsistentNaming
        
    /// <summary>
    /// Push source frame data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AVFrameData
    {
        public IntPtr Data;

        public int Size;

        public long StartTime;

        public long StopTime;
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("8E1449D8-0106-486A-85F4-FC9A5D52C2B2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFLiveVideoSource
    {
        [PreserveSig]
        int AddFrame([In, MarshalAs(UnmanagedType.LPStruct)] AVFrameData frame);

        [PreserveSig]
        int AddFrame2([In] AVFrameData frame);

        [PreserveSig]
        int SetBitmapInfo(BitmapInfoHeader bmi);

        [PreserveSig]
        int SetFrameRate(double frameRate);
    }

    ///// <summary>
    ///// VisioForge constants.
    ///// </summary>
    //public static class Consts
    //{
    //    // ReSharper disable InconsistentNaming

    //    public const string CLSID_StreamBufferSink = "2DB47AE5-CF39-43C2-B4D6-0CD8D90946F4";
    //    public const string CLSID_StreamBufferSink2 = "E2448508-95DA-4205-9A27-7EC81E723B1A";
    //    public const string CLSID_StreamBufferSource = "C9F5FE02-F851-4EB5-99EE-AD602AF1E619";

    //    public const string CLSID_MPEG2VideoAnalyzer = "6CFAD761-735D-4AA5-8AFC-AF91A7D61EBA";
    //    public const string CLSID_MPEG2VideoEncoder = "42150CD9-CA9A-4EA5-9939-30EE037F6E74";
    //    public const string CLSID_MPEG2AudioEncoder = "ACD453BC-C58A-44D1-BBF5-BFB325BE2D78";

 

    //    /// <summary>
    //    /// WAV Dest filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFWavDest = "16EF2357-E074-436d-A37A-20BBE06A5D93";

    //    /// <summary>
    //    /// Matroska muxer CLSID.
    //    /// </summary>
    //    public const string CLSID_MatroskaMuxer = "A28F324B-DDC5-4999-AA25-D3A7E25EF7A8";

    //    /// <summary>
    //    /// GDCL MP4 muxer CLSID.
    //    /// </summary>
    //    public const string CLSID_GDCL_MP4_Muxer = "5FD85181-E542-4E52-8D9D-5D613C30131B";

    //    /// <summary>
    //    /// GDCL MP4 muxer CLSID.
    //    /// </summary>
    //    public const string CLSID_Monogram_MP4_Muxer = "5D33564D-873C-47FB-90AD-C6B2657ECE1A";

    //    /// <summary>
    //    /// GDCL MP4 muxer CLSID.
    //    /// </summary>
    //    public const string CLSID_Monogram_H264 = "1FB0F046-623C-40A7-B439-41E4BFCB8BAB";

    //    /// <summary>
    //    /// GDCL MP4 muxer CLSID.
    //    /// </summary>
    //    public const string CLSID_Monogram_AAC = "88F36DB6-D898-40B5-B409-466A0EECC26A";

    //    /// <summary>
    //    /// LAME CLSID.
    //    /// </summary>
    //    public const string CLSID_LAMEDShowFilter = "B8D27088-FF5F-4B7C-98DC-0E91A1696286";

    //    /// <summary>
    //    /// LAME encoder interface IID.
    //    /// </summary>
    //    public const string IID_IAudioEncoderProperties = "ca7e9ef0-1cbe-11d3-8d29-00a0c94bbfee";

    //    /// <summary>
    //    /// PIP filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFVideoMixer = "3F4B9E80-436F-4f97-AD39-4656943278D2";

    //    /// <summary>
    //    /// OGG Muxer filter CLSID.
    //    /// </summary>
    //    public const string CLSID_OGGMuxer = "1F3EFFE4-0E70-47C7-9C48-05EB99E20011";

    //    /// <summary>
    //    /// RTSP Dest filter CLSID.
    //    /// </summary>
    //    public const string CLSID_RTSPDest = "3E6E38A3-B90D-4DED-B67A-482B35163D2E";

    //    /// <summary>
    //    /// Chroma key CLSID.
    //    /// </summary>
    //    public const string CLSID_VFChromaKey = "CB84E907-D2DF-4dee-8837-4BCB3DCB8A2A";

    //    /// <summary>
    //    /// Chroma key filter interface IID.
    //    /// </summary>
    //    public const string IID_IVFChromaKey = "AF6E8208-30E3-44f0-AAFE-787A6250BAB3";

    //    /// <summary>
    //    /// Video Effects filter v4.5 CLSID.
    //    /// </summary>
    //    public const string CLSID_VFVideoEffects45 = "1D0523AB-7E41-40e8-9A78-839E93BA9444";

    //    /// <summary>
    //    /// Video Effects Pro filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFVideoEffectsPro = "980B1181-1619-44F9-AEBE-F2D7E5CE1EFE";

    //    /// <summary>
    //    /// Screen capture filter interface IID.
    //    /// </summary>
    //    public const string IID_IVFScreenCapture = "259E0009-9963-4a71-91AE-34B96D75486F";

    //    /// <summary>
    //    /// Screen capture filter interface 2 IID.
    //    /// </summary>
    //    public const string IID_IVFScreenCapture2 = "BC91012D-22E0-4091-8C0A-3913BDAB8A42";

    //    /// <summary>
    //    /// Screen capture filter CLSID (CSharp filter).
    //    /// </summary>
    //    public const string CLSID_VFScreenCaptureCS = "9100239C-FFB4-4d7f-ABA8-854A575C9DFB";

    //    /// <summary>
    //    /// Screen capture filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFScreenCapture = "B74136FB-1F94-4180-8695-C9307810D944";

    //    /// <summary>
    //    /// Resize filter interface IID.
    //    /// </summary>
    //    public const string IID_IVFResize = "12BC6F20-2812-4660-8684-10F3FD3B4487";

    //    /// <summary>
    //    /// FFMPEGEncoder filter CLSID.
    //    /// </summary>
    //    public const string CLSID_FFMPEGEncoder = "554AB365-B293-4C1D-9245-E8DB01F027F7";

    //    /// <summary>
    //    /// Resize filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFResizer_4 = "2E0E7313-71DC-4455-ADB4-F80718B7B727";

    //    /// <summary>
    //    /// MPEG-2 PSI parser interface IID.
    //    /// </summary>
    //    public const string IID_IMpeg2PsiParser = "AE1A2884-540E-4077-B1AB-67A34A72298C";

    //    /// <summary>
    //    /// MPEG-2 PSI parser CLSID.
    //    /// </summary>
    //    public const string CLSID_VFPSIParser = "DDF7480E-13E2-4481-BA2B-3C17C4FC469F";

    //    /// <summary>
    //    /// Dump filter v4 CLSID.
    //    /// </summary>
    //    public const string CLSID_VFDump_4 = "83DF94EE-5A0A-4730-9818-9726CE117CEC";

    //    /// <summary>
    //    /// Motion detection filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFMotDet_45 = "1F8FCE2E-16AC-4c88-9F8D-96DDF3F1B34F";

    //    /// <summary>
    //    /// IP HTTP source filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFIPHTTPSource = "4ea6930a-2c8a-4ae6-a561-56e4b5044437";

    //    /// <summary>
    //    /// IP HTTP FFMPEG source filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFIPHTTPFFMPEGSource = "1269EA71-85DB-40c6-AD87-35AD707C821C";

    //    /// <summary>
    //    /// RTSP source filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFRTSPSource = "990CC6D7-9472-4ee4-818A-07FD8146103F";

    //    /// <summary>
    //    /// RTSP FFMPEG source filter CLSID.
    //    /// </summary>
    //    public const string CLSID_VFRTSPFFMPEGSource = "EDB1A24C-4B0B-4c8d-AEE5-52231A062BB4";

    //    /// <summary>
    //    /// Audio effect filter v4 CLSID.
    //    /// </summary>
    //    public const string CLSID_VFAudioEffects4 = "E90165B8-8E1C-4c96-AC89-266A20BB3EBD";

    //    /// <summary>
    //    /// Audio effects filter audio interface IID.
    //    /// </summary>
    //    public const string IID_DCDSPFilter = "91692C5B-6878-4ba4-A3D9-7C8DB0D08E44";

    //    /// <summary>
    //    /// Audio effects filter visual interface IID.
    //    /// </summary>
    //    public const string IID_DCDSPFilterVisual = "081605E3-2B0E-4120-9F72-87A007B208B2";

    //    // Microsoft

    //    /// <summary>
    //    /// Microsoft Windows 7 DTV video decoder CLSID.
    //    /// </summary>
    //    public const string CLSID_MSWin7DTVVideoDecoder = "212690FB-83E5-4526-8FD7-74478B7939CD";

    //    /// <summary>
    //    /// Microsoft Windows 7 DTV audio decoder CLSID.
    //    /// </summary>
    //    public const string CLSID_MSWin7DTVAudioDecoder = "E1F1A0B8-BEEE-490D-BA7C-066C40B5E2B9";

    //    public const string CLSID_CMPEG2EncoderDS = "5F5AFF4A-2F7F-4279-88C2-CD88EB39D144";

    //    public const string CLSID_CMPEG2EncoderVideoDS = "42150cd9-ca9a-4ea5-9939-30ee037f6e74";

    //    public const string CLSID_CMPEG2EncoderAudioDS = "acd453bc-c58a-44d1-bbf5-bfb325be2d78";

    //    // ReSharper restore InconsistentNaming
    //}

    /// <summary>
    /// Virtual camera sink interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("A96631D2-4AC9-4F09-9F34-FF8229087DEB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFVirtualCameraSink
    {
        [PreserveSig]
        int set_license([MarshalAs(UnmanagedType.LPWStr)] string license);
    }
}
