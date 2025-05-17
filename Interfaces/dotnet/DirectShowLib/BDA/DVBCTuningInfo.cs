// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="DVBCTuningInfo.cs" company="VisioForge">
//     VisioForge (c) 2006 - 2021
// </copyright>
// <summary>Defines the DVBCTuningInfo type.</summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA
{
    using System;

    /// <summary>
    /// Class DVBCTuningInfo.
    /// Implements the <see cref="VisioForge.Core.BDA.TuningInfo" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.TuningInfo" />
    internal class DVBCTuningInfo : TuningInfo
    {
        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if successfull, <c>false</c> otherwise.</returns>
        protected bool Equals(DVBCTuningInfo other)
        {
            return this._symbolRate == other._symbolRate && this._frequency == other._frequency && this._modulationType == other._modulationType;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this._symbolRate;
                hashCode = (hashCode * 397) ^ this._frequency;
                hashCode = (hashCode * 397) ^ (int)this._modulationType;
                return hashCode;
            }
        }

        /// <summary>
        /// The symbol rate.
        /// </summary>
        private readonly int _symbolRate;

        /// <summary>
        /// The frequency.
        /// </summary>
        private readonly int _frequency;

        /// <summary>
        /// The modulation type.
        /// </summary>
        private readonly ModulationType _modulationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DVBCTuningInfo"/> class.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="symbolRate">The symbol rate.</param>
        /// <param name="modulationType">Type of the modulation.</param>
        public DVBCTuningInfo(int frequency, int symbolRate, ModulationType modulationType)
        {
            this._frequency = frequency;
            this._symbolRate = symbolRate;
            _modulationType = modulationType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">Object must be DVBCTuningInfo.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3877:Exceptions should not be thrown from unexpected methods", Justification = "<Pending>")]
        public override bool Equals(object obj)
        {
            DVBCTuningInfo info = obj as DVBCTuningInfo;
            if (info == null)
            {
                throw new ArgumentException("Object must be DVBCTuningInfo"); //-V3115
            }

            return ((this._frequency == info.Frequency) && (this._symbolRate == info.SymbolRate) && (this._modulationType == info.ModulationType));
        }

        /// <summary>
        /// Serialises to string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string SerialiseToString()
        {
            return (this._frequency.ToString() + " " + this._symbolRate.ToString() + " " + this._modulationType.ToString());
        }

        /// <summary>
        /// Gets the symbol rate.
        /// </summary>
        /// <value>The symbol rate.</value>
        public int SymbolRate
        {
            get
            {
                return this._symbolRate;
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
                return this._frequency;
            }
        }

        /// <summary>
        /// Gets the type of the modulation.
        /// </summary>
        /// <value>The type of the modulation.</value>
        public ModulationType ModulationType
        {
            get
            {
                return this._modulationType;
            }
        }
    }
}