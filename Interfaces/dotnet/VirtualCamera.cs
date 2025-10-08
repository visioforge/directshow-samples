namespace VisioForge.DirectShowAPI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    using DirectShowLib;

    // ReSharper disable InconsistentNaming
        
    /// <summary>
    /// Push source frame data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AVFrameData
    {
        public IntPtr Data;

        public int Size;

        public long StartTime;

        public long StopTime;
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("8E1449D8-0106-486A-85F4-FC9A5D52C2B2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFLiveVideoSource
    {
        [PreserveSig]
        int AddFrame([In, MarshalAs(UnmanagedType.LPStruct)] AVFrameData frame);

        [PreserveSig]
        int AddFrame2([In] AVFrameData frame);

        [PreserveSig]
        int SetBitmapInfo(BitmapInfoHeader bmi);

        [PreserveSig]
        int SetFrameRate(double frameRate);
    }

    /// <summary>
    /// Virtual camera sink interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("A96631D2-4AC9-4F09-9F34-FF8229087DEB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFVirtualCameraSink
    {
        [PreserveSig]
        int set_license([MarshalAs(UnmanagedType.LPWStr)] string license);
    }
}
