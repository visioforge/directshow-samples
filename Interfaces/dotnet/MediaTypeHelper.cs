// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaTypeHelper.cs" company="VisioForge">
//   VisioForge (c) 2006 - 2017
// </copyright>
// <summary>
//   Media type helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    using DirectShowLib;
    using DirectShowLib.DES;


    /// <summary>
    /// Media types enumerator.
    /// </summary>
    public class VFEnumMediaType
    {
        /// <summary>
        /// Types list.
        /// </summary>
        private readonly List<AMMediaType> FFList = new List<AMMediaType>();

        /// <summary>
        /// Gets types count.
        /// </summary>
        public int Count
        {
            get
            {
                return FFList.Count;
            }
        }

        /// <summary>
        /// Gets media type.
        /// </summary>
        /// <param name="index">
        /// Item index.
        /// </param>
        /// <returns>
        /// Returns media type.
        /// </returns>
        public AMMediaType Items(int index)
        {
            return FFList[index];
        }

        ///// <summary>
        ///// Gets media description.
        ///// </summary>
        ///// <param name="index">
        ///// Item index.
        ///// </param>
        ///// <returns>
        ///// Returns media description.
        ///// </returns>
        //public string MediaDescription(int index)
        //{
        //    if ((index < Count) && (index > -1))
        //    {
        //        return FFList[index].GetMediaTypeDescription();
        //    }

        //    return string.Empty;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="VFEnumMediaType"/> class.
        /// </summary>
        public VFEnumMediaType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VFEnumMediaType"/> class.
        /// </summary>
        /// <param name="enumMT">
        /// Media types enumerator.
        /// </param>
        public VFEnumMediaType(IEnumMediaTypes enumMT)
        {
            Assign(enumMT);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VFEnumMediaType"/> class.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        public VFEnumMediaType(IPin pin)
        {
            if (pin == null)
            {
                return;
            }

            IEnumMediaTypes enumMT;

            Debug.Assert(pin != null, "IPin not assigned");
            int hr = pin.EnumMediaTypes(out enumMT);
            if (hr == 0)
            {
                Assign(enumMT);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VFEnumMediaType"/> class.
        /// </summary>
        /// <param name="fileName">
        /// File name.
        /// </param>
        public VFEnumMediaType(string fileName)
        {
            Assign(fileName);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="VFEnumMediaType"/> class. 
        /// </summary>
        ~VFEnumMediaType()
        {
            Clear();
        }

        ///// <summary>
        ///// Gets descriptions.
        ///// </summary>
        ///// <returns>
        ///// Returns descriptions.
        ///// </returns>
        //public List<string> Descriptions()
        //{
        //    List<string> lst = new List<string>();

        //    foreach (var mediaType in FFList)
        //    {
        //        lst.Add(mediaType.GetMediaTypeDescription());
        //    }

        //    return lst;
        //}

        /// <summary>
        /// Adds item.
        /// </summary>
        /// <param name="item">
        /// Item.
        /// </param>
        public void Add(AMMediaType item)
        {
            FFList.Add(item);
        }

        /// <summary>
        /// Assign media types enumerator.
        /// </summary>
        /// <param name="enumMT">
        /// Media types enumerator.
        /// </param>
        public void Assign(IEnumMediaTypes enumMT)
        {
            if (Count != 0)
            {
                Clear();
            }

            if (enumMT == null)
            {
                return;
            }

            AMMediaType[] pmt = new AMMediaType[1];

            Debug.Assert(enumMT != null, "IEnumMediaType not assigned");

            while (enumMT.Next(1, pmt, IntPtr.Zero) == 0)
            {
                FFList.Add(pmt[0]);
            }
        }

        /// <summary>
        /// Assign to a pin.
        /// </summary>
        /// <param name="pin">
        /// Pin.
        /// </param>
        public void Assign(IPin pin)
        {
            if (pin == null)
            {
                return;
            }

            IEnumMediaTypes enumMT;
            Clear();

            Debug.Assert(pin != null, "IPin not assigned");

            int hr = pin.EnumMediaTypes(out enumMT);
            if (hr != 0)
            {
                return;
            }

            Assign(enumMT);
        }

        /// <summary>
        /// Assign to a file name.
        /// </summary>
        /// <param name="fileName">
        /// File name.
        /// </param>
        public void Assign(string fileName)
        {
            int streams;
            AMMediaType mediaType = new AMMediaType();
            Clear();

            IMediaDet mediaDet = (IMediaDet)new MediaDet();

            Debug.Assert(mediaDet != null, "Media Detector not available");
            int hr = mediaDet.put_Filename(fileName);

            if (hr != 0)
            {
                Marshal.ReleaseComObject(mediaDet);
                return;
            }

            mediaDet.get_OutputStreams(out streams);
            if (streams > 0)
            {
                for (int i = 0; i <= (streams - 1); i++)
                {
                    mediaDet.put_CurrentStream(i);
                    mediaDet.get_StreamMediaType(mediaType);
                    FFList.Add(mediaType);
                }
            }

            Marshal.ReleaseComObject(mediaDet);
        }

        /// <summary>
        /// Clears list.
        /// </summary>
        public void Clear()
        {
            try
            {
                if (Count != 0)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        try
                        {
                            DsUtils.FreeAMMediaType(FFList[i]);
                        }
                        catch
                        {
                        }
                    }
                }

                FFList.Clear();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Clears list, except first item.
        /// </summary>
        public void ClearExceptFirst()
        {
            try
            {
                if (Count > 1)
                {
                    for (int i = 1; i < Count; i++)
                    {
                        try
                        {
                            DsUtils.FreeAMMediaType(FFList[i]);
                        }
                        catch
                        {
                        }
                    }
                }

                FFList.Clear();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Deletes media type.
        /// </summary>
        /// <param name="index">
        /// Item index.
        /// </param>
        public void Delete(int index)
        {
            if (FFList[index] != null)
            {
                DsUtils.FreeAMMediaType(FFList[index]);
            }

            FFList.RemoveAt(index);
        }
    }

    /// <summary>
    /// Media type helper.
    /// </summary>
    public static class MediaTypeHelper
    {
        /// <summary>
        /// NLog logger.
        /// </summary>
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Create an audio media type
        /// </summary>
        /// <param name="sampleRate">
        /// Sample rate.
        /// </param>
        /// <param name="channels">
        /// Channels.
        /// </param>
        /// <param name="bps">
        /// BPS.
        /// </param>
        /// <returns>
        /// The constructed media type
        /// </returns>
        public static AMMediaType GenerateAudioMediaType(int sampleRate, int channels, int bps)
        {
            var AudioGroupType = new AMMediaType
            {
                majorType = MediaType.Audio,
                subType = MediaSubType.PCM
            };

            if (bps == 0)
            {
                bps = 16;
            }

            if (sampleRate != 0 && channels != 0 && bps != 0)
            {
                AudioGroupType.formatType = FormatType.WaveEx;
                AudioGroupType.fixedSizeSamples = true;
                AudioGroupType.formatSize = Marshal.SizeOf(typeof(WaveFormatEx));

                var wfx = new WaveFormatEx
                {
                    wFormatTag = 1,
                    nChannels = (short)channels,
                    nSamplesPerSec = sampleRate,
                    wBitsPerSample = (short)bps
                };
                wfx.nBlockAlign = (short)(wfx.nChannels * wfx.wBitsPerSample / 8);
                wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;

                AudioGroupType.sampleSize = wfx.nBlockAlign;
                AudioGroupType.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(wfx));

                Marshal.StructureToPtr(wfx, AudioGroupType.formatPtr, false);
            }

            return AudioGroupType;
        }

        /// <summary>
        /// Generatea video media type for Sample Grabber.
        /// </summary>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="frameRate">
        /// Frame rate.
        /// </param>
        /// <param name="subtype">
        /// Subtype.
        /// </param>
        /// <returns>
        /// The <see cref="AMMediaType"/>.
        /// </returns>
        public static AMMediaType GenerateVideoMediaType(int width, int height, double frameRate, Guid subtype)
        {
            AMMediaType sampleGrabberMediaType = null;

            try
            {
                sampleGrabberMediaType = new AMMediaType
                {
                    majorType = MediaType.Video,
                    subType = subtype,
                    formatType = FormatType.VideoInfo
                };

                VideoInfoHeader vih = new VideoInfoHeader
                {
                    BmiHeader = new BitmapInfoHeader(),
                    SrcRect = new DsRect(),
                    TargetRect = new DsRect()
                };

                //vih.SrcRect.Right = width;
                //vih.SrcRect.Bottom = height;
                //vih.TargetRect = vih.SrcRect;

                vih.BmiHeader.Width = width;
                vih.BmiHeader.Height = height;
                vih.BmiHeader.Planes = 1;
                vih.BmiHeader.Size = Marshal.SizeOf(typeof(BitmapInfoHeader));

                if (frameRate > 0)
                {
                    vih.AvgTimePerFrame = (long)(10000 * 1000 / frameRate);
                }

                const uint BI_RGB = 0;
                if (subtype == MediaSubType.RGB24)
                {
                    vih.BmiHeader.BitCount = 24;
                    vih.BmiHeader.Compression = 0;
                    vih.BmiHeader.ImageSize = ImageHelper.GetStrideRGB24(width) * Math.Abs(height);
                }
                else if (subtype == MediaSubType.RGB32 || subtype == MediaSubType.ARGB32)
                {
                    vih.BmiHeader.BitCount = 32;
                    vih.BmiHeader.Compression = 0;
                    vih.BmiHeader.ImageSize = ImageHelper.GetStrideRGB32(width) * Math.Abs(height);
                }

                sampleGrabberMediaType.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(vih));
                Marshal.StructureToPtr(vih, sampleGrabberMediaType.formatPtr, false);

                sampleGrabberMediaType.formatSize = Marshal.SizeOf(typeof(VideoInfoHeader));
                sampleGrabberMediaType.sampleSize = vih.BmiHeader.ImageSize;
            }
            catch (Exception e)
            {
                //Logger?.Error(e.Message, e);
            }

            return sampleGrabberMediaType;
        }

        /// <summary>
        /// Converts major type to string.
        /// </summary>
        /// <param name="guid">
        /// Guid.
        /// </param>
        /// <returns>
        /// Returns major type.
        /// </returns>
        public static string MajorTypeToString(Guid guid)
        {
            if (guid == MediaType.AnalogAudio)
            {
                return "AnalogAudio";
            }

            if (guid == MediaType.AnalogVideo)
            {
                return "AnalogVideo";
            }

            if (guid == MediaType.Audio)
            {
                return "Audio";
            }

            if (guid == MediaType.AuxLine21Data)
            {
                return "AUXLine21Data";
            }

            if (guid == MediaType.File)
            {
                return "File";
            }

            if (guid == MediaType.Interleaved)
            {
                return "Interleaved";
            }

            if (guid == MediaType.LMRT)
            {
                return "LMRT";
            }

            if (guid == MediaType.Midi)
            {
                return "Midi";
            }

            if (guid == MediaType.ScriptCommand)
            {
                return "ScriptCommand";
            }

            if (guid == MediaType.Stream)
            {
                return "Stream";
            }

            if (guid == MediaType.Texts)
            {
                return "Text";
            }

            if (guid == MediaType.Timecode)
            {
                return "Timecode";
            }

            if (guid == MediaType.URLStream)
            {
                return "URL_STREAM";
            }

            if (guid == MediaType.Video)
            {
                return "Video";
            }

            if (guid == MediaType.VBI)
            {
                return "VBI";
            }

            if (guid == new Guid(DSConsts.MEDIATYPE_MPEG2_PACK))
            {
                return "MPEG2_PACK";
            }

            if (guid == new Guid(DSConsts.MEDIATYPE_MPEG2_PES))
            {
                return "MPEG2_PES";
            }

            if (guid == MediaType.Mpeg2Sections)
            {
                return "MPEG2_SECTIONS";
            }

            return "Unknown: " + guid;
        }

        /// <summary>
        /// Converts sub type to string.
        /// </summary>
        /// <param name="guid">
        /// Guid.
        /// </param>
        /// <returns>
        /// Returns sub type.
        /// </returns>
        public static string SubTypeToString(Guid guid)
        {
            if (guid == MediaSubType.CLPL)
            {
                return "CLPL";
            }

            if (guid == MediaSubType.YUYV)
            {
                return "YUYV";
            }

            if (guid == MediaSubType.IYUV)
            {
                return "IYUV";
            }

            if (guid == MediaSubType.YVU9)
            {
                return "YVU9";
            }

            if (guid == MediaSubType.Y411)
            {
                return "Y411";
            }

            if (guid == MediaSubType.Y41P)
            {
                return "Y41P";
            }

            if (guid == MediaSubType.YUY2)
            {
                return "YUY2";
            }

            if (guid == MediaSubType.YVYU)
            {
                return "YVYU";
            }

            if (guid == MediaSubType.UYVY)
            {
                return "UYVY";
            }

            if (guid == MediaSubType.Y211)
            {
                return "Y211";
            }

            if (guid == MediaSubType.YV12)
            {
                return "YV12";
            }

            if (guid == MediaSubType.CLJR)
            {
                return "CLJR";
            }

            if (guid == MediaSubType.IF09)
            {
                return "IF09";
            }

            if (guid == MediaSubType.CPLA)
            {
                return "CPLA";
            }

            if (guid == MediaSubType.MJPG)
            {
                return "MJPG";
            }

            if (guid == MediaSubType.TVMJ)
            {
                return "TVMJ";
            }

            if (guid == MediaSubType.WAKE)
            {
                return "WAKE";
            }

            if (guid == MediaSubType.CFCC)
            {
                return "CFCC";
            }

            if (guid == MediaSubType.IJPG)
            {
                return "IJPG";
            }

            if (guid == MediaSubType.PLUM)
            {
                return "Plum";
            }

            if (guid == MediaSubType.DVCS)
            {
                return "DVCS";
            }

            if (guid == MediaSubType.DVSD)
            {
                return "DVSD";
            }

            if (guid == MediaSubType.MDVF)
            {
                return "MDVF";
            }

            if (guid == MediaSubType.RGB1)
            {
                return "RGB1";
            }

            if (guid == MediaSubType.RGB4)
            {
                return "RGB4";
            }

            if (guid == MediaSubType.RGB8)
            {
                return "RGB8";
            }

            if (guid == MediaSubType.RGB565)
            {
                return "RGB565";
            }

            if (guid == MediaSubType.RGB555)
            {
                return "RGB555";
            }

            if (guid == MediaSubType.RGB24)
            {
                return "RGB24";
            }

            if (guid == MediaSubType.RGB32)
            {
                return "RGB32";
            }

            if (guid == MediaSubType.ARGB32)
            {
                return "ARGB32";
            }

            if (guid == MediaSubType.Overlay)
            {
                return "Overlay";
            }

            if (guid == MediaSubType.MPEG1Packet)
            {
                return "MPEG1Packet";
            }

            if (guid == MediaSubType.MPEG1Payload)
            {
                return "MPEG1Payload";
            }

            if (guid == MediaSubType.MPEG1AudioPayload)
            {
                return "MPEG1AudioPayload";
            }

            if (guid == MediaSubType.MPEG1System)
            {
                return "MPEG1System";
            }

            if (guid == MediaSubType.MPEG1VideoCD)
            {
                return "MPEG1VideoCD";
            }

            if (guid == MediaSubType.MPEG1Video)
            {
                return "MPEG1Video";
            }

            if (guid == MediaSubType.MPEG1Audio)
            {
                return "MPEG1Audio";
            }

            if (guid == MediaSubType.Avi)
            {
                return "AVI";
            }

            if (guid == MediaSubType.Asf)
            {
                return "ASF";
            }

            if (guid == MediaSubType.QTMovie)
            {
                return "QTMovie";
            }

            if (guid == MediaSubType.QTRpza)
            {
                return "QTRpza";
            }

            if (guid == MediaSubType.QTSmc)
            {
                return "QTSmc";
            }

            if (guid == MediaSubType.QTRle)
            {
                return "QTRle";
            }

            if (guid == MediaSubType.QTJpeg)
            {
                return "QTJpeg";
            }

            if (guid == MediaSubType.PCMAudio_Obsolete)
            {
                return "PCMAudio_Obsolete";
            }

            if (guid == MediaSubType.PCM)
            {
                return "PCM";
            }

            if (guid == MediaSubType.WAVE)
            {
                return "WAVE";
            }

            if (guid == MediaSubType.AU)
            {
                return "AU";
            }

            if (guid == MediaSubType.AIFF)
            {
                return "AIFF";
            }

            if (guid == MediaSubType.DVSD)
            {
                return "DVSD";
            }

            if (guid == MediaSubType.dvhd)
            {
                return "DVHD";
            }

            if (guid == MediaSubType.dvsl)
            {
                return "DVSL";
            }

            if (guid == MediaSubType.Line21_BytePair)
            {
                return "Line21_BytePair";
            }

            if (guid == MediaSubType.Line21_GOPPacket)
            {
                return "Line21_GOPPacket";
            }

            if (guid == MediaSubType.Line21_VBIRawData)
            {
                return "Line21_VBIRawData";
            }

            if (guid == MediaSubType.DRM_Audio)
            {
                return "DRM_Audio";
            }

            if (guid == MediaSubType.IEEE_FLOAT)
            {
                return "IEEE_FLOAT";
            }

            if (guid == MediaSubType.DOLBY_AC3_SPDIF)
            {
                return "DOLBY_AC3_SPDIF";
            }

            if (guid == MediaSubType.RAW_SPORT)
            {
                return "RAW_SPORT";
            }

            if (guid == MediaSubType.SPDIF_TAG_241h)
            {
                return "SPDIF_TAG_241h";
            }

            if (guid == MediaSubType.DssVideo)
            {
                return "DssVideo";
            }

            if (guid == MediaSubType.DssAudio)
            {
                return "DssAudio";
            }

            if (guid == MediaSubType.VPVideo)
            {
                return "VPVideo";
            }

            if (guid == MediaSubType.VPVBI)
            {
                return "VPVBI";
            }

            if (guid == MediaSubType.AnalogVideo_NTSC_M)
            {
                return "AnalogVideo_NTSC_M";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_B)
            {
                return "AnalogVideo_PAL_B";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_D)
            {
                return "AnalogVideo_PAL_D";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_G)
            {
                return "AnalogVideo_PAL_G";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_H)
            {
                return "AnalogVideo_PAL_H";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_I)
            {
                return "AnalogVideo_PAL_I";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_M)
            {
                return "AnalogVideo_PAL_M";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_N)
            {
                return "AnalogVideo_PAL_N";
            }

            if (guid == MediaSubType.AnalogVideo_PAL_N_COMBO)
            {
                return "AnalogVideo_PAL_N_COMBO";
            }

            if (guid == MediaSubType.AnalogVideo_SECAM_B)
            {
                return "AnalogVideo_SECAM_B";
            }

            if (guid == MediaSubType.AnalogVideo_SECAM_D)
            {
                return "AnalogVideo_SECAM_D";
            }

            if (guid == MediaSubType.AnalogVideo_SECAM_G)
            {
                return "AnalogVideo_SECAM_G";
            }

            if (guid == MediaSubType.AnalogVideo_SECAM_H)
            {
                return "AnalogVideo_SECAM_H";
            }

            if (guid == MediaSubType.AnalogVideo_SECAM_K)
            {
                return "AnalogVideo_SECAM_K";
            }

            if (guid == MediaSubType.AnalogVideo_SECAM_K1)
            {
                return "AnalogVideo_SECAM_K1";
            }

            if (guid == MediaSubType.AnalogVideo_SECAM_L)
            {
                return "AnalogVideo_SECAM_L";
            }

            if (guid == MediaSubType.Mpeg2Video)
            {
                return "MPEG2_VIDEO";
            }

            if (guid == MediaSubType.Mpeg2Program)
            {
                return "MPEG2_PROGRAM";
            }

            if (guid == MediaSubType.Mpeg2Transport)
            {
                return "MPEG2_TRANSPORT";
            }

            if (guid == MediaSubType.Mpeg2Audio)
            {
                return "MPEG2_AUDIO";
            }

            if (guid == MediaSubType.DolbyAC3)
            {
                return "DOLBY_AC3";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_DVD_SUBPICTURE))
            {
                return "DVD_SUBPICTURE";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_DVD_LPCM_AUDIO))
            {
                return "DVD_LPCM_AUDIO";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_DTS))
            {
                return "DTS";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_SDDS))
            {
                return "SDDS";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_DVD_NAVIGATION_PCI))
            {
                return "PCI";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_DVD_NAVIGATION_DSI))
            {
                return "DSI";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_DVD_NAVIGATION_PROVIDER))
            {
                return "PROVIDER";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_MP42))
            {
                return "MS-MPEG4";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_DIVX))
            {
                return "DIVX";
            }

            if (guid == new Guid(DSConsts.MEDIASUBTYPE_VOXWARE))
            {
                return "VOXWARE_MetaSound";
            }

            if (guid == MediaSubType.VPVBI)
            {
                return "VPVBI";
            }

            return "Unknown: " + guid;
        }

        ///// <summary>
        ///// Gets media type description.
        ///// </summary>
        ///// <param name="mediaType">
        ///// Media type.
        ///// </param>
        ///// <returns>
        ///// Returns media type description.
        ///// </returns>
        //public static string GetMediaTypeDescription(this AMMediaType mediaType)
        //{
        //    string result = string.Empty;

        //    if (mediaType != null)
        //    {
        //        if (mediaType.formatType == FormatType.VideoInfo)
        //        {
        //            if (mediaType.formatSize > 0)
        //            {
        //                VideoInfoHeader header =
        //                    (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));

        //                double fr = 0;
        //                if (header.AvgTimePerFrame > 0)
        //                {
        //                    fr = 10000000.0 / header.AvgTimePerFrame;
        //                }

        //                result += header.BmiHeader.Width + "x" + header.BmiHeader.Height + " "
        //                          + GetFOURCC(header.BmiHeader.Compression) + ", " + header.BmiHeader.BitCount + " bit, Frame rate: " + fr.ToString("F2");
        //            }
        //        }
        //        else if (mediaType.formatType == FormatType.VideoInfo2)
        //        {
        //            if (mediaType.formatSize > 0)
        //            {
        //                VideoInfoHeader2 header =
        //                    (VideoInfoHeader2)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader2));

        //                double fr = 0;
        //                if (header.AvgTimePerFrame > 0)
        //                {
        //                    fr = 10000000.0 / header.AvgTimePerFrame;
        //                }

        //                result += header.BmiHeader.Width + "x" + header.BmiHeader.Height + " "
        //                          + GetFOURCC(header.BmiHeader.Compression) + ", " + header.BmiHeader.BitCount + " bit, Frame rate: " + fr.ToString("F2");
        //            }
        //        }
        //        else if (mediaType.formatType == FormatType.WaveEx)
        //        {
        //            if (mediaType.formatSize > 0)
        //            {
        //                WaveFormatEx header =
        //                    (WaveFormatEx)Marshal.PtrToStructure(mediaType.formatPtr, typeof(WaveFormatEx));

        //                result += AudioTagToString(header.wFormatTag);
        //                result += ", " + header.nSamplesPerSec + " Hz, " + header.wBitsPerSample + " Bits, "
        //                          + header.nChannels + " Channels";
        //            }
        //        }
        //        else if (mediaType.formatType == FormatType.MpegVideo)
        //        {
        //            result += "MPEG1 ";
        //            if (mediaType.formatSize > 0)
        //            {
        //                MPEG1VideoInfo header =
        //                    (MPEG1VideoInfo)Marshal.PtrToStructure(mediaType.formatPtr, typeof(MPEG1VideoInfo));

        //                result += ", " + header.hdr.BmiHeader.Width + "x" + header.hdr.BmiHeader.Height + " "
        //                          + GetFOURCC(header.hdr.BmiHeader.Compression) + ", " + header.hdr.BmiHeader.BitCount
        //                          + " bit";
        //            }
        //        }
        //        else if (mediaType.formatType == FormatType.Mpeg2Video)
        //        {
        //            result += "MPEG2 ";
        //            if (mediaType.formatSize > 0)
        //            {
        //                MPEG2VideoInfo header =
        //                    (MPEG2VideoInfo)Marshal.PtrToStructure(mediaType.formatPtr, typeof(MPEG2VideoInfo));

        //                result += ", " + header.HDR.BmiHeader.Width + "x" + header.HDR.BmiHeader.Height + " "
        //                          + GetFOURCC(header.HDR.BmiHeader.Compression) + ", " + header.HDR.BmiHeader.BitCount
        //                          + " bit";
        //            }
        //        }
        //        else if (mediaType.formatType == FormatType.DvInfo)
        //        {
        //            result = result + "DV";
        //        }
        //        else if (mediaType.formatType == FormatType.MpegStreams)
        //        {
        //            result = result + "MPEGStreams";
        //        }
        //        else if (mediaType.formatType == FormatType.DolbyAC3)
        //        {
        //            result = result + "DolbyAC3";
        //        }
        //        else if (mediaType.formatType == FormatType.Mpeg2Audio)
        //        {
        //            result = result + "MPEG2 Audio";
        //        }
        //        else
        //        {
        //            result = "MajorType: " + MajorTypeToString(mediaType.majorType);
        //            result = result + ", SubType: " + SubTypeToString(mediaType.subType);
        //            result = result + ", FormatType: " + FormatTypeToString(mediaType.formatType);
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Gets media type description.
        /// </summary>
        /// <param name="mediaType">
        /// Media type.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="frameRate">
        /// Frame rate.
        /// </param>
        public static void GetMediaTypeInfo(this AMMediaType mediaType, out int width, out int height, out double frameRate)
        {
            width = 0;
            height = 0;
            frameRate = 0;

            if (mediaType != null)
            {
                if (mediaType.formatType == FormatType.VideoInfo)
                {
                    if (mediaType.formatSize > 0)
                    {
                        VideoInfoHeader header =
                            (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));

                        if (header.AvgTimePerFrame > 0)
                        {
                            frameRate = 10000000.0 / header.AvgTimePerFrame;
                        }

                        width = header.BmiHeader.Width;
                        height = header.BmiHeader.Height;
                    }
                }
                else if (mediaType.formatType == FormatType.VideoInfo2)
                {
                    if (mediaType.formatSize > 0)
                    {
                        VideoInfoHeader2 header =
                            (VideoInfoHeader2)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader2));

                        if (header.AvgTimePerFrame > 0)
                        {
                            frameRate = 10000000.0 / header.AvgTimePerFrame;
                        }

                        width = header.BmiHeader.Width;
                        height = header.BmiHeader.Height;
                    }
                }
                //else if (mediaType.formatType == FormatType.MpegVideo)
                //{
                //    if (mediaType.formatSize > 0)
                //    {
                //        MPEG1VideoInfo header =
                //            (MPEG1VideoInfo)Marshal.PtrToStructure(mediaType.formatPtr, typeof(MPEG1VideoInfo));

                //        if (header.hdr.AvgTimePerFrame > 0)
                //        {
                //            frameRate = 10000000.0 / header.hdr.AvgTimePerFrame;
                //        }

                //        width = header.hdr.BmiHeader.Width;
                //        height = header.hdr.BmiHeader.Height;
                //    }
                //}
                else if (mediaType.formatType == FormatType.Mpeg2Video)
                {
                    if (mediaType.formatSize > 0)
                    {
                        MPEG2VideoInfo header =
                            (MPEG2VideoInfo)Marshal.PtrToStructure(mediaType.formatPtr, typeof(MPEG2VideoInfo));

                        if (header.HDR.AvgTimePerFrame > 0)
                        {
                            frameRate = 10000000.0 / header.HDR.AvgTimePerFrame;
                        }

                        width = header.HDR.BmiHeader.Width;
                        height = header.HDR.BmiHeader.Height;
                    }
                }
            }
        }

        /// <summary>
        /// Converts format type to string.
        /// </summary>
        /// <param name="guid">
        /// Format type.
        /// </param>
        /// <returns>
        /// Returns format type as string.
        /// </returns>
        public static string FormatTypeToString(Guid guid)
        {
            // sub types
            if (guid == FormatType.None)
            {
                return "None";
            }

            if (guid == FormatType.VideoInfo)
            {
                return "VideoInfo";
            }

            if (guid == FormatType.VideoInfo2)
            {
                return "VideoInfo2";
            }

            if (guid == FormatType.WaveEx)
            {
                return "WaveFormatEx";
            }

            if (guid == FormatType.MpegVideo)
            {
                return "MPEG Video";
            }

            if (guid == FormatType.MpegStreams)
            {
                return "MPEG Streams";
            }

            if (guid == FormatType.DvInfo)
            {
                return "DV Info";
            }

            if (guid == FormatType.AnalogVideo)
            {
                return "AnalogVideo";
            }

            if (guid == FormatType.Mpeg2Video)
            {
                return "MPEG-2 Video";
            }

            if (guid == FormatType.DolbyAC3)
            {
                return "Dolby AC3";
            }

            if (guid == FormatType.Mpeg2Audio)
            {
                return "MPEG-2 Audio";
            }

            if (guid == new Guid(DSConsts.FORMAT_DVD_LPCMAudio))
            {
                return "DVD LPCM Audio";
            }

            return "Unknown: " + guid;
        }

        /// <summary>
        /// Gets FourCC code as string.
        /// </summary>
        /// <param name="fourCC">
        /// FourCC code.
        /// </param>
        /// <returns>
        /// Returns FourCC code as string.
        /// </returns>
        public static string GetFOURCC(uint fourCC)
        {
            switch (fourCC)
            {
                case 0:
                    return "RGB";
                case 1:
                    return DSConsts.FCC_RLE8;
                case 2:
                    return DSConsts.FCC_RLE4;
                case 3:
                    return DSConsts.FCC_BITFIELDS;
                default:
                    byte[] asc = BitConverter.GetBytes(fourCC);
                    return Encoding.ASCII.GetString(asc);
            }
        }

        /// <summary>
        /// Gets FourCC code as string.
        /// </summary>
        /// <param name="fourCC">
        /// FourCC code.
        /// </param>
        /// <returns>
        /// Returns FourCC code as string.
        /// </returns>
        public static string GetFOURCC(int fourCC)
        {
            switch (fourCC)
            {
                case 0:
                    return "RGB";
                case 1:
                    return DSConsts.FCC_RLE8;
                case 2:
                    return DSConsts.FCC_RLE4;
                case 3:
                    return DSConsts.FCC_BITFIELDS;
                default:
                    byte[] asc = BitConverter.GetBytes(fourCC);
                    return Encoding.ASCII.GetString(asc);
            }
        }

        /// <summary>
        /// Gets FourCC code.
        /// </summary>
        /// <param name="fourCC">
        /// FourCC code as string.
        /// </param>
        /// <returns>
        /// Returns FourCC code.
        /// </returns>
        [Localizable(false)]
        public static int MakeFOURCC(string fourCC)
        {
            byte[] code = Encoding.ASCII.GetBytes(fourCC);
            return BitConverter.ToInt32(code, 0);
        }

        /// <summary>
        /// Gets FourCC code.
        /// </summary>
        /// <param name="fourCC">
        /// FourCC code as string.
        /// </param>
        /// <returns>
        /// Returns FourCC code.
        /// </returns>
        public static uint MakeFOURCCU(string fourCC)
        {
            byte[] code = Encoding.ASCII.GetBytes(fourCC);
            return BitConverter.ToUInt32(code, 0);
        }

        /// <summary>
        /// Converts audio tags to string.
        /// </summary>
        /// <param name="code">
        /// Audio tag code.
        /// </param>
        /// <returns>
        /// Returns string from audio tag.
        /// </returns>
        public static string AudioTagToString(int code)
        {
            string result;

            switch (code)
            {
                case 0x0001:
                    result = "PCM";
                    break;
                case 0x0002:
                    // common
                    result = "ADPCM";
                    break;
                case 0x0003:
                    result = "IEEE_FLOAT";
                    break;
                case 0x0005:
                    result = "IBM_CVSD";
                    break;
                case 0x0006:
                    result = "ALAW";
                    break;
                case 0x0007:
                    result = "MULAW";
                    break;
                case 0x0010:
                    result = "OKI_ADPCM";
                    break;
                case 0x0011:
                    result = "DVI_ADPCM";
                    break;
                case 0x0012:
                    result = "MEDIASPACE_ADPCM";
                    break;
                case 0x0013:
                    result = "SIERRA_ADPCM";
                    break;
                case 0x0014:
                    result = "G723_ADPCM";
                    break;
                case 0x0015:
                    result = "DIGISTD";
                    break;
                case 0x0016:
                    result = "DIGIFIX";
                    break;
                case 0x0017:
                    result = "DIALOGIC_OKI_ADPCM";
                    break;
                case 0x0018:
                    result = "MEDIAVISION_ADPCM";
                    break;
                case 0x0020:
                    result = "YAMAHA_ADPCM";
                    break;
                case 0x0021:
                    result = "SONARC";
                    break;
                case 0x0022:
                    result = "DSPGROUP_TRUESPEECH";
                    break;
                case 0x0023:
                    result = "ECHOSC1";
                    break;
                case 0x0024:
                    result = "AUDIOFILE_AF36";
                    break;
                case 0x0025:
                    result = "APTX";
                    break;
                case 0x0026:
                    result = "AUDIOFILE_AF10";
                    break;
                case 0x0030:
                    result = "DOLBY_AC2";
                    break;
                case 0x0031:
                    result = "GSM610";
                    break;
                case 0x0032:
                    result = "MSNAUDIO";
                    break;
                case 0x0033:
                    result = "ANTEX_ADPCME";
                    break;
                case 0x0034:
                    result = "CONTROL_RES_VQLPC";
                    break;
                case 0x0035:
                    result = "DIGIREAL";
                    break;
                case 0x0036:
                    result = "DIGIADPCM";
                    break;
                case 0x0037:
                    result = "CONTROL_RES_CR10";
                    break;
                case 0x0038:
                    result = "NMS_VBXADPCM";
                    break;
                case 0x0039:
                    result = "CS_IMAADPCM";
                    break;
                case 0x003A:
                    result = "ECHOSC3";
                    break;
                case 0x003B:
                    result = "ROCKWELL_ADPCM";
                    break;
                case 0x003C:
                    result = "ROCKWELL_DIGITALK";
                    break;
                case 0x003D:
                    result = "XEBEC";
                    break;
                case 0x0040:
                    result = "G721_ADPCM";
                    break;
                case 0x0041:
                    result = "G728_CELP";
                    break;
                case 0x0050:
                    result = "MPEG";
                    break;
                case 0x0055:
                    result = "MPEGLAYER3";
                    break;
                case 0x0060:
                    result = "CIRRUS";
                    break;
                case 0x0061:
                    result = "ESPCM";
                    break;
                case 0x0062:
                    result = "VOXWARE";
                    break;
                case 0x0063:
                    result = "CANOPUS_ATRAC";
                    break;
                case 0x0064:
                    result = "G726_ADPCM";
                    break;
                case 0x0065:
                    result = "G722_ADPCM";
                    break;
                case 0x0066:
                    result = "DSAT";
                    break;
                case 0x0067:
                    result = "DSAT_DISPLAY";
                    break;
                case 0x0075:
                    result = "VOXWARE";
                    break;
                case 0x0080:
                    result = "SOFTSOUND";
                    break;
                case 0x0100:
                    result = "RHETOREX_ADPCM";
                    break;
                case 0x0200:
                    result = "CREATIVE_ADPCM";
                    break;
                case 0x0202:
                    result = "CREATIVE_FASTSPEECH8";
                    break;
                case 0x0203:
                    result = "CREATIVE_FASTSPEECH10";
                    break;
                case 0x0220:
                    result = "QUARTERDECK";
                    break;
                case 0x0300:
                    result = "FM_TOWNS_SND";
                    break;
                case 0x0400:
                    result = "BTV_DIGITAL";
                    break;
                case 0x1000:
                    result = "OLIGSM";
                    break;
                case 0x1001:
                    result = "OLIADPCM";
                    break;
                case 0x1002:
                    result = "OLICELP";
                    break;
                case 0x1003:
                    result = "OLISBC";
                    break;
                case 0x1004:
                    result = "OLIOPR";
                    break;
                case 0x1100:
                    result = "LH_CODEC";
                    break;
                case 0x1400:
                    result = "NORRIS";
                    break;
                default:
                    result = "Unknown";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets media type description.
        /// </summary>
        /// <param name="format">
        /// Video capture format.
        /// </param>
        /// <returns>
        /// Returns media type description.
        /// </returns>
        public static string GetMediaTypeDescription(VFVideoCaptureFormat format)
        {
            string result = string.Empty;

            if (format != null)
            {
                if (format.FormatType == FormatType.VideoInfo)
                {
                    result += format.Width + "x" + format.Height + " " + GetFOURCC(format.VIH.BmiHeader.Compression)
                              + ", " + format.VIH.BmiHeader.BitCount + " bit";
                }
                else if (format.FormatType == FormatType.VideoInfo2)
                {
                    result += format.Width + "x" + format.Height + " " + GetFOURCC(format.VIH2.BmiHeader.Compression)
                              + ", " + format.VIH2.BmiHeader.BitCount + " bit";
                }
                //else if (format.FormatType == FormatType.MpegVideo)
                //{
                //    result += "MPEG1 ";

                //    result += ", " + format.Width + "x" + format.Height + " "
                //              + GetFOURCC(format.MPEG1.hdr.BmiHeader.Compression) + ", "
                //              + format.MPEG1.hdr.BmiHeader.BitCount + " bit";
                //}
                else if (format.FormatType == FormatType.Mpeg2Video)
                {
                    result += ", " + format.MPEG2.HDR.BmiHeader.Width + "x" + format.MPEG2.HDR.BmiHeader.Height + " "
                              + GetFOURCC(format.MPEG2.HDR.BmiHeader.Compression) + ", "
                              + format.MPEG2.HDR.BmiHeader.BitCount + " bit";
                }
                else if (format.FormatType == FormatType.DvInfo)
                {
                    result = result + "DV";
                }
                else if (format.FormatType == FormatType.MpegStreams)
                {
                    result = result + "MPEGStreams";
                }
                else if (format.FormatType == FormatType.DolbyAC3)
                {
                    result = result + "DolbyAC3";
                }
                else if (format.FormatType == FormatType.Mpeg2Audio)
                {
                    result = result + "MPEG2 Audio";
                }
                else
                {
                    result = "MajorType: " + MajorTypeToString(format.MajorType);
                    result = result + ", SubType: " + SubTypeToString(format.SubType);
                    result = result + ", FormatType: " + FormatTypeToString(format.FormatType);
                }
            }

            return result;
        }
    }
}
