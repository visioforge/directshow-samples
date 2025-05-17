// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="TableType.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Enum TableType.
    /// </summary>
    internal enum TableType : byte
    {
        /// <summary>
        /// The cat
        /// </summary>
        CAT = 1,

        /// <summary>
        /// The eit now next actual
        /// </summary>
        EITNowNextActual = 0x4e,

        /// <summary>
        /// The nit actual
        /// </summary>
        NITActual = 0x40,

        /// <summary>
        /// The nit other
        /// </summary>
        NITOther = 0x41,

        /// <summary>
        /// The pat
        /// </summary>
        PAT = 0,

        /// <summary>
        /// The PMT
        /// </summary>
        PMT = 2,

        /// <summary>
        /// The SDT actual
        /// </summary>
        SDTActual = 0x42,

        /// <summary>
        /// The SDT other
        /// </summary>
        SDTOther = 70,

        /// <summary>
        /// The TSD
        /// </summary>
        TSD = 3
    }
}