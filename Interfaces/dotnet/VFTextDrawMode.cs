// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFTextDrawMode.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Text draw mode.
    /// </summary>
    public enum VFTextDrawMode
    {
        /// <summary>
        /// Bicubic HQ.
        /// </summary>
        BicubicHQ,

        /// <summary>
        /// Bilinear HQ.
        /// </summary>
        BilinearHQ,

        /// <summary>
        /// Nearest neighbor.
        /// </summary>
        NearestNeighbor,

        /// <summary>
        /// Bicubic.
        /// </summary>
        Bicubic,

        /// <summary>
        /// Bilinear.
        /// </summary>
        Bilinear,

        /// <summary>
        /// Standard HQ,
        /// </summary>
        StandardHQ,

        /// <summary>
        /// Standard LQ.
        /// </summary>
        StandardLQ,

        /// <summary>
        /// System default.
        /// </summary>
        SystemDefault,
    }
}