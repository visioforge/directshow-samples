// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-22-2021
//
// Last Modified By : roman
// Last Modified On : 12-22-2021
// ***********************************************************************
// <copyright file="DSHelper.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security.Permissions;
    using DirectShowLib;

    /// <summary>
    /// Class Helper.
    /// </summary>
    public static class DSHelper
    {
        /// <summary>
        /// Adds the filter from CLSID.
        /// </summary>
        /// <param name="graphBuilder">The graph builder.</param>
        /// <param name="clsid">The CLSID.</param>
        /// <param name="name">The name.</param>
        /// <returns>IBaseFilter.</returns>
        /// <exception cref="System.ArgumentNullException">graphBuilder.</exception>
        [Localizable(false)]
#if NET472
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
#endif
        public static IBaseFilter AddFilterFromClsid(IGraphBuilder graphBuilder, Guid clsid, string name)
        {
            IBaseFilter filter = null;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            try
            {
                Type type = Type.GetTypeFromCLSID(clsid);
                filter = (IBaseFilter)Activator.CreateInstance(type);

                int hr = graphBuilder.AddFilter(filter, name);
                DsError.ThrowExceptionForHR(hr);
            }
            catch
            {
                if (filter != null)
                {
                    Marshal.ReleaseComObject(filter);
                    filter = null;
                }
            }

            return filter;
        }

        /// <summary>
        /// Applies video format.
        /// </summary>
        /// <param name="streamConfig">
        /// Stream config.
        /// </param>
        /// <param name="videoCaptureFormat">
        /// Video capture format.
        /// </param>
        /// <param name="frameRate">
        /// Frame rate.
        /// </param>
        /// <returns>
        /// Returns true if the operation was successful.
        /// </returns>
        public static bool ApplyVideoFormat(
            IAMStreamConfig streamConfig, VFVideoCaptureFormat videoCaptureFormat, double frameRate)
        {
            AMMediaType mt = new AMMediaType();
            bool result = false;
            try
            {
                if (streamConfig == null)
                {
                    return false;
                }

                if (videoCaptureFormat == null)
                {
                    return false;
                }

                mt.majorType = videoCaptureFormat.MajorType;
                mt.subType = videoCaptureFormat.SubType;
                mt.formatType = videoCaptureFormat.FormatType;
                mt.fixedSizeSamples = videoCaptureFormat.FixedSizeSamples;
                mt.sampleSize = videoCaptureFormat.SampleSize;
                mt.temporalCompression = videoCaptureFormat.TemporalCompression;

                // frame rate
                if (Math.Abs(frameRate - 0) < double.Epsilon)
                {
                    frameRate = 1;
                }

                videoCaptureFormat.VIH.AvgTimePerFrame = (long)Math.Round(10000000 / frameRate);

                if (mt.formatType == FormatType.VideoInfo)
                {
                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.VIH));
                    Marshal.StructureToPtr(videoCaptureFormat.VIH, mt.formatPtr, true);
                    mt.formatSize = Marshal.SizeOf(typeof(VideoInfoHeader));
                }
                else if (mt.formatType == FormatType.VideoInfo2)
                {
                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.VIH2));
                    Marshal.StructureToPtr(videoCaptureFormat.VIH2, mt.formatPtr, true);
                    mt.formatSize = Marshal.SizeOf(typeof(VideoInfoHeader2));
                }
                else if (mt.formatType == FormatType.DvInfo)
                {
                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.DV));
                    Marshal.StructureToPtr(videoCaptureFormat.DV, mt.formatPtr, true);
                    mt.formatSize = Marshal.SizeOf(typeof(DVInfo));
                }
                //else if (videoCaptureFormat.FormatType == FormatType.MpegVideo)
                //{
                //    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.MPEG1));
                //    Marshal.StructureToPtr(videoCaptureFormat.MPEG1, mt.formatPtr, true);
                //    mt.formatSize = Marshal.SizeOf(typeof(MPEG1VideoInfo));
                //}
                else if (videoCaptureFormat.FormatType == FormatType.Mpeg2Video)
                {
                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.MPEG2));
                    Marshal.StructureToPtr(videoCaptureFormat.MPEG2, mt.formatPtr, true);
                    mt.formatSize = Marshal.SizeOf(typeof(MPEG2VideoInfo));
                }
                else
                {
                    return false;
                }

                int hr = streamConfig.SetFormat(mt);

                if (hr == 0)
                {
                    result = true;
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Removes all filters.
        /// </summary>
        /// <param name="graphBuilder">The graph builder.</param>
        /// <param name="final">if set to <c>true</c> [final].</param>
        /// <exception cref="System.ArgumentNullException">graphBuilder.</exception>
#if NET472
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
#endif
        public static void RemoveAllFilters(IGraphBuilder graphBuilder, bool final)
        {
            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            int hr = 0;
            IEnumFilters enumFilters;
            ArrayList filtersArray = new ArrayList();

            hr = graphBuilder.EnumFilters(out enumFilters);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                IBaseFilter[] filters = new IBaseFilter[1];

                while (enumFilters.Next(filters.Length, filters, IntPtr.Zero) == 0)
                {
                    filtersArray.Add(filters[0]);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(enumFilters);
            }

            foreach (IBaseFilter filter in filtersArray)
            {
                graphBuilder.RemoveFilter(filter);

                if (final)
                {
                    Marshal.FinalReleaseComObject(filter);
                }
                else
                {
                    Marshal.ReleaseComObject(filter);
                }
            }
        }


        /// <summary>
        /// Removes duplicate list.
        /// </summary>
        /// <param name="list">
        /// Input list.
        /// </param>
        /// <returns>
        /// Returns result list.
        /// </returns>
        public static List<string> RemoveDuplicateStrings(List<string> list)
        {
            Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
            List<string> finalList = new List<string>();
            int i = 0;

            if (list != null)
            {
                foreach (string currValue in list)
                {
                    if (!uniqueStore.ContainsKey(currValue))
                    {
                        uniqueStore.Add(currValue, 0);
                        finalList.Add(list[i]);
                    }

                    i++;
                }
            }

            return finalList;
        }

        /// <summary>
        /// Adds the file source.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="filterGraph">The filter graph.</param>
        /// <returns>IBaseFilter.</returns>
        /// <exception cref="System.IO.FileNotFoundException">File not found.</exception>
        public static IBaseFilter AddFileSource(string filename, IFilterGraph2 filterGraph)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("File not found", filename);
            }

            var hr = filterGraph.AddSourceFilter(filename, "SRC", out var file);
            DsError.ThrowExceptionForHR(hr);

            return file;
        }

        /// <summary>
        /// Saves the graph file.
        /// </summary>
        /// <param name="graphBuilder">The graph builder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.ArgumentNullException">graphBuilder.</exception>
