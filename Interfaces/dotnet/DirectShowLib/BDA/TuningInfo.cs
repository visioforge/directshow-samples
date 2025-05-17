// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 10-08-2021
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="TuningInfo.cs" company="VisioForge">
//     VisioForge (c) 2010
// </copyright>
// <summary>Defines BDA helpful types.</summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    using VisioForge.DirectShowLib;

    /// <summary>
    /// Class TuningInfo.
    /// </summary>
    [Serializable]
    internal abstract class TuningInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TuningInfo"/> class.
        /// </summary>
        protected TuningInfo()
        {
        }

        /// <summary>
        /// Serialises to string.
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string SerialiseToString();
    }

    //[ComImport,
    //Guid("B196B286-BAB4-101A-B69C-00AA00341D07"),
    //InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    //internal interface IConnectionPoint
    //{

    //    [PreserveSig]
    //    int GetConnectionInterface([Out] out Guid piid);

    //    [PreserveSig]
    //    int GetConnectionPointContainer([Out] out IConnectionPointContainer ppCPC);

    //    [PreserveSig]
    //    int Advise([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSink, [Out]  out int pdwCookie);

    //    [PreserveSig]
    //    int Unadvise([In]    int dwCookie);

    //    [PreserveSig]
    //    int EnumConnections([Out] out   IEnumConnections ppEnum);
    //}

    //[ComImport,
    //Guid("B196B284-BAB4-101A-B69C-00AA00341D07"),
    //InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    //internal interface IConnectionPointContainer
    //{
    //    [PreserveSig]
    //    int EnumConnectionPoints([Out]  out IEnumConnectionPoints ppEnum);

    //    [PreserveSig]
    //    int FindConnectionPoint([In]    ref Guid riid, [Out]  out IConnectionPoint ppCP);
    //    //int FindConnectionPoint([In]    ref Guid riid, [Out]  out object ppCP);
    //}
}
