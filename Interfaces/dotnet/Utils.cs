namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security.Permissions;

    using DirectShowLib;

    /// <summary>
    /// A collection of methods to do common DirectShow tasks.
    /// </summary>
    [Localizable(false)]
    public static class FilterGraphTools
    {
        /// <summary>
        /// Check if a DirectShow filter can display Property Pages
        /// </summary>
        /// <param name="filter">A DirectShow Filter</param>
        /// <exception cref="System.ArgumentNullException">Thrown if filter is null</exception>
        /// <seealso cref="ShowFilterPropertyPage"/>
        /// <returns>true if the filter has Property Pages or VfW Compression Dialog, false if not</returns>
        /// <remarks>
        /// This method is intended to be used with <see cref="ShowFilterPropertyPage">ShowFilterPropertyPage</see>
        /// </remarks>
        public static bool HasPropertyPagesOrCompressionDialog(IBaseFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            return ((filter as ISpecifyPropertyPages) != null) || ((filter as IAMVfwCompressDialogs) != null);
        }

        /// <summary>
        /// Display Property Pages of a given DirectShow filter
        /// </summary>
        /// <param name="filter">
        /// A DirectShow Filter
        /// </param>
        /// <param name="parent">
        /// A hwnd handle of a window to contain the pages
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if filter is null
        /// </exception>
        /// <seealso cref="HasPropertyPages"/>
        /// <remarks>
        /// You can check if a filter supports Property Pages with the <see cref="HasPropertyPages">HasPropertyPages</see> method.<br/>
        /// <strong>Warning</strong> : This method is blocking. It only returns when the Property Pages are closed.
        /// </remarks>
        /// <example>
        /// This sample shows how to check if a filter supports Property Pages and displays them
        /// <code>
        /// if (FilterGraphTools.HasPropertyPages(myFilter))
        /// {
        ///   FilterGraphTools.ShowFilterPropertyPage(myFilter, myForm.Handle);
        /// }
        /// </code>
        /// </example>
        /// <returns>
        /// True or False.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool ShowFilterPropertyPage(IBaseFilter filter, IntPtr parent)
        {
            int hr;

            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            // ReSharper disable RedundantIfElseBlock
            if (HasPropertyPages(filter))
            {
                FilterInfo filterInfo;
                hr = filter.QueryFilterInfo(out filterInfo);
                DsError.ThrowExceptionForHR(hr);

                if (filterInfo.pGraph != null)
                {
                    Marshal.ReleaseComObject(filterInfo.pGraph);
                }

                // ReSharper disable PossibleNullReferenceException
                DsCAUUID dscaGuid;
                hr = (filter as ISpecifyPropertyPages).GetPages(out dscaGuid);
                // ReSharper restore PossibleNullReferenceException
                DsError.ThrowExceptionForHR(hr);

                try
                {
                    object[] objs = new object[1];
                    objs[0] = filter;

                    NativeMethods.OleCreatePropertyFrame(
                        parent,
                        0,
                        0,
                        filterInfo.achName,
                        objs.Length,
                        objs,
                        dscaGuid.cElems,
                        dscaGuid.pElems,
                        0,
                        0,
                        IntPtr.Zero);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(dscaGuid.pElems);
                }

                return true;
            }
            else
            {
                // check IAMVfwCompressDialogs dialog

                // If the filter doesn't implement ISpecifyPropertyPages, try displaying IAMVfwCompressDialogs instead!
                IAMVfwCompressDialogs compressDialog = filter as IAMVfwCompressDialogs;
                if (compressDialog != null)
                {
                    hr = compressDialog.ShowDialog(VfwCompressDialogs.Config, IntPtr.Zero);
                    // DsError.ThrowExceptionForHR(hr);
                    if (hr != 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            // ReSharper restore RedundantIfElseBlock
        }

        /// <summary>
        /// Add a filter to a DirectShow Graph using its CLSID
        /// </summary>
        /// <param name="graphBuilder">the IGraphBuilder interface of the graph</param>
        /// <param name="clsid">a valid CLSID. This object must implement IBaseFilter</param>
        /// <param name="name">the name used in the graph (may be null)</param>
        /// <returns>an instance of the filter if the method successfully created it, null if not</returns>
        /// <remarks>
        /// You can use <see cref="IsThisComObjectInstalled">IsThisComObjectInstalled</see> to check is the CLSID is valid before calling this method
        /// </remarks>
        /// <example>This sample shows how to programmatically add a NVIDIA Video decoder filter to a graph
        /// <code>
        /// Guid nvidiaVideoDecClsid = new Guid("71E4616A-DB5E-452B-8CA5-71D9CC7805E9");
        /// if (FilterGraphTools.IsThisComObjectInstalled(nvidiaVideoDecClsid))
        /// {
        ///   filter = FilterGraphTools.AddFilterFromClsid(graphBuilder, nvidiaVideoDecClsid, "NVIDIA Video Decoder");
        /// }
        /// else
        /// {
        ///   // use another filter...
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsThisComObjectInstalled"/>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">
        /// Thrown if errors occur when the filter is add to the graph
        /// </exception>
        [Localizable(false)]
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
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
        /// Add a filter to a DirectShow Graph using its name
        /// </summary>
        /// <param name="graphBuilder">the IGraphBuilder interface of the graph</param>
        /// <param name="deviceCategory">the filter category (see DirectShowLib.FilterCategory)</param>
        /// <param name="friendlyName">the filter name (case-sensitive)</param>
        /// <returns>an instance of the filter if the method successfully created it, null if not</returns>
        /// <example>This sample shows how to programmatically add a NVIDIA Video decoder filter to a graph
        /// <code>
        /// filter = FilterGraphTools.AddFilterByName(graphBuilder, FilterCategory.LegacyAmFilterCategory, "NVIDIA Video Decoder");
        /// </code>
        /// </example>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">
        /// Thrown if errors occur when the filter is add to the graph
        /// </exception>
        public static IBaseFilter AddFilterByName(IFilterGraph2 graphBuilder, Guid deviceCategory, string friendlyName)
        {
            IBaseFilter filter = null;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            // Thread.Sleep(100);
            DsDevice[] devices = DsDevice.GetDevicesOfCat(deviceCategory);

            for (int i = 0; i < devices.Length; i++)
            {
                if (!devices[i].Name.Trim().Equals(friendlyName.Trim()))
                {
                    continue;
                }

                // MessageBox.Show("x2 " + friendlyName);

                // Debug.WriteLine("Adding source filter for moniker - " + friendlyName);
                int hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, friendlyName, out filter);
                DsError.ThrowExceptionForHR(hr);
                // Debug.WriteLine("Adding source filter for moniker - " + friendlyName + "...Done");

                break;
            }

            // MessageBox.Show("x4");
            return filter;
        }

        /// <summary>
        /// Add a filter to a DirectShow Graph using its Moniker's device path
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="devicePath">a moniker path</param>
        /// <param name="name">the name to use for the filter in the graph</param>
        /// <returns>an instance of the filter if the method successfully creates it, null if not</returns>
        /// <example>This sample shows how to programmatically add a NVIDIA Video decoder filter to a graph
        /// <code>
        /// string devicePath = @"@device:sw:{083863F1-70DE-11D0-BD40-00A0C911CE86}\{71E4616A-DB5E-452B-8CA5-71D9CC7805E9}";
        /// filter = FilterGraphTools.AddFilterByDevicePath(graphBuilder, devicePath, "NVIDIA Video Decoder");
        /// </code>
        /// </example>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">
        /// Thrown if errors occur when the filter is add to the graph
        /// </exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static IBaseFilter AddFilterByDevicePath(IFilterGraph2 graphBuilder, string devicePath, string name)
        {
            IBaseFilter filter = null;
            IBindCtx bindCtx = null;
            IMoniker moniker = null;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            try
            {
                int hr = NativeMethods.CreateBindCtx(0, out bindCtx);
                Marshal.ThrowExceptionForHR(hr);

                int eaten;
                hr = NativeMethods.MkParseDisplayName(bindCtx, devicePath, out eaten, out moniker);
                Marshal.ThrowExceptionForHR(hr);

                hr = graphBuilder.AddSourceFilterForMoniker(moniker, bindCtx, name, out filter);
                DsError.ThrowExceptionForHR(hr);
            }
            catch
            {
                // An error occur. Just returning null...
            }
            finally
            {
                if (bindCtx != null)
                {
                    Marshal.ReleaseComObject(bindCtx);
                }

                if (moniker != null)
                {
                    Marshal.ReleaseComObject(moniker);
                }
            }

            return filter;
        }

        /// <summary>
        /// Find a filter in a DirectShow Graph using its name
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="filterName">the filter name to find (case-sensitive)</param>
        /// <returns>an instance of the filter if found, null if not</returns>
        /// <seealso cref="FindFilterByClsid"/>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static IBaseFilter FindFilterByName(IFilterGraph2 graphBuilder, string filterName)
        {
            IBaseFilter filter = null;
            IEnumFilters enumFilters;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            int hr = graphBuilder.EnumFilters(out enumFilters);
            if (hr == 0)
            {
                IBaseFilter[] filters = new IBaseFilter[1];

                while (enumFilters.Next(filters.Length, filters, IntPtr.Zero) == 0)
                {
                    FilterInfo filterInfo;

                    hr = filters[0].QueryFilterInfo(out filterInfo);
                    if (hr == 0)
                    {
                        if (filterInfo.pGraph != null)
                        {
                            Marshal.ReleaseComObject(filterInfo.pGraph);
                        }

                        if (filterInfo.achName.Equals(filterName))
                        {
                            filter = filters[0];
                            break;
                        }
                    }

                    Marshal.ReleaseComObject(filters[0]);
                }

                Marshal.ReleaseComObject(enumFilters);
            }

            return filter;
        }

        /// <summary>
        /// Find a filter in a DirectShow Graph using its CLSID
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="filterClsid">the CLSID to find</param>
        /// <returns>an instance of the filter if found, null if not</returns>
        /// <seealso cref="FindFilterByName"/>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static IBaseFilter FindFilterByClsid(IFilterGraph2 graphBuilder, Guid filterClsid)
        {
            IBaseFilter filter = null;
            IEnumFilters enumFilters;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            int hr = graphBuilder.EnumFilters(out enumFilters);
            if (hr == 0)
            {
                IBaseFilter[] filters = new IBaseFilter[1];

                while (enumFilters.Next(filters.Length, filters, IntPtr.Zero) == 0)
                {
                    Guid clsid;

                    hr = filters[0].GetClassID(out clsid);

                    if ((hr == 0) && (clsid == filterClsid))
                    {
                        filter = filters[0];
                        break;
                    }

                    Marshal.ReleaseComObject(filters[0]);
                }

                Marshal.ReleaseComObject(enumFilters);
            }

            return filter;
        }

        /// <summary>
        /// Render a filter's pin in a DirectShow Graph
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="source">the filter containing the pin to render</param>
        /// <param name="pinName">the pin name</param>
        /// <returns>true if rendering is a success, false if not</returns>
        /// <example>
        /// <code>
        /// hr = graphBuilder.AddSourceFilter(@"foo.avi", "Source Filter", out filter);
        /// DsError.ThrowExceptionForHR(hr);
        /// if (!FilterGraphTools.RenderPin(graphBuilder, filter, "Output"))
        /// {
        ///   // Something went wrong...
        /// }
        /// </code>
        /// </example>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder or source is null</exception>
        /// <remarks>This method assumes that the filter is part of the given graph</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool RenderPin(IFilterGraph2 graphBuilder, IBaseFilter source, string pinName)
        {
            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            IPin pin = DsFindPin.ByName(source, pinName);

            if (pin != null)
            {
                int hr = graphBuilder.Render(pin);
                Marshal.ReleaseComObject(pin);

                return hr >= 0;
            }

            return false;
        }

        /// <summary>
        /// Disconnect all pins on a given filter
        /// </summary>
        /// <param name="filter">the filter on which to disconnect all the pins</param>
        /// <exception cref="System.ArgumentNullException">Thrown if filter is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Thrown if errors occured during the disconnection process</exception>
        /// <remarks>Both input and output pins are disconnected</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void DisconnectPins(IBaseFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            IEnumPins enumPins;
            IPin[] pins = new IPin[1];

            int hr = filter.EnumPins(out enumPins);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                while (enumPins.Next(pins.Length, pins, IntPtr.Zero) == 0)
                {
                    try
                    {
                        hr = pins[0].Disconnect();
                        DsError.ThrowExceptionForHR(hr);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(pins[0]);
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(enumPins);
            }
        }

        /// <summary>
        /// Disconnect pins of all the filters in a DirectShow Graph
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Thrown if the method can't enumerate its filters</exception>
        /// <remarks>This method doesn't throw an exception if an error occurs during pin disconnections</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void DisconnectAllPins(IFilterGraph2 graphBuilder)
        {
            IEnumFilters enumFilters;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            int hr = graphBuilder.EnumFilters(out enumFilters);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                IBaseFilter[] filters = new IBaseFilter[1];

                while (enumFilters.Next(filters.Length, filters, IntPtr.Zero) == 0)
                {
                    try
                    {
                        DisconnectPins(filters[0]);
                    }
                    catch
                    {
                    }

                    Marshal.ReleaseComObject(filters[0]);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(enumFilters);
            }
        }

        /// <summary>
        /// Remove and release all filters from a DirectShow Graph
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">
        /// Thrown if the method can't enumerate its filters
        /// </exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void RemoveAllFilters(IFilterGraph2 graphBuilder)
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

                try
                {
                    int hr = NativeMethods.StgCreateDocfile(
                        fileName, STGM.Create | STGM.Transacted | STGM.ReadWrite | STGM.ShareExclusive, 0, out storage);

                    Marshal.ThrowExceptionForHR(hr);

                    hr = storage.CreateStream(
                        @"ActiveMovieGraph", STGM.Write | STGM.Create | STGM.ShareExclusive, 0, 0, out stream);

                    Marshal.ThrowExceptionForHR(hr);

                    // ReSharper disable PossibleNullReferenceException
                    hr = (graphBuilder as IPersistStream).Save(stream, true);
                    // ReSharper restore PossibleNullReferenceException
                    Marshal.ThrowExceptionForHR(hr);

                    hr = storage.Commit(STGC.Default);
                    Marshal.ThrowExceptionForHR(hr);
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

        /// <summary>
        /// Load a DirectShow Graph from a file
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="fileName">the file to be loaded</param>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder is null</exception>
        /// <exception cref="System.ArgumentException">Thrown if the given file is not a valid graph file</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Thrown if errors occur during loading</exception>
        /// <seealso cref="SaveGraphFile"/>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void LoadGraphFile(IFilterGraph2 graphBuilder, string fileName)
        {
            IStorage storage = null;
            IStream stream = null;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            try
            {
                if (NativeMethods.StgIsStorageFile(fileName) != 0)
                {
                    throw new ArgumentException("Incorrect filename", "fileName");
                }

                int hr = NativeMethods.StgOpenStorage(
                    fileName, null, STGM.Transacted | STGM.Read | STGM.ShareDenyWrite, IntPtr.Zero, 0, out storage);

                Marshal.ThrowExceptionForHR(hr);

                hr = storage.OpenStream(
                    @"ActiveMovieGraph", IntPtr.Zero, STGM.Read | STGM.ShareExclusive, 0, out stream);

                Marshal.ThrowExceptionForHR(hr);

                // ReSharper disable PossibleNullReferenceException
                hr = (graphBuilder as IPersistStream).Load(stream);
                // ReSharper restore PossibleNullReferenceException
                Marshal.ThrowExceptionForHR(hr);
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

        /// <summary>
        /// Check if a DirectShow filter can display Property Pages
        /// </summary>
        /// <param name="filter">A DirectShow Filter</param>
        /// <exception cref="System.ArgumentNullException">Thrown if filter is null</exception>
        /// <seealso cref="ShowFilterPropertyPage"/>
        /// <returns>true if the filter has Property Pages, false if not</returns>
        /// <remarks>
        /// This method is intended to be used with <see cref="ShowFilterPropertyPage">ShowFilterPropertyPage</see>
        /// </remarks>
        public static bool HasPropertyPages(IBaseFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            return (filter as ISpecifyPropertyPages) != null;
        }

        /// <summary>
        /// Check if a COM Object is available
        /// </summary>
        /// <param name="clsid">The CLSID of this object</param>
        /// <example>This sample shows how to check if the MPEG-2 Demultiplexer filter is available
        /// <code>
        /// if (FilterGraphTools.IsThisComObjectInstalled(typeof(MPEG2Demultiplexer).GUID))
        /// {
        ///   // Use it...
        /// }
        /// </code>
        /// </example>
        /// <returns>true if the object is available, false if not</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool IsThisComObjectInstalled(Guid clsid)
        {
            bool retval = false;

            try
            {
                Type type = Type.GetTypeFromCLSID(clsid);
                object o = Activator.CreateInstance(type);
                retval = true;
                Marshal.ReleaseComObject(o);
            }
            catch
            {
            }

            return retval;
        }

        /// <summary>
        /// Check if the Video Mixing Renderer 9 Filter is available
        /// <seealso cref="IsThisComObjectInstalled"/>
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="IsThisComObjectInstalled">IsThisComObjectInstalled</see> internally
        /// </remarks>
        /// <returns>true if VMR9 is present, false if not</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool IsVMR9Present()
        {
            return IsThisComObjectInstalled(typeof(VideoMixingRenderer9).GUID);
        }

        /// <summary>
        /// Check if the Video Mixing Renderer 7 Filter is available
        /// <seealso cref="IsThisComObjectInstalled"/>
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="IsThisComObjectInstalled">IsThisComObjectInstalled</see> internally
        /// </remarks>
        /// <returns>true if VMR7 is present, false if not</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool IsVMR7Present()
        {
            return IsThisComObjectInstalled(typeof(VideoMixingRenderer).GUID);
        }

        /// <summary>
        /// Connect pins from two filters
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="upFilter">the upstream filter</param>
        /// <param name="sourcePinName">the upstream filter pin name</param>
        /// <param name="downFilter">the downstream filter</param>
        /// <param name="destPinName">the downstream filter pin name</param>
        /// <param name="useIntelligentConnect">indicate if the method should use DirectShow's Intelligent Connect</param>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder, upFilter or downFilter are null</exception>
        /// <exception cref="System.ArgumentException">Thrown if pin names are not found in filters</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Thrown if pins can't connect</exception>
        /// <remarks>
        /// If useIntelligentConnect is true, this method can add missing filters between the two pins.<br/>
        /// If useIntelligentConnect is false, this method works only if the two media types are compatible.
        /// </remarks>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Reviewed. Suppression is OK here.")]
        public static void ConnectFilters(
            IFilterGraph2 graphBuilder,
            IBaseFilter upFilter,
            string sourcePinName,
            IBaseFilter downFilter,
            string destPinName,
            bool useIntelligentConnect)
        {
            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            if (upFilter == null)
            {
                throw new ArgumentNullException("upFilter");
            }

            if (downFilter == null)
            {
                throw new ArgumentNullException("downFilter");
            }

            IPin sourcePin = DsFindPin.ByName(upFilter, sourcePinName);
            if (sourcePin == null)
            {
                throw new ArgumentException("The source filter has no pin called : " + sourcePinName, sourcePinName);
            }

            IPin destPin = DsFindPin.ByName(downFilter, destPinName);
            if (destPin == null)
            {
                throw new ArgumentException("The downstream filter has no pin called : " + destPinName, destPinName);
            }

            try
            {
                ConnectFilters(graphBuilder, sourcePin, destPin, useIntelligentConnect);
            }
            finally
            {
                Marshal.ReleaseComObject(sourcePin);
                Marshal.ReleaseComObject(destPin);
            }
        }

        /// <summary>
        /// Connect pins from two filters
        /// </summary>
        /// <param name="graphBuilder">the IFilterGraph2 interface of the graph</param>
        /// <param name="sourcePin">the source (upstream / output) pin</param>
        /// <param name="destPin">the destination (downstream / input) pin</param>
        /// <param name="useIntelligentConnect">indicates if the method should use DirectShow's Intelligent Connect</param>
        /// <exception cref="System.ArgumentNullException">Thrown if graphBuilder, sourcePin or destPin are null</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Thrown if pins can't connect</exception>
        /// <remarks>
        /// If useIntelligentConnect is true, this method can add missing filters between the two pins.<br/>
        /// If useIntelligentConnect is false, this method works only if the two media types are compatible.
        /// </remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void ConnectFilters(
            IFilterGraph2 graphBuilder, IPin sourcePin, IPin destPin, bool useIntelligentConnect)
        {
            int hr;

            if (graphBuilder == null)
            {
                throw new ArgumentNullException("graphBuilder");
            }

            if (sourcePin == null)
            {
                throw new ArgumentNullException("sourcePin");
            }

            if (destPin == null)
            {
                throw new ArgumentNullException("destPin");
            }

            if (useIntelligentConnect)
            {
                hr = graphBuilder.Connect(sourcePin, destPin);
                DsError.ThrowExceptionForHR(hr);
            }
            else
            {
                hr = graphBuilder.ConnectDirect(sourcePin, destPin, null);
                DsError.ThrowExceptionForHR(hr);
            }
        }
    }
}