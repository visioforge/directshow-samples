// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DSFilterInitInfoConsts.cs" company="VisioForge">
//   VisioForge (c) 2006 - 2021
// </copyright>
// <summary>
//   DSFilterInitInfoConsts class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Filters initialization info DB.
    /// </summary>
    public static class DSFilterInitInfoConsts
    {
        public static readonly DSFilterInitInfo VFWebMSource = new DSFilterInitInfo(
            "193F7741-E789-4D2F-8546-C132E2E80FB8",
            "VisioForge WebM Source",
            "VisioForge_WebM_Source.ax", 
            string.Empty);

        public static readonly DSFilterInitInfo VFWebMVorbisEncoder = new DSFilterInitInfo(
            "F86DC6FF-66F2-4B42-BE37-2EFE9DD4F549",
            "VisioForge WebM Vorbis Encoder", 
            "VisioForge_WebM_Vorbis_Encoder.ax",
            string.Empty);

        public static readonly DSFilterInitInfo VFWebmSplit = new DSFilterInitInfo(
            "A24F64C4-E527-49AD-A150-11C83DBE486C", 
            "VisioForge WebM Splitter", 
            "VisioForge_WebM_Split.ax", 
            string.Empty);

        public static readonly DSFilterInitInfo VFWebMVorbisDecoder = new DSFilterInitInfo(
            "4BBE1D6F-CB80-4211-AD00-6A0555EB474F", 
            "VisioForge WebM Vorbis Decoder", 
            "VisioForge_WebM_Vorbis_Decoder.ax",
            string.Empty);

        public static readonly DSFilterInitInfo VFWebMVP8Encoder = new DSFilterInitInfo(
            "7B2B9E53-F65D-4967-BE13-66295E90382E", 
            "VisioForge WebM VP8 Encoder", 
            "VisioForge_WebM_VP8_Encoder.ax",
            string.Empty);

        public static readonly DSFilterInitInfo VFWebMVP8Decoder = new DSFilterInitInfo(
            "4C33B713-07EE-4954-AF82-5DE369B2978C",
            "VisioForge WebM VP8 Decoder",
            "VisioForge_WebM_VP8_Decoder.ax", 
            string.Empty);

        public static readonly DSFilterInitInfo VFWebmMuxer = new DSFilterInitInfo(
            "25908CCA-6ED3-4FC8-A358-6D30398D15BF",
            "VisioForge WebM Muxer", 
            "VisioForge_WebM_Mux.ax",
            string.Empty);

        public static readonly DSFilterInitInfo VFXiphVorbisDecoder = new DSFilterInitInfo(
            "0FE80394-4F9D-48FD-B242-5B9F7FEEC166", 
            "VisioForge Xiph Vorbis Decoder",
            "VisioForge_Xiph_Vorbis_Decoder.ax",
            "VisioForge_Xiph_Vorbis_Decoder_x64.ax");

        public static readonly DSFilterInitInfo VFXiphSpeexEncoder = new DSFilterInitInfo(
            "D736AFA5-87CA-49C4-BDD5-31E36F02B202", 
            "VisioForge Xiph Speex Encoder",
            "VisioForge_Xiph_Speex_Encoder.ax",
            "VisioForge_Xiph_Speex_Encoder_x64.ax");

        public static readonly DSFilterInitInfo VFXiphSpeexDecoder = new DSFilterInitInfo(
            "CD598410-0DA1-4ADD-82B1-59C216DDFD77",
            "VisioForge Xiph Speex Decoder",
            "VisioForge_Xiph_Speex_Decoder.ax",
            "VisioForge_Xiph_Speex_Decoder_x64.ax");

        public static readonly DSFilterInitInfo VFXiphOGGMux = new DSFilterInitInfo(
            "8B74A505-3836-43A3-A2DE-91813CEFE94C",
            "VisioForge Xiph Ogg Muxer",
            "VisioForge_Xiph_Ogg_Mux.ax", 
            "VisioForge_Xiph_Ogg_Mux_x64.ax");

        public static readonly DSFilterInitInfo VFXiphOGGDemux2 = new DSFilterInitInfo(
            "5506AF15-04E2-46C3-BF25-D5886EACA88B",
            "VisioForge Xiph Ogg Demux 2",
            "VisioForge_Xiph_Ogg_Demux2.ax", 
            "VisioForge_Xiph_Ogg_Demux2_x64.ax");

        public static readonly DSFilterInitInfo VFXiphFLACEncoder = new DSFilterInitInfo(
            "BE5B270D-73AA-4368-AA56-BFBC870B52CF", 
            "VisioForge Xiph FLAC Encoder", 
            "VisioForge_Xiph_FLAC_Encoder.ax", 
            "VisioForge_Xiph_FLAC_Encoder_x64.ax");

        public static readonly DSFilterInitInfo VFXiphFLACSource = new DSFilterInitInfo(
            "A648D164-A8BD-48A8-A4B2-5E1442260EF0", 
            "VisioForge Xiph FLAC Source", 
            "VisioForge_Xiph_FLAC_Source.ax", 
            "VisioForge_Xiph_FLAC_Source_x64.ax");

        public static readonly DSFilterInitInfo VFXiphVorbisEncoder = new DSFilterInitInfo(
            "5F06C018-3968-4775-BD8C-A6E8EF766C7D", 
            "VisioForge Xiph Vorbis Encoder", 
            "VisioForge_Xiph_Vorbis_Encoder.ax",
            "VisioForge_Xiph_Vorbis_Encoder_x64.ax");

        // v10
        public static readonly DSFilterInitInfo VFH264EncoderV10 = new DSFilterInitInfo(
            "EA1FED6B-B876-4DB0-B7B1-778463E59978",
            "VisioForge H264 Encoder v10",
            "VisioForge_H264_Encoder.ax",
            "VisioForge_H264_Encoder_x64.ax");

        public static readonly DSFilterInitInfo VFAACEncoderV10 = new DSFilterInitInfo(
            "763CAC70-373C-4892-898B-AC80661B15F3", 
            "VisioForge AAC Encoder v10",
            "VisioForge_AAC_Encoder_v10.ax", 
            "VisioForge_AAC_Encoder_v10_x64.ax");

        public static readonly DSFilterInitInfo VFMP4DestV10 = new DSFilterInitInfo(
            "0B0D654C-7AC1-441E-9C4D-3C29ABEDB6A8",
            "VisioForge MP4 Muxer v10",
            "VisioForge_MP4_Muxer_v10.ax", 
            "VisioForge_MP4_Muxer_v10_x64.ax");

        public static readonly DSFilterInitInfo VFH264EncoderNVENC = new DSFilterInitInfo(
            "6eec9161-7276-430b-a197-0d4c3bcc87e5",
            "VisioForge H264 Encoder NVENC",
            string.Empty,
            string.Empty);
        
        public static readonly DSFilterInitInfo VFRTSPSink = new DSFilterInitInfo(
            "57D8EE19-9AF6-4137-BBC0-31A1846EA393",
            "VisioForge RTSP Sink",
            "VisioForge_RTSP_Sink.ax", 
            "VisioForge_RTSP_Sink_x64.ax");

        public static readonly DSFilterInitInfo VFDump = new DSFilterInitInfo(
            "83DF94EE-5A0A-4730-9818-9726CE117CEC", 
            "VisioForge Dump",
            "VisioForge_BaseFilters.ax",
            "VisioForge_BaseFilters_x64.ax");

        public static readonly DSFilterInitInfo VFRGB2YUV = new DSFilterInitInfo(
            "3BDA461E-12DB-4C24-9815-B68D1AA4D34A",
            "VisioForge RGB2YUV",
            "VisioForge_BaseFilters.ax",
            "VisioForge_BaseFilters_x64.ax");

        public static readonly DSFilterInitInfo VFYUV2RGB = new DSFilterInitInfo(
            "CB54D323-9327-49F5-8147-859FE8FAEFF5",
            "VisioForge YUV2RGB",
            "VisioForge_BaseFilters.ax",
            "VisioForge_BaseFilters_x64.ax");

        public static readonly DSFilterInitInfo VFFFMPEGEncoder = new DSFilterInitInfo(
            "69F244AB-31A9-4B74-81CB-CF8C9FACB2EC",
            "VisioForge FFMPEG Encoder", 
            "VisioForge_FFMPEG_Encoder.ax", 
            "VisioForge_FFMPEG_Encoder_x64.ax");
        
        public static readonly DSFilterInitInfo VFWavDest = new DSFilterInitInfo(
            "16EF2357-E074-436d-A37A-20BBE06A5D93", 
            "VisioForge WAV Dest",
            "VisioForge_BaseFilters.ax",
            "VisioForge_BaseFilters_x64.ax");

        public static readonly DSFilterInitInfo LAMEDShowFilter = new DSFilterInitInfo("A3744173-10B0-4F23-B8D7-63A68B20EB53", "LAME Encoder", "VisioForge_LAME.ax", "VisioForge_LAME_x64.ax");

        public static readonly DSFilterInitInfo LAVSplitter = new DSFilterInitInfo("171252A0-8820-4AFE-9DF8-5C92B2D66B04", "LAV Splitter", "LAVSplitter.ax", "LAVSplitter.ax");

        public static readonly DSFilterInitInfo LAVSplitterSource = new DSFilterInitInfo("B98D13E7-55DB-4385-A33D-09FD1BA26338", "LAV Splitter Source", "LAVSplitter.ax", "LAVSplitter.ax");

        public static readonly DSFilterInitInfo LAVVideoDecoder = new DSFilterInitInfo("EE30215D-164F-4A92-A4EB-9D4C13390F9F", "LAV Video Decoder", "LAVVideo.ax", "LAVVideo.ax");

        public static readonly DSFilterInitInfo LAVAudioDecoder = new DSFilterInitInfo("E8E73B6B-4CB3-44A4-BE99-4F7BCB96E491", "LAV Audio Decoder", "LAVAudio.ax", "LAVAudio.ax");

        public static readonly DSFilterInitInfo SampleGrabber = new DSFilterInitInfo("C1F400A0-3F08-11D3-9F0B-006008039E37", "Sample Grabber", string.Empty, string.Empty);

        public static readonly DSFilterInitInfo NullRenderer = new DSFilterInitInfo("C1F400A4-3F08-11D3-9F0B-006008039E37", "Null Renderer", string.Empty, string.Empty);

        public static readonly DSFilterInitInfo VFMFMux = new DSFilterInitInfo("6BBF4BCA-9473-4A69-9BC2-6FF6B3BBB02B", "VisioForge MF Mux", "VisioForge_MF_Mux.ax", "VisioForge_MF_Mux_x64.ax");
    }
}
