// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="EITTable.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    using VisioForge.DirectShowLib;

#pragma warning disable S1104 // Fields should not have public accessibility
    /// <summary>
    /// Class EITTable.
    /// Implements the <see cref="VisioForge.Core.BDA.MPEGTable" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.MPEGTable" />
    internal class EITTable : MPEGTable
    {
        /// <summary>
        /// The events.
        /// </summary>
        public List<Event> events;

        /// <summary>
        /// The last section number.
        /// </summary>
        public byte LastSectionNumber;

        /// <summary>
        /// The last table identifier.
        /// </summary>
        public byte LastTableID;

        /// <summary>
        /// The section number.
        /// </summary>
        public byte SectionNumber;

        /// <summary>
        /// The service identifier.
        /// </summary>
        public short serviceID;

        /// <summary>
        /// Initializes a new instance of the <see cref="EITTable"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="sectionLength">Length of the section.</param>
        public unsafe EITTable(byte* p, int sectionLength)
            : base(p, sectionLength)
        {
            int num2;
            this.events = new List<Event>();
            this.serviceID = Utility.GetShortReversed(p + 3);
            this.SectionNumber = p[6];
            this.LastSectionNumber = p[7];
            this.LastTableID = p[13];
            for (int i = 14; i < (sectionLength - 4); i += num2)
            {
                Event item = this.ParseEvent(p + i, out num2);
                if (item != null)
                {
                    this.events.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>DateTime.</returns>
        private unsafe DateTime GetDateTime(byte* p)
        {
            try
            {
                int uShort = Utility.GetUShort(p);
                int num2 = (int)((uShort - 15078.2) / 365.25);
                int month = (int)(((uShort - 14956.1) - ((int)(num2 * 365.25))) / 30.6001);
                int day = ((uShort - 0x3a6c) - ((int)(num2 * 365.25))) - ((int)(month * 30.6001));
                int num5 = ((month == 14) || (month == 15)) ? 1 : 0;
                num2 += num5;
                month = (month - 1) - (num5 * 12);
                DateTime time = new DateTime(0x76c + num2, month, day);
                return time.Add(this.GetDuration(p + 2));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>TimeSpan.</returns>
        private unsafe TimeSpan GetDuration(byte* p)
        {
            int bCD = Utility.GetBCD(p);
            int minutes = Utility.GetBCD(p + 1);
            return new TimeSpan(bCD, minutes, Utility.GetBCD(p + 2));
        }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <param name="tableSectionIDs">The table section i ds.</param>
        /// <returns>MPEG2Filter.</returns>
        public static MPEG2Filter GetFilter(Dictionary<byte, List<byte>> tableSectionIDs)
        {
            MPEG2Filter structure = new MPEG2Filter();
            structure.bVersionNumber = 1;
            structure.wFilterSize = (short)Marshal.SizeOf(structure);
            structure.fUseRawFilteringBits = true;
            structure.Mask = new byte[0x10];
            structure.Filter = new byte[0x10];
            for (int i = 0; i < 0x10; i++)
            {
                structure.Mask[i] = 0xff;
            }

            byte num2 = 0;
            byte num3 = 0xff;
            byte num4 = 0xff;
            byte num5 = 0;
            byte num6 = 0xff;
            byte num7 = 0xff;
            foreach (byte num8 in tableSectionIDs.Keys)
            {
                num2 = num8;
                num3 = (byte)(num3 & num8);
                num4 = (byte)(num4 & ~num8);
                foreach (byte num9 in tableSectionIDs[num8])
                {
                    num5 = num9;
                    num6 = (byte)(num6 & num9);
                    num7 = (byte)(num7 & ~num9);
                }
            }

            structure.Mask[0] = (byte)~(num3 | num4);
            structure.Filter[0] = num2;
            structure.Mask[6] = (byte)~(num6 | num7);
            structure.Filter[6] = num5;
            return structure;
        }

        /// <summary>
        /// Parses the event.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="length">The length.</param>
        /// <returns>Event.</returns>
        private unsafe Event ParseEvent(byte* p, out int length)
        {
            short @short = Utility.GetShort(p + 10, 0xfff);
            length = 12 + @short;
            short num2 = Utility.NTOH(Utility.GetShort(p));
            if (num2 == 0)
            {
                return null;
            }

            Event event2 = new Event();
            event2.eventID = num2;
            event2.startTime = this.GetDateTime(p + 2);
            event2.duration = this.GetDuration(p + 7);
            event2.runningStatus = (RunningStatus)((p[10] & 0xd0) >> 5);
            event2.descriptors = Descriptor.ReadList(p + 12, @short);
            foreach (Descriptor descriptor in event2.descriptors)
            {
                if (descriptor is ShortEventDescriptor descriptor2)
                {
                    event2.name = descriptor2.name;
                    event2.text = descriptor2.text;
                }
            }

            return event2;
        }

        //private unsafe void WriteEIT(byte* p)
        //{
        //    Log.WriteLine("", new object[0]);
        //    Log.WriteLine("table_id: {0:x}", new object[] { p[0] });
        //    Log.WriteLine("last_table_id: {0:x}", new object[] { p[13] });
        //    Log.WriteLine("service_id: {0:x}", new object[] { Utility.GetShort(p + 3) });
        //    Log.WriteLine("version_number: {0:x}", new object[] { (p[5] & 0x3e) >> 1 });
        //    Log.WriteLine("current_next_indicator: {0:x}", new object[] { p[5] & 1 });
        //    Log.WriteLine("section_number: {0:x}", new object[] { p[6] });
        //    Log.WriteLine("last_section_number: {0:x}", new object[] { p[7] });
        //}

        /// <summary>
        /// Gets the table identifier.
        /// </summary>
        /// <value>The table identifier.</value>
        public byte TableID
        {
            get
            {
                return (byte)base.tableID;
            }
        }

        /// <summary>
        /// Class Event.
        /// </summary>
        internal class Event
        {
            /// <summary>
            /// The descriptors.
            /// </summary>
            public List<Descriptor> descriptors;

            /// <summary>
            /// The duration.
            /// </summary>
            public TimeSpan duration;

            /// <summary>
            /// The event identifier.
            /// </summary>
            public short eventID;

            /// <summary>
            /// The name.
            /// </summary>
            public string name;

            /// <summary>
            /// The running status.
            /// </summary>
            public RunningStatus runningStatus;

            /// <summary>
            /// The start time.
            /// </summary>
            public DateTime startTime;

            /// <summary>
            /// The text.
            /// </summary>
            public string text;
        }
    }

#pragma warning restore S1104 // Fields should not have public accessibility
}