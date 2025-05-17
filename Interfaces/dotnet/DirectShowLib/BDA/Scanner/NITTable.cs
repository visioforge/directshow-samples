// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="NITTable.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class NITTable.
    /// Implements the <see cref="VisioForge.Core.BDA.MPEGTable" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.MPEGTable" />
    internal partial class NITTable : MPEGTable
    {
        /// <summary>
        /// The center frequency.
        /// </summary>
        private int? centerFrequency;

        /// <summary>
        /// The descriptors.
        /// </summary>
        private List<Descriptor> descriptors;

        /// <summary>
        /// The network identifier.
        /// </summary>
        private short networkID;

        /// <summary>
        /// The network name.
        /// </summary>
        private string networkName;

        /// <summary>
        /// The streams.
        /// </summary>
        private List<NITTableStreamDescriptor> streams;

        /// <summary>
        /// Initializes a new instance of the <see cref="NITTable"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="length">The length.</param>
        /// <exception cref="System.ArgumentException">Error parsing NIT descriptors.</exception>
        public unsafe NITTable(byte* data, int length)
            : base(data, length)
        {
            this.descriptors = new List<Descriptor>();
            this.streams = new List<NITTableStreamDescriptor>();
            Utility.GetByte(data[1], 0, 1);
            Utility.GetByte(data[1], 4, 4);

            //byte num1 = data[2];
            this.networkID = (short)((data[3] << 8) | data[4]);
            ushort num = (ushort)((Utility.GetByte(data[8], 4, 4) << 8) | data[9]);
            int num2 = num;
            int index = 10;
            while (num2 > 0)
            {
                Descriptor descriptor = Descriptor.Read(data + index);
                num2 -= descriptor.length + 2;
                index += descriptor.length + 2;
                if (descriptor is NetworkNameDescriptor descriptor2)
                {
                    this.networkName = descriptor2.Name;
                }
            }

            if ((index - 10) != num)
            {
                throw new ArgumentException("Error parsing NIT descriptors");
            }

            ushort num4 = (ushort)((Utility.GetByte(data[index], 4, 4) << 8) | data[index + 1]);
            int num5 = index + 2;
            num2 = num4;
            while (num2 > 0)
            {
                int num6;
                this.streams.Add(NITTableStreamDescriptor.ParseStream(data + num5, out num6));
                num2 -= num6;
                num5 += num6;
            }
        }

        /// <summary>
        /// Selects the stream.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>StreamDescriptor.</returns>
        public NITTableStreamDescriptor SelectStream(int frequency)
        {
            foreach (NITTableStreamDescriptor descriptor in this.streams)
            {
                if (descriptor.centerFrequency.HasValue)
                {
                    int num = frequency - descriptor.centerFrequency.Value;
                    if (num <= 500)
                    {
                        this.centerFrequency = descriptor.centerFrequency;
                        return descriptor;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets or sets the center frequency.
        /// </summary>
        /// <value>The center frequency.</value>
        public int? CenterFrequency
        {
            get
            {
                return this.centerFrequency;
            }

            set
            {
                this.centerFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the descriptors.
        /// </summary>
        /// <value>The descriptors.</value>
        public List<Descriptor> Descriptors
        {
            get
            {
                return this.descriptors;
            }

            set
            {
                this.descriptors = value;
            }
        }

        /// <summary>
        /// Gets the network identifier.
        /// </summary>
        /// <value>The network identifier.</value>
        public short NetworkID
        {
            get
            {
                return this.networkID;
            }
        }

        /// <summary>
        /// Gets or sets the name of the network.
        /// </summary>
        /// <value>The name of the network.</value>
        public string NetworkName
        {
            get
            {
                return this.networkName;
            }

            set
            {
                this.networkName = value;
            }
        }

        /// <summary>
        /// Gets the streams.
        /// </summary>
        /// <value>The streams.</value>
        public List<NITTableStreamDescriptor> Streams
        {
            get
            {
                return this.streams;
            }
        }
    }
}