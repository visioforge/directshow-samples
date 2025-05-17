// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFEffectsPro.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Buffer callback delegate.
    /// </summary>
    /// <param name="handle">
    /// Handle.
    /// </param>
    /// <param name="handle_id">
    /// Handle id.
    /// </param>
    /// <param name="pBuffer">
    /// Buffer pointer.
    /// </param>
    /// <param name="bufferLen">
    /// Buffer size.
    /// </param>
    /// <param name="width">
    /// Width.
    /// </param>
    /// <param name="height">
    /// Height.
    /// </param>
    /// <param name="startTime">
    /// Start time (milliseconds).
    /// </param>
    /// <param name="stopTime">
    /// Stop time (milliseconds).
    /// </param>
    /// <param name="updateFrame">
    /// Updates frame.
    /// </param>
    /// <returns>
    /// Returns 0 if the operation has been successful.
    /// </returns>
    public delegate int BufferCBProc([In] IntPtr handle, [In] uint handle_id, [In] IntPtr pBuffer, int bufferLen, int width, int height, long startTime, long stopTime, [MarshalAs(UnmanagedType.Bool)] ref bool updateFrame);

    /// <summary>
    /// Video effects filter main interface settings.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("9A794ABE-98AD-45AF-BBB0-042172C74C79")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an issue.")]
    public interface IVFEffectsPro
    {
        /// <summary>
        /// Sets filter parts state.
        /// </summary>
        /// <param name="effects">Video effects.</param>
        /// <param name="motdet">Motion detection.</param>
        /// <param name="chroma">Chroma-key.</param>
        /// <param name="sg">Sample grabber.</param>
        [PreserveSig]
        void set_enabled(
            [In, MarshalAs(UnmanagedType.Bool)] bool effects,
            [In, MarshalAs(UnmanagedType.Bool)] bool motdet,
            [In, MarshalAs(UnmanagedType.Bool)] bool chroma,
            [In] [MarshalAs(UnmanagedType.Bool)] bool sg);

        /// <summary>
        /// Sets callback for RGB24 buffer.
        /// </summary>
        /// <param name="callback">Callback pointer.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int set_sg_callback_24([MarshalAs(UnmanagedType.FunctionPtr)] BufferCBProc callback);

        /// <summary>
        /// Sets callback for RGB32 buffer.
        /// </summary>
        /// <param name="callback">Callback pointer.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int set_sg_callback_32([MarshalAs(UnmanagedType.FunctionPtr)] BufferCBProc callback);

        /// <summary>
        /// Sets sample grabber handle.
        /// </summary>
        /// <param name="handle">Handle.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int put_sg_app_handle(object handle);

        /// <summary>
        /// Sets sample grabber unique handle id.
        /// </summary>
        /// <param name="handle_id">Handle id.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int put_sg_app_handle_id([MarshalAs(UnmanagedType.U4)] uint handle_id);
    }
}