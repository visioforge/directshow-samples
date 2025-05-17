// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="PMTTable.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class PMTTable.
    /// Implements the <see cref="VisioForge.Core.BDA.MPEGTable" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.BDA.MPEGTable" />
    internal class PMTTable : MPEGTable
    {
        /// <summary>
        /// The PCR pid.
        /// </summary>
        private short pcrPid;

        /// <summary>
        /// The program number.
        /// </summary>
        private short programNumber;

        /// <summary>
        /// The streams.
        /// </summary>
        private readonly List<StreamDescription> streams;

        /// <summary>
        /// Initializes a new instance of the <see cref="PMTTable"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="length">The length.</param>
        internal unsafe PMTTable(byte* p, int length)
            : base(p, length)
        {
            int num3;
            this.streams = new List<StreamDescription>();
            this.programNumber = Utility.GetShortReversed(p + 3);
            this.pcrPid = Utility.GetShort(p + 8, 0x1fff);
            short @short = Utility.GetShort(p + 10, 0xfff);
            Descriptor.ReadList(p + 12, @short);
            for (int i = @short + 12; i < (base.sectionLength - 4); i += num3)
            {
                this.streams.Add(this.ReadStream(p + i, out num3));
            }
        }

        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="length">The length.</param>
        /// <returns>StreamDescription.</returns>
        private unsafe StreamDescription ReadStream(byte* p, out int length)
        {
            StreamDescription description = new StreamDescription();
            description.Type = *((StreamType*)p);
            description.PID = Utility.GetShort(p + 1, 0x1fff);
            short @short = Utility.GetShort(p + 3, 0xfff);
            description.extraData = new byte[@short];
            Marshal.Copy(new IntPtr((void*)(p + 5)), description.extraData, 0, @short);
            length = @short + 5;
            return description;
        }

        /// <summary>
        /// Gets or sets the PCR pid.
        /// </summary>
        /// <value>The PCR pid.</value>
        public short PcrPid
        {
            get
            {
                return this.pcrPid;
            }

            set
            {
                this.pcrPid = value;
            }
        }

        /// <summary>
        /// Gets or sets the program number.
        /// </summary>
        /// <value>The program number.</value>
        public short ProgramNumber
        {
            get
            {
                return this.programNumber;
            }

            set
            {
                this.programNumber = value;
            }
        }

        /// <summary>
        /// Gets the streams.
        /// </summary>
        /// <value>The streams.</value>
        public List<StreamDescription> Streams
        {
            get
            {
                return this.streams;
            }
        }

        /// <summary>
        /// Class StreamDescription.
        /// </summary>
        internal class StreamDescription
        {
#pragma warning disable S1104 // Fields should not have public accessibility

            /// <summary>
            /// The extra data.
            /// </summary>
            public byte[] extraData;

            /// <summary>
            /// The pid.
            /// </summary>
            public short PID;

            /// <summary>
            /// The type.
            /// </summary>
            public StreamType Type;

            /// <summary>
            /// Gets a value indicating whether this instance is a c3.
            /// </summary>
            /// <value><c>true</c> if this instance is a c3; otherwise, <c>false</c>.</value>
            public bool IsAC3
            {
                get
                {
                    return ((this.Type == StreamType.Private) && (this.extraData[0] == 0x6a));
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is audio.
            /// </summary>
            /// <value><c>true</c> if this instance is audio; otherwise, <c>false</c>.</value>
            public bool IsAudio
            {
                get
                {
                    return (((this.Type == StreamType.MPEG1Audio) || (this.Type == StreamType.MPEG2Audio)) || ((this.Type == StreamType.Private) && (this.extraData[0] == 0x6a)));
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is tele text.
            /// </summary>
            /// <value><c>true</c> if this instance is tele text; otherwise, <c>false</c>.</value>
            public bool IsTeleText
            {
                get
                {
                    return ((this.Type == StreamType.Private) && (this.extraData[0] == 0x56));
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is video.
            /// </summary>
            /// <value><c>true</c> if this instance is video; otherwise, <c>false</c>.</value>
            public bool IsVideo
            {
                get
                {
                    if (this.Type != StreamType.MPEG1Video)
                    {
                        return (this.Type == StreamType.MPEG2Video);
                    }

                    return true;
                }
            }

#pragma warning restore S1104 // Fields should not have public accessibility
        }
    }
}