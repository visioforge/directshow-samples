using System;
using System.Collections.Generic;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("A6096781-2A65-4540-A536-011235D0A5FE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFLACEncodeSettings
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool canModifySettings();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setEncodingLevel(uint inLevel);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setLPCOrder(uint inLPCOrder);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setBlockSize(uint inBlockSize);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool useMidSideCoding([In, MarshalAs(UnmanagedType.Bool)] bool inUseMidSideCoding); // Only for 2 channels

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool useAdaptiveMidSideCoding([In, MarshalAs(UnmanagedType.Bool)] bool inUseAdaptiveMidSideCoding); // Only for 2 channels, overrides midside, is faster

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool useExhaustiveModelSearch([In, MarshalAs(UnmanagedType.Bool)] bool inUseExhaustiveModelSearch);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool setRicePartitionOrder(uint inMin, uint inMax);

        // const vector<unsigned long>& getValidBlockSizes();

        // virtual STDMETHODIMP_(unsigned long) numChannels() PURE;
        // virtual STDMETHODIMP_(unsigned long) sampleRate() PURE;
        // virtual STDMETHODIMP_(unsigned long) bitsPerSample() PURE;

        [PreserveSig]
        int encoderLevel();

        [PreserveSig]
        uint LPCOrder();

        [PreserveSig]
        uint blockSize();

        [PreserveSig]
        uint riceMin();

        [PreserveSig]
        uint riceMax();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool isUsingMidSideCoding();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool isUsingAdaptiveMidSideCoding();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool isUsingExhaustiveModel();
    }
}