#if NET472
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
#endif
        [SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "All OK.")]
        [SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "All OK.")]
        public static void SaveGraphFile(IFilterGraph2 graphBuilder, string fileName)
        {
            try
            {
                IStorage storage = null;
                IStream stream = null;

                if (graphBuilder == null)
                {
                    throw new ArgumentNullException("graphBuilder");
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                fileName = fileName.Replace("\\\\", "\\");

                try
                {
                    var path = fileName.Replace(Path.GetFileName(fileName), string.Empty);
                    Directory.CreateDirectory(path);
                }
                catch
                {
                }

                try
                {
                    try
                    {
                        int hr = NativeMethods.StgCreateDocfile(
                            fileName,
                            STGM.Create | STGM.Transacted | STGM.ReadWrite | STGM.ShareExclusive,
                            0,
                            out storage);

                        Marshal.ThrowExceptionForHR(hr);

                        hr = storage.CreateStream(
                            @"ActiveMovieGraph",
                            STGM.Write | STGM.Create | STGM.ShareExclusive,
                            0,
                            0,
                            out stream);

                        Marshal.ThrowExceptionForHR(hr);

                        // ReSharper disable PossibleNullReferenceException
                        var persistStream = graphBuilder as IPersistStream;
                        hr = persistStream.Save(stream, true);
                        if (hr != 0)
                        {
                            return;
                        }

                        hr = storage.Commit(STGC.Default);
                        if (hr != 0)
                        {
                            return;
                        }
                    }
                    catch
                    {
                    }
                }
                finally
                {
                    if (stream != null)
                    {
                        Marshal.ReleaseComObject(stream);
                    }

                    if (storage != null)
                    {
                        Marshal.ReleaseComObject(storage);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
