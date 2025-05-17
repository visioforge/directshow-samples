// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodecAPI.cs" company="VisioForge">
//   VisioForge (c) 2012
// </copyright>
// <summary>
//   ICodecAPI GUIDs and enums.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VisioForge.DirectShowLib
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security;

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("901db4c7-31ce-41a2-85dc-8fa0bf41b8da"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICodecAPI
    {
        [PreserveSig]
        int IsSupported([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api);

        [PreserveSig]
        int IsModifiable([In] Guid Api);

        [PreserveSig]
        int GetParameterRange(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api,
            [Out] out object ValueMin,
            [Out] out object ValueMax,
            [Out] out object SteppingDelta
            );

        [PreserveSig]
        int GetParameterValues(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api,
            [Out] out object[] Values,
            [Out] out int ValuesCount
            );

        [PreserveSig]
        int GetDefaultValue(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api,
            [Out] out object Value
            );

        [PreserveSig]
        int GetValue(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api,
            [Out] out object Value
            );

        [PreserveSig]
        int SetValue(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api,
            //[In] ref object Value
            [In] IntPtr Value
            //[In] COM.Variant Value
            );

        [PreserveSig]
        int RegisterForEvent(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api,
            [In] IntPtr userData
            );

        [PreserveSig]
        int UnregisterForEvent(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api
            );

        [PreserveSig]
        int SetAllDefaults();

        [PreserveSig]
        int SetValueWithNotify(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid Api,
            [In] object Value,
            [Out] out Guid[] ChangedParam,
            [Out] int ChangedParamCount
            );

        [PreserveSig]
        int SetAllDefaultsWithNotify(
            [Out] out Guid[] ChangedParam,
            [Out] out int ChangedParamCount
            );

        [PreserveSig]
        int GetAllSettings(
            [In] IStream pStream
            );

        [PreserveSig]
        int SetAllSettings(
            [In] IStream pStream
            );

        [PreserveSig]
        int SetAllSettingsWithNotify(
            [In] IStream pStream,
            [Out] out Guid[] ChangedParam,
            [Out] out int ChangedParamCount
            );
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    SuppressUnmanagedCodeSecurity, Guid("901db4c7-31ce-41a2-85dc-8fa0bf41b8da")]
    internal interface ICodecAPI2
    {
        [PreserveSig]
        int IsSupported([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api);
        [PreserveSig]
        int IsModifiable([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api);
        [PreserveSig]
        int GetParameterRange([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api, out object ValueMin, out object ValueMax, out object SteppingDelta);
        [PreserveSig]
        int GetParameterValues([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api, out IntPtr ip, out int ValuesCount);
        [PreserveSig]
        int GetDefaultValue([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api, [MarshalAs(UnmanagedType.Struct)] out object Value);
        [PreserveSig]
        int GetValue([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api, [MarshalAs(UnmanagedType.Struct)] out object Value);
        [PreserveSig]
        int SetValue([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api, [In] ref object Value);
        [PreserveSig]
        int RegisterForEvent([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api, [In] IntPtr userData);
        [PreserveSig]
        int UnregisterForEvent([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api);
        [PreserveSig]
        int SetAllDefaults();
        [PreserveSig]
        int SetValueWithNotify([In, MarshalAs(UnmanagedType.LPStruct)] Guid Api, [In] object Value, out Guid[] ChangedParam, out int ChangedParamCount);
        [PreserveSig]
        int SetAllDefaultsWithNotify(out Guid[] ChangedParam, out int ChangedParamCount);
        [PreserveSig]
        int GetAllSettings([In] IStream pStream);
        [PreserveSig]
        int SetAllSettings([In] IStream pStream);
        [PreserveSig]
        int SetAllSettingsWithNotify([In] IStream pStream, out Guid[] ChangedParam, out int ChangedParamCount);
    }




    /// <summary>
    /// ICodecAPI GUIDs and enums.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S4663:Comments should not be empty", Justification = "<Pending>")]
    internal static class CodecAPI
    {
        public const string AVEncAudioMeanBitRate = "{921295bb-4fca-4679-aab8-9e2a1d753384}";

        public const string AVEncCommonFormatConstraint = "{57cbb9b8-116f-4951-b40c-c2a035ed8f17}";
        public const string AVEncCommonFormatUnSpecified = "{af46a35a-6024-4525-a48a-094b97f5b3c2}";
        public const string AVEncCommonFormatDVD_V = "{cc9598c4-e7fe-451d-b1ca-761bc840b7f3}";
        public const string AVEncCommonFormatDVD_DashVR = "{e55199d6-044c-4dae-a488-531ed306235b}";
        public const string AVEncCommonFormatDVD_PlusVR = "{e74c6f2e-ec37-478d-9af4-a5e135b6271c}";
        public const string AVEncCommonFormatVCD = "{95035bf7-9d90-40ff-ad5c-5cf8cf71ca1d}";
        public const string AVEncCommonFormatSVCD = "{51d85818-8220-448c-8066-d69bed16c9ad}";
        public const string AVEncCommonFormatATSC = "{8d7b897c-a019-4670-aa76-2edcac7ac296}";
        public const string AVEncCommonFormatDVB = "{71830d8f-6c33-430d-844b-c2705baae6db}";
        public const string AVEncCommonFormatMP3 = "{349733cd-eb08-4dc2-8197-e49835ef828b}";
        public const string AVEncCommonFormatHighMAT = "{1eabe760-fb2b-4928-90d1-78db88eee889}";
        public const string AVEncCommonFormatHighMPV = "{a2d25db8-b8f9-42c2-8bc7-0b93cf604788}";
        public const string AVEncCodecType = "{08af4ac1-f3f2-4c74-9dcf-37f2ec79f826}";
        public const string AVEncMPEG1Video = "{c8dafefe-da1e-4774-b27d-11830c16b1fe}";
        public const string AVEncMPEG2Video = "{046dc19a-6677-4aaa-a31d-c1ab716f4560}";
        public const string AVEncMPEG1Audio = "{d4dd1362-cd4a-4cd6-8138-b94db4542b04}";
        public const string AVEncMPEG2Audio = "{ee4cbb1f-9c3f-4770-92b5-fcb7c2a8d381}";
        public const string AVEncWMV = "{4e0fef9b-1d43-41bd-b8bd-4d7bf7457a2a}";
        public const string AVEndMPEG4Video = "{dd37b12a-9503-4f8b-b8d0-324a00c0a1cf}";
        public const string AVEncH264Video = "{95044eab-31b3-47de-8e75-38a42bb03e28}";
        public const string AVEncDV = "{09b769c7-3329-44fb-8954-fa30937d3d5a}";
        public const string AVEncWMAPro = "{1955f90c-33f7-4a68-ab81-53f5657125c4}";
        public const string AVEncWMALossless = "{55ca7265-23d8-4761-9031-b74fbe12f4c1}";
        public const string AVEncWMAVoice = "{13ed18cb-50e8-4276-a288-a6aa228382d9}";
        public const string AVEncDolbyDigitalPro = "{f5be76cc-0ff8-40eb-9cb1-bba94004d44f}";
        public const string AVEncDolbyDigitalConsumer = "{c1a7bf6c-0059-4bfa-94ef-ef747a768d52}";
        public const string AVEncDolbyDigitalPlus = "{698d1b80-f7dd-415c-971c-42492a2056c6}";
        public const string AVEncDTSHD = "{2052e630-469d-4bfb-80ca-1d656e7e918f}";
        public const string AVEncDTS = "{45fbcaa2-5e6e-4ab0-8893-5903bee93acf}";
        public const string AVEncMLP = "{05f73e29-f0d1-431e-a41c-a47432ec5a66}";
        public const string AVEncPCM = "{844be7f4-26cf-4779-b386-cc05d187990c}";
        public const string AVEncSDDS = "{1dc1b82f-11c8-4c71-b7b6-ee3eb9bc2b94}";
        public const string AVEncCommonRateControlMode = "{1c0608e9-370c-4710-8a58-cb6181c42423}"; //  AVEncCommonRateControlMode (UINT32)
        public const string AVEncCommonLowLatency = "{9d3ecd55-89e8-490a-970a-0c9548d5a56e}"; //  AVEncCommonLowLatency (BOOL)
        public const string AVEncCommonMultipassMode = "{22533d4c-47e1-41b5-9352-a2b7780e7ac4}"; //  AVEncCommonMultipassMode (UINT32)
        public const string AVEncCommonPassStart = "{6a67739f-4eb5-4385-9928-f276a939ef95}"; //  AVEncCommonPassStart (UINT32)
        public const string AVEncCommonPassEnd = "{0e3d01bc-c85c-467d-8b60-c41012ee3bf6}"; //  AVEncCommonPassEnd (UINT32)
        public const string AVEncCommonRealTime = "{143a0ff6-a131-43da-b81e-98fbb8ec378e}"; //  AVEncCommonRealTime (BOOL)
        public const string AVEncCommonQuality = "{fcbf57a3-7ea5-4b0c-9644-69b40c39c391}"; //  AVEncCommonQuality (UINT32)
        public const string AVEncCommonQualityVsSpeed = "{98332df8-03cd-476b-89fa-3f9e442dec9f}"; //  AVEncCommonQualityVsSpeed (UINT32)
        public const string AVEncCommonMeanBitRate = "{f7222374-2144-4815-b550-a37f8e12ee52}"; //  AVEncCommonMeanBitRate (UINT32)
        public const string AVEncCommonMeanBitRateInterval = "{bfaa2f0c-cb82-4bc0-8474-f06a8a0d0258}"; //  AVEncCommonMeanBitRateInterval (UINT64)
        public const string AVEncCommonMaxBitRate = "{9651eae4-39b9-4ebf-85ef-d7f444ec7465}"; //  AVEncCommonMaxBitRate (UINT32)
        public const string AVEncCommonMinBitRate = "{101405b2-2083-4034-a806-efbeddd7c9ff}"; //  AVEncCommonMinBitRate (UINT32)
        public const string AVEncCommonBufferSize = "{0db96574-b6a4-4c8b-8106-3773de0310cd}"; //  AVEncCommonBufferSize (UINT32)
        public const string AVEncCommonBufferInLevel = "{d9c5c8db-fc74-4064-94e9-cd19f947ed45}"; //  AVEncCommonBufferInLevel (UINT32)
        public const string AVEncCommonBufferOutLevel = "{ccae7f49-d0bc-4e3d-a57e-fb5740140069}"; //  AVEncCommonBufferOutLevel (UINT32)
        public const string AVEncCommonStreamEndHandling = "{6aad30af-6ba8-4ccc-8fca-18d19beaeb1c}"; /* AVEncCommonStreamEndHandling (UINT32)*/public const string AVEncStatCommonCompletedPasses = "{3e5de533-9df7-438c-854f-9f7dd3683d34}";
        public const string AVEncVideoOutputFrameRate = "{ea85e7c3-9567-4d99-87c4-02c1c278ca7c}"; /**//* Common Video Parameters*//**///  AVEncVideoOutputFrameRate (UINT32)
        public const string AVEncVideoOutputFrameRateConversion = "{8c068bf4-369a-4ba3-82fd-b2518fb3396e}"; //  AVEncVideoOutputFrameRateConversion (UINT32)
        public const string AVEncVideoPixelAspectRatio = "{3cdc718f-b3e9-4eb6-a57f-cf1f1b321b87}"; //  AVEncVideoPixelAspectRatio (UINT32 as UINT16/UNIT16) <---- You have WORD in the doc
        public const string AVEncVideoForceSourceScanType = "{1ef2065f-058a-4765-a4fc-8a864c103012}"; //  AVEncVideoForceSourceScanType (UINT32)
        public const string AVEncVideoNoOfFieldsToEncode = "{61e4bbe2-4ee0-40e7-80ab-51ddeebe6291}"; //  AVEncVideoNoOfFieldsToEncode (UINT64)
        public const string AVEncVideoNoOfFieldsToSkip = "{a97e1240-1427-4c16-a7f7-3dcfd8ba4cc5}"; //  AVEncVideoNoOfFieldsToSkip (UINT64)
        public const string AVEncVideoEncodeDimension = "{1074df28-7e0f-47a4-a453-cdd73870f5ce}"; //  AVEncVideoEncodeDimension (UINT32)
        public const string AVEncVideoEncodeOffsetOrigin = "{6bc098fe-a71a-4454-852e-4d2ddeb2cd24}"; //  AVEncVideoEncodeOffsetOrigin (UINT32)
        public const string AVEncVideoDisplayDimension = "{de053668-f4ec-47a9-86d0-836770f0c1d5}"; //  AVEncVideoDisplayDimension (UINT32)
        public const string AVEncVideoOutputScanType = "{460b5576-842e-49ab-a62d-b36f7312c9db}"; //  AVEncVideoOutputScanType (UINT32)
        public const string AVEncVideoInverseTelecineEnable = "{2ea9098b-e76d-4ccd-a030-d3b889c1b64c}"; //  AVEncVideoInverseTelecineEnable (BOOL)
        public const string AVEncVideoInverseTelecineThreshold = "{40247d84-e895-497f-b44c-b74560acfe27}"; //  AVEncVideoInverseTelecineThreshold (UINT32)
        public const string AVEncVideoSourceFilmContent = "{1791c64b-ccfc-4827-a0ed-2557793b2b1c}"; //  AVEncVideoSourceFilmContent (UINT32)
        public const string AVEncVideoSourceIsBW = "{42ffc49b-1812-4fdc-8d24-7054c521e6eb}"; //  AVEncVideoSourceIsBW (BOOL)
        public const string AVEncVideoFieldSwap = "{fefd7569-4e0a-49f2-9f2b-360ea48c19a2}"; //  AVEncVideoFieldSwap (BOOL)
        public const string AVEncVideoInputChromaResolution = "{bb0cec33-16f1-47b0-8a88-37815bee1739}"; /* AVEncVideoInputChromaResolution (UINT32)*///  AVEncVideoOutputChromaSubsamplingFormat (UINT32)
        public const string AVEncVideoOutputChromaResolution = "{6097b4c9-7c1d-4e64-bfcc-9e9765318ae7}";
        public const string AVEncVideoInputChromaSubsampling = "{a8e73a39-4435-4ec3-a6ea-98300f4b36f7}"; /* AVEncVideoInputChromaSubsampling (UINT32)*///  AVEncVideoOutputChromaSubsampling (UINT32)
        public const string AVEncVideoOutputChromaSubsampling = "{fa561c6c-7d17-44f0-83c9-32ed12e96343}";
        public const string AVEncVideoInputColorPrimaries = "{c24d783f-7ce6-4278-90ab-28a4f1e5f86c}"; /* AVEncVideoInputColorPrimaries (UINT32)*///  AVEncVideoOutputColorPrimaries (UINT32)
        public const string AVEncVideoOutputColorPrimaries = "{be95907c-9d04-4921-8985-a6d6d87d1a6c}";
        public const string AVEncVideoInputColorTransferFunction = "{8c056111-a9c3-4b08-a0a0-ce13f8a27c75}"; /* AVEncVideoInputColorTransferFunction (UINT32)*///  AVEncVideoOutputColorTransferFunction (UINT32)
        public const string AVEncVideoOutputColorTransferFunction = "{4a7f884a-ea11-460d-bf57-b88bc75900de}";
        public const string AVEncVideoInputColorTransferMatrix = "{52ed68b9-72d5-4089-958d-f5405d55081c}"; /* AVEncVideoInputColorTransferMatrix (UINT32)*///  AVEncVideoOutputColorTransferMatrix (UINT32)
        public const string AVEncVideoOutputColorTransferMatrix = "{a9b90444-af40-4310-8fbe-ed6d933f892b}";
        public const string AVEncVideoInputColorLighting = "{46a99549-0015-4a45-9c30-1d5cfa258316}"; /* AVEncVideoInputColorLighting (UINT32)*///  AVEncVideoOutputColorLighting (UINT32)
        public const string AVEncVideoOutputColorLighting = "{0e5aaac6-ace6-4c5c-998e-1a8c9c6c0f89}";
        public const string AVEncVideoInputColorNominalRange = "{16cf25c6-a2a6-48e9-ae80-21aec41d427e}"; /* AVEncVideoInputColorNominalRange (UINT32)*///  AVEncVideoOutputColorNominalRange (UINT32)
        public const string AVEncVideoOutputColorNominalRange = "{972835ed-87b5-4e95-9500-c73958566e54}";
        public const string AVEncInputVideoSystem = "{bede146d-b616-4dc7-92b2-f5d9fa9298f7}"; //  AVEncInputVideoSystem (UINT32)
        public const string AVEncVideoHeaderDropFrame = "{6ed9e124-7925-43fe-971b-e019f62222b4}"; //  AVEncVideoHeaderDropFrame (UINT32)
        public const string AVEncVideoHeaderHours = "{2acc7702-e2da-4158-bf9b-88880129d740}"; //  AVEncVideoHeaderHours (UINT32)
        public const string AVEncVideoHeaderMinutes = "{dc1a99ce-0307-408b-880b-b8348ee8ca7f}"; //  AVEncVideoHeaderMinutes (UINT32)
        public const string AVEncVideoHeaderSeconds = "{4a2e1a05-a780-4f58-8120-9a449d69656b}"; //  AVEncVideoHeaderSeconds (UINT32)
        public const string AVEncVideoHeaderFrames = "{afd5f567-5c1b-4adc-bdaf-735610381436}"; //  AVEncVideoHeaderFrames (UINT32)
        public const string AVEncVideoDefaultUpperFieldDominant = "{810167c4-0bc1-47ca-8fc2-57055a1474a5}"; //  AVEncVideoDefaultUpperFieldDominant (BOOL)
        public const string AVEncVideoCBRMotionTradeoff = "{0d49451e-18d5-4367-a4ef-3240df1693c4}"; //  AVEncVideoCBRMotionTradeoff (UINT32)
        public const string AVEncVideoCodedVideoAccessUnitSize = "{b4b10c15-14a7-4ce8-b173-dc90a0b4fcdb}"; //  AVEncVideoCodedVideoAccessUnitSize (UINT32)
        public const string AVEncVideoMaxKeyframeDistance = "{2987123a-ba93-4704-b489-ec1e5f25292c}"; //  AVEncVideoMaxKeyframeDistance (UINT32)
        public const string AVEncStatVideoOutputFrameRate = "{be747849-9ab4-4a63-98fe-f143f04f8ee9}"; /**//* Common Post-Encode Video Statistical Parameters*//**///  AVEncStatVideoOutputFrameRate (UINT32/UINT32)
        public const string AVEncStatVideoCodedFrames = "{d47f8d61-6f5a-4a26-bb9f-cd9518462bcd}"; //  AVEncStatVideoCodedFrames (UINT32)
        public const string AVEncStatVideoTotalFrames = "{fdaa9916-119a-4222-9ad6-3f7cab99cc8b}"; //  AVEncStatVideoTotalFrames (UINT32)
        public const string AVEncAudioIntervalToEncode = "{866e4b4d-725a-467c-bb01-b496b23b25f9}"; /**//* Common Audio Parameters*//**///  AVEncAudioIntervalToEncode (UINT64)
        public const string AVEncAudioIntervalToSkip = "{88c15f94-c38c-4796-a9e8-96e967983f26}"; //  AVEncAudioIntervalToSkip (UINT64)
        public const string AVEncAudioDualMono = "{3648126b-a3e8-4329-9b3a-5ce566a43bd3}"; /* AVEncAudioDualMono (UINT32) - Read/Write*//* Some audio encoders can encode 2 channel input as 'dual mono'. Use this*//* property to set the appropriate field in the bitstream header to indicate that the*//* 2 channel bitstream is or isn't dual mono.*///  For encoding MPEG audio use the DualChannel option in AVEncMPACodingMode instead
        public const string AVEncAudioMapDestChannel0 = "{bc5d0b60-df6a-4e16-9803-b82007a30c8d}"; //  AVEncAudioMapDestChannel0..15 (UINT32)
        public const string AVEncAudioMapDestChannel1 = "{bc5d0b61-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel2 = "{bc5d0b62-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel3 = "{bc5d0b63-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel4 = "{bc5d0b64-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel5 = "{bc5d0b65-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel6 = "{bc5d0b66-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel7 = "{bc5d0b67-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel8 = "{bc5d0b68-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel9 = "{bc5d0b69-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel10 = "{bc5d0b6a-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel11 = "{bc5d0b6b-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel12 = "{bc5d0b6c-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel13 = "{bc5d0b6d-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel14 = "{bc5d0b6e-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioMapDestChannel15 = "{bc5d0b6f-df6a-4e16-9803-b82007a30c8d}";
        public const string AVEncAudioInputContent = "{3e226c2b-60b9-4a39-b00b-a7b40f70d566}"; //  AVEncAudioInputContent (UINT32) <---- You have type in the doc
        public const string AVEncStatAudioPeakPCMValue = "{dce7fd34-dc00-4c16-821b-35d9eb00fb1a}"; //  AVEncStatAudioPeakPCMValue (UINT32)
        public const string AVEncStatAudioAveragePCMValue = "{979272f8-d17f-4e32-bb73-4e731c68ba2d}"; //  AVEncStatAudioAveragePCMValue (UINT32)
        public const string AVEncStatAudioAverageBPS = "{ca6724db-7059-4351-8b43-f82198826a14}"; //  AVEncStatAudioAverageBPS (UINT32)
        public const string AVEncMPVGOPSize = "{95f31b26-95a4-41aa-9303-246a7fc6eef1}"; /**//* MPEG Video Encoding Interface*//**//**//* MPV Encoder Specific Parameters*//**///  AVEncMPVGOPSize (UINT32)
        public const string AVEncMPVGOPOpen = "{b1d5d4a6-3300-49b1-ae61-a09937ab0e49}"; //  AVEncMPVGOPOpen (BOOL)
        public const string AVEncMPVDefaultBPictureCount = "{8d390aac-dc5c-4200-b57f-814d04babab2}"; //  AVEncMPVDefaultBPictureCount (UINT32)
        public const string AVEncMPVProfile = "{dabb534a-1d99-4284-975a-d90e2239baa1}"; //  AVEncMPVProfile (UINT32) <---- You have GUID in the doc
        public const string AVEncMPVLevel = "{6ee40c40-a60c-41ef-8f50-37c2249e2cb3}"; //  AVEncMPVLevel (UINT32) <---- You have GUID in the doc
        public const string AVEncMPVFrameFieldMode = "{acb5de96-7b93-4c2f-8825-b0295fa93bf4}"; //  AVEncMPVFrameFieldMode (UINT32)
        public const string AVEncMPVAddSeqEndCode = "{a823178f-57df-4c7a-b8fd-e5ec8887708d}"; //  AVEncMPVAddSeqEndCode (BOOL)
        public const string AVEncMPVGOPSInSeq = "{993410d4-2691-4192-9978-98dc2603669f}"; //  AVEncMPVGOPSInSeq (UINT32)
        public const string AVEncMPVUseConcealmentMotionVectors = "{ec770cf3-6908-4b4b-aa30-7fb986214fea}"; //  AVEncMPVUseConcealmentMotionVectors (BOOL)
        public const string AVEncMPVSceneDetection = "{552799f1-db4c-405b-8a3a-c93f2d0674dc}"; //  AVEncMPVSceneDetection (UINT32)
        public const string AVEncMPVGenerateHeaderSeqExt = "{d5e78611-082d-4e6b-98af-0f51ab139222}"; //  AVEncMPVGenerateHeaderSeqExt (BOOL)
        public const string AVEncMPVGenerateHeaderSeqDispExt = "{6437aa6f-5a3c-4de9-8a16-53d9c4ad326f}"; //  AVEncMPVGenerateHeaderSeqDispExt (BOOL)
        public const string AVEncMPVGenerateHeaderPicExt = "{1b8464ab-944f-45f0-b74e-3a58dad11f37}"; //  AVEncMPVGenerateHeaderPicExt (BOOL)
        public const string AVEncMPVGenerateHeaderPicDispExt = "{c6412f84-c03f-4f40-a00c-4293df8395bb}"; //  AVEncMPVGenerateHeaderPicDispExt (BOOL)
        public const string AVEncMPVGenerateHeaderSeqScaleExt = "{0722d62f-dd59-4a86-9cd5-644f8e2653d8}"; //  AVEncMPVGenerateHeaderSeqScaleExt (BOOL)
        public const string AVEncMPVScanPattern = "{7f8a478e-7bbb-4ae2-b2fc-96d17fc4a2d6}"; //  AVEncMPVScanPattern (UINT32)
        public const string AVEncMPVIntraDCPrecision = "{a0116151-cbc8-4af3-97dc-d00cceb82d79}"; //  AVEncMPVIntraDCPrecision (UINT32)
        public const string AVEncMPVQScaleType = "{2b79ebb7-f484-4af7-bb58-a2a188c5cbbe}"; //  AVEncMPVQScaleType (UINT32)
        public const string AVEncMPVIntraVLCTable = "{a2b83ff5-1a99-405a-af95-c5997d558d3a}"; //  AVEncMPVIntraVLCTable (UINT32)
        public const string AVEncMPVQuantMatrixIntra = "{9bea04f3-6621-442c-8ba1-3ac378979698}"; //  AVEncMPVQuantMatrixIntra (BYTE[64] encoded as a string of 128 hex digits)
        public const string AVEncMPVQuantMatrixNonIntra = "{87f441d8-0997-4beb-a08e-8573d409cf75}"; //  AVEncMPVQuantMatrixNonIntra (BYTE[64] encoded as a string of 128 hex digits)
        public const string AVEncMPVQuantMatrixChromaIntra = "{9eb9ecd4-018d-4ffd-8f2d-39e49f07b17a}"; //  AVEncMPVQuantMatrixChromaIntra (BYTE[64] encoded as a string of 128 hex digits)
        public const string AVEncMPVQuantMatrixChromaNonIntra = "{1415b6b1-362a-4338-ba9a-1ef58703c05b}"; //  AVEncMPVQuantMatrixChromaNonIntra (BYTE[64] encoded as a string of 128 hex digits)
        public const string AVEncMPALayer = "{9d377230-f91b-453d-9ce0-78445414c22d}"; /**//* MPEG1 Audio Encoding Interface*//**//**//* MPEG1 Audio Specific Parameters*//**///  AVEncMPALayer (UINT)
        public const string AVEncMPACodingMode = "{b16ade03-4b93-43d7-a550-90b4fe224537}"; //  AVEncMPACodingMode (UINT)
        public const string AVEncMPACopyright = "{a6ae762a-d0a9-4454-b8ef-f2dbeefdd3bd}"; /* AVEncMPACopyright (BOOL) - default state to encode into the stream (may be overridden by input)*//* 1 (true)  - copyright protected*///  0 (false) - not copyright protected
        public const string AVEncMPAOriginalBitstream = "{3cfb7855-9cc9-47ff-b829-b36786c92346}"; /* AVEncMPAOriginalBitstream (BOOL) - default value to encode into the stream (may be overridden by input)*//* 1 (true)  - for original bitstream*///  0 (false) - for copy bitstream
        public const string AVEncMPAEnableRedundancyProtection = "{5e54b09e-b2e7-4973-a89b-0b3650a3beda}"; /* AVEncMPAEnableRedundancyProtection (BOOL)*//* 1 (true)  -  Redundancy should be added to facilitate error detection and concealment (CRC)*///  0 (false) -  No redundancy should be added
        public const string AVEncMPApublicUserBit = "{afa505ce-c1e3-4e3d-851b-61b700e5e6cc}"; //  AVEncMPApublicUserBit (UINT) - User data bit value to encode in the stream
        public const string AVEncMPAEmphasisType = "{2d59fcda-bf4e-4ed6-b5df-5b03b36b0a1f}"; /* AVEncMPAEmphasisType (UINT)*///  Indicates type of de-emphasis filter to be used
        public const string AVEncDDService = "{d2e1bec7-5172-4d2a-a50e-2f3b82b1ddf8}"; //  AVEncDDService (UINT)
        public const string AVEncDDDialogNormalization = "{d7055acf-f125-437d-a704-79c79f0404a8}"; //  AVEncDDDialogNormalization (UINT32)
        public const string AVEncDDCentreDownMixLevel = "{e285072c-c958-4a81-afd2-e5e0daf1b148}"; //  AVEncDDCentreDownMixLevel (UINT32)
        public const string AVEncDDSurroundDownMixLevel = "{7b20d6e5-0bcf-4273-a487-506b047997e9}"; //  AVEncDDSurroundDownMixLevel (UINT32)
        public const string AVEncDDProductionInfoExists = "{b0b7fe5f-b6ab-4f40-964d-8d91f17c19e8}"; //  AVEncDDProductionInfoExists (BOOL)
        public const string AVEncDDProductionRoomType = "{dad7ad60-23d8-4ab7-a284-556986d8a6fe}"; //  AVEncDDProductionRoomType (UINT32)
        public const string AVEncDDProductionMixLevel = "{301d103a-cbf9-4776-8899-7c15b461ab26}"; //  AVEncDDProductionMixLevel (UINT32)
        public const string AVEncDDCopyright = "{8694f076-cd75-481d-a5c6-a904dcc828f0}"; //  AVEncDDCopyright (BOOL)
        public const string AVEncDDOriginalBitstream = "{966ae800-5bd3-4ff9-95b9-d30566273856}"; //  AVEncDDOriginalBitstream (BOOL)
        public const string AVEncDDDigitalDeemphasis = "{e024a2c2-947c-45ac-87d8-f1030c5c0082}"; //  AVEncDDDigitalDeemphasis (BOOL)
        public const string AVEncDDDCHighPassFilter = "{9565239f-861c-4ac8-bfda-e00cb4db8548}"; //  AVEncDDDCHighPassFilter (BOOL)
        public const string AVEncDDChannelBWLowPassFilter = "{e197821d-d2e7-43e2-ad2c-00582f518545}"; //  AVEncDDChannelBWLowPassFilter (BOOL)
        public const string AVEncDDLFELowPassFilter = "{d3b80f6f-9d15-45e5-91be-019c3fab1f01}"; //  AVEncDDLFELowPassFilter (BOOL)
        public const string AVEncDDSurround90DegreeePhaseShift = "{25ecec9d-3553-42c0-bb56-d25792104f80}"; //  AVEncDDSurround90DegreeePhaseShift (BOOL)
        public const string AVEncDDSurround3dBAttenuation = "{4d43b99d-31e2-48b9-bf2e-5cbf1a572784}"; //  AVEncDDSurround3dBAttenuation (BOOL)
        public const string AVEncDDDynamicRangeCompressionControl = "{cfc2ff6d-79b8-4b8d-a8aa-a0c9bd1c2940}"; //  AVEncDDDynamicRangeCompressionControl (UINT32)
        public const string AVEncDDRFPreEmphasisFilter = "{21af44c0-244e-4f3d-a2cc-3d3068b2e73f}"; //  AVEncDDRFPreEmphasisFilter (BOOL)
        public const string AVEncDDSurroundExMode = "{91607cee-dbdd-4eb6-bca2-aadfafa3dd68}"; //  AVEncDDSurroundExMode (UINT32)
        public const string AVEncDDPreferredStereoDownMixMode = "{7f4e6b31-9185-403d-b0a2-763743e6f063}"; //  AVEncDDPreferredStereoDownMixMode (UINT32)
        public const string AVEncDDLtRtCenterMixLvl_x10 = "{dca128a2-491f-4600-b2da-76e3344b4197}"; //  AVEncDDLtRtCenterMixLvl_x10 (INT32)
        public const string AVEncDDLtRtSurroundMixLvl_x10 = "{212246c7-3d2c-4dfa-bc21-652a9098690d}"; //  AVEncDDLtRtSurroundMixLvl_x10 (INT32)
        public const string AVEncDDLoRoCenterMixLvl_x10 = "{1cfba222-25b3-4bf4-9bfd-e7111267858c}"; //  AVEncDDLoRoCenterMixLvl (INT32)
        public const string AVEncDDLoRoSurroundMixLvl_x10 = "{e725cff6-eb56-40c7-8450-2b9367e91555}"; //  AVEncDDLoRoSurroundMixLvl_x10 (INT32)
        public const string AVEncDDAtoDConverterType = "{719f9612-81a1-47e0-9a05-d94ad5fca948}"; //  AVEncDDAtoDConverterType (UINT32)
        public const string AVEncDDHeadphoneMode = "{4052dbec-52f5-42f5-9b00-d134b1341b9d}"; //  AVEncDDHeadphoneMode (UINT32)
        public const string AVEncWMVKeyFrameDistance = "{5569055e-e268-4771-b83e-9555ea28aed3}"; //  AVEncWMVKeyFrameDistance (UINT32)
        public const string AVEncWMVInterlacedEncoding = "{e3d00f8a-c6f5-4e14-a588-0ec87a726f9b}"; //  AVEncWMVInterlacedEncoding (UINT32)
        public const string AVEncWMVDecoderComplexity = "{f32c0dab-f3cb-4217-b79f-8762768b5f67}"; //  AVEncWMVDecoderComplexity (UINT32)
        public const string AVEncWMVKeyFrameBufferLevelMarker = "{51ff1115-33ac-426c-a1b1-09321bdf96b4}"; //  AVEncWMVHasKeyFrameBufferLevelMarker (BOOL)
        public const string AVEncWMVProduceDummyFrames = "{d669d001-183c-42e3-a3ca-2f4586d2396c}"; //  AVEncWMVProduceDummyFrames (UINT32)
        public const string AVEncStatWMVCBAvg = "{6aa6229f-d602-4b9d-b68c-c1ad78884bef}"; /**//* WMV Post-Encode Statistical Parameters*//**///  AVEncStatWMVCBAvg (UINT32/UINT32)
        public const string AVEncStatWMVCBMax = "{e976bef8-00fe-44b4-b625-8f238bc03499}"; //  AVEncStatWMVCBMax (UINT32/UINT32)
        public const string AVEncStatWMVDecoderComplexityProfile = "{89e69fc3-0f9b-436c-974a-df821227c90d}"; //  AVEncStatWMVDecoderComplexityProfile (UINT32)
        public const string AVEncStatMPVSkippedEmptyFrames = "{32195fd3-590d-4812-a7ed-6d639a1f9711}"; //  AVEncStatMPVSkippedEmptyFrames (UINT32)
        public const string AVEncMP12PktzSTDBuffer = "{0b751bd0-819e-478c-9435-75208926b377}"; /**//* MPEG1/2 Multiplexer Interfaces*//**//**//* MPEG1/2 Packetizer Interface*//**//* Shared with Mux:*//* AVEncMP12MuxEarliestPTS (UINT32)*//* AVEncMP12MuxLargestPacketSize (UINT32)*//* AVEncMP12MuxSysSTDBufferBound (UINT32)*///  AVEncMP12PktzSTDBuffer (UINT32)
        public const string AVEncMP12PktzStreamID = "{c834d038-f5e8-4408-9b60-88f36493fedf}"; //  AVEncMP12PktzStreamID (UINT32)
        public const string AVEncMP12PktzInitialPTS = "{2a4f2065-9a63-4d20-ae22-0a1bc896a315}"; //  AVEncMP12PktzInitialPTS (UINT32)
        public const string AVEncMP12PktzPacketSize = "{ab71347a-1332-4dde-a0e5-ccf7da8a0f22}"; //  AVEncMP12PktzPacketSize (UINT32)
        public const string AVEncMP12PktzCopyright = "{c8f4b0c1-094c-43c7-8e68-a595405a6ef8}"; //  AVEncMP12PktzCopyright (BOOL)
        public const string AVEncMP12PktzOriginal = "{6b178416-31b9-4964-94cb-6bff866cdf83}"; //  AVEncMP12PktzOriginal (BOOL)
        public const string AVEncMP12MuxPacketOverhead = "{e40bd720-3955-4453-acf9-b79132a38fa0}"; /**//* MPEG1/2 Multiplexer Interface*//**///  AVEncMP12MuxPacketOverhead (UINT32)
        public const string AVEncMP12MuxNumStreams = "{f7164a41-dced-4659-a8f2-fb693f2a4cd0}"; //  AVEncMP12MuxNumStreams (UINT32)
        public const string AVEncMP12MuxEarliestPTS = "{157232b6-f809-474e-9464-a7f93014a817}"; //  AVEncMP12MuxEarliestPTS (UINT32)
        public const string AVEncMP12MuxLargestPacketSize = "{35ceb711-f461-4b92-a4ef-17b6841ed254}"; //  AVEncMP12MuxLargestPacketSize (UINT32)
        public const string AVEncMP12MuxInitialSCR = "{3433ad21-1b91-4a0b-b190-2b77063b63a4}"; //  AVEncMP12MuxInitialSCR (UINT32)
        public const string AVEncMP12MuxMuxRate = "{ee047c72-4bdb-4a9d-8e21-41926c823da7}"; //  AVEncMP12MuxMuxRate (UINT32)
        public const string AVEncMP12MuxPackSize = "{f916053a-1ce8-4faf-aa0b-ba31c80034b8}"; //  AVEncMP12MuxPackSize (UINT32)
        public const string AVEncMP12MuxSysSTDBufferBound = "{35746903-b545-43e7-bb35-c5e0a7d5093c}"; //  AVEncMP12MuxSysSTDBufferBound (UINT32)
        public const string AVEncMP12MuxSysRateBound = "{05f0428a-ee30-489d-ae28-205c72446710}"; //  AVEncMP12MuxSysRateBound (UINT32)
        public const string AVEncMP12MuxTargetPacketizer = "{d862212a-2015-45dd-9a32-1b3aa88205a0}"; //  AVEncMP12MuxTargetPacketizer (UINT32)
        public const string AVEncMP12MuxSysFixed = "{cefb987e-894f-452e-8f89-a4ef8cec063a}"; //  AVEncMP12MuxSysFixed (UINT32)
        public const string AVEncMP12MuxSysCSPS = "{7952ff45-9c0d-4822-bc82-8ad772e02993}"; //  AVEncMP12MuxSysCSPS (UINT32)
        public const string AVEncMP12MuxSysVideoLock = "{b8296408-2430-4d37-a2a1-95b3e435a91d}"; //  AVEncMP12MuxSysVideoLock (BOOL)
        public const string AVEncMP12MuxSysAudioLock = "{0fbb5752-1d43-47bf-bd79-f2293d8ce337}"; //  AVEncMP12MuxSysAudioLock (BOOL)
        public const string AVEncMP12MuxDVDNavPacks = "{c7607ced-8cf1-4a99-83a1-ee5461be3574}"; //  AVEncMP12MuxDVDNavPacks (BOOL)
        public const string AVDecCommonInputFormat = "{E5005239-BD89-4be3-9C0F-5DDE317988CC}"; /**//* Decoding Interface*//**///  format values are GUIDs as VARIANT BSTRs
        public const string AVDecCommonOutputFormat = "{3c790028-c0ce-4256-b1a2-1b0fc8b1dcdc}";
        public const string AVDecCommonMeanBitRate = "{59488217-007A-4f7a-8E41-5C48B1EAC5C6}"; //  AVDecCommonMeanBitRate - Mean bitrate in mbits/sec (UINT32)
        public const string AVDecCommonMeanBitRateInterval = "{0EE437C6-38A7-4c5c-944C-68AB42116B85}"; //  AVDecCommonMeanBitRateInterval - Mean bitrate interval (in 100ns) (UINT64)
        public const string GUID_AVDecAudioOutputFormat_PCM_Stereo_MatrixEncoded = "{696E1D30-548F-4036-825F-7026C60011BD}"; /**//* Audio Decoding Interface*//**//* Value GUIDS*//* The following 6 GUIDs are values of the AVDecCommonOutputFormat property*//**///  Stereo PCM output using matrix-encoded stereo down mix (aka Lt/Rt)
        public const string GUID_AVDecAudioOutputFormat_PCM = "{696E1D31-548F-4036-825F-7026C60011BD}"; /**///  Regular PCM output (any number of channels)
        public const string GUID_AVDecAudioOutputFormat_SPDIF_PCM = "{696E1D32-548F-4036-825F-7026C60011BD}"; /**//* SPDIF PCM (IEC 60958) stereo output. Type of stereo down mix should*///  be specified by the application.
        public const string GUID_AVDecAudioOutputFormat_SPDIF_Bitstream = "{696E1D33-548F-4036-825F-7026C60011BD}"; /**///  SPDIF bitstream (IEC 61937) output such as AC3 MPEG or DTS.
        public const string GUID_AVDecAudioOutputFormat_PCM_Headphones = "{696E1D34-548F-4036-825F-7026C60011BD}"; /**///  Stereo PCM output using regular stereo down mix (aka Lo/Ro)
        public const string GUID_AVDecAudioOutputFormat_PCM_Stereo_Auto = "{696E1D35-548F-4036-825F-7026C60011BD}"; /* Stereo PCM output using automatic selection of stereo down mix*//* mode (Lo/Ro or Lt/Rt). Use this when the input stream includes*//* information about the preferred downmix mode (such as Annex D of AC3).*///  Default down mix mode should be specified by the application.
        public const string AVDecVideoImageSize = "{5EE5747C-6801-4cab-AAF1-6248FA841BA4}"; /**//* Video Decoder properties*//**///  AVDecVideoImageSize (UINT32) - High UINT16 width low UINT16 height
        public const string AVDecVideoPixelAspectRatio = "{B0CF8245-F32D-41df-B02C-87BD304D12AB}"; //  AVDecVideoPixelAspectRatio (UINT32 as UINT16/UNIT16) - High UINT16 width low UINT16 height
        public const string AVDecVideoInputScanType = "{38477E1F-0EA7-42cd-8CD1-130CED57C580}"; /* AVDecVideoInputScanType (UINT32)*/public const string GUID_AVDecAudioInputWMA = "{C95E8DCF-4058-4204-8C42-CB24D91E4B9B}";
        public const string GUID_AVDecAudioInputWMAPro = "{0128B7C7-DA72-4fe3-BEF8-5C52E3557704}";
        public const string GUID_AVDecAudioInputDolby = "{8E4228A0-F000-4e0b-8F54-AB8D24AD61A2}";
        public const string GUID_AVDecAudioInputDTS = "{600BC0CA-6A1F-4e91-B241-1BBEB1CB19E0}";
        public const string GUID_AVDecAudioInputPCM = "{F2421DA5-BBB4-4cd5-A996-933C6B5D1347}";
        public const string GUID_AVDecAudioInputMPEG = "{91106F36-02C5-4f75-9719-3B7ABF75E1F6}";
        public const string AVDecAudioDualMono = "{4a52cda8-30f8-4216-be0f-ba0b2025921d}"; /* AVDecAudioDualMono (UINT32) - Read only*//* The input bitstream header might have a field indicating whether the 2-ch bitstream*//* is dual mono or not. Use this property to read this field.*//* If it's dual mono the application can set AVDecAudioDualMonoReproMode to determine*//* one of 4 reproduction modes*/public const string AVDecAudioDualMonoReproMode = "{a5106186-cc94-4bc9-8cd9-aa2f61f6807e}";
        public const string AVAudioChannelCount = "{1d3583c4-1583-474e-b71a-5ee463c198e4}"; /* AVAudioChannelCount (UINT32)*///  Total number of audio channels including LFE if it exists.
        public const string AVAudioChannelConfig = "{17f89cb3-c38d-4368-9ede-63b94d177f9f}"; /* AVAudioChannelConfig (UINT32)*///  A bit-wise OR of any number of type values specified by eAVAudioChannelConfig
        public const string AVAudioSampleRate = "{971d2723-1acb-42e7-855c-520a4b70a5f2}"; /* AVAudioSampleRate (UINT32)*///  In samples per second (Hz)
        public const string AVDDSurroundMode = "{99f2f386-98d1-4452-a163-abc78a6eb770}"; /**//* Dolby Digital(TM) Audio Specific Parameters*//**///  AVDDSurroundMode (UINT32) common to encoder/decoder
        public const string AVDecDDOperationalMode = "{d6d6c6d1-064e-4fdd-a40e-3ecbfcb7ebd0}"; //  AVDecDDOperationalMode (UINT32)
        public const string AVDecDDMatrixDecodingMode = "{ddc811a5-04ed-4bf3-a0ca-d00449f9355f}"; /* AVDecDDMatrixDecodingMode(UINT32)*//* A ProLogic decoder has a built-in auto-detection feature. When the Dolby Digital decoder*//* is set to the 6-channel output configuration and it is fed a 2/0 bit stream to decode it can*//* do one of the following:*//* a) decode the bit stream and output it on the two front channels (eAVDecDDMatrixDecodingMode_OFF)*//* b) decode the bit stream followed by ProLogic decoding to create 6-channels (eAVDecDDMatrixDecodingMode_ON).*//* c) the decoder will look at the Surround bit ('dsurmod') in the bit stream to determine whether*///     apply ProLogic decoding or not (eAVDecDDMatrixDecodingMode_AUTO).
        public const string AVDecDDDynamicRangeScaleHigh = "{50196c21-1f33-4af5-b296-11426d6c8789}"; /* AVDecDDDynamicRangeScaleHigh (UINT32)*//* Indicates what fraction of the dynamic range compression*//* to apply. Relevant for negative values of dynrng only.*//* Linear range 0-100 where:*//*   0 - No dynamic range compression (preserve full dynamic range)*///  100 - Apply full dynamic range compression
        public const string AVDecDDDynamicRangeScaleLow = "{044e62e4-11a5-42d5-a3b2-3bb2c7c2d7cf}"; /* AVDecDDDynamicRangeScaleLow (UINT32)*//* Indicates what fraction of the dynamic range compression*//* to apply. Relevant for positive values of dynrng only.*//* Linear range 0-100 where:*//*   0 - No dynamic range compression (preserve full dynamic range)*//* 100 - Apply full dynamic range compression*/

        internal enum eAVEncCommonRateControlMode
        {
            eAVEncCommonRateControlMode_CBR,
            eAVEncCommonRateControlMode_PeakConstrainedVBR,
            eAVEncCommonRateControlMode_UnconstrainedVBR,
            eAVEncCommonRateControlMode_Quality
        }

        internal enum eAVEncCommonStreamEndHandling
        {
            eAVEncCommonStreamEndHandling_DiscardPartial,
            eAVEncCommonStreamEndHandling_EnsureComplete
        }

        internal enum eAVEncVideoOutputFrameRateConversion
        {
            eAVEncVideoOutputFrameRateConversion_Disable,
            eAVEncVideoOutputFrameRateConversion_Enable,
            eAVEncVideoOutputFrameRateConversion_Alias
        }

        internal enum eAVEncVideoSourceScanType
        {
            eAVEncVideoSourceScan_Automatic,
            eAVEncVideoSourceScan_Interlaced,
            eAVEncVideoSourceScan_Progressive
        }

        internal enum eAVEncVideoOutputScanType
        {
            eAVEncVideoOutputScan_Progressive,
            eAVEncVideoOutputScan_Interlaced,
            eAVEncVideoOutputScan_SameAsInput,
            eAVEncVideoOutputScan_Automatic
        }

        internal enum eAVEncVideoFilmContent
        {
            eAVEncVideoFilmContent_VideoOnly,
            eAVEncVideoFilmContent_FilmOnly,
            eAVEncVideoFilmContent_Mixed
        }

        internal enum eAVEncVideoChromaResolution
        {
            eAVEncVideoChromaResolution_SameAsSource,
            eAVEncVideoChromaResolution_444,
            eAVEncVideoChromaResolution_422,
            eAVEncVideoChromaResolution_420,
            eAVEncVideoChromaResolution_411
        }

        internal enum eAVEncVideoColorTransferFunction
        {
            eAVEncVideoColorTransferFunction_SameAsSource,
            eAVEncVideoColorTransferFunction_10, //  (Linear scRGB)
            eAVEncVideoColorTransferFunction_18,
            eAVEncVideoColorTransferFunction_20,
            eAVEncVideoColorTransferFunction_22, //  (BT470-2 SysM)
            eAVEncVideoColorTransferFunction_22_709, //  (BT709  SMPTE296M SMPTE170M BT470 SMPTE274M BT.1361)
            eAVEncVideoColorTransferFunction_22_240M, //  (SMPTE240M interim 274M)
            eAVEncVideoColorTransferFunction_22_8bit_sRGB, //  (sRGB)
            eAVEncVideoColorTransferFunction_28
        }

        internal enum eAVEncVideoColorTransferMatrix
        {
            eAVEncVideoColorTransferMatrix_SameAsSource,
            eAVEncVideoColorTransferMatrix_BT709,
            eAVEncVideoColorTransferMatrix_BT601, //  (601 BT470-2 BB 170M)
            eAVEncVideoColorTransferMatrix_SMPTE240M
        }

        internal enum eAVEncVideoColorLighting
        {
            eAVEncVideoColorLighting_SameAsSource,
            eAVEncVideoColorLighting_Unknown,
            eAVEncVideoColorLighting_Bright,
            eAVEncVideoColorLighting_Office,
            eAVEncVideoColorLighting_Dim,
            eAVEncVideoColorLighting_Dark
        }

        internal enum eAVEncVideoColorNominalRange
        {
            eAVEncVideoColorNominalRange_SameAsSource,
            eAVEncVideoColorNominalRange_0_255, //  (8 bit: 0..255 10 bit: 0..1023)
            eAVEncVideoColorNominalRange_16_235, //  (16..235 64..940 (16*4...235*4)
            eAVEncVideoColorNominalRange_48_208 //  (48..208)
        }

        internal enum eAVEncInputVideoSystem
        {
            eAVEncInputVideoSystem_Unspecified,
            eAVEncInputVideoSystem_PAL,
            eAVEncInputVideoSystem_NTSC,
            eAVEncInputVideoSystem_SECAM,
            eAVEncInputVideoSystem_MAC,
            eAVEncInputVideoSystem_HDV,
            eAVEncInputVideoSystem_Component
        }

        internal enum eAVEncAudioDualMono
        {
            eAVEncAudioDualMono_SameAsInput, //  As indicated by input media type
            eAVEncAudioDualMono_Off, //  2-ch output bitstream should not be dual mono
            eAVEncAudioDualMono_On //  2-ch output bitstream should be dual mono
        }

        internal enum eAVEncAudioInputContent
        {
            AVEncAudioInputContent_Unknown,
            AVEncAudioInputContent_Voice,
            AVEncAudioInputContent_Music
        }

        internal enum eAVEncMPVProfile
        {
            eAVEncMPVProfile_unknown,
            eAVEncMPVProfile_Simple,
            eAVEncMPVProfile_Main,
            eAVEncMPVProfile_High,
            eAVEncMPVProfile_422
        }

        internal enum eAVEncMPVFrameFieldMode
        {
            eAVEncMPVFrameFieldMode_FieldMode,
            eAVEncMPVFrameFieldMode_FrameMode
        }

        internal enum eAVEncMPVSceneDetection
        {
            eAVEncMPVSceneDetection_None,
            eAVEncMPVSceneDetection_InsertIPicture,
            eAVEncMPVSceneDetection_StartNewGOP,
            eAVEncMPVSceneDetection_StartNewLocatableGOP
        }

        internal enum eAVEncMPVScanPattern
        {
            eAVEncMPVScanPattern_Auto,
            eAVEncMPVScanPattern_ZigZagScan,
            eAVEncMPVScanPattern_AlternateScan
        }

        internal enum eAVEncMPVQScaleType
        {
            eAVEncMPVQScaleType_Auto,
            eAVEncMPVQScaleType_Linear,
            eAVEncMPVQScaleType_NonLinear
        }

        internal enum eAVEncMPVIntraVLCTable
        {
            eAVEncMPVIntraVLCTable_Auto,
            eAVEncMPVIntraVLCTable_MPEG1,
            eAVEncMPVIntraVLCTable_Alternate
        }

        internal enum eAVEncMPACodingMode
        {
            eAVEncMPACodingMode_Mono,
            eAVEncMPACodingMode_Stereo,
            eAVEncMPACodingMode_DualChannel,
            eAVEncMPACodingMode_JointStereo,
            eAVEncMPACodingMode_Surround
        }

        internal enum eAVEncMPAEmphasisType
        {
            eAVEncMPAEmphasisType_None,
            eAVEncMPAEmphasisType_50_15,
            eAVEncMPAEmphasisType_Reserved,
            eAVEncMPAEmphasisType_CCITT_J17
        }

        internal enum eAVEncDDService
        {
            eAVEncDDService_CM, //  (Main Service: Complete Main)
            eAVEncDDService_ME, //  (Main Service: Music and Effects (ME))
            eAVEncDDService_VI, //  (Associated Service: Visually-Impaired (VI)
            eAVEncDDService_HI, //  (Associated Service: Hearing-Impaired (HI))
            eAVEncDDService_D, //  (Associated Service: Dialog (D))
            eAVEncDDService_C, //  (Associated Service: Commentary (C))
            eAVEncDDService_E, //  (Associated Service: Emergency (E))
            eAVEncDDService_VO //  (Associated Service: Voice Over (VO) / Karaoke)
        }

        internal enum eAVEncDDProductionRoomType
        {
            eAVEncDDProductionRoomType_NotIndicated,
            eAVEncDDProductionRoomType_Large,
            eAVEncDDProductionRoomType_Small
        }

        internal enum eAVEncDDDynamicRangeCompressionControl
        {
            eAVEncDDDynamicRangeCompressionControl_None,
            eAVEncDDDynamicRangeCompressionControl_FilmStandard,
            eAVEncDDDynamicRangeCompressionControl_FilmLight,
            eAVEncDDDynamicRangeCompressionControl_MusicStandard,
            eAVEncDDDynamicRangeCompressionControl_MusicLight,
            eAVEncDDDynamicRangeCompressionControl_Speech
        }

        internal enum eAVEncDDSurroundExMode
        {
            eAVEncDDSurroundExMode_NotIndicated,
            eAVEncDDSurroundExMode_No,
            eAVEncDDSurroundExMode_Yes
        }

        internal enum eAVEncDDPreferredStereoDownMixMode
        {
            eAVEncDDPreferredStereoDownMixMode_LtRt,
            eAVEncDDPreferredStereoDownMixMode_LoRo
        }

        internal enum eAVEncDDAtoDConverterType
        {
            eAVEncDDAtoDConverterType_Standard,
            eAVEncDDAtoDConverterType_HDCD
        }

        internal enum eAVEncDDHeadphoneMode
        {
            eAVEncDDHeadphoneMode_NotIndicated,
            eAVEncDDHeadphoneMode_NotEncoded,
            eAVEncDDHeadphoneMode_Encoded
        }

        internal enum eAVDecVideoInputScanType
        {
            eAVDecVideoInputScan_Unknown,
            eAVDecVideoInputScan_Progressive,
            eAVDecVideoInputScan_Interlaced_UpperFieldFirst,
            eAVDecVideoInputScan_Interlaced_LowerFieldFirst
        }

        internal enum eAVDecAudioDualMono
        {
            eAVDecAudioDualMono_IsNotDualMono, //  2-ch bitstream input is not dual mono
            eAVDecAudioDualMono_IsDualMono, //  2-ch bitstream input is dual mono
            eAVDecAudioDualMono_UnSpecified //  There is no indication in the bitstream
        }

        internal enum eAVDecAudioDualMonoReproMode
        {
            eAVDecAudioDualMonoReproMode_STEREO, //  Ch1+Ch2 for mono output (Ch1 left     Ch2 right) for stereo output
            eAVDecAudioDualMonoReproMode_LEFT_MONO, //  Ch1 for mono output     (Ch1 left     Ch1 right) for stereo output
            eAVDecAudioDualMonoReproMode_RIGHT_MONO, //  Ch2 for mono output     (Ch2 left     Ch2 right) for stereo output
            eAVDecAudioDualMonoReproMode_MIX_MONO //  Ch1+Ch2 for mono output (Ch1+Ch2 left Ch1+Ch2 right) for stereo output
        }

        internal enum eAVDDSurroundMode
        {
            eAVDDSurroundMode_NotIndicated,
            eAVDDSurroundMode_No,
            eAVDDSurroundMode_Yes
        }

        internal enum eAVDecDDOperationalMode
        {
            eAVDecDDOperationalMode_NONE,
            eAVDecDDOperationalMode_LINE, //  Dialnorm enabled dialogue at -31dBFS dynrng used high/low scaling allowed
            eAVDecDDOperationalMode_RF, //  Dialnorm enabled dialogue at -20dBFS dynrng & compr used high/low scaling NOT allowed (always fully compressed)
            eAVDecDDOperationalMode_CUSTOM0, //  Analog dialnorm (dialogue normalization not part of the decoder)
            eAVDecDDOperationalMode_CUSTOM1 //  Digital dialnorm (dialogue normalization is part of the decoder)
        }

        internal enum eAVDecDDMatrixDecodingMode
        {
            eAVDecDDMatrixDecodingMode_OFF,
            eAVDecDDMatrixDecodingMode_ON,
            eAVDecDDMatrixDecodingMode_AUTO
        }

        internal enum eAVEncMPVLevel
        {
            eAVEncMPVLevel_Low = 1,
            eAVEncMPVLevel_Main = 2,
            eAVEncMPVLevel_High1440 = 3,
            eAVEncMPVLevel_High = 4
        }

        public enum eAVEncMPALayer
        {
            eAVEncMPALayer_1 = 1,
            eAVEncMPALayer_2 = 2,
            eAVEncMPALayer_3 = 3
        }
    }
}
