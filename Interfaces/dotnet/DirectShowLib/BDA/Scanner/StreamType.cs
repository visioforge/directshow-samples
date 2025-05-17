// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="StreamType.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Enum StreamType.
    /// </summary>
    internal enum StreamType
    {
        /// <summary>
        /// The mheg.
        /// </summary>
        MHEG = 7,

        /// <summary>
        /// The mpe g1 audio.
        /// </summary>
        MPEG1Audio = 3,

        /// <summary>
        /// The mpe g1 video.
        /// </summary>
        MPEG1Video = 1,

        /// <summary>
        /// The mpe g2 audio.
        /// </summary>
        MPEG2Audio = 4,

        /// <summary>
        /// The mpe g2 video.
        /// </summary>
        MPEG2Video = 2,

        /// <summary>
        /// The private.
        /// </summary>
        Private = 6
    }
}