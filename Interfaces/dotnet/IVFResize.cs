// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFResize.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Video resize interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("12BC6F20-2812-4660-8684-10F3FD3B4487")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an error.")]
    public interface IVFResize
    {
        /// <summary>
        /// Sets resolution.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_Resolution([In] uint x, [In] uint y);

        /// <summary>
        /// Sets resize mode.
        /// </summary>
        /// <param name="mode">Resize mode.</param>
        /// <param name="letterbox">Letterbox mode.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_ResizeMode([In] VFResizeMode mode, [In] bool letterbox);

        /// <summary>
        /// Sets crop coordinates.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="top">Top.</param>
        /// <param name="right">Right.</param>
        /// <param name="bottom">Bottom.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_Crop([In] uint left, [In] uint top, [In] uint right, [In] uint bottom);

        /// <summary>
        /// Sets filter mode.
        /// </summary>
        /// <param name="mode">Filter mode.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_FilterMode([In] VFResizeFilterMode mode);

        /// <summary>
        /// Sets rotate mode.
        /// </summary>
        /// <param name="mode">Rotate mode.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_RotateMode([In] VFRotateMode mode);
    }
}