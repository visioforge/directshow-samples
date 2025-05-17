// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="TerrestrialDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Class TerrestrialDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class TerrestrialDescriptor : Descriptor
    {
        /// <summary>
        /// The center frequency.
        /// </summary>
#pragma warning disable S1104 // Fields should not have public accessibility
        public int centerFrequency;
#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrestrialDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe TerrestrialDescriptor(byte* p)
            : base(p)
        {
            this.centerFrequency = Utility.GetInt(p + 2) / 100;
        }
    }
}