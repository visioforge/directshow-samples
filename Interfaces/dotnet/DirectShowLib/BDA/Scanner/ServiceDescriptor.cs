// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="ServiceDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Class ServiceDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class ServiceDescriptor : Descriptor
    {
#pragma warning disable S1104 // Fields should not have public accessibility
        /// <summary>
        /// The provider name.
        /// </summary>
        public string providerName;

        /// <summary>
        /// The service name.
        /// </summary>
        public string serviceName;

        /// <summary>
        /// The type.
        /// </summary>
        public ServiceType type;

#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe ServiceDescriptor(byte* p)
            : base(p)
        {
            this.type = *((ServiceType*)(p + 2));
            byte length = p[3];
            this.providerName = base.GetString(p, 4, length);
            byte num2 = p[4 + length];
            this.serviceName = base.GetString(p, (byte)(5 + length), num2);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Service descriptor - '{0}, {1}' - {2}", this.providerName, this.serviceName, this.type);
        }
    }
}