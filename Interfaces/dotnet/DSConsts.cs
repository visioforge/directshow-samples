using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Constants.
    /// </summary>
    public static class DSConsts
    {
        // ReSharper disable InconsistentNaming

        public const int VfwCaptureDialog_Source = 0x01;

        public const int VfwCaptureDialog_Format = 0x2;

        public const int VfwCaptureDialog_Display = 0x4;

        public const int VfwCompressDialog_Config = 0x1;

        public const int VfwCompressDialog_About = 0x2;

        public const int VfwCompressDialog_QueryConfig = 0x4;

        public const int VfwCompressDialog_QueryAbout = 0x8;

        public const string MEDIASUBTYPE_MP42 = "3234504D-0000-0010-8000-00AA00389B71";

        public const string MEDIASUBTYPE_DIVX = "58564944-0000-0010-8000-00AA00389B71";

        public const string MEDIASUBTYPE_VOXWARE = "00000075-0000-0010-8000-00AA00389B71";

        public const string MEDIASUBTYPE_MPEG2_TRANSPORT_STRIDE = "138AA9A4-1EE2-4c5b-988E-19ABFDBC8A11";

        public const string FORMAT_MPEG2Video = "E06D80E3-DB46-11CF-B4D1-00805F6CBBEA";

        public const string FORMAT_DolbyAC3 = "E06D80E4-DB46-11CF-B4D1-00805F6CBBEA";

        public const string FORMAT_MPEG2Audio = "E06D80E5-DB46-11CF-B4D1-00805F6CBBEA";

        public const string FORMAT_DVD_LPCMAudio = "E06D80E6-DB46-11CF-B4D1-00805F6CBBEA";

        public const string MEDIASUBTYPE_DVD_SUBPICTURE = "e06d802d-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIASUBTYPE_DVD_LPCM_AUDIO = "e06d8032-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIASUBTYPE_DTS = "e06d8033-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIASUBTYPE_SDDS = "e06d8034-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIATYPE_DVD_ENCRYPTED_PACK = "ED0B916A-044D-11d1-AA78-00C04FC31D60";

        public const string MEDIATYPE_DVD_NAVIGATION = "e06d802e-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIASUBTYPE_DVD_NAVIGATION_PCI = "e06d802f-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIASUBTYPE_DVD_NAVIGATION_DSI = "e06d8030-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIASUBTYPE_DVD_NAVIGATION_PROVIDER = "e06d8031-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIATYPE_MPEG2_PACK = "36523B13-8EE5-11d1-8CA3-0060B057664A";

        public const string MEDIATYPE_MPEG2_PES = "e06d8020-db46-11cf-b4d1-00805f6cbbea";

        public const string MEDIATYPE_MPEG1SystemStream = "e436eb82-524f-11ce-9f53-0020af0ba770";

        public const string IID_IPropertyBag = "55272A00-42CB-11CE-8135-00AA004BB851";

        public const string IID_ISpecifyPropertyPages = "B196B28B-BAB4-101A-B69C-00AA00341D07";

        public const string IID_IPersistStream = "00000109-0000-0000-C000-000000000046";

        public const string IID_IServiceProvider = "6D5140C1-7436-11CE-8034-00AA006009FA";

        public const string IID_IMpeg2PsiParser = "AE1A2884-540E-4077-B1AB-67A34A72298C";

        public const string CLSID_VFPSIParser = "DDF7480E-13E2-4481-BA2B-3C17C4FC469F";

        public const string DSKey = "CLSID\\{083863F1-70DE-11D0-BD40-00A0C911CE86}\\Instance\\";

        public const int PixelCountMax = 32768;

        public const string FIL_AVIDecompressor = "AVI Decompressor";

        public const string FIL_PSIParser = "PSI Parser";

        public const string FIL_PSI = "PSI";

        public const string Deint_BOBLineReplicate = "BOB Line Replicate";

        public const string Deint_BOBVerticalStretch = "BOB Vertical Stretch";

        public const string Deint_MedianFiltering = "Median Filtering";

        public const string Deint_EdgeFiltering = "Edge Filtering";

        public const string Deint_FieldAdaptive = "Field Adaptive";

        public const string Deint_PixelAdaptive = "Pixel Adaptive";

        public const string Deint_MotionVectorSteered = "Motion Vector Steered";

        public const string Deint_UnknownProprietary = "Unknown/Proprietary";

        public const string SFilterData = "FilterData";

        public const string FCC_BITFIELDS = "BITFIELDS";

        public const string FCC_RLE4 = "RLE4";

        public const string FCC_RLE8 = "RLE8";

        public const string FCC_RGB = "RGB";

        public const string PP_DevicePath = "DevicePath";

        public const string PP_FccHandler = "FccHandler";

        public const string PP_Description = "Description";

        public const string PP_CLSID = "CLSID";

        public const string PP_FriendlyName = "FriendlyName";

        public const string SUnknown = "Unknown";

        public const string ER_ENOTDETERMINED = "E_NOTDETERMINED";

        public const string ER_VFWEPINALREADYBLOCKED = "VFW_E_PIN_ALREADY_BLOCKED";

        public const string ER_ENOTIMELINE = "E_NO_TIMELINE";

        public const string ER_ERENDERENGINEISBROKEN = "E_RENDER_ENGINE_IS_BROKEN";

        public const string ER_VFWEDDRAWCAPSNOTSUITABLE = "VFW_E_DDRAW_CAPS_NOT_SUITABLE";

        public const string ER_EMUSTINITRENDERER = "E_MUST_INIT_RENDERER";

        public const string ER_VFWEVMRNOTINMIXERMODE = "VFW_E_VMR_NOT_IN_MIXER_MODE";

        public const string ER_SWARNOUTPUTRESET = "S_WARN_OUTPUTRESET";



        public const string ER_ENOTINTREE = "E_NOTINTREE";



        public const string ER_VFWEVMRNODEINTERLACEHW = "VFW_E_VMR_NO_DEINTERLACE_HW";



        public const string ER_VFWEDVDVMR9INCOMPATIBLEDEC = "VFW_E_DVD_VMR9_INCOMPATIBLEDEC";



        public const string ER_VFWEBADKEY = "VFW_E_BAD_KEY";



        public const string ER_VFWEVMRNOPROCAMPHW = "VFW_E_VMR_NO_PROCAMP_HW";



        public const string ER_VFWEVMRNOAPSUPPLIED = "VFW_E_VMR_NO_AP_SUPPLIED";



        public const string ER_VFWECERTIFICATIONFAILURE = "VFW_E_CERTIFICATION_FAILURE";



        public const string ER_VFWEPINALREADYBLOCKEDONTHISTHREAD = "VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD";



        public const string ER_VFWEDVDNOGOUPPGC = "VFW_E_DVD_NO_GOUP_PGC";



        public const string ER_VFWEDVDNOTINKARAOKEMODE = "VFW_E_DVD_NOT_IN_KARAOKE_MODE";



        public const string ER_VFWEDVDSTREAMDISABLED = "VFW_E_DVD_STREAM_DISABLED";



        public const string ER_VFWEDVDINVALIDDISC = "VFW_E_DVD_INVALID_DISC";



        public const string ER_VFWEDVDNORESUMEINFORMATION = "VFW_E_DVD_NO_RESUME_INFORMATION";



        public const string ER_VFWEDVDTITLEUNKNOWN = "VFW_E_DVD_TITLE_UNKNOWN";



        public const string ER_VFWEFRAMESTEPUNSUPPORTED = "VFW_E_FRAME_STEP_UNSUPPORTED";



        public const string ER_VFWEDVDLOWPARENTALLEVEL = "VFW_E_DVD_LOW_PARENTAL_LEVEL";



        public const string ER_VFWEDVDDECNOTENOUGH = "VFW_E_DVD_DECNOTENOUGH";



        public const string ER_VFWEDVDINCOMPATIBLEREGION = "VFW_E_DVD_INCOMPATIBLE_REGION";



        public const string ER_VFWEDVDNOATTRIBUTES = "VFW_E_DVD_NO_ATTRIBUTES";



        public const string ER_VFWEDVDSTATECORRUPT = "VFW_E_DVD_STATE_CORRUPT";



        public const string ER_VFWEDVDSTATEWRONGDISC = "VFW_E_DVD_STATE_WRONG_DISC";



        public const string ER_VFWEDVDWRONGSPEED = "VFW_E_DVD_WRONG_SPEED";



        public const string ER_VFWEDVDCMDCANCELLED = "VFW_E_DVD_CMD_CANCELLED";



        public const string ER_VFWECOPYPROTFAILED = "VFW_E_COPYPROT_FAILED";



        public const string ER_VFWEDVDSTATEWRONGVERSION = "VFW_E_DVD_STATE_WRONG_VERSION";



        public const string ER_VFWEDVDMENUDOESNOTEXIST = "VFW_E_DVD_MENU_DOES_NOT_EXIST";



        public const string ER_VFWETIMEEXPIRED = "VFW_E_TIME_EXPIRED";



        public const string ER_VFWEDDRAWVERSIONNOTSUITABLE = "VFW_E_DDRAW_VERSION_NOT_SUITABLE";



        public const string ER_VFWENOCAPTUREHARDWARE = "VFW_E_NO_CAPTURE_HARDWARE";



        public const string ER_VFWEDVDINVALIDDOMAIN = "VFW_E_DVD_INVALIDDOMAIN";



        public const string ER_VFWEDVDGRAPHNOTREADY = "VFW_E_DVD_GRAPHNOTREADY";



        public const string ER_VFWEDVDRENDERFAIL = "VFW_E_DVD_RENDERFAIL";



        public const string ER_VFWEDVDNOBUTTON = "VFW_E_DVD_NO_BUTTON";



        public const string ER_VFWEDVDOPERATIONINHIBITED = "VFW_E_DVD_OPERATION_INHIBITED";



        public const string ER_VFWENOVPHARDWARE = "VFW_E_NO_VP_HARDWARE";



        public const string ER_VFWEALREADYCANCELLED = "VFW_E_ALREADY_CANCELLED";



        public const string ER_VFWEREADONLY = "VFW_E_READ_ONLY";



        public const string ER_VFWEOUTOFVIDEOMEMORY = "VFW_E_OUT_OF_VIDEO_MEMORY";



        public const string ER_VFWEVPNEGOTIATIONFAILED = "VFW_E_VP_NEGOTIATION_FAILED";



        public const string ER_VFWEUNSUPPORTEDSTREAM = "VFW_E_UNSUPPORTED_STREAM";



        public const string ER_VFWEBADVIDEOCD = "VFW_E_BAD_VIDEOCD";



        public const string ER_VFWSNOSTOPTIME = "VFW_S_NO_STOP_TIME";



        public const string ER_VFWENOTRANSPORT = "VFW_E_NO_TRANSPORT";



        public const string ER_VFWEBUFFERUNDERFLOW = "VFW_E_BUFFER_UNDERFLOW";



        public const string ER_VFWENOTINGRAPH = "VFW_E_NOT_IN_GRAPH";



        public const string ER_VFWENOTIMEFORMAT = "VFW_E_NO_TIME_FORMAT";



        public const string ER_VFWEINVALIDCLSID = "VFW_E_INVALID_CLSID";



        public const string ER_VFWEPROCESSORNOTSUITABLE = "VFW_E_PROCESSOR_NOT_SUITABLE";



        public const string ER_VFWEUNSUPPORTEDVIDEO = "VFW_E_UNSUPPORTED_VIDEO";



        public const string ER_VFWEMPEGNOTCONSTRAINED = "VFW_E_MPEG_NOT_CONSTRAINED";



        public const string ER_VFWEUNSUPPORTEDAUDIO = "VFW_E_UNSUPPORTED_AUDIO";



        public const string ER_VFWESAMPLETIMENOTSET = "VFW_E_SAMPLE_TIME_NOT_SET";



        public const string ER_VFWENOTIMEFORMATSET = "VFW_E_NO_TIME_FORMAT_SET";



        public const string ER_VFWENOAUDIOHARDWARE = "VFW_E_NO_AUDIO_HARDWARE";



        public const string ER_VFWERPZA = "VFW_E_RPZA";



        public const string ER_VFWEMONOAUDIOHW = "VFW_E_MONO_AUDIO_HW";



        public const string ER_VFWEMEDIATIMENOTSET = "VFW_E_MEDIA_TIME_NOT_SET";



        public const string ER_VFWEINVALIDMEDIATYPE = "VFW_E_INVALID_MEDIA_TYPE";



        public const string ER_VFWEUNKNOWNFILETYPE = "VFW_E_UNKNOWN_FILE_TYPE";



        public const string ER_VFWEFILETOOSHORT = "VFW_E_FILE_TOO_SHORT";



        public const string ER_VFWEINVALIDFILEVERSION = "VFW_E_INVALID_FILE_VERSION";



        public const string ER_VFWECANNOTLOADSOURCEFILTER = "VFW_E_CANNOT_LOAD_SOURCE_FILTER";



        public const string ER_VFWENOADVISESET = "VFW_E_NO_ADVISE_SET";



        public const string ER_VFWECIRCULARGRAPH = "VFW_E_CIRCULAR_GRAPH";



        public const string ER_VFWEADVISEALREADYSET = "VFW_E_ADVISE_ALREADY_SET";



        public const string ER_VFWENOFULLSCREEN = "VFW_E_NO_FULLSCREEN";



        public const string ER_VFWENOMODEXAVAILABLE = "VFW_E_NO_MODEX_AVAILABLE";



        public const string ER_VFWECORRUPTGRAPHFILE = "VFW_E_CORRUPT_GRAPH_FILE";



        public const string ER_VFWETIMEALREADYPASSED = "VFW_E_TIME_ALREADY_PASSED";



        public const string ER_VFWESAMPLEREJECTEDEOS = "VFW_E_SAMPLE_REJECTED_EOS";



        public const string ER_VFWEINVALIDFILEFORMAT = "VFW_E_INVALID_FILE_FORMAT";



        public const string ER_VFWENOTALLOWEDTOSAVE = "VFW_E_NOT_ALLOWED_TO_SAVE";



        public const string ER_VFWEENUMOUTOFRANGE = "VFW_E_ENUM_OUT_OF_RANGE";



        public const string ER_VFWETIMEOUT = "VFW_E_TIMEOUT";



        public const string ER_VFWEINVALIDRECT = "VFW_E_INVALID_RECT";



        public const string ER_VFWEDUPLICATENAME = "VFW_E_DUPLICATE_NAME";



        public const string ER_VFWESAMPLEREJECTED = "VFW_E_SAMPLE_REJECTED";



        public const string ER_VFWEWRONGSTATE = "VFW_E_WRONG_STATE";



        public const string ER_VFWETYPENOTACCEPTED = "VFW_E_TYPE_NOT_ACCEPTED";



        public const string ER_VFWESTARTTIMEAFTEREND = "VFW_E_START_TIME_AFTER_END";



        public const string ER_VFWENOTRUNNING = "VFW_E_NOT_RUNNING";



        public const string ER_VFWENOTPAUSED = "VFW_E_NOT_PAUSED";



        public const string ER_VFWENOTSTOPPED = "VFW_E_NOT_STOPPED";



        public const string ER_VFWESTATECHANGED = "VFW_E_STATE_CHANGED";



        public const string ER_VFWENODISPLAYPALETTE = "VFW_E_NO_DISPLAY_PALETTE";



        public const string ER_VFWETOOMANYCOLORS = "VFW_E_TOO_MANY_COLORS";



        public const string ER_VFWENOCOLORKEYFOUND = "VFW_E_NO_COLOR_KEY_FOUND";



        public const string ER_VFWENOPALETTEAVAILABLE = "VFW_E_NO_PALETTE_AVAILABLE";



        public const string ER_VFWECOLORKEYSET = "VFW_E_COLOR_KEY_SET";



        public const string ER_VFWEPALETTESET = "VFW_E_PALETTE_SET";



        public const string ER_VFWENOTSAMPLECONNECTION = "VFW_E_NOT_SAMPLE_CONNECTION";



        public const string ER_VFWENOTOVERLAYCONNECTION = "VFW_E_NOT_OVERLAY_CONNECTION";



        public const string ER_VFWENOCOLORKEYSET = "VFW_E_NO_COLOR_KEY_SET";



        public const string ER_VFWECHANGINGFORMAT = "VFW_E_CHANGING_FORMAT";



        public const string ER_VFWECANNOTRENDER = "VFW_E_CANNOT_RENDER";



        public const string ER_VFWENOTFOUND = "VFW_E_NOT_FOUND";



        public const string ER_VFWECANNOTCONNECT = "VFW_E_CANNOT_CONNECT";



        public const string ER_VFWENOINTERFACE = "VFW_E_NO_INTERFACE";



        public const string ER_VFWENOSINK = "VFW_E_NO_SINK";



        public const string ER_VFWENOCLOCK = "VFW_E_NO_CLOCK";



        public const string ER_VFWESIZENOTSET = "VFW_E_SIZENOTSET";



        public const string ER_VFWEALREADYCOMMITTED = "VFW_E_ALREADY_COMMITTED";



        public const string ER_VFWEBADALIGN = "VFW_E_BADALIGN";



        public const string ER_VFWEBUFFEROVERFLOW = "VFW_E_BUFFER_OVERFLOW";



        public const string ER_VFWERUNTIMEERROR = "VFW_E_RUNTIME_ERROR";



        public const string ER_VFWEBUFFERSOUTSTANDING = "VFW_E_BUFFERS_OUTSTANDING";



        public const string ER_VFWEBUFFERNOTSET = "VFW_E_BUFFER_NOTSET";



        public const string ER_VFWENOTCOMMITTED = "VFW_E_NOT_COMMITTED";



        public const string ER_VFWENOALLOCATOR = "VFW_E_NO_ALLOCATOR";



        public const string ER_VFWENOTCONNECTED = "VFW_E_NOT_CONNECTED";



        public const string ER_VFWEINVALIDDIRECTION = "VFW_E_INVALID_DIRECTION";



        public const string ER_SVFWENOACCEPTABLETYPES = "VFW_E_NO_ACCEPTABLE_TYPES";



        public const string ER_VFWENOTYPES = "VFW_E_NO_TYPES";



        public const string ER_VFWEFILTERACTIVE = "VFW_E_FILTER_ACTIVE";



        public const string ER_VFWEALREADYCONNECTED = "VFW_E_ALREADY_CONNECTED";



        public const string ER_VFWEENUMOUTOFSYNC = "VFW_E_ENUM_OUT_OF_SYNC";



        public const string ER_VFWENEEDOWNER = "VFW_E_NEED_OWNER";



        public const string ER_VFWEINVALIDSUBTYPE = "VFW_E_INVALIDSUBTYPE";



        public const string ER_VFWSDVDNOTACCURATE = "VFW_S_DVD_NOT_ACCURATE";



        public const string ER_VFWSDVDNONONESEQUENTIAL = "VFW_S_DVD_NON_ONE_SEQUENTIAL";



        public const string ER_VFWSNOPREVIEWPIN = "VFW_S_NOPREVIEWPIN";



        public const string ER_VFWSCANTCUE = "VFW_S_CANT_CUE";



        public const string ER_VFWSSTREAMOFF = "VFW_S_STREAM_OFF";



        public const string ER_VFWSRESERVED = "VFW_S_RESERVED";



        public const string ER_VFWSESTIMATED = "VFW_S_ESTIMATED";



        public const string ER_VFWSDVDCHANNELCONTENTSNOTAVAILABLE = "VFW_S_DVD_CHANNEL_CONTENTS_NOT_AVAILABLE";



        public const string ER_VFWSRPZA = "VFW_S_RPZA";



        public const string ER_VFWSAUDIONOTRENDERED = "VFW_S_AUDIO_NOT_RENDERED";



        public const string ER_VFWSVIDEONOTRENDERED = "VFW_S_VIDEO_NOT_RENDERED";



        public const string ER_VFWSMEDIATYPEIGNORED = "VFW_S_MEDIA_TYPE_IGNORED";



        public const string ER_VFWSRESOURCENOTNEEDED = "VFW_S_RESOURCE_NOT_NEEDED";



        public const string ER_VFWSCONNECTIONSDEFERRED = "VFW_S_CONNECTIONS_DEFERRED";



        public const string ER_VFWSSOMEDATAIGNORED = "VFW_S_SOME_DATA_IGNORED";



        public const string ER_VFWSPARTIALRENDER = "VFW_S_PARTIAL_RENDER";



        public const string ER_VFWSSTATEINTERMEDIATE = "VFW_S_STATE_INTERMEDIATE";



        public const string ER_VFWSDUPLICATENAME = "VFW_S_DUPLICATE_NAME";



        public const string ER_VFWSNOMOREITEMS = "VFW_S_NO_MORE_ITEMS";



        public const string ER_SFALSE = "S_FALSE";



        public const string ER_SOK = "S_OK";



        public const string MT_UnKnown = "Unknown ";



        public const string MT_Video = "Video";



        public const string MT_URLSTREAM = "URL_STREAM";



        public const string MT_Timecode = "Timecode";



        public const string MT_Text = "Text";



        public const string MT_Stream = "Stream";



        public const string MT_ScriptCommand = "ScriptCommand";



        public const string MT_MPEG2PES = "MPEG2_PES";



        public const string MT_Midi = "Midi";



        public const string MT_LMRT = "LMRT";



        public const string MT_Interleaved = "Interleaved";



        public const string MT_AUXLine21Data = "AUXLine21Data";



        public const string MT_File = "File";



        public const string MT_Audio = "Audio";



        public const string MT_AnalogVideo = "AnalogVideo";



        public const string MT_AnalogAudio = "AnalogAudio";



        public const string AUD_Unknown = "Unknown";



        public const string AUD_NORRIS = "NORRIS";



        public const string AUD_LHCODEC = "LH_CODEC";



        public const string AUD_OLIOPR = "OLIOPR";



        public const string AUD_OLISBC = "OLISBC";



        public const string AUD_OLICELP = "OLICELP";



        public const string AUD_OLIADPCM = "OLIADPCM";



        public const string AUD_BTVDIGITAL = "BTV_DIGITAL";



        public const string AUD_OLIGSM = "OLIGSM";



        public const string AUD_FMTOWNSSND = "FM_TOWNS_SND";



        public const string AUD_QUARTERDECK = "QUARTERDECK";



        public const string AUD_CREATIVEFASTSPEECH10 = "CREATIVE_FASTSPEECH10";



        public const string AUD_CREATIVEFASTSPEECH8 = "CREATIVE_FASTSPEECH8";



        public const string AUD_CREATIVEADPCM = "CREATIVE_ADPCM";



        public const string AUD_RHETOREXADPCM = "RHETOREX_ADPCM";



        public const string AUD_SOFTSOUND = "SOFTSOUND";



        public const string AUD_VOXWARE = "VOXWARE";



        public const string AUD_DSATDISPLAY = "DSAT_DISPLAY";



        public const string AUD_DSAT = "DSAT";



        public const string AUD_G722ADPCM = "G722_ADPCM";



        public const string AUD_G726ADPCM = "G726_ADPCM";



        public const string AUD_CANOPUSATRAC = "CANOPUS_ATRAC";



        public const string AUD_ESPCM = "ESPCM";



        public const string AUD_CIRRUS = "CIRRUS";



        public const string AUD_MPEGLAYER3 = "MPEGLAYER3";



        public const string AUD_MPEG = "MPEG";



        public const string AUD_G728CELP = "G728_CELP";



        public const string AUD_G721ADPCM = "G721_ADPCM";



        public const string AUD_XEBEC = "XEBEC";



        public const string AUD_ROCKWELLDIGITALK = "ROCKWELL_DIGITALK";



        public const string AUD_ROCKWELLADPCM = "ROCKWELL_ADPCM";



        public const string AUD_ECHOSC3 = "ECHOSC3";



        public const string AUD_CSIMAADPCM = "CS_IMAADPCM";



        public const string AUD_NMSVBXADPCM = "NMS_VBXADPCM";



        public const string AUD_CONTROLRESCR10 = "CONTROL_RES_CR10";



        public const string AUD_DIGIADPCM = "DIGIADPCM";



        public const string AUD_DIGIREAL = "DIGIREAL";



        public const string AUD_CONTROLRESVQLPC = "CONTROL_RES_VQLPC";



        public const string AUD_ANTEXADPCME = "ANTEX_ADPCME";



        public const string AUD_MSNAUDIO = "MSNAUDIO";



        public const string AUD_GSM610 = "GSM610";



        public const string AUD_DOLBYAC2 = "DOLBY_AC2";



        public const string AUD_AUDIOFILEAF10 = "AUDIOFILE_AF10";



        public const string AUD_APTX = "APTX";



        public const string AUD_AUDIOFILEAF36 = "AUDIOFILE_AF36";



        public const string AUD_ECHOSC1 = "ECHOSC1";



        public const string AUD_DSPGROUPTRUESPEECH = "DSPGROUP_TRUESPEECH";



        public const string AUD_SONARC = "SONARC";



        public const string AUD_YAMAHAADPCM = "YAMAHA_ADPCM";



        public const string AUD_MEDIAVISIONADPCM = "MEDIAVISION_ADPCM";



        public const string AUD_DIALOGICOKIADPCM = "DIALOGIC_OKI_ADPCM";



        public const string AUD_DIGIFIX = "DIGIFIX";



        public const string AUD_DIGISTD = "DIGISTD";



        public const string AUD_G723ADPCM = "G723_ADPCM";



        public const string AUD_SIERRAADPCM = "SIERRA_ADPCM";



        public const string AUD_MEDIASPACEADPCM = "MEDIASPACE_ADPCM";



        public const string AUD_DVIADPCM = "DVI_ADPCM";



        public const string AUD_OKIADPCM = "OKI_ADPCM";



        public const string AUD_MULAW = "MULAW";



        public const string AUD_ALAW = "ALAW";



        public const string AUD_IBMCVSD = "IBM_CVSD";



        public const string AUD_IEEEFLOAT = "IEEE_FLOAT";



        public const string AUD_ADPCM = "ADPCM";



        public const string AUD_PCM = "PCM";



        public const string MST_UnKnown = "Unknown ";



        public const string MST_VOXWAREMetaSound = "VOXWARE_MetaSound";



        public const string MST_DIVX = "DIVX";



        public const string MST_MSMPEG4 = "MS-MPEG4";



        public const string MST_PROVIDER = "PROVIDER";



        public const string MST_DSI = "DSI";



        public const string MST_PCI = "PCI";



        public const string MST_SDDS = "SDDS";



        public const string MST_DVDLPCMAUDIO = "DVD_LPCM_AUDIO";



        public const string MST_DTS = "DTS";



        public const string MST_DVDSUBPICTURE = "DVD_SUBPICTURE";



        public const string MST_DOLBYAC3 = "DOLBY_AC3";



        public const string MST_MPEG2AUDIO = "MPEG2_AUDIO";



        public const string MST_MPEG2TRANSPORT = "MPEG2_TRANSPORT";



        public const string MST_MPEG2PROGRAM = "MPEG2_PROGRAM";



        public const string MST_MPEG2VIDEO = "MPEG2_VIDEO";



        public const string MST_AnalogVideoSECAML = "AnalogVideo_SECAM_L";



        public const string MST_AnalogVideoSECAMK1 = "AnalogVideo_SECAM_K1";



        public const string MST_AnalogVideoSECAMK = "AnalogVideo_SECAM_K";



        public const string MST_AnalogVideoSECAMH = "AnalogVideo_SECAM_H";



        public const string MST_AnalogVideoSECAMG = "AnalogVideo_SECAM_G";



        public const string MST_AnalogVideoSECAMD = "AnalogVideo_SECAM_D";



        public const string MST_AnalogVideoSECAMB = "AnalogVideo_SECAM_B";



        public const string MST_AnalogVideoPALNCOMBO = "AnalogVideo_PAL_N_COMBO";



        public const string MST_AnalogVideoPALN = "AnalogVideo_PAL_N";



        public const string MST_AnalogVideoPALM = "AnalogVideo_PAL_M";



        public const string MST_AnalogVideoPALI = "AnalogVideo_PAL_I";



        public const string MST_AnalogVideoPALH = "AnalogVideo_PAL_H";



        public const string MST_AnalogVideoPALG = "AnalogVideo_PAL_G";



        public const string MST_AnalogVideoPALD = "AnalogVideo_PAL_D";



        public const string MST_AnalogVideoPALB = "AnalogVideo_PAL_B";



        public const string MST_AnalogVideoNTSCM = "AnalogVideo_NTSC_M";



        public const string MST_VPVBI = "VPVBI";



        public const string MST_VPVideo = "VPVideo";



        public const string MST_DssAudio = "DssAudio";



        public const string MST_DssVideo = "DssVideo";



        public const string MST_RAWSPORT = "RAW_SPORT";



        public const string MST_SPDIFTAG241h = "SPDIF_TAG_241h";



        public const string MST_DOLBYAC3SPDIF = "DOLBY_AC3_SPDIF";



        public const string MST_IEEEFLOAT = "IEEE_FLOAT";



        public const string MST_DRMAudio = "DRM_Audio";



        public const string MST_Line21VBIRawData = "Line21_VBIRawData";



        public const string MST_Line21GOPPacket = "Line21_GOPPacket";



        public const string MST_Line21BytePair = "Line21_BytePair";



        public const string MST_Dvsl = "dvsl";



        public const string MST_Dvhd = "dvhd";



        public const string MST_Dvsd = "dvsd_";



        public const string MST_AIFF = "AIFF";



        public const string MST_AU = "AU";



        public const string MST_PCMAudioObsolete = "PCMAudio_Obsolete";



        public const string MST_WAVE = "WAVE";



        public const string MST_QTJpeg = "QTJpeg";



        public const string MST_PCM = "PCM";



        public const string MST_QTRpza = "QTRpza";



        public const string MST_QTRle = "QTRle";



        public const string MST_Asf = "ASF";



        public const string MST_QTMovie = "QTMovie";



        public const string MST_QTSmc = "QTSmc";



        public const string MST_Avi = "AVI";



        public const string MST_MPEG1Audio = "MPEG1Audio";



        public const string MST_MPEG1Video = "MPEG1Video";



        public const string MST_MPEG1VideoCD = "MPEG1VideoCD";



        public const string MST_MPEG1System = "MPEG1System";



        public const string MST_MPEG1AudioPayload = "MPEG1AudioPayload";



        public const string MST_MPEG1Payload = "MPEG1Payload";



        public const string MST_MPEG1Packet = "MPEG1Packet";



        public const string MST_Overlay = "Overlay";



        public const string MST_ARGB32 = "ARGB32";



        public const string MST_RGB32 = "RGB32";



        public const string MST_RGB24 = "RGB24";



        public const string MST_RGB555 = "RGB555";



        public const string MST_RGB565 = "RGB565";



        public const string MST_RGB8 = "RGB8";



        public const string MST_RGB4 = "RGB4";



        public const string MST_RGB1 = "RGB1";



        public const string MST_MDVF = "MDVF";



        public const string MST_IJPG = "IJPG";



        public const string MST_DVCS = "DVCS";



        public const string MST_Plum = "Plum";



        public const string MST_CFCC = "CFCC";



        public const string MST_TVMJ = "TVMJ";



        public const string MST_WAKE = "WAKE";



        public const string MST_MJPG = "MJPG";



        public const string MST_CLJR = "CLJR";



        public const string MST_Y211 = "Y211";



        public const string MST_CPLA = "CPLA";



        public const string MST_IF09 = "IF09";



        public const string MST_YV12 = "YV12";



        public const string MST_UYVY = "UYVY";



        public const string MST_YVYU = "YVYU";



        public const string MST_YUY2 = "YUY2";



        public const string MST_Y41P = "Y41P";



        public const string MST_Y411 = "Y411";
        
        public const string MST_YVU9 = "YVU9";
        
        public const string MST_IYUV = "IYUV";
        
        public const string MST_YUYV = "YUYV";
        
        public const string MST_CLPL = "CLPL";
        
        public const string ActiveMovieGraph = "ActiveMovieGraph";

        // ReSharper disable InconsistentNaming
        
        public const string CLSID_VideoMixingRenderer9 = "51b4abf3-748f-4e3b-a276-c828330e926a";
        
        public const string CLSID_EnhancedVideoRenderer = "FA10746C-9B63-4b6c-BC49-FC300EA5F256";
        
        public const string CLSID_AudioRender = "E30629D1-27E5-11CE-875D-00608CB78066";
        
        public const string CLSID_DSoundRender = "79376820-07D0-11CF-A24D-0020AFD79767";

        public const string CLSID_DVDNavigator = "9B8C4620-2C1A-11d0-8493-00A02438AD48";
        
        public const string CLSID_VideoMixingRenderer = "B87BEB7B-8D29-423f-AE4D-6582C10175AC";
        
        public const string CLSID_VideoRenderer = "70e102b0-5556-11ce-97c0-00aa0055595a";
        
        public const string CLSID_VideoRendererDefault = "6BC1CFFA-8FC1-4261-AC22-CFB4CC38DB50";

        public const string CLSID_DirectVobSub = "9852A670-F845-491B-9BE6-EBD841B8A613";

        public const string CLSID_DirectVobSubManual = "93A22E7A-5091-45EF-BA61-6DA26156A5D0";

        // ReSharper restore InconsistentNaming
    }


}
