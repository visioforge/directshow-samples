// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFPan.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Pan.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFPan
    {
        /// <summary>
        /// The start X.
        /// </summary>
        public int StartX;

        /// <summary>
        /// The start Y.
        /// </summary>
        public int StartY;

        /// <summary>
        /// The start width.
        /// </summary>
        public int StartWidth;

        /// <summary>
        /// The start height.
        /// </summary>
        public int StartHeight;

        /// <summary>
        /// The stop X.
        /// </summary>
        public int StopX;

        /// <summary>
        /// The stop Y.
        /// </summary>
        public int StopY;

        /// <summary>
        /// The stop width.
        /// </summary>
        public int StopWidth;

        /// <summary>
        /// The stop height.
        /// </summary>
        public int StopHeight;
    }
}