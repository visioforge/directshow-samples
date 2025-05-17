// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFPropertyPage.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Property page type.
    /// </summary>
    public enum VFPropertyPage
    {
        /// <summary>
        /// Default.
        /// </summary>
        Default,

        /// <summary>
        /// Capture display dialog.
        /// </summary>
        VFWCapDisplay,

        /// <summary>
        /// Capture format dialog.
        /// </summary>
        VFWCapFormat,

        /// <summary>
        /// Capture source dialog.
        /// </summary>
        VFWCapSource,

        /// <summary>
        /// Compressor config.
        /// </summary>
        VFWCompConfig,

        /// <summary>
        /// Compressor about.
        /// </summary>
        VFWCompAbout,
    }
}