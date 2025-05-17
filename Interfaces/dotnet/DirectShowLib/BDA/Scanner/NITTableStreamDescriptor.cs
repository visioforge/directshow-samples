// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="NITTableStreamDescriptor.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;

#pragma warning disable S1104 // Fields should not have public accessibility

    /// <summary>
    /// Class StreamDescriptor.
    /// </summary>
    internal class NITTableStreamDescriptor
    {
        /// <summary>
        /// The center frequency.
        /// </summary>
        public int? centerFrequency;

        /// <summary>
        /// The channel numbers.
        /// </summary>
        public Dictionary<short, short> channelNumbers = new Dictionary<short, short>();

        /// <summary>
        /// The onid.
        /// </summary>
        public int onid;

        /// <summary>
        /// The tsid.
        /// </summary>
        public int tsid;

        /// <summary>
        /// Parses the stream.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="length">The length.</param>
        /// <returns>NITTable.StreamDescriptor.</returns>
        /// <exception cref="System.ArgumentException">Error parsing NIT descriptors.</exception>
        public static unsafe NITTableStreamDescriptor ParseStream(byte* data, out int length)
        {
            NITTableStreamDescriptor descriptor = new NITTableStreamDescriptor();
            descriptor.tsid = (ushort)((data[0] << 8) | data[1]);
            descriptor.onid = (ushort)((data[2] << 8) | data[3]);
            ushort num = (ushort)((Utility.GetByte(data[4], 4, 4) << 8) | data[5]);
            int num2 = num;
            int num3 = 6;

            while (num2 > 0)
            {
                Descriptor descriptor2 = Descriptor.Read(data + num3);
                num2 -= descriptor2.length + 2;
                num3 += descriptor2.length + 2;

                if (descriptor2 is TerrestrialDescriptor descriptor3)
                {
                    descriptor.centerFrequency = new int?(descriptor3.centerFrequency);
                }
                else if (descriptor2 is LogicalChannelDescriptor descriptor4)
                {
                    foreach (short num4 in descriptor4.ChannelNumbers.Keys)
                    {
                        descriptor.channelNumbers.Add(num4, descriptor4.ChannelNumbers[num4]);
                    }
                }
            }

            if ((num3 - 6) != num)
            {
                throw new ArgumentException("Error parsing NIT descriptors");
            }

            length = num + 6;
            return descriptor;
        }
    }

#pragma warning restore S1104 // Fields should not have public accessibility
}