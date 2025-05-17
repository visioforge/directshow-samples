using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security.Permissions;

    using DirectShowLib;

    public static class FilterGraphHelper
    {
        /// <summary>
        /// Remove and release all filters from a DirectShow Graph
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">
        /// Thrown if the method can't enumerate its filters
        /// </exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void RemoveAllFiltersOld(this IFilterGraph2 graphBuilder)
        {
            IEnumFilters enumFilters;
            ArrayList filtersArray = new ArrayList();

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            int hr = graphBuilder.EnumFilters(out enumFilters);

            if (enumFilters == null)
            {
                return;
            }

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
                try
                {
                    graphBuilder.RemoveFilter(filter);
                }
                catch
                {
                }

                try
                {
                    Marshal.ReleaseComObject(filter);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Save a DirectShow Graph to a GRF file
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="fileName">the file to be saved</param>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Thrown if errors occur during the file creation</exception>
        /// <seealso cref="LoadGraphFile"/>
        /// <remarks>This method overwrites any existing file</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
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
                        hr = (graphBuilder as IPersistStream).Save(stream, true);
                        // ReSharper restore PossibleNullReferenceException
                        Marshal.ThrowExceptionForHR(hr);

                        hr = storage.Commit(STGC.Default);
                        Marshal.ThrowExceptionForHR(hr);
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
