// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFResizeMode.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Resize mode.
    /// </summary>
    public enum VFResizeMode
    {
        /// <summary>
        /// Crop only.
        /// </summary>
        CropOnly,

        /// <summary>
        /// Nearest neighbor.
        /// </summary>
        NearestNeighbor,

        /// <summary>
        /// Bilinear.
        /// </summary>
        Bilinear,

        /// <summary>
        /// Bicubic.
        /// </summary>
        Bicubic,

        /// <summary>
        /// Lancroz.
        /// </summary>
        Lancroz,
    }
}