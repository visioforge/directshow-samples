// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="ServiceType.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Enum ServiceType.
    /// </summary>
    internal enum ServiceType
    {
        /// <summary>
        /// The HDTV.
        /// </summary>
        HDTV = 0x11,

        /// <summary>
        /// The radio.
        /// </summary>
        Radio = 2,

        /// <summary>
        /// The teletext.
        /// </summary>
        Teletext = 3,

        /// <summary>
        /// The tv.
        /// </summary>
        TV = 1
    }
}