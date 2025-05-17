// --------------------------------------------------------------------------------------------------------------------
// <copyright file="COM_Wrapper.cs" company="VisioForge">
//   VisioForge (c) 2010
// </copyright>
// <summary>
//   COM wrappers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.CompilerServices;

//using System.Runtime.InteropServices.CustomMarshalers;

//[System.Runtime.InteropServices.DllImport("ole32.dll")]
//static extern int StgCreateDocfile(string
//pwcsName, STGM grfMode, uint reserved, out IStorage ppstgOpen);

namespace VisioForge.DirectShowLib
{
    [Flags]
    internal enum CSMFlags : short
    {
        NONE = 0,
        EVENT_EN = 0x0100,
        POLLED_EVENTS = 0x0001
    }

    [Flags]
    internal enum CSMFilterType : byte
    {
        ENABLE_MSG = 0x00,
        BLOCK_MSG = 0x01,
        NO_FILTER = 0x02
    }

    [ComImport]
    [Guid("0000000d-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CLSCompliant(false)]
    internal interface IEnumSTATSTG
    {
        // The user needs to allocate an STATSTG array whose size is celt.
        [PreserveSig]
        uint
            Next(
            uint celt,
            [MarshalAs(UnmanagedType.LPArray), Out]
                System.Runtime.InteropServices.ComTypes.STATSTG[] rgelt,
            out uint pceltFetched
            );
        void Skip(uint celt);
        void Reset();
        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATSTG Clone();
    }
}