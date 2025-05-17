using System;
using System.Collections.Generic;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    public delegate int VFFilterLogProc([In][MarshalAs(UnmanagedType.LPWStr)] string text);

    /// <summary>
    /// IP RTSP server filter params.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFRTSPServerParam
    {
        /// <summary>
        /// Log enabled.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool LogToFile;

        /// <summary>
        /// Log file name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string LogFilename;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public VFFilterLogProc LogCallback;

        /// <summary>
        /// Port.
        /// </summary>
        public int Port;
    }

    /// <summary>
    /// IP RTSP Server filter interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("63A7CD07-BC0F-487b-91DD-06DF1F4B0D94")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFRTSPServer
    {
        /// <summary>
        /// Sets filter parameters.
        /// </summary>
        /// <param name="FilterParams">
        /// Filter parameters.
        /// </param>
        /// <returns>
        /// Returns HRESULT.
        /// </returns>
        [PreserveSig]
        int SetParams([In] VFRTSPServerParam FilterParams);
    }



    /// <summary>
    /// RTSP sink filter interface.
    /// </summary>
    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
           Guid("80DF6C2F-83E7-4217-801B-29BB4BF0C77F"),
           InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IRTSPSinkConfig
    {
        [PreserveSig]
        int set_URL(/*LPCOLESTR*/ [In, MarshalAs(UnmanagedType.BStr)] string pszURL);

        [PreserveSig]
        int get_URL(/*LPOLESTR*/ [Out, MarshalAs(UnmanagedType.BStr)] out string ppszURL);

        [PreserveSig]
        int set_Info(/*LPCOLESTR*/ [In, MarshalAs(UnmanagedType.BStr)] string pszInfo);

        [PreserveSig]
        int get_Info(/*LPOLESTR**/ [Out, MarshalAs(UnmanagedType.BStr)] out string ppszInfo);

        [PreserveSig]
        int set_Description(/*LPCOLESTR*/ [In, MarshalAs(UnmanagedType.BStr)] string pszDescription);

        [PreserveSig]
        int get_Description(/*LPOLESTR**/ [Out, MarshalAs(UnmanagedType.BStr)] out string ppszDescription);

        [PreserveSig]
        int set_TTL(int ttl);

        [PreserveSig]
        int get_TTL(out int ttl);

        [PreserveSig]
        int set_SSM([In, MarshalAs(UnmanagedType.Bool)] bool _ssm);

        [PreserveSig]
        int get_SSM([Out, MarshalAs(UnmanagedType.Bool)] out bool _ssm);

        [PreserveSig]
        int set_StartRTPPort(int _port);

        [PreserveSig]
        int get_StartRTPPort(out int _port);

        [PreserveSig]
        int set_Port(int _port);

        [PreserveSig]
        int get_Port(out int _port);

        [PreserveSig]
        int get_BroadcastIP(/*LPOLESTR**/ [In, MarshalAs(UnmanagedType.BStr)] string ppszIP);

        [PreserveSig]
        int set_BroadcastIP(/*LPOLESTR*/ [Out, MarshalAs(UnmanagedType.BStr)] out string pszIP);
    }
}
