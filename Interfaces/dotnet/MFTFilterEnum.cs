using MediaFoundation;
using MediaFoundation.Misc;
using MediaFoundation.Transform;

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class MFTEncoders
    {
        public List<string> H264_HW_Encoders;

        public List<string> H265_HW_Encoders;

        public List<string> H264_SW_Encoders;

        public List<string> H265_SW_Encoders;

        public MFTEncoders()
        {
            H264_HW_Encoders = new List<string>();
            H265_HW_Encoders = new List<string>();
            H264_SW_Encoders = new List<string>();
            H265_SW_Encoders = new List<string>();
        }
    }

    /// <summary>
    /// Filters available information.
    /// </summary>
    public struct FiltersAvailableInfo
    {
        /// <summary>
        /// H264 CPU encoder.
        /// </summary>
        public bool H264_CPU;

        /// <summary>
        /// H264 Intel QuickSync GPU encoder.
        /// </summary>
        public bool QSV_H264;

        /// <summary>
        /// H265 Intel QuickSync GPU encoder.
        /// </summary>
        public bool QSV_H265;

        /// <summary>
        /// H264 nVidia NVENC GPU encoder.
        /// </summary>
        public bool NVENC_H264;

        /// <summary>
        /// H265 nVidia NVENC GPU encoder.
        /// </summary>
        public bool NVENC_H265;

        /// <summary>
        /// AAC encoder.
        /// </summary>
        public bool AAC;

        /// <summary>
        /// H264 AMD GPU encoder.
        /// </summary>
        public bool AMD_H264;

        /// <summary>
        /// H265 AMD GPU encoder.
        /// </summary>
        public bool AMD_H265;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = string.Empty;
            if (H264_CPU)
            {
                str += "H264 CPU, ";
            }

            if (QSV_H264)
            {
                str += "QSV H264, ";
            }

            if (QSV_H265)
            {
                str += "QSV H265, ";
            }

            if (NVENC_H264)
            {
                str += "NVENC H264, ";
            }

            if (NVENC_H265)
            {
                str += "NVENC H265, ";
            }

            if (AMD_H265)
            {
                str += "AMD H265, ";
            }

            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            str = str.Trim().Remove(str.Length - 2, 1);
            return str.Trim();
        }
    }

    public static class MFTFilterEnum
    {
        public static FiltersAvailableInfo GetFiltersAvailable(out MFTEncoders encoders)
        {
            var info = new FiltersAvailableInfo();

            GetEncodersAvailable(ref info, out encoders);

            return info;
        }

        private static HResult GetMFTNames(
            bool bEncoder,
            bool hw,
            Guid mediaType,
            Guid inputFormat,
            Guid outputFormat,
            ref List<string> names)
        {
            Debug.Assert(mediaType == MFMediaType.Video || mediaType == MFMediaType.Audio); // only audio and video codecs are support for now

            HResult hr = 0;

            //bool bAsync = false;
            //Guid guidActivateCLSID = Guid.Empty;

            MFTRegisterTypeInfo infoInput = new MFTRegisterTypeInfo() { guidMajorType = mediaType, guidSubtype = inputFormat };
            MFTRegisterTypeInfo infoOutput = new MFTRegisterTypeInfo() { guidMajorType = mediaType, guidSubtype = outputFormat };

            MFT_EnumFlag unFlags = MFT_EnumFlag.LocalMFT |
                           MFT_EnumFlag.TranscodeOnly | // Otherwise Intel Quick Sync will not be listed
                           MFT_EnumFlag.SortAndFilter;

            if (hw)
            {
                unFlags |= MFT_EnumFlag.Hardware;
            }
            else
            {
                unFlags |= MFT_EnumFlag.SyncMFT | MFT_EnumFlag.AsyncMFT;
            }

            Guid category = (mediaType == MFMediaType.Video)
                                ? (bEncoder
                                       ? MFTransformCategory.MFT_CATEGORY_VIDEO_ENCODER
                                       : MFTransformCategory.MFT_CATEGORY_VIDEO_DECODER)
                                : (bEncoder
                                       ? MFTransformCategory.MFT_CATEGORY_AUDIO_ENCODER
                                       : MFTransformCategory.MFT_CATEGORY_AUDIO_DECODER);
            hr = MFExtern.MFTEnumEx(
                category,
                unFlags,
                (inputFormat == Guid.Empty) ? null : infoInput,      // Input type
                (outputFormat == Guid.Empty) ? null : infoOutput,       // Output type
                out var ppActivate,
                out var count);

            for (uint i = 0; i < count; ++i)
            {
                hr = ppActivate[i].GetAllocatedString(
                    MFAttributesClsid.MFT_FRIENDLY_NAME_Attribute,
                    out string name,
                    out int length);

                if (hr == HResult.S_OK && !string.IsNullOrEmpty(name))
                {
                    names.Add(name);
                }
            }

            for (uint i = 0; i < count; i++)
            {
                COMBase.SafeRelease(ppActivate[i]);
            }

            ppActivate = null;

            return 0;
        }


        public static void GetEncodersAvailable(ref FiltersAvailableInfo info, out MFTEncoders encoders)
        {
            encoders = new MFTEncoders();

            info.AAC = true;
            info.H264_CPU = true;

            GetMFTNames(true, true, MFMediaType.Video, MFMediaType.NV12, MFMediaType.H264, ref encoders.H264_HW_Encoders);

            foreach (var name in encoders.H264_HW_Encoders)
            {
                if (name.Contains("Intel") && name.Contains("H.264 Encoder"))
                {
                    info.QSV_H264 = true;
                    continue;
                }

                if (name.Contains("NVIDIA") && name.Contains("H.264 Encoder"))
                {
                    info.NVENC_H264 = true;
                    continue;
                }

                if (name.Contains("AMD") && name.ToUpperInvariant().Contains("H264"))
                {
                    info.AMD_H264 = true;
                    continue;
                }
            }

            GetMFTNames(true, true, MFMediaType.Video, Guid.Empty, MFMediaType.H265, ref encoders.H265_HW_Encoders);
            GetMFTNames(true, true, MFMediaType.Video, Guid.Empty, MFMediaType.HEVC, ref encoders.H265_HW_Encoders);

            foreach (var name in encoders.H265_HW_Encoders)
            {
                if (name.Contains("Intel") && name.Contains("H.265 Encoder"))
                {
                    info.QSV_H264 = true;
                    continue;
                }

                if (name.Contains("NVIDIA") && name.Contains("HEVC Encoder"))
                {
                    info.NVENC_H264 = true;
                    continue;
                }

                if (name.Contains("AMD") && name.ToUpperInvariant().Contains("H265"))
                {
                    info.AMD_H265 = true;
                    continue;
                }
            }

            GetMFTNames(true, false, MFMediaType.Video, Guid.Empty, MFMediaType.H265, ref encoders.H265_SW_Encoders);
            GetMFTNames(true, false, MFMediaType.Video, Guid.Empty, MFMediaType.HEVC, ref encoders.H265_SW_Encoders);
            GetMFTNames(true, false, MFMediaType.Video, MFMediaType.NV12, MFMediaType.H264, ref encoders.H264_SW_Encoders);
        }
    }
}
