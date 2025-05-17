// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFScreenCapture3.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// IVFScreenCapture3 interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("259E0009-9963-4a71-91AE-34B96D754899")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an issue.")]
    public interface IVFScreenCapture3
    {
        /// <summary>
        /// Inits filter.
        /// </summary>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int init();

        /// <summary>
        /// Sets FPS.
        /// </summary>
        /// <param name="fps">FPS.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int set_fps([In] double fps);

        /// <summary>
        /// Sets rect.
        /// </summary>
        /// <param name="rect">Rect.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int set_rect([In] VFRect rect);

        /// <summary>
        /// Sets mouse cursor mode.
        /// </summary>
        /// <param name="draw">True to draw mouse.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int set_mouse([In] bool draw);

        /// <summary>
        /// Sets display index to capture.
        /// </summary>
        /// <param name="index">Display index.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int set_display_index([In] int index);

        /// <summary>
        /// Sets mode.
        /// </summary>
        /// <param name="mode">Screen capture mode.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int set_mode([In] VFScreenCaptureMode mode);

        /// <summary>
        /// Refreshes picture.
        /// </summary>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int refresh_pic();

        /// <summary>
        /// Sets stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="length">Length.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int set_stream([In] IStream stream, [In] long length);

        /// <summary>
        /// Sets window handle to capture.
        /// </summary>
        /// <param name="handle">Window handle.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int set_window_handle([In] IntPtr handle);

        /// <summary>
        /// Gets window size by handle.
        /// </summary>
        /// <param name="handle">Window handle.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int get_window_size([In] IntPtr handle, [Out] out int width, [Out] out int height);
    }
}