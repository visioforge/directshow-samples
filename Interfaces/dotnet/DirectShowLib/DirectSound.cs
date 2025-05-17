// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectSound.cs" company="VisioForge">
//   VisioForge (c) 2012
// </copyright>
// <summary>
//   DirectSound types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VisioForge.DirectShowLib
{
    using System.Runtime.InteropServices;
    using System.Security;

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXChorus
    {
        public float WetDryMix;
        public float Depth;
        public float Feedback;
        public float Frequency;
        public int Waveform;
        public float Delay;
        public int Phase;
    } // end DSFXChorus

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("880842e3-145f-43e6-a934-a71806e50547"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXChorus
    {
        // IDirectSoundFXChorus methods
        [PreserveSig]
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXChorus pcDsFxChorus);

        [PreserveSig]
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXChorus pDsFxChorus);
    } // end IDirectSoundFXChorus

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXCompressor
    {
        public float Gain;
        public float Attack;
        public float Release;
        public float Threshold;
        public float Ratio;
        public float Predelay;
    } // end DSFXCompressor

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("4bbd1154-62f6-4e2c-a15c-d3b6c417f7a0"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXCompressor
    {
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXCompressor pcDsFxCompressor);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXCompressor pDsFxCompressor);
    } // end IDirectSoundFXCompressor

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXDistortion
    {
        public float Gain;
        public float Edge;
        public float PostEQCenterFrequency;
        public float PostEQBandwidth;
        public float PreLowpassCutoff;
    } // end DSFXDistortion

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("8ecf4326-455f-4d8b-bda9-8d5d3e9e3e0b"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXDistortion
    {
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXDistortion pcDsFxDistortion);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXDistortion pDsFxDistortion);
    } // end IDirectSoundFXDistortion

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXEcho
    {
        public float WetDryMix;
        public float Feedback;
        public float LeftDelay;
        public float RightDelay;
        public int PanDelay;
    } // end DSFXEcho

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("8bd28edf-50db-4e92-a2bd-445488d1ed42"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXEcho
    {
        // IDirectSoundFXEcho methods
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXEcho pcDsFxEcho);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXEcho pDsFxEcho);
    } // end IDirectSoundFXEcho

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXFlanger
    {
        public float WetDryMix;
        public float Depth;
        public float Feedback;
        public float Frequency;
        public int Waveform;
        public float Delay;
        public int Phase;
    } // end DSFXFlanger

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("903e9878-2c92-4072-9b2c-ea68f5396783"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXFlanger
    {
        // IDirectSoundFXFlanger methods
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXFlanger pcDsFxFlanger);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXFlanger pDsFxFlanger);
    } // end IDirectSoundFXFlanger

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXGargle
    {
        public int RateHz; // Rate of modulation in hz
        public int WaveShape;
    } // end DSFXGargle

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("d616f352-d622-11ce-aac5-0020af0b99a3"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXGargle
    {
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXGargle pcDsFxGargle);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXGargle pDsFxGargle);
    } // end IDirectSoundFXGargle

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXI3DL2Reverb
    {
        public int Room; // [-10000, 0]      default: -1000 mB
        public int RoomHF; // [-10000, 0]      default: 0 mB
        public float RoomRolloffFactor; // [0.0, 10.0]      default: 0.0
        public float DecayTime; // [0.1, 20.0]      default: 1.49s
        public float DecayHFRatio; // [0.1, 2.0]       default: 0.83
        public int Reflections; // [-10000, 1000]   default: -2602 mB
        public float ReflectionsDelay; // [0.0, 0.3]       default: 0.007 s
        public int Reverb; // [-10000, 2000]   default: 200 mB
        public float ReverbDelay; // [0.0, 0.1]       default: 0.011 s
        public float Diffusion; // [0.0, 100.0]     default: 100.0 %
        public float Density; // [0.0, 100.0]     default: 100.0 %
        public float HFReference;
    } // end DSFXI3DL2Reverb

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("4b166a6a-0d66-43f3-80e3-ee6280dee1a4"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXI3DL2Reverb
    {
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXI3DL2Reverb pcDsFxI3DL2Reverb);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXI3DL2Reverb pDsFxI3DL2Reverb);
        int SetPreset([In] int dwPreset);
        int GetPreset(out int pdwPreset);
        int SetQuality([In] int lQuality);
        int GetQuality(out int plQuality);
    } // end IDirectSoundFXI3DL2Reverb

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXParamEq
    {
        public float Center;
        public float Bandwidth;
        public float Gain;
    } // end DSFXParamEq

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("c03ca9fe-fe90-4204-8078-82334cd177da"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXParamEq
    {
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXParamEq pcDsFxParamEq);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXParamEq pDsFxParamEq);
    } // end IDirectSoundFXParamEq

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXWavesReverb
    {
        public float InGain; // [-96.0,0.0]            default: 0.0 dB
        public float ReverbMix; // [-96.0,0.0]            default: 0.0 db
        public float ReverbTime; // [0.001,3000.0]         default: 1000.0 ms
        public float HighFreqRTRatio;
    } // end DSFXWavesReverb

    [ComImport, SuppressUnmanagedCodeSecurity,
     Guid("46858c3a-0dc6-45e3-b760-d4eef16cb325"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectSoundFXWavesReverb
    {
        int SetAllParameters([In][MarshalAs(UnmanagedType.Struct)] DSFXWavesReverb pcDsFxWavesReverb);
        int GetAllParameters([MarshalAs(UnmanagedType.Struct)] out DSFXWavesReverb pDsFxWavesReverb);
    } // end IDirectSoundFXWavesReverb

    public static class DirectSoundConsts
    {
        // ReSharper disable InconsistentNaming
        public const int DSFX_LOCHARDWARE = 0x00000001;
        public const int DSFX_LOCSOFTWARE = 0x00000002;
        public const int DSFXR_PRESENT = 0;
        public const int DSFXR_LOCHARDWARE = 1;
        public const int DSFXR_LOCSOFTWARE = 2;
        public const int DSFXR_UNALLOCATED = 3;
        public const int DSFXR_FAILED = 4;
        public const int DSFXR_UNKNOWN = 5;
        public const int DSFXR_SENDLOOP = 6;
        public const int DSCFX_LOCHARDWARE = 0x00000001;
        public const int DSCFX_LOCSOFTWARE = 0x00000002;
        public const int DSCFXR_LOCHARDWARE = 0x00000010;
        public const int DSCFXR_LOCSOFTWARE = 0x00000020;
        //// Special GUID meaning "select all objects" for use in GetObjectInPath()
        //public const Guid GUID_All_Objects = "{aa114de5-c262-4169-a1c8-23d698cc73b5}";
        // IKsPropertySet
        public const int KSPROPERTY_SUPPORT_GET = 0x00000001;
        public const int KSPROPERTY_SUPPORT_SET = 0x00000002;
        public const int DSFXGARGLE_WAVE_TRIANGLE = 0;
        public const int DSFXGARGLE_WAVE_SQUARE = 1;
        public const int DSFXGARGLE_RATEHZ_MIN = 1;
        public const int DSFXGARGLE_RATEHZ_MAX = 1000;
        public const int DSFXCHORUS_WAVE_TRIANGLE = 0;
        public const int DSFXCHORUS_WAVE_SIN = 1;
        public const float DSFXCHORUS_WETDRYMIX_MIN = 0.0f;
        public const float DSFXCHORUS_WETDRYMIX_MAX = 100.0f;
        public const float DSFXCHORUS_DEPTH_MIN = 0.0f;
        public const float DSFXCHORUS_DEPTH_MAX = 100.0f;
        public const float DSFXCHORUS_FEEDBACK_MIN = -99.0f;
        public const float DSFXCHORUS_FEEDBACK_MAX = 99.0f;
        public const float DSFXCHORUS_FREQUENCY_MIN = 0.0f;
        public const float DSFXCHORUS_FREQUENCY_MAX = 10.0f;
        public const float DSFXCHORUS_DELAY_MIN = 0.0f;
        public const float DSFXCHORUS_DELAY_MAX = 20.0f;
        public const int DSFXCHORUS_PHASE_MIN = 0;
        public const int DSFXCHORUS_PHASE_MAX = 4;
        public const int DSFXCHORUS_PHASE_NEG_180 = 0;
        public const int DSFXCHORUS_PHASE_NEG_90 = 1;
        public const int DSFXCHORUS_PHASE_ZERO = 2;
        public const int DSFXCHORUS_PHASE_90 = 3;
        public const int DSFXCHORUS_PHASE_180 = 4;
        public const int DSFXFLANGER_WAVE_TRIANGLE = 0;
        public const int DSFXFLANGER_WAVE_SIN = 1;
        public const float DSFXFLANGER_WETDRYMIX_MIN = 0.0f;
        public const float DSFXFLANGER_WETDRYMIX_MAX = 100.0f;
        public const float DSFXFLANGER_FREQUENCY_MIN = 0.0f;
        public const float DSFXFLANGER_FREQUENCY_MAX = 10.0f;
        public const float DSFXFLANGER_DEPTH_MIN = 0.0f;
        public const float DSFXFLANGER_DEPTH_MAX = 100.0f;
        public const float DSFXFLANGER_FEEDBACK_MIN = -99.0f;
        public const float DSFXFLANGER_FEEDBACK_MAX = 99.0f;
        public const float DSFXFLANGER_DELAY_MIN = 0.0f;
        public const float DSFXFLANGER_DELAY_MAX = 4.0f;
        public const int DSFXFLANGER_PHASE_MIN = 0;
        public const int DSFXFLANGER_PHASE_MAX = 4;
        public const int DSFXFLANGER_PHASE_NEG_180 = 0;
        public const int DSFXFLANGER_PHASE_NEG_90 = 1;
        public const int DSFXFLANGER_PHASE_ZERO = 2;
        public const int DSFXFLANGER_PHASE_90 = 3;
        public const int DSFXFLANGER_PHASE_180 = 4;
        public const float DSFXECHO_WETDRYMIX_MIN = 0.0f;
        public const float DSFXECHO_WETDRYMIX_MAX = 100.0f;
        public const float DSFXECHO_FEEDBACK_MIN = 0.0f;
        public const float DSFXECHO_FEEDBACK_MAX = 100.0f;
        public const float DSFXECHO_LEFTDELAY_MIN = 1.0f;
        public const float DSFXECHO_LEFTDELAY_MAX = 2000.0f;
        public const float DSFXECHO_RIGHTDELAY_MIN = 1.0f;
        public const float DSFXECHO_RIGHTDELAY_MAX = 2000.0f;
        public const int DSFXECHO_PANDELAY_MIN = 0;
        public const int DSFXECHO_PANDELAY_MAX = 1;
        public const float DSFXDISTORTION_GAIN_MIN = -60.0f;
        public const float DSFXDISTORTION_GAIN_MAX = 0.0f;
        public const float DSFXDISTORTION_EDGE_MIN = 0.0f;
        public const float DSFXDISTORTION_EDGE_MAX = 100.0f;
        public const float DSFXDISTORTION_POSTEQCENTERFREQUENCY_MIN = 100.0f;
        public const float DSFXDISTORTION_POSTEQCENTERFREQUENCY_MAX = 8000.0f;
        public const float DSFXDISTORTION_POSTEQBANDWIDTH_MIN = 100.0f;
        public const float DSFXDISTORTION_POSTEQBANDWIDTH_MAX = 8000.0f;
        public const float DSFXDISTORTION_PRELOWPASSCUTOFF_MIN = 100.0f;
        public const float DSFXDISTORTION_PRELOWPASSCUTOFF_MAX = 8000.0f;
        public const float DSFXCOMPRESSOR_GAIN_MIN = -60.0f;
        public const float DSFXCOMPRESSOR_GAIN_MAX = 60.0f;
        public const float DSFXCOMPRESSOR_ATTACK_MIN = 0.01f;
        public const float DSFXCOMPRESSOR_ATTACK_MAX = 500.0f;
        public const float DSFXCOMPRESSOR_RELEASE_MIN = 50.0f;
        public const float DSFXCOMPRESSOR_RELEASE_MAX = 3000.0f;
        public const float DSFXCOMPRESSOR_THRESHOLD_MIN = -60.0f;
        public const float DSFXCOMPRESSOR_THRESHOLD_MAX = 0.0f;
        public const float DSFXCOMPRESSOR_RATIO_MIN = 1.0f;
        public const float DSFXCOMPRESSOR_RATIO_MAX = 100.0f;
        public const float DSFXCOMPRESSOR_PREDELAY_MIN = 0.0f;
        public const float DSFXCOMPRESSOR_PREDELAY_MAX = 4.0f;
        public const float DSFXPARAMEQ_CENTER_MIN = 80.0f;
        public const float DSFXPARAMEQ_CENTER_MAX = 16000.0f;
        public const float DSFXPARAMEQ_BANDWIDTH_MIN = 1.0f;
        public const float DSFXPARAMEQ_BANDWIDTH_MAX = 36.0f;
        public const float DSFXPARAMEQ_GAIN_MIN = -15.0f;
        public const float DSFXPARAMEQ_GAIN_MAX = 15.0f;
        public const int DSFX_I3DL2REVERB_ROOM_MIN = -10000;
        public const int DSFX_I3DL2REVERB_ROOM_MAX = 0;
        public const int DSFX_I3DL2REVERB_ROOM_DEFAULT = -1000;
        public const int DSFX_I3DL2REVERB_ROOMHF_MIN = -10000;
        public const int DSFX_I3DL2REVERB_ROOMHF_MAX = 0;
        public const int DSFX_I3DL2REVERB_ROOMHF_DEFAULT = -100;
        public const float DSFX_I3DL2REVERB_ROOMROLLOFFFACTOR_MIN = 0.0f;
        public const float DSFX_I3DL2REVERB_ROOMROLLOFFFACTOR_MAX = 10.0f;
        public const float DSFX_I3DL2REVERB_ROOMROLLOFFFACTOR_DEFAULT = 0.0f;
        public const float DSFX_I3DL2REVERB_DECAYTIME_MIN = 0.1f;
        public const float DSFX_I3DL2REVERB_DECAYTIME_MAX = 20.0f;
        public const float DSFX_I3DL2REVERB_DECAYTIME_DEFAULT = 1.49f;
        public const float DSFX_I3DL2REVERB_DECAYHFRATIO_MIN = 0.1f;
        public const float DSFX_I3DL2REVERB_DECAYHFRATIO_MAX = 2.0f;
        public const float DSFX_I3DL2REVERB_DECAYHFRATIO_DEFAULT = 0.83f;
        public const int DSFX_I3DL2REVERB_REFLECTIONS_MIN = -10000;
        public const int DSFX_I3DL2REVERB_REFLECTIONS_MAX = 1000;
        public const int DSFX_I3DL2REVERB_REFLECTIONS_DEFAULT = -2602;
        public const float DSFX_I3DL2REVERB_REFLECTIONSDELAY_MIN = 0.0f;
        public const float DSFX_I3DL2REVERB_REFLECTIONSDELAY_MAX = 0.3f;
        public const float DSFX_I3DL2REVERB_REFLECTIONSDELAY_DEFAULT = 0.007f;
        public const int DSFX_I3DL2REVERB_REVERB_MIN = -10000;
        public const int DSFX_I3DL2REVERB_REVERB_MAX = 2000;
        public const int DSFX_I3DL2REVERB_REVERB_DEFAULT = 200;
        public const float DSFX_I3DL2REVERB_REVERBDELAY_MIN = 0.0f;
        public const float DSFX_I3DL2REVERB_REVERBDELAY_MAX = 0.1f;
        public const float DSFX_I3DL2REVERB_REVERBDELAY_DEFAULT = 0.011f;
        public const float DSFX_I3DL2REVERB_DIFFUSION_MIN = 0.0f;
        public const float DSFX_I3DL2REVERB_DIFFUSION_MAX = 100.0f;
        public const float DSFX_I3DL2REVERB_DIFFUSION_DEFAULT = 100.0f;
        public const float DSFX_I3DL2REVERB_DENSITY_MIN = 0.0f;
        public const float DSFX_I3DL2REVERB_DENSITY_MAX = 100.0f;
        public const float DSFX_I3DL2REVERB_DENSITY_DEFAULT = 100.0f;
        public const float DSFX_I3DL2REVERB_HFREFERENCE_MIN = 20.0f;
        public const float DSFX_I3DL2REVERB_HFREFERENCE_MAX = 20000.0f;
        public const float DSFX_I3DL2REVERB_HFREFERENCE_DEFAULT = 5000.0f;
        public const int DSFX_I3DL2REVERB_QUALITY_MIN = 0;
        public const int DSFX_I3DL2REVERB_QUALITY_MAX = 3;
        public const int DSFX_I3DL2REVERB_QUALITY_DEFAULT = 2;
        public const float DSFX_WAVESREVERB_INGAIN_MIN = -96.0f;
        public const float DSFX_WAVESREVERB_INGAIN_MAX = 0.0f;
        public const float DSFX_WAVESREVERB_INGAIN_DEFAULT = 0.0f;
        public const float DSFX_WAVESREVERB_REVERBMIX_MIN = -96.0f;
        public const float DSFX_WAVESREVERB_REVERBMIX_MAX = 0.0f;
        public const float DSFX_WAVESREVERB_REVERBMIX_DEFAULT = 0.0f;
        public const float DSFX_WAVESREVERB_REVERBTIME_MIN = 0.001f;
        public const float DSFX_WAVESREVERB_REVERBTIME_MAX = 3000.0f;
        public const float DSFX_WAVESREVERB_REVERBTIME_DEFAULT = 1000.0f;
        public const float DSFX_WAVESREVERB_HIGHFREQRTRATIO_MIN = 0.001f;
        public const float DSFX_WAVESREVERB_HIGHFREQRTRATIO_MAX = 0.999f;
        public const float DSFX_WAVESREVERB_HIGHFREQRTRATIO_DEFAULT = 0.001f;
        // These match the AEC_MODE_* constants in the DDK's ksmedia.h file
        public const int DSCFX_AEC_MODE_PASS_THROUGH = 0x0;
        public const int DSCFX_AEC_MODE_HALF_DUPLEX = 0x1;
        public const int DSCFX_AEC_MODE_FULL_DUPLEX = 0x2;
        // These match the AEC_STATUS_* constants in ksmedia.h
        public const int DSCFX_AEC_STATUS_HISTORY_UNINITIALIZED = 0x0;
        public const int DSCFX_AEC_STATUS_HISTORY_CONTINUOUSLY_CONVERGED = 0x1;
        public const int DSCFX_AEC_STATUS_HISTORY_PREVIOUSLY_DIVERGED = 0x2;
        public const int DSCFX_AEC_STATUS_CURRENTLY_CONVERGED = 0x8;
        // ReSharper restore InconsistentNaming
    }
}