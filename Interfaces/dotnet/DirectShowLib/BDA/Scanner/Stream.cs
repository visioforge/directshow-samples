// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="Stream.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Data.Common;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Xml.Serialization;

    using VisioForge.DirectShowLib;

    /// <summary>
    /// Class Stream.
    /// </summary>
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
    internal class Stream
    {
        /// <summary>
        /// The audio format.
        /// </summary>
        private static readonly byte[] audioFormat = new byte[]
        {
        80, 0, 2, 0, 0x80, 0xbb, 0, 0, 0, 0x7d, 0, 0, 1, 0, 0, 0,
        0x16, 0, 2, 0, 0, 0xe8, 3, 0, 1, 0, 1, 0, 1, 0, 0x16, 0,
        0, 0, 0, 0, 0, 0, 0, 0
     };

        /// <summary>
        /// The extra data.
        /// </summary>
        private byte[] extraData;

        /// <summary>
        /// The identifier.
        /// </summary>
        private Guid? id;

        /// <summary>
        /// The ks data format bdampe g2 transport.
        /// </summary>
        private static readonly Guid KSDataFormatBDAMPEG2Transport = new Guid("F4AEB342-0329-4fdd-A8FD-4AFF4926C978");

        /// <summary>
        /// The ks data format media type video.
        /// </summary>
        private static readonly Guid KSDataFormatMediaTypeVideo = new Guid("73646976-0000-0010-8000-00aa00389b71");

        /// <summary>
        /// The ks data format specifier none.
        /// </summary>
        private static readonly Guid KSDataFormatSpecifierNone = new Guid("0F6417D6-C318-11D0-A43F-00A0C9223196");

        /// <summary>
        /// The ks data format subtype none.
        /// </summary>
        private static readonly Guid KSDataFormatSubtypeNone = new Guid("E436EB8E-524F-11CE-9F53-0020AF0BA770");

        /// <summary>
        /// The ks data format type mpe g2 sections.
        /// </summary>
        public static readonly Guid KSDataFormatTypeMPEG2Sections = new Guid("455F176C-4B06-47CE-9AEF-8CAEF73DF7B5");

        /// <summary>
        /// The media subtype a c3.
        /// </summary>
        private static readonly Guid MediaSubtypeAC3 = new Guid("e06d802c-db46-11cf-b4d1-00805f6cbbea");

        /// <summary>
        /// The media subtype mpeg2 audio.
        /// </summary>
        private static readonly Guid MediaSubtypeMpeg2Audio = new Guid("e06d802b-db46-11cf-b4d1-00805f6cbbea");

        /// <summary>
        /// The media type audio.
        /// </summary>
        private static readonly Guid MediaTypeAudio = new Guid("73647561-0000-0010-8000-00AA00389B71");

        /// <summary>
        /// The mpe g1 audio format.
        /// </summary>
        private static byte[] MPEG1AudioFormat = new byte[]
        {
        80, 0, 2, 0, 0x80, 0xbb, 0, 0, 0, 0x7d, 0, 0, 1, 0, 0, 0,
        0x16, 0, 2, 0, 0, 0xe8, 3, 0, 1, 0, 1, 0, 1, 0, 0x16, 0,
        0, 0, 0, 0, 0, 0, 0, 0
     };

        /// <summary>
        /// The mpe g1 audio format PTR.
        /// </summary>
        private static GCHandle MPEG1AudioFormatPtr = GCHandle.Alloc(MPEG1AudioFormat, GCHandleType.Pinned);

        /// <summary>
        /// The mpeg2 program video.
        /// </summary>
        private static byte[] Mpeg2ProgramVideo = new byte[]
        {
        0, 0, 0, 0, 0, 0, 0, 0, 0xd0, 2, 0, 0, 0x40, 2, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0xc0, 0xe1, 0xe4, 0, 0, 0, 0, 0, 0x80, 0x1a, 6, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0x10, 0, 0, 0, 9, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0xd0, 2, 0, 0,
        0xe0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0xd0, 2, 0, 0, 0x40, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0x98, 0xf4, 6, 0, 0x56, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 1, 0xb3, 0x2d, 1, 0xe0, 0x24, 9, 0xc4, 0x23, 0x81,
        0x10, 0x11, 0x11, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 20, 20, 20, 20, 20, 0x15,
        0x15, 0x15, 0x15, 0x15, 0x15, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17,
        0x17, 0x17, 0x17, 0x17, 0x18, 0x18, 0x18, 0x19, 0x18, 0x18, 0x18, 0x19, 0x1a, 0x1a, 0x1a, 0x1a,
        0x19, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1c, 0x1c, 0x1c, 0x1c, 30, 30, 30, 0x1f, 0x1f, 0x21,
        0, 0, 1, 0xb5, 20, 130, 0, 1, 0, 0
     };

        /// <summary>
        /// The mpeg2 program video PTR.
        /// </summary>
        private static GCHandle Mpeg2ProgramVideoPtr = GCHandle.Alloc(Mpeg2ProgramVideo, GCHandleType.Pinned);

        /// <summary>
        /// The MPEG sequence header length.
        /// </summary>
        private const int MpegSequenceHeaderLength = 0x56;

        /// <summary>
        /// The pid.
        /// </summary>
        private short pid;

        /// <summary>
        /// The sequence header.
        /// </summary>
        private static readonly byte[] sequenceHeader = new byte[]
        {
        0, 0, 1, 0xb3, 0x2d, 1, 0xe0, 0x24, 9, 0xc4, 0x23, 0x81, 0x10, 0x11, 0x11, 0x12,
        0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 20, 20, 20, 20, 20, 0x15, 0x15, 0x15, 0x15, 0x15,
        0x15, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17,
        0x18, 0x18, 0x18, 0x19, 0x18, 0x18, 0x18, 0x19, 0x1a, 0x1a, 0x1a, 0x1a, 0x19, 0x1b, 0x1b, 0x1b,
        0x1b, 0x1b, 0x1c, 0x1c, 0x1c, 0x1c, 30, 30, 30, 0x1f, 0x1f, 0x21, 0, 0, 1, 0xb5,
        20, 130, 0, 1, 0, 0
     };

        /// <summary>
        /// The type.
        /// </summary>
        private StreamType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class.
        /// </summary>
        public Stream()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public Stream(PMTTable.StreamDescription desc)
        {
            this.type = desc.Type;
            this.pid = desc.PID;
            this.extraData = desc.extraData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        internal Stream(DbDataReader reader)
        {
            this.id = new Guid?(reader.GetGuid(0));
            this.type = (StreamType)Enum.Parse(typeof(StreamType), reader.GetString(1));
            this.pid = reader.GetInt16(2);
            this.extraData = (byte[])reader.GetValue(3);
        }

        /// <summary>
        /// Gets the type of a c3 media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetAC3MediaType()
        {
            AMMediaType mediaType = new AMMediaType();
            mediaType.majorType = MediaType.Audio;
            mediaType.subType = MediaSubType.DolbyAC3;
            mediaType.fixedSizeSamples = true;
            mediaType.temporalCompression = false;
            mediaType.sampleSize = 1;
            WaveFormatExtensible structure = new WaveFormatExtensible();
            structure.baseFormat.wFormatTag = 80;
            structure.baseFormat.nChannels = 2;
            structure.baseFormat.nSamplesPerSec = 0xbb80;
            structure.baseFormat.nAvgBytesPerSec = 0x7d00;
            structure.baseFormat.nBlockAlign = 0x300;
            structure.baseFormat.wBitsPerSample = 0;
            structure.baseFormat.cbSize = 0x16;
            structure.wSamplesPerBlock = 0x9d30;
            structure.dwChannelMask = 0x11;
            structure.SubFormat = Guid.Empty;
            mediaType.formatType = FormatType.WaveEx;
            mediaType.formatSize = Marshal.SizeOf(structure);
            mediaType.formatPtr = Marshal.AllocCoTaskMem(mediaType.formatSize);
            Marshal.StructureToPtr(structure, mediaType.formatPtr, false);
            return mediaType;
        }

        /// <summary>
        /// Gets the type of the audio media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetAudioMediaType()
        {
            AMMediaType mediaType = new AMMediaType();
            mediaType.majorType = MediaType.Audio;
            mediaType.subType = MediaSubType.Mpeg2Audio;
            mediaType.fixedSizeSamples = true;
            mediaType.temporalCompression = false;
            mediaType.sampleSize = 1;
            mediaType.formatType = FormatType.WaveEx;
            mediaType.formatSize = audioFormat.Length;
            mediaType.formatPtr = Marshal.AllocHGlobal(audioFormat.Length);
            Marshal.Copy(audioFormat, 0, mediaType.formatPtr, audioFormat.Length);
            return mediaType;
        }

        /// <summary>
        /// Gets the type of the MPEG DVB si media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetMpegDvbSIMediaType()
        {
            AMMediaType mediaType = new AMMediaType();
            mediaType.majorType = KSDataFormatTypeMPEG2Sections;
            mediaType.subType = MediaSubType.DvbSI;
            mediaType.formatType = FormatType.None;
            return mediaType;
        }

        /// <summary>
        /// Gets the type of the MPEG sections media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetMpegSectionsMediaType()
        {
            AMMediaType mediaType = new AMMediaType();
            mediaType.majorType = KSDataFormatTypeMPEG2Sections;
            mediaType.subType = MediaSubType.Mpeg2Data;
            mediaType.formatType = FormatType.None;
            return mediaType;
        }

        /// <summary>
        /// Gets the type of the null media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetNullMediaType()
        {
            AMMediaType mediaType = new AMMediaType();
            mediaType.majorType = MediaType.Stream;
            mediaType.subType = MediaSubType.Null;
            mediaType.formatType = FormatType.None;
            return mediaType;
        }

        /// <summary>
        /// Gets the type of the transport stream media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetTransportStreamMediaType()
        {
            AMMediaType mediaType = new AMMediaType();
            mediaType.majorType = MediaType.Stream;
            mediaType.subType = KSDataFormatBDAMPEG2Transport;
            mediaType.formatType = FormatType.None;
            return mediaType;
        }

        /// <summary>
        /// Gets the type of the video media.
        /// </summary>
        /// <returns>AMMediaType.</returns>
        public static AMMediaType GetVideoMediaType()
        {
            int right = 720;
            int bottom = 0x240;
            AMMediaType mediaType = new AMMediaType();
            mediaType.majorType = MediaType.Video;
            mediaType.subType = MediaSubType.Mpeg2Video;
            mediaType.fixedSizeSamples = true;
            mediaType.temporalCompression = false;
            mediaType.sampleSize = 1;
            mediaType.unkPtr = IntPtr.Zero;
            Mpeg2VideoInfo structure = new Mpeg2VideoInfo();
            structure.videoInfo.SrcRect = new DsRect(0, 0, right, bottom);
            structure.videoInfo.TargetRect = new DsRect(0, 0, 0, 0);
            structure.videoInfo.BitRate = 0xe4e1c0;
            structure.videoInfo.BitErrorRate = 0;
            structure.videoInfo.AvgTimePerFrame = 0x61a80L;
            structure.videoInfo.InterlaceFlags = 0;
            structure.videoInfo.CopyProtectFlags = 0;
            structure.videoInfo.PictAspectRatioX = 0;
            structure.videoInfo.PictAspectRatioY = 0;
            structure.videoInfo.BmiHeader.Size = Marshal.SizeOf(structure.videoInfo.BmiHeader);
            structure.videoInfo.BmiHeader.Width = right;
            structure.videoInfo.BmiHeader.Height = bottom;
            structure.videoInfo.BmiHeader.Planes = 0;
            structure.videoInfo.BmiHeader.BitCount = 0;
            structure.videoInfo.BmiHeader.Compression = 0;
            structure.videoInfo.BmiHeader.ImageSize = 0;
            structure.videoInfo.BmiHeader.XPelsPerMeter = 0x7d0;
            structure.videoInfo.BmiHeader.YPelsPerMeter = 0xd842;
            structure.videoInfo.BmiHeader.ClrUsed = 0;
            structure.videoInfo.BmiHeader.ClrImportant = 0;
            structure.dwStartTimeCode = 0;
            structure.dwProfile = 0;
            structure.dwLevel = 0;
            structure.dwFlags = 0;
            structure.PopulateSequenceHeader();
            mediaType.formatType = FormatType.Mpeg2Video;
            mediaType.formatSize = Marshal.SizeOf(structure);
            //mediaType.formatSize = 0x1b4;
            mediaType.formatPtr = Marshal.AllocCoTaskMem(mediaType.formatSize);
            Marshal.StructureToPtr(structure, mediaType.formatPtr, false);
            return mediaType;
        }

        /// <summary>
        /// Saves the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        public void Save(Service service)
        {
            //new DB().ExecuteNonQuery("INSERT INTO ServiceStreams (ServiceID, Type, PID, ExtraData, ID) SELECT ?, ?, ?, ?, ?", new object[] { service.ID.ToString(), this.Type.ToString(), this.PID, this.ExtraData, this.ID.ToString() });
        }

        /// <summary>
        /// Gets or sets the extra data.
        /// </summary>
        /// <value>The extra data.</value>
        public byte[] ExtraData
        {
            get
            {
                return this.extraData;
            }

            set
            {
                this.extraData = value;
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid ID
        {
            get
            {
                if (!this.id.HasValue)
                {
                    this.id = new Guid?(Guid.NewGuid());
                }

                return this.id.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a c3.
        /// </summary>
        /// <value><c>true</c> if this instance is a c3; otherwise, <c>false</c>.</value>
        public bool IsAC3
        {
            get
            {
                if (this.Type != StreamType.Private)
                {
                    return false;
                }

                if (this.extraData[0] != 0x6a)
                {
                    return (this.extraData[0] == 10);
                }

                return true;
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
                if ((this.Type != StreamType.MPEG1Audio) && (this.Type != StreamType.MPEG2Audio))
                {
                    return this.IsAC3;
                }

                return true;
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

        /// <summary>
        /// Gets or sets the pid.
        /// </summary>
        /// <value>The pid.</value>
        public short PID
        {
            get
            {
                return this.pid;
            }

            set
            {
                this.pid = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the raw.
        /// </summary>
        /// <value>The type of the raw.</value>
        public int RawType
        {
            get
            {
                return (int)this.type;
            }

            set
            {
                this.type = (StreamType)value;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public StreamType Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
            }
        }

        /// <summary>
        /// Struct BitmapInfoHeader.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct BitmapInfoHeader
        {
            /// <summary>
            /// The bi size.
            /// </summary>
            public uint biSize;

            /// <summary>
            /// The bi width.
            /// </summary>
            public int biWidth;

            /// <summary>
            /// The bi height.
            /// </summary>
            public int biHeight;

            /// <summary>
            /// The bi planes.
            /// </summary>
            public ushort biPlanes;

            /// <summary>
            /// The bi bit count.
            /// </summary>
            public ushort biBitCount;

            /// <summary>
            /// The bi compression.
            /// </summary>
            public uint biCompression;

            /// <summary>
            /// The bi size image.
            /// </summary>
            public uint biSizeImage;

            /// <summary>
            /// The bi x pels per meter.
            /// </summary>
            public int biXPelsPerMeter;

            /// <summary>
            /// The bi y pels per meter.
            /// </summary>
            public int biYPelsPerMeter;

            /// <summary>
            /// The bi color used.
            /// </summary>
            public uint biClrUsed;

            /// <summary>
            /// The bi color important.
            /// </summary>
            public uint biClrImportant;
        }

        /// <summary>
        /// Struct Mpeg2VideoInfo
        /// Implements the <see cref="System.IDisposable" />.
        /// </summary>
        /// <seealso cref="System.IDisposable" />
        [StructLayout(LayoutKind.Sequential)]
        internal struct Mpeg2VideoInfo : IDisposable
        {
            /// <summary>
            /// The video information.
            /// </summary>
            public VideoInfoHeader2 videoInfo;

            /// <summary>
            /// The dw start time code.
            /// </summary>
            public UInt32 dwStartTimeCode;

            /// <summary>
            /// The cb sequence header.
            /// </summary>
            public UInt32 cbSequenceHeader;

            /// <summary>
            /// The dw profile.
            /// </summary>
            public UInt32 dwProfile;

            /// <summary>
            /// The dw level.
            /// </summary>
            public UInt32 dwLevel;

            /// <summary>
            /// The dw flags.
            /// </summary>
            public uint dwFlags; // ([Flags] enum)

            /// <summary>
            /// The dw sequence header.
            /// </summary>
            public IntPtr dwSequenceHeader;

            /// <summary>
            /// Populates the sequence header.
            /// </summary>
            public void PopulateSequenceHeader()
            {
                byte[] buffer = new byte[]
               {
                0, 0, 1, 0xb3, 0x6c, 0xc9, 0x2c, 1, 0x20, 0xca, 0x2c, 1, 1, 0, 0, 0,
                1, 0, 0, 0, 0xb0, 0xc9, 0x2c, 1, 160, 0xc9, 0x2c, 1, 0, 0, 0, 0,
                0x6c, 0xc9, 0x2c, 1, 0x74, 0xc4, 0x2c, 1, 240, 230, 0x29, 1, 0x40, 140, 90, 0x7b,
                0x21, 0xbc, 12, 0x7b, 0, 0, 0, 0, 240, 230, 0x29, 1, 0x22, 6, 0x37, 0,
                0x2b, 0, 0, 0, 240, 230, 0x29, 1, 0x18, 0x56, 0x2c, 1, 0xb8, 240, 0x12, 0,
                0x6b, 10, 6, 0x7b, 0x18, 0x56, 0x2c, 1, 0x7c, 0xf7, 0x29, 1, 240, 230, 0x29, 1,
                0x18, 0x56, 0x2c, 1, 0x79, 0x53, 0x10, 0x7b, 240, 230, 0x29, 1, 0x18, 0x56, 0x2c
             };

                dwSequenceHeader = Marshal.AllocCoTaskMem(buffer.Length);

                Marshal.Copy(buffer, 0, dwSequenceHeader, buffer.Length);

                //fixed (Mpeg2VideoInfo* infoRef = (Mpeg2VideoInfo*)this)
                //{
                //    for (int i = 0; i < buffer.Length; i++)
                //    {
                //        &infoRef->dwSequenceHeader.FixedElementField[i] = buffer[i];
                //    }

                //    this.cbSequenceHeader = (uint)buffer.Length;
                //}
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Marshal.FreeCoTaskMem(dwSequenceHeader);
            }
        }

        //[StructLayout(LayoutKind.Sequential)]
        //internal struct Mpeg2VideoInfox
        //{
        //    public VideoInfoHeader2 videoInfo;
        //    public uint dwStartTimeCode;
        //    public uint cbSequenceHeader;
        //    public uint dwProfile;
        //    public uint dwLevel;
        //    public uint dwFlags;

        //    [FixedBuffer(typeof(byte), 0x6f)]
        //    public dwSequenceHeaderCl dwSequenceHeader;

        //    public unsafe void PopulateSequenceHeader()
        //    {
        //        byte[] buffer = new byte[] { 
        //            0, 0, 1, 0xb3, 0x6c, 0xc9, 0x2c, 1, 0x20, 0xca, 0x2c, 1, 1, 0, 0, 0, 
        //            1, 0, 0, 0, 0xb0, 0xc9, 0x2c, 1, 160, 0xc9, 0x2c, 1, 0, 0, 0, 0, 
        //            0x6c, 0xc9, 0x2c, 1, 0x74, 0xc4, 0x2c, 1, 240, 230, 0x29, 1, 0x40, 140, 90, 0x7b, 
        //            0x21, 0xbc, 12, 0x7b, 0, 0, 0, 0, 240, 230, 0x29, 1, 0x22, 6, 0x37, 0, 
        //            0x2b, 0, 0, 0, 240, 230, 0x29, 1, 0x18, 0x56, 0x2c, 1, 0xb8, 240, 0x12, 0, 
        //            0x6b, 10, 6, 0x7b, 0x18, 0x56, 0x2c, 1, 0x7c, 0xf7, 0x29, 1, 240, 230, 0x29, 1, 
        //            0x18, 0x56, 0x2c, 1, 0x79, 0x53, 0x10, 0x7b, 240, 230, 0x29, 1, 0x18, 0x56, 0x2c
        //         };

        //        fixed (Mpeg2VideoInfo* infoRef = (Mpeg2VideoInfo*) this)
        //        {
        //            for (int i = 0; i < buffer.Length; i++)
        //            {
        //                &infoRef->dwSequenceHeader.FixedElementField[i] = buffer[i];
        //            }

        //            this.cbSequenceHeader = (uint) buffer.Length;
        //        }
        //    }

        //    // Nested Types
        //    [StructLayout(LayoutKind.Sequential, Size = 0x6f), CompilerGenerated, UnsafeValueType]
        //    internal struct dwSequenceHeaderCl
        //    {
        //        public byte FixedElementField;
        //    }
        //}

        /// <summary>
        /// Struct WaveFormatEx.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct WaveFormatEx
        {
            /// <summary>
            /// The w format tag.
            /// </summary>
            public ushort wFormatTag;

            /// <summary>
            /// The n channels.
            /// </summary>
            public ushort nChannels;

            /// <summary>
            /// The n samples per sec.
            /// </summary>
            public uint nSamplesPerSec;

            /// <summary>
            /// The n average bytes per sec.
            /// </summary>
            public uint nAvgBytesPerSec;

            /// <summary>
            /// The n block align.
            /// </summary>
            public ushort nBlockAlign;

            /// <summary>
            /// The w bits per sample.
            /// </summary>
            public ushort wBitsPerSample;

            /// <summary>
            /// The cb size.
            /// </summary>
            public ushort cbSize;
        }

        /// <summary>
        /// Struct WaveFormatExtensible.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal struct WaveFormatExtensible
        {
            /// <summary>
            /// The base format.
            /// </summary>
            [FieldOffset(0)]
            public Stream.WaveFormatEx baseFormat;

            /// <summary>
            /// The dw channel mask.
            /// </summary>
            [FieldOffset(20)]
            public uint dwChannelMask;

            /// <summary>
            /// The sub format.
            /// </summary>
            [FieldOffset(0x18)]
            public Guid SubFormat;

            /// <summary>
            /// The w reserved.
            /// </summary>
            [FieldOffset(0x12)]
            public ushort wReserved;

            /// <summary>
            /// The w samples per block.
            /// </summary>
            [FieldOffset(0x12)]
            public ushort wSamplesPerBlock;

            /// <summary>
            /// The w valid bits per sample.
            /// </summary>
            [FieldOffset(0x12)]
            public ushort wValidBitsPerSample;
        }
    }
}