// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="MPEGTable.cs" company="VisioForge">
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

    /// <summary>
    /// Class MPEGTable.
    /// </summary>
    internal class MPEGTable
    {
        /// <summary>
        /// The raw data.
        /// </summary>
        private readonly byte[] rawData;

        /// <summary>
        /// The section length.
        /// </summary>
        protected short sectionLength;

        /// <summary>
        /// The table identifier.
        /// </summary>
#pragma warning disable S1104 // Fields should not have public accessibility
        public TableType tableID;
#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Initializes a new instance of the <see cref="MPEGTable"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="length">The length.</param>
        public unsafe MPEGTable(byte* p, int length)
        {
            this.rawData = new byte[length];
            for (int i = 0; i < length; i++)
            {
                this.rawData[i] = p[i];
            }

            this.tableID = *((TableType*)p);
            this.sectionLength = Utility.GetShort(p + 1, 0xfff);
            if (this.sectionLength != length)
            {
                this.sectionLength = Utility.NTOH(this.sectionLength);
            }

            if (this.sectionLength != length)
            {
                this.sectionLength = Utility.NTOH(this.sectionLength);
            }

            this.sectionLength = Math.Min(this.sectionLength, (short)length);
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="pSection">The p section.</param>
        /// <param name="length">The length.</param>
        /// <returns>MPEGTable.</returns>
        public static unsafe MPEGTable GetSection(IntPtr pSection, int length)
        {
            if (length >= 4)
            {
                byte* p = (byte*)pSection.ToPointer();
                TableType type = *((TableType*)p);
                switch (type)
                {
                    case TableType.PAT:
                        return new PATTable(p, length);

                    case TableType.PMT:
                        return new PMTTable(p, length);

                    case TableType.NITActual:
                    case TableType.NITOther:
                        return new NITTable(p, length);

                    case TableType.SDTActual:
                    case TableType.SDTOther:
                        return new SDTTable(p, length);

                    case TableType.EITNowNextActual:
                        return new EITTable(p, length);
                }

                short num = (short)type;
                if ((num >= 80) && (num <= 0x6f))
                {
                    return new EITTable(p, length);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the raw data.
        /// </summary>
        /// <value>The raw data.</value>
        public byte[] RawData
        {
            get
            {
                return this.rawData;
            }
        }
    }
}