// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFTextAntialiasingMode.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Antialiasing mode.
    /// </summary>
    public enum VFTextAntialiasingMode
    {
        /// <summary>
        /// System default.
        /// </summary>
        SystemDefault,

        /// <summary>
        /// Single bit per pixel (grid fit).
        /// </summary>
        SingleBitPerPixelGridFit,

        /// <summary>
        /// Single bit per pixel.
        /// </summary>
        SingleBitPerPixel,

        /// <summary>
        /// Antialiasing (grid fit).
        /// </summary>
        AntiAliasGridFit,

        /// <summary>
        /// Antialiasing.
        /// </summary>
        AntiAlias,

        /// <summary>
        /// ClearType (grid fit).
        /// </summary>
        ClearTypeGridFit,
    }
}