// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 09-09-2021
// ***********************************************************************
// <copyright file="VFDVBChannel.cs" company="VisioForge">
//     VisioForge (c) 2011
// </copyright>
// <summary>Defines the DSBDASource type.</summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA
{
#pragma warning disable S1104 // Fields should not have public accessibility
    /// <summary>
    /// Class VFDVBChannel.
    /// </summary>
    public class VFDVBChannel
    {
        /// <summary>
        /// The serv identifier.
        /// </summary>
        public short ServId;

        /// <summary>
        /// The name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The serv type.
        /// </summary>
        public byte ServType;

        /// <summary>
        /// The free CA mode.
        /// </summary>
        public bool FreeCAmode;

        /// <summary>
        /// The video pid.
        /// </summary>
        public short VideoPid;

        /// <summary>
        /// The audio pid.
        /// </summary>
        public short AudioPid;

        /// <summary>
        /// The modulation.
        /// </summary>
        public short Modulation;

        /// <summary>
        /// Initializes a new instance of the <see cref="VFDVBChannel"/> class.
        /// </summary>
        /// <param name="aSID">a sid.</param>
        /// <param name="aName">a name.</param>
        /// <param name="aServType">Type of a serv.</param>
        /// <param name="aFreeCAmode">if set to <c>true</c> [a free c amode].</param>
        /// <param name="aVideoPid">a video pid.</param>
        /// <param name="aAudioPid">a audio pid.</param>
        /// <param name="aModulation">a modulation.</param>
        public VFDVBChannel(short aSID, string aName, byte aServType, bool aFreeCAmode, short aVideoPid, short aAudioPid, byte aModulation)
        {
            ServId = aSID; Name = aName; ServType = aServType; FreeCAmode = aFreeCAmode; VideoPid = aVideoPid; AudioPid = aAudioPid;
            Modulation = aModulation;
        }
    }
#pragma warning restore S1104 // Fields should not have public accessibility
}
