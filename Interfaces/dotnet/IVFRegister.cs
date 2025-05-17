// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFRegister.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Public filter registration interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("59E82754-B531-4A8E-A94D-57C75F01DA30")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFRegister
    {
        /// <summary>
        /// Registers the filter.
        /// </summary>
        /// <param name="licenseKey">
        /// License Key.
        /// </param>
        [PreserveSig]
        void SetLicenseKey([In, MarshalAs(UnmanagedType.LPWStr)] string licenseKey);
    }
}