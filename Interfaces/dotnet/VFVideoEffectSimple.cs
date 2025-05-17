// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFVideoEffectSimple.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Video Effect Simple structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFVideoEffectSimple
    {
        /// <summary>
        /// Type.
        /// </summary>
        public int Type;

        /// <summary>
        /// ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// Start time.
        /// </summary>
        public long StartTime;

        /// <summary>
        /// Stop time.
        /// </summary>
        public long StopTime;

        /// <summary>
        /// Enabled.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool Enabled;

        /// <summary>
        /// Amount (integer).
        /// </summary>
        public int AmountI;

        /// <summary>
        /// Minimum.
        /// </summary>
        public int MinI;

        /// <summary>
        /// Maximum.
        /// </summary>
        public int MaxI;

        /// <summary>
        /// Amount (double).
        /// </summary>
        public double AmountD;

        /// <summary>
        /// Scale.
        /// </summary>
        public double ScaleD;

        /// <summary>
        /// Turbulence.
        /// </summary>
        public int TurbulenceI;

        /// <summary>
        /// Size.
        /// </summary>
        public int SizeI;

        /// <summary>
        /// Seam.
        /// </summary>
        public int SeamI;

        /// <summary>
        /// Factor.
        /// </summary>
        public int FactorI;

        /// <summary>
        /// Inference.
        /// </summary>
        public int InferenceI;

        /// <summary>
        /// Style.
        /// </summary>
        public int StyleI;

        /// <summary>
        /// Denoise SNR threshold. Default is 20, range is [0, 255].
        /// </summary>
        public int DenoiseSNRThreshold;

        /// <summary>
        /// Deinterlace Triangle weight. Default is 180, range is [128, 256].
        /// </summary>
        public int DeintTriangleWeight;

        /// <summary>
        /// Deinterlace CAVT threshold. Default is 20, range is [0, 255].
        /// </summary>
        public int DeintCAVTThreshold;

        /// <summary>
        /// Denoise Adaptive threshold. Default is 20, range is [0, 255].
        /// </summary>
        public int DenoiseAdaptiveThreshold;

        /// <summary>
        /// Denoise Adaptive blur mode. Default is 0, range is [0, 3].
        /// </summary>
        public int DenoiseAdaptiveBlurMode;

        /// <summary>
        /// Text logo.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public VFTextLogo TextLogo;

        /// <summary>
        /// Graphical logo.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public VFGraphicalLogo GraphicalLogo;

        /// <summary>
        /// Denoise CAST.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public VFDenoiseCAST DenoiseCAST;

        /// <summary>
        /// Deinterlace Blend.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public VFDeintBlend DeintBlend;

        /// <summary>
        /// Zoom.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public VFZoom Zoom;

        /// <summary>
        /// Pan.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public VFPan Pan;
    }
}