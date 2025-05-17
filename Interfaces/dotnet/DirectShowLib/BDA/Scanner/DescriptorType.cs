// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 04-19-2013
// ***********************************************************************
// <copyright file="DescriptorType.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    /// <summary>
    /// Enum DescriptorType.
    /// </summary>
    internal enum DescriptorType : byte
    {
        /// <summary>
        /// The audio stream
        /// </summary>
        AudioStream = 3,

        /// <summary>
        /// The component descriptor
        /// </summary>
        ComponentDescriptor = 80,

        /// <summary>
        /// The extended event
        /// </summary>
        ExtendedEvent = 0x4e,

        /// <summary>
        /// The language
        /// </summary>
        Language = 10,

        /// <summary>
        /// The logical channel
        /// </summary>
        LogicalChannel = 0x83,

        /// <summary>
        /// The network name
        /// </summary>
        NetworkName = 0x40,

        /// <summary>
        /// The service
        /// </summary>
        Service = 0x48,

        /// <summary>
        /// The short event
        /// </summary>
        ShortEvent = 0x4d,

        /// <summary>
        /// The terrestrial delivery system
        /// </summary>
        TerrestrialDeliverySystem = 90,

        /// <summary>
        /// The video stream
        /// </summary>
        VideoStream = 2
    }
}