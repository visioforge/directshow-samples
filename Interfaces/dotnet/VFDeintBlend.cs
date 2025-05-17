// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFDeintBlend.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Deinterlace blend.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFDeintBlend
    {
        /// <summary>
        /// Threshold 1.
        /// </summary>
        public int BlendThresh1;

        /// <summary>
        /// Threshold 2.
        /// </summary>
        /// <remarks>
        /// Default 5 - range [0, 255].
        /// </remarks>
        public int BlendThresh2;

        /// <summary>
        /// Constants 1.
        /// </summary>
        /// <remarks>
        /// Default 9 - range [0, 255].
        /// </remarks>
        public double BlendConstants1;

        /// <summary>
        /// Constants 2.
        /// </summary>
        /// <remarks>
        /// Default 0.3 - range [0, 1].
        /// </remarks>
        public double BlendConstants2;
    }
}