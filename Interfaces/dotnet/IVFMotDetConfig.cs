// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFMotDetConfig.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Motion callback delegate.
    /// </summary>
    /// <param name="handle">Handle.</param>
    /// <param name="pBuffer">Buffer.</param>
    /// <param name="bufferLen">Buffer length.</param>
    /// <param name="similarity">Similarity.</param>
    /// <returns>Returns HRESULT.</returns>
    public delegate int MotionCBProc([In] IntPtr handle, [In] IntPtr pBuffer, int bufferLen, int similarity);

    /// <summary>
    /// Motion detection callback.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("A77713DE-E16F-4f64-AFE4-27F536B3F4EC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an error.")]
    public interface IVFMotDetConfig
    {
        /// <summary>
        /// Sets callback.
        /// </summary>
        /// <param name="callback">Callback method.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int SetCallback([MarshalAs(UnmanagedType.FunctionPtr)] MotionCBProc callback);

        /// <summary>
        /// Enables CHL.
        /// </summary>
        /// <param name="enabled">Enabled.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_CHL_enabled([In] [MarshalAs(UnmanagedType.Bool)] bool enabled);

        /// <summary>
        /// Sets CHL color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_CHL_color([In] int color);

        /// <summary>
        /// Sets threshold.
        /// </summary>
        /// <param name="threshold">Threshold.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_CHL_threshold([In] int threshold);

        /// <summary>
        /// Sets lines X.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_lines_x([In] int value);

        /// <summary>
        /// Sets lines Y.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_lines_y([In] int value);

        /// <summary>
        /// Enables frame drop.
        /// </summary>
        /// <param name="enabled">Enabled.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_drop_frames_enabled([In] [MarshalAs(UnmanagedType.Bool)] bool enabled);

        /// <summary>
        /// Sets drop frames threshold.
        /// </summary>
        /// <param name="value">Drop frames threshold.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_drop_frames_threshold([In] int value);

        /// <summary>
        /// Sets frame interval.
        /// </summary>
        /// <param name="value">Frame interval.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_frame_interval([In] int value);

        /// <summary>
        /// Sets compare mode.
        /// </summary>
        /// <param name="red">Red channel.</param>
        /// <param name="green">Green channel.</param>
        /// <param name="blue">Blue channel.</param>
        /// <param name="greyscale">Grayscale mode.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_compare_mode(
            [In] [MarshalAs(UnmanagedType.Bool)] bool red,
            [In] [MarshalAs(UnmanagedType.Bool)] bool green,
            [In] [MarshalAs(UnmanagedType.Bool)] bool blue,
            [In] [MarshalAs(UnmanagedType.Bool)] bool greyscale);

        /// <summary>
        /// Sets application handle.
        /// </summary>
        /// <param name="handle">Handle.</param>
        /// <returns>Returns HRESULT.</returns>
        [PreserveSig]
        int put_app_handle([In] IntPtr handle);
    }
}