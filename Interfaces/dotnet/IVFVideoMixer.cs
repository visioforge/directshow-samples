// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFVideoMixer.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Video mixer interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("3318300E-F6F1-4d81-8BC3-9DB06B09F77A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFVideoMixer
    {
        /// <summary>
        /// Sets input parameters.
        /// </summary>
        /// <param name="pin_index">Pin index.</param>
        /// <param name="param">Parameters.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int SetInputParam([In] int pin_index, [In] VFPIPVideoInputParam param);

        /// <summary>
        /// Gets input parameters.
        /// </summary>
        /// <param name="pin_index">Pin index.</param>
        /// <param name="param">Parameters.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int GetInputParam([In] int pin_index, [Out] out VFPIPVideoInputParam param);

        /// <summary>
        /// Gets input parameters.
        /// </summary>
        /// <param name="pin">Pin.</param>
        /// <param name="param">Parameters.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int GetInputParam2([In] object pin, [Out] out VFPIPVideoInputParam param);

        /// <summary>
        /// Sets output parameters.
        /// </summary>
        /// <param name="param">Parameters.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int SetOutputParam([In] VFPIPVideoOutputParam param);

        /// <summary>
        /// Gets output parameters.
        /// </summary>
        /// <param name="param">Parameters.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int GetOutputParam([Out] out VFPIPVideoOutputParam param);

        /// <summary>
        /// Sets the chroma settings.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="color">The color.</param>
        /// <param name="tolerance1">The tolerance1.</param>
        /// <param name="tolerance2">The tolerance2.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetChromaSettings([In, MarshalAs(UnmanagedType.Bool)] bool enabled, int color, int tolerance1, int tolerance2);

        /// <summary>
        /// Sets the input order.
        /// </summary>
        /// <param name="pin_index">Index of the pin.</param>
        /// <param name="order">The order.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetInputOrder(int pin_index, int order);

        /// <summary>
        /// Sets the resize quality.
        /// </summary>
        /// <param name="quality">The quality.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetResizeQuality(VFPIPResizeQuality quality);
    }
}