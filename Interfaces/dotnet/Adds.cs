namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    using DirectShowLib;
    using DirectShowLib.DES;

    using Microsoft.Win32;


    /// <summary>
    /// Filter category mode.
    /// </summary>
    public struct FilCatNode
    {
        /// <summary>
        /// Gets or sets friendly name.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets CLSID.
        /// </summary>
        public Guid CLSID { get; set; }

        /// <summary>
        /// Gets or sets FOURCC.
        /// </summary>
        public string FOURCC { get; set; }

        /// <summary>
        /// Gets or sets device path.
        /// </summary>
        public string DevicePath { get; set; }
    }

    //public class VFSysDevEnum
    //{
    //    private FilCatNode aCategory;
    //    private readonly List<FilCatNode> fCategories = new List<FilCatNode>();
    //    private readonly List<FilCatNode> fFilters = new List<FilCatNode>();
    //    private Guid fguid;
        
    //    public int CountCategories
    //    {
    //        get { return fCategories.Count; }
    //    }

    //    public int CountFilters
    //    {
    //        get { return fFilters.Count; }
    //    }

    //    public VFSysDevEnum()
    //    {
    //        GetCat(fCategories, FilterCategory.ActiveMovieCategories);
    //    }

    //    public VFSysDevEnum(Guid guid)
    //    {
    //        GetCat(fCategories, FilterCategory.ActiveMovieCategories);
    //        SelectGUIDCategory(guid);
    //    }

    //    public FilCatNode Categories(int item)
    //    {
    //        return fCategories[item];
    //    }

    //    public FilCatNode Filters(int item)
    //    {
    //        return fFilters[item];
    //    }
        
    //    public void Clear()
    //    {
    //        //if (fCategories.Count > 0)
    //        //{
    //        //    for (i = 0; i <= (fCategories.Count - 1); i ++ )
    //        //    {
    //        //        if ((fCategories[i] != null))
    //        //        {
    //        //            //@ Unsupported function or procedure: 'Dispose'
    //        //            Dispose(fCategories[i]);
    //        //        }
    //        //    }
    //        //}

    //        fCategories.Clear();
    //        //Units.vf_cap_ds_adds.FreeAndNil(ref fCategories);

    //        //if (fFilters.Count > 0)
    //        //{
    //        //    for (i = 0; i <= (fFilters.Count - 1); i ++ )
    //        //    {
    //        //        if ((fFilters[i] != null))
    //        //        {
    //        //            //@ Unsupported function or procedure: 'Dispose'
    //        //            Dispose(fFilters[i]);
    //        //        }
    //        //    }
    //        //}
    //        fFilters.Clear();
    //        //Units.vf_cap_ds_adds.FreeAndNil(ref fFilters);
    //    }

    //    ~VFSysDevEnum()
    //    {
    //        Clear();
    //    }

    //    public int FilterIndexOfFriendlyName(string friendlyName)
    //    {
    //        int result = fFilters.Count - 1;
    //        if (friendlyName != null)
    //        {
    //            while ((result >= 0) && (fFilters[result].FriendlyName.ToUpperInvariant().CompareTo(friendlyName.ToUpperInvariant()) != 0))
    //            {
    //                result -= 1;
    //            }
    //        }

    //        return result;
    //    }

    //    public IBaseFilter GetBaseFilter(int index)
    //    {
    //        object filter = null;
    //        IEnumMoniker enumCat;
    //        IMoniker[] moniker = new IMoniker[1];

    //        if ((index < CountFilters) && (index >= 0))
    //        {
    //            ICreateDevEnum sysDevEnum = (ICreateDevEnum)new CreateDevEnum();
    //            sysDevEnum.CreateClassEnumerator(fguid, out enumCat, 0);
    //            enumCat.Skip(index);
    //            enumCat.Next(1, moniker, IntPtr.Zero);
    //            Guid iid = typeof(IBaseFilter).GUID;
    //            moniker[0].BindToObject(null, null, ref iid, out filter);
    //            enumCat.Reset();

    //            Marshal.ReleaseComObject(enumCat);
    //            Marshal.ReleaseComObject(sysDevEnum);
    //            Marshal.ReleaseComObject(moniker[0]);
    //        }

    //        return filter as IBaseFilter;
    //    }

    //    public IBaseFilter GetBaseFilter(Guid guid)
    //    {
    //        if (CountFilters > 0)
    //        {
    //            for (int i = 0; i < CountFilters; i++)
    //            {
    //                if (guid == Filters(i).CLSID)
    //                {
    //                    return GetBaseFilter(i);
    //                }
    //            }
    //        }

    //        return null;
    //    }

    //    private void GetCat(List<FilCatNode> catlist, Guid catGUID)
    //    {
    //        IEnumMoniker enumCat;
    //        List<string> names = new List<string>();
    //        IMoniker[] moniker = new IMoniker[1];
    //        IPropertyBag propBag = null;
    //        object name;
    //        string nameS;

    //        try
    //        {
    //            //if (catlist.Count > 0)
    //            //{
    //            //    for (i = 0; i <= (catlist.Count - 1); i ++ )
    //            //    {
    //            //        if ((catlist[i] != null))
    //            //        {
    //            //            //@ Unsupported function or procedure: 'Dispose'
    //            //            Dispose(catlist[i]);
    //            //        }
    //            //    }
    //            //}
    //            catlist.Clear();

    //            ICreateDevEnum sysDevEnum = (ICreateDevEnum)new CreateDevEnum();
    //            int hr = sysDevEnum.CreateClassEnumerator(catGUID, out enumCat, 0);

    //            if (hr == 0)
    //            {
    //                while (enumCat.Next(1, moniker, IntPtr.Zero) == 0)
    //                {
    //                    try
    //                    {
    //                        //name = "";

    //                        Guid iid = typeof(IPropertyBag).GUID;
    //                        object ppvObj;
    //                        moniker[0].BindToStorage(null, null, ref iid, out ppvObj);
    //                        propBag = ppvObj as IPropertyBag;

    //                        if (propBag != null)
    //                        {
    //                            propBag.Read("FriendlyName", out name, null);

    //                            nameS = name.ToString().Trim();
    //                            nameS = nameS.Trim();
    //                            //                            new(aCategory);

    //                            if (catGUID == FilterCategory.VideoInputDevice)
    //                            {
    //                                nameS = nameS.Trim();
    //                            }

    //                            if (catGUID == FilterCategory.AudioRendererCategory)
    //                            {
    //                                //sName = name.ToString().Trim();
    //                                if (!nameS.Contains("DirectSound"))
    //                                {
    //                                    nameS = "<ignore>";
    //                                }
    //                                else
    //                                {
    //                                    nameS = nameS.Replace("DirectSound:", string.Empty).Trim();
    //                                }
    //                            }

    //                            if (string.IsNullOrEmpty(nameS.Trim()))
    //                            {
    //                                nameS = "[no name]";
    //                            }

    //                            aCategory.FriendlyName = nameS;
    //                            //name = "";

    //                            if (propBag.Read("CLSID", out name, null) == 0)
    //                            {
    //                                try
    //                                {
    //                                    aCategory.CLSID = new Guid(name.ToString());
    //                                }
    //                                catch
    //                                {

    //                                    aCategory.CLSID = Guid.Empty;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                aCategory.CLSID = Guid.Empty;
    //                            }

    //                            //name = "";

    //                            if (propBag.Read("FccHandler", out name, null) == 0)
    //                            {
    //                                aCategory.FOURCC = name.ToString().Trim();
    //                            }
    //                            else
    //                            {
    //                                aCategory.FOURCC = string.Empty;
    //                            }

    //                            //name = "";

    //                            propBag.Read("Description", out name, null);
    //                            if (name != null)
    //                            {
    //                                aCategory.Description = name.ToString().Trim();
    //                            }
    //                            else
    //                            {
    //                                aCategory.Description = string.Empty;
    //                            }

    //                            string s;
    //                            if (aCategory.FriendlyName.IndexOf("Microsoft DV Camera and VCR", StringComparison.InvariantCultureIgnoreCase) != -1)
    //                            {
    //                                s = aCategory.FriendlyName.Replace("Microsoft DV Camera and VCR", "(DV)");
    //                                aCategory.FriendlyName = aCategory.Description + ' ' + s;
    //                            }

    //                            if (aCategory.FriendlyName.IndexOf("Microsoft AV/C Tape Subunit Device", StringComparison.InvariantCultureIgnoreCase) != -1)
    //                            {
    //                                s = aCategory.FriendlyName.Replace("Microsoft AV/C Tape Subunit Device",
    //                                                                   "(DV/MPEG/Tape Device)");
    //                                aCategory.FriendlyName = aCategory.Description + ' ' + s;
    //                            }

    //                            if (names.IndexOf(aCategory.FriendlyName) != -1)
    //                            {
    //                                int k;
    //                                for (k = 2; k <= 10; k++)
    //                                {
    //                                    if (names.IndexOf(aCategory.FriendlyName + " (" + k + ')') == -1)
    //                                    {
    //                                        aCategory.FriendlyName = aCategory.FriendlyName + " (" + k + ')';
    //                                        break;
    //                                    }
    //                                }
    //                            }

    //                            names.Add(aCategory.FriendlyName);
    //                            //name = "";
    //                            if (propBag.Read("DevicePath", out name, null) == 0)
    //                            {
    //                                if (name != null)
    //                                {
    //                                    aCategory.DevicePath = name.ToString().Trim();
    //                                }
    //                                else
    //                                {
    //                                    aCategory.DevicePath = string.Empty;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                aCategory.DevicePath = string.Empty;
    //                            }

    //                            catlist.Add(aCategory);

    //                            Marshal.ReleaseComObject(propBag);
    //                        }

    //                        Marshal.ReleaseComObject(moniker);
    //                    }
    //                    catch
    //                    {
    //                    }
    //                }
    //            }

    //            Marshal.ReleaseComObject(enumCat);
    //            Marshal.ReleaseComObject(sysDevEnum);
    //            Marshal.ReleaseComObject(propBag);
    //        }
    //        catch
    //        {
    //        }
    //    }
        
    //    public IMoniker GetMoniker(int index)
    //    {
    //        // ReSharper disable RedundantAssignment
    //        IMoniker[] result = new IMoniker[1];
    //        // ReSharper restore RedundantAssignment
    //        ICreateDevEnum sysDevEnum;
    //        IEnumMoniker enumCat;
    //        //result = null;

    //        if ((index < CountFilters) && (index >= 0))
    //        {
    //            sysDevEnum = (ICreateDevEnum)new CreateDevEnum();
    //            sysDevEnum.CreateClassEnumerator(fguid, out enumCat, 0);
    //            enumCat.Skip(index);
    //            enumCat.Next(1, result, IntPtr.Zero);
    //            enumCat.Reset();

    //            Marshal.ReleaseComObject(enumCat);
    //            Marshal.ReleaseComObject(sysDevEnum);
    //        }

    //        // ReSharper disable PossibleNullReferenceException
    //        return result[0];
    //        // ReSharper restore PossibleNullReferenceException
    //    }

    //    public void SelectGUIDCategory(Guid guid)
    //    {
    //        fguid = guid;
    //        GetCat(fFilters, fguid);
    //    }

    //    public void SelectIndexCategory(int index)
    //    {
    //        SelectGUIDCategory(Categories(index).CLSID);
    //    }
    //}

    /// <summary>
    /// Pin list class.
    /// </summary>
    public class VFPinList
    {
        /// <summary>
        /// Item list.
        /// </summary>
        private readonly List<IPin> FItems;

        /// <summary>
        /// Filter.
        /// </summary>
        private IBaseFilter FFilter;

        /// <summary>
        /// Used list.
        /// </summary>
        private List<bool> FUsed;

        /// <summary>
        /// Gets used list.
        /// </summary>
        public List<bool> Used
        {
            get
            {
                return this.FUsed;
            }
        }

        /// <summary>
        /// Gets connected state.
        /// </summary>
        /// <param name="index">
        /// Pin index.
        /// </param>
        /// <returns>
        /// Returns true if pin connected.
        /// </returns>
        public bool Connected(int index)
        {
            IPin pin;

            this[index].ConnectedTo(out pin);
            return pin != null;
        }

        /// <summary>
        /// Pin list.
        /// </summary>
        /// <param name="index">
        /// Pin index.
        /// </param>
        public IPin this[int index]
        {
            get
            {
                return this.FItems[index];
            }

            set
            {
                this.FItems[index] = value;
            }
        }

        /// <summary>
        /// Gets pin info.
        /// </summary>
        /// <param name="index">
        /// Pin index.
        /// </param>
        /// <returns>
        /// Returns pin info.
        /// </returns>
        public PinInfo PinInfo(int index)
        {
            if (this.FItems[index] != null)
            {
                PinInfo pi;
                this[index].QueryPinInfo(out pi);
                return pi;
            }

            return new PinInfo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VFPinList"/> class.
        /// </summary>
        /// <param name="baseFilter">
        /// Base filter.
        /// </param>
        public VFPinList(IBaseFilter baseFilter)
        {
            this.FFilter = baseFilter;

            this.FUsed = new List<bool>();
            this.FItems = new List<IPin>();

            this.Update();
        }

        /// <summary>
        /// Clears list.
        /// </summary>
        public void Clear()
        {
            int k = 0;
            foreach (IPin item in this.FItems)
            {
                if (!this.Used[k])
                {
                    if (item != null)
                    {
                        Marshal.ReleaseComObject(item);
                    }
                }

                k++;
            }

            this.FItems.Clear();
            this.Used.Clear();
        }

        /// <summary>
        /// Gets pins count.
        /// </summary>
        /// <returns>
        /// Returns pins count.
        /// </returns>
        public int Count()
        {
            return this.FItems.Count;
        }

        /// <summary>
        /// Removes pin by index.
        /// </summary>
        /// <param name="index">
        /// Pin index.
        /// </param>
        public void RemoveAt(int index)
        {
            this.FItems.RemoveAt(index);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="VFPinList"/> class. 
        /// </summary>
        ~VFPinList()
        {
            this.Clear();

            if (this.FFilter != null)
            {
                Marshal.ReleaseComObject(this.FFilter);
                this.FFilter = null;
            }
        }

        /// <summary>
        /// Adds pins to the list.
        /// </summary>
        /// <param name="item">
        /// Item.
        /// </param>
        public void Add(IPin item)
        {
            this.FItems.Add(item);
        }

        /// <summary>
        /// Assign base filter.
        /// </summary>
        /// <param name="baseFilter">
        /// Base filter.
        /// </param>
        public void Assign(IBaseFilter baseFilter)
        {
            this.FItems.Clear();
            this.Used.Clear();
            this.FFilter = baseFilter;
            if (this.FFilter != null)
            {
                this.Update();
            }
        }

        /// <summary>
        /// Gets first pin.
        /// </summary>
        /// <returns>
        /// Returns first pin.
        /// </returns>
        public IPin First()
        {
            return this.FItems[0];
        }

        /// <summary>
        /// Gets pin index.
        /// </summary>
        /// <param name="item">
        /// Item.
        /// </param>
        /// <returns>
        /// Returns pin index.
        /// </returns>
        public int IndexOf(IPin item)
        {
            return this.FItems.IndexOf(item);
        }

        /// <summary>
        /// Inserts item.
        /// </summary>
        /// <param name="index">
        /// Item index.
        /// </param>
        /// <param name="item">
        /// Item.
        /// </param>
        public void Insert(int index, IPin item)
        {
            this.FItems.Insert(index, item);
        }

        /// <summary>
        /// Gets last item.
        /// </summary>
        /// <returns>
        /// Returns last item.
        /// </returns>
        public IPin Last()
        {
            return this.FItems[this.FItems.Count - 1];
        }

        /// <summary>
        /// Removes item.
        /// </summary>
        /// <param name="item">
        /// Item.
        /// </param>
        /// <returns>
        /// Returns true if the operation was successful.
        /// </returns>
        public bool Remove(IPin item)
        {
            return this.FItems.Remove(item);
        }

        /// <summary>
        /// Updates pin list.
        /// </summary>
        public void Update()
        {
            IEnumPins enumPins;
            IPin[] pin = new IPin[1];
            this.FItems.Clear();
            this.Used.Clear();
            if (this.FFilter != null)
            {
                this.FFilter.EnumPins(out enumPins);
            }
            else
            {
                return;
            }

            if (enumPins == null)
            {
                return;
            }

            while (enumPins.Next(1, pin, IntPtr.Zero) == 0)
            {
                this.Add(pin[0]);
                this.Used.Add(false);
            }

            Marshal.ReleaseComObject(enumPins);
        }
    }

    /// <summary>
    /// Filter list class.
    /// </summary>
    public class VFFilterList
    {
        /// <summary>
        /// Item list.
        /// </summary>
        private readonly List<IBaseFilter> FItems = new List<IBaseFilter>();

        /// <summary>
        /// Gets count.
        /// </summary>
        public int Count
        {
            get { return this.FItems.Count; }
        }

        /// <summary>
        /// Gets filter info.
        /// </summary>
        /// <param name="index">
        /// Filter index.
        /// </param>
        /// <returns>
        /// Returns filter info.
        /// </returns>
        public FilterInfo FilterInfo(int index)
        {
            if (this.FItems[index] != null)
            {
                FilterInfo fi;
                this[index].QueryFilterInfo(out fi);
                return fi;
            }

            return new FilterInfo();
        }

        /// <summary>
        /// Gets filter.
        /// </summary>
        /// <param name="index">
        /// Filter index.
        /// </param>
        public IBaseFilter this[int index]
        {
            get
            {
                return this.FItems[index];
            }

            set
            {
                this.FItems[index] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VFFilterList"/> class.
        /// </summary>
        /// <param name="filterGraph">
        /// Filter graph.
        /// </param>
        public VFFilterList(IFilterGraph2 filterGraph)
        {
            this.Update(filterGraph);
        }

        /// <summary>
        /// Clears list.
        /// </summary>
        private void Clear()
        {
            foreach (var item in this.FItems)
            {
                if (item != null)
                {
                    Marshal.ReleaseComObject(item);
                }
            }

            this.FItems.Clear();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="VFFilterList"/> class. 
        /// </summary>
        ~VFFilterList()
        {
            this.Clear();
        }

        /// <summary>
        /// Adds item.
        /// </summary>
        /// <param name="item">
        /// Item.
        /// </param>
        public void Add(IBaseFilter item)
        {
            this.FItems.Add(item);
        }

        /// <summary>
        /// Assign to a filter graph.
        /// </summary>
        /// <param name="filterGraph">
        /// Filter graph.
        /// </param>
        public void Assign(IFilterGraph2 filterGraph)
        {
            this.Clear();
            this.Update(filterGraph);
        }

        /// <summary>
        /// Gets first filter.
        /// </summary>
        /// <returns>
        /// Returns first filter.
        /// </returns>
        public IBaseFilter First()
        {
            return this.FItems[0];
        }

        /// <summary>
        /// Gets item index.
        /// </summary>
        /// <param name="item">
        /// Item.
        /// </param>
        /// <returns>
        /// Returns item index.
        /// </returns>
        public int IndexOf(IBaseFilter item)
        {
            return this.FItems.IndexOf(item);
        }

        /// <summary>
        /// Inserts filter.
        /// </summary>
        /// <param name="index">
        /// Item index.
        /// </param>
        /// <param name="item">
        /// Filter item.
        /// </param>
        public void Insert(int index, IBaseFilter item)
        {
            this.FItems.Insert(index, item);
        }

        /// <summary>
        /// Gets last filter.
        /// </summary>
        /// <returns>
        /// Returns last filter.
        /// </returns>
        public IBaseFilter Last()
        {
            return this.FItems[this.FItems.Count - 1];
        }

        /// <summary>
        /// Removes filter from the list.
        /// </summary>
        /// <param name="item">
        /// Item.
        /// </param>
        /// <returns>
        /// Returns true if the operation was successful.
        /// </returns>
        public bool Remove(IBaseFilter item)
        {
            return this.FItems.Remove(item);
        }

        /// <summary>
        /// Updates filter list.
        /// </summary>
        /// <param name="filterGraph">
        /// Filter graph.
        /// </param>
        public void Update(IFilterGraph2 filterGraph)
        {
            IBaseFilter[] filter = new IBaseFilter[1];
            if (filterGraph != null)
            {
                IEnumFilters enumFilters;
                filterGraph.EnumFilters(out enumFilters);

                if (enumFilters == null)
                {
                    return;
                }

                while (enumFilters.Next(1, filter, IntPtr.Zero) == 0)
                {
                    this.Add(filter[0]);
                }

                Marshal.ReleaseComObject(enumFilters);
            }
        }
    }

    /// <summary>
    /// Graph callback class.
    /// </summary>
    public class VFGraphCallbacks : IAMGraphBuilderCallback
    {
        /// <summary>
        /// Filters blacklist.
        /// </summary>
        private readonly List<string> FBlackList = new List<string>();

        /// <summary>
        /// Gets black list.
        /// </summary>
        public List<string> BlackList
        {
            get
            {
                return this.FBlackList;
            }
        }

        /// <summary>
        /// Assign to graph.
        /// </summary>
        /// <param name="filterGraph">
        /// Filter graph.
        /// </param>
        public void AssignToGraph(IFilterGraph2 filterGraph)
        {
            try
            {
                IObjectWithSite obj = filterGraph as IObjectWithSite;
                if (obj != null)
                {
                    obj.SetSite(this);
                    Marshal.ReleaseComObject(obj);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Created filter callback.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <returns>
        /// Returns HRESULT.
        /// </returns>
        public int CreatedFilter(IBaseFilter filter)
        {
            return 0;
        }

        /// <summary>
        /// Selected filter callback.
        /// </summary>
        /// <param name="moniker">
        /// Moniker.
        /// </param>
        /// <returns>
        /// Returns HRESULT.
        /// </returns>
        public int SelectedFilter(IMoniker moniker)
        {
            IPropertyBag propBag;
            object name = null;
            try
            {
                if (moniker == null)
                {
                    return 0;
                }

                Guid iid = typeof(IPropertyBag).GUID;

                object ppvObj;
                moniker.BindToStorage(null, null, ref iid, out ppvObj);
                propBag = ppvObj as IPropertyBag;

                if (propBag != null)
                {
                    propBag.Read(DSConsts.PP_FriendlyName, out name, null);
                    // Marshal.ReleaseComObject(propBag);
                }

                // check
                if (name != null && this.FBlackList.IndexOf(name.ToString()) != -1)
                {
                    return ErrorCodes.E_FAIL;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Crossbar output pin.
    /// </summary>
    public class VFCrossBarOutputPin : object
    {
        /// <summary>
        /// Input pins.
        /// </summary>
        private List<VFCrossBarInputPin> FInputPins;

        /// <summary>
        /// Gets input pins.
        /// </summary>
        public List<VFCrossBarInputPin> InputPins
        {
            get
            {
                return this.FInputPins;
            }
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets pin index.
        /// </summary>
        public int PinIndex { get; set; }

        /// <summary>
        /// Gets or sets related pin index.
        /// </summary>
        public int RelatedPinIndex { get; set; }

        /// <summary>
        /// Gets or sets routed to pin index.
        /// </summary>
        public int RoutedTo { get; set; }

        /// <summary>
        /// Finalizes an instance of the <see cref="VFCrossBarOutputPin"/> class. 
        /// </summary>
        ~VFCrossBarOutputPin()
        {
            try
            {
                foreach (var inputPin in this.FInputPins)
                {
                    if (inputPin != null)
                    {
                        try
                        {
                            Marshal.ReleaseComObject(inputPin);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }

            try
            {
                this.FInputPins.Clear();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VFCrossBarOutputPin"/> class.
        /// </summary>
        public VFCrossBarOutputPin()
        {
            this.FInputPins = new List<VFCrossBarInputPin>();
        }
    }

    /// <summary>
    /// Crossbar input pin.
    /// </summary>
    public class VFCrossBarInputPin : object
    {
        /// <summary>
        /// Gets or sets a value indicating whether pins can be routed.
        /// </summary>
        public bool CanRoute { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets pin index.
        /// </summary>
        public int PinIndex { get; set; }

        /// <summary>
        /// Gets or sets real pin index.
        /// </summary>
        public int RealPinIndex { get; set; }

        /// <summary>
        /// Gets or sets related pin index.
        /// </summary>
        public int RelatedPinIndex { get; set; }
    }

    /// <summary>
    /// Crossbar connections.
    /// </summary>
    public class VFCrossBarConnections : object
    {
        /// <summary>
        /// Gets or sets input pin ID.
        /// </summary>
        public int InPin { get; set; }

        /// <summary>
        /// Gets or sets output pin ID.
        /// </summary>
        public int OutPin { get; set; }
    }

    /// <summary>
    /// Video capture format.
    /// </summary>
    public class VFVideoCaptureFormat : object
    {
        /// <summary>
        /// Gets or sets DV info.
        /// </summary>
        public DVInfo DV { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether fixed size samples enabled.
        /// </summary>
        public bool FixedSizeSamples { get; set; }

        /// <summary>
        /// Gets or sets format type.
        /// </summary>
        public Guid FormatType { get; set; }

        /// <summary>
        /// Gets or sets major type.
        /// </summary>
        public Guid MajorType { get; set; }

        /// <summary>
        /// Gets or sets MPEG-1 video info header.
        /// </summary>
        //public MPEG1VideoInfo MPEG1 { get; set; }

        /// <summary>
        /// Gets or sets MPEG-2 video info header.
        /// </summary>
        public MPEG2VideoInfo MPEG2 { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets sample size.
        /// </summary>
        public int SampleSize { get; set; }

        /// <summary>
        /// Gets or sets sub type.
        /// </summary>
        public Guid SubType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether temporal compression enabled.
        /// </summary>
        public bool TemporalCompression { get; set; }

        /// <summary>
        /// Gets or sets video info header.
        /// </summary>
        public VideoInfoHeader VIH { get; set; }

        /// <summary>
        /// Gets or sets video info header 2.
        /// </summary>
        public VideoInfoHeader2 VIH2 { get; set; }

        /// <summary>
        /// Gets or sets width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets height.
        /// </summary>
        public int Height { get; set; }
    }

    /// <summary>
    /// Pin class.
    /// </summary>
    public class VFPin : object
    {
        /// <summary>
        /// Format list.
        /// </summary>
        private readonly List<string> FFormats;

        /// <summary>
        /// Gets format list.
        /// </summary>
        public List<string> Formats
        {
            get
            {
                return this.FFormats;
            }
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VFPin"/> class.
        /// </summary>
        public VFPin()
        {
            this.FFormats = new List<string>();
        }
    }

    //    // IMediaStore
    //    // (*
    //    // PPROPERTYKEY = ^TPROPERTYKEY;
    //    // TPROPERTYKEY = record
    //    // fmtid: TGUID;
    //    // pid: DWORD;
    //    // end;
    //    // 
    //    // IInitializeWithFile = interface(IUnknown)
    //    // ['{b7d14566-0509-4cce-a71f-0a554233bd9b}']
    //    // function Initialize(pszFilePath: PWideChar; grfMode: DWORD):
    //    // HRESULT; stdcall;
    //    // end;
    //    // 
    //    // IInitializeWithStream = interface(IUnknown)
    //    // ['{b824b49d-22ac-4161-ac8a-9916e8fa3f7f}']
    //    // function Initialize(var pIStream: IStream; grfMode: DWORD):
    //    // HRESULT; stdcall;
    //    // end;
    //    // 
    //    // IPropertyStore = interface(IUnknown)
    //    // ['{886d8eeb-8cf2-4446-8d02-cdba1dbdcf99}']
    //    // function GetCount(out cProps: DWORD): HRESULT; stdcall;
    //    // function GetAt(iProp: DWORD; out PropKey: TPropertyKey): HRESULT;
    //    // stdcall;
    //    // function GetValue(pPropKey: PPropertyKey; out PropVar:
    //    // TPropVariant): HRESULT; stdcall;
    //    // function SetValue(pPropKey: PPropertyKey; pPropVar: PPropVariant):
    //    // HRESULT; stdcall;
    //    // function Commit: HRESULT; stdcall;
    //    // end;
    //    // 
    //    // IPropertyStoreCapabilities = interface(IUnknown)
    //    // ['{c8e2d566-186e-4d49-bf41-6909ead56acc}']
    //    // function IsPropertyWritable(pPropKey: PPropertyKey): HRESULT; stdcall;
    //    // end;
    //    // *)
    //    // Main

    //    public enum VF_StreamType
    //    {
    //        VF_ST_Audio,
    //        VF_ST_Video,
    //        VF_ST_Any,
    //    } // end VF_StreamType

    //}

    //    public class vf_cap_ds_adds
    //    {

    //        public static bool ImageCompare(Bitmap Img1, Bitmap Img2, byte Difference, ref int FinalDiff, bool ReadDiff, ref Bitmap imgDiff, ref int ImgDiffTop, ref int ImgDiffLeft, ref double DiffPerc)
    //        {
    //            bool result;
    //            int w;
    //            int h;
    //            int i;
    //            int j;
    //            TRGBTriple[] RowOriginal;
    //            TRGBTriple[] RowProcessed;
    //            TRGBTriple[] RowDiff;
    //            TRGBQuad[] RowOriginal32;
    //            TRGBQuad[] RowProcessed32;
    //            TRGBQuad[] RowDiff32;
    //            long Fullsize;
    //            long Errors;
    //            int MLeft;
    //            int MTop;
    //            int MRight;
    //            int MBottom;
    //            // imgDiff: TBitmap;
    //            ArrayList log;
    //            Image jpg;
    //            string logfile;
    //            try {
    //                result = true;
    //                if (Img1.Width != Img2.Width)
    //                {
    //                    return result;
    //                }
    //                if (Img1.Height != Img2.Height)
    //                {
    //                    return result;
    //                }
    //                //@ Undeclared identifier(3): 'pf24bit'
    //                //@ Undeclared identifier(3): 'pf32bit'
    //                //@ Undeclared identifier(3): 'pf16bit'
    //                //@ Undeclared identifier(3): 'pf8bit'
    //                if ((Img1.PixelFormat != pf24bit) && (Img1.PixelFormat != pf32bit) && (Img1.PixelFormat != pf16bit) && (Img1.PixelFormat != pf8bit))
    //                {
    //                    return result;
    //                }
    //                DiffPerc = 0;
    //                Fullsize = Img1.Width * Img1.Height;
    //                Errors = 0;
    //                if (ReadDiff)
    //                {
    //                    MLeft = Img1.Width;
    //                    MRight = 0;
    //                    MTop = Img1.Height;
    //                    MBottom = 0;
    //                }
    //                //@ Undeclared identifier(3): 'pf24bit'
    //                if (Img1.PixelFormat == pf24bit)
    //                {
    //                    // 24bit picture
    //                    for (j = Img1.Height - 1; j >= 0; j-- )
    //                    {
    //                        //@ Unsupported property or method: 'ScanLine'
    //                        RowOriginal = ((TRGBArray[])(Img1.ScanLine[j]));
    //                        //@ Unsupported property or method: 'ScanLine'
    //                        RowProcessed = ((TRGBArray[])(Img2.ScanLine[j]));
    //                        if (!ReadDiff && ((100 - ((Errors / Fullsize) * 100)) < Difference))
    //                        {
    //                            result = false;
    //                            return result;
    //                        }
    //                        for (i = Img1.Width - 1; i >= 0; i-- )
    //                        {
    //                            //@ Unsupported property or method: 'rgbtRed'
    //                            //@ Unsupported property or method: 'rgbtRed'
    //                            //@ Unsupported property or method: 'rgbtGreen'
    //                            //@ Unsupported property or method: 'rgbtGreen'
    //                            //@ Unsupported property or method: 'rgbtBlue'
    //                            //@ Unsupported property or method: 'rgbtBlue'
    //                            if ((RowProcessed[i].rgbtRed != RowOriginal[i].rgbtRed) || (RowProcessed[i].rgbtGreen != RowOriginal[i].rgbtGreen) || (RowProcessed[i].rgbtBlue != RowOriginal[i].rgbtBlue))
    //                            {
    //                                Errors = Errors + 1;
    //                                if (ReadDiff)
    //                                {
    //                                    if (i < MLeft)
    //                                    {
    //                                        MLeft = i;
    //                                    }
    //                                    if (i > MRight)
    //                                    {
    //                                        MRight = i;
    //                                    }
    //                                    if (j < MTop)
    //                                    {
    //                                        MTop = j;
    //                                    }
    //                                    if (j > MBottom)
    //                                    {
    //                                        MBottom = j;
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    //@ Undeclared identifier(3): 'pf32bit'
    //                    if (Img1.PixelFormat == pf32bit)
    //                    {
    //                        // 32bit picture
    //                        for (j = Img1.Height - 1; j >= 0; j-- )
    //                        {
    //                            //@ Unsupported property or method: 'ScanLine'
    //                            RowOriginal32 = ((TRGBAArray[])(Img1.ScanLine[j]));
    //                            //@ Unsupported property or method: 'ScanLine'
    //                            RowProcessed32 = ((TRGBAArray[])(Img2.ScanLine[j]));
    //                            if ((100 - ((Errors / Fullsize) * 100)) < Difference)
    //                            {
    //                                result = false;
    //                                return result;
    //                            }
    //                            for (i = Img1.Width - 1; i >= 0; i-- )
    //                            {
    //                                //@ Unsupported property or method: 'rgbRed'
    //                                //@ Unsupported property or method: 'rgbRed'
    //                                //@ Unsupported property or method: 'rgbGreen'
    //                                //@ Unsupported property or method: 'rgbGreen'
    //                                //@ Unsupported property or method: 'rgbBlue'
    //                                //@ Unsupported property or method: 'rgbBlue'
    //                                if ((RowProcessed32[i].rgbRed != RowOriginal32[i].rgbRed) || (RowProcessed32[i].rgbGreen != RowOriginal32[i].rgbGreen) || (RowProcessed32[i].rgbBlue != RowOriginal32[i].rgbBlue))
    //                                {
    //                                    Errors = Errors + 1;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            } finally {
    //                FinalDiff = Math.Round(100 - ((Errors / Fullsize) * 100));
    //                if (ReadDiff && (FinalDiff < Difference))
    //                {
    //                    result = false;
    //                }
    //                if (ReadDiff && !result)
    //                {
    //                    try {
    //                        try {
    //                            imgDiff.PixelFormat = Img2.PixelFormat;
    //                            imgDiff.Width = Math.Abs(MRight - MLeft);
    //                            imgDiff.Height = Math.Abs(MTop - MBottom);
    //                            ImgDiffTop = MTop;
    //                            ImgDiffLeft = MLeft;
    //                            DiffPerc = ((imgDiff.Width * imgDiff.Height) / Fullsize) * 100;
    //                            //@ Unsupported property or method: 'Canvas'
    //                            //@ Unsupported property or method: 'Canvas'
    //                            //@ Unsupported property or method: 'CopyRect'
    //                            imgDiff.Canvas.CopyRect(new Rectangle(0, 0, Math.Abs(MRight - MLeft), Math.Abs(MBottom - MTop)), Img2.Canvas, new Rectangle(MLeft, MTop, MRight, MBottom));
    //                        }
    //                        catch {
    //                        }
    //                    }
    //                    catch {
    //                    }
    //                }
    //            }
    //            return result;
    //        }
    
    //        public static bool DumpTBitmapToBuffer(Bitmap Bitmap, object pBuf, int size, ref bool Depth32b)
    //        {
    //            bool result;
    //            Bitmap bmp;
    //            int i;
    //            object dest;
    //            int stride;
    //            result = false;
    //            if ((pBuf == null) || (Bitmap == null) || (Bitmap.Width == 0) || (Bitmap.Height == 0))
    //            {
    //                return result;
    //            }
    //            //@ Undeclared identifier(3): 'pf24bit'
    //            if ((Bitmap.PixelFormat == pf24bit) && (Bitmap.Width * Bitmap.Height * 3 != size))
    //            {
    //                return result;
    //            }
    //            //@ Undeclared identifier(3): 'pf32bit'
    //            if ((Bitmap.PixelFormat == pf32bit) && (Bitmap.Width * Bitmap.Height * 4 != size))
    //            {
    //                return result;
    //            }
    //            // convert if needed
    //            //@ Undeclared identifier(3): 'pf24bit'
    //            //@ Undeclared identifier(3): 'pf32bit'
    //            if ((Bitmap.PixelFormat == pf24bit) || (Bitmap.PixelFormat == pf32bit))
    //            {
    //                bmp = Bitmap;
    //            }
    //            else
    //            {
    //                bmp = new Bitmap();
    //                bmp.Width = Bitmap.Width;
    //                bmp.Height = Bitmap.Height;
    //                //@ Undeclared identifier(3): 'pf24bit'
    //                bmp.PixelFormat = pf24bit;
    //                //@ Unsupported property or method: 'Canvas'
    //                //@ Unsupported property or method: 'Draw'
    //                bmp.Canvas.Draw(0, 0, Bitmap);
    //            }
    //            // dump
    //            //@ Undeclared identifier(3): 'pf24bit'
    //            if (bmp.PixelFormat == pf24bit)
    //            {
    //                stride = bmp.Width * 3;
    //            }
    //            else
    //            {
    //                stride = bmp.Width * 4;
    //            }
    //            for (i = 0; i < bmp.Height; i ++ )
    //            {
    //                dest = ((object)(((uint)(pBuf)) + stride));
    //                //@ Unsupported property or method: 'ScanLine'
    //                //@ Undeclared identifier(3): 'CopyMemory'
    //                CopyMemory(dest, bmp.ScanLine[i], stride);
    //            }
    //            if (bmp != Bitmap)
    //            {
    //                //@ Unsupported property or method: 'Free'
    //                bmp.Free;
    //            }
    //            result = true;
    //            return result;
    //        }
}