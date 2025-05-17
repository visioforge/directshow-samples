// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFAudioEnhancer3.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Audio enhancer filter interface, v3.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("915E95CE-70F6-4FA5-B608-9B0BCDBE06B3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an issue.")]
    public interface IVFAudioEnhancer3
    {
        /// <summary>
        /// Gets the IEEE output enabled.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> is enabled.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int get_ieee_output_enabled([Out, MarshalAs(UnmanagedType.Bool)] out bool enabled);

        /// <summary>
        /// Sets the IEEE output enabled.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> is enabled.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_ieee_output_enabled([MarshalAs(UnmanagedType.Bool)] bool enabled);
    }
}
