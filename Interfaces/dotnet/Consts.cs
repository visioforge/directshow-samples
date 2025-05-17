// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="Consts.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// VisioForge constants.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Not an issue.")]
    public static class Consts
    {
        /// <summary>
        /// Virtual audio card sink CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVirtualAudioCardSink = new Guid("1A2673B0-553E-4027-AECC-839405468950");

        /// <summary>
        /// Virtual audio card source CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVirtualAudioCardSource = new Guid("B5A463DF-4016-4C34-AA4F-48EC1B51C73F");

        /// <summary>
        /// Virtual camera sink CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVirtualCameraSink = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340");

        /// <summary>
        /// Virtual camera source CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVirtualCameraSource = new Guid("AA4DA14E-644B-487a-A7CB-517A390B4BB8");

        /// <summary>
        /// Network streamer video sink CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVideoNetworkStreamerSink = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340");

        /// <summary>
        /// Network streamer audio sink CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFAudioNetworkStreamerSink = new Guid("1A2673B0-553E-4027-AECC-839405468951");

        /// <summary>
        /// Network streamer source CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFNetworkStreamerSource = new Guid("AA4DA14E-644B-487a-A7CB-517A390B4BB8");



        /// <summary>
        /// PIP filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVideoMixer = new Guid("3F4B9E80-436F-4f97-AD39-4656943278D2");

        /// <summary>
        /// Video Effects filter v4.5 CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVideoEffects45 = new Guid("1D0523AB-7E41-40e8-9A78-839E93BA9444");

        /// <summary>
        /// Video Effects Pro filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVideoEffectsPro = new Guid("980B1181-1619-44F9-AEBE-F2D7E5CE1EFE");

        /// <summary>
        /// Screen capture filter interface IID.
        /// </summary>
        public static readonly Guid IID_IVFScreenCapture = new Guid("259E0009-9963-4a71-91AE-34B96D75486F");

        /// <summary>
        /// Screen capture filter interface 2 IID.
        /// </summary>
        public static readonly Guid IID_IVFScreenCapture2 = new Guid("BC91012D-22E0-4091-8C0A-3913BDAB8A42");

        /// <summary>
        /// Screen capture filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFScreenCapture = new Guid("B74136FB-1F94-4180-8695-C9307810D944");

        /// <summary>
        /// Screen capture filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFScreenCaptureDD = new Guid("0118D5CC-77E4-4199-81B0-548988688261");

        /// <summary>
        /// Resize filter interface IID.
        /// </summary>
        public static readonly Guid IID_IVFResize = new Guid("12BC6F20-2812-4660-8684-10F3FD3B4487");

        /// <summary>
        /// Resize filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFResizer_4 = new Guid("2E0E7313-71DC-4455-ADB4-F80718B7B727");

        /// <summary>
        /// RGB2YUV filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFRGB2YUV = new Guid("3BDA461E-12DB-4C24-9815-B68D1AA4D34A");

        /// <summary>
        /// YUV2RGB filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFYUV2RGB = new Guid("CB54D323-9327-49F5-8147-859FE8FAEFF5");

        /// <summary>
        /// IP HTTP source filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFIPHTTPSource = new Guid("4ea6930a-2c8a-4ae6-a561-56e4b5044437");

        /// <summary>
        /// IP HTTP FFMPEG source filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFIPHTTPFFMPEGSource = new Guid("1269EA71-85DB-40c6-AD87-35AD707C821C");

        /// <summary>
        /// FFMPEG source filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFFFMPEGSource = new Guid("F15FF9D9-F69A-43E6-92F7-13268D10F938");

        /// <summary>
        /// VLC source filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFVLCSource = new Guid("3FC97748-7CB6-4195-89DE-0717582A4863");

        /// <summary>
        /// RTSP source filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFRTSPSource = new Guid("990CC6D7-9472-4ee4-818A-07FD8146103F");

        /// <summary>
        /// RTSP FFMPEG source filter CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFRTSPFFMPEGSource = new Guid("EDB1A24C-4B0B-4c8d-AEE5-52231A062BB4");

        /// <summary>
        /// Audio effect filter v4 CLSID.
        /// </summary>
        public static readonly Guid CLSID_VFAudioEffects4 = new Guid("E90165B8-8E1C-4c96-AC89-266A20BB3EBD");

        /// <summary>
        /// Audio effects filter audio interface IID.
        /// </summary>
        public static readonly Guid IID_DCDSPFilter = new Guid("91692C5B-6878-4ba4-A3D9-7C8DB0D08E44");

        /// <summary>
        /// Audio effects filter visual interface IID.
        /// </summary>
        public static readonly Guid IID_DCDSPFilterVisual = new Guid("081605E3-2B0E-4120-9F72-87A007B208B2");

        /// <summary>
        /// Audio Enhancer filter.
        /// </summary>
        public static readonly Guid CLSID_VFAudioEnhancer = new Guid("DBC661F0-166C-4AC5-A219-C0838F789A2F");

        /// <summary>
        /// Audio mixer filter.
        /// </summary>
        public static readonly Guid CLSID_VFAudioMixerFilter = new Guid("207FEF9F-D6C1-40CF-AEC4-0AEC61F30BEA");

        // ReSharper restore InconsistentNaming
    }
}