// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFChromaKey.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Chroma-key interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("AF6E8208-30E3-44f0-AAFE-787A6250BAB3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an issue.")]
    public interface IVFChromaKey
    {
        /// <summary>
        /// Sets contrast.
        /// </summary>
        /// <param name="low">Low level.</param>
        /// <param name="high">High level.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int put_contrast([In] int low, [In] int high);

        /// <summary>
        /// Sets color.
        /// </summary>
        /// <param name="color">Color as int.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int put_color([In] int color);

        /// <summary>
        /// Sets image file name.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int put_image([MarshalAs(UnmanagedType.BStr)] string filename);
    }
}