// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="LogicalChannelDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System.Collections.Generic;

    /// <summary>
    /// Class LogicalChannelDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class LogicalChannelDescriptor : Descriptor
    {
        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public Dictionary<short, short> ChannelNumbers { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalChannelDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe LogicalChannelDescriptor(byte* p)
            : base(p)
        {
            ChannelNumbers = new Dictionary<short, short>();
            int index = 2;
            int length = base.length;
            while (length > 0)
            {
                short key = (short)((p[index] << 8) | p[index + 1]);
                short num4 = (short)(Utility.GetByte(p[index + 2], 6, 2) | p[index + 3]);
                ChannelNumbers.Add(key, num4);
                length -= 4;
                index += 4;
            }
        }
    }
}