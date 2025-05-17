#region license

/*
DirectShowLib - Provide access to DirectShow interfaces via .NET
Copyright (C) 2007
http://sourceforge.net/projects/directshownet/

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

#endregion

using System;
using System.Runtime.InteropServices;

namespace VisioForge.DirectShowLib
{
    #region Declarations

    /// <summary>
    /// From AM_GBF_* defines
    /// </summary>
    [Flags]
    public enum AMGBF
    {
        None = 0,
        PrevFrameSkipped = 1,
        NotAsyncPoint = 2,
        NoWait = 4,
        NoDDSurfaceLock = 8
    }

    /// <summary>
    /// From AM_VIDEO_FLAG_* defines
    /// </summary>
    [Flags]
    public enum AMVideoFlag
    {
        FieldMask = 0x0003,
        InterleavedFrame = 0x0000,
        Field1 = 0x0001,
        Field2 = 0x0002,
        Field1First = 0x0004,
        Weave = 0x0008,
        IPBMask = 0x0030,
        ISample = 0x0000,
        PSample = 0x0010,
        BSample = 0x0020,
        RepeatField = 0x0040
    }

    /// <summary>
    /// From AM_SAMPLE_PROPERTY_FLAGS
    /// </summary>
    [Flags]
    public enum AMSamplePropertyFlags
    {
        SplicePoint = 0x01,
        PreRoll = 0x02,
        DataDiscontinuity = 0x04,
        TypeChanged = 0x08,
        TimeValid = 0x10,
        MediaTimeValid = 0x20,
        TimeDiscontinuity = 0x40,
        FlushOnPause = 0x80,
        StopValid = 0x100,
        EndOfStream = 0x200,
        Media = 0,
        Control = 1
    }

    /// <summary>
    /// From PIN_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PinInfo
    {
        [MarshalAs(UnmanagedType.Interface)]
        public IBaseFilter filter;
        public PinDirection dir;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;
    }

    /// <summary>
    /// From AM_MEDIA_TYPE - When you are done with an instance of this class,
    /// it should be released with FreeAMMediaType() to avoid leaking
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    public class AMMediaType
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);

        #region  Structure Variables

        public Guid majorType;
        public Guid subType;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fixedSizeSamples;
        [MarshalAs(UnmanagedType.Bool)]
        public bool temporalCompression;
        public int sampleSize;
        public Guid formatType;
        public IntPtr unkPtr; // IUnknown Pointer
        public int formatSize;
        public IntPtr formatPtr; // Pointer to a buff determined by formatType
        #endregion

        #region Constructor

        public AMMediaType()
        {
            unkPtr = IntPtr.Zero;
            formatPtr = IntPtr.Zero;
            Init();
        }

        public AMMediaType(AMMediaType mt)
            : this()
        {
            Set(mt);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new AMMediaType(this);
        }

        #endregion

        #region Overriden Methods

        public override bool Equals(object obj)
        {
            if (obj is AMMediaType)
            {
                AMMediaType _dst = (obj as AMMediaType);
                if ((_dst.majorType != majorType))
                {
                    return false;
                }
                if (subType != _dst.subType)
                {
                    return false;
                }
                if (formatType != _dst.formatType)
                {
                    return false;
                }
                if (formatSize != _dst.formatSize)
                {
                    return false;
                }
                if (formatSize > 0)
                {
                    byte[] _source = new byte[formatSize];
                    byte[] _dest = new byte[formatSize];
                    Marshal.Copy(formatPtr, _source, 0, _source.Length);
                    Marshal.Copy(_dst.formatPtr, _dest, 0, _dest.Length);
                    for (int i = 0; i < _source.Length; i++)
                    {
                        if (_dest[i] != _source[i]) return false;
                    }
                }

                return true;
            }

#pragma warning disable S3249
            return base.Equals(obj);
#pragma warning restore S3249
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Public Methods

        public void Set(AMMediaType mt)
        {
            Free();

            majorType = mt.majorType;
            subType = mt.subType;
            fixedSizeSamples = mt.fixedSizeSamples;
            temporalCompression = mt.temporalCompression;
            sampleSize = mt.sampleSize;
            formatType = mt.formatType;
            unkPtr = mt.unkPtr;
            formatPtr = IntPtr.Zero;
            formatSize = mt.formatSize;
            if (unkPtr != IntPtr.Zero)
            {
                Marshal.AddRef(unkPtr);
            }
            if (formatSize > 0)
            {
                SetFormat(mt.formatPtr, formatSize);
            }
        }

        public void Free()
        {
            FreeFormat();
            if (unkPtr != IntPtr.Zero)
            {
                Marshal.Release(unkPtr);
                unkPtr = IntPtr.Zero;
            }
        }

        public void Init()
        {
            Free();
            majorType = Guid.Empty;
            subType = Guid.Empty;
            fixedSizeSamples = true;
            temporalCompression = false;
            sampleSize = 0;
            formatType = Guid.Empty;
            unkPtr = IntPtr.Zero;
            formatPtr = IntPtr.Zero;
            formatSize = 0;
        }

        public void FreeFormat()
        {
            if (formatPtr != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(formatPtr);
                formatPtr = IntPtr.Zero;
            }
            formatSize = 0;
        }

        public bool IsValid()
        {
            return majorType != null && majorType != Guid.Empty;
        }

        public bool IsPartiallySpecified()
        {
            if ((majorType == Guid.Empty) || (formatType == Guid.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MatchesPartial(AMMediaType _dst)
        {
            if ((_dst.majorType != Guid.Empty) && (majorType != _dst.majorType))
            {
                return false;
            }
            if ((_dst.subType != Guid.Empty) && (subType != _dst.subType))
            {
                return false;
            }
            if (_dst.formatType != Guid.Empty)
            {
                if (formatType != _dst.formatType)
                {
                    return false;
                }
                if (formatSize != _dst.formatSize)
                {
                    return false;
                }
                if (formatSize > 0)
                {
                    byte[] _source = new byte[formatSize];
                    byte[] _dest = new byte[formatSize];
                    Marshal.Copy(formatPtr, _source, 0, _source.Length);
                    Marshal.Copy(_dst.formatPtr, _dest, 0, _dest.Length);
                    for (int i = 0; i < _source.Length; i++)
                    {
                        if (_dest[i] != _source[i]) return false;
                    }
                }

            }
            return true;
        }

        public void AllocFormat(int nSize)
        {
            FreeFormat();
            if (nSize > 0)
            {
                formatPtr = Marshal.AllocCoTaskMem(nSize);
                formatSize = nSize;
            }
        }

        public void SetFormat(byte[] _format)
        {
            SetFormat(_format, _format.Length);
        }

        public void SetFormat(byte[] _format, int nSize)
        {
            if (_format != null && nSize > 0)
            {
                IntPtr _ptr = Marshal.AllocCoTaskMem(nSize);
                Marshal.Copy(_format, 0, _ptr, nSize);
                SetFormat(_ptr, nSize);
                Marshal.FreeCoTaskMem(_ptr);
            }
        }

        public void SetFormat(IntPtr pFormat, int nSize)
        {
            AllocFormat(nSize);
            if (pFormat != IntPtr.Zero)
            {
                CopyMemory(formatPtr, pFormat, formatSize);
            }
        }

        public void SetFormat(WaveFormatEx wfx)
        {
            if (wfx != null)
            {
                int cb = Marshal.SizeOf(wfx);
                IntPtr _ptr = Marshal.AllocCoTaskMem(cb);
                try
                {
                    Marshal.StructureToPtr(wfx, _ptr, true);
                    SetFormat(_ptr, cb);
                    formatType = FormatType.WaveEx;
                }
                finally
                {
                    Marshal.FreeCoTaskMem(_ptr);
                }
            }
        }

        public void SetFormat(VideoInfoHeader vih)
        {
            if (vih != null)
            {
                int cb = Marshal.SizeOf(vih);
                IntPtr _ptr = Marshal.AllocCoTaskMem(cb);
                try
                {
                    Marshal.StructureToPtr(vih, _ptr, true);
                    SetFormat(_ptr, cb);
                    formatType = FormatType.VideoInfo;
                }
                finally
                {
                    Marshal.FreeCoTaskMem(_ptr);
                }
            }
        }

        public void SetFormat(VideoInfoHeader2 vih)
        {
            if (vih != null)
            {
                int cb = Marshal.SizeOf(vih);
                IntPtr _ptr = Marshal.AllocCoTaskMem(cb);
                try
                {
                    Marshal.StructureToPtr(vih, _ptr, true);
                    SetFormat(_ptr, cb);
                    formatType = FormatType.VideoInfo2;
                }
                finally
                {
                    Marshal.FreeCoTaskMem(_ptr);
                }
            }
        }

        public byte[] GetExtraData()
        {
            int _size = 0;
            if (formatPtr != IntPtr.Zero && formatSize != 0)
            {
                if (formatType == FormatType.VideoInfo)
                {
                    _size = formatSize - Marshal.SizeOf(typeof(VideoInfoHeader));
                }
                if (formatType == FormatType.VideoInfo2)
                {
                    _size = formatSize - Marshal.SizeOf(typeof(VideoInfoHeader2));
                }
                if (formatType == FormatType.WaveEx || formatType == FormatType.Mpeg2Audio || formatType == FormatType.DolbyAC3)
                {
                    _size = formatSize - Marshal.SizeOf(typeof(WaveFormatEx));
                }
                if (formatType == FormatType.Mpeg2Video)
                {
                    _size = formatSize - Marshal.SizeOf(typeof(VideoInfoHeader2)) - 18;
                }
            }
            byte[] _data = null;
            if (_size > 0)
            {
                _data = new byte[_size];
                IntPtr _ptr = new IntPtr(formatPtr.ToInt32() + (formatSize - _size));
                Marshal.Copy(_ptr, _data, 0, _size);
            }
            return _data;
        }

        public void AddFormatExtraData(IntPtr _data, int _size)
        {
            if (_size > 0 && _data != IntPtr.Zero)
            {
                byte[] _buffer = new byte[_size];
                Marshal.Copy(_data, _buffer, 0, _size);
                AddFormatExtraData(_buffer, _size);
            }
        }

        public void AddFormatExtraData(byte[] _data, int _size)
        {
            if (_data != null && _size > 0)
            {
                if (formatPtr != IntPtr.Zero && formatSize != 0)
                {
                    IntPtr _ptr = Marshal.AllocCoTaskMem(formatSize + _size);
                    CopyMemory(_ptr, formatPtr, formatSize);
                    Marshal.FreeCoTaskMem(formatPtr);
                    formatPtr = _ptr;
                    _ptr = new IntPtr(_ptr.ToInt32() + formatSize);
                    Marshal.Copy(_data, 0, _ptr, _size);
                    formatSize += _size;
                }
            }
        }

        public void AddFormatExtraData(byte[] _data)
        {
            AddFormatExtraData(_data, _data.Length);
        }

        public byte[] ToArray()
        {
            byte[] _data = null;
            if (formatPtr != IntPtr.Zero && formatSize != 0)
            {
                _data = new byte[formatSize];
                Marshal.Copy(formatPtr, _data, 0, formatSize);
            }
            return _data;
        }

        public long GetFrameRate()
        {
            if (majorType == MediaType.Video && formatPtr != IntPtr.Zero)
            {
                VideoInfoHeader _vih = (VideoInfoHeader)Marshal.PtrToStructure(formatPtr, typeof(VideoInfoHeader));
                return _vih.AvgTimePerFrame;
            }
            return 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(AMMediaType _src, AMMediaType _dest)
        {
            if (System.Object.ReferenceEquals(_src, _dest))
            {
                return true;
            }
            if (((object)_src == null) || ((object)_dest == null))
            {
                return false;
            }
            return _src.Equals(_dest);

        }

        public static bool operator !=(AMMediaType _src, AMMediaType _dest)
        {
            return !(_src == _dest);
        }

        public static implicit operator WaveFormatEx(AMMediaType mt)
        {
            if (mt.formatPtr != IntPtr.Zero && mt.formatSize != 0 && mt.formatType == FormatType.WaveEx)
            {
                return (WaveFormatEx)Marshal.PtrToStructure(mt.formatPtr, typeof(WaveFormatEx));
            }
            return null;
        }

        public static implicit operator VideoInfoHeader(AMMediaType mt)
        {
            if (mt.formatPtr != IntPtr.Zero && mt.formatSize != 0)
            {
                return (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));
            }
            return null;
        }

        public static implicit operator VideoInfoHeader2(AMMediaType mt)
        {
            if (mt.formatPtr != IntPtr.Zero && mt.formatSize != 0 && (mt.formatType == FormatType.VideoInfo2 || mt.formatType == FormatType.Mpeg2Video))
            {
                return (VideoInfoHeader2)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader2));
            }
            return null;
        }

        public static implicit operator BitmapInfoHeader(AMMediaType mt)
        {
            if (mt.formatType == FormatType.VideoInfo2 || mt.formatType == FormatType.Mpeg2Video)
            {
                VideoInfoHeader2 _vih = (VideoInfoHeader2)mt;
                return _vih.BmiHeader;
            }
            if (mt.formatType == FormatType.VideoInfo || mt.formatType == FormatType.MpegVideo)
            {
                VideoInfoHeader _vih = (VideoInfoHeader)mt;
                return _vih.BmiHeader;
            }
            return null;
        }

        #endregion

        #region Static Methods

        public static bool IsPartiallySpecified(AMMediaType mt)
        {
            if ((mt.majorType == Guid.Empty) || (mt.formatType == Guid.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MatchesPartial(AMMediaType _src, AMMediaType _dst)
        {
            if ((_dst.majorType != Guid.Empty) && (_src.majorType != _dst.majorType))
            {
                return false;
            }
            if ((_dst.subType != Guid.Empty) && (_src.subType != _dst.subType))
            {
                return false;
            }
            if (_dst.formatType != Guid.Empty)
            {
                if (_src.formatType != _dst.formatType)
                {
                    return false;
                }
                if (_src.formatSize != _dst.formatSize)
                {
                    return false;
                }
                if (_src.formatSize > 0)
                {
                    byte[] _source = new byte[_src.formatSize];
                    byte[] _dest = new byte[_src.formatSize];
                    Marshal.Copy(_src.formatPtr, _source, 0, _source.Length);
                    Marshal.Copy(_dst.formatPtr, _dest, 0, _dest.Length);
                    for (int i = 0; i < _source.Length; i++)
                    {
                        if (_dest[i] != _source[i]) return false;
                    }
                }

            }
            return true;
        }

        public static void Init(ref AMMediaType mt)
        {
            if (((object)mt) == null)
            {
                mt = new AMMediaType();
            }
            else
            {
                Free(ref mt);
            }
            mt.majorType = Guid.Empty;
            mt.subType = Guid.Empty;
            mt.fixedSizeSamples = true;
            mt.temporalCompression = false;
            mt.sampleSize = 0;
            mt.formatType = Guid.Empty;
            mt.unkPtr = IntPtr.Zero;
            mt.formatPtr = IntPtr.Zero;
            mt.formatSize = 0;
        }

        public static void Copy(AMMediaType mt, ref AMMediaType _dest)
        {
            if (((object)_dest) == null)
            {
                _dest = new AMMediaType();
            }
            else
            {
                Free(ref _dest);
            }

            _dest.majorType = mt.majorType;
            _dest.subType = mt.subType;
            _dest.fixedSizeSamples = mt.fixedSizeSamples;
            _dest.temporalCompression = mt.temporalCompression;
            _dest.sampleSize = mt.sampleSize;
            _dest.formatType = mt.formatType;
            _dest.unkPtr = mt.unkPtr;
            _dest.formatPtr = IntPtr.Zero;
            _dest.formatSize = mt.formatSize;
            if (_dest.unkPtr != IntPtr.Zero)
            {
                Marshal.AddRef(_dest.unkPtr);
            }
            if (_dest.formatSize > 0)
            {
                _dest.formatPtr = Marshal.AllocCoTaskMem(_dest.formatSize);
                CopyMemory(_dest.formatPtr, mt.formatPtr, _dest.formatSize);
            }
        }

        public static void Free(ref AMMediaType mt)
        {
            if (mt != null)
            {
                FreeFormat(ref mt);
                if (mt.unkPtr != IntPtr.Zero)
                {
                    Marshal.Release(mt.unkPtr);
                    mt.unkPtr = IntPtr.Zero;
                }
            }
        }

        public static void AllocFormat(ref AMMediaType mt, int nSize)
        {
            FreeFormat(ref mt);
            if (mt != null && nSize > 0)
            {
                mt.formatPtr = Marshal.AllocCoTaskMem(nSize);
                mt.formatSize = nSize;
            }
        }

        public static void SetFormat(ref AMMediaType mt, IntPtr pFormat, int nSize)
        {
            AllocFormat(ref mt, nSize);
            if (mt != null && pFormat != IntPtr.Zero)
            {
                CopyMemory(mt.formatPtr, pFormat, mt.formatSize);
            }
        }

        public static void SetFormat(ref AMMediaType mt, ref WaveFormatEx wfx)
        {
            if (wfx != null)
            {
                int cb = Marshal.SizeOf(wfx);
                IntPtr _ptr = Marshal.AllocCoTaskMem(cb);
                try
                {
                    Marshal.StructureToPtr(wfx, _ptr, true);
                    SetFormat(ref mt, _ptr, cb);
                    if (mt != null)
                    {
                        mt.formatType = FormatType.WaveEx;
                    }
                }
                finally
                {
                    Marshal.FreeCoTaskMem(_ptr);
                }
            }
        }

        public static void SetFormat(ref AMMediaType mt, ref VideoInfoHeader vih)
        {
            if (vih != null)
            {
                int cb = Marshal.SizeOf(vih);
                IntPtr _ptr = Marshal.AllocCoTaskMem(cb);
                try
                {
                    Marshal.StructureToPtr(vih, _ptr, true);
                    SetFormat(ref mt, _ptr, cb);
                    if (mt != null)
                    {
                        mt.formatType = FormatType.VideoInfo;
                    }
                }
                finally
                {
                    Marshal.FreeCoTaskMem(_ptr);
                }
            }
        }

        public static void FreeFormat(ref AMMediaType mt)
        {
            if (mt != null)
            {
                if (mt.formatPtr != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(mt.formatPtr);
                    mt.formatPtr = IntPtr.Zero;
                }
                mt.formatSize = 0;
            }
        }

        public static bool IsValid(AMMediaType mt)
        {
            return (mt.majorType != null && mt.majorType != Guid.Empty);
        }

        public static bool AreEquals(AMMediaType _src, AMMediaType _dst)
        {
            if ((_dst.majorType != _src.majorType))
            {
                return false;
            }
            if (_src.subType != _dst.subType)
            {
                return false;
            }
            if (_src.formatType != _dst.formatType)
            {
                return false;
            }
            if (_src.formatSize != _dst.formatSize)
            {
                return false;
            }
            if (_src.formatSize > 0)
            {
                byte[] _source = new byte[_src.formatSize];
                byte[] _dest = new byte[_src.formatSize];
                Marshal.Copy(_src.formatPtr, _source, 0, _source.Length);
                Marshal.Copy(_dst.formatPtr, _dest, 0, _dest.Length);
                for (int i = 0; i < _source.Length; i++)
                {
                    if (_dest[i] != _source[i]) return false;
                }
            }
            return true;
        }

        #endregion
    }



    /// <summary>
    /// From PIN_DIRECTION
    /// </summary>
    public enum PinDirection
    {
        Input,
        Output
    }

    /// <summary>
    /// From AM_SEEKING_SeekingCapabilities
    /// </summary>
    [Flags]
    public enum AMSeekingSeekingCapabilities
    {
        None = 0,
        CanSeekAbsolute = 0x001,
        CanSeekForwards = 0x002,
        CanSeekBackwards = 0x004,
        CanGetCurrentPos = 0x008,
        CanGetStopPos = 0x010,
        CanGetDuration = 0x020,
        CanPlayBackwards = 0x040,
        CanDoSegments = 0x080,
        Source = 0x100
    }

    /// <summary>
    /// From FILTER_STATE
    /// </summary>
    public enum FilterState
    {
        Stopped,
        Paused,
        Running
    }

    /// <summary>
    /// From FILTER_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FilterInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string achName;
        [MarshalAs(UnmanagedType.Interface)]
        public IFilterGraph pGraph;
    }

    /// <summary>
    /// From AM_SEEKING_SeekingFlags
    /// </summary>
    [Flags]
    public enum AMSeekingSeekingFlags
    {
        NoPositioning = 0x00,
        AbsolutePositioning = 0x01,
        RelativePositioning = 0x02,
        IncrementalPositioning = 0x03,
        PositioningBitsMask = 0x03,
        SeekToKeyFrame = 0x04,
        ReturnTime = 0x08,
        Segment = 0x10,
        NoFlush = 0x20
    }

    /// <summary>
    /// From ALLOCATOR_PROPERTIES
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AllocatorProperties
    {
        public int cBuffers;
        public int cbBuffer;
        public int cbAlign;
        public int cbPrefix;
    }

    /// <summary>
    /// From AM_SAMPLE2_PROPERTIES
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AMSample2Properties
    {
        public int cbData;
        public AMVideoFlag dwTypeSpecificFlags;
        public AMSamplePropertyFlags dwSampleFlags;
        public int lActual;
        public long tStart;
        public long tStop;
        public int dwStreamId;
        public IntPtr pMediaType;
        public IntPtr pbBuffer; // BYTE *
        public int cbBuffer;
    }


    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("68961E68-832B-41ea-BC91-63593F3E70E3"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSample2Config
    {
        [PreserveSig]
        int GetSurface(
            [MarshalAs(UnmanagedType.IUnknown)] out object ppDirect3DSurface9
            );

    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("36b73885-c2c8-11cf-8b46-00805f6cef60"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IReferenceClock2 : IReferenceClock
    {
        #region IReferenceClock Methods

        [PreserveSig]
        new int GetTime([Out] out long pTime);

        [PreserveSig]
        new int AdviseTime(
            [In] long baseTime,
            [In] long streamTime,
            [In] IntPtr hEvent, // System.Threading.WaitHandle?
            [Out] out int pdwAdviseCookie
            );

        [PreserveSig]
        new int AdvisePeriodic(
            [In] long startTime,
            [In] long periodTime,
            [In] IntPtr hSemaphore, // System.Threading.WaitHandle?
            [Out] out int pdwAdviseCookie
            );

        [PreserveSig]
        new int Unadvise([In] int dwAdviseCookie);

        #endregion
    }

    //[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    //Guid("56a8689d-0ad4-11ce-b03a-0020af0ba770"),
    //InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    //public interface IMemInputPin
    //{
    //    [PreserveSig]
    //    int GetAllocator([Out] out IMemAllocator ppAllocator);

    //    [PreserveSig]
    //    int NotifyAllocator(
    //        [In] IMemAllocator pAllocator,
    //        [In, MarshalAs(UnmanagedType.Bool)] bool bReadOnly
    //        );

    //    [PreserveSig]
    //    int GetAllocatorRequirements([Out] out AllocatorProperties pProps);

    //    [PreserveSig]
    //    int Receive([In] IMediaSample pSample);

    //    [PreserveSig]
    //    int ReceiveMultiple(
    //        [In] IntPtr pSamples, // IMediaSample[]
    //        [In] int nSamples,
    //        [Out] out int nSamplesProcessed
    //        );

    //    [PreserveSig]
    //    int ReceiveCanBlock();
    //}

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a8689d-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMemInputPin
    {
        [PreserveSig]
        int GetAllocator([Out] out IMemAllocator ppAllocator);

        [PreserveSig]
        int NotifyAllocator(
            [In] IntPtr pAllocator,
            [In, MarshalAs(UnmanagedType.Bool)] bool bReadOnly
            );

        [PreserveSig]
        int GetAllocatorRequirements([Out] AllocatorProperties pProps);

        [PreserveSig]
        int Receive(IntPtr pSample);

        [PreserveSig]
        int ReceiveMultiple(
            [In] IntPtr pSamples,
            [In] int nSamples,
            [Out] out int nSamplesProcessed
            );

        [PreserveSig]
        int ReceiveCanBlock();
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("a3d8cec0-7e5a-11cf-bbc5-00805f6cef20"),
    Obsolete("This interface has been deprecated.", false),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMovieSetup
    {
        [PreserveSig]
        int Register();

        [PreserveSig]
        int Unregister();
    }

#endif

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a86891-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPin
    {
        [PreserveSig]
        int Connect(
            [In] IPin pReceivePin,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        int ReceiveConnection(
            [In] IPin pReceivePin,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        int Disconnect();

        [PreserveSig]
        int ConnectedTo(
            [Out] out IPin ppPin);

        /// <summary>
        /// Release returned parameter with DsUtils.FreeAMMediaType
        /// </summary>
        [PreserveSig]
        int ConnectionMediaType(
            [Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        /// <summary>
        /// Release returned parameter with DsUtils.FreePinInfo
        /// </summary>
        [PreserveSig]
        int QueryPinInfo([Out] out PinInfo pInfo);

        [PreserveSig]
        int QueryDirection(out PinDirection pPinDir);

        [PreserveSig]
        int QueryId([Out, MarshalAs(UnmanagedType.LPWStr)] out string Id);

        [PreserveSig]
        int QueryAccept([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        [PreserveSig]
        int EnumMediaTypes([Out] out IEnumMediaTypes ppEnum);

        [PreserveSig]
        int QueryInternalConnections(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IPin[] ppPins,
            [In, Out] ref int nPin
            );

        [PreserveSig]
        int EndOfStream();

        [PreserveSig]
        int BeginFlush();

        [PreserveSig]
        int EndFlush();

        [PreserveSig]
        int NewSegment(
            [In] long tStart,
            [In] long tStop,
            [In] double dRate
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("36b73880-c2c8-11cf-8b46-00805f6cef60"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSeeking
    {
        [PreserveSig]
        int GetCapabilities([Out] out AMSeekingSeekingCapabilities pCapabilities);

        [PreserveSig]
        int CheckCapabilities([In, Out] ref AMSeekingSeekingCapabilities pCapabilities);

        [PreserveSig]
        int IsFormatSupported([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int QueryPreferredFormat([Out] out Guid pFormat);

        [PreserveSig]
        int GetTimeFormat([Out] out Guid pFormat);

        [PreserveSig]
        int IsUsingTimeFormat([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int SetTimeFormat([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int GetDuration([Out] out long pDuration);

        [PreserveSig]
        int GetStopPosition([Out] out long pStop);

        [PreserveSig]
        int GetCurrentPosition([Out] out long pCurrent);

        [PreserveSig]
        int ConvertTimeFormat(
            [Out] out long pTarget,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsGuid pTargetFormat,
            [In] long Source,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsGuid pSourceFormat
            );

        [PreserveSig]
        int SetPositions(
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] DsLong pCurrent,
            [In] AMSeekingSeekingFlags dwCurrentFlags,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] DsLong pStop,
            [In] AMSeekingSeekingFlags dwStopFlags
            );

        [PreserveSig]
        int GetPositions(
            [Out] out long pCurrent,
            [Out] out long pStop
            );

        [PreserveSig]
        int GetAvailable(
            [Out] out long pEarliest,
            [Out] out long pLatest
            );

        [PreserveSig]
        int SetRate([In] double dRate);

        [PreserveSig]
        int GetRate([Out] out double pdRate);

        [PreserveSig]
        int GetPreroll([Out] out long pllPreroll);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a8689a-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSample
    {
        [PreserveSig]
        int GetPointer([Out] out IntPtr ppBuffer); // BYTE **

        [PreserveSig]
        int GetSize();

        [PreserveSig]
        int GetTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        int SetTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
            );

        [PreserveSig]
        int IsSyncPoint();

        [PreserveSig]
        int SetSyncPoint([In, MarshalAs(UnmanagedType.Bool)] bool bIsSyncPoint);

        [PreserveSig]
        int IsPreroll();

        [PreserveSig]
        int SetPreroll([In, MarshalAs(UnmanagedType.Bool)] bool bIsPreroll);

        [PreserveSig]
        int GetActualDataLength();

        [PreserveSig]
        int SetActualDataLength([In] int len);

        /// <summary>
        /// Returned object must be released with DsUtils.FreeAMMediaType()
        /// </summary>
        [PreserveSig]
        int GetMediaType([Out, MarshalAs(UnmanagedType.LPStruct)] out AMMediaType ppMediaType);

        [PreserveSig]
        int SetMediaType([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pMediaType);

        [PreserveSig]
        int IsDiscontinuity();

        [PreserveSig]
        int SetDiscontinuity([In, MarshalAs(UnmanagedType.Bool)] bool bDiscontinuity);

        [PreserveSig]
        int GetMediaTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        int SetMediaTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a86899-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaFilter : IPersist
    {
        #region IPersist Methods

        [PreserveSig]
        new int GetClassID(
            [Out] out Guid pClassID);

        #endregion

        [PreserveSig]
        int Stop();

        [PreserveSig]
        int Pause();

        [PreserveSig]
        int Run([In] long tStart);

        [PreserveSig]
        int GetState(
            [In] int dwMilliSecsTimeout,
            [Out] out FilterState filtState
            );

        [PreserveSig]
        int SetSyncSource([In] IReferenceClock pClock);

        [PreserveSig]
        int GetSyncSource([Out] out IReferenceClock pClock);

    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a86895-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBaseFilter : IMediaFilter
    {
        #region IPersist Methods

        [PreserveSig]
        new int GetClassID(
            [Out] out Guid pClassID);

        #endregion

        #region IMediaFilter Methods

        [PreserveSig]
        new int Stop();

        [PreserveSig]
        new int Pause();

        [PreserveSig]
        new int Run(long tStart);

        [PreserveSig]
        new int GetState([In] int dwMilliSecsTimeout, [Out] out FilterState filtState);

        [PreserveSig]
        new int SetSyncSource([In] IReferenceClock pClock);

        [PreserveSig]
        new int GetSyncSource([Out] out IReferenceClock pClock);

        #endregion

        [PreserveSig]
        int EnumPins([Out] out IEnumPins ppEnum);

        [PreserveSig]
        int FindPin(
            [In, MarshalAs(UnmanagedType.LPWStr)] string Id,
            [Out] out IPin ppPin
            );

        [PreserveSig]
        int QueryFilterInfo([Out] out FilterInfo pInfo);

        [PreserveSig]
        int JoinFilterGraph(
            [In, MarshalAs(UnmanagedType.Interface)] IFilterGraph pGraph,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName);

        [PreserveSig]
        int QueryVendorInfo([Out, MarshalAs(UnmanagedType.LPWStr)] out string pVendorInfo);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a8689f-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFilterGraph
    {
        [PreserveSig]
        int AddFilter(
            [In] IBaseFilter pFilter,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName
            );

        [PreserveSig]
        int RemoveFilter([In] IBaseFilter pFilter);

        [PreserveSig]
        int EnumFilters([Out] out IEnumFilters ppEnum);

        [PreserveSig]
        int FindFilterByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName,
            [Out] out IBaseFilter ppFilter
            );

        [PreserveSig]
        int ConnectDirect(
            [In] IPin ppinOut,
            [In] IPin ppinIn,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        [Obsolete("This method is obsolete; use the IFilterGraph2.ReconnectEx method instead.")]
        int Reconnect([In] IPin ppin);

        [PreserveSig]
        int Disconnect([In] IPin ppin);

        [PreserveSig]
        int SetDefaultSyncSource();
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a86893-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumFilters
    {
        [PreserveSig]
        int Next(
            [In] int cFilters,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IBaseFilter[] ppFilter,
            [In] IntPtr pcFetched
            );

        [PreserveSig]
        int Skip([In] int cFilters);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumFilters ppEnum);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a86892-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumPins
    {
        [PreserveSig]
        int Next(
            [In] int cPins,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IPin[] ppPins,
            [In] IntPtr pcFetched
            );

        [PreserveSig]
        int Skip([In] int cPins);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumPins ppEnum);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a86897-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IReferenceClock
    {
        [PreserveSig]
        int GetTime([Out] out long pTime);

        [PreserveSig]
        int AdviseTime(
            [In] long baseTime,
            [In] long streamTime,
            [In] IntPtr hEvent, // System.Threading.WaitHandle?
            [Out] out int pdwAdviseCookie
            );

        [PreserveSig]
        int AdvisePeriodic(
            [In] long startTime,
            [In] long periodTime,
            [In] IntPtr hSemaphore, // System.Threading.WaitHandle?
            [Out] out int pdwAdviseCookie
            );

        [PreserveSig]
        int Unadvise([In] int dwAdviseCookie);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("89c31040-846b-11ce-97d3-00aa0055595a"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumMediaTypes
    {
        [PreserveSig]
        int Next(
            [In] int cMediaTypes,
            [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(EMTMarshaler), SizeParamIndex = 0)] AMMediaType[] ppMediaTypes,
            [In] IntPtr pcFetched
            );

        [PreserveSig]
        int Skip([In] int cMediaTypes);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumMediaTypes ppEnum);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("36b73884-c2c8-11cf-8b46-00805f6cef60"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSample2 : IMediaSample
    {
        #region IMediaSample Methods

        [PreserveSig]
        new int GetPointer([Out] out IntPtr ppBuffer); // BYTE **

        [PreserveSig]
        new int GetSize();

        [PreserveSig]
        new int GetTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        new int SetTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
            );

        [PreserveSig]
        new int IsSyncPoint();

        [PreserveSig]
        new int SetSyncPoint([In, MarshalAs(UnmanagedType.Bool)] bool bIsSyncPoint);

        [PreserveSig]
        new int IsPreroll();

        [PreserveSig]
        new int SetPreroll([In, MarshalAs(UnmanagedType.Bool)] bool bIsPreroll);

        [PreserveSig]
        new int GetActualDataLength();

        [PreserveSig]
        new int SetActualDataLength([In] int len);

        [PreserveSig]
        new int GetMediaType([Out] out AMMediaType ppMediaType);

        [PreserveSig]
        new int SetMediaType([In] AMMediaType pMediaType);

        [PreserveSig]
        new int IsDiscontinuity();

        [PreserveSig]
        new int SetDiscontinuity([In, MarshalAs(UnmanagedType.Bool)] bool bDiscontinuity);

        [PreserveSig]
        new int GetMediaTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        new int SetMediaTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
            );

        #endregion

        [PreserveSig]
        int GetProperties(
            [In] int cbProperties,
            [In] IntPtr pbProperties // BYTE *
            );

        [PreserveSig]
        int SetProperties(
            [In] int cbProperties,
            [In] IntPtr pbProperties // BYTE *
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("92980b30-c1de-11d2-abf5-00a0c905f375"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMemAllocatorNotifyCallbackTemp
    {
        [PreserveSig]
        int NotifyRelease();
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("379a0cf0-c1de-11d2-abf5-00a0c905f375"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMemAllocatorCallbackTemp : IMemAllocator
    {
        #region IMemAllocator Methods

        [PreserveSig]
        new int SetProperties(
            [In] AllocatorProperties pRequest,
            [Out, MarshalAs(UnmanagedType.LPStruct)] AllocatorProperties pActual
            );

        [PreserveSig]
        new int GetProperties([Out] AllocatorProperties pProps);

        [PreserveSig]
        new int Commit();

        [PreserveSig]
        new int Decommit();

        [PreserveSig]
        new int GetBuffer(
            [Out] out IMediaSample ppBuffer,
            [In] long pStartTime,
            [In] long pEndTime,
            [In] AMGBF dwFlags
            );

        [PreserveSig]
        new int ReleaseBuffer(
            [In] IntPtr pBuffer
            );

        #endregion

        [PreserveSig]
        int SetNotify([In] IMemAllocatorNotifyCallbackTemp pNotify);

        [PreserveSig]
        int GetFreeCount([Out] out int plBuffersFree);
    }


    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("56a8689c-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMemAllocator
    {
        [PreserveSig]
        int SetProperties(
            [In, MarshalAs(UnmanagedType.LPStruct)] AllocatorProperties pRequest,
            [Out, MarshalAs(UnmanagedType.LPStruct)] AllocatorProperties pActual
            );

        [PreserveSig]
        int GetProperties(
            [Out, MarshalAs(UnmanagedType.LPStruct)] AllocatorProperties pProps
            );

        [PreserveSig]
        int Commit();

        [PreserveSig]
        int Decommit();

        [PreserveSig]
        int GetBuffer(
            [Out] out IMediaSample ppBuffer,
            [In] long pStartTime,
            [In] long pEndTime,
            [In] AMGBF dwFlags
            );

        [PreserveSig]
        int ReleaseBuffer(
            [In] IntPtr pBuffer
            );
    }

    //[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    //Guid("56a8689c-0ad4-11ce-b03a-0020af0ba770"),
    //InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    //public interface IMemAllocator
    //{
    //    [PreserveSig]
    //    int SetProperties(
    //        [In, MarshalAs(UnmanagedType.LPStruct)] AllocatorProperties pRequest,
    //        [Out, MarshalAs(UnmanagedType.LPStruct)] AllocatorProperties pActual
    //        );

    //    [PreserveSig]
    //    int GetProperties(
    //        [Out, MarshalAs(UnmanagedType.LPStruct)] AllocatorProperties pProps
    //        );

    //    [PreserveSig]
    //    int Commit();

    //    [PreserveSig]
    //    int Decommit();

    //    [PreserveSig]
    //    int GetBuffer(
    //        [Out] out IMediaSample ppBuffer,
    //        [In] long pStartTime,
    //        [In] long pEndTime,
    //        [In] AMGBF dwFlags
    //        );

    //    [PreserveSig]
    //    int ReleaseBuffer(
    //        [In] IMediaSample pBuffer
    //        );
    //}

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("ebec459c-2eca-4d42-a8af-30df557614b8"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IReferenceClockTimerControl
    {
        [PreserveSig]
        int SetDefaultTimerResolution(
            long timerResolution
            );

        [PreserveSig]
        int GetDefaultTimerResolution(
            out long pTimerResolution
            );
    }

    #endregion
}
