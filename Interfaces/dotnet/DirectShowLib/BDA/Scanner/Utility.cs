// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="Utility.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using VisioForge.DirectShowLib;

    /// <summary>
    /// Class Utility.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
    internal static class Utility
    {
        /// <summary>
        /// The dump filter unique identifier.
        /// </summary>
        private static readonly Guid DumpFilterGuid = new Guid("{36A5F770-FE4C-11CE-A8ED-00AA002FEAB5}");

        /// <summary>
        /// The ks data format sub type bdampe g2 transport.
        /// </summary>
        private static readonly Guid KSDataFormatSubTypeBDAMPEG2Transport = new Guid("F4AEB342-0329-4fdd-A8FD-4AFF4926C978");

        /// <summary>
        /// The MPG mux filter unique identifier.
        /// </summary>
        internal static readonly Guid MpgMuxFilterGuid = new Guid("{6770E328-9B73-40C5-91E6-E2F321AEDE57}");

        /// <summary>
        /// The rik dump filter unique identifier.
        /// </summary>
        private static readonly Guid RikDumpFilterGuid = new Guid("{276806CD-1D0F-4bfd-B931-105D8199AD0F}");

        /// <summary>
        /// The rik shared sink unique identifier.
        /// </summary>
        private static readonly Guid RikSharedSinkGuid = new Guid("{C70EAEBF-724E-4054-BA01-34C82C46663D}");

        /// <summary>
        /// The rik shared source unique identifier.
        /// </summary>
        private static readonly Guid RikSharedSourceGuid = new Guid("{A5CBA878-032E-442a-ACA1-621AC313E448}");

        // Methods
        //public static IBaseFilter CreateDumpFilter(TVGraph graph)
        //{
        //    return CreateDumpFilter(graph, null);
        //}

        //public static IBaseFilter CreateDumpFilter(TVGraph graph, string name)
        //{
        //    IBaseFilter filter = FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, DumpFilterGuid, name ?? "Dump");
        //    if (filter != null)
        //    {
        //        return filter;
        //    }
        //    if (!RegisterFilter("Dump.ax"))
        //    {
        //        throw new Exception("Unable to create dump filter");
        //    }
        //    return FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, DumpFilterGuid, name ?? "Dump");
        //}

        //public static IBaseFilter CreateMpgMuxFilter(TVGraph graph, string name)
        //{
        //    return FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, MpgMuxFilterGuid, name ?? "Muxer");
        //}

        //public static IBaseFilter CreateRikDumpFilter(TVGraph graph, string name)
        //{
        //    IBaseFilter filter = FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, RikDumpFilterGuid, name ?? "Dump");
        //    if (filter != null)
        //    {
        //        return filter;
        //    }
        //    if (!RegisterFilter("RikDump.ax"))
        //    {
        //        throw new Exception("Unable to create dump filter");
        //    }
        //    return FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, RikDumpFilterGuid, name ?? "Dump");
        //}

        //public static IBaseFilter CreateRikSharedSinkFilter(TVGraph graph, string name)
        //{
        //    IBaseFilter filter = FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, RikSharedSinkGuid, name ?? "RikSink");
        //    if (filter != null)
        //    {
        //        return filter;
        //    }
        //    if (!RegisterFilter("RikDump.ax"))
        //    {
        //        throw new Exception("Unable to create sink filter");
        //    }
        //    return FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, RikSharedSinkGuid, name ?? "RikSink");
        //}

        //public static IBaseFilter CreateRikSharedSourceFilter(TVGraph graph, string name)
        //{
        //    IBaseFilter filter = FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, RikSharedSourceGuid, name ?? "RikSource");
        //    if (filter != null)
        //    {
        //        return filter;
        //    }
        //    if (!RegisterFilter("RikDump.ax"))
        //    {
        //        throw new Exception("Unable to create source filter");
        //    }
        //    return FilterGraphTools.AddFilterFromClsid(graph.GraphBuilder, RikSharedSourceGuid, name ?? "RikSource");
        //}

        /// <summary>
        /// Gets the BCD.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.Byte.</returns>
        internal static unsafe byte GetBCD(byte* p)
        {
            return (byte)((((p[0] & 240) >> 4) * 10) + (p[0] & 15));
        }

        /// <summary>
        /// Gets the bool.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="mask">The mask.</param>
        /// <param name="offset">The offset.</param>
        /// <returns><c>true</c> if successfull, <c>false</c> otherwise.</returns>
        internal static unsafe bool GetBool(byte* p, int mask, int offset)
        {
            return (((p[0] & mask) >> offset) == 1);
        }

        /// <summary>
        /// Gets the byte.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Byte.</returns>
        public static byte GetByte(byte data, byte startIndex, byte length)
        {
            byte mask = GetMask(startIndex, length);
            return (byte)((data & mask) >> ((8 - startIndex) - length));
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.Int32.</returns>
        public static unsafe int GetInt(byte* p)
        {
            return ((((p[0] << 0x18) | (p[1] << 0x10)) | (p[2] << 8)) | p[3]);
        }

        /// <summary>
        /// Gets the mask.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Byte.</returns>
        public static byte GetMask(byte startIndex, byte length)
        {
            byte num = 0;
            for (int i = 0; i < length; i++)
            {
                num = (byte)(num | ((byte)((1) << (7 - startIndex))));
                startIndex = (byte)(startIndex + 1);
            }

            return num;
        }

        /// <summary>
        /// Gets the proc address.
        /// </summary>
        /// <param name="hModule">The h module.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// Gets the short.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.Int16.</returns>
        public static unsafe short GetShort(byte* p)
        {
            return (short)((p[0] << 8) | p[1]);
        }

        /// <summary>
        /// Gets the short.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="mask">The mask.</param>
        /// <returns>System.Int16.</returns>
        public static unsafe short GetShort(byte* p, short mask)
        {
            return (short)(((p[0] << 8) | p[1]) & mask);
        }

        /// <summary>
        /// Gets the short reversed.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.Int16.</returns>
        public static unsafe short GetShortReversed(byte* p)
        {
            return (short)((p[1] << 8) | p[0]);
        }

        /// <summary>
        /// Gets the type of the ts media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetTSMediaType()
        {
            AMMediaType type = new AMMediaType();
            type.majorType = MediaType.Stream;
            type.subType = KSDataFormatSubTypeBDAMPEG2Transport;
            type.formatType = FormatType.None;
            return type;
        }

        /// <summary>
        /// Gets the u int.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.UInt32.</returns>
        internal static unsafe uint GetUInt(byte* p)
        {
            return (uint)((((p[0] << 0x18) | (p[1] << 0x10)) | (p[2] << 8)) | p[3]);
        }

        /// <summary>
        /// Gets the u short.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.UInt16.</returns>
        public static unsafe ushort GetUShort(byte* p)
        {
            return (ushort)((p[0] << 8) | p[1]);
        }

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the lp file.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// Loads the section.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="tid">The tid.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>List&lt;MPEGTable&gt;.</returns>
        public static List<MPEGTable> LoadSection(IMpeg2Data data, short pid, TableType tid, int timeout)
        {
            ISectionList list2;
            List<MPEGTable> list = new List<MPEGTable>();
            int hr = data.GetSection(pid, (byte)tid, null, timeout, out list2);

            switch (hr)
            {
                case 0:
                    short num2;
                    DsError.ThrowExceptionForHR(list2.GetNumberOfSections(out num2));
                    for (short i = 0; i < num2; i = (short)(i + 1))
                    {
                        int num4;
                        IntPtr ptr;
                        DsError.ThrowExceptionForHR(list2.GetSectionData(i, out num4, out ptr));
                        MPEGTable section = MPEGTable.GetSection(ptr, num4);
                        if (section != null)
                        {
                            list.Add(section);
                        }
                    }

                    return list;

                case -2147467262:
                    {
                        FilterInfo info;
                        IBaseFilter filter = (IBaseFilter)data;
                        DsError.ThrowExceptionForHR(filter.QueryFilterInfo(out info));
                        ReleaseFilterInfo(ref info);

                        // Log.WriteLine("IMpeg2Data interface not found - {0}", new object[] { info.achName });

                        DsError.ThrowExceptionForHR(filter.Stop());
                        DsError.ThrowExceptionForHR(filter.Run(0L));

                        // Log.WriteLine("Error loading SI table: The strange error. Attempting recovery. '{0}({3:x})' {1}/{2}", new object[] { DsError.GetErrorText(hr), (DVBReservedPID)pid, tid, hr });
                        DsError.ThrowExceptionForHR(hr);
                        break;
                    }
            }

            return list;
        }

        /// <summary>
        /// Loads the section.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="tid">The tid.</param>
        /// <param name="sectionNumber">The section number.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>List&lt;MPEGTable&gt;.</returns>
        public static List<MPEGTable> LoadSection(IMpeg2Data data, short pid, TableType tid, byte sectionNumber, int timeout)
        {
            ISectionList list2;
            List<MPEGTable> list = new List<MPEGTable>();
            MPEG2Filter structure = new MPEG2Filter();
            structure.bVersionNumber = 1;
            structure.wFilterSize = (short)Marshal.SizeOf(structure);
            structure.fSpecifySectionNumber = true;
            structure.SectionNumber = sectionNumber;
            int hr = data.GetSection(pid, (byte)tid, structure, timeout, out list2);
            switch (hr)
            {
                case 0:
                    short num2;
                    DsError.ThrowExceptionForHR(list2.GetNumberOfSections(out num2));
                    for (short i = 0; i < num2; i = (short)(i + 1))
                    {
                        int num4;
                        IntPtr ptr;
                        DsError.ThrowExceptionForHR(list2.GetSectionData(i, out num4, out ptr));
                        MPEGTable section = MPEGTable.GetSection(ptr, num4);
                        if (section != null)
                        {
                            list.Add(section);
                        }
                    }

                    return list;

                case -2147467262:
                    {
                        FilterInfo info;
                        IBaseFilter filter2 = (IBaseFilter)data;
                        DsError.ThrowExceptionForHR(filter2.QueryFilterInfo(out info));
                        ReleaseFilterInfo(ref info);

                        //Log.WriteLine("IMpeg2Data interface not found - {0}", new object[] { info.achName });
                        DsError.ThrowExceptionForHR(filter2.Stop());
                        DsError.ThrowExceptionForHR(filter2.Run(0L));

                        //Log.WriteLine("Error loading SI table: The strange error. Attempting recovery. '{0}({3:x})' {1}/{2}", new object[] { DsError.GetErrorText(hr), (DVBReservedPID)pid, tid, hr });
                        DsError.ThrowExceptionForHR(hr);
                        break;
                    }
            }

            return list;
        }

        /// <summary>
        /// Loads the section.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="tid">The tid.</param>
        /// <param name="mpegFilter">The MPEG filter.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>List&lt;MPEGTable&gt;.</returns>
        public static List<MPEGTable> LoadSection(IMpeg2Data data, short pid, byte tid, MPEG2Filter mpegFilter, int timeout)
        {
            ISectionList list2;
            List<MPEGTable> list = new List<MPEGTable>();
            int hr = data.GetSection(pid, tid, mpegFilter, timeout, out list2);
            switch (hr)
            {
                case 0:
                    short num2;
                    DsError.ThrowExceptionForHR(list2.GetNumberOfSections(out num2));
                    for (short i = 0; i < num2; i = (short)(i + 1))
                    {
                        int num4;
                        IntPtr ptr;
                        DsError.ThrowExceptionForHR(list2.GetSectionData(i, out num4, out ptr));
                        MPEGTable section = MPEGTable.GetSection(ptr, num4);
                        if (section != null)
                        {
                            list.Add(section);
                        }
                    }

                    return list;

                case -2147467262:
                    {
                        FilterInfo info;
                        IBaseFilter filter = (IBaseFilter)data;
                        DsError.ThrowExceptionForHR(filter.QueryFilterInfo(out info));
                        ReleaseFilterInfo(ref info);

                        //Log.WriteLine("IMpeg2Data interface not found - {0}", new object[] { info.achName });
                        DsError.ThrowExceptionForHR(filter.Stop());
                        DsError.ThrowExceptionForHR(filter.Run(0L));

                        //Log.WriteLine("Error loading SI table: The strange error. Attempting recovery. '{0}({2:x})' {1}/--", new object[] { DsError.GetErrorText(hr), (DVBReservedPID)pid, hr });
                        DsError.ThrowExceptionForHR(hr);
                        break;
                    }
            }

            return list;
        }

        /// <summary>
        /// Loads the table.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="tid">The tid.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>List&lt;MPEGTable&gt;.</returns>
        public static List<MPEGTable> LoadTable(IMpeg2Data data, short pid, TableType tid, int timeout)
        {
            ISectionList list2;
            int num2;
            List<MPEGTable> list = new List<MPEGTable>();
            int num = timeout;
        Label_0008:
            num2 = data.GetSection(pid, (byte)tid, null, timeout, out list2);
            switch (num2)
            {
                case 0:
                    short num3;
                    DsError.ThrowExceptionForHR(list2.GetNumberOfSections(out num3));
                    for (short i = 0; i < num3; i = (short)(i + 1))
                    {
                        int num5;
                        IntPtr ptr;
                        DsError.ThrowExceptionForHR(list2.GetSectionData(i, out num5, out ptr));
                        MPEGTable section = MPEGTable.GetSection(ptr, num5);
                        if (section != null)
                        {
                            list.Add(section);
                        }
                    }

                    return list;

                case -2147467262:
                    {
                        FilterInfo info;
                        IBaseFilter filter = (IBaseFilter)data;
                        DsError.ThrowExceptionForHR(filter.QueryFilterInfo(out info));
                        ReleaseFilterInfo(ref info);

                        //Log.WriteLine("IMpeg2Data interface not found - {0}", new object[] { info.achName });
                        DsError.ThrowExceptionForHR(filter.Stop());
                        DsError.ThrowExceptionForHR(filter.Run(0L));

                        //Log.WriteLine("Error loading SI table: The strange error. Attempting recovery. '{0}({3:x})' {1}/{2}", new object[] { DsError.GetErrorText(num2), (DVBReservedPID)pid, tid, num2 });
                        DsError.ThrowExceptionForHR(num2);
                        return list;
                    }
            }

            //Log.WriteLine("Error loading SI table: '{0}({3:x})' {1}/{2}", new object[] { DsError.GetErrorText(num2), (DVBReservedPID)pid, tid, num2 });
#pragma warning disable S2589 // Boolean expressions should not be gratuitous
            if (timeout == num)
            {
                //Log.WriteLine("Retrying with timeout " + (timeout * 2), new object[0]);
                goto Label_0008;
            }
#pragma warning restore S2589 // Boolean expressions should not be gratuitous

            return list;
        }

        /// <summary>
        /// Ntohes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.Int16.</returns>
        public static short NTOH(short input)
        {
            return (short)(((input & 0xff00) >> 8) | ((input & 0xff) << 8));
        }

        /// <summary>
        /// Registers the filter.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if successfull, <c>false</c> otherwise.</returns>
        private static bool RegisterFilter(string fileName)
        {
            string path1 = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (path1 != null)
            {
                string path = Path.Combine(path1, fileName);
                if (!File.Exists(path))
                {
                    //Log.WriteLine("Dump filter unavailable from '{0}'. Unable to register filter.", new object[] { path });
                    return false;
                }

                IntPtr hModule = LoadLibrary(path);
                if (hModule == IntPtr.Zero)
                {
                    //Log.WriteLine("Unable to load Dump filter library.", new object[0]);
                    return false;
                }

                IntPtr procAddress = GetProcAddress(hModule, "DllRegisterServer");
                if (procAddress == IntPtr.Zero)
                {
                    //Log.WriteLine("Unable to find DllRegisterServer in Dump Filter", new object[0]);
                    return false;
                }

                DllRegisterServerInvoker delegateForFunctionPointer = (DllRegisterServerInvoker)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(DllRegisterServerInvoker));
                int num = delegateForFunctionPointer();
                if (num != 0)
                {
                    //Log.WriteLine("Error registering dump filter - '{0}'", new object[] { num });
                    return false;
                }
            }

            //Log.WriteLine("Dump filter registered", new object[0]);
            return true;
        }

        /// <summary>
        /// Delegate DllRegisterServerInvoker.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private delegate int DllRegisterServerInvoker();

        private static void ReleaseFilterInfo(ref FilterInfo fi)
        {
            if (fi.pGraph == null)
            {
                return;
            }

            Marshal.ReleaseComObject(fi.pGraph);
            fi.pGraph = null;
        }
    }
}