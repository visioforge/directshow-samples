// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFRect.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Custom rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFRect
    {
        /// <summary>
        /// Left coordinate.
        /// </summary>
        public uint Left;

        /// <summary>
        /// Top coordinate.
        /// </summary>
        public uint Top;

        /// <summary>
        /// Right coordinate.
        /// </summary>
        public uint Right;

        /// <summary>
        /// Bottom coordinate.
        /// </summary>
        public uint Bottom;
    }
}