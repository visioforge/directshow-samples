// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="RunningStatus.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Enum RunningStatus.
    /// </summary>
    internal enum RunningStatus
    {
        /// <summary>
        /// The undefined
        /// </summary>
        Undefined,

        /// <summary>
        /// The not running
        /// </summary>
        NotRunning,

        /// <summary>
        /// The starts soon
        /// </summary>
        StartsSoon,

        /// <summary>
        /// The pausing
        /// </summary>
        Pausing,

        /// <summary>
        /// The running
        /// </summary>
        Running
    }
}