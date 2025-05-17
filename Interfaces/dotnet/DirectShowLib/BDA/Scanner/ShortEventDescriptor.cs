// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="ShortEventDescriptor.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Class ShortEventDescriptor.
    /// Implements the <see cref="VisioForge.Core.BDA.Descriptor" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.Descriptor" />
    internal class ShortEventDescriptor : Descriptor
    {
#pragma warning disable S1104 // Fields should not have public accessibility

        /// <summary>
        /// The language.
        /// </summary>
        public string language;

        /// <summary>
        /// The name.
        /// </summary>
        public string name;

        /// <summary>
        /// The text.
        /// </summary>
        public string text;

#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortEventDescriptor"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public unsafe ShortEventDescriptor(byte* p)
            : base(p)
        {
            this.language = base.GetString(p, 2, 3);
            byte length = p[5];
            this.name = base.GetString(p, 6, length);
            byte num2 = p[6 + length];
            this.text = base.GetString(p, (byte)(7 + length), num2);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Short Event: '{0}' - '{1}' ({2})", this.name, this.text, this.language);
        }
    }
}