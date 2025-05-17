// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-22-2021
//
// Last Modified By : roman
// Last Modified On : 12-22-2021
// ***********************************************************************
// <copyright file="STGC.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;

    /// <summary>
    /// Enum STGC.
    /// </summary>
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"", Justification = "None.")]
    internal enum STGC
    {
        /// <summary>
        /// The default.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The overwrite.
        /// </summary>
        Overwrite = 1,

        /// <summary>
        /// The only if current.
        /// </summary>
        OnlyIfCurrent = 2,

        /// <summary>
        /// The dangerously commit merely to disk cache.
        /// </summary>
        DangerouslyCommitMerelyToDiskCache = 4,

        /// <summary>
        /// The consolidate.
        /// </summary>
        Consolidate = 8,
    }
}
