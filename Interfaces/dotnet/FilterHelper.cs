using System;
using System.Collections.Generic;

namespace VisioForge.DirectShowAPI
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using DirectShowLib;

    public static class FilterHelper
    {
        public static void Clear(this DsDevice[] devices)
        {
            foreach (var device in devices)
            {
                device.Dispose();
            }
        }

        /// <summary>
        /// Disconnects filter.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        public static void DisconnectFilter(this IBaseFilter filter)
        {
            IPin pin1;

            GetPin(filter, PinDirection.Input, 1, out pin1);
            if (pin1 != null)
            {
                IPin pin2;
                pin1.ConnectedTo(out pin2);
                if (pin2 != null)
                {
                    pin1.Disconnect();
                    pin2.Disconnect();

                    Marshal.ReleaseComObject(pin2);
                    // ReSharper disable RedundantAssignment
                    pin2 = null;
                    // ReSharper restore RedundantAssignment
                }

                Marshal.ReleaseComObject(pin1);
                // ReSharper disable RedundantAssignment
                pin1 = null;
                // ReSharper restore RedundantAssignment
            }
        }


        /// <summary>
        /// Checks that filter connected
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="noInputPinsResult">
        /// Result if no input pins. 
        /// </param>
        /// <returns>
        /// Returns true if the filter connected.
        /// </returns>
        public static bool IsFilterConnected(this IBaseFilter filter, bool noInputPinsResult = false)
        {
            bool result;

            try
            {
                result = false;

                IPin pin1;
                GetPin(filter, PinDirection.Input, 1, out pin1);
                if (pin1 == null)
                {
                    return noInputPinsResult;
                }

                IPin pin2;
                pin1.ConnectedTo(out pin2);
                if (pin2 != null)
                {
                    result = true;
                    Marshal.ReleaseComObject(pin2);
                    // ReSharper disable RedundantAssignment
                    pin2 = null;
                    // ReSharper restore RedundantAssignment
                }

                Marshal.ReleaseComObject(pin1);
                // ReSharper disable RedundantAssignment
                pin1 = null;
                // ReSharper restore RedundantAssignment
            }
            catch
            {
                return false;
            }

            return result;
        }

        /// <summary>
        /// Shows filter info while debugging.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        public static void DebugFilterInfo(this IBaseFilter filter)
        {
            try
            {
                if (filter == null)
                {
                    return;
                }

                FilterInfo filterInfo;
                filter.QueryFilterInfo(out filterInfo);

                Guid clsid;
                filter.GetClassID(out clsid);

                Debug.Print("Filter info:");
                Debug.Print("Name:" + filterInfo.achName);
                Debug.Print("CLSID:" + clsid.ToString());

                ReleaseFilterInfo(ref filterInfo);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Delegate DllRegisterServerInvoker.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private delegate int DllRegisterServerInvoker();

        /// <summary>
        /// Releases the filter information.
        /// </summary>
        /// <param name="fi">The fi.</param>
        private static void ReleaseFilterInfo(ref FilterInfo fi)
        {
            if (fi.pGraph == null)
            {
                return;
            }

            Marshal.ReleaseComObject(fi.pGraph);
            fi.pGraph = null;
        }


        /// <summary>
        /// gets filter name
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetName(this IBaseFilter filter)
        {
            try
            {
                if (filter == null)
                {
                    return string.Empty;
                }

                FilterInfo filterInfo;
                filter.QueryFilterInfo(out filterInfo);

                string name = filterInfo.achName;

                ReleaseFilterInfo(ref filterInfo);

                return name;
            }
            catch
            {
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets connected filter.
        /// </summary>
        /// <param name="filter">
        /// Source filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="destFilter">
        /// Destination filter.
        /// </param>
        public static void FilterConnectedTo(this IBaseFilter filter, PinDirection pinDirection, out IBaseFilter destFilter)
        {
            destFilter = null;

            try
            {
                IPin pin1;

                GetPin(filter, pinDirection, 1, out pin1);
                if (pin1 != null)
                {
                    IPin pin2;
                    pin1.ConnectedTo(out pin2);
                    if (pin2 != null)
                    {
                        PinInfo pi;
                        pin2.QueryPinInfo(out pi);
                        destFilter = pi.filter;

                        Marshal.ReleaseComObject(pin2);
                        // ReSharper disable RedundantAssignment
                        pin2 = null;
                        // ReSharper restore RedundantAssignment
                    }

                    Marshal.ReleaseComObject(pin1);
                    // ReSharper disable RedundantAssignment
                    pin1 = null;
                    // ReSharper restore RedundantAssignment
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets connected filter.
        /// </summary>
        /// <param name="filter">
        /// Source filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <returns>
        /// Returns destination filter.
        /// </returns>
        public static IBaseFilter FilterConnectedTo(this IBaseFilter filter, PinDirection pinDirection)
        {
            IBaseFilter destFilter = null;

            try
            {
                IPin pin1;

                GetPin(filter, pinDirection, 1, out pin1);
                if (pin1 != null)
                {
                    IPin pin2;
                    pin1.ConnectedTo(out pin2);
                    if (pin2 != null)
                    {
                        PinInfo pi;
                        pin2.QueryPinInfo(out pi);
                        destFilter = pi.filter;

                        Marshal.ReleaseComObject(pin2);
                        // ReSharper disable RedundantAssignment
                        pin2 = null;
                        // ReSharper restore RedundantAssignment
                    }

                    Marshal.ReleaseComObject(pin1);
                    // ReSharper disable RedundantAssignment
                    pin1 = null;
                    // ReSharper restore RedundantAssignment

                    return destFilter;
                }
            }
            catch
            {
            }

            return destFilter;
        }

        /// <summary>
        /// Gets filter output formats, first format for each pin. 
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="formats">
        /// Formats (string list).
        /// </param>
        public static void GetFilterFormats(
            this IBaseFilter filter,
            PinDirection pinDirection,
            out List<AMMediaType> formats)
        {
            formats = new List<AMMediaType>();

            IEnumPins enumPins;
            IPin[] pins = new IPin[1];

            if (filter == null)
            {
                return;
            }

            // Get the pin enumerator
            int hr = filter.EnumPins(out enumPins);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
                {
                    PinDirection pinDir;
                    pins[0].QueryDirection(out pinDir);

                    if (pinDir != pinDirection)
                    {
                        Marshal.ReleaseComObject(pins[0]);
                        continue;
                    }

                    VFEnumMediaType mts = new VFEnumMediaType(pins[0]);
                    if (mts.Count > 0)
                    {
                        AMMediaType mt = mts.Items(0);
                        formats.Add(mt);
                    }

                    mts.ClearExceptFirst();

                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            finally
            {
                if (enumPins != null)
                {
                    Marshal.ReleaseComObject(enumPins);
                }
            }
        }


        /// <summary>
        /// Gets filter video output pin.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="outputPin">
        /// Output pin.
        /// </param>
        /// <param name="streamIndex">
        /// Video pin index.
        /// </param>
        /// <returns>
        /// Returns video output pin.
        /// </returns>
        public static bool GetFilterVideoOutputPin(this IBaseFilter filter, out IPin outputPin, int streamIndex = 0)
        {
            bool result = false;
            outputPin = null;
            int index = 0;

            try
            {
                VFPinList pl1 = new VFPinList(filter);
                bool f = false;
                VFEnumMediaType videoMediaTypes = new VFEnumMediaType();

                for (int i = 0; i < pl1.Count(); i++)
                {
                    if (pl1.PinInfo(i).dir == PinDirection.Output)
                    {
                        videoMediaTypes.Assign(pl1[i]);
                        for (int j = 0; j < videoMediaTypes.Count; j++)
                        {
                            if ((videoMediaTypes.Items(j).majorType == MediaType.Video)
                                || (videoMediaTypes.Items(j).majorType == MediaType.Interleaved)
                                || (videoMediaTypes.Items(j).majorType == MediaType.Stream)
                                || (videoMediaTypes.Items(j).majorType == MediaType.AnalogVideo)
                                || (videoMediaTypes.Items(j).majorType == new Guid(DSConsts.MEDIATYPE_MPEG2_PACK))
                                || (videoMediaTypes.Items(j).majorType == new Guid(DSConsts.MEDIATYPE_MPEG2_PES))
                                || (videoMediaTypes.Items(j).majorType == MediaType.Mpeg2Sections)
                                || (videoMediaTypes.Items(j).majorType == new Guid(DSConsts.MEDIATYPE_MPEG1SystemStream))
                                || (videoMediaTypes.Items(j).majorType == MediaType.Null))
                            {
                                if (index == streamIndex)
                                {
                                    outputPin = pl1[i];
                                    pl1.Used[i] = true;
                                    f = true;
                                    break;
                                }
                                else
                                {
                                    index++;
                                    break;
                                }
                            }
                        }

                        if (f)
                        {
                            break;
                        }
                    }
                }

                if (outputPin != null)
                {
                    result = true;
                }

                videoMediaTypes.Clear();
                pl1.Clear();
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Gets video capture device output pin.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <returns>
        /// Returns output pin.
        /// </returns>
        public static IPin GetVideoCaptureDeviceOutputPin(this IBaseFilter filter)
        {
            IPin pin = DsFindPin.ByDirection(filter, PinDirection.Output, 0);

            return pin;
        }

        /// <summary>
        /// Counts filter pins.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pulInPins">
        /// Input pins count.
        /// </param>
        /// <param name="pulOutPins">
        /// Output pins count.
        /// </param>
        /// <returns>
        /// Returns true or 0 if the operation was successful.
        /// </returns>
        public static int CountFilterPins(this IBaseFilter filter, out int pulInPins, out int pulOutPins)
        {
            IEnumPins enumPins;
            IntPtr found = IntPtr.Zero;
            IPin[] pins = new IPin[1];

            // Verify input
            if (filter == null)
            {
                pulInPins = 0;
                pulOutPins = 0;
                return ErrorCodes.E_POINTER;
            }

            // Clear number of pins found
            pulInPins = 0;
            pulOutPins = 0;

            // Get pin enumerator
            int result = filter.EnumPins(out enumPins);
            if (result != 0)
            {
                return result;
            }

            enumPins.Reset();

            // Count every pin on the filter
            while (0 == enumPins.Next(1, pins, found))
            {
                PinDirection pinDir;
                result = pins[0].QueryDirection(out pinDir);
                if (pinDir == PinDirection.Input)
                {
                    pulInPins++;
                }
                else
                {
                    pulOutPins++;
                }

                Marshal.ReleaseComObject(pins[0]);
                pins[0] = null;
            }

            // ReSharper disable RedundantAssignment

            Marshal.ReleaseComObject(enumPins);
            enumPins = null;

            // ReSharper restore RedundantAssignment

            return result;
        }


        /// <summary>
        /// Gets pin with a specified media type.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="majorType">
        /// Major type.
        /// </param>
        /// <param name="subType">
        /// Subtype.
        /// </param>
        /// <param name="freePin">
        /// Pin must be not connected.
        /// </param>
        /// <returns>
        /// Returns pin.
        /// </returns>
        public static IPin GetPinWithMediaTypeAndSubtype(
            this IBaseFilter filter,
            PinDirection pinDirection,
            Guid majorType,
            Guid subType,
            bool freePin = false,
            bool ignoreSubType = false)
        {
            List<Guid> majorTypes = new List<Guid> { majorType };

            List<Guid> subTypes = new List<Guid> { subType };

            return GetPinWithMediaTypeAndSubtype(filter, pinDirection, majorTypes, subTypes, freePin, ignoreSubType);
        }

        /// <summary>
        /// Gets pin with a specified media type.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="majorTypes">
        /// Media types.
        /// </param>
        /// <param name="subTypes">
        /// Sub types.
        /// </param>
        /// <param name="freePin">
        /// Pin must be not connected.
        /// </param>
        /// <param name="ignoreSubType">
        /// True to ignore subtype.
        /// </param>
        /// <returns>
        /// Returns pin.
        /// </returns>
        public static IPin GetPinWithMediaTypeAndSubtype(
            this IBaseFilter filter,
            PinDirection pinDirection,
            List<Guid> majorTypes,
            List<Guid> subTypes,
            bool freePin = false,
            bool ignoreSubType = false)
        {
            IEnumPins enumPins;
            IPin pinRet = null;
            IPin[] pins = new IPin[1];

            if ((filter == null) || (majorTypes.Count == 0) || (majorTypes.Count != subTypes.Count)
                || (subTypes.Count == 0))
            {
                return null;
            }

            // Get the pin enumerator
            int hr = filter.EnumPins(out enumPins);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
                {
                    bool f = false;

                    PinInfo pi;

                    if (pins[0] == null)
                    {
                        continue;
                    }

                    pins[0].QueryPinInfo(out pi);

                    if (pi.dir == pinDirection)
                    {
                        DsUtils.FreePinInfo(ref pi);

                        for (int i = 0; i < majorTypes.Count; i++)
                        {
                            if ((pins[0].PinHaveThisType(majorTypes[i], subTypes[i], ignoreSubType)))
                            {
                                if (!freePin || (freePin && !pins[0].IsPinConnected()))
                                {
                                    pinRet = pins[0];
                                    // Marshal.ReleaseComObject(enumPins);
                                    f = true;

                                    break;
                                }
                            }
                        }
                    }

                    if (f)
                    {
                        break;
                    }

                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            finally
            {
                if (enumPins != null)
                {
                    Marshal.ReleaseComObject(enumPins);
                }
            }

            return pinRet;
        }

        /// <summary>
        /// Gets free pin with a specified media type.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="guid">
        /// Guid.
        /// </param>
        /// <returns>
        /// Returns pin.
        /// </returns>
        public static IPin GetFreePinWithMediaType(this IBaseFilter filter, PinDirection pinDirection, Guid guid)
        {
            IEnumPins enumPins;
            IPin pinRet = null;
            IPin[] pins = new IPin[1];

            if (filter == null)
            {
                return null;
            }

            // Get the pin enumerator
            int hr = filter.EnumPins(out enumPins);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
                {
                    PinInfo pi;
                    pins[0].QueryPinInfo(out pi);
                    if (pi.dir == pinDirection)
                    {
                        DsUtils.FreePinInfo(ref pi);

                        IPin pin2;
                        pins[0].ConnectedTo(out pin2);
                        if (pin2 != null)
                        {
                            Marshal.ReleaseComObject(pin2);
                            continue;
                        }

                        if (pins[0].PinHaveThisType(guid))
                        {
                            pinRet = pins[0];
                            break;
                        }
                    }

                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            finally
            {
                if (enumPins != null)
                {
                    Marshal.ReleaseComObject(enumPins);
                }
            }

            return pinRet;
        }

        /// <summary>
        /// Gets free pin with a specified media type.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="guid">
        /// Guid.
        /// </param>
        /// <returns>
        /// Returns pin.
        /// </returns>
        public static IPin GetFreePinWithMediaSubType(this IBaseFilter filter, PinDirection pinDirection, Guid guid)
        {
            IEnumPins enumPins;
            IPin pinRet = null;
            IPin[] pins = new IPin[1];

            if (filter == null)
            {
                return null;
            }

            // Get the pin enumerator
            int hr = filter.EnumPins(out enumPins);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
                {
                    PinInfo pi;
                    pins[0].QueryPinInfo(out pi);
                    if (pi.dir == pinDirection)
                    {
                        DsUtils.FreePinInfo(ref pi);

                        IPin pin2;
                        pins[0].ConnectedTo(out pin2);
                        if (pin2 != null)
                        {
                            Marshal.ReleaseComObject(pin2);
                            continue;
                        }

                        if (pins[0].PinHaveThisSubType(guid))
                        {
                            pinRet = pins[0];
                            break;
                        }
                    }

                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            finally
            {
                if (enumPins != null)
                {
                    Marshal.ReleaseComObject(enumPins);
                }
            }

            return pinRet;
        }

        /// <summary>
        /// Gets free pin with a specified media type.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <returns>
        /// Returns pin.
        /// </returns>
        public static IPin GetFreePin(this IBaseFilter filter, PinDirection pinDirection)
        {
            IEnumPins enumPins;
            //IPin pinRet = null;
            IPin[] pins = new IPin[1];

            if (filter == null)
            {
                return null;
            }

            // Get the pin enumerator
            int hr = filter.EnumPins(out enumPins);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
                {
                    PinInfo pi;
                    pins[0].QueryPinInfo(out pi);
                    if (pi.dir == pinDirection)
                    {
                        DsUtils.FreePinInfo(ref pi);

                        IPin pin2;
                        pins[0].ConnectedTo(out pin2);
                        if (pin2 != null)
                        {
                            Marshal.ReleaseComObject(pin2);
                            continue;
                        }

                        break;
                    }

                    Marshal.ReleaseComObject(pins[0]);
                }
            }
            finally
            {
                if (enumPins != null)
                {
                    Marshal.ReleaseComObject(enumPins);
                }
            }

            return pins[0];
        }

        /// <summary>
        /// Gets pin.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="index">
        /// Pin index.
        /// </param>
        /// <param name="pin">
        /// Output pin.
        /// </param>
        /// <returns>
        /// Returns true or 0 if the operation was successful.
        /// </returns>
        public static bool GetPin(this IBaseFilter filter, PinDirection pinDirection, int index, out IPin pin)
        {
            pin = DsFindPin.ByDirection(filter, pinDirection, index - 1);
            return pin != null;
        }

        /// <summary>
        /// Connects the specified source filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sourceFilter">The source filter.</param>
        /// <param name="filterGraph">The filter graph.</param>
        /// <returns>System.Int32.</returns>
        public static int Connect(this IBaseFilter filter, IBaseFilter sourceFilter, IFilterGraph2 filterGraph)
        {
            IPin srcPin = DsFindPin.ByDirection(sourceFilter, PinDirection.Output, 0);
            IPin dstPin = DsFindPin.ByDirection(filter, PinDirection.Input, 0);

            int hr = filterGraph.Connect(srcPin, dstPin);

            Marshal.ReleaseComObject(srcPin);
            Marshal.ReleaseComObject(dstPin);

            return hr;
        }

        /// <summary>
        /// Connects the specified source pin.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="srcPin">The source pin.</param>
        /// <param name="filterGraph">The filter graph.</param>
        /// <returns>System.Int32.</returns>
        public static int Connect(this IBaseFilter filter, IPin srcPin, IFilterGraph2 filterGraph)
        {
            IPin dstPin = DsFindPin.ByDirection(filter, PinDirection.Input, 0);

            int hr = filterGraph.Connect(srcPin, dstPin);

            Marshal.ReleaseComObject(dstPin);

            return hr;
        }

        /// <summary>
        /// Gets pins count.
        /// </summary>
        /// <param name="vSource">
        /// Filter.
        /// </param>
        /// <param name="vDir">
        /// Direction.
        /// </param>
        /// <returns>
        /// Returns pins count.
        /// </returns>
        public static int GetPinCount(this IBaseFilter vSource, PinDirection vDir)
        {
            IEnumPins ppEnum;
            IPin[] pPins = new IPin[1];

            int count = 0;

            if (vSource == null)
            {
                return 0;
            }

            // Get the pin enumerator
            var hr = vSource.EnumPins(out ppEnum);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while (ppEnum.Next(1, pPins, IntPtr.Zero) == 0)
                {
                    // Read the direction
                    PinDirection ppindir;
                    hr = pPins[0].QueryDirection(out ppindir);
                    DsError.ThrowExceptionForHR(hr);

                    // Is it the right direction?
                    if (ppindir == vDir)
                    {
                        count++;
                    }

                    Marshal.ReleaseComObject(pPins[0]);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(ppEnum);
            }

            return count;
        }

        /// <summary>
        /// Gets pin state.
        /// </summary>
        /// <param name="filter">
        /// Filter.
        /// </param>
        /// <param name="pinDirection">
        /// Pin direction.
        /// </param>
        /// <param name="mediaType">
        /// Media type.
        /// </param>
        /// <returns></returns>
        public static bool HavePin(this IBaseFilter filter, PinDirection pinDirection, Guid mediaType)
        {
            IPin pin = GetPinWithMediaTypeAndSubtype(filter, pinDirection, mediaType, Guid.Empty, false, true);

            if (pin == null)
            {
                return false;
            }
            else
            {
                Marshal.ReleaseComObject(pin);
                // ReSharper disable once RedundantAssignment
                pin = null;

                return true;
            }
        }
    }

}
