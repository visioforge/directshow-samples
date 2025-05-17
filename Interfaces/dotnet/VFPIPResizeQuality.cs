// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFPIPResizeQuality.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// PIP resize quality.
    /// </summary>
    public enum VFPIPResizeQuality
    {
        /// <summary>
        /// Nearest neighbor. Lowest quality, very fast.
        /// </summary>
        RQ_NN,

        /// <summary>
        /// Linear interpolation. Average quality, average speed.
        /// </summary>
        RQ_LINEAR,

        /// <summary>
        /// Bicubic interpolation. Better than average quality, lower than average speed.
        /// </summary>
        RQ_CUBIC,

        /// <summary>
        /// Lanczos. Best quality, lowest speed.
        /// </summary>
        RQ_LANCZOS,
    }
}