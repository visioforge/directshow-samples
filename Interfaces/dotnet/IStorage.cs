// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-22-2021
//
// Last Modified By : roman
// Last Modified On : 12-22-2021
// ***********************************************************************
// <copyright file="IStorage.cs" company="VisioForge">
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
    /// Interface IStorage.
    /// </summary>
    [Guid("0000000b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IStorage
    {
        /// <summary>
        /// Creates the stream.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="grfMode">The GRF mode.</param>
        /// <param name="reserved1">The reserved1.</param>
        /// <param name="reserved2">The reserved2.</param>
        /// <param name="ppstm">The PPSTM.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int CreateStream(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] STGM grfMode,
            [In] int reserved1,
            [In] int reserved2,
            [Out] out IStream ppstm);

        /// <summary>
        /// Opens the stream.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="reserved1">The reserved1.</param>
        /// <param name="grfMode">The GRF mode.</param>
        /// <param name="reserved2">The reserved2.</param>
        /// <param name="ppstm">The PPSTM.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int OpenStream(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IntPtr reserved1,
            [In] STGM grfMode,
            [In] int reserved2,
            [Out] out IStream ppstm);

        /// <summary>
        /// Creates the storage.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="grfMode">The GRF mode.</param>
        /// <param name="reserved1">The reserved1.</param>
        /// <param name="reserved2">The reserved2.</param>
        /// <param name="ppstg">The PPSTG.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int CreateStorage(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] STGM grfMode,
            [In] int reserved1,
            [In] int reserved2,
            [Out] out IStorage ppstg);

        /// <summary>
        /// Opens the storage.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="pstgPriority">The PSTG priority.</param>
        /// <param name="grfMode">The GRF mode.</param>
        /// <param name="snbExclude">The SNB exclude.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="ppstg">The PPSTG.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int OpenStorage(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IStorage pstgPriority,
            [In] STGM grfMode,
            [In] int snbExclude,
            [In] int reserved,
            [Out] out IStorage ppstg);

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="ciidExclude">The ciid exclude.</param>
        /// <param name="rgiidExclude">The rgiid exclude.</param>
        /// <param name="snbExclude">The SNB exclude.</param>
        /// <param name="pstgDest">The PSTG dest.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int CopyTo([In] int ciidExclude, [In] Guid[] rgiidExclude, [In] string[] snbExclude, [In] IStorage pstgDest);

        /// <summary>
        /// Moves the element to.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="pstgDest">The PSTG dest.</param>
        /// <param name="pwcsNewName">New name of the PWCS.</param>
        /// <param name="grfFlags">The GRF flags.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int MoveElementTo(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IStorage pstgDest,
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName,
            [In] STGM grfFlags);

        /// <summary>
        /// Commits the specified GRF commit flags.
        /// </summary>
        /// <param name="grfCommitFlags">The GRF commit flags.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int Commit([In] STGC grfCommitFlags);

        /// <summary>
        /// Reverts this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int Revert();

        /// <summary>
        /// Enums the elements.
        /// </summary>
        /// <param name="reserved1">The reserved1.</param>
        /// <param name="reserved2">The reserved2.</param>
        /// <param name="reserved3">The reserved3.</param>
        /// <param name="ppenum">The ppenum.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int EnumElements(
            [In] int reserved1,
            [In] IntPtr reserved2,
            [In] int reserved3,
            [Out][MarshalAs(UnmanagedType.Interface)] out object ppenum);

        /// <summary>
        /// Destroys the element.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int DestroyElement([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

        /// <summary>
        /// Renames the element.
        /// </summary>
        /// <param name="pwcsOldName">Old name of the PWCS.</param>
        /// <param name="pwcsNewName">New name of the PWCS.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int RenameElement(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsOldName,
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName);

        /// <summary>
        /// Sets the element times.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="pctime">The pctime.</param>
        /// <param name="patime">The patime.</param>
        /// <param name="pmtime">The pmtime.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetElementTimes(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] System.Runtime.InteropServices.ComTypes.FILETIME pctime,
            [In] System.Runtime.InteropServices.ComTypes.FILETIME patime,
            [In] System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

        /// <summary>
        /// Sets the class.
        /// </summary>
        /// <param name="clsid">The CLSID.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetClass([In][MarshalAs(UnmanagedType.LPStruct)] Guid clsid);

        /// <summary>
        /// Sets the state bits.
        /// </summary>
        /// <param name="grfStateBits">The GRF state bits.</param>
        /// <param name="grfMask">The GRF mask.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetStateBits([In] int grfStateBits, [In] int grfMask);

        /// <summary>
        /// Stats the specified STATSTG.
        /// </summary>
        /// <param name="pStatStg">The STATSTG.</param>
        /// <param name="grfStatFlag">The GRF stat flag.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int Stat([Out] out System.Runtime.InteropServices.ComTypes.STATSTG pStatStg, [In] int grfStatFlag);
    }
}
