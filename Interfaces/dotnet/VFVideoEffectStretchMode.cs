// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFVideoEffectStretchMode.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Stretch mode.
    /// </summary>
    public enum VFVideoEffectStretchMode
    {
        /// <summary>
        /// Stretch.
        /// </summary>
        Stretch,

        /// <summary>
        /// Letterbox.
        /// </summary>
        Letterbox,

        /// <summary>
        /// Crop.
        /// </summary>
        Crop,

        /// <summary>
        /// None.
        /// </summary>
        None,
    }
}