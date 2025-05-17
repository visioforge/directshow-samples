// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFAudioEnhancer.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Audio enhancer filter interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("C2C0512A-AE91-4B4D-B4E0-913A0227DCD7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an error.")]
    public interface IVFAudioEnhancer
    {
        /// <summary>
        /// Gets the automatic gain.
        /// </summary>
        /// <param name="auto_gain">Value.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int get_auto_gain([Out, MarshalAs(UnmanagedType.Bool)] out bool auto_gain);

        /// <summary>
        /// Sets the automatic gain.
        /// </summary>
        /// <param name="auto_gain">Value.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_auto_gain([MarshalAs(UnmanagedType.Bool)] bool auto_gain);

        /// <summary>
        /// Gets the normalize.
        /// </summary>
        /// <param name="normalize">Value.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int get_normalize([Out, MarshalAs(UnmanagedType.Bool)] out bool normalize);

        /// <summary>
        /// Sets the normalize.
        /// </summary>
        /// <param name="normalize">Value.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_normalize([MarshalAs(UnmanagedType.Bool)] bool normalize);

        /// <summary>
        /// Gets the input gains.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="c">The c.</param>
        /// <param name="r">The r.</param>
        /// <param name="sl">The sl.</param>
        /// <param name="sr">The sr.</param>
        /// <param name="lfe">The lfe.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int get_input_gains(out float l, out float c, out float r, out float sl, out float sr, out float lfe);

        /// <summary>
        /// Sets the input gains.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="c">The c.</param>
        /// <param name="r">The r.</param>
        /// <param name="sl">The sl.</param>
        /// <param name="sr">The sr.</param>
        /// <param name="lfe">The lfe.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_input_gains(float l, float c, float r, float sl, float sr, float lfe);

        /// <summary>
        /// Gets the output gains.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="c">The c.</param>
        /// <param name="r">The r.</param>
        /// <param name="sl">The sl.</param>
        /// <param name="sr">The sr.</param>
        /// <param name="lfe">The lfe.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int get_output_gains(out float l, out float c, out float r, out float sl, out float sr, out float lfe);

        /// <summary>
        /// Sets the output gains.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="c">The c.</param>
        /// <param name="r">The r.</param>
        /// <param name="sl">The sl.</param>
        /// <param name="sr">The sr.</param>
        /// <param name="lfe">The lfe.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_output_gains(float l, float c, float r, float sl, float sr, float lfe);

        /// <summary>
        /// Gets the time shift.
        /// </summary>
        /// <param name="time_shift">The time shift.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int get_time_shift(out int time_shift);

        /// <summary>
        /// Sets the time shift.
        /// </summary>
        /// <param name="time_shift">The time shift.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int set_time_shift(int time_shift);
    }
}
