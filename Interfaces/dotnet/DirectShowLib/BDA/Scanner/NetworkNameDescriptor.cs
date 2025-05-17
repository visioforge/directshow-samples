// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="NetworkNameDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Class NetworkNameDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class NetworkNameDescriptor : Descriptor
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkNameDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe NetworkNameDescriptor(byte* p)
            : base(p)
        {
            this.Name = base.GetString(p, 2, base.length);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Network name descriptor - '{0}'", this.Name);
        }
    }
}