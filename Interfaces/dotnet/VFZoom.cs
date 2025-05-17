// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFZoom.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Zoom.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFZoom
    {
        /// <summary>
        /// Zoom X.
        /// </summary>
        public double ZoomX;

        /// <summary>
        /// Zoom Y.
        /// </summary>
        public double ZoomY;

        /// <summary>
        /// Shift X.
        /// </summary>
        public int ShiftX;

        /// <summary>
        /// Shift Y.
        /// </summary>
        public int ShiftY;
    }
}