// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="DVBTTuningInfo.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA
{
    using System;

    /// <summary>
    /// Class DVBTTuningInfo.
    /// Implements the <see cref="VisioForge.Core.BDA.TuningInfo" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.TuningInfo" />
    internal class DVBTTuningInfo : TuningInfo
    {
        /// <summary>
        /// The bandwidth.
        /// </summary>
        private int bandwidth;

        /// <summary>
        /// The frequency.
        /// </summary>
        private int frequency;

        /// <summary>
        /// Initializes a new instance of the <see cref="DVBTTuningInfo"/> class.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="bandwidth">The bandwidth.</param>
        public DVBTTuningInfo(int frequency, int bandwidth)
        {
            this.frequency = frequency;
            this.bandwidth = bandwidth;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">Object must be DVBTTuningInfo.</exception>
        public override bool Equals(object obj)
        {
            DVBTTuningInfo info = obj as DVBTTuningInfo;
            if (info == null)
            {
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
                throw new ArgumentException("Object must be DVBTTuningInfo"); //-V3115
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
            }

            return ((this.frequency == info.Frequency) && (this.bandwidth == info.bandwidth));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (this.frequency ^ this.bandwidth);
        }

        /// <summary>
        /// Serialises to string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string SerialiseToString()
        {
            return (this.frequency.ToString() + " " + this.bandwidth.ToString());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Concat(new object[] { this.Frequency.ToString(), " (", this.Bandwidth, "MHz)" });
        }

        /// <summary>
        /// Gets the bandwidth.
        /// </summary>
        /// <value>The bandwidth.</value>
        public int Bandwidth
        {
            get
            {
                return this.bandwidth;
            }
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get
            {
                return this.frequency;
            }
        }
    }
}