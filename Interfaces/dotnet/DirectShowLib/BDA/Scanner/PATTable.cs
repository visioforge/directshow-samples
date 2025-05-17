// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="PATTable.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System.Collections.Generic;

    /// <summary>
    /// Class PATTable.
    /// Implements the <see cref="VisioForge.Core.BDA.MPEGTable" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.MPEGTable" />
    internal class PATTable : MPEGTable
    {
        /// <summary>
        /// The programs.
        /// </summary>
        private List<PMTDescriptor> programs;

        /// <summary>
        /// Initializes a new instance of the <see cref="PATTable"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="length">The length.</param>
        internal unsafe PATTable(byte* p, int length)
            : base(p, length)
        {
            this.programs = new List<PMTDescriptor>();
            int num = (base.sectionLength - 5) / 4;
            byte* numPtr = p + 12;
            for (int i = 0; num > 0; i += 4)
            {
                short @short = Utility.GetShort(numPtr + i);
                if ((@short != 0) && ((numPtr[i + 2] & 0xe0) == 0xe0))
                {
                    short pmtPid = Utility.GetShort((numPtr + i) + 2, 0x1fff);
                    this.programs.Add(new PMTDescriptor(@short, pmtPid));
                }

                num--;
            }
        }

        /// <summary>
        /// Gets or sets the programs.
        /// </summary>
        /// <value>The programs.</value>
        public List<PMTDescriptor> Programs
        {
            get
            {
                return this.programs;
            }

            set
            {
                this.programs = value;
            }
        }
    }
}