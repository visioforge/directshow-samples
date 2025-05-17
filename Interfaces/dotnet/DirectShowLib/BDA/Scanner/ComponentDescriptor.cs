// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="ComponentDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Class ComponentDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class ComponentDescriptor : Descriptor
    {
        /// <summary>
        /// The component tag.
        /// </summary>
        private byte componentTag;

        /// <summary>
        /// The component type.
        /// </summary>
        private byte componentType;

        /// <summary>
        /// The language code.
        /// </summary>
        private string languageCode;

        /// <summary>
        /// The stream content.
        /// </summary>
        private byte streamContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe ComponentDescriptor(byte* p)
            : base(p)
        {
            this.streamContent = (byte)(p[2] & 15);
            this.componentType = p[3];
            this.componentTag = p[4];
            this.languageCode = base.GetString(p, 5, (byte)(base.length - 5));
        }
    }
}