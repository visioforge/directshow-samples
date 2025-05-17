// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-22-2021
//
// Last Modified By : roman
// Last Modified On : 12-22-2021
// ***********************************************************************
// <copyright file="NativeMethods.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// Class NativeMethods.
    /// </summary>
    [Localizable(false)]
    internal static class NativeMethods
    {
        /// <summary>
        /// Creates the bind CTX.
        /// </summary>
        /// <param name="reserved">The reserved.</param>
        /// <param name="ppbc">The PPBC.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        /// <summary>
        /// Mks the display name of the parse.
        /// </summary>
        /// <param name="pcb">The PCB.</param>
        /// <param name="szUserName">Name of the sz user.</param>
        /// <param name="pchEaten">The PCH eaten.</param>
        /// <param name="ppmk">The PPMK.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("ole32.dll")]
        public static extern int MkParseDisplayName(
            IBindCtx pcb, [MarshalAs(UnmanagedType.LPWStr)] string szUserName, out int pchEaten, out IMoniker ppmk);

        /// <summary>
        /// OLEs the create property frame.
        /// </summary>
        /// <param name="hwndOwner">The HWND owner.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="lpszCaption">The LPSZ caption.</param>
        /// <param name="cObjects">The c objects.</param>
        /// <param name="ppUnk">The pp unk.</param>
        /// <param name="cPages">The c pages.</param>
        /// <param name="pPageClsID">The p page CLS identifier.</param>
        /// <param name="lcid">The lcid.</param>
        /// <param name="dwReserved">The dw reserved.</param>
        /// <param name="pvReserved">The pv reserved.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int OleCreatePropertyFrame(
            [In] IntPtr hwndOwner,
            [In] int x,
            [In] int y,
            [In][MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
            [In] int cObjects,
            [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] object[] ppUnk,
            [In] int cPages,
            [In] IntPtr pPageClsID,
            [In] int lcid,
            [In] int dwReserved,
            [In] IntPtr pvReserved);

        /// <summary>
        /// STGs the create docfile.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="grfMode">The GRF mode.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="ppstgOpen">The PPSTG open.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StgCreateDocfile(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] STGM grfMode,
            [In] int reserved,
            [Out] out IStorage ppstgOpen);

        /// <summary>
        /// STGs the is storage file.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StgIsStorageFile([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

        /// <summary>
        /// STGs the open storage.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="pstgPriority">The PSTG priority.</param>
        /// <param name="grfMode">The GRF mode.</param>
        /// <param name="snbExclude">The SNB exclude.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="ppstgOpen">The PPSTG open.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StgOpenStorage(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IStorage pstgPriority,
            [In] STGM grfMode,
            [In] IntPtr snbExclude,
            [In] int reserved,
            [Out] out IStorage ppstgOpen);
    }
}
