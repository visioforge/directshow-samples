// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFPIPVideoInputParam.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// PIP video input parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Not an error.")]
    public struct VFPIPVideoInputParam
    {
        /// <summary>
        /// Output x.
        /// </summary>
        public int X;

        /// <summary>
        /// Output y.
        /// </summary>
        public int Y;

        /// <summary>
        /// Output width.
        /// </summary>
        public int Width;

        /// <summary>
        /// Output height.
        /// </summary>
        public int Height;

        /// <summary>
        /// Alpha 0 (fill) - 255 (transparent).
        /// </summary>
        public int Alpha;

        /// <summary>
        /// Flip horizontally.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool FlipX;

        /// <summary>
        /// Flip vertically.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool FlipY;

        /// <summary>
        /// Disable stream.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool Disabled;

        /// <summary>
        /// Stream order. From 0 to streams count - 1.
        /// </summary>
        public int OrderID;
    }
}