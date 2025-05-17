// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 11-18-2021
// ***********************************************************************
// <copyright file="MathHelper.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Class MathHelper.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Lows the word.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.UInt32.</returns>
        public static uint LowWord(this uint number)
        {
            return number & 0x0000FFFF;
        }

        /// <summary>
        /// Highes the word.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.UInt32.</returns>
        public static uint HighWord(this uint number)
        {
            return unchecked(number >> 16);
        }

        /// <summary>
        /// Clamps the specified minimum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>T.</returns>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static int RoundToSpecial(int value, int roundTo)
        {
            var res = value / roundTo;
            return res * roundTo;
        }

        /// <summary>
        /// Determines whether integer is in range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public static bool IsIntInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Ranges the specified minimum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="step">The step.</param>
        /// <returns>IEnumerable&lt;System.Int32&gt;.</returns>
        public static IEnumerable<int> GenRange(int min, int max, int step)
        {
            for (int i = min; i <= max; i = checked(i + step)) yield return i;
        }

        /// <summary>
        /// Hypotenuse.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>System.Double.</returns>
        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        /// <summary>
        /// Degreeses to radians.
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns>System.Double.</returns>
        public static double DegreesToRadians(double degrees)
        {
            const double pi = Math.PI;
            return degrees * pi / 180.0;
        }
    }
}
