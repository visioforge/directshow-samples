// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFTextEffectMode.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Text effect mode.
    /// </summary>
    public enum VFTextEffectMode
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// Inner.
        /// </summary>
        Inner,

        /// <summary>
        /// Outer.
        /// </summary>
        Outer,

        /// <summary>
        /// Inner and outer.
        /// </summary>
        InnerAndOuter,

        /// <summary>
        /// Embossed.
        /// </summary>
        Embossed,

        /// <summary>
        /// Outline.
        /// </summary>
        Outline,

        /// <summary>
        /// Filled outline.
        /// </summary>
        FilledOutline,

        /// <summary>
        /// Halo.
        /// </summary>
        Halo,
    }
}