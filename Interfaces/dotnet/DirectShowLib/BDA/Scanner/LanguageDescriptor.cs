// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="LanguageDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Class LanguageDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class LanguageDescriptor : Descriptor
    {
        /// <summary>
        /// The code.
        /// </summary>
        private string code;

        /// <summary>
        /// The type.
        /// </summary>
        private AudioType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe LanguageDescriptor(byte* p)
            : base(p)
        {
            this.code = base.GetString(p, 2, 3);
            this.type = *((AudioType*)(p + 5));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Language descriptor - '{0}' - {1:x}", this.code, this.type);
        }
    }
}