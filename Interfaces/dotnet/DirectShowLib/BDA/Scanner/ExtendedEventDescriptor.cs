// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="ExtendedEventDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Class ExtendedEventDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class ExtendedEventDescriptor : Descriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedEventDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe ExtendedEventDescriptor(byte* p)
            : base(p)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "Extended Event";
        }
    }
}