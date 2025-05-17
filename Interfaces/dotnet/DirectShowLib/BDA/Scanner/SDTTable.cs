// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="SDTTable.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class SDTTable.
    /// Implements the <see cref="VisioForge.Core.BDA.MPEGTable" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.MPEGTable" />
    internal class SDTTable : MPEGTable
    {
        /// <summary>
        /// The onid.
        /// </summary>
        private ushort onid;

        /// <summary>
        /// The services.
        /// </summary>
        private Dictionary<short, ServiceDescription> services;

        /// <summary>
        /// The tsid.
        /// </summary>
        private ushort tsid;

        /// <summary>
        /// Initializes a new instance of the <see cref="SDTTable"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="length">The length.</param>
        public unsafe SDTTable(byte* p, int length)
            : base(p, length)
        {
            this.services = new Dictionary<short, ServiceDescription>();
            this.Parse(p, length);
        }

        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="origLength">Length of the original.</param>
        public unsafe void Parse(byte* data, int origLength)
        {
            Utility.GetByte(data[1], 0, 1);
            Utility.GetByte(data[1], 4, 4);

            //byte num1 = data[2];
            this.tsid = (ushort)((data[3] << 8) | data[4]);
            this.onid = (ushort)((data[8] << 8) | data[9]);
            int pos = 11;
            int num2 = origLength - 15;
            while (num2 > 0)
            {
                int num3;
                ServiceDescription description = ServiceDescription.ParseService(data, pos, out num3);
                this.services.Add(description.ServiceID, description);
                num2 -= num3;
                pos += num3;
            }
        }

        /// <summary>
        /// Gets the onid.
        /// </summary>
        /// <value>The onid.</value>
        public ushort ONID
        {
            get
            {
                return this.onid;
            }
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        public Dictionary<short, ServiceDescription> Services
        {
            get
            {
                return this.services;
            }
        }

        /// <summary>
        /// Gets the tsid.
        /// </summary>
        /// <value>The tsid.</value>
        public ushort TSID
        {
            get
            {
                return this.tsid;
            }
        }

        /// <summary>
        /// Class ServiceDescription.
        /// </summary>
        internal class ServiceDescription
        {
            /// <summary>
            /// The desciptors.
            /// </summary>
            private List<Descriptor> desciptors = new List<Descriptor>();

            /// <summary>
            /// The has present following.
            /// </summary>
            private bool hasPresentFollowing;

            /// <summary>
            /// The has schedule.
            /// </summary>
            private bool hasSchedule;

            /// <summary>
            /// The is scrambled.
            /// </summary>
            private bool isScrambled;

            /// <summary>
            /// The name.
            /// </summary>
            private string name;

            /// <summary>
            /// The service identifier.
            /// </summary>
            private short serviceID;

            /// <summary>
            /// The type.
            /// </summary>
            private ServiceType type;

            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceDescription"/> class.
            /// </summary>
            /// <param name="serviceID">The service identifier.</param>
            public ServiceDescription(short serviceID)
            {
                this.serviceID = serviceID;
            }

            /// <summary>
            /// Adds the descriptors.
            /// </summary>
            /// <param name="descriptors">The descriptors.</param>
            public void AddDescriptors(ICollection<Descriptor> descriptors)
            {
                foreach (Descriptor descriptor in descriptors)
                {
                    this.desciptors.Add(descriptor);
                    if (descriptor is ServiceDescriptor descriptor2)
                    {
                        this.name = descriptor2.serviceName;
                        this.type = descriptor2.type;
                    }
                }
            }

            /// <summary>
            /// Parses the service.
            /// </summary>
            /// <param name="data">The data.</param>
            /// <param name="pos">The position.</param>
            /// <param name="length">The length.</param>
            /// <returns>SDTTable.ServiceDescription.</returns>
            /// <exception cref="System.ArgumentException">Error parsing service description.</exception>
            public static unsafe SDTTable.ServiceDescription ParseService(byte* data, int pos, out int length)
            {
                SDTTable.ServiceDescription description = new SDTTable.ServiceDescription((short)((data[pos] << 8) | data[pos + 1]));
                description.hasSchedule = Utility.GetByte(data[pos + 2], 6, 1) == 1;
                description.hasPresentFollowing = Utility.GetByte(data[pos + 2], 7, 1) == 1;
                description.isScrambled = Utility.GetByte(data[pos + 3], 3, 1) == 1;
                ushort num = (ushort)((Utility.GetByte(data[pos + 3], 4, 4) << 8) | data[pos + 4]);
                int num2 = num;
                int num3 = 5;
                while (num2 > 0)
                {
                    Descriptor item = Descriptor.Read((data + pos) + num3);
                    num2 -= item.length + 2;
                    num3 += item.length + 2;
                    description.desciptors.Add(item);
                    if (item is ServiceDescriptor descriptor2)
                    {
                        description.name = descriptor2.serviceName;
                    }
                }

                if ((num3 - 5) != num)
                {
                    throw new ArgumentException("Error parsing service description");
                }

                length = num + 5;
                return description;
            }

            /// <summary>
            /// Gets the descriptors.
            /// </summary>
            /// <value>The descriptors.</value>
            public List<Descriptor> Descriptors
            {
                get
                {
                    return this.desciptors;
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance has present following.
            /// </summary>
            /// <value><c>true</c> if this instance has present following; otherwise, <c>false</c>.</value>
            public bool HasPresentFollowing
            {
                get
                {
                    return this.hasPresentFollowing;
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance has schedule.
            /// </summary>
            /// <value><c>true</c> if this instance has schedule; otherwise, <c>false</c>.</value>
            public bool HasSchedule
            {
                get
                {
                    return this.hasSchedule;
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is scrambled.
            /// </summary>
            /// <value><c>true</c> if this instance is scrambled; otherwise, <c>false</c>.</value>
            public bool IsScrambled
            {
                get
                {
                    return this.isScrambled;
                }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name
            {
                get
                {
                    return this.name;
                }
            }

            /// <summary>
            /// Gets the service identifier.
            /// </summary>
            /// <value>The service identifier.</value>
            public short ServiceID
            {
                get
                {
                    return this.serviceID;
                }
            }

            /// <summary>
            /// Gets the type.
            /// </summary>
            /// <value>The type.</value>
            public ServiceType Type
            {
                get
                {
                    return this.type;
                }
            }
        }
    }
}