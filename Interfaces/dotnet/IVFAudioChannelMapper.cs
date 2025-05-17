// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFAudioChannelMapper.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary>Defines the IVFAudioChannelMapper type.</summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Audio channel mapper filter interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("D1ACE6DD-A8F9-451D-9C92-7C31072CFE2E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an error.")]
    public interface IVFAudioChannelMapper
    {
        /// <summary>
        /// Sets the output channels count.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_output_channels_count([In] int value);

        /// <summary>
        /// Sets the map.
        /// </summary>
        /// <param name="sourceChannel">The source channel.</param>
        /// <param name="destChannel">The destination channel.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_map([In] int sourceChannel, [In] int destChannel);

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <param name="destChannel">The destination channel.</param>
        /// <param name="volume">The volume.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_volume([In] int destChannel, [In] float volume);
    }
}
