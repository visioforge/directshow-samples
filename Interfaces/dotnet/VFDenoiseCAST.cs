// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFDenoiseCAST.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Denoise CAST.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Not an issue.")]
    public struct VFDenoiseCAST
    {
        /// <summary>
        /// Temporal difference threshold.
        /// </summary>
        public int TemporalDifferenceThreshold;

        /// <summary>
        /// Number of motion pixels threshold.
        /// </summary>
        public int NumberOfMotionPixelsThreshold;

        /// <summary>
        /// Strong edge threshold.
        /// </summary>
        public int StrongEdgeThreshold;

        /// <summary>
        /// Block width.
        /// </summary>
        public int BlockWidth;

        /// <summary>
        /// Block height.
        /// </summary>
        public int BlockHeight;

        /// <summary>
        /// Edge pixel weight.
        /// </summary>
        public int EdgePixelWeight;

        /// <summary>
        /// Non-edge pixel weight.
        /// </summary>
        public int NonEdgePixelWeight;

        /// <summary>
        /// Gaussian threshold Y.
        /// </summary>
        public int GaussianThresholdY;

        /// <summary>
        /// Gaussian threshold UV.
        /// </summary>
        public int GaussianThresholdUV;

        /// <summary>
        /// History weight.
        /// </summary>
        public int HistoryWeight;
    }
}