// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-22-2021
//
// Last Modified By : roman
// Last Modified On : 12-22-2021
// ***********************************************************************
// <copyright file="STGM.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;

    /// <summary>
    /// STGM enumeration.
    /// </summary>
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"", Justification = "None.")]
    internal enum STGM
    {
        /// <summary>
        /// The read.
        /// </summary>
        Read = 0x00000000,

        /// <summary>
        /// The write.
        /// </summary>
        Write = 0x00000001,

        /// <summary>
        /// The read write.
        /// </summary>
        ReadWrite = 0x00000002,

        /// <summary>
        /// The share deny none.
        /// </summary>
        ShareDenyNone = 0x00000040,

        /// <summary>
        /// The share deny read.
        /// </summary>
        ShareDenyRead = 0x00000030,

        /// <summary>
        /// The share deny write.
        /// </summary>
        ShareDenyWrite = 0x00000020,

        /// <summary>
        /// The share exclusive.
        /// </summary>
        ShareExclusive = 0x00000010,

        /// <summary>
        /// The priority.
        /// </summary>
        Priority = 0x00040000,

        /// <summary>
        /// The create.
        /// </summary>
        Create = 0x00001000,

        /// <summary>
        /// The convert.
        /// </summary>
        Convert = 0x00020000,

        /// <summary>
        /// The fail if there.
        /// </summary>
        FailIfThere = 0x00000000,

        /// <summary>
        /// The direct.
        /// </summary>
        Direct = 0x00000000,

        /// <summary>
        /// The transacted.
        /// </summary>
        Transacted = 0x00010000,

        /// <summary>
        /// The no scratch.
        /// </summary>
        NoScratch = 0x00100000,

        /// <summary>
        /// The no snap shot.
        /// </summary>
        NoSnapShot = 0x00200000,

        /// <summary>
        /// The simple.
        /// </summary>
        Simple = 0x08000000,

        /// <summary>
        /// The direct SWMR.
        /// </summary>
        DirectSWMR = 0x00400000,

        /// <summary>
        /// The delete on release.
        /// </summary>
        DeleteOnRelease = 0x04000000,
    }
}
