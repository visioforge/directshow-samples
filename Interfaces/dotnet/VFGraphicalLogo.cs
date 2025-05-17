// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFGraphicalLogo.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Graphical logo.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFGraphicalLogo
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public uint X;

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public uint Y;

        /// <summary>
        /// Stretch mode.
        /// </summary>
        public int StretchMode;

        /// <summary>
        /// HBITMAP handle.
        /// </summary>
        public int HBmp;

        /// <summary>
        /// Transparency level.
        /// </summary>
        public int TranspLevel;

        /// <summary>
        /// Color key.
        /// </summary>
        public int ColorKey;

        /// <summary>
        /// True to use color key.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool UseColorKey;

        /// <summary>
        /// File name.
        /// </summary>
        [MarshalAs(UnmanagedType.BStr)]
        public string Filename;
    }
}