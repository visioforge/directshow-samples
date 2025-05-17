// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFPIPVideoOutputParam.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// PIP output parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Not an issue.")]
    public struct VFPIPVideoOutputParam
    {
        /// <summary>
        /// Width.
        /// </summary>
        public int Width;

        /// <summary>
        /// Height.
        /// </summary>
        public int Height;

        /// <summary>
        /// Frame rate.
        /// </summary>
        public int FrameRateTime;

        /// <summary>
        /// Background color.
        /// </summary>
        public int Backcolor;

        /// <summary>
        /// Backgroung image.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Backimage;
    }
}