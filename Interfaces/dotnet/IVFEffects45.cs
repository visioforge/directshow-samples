// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFEffects45.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Video effects filter interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("5E767DA8-97AF-4607-B95F-8CC6010B84CA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Not an issue.")]
    public interface IVFEffects45
    {
        /// <summary>
        /// Adds video effect.
        /// </summary>
        /// <param name="effect">Effect parameters.</param>
        [PreserveSig]
        void add_effect([In] VFVideoEffectSimple effect);

        /// <summary>
        /// Sets video effects settings.
        /// </summary>
        /// <param name="effect">Effect parameters.</param>
        [PreserveSig]
        void set_effect_settings([In] VFVideoEffectSimple effect);

        /// <summary>
        /// Removes effect.
        /// </summary>
        /// <param name="id">Effect ID.</param>
        [PreserveSig]
        void remove_effect([In] int id);

        /// <summary>
        /// Clears effects.
        /// </summary>
        [PreserveSig]
        void clear_effects();
    }
}