// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="Descriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Class Descriptor.
    /// </summary>
    internal class Descriptor
    {
#pragma warning disable S1104 // Fields should not have public accessibility

        /// <summary>
        /// The length.
        /// </summary>
        public byte length;

        /// <summary>
        /// The minimum length.
        /// </summary>
        public const int MinLength = 2;

        /// <summary>
        /// The tag.
        /// </summary>
        public DescriptorType tag;

#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Initializes a new instance of the <see cref="Descriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe Descriptor(byte* p)
        {
            this.tag = *((DescriptorType*)p);
            this.length = p[1];
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        protected unsafe string GetString(byte* p, byte offset, byte length)
        {
            byte[] destination = new byte[length];
            Marshal.Copy(new IntPtr((void*)(p + offset)), destination, 0, length);
            return Encoding.ASCII.GetString(destination);
        }

        /// <summary>
        /// Reads the specified p.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>Descriptor.</returns>
        internal static unsafe Descriptor Read(byte* p)
        {
            switch (*(DescriptorType*)p)
            {
                case DescriptorType.Language:
                    return new LanguageDescriptor(p);

                case DescriptorType.NetworkName:
                    return new NetworkNameDescriptor(p);

                case DescriptorType.Service:
                    return new ServiceDescriptor(p);

                case DescriptorType.ShortEvent:
                    return new ShortEventDescriptor(p);

                case DescriptorType.ExtendedEvent:
                    return new ExtendedEventDescriptor(p);

                case DescriptorType.ComponentDescriptor:
                    return new ComponentDescriptor(p);

                case DescriptorType.TerrestrialDeliverySystem:
                    return new TerrestrialDescriptor(p);

                case DescriptorType.LogicalChannel:
                    return new LogicalChannelDescriptor(p);
            }

            return new Descriptor(p);
        }

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="descriptorLength">Length of the descriptor.</param>
        /// <returns>List&lt;Descriptor&gt;.</returns>
        internal static unsafe List<Descriptor> ReadList(byte* p, short descriptorLength)
        {
            Descriptor descriptor;
            List<Descriptor> list = new List<Descriptor>();
            for (int i = 0; i < descriptorLength; i += descriptor.length + 2)
            {
                descriptor = Read(p + i);
                list.Add(descriptor);
            }

            return list;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Unknown descriptor - {0:x}", this.tag);
        }
    }
}