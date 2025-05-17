// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="PMTDescriptor.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Struct PMTDescriptor.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PMTDescriptor
    {
        /// <summary>
        /// The program number.
        /// </summary>
        public short programNumber;

        /// <summary>
        /// The PMT pid.
        /// </summary>
        public short pmtPid;

        /// <summary>
        /// Initializes a new instance of the <see cref="PMTDescriptor"/> struct.
        /// </summary>
        /// <param name="programNumber">The program number.</param>
        /// <param name="pmtPid">The PMT pid.</param>
        public PMTDescriptor(short programNumber, short pmtPid)
        {
            this.programNumber = programNumber;
            this.pmtPid = pmtPid;
        }
    }
}