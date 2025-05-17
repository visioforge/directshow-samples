namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Text;

    using DirectShowLib;
    using DirectShowLib.DES;

    using AMMediaType = DirectShowLib.AMMediaType;
    using BitmapInfoHeader = DirectShowLib.BitmapInfoHeader;
    //using MPEG1VideoInfo = DirectShowLib.MPEG1VideoInfo;
    using VideoInfoHeader = DirectShowLib.VideoInfoHeader;
    using VideoInfoHeader2 = DirectShowLib.VideoInfoHeader2;
    using WaveFormatEx = DirectShowLib.WaveFormatEx;

    /// <summary>
    /// MPEG2VideoInfo structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MPEG2VideoInfo
    {
        /// <summary>
        /// VideoInfoHeader2 structure.
        /// </summary>
        public VideoInfoHeader2 HDR;

        /// <summary>
        /// Start time code. 25-bit group-of-pictures (GOP) time code at start of data. This field is not used for DVD.
        /// </summary>
        public int StartTimeCode;

        /// <summary>
        /// Length of the sequence header, in bytes. For DVD, set this field to zero. The sequence header is given in the SequenceHeader field. 
        /// </summary>
        public int SequenceHeaderSize;

        /// <summary>
        /// Profile. Use enum MPEG2Profile.
        /// </summary>
        public int Profile; 

        /// <summary>
        /// Level. Use enum MPEG2Level.
        /// </summary>
        public int Level;

        /// <summary>
        /// Flags. Use AMMPEG2_* defines.  Reject connection if undefined bits are not 0.
        /// </summary>
        public int Flags;

        /// <summary>
        /// Sequence header. DWORD instead of Byte for alignment purposes. For MPEG-2, if a sequence_header 
        /// is included, the sequence_extension should also be included  .
        /// </summary>
        public int SequenceHeader;     
    }

    /// <summary>
    /// Deinterlace technology.
    /// </summary>
    public static class DeinterlaceTech9
    {
        // ReSharper disable RedundantCast
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// E_FAIL code.
        /// </summary>
        public const int E_FAIL = unchecked((int)0x80004005);

        /// <summary>
        /// the algorithm is unknown or proprietary.
        /// </summary>
        public const int Unknown = unchecked((int)0x0000);

        /// <summary>
        /// the algorithm creates the missing lines by repeating
        /// the line either above or below it - this method will look very jaggy and
        /// isn't recommended.
        /// </summary>
        public const int BOBLineReplicate = unchecked((int)0x0001);

        /// <summary>
        /// the algorithm creates the missing lines by vertically stretching each
        /// video field by a factor of two, for example by averaging two lines or
        /// using a [-1, 9, 9, -1]/16 filter across four lines.
        /// Slight vertical adjustments are made to ensure that the resulting image
        /// does not "bob" up and down.
        /// </summary>
        public const int BOBVerticalStretch = unchecked((int)0x0002);

        /// <summary>
        /// the pixels in the missing line are recreated by a median filtering operation.
        /// </summary>
        public const int MedianFiltering = unchecked((int)0x0004);

        /// <summary>
        /// the pixels in the missing line are recreated by an edge filter.
        /// In this process, spatial directional filters are applied to determine
        /// the orientation of edges in the picture content, and missing
        /// pixels are created by filtering along (rather than across) the
        /// detected edges.
        /// </summary>
        public const int EdgeFiltering = unchecked((int)0x0010);

        /// <summary>
        /// the pixels in the missing line are recreated by switching on a field by
        /// field basis between using either spatial or temporal interpolation
        /// depending on the amount of motion.
        /// </summary>
        public const int FieldAdaptive = unchecked((int)0x0020);

        /// <summary>
        /// the pixels in the missing line are recreated by switching on a pixel by pixel
        /// basis between using either spatial or temporal interpolation depending on
        /// the amount of motion.
        /// </summary>
        public const int PixelAdaptive = unchecked((int)0x0040);

        /// <summary>
        /// Motion Vector Steering  identifies objects within a sequence of video 
        /// fields.  The missing pixels are recreated after first aligning the
        /// movement axes of the individual objects in the scene to make them
        /// parallel with the time axis.
        /// </summary>
        public const int MotionVectorSteered = unchecked((int)0x0080);

        // ReSharper restore RedundantCast
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// WMV network streaming error codes.
    /// </summary>
    public static class WMVNSResults
    {
        public const int S_CALLPENDING = unchecked((int)0x000D0000);
        public const int S_CALLABORTED = unchecked((int)0x000D0001);
        public const int S_STREAM_TRUNCATED = unchecked((int)0x000D0002);
        public const int W_SERVER_BANDWIDTH_LIMIT = unchecked((int)0x800D0003);
        public const int W_FILE_BANDWIDTH_LIMIT = unchecked((int)0x800D0004);
        public const int E_NOCONNECTION = unchecked((int)0xC00D0005);
        public const int E_CANNOTCONNECT = unchecked((int)0xC00D0006);
        public const int E_CANNOTDESTROYTITLE = unchecked((int)0xC00D0007);
        public const int E_CANNOTRENAMETITLE = unchecked((int)0xC00D0008);
        public const int E_CANNOTOFFLINEDISK = unchecked((int)0xC00D0009);
        public const int E_CANNOTONLINEDISK = unchecked((int)0xC00D000A);
        public const int E_NOREGISTEREDWALKER = unchecked((int)0xC00D000B);
        public const int E_NOFUNNEL = unchecked((int)0xC00D000C);
        public const int E_NO_LOCALPLAY = unchecked((int)0xC00D000D);
        public const int E_NETWORK_BUSY = unchecked((int)0xC00D000E);
        public const int E_TOO_MANY_SESS = unchecked((int)0xC00D000F);
        public const int E_ALREADY_CONNECTED = unchecked((int)0xC00D0010);
        public const int E_INVALID_INDEX = unchecked((int)0xC00D0011);
        public const int E_PROTOCOL_MISMATCH = unchecked((int)0xC00D0012);
        public const int E_TIMEOUT = unchecked((int)0xC00D0013);
        public const int E_NET_WRITE = unchecked((int)0xC00D0014);
        public const int E_NET_READ = unchecked((int)0xC00D0015);
        public const int E_DISK_WRITE = unchecked((int)0xC00D0016);
        public const int E_DISK_READ = unchecked((int)0xC00D0017);
        public const int E_FILE_WRITE = unchecked((int)0xC00D0018);
        public const int E_FILE_READ = unchecked((int)0xC00D0019);
        public const int E_FILE_NOT_FOUND = unchecked((int)0xC00D001A);
        public const int E_FILE_EXISTS = unchecked((int)0xC00D001B);
        public const int E_INVALID_NAME = unchecked((int)0xC00D001C);
        public const int E_FILE_OPEN_FAILED = unchecked((int)0xC00D001D);
        public const int E_FILE_ALLOCATION_FAILED = unchecked((int)0xC00D001E);
        public const int E_FILE_INIT_FAILED = unchecked((int)0xC00D001F);
        public const int E_FILE_PLAY_FAILED = unchecked((int)0xC00D0020);
        public const int E_SET_DISK_UID_FAILED = unchecked((int)0xC00D0021);
        public const int E_INDUCED = unchecked((int)0xC00D0022);
        public const int E_CCLINK_DOWN = unchecked((int)0xC00D0023);
        public const int E_INTERNAL = unchecked((int)0xC00D0024);
        public const int E_BUSY = unchecked((int)0xC00D0025);
        public const int E_UNRECOGNIZED_STREAM_TYPE = unchecked((int)0xC00D0026);
        public const int E_NETWORK_SERVICE_FAILURE = unchecked((int)0xC00D0027);
        public const int E_NETWORK_RESOURCE_FAILURE = unchecked((int)0xC00D0028);
        public const int E_CONNECTION_FAILURE = unchecked((int)0xC00D0029);
        public const int E_SHUTDOWN = unchecked((int)0xC00D002A);
        public const int E_INVALID_REQUEST = unchecked((int)0xC00D002B);
        public const int E_INSUFFICIENT_BANDWIDTH = unchecked((int)0xC00D002C);
        public const int E_NOT_REBUILDING = unchecked((int)0xC00D002D);
        public const int E_LATE_OPERATION = unchecked((int)0xC00D002E);
        public const int E_InvalidData = unchecked((int)0xC00D002F);
        public const int E_FILE_BANDWIDTH_LIMIT = unchecked((int)0xC00D0030);
        public const int E_OPEN_FILE_LIMIT = unchecked((int)0xC00D0031);
        public const int E_BAD_CONTROL_DATA = unchecked((int)0xC00D0032);
        public const int E_NO_STREAM = unchecked((int)0xC00D0033);
        public const int E_STREAM_END = unchecked((int)0xC00D0034);
        public const int E_SERVER_NOT_FOUND = unchecked((int)0xC00D0035);
        public const int E_DUPLICATE_NAME = unchecked((int)0xC00D0036);
        public const int E_DUPLICATE_ADDRESS = unchecked((int)0xC00D0037);
        public const int E_BAD_MULTICAST_ADDRESS = unchecked((int)0xC00D0038);
        public const int E_BAD_ADAPTER_ADDRESS = unchecked((int)0xC00D0039);
        public const int E_BAD_DELIVERY_MODE = unchecked((int)0xC00D003A);
        public const int E_INVALID_CHANNEL = unchecked((int)0xC00D003B);
        public const int E_INVALID_STREAM = unchecked((int)0xC00D003C);
        public const int E_INVALID_ARCHIVE = unchecked((int)0xC00D003D);
        public const int E_NOTITLES = unchecked((int)0xC00D003E);
        public const int E_INVALID_CLIENT = unchecked((int)0xC00D003F);
        public const int E_INVALID_BLACKHOLE_ADDRESS = unchecked((int)0xC00D0040);
        public const int E_INCOMPATIBLE_FORMAT = unchecked((int)0xC00D0041);
        public const int E_INVALID_KEY = unchecked((int)0xC00D0042);
        public const int E_INVALID_PORT = unchecked((int)0xC00D0043);
        public const int E_INVALID_TTL = unchecked((int)0xC00D0044);
        public const int E_STRIDE_REFUSED = unchecked((int)0xC00D0045);
        public const int E_MMSAUTOSERVER_CANTFINDWALKER = unchecked((int)0xC00D0046);
        public const int E_MAX_BITRATE = unchecked((int)0xC00D0047);
        public const int E_LOGFILEPERIOD = unchecked((int)0xC00D0048);
        public const int E_MAX_CLIENTS = unchecked((int)0xC00D0049);
        public const int E_LOG_FILE_SIZE = unchecked((int)0xC00D004A);
        public const int E_MAX_FILERATE = unchecked((int)0xC00D004B);
        public const int E_WALKER_UNKNOWN = unchecked((int)0xC00D004C);
        public const int E_WALKER_SERVER = unchecked((int)0xC00D004D);
        public const int E_WALKER_USAGE = unchecked((int)0xC00D004E);
        public const int I_TIGER_START = unchecked((int)0x400D004F);
        public const int E_TIGER_FAIL = unchecked((int)0xC00D0050);
        public const int I_CUB_START = unchecked((int)0x400D0051);
        public const int I_CUB_RUNNING = unchecked((int)0x400D0052);
        public const int E_CUB_FAIL = unchecked((int)0xC00D0053);
        public const int I_DISK_START = unchecked((int)0x400D0054);
        public const int E_DISK_FAIL = unchecked((int)0xC00D0055);
        public const int I_DISK_REBUILD_STARTED = unchecked((int)0x400D0056);
        public const int I_DISK_REBUILD_FINISHED = unchecked((int)0x400D0057);
        public const int I_DISK_REBUILD_ABORTED = unchecked((int)0x400D0058);
        public const int I_LIMIT_FUNNELS = unchecked((int)0x400D0059);
        public const int I_START_DISK = unchecked((int)0x400D005A);
        public const int I_STOP_DISK = unchecked((int)0x400D005B);
        public const int I_STOP_CUB = unchecked((int)0x400D005C);
        public const int I_KILL_USERSESSION = unchecked((int)0x400D005D);
        public const int I_KILL_CONNECTION = unchecked((int)0x400D005E);
        public const int I_REBUILD_DISK = unchecked((int)0x400D005F);
        public const int W_UNKNOWN_EVENT = unchecked((int)0x800D0060);
        public const int E_MAX_FUNNELS_ALERT = unchecked((int)0xC00D0060);
        public const int E_ALLOCATE_FILE_FAIL = unchecked((int)0xC00D0061);
        public const int E_PAGING_ERROR = unchecked((int)0xC00D0062);
        public const int E_BAD_BLOCK0_VERSION = unchecked((int)0xC00D0063);
        public const int E_BAD_DISK_UID = unchecked((int)0xC00D0064);
        public const int E_BAD_FSMAJOR_VERSION = unchecked((int)0xC00D0065);
        public const int E_BAD_STAMPNUMBER = unchecked((int)0xC00D0066);
        public const int E_PARTIALLY_REBUILT_DISK = unchecked((int)0xC00D0067);
        public const int E_ENACTPLAN_GIVEUP = unchecked((int)0xC00D0068);
        public const int E_NO_FORMATS = unchecked((int)0xC00D006B);
        public const int E_NO_REFERENCES = unchecked((int)0xC00D006C);
        public const int E_WAVE_OPEN = unchecked((int)0xC00D006D);
        public const int I_LOGGING_FAILED = unchecked((int)0x400D006E);
        public const int E_CANNOTCONNECTEVENTS = unchecked((int)0xC00D006F);
        public const int I_LIMIT_BANDWIDTH = unchecked((int)0x400D0070);
        public const int E_NO_DEVICE = unchecked((int)0xC00D0071);
        public const int E_NO_SPECIFIED_DEVICE = unchecked((int)0xC00D0072);
        public const int E_NOTHING_TO_DO = unchecked((int)0xC00D07F1);
        public const int E_NO_MULTICAST = unchecked((int)0xC00D07F2);
        public const int E_MONITOR_GIVEUP = unchecked((int)0xC00D00C8);
        public const int E_REMIRRORED_DISK = unchecked((int)0xC00D00C9);
        public const int E_INSUFFICIENT_DATA = unchecked((int)0xC00D00CA);
        public const int E_ASSERT = unchecked((int)0xC00D00CB);
        public const int E_BAD_ADAPTER_NAME = unchecked((int)0xC00D00CC);
        public const int E_NOT_LICENSED = unchecked((int)0xC00D00CD);
        public const int E_NO_SERVER_CONTACT = unchecked((int)0xC00D00CE);
        public const int E_TOO_MANY_TITLES = unchecked((int)0xC00D00CF);
        public const int E_TITLE_SIZE_EXCEEDED = unchecked((int)0xC00D00D0);
        public const int E_UDP_DISABLED = unchecked((int)0xC00D00D1);
        public const int E_TCP_DISABLED = unchecked((int)0xC00D00D2);
        public const int E_HTTP_DISABLED = unchecked((int)0xC00D00D3);
        public const int E_LICENSE_EXPIRED = unchecked((int)0xC00D00D4);
        public const int E_TITLE_BITRATE = unchecked((int)0xC00D00D5);
        public const int E_EMPTY_PROGRAM_NAME = unchecked((int)0xC00D00D6);
        public const int E_MISSING_CHANNEL = unchecked((int)0xC00D00D7);
        public const int E_NO_CHANNELS = unchecked((int)0xC00D00D8);
        public const int E_INVALID_INDEX2 = unchecked((int)0xC00D00D9);
        public const int E_CUB_FAIL_LINK = unchecked((int)0xC00D0190);
        public const int I_CUB_UNFAIL_LINK = unchecked((int)0x400D0191);
        public const int E_BAD_CUB_UID = unchecked((int)0xC00D0192);
        public const int I_RESTRIPE_START = unchecked((int)0x400D0193);
        public const int I_RESTRIPE_DONE = unchecked((int)0x400D0194);
        public const int E_GLITCH_MODE = unchecked((int)0xC00D0195);
        public const int I_RESTRIPE_DISK_OUT = unchecked((int)0x400D0196);
        public const int I_RESTRIPE_CUB_OUT = unchecked((int)0x400D0197);
        public const int I_DISK_STOP = unchecked((int)0x400D0198);
        public const int I_CATATONIC_FAILURE = unchecked((int)0x800D0199);
        public const int I_CATATONIC_AUTO_UNFAIL = unchecked((int)0x800D019A);
        public const int E_NO_MEDIA_PROTOCOL = unchecked((int)0xC00D019B);
        public const int E_INVALID_INPUT_FORMAT = unchecked((int)0xC00D0BB8);
        public const int E_MSAUDIO_NOT_INSTALLED = unchecked((int)0xC00D0BB9);
        public const int E_UNEXPECTED_MSAUDIO_ERROR = unchecked((int)0xC00D0BBA);
        public const int E_INVALID_OUTPUT_FORMAT = unchecked((int)0xC00D0BBB);
        public const int E_NOT_CONFIGURED = unchecked((int)0xC00D0BBC);
        public const int E_PROTECTED_CONTENT = unchecked((int)0xC00D0BBD);
        public const int E_LICENSE_REQUIRED = unchecked((int)0xC00D0BBE);
        public const int E_TAMPERED_CONTENT = unchecked((int)0xC00D0BBF);
        public const int E_LICENSE_OUTOFDATE = unchecked((int)0xC00D0BC0);
        public const int E_LICENSE_INCORRECT_RIGHTS = unchecked((int)0xC00D0BC1);
        public const int E_AUDIO_CODEC_NOT_INSTALLED = unchecked((int)0xC00D0BC2);
        public const int E_AUDIO_CODEC_ERROR = unchecked((int)0xC00D0BC3);
        public const int E_VIDEO_CODEC_NOT_INSTALLED = unchecked((int)0xC00D0BC4);
        public const int E_VIDEO_CODEC_ERROR = unchecked((int)0xC00D0BC5);
        public const int E_INVALIDPROFILE = unchecked((int)0xC00D0BC6);
        public const int E_INCOMPATIBLE_VERSION = unchecked((int)0xC00D0BC7);
        public const int S_REBUFFERING = unchecked((int)0x000D0BC8);
        public const int S_DEGRADING_QUALITY = unchecked((int)0x000D0BC9);
        public const int E_OFFLINE_MODE = unchecked((int)0xC00D0BCA);
        public const int E_NOT_CONNECTED = unchecked((int)0xC00D0BCB);
        public const int E_TOO_MUCH_DATA = unchecked((int)0xC00D0BCC);
        public const int E_UNSUPPORTED_PROPERTY = unchecked((int)0xC00D0BCD);
        public const int E_8BIT_WAVE_UNSUPPORTED = unchecked((int)0xC00D0BCE);
        public const int E_NO_MORE_SAMPLES = unchecked((int)0xC00D0BCF);
        public const int E_INVALID_SAMPLING_RATE = unchecked((int)0xC00D0BD0);
        public const int E_MAX_PACKET_SIZE_TOO_SMALL = unchecked((int)0xC00D0BD1);
        public const int E_LATE_PACKET = unchecked((int)0xC00D0BD2);
        public const int E_DUPLICATE_PACKET = unchecked((int)0xC00D0BD3);
        public const int E_SDK_BUFFERTOOSMALL = unchecked((int)0xC00D0BD4);
        public const int E_INVALID_NUM_PASSES = unchecked((int)0xC00D0BD5);
        public const int E_ATTRIBUTE_READ_ONLY = unchecked((int)0xC00D0BD6);
        public const int E_ATTRIBUTE_NOT_ALLOWED = unchecked((int)0xC00D0BD7);
        public const int E_INVALID_EDL = unchecked((int)0xC00D0BD8);
        public const int E_DATA_UNIT_EXTENSION_TOO_LARGE = unchecked((int)0xC00D0BD9);
        public const int E_CODEC_DMO_ERROR = unchecked((int)0xC00D0BDA);
        public const int E_NO_CD = unchecked((int)0xC00D0FA0);
        public const int E_CANT_READ_DIGITAL = unchecked((int)0xC00D0FA1);
        public const int E_DEVICE_DISCONNECTED = unchecked((int)0xC00D0FA2);
        public const int E_DEVICE_NOT_SUPPORT_FORMAT = unchecked((int)0xC00D0FA3);
        public const int E_SLOW_READ_DIGITAL = unchecked((int)0xC00D0FA4);
        public const int E_MIXER_INVALID_LINE = unchecked((int)0xC00D0FA5);
        public const int E_MIXER_INVALID_CONTROL = unchecked((int)0xC00D0FA6);
        public const int E_MIXER_INVALID_VALUE = unchecked((int)0xC00D0FA7);
        public const int E_MIXER_UNKNOWN_MMRESULT = unchecked((int)0xC00D0FA8);
        public const int E_USER_STOP = unchecked((int)0xC00D0FA9);
        public const int E_MP3_FORMAT_NOT_FOUND = unchecked((int)0xC00D0FAA);
        public const int E_CD_READ_ERROR_NO_CORRECTION = unchecked((int)0xC00D0FAB);
        public const int E_CD_READ_ERROR = unchecked((int)0xC00D0FAC);
        public const int E_CD_SLOW_COPY = unchecked((int)0xC00D0FAD);
        public const int E_CD_COPYTO_CD = unchecked((int)0xC00D0FAE);
        public const int E_MIXER_NODRIVER = unchecked((int)0xC00D0FAF);
        public const int E_REDBOOK_ENABLED_WHILE_COPYING = unchecked((int)0xC00D0FB0);
        public const int E_CD_REFRESH = unchecked((int)0xC00D0FB1);
        public const int E_CD_DRIVER_PROBLEM = unchecked((int)0xC00D0FB2);
        public const int E_WONT_DO_DIGITAL = unchecked((int)0xC00D0FB3);
        public const int E_WMPXML_NOERROR = unchecked((int)0xC00D0FB4);
        public const int E_WMPXML_ENDOFDATA = unchecked((int)0xC00D0FB5);
        public const int E_WMPXML_PARSEERROR = unchecked((int)0xC00D0FB6);
        public const int E_WMPXML_ATTRIBUTENOTFOUND = unchecked((int)0xC00D0FB7);
        public const int E_WMPXML_PINOTFOUND = unchecked((int)0xC00D0FB8);
        public const int E_WMPXML_EMPTYDOC = unchecked((int)0xC00D0FB9);
        public const int E_WMP_WINDOWSAPIFAILURE = unchecked((int)0xC00D0FC8);
        public const int E_WMP_RECORDING_NOT_ALLOWED = unchecked((int)0xC00D0FC9);
        public const int E_DEVICE_NOT_READY = unchecked((int)0xC00D0FCA);
        public const int E_DAMAGED_FILE = unchecked((int)0xC00D0FCB);
        public const int E_MPDB_GENERIC = unchecked((int)0xC00D0FCC);
        public const int E_FILE_FAILED_CHECKS = unchecked((int)0xC00D0FCD);
        public const int E_MEDIA_LIBRARY_FAILED = unchecked((int)0xC00D0FCE);
        public const int E_SHARING_VIOLATION = unchecked((int)0xC00D0FCF);
        public const int E_NO_ERROR_STRING_FOUND = unchecked((int)0xC00D0FD0);
        public const int E_WMPOCX_NO_REMOTE_CORE = unchecked((int)0xC00D0FD1);
        public const int E_WMPOCX_NO_ACTIVE_CORE = unchecked((int)0xC00D0FD2);
        public const int E_WMPOCX_NOT_RUNNING_REMOTELY = unchecked((int)0xC00D0FD3);
        public const int E_WMPOCX_NO_REMOTE_WINDOW = unchecked((int)0xC00D0FD4);
        public const int E_WMPOCX_ERRORMANAGERNOTAVAILABLE = unchecked((int)0xC00D0FD5);
        public const int E_PLUGIN_NOTSHUTDOWN = unchecked((int)0xC00D0FD6);
        public const int E_WMP_CANNOT_FIND_FOLDER = unchecked((int)0xC00D0FD7);
        public const int E_WMP_STREAMING_RECORDING_NOT_ALLOWED = unchecked((int)0xC00D0FD8);
        public const int E_WMP_PLUGINDLL_NOTFOUND = unchecked((int)0xC00D0FD9);
        public const int E_NEED_TO_ASK_USER = unchecked((int)0xC00D0FDA);
        public const int E_WMPOCX_PLAYER_NOT_DOCKED = unchecked((int)0xC00D0FDB);
        public const int E_WMP_EXTERNAL_NOTREADY = unchecked((int)0xC00D0FDC);
        public const int E_WMP_MLS_STALE_DATA = unchecked((int)0xC00D0FDD);
        public const int E_WMP_UI_SUBCONTROLSNOTSUPPORTED = unchecked((int)0xC00D0FDE);
        public const int E_WMP_UI_VERSIONMISMATCH = unchecked((int)0xC00D0FDF);
        public const int E_WMP_UI_NOTATHEMEFILE = unchecked((int)0xC00D0FE0);
        public const int E_WMP_UI_SUBELEMENTNOTFOUND = unchecked((int)0xC00D0FE1);
        public const int E_WMP_UI_VERSIONPARSE = unchecked((int)0xC00D0FE2);
        public const int E_WMP_UI_VIEWIDNOTFOUND = unchecked((int)0xC00D0FE3);
        public const int E_WMP_UI_PASSTHROUGH = unchecked((int)0xC00D0FE4);
        public const int E_WMP_UI_OBJECTNOTFOUND = unchecked((int)0xC00D0FE5);
        public const int E_WMP_UI_SECONDHANDLER = unchecked((int)0xC00D0FE6);
        public const int E_WMP_UI_NOSKININZIP = unchecked((int)0xC00D0FE7);
        public const int S_WMP_UI_VERSIONMISMATCH = unchecked((int)0x000D0FE8);
        public const int S_WMP_EXCEPTION = unchecked((int)0x000D0FE9);
        public const int E_WMP_URLDOWNLOADFAILED = unchecked((int)0xC00D0FEA);
        public const int E_WMPOCX_UNABLE_TO_LOAD_SKIN = unchecked((int)0xC00D0FEB);
        public const int E_WMP_INVALID_SKIN = unchecked((int)0xC00D0FEC);
        public const int E_WMP_SENDMAILFAILED = unchecked((int)0xC00D0FED);
        public const int E_WMP_SAVEAS_READONLY = unchecked((int)0xC00D0FF0);
        public const int E_WMP_RBC_JPGMAPPINGIMAGE = unchecked((int)0xC00D1004);
        public const int E_WMP_JPGTRANSPARENCY = unchecked((int)0xC00D1005);
        public const int E_WMP_INVALID_MAX_VAL = unchecked((int)0xC00D1009);
        public const int E_WMP_INVALID_MIN_VAL = unchecked((int)0xC00D100A);
        public const int E_WMP_CS_JPGPOSITIONIMAGE = unchecked((int)0xC00D100E);
        public const int E_WMP_CS_NOTEVENLYDIVISIBLE = unchecked((int)0xC00D100F);
        public const int E_WMPZIP_NOTAZIPFILE = unchecked((int)0xC00D1018);
        public const int E_WMPZIP_CORRUPT = unchecked((int)0xC00D1019);
        public const int E_WMPZIP_FILENOTFOUND = unchecked((int)0xC00D101A);
        public const int E_WMP_IMAGE_FILETYPE_UNSUPPORTED = unchecked((int)0xC00D1022);
        public const int E_WMP_IMAGE_INVALID_FORMAT = unchecked((int)0xC00D1023);
        public const int E_WMP_GIF_UNEXPECTED_ENDOFFILE = unchecked((int)0xC00D1024);
        public const int E_WMP_GIF_INVALID_FORMAT = unchecked((int)0xC00D1025);
        public const int E_WMP_GIF_BAD_VERSION_NUMBER = unchecked((int)0xC00D1026);
        public const int E_WMP_GIF_NO_IMAGE_IN_FILE = unchecked((int)0xC00D1027);
        public const int E_WMP_PNG_INVALIDFORMAT = unchecked((int)0xC00D1028);
        public const int E_WMP_PNG_UNSUPPORTED_BITDEPTH = unchecked((int)0xC00D1029);
        public const int E_WMP_PNG_UNSUPPORTED_COMPRESSION = unchecked((int)0xC00D102A);
        public const int E_WMP_PNG_UNSUPPORTED_FILTER = unchecked((int)0xC00D102B);
        public const int E_WMP_PNG_UNSUPPORTED_INTERLACE = unchecked((int)0xC00D102C);
        public const int E_WMP_PNG_UNSUPPORTED_BAD_CRC = unchecked((int)0xC00D102D);
        public const int E_WMP_BMP_INVALID_BITMASK = unchecked((int)0xC00D102E);
        public const int E_WMP_BMP_TOPDOWN_DIB_UNSUPPORTED = unchecked((int)0xC00D102F);
        public const int E_WMP_BMP_BITMAP_NOT_CREATED = unchecked((int)0xC00D1030);
        public const int E_WMP_BMP_COMPRESSION_UNSUPPORTED = unchecked((int)0xC00D1031);
        public const int E_WMP_BMP_INVALID_FORMAT = unchecked((int)0xC00D1032);
        public const int E_WMP_JPG_JERR_ARITHCODING_NOTIMPL = unchecked((int)0xC00D1033);
        public const int E_WMP_JPG_INVALID_FORMAT = unchecked((int)0xC00D1034);
        public const int E_WMP_JPG_BAD_DCTSIZE = unchecked((int)0xC00D1035);
        public const int E_WMP_JPG_BAD_VERSION_NUMBER = unchecked((int)0xC00D1036);
        public const int E_WMP_JPG_BAD_PRECISION = unchecked((int)0xC00D1037);
        public const int E_WMP_JPG_CCIR601_NOTIMPL = unchecked((int)0xC00D1038);
        public const int E_WMP_JPG_NO_IMAGE_IN_FILE = unchecked((int)0xC00D1039);
        public const int E_WMP_JPG_READ_ERROR = unchecked((int)0xC00D103A);
        public const int E_WMP_JPG_FRACT_SAMPLE_NOTIMPL = unchecked((int)0xC00D103B);
        public const int E_WMP_JPG_IMAGE_TOO_BIG = unchecked((int)0xC00D103C);
        public const int E_WMP_JPG_UNEXPECTED_ENDOFFILE = unchecked((int)0xC00D103D);
        public const int E_WMP_JPG_SOF_UNSUPPORTED = unchecked((int)0xC00D103E);
        public const int E_WMP_JPG_UNKNOWN_MARKER = unchecked((int)0xC00D103F);
        public const int S_WMP_LOADED_GIF_IMAGE = unchecked((int)0x000D1040);
        public const int S_WMP_LOADED_PNG_IMAGE = unchecked((int)0x000D1041);
        public const int S_WMP_LOADED_BMP_IMAGE = unchecked((int)0x000D1042);
        public const int S_WMP_LOADED_JPG_IMAGE = unchecked((int)0x000D1043);
        public const int E_WMG_RATEUNAVAILABLE = unchecked((int)0xC00D104A);
        public const int E_WMG_PLUGINUNAVAILABLE = unchecked((int)0xC00D104B);
        public const int E_WMG_CANNOTQUEUE = unchecked((int)0xC00D104C);
        public const int E_WMG_PREROLLLICENSEACQUISITIONNOTALLOWED = unchecked((int)0xC00D104D);
        public const int E_WMG_UNEXPECTEDPREROLLSTATUS = unchecked((int)0xC00D104E);
        public const int E_WMG_INVALIDSTATE = unchecked((int)0xC00D1054);
        public const int E_WMG_SINKALREADYEXISTS = unchecked((int)0xC00D1055);
        public const int E_WMG_NOSDKINTERFACE = unchecked((int)0xC00D1056);
        public const int E_WMG_NOTALLOUTPUTSRENDERED = unchecked((int)0xC00D1057);
        public const int E_WMG_FILETRANSFERNOTALLOWED = unchecked((int)0xC00D1058);
        public const int E_WMR_UNSUPPORTEDSTREAM = unchecked((int)0xC00D1059);
        public const int E_WMR_PINNOTFOUND = unchecked((int)0xC00D105A);
        public const int E_WMR_WAITINGONFORMATSWITCH = unchecked((int)0xC00D105B);
        public const int E_WMR_NOSOURCEFILTER = unchecked((int)0xC00D105C);
        public const int E_WMR_PINTYPENOMATCH = unchecked((int)0xC00D105D);
        public const int E_WMR_NOCALLBACKAVAILABLE = unchecked((int)0xC00D105E);
        public const int S_WMR_ALREADYRENDERED = unchecked((int)0x000D105F);
        public const int S_WMR_PINTYPEPARTIALMATCH = unchecked((int)0x000D1060);
        public const int S_WMR_PINTYPEFULLMATCH = unchecked((int)0x000D1061);
        public const int E_WMR_SAMPLEPROPERTYNOTSET = unchecked((int)0xC00D1062);
        public const int E_WMR_CANNOT_RENDER_BINARY_STREAM = unchecked((int)0xC00D1063);
        public const int E_WMG_LICENSE_TAMPERED = unchecked((int)0xC00D1064);
        public const int E_WMR_WILLNOT_RENDER_BINARY_STREAM = unchecked((int)0xC00D1065);
        public const int E_WMX_UNRECOGNIZED_PLAYLIST_FORMAT = unchecked((int)0xC00D1068);
        public const int E_ASX_INVALIDFORMAT = unchecked((int)0xC00D1069);
        public const int E_ASX_INVALIDVERSION = unchecked((int)0xC00D106A);
        public const int E_ASX_INVALID_REPEAT_BLOCK = unchecked((int)0xC00D106B);
        public const int E_ASX_NOTHING_TO_WRITE = unchecked((int)0xC00D106C);
        public const int E_URLLIST_INVALIDFORMAT = unchecked((int)0xC00D106D);
        public const int E_WMX_ATTRIBUTE_DOES_NOT_EXIST = unchecked((int)0xC00D106E);
        public const int E_WMX_ATTRIBUTE_ALREADY_EXISTS = unchecked((int)0xC00D106F);
        public const int E_WMX_ATTRIBUTE_UNRETRIEVABLE = unchecked((int)0xC00D1070);
        public const int E_WMX_ITEM_DOES_NOT_EXIST = unchecked((int)0xC00D1071);
        public const int E_WMX_ITEM_TYPE_ILLEGAL = unchecked((int)0xC00D1072);
        public const int E_WMX_ITEM_UNSETTABLE = unchecked((int)0xC00D1073);
        public const int E_WMX_PLAYLIST_EMPTY = unchecked((int)0xC00D1074);
        public const int E_MLS_SMARTPLAYLIST_FILTER_NOT_REGISTERED = unchecked((int)0xC00D1075);
        public const int E_WMX_INVALID_FORMAT_OVER_NESTING = unchecked((int)0xC00D1076);
        public const int E_WMPCORE_NOSOURCEURLSTRING = unchecked((int)0xC00D107C);
        public const int E_WMPCORE_COCREATEFAILEDFORGITOBJECT = unchecked((int)0xC00D107D);
        public const int E_WMPCORE_FAILEDTOGETMARSHALLEDEVENTHANDLERINTERFACE = unchecked((int)0xC00D107E);
        public const int E_WMPCORE_BUFFERTOOSMALL = unchecked((int)0xC00D107F);
        public const int E_WMPCORE_UNAVAILABLE = unchecked((int)0xC00D1080);
        public const int E_WMPCORE_INVALIDPLAYLISTMODE = unchecked((int)0xC00D1081);
        public const int E_WMPCORE_ITEMNOTINPLAYLIST = unchecked((int)0xC00D1086);
        public const int E_WMPCORE_PLAYLISTEMPTY = unchecked((int)0xC00D1087);
        public const int E_WMPCORE_NOBROWSER = unchecked((int)0xC00D1088);
        public const int E_WMPCORE_UNRECOGNIZED_MEDIA_URL = unchecked((int)0xC00D1089);
        public const int E_WMPCORE_GRAPH_NOT_IN_LIST = unchecked((int)0xC00D108A);
        public const int E_WMPCORE_PLAYLIST_EMPTY_OR_SINGLE_MEDIA = unchecked((int)0xC00D108B);
        public const int E_WMPCORE_ERRORSINKNOTREGISTERED = unchecked((int)0xC00D108C);
        public const int E_WMPCORE_ERRORMANAGERNOTAVAILABLE = unchecked((int)0xC00D108D);
        public const int E_WMPCORE_WEBHELPFAILED = unchecked((int)0xC00D108E);
        public const int E_WMPCORE_MEDIA_ERROR_RESUME_FAILED = unchecked((int)0xC00D108F);
        public const int E_WMPCORE_NO_REF_IN_ENTRY = unchecked((int)0xC00D1090);
        public const int E_WMPCORE_WMX_LIST_ATTRIBUTE_NAME_EMPTY = unchecked((int)0xC00D1091);
        public const int E_WMPCORE_WMX_LIST_ATTRIBUTE_NAME_ILLEGAL = unchecked((int)0xC00D1092);
        public const int E_WMPCORE_WMX_LIST_ATTRIBUTE_VALUE_EMPTY = unchecked((int)0xC00D1093);
        public const int E_WMPCORE_WMX_LIST_ATTRIBUTE_VALUE_ILLEGAL = unchecked((int)0xC00D1094);
        public const int E_WMPCORE_WMX_LIST_ITEM_ATTRIBUTE_NAME_EMPTY = unchecked((int)0xC00D1095);
        public const int E_WMPCORE_WMX_LIST_ITEM_ATTRIBUTE_NAME_ILLEGAL = unchecked((int)0xC00D1096);
        public const int E_WMPCORE_WMX_LIST_ITEM_ATTRIBUTE_VALUE_EMPTY = unchecked((int)0xC00D1097);
        public const int E_WMPCORE_LIST_ENTRY_NO_REF = unchecked((int)0xC00D1098);
        public const int E_WMPCORE_MISNAMED_FILE = unchecked((int)0xC00D1099);
        public const int E_WMPCORE_CODEC_NOT_TRUSTED = unchecked((int)0xC00D109A);
        public const int E_WMPCORE_CODEC_NOT_FOUND = unchecked((int)0xC00D109B);
        public const int E_WMPCORE_CODEC_DOWNLOAD_NOT_ALLOWED = unchecked((int)0xC00D109C);
        public const int E_WMPCORE_ERROR_DOWNLOADING_PLAYLIST = unchecked((int)0xC00D109D);
        public const int E_WMPCORE_FAILED_TO_BUILD_PLAYLIST = unchecked((int)0xC00D109E);
        public const int E_WMPCORE_PLAYLIST_ITEM_ALTERNATE_NONE = unchecked((int)0xC00D109F);
        public const int E_WMPCORE_PLAYLIST_ITEM_ALTERNATE_EXHAUSTED = unchecked((int)0xC00D10A0);
        public const int E_WMPCORE_PLAYLIST_ITEM_ALTERNATE_NAME_NOT_FOUND = unchecked((int)0xC00D10A1);
        public const int E_WMPCORE_PLAYLIST_ITEM_ALTERNATE_MORPH_FAILED = unchecked((int)0xC00D10A2);
        public const int E_WMPCORE_PLAYLIST_ITEM_ALTERNATE_INIT_FAILED = unchecked((int)0xC00D10A3);
        public const int E_WMPCORE_MEDIA_ALTERNATE_REF_EMPTY = unchecked((int)0xC00D10A4);
        public const int E_WMPCORE_PLAYLIST_NO_EVENT_NAME = unchecked((int)0xC00D10A5);
        public const int E_WMPCORE_PLAYLIST_EVENT_ATTRIBUTE_ABSENT = unchecked((int)0xC00D10A6);
        public const int E_WMPCORE_PLAYLIST_EVENT_EMPTY = unchecked((int)0xC00D10A7);
        public const int E_WMPCORE_PLAYLIST_STACK_EMPTY = unchecked((int)0xC00D10A8);
        public const int E_WMPCORE_CURRENT_MEDIA_NOT_ACTIVE = unchecked((int)0xC00D10A9);
        public const int E_WMPCORE_USER_CANCEL = unchecked((int)0xC00D10AB);
        public const int E_WMPCORE_PLAYLIST_REPEAT_EMPTY = unchecked((int)0xC00D10AC);
        public const int E_WMPCORE_PLAYLIST_REPEAT_START_MEDIA_NONE = unchecked((int)0xC00D10AD);
        public const int E_WMPCORE_PLAYLIST_REPEAT_END_MEDIA_NONE = unchecked((int)0xC00D10AE);
        public const int E_WMPCORE_INVALID_PLAYLIST_URL = unchecked((int)0xC00D10AF);
        public const int E_WMPCORE_MISMATCHED_RUNTIME = unchecked((int)0xC00D10B0);
        public const int E_WMPCORE_PLAYLIST_IMPORT_FAILED_NO_ITEMS = unchecked((int)0xC00D10B1);
        public const int E_WMPCORE_VIDEO_TRANSFORM_FILTER_INSERTION = unchecked((int)0xC00D10B2);
        public const int E_WMPCORE_MEDIA_UNAVAILABLE = unchecked((int)0xC00D10B3);
        public const int E_WMPCORE_WMX_ENTRYREF_NO_REF = unchecked((int)0xC00D10B4);
        public const int E_WMPCORE_NO_PLAYABLE_MEDIA_IN_PLAYLIST = unchecked((int)0xC00D10B5);
        public const int E_WMPCORE_PLAYLIST_EMPTY_NESTED_PLAYLIST_SKIPPED_ITEMS = unchecked((int)0xC00D10B6);
        public const int E_WMPCORE_BUSY = unchecked((int)0xC00D10B7);
        public const int E_WMPCORE_MEDIA_CHILD_PLAYLIST_UNAVAILABLE = unchecked((int)0xC00D10B8);
        public const int E_WMPCORE_MEDIA_NO_CHILD_PLAYLIST = unchecked((int)0xC00D10B9);
        public const int E_WMPCORE_FILE_NOT_FOUND = unchecked((int)0xC00D10BA);
        public const int E_WMPCORE_TEMP_FILE_NOT_FOUND = unchecked((int)0xC00D10BB);
        public const int E_WMDM_REVOKED = unchecked((int)0xC00D10BC);
        public const int E_DDRAW_GENERIC = unchecked((int)0xC00D10BD);
        public const int E_DISPLAY_MODE_CHANGE_FAILED = unchecked((int)0xC00D10BE);
        public const int E_PLAYLIST_CONTAINS_ERRORS = unchecked((int)0xC00D10BF);
        public const int E_CHANGING_PROXY_NAME = unchecked((int)0xC00D10C0);
        public const int E_CHANGING_PROXY_PORT = unchecked((int)0xC00D10C1);
        public const int E_CHANGING_PROXY_EXCEPTIONLIST = unchecked((int)0xC00D10C2);
        public const int E_CHANGING_PROXYBYPASS = unchecked((int)0xC00D10C3);
        public const int E_CHANGING_PROXY_PROTOCOL_NOT_FOUND = unchecked((int)0xC00D10C4);
        public const int E_GRAPH_NOAUDIOLANGUAGE = unchecked((int)0xC00D10C5);
        public const int E_GRAPH_NOAUDIOLANGUAGESELECTED = unchecked((int)0xC00D10C6);
        public const int E_CORECD_NOTAMEDIACD = unchecked((int)0xC00D10C7);
        public const int E_WMPCORE_MEDIA_URL_TOO_LONG = unchecked((int)0xC00D10C8);
        public const int E_WMPFLASH_CANT_FIND_COM_SERVER = unchecked((int)0xC00D10C9);
        public const int E_WMPFLASH_INCOMPATIBLEVERSION = unchecked((int)0xC00D10CA);
        public const int E_WMPOCXGRAPH_IE_DISALLOWS_ACTIVEX_CONTROLS = unchecked((int)0xC00D10CB);
        public const int E_NEED_CORE_REFERENCE = unchecked((int)0xC00D10CC);
        public const int E_MEDIACD_READ_ERROR = unchecked((int)0xC00D10CD);
        public const int E_IE_DISALLOWS_ACTIVEX_CONTROLS = unchecked((int)0xC00D10CE);
        public const int E_FLASH_PLAYBACK_NOT_ALLOWED = unchecked((int)0xC00D10CF);
        public const int E_UNABLE_TO_CREATE_RIP_LOCATION = unchecked((int)0xC00D10D0);
        public const int E_WMPCORE_SOME_CODECS_MISSING = unchecked((int)0xC00D10D1);
        public const int S_WMPCORE_PLAYLISTCLEARABORT = unchecked((int)0x000D10FE);
        public const int S_WMPCORE_PLAYLISTREMOVEITEMABORT = unchecked((int)0x000D10FF);
        public const int S_WMPCORE_PLAYLIST_CREATION_PENDING = unchecked((int)0x000D1102);
        public const int S_WMPCORE_MEDIA_VALIDATION_PENDING = unchecked((int)0x000D1103);
        public const int S_WMPCORE_PLAYLIST_REPEAT_SECONDARY_SEGMENTS_IGNORED = unchecked((int)0x000D1104);
        public const int S_WMPCORE_COMMAND_NOT_AVAILABLE = unchecked((int)0x000D1105);
        public const int S_WMPCORE_PLAYLIST_NAME_AUTO_GENERATED = unchecked((int)0x000D1106);
        public const int S_WMPCORE_PLAYLIST_IMPORT_MISSING_ITEMS = unchecked((int)0x000D1107);
        public const int S_WMPCORE_PLAYLIST_COLLAPSED_TO_SINGLE_MEDIA = unchecked((int)0x000D1108);
        public const int S_WMPCORE_MEDIA_CHILD_PLAYLIST_OPEN_PENDING = unchecked((int)0x000D1109);
        public const int S_WMPCORE_MORE_NODES_AVAIABLE = unchecked((int)0x000D110A);
        public const int E_WMPIM_USEROFFLINE = unchecked((int)0xC00D1126);
        public const int E_WMPIM_USERCANCELED = unchecked((int)0xC00D1127);
        public const int E_WMPIM_DIALUPFAILED = unchecked((int)0xC00D1128);
        public const int E_WINSOCK_ERROR_STRING = unchecked((int)0xC00D1129);
        public const int E_WMPBR_NOLISTENER = unchecked((int)0xC00D1130);
        public const int E_WMPBR_BACKUPCANCEL = unchecked((int)0xC00D1131);
        public const int E_WMPBR_RESTORECANCEL = unchecked((int)0xC00D1132);
        public const int E_WMPBR_ERRORWITHURL = unchecked((int)0xC00D1133);
        public const int E_WMPBR_NAMECOLLISION = unchecked((int)0xC00D1134);
        public const int S_WMPBR_SUCCESS = unchecked((int)0x000D1135);
        public const int S_WMPBR_PARTIALSUCCESS = unchecked((int)0x000D1136);
        public const int E_WMPBR_DRIVE_INVALID = unchecked((int)0xC00D1137);
        public const int S_WMPEFFECT_TRANSPARENT = unchecked((int)0x000D1144);
        public const int S_WMPEFFECT_OPAQUE = unchecked((int)0x000D1145);
        public const int S_OPERATION_PENDING = unchecked((int)0x000D114E);
        public const int E_DVD_NO_SUBPICTURE_STREAM = unchecked((int)0xC00D1162);
        public const int E_DVD_COPY_PROTECT = unchecked((int)0xC00D1163);
        public const int E_DVD_AUTHORING_PROBLEM = unchecked((int)0xC00D1164);
        public const int E_DVD_INVALID_DISC_REGION = unchecked((int)0xC00D1165);
        public const int E_DVD_COMPATIBLE_VIDEO_CARD = unchecked((int)0xC00D1166);
        public const int E_DVD_MACROVISION = unchecked((int)0xC00D1167);
        public const int E_DVD_SYSTEM_DECODER_REGION = unchecked((int)0xC00D1168);
        public const int E_DVD_DISC_DECODER_REGION = unchecked((int)0xC00D1169);
        public const int E_DVD_NO_VIDEO_STREAM = unchecked((int)0xC00D116A);
        public const int E_DVD_NO_AUDIO_STREAM = unchecked((int)0xC00D116B);
        public const int E_DVD_GRAPH_BUILDING = unchecked((int)0xC00D116C);
        public const int E_DVD_NO_DECODER = unchecked((int)0xC00D116D);
        public const int E_DVD_PARENTAL = unchecked((int)0xC00D116E);
        public const int E_DVD_CANNOT_JUMP = unchecked((int)0xC00D116F);
        public const int E_DVD_DEVICE_CONTENTION = unchecked((int)0xC00D1170);
        public const int E_DVD_NO_VIDEO_MEMORY = unchecked((int)0xC00D1171);
        public const int E_DVD_CANNOT_COPY_PROTECTED = unchecked((int)0xC00D1172);
        public const int E_DVD_REQUIRED_PROPERTY_NOT_SET = unchecked((int)0xC00D1173);
        public const int E_DVD_INVALID_TITLE_CHAPTER = unchecked((int)0xC00D1174);
        public const int E_NO_CD_BURNER = unchecked((int)0xC00D1176);
        public const int E_DEVICE_IS_NOT_READY = unchecked((int)0xC00D1177);
        public const int E_PDA_UNSUPPORTED_FORMAT = unchecked((int)0xC00D1178);
        public const int E_NO_PDA = unchecked((int)0xC00D1179);
        public const int E_PDA_UNSPECIFIED_ERROR = unchecked((int)0xC00D117A);
        public const int E_MEMSTORAGE_BAD_DATA = unchecked((int)0xC00D117B);
        public const int E_PDA_FAIL_SELECT_DEVICE = unchecked((int)0xC00D117C);
        public const int E_PDA_FAIL_READ_WAVE_FILE = unchecked((int)0xC00D117D);
        public const int E_IMAPI_LOSSOFSTREAMING = unchecked((int)0xC00D117E);
        public const int E_PDA_DEVICE_FULL = unchecked((int)0xC00D117F);
        public const int E_FAIL_LAUNCH_ROXIO_PLUGIN = unchecked((int)0xC00D1180);
        public const int E_PDA_DEVICE_FULL_IN_SESSION = unchecked((int)0xC00D1181);
        public const int E_IMAPI_MEDIUM_INVALIDTYPE = unchecked((int)0xC00D1182);
        public const int E_WMP_PROTOCOL_PROBLEM = unchecked((int)0xC00D1194);
        public const int E_WMP_NO_DISK_SPACE = unchecked((int)0xC00D1195);
        public const int E_WMP_LOGON_FAILURE = unchecked((int)0xC00D1196);
        public const int E_WMP_CANNOT_FIND_FILE = unchecked((int)0xC00D1197);
        public const int E_WMP_SERVER_INACCESSIBLE = unchecked((int)0xC00D1198);
        public const int E_WMP_UNSUPPORTED_FORMAT = unchecked((int)0xC00D1199);
        public const int E_WMP_DSHOW_UNSUPPORTED_FORMAT = unchecked((int)0xC00D119A);
        public const int E_WMP_PLAYLIST_EXISTS = unchecked((int)0xC00D119B);
        public const int E_WMP_NONMEDIA_FILES = unchecked((int)0xC00D119C);
        public const int E_WMP_INVALID_ASX = unchecked((int)0xC00D119D);
        public const int E_WMP_ALREADY_IN_USE = unchecked((int)0xC00D119E);
        public const int E_WMP_IMAPI_FAILURE = unchecked((int)0xC00D119F);
        public const int E_WMP_WMDM_FAILURE = unchecked((int)0xC00D11A0);
        public const int E_WMP_CODEC_NEEDED_WITH_4CC = unchecked((int)0xC00D11A1);
        public const int E_WMP_CODEC_NEEDED_WITH_FORMATTAG = unchecked((int)0xC00D11A2);
        public const int E_WMP_MSSAP_NOT_AVAILABLE = unchecked((int)0xC00D11A3);
        public const int E_WMP_WMDM_INTERFACEDEAD = unchecked((int)0xC00D11A4);
        public const int E_WMP_WMDM_NOTCERTIFIED = unchecked((int)0xC00D11A5);
        public const int E_WMP_WMDM_LICENSE_NOTEXIST = unchecked((int)0xC00D11A6);
        public const int E_WMP_WMDM_LICENSE_EXPIRED = unchecked((int)0xC00D11A7);
        public const int E_WMP_WMDM_BUSY = unchecked((int)0xC00D11A8);
        public const int E_WMP_WMDM_NORIGHTS = unchecked((int)0xC00D11A9);
        public const int E_WMP_WMDM_INCORRECT_RIGHTS = unchecked((int)0xC00D11AA);
        public const int E_WMP_IMAPI_GENERIC = unchecked((int)0xC00D11AB);
        public const int E_WMP_IMAPI_DEVICE_NOTPRESENT = unchecked((int)0xC00D11AD);
        public const int E_WMP_IMAPI_STASHINUSE = unchecked((int)0xC00D11AE);
        public const int E_WMP_IMAPI_LOSS_OF_STREAMING = unchecked((int)0xC00D11AF);
        public const int E_WMP_SERVER_UNAVAILABLE = unchecked((int)0xC00D11B0);
        public const int E_WMP_FILE_OPEN_FAILED = unchecked((int)0xC00D11B1);
        public const int E_WMP_VERIFY_ONLINE = unchecked((int)0xC00D11B2);
        public const int E_WMP_SERVER_NOT_RESPONDING = unchecked((int)0xC00D11B3);
        public const int E_WMP_DRM_CORRUPT_BACKUP = unchecked((int)0xC00D11B4);
        public const int E_WMP_DRM_LICENSE_SERVER_UNAVAILABLE = unchecked((int)0xC00D11B5);
        public const int E_WMP_NETWORK_FIREWALL = unchecked((int)0xC00D11B6);
        public const int E_WMP_NO_REMOVABLE_MEDIA = unchecked((int)0xC00D11B7);
        public const int E_WMP_PROXY_CONNECT_TIMEOUT = unchecked((int)0xC00D11B8);
        public const int E_WMP_NEED_UPGRADE = unchecked((int)0xC00D11B9);
        public const int E_WMP_AUDIO_HW_PROBLEM = unchecked((int)0xC00D11BA);
        public const int E_WMP_INVALID_PROTOCOL = unchecked((int)0xC00D11BB);
        public const int E_WMP_INVALID_LIBRARY_ADD = unchecked((int)0xC00D11BC);
        public const int E_WMP_MMS_NOT_SUPPORTED = unchecked((int)0xC00D11BD);
        public const int E_WMP_NO_PROTOCOLS_SELECTED = unchecked((int)0xC00D11BE);
        public const int E_WMP_GOFULLSCREEN_FAILED = unchecked((int)0xC00D11BF);
        public const int E_WMP_NETWORK_ERROR = unchecked((int)0xC00D11C0);
        public const int E_WMP_CONNECT_TIMEOUT = unchecked((int)0xC00D11C1);
        public const int E_WMP_MULTICAST_DISABLED = unchecked((int)0xC00D11C2);
        public const int E_WMP_SERVER_DNS_TIMEOUT = unchecked((int)0xC00D11C3);
        public const int E_WMP_PROXY_NOT_FOUND = unchecked((int)0xC00D11C4);
        public const int E_WMP_TAMPERED_CONTENT = unchecked((int)0xC00D11C5);
        public const int E_WMP_OUTOFMEMORY = unchecked((int)0xC00D11C6);
        public const int E_WMP_AUDIO_CODEC_NOT_INSTALLED = unchecked((int)0xC00D11C7);
        public const int E_WMP_VIDEO_CODEC_NOT_INSTALLED = unchecked((int)0xC00D11C8);
        public const int E_WMP_IMAPI_DEVICE_INVALIDTYPE = unchecked((int)0xC00D11C9);
        public const int E_WMP_DRM_DRIVER_AUTH_FAILURE = unchecked((int)0xC00D11CA);
        public const int E_WMP_NETWORK_RESOURCE_FAILURE = unchecked((int)0xC00D11CB);
        public const int E_WMP_UPGRADE_APPLICATION = unchecked((int)0xC00D11CC);
        public const int E_WMP_UNKNOWN_ERROR = unchecked((int)0xC00D11CD);
        public const int E_WMP_INVALID_KEY = unchecked((int)0xC00D11CE);
        public const int E_WMP_CD_ANOTHER_USER = unchecked((int)0xC00D11CF);
        public const int E_WMP_DRM_NEEDS_AUTHORIZATION = unchecked((int)0xC00D11D0);
        public const int E_WMP_BAD_DRIVER = unchecked((int)0xC00D11D1);
        public const int E_WMP_ACCESS_DENIED = unchecked((int)0xC00D11D2);
        public const int E_WMP_LICENSE_RESTRICTS = unchecked((int)0xC00D11D3);
        public const int E_WMP_INVALID_REQUEST = unchecked((int)0xC00D11D4);
        public const int E_WMP_CD_STASH_NO_SPACE = unchecked((int)0xC00D11D5);
        public const int E_WMP_DRM_NEW_HARDWARE = unchecked((int)0xC00D11D6);
        public const int E_WMP_DRM_INVALID_SIG = unchecked((int)0xC00D11D7);
        public const int E_WMP_DRM_CANNOT_RESTORE = unchecked((int)0xC00D11D8);
        public const int E_CD_NO_BUFFERS_READ = unchecked((int)0xC00D11F8);
        public const int E_CD_EMPTY_TRACK_QUEUE = unchecked((int)0xC00D11F9);
        public const int E_CD_NO_READER = unchecked((int)0xC00D11FA);
        public const int E_CD_ISRC_INVALID = unchecked((int)0xC00D11FB);
        public const int E_CD_MEDIA_CATALOG_NUMBER_INVALID = unchecked((int)0xC00D11FC);
        public const int E_SLOW_READ_DIGITAL_WITH_ERRORCORRECTION = unchecked((int)0xC00D11FD);
        public const int E_CD_SPEEDDETECT_NOT_ENOUGH_READS = unchecked((int)0xC00D11FE);
        public const int E_CD_QUEUEING_DISABLED = unchecked((int)0xC00D11FF);
        public const int E_WMP_POLICY_VALUE_NOT_CONFIGURED = unchecked((int)0xC00D122A);
        public const int E_WMP_HWND_NOTFOUND = unchecked((int)0xC00D125C);
        public const int E_BKGDOWNLOAD_WRONG_NO_FILES = unchecked((int)0xC00D125D);
        public const int E_BKGDOWNLOAD_COMPLETECANCELLEDJOB = unchecked((int)0xC00D125E);
        public const int E_BKGDOWNLOAD_CANCELCOMPLETEDJOB = unchecked((int)0xC00D125F);
        public const int E_BKGDOWNLOAD_NOJOBPOINTER = unchecked((int)0xC00D1260);
        public const int E_BKGDOWNLOAD_INVALIDJOBSIGNATURE = unchecked((int)0xC00D1261);
        public const int E_BKGDOWNLOAD_FAILED_TO_CREATE_TEMPFILE = unchecked((int)0xC00D1262);
        public const int E_BKGDOWNLOAD_PLUGIN_FAILEDINITIALIZE = unchecked((int)0xC00D1263);
        public const int E_BKGDOWNLOAD_PLUGIN_FAILEDTOMOVEFILE = unchecked((int)0xC00D1264);
        public const int E_BKGDOWNLOAD_CALLFUNCFAILED = unchecked((int)0xC00D1265);
        public const int E_BKGDOWNLOAD_CALLFUNCTIMEOUT = unchecked((int)0xC00D1266);
        public const int E_BKGDOWNLOAD_CALLFUNCENDED = unchecked((int)0xC00D1267);
        public const int E_BKGDOWNLOAD_WMDUNPACKFAILED = unchecked((int)0xC00D1268);
        public const int E_BKGDOWNLOAD_FAILEDINITIALIZE = unchecked((int)0xC00D1269);
        public const int E_INTERFACE_NOT_REGISTERED_IN_GIT = unchecked((int)0xC00D126A);
        public const int E_BKGDOWNLOAD_INVALID_FILE_NAME = unchecked((int)0xC00D126B);
        public const int E_IMAGE_DOWNLOAD_FAILED = unchecked((int)0xC00D128E);
        public const int E_WMP_UDRM_NOUSERLIST = unchecked((int)0xC00D12C0);
        public const int E_WMP_DRM_NOT_ACQUIRING = unchecked((int)0xC00D12C1);
        public const int E_WMP_BSTR_TOO_LONG = unchecked((int)0xC00D12F2);
        public const int E_WMP_AUTOPLAY_INVALID_STATE = unchecked((int)0xC00D12FC);
        public const int E_CURL_NOTSAFE = unchecked((int)0xC00D1324);
        public const int E_CURL_INVALIDCHAR = unchecked((int)0xC00D1325);
        public const int E_CURL_INVALIDHOSTNAME = unchecked((int)0xC00D1326);
        public const int E_CURL_INVALIDPATH = unchecked((int)0xC00D1327);
        public const int E_CURL_INVALIDSCHEME = unchecked((int)0xC00D1328);
        public const int E_CURL_INVALIDURL = unchecked((int)0xC00D1329);
        public const int E_CURL_CANTWALK = unchecked((int)0xC00D132B);
        public const int E_CURL_INVALIDPORT = unchecked((int)0xC00D132C);
        public const int E_CURLHELPER_NOTADIRECTORY = unchecked((int)0xC00D132D);
        public const int E_CURLHELPER_NOTAFILE = unchecked((int)0xC00D132E);
        public const int E_CURL_CANTDECODE = unchecked((int)0xC00D132F);
        public const int E_CURLHELPER_NOTRELATIVE = unchecked((int)0xC00D1330);
        public const int E_CURL_INVALIDBUFFERSIZE = unchecked((int)0xC00D1355);
        public const int E_SUBSCRIPTIONSERVICE_PLAYBACK_DISALLOWED = unchecked((int)0xC00D1356);
        public const int E_ADVANCEDEDIT_TOO_MANY_PICTURES = unchecked((int)0xC00D136A);
        public const int E_REDIRECT = unchecked((int)0xC00D1388);
        public const int E_STALE_PRESENTATION = unchecked((int)0xC00D1389);
        public const int E_NAMESPACE_WRONG_PERSIST = unchecked((int)0xC00D138A);
        public const int E_NAMESPACE_WRONG_TYPE = unchecked((int)0xC00D138B);
        public const int E_NAMESPACE_NODE_CONFLICT = unchecked((int)0xC00D138C);
        public const int E_NAMESPACE_NODE_NOT_FOUND = unchecked((int)0xC00D138D);
        public const int E_NAMESPACE_BUFFER_TOO_SMALL = unchecked((int)0xC00D138E);
        public const int E_NAMESPACE_TOO_MANY_CALLBACKS = unchecked((int)0xC00D138F);
        public const int E_NAMESPACE_DUPLICATE_CALLBACK = unchecked((int)0xC00D1390);
        public const int E_NAMESPACE_CALLBACK_NOT_FOUND = unchecked((int)0xC00D1391);
        public const int E_NAMESPACE_NAME_TOO_LONG = unchecked((int)0xC00D1392);
        public const int E_NAMESPACE_DUPLICATE_NAME = unchecked((int)0xC00D1393);
        public const int E_NAMESPACE_EMPTY_NAME = unchecked((int)0xC00D1394);
        public const int E_NAMESPACE_INDEX_TOO_LARGE = unchecked((int)0xC00D1395);
        public const int E_NAMESPACE_BAD_NAME = unchecked((int)0xC00D1396);
        public const int E_NAMESPACE_WRONG_SECURITY = unchecked((int)0xC00D1397);
        public const int E_CACHE_ARCHIVE_CONFLICT = unchecked((int)0xC00D13EC);
        public const int E_CACHE_ORIGIN_SERVER_NOT_FOUND = unchecked((int)0xC00D13ED);
        public const int E_CACHE_ORIGIN_SERVER_TIMEOUT = unchecked((int)0xC00D13EE);
        public const int E_CACHE_NOT_BROADCAST = unchecked((int)0xC00D13EF);
        public const int E_CACHE_CANNOT_BE_CACHED = unchecked((int)0xC00D13F0);
        public const int E_CACHE_NOT_MODIFIED = unchecked((int)0xC00D13F1);
        public const int E_CANNOT_REMOVE_PUBLISHING_POINT = unchecked((int)0xC00D1450);
        public const int E_CANNOT_REMOVE_PLUGIN = unchecked((int)0xC00D1451);
        public const int E_WRONG_PUBLISHING_POINT_TYPE = unchecked((int)0xC00D1452);
        public const int E_UNSUPPORTED_LOAD_TYPE = unchecked((int)0xC00D1453);
        public const int E_INVALID_PLUGIN_LOAD_TYPE_CONFIGURATION = unchecked((int)0xC00D1454);
        public const int E_INVALID_PUBLISHING_POINT_NAME = unchecked((int)0xC00D1455);
        public const int E_TOO_MANY_MULTICAST_SINKS = unchecked((int)0xC00D1456);
        public const int E_PUBLISHING_POINT_INVALID_REQUEST_WHILE_STARTED = unchecked((int)0xC00D1457);
        public const int E_MULTICAST_PLUGIN_NOT_ENABLED = unchecked((int)0xC00D1458);
        public const int E_INVALID_OPERATING_SYSTEM_VERSION = unchecked((int)0xC00D1459);
        public const int E_PUBLISHING_POINT_REMOVED = unchecked((int)0xC00D145A);
        public const int E_INVALID_PUSH_PUBLISHING_POINT_START_REQUEST = unchecked((int)0xC00D145B);
        public const int E_UNSUPPORTED_LANGUAGE = unchecked((int)0xC00D145C);
        public const int E_WRONG_OS_VERSION = unchecked((int)0xC00D145D);
        public const int E_PUBLISHING_POINT_STOPPED = unchecked((int)0xC00D145E);
        public const int E_PLAYLIST_ENTRY_ALREADY_PLAYING = unchecked((int)0xC00D14B4);
        public const int E_EMPTY_PLAYLIST = unchecked((int)0xC00D14B5);
        public const int E_PLAYLIST_PARSE_FAILURE = unchecked((int)0xC00D14B6);
        public const int E_PLAYLIST_UNSUPPORTED_ENTRY = unchecked((int)0xC00D14B7);
        public const int E_PLAYLIST_ENTRY_NOT_IN_PLAYLIST = unchecked((int)0xC00D14B8);
        public const int E_PLAYLIST_ENTRY_SEEK = unchecked((int)0xC00D14B9);
        public const int E_PLAYLIST_RECURSIVE_PLAYLISTS = unchecked((int)0xC00D14BA);
        public const int E_PLAYLIST_TOO_MANY_NESTED_PLAYLISTS = unchecked((int)0xC00D14BB);
        public const int E_PLAYLIST_SHUTDOWN = unchecked((int)0xC00D14BC);
        public const int E_PLAYLIST_END_RECEDING = unchecked((int)0xC00D14BD);
        public const int I_PLAYLIST_CHANGE_RECEDING = unchecked((int)0x400D14BE);
        public const int E_DATAPATH_NO_SINK = unchecked((int)0xC00D1518);
        public const int S_PUBLISHING_POINT_STARTED_WITH_FAILED_SINKS = unchecked((int)0x000D1519);
        public const int E_INVALID_PUSH_TEMPLATE = unchecked((int)0xC00D151A);
        public const int E_INVALID_PUSH_PUBLISHING_POINT = unchecked((int)0xC00D151B);
        public const int E_CRITICAL_ERROR = unchecked((int)0xC00D151C);
        public const int E_NO_NEW_CONNECTIONS = unchecked((int)0xC00D151D);
        public const int E_WSX_INVALID_VERSION = unchecked((int)0xC00D151E);
        public const int E_HEADER_MISMATCH = unchecked((int)0xC00D151F);
        public const int E_PUSH_DUPLICATE_PUBLISHING_POINT_NAME = unchecked((int)0xC00D1520);
        public const int E_NO_SCRIPT_ENGINE = unchecked((int)0xC00D157C);
        public const int E_PLUGIN_ERROR_REPORTED = unchecked((int)0xC00D157D);
        public const int E_SOURCE_PLUGIN_NOT_FOUND = unchecked((int)0xC00D157E);
        public const int E_PLAYLIST_PLUGIN_NOT_FOUND = unchecked((int)0xC00D157F);
        public const int E_DATA_SOURCE_ENUMERATION_NOT_SUPPORTED = unchecked((int)0xC00D1580);
        public const int E_MEDIA_PARSER_INVALID_FORMAT = unchecked((int)0xC00D1581);
        public const int E_SCRIPT_DEBUGGER_NOT_INSTALLED = unchecked((int)0xC00D1582);
        public const int E_FEATURE_REQUIRES_ENTERPRISE_SERVER = unchecked((int)0xC00D1583);
        public const int E_WIZARD_RUNNING = unchecked((int)0xC00D1584);
        public const int E_INVALID_LOG_URL = unchecked((int)0xC00D1585);
        public const int E_INVALID_MTU_RANGE = unchecked((int)0xC00D1586);
        public const int E_INVALID_PLAY_STATISTICS = unchecked((int)0xC00D1587);
        public const int E_LOG_NEED_TO_BE_SKIPPED = unchecked((int)0xC00D1588);
        public const int E_HTTP_TEXT_DATACONTAINER_SIZE_LIMIT_EXCEEDED = unchecked((int)0xC00D1589);
        public const int E_PORT_IN_USE = unchecked((int)0xC00D158A);
        public const int E_PORT_IN_USE_HTTP = unchecked((int)0xC00D158B);
        public const int E_HTTP_TEXT_DATACONTAINER_INVALID_SERVER_RESPONSE = unchecked((int)0xC00D158C);
        public const int E_ARCHIVE_REACH_QUOTA = unchecked((int)0xC00D158D);
        public const int E_ARCHIVE_ABORT_DUE_TO_BCAST = unchecked((int)0xC00D158E);
        public const int E_ARCHIVE_GAP_DETECTED = unchecked((int)0xC00D158F);
        public const int E_BAD_MARKIN = unchecked((int)0xC00D1B58);
        public const int E_BAD_MARKOUT = unchecked((int)0xC00D1B59);
        public const int E_NOMATCHING_MEDIASOURCE = unchecked((int)0xC00D1B5A);
        public const int E_UNSUPPORTED_SOURCETYPE = unchecked((int)0xC00D1B5B);
        public const int E_TOO_MANY_AUDIO = unchecked((int)0xC00D1B5C);
        public const int E_TOO_MANY_VIDEO = unchecked((int)0xC00D1B5D);
        public const int E_NOMATCHING_ELEMENT = unchecked((int)0xC00D1B5E);
        public const int E_MISMATCHED_MEDIACONTENT = unchecked((int)0xC00D1B5F);
        public const int E_CANNOT_DELETE_ACTIVE_SOURCEGROUP = unchecked((int)0xC00D1B60);
        public const int E_AUDIODEVICE_BUSY = unchecked((int)0xC00D1B61);
        public const int E_AUDIODEVICE_UNEXPECTED = unchecked((int)0xC00D1B62);
        public const int E_AUDIODEVICE_BADFORMAT = unchecked((int)0xC00D1B63);
        public const int E_VIDEODEVICE_BUSY = unchecked((int)0xC00D1B64);
        public const int E_VIDEODEVICE_UNEXPECTED = unchecked((int)0xC00D1B65);
        public const int E_INVALIDCALL_WHILE_ENCODER_RUNNING = unchecked((int)0xC00D1B66);
        public const int E_NO_PROFILE_IN_SOURCEGROUP = unchecked((int)0xC00D1B67);
        public const int E_VIDEODRIVER_UNSTABLE = unchecked((int)0xC00D1B68);
        public const int E_VIDCAPSTARTFAILED = unchecked((int)0xC00D1B69);
        public const int E_VIDSOURCECOMPRESSION = unchecked((int)0xC00D1B6A);
        public const int E_VIDSOURCESIZE = unchecked((int)0xC00D1B6B);
        public const int E_ICMQUERYFORMAT = unchecked((int)0xC00D1B6C);
        public const int E_VIDCAPCREATEWINDOW = unchecked((int)0xC00D1B6D);
        public const int E_VIDCAPDRVINUSE = unchecked((int)0xC00D1B6E);
        public const int E_NO_MEDIAFORMAT_IN_SOURCE = unchecked((int)0xC00D1B6F);
        public const int E_NO_VALID_OUTPUT_STREAM = unchecked((int)0xC00D1B70);
        public const int E_NO_VALID_SOURCE_PLUGIN = unchecked((int)0xC00D1B71);
        public const int E_NO_ACTIVE_SOURCEGROUP = unchecked((int)0xC00D1B72);
        public const int E_NO_SCRIPT_STREAM = unchecked((int)0xC00D1B73);
        public const int E_INVALIDCALL_WHILE_ARCHIVAL_RUNNING = unchecked((int)0xC00D1B74);
        public const int E_INVALIDPACKETSIZE = unchecked((int)0xC00D1B75);
        public const int E_PLUGIN_CLSID_INVALID = unchecked((int)0xC00D1B76);
        public const int E_UNSUPPORTED_ARCHIVETYPE = unchecked((int)0xC00D1B77);
        public const int E_UNSUPPORTED_ARCHIVEOPERATION = unchecked((int)0xC00D1B78);
        public const int E_ARCHIVE_FILENAME_NOTSET = unchecked((int)0xC00D1B79);
        public const int E_SOURCEGROUP_NOTPREPARED = unchecked((int)0xC00D1B7A);
        public const int E_PROFILE_MISMATCH = unchecked((int)0xC00D1B7B);
        public const int E_INCORRECTCLIPSETTINGS = unchecked((int)0xC00D1B7C);
        public const int E_NOSTATSAVAILABLE = unchecked((int)0xC00D1B7D);
        public const int E_NOTARCHIVING = unchecked((int)0xC00D1B7E);
        public const int E_INVALIDCALL_WHILE_ENCODER_STOPPED = unchecked((int)0xC00D1B7F);
        public const int E_NOSOURCEGROUPS = unchecked((int)0xC00D1B80);
        public const int E_INVALIDINPUTFPS = unchecked((int)0xC00D1B81);
        public const int E_NO_DATAVIEW_SUPPORT = unchecked((int)0xC00D1B82);
        public const int E_CODEC_UNAVAILABLE = unchecked((int)0xC00D1B83);
        public const int E_ARCHIVE_SAME_AS_INPUT = unchecked((int)0xC00D1B84);
        public const int E_SOURCE_NOTSPECIFIED = unchecked((int)0xC00D1B85);
        public const int E_NO_REALTIME_TIMECOMPRESSION = unchecked((int)0xC00D1B86);
        public const int E_UNSUPPORTED_ENCODER_DEVICE = unchecked((int)0xC00D1B87);
        public const int E_UNEXPECTED_DISPLAY_SETTINGS = unchecked((int)0xC00D1B88);
        public const int E_NO_AUDIODATA = unchecked((int)0xC00D1B89);
        public const int E_INPUTSOURCE_PROBLEM = unchecked((int)0xC00D1B8A);
        public const int E_WME_VERSION_MISMATCH = unchecked((int)0xC00D1B8B);
        public const int E_NO_REALTIME_PREPROCESS = unchecked((int)0xC00D1B8C);
        public const int E_NO_REPEAT_PREPROCESS = unchecked((int)0xC00D1B8D);
        public const int E_CANNOT_PAUSE_LIVEBROADCAST = unchecked((int)0xC00D1B8E);
        public const int E_DRM_PROFILE_NOT_SET = unchecked((int)0xC00D1B8F);
        public const int E_DUPLICATE_DRMPROFILE = unchecked((int)0xC00D1B90);
        public const int E_INVALID_DEVICE = unchecked((int)0xC00D1B91);
        public const int E_SPEECHEDL_ON_NON_MIXEDMODE = unchecked((int)0xC00D1B92);
        public const int E_DRM_PASSWORD_TOO_LONG = unchecked((int)0xC00D1B93);
        public const int E_DEVCONTROL_FAILED_SEEK = unchecked((int)0xC00D1B94);
        public const int E_INTERLACE_REQUIRE_SAMESIZE = unchecked((int)0xC00D1B95);
        public const int E_TOO_MANY_DEVICECONTROL = unchecked((int)0xC00D1B96);
        public const int E_NO_MULTIPASS_FOR_LIVEDEVICE = unchecked((int)0xC00D1B97);
        public const int E_MISSING_AUDIENCE = unchecked((int)0xC00D1B98);
        public const int E_AUDIENCE_CONTENTTYPE_MISMATCH = unchecked((int)0xC00D1B99);
        public const int E_MISSING_SOURCE_INDEX = unchecked((int)0xC00D1B9A);
        public const int E_NUM_LANGUAGE_MISMATCH = unchecked((int)0xC00D1B9B);
        public const int E_LANGUAGE_MISMATCH = unchecked((int)0xC00D1B9C);
        public const int E_VBRMODE_MISMATCH = unchecked((int)0xC00D1B9D);
        public const int E_INVALID_INPUT_AUDIENCE_INDEX = unchecked((int)0xC00D1B9E);
        public const int E_INVALID_INPUT_LANGUAGE = unchecked((int)0xC00D1B9F);
        public const int E_INVALID_INPUT_STREAM = unchecked((int)0xC00D1BA0);
        public const int E_EXPECT_MONO_WAV_INPUT = unchecked((int)0xC00D1BA1);
        public const int E_INPUT_WAVFORMAT_MISMATCH = unchecked((int)0xC00D1BA2);
        public const int E_RECORDQ_DISK_FULL = unchecked((int)0xC00D1BA3);
        public const int E_NO_PAL_INVERSE_TELECINE = unchecked((int)0xC00D1BA4);
        public const int E_ACTIVE_SG_DEVICE_DISCONNECTED = unchecked((int)0xC00D1BA5);
        public const int E_ACTIVE_SG_DEVICE_CONTROL_DISCONNECTED = unchecked((int)0xC00D1BA6);
        public const int E_NO_FRAMES_SUBMITTED_TO_ANALYZER = unchecked((int)0xC00D1BA7);
        public const int E_INPUT_DOESNOT_SUPPORT_SMPTE = unchecked((int)0xC00D1BA8);
        public const int E_NO_SMPTE_WITH_MULTIPLE_SOURCEGROUPS = unchecked((int)0xC00D1BA9);
        public const int E_BAD_CONTENTEDL = unchecked((int)0xC00D1BAA);
        public const int E_INTERLACEMODE_MISMATCH = unchecked((int)0xC00D1BAB);
        public const int E_NONSQUAREPIXELMODE_MISMATCH = unchecked((int)0xC00D1BAC);
        public const int E_SMPTEMODE_MISMATCH = unchecked((int)0xC00D1BAD);
        public const int E_END_OF_TAPE = unchecked((int)0xC00D1BAE);
        public const int E_NO_MEDIA_IN_AUDIENCE = unchecked((int)0xC00D1BAF);
        public const int E_NO_AUDIENCES = unchecked((int)0xC00D1BB0);
        public const int E_NO_AUDIO_COMPAT = unchecked((int)0xC00D1BB1);
        public const int E_INVALID_VBR_COMPAT = unchecked((int)0xC00D1BB2);
        public const int E_NO_PROFILE_NAME = unchecked((int)0xC00D1BB3);
        public const int E_INVALID_VBR_WITH_UNCOMP = unchecked((int)0xC00D1BB4);
        public const int E_MULTIPLE_VBR_AUDIENCES = unchecked((int)0xC00D1BB5);
        public const int E_UNCOMP_COMP_COMBINATION = unchecked((int)0xC00D1BB6);
        public const int E_MULTIPLE_AUDIO_CODECS = unchecked((int)0xC00D1BB7);
        public const int E_MULTIPLE_AUDIO_FORMATS = unchecked((int)0xC00D1BB8);
        public const int E_AUDIO_BITRATE_STEPDOWN = unchecked((int)0xC00D1BB9);
        public const int E_INVALID_AUDIO_PEAKRATE = unchecked((int)0xC00D1BBA);
        public const int E_INVALID_AUDIO_PEAKRATE_2 = unchecked((int)0xC00D1BBB);
        public const int E_INVALID_AUDIO_BUFFERMAX = unchecked((int)0xC00D1BBC);
        public const int E_MULTIPLE_VIDEO_CODECS = unchecked((int)0xC00D1BBD);
        public const int E_MULTIPLE_VIDEO_SIZES = unchecked((int)0xC00D1BBE);
        public const int E_INVALID_VIDEO_BITRATE = unchecked((int)0xC00D1BBF);
        public const int E_VIDEO_BITRATE_STEPDOWN = unchecked((int)0xC00D1BC0);
        public const int E_INVALID_VIDEO_PEAKRATE = unchecked((int)0xC00D1BC1);
        public const int E_INVALID_VIDEO_PEAKRATE_2 = unchecked((int)0xC00D1BC2);
        public const int E_INVALID_VIDEO_WIDTH = unchecked((int)0xC00D1BC3);
        public const int E_INVALID_VIDEO_HEIGHT = unchecked((int)0xC00D1BC4);
        public const int E_INVALID_VIDEO_FPS = unchecked((int)0xC00D1BC5);
        public const int E_INVALID_VIDEO_KEYFRAME = unchecked((int)0xC00D1BC6);
        public const int E_INVALID_VIDEO_IQUALITY = unchecked((int)0xC00D1BC7);
        public const int E_INVALID_VIDEO_CQUALITY = unchecked((int)0xC00D1BC8);
        public const int E_INVALID_VIDEO_BUFFER = unchecked((int)0xC00D1BC9);
        public const int E_INVALID_VIDEO_BUFFERMAX = unchecked((int)0xC00D1BCA);
        public const int E_INVALID_VIDEO_BUFFERMAX_2 = unchecked((int)0xC00D1BCB);
        public const int E_INVALID_VIDEO_WIDTH_ALIGN = unchecked((int)0xC00D1BCC);
        public const int E_INVALID_VIDEO_HEIGHT_ALIGN = unchecked((int)0xC00D1BCD);
        public const int E_MULTIPLE_SCRIPT_BITRATES = unchecked((int)0xC00D1BCE);
        public const int E_INVALID_SCRIPT_BITRATE = unchecked((int)0xC00D1BCF);
        public const int E_MULTIPLE_FILE_BITRATES = unchecked((int)0xC00D1BD0);
        public const int E_INVALID_FILE_BITRATE = unchecked((int)0xC00D1BD1);
        public const int E_SAME_AS_INPUT_COMBINATION = unchecked((int)0xC00D1BD2);
        public const int E_SOURCE_CANNOT_LOOP = unchecked((int)0xC00D1BD3);
        public const int E_INVALID_FOLDDOWN_COEFFICIENTS = unchecked((int)0xC00D1BD4);
        public const int E_DRMPROFILE_NOTFOUND = unchecked((int)0xC00D1BD5);
        public const int E_INVALID_TIMECODE = unchecked((int)0xC00D1BD6);
        public const int E_NO_AUDIO_TIMECOMPRESSION = unchecked((int)0xC00D1BD7);
        public const int E_NO_TWOPASS_TIMECOMPRESSION = unchecked((int)0xC00D1BD8);
        public const int E_TIMECODE_REQUIRES_VIDEOSTREAM = unchecked((int)0xC00D1BD9);
        public const int E_NO_MBR_WITH_TIMECODE = unchecked((int)0xC00D1BDA);
        public const int E_INVALID_INTERLACEMODE = unchecked((int)0xC00D1BDB);
        public const int E_INVALID_INTERLACE_COMPAT = unchecked((int)0xC00D1BDC);
        public const int E_INVALID_NONSQUAREPIXEL_COMPAT = unchecked((int)0xC00D1BDD);
        public const int E_INVALID_SOURCE_WITH_DEVICE_CONTROL = unchecked((int)0xC00D1BDE);
        public const int E_CANNOT_GENERATE_BROADCAST_INFO_FOR_QUALITYVBR = unchecked((int)0xC00D1BDF);
        public const int E_EXCEED_MAX_DRM_PROFILE_LIMIT = unchecked((int)0xC00D1BE0);
        public const int E_DEVICECONTROL_UNSTABLE = unchecked((int)0xC00D1BE1);
        public const int E_INVALID_PIXEL_ASPECT_RATIO = unchecked((int)0xC00D1BE2);
        public const int E_AUDIENCE__LANGUAGE_CONTENTTYPE_MISMATCH = unchecked((int)0xC00D1BE3);
        public const int E_INVALID_PROFILE_CONTENTTYPE = unchecked((int)0xC00D1BE4);
        public const int E_TRANSFORM_PLUGIN_NOT_FOUND = unchecked((int)0xC00D1BE5);
        public const int E_TRANSFORM_PLUGIN_INVALID = unchecked((int)0xC00D1BE6);
        public const int E_EDL_REQUIRED_FOR_DEVICE_MULTIPASS = unchecked((int)0xC00D1BE7);
        public const int E_INVALID_VIDEO_WIDTH_FOR_INTERLACED_ENCODING = unchecked((int)0xC00D1BE8);
        public const int E_DRM_INVALID_APPLICATION = unchecked((int)0xC00D2711);
        public const int E_DRM_LICENSE_STORE_ERROR = unchecked((int)0xC00D2712);
        public const int E_DRM_SECURE_STORE_ERROR = unchecked((int)0xC00D2713);
        public const int E_DRM_LICENSE_STORE_SAVE_ERROR = unchecked((int)0xC00D2714);
        public const int E_DRM_SECURE_STORE_UNLOCK_ERROR = unchecked((int)0xC00D2715);
        public const int E_DRM_INVALID_CONTENT = unchecked((int)0xC00D2716);
        public const int E_DRM_UNABLE_TO_OPEN_LICENSE = unchecked((int)0xC00D2717);
        public const int E_DRM_INVALID_LICENSE = unchecked((int)0xC00D2718);
        public const int E_DRM_INVALID_MACHINE = unchecked((int)0xC00D2719);
        public const int E_DRM_ENUM_LICENSE_FAILED = unchecked((int)0xC00D271B);
        public const int E_DRM_INVALID_LICENSE_REQUEST = unchecked((int)0xC00D271C);
        public const int E_DRM_UNABLE_TO_INITIALIZE = unchecked((int)0xC00D271D);
        public const int E_DRM_UNABLE_TO_ACQUIRE_LICENSE = unchecked((int)0xC00D271E);
        public const int E_DRM_INVALID_LICENSE_ACQUIRED = unchecked((int)0xC00D271F);
        public const int E_DRM_NO_RIGHTS = unchecked((int)0xC00D2720);
        public const int E_DRM_KEY_ERROR = unchecked((int)0xC00D2721);
        public const int E_DRM_ENCRYPT_ERROR = unchecked((int)0xC00D2722);
        public const int E_DRM_DECRYPT_ERROR = unchecked((int)0xC00D2723);
        public const int E_DRM_LICENSE_INVALID_XML = unchecked((int)0xC00D2725);
        public const int S_DRM_LICENSE_ACQUIRED = unchecked((int)0x000D2726);
        public const int S_DRM_INDIVIDUALIZED = unchecked((int)0x000D2727);
        public const int E_DRM_NEEDS_INDIVIDUALIZATION = unchecked((int)0xC00D2728);
        public const int E_DRM_ALREADY_INDIVIDUALIZED = unchecked((int)0xC00D2729);
        public const int E_DRM_ACTION_NOT_QUERIED = unchecked((int)0xC00D272A);
        public const int E_DRM_ACQUIRING_LICENSE = unchecked((int)0xC00D272B);
        public const int E_DRM_INDIVIDUALIZING = unchecked((int)0xC00D272C);
        public const int E_DRM_PARAMETERS_MISMATCHED = unchecked((int)0xC00D272F);
        public const int E_DRM_UNABLE_TO_CREATE_LICENSE_OBJECT = unchecked((int)0xC00D2730);
        public const int E_DRM_UNABLE_TO_CREATE_INDI_OBJECT = unchecked((int)0xC00D2731);
        public const int E_DRM_UNABLE_TO_CREATE_ENCRYPT_OBJECT = unchecked((int)0xC00D2732);
        public const int E_DRM_UNABLE_TO_CREATE_DECRYPT_OBJECT = unchecked((int)0xC00D2733);
        public const int E_DRM_UNABLE_TO_CREATE_PROPERTIES_OBJECT = unchecked((int)0xC00D2734);
        public const int E_DRM_UNABLE_TO_CREATE_BACKUP_OBJECT = unchecked((int)0xC00D2735);
        public const int E_DRM_INDIVIDUALIZE_ERROR = unchecked((int)0xC00D2736);
        public const int E_DRM_LICENSE_OPEN_ERROR = unchecked((int)0xC00D2737);
        public const int E_DRM_LICENSE_CLOSE_ERROR = unchecked((int)0xC00D2738);
        public const int E_DRM_GET_LICENSE_ERROR = unchecked((int)0xC00D2739);
        public const int E_DRM_QUERY_ERROR = unchecked((int)0xC00D273A);
        public const int E_DRM_REPORT_ERROR = unchecked((int)0xC00D273B);
        public const int E_DRM_GET_LICENSESTRING_ERROR = unchecked((int)0xC00D273C);
        public const int E_DRM_GET_CONTENTSTRING_ERROR = unchecked((int)0xC00D273D);
        public const int E_DRM_MONITOR_ERROR = unchecked((int)0xC00D273E);
        public const int E_DRM_UNABLE_TO_SET_PARAMETER = unchecked((int)0xC00D273F);
        public const int E_DRM_INVALID_APPDATA = unchecked((int)0xC00D2740);
        public const int E_DRM_INVALID_APPDATA_VERSION = unchecked((int)0xC00D2741);
        public const int E_DRM_BACKUP_EXISTS = unchecked((int)0xC00D2742);
        public const int E_DRM_BACKUP_CORRUPT = unchecked((int)0xC00D2743);
        public const int E_DRM_BACKUPRESTORE_BUSY = unchecked((int)0xC00D2744);
        public const int S_DRM_MONITOR_CANCELLED = unchecked((int)0x000D2746);
        public const int S_DRM_ACQUIRE_CANCELLED = unchecked((int)0x000D2747);
        public const int E_DRM_LICENSE_UNUSABLE = unchecked((int)0xC00D2748);
        public const int E_DRM_INVALID_PROPERTY = unchecked((int)0xC00D2749);
        public const int E_DRM_SECURE_STORE_NOT_FOUND = unchecked((int)0xC00D274A);
        public const int E_DRM_CACHED_CONTENT_ERROR = unchecked((int)0xC00D274B);
        public const int E_DRM_INDIVIDUALIZATION_INCOMPLETE = unchecked((int)0xC00D274C);
        public const int E_DRM_DRIVER_AUTH_FAILURE = unchecked((int)0xC00D274D);
        public const int E_DRM_NEED_UPGRADE_MSSAP = unchecked((int)0xC00D274E);
        public const int E_DRM_REOPEN_CONTENT = unchecked((int)0xC00D274F);
        public const int E_DRM_DRIVER_DIGIOUT_FAILURE = unchecked((int)0xC00D2750);
        public const int E_DRM_INVALID_SECURESTORE_PASSWORD = unchecked((int)0xC00D2751);
        public const int E_DRM_APPCERT_REVOKED = unchecked((int)0xC00D2752);
        public const int E_DRM_RESTORE_FRAUD = unchecked((int)0xC00D2753);
        public const int E_DRM_HARDWARE_INCONSISTENT = unchecked((int)0xC00D2754);
        public const int E_DRM_SDMI_TRIGGER = unchecked((int)0xC00D2755);
        public const int E_DRM_SDMI_NOMORECOPIES = unchecked((int)0xC00D2756);
        public const int E_DRM_UNABLE_TO_CREATE_HEADER_OBJECT = unchecked((int)0xC00D2757);
        public const int E_DRM_UNABLE_TO_CREATE_KEYS_OBJECT = unchecked((int)0xC00D2758);
        public const int E_DRM_LICENSE_NOTACQUIRED = unchecked((int)0xC00D2759);
        public const int E_DRM_UNABLE_TO_CREATE_CODING_OBJECT = unchecked((int)0xC00D275A);
        public const int E_DRM_UNABLE_TO_CREATE_STATE_DATA_OBJECT = unchecked((int)0xC00D275B);
        public const int E_DRM_BUFFER_TOO_SMALL = unchecked((int)0xC00D275C);
        public const int E_DRM_UNSUPPORTED_PROPERTY = unchecked((int)0xC00D275D);
        public const int E_DRM_ERROR_BAD_NET_RESP = unchecked((int)0xC00D275E);
        public const int E_DRM_STORE_NOTALLSTORED = unchecked((int)0xC00D275F);
        public const int E_DRM_SECURITY_COMPONENT_SIGNATURE_INVALID = unchecked((int)0xC00D2760);
        public const int E_DRM_INVALID_DATA = unchecked((int)0xC00D2761);
        public const int E_DRM_UNABLE_TO_CONTACT_SERVER = unchecked((int)0xC00D2762);
        public const int E_DRM_UNABLE_TO_CREATE_AUTHENTICATION_OBJECT = unchecked((int)0xC00D2763);
        public const int E_DRM_NOT_CONFIGURED = unchecked((int)0xC00D2764);
        public const int E_DRM_DEVICE_ACTIVATION_CANCELED = unchecked((int)0xC00D2765);
        public const int E_DRM_LICENSE_EXPIRED = unchecked((int)0xC00D27D8);
        public const int E_DRM_LICENSE_NOTENABLED = unchecked((int)0xC00D27D9);
        public const int E_DRM_LICENSE_APPSECLOW = unchecked((int)0xC00D27DA);
        public const int E_DRM_STORE_NEEDINDI = unchecked((int)0xC00D27DB);
        public const int E_DRM_STORE_NOTALLOWED = unchecked((int)0xC00D27DC);
        public const int E_DRM_LICENSE_APP_NOTALLOWED = unchecked((int)0xC00D27DD);
        public const int S_DRM_NEEDS_INDIVIDUALIZATION = unchecked((int)0x000D27DE);
        public const int E_DRM_LICENSE_CERT_EXPIRED = unchecked((int)0xC00D27DF);
        public const int E_DRM_LICENSE_SECLOW = unchecked((int)0xC00D27E0);
        public const int E_DRM_LICENSE_CONTENT_REVOKED = unchecked((int)0xC00D27E1);
        public const int E_DRM_LICENSE_NOSAP = unchecked((int)0xC00D280A);
        public const int E_DRM_LICENSE_NOSVP = unchecked((int)0xC00D280B);
        public const int E_DRM_LICENSE_NOWDM = unchecked((int)0xC00D280C);
        public const int E_DRM_LICENSE_NOTRUSTEDCODEC = unchecked((int)0xC00D280D);
        public const int E_DRM_NEEDS_UPGRADE_TEMPFILE = unchecked((int)0xC00D283D);
        public const int E_DRM_NEED_UPGRADE_PD = unchecked((int)0xC00D283E);
        public const int E_DRM_SIGNATURE_FAILURE = unchecked((int)0xC00D283F);
        public const int E_DRM_LICENSE_SERVER_INFO_MISSING = unchecked((int)0xC00D2840);
        public const int E_DRM_BUSY = unchecked((int)0xC00D2841);
        public const int E_DRM_PD_TOO_MANY_DEVICES = unchecked((int)0xC00D2842);
        public const int E_DRM_INDIV_FRAUD = unchecked((int)0xC00D2843);
        public const int E_DRM_INDIV_NO_CABS = unchecked((int)0xC00D2844);
        public const int E_DRM_INDIV_SERVICE_UNAVAILABLE = unchecked((int)0xC00D2845);
        public const int E_DRM_RESTORE_SERVICE_UNAVAILABLE = unchecked((int)0xC00D2846);
        public const int S_REBOOT_RECOMMENDED = unchecked((int)0x000D2AF8);
        public const int S_REBOOT_REQUIRED = unchecked((int)0x000D2AF9);
        public const int E_REBOOT_RECOMMENDED = unchecked((int)0xC00D2AFA);
        public const int E_REBOOT_REQUIRED = unchecked((int)0xC00D2AFB);
        public const int E_UNKNOWN_PROTOCOL = unchecked((int)0xC00D2EE0);
        public const int E_REDIRECT_TO_PROXY = unchecked((int)0xC00D2EE1);
        public const int E_INTERNAL_SERVER_ERROR = unchecked((int)0xC00D2EE2);
        public const int E_BAD_REQUEST = unchecked((int)0xC00D2EE3);
        public const int E_ERROR_FROM_PROXY = unchecked((int)0xC00D2EE4);
        public const int E_PROXY_TIMEOUT = unchecked((int)0xC00D2EE5);
        public const int E_SERVER_UNAVAILABLE = unchecked((int)0xC00D2EE6);
        public const int E_REFUSED_BY_SERVER = unchecked((int)0xC00D2EE7);
        public const int E_INCOMPATIBLE_SERVER = unchecked((int)0xC00D2EE8);
        public const int E_MULTICAST_DISABLED = unchecked((int)0xC00D2EE9);
        public const int E_INVALID_REDIRECT = unchecked((int)0xC00D2EEA);
        public const int E_ALL_PROTOCOLS_DISABLED = unchecked((int)0xC00D2EEB);
        public const int E_MSBD_NO_LONGER_SUPPORTED = unchecked((int)0xC00D2EEC);
        public const int E_PROXY_NOT_FOUND = unchecked((int)0xC00D2EED);
        public const int E_CANNOT_CONNECT_TO_PROXY = unchecked((int)0xC00D2EEE);
        public const int E_SERVER_DNS_TIMEOUT = unchecked((int)0xC00D2EEF);
        public const int E_PROXY_DNS_TIMEOUT = unchecked((int)0xC00D2EF0);
        public const int E_CLOSED_ON_SUSPEND = unchecked((int)0xC00D2EF1);
        public const int E_CANNOT_READ_PLAYLIST_FROM_MEDIASERVER = unchecked((int)0xC00D2EF2);
        public const int E_SESSION_NOT_FOUND = unchecked((int)0xC00D2EF3);
        public const int E_REQUIRE_STREAMING_CLIENT = unchecked((int)0xC00D2EF4);
        public const int E_PLAYLIST_ENTRY_HAS_CHANGED = unchecked((int)0xC00D2EF5);
        public const int E_PROXY_ACCESSDENIED = unchecked((int)0xC00D2EF6);
        public const int E_PROXY_SOURCE_ACCESSDENIED = unchecked((int)0xC00D2EF7);
        public const int E_NETWORK_SINK_WRITE = unchecked((int)0xC00D2EF8);
        public const int E_FIREWALL = unchecked((int)0xC00D2EF9);
        public const int E_MMS_NOT_SUPPORTED = unchecked((int)0xC00D2EFA);
        public const int E_SERVER_ACCESSDENIED = unchecked((int)0xC00D2EFB);
        public const int E_RESOURCE_GONE = unchecked((int)0xC00D2EFC);
        public const int E_NO_EXISTING_PACKETIZER = unchecked((int)0xC00D2EFD);
        public const int E_BAD_SYNTAX_IN_SERVER_RESPONSE = unchecked((int)0xC00D2EFE);
        public const int I_RECONNECTED = unchecked((int)0x400D2EFF);
        public const int E_RESET_SOCKET_CONNECTION = unchecked((int)0xC00D2F00);
        public const int I_NOLOG_STOP = unchecked((int)0x400D2F01);
        public const int E_TOO_MANY_HOPS = unchecked((int)0xC00D2F02);
        public const int I_EXISTING_PACKETIZER = unchecked((int)0x400D2F03);
        public const int I_MANUAL_PROXY = unchecked((int)0x400D2F04);
        public const int E_TOO_MUCH_DATA_FROM_SERVER = unchecked((int)0xC00D2F05);
        public const int E_CONNECT_TIMEOUT = unchecked((int)0xC00D2F06);
        public const int E_PROXY_CONNECT_TIMEOUT = unchecked((int)0xC00D2F07);
        public const int E_SESSION_INVALID = unchecked((int)0xC00D2F08);
        public const int S_EOSRECEDING = unchecked((int)0x000D2F09);
        public const int E_PACKETSINK_UNKNOWN_FEC_STREAM = unchecked((int)0xC00D2F0A);
        public const int E_PUSH_CANNOTCONNECT = unchecked((int)0xC00D2F0B);
        public const int E_INCOMPATIBLE_PUSH_SERVER = unchecked((int)0xC00D2F0C);
        public const int S_CHANGENOTICE = unchecked((int)0x000D2F0D);
        public const int E_END_OF_PLAYLIST = unchecked((int)0xC00D32C8);
        public const int E_USE_FILE_SOURCE = unchecked((int)0xC00D32C9);
        public const int E_PROPERTY_NOT_FOUND = unchecked((int)0xC00D32CA);
        public const int E_PROPERTY_READ_ONLY = unchecked((int)0xC00D32CC);
        public const int E_TABLE_KEY_NOT_FOUND = unchecked((int)0xC00D32CD);
        public const int E_INVALID_QUERY_OPERATOR = unchecked((int)0xC00D32CF);
        public const int E_INVALID_QUERY_PROPERTY = unchecked((int)0xC00D32D0);
        public const int E_PROPERTY_NOT_SUPPORTED = unchecked((int)0xC00D32D2);
        public const int E_SCHEMA_CLASSIFY_FAILURE = unchecked((int)0xC00D32D4);
        public const int E_METADATA_FORMAT_NOT_SUPPORTED = unchecked((int)0xC00D32D5);
        public const int E_METADATA_NO_EDITING_CAPABILITY = unchecked((int)0xC00D32D6);
        public const int E_METADATA_CANNOT_SET_LOCALE = unchecked((int)0xC00D32D7);
        public const int E_METADATA_LANGUAGE_NOT_SUPORTED = unchecked((int)0xC00D32D8);
        public const int E_METADATA_NO_RFC1766_NAME_FOR_LOCALE = unchecked((int)0xC00D32D9);
        public const int E_METADATA_NOT_AVAILABLE = unchecked((int)0xC00D32DA);
        public const int E_METADATA_CACHE_DATA_NOT_AVAILABLE = unchecked((int)0xC00D32DB);
        public const int E_METADATA_INVALID_DOCUMENT_TYPE = unchecked((int)0xC00D32DC);
        public const int E_METADATA_IDENTIFIER_NOT_AVAILABLE = unchecked((int)0xC00D32DD);
        public const int E_METADATA_CANNOT_RETRIEVE_FROM_OFFLINE_CACHE = unchecked((int)0xC00D32DE);

        public const int I_NO_EVENTS = unchecked((int)0x400D0069);
        public const int E_REGKEY_NOT_FOUND = unchecked((int)0xC00D006A);
    } 
 

    /// <summary>
    /// Error codes.
    /// </summary>
    public static class ErrorCodes
    {
        // ReSharper disable RedundantCast
        // ReSharper disable InconsistentNaming

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int S_OK = 0;

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int S_FALSE = 1;

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int E_POINTER = unchecked((int)0x80004003);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int E_ABORT = unchecked((int)0x80004004);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int E_FAIL = unchecked((int)0x80004005);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int E_INVALIDARG = unchecked((int)0x80000003);
     
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int E_NOTIMPL = unchecked((int)0x80004001);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALIDMEDIATYPE = unchecked((int)0x80040200);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALIDSUBTYPE = unchecked((int)0x80040201);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NEED_OWNER = unchecked((int)0x80040202);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_ENUM_OUT_OF_SYNC = unchecked((int)0x80040203);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_ALREADY_CONNECTED = unchecked((int)0x80040204);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_FILTER_ACTIVE = unchecked((int)0x80040205);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_TYPES = unchecked((int)0x80040206);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_ACCEPTABLE_TYPES = unchecked((int)0x80040207);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALID_DIRECTION = unchecked((int)0x80040208);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_CONNECTED = unchecked((int)0x80040209);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_ALLOCATOR = unchecked((int)0x8004020A);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_RUNTIME_ERROR = unchecked((int)0x8004020B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_BUFFER_NOTSET = unchecked((int)0x8004020C);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_BUFFER_OVERFLOW = unchecked((int)0x8004020D);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_BADALIGN = unchecked((int)0x8004020E);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_ALREADY_COMMITTED = unchecked((int)0x8004020F);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_BUFFERS_OUTSTANDING = unchecked((int)0x80040210);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_COMMITTED = unchecked((int)0x80040211);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_SIZENOTSET = unchecked((int)0x80040212);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_CLOCK = unchecked((int)0x80040213);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_SINK = unchecked((int)0x80040214);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_INTERFACE = unchecked((int)0x80040215);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_FOUND = unchecked((int)0x80040216);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_CANNOT_CONNECT = unchecked((int)0x80040217);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_CANNOT_RENDER = unchecked((int)0x80040218);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_CHANGING_FORMAT = unchecked((int)0x80040219);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_COLOR_KEY_SET = unchecked((int)0x8004021A);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_OVERLAY_CONNECTION = unchecked((int)0x8004021B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_SAMPLE_CONNECTION = unchecked((int)0x8004021C);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_PALETTE_SET = unchecked((int)0x8004021D);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_COLOR_KEY_SET = unchecked((int)0x8004021E);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_COLOR_KEY_FOUND = unchecked((int)0x8004021F);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_PALETTE_AVAILABLE = unchecked((int)0x80040220);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_DISPLAY_PALETTE = unchecked((int)0x80040221);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_TOO_MANY_COLORS = unchecked((int)0x80040222);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_STATE_CHANGED = unchecked((int)0x80040223);
        
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_STOPPED = unchecked((int)0x80040224);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_PAUSED = unchecked((int)0x80040225);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_RUNNING = unchecked((int)0x80040226);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_WRONG_STATE = unchecked((int)0x80040227);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_START_TIME_AFTER_END = unchecked((int)0x80040228);
        
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALID_RECT = unchecked((int)0x80040229);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_TYPE_NOT_ACCEPTED = unchecked((int)0x8004022A);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_SAMPLE_REJECTED = unchecked((int)0x8004022B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_SAMPLE_REJECTED_EOS = unchecked((int)0x8004022C);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DUPLICATE_NAME = unchecked((int)0x8004022D);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_DUPLICATE_NAME = unchecked((int)0x0004022D);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_TIMEOUT = unchecked((int)0x8004022E);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALID_FILE_FORMAT = unchecked((int)0x8004022F);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_ENUM_OUT_OF_RANGE = unchecked((int)0x80040230);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_CIRCULAR_GRAPH = unchecked((int)0x80040231);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_ALLOWED_TO_SAVE = unchecked((int)0x80040232);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_TIME_ALREADY_PASSED = unchecked((int)0x80040233);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_ALREADY_CANCELLED = unchecked((int)0x80040234);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_CORRUPT_GRAPH_FILE = unchecked((int)0x80040235);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_ADVISE_ALREADY_SET = unchecked((int)0x80040236);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_STATE_INTERMEDIATE = unchecked((int)0x00040237);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_MODEX_AVAILABLE = unchecked((int)0x80040238);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_ADVISE_SET = unchecked((int)0x80040239);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_FULLSCREEN = unchecked((int)0x8004023B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_UNKNOWN_FILE_TYPE = unchecked((int)0x80040240);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_CANNOT_LOAD_SOURCE_FILTER = unchecked((int)0x80040241);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_PARTIAL_RENDER = unchecked((int)0x00040242);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_FILE_TOO_SHORT = unchecked((int)0x80040243);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALID_FILE_VERSION = unchecked((int)0x80040244);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_SOME_DATA_IGNORED = unchecked((int)0x00040245);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_CONNECTIONS_DEFERRED = unchecked((int)0x00040246);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALID_CLSID = unchecked((int)0x80040247);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_INVALID_MEDIA_TYPE = unchecked((int)0x80040248);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_BAD_KEY = unchecked((int)0x800403F2);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_NO_MORE_ITEMS = unchecked((int)0x00040103);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_SAMPLE_TIME_NOT_SET = unchecked((int)0x80040249);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_RESOURCE_NOT_NEEDED = unchecked((int)0x00040250);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_MEDIA_TIME_NOT_SET = unchecked((int)0x80040251);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_TIME_FORMAT_SET = unchecked((int)0x80040252);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_MONO_AUDIO_HW = unchecked((int)0x80040253);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_MEDIA_TYPE_IGNORED = unchecked((int)0x00040254);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_AUDIO_HARDWARE = unchecked((int)0x80040256);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_VIDEO_NOT_RENDERED = unchecked((int)0x00040257);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_AUDIO_NOT_RENDERED = unchecked((int)0x00040258);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_RPZA = unchecked((int)0x80040259);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_RPZA = unchecked((int)0x0004025A);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_PROCESSOR_NOT_SUITABLE = unchecked((int)0x8004025B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_UNSUPPORTED_AUDIO = unchecked((int)0x8004025C);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_UNSUPPORTED_VIDEO = unchecked((int)0x8004025D);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_MPEG_NOT_CONSTRAINED = unchecked((int)0x8004025E);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NOT_IN_GRAPH = unchecked((int)0x8004025F);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_ESTIMATED = unchecked((int)0x00040260);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_TIME_FORMAT = unchecked((int)0x80040261);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_READ_ONLY = unchecked((int)0x80040262);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_RESERVED = unchecked((int)0x00040263);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_BUFFER_UNDERFLOW = unchecked((int)0x80040264);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_UNSUPPORTED_STREAM = unchecked((int)0x80040265);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_TRANSPORT = unchecked((int)0x80040266);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_STREAM_OFF = unchecked((int)0x00040267);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_CANT_CUE = unchecked((int)0x00040268);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_BAD_VIDEOCD = unchecked((int)0x80040269);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_NO_STOP_TIME = unchecked((int)0x00040270);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_OUT_OF_VIDEO_MEMORY = unchecked((int)0x80040271);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_VP_NEGOTIATION_FAILED = unchecked((int)0x80040272);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DDRAW_CAPS_NOT_SUITABLE = unchecked((int)0x80040273);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_VP_HARDWARE = unchecked((int)0x80040274);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_CAPTURE_HARDWARE = unchecked((int)0x80040275);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_OPERATION_INHIBITED = unchecked((int)0x80040276);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_INVALIDDOMAIN = unchecked((int)0x80040277);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_NO_BUTTON = unchecked((int)0x80040278);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_GRAPHNOTREADY = unchecked((int)0x80040279);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_RENDERFAIL = unchecked((int)0x8004027A);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_DECNOTENOUGH = unchecked((int)0x8004027B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DDRAW_VERSION_NOT_SUITABLE = unchecked((int)0x8004027C);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_COPYPROT_FAILED = unchecked((int)0x8004027D);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_NOPREVIEWPIN = unchecked((int)0x0004027E);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_TIME_EXPIRED = unchecked((int)0x8004027F);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_DVD_NON_ONE_SEQUENTIAL = unchecked((int)0x00040280);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_WRONG_SPEED = unchecked((int)0x80040281);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_MENU_DOES_NOT_EXIST = unchecked((int)0x80040282);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_CMD_CANCELLED = unchecked((int)0x80040283);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_STATE_WRONG_VERSION = unchecked((int)0x80040284);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_STATE_CORRUPT = unchecked((int)0x80040285);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_STATE_WRONG_DISC = unchecked((int)0x80040286);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_INCOMPATIBLE_REGION = unchecked((int)0x80040287);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_NO_ATTRIBUTES = unchecked((int)0x80040288);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_NO_GOUP_PGC = unchecked((int)0x80040289);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_LOW_PARENTAL_LEVEL = unchecked((int)0x8004028A);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_NOT_IN_KARAOKE_MODE = unchecked((int)0x8004028B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_DVD_CHANNEL_CONTENTS_NOT_AVAILABLE = unchecked((int)0x0004028C);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_S_DVD_NOT_ACCURATE = unchecked((int)0x0004028D);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_FRAME_STEP_UNSUPPORTED = unchecked((int)0x8004028E);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_STREAM_DISABLED = unchecked((int)0x8004028F);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_TITLE_UNKNOWN = unchecked((int)0x80040290);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_INVALID_DISC = unchecked((int)0x80040291);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_NO_RESUME_INFORMATION = unchecked((int)0x80040292);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD = unchecked((int)0x80040293);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_PIN_ALREADY_BLOCKED = unchecked((int)0x80040294);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_CERTIFICATION_FAILURE = unchecked((int)0x80040295);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_VMR_NOT_IN_MIXER_MODE = unchecked((int)0x80040296);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_VMR_NO_AP_SUPPLIED = unchecked((int)0x80040297);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_VMR_NO_DEINTERLACE_HW = unchecked((int)0x80040298);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_VMR_NO_PROCAMP_HW = unchecked((int)0x80040299);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_DVD_VMR9_INCOMPATIBLEDEC = unchecked((int)0x8004029A);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int VFW_E_NO_COPP_HW = unchecked((int)0x8004029B);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int E_PROP_SET_UNSUPPORTED = unchecked((int)0x80070492);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int E_PROP_ID_UNSUPPORTED = unchecked((int)0x80070490);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int E_NOTINTREE = unchecked((int)0x80040400);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int E_RENDER_ENGINE_IS_BROKEN = unchecked((int)0x80040401);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int E_MUST_INIT_RENDERER = unchecked((int)0x80040402);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int E_NOTDETERMINED = unchecked((int)0x80040403);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int E_NO_TIMELINE = unchecked((int)0x80040404);

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const int S_WARN_OUTPUTRESET = 40404;

        // ReSharper restore RedundantCast
        // ReSharper restore InconsistentNaming
    }

    ///// <summary>
    ///// Constants.
    ///// </summary>
    //public static class DSConsts
    //{
    //    // ReSharper disable InconsistentNaming

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int VfwCaptureDialog_Source = 0x01;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int VfwCaptureDialog_Format = 0x2;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int VfwCaptureDialog_Display = 0x4;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int VfwCompressDialog_Config = 0x1;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int VfwCompressDialog_About = 0x2;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int VfwCompressDialog_QueryConfig = 0x4;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int VfwCompressDialog_QueryAbout = 0x8;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_MP42 = "3234504D-0000-0010-8000-00AA00389B71";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_DIVX = "58564944-0000-0010-8000-00AA00389B71";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_VOXWARE = "00000075-0000-0010-8000-00AA00389B71";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_MPEG2_TRANSPORT_STRIDE = "138AA9A4-1EE2-4c5b-988E-19ABFDBC8A11";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FORMAT_MPEG2Video = "E06D80E3-DB46-11CF-B4D1-00805F6CBBEA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FORMAT_DolbyAC3 = "E06D80E4-DB46-11CF-B4D1-00805F6CBBEA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FORMAT_MPEG2Audio = "E06D80E5-DB46-11CF-B4D1-00805F6CBBEA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FORMAT_DVD_LPCMAudio = "E06D80E6-DB46-11CF-B4D1-00805F6CBBEA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_DVD_SUBPICTURE = "e06d802d-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_DVD_LPCM_AUDIO = "e06d8032-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_DTS = "e06d8033-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_SDDS = "e06d8034-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIATYPE_DVD_ENCRYPTED_PACK = "ED0B916A-044D-11d1-AA78-00C04FC31D60";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIATYPE_DVD_NAVIGATION = "e06d802e-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_DVD_NAVIGATION_PCI = "e06d802f-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_DVD_NAVIGATION_DSI = "e06d8030-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIASUBTYPE_DVD_NAVIGATION_PROVIDER = "e06d8031-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIATYPE_MPEG2_PACK = "36523B13-8EE5-11d1-8CA3-0060B057664A";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIATYPE_MPEG2_PES = "e06d8020-db46-11cf-b4d1-00805f6cbbea";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MEDIATYPE_MPEG1SystemStream = "e436eb82-524f-11ce-9f53-0020af0ba770";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string IID_IPropertyBag = "55272A00-42CB-11CE-8135-00AA004BB851";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string IID_ISpecifyPropertyPages = "B196B28B-BAB4-101A-B69C-00AA00341D07";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string IID_IPersistStream = "00000109-0000-0000-C000-000000000046";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string IID_IServiceProvider = "6D5140C1-7436-11CE-8034-00AA006009FA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string IID_IMpeg2PsiParser = "AE1A2884-540E-4077-B1AB-67A34A72298C";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_VFPSIParser = "DDF7480E-13E2-4481-BA2B-3C17C4FC469F";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string DSKey = "CLSID\\{083863F1-70DE-11D0-BD40-00A0C911CE86}\\Instance\\";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const int PixelCountMax = 32768;

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FIL_AVIDecompressor = "AVI Decompressor";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FIL_PSIParser = "PSI Parser";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FIL_PSI = "PSI";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_BOBLineReplicate = "BOB Line Replicate";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_BOBVerticalStretch = "BOB Vertical Stretch";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_MedianFiltering = "Median Filtering";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_EdgeFiltering = "Edge Filtering";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_FieldAdaptive = "Field Adaptive";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_PixelAdaptive = "Pixel Adaptive";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_MotionVectorSteered = "Motion Vector Steered";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string Deint_UnknownProprietary = "Unknown/Proprietary";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string SFilterData = "FilterData";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FCC_BITFIELDS = "BITFIELDS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FCC_RLE4 = "RLE4";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FCC_RLE8 = "RLE8";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string FCC_RGB = "RGB";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string PP_DevicePath = "DevicePath";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string PP_FccHandler = "FccHandler";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string PP_Description = "Description";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string PP_CLSID = "CLSID";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string PP_FriendlyName = "FriendlyName";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string SUnknown = "Unknown";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_ENOTDETERMINED = "E_NOTDETERMINED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEPINALREADYBLOCKED = "VFW_E_PIN_ALREADY_BLOCKED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_ENOTIMELINE = "E_NO_TIMELINE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_ERENDERENGINEISBROKEN = "E_RENDER_ENGINE_IS_BROKEN";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDDRAWCAPSNOTSUITABLE = "VFW_E_DDRAW_CAPS_NOT_SUITABLE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_EMUSTINITRENDERER = "E_MUST_INIT_RENDERER";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEVMRNOTINMIXERMODE = "VFW_E_VMR_NOT_IN_MIXER_MODE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_SWARNOUTPUTRESET = "S_WARN_OUTPUTRESET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_ENOTINTREE = "E_NOTINTREE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEVMRNODEINTERLACEHW = "VFW_E_VMR_NO_DEINTERLACE_HW";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDVMR9INCOMPATIBLEDEC = "VFW_E_DVD_VMR9_INCOMPATIBLEDEC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEBADKEY = "VFW_E_BAD_KEY";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEVMRNOPROCAMPHW = "VFW_E_VMR_NO_PROCAMP_HW";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEVMRNOAPSUPPLIED = "VFW_E_VMR_NO_AP_SUPPLIED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECERTIFICATIONFAILURE = "VFW_E_CERTIFICATION_FAILURE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEPINALREADYBLOCKEDONTHISTHREAD = "VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDNOGOUPPGC = "VFW_E_DVD_NO_GOUP_PGC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDNOTINKARAOKEMODE = "VFW_E_DVD_NOT_IN_KARAOKE_MODE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDSTREAMDISABLED = "VFW_E_DVD_STREAM_DISABLED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDINVALIDDISC = "VFW_E_DVD_INVALID_DISC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDNORESUMEINFORMATION = "VFW_E_DVD_NO_RESUME_INFORMATION";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDTITLEUNKNOWN = "VFW_E_DVD_TITLE_UNKNOWN";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEFRAMESTEPUNSUPPORTED = "VFW_E_FRAME_STEP_UNSUPPORTED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDLOWPARENTALLEVEL = "VFW_E_DVD_LOW_PARENTAL_LEVEL";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDDECNOTENOUGH = "VFW_E_DVD_DECNOTENOUGH";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDINCOMPATIBLEREGION = "VFW_E_DVD_INCOMPATIBLE_REGION";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDNOATTRIBUTES = "VFW_E_DVD_NO_ATTRIBUTES";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDSTATECORRUPT = "VFW_E_DVD_STATE_CORRUPT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDSTATEWRONGDISC = "VFW_E_DVD_STATE_WRONG_DISC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDWRONGSPEED = "VFW_E_DVD_WRONG_SPEED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDCMDCANCELLED = "VFW_E_DVD_CMD_CANCELLED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECOPYPROTFAILED = "VFW_E_COPYPROT_FAILED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDSTATEWRONGVERSION = "VFW_E_DVD_STATE_WRONG_VERSION";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDMENUDOESNOTEXIST = "VFW_E_DVD_MENU_DOES_NOT_EXIST";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWETIMEEXPIRED = "VFW_E_TIME_EXPIRED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDDRAWVERSIONNOTSUITABLE = "VFW_E_DDRAW_VERSION_NOT_SUITABLE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOCAPTUREHARDWARE = "VFW_E_NO_CAPTURE_HARDWARE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDINVALIDDOMAIN = "VFW_E_DVD_INVALIDDOMAIN";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDGRAPHNOTREADY = "VFW_E_DVD_GRAPHNOTREADY";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDRENDERFAIL = "VFW_E_DVD_RENDERFAIL";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDNOBUTTON = "VFW_E_DVD_NO_BUTTON";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDVDOPERATIONINHIBITED = "VFW_E_DVD_OPERATION_INHIBITED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOVPHARDWARE = "VFW_E_NO_VP_HARDWARE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEALREADYCANCELLED = "VFW_E_ALREADY_CANCELLED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEREADONLY = "VFW_E_READ_ONLY";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEOUTOFVIDEOMEMORY = "VFW_E_OUT_OF_VIDEO_MEMORY";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEVPNEGOTIATIONFAILED = "VFW_E_VP_NEGOTIATION_FAILED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEUNSUPPORTEDSTREAM = "VFW_E_UNSUPPORTED_STREAM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEBADVIDEOCD = "VFW_E_BAD_VIDEOCD";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSNOSTOPTIME = "VFW_S_NO_STOP_TIME";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTRANSPORT = "VFW_E_NO_TRANSPORT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEBUFFERUNDERFLOW = "VFW_E_BUFFER_UNDERFLOW";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTINGRAPH = "VFW_E_NOT_IN_GRAPH";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTIMEFORMAT = "VFW_E_NO_TIME_FORMAT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEINVALIDCLSID = "VFW_E_INVALID_CLSID";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEPROCESSORNOTSUITABLE = "VFW_E_PROCESSOR_NOT_SUITABLE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEUNSUPPORTEDVIDEO = "VFW_E_UNSUPPORTED_VIDEO";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEMPEGNOTCONSTRAINED = "VFW_E_MPEG_NOT_CONSTRAINED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEUNSUPPORTEDAUDIO = "VFW_E_UNSUPPORTED_AUDIO";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWESAMPLETIMENOTSET = "VFW_E_SAMPLE_TIME_NOT_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTIMEFORMATSET = "VFW_E_NO_TIME_FORMAT_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOAUDIOHARDWARE = "VFW_E_NO_AUDIO_HARDWARE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWERPZA = "VFW_E_RPZA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEMONOAUDIOHW = "VFW_E_MONO_AUDIO_HW";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEMEDIATIMENOTSET = "VFW_E_MEDIA_TIME_NOT_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEINVALIDMEDIATYPE = "VFW_E_INVALID_MEDIA_TYPE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEUNKNOWNFILETYPE = "VFW_E_UNKNOWN_FILE_TYPE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEFILETOOSHORT = "VFW_E_FILE_TOO_SHORT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEINVALIDFILEVERSION = "VFW_E_INVALID_FILE_VERSION";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECANNOTLOADSOURCEFILTER = "VFW_E_CANNOT_LOAD_SOURCE_FILTER";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOADVISESET = "VFW_E_NO_ADVISE_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECIRCULARGRAPH = "VFW_E_CIRCULAR_GRAPH";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEADVISEALREADYSET = "VFW_E_ADVISE_ALREADY_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOFULLSCREEN = "VFW_E_NO_FULLSCREEN";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOMODEXAVAILABLE = "VFW_E_NO_MODEX_AVAILABLE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECORRUPTGRAPHFILE = "VFW_E_CORRUPT_GRAPH_FILE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWETIMEALREADYPASSED = "VFW_E_TIME_ALREADY_PASSED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWESAMPLEREJECTEDEOS = "VFW_E_SAMPLE_REJECTED_EOS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEINVALIDFILEFORMAT = "VFW_E_INVALID_FILE_FORMAT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTALLOWEDTOSAVE = "VFW_E_NOT_ALLOWED_TO_SAVE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEENUMOUTOFRANGE = "VFW_E_ENUM_OUT_OF_RANGE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWETIMEOUT = "VFW_E_TIMEOUT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEINVALIDRECT = "VFW_E_INVALID_RECT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEDUPLICATENAME = "VFW_E_DUPLICATE_NAME";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWESAMPLEREJECTED = "VFW_E_SAMPLE_REJECTED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEWRONGSTATE = "VFW_E_WRONG_STATE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWETYPENOTACCEPTED = "VFW_E_TYPE_NOT_ACCEPTED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWESTARTTIMEAFTEREND = "VFW_E_START_TIME_AFTER_END";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTRUNNING = "VFW_E_NOT_RUNNING";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTPAUSED = "VFW_E_NOT_PAUSED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTSTOPPED = "VFW_E_NOT_STOPPED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWESTATECHANGED = "VFW_E_STATE_CHANGED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENODISPLAYPALETTE = "VFW_E_NO_DISPLAY_PALETTE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWETOOMANYCOLORS = "VFW_E_TOO_MANY_COLORS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOCOLORKEYFOUND = "VFW_E_NO_COLOR_KEY_FOUND";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOPALETTEAVAILABLE = "VFW_E_NO_PALETTE_AVAILABLE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECOLORKEYSET = "VFW_E_COLOR_KEY_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEPALETTESET = "VFW_E_PALETTE_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTSAMPLECONNECTION = "VFW_E_NOT_SAMPLE_CONNECTION";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTOVERLAYCONNECTION = "VFW_E_NOT_OVERLAY_CONNECTION";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOCOLORKEYSET = "VFW_E_NO_COLOR_KEY_SET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECHANGINGFORMAT = "VFW_E_CHANGING_FORMAT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECANNOTRENDER = "VFW_E_CANNOT_RENDER";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTFOUND = "VFW_E_NOT_FOUND";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWECANNOTCONNECT = "VFW_E_CANNOT_CONNECT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOINTERFACE = "VFW_E_NO_INTERFACE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOSINK = "VFW_E_NO_SINK";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOCLOCK = "VFW_E_NO_CLOCK";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWESIZENOTSET = "VFW_E_SIZENOTSET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEALREADYCOMMITTED = "VFW_E_ALREADY_COMMITTED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEBADALIGN = "VFW_E_BADALIGN";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEBUFFEROVERFLOW = "VFW_E_BUFFER_OVERFLOW";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWERUNTIMEERROR = "VFW_E_RUNTIME_ERROR";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEBUFFERSOUTSTANDING = "VFW_E_BUFFERS_OUTSTANDING";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEBUFFERNOTSET = "VFW_E_BUFFER_NOTSET";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTCOMMITTED = "VFW_E_NOT_COMMITTED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOALLOCATOR = "VFW_E_NO_ALLOCATOR";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTCONNECTED = "VFW_E_NOT_CONNECTED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEINVALIDDIRECTION = "VFW_E_INVALID_DIRECTION";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_SVFWENOACCEPTABLETYPES = "VFW_E_NO_ACCEPTABLE_TYPES";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENOTYPES = "VFW_E_NO_TYPES";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEFILTERACTIVE = "VFW_E_FILTER_ACTIVE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEALREADYCONNECTED = "VFW_E_ALREADY_CONNECTED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEENUMOUTOFSYNC = "VFW_E_ENUM_OUT_OF_SYNC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWENEEDOWNER = "VFW_E_NEED_OWNER";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWEINVALIDSUBTYPE = "VFW_E_INVALIDSUBTYPE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSDVDNOTACCURATE = "VFW_S_DVD_NOT_ACCURATE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSDVDNONONESEQUENTIAL = "VFW_S_DVD_NON_ONE_SEQUENTIAL";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSNOPREVIEWPIN = "VFW_S_NOPREVIEWPIN";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSCANTCUE = "VFW_S_CANT_CUE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSSTREAMOFF = "VFW_S_STREAM_OFF";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSRESERVED = "VFW_S_RESERVED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSESTIMATED = "VFW_S_ESTIMATED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSDVDCHANNELCONTENTSNOTAVAILABLE = "VFW_S_DVD_CHANNEL_CONTENTS_NOT_AVAILABLE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSRPZA = "VFW_S_RPZA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSAUDIONOTRENDERED = "VFW_S_AUDIO_NOT_RENDERED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSVIDEONOTRENDERED = "VFW_S_VIDEO_NOT_RENDERED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSMEDIATYPEIGNORED = "VFW_S_MEDIA_TYPE_IGNORED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSRESOURCENOTNEEDED = "VFW_S_RESOURCE_NOT_NEEDED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSCONNECTIONSDEFERRED = "VFW_S_CONNECTIONS_DEFERRED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSSOMEDATAIGNORED = "VFW_S_SOME_DATA_IGNORED";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSPARTIALRENDER = "VFW_S_PARTIAL_RENDER";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSSTATEINTERMEDIATE = "VFW_S_STATE_INTERMEDIATE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSDUPLICATENAME = "VFW_S_DUPLICATE_NAME";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_VFWSNOMOREITEMS = "VFW_S_NO_MORE_ITEMS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_SFALSE = "S_FALSE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ER_SOK = "S_OK";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_UnKnown = "Unknown ";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_Video = "Video";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_URLSTREAM = "URL_STREAM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_Timecode = "Timecode";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_Text = "Text";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_Stream = "Stream";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_ScriptCommand = "ScriptCommand";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_MPEG2PES = "MPEG2_PES";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_Midi = "Midi";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_LMRT = "LMRT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_Interleaved = "Interleaved";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_AUXLine21Data = "AUXLine21Data";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_File = "File";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_Audio = "Audio";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_AnalogVideo = "AnalogVideo";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MT_AnalogAudio = "AnalogAudio";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_Unknown = "Unknown";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_NORRIS = "NORRIS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_LHCODEC = "LH_CODEC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_OLIOPR = "OLIOPR";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_OLISBC = "OLISBC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_OLICELP = "OLICELP";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_OLIADPCM = "OLIADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_BTVDIGITAL = "BTV_DIGITAL";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_OLIGSM = "OLIGSM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_FMTOWNSSND = "FM_TOWNS_SND";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_QUARTERDECK = "QUARTERDECK";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CREATIVEFASTSPEECH10 = "CREATIVE_FASTSPEECH10";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CREATIVEFASTSPEECH8 = "CREATIVE_FASTSPEECH8";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CREATIVEADPCM = "CREATIVE_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_RHETOREXADPCM = "RHETOREX_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_SOFTSOUND = "SOFTSOUND";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_VOXWARE = "VOXWARE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DSATDISPLAY = "DSAT_DISPLAY";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DSAT = "DSAT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_G722ADPCM = "G722_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_G726ADPCM = "G726_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CANOPUSATRAC = "CANOPUS_ATRAC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ESPCM = "ESPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CIRRUS = "CIRRUS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_MPEGLAYER3 = "MPEGLAYER3";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_MPEG = "MPEG";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_G728CELP = "G728_CELP";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_G721ADPCM = "G721_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_XEBEC = "XEBEC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ROCKWELLDIGITALK = "ROCKWELL_DIGITALK";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ROCKWELLADPCM = "ROCKWELL_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ECHOSC3 = "ECHOSC3";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CSIMAADPCM = "CS_IMAADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_NMSVBXADPCM = "NMS_VBXADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CONTROLRESCR10 = "CONTROL_RES_CR10";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DIGIADPCM = "DIGIADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DIGIREAL = "DIGIREAL";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_CONTROLRESVQLPC = "CONTROL_RES_VQLPC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ANTEXADPCME = "ANTEX_ADPCME";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_MSNAUDIO = "MSNAUDIO";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_GSM610 = "GSM610";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DOLBYAC2 = "DOLBY_AC2";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_AUDIOFILEAF10 = "AUDIOFILE_AF10";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_APTX = "APTX";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_AUDIOFILEAF36 = "AUDIOFILE_AF36";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ECHOSC1 = "ECHOSC1";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DSPGROUPTRUESPEECH = "DSPGROUP_TRUESPEECH";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_SONARC = "SONARC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_YAMAHAADPCM = "YAMAHA_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_MEDIAVISIONADPCM = "MEDIAVISION_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DIALOGICOKIADPCM = "DIALOGIC_OKI_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DIGIFIX = "DIGIFIX";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DIGISTD = "DIGISTD";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_G723ADPCM = "G723_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_SIERRAADPCM = "SIERRA_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_MEDIASPACEADPCM = "MEDIASPACE_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_DVIADPCM = "DVI_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_OKIADPCM = "OKI_ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_MULAW = "MULAW";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ALAW = "ALAW";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_IBMCVSD = "IBM_CVSD";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_IEEEFLOAT = "IEEE_FLOAT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_ADPCM = "ADPCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string AUD_PCM = "PCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_UnKnown = "Unknown ";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_VOXWAREMetaSound = "VOXWARE_MetaSound";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DIVX = "DIVX";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MSMPEG4 = "MS-MPEG4";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_PROVIDER = "PROVIDER";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DSI = "DSI";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_PCI = "PCI";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_SDDS = "SDDS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DVDLPCMAUDIO = "DVD_LPCM_AUDIO";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DTS = "DTS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DVDSUBPICTURE = "DVD_SUBPICTURE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DOLBYAC3 = "DOLBY_AC3";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG2AUDIO = "MPEG2_AUDIO";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG2TRANSPORT = "MPEG2_TRANSPORT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG2PROGRAM = "MPEG2_PROGRAM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG2VIDEO = "MPEG2_VIDEO";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoSECAML = "AnalogVideo_SECAM_L";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoSECAMK1 = "AnalogVideo_SECAM_K1";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoSECAMK = "AnalogVideo_SECAM_K";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoSECAMH = "AnalogVideo_SECAM_H";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoSECAMG = "AnalogVideo_SECAM_G";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoSECAMD = "AnalogVideo_SECAM_D";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoSECAMB = "AnalogVideo_SECAM_B";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALNCOMBO = "AnalogVideo_PAL_N_COMBO";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALN = "AnalogVideo_PAL_N";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALM = "AnalogVideo_PAL_M";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALI = "AnalogVideo_PAL_I";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALH = "AnalogVideo_PAL_H";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALG = "AnalogVideo_PAL_G";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALD = "AnalogVideo_PAL_D";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoPALB = "AnalogVideo_PAL_B";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AnalogVideoNTSCM = "AnalogVideo_NTSC_M";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_VPVBI = "VPVBI";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_VPVideo = "VPVideo";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DssAudio = "DssAudio";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DssVideo = "DssVideo";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RAWSPORT = "RAW_SPORT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_SPDIFTAG241h = "SPDIF_TAG_241h";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DOLBYAC3SPDIF = "DOLBY_AC3_SPDIF";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_IEEEFLOAT = "IEEE_FLOAT";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DRMAudio = "DRM_Audio";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Line21VBIRawData = "Line21_VBIRawData";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Line21GOPPacket = "Line21_GOPPacket";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Line21BytePair = "Line21_BytePair";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Dvsl = "dvsl";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Dvhd = "dvhd";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Dvsd = "dvsd_";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AIFF = "AIFF";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_AU = "AU";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_PCMAudioObsolete = "PCMAudio_Obsolete";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_WAVE = "WAVE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_QTJpeg = "QTJpeg";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_PCM = "PCM";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_QTRpza = "QTRpza";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_QTRle = "QTRle";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Asf = "ASF";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_QTMovie = "QTMovie";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_QTSmc = "QTSmc";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Avi = "AVI";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG1Audio = "MPEG1Audio";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG1Video = "MPEG1Video";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG1VideoCD = "MPEG1VideoCD";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG1System = "MPEG1System";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG1AudioPayload = "MPEG1AudioPayload";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG1Payload = "MPEG1Payload";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MPEG1Packet = "MPEG1Packet";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Overlay = "Overlay";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_ARGB32 = "ARGB32";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RGB32 = "RGB32";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RGB24 = "RGB24";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RGB555 = "RGB555";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RGB565 = "RGB565";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RGB8 = "RGB8";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RGB4 = "RGB4";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_RGB1 = "RGB1";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MDVF = "MDVF";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_IJPG = "IJPG";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_DVCS = "DVCS";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Plum = "Plum";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_CFCC = "CFCC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_TVMJ = "TVMJ";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_WAKE = "WAKE";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_MJPG = "MJPG";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_CLJR = "CLJR";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Y211 = "Y211";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_CPLA = "CPLA";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_IF09 = "IF09";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_YV12 = "YV12";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_UYVY = "UYVY";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_YVYU = "YVYU";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_YUY2 = "YUY2";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Y41P = "Y41P";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_Y411 = "Y411";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_YVU9 = "YVU9";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_IYUV = "IYUV";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_YUYV = "YUYV";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string MST_CLPL = "CLPL";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string ActiveMovieGraph = "ActiveMovieGraph";

    //    // ReSharper disable InconsistentNaming

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_VideoMixingRenderer9 = "51b4abf3-748f-4e3b-a276-c828330e926a";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_EnhancedVideoRenderer = "FA10746C-9B63-4b6c-BC49-FC300EA5F256";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_AudioRender = "E30629D1-27E5-11CE-875D-00608CB78066";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_DSoundRender = "79376820-07D0-11CF-A24D-0020AFD79767";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_DVDNavigator = "9B8C4620-2C1A-11d0-8493-00A02438AD48";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_VideoMixingRenderer = "B87BEB7B-8D29-423f-AE4D-6582C10175AC";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_VideoRenderer = "70e102b0-5556-11ce-97c0-00aa0055595a";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_VideoRendererDefault = "6BC1CFFA-8FC1-4261-AC22-CFB4CC38DB50";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_DirectVobSub = "9852A670-F845-491B-9BE6-EBD841B8A613";

    //    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    //        Justification = "Reviewed. Suppression is OK here.")]
    //    public const string CLSID_DirectVobSubManual = "93A22E7A-5091-45EF-BA61-6DA26156A5D0";

    //    // ReSharper restore InconsistentNaming
    //}

//    /// <summary>
//    /// DirectShow helper class.
//    /// </summary>
//    [Localizable(false)]
//    public static class DSHelper
//    {
//        // ReSharper disable InconsistentNaming

//        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
//            Justification = "Reviewed. Suppression is OK here.")]
//        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
//        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);

//        [DllImport("olepro32.dll")]
//        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
//            Justification = "Reviewed. Suppression is OK here.")]
//        public static extern int OleCreatePropertyFrame(
//            IntPtr hwndOwner,
//            int x,
//            int y,
//            [MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
//            int objectsCount,
//            [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] ref object objUnknown,
//            int pagesCount,
//            IntPtr pageClassID,
//            int lcid,
//            int reserved,
//            IntPtr lpvReserved);

//        // ReSharper restore InconsistentNaming

//        /// <summary>
//        /// Remover bad codecs.
//        /// </summary>
//        /// <param name="codecs">
//        /// Codecs list.
//        /// </param>
//        public static void RemoveBadVideoCodecs(ref List<string> codecs)
//        {
//            if (codecs != null)
//            {
//                codecs.Remove("Intel IYUV codec");
//                codecs.Remove("Intel IYUV codec");
//                codecs.Remove("Microsoft RLE");
//                codecs.Remove("Microsoft Video 1");
//                codecs.Remove("MSScreen 9 encoder DMO");
//                codecs.Remove("WMVideo8 Encoder DMO");
//                codecs.Remove("WMVideo9 Encoder DMO");
//                codecs.Remove("DivX 6.8.5 YV12 Decoder");
//            }
//        }

//        /// <summary>
//        /// Shows pin info while debugging.
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        public static void DebugPinInfo(IPin pin)
//        {
//            try
//            {
//                if (pin == null)
//                {
//                    return;
//                }

//                PinInfo pinInfo;
//                pin.QueryPinInfo(out pinInfo);

//                Debug.Print("Pin info:");
//                Debug.Print("Name:" + pinInfo.name);

//                if (pinInfo.dir == PinDirection.Input)
//                {
//                    Debug.Print("Direction: Input");
//                }
//                else
//                {
//                    Debug.Print("Direction: Output");
//                }
//            }
//            catch
//            {
//            }
//        }

//        /// <summary>
//        /// Shows filter info while debugging.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        public static void DebugFilterInfo(IBaseFilter filter)
//        {
//            try
//            {
//                if (filter == null)
//                {
//                    return;
//                }

//                FilterInfo filterInfo;
//                filter.QueryFilterInfo(out filterInfo);

//                Guid clsid;
//                filter.GetClassID(out clsid);

//                Debug.Print("Filter info:");
//                Debug.Print("Name:" + filterInfo.achName);
//                Debug.Print("CLSID:" + clsid.ToString());
//            }
//            catch
//            {
//            }
//        }

//        ///// <summary>
//        ///// Releases COM object.
//        ///// </summary>
//        ///// <param name="com">
//        ///// COM object.
//        ///// </param>
//        //public static void Marshal.ReleaseComObject(object com)
//        //{
//        //    try
//        //    {
//        //        if (com == null)
//        //        {
//        //            return;
//        //        }

//        //        Marshal.ReleaseComObject(com);
//        //        // ReSharper disable RedundantAssignment
//        //        com = null;
//        //        // ReSharper restore RedundantAssignment
//        //    }
//        //    catch
//        //    {
//        //    }
//        //}

//        /// <summary>
//        /// Gets pin video info.
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        /// <param name="width">
//        /// Width.
//        /// </param>
//        /// <param name="height">
//        /// Height.
//        /// </param>
//        /// <returns>
//        /// Returns true if operation successful.
//        /// </returns>
//        public static bool GetPinVideoInfo(IPin pin, out int width, out int height)
//        {
//            width = 0;
//            height = 0;

//            VFEnumMediaType mts = new VFEnumMediaType(pin);

//            for (int i = 0; i < mts.Count; i++)
//            {
//                AMMediaType mt = mts.Items(i);

//                if ((mt.formatSize > 0) && (mt.formatPtr != IntPtr.Zero))
//                {
//                    if (mt.formatType == FormatType.VideoInfo)
//                    {
//                        VideoInfoHeader header =
//                            (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));
//                        width = header.BmiHeader.Width;
//                        height = Math.Abs(header.BmiHeader.Height);

//                        mts.Clear();

//                        return true;
//                    }
//                }
//            }

//            return false;
//        }

//        /// <summary>
//        /// Detects application type - X86 or X64.
//        /// </summary>
//        /// <returns>
//        /// Returns true if X64.
//        /// </returns>
//        public static bool IsX64App()
//        {
//            return IntPtr.Size == 8;
//        }

//        /// <summary>
//        /// Gets filter video output pin.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="outputPin">
//        /// Output pin.
//        /// </param>
//        /// <param name="streamIndex">
//        /// Video pin index.
//        /// </param>
//        /// <returns>
//        /// Returns video output pin.
//        /// </returns>
//        public static bool GetFilterVideoOutputPin(IBaseFilter filter, out IPin outputPin, int streamIndex = 0)
//        {
//            bool result = false;
//            outputPin = null;
//            int index = 0;

//            try
//            {
//                VFPinList pl1 = new VFPinList(filter);
//                bool f = false;
//                VFEnumMediaType videoMediaTypes = new VFEnumMediaType();

//                for (int i = 0; i < pl1.Count(); i++)
//                {
//                    if (pl1.PinInfo(i).dir == PinDirection.Output)
//                    {
//                        videoMediaTypes.Assign(pl1[i]);
//                        for (int j = 0; j < videoMediaTypes.Count; j++)
//                        {
//                            if ((videoMediaTypes.Items(j).majorType == MediaType.Video)
//                                || (videoMediaTypes.Items(j).majorType == MediaType.Interleaved)
//                                || (videoMediaTypes.Items(j).majorType == MediaType.Stream)
//                                || (videoMediaTypes.Items(j).majorType == MediaType.AnalogVideo)
//                                || (videoMediaTypes.Items(j).majorType == MediaType.Mpeg2Sections)
//                                || (videoMediaTypes.Items(j).majorType == MediaType.Null))
//                            {
//                                if (index == streamIndex)
//                                {
//                                    outputPin = pl1[i];
//                                    pl1.Used[i] = true;
//                                    f = true;
//                                    break;
//                                }
//                                else
//                                {
//                                    index++;
//                                    break;
//                                }
//                            }
//                        }

//                        if (f)
//                        {
//                            break;
//                        }
//                    }
//                }

//                if (outputPin != null)
//                {
//                    result = true;
//                }

//                videoMediaTypes.Clear();
//                pl1.Clear();
//            }
//            catch
//            {
//            }

//            return result;
//        }

//        /// <summary>
//        /// Gets video capture device output pin.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <returns>
//        /// Returns output pin.
//        /// </returns>
//        public static IPin GetVideoCaptureDeviceOutputPin(IBaseFilter filter)
//        {
//            IPin pin = DsFindPin.ByDirection(filter, PinDirection.Output, 0);

//            return pin;
//        }

//        /// <summary>
//        /// Converts audio tags to string.
//        /// </summary>
//        /// <param name="code">
//        /// Audio tag code.
//        /// </param>
//        /// <returns>
//        /// Returns string from audio tag.
//        /// </returns>
//        public static string AudioTagToString(int code)
//        {
//            string result;

//            switch (code)
//            {
//                case 0x0001:
//                    result = "PCM";
//                    break;
//                case 0x0002:
//                    // common
//                    result = "ADPCM";
//                    break;
//                case 0x0003:
//                    result = "IEEE_FLOAT";
//                    break;
//                case 0x0005:
//                    result = "IBM_CVSD";
//                    break;
//                case 0x0006:
//                    result = "ALAW";
//                    break;
//                case 0x0007:
//                    result = "MULAW";
//                    break;
//                case 0x0010:
//                    result = "OKI_ADPCM";
//                    break;
//                case 0x0011:
//                    result = "DVI_ADPCM";
//                    break;
//                case 0x0012:
//                    result = "MEDIASPACE_ADPCM";
//                    break;
//                case 0x0013:
//                    result = "SIERRA_ADPCM";
//                    break;
//                case 0x0014:
//                    result = "G723_ADPCM";
//                    break;
//                case 0x0015:
//                    result = "DIGISTD";
//                    break;
//                case 0x0016:
//                    result = "DIGIFIX";
//                    break;
//                case 0x0017:
//                    result = "DIALOGIC_OKI_ADPCM";
//                    break;
//                case 0x0018:
//                    result = "MEDIAVISION_ADPCM";
//                    break;
//                case 0x0020:
//                    result = "YAMAHA_ADPCM";
//                    break;
//                case 0x0021:
//                    result = "SONARC";
//                    break;
//                case 0x0022:
//                    result = "DSPGROUP_TRUESPEECH";
//                    break;
//                case 0x0023:
//                    result = "ECHOSC1";
//                    break;
//                case 0x0024:
//                    result = "AUDIOFILE_AF36";
//                    break;
//                case 0x0025:
//                    result = "APTX";
//                    break;
//                case 0x0026:
//                    result = "AUDIOFILE_AF10";
//                    break;
//                case 0x0030:
//                    result = "DOLBY_AC2";
//                    break;
//                case 0x0031:
//                    result = "GSM610";
//                    break;
//                case 0x0032:
//                    result = "MSNAUDIO";
//                    break;
//                case 0x0033:
//                    result = "ANTEX_ADPCME";
//                    break;
//                case 0x0034:
//                    result = "CONTROL_RES_VQLPC";
//                    break;
//                case 0x0035:
//                    result = "DIGIREAL";
//                    break;
//                case 0x0036:
//                    result = "DIGIADPCM";
//                    break;
//                case 0x0037:
//                    result = "CONTROL_RES_CR10";
//                    break;
//                case 0x0038:
//                    result = "NMS_VBXADPCM";
//                    break;
//                case 0x0039:
//                    result = "CS_IMAADPCM";
//                    break;
//                case 0x003A:
//                    result = "ECHOSC3";
//                    break;
//                case 0x003B:
//                    result = "ROCKWELL_ADPCM";
//                    break;
//                case 0x003C:
//                    result = "ROCKWELL_DIGITALK";
//                    break;
//                case 0x003D:
//                    result = "XEBEC";
//                    break;
//                case 0x0040:
//                    result = "G721_ADPCM";
//                    break;
//                case 0x0041:
//                    result = "G728_CELP";
//                    break;
//                case 0x0050:
//                    result = "MPEG";
//                    break;
//                case 0x0055:
//                    result = "MPEGLAYER3";
//                    break;
//                case 0x0060:
//                    result = "CIRRUS";
//                    break;
//                case 0x0061:
//                    result = "ESPCM";
//                    break;
//                case 0x0062:
//                    result = "VOXWARE";
//                    break;
//                case 0x0063:
//                    result = "CANOPUS_ATRAC";
//                    break;
//                case 0x0064:
//                    result = "G726_ADPCM";
//                    break;
//                case 0x0065:
//                    result = "G722_ADPCM";
//                    break;
//                case 0x0066:
//                    result = "DSAT";
//                    break;
//                case 0x0067:
//                    result = "DSAT_DISPLAY";
//                    break;
//                case 0x0075:
//                    result = "VOXWARE";
//                    break;
//                case 0x0080:
//                    result = "SOFTSOUND";
//                    break;
//                case 0x0100:
//                    result = "RHETOREX_ADPCM";
//                    break;
//                case 0x0200:
//                    result = "CREATIVE_ADPCM";
//                    break;
//                case 0x0202:
//                    result = "CREATIVE_FASTSPEECH8";
//                    break;
//                case 0x0203:
//                    result = "CREATIVE_FASTSPEECH10";
//                    break;
//                case 0x0220:
//                    result = "QUARTERDECK";
//                    break;
//                case 0x0300:
//                    result = "FM_TOWNS_SND";
//                    break;
//                case 0x0400:
//                    result = "BTV_DIGITAL";
//                    break;
//                case 0x1000:
//                    result = "OLIGSM";
//                    break;
//                case 0x1001:
//                    result = "OLIADPCM";
//                    break;
//                case 0x1002:
//                    result = "OLICELP";
//                    break;
//                case 0x1003:
//                    result = "OLISBC";
//                    break;
//                case 0x1004:
//                    result = "OLIOPR";
//                    break;
//                case 0x1100:
//                    result = "LH_CODEC";
//                    break;
//                case 0x1400:
//                    result = "NORRIS";
//                    break;
//                default:
//                    result = "Unknown";
//                    break;
//            }

//            return result;
//        }

//        /// <summary>
//        /// Converts major type to string.
//        /// </summary>
//        /// <param name="guid">
//        /// Guid.
//        /// </param>
//        /// <returns>
//        /// Returns major type.
//        /// </returns>
//        public static string MajorTypeToString(Guid guid)
//        {
//            if (guid == MediaType.AnalogAudio)
//            {
//                return "AnalogAudio";
//            }

//            if (guid == MediaType.AnalogVideo)
//            {
//                return "AnalogVideo";
//            }

//            if (guid == MediaType.Audio)
//            {
//                return "Audio";
//            }

//            if (guid == MediaType.AuxLine21Data)
//            {
//                return "AUXLine21Data";
//            }

//            if (guid == MediaType.File)
//            {
//                return "File";
//            }

//            if (guid == MediaType.Interleaved)
//            {
//                return "Interleaved";
//            }

//            if (guid == MediaType.LMRT)
//            {
//                return "LMRT";
//            }

//            if (guid == MediaType.Midi)
//            {
//                return "Midi";
//            }

//            if (guid == MediaType.ScriptCommand)
//            {
//                return "ScriptCommand";
//            }

//            if (guid == MediaType.Stream)
//            {
//                return "Stream";
//            }

//            if (guid == MediaType.Texts)
//            {
//                return "Text";
//            }

//            if (guid == MediaType.Timecode)
//            {
//                return "Timecode";
//            }

//            if (guid == MediaType.URLStream)
//            {
//                return "URL_STREAM";
//            }

//            if (guid == MediaType.Video)
//            {
//                return "Video";
//            }

//            if (guid == MediaType.VBI)
//            {
//                return "VBI";
//            }

//            if (guid == MediaType.Mpeg2Sections)
//            {
//                return "MPEG2_SECTIONS";
//            }

//            return "Unknown: " + guid;
//        }

//        /// <summary>
//        /// Gets FourCC code.
//        /// </summary>
//        /// <param name="fourCC">
//        /// FourCC code as string.
//        /// </param>
//        /// <returns>
//        /// Returns FourCC code.
//        /// </returns>
//        [Localizable(false)]
//        public static int MakeFOURCC(string fourCC)
//        {
//            byte[] code = Encoding.ASCII.GetBytes(fourCC);
//            return BitConverter.ToInt32(code, 0);
//        }

//        /// <summary>
//        /// Gets FourCC code.
//        /// </summary>
//        /// <param name="fourCC">
//        /// FourCC code as string.
//        /// </param>
//        /// <returns>
//        /// Returns FourCC code.
//        /// </returns>
//        public static uint MakeFOURCCU(string fourCC)
//        {
//            byte[] code = Encoding.ASCII.GetBytes(fourCC);
//            return BitConverter.ToUInt32(code, 0);
//        }

//        /// <summary>
//        /// Gets encoder Class ID.
//        /// </summary>
//        /// <param name="mime">
//        /// Mime code.
//        /// </param>
//        /// <param name="clsid">
//        /// Encoder ClassID.
//        /// </param>
//        /// <returns>
//        /// Returns true if the operation was successful.
//        /// </returns>
//        public static bool GetEncoderClsid(string mime, out Guid clsid)
//        {
//            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

//            int numCodecs = codecs.GetLength(0);

//            // Check to determine whether any codecs were found.
//            if (numCodecs > 0)
//            {
//                for (int i = 0; i < numCodecs; i++)
//                {
//                    if (codecs[i].MimeType == mime)
//                    {
//                        clsid = codecs[i].Clsid;
//                        return true;
//                    }
//                }
//            }

//            clsid = Guid.Empty;
//            return false;
//        }
        
//        /// <summary>
//        /// Gets frame from the file.
//        /// </summary>
//        /// <param name="filename">
//        /// File name.
//        /// </param>
//        /// <param name="time">
//        /// Time (in milliseconds).
//        /// </param>
//        /// <returns>
//        /// Returns frame as image or null if method was unable to get a frame.
//        /// </returns>
//        public static Bitmap GetFrameFromFile(string filename, int time)
//        {

//            //try
//            //{
//            //    IFilterGraph2 graphBuilder;
//            //    ISampleGrabber sampleGrabber;
//            //    IBaseFilter sgFilter;
//            //    IBaseFilter sourceFilter;
//            //    //Type comType = null;
//            //    ICaptureGraphBuilder2 captureGraph;
//            //    //IMediaSeeking mediaSeeking;


//            //    DSNullRenderer nullRenderer = new DSNullRenderer();

//            //    object comObj = null;
//            //    int hr;
                
//            //    try
//            //    {
//            //        graphBuilder = (IFilterGraph2)new FilterGraph();
//            //        captureGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
//            //        captureGraph.SetFiltergraph(graphBuilder);

//            //        sampleGrabber = (ISampleGrabber)new SampleGrabber();

//            //        AMMediaType media = new AMMediaType();
//            //        media.majorType = MediaType.Video;
//            //        media.subType = MediaSubType.RGB24;
//            //        media.formatType = FormatType.VideoInfo;
//            //        hr = sampleGrabber.SetMediaType(media);

//            //        if (hr < 0)
//            //        {
//            //            Marshal.ThrowExceptionForHR(hr);
//            //        }

//            //        IMediaControl mediaCtrl = (IMediaControl)graphBuilder;
//            //        IMediaSeeking mediaSeeking = (IMediaSeeking)graphBuilder; 
                    
//            //        sgFilter = (IBaseFilter)sampleGrabber;
//            //        graphBuilder.AddFilter(sgFilter, "SampleGrabber");
                    
//            //        graphBuilder.AddSourceFilter(filename, "Source", out sourceFilter);

//            //        nullRenderer.Init(graphBuilder);

//            //        //if (captureGraph.RenderStream(null, MediaType.Video, sourceFilter, sgFilter, nullRenderer.Filter) != 0)
//            //        //{
//            //        //    if (captureGraph.RenderStream(null, MediaType.Stream, sourceFilter, sgFilter, nullRenderer.Filter) != 0)
//            //        //    {
//            //        //        if (captureGraph.RenderStream(null, null, sourceFilter, sgFilter, nullRenderer.Filter) != 0)
//            //        //        {

//            //        //            //all bad... but who knows ;)
//            //        //        }
//            //        //    }
//            //        //}

//            //        captureGraph.RenderStream(null, MediaType.Video, sourceFilter, sgFilter, nullRenderer.Filter);
//            //        captureGraph.RenderStream(null, MediaType.Stream, sourceFilter, sgFilter, nullRenderer.Filter);
//            //        captureGraph.RenderStream(null, null, sourceFilter, sgFilter, nullRenderer.Filter);
                    
//            //        media = new AMMediaType();
//            //        hr = sampleGrabber.GetConnectedMediaType(media);         

//            //        if (hr < 0)
//            //        {
//            //            return null;
//            //        }

//            //        if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
//            //        {
//            //            return null;
//            //        }

//            //        VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
//            //        Marshal.FreeCoTaskMem(media.formatPtr); media.formatPtr = IntPtr.Zero;
                    
//            //        mediaCtrl.Run();
                    
//            //        hr = sampleGrabber.SetBufferSamples(true);

//            //        if (hr == 0)
//            //        {
//            //            hr = sampleGrabber.SetOneShot(true);
//            //        }

//            //        if (hr < 0)
//            //        {
//            //            Marshal.ThrowExceptionForHR(hr);
//            //        }

//            //        int bufferSize = 0;
//            //        IntPtr pBuffer;

//            //        mediaSeeking.SetPositions(time * 10000, AMSeekingSeekingFlags.AbsolutePositioning, 0, AMSeekingSeekingFlags.NoPositioning);
                    
//            //        sampleGrabber.GetCurrentBuffer(ref bufferSize, IntPtr.Zero);
//            //        pBuffer = Marshal.AllocHGlobal(bufferSize);
//            //        sampleGrabber.GetCurrentBuffer(ref bufferSize, pBuffer);
                    
//            //        mediaCtrl.Stop();
                    
//            //        int w = videoInfoHeader.BmiHeader.Width;
//            //        int h = videoInfoHeader.BmiHeader.Height;

//            //        if (((w & 0x03) != 0) || (w < 32) || (w > 4096) || (h < 32) || (h > 4096))
//            //        {
//            //            return null;
//            //        }

//            //        int stride = w * 3;
//            //        int scan0 = (int)pBuffer + (h - 1) * stride;

//            //        Marshal.ReleaseComObject(graphBuilder);
//            //        Marshal.ReleaseComObject(sampleGrabber);
//            //        Marshal.ReleaseComObject(captureGraph);
//            //        Marshal.ReleaseComObject(mediaSeeking);
//            //        Marshal.ReleaseComObject(mediaCtrl);
//            //        Marshal.ReleaseComObject(sourceFilter);
//            //        Marshal.ReleaseComObject(sgFilter);

//            //        return new Bitmap(w, h, -stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, (IntPtr)scan0);
//            //    }
//            //    catch 
//            //    {
//            //        return null;
//            //    }

//            //    return null;
//            //}
//            //catch
//            //{
//            //}


//            try
//            {
//                Bitmap result = null;

//                IMediaDet md = (IMediaDet)new MediaDet();
//                md.put_Filename(filename);

//                int streamsCount;
//                md.get_OutputStreams(out streamsCount);

//                for (int i = 0; i < streamsCount; i++)
//                {
//                    md.put_CurrentStream(i);

//                    Guid majorType;
//                    md.get_StreamType(out majorType);

//                    if (majorType == MediaType.Video)
//                    {
//                        double duration;
//                        md.get_StreamLength(out duration);

//                        AMMediaType mt = new AMMediaType();
//                        md.get_StreamMediaType(mt);

//                        VideoInfoHeader vih = (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));
//                        int width = vih.BmiHeader.Width;
//                        int height = vih.BmiHeader.Height;

//                        IntPtr bufPtr = IntPtr.Zero;
//                        int bufSize;

//                        // call once to get the buffer size needed
//                        md.GetBitmapBits(0.0, out bufSize, bufPtr, width, height);

//                        // allocate memory
//                        bufPtr = Marshal.AllocHGlobal(bufSize);

//                        // call to retrieve the dib
//                        if (time <= (duration * 1000))
//                        {
//                            md.GetBitmapBits(time, out bufSize, bufPtr, width, height);
//                        }
//                        else
//                        {
//                            md.GetBitmapBits(0.0, out bufSize, bufPtr, width, height);
//                        }

//                        // create a bitmap object; 40 is the size of the header on a 32-bit machine
//                        result = new Bitmap(
//                            width, height, width * 3, PixelFormat.Format24bppRgb, new IntPtr((int)bufPtr + 40));

//                        result.RotateFlip(RotateFlipType.Rotate180FlipX);
                        
//                        break;
//                    }
//                }

//                if (md != null)
//                {
//                    Marshal.ReleaseComObject(md);
//                }

//                return result;
//            }
//            catch 
//            {
//            }

//            return null;
//        }

//        /// <summary>
//        /// Applies video format.
//        /// </summary>
//        /// <param name="streamConfig">
//        /// Stream config.
//        /// </param>
//        /// <param name="videoCaptureFormat">
//        /// Video capture format.
//        /// </param>
//        /// <param name="frameRate">
//        /// Frame rate.
//        /// </param>
//        /// <returns>
//        /// Returns true if the operation was successful.
//        /// </returns>
//        public static bool ApplyVideoFormat(
//            IAMStreamConfig streamConfig, VFVideoCaptureFormat videoCaptureFormat, double frameRate)
//        {
//            AMMediaType mt = new AMMediaType();
//            bool result = false;
//            try
//            {
//                if (streamConfig == null)
//                {
//                    return false;
//                }

//                if (videoCaptureFormat == null)
//                {
//                    return false;
//                }

//                mt.majorType = videoCaptureFormat.MajorType;
//                mt.subType = videoCaptureFormat.SubType;
//                mt.formatType = videoCaptureFormat.FormatType;
//                mt.fixedSizeSamples = videoCaptureFormat.FixedSizeSamples;
//                mt.sampleSize = videoCaptureFormat.SampleSize;
//                mt.temporalCompression = videoCaptureFormat.TemporalCompression;

//                // frame rate
//                if (Math.Abs(frameRate - 0) < double.Epsilon)
//                {
//                    frameRate = 1;
//                }

//                videoCaptureFormat.VIH.AvgTimePerFrame = (long)Math.Round(10000000 / frameRate);

//                if (mt.formatType == FormatType.VideoInfo)
//                {
//                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.VIH));
//                    Marshal.StructureToPtr(videoCaptureFormat.VIH, mt.formatPtr, true);
//                    mt.formatSize = Marshal.SizeOf(typeof(VideoInfoHeader));
//                }
//                else if (mt.formatType == FormatType.VideoInfo2)
//                {
//                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.VIH2));
//                    Marshal.StructureToPtr(videoCaptureFormat.VIH2, mt.formatPtr, true);
//                    mt.formatSize = Marshal.SizeOf(typeof(VideoInfoHeader2));
//                }
//                else if (mt.formatType == FormatType.DvInfo)
//                {
//                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.DV));
//                    Marshal.StructureToPtr(videoCaptureFormat.DV, mt.formatPtr, true);
//                    mt.formatSize = Marshal.SizeOf(typeof(DVInfo));
//                }
//                //else if (videoCaptureFormat.FormatType == FormatType.MpegVideo)
//                //{
//                //    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.MPEG1));
//                //    Marshal.StructureToPtr(videoCaptureFormat.MPEG1, mt.formatPtr, true);
//                //    mt.formatSize = Marshal.SizeOf(typeof(MPEG1VideoInfo));
//                //}
//                else if (videoCaptureFormat.FormatType == FormatType.Mpeg2Video)
//                {
//                    mt.formatPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(videoCaptureFormat.MPEG2));
//                    Marshal.StructureToPtr(videoCaptureFormat.MPEG2, mt.formatPtr, true);
//                    mt.formatSize = Marshal.SizeOf(typeof(MPEG2VideoInfo));
//                }
//                else
//                {
//                    return false;
//                }

//                int hr = streamConfig.SetFormat(mt);

//                if (hr == 0)
//                {
//                    result = true;
//                }
//            }
//            catch
//            {
//            }

//            return result;
//        }

//        /// <summary>
//        /// Gets pin state.
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        /// <returns>
//        /// Returns true if pin connected.
//        /// </returns>
//        public static bool PinConnected(IPin pin)
//        {
//            IPin pin2;

//            if (pin == null)
//            {
//                return false;
//            }

//            pin.ConnectedTo(out pin2);
//            if (pin2 != null)
//            {
//                Marshal.ReleaseComObject(pin2);
//                return true;
//            }

//            return false;
//        }

//        /// <summary>
//        /// Gets connected pin.
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        /// <returns>
//        /// Returns connected pin.
//        /// </returns>
//        public static IPin PinConnectedTo(IPin pin)
//        {
//            IPin pin2;

//            if (pin == null)
//            {
//                return null;
//            }

//            pin.ConnectedTo(out pin2);
//            return pin2;
//        }

//        /// <summary>
//        /// Checks that filter connected
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="noInputPinsResult">
//        /// Result if no input pins. 
//        /// </param>
//        /// <returns>
//        /// Returns true if the filter connected.
//        /// </returns>
//        public static bool IsFilterConnected(IBaseFilter filter, bool noInputPinsResult = false)
//        {
//            bool result;

//            try
//            {
//                result = false;

//                IPin pin1;
//                GetPin(filter, PinDirection.Input, 1, out pin1);
//                if (pin1 == null)
//                {
//                    return noInputPinsResult;
//                }

//                IPin pin2;
//                pin1.ConnectedTo(out pin2);
//                if (pin2 != null)
//                {
//                    result = true;
//                    Marshal.ReleaseComObject(pin2);
//                    // ReSharper disable RedundantAssignment
//                    pin2 = null;
//                    // ReSharper restore RedundantAssignment
//                }

//                Marshal.ReleaseComObject(pin1);
//                // ReSharper disable RedundantAssignment
//                pin1 = null;
//                // ReSharper restore RedundantAssignment
//            }
//            catch
//            {
//                return false;
//            }

//            return result;
//        }

//        /// <summary>
//        /// Gets letterbox coordinates.
//        /// </summary>
//        /// <param name="screenWidth">
//        /// Screen width.
//        /// </param>
//        /// <param name="screenHeight">
//        /// Screen height.
//        /// </param>
//        /// <param name="srcWidth">
//        /// Source video width.
//        /// </param>
//        /// <param name="srcHeight">
//        /// Source video height.
//        /// </param>
//        /// <returns>
//        /// Returns letterbox coordinates.
//        /// </returns>
//        public static Rectangle GetLetterboxCoordinates(int screenWidth, int screenHeight, int srcWidth, int srcHeight)
//        {
//            // ReSharper disable RedundantCast

//            if ((srcHeight == 0) || (srcWidth == 0) || (screenHeight == 0) || (screenWidth == 0))
//            {
//                return new Rectangle(0, 0, 0, 0);
//            }

//            int newScrWidth = 0;
//            int newScrHeight = 0;

//            if ((screenWidth > 0) && (screenHeight > 0))
//            {
//                if (((double)srcWidth / (double)srcHeight) > ((double)screenWidth / (double)screenHeight))
//                {
//                    newScrWidth = screenWidth;
//                    newScrHeight = (int)Math.Round((double)srcHeight / (double)srcWidth * (double)screenWidth);
//                }
//                else
//                {
//                    newScrWidth = (int)Math.Round((double)srcWidth / (double)srcHeight * (double)screenHeight);
//                    newScrHeight = screenHeight;
//                }
//            }

//            int newScrLeft = (int)Math.Round((double)(((double)screenWidth / 2.0) - ((double)newScrWidth / 2.0)));
//            int newScrTop = (int)Math.Round((double)(((double)screenHeight / 2.0) - ((double)newScrHeight / 2.0)));

//            return new Rectangle(newScrLeft, newScrTop, newScrWidth, newScrHeight);

//            // ReSharper restore RedundantCast
//        }

//        /// <summary>
//        /// Gets zoom position.
//        /// </summary>
//        /// <param name="srcWidth">
//        /// Source width.
//        /// </param>
//        /// <param name="srcHeight">
//        /// Source height.
//        /// </param>
//        /// <param name="shiftX">
//        /// Shift x.
//        /// </param>
//        /// <param name="shiftY">
//        /// Shift y.
//        /// </param>
//        /// <param name="zoom">
//        /// Zoom.
//        /// </param>
//        /// <returns>
//        /// Returns zoom position.
//        /// </returns>
//        public static DsRect GetZoomPos(int srcWidth, int srcHeight, int shiftX, int shiftY, int zoom)
//        {
//            // ReSharper disable RedundantCast
//            double ratio = (double)srcHeight / (double)srcWidth;
//            // ReSharper restore RedundantCast

//            double tmpX = srcWidth - ((zoom * srcWidth) / 100);
//            double tmpY = tmpX * ratio;

//            // TmpX := srcWidth * (zoom / 100);//  srcWidth - ((zoom * srcWidth) / 100);
//            // TmpY := TmpX * Ratio;

//            double tmpLeft = (srcWidth - tmpX) / 2;
//            double tmpTop = (srcHeight - tmpY) / 2;

//            int newLeft = (int)(tmpLeft + shiftX);
//            int newTop = (int)(tmpTop + shiftY);
//            int newRight = (int)(tmpX + tmpLeft + shiftX);
//            int newBottom = (int)(tmpY + tmpTop + shiftY);

//            return new DsRect(newLeft, newTop, newRight, newBottom);
//        }

//        /// <summary>
//        /// Checks that pin have specified media type. 
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        /// <param name="mediaType">
//        /// Media type.
//        /// </param>
//        /// <param name="subtype">
//        /// Sub type.
//        /// </param>
//        /// <returns>
//        /// Returns true in pin have media type.
//        /// </returns>
//        public static bool PinHaveThisType(IPin pin, Guid mediaType, Guid subtype)
//        {
//            bool result;
//            AMMediaType[] mt = new AMMediaType[1];

//            if (pin == null)
//            {
//                return false;
//            }

//            try
//            {
//                result = false;

//                IEnumMediaTypes mts;
//                pin.EnumMediaTypes(out mts);

//                if (mts == null)
//                {
//                    return false;
//                }

//                mts.Reset();

//                if (subtype == Guid.Empty)
//                {
//                    while (mts.Next(1, mt, IntPtr.Zero) == 0)
//                    {
//                        if (mt[0] != null)
//                        {
//                            if (mt[0].majorType == mediaType)
//                            {
//                                result = true;
//                                break;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    while (mts.Next(1, mt, IntPtr.Zero) == 0)
//                    {
//                        if (mt[0] != null)
//                        {
//                            if ((mt[0].majorType == mediaType) && (mt[0].subType == subtype))
//                            {
//                                result = true;
//                                break;
//                            }
//                        }
//                    }
//                }

//                Marshal.ReleaseComObject(mts);
//            }
//            catch
//            {
//                result = false;
//            }

//            return result;
//        }

//        /// <summary>
//        /// Checks that pin have specified media type. 
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        /// <param name="mediaType">
//        /// Media type.
//        /// </param>
//        /// <returns>
//        /// Returns true in pin have media type.
//        /// </returns>
//        public static bool PinHaveThisType(IPin pin, Guid mediaType)
//        {
//            bool result;
//            AMMediaType[] mt = new AMMediaType[1];

//            if (pin == null)
//            {
//                return false;
//            }

//            try
//            {
//                result = false;

//                IEnumMediaTypes mts;
//                pin.EnumMediaTypes(out mts);

//                if (mts == null)
//                {
//                    return false;
//                }

//                mts.Reset();

//                    while (mts.Next(1, mt, IntPtr.Zero) == 0)
//                    {
//                        if (mt[0] != null)
//                        {
//                            if (mt[0].majorType == mediaType)
//                            {
//                                result = true;
//                                break;
//                            }
//                        }
//                    }

//                Marshal.ReleaseComObject(mts);
//            }
//            catch
//            {
//                result = false;
//            }

//            return result;
//        }

//        /// <summary>
//        /// Gets pin with a specified media type.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="pinDirection">
//        /// Pin direction.
//        /// </param>
//        /// <param name="majorType">
//        /// Major type.
//        /// </param>
//        /// <param name="subType">
//        /// Subtype.
//        /// </param>
//        /// <param name="freePin">
//        /// Pin must be not connected.
//        /// </param>
//        /// <returns>
//        /// Returns pin.
//        /// </returns>
//        public static IPin GetPinWithMediaTypeAndSubtype(IBaseFilter filter, PinDirection pinDirection,
//            Guid majorType, Guid subType, bool freePin = false)
//        {
//            List<Guid> majorTypes = new List<Guid>
//                {
//                    majorType
//                };

//            List<Guid> subTypes = new List<Guid>
//                {
//                    subType
//                };

//            return GetPinWithMediaTypeAndSubtype(filter, pinDirection, majorTypes, subTypes, freePin);
//        }

//        /// <summary>
//        /// Gets pin connection status.
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        /// <returns>
//        /// 
//        /// </returns>
//        public static bool IsPinConnected(IPin pin)
//        {
//            IPin connectedPin;
//            pin.ConnectedTo(out connectedPin);

//            if (connectedPin != null)
//            {
//                Marshal.ReleaseComObject(connectedPin);
//                return true;
//            }

//            return false;
//        }
        
//        /// <summary>
//        /// Gets pin with a specified media type.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="pinDirection">
//        /// Pin direction.
//        /// </param>
//        /// <param name="majorTypes">
//        /// Media types.
//        /// </param>
//        /// <param name="subTypes">
//        /// Sub types.
//        /// </param>
//        /// <param name="freePin">
//        /// Pin must be not connected.
//        /// </param>
//        /// <returns>
//        /// Returns pin.
//        /// </returns>
//        public static IPin GetPinWithMediaTypeAndSubtype(IBaseFilter filter, PinDirection pinDirection,
//            List<Guid> majorTypes, List<Guid> subTypes, bool freePin = false)
//        {
//            IEnumPins enumPins;
//            IPin pinRet = null;
//            IPin[] pins = new IPin[1];

//            if ((filter == null) || (majorTypes.Count == 0) || (majorTypes.Count != subTypes.Count) || (subTypes.Count == 0))
//            {
//                return null;
//            }

//            // Get the pin enumerator
//            int hr = filter.EnumPins(out enumPins);
//            DsError.ThrowExceptionForHR(hr);

//            try
//            {
//                // Walk the pins looking for a match
//                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
//                {
//                    bool f = false;

//                    PinInfo pi;

//                    if (pins[0] == null)
//                    {
//                        continue;
//                    }

//                    pins[0].QueryPinInfo(out pi);

//                    if (pi.dir == pinDirection)
//                    {
//                        for (int i = 0; i < majorTypes.Count; i++)
//                        {
//                            if ((PinHaveThisType(pins[0], majorTypes[i], subTypes[i])))
//                            {
//                                if (!freePin || (freePin && !IsPinConnected(pins[0])))
//                                {
//                                    pinRet = pins[0];
//                                    // Marshal.ReleaseComObject(enumPins);
//                                    f = true;

//                                    break;
//                                }
//                            }
//                        }
//                    }

//                    if (f)
//                    {
//                        break;
//                    } 
                    
//                    Marshal.ReleaseComObject(pins[0]);
//                }
//            }
//            finally
//            {
//                if (enumPins != null)
//                {
//                    Marshal.ReleaseComObject(enumPins);
//                }
//            }

//            return pinRet;
//        }

//        /// <summary>
//        /// Gets free pin with a specified media type.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="pinDirection">
//        /// Pin direction.
//        /// </param>
//        /// <param name="guid">
//        /// Guid.
//        /// </param>
//        /// <returns>
//        /// Returns pin.
//        /// </returns>
//        public static IPin GetFreePinWithMediaType(IBaseFilter filter, PinDirection pinDirection, Guid guid)
//        {
//            IEnumPins enumPins;
//            IPin pinRet = null;
//            IPin[] pins = new IPin[1];

//            if (filter == null)
//            {
//                return null;
//            }

//            // Get the pin enumerator
//            int hr = filter.EnumPins(out enumPins);
//            DsError.ThrowExceptionForHR(hr);

//            try
//            {
//                // Walk the pins looking for a match
//                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
//                {
//                    PinInfo pi;
//                    pins[0].QueryPinInfo(out pi);
//                    if (pi.dir == pinDirection)
//                    {
//                        IPin pin2;
//                        pins[0].ConnectedTo(out pin2);
//                        if (pin2 != null)
//                        {
//                            Marshal.ReleaseComObject(pin2);
//                            continue;
//                        }

//                        if (PinHaveThisType(pins[0], guid))
//                        {
//                            pinRet = pins[0];
//                            break;
//                        }
//                    }

//                    Marshal.ReleaseComObject(pins[0]);
//                }
//            }
//            finally
//            {
//                if (enumPins != null)
//                {
//                    Marshal.ReleaseComObject(enumPins);
//                }
//            }

//            return pinRet;
//        }

//        /// <summary>
//        /// Gets free pin with a specified media type.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="pinDirection">
//        /// Pin direction.
//        /// </param>
//        /// <returns>
//        /// Returns pin.
//        /// </returns>
//        public static IPin GetFreePin(IBaseFilter filter, PinDirection pinDirection)
//        {
//            IEnumPins enumPins;
//            //IPin pinRet = null;
//            IPin[] pins = new IPin[1];

//            if (filter == null)
//            {
//                return null;
//            }

//            // Get the pin enumerator
//            int hr = filter.EnumPins(out enumPins);
//            DsError.ThrowExceptionForHR(hr);

//            try
//            {
//                // Walk the pins looking for a match
//                while (enumPins.Next(1, pins, IntPtr.Zero) == 0)
//                {
//                    PinInfo pi;
//                    pins[0].QueryPinInfo(out pi);
//                    if (pi.dir == pinDirection)
//                    {
//                        IPin pin2;
//                        pins[0].ConnectedTo(out pin2);
//                        if (pin2 != null)
//                        {
//                            Marshal.ReleaseComObject(pin2);
//                            continue;
//                        }

//                        break;
//                    }

//                    Marshal.ReleaseComObject(pins[0]);
//                }
//            }
//            finally
//            {
//                if (enumPins != null)
//                {
//                    Marshal.ReleaseComObject(enumPins);
//                }
//            }

//            return pins[0];
//        }
        
//        /// <summary>
//        /// Gets pin.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="pinDirection">
//        /// Pin direction.
//        /// </param>
//        /// <param name="index">
//        /// Pin index.
//        /// </param>
//        /// <param name="pin">
//        /// Output pin.
//        /// </param>
//        /// <returns>
//        /// Returns true or 0 if the operation was successful.
//        /// </returns>
//        public static bool GetPin(IBaseFilter filter, PinDirection pinDirection, int index, out IPin pin)
//        {
//            pin = DsFindPin.ByDirection(filter, pinDirection, index - 1);
//            return pin != null;
//        }

//        /// <summary>
//        /// Gets physical pin name.
//        /// </summary>
//        /// <param name="connectorType">
//        /// Connector type.
//        /// </param>
//        /// <returns>
//        /// Returns physical pin name.
//        /// </returns>
//        public static string Crossbar_GetPhysicalPinName(PhysicalConnectorType connectorType)
//        {
//            switch (connectorType)
//            {
//                case PhysicalConnectorType.Video_Tuner:
//                    return "Video Tuner";
//                case PhysicalConnectorType.Video_Composite:
//                    return "Video Composite";
//                case PhysicalConnectorType.Video_SVideo:
//                    return "S-Video";
//                case PhysicalConnectorType.Video_RGB:
//                    return "Video RGB";
//                case PhysicalConnectorType.Video_YRYBY:
//                    return "Video YRYBY";
//                case PhysicalConnectorType.Video_SerialDigital:
//                    return "Video Serial Digital";
//                case PhysicalConnectorType.Video_ParallelDigital:
//                    return "Video Parallel Digital";
//                case PhysicalConnectorType.Video_SCSI:
//                    return "Video SCSI";
//                case PhysicalConnectorType.Video_AUX:
//                    return "Video AUX";
//                case PhysicalConnectorType.Video_1394:
//                    return "Video 1394";
//                case PhysicalConnectorType.Video_USB:
//                    return "Video USB";
//                case PhysicalConnectorType.Video_VideoDecoder:
//                    return "Video Decoder";
//                case PhysicalConnectorType.Video_VideoEncoder:
//                    return "Video Encoder";
//                case PhysicalConnectorType.Audio_Tuner:
//                    return "Audio Tuner";
//                case PhysicalConnectorType.Audio_Line:
//                    return "Audio Line";
//                case PhysicalConnectorType.Audio_Mic:
//                    return "Audio Microphone";
//                case PhysicalConnectorType.Audio_AESDigital:
//                    return "Audio AES/EBU Digital";
//                case PhysicalConnectorType.Audio_SPDIFDigital:
//                    return "Audio S/PDIF";
//                case PhysicalConnectorType.Audio_SCSI:
//                    return "Audio SCSI";
//                case PhysicalConnectorType.Audio_AUX:
//                    return "Audio AUX";
//                case PhysicalConnectorType.Audio_1394:
//                    return "Audio 1394";
//                case PhysicalConnectorType.Audio_USB:
//                    return "Audio USB";
//                case PhysicalConnectorType.Audio_AudioDecoder:
//                    return "Audio Decoder";
//                default:
//                    return "Unknown Type";
//            }
//        }

//        /// <summary>
//        /// Checks pin.
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        public static void CheckPin(IPin pin)
//        {
//            if (pin != null)
//            {
//                PinInfo pi;
//                pin.QueryPinInfo(out pi);
//            }
//        }

//        /// <summary>
//        /// Checks if filter exists.
//        /// </summary>
//        /// <param name="filters">
//        /// Filter list.
//        /// </param>
//        /// <param name="filterName">
//        /// Filter name.
//        /// </param>
//        /// <returns>
//        /// Returns true if the filters exists.
//        /// </returns>
//        public static bool FilterExists(DsDevice[] filters, string filterName)
//        {
//            if (filters != null)
//            {
//                foreach (var device in filters)
//                {
//                    if (device.Name == filterName)
//                    {
//                        return true;
//                    }
//                }
//            }

//            return false;
//        }

//        /// <summary>
//        /// Counts filter pins.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        /// <param name="pulInPins">
//        /// Input pins count.
//        /// </param>
//        /// <param name="pulOutPins">
//        /// Output pins count.
//        /// </param>
//        /// <returns>
//        /// Returns true or 0 if the operation was successful.
//        /// </returns>
//        public static int CountFilterPins(IBaseFilter filter, out int pulInPins, out int pulOutPins)
//        {
//            IEnumPins enumPins;
//            IntPtr found = IntPtr.Zero;
//            IPin[] pins = new IPin[1];

//            // Verify input
//            if (filter == null)
//            {
//                pulInPins = 0;
//                pulOutPins = 0;
//                return ErrorCodes.E_POINTER;
//            }

//            // Clear number of pins found
//            pulInPins = 0;
//            pulOutPins = 0;

//            // Get pin enumerator
//            int result = filter.EnumPins(out enumPins);
//            if (result != 0)
//            {
//                return result;
//            }

//            enumPins.Reset();

//            // Count every pin on the filter
//            while (0 == enumPins.Next(1, pins, found))
//            {
//                PinDirection pinDir;
//                result = pins[0].QueryDirection(out pinDir);
//                if (pinDir == PinDirection.Input)
//                {
//                    pulInPins++;
//                }
//                else
//                {
//                    pulOutPins++;
//                }

//                Marshal.ReleaseComObject(pins[0]);
//                pins[0] = null;
//            }

//            // ReSharper disable RedundantAssignment

//            Marshal.ReleaseComObject(enumPins);
//            enumPins = null;

//            // ReSharper restore RedundantAssignment

//            return result;
//        }

//        /// <summary>
//        /// Finds splitter.
//        /// </summary>
//        /// <param name="graphBuilder">
//        /// Graph builder.
//        /// </param>
//        /// <param name="filterName">
//        /// Filter name.
//        /// </param>
//        public static void FindSplitter(IGraphBuilder graphBuilder, out string filterName)
//        {
//            IEnumFilters enumFilters;

//            filterName = string.Empty;

//            if (graphBuilder == null)
//            {
//                return;
//            }

//            int hr = graphBuilder.EnumFilters(out enumFilters);
//            if (hr != 0)
//            {
//                return;
//            }

//            IBaseFilter[] filters = new IBaseFilter[1];
//            while (enumFilters.Next(1, filters, IntPtr.Zero) == 0)
//            {
//                bool audioFound = false;
//                bool videoFound = false;
//                int inpins;
//                int outpins;

//                CountFilterPins(filters[0], out inpins, out outpins);
//                int i;
//                for (i = 0; i < outpins; i++)
//                {
//                    IPin pin;
//                    GetPin(filters[0], PinDirection.Output, i + 1, out pin);
//                    if (PinHaveThisType(pin, MediaType.Audio))
//                    {
//                        if (pin != null)
//                        {
//                            Marshal.ReleaseComObject(pin);
//                            pin = null;
//                        }

//                        audioFound = true;

//                        // break;
//                    }
//                    else
//                    {
//                        if (PinHaveThisType(pin, MediaType.Video))
//                        {
//                            if (pin != null)
//                            {
//                                Marshal.ReleaseComObject(pin);
//                                pin = null;
//                            }

//                            videoFound = true;

//                            // break;
//                        }
//                    }

//                    if (pin != null)
//                    {
//                        // ReSharper disable RedundantAssignment

//                        Marshal.ReleaseComObject(pin);
//                        pin = null;

//                        // ReSharper restore RedundantAssignment
//                    }

//                    if (audioFound && videoFound)
//                    {
//                        FilterInfo info;
//                        filters[0].QueryFilterInfo(out info);
//                        filterName = info.achName;
//                        break;
//                    }
//                }

//                if (filters[0] != null)
//                {
//                    Marshal.ReleaseComObject(filters[0]);
//                    filters[0] = null;
//                }

//                if (audioFound && videoFound)
//                {
//                    break;
//                }
//            }

//            // ReSharper disable RedundantAssignment
//            Marshal.ReleaseComObject(enumFilters);
//            enumFilters = null;
//            // ReSharper restore RedundantAssignment
//        }

//        /// <summary>
//        /// Removes duplicate list.
//        /// </summary>
//        /// <param name="list">
//        /// Input list.
//        /// </param>
//        /// <returns>
//        /// Returns result list.
//        /// </returns>
//        public static List<string> RemoveDuplicateStrings(List<string> list)
//        {
//            Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
//            List<string> finalList = new List<string>();
//            int i = 0;

//            if (list != null)
//            {
//                foreach (string currValue in list)
//                {
//                    if (!uniqueStore.ContainsKey(currValue))
//                    {
//                        uniqueStore.Add(currValue, 0);
//                        finalList.Add(list[i]);
//                    }

//                    i++;
//                }
//            }

//            return finalList;
//        }


//        /// <summary>
//        /// Draws uncompressed image.
//        /// </summary>
//        /// <param name="src">
//        /// Source image.
//        /// </param>
//        /// <param name="srcWidth">
//        /// Source width.
//        /// </param>
//        /// <param name="srcHeight">
//        /// Source height.
//        /// </param>
//        /// <param name="dest">
//        /// Destination image.
//        /// </param>
//        /// <param name="destWidth">
//        /// Destination width.
//        /// </param>
//        /// <param name="y">
//        /// Y.
//        /// </param>
//        /// <param name="draw32b">
//        /// True if RGB32 image used.
//        /// </param>
//        /// <param name="x">
//        /// X.
//        /// </param>
//        public static void DrawRGBImage(
//            IntPtr src, int srcWidth, int srcHeight, IntPtr dest, int destWidth, int x, int y, bool draw32b)
//        {
//            try
//            {
//                if (draw32b)
//                {
//                    if (src != IntPtr.Zero && dest != IntPtr.Zero)
//                    {
//                        int stride1 = destWidth * 4;
//                        int stride2 = srcWidth * 4;

//                        for (int i = y; i < srcHeight + y; i++)
//                        {
//                            IntPtr tmpDest = new IntPtr(dest.ToInt64() + (stride1 * i) + (x * 4));
//                            IntPtr tmpSrc = new IntPtr(src.ToInt64() + (stride2 * i) + (x * 4));

//                            CopyMemory(tmpDest, tmpSrc, stride2);
//                        }
//                    }
//                }
//                else
//                {
//                    if (src != IntPtr.Zero && dest != IntPtr.Zero)
//                    {
//                        int stride1 = destWidth * 3;
//                        int stride2 = srcWidth * 3;

//                        for (int i = y; i < srcHeight + y; i++)
//                        {
//                            IntPtr tmpDest = new IntPtr(dest.ToInt64() + (stride1 * i) + (x * 3));
//                            IntPtr tmpSrc = new IntPtr(src.ToInt64() + (stride2 * i) + (x * 3));

//                            CopyMemory(tmpDest, tmpSrc, stride2);
//                        }
//                    }
//                }
//            }
//            catch
//            {
//            }
//        }

//        /// <summary>
//        /// Disconnects filter.
//        /// </summary>
//        /// <param name="filter">
//        /// Filter.
//        /// </param>
//        public static void DisconnectFilter(IBaseFilter filter)
//        {
//            IPin pin1;

//            GetPin(filter, PinDirection.Input, 1, out pin1);
//            if (pin1 != null)
//            {
//                IPin pin2;
//                pin1.ConnectedTo(out pin2);
//                if (pin2 != null)
//                {
//                    pin1.Disconnect();
//                    pin2.Disconnect();

//                    Marshal.ReleaseComObject(pin2);
//                    // ReSharper disable RedundantAssignment
//                    pin2 = null;
//                    // ReSharper restore RedundantAssignment
//                }

//                Marshal.ReleaseComObject(pin1);
//                // ReSharper disable RedundantAssignment
//                pin1 = null;
//                // ReSharper restore RedundantAssignment
//            }
//        }

//        /// <summary>
//        /// Disconnects filter pin.
//        /// </summary>
//        /// <param name="pin">
//        /// Pin.
//        /// </param>
//        public static void DisconnectPin(IPin pin)
//        {
//            if (pin == null)
//            {
//                return;
//            }

//            IPin pin2;
//            pin.ConnectedTo(out pin2);
//            if (pin2 != null)
//            {
//                pin.Disconnect();
//                pin2.Disconnect();

//                Marshal.ReleaseComObject(pin2);
//                // ReSharper disable RedundantAssignment
//                pin2 = null;
//                // ReSharper restore RedundantAssignment
//            }
//        }

//        /// <summary>
//        /// Gets connected filter.
//        /// </summary>
//        /// <param name="filter">
//        /// Source filter.
//        /// </param>
//        /// <param name="pinDirection">
//        /// Pin direction.
//        /// </param>
//        /// <param name="destFilter">
//        /// Destination filter.
//        /// </param>
//        public static void FilterConnectedTo(IBaseFilter filter, PinDirection pinDirection, out IBaseFilter destFilter)
//        {
//            destFilter = null;

//            try
//            {
//                IPin pin1;
                
//                GetPin(filter, pinDirection, 1, out pin1);
//                if (pin1 != null)
//                {
//                    IPin pin2;
//                    pin1.ConnectedTo(out pin2);
//                    if (pin2 != null)
//                    {
//                        PinInfo pi;
//                        pin2.QueryPinInfo(out pi);
//                        destFilter = pi.filter;

//                        Marshal.ReleaseComObject(pin2);
//                        // ReSharper disable RedundantAssignment
//                        pin2 = null;
//                        // ReSharper restore RedundantAssignment
//                    }

//                    Marshal.ReleaseComObject(pin1);
//                    // ReSharper disable RedundantAssignment
//                    pin1 = null;
//                    // ReSharper restore RedundantAssignment
//                }
//            }
//            catch
//            {
//            }
//        }

//        /// <summary>
//        /// Gets connected filter.
//        /// </summary>
//        /// <param name="filter">
//        /// Source filter.
//        /// </param>
//        /// <param name="pinDirection">
//        /// Pin direction.
//        /// </param>
//        /// <returns>
//        /// Returns destination filter.
//        /// </returns>
//        public static IBaseFilter FilterConnectedTo(IBaseFilter filter, PinDirection pinDirection)
//        {
//            IBaseFilter destFilter = null;

//            try
//            {
//                IPin pin1;

//                GetPin(filter, pinDirection, 1, out pin1);
//                if (pin1 != null)
//                {
//                    IPin pin2;
//                    pin1.ConnectedTo(out pin2);
//                    if (pin2 != null)
//                    {
//                        PinInfo pi;
//                        pin2.QueryPinInfo(out pi);
//                        destFilter = pi.filter;

//                        Marshal.ReleaseComObject(pin2);
//                        // ReSharper disable RedundantAssignment
//                        pin2 = null;
//                        // ReSharper restore RedundantAssignment
//                    }

//                    Marshal.ReleaseComObject(pin1);
//                    // ReSharper disable RedundantAssignment
//                    pin1 = null;
//                    // ReSharper restore RedundantAssignment

//                    return destFilter;
//                }
//            }
//            catch
//            {
//            }

//            return destFilter;
//        }

//        public static void KillNotConnectedFilters(IFilterGraph2 filterGraph, bool exceptVisioForge, bool exceptVideoRenderer, bool exceptGMFBridge)
//        {
//            // ReSharper disable InconsistentNaming
//            Guid CLSID_VideoMixingRenderer9 = new Guid("51b4abf3-748f-4e3b-a276-c828330e926a");
//            Guid CLSID_EnhancedVideoRenderer = new Guid("FA10746C-9B63-4b6c-BC49-FC300EA5F256");
//            Guid CLSID_NullRenderer = new Guid("C1F400A4-3F08-11d3-9F0B-006008039E37");
//            Guid CLSID_VideoRenderer = new Guid("70e102b0-5556-11ce-97c0-00aa0055595a");
//            // ReSharper restore InconsistentNaming
            
//            try
//            {
//                VFFilterList filterList = new VFFilterList(filterGraph);

//                for (int i = 0; i < filterList.Count; i++)
//                {
//                    if (!IsFilterConnected(filterList[i], true))
//                    {
//                        if (exceptVisioForge)
//                        {
//                            FilterInfo filterInfo;
//                            filterList[i].QueryFilterInfo(out filterInfo);

//                            if (filterInfo.achName.Contains("VisioForge"))
//                            {
//                                continue;
//                            }
//                        }

//                        if (exceptGMFBridge)
//                        {
//                            FilterInfo filterInfo;
//                            filterList[i].QueryFilterInfo(out filterInfo);

//                            if (filterInfo.achName.Contains("Bridge"))
//                            {
//                                continue;
//                            }
//                        }

//                        if (exceptVideoRenderer)
//                        {
//                            Guid clsid;
//                            filterList[i].GetClassID(out clsid);

//                            if ((clsid == CLSID_VideoRenderer) || 
//                                (clsid == CLSID_NullRenderer) || 
//                                (clsid == CLSID_EnhancedVideoRenderer) || 
//                                (clsid == CLSID_VideoMixingRenderer9))
//                            {
//                                continue;
//                            }
//                        }

//                        filterGraph.RemoveFilter(filterList[i]);

//                        KillNotConnectedFilters(filterGraph, exceptVisioForge, exceptVideoRenderer, exceptGMFBridge);

//                        break;
//                    }
//                }
//            }
//            catch
//            {
//            }
//        }

//        /// <summary>
//        /// Gets bitmap size.
//        /// </summary>
//        /// <param name="width">
//        /// Width.
//        /// </param>
//        /// <param name="height">
//        /// Height.
//        /// </param>
//        /// <param name="bitCount">
//        /// Bit count.
//        /// </param>
//        /// <returns>
//        /// Returns bitmap size.
//        /// </returns>
//        public static int GetBitmapSize(int width, int height, int bitCount)
//        {
//            return width * height * (bitCount / 8);
//        }

//        /// <summary>
//        /// Gets bitmap size.
//        /// </summary>
//        /// <param name="bmi">
//        /// BitmapInfoHeader structure.
//        /// </param>
//        /// <returns>
//        /// Returns bitmap size.
//        /// </returns>
//        public static int GetBitmapSize(BitmapInfoHeader bmi)
//        {
//            if (bmi != null)
//            {
//                return bmi.Width * bmi.Height * (bmi.BitCount / 8);
//            }

//            return 0;
//        }

//        public static bool ReadMediaInfoDeep(string filename, out int width, out int height)
//        {
//            IFilterGraph2 graphBuilder = null;

//            width = 0;
//            height = 0;

//            try
//            {
//                try
//                {
//                    graphBuilder = (IFilterGraph2)new FilterGraph();

//                    graphBuilder.RenderFile(filename, null);

//                    IBasicVideo basicVideo = graphBuilder as IBasicVideo;

//                    if (basicVideo != null)
//                    {
//                        basicVideo.get_VideoWidth(out width);
//                        basicVideo.get_VideoHeight(out height);

//                        Marshal.ReleaseComObject(basicVideo);
//// ReSharper disable RedundantAssignment
//                        basicVideo = null;
//// ReSharper restore RedundantAssignment

//                        return true;
//                    }
//                }
//                finally
//                {
//                    if (graphBuilder != null)
//                    {
//                        Marshal.ReleaseComObject(graphBuilder);
//                    }

//                    // ReSharper disable RedundantAssignment
//                    graphBuilder = null;
//// ReSharper restore RedundantAssignment
//                }
//            }
//            catch
//            {
//            }

//            return false;
//        }
//    }

    /// <summary>
    /// WMV 8 profiles.
    /// </summary>
    public static class WMV8Profiles
    {
        // ReSharper disable InconsistentNaming
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_255VideoPDA = "FEEDBCDF-3FAC-4c93-AC0D-47941EC72C0B";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_150VideoPDA = "AEE16DFA-2C14-4a2f-AD3F-A3034031784F";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_28856VideoMBR = "D66920C4-C21F-4ec8-A0B4-95CF2BD57FC4";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_100768VideoMBR = "5BDB5A0E-979E-47d3-9596-73B386392A55";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_288100VideoMBR = "D8722C69-2419-4b36-B4E0-6E17B60564E5";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_288Video = "3DF678D9-1352-4186-BBF8-74F0C19B6AE2";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_56Video = "254E8A96-2612-405c-8039-F0BF725CED7D";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_100Video = "A2E300B4-C2D4-4fc0-B5DD-ECBD948DC0DF";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_256Video = "BBC75500-33D2-4466-B86B-122B201CC9AE";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_384Video = "29B00C2B-09A9-48bd-AD09-CDAE117D1DA7";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_768Video = "74D01102-E71A-4820-8F0D-13D2EC1E4872";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_700NTSCVideo = "C8C2985F-E5D9-4538-9E23-9B21BF78F745";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_1400NTSCVideo = "931D1BEE-617A-4bcd-9905-CCD0786683EE";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_384PALVideo = "9227C692-AE62-4f72-A7EA-736062D0E21E";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_700PALVideo = "EC298949-639B-45e2-96FD-4AB32D5919C2";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_288MonoAudio = "7EA3126D-E1BA-4716-89AF-F65CEE0C0C67";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_288StereoAudio = "7E4CAB5C-35DC-45bb-A7C0-19B28070D0CC";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_32StereoAudio = "60907F9F-B352-47e5-B210-0EF1F47E9F9D";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_48StereoAudio = "5EE06BE5-492B-480a-8A8F-12F373ECF9D4";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_64StereoAudio = "09BB5BC4-3176-457f-8DD6-3CD919123E2D";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_96StereoAudio = "1FC81930-61F2-436f-9D33-349F2A1C0F10";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_128StereoAudio = "407B9450-8BDC-4ee5-88B8-6F527BD941F2";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_288VideoOnly = "8C45B4C7-4AEB-4f78-A5EC-88420B9DADEF";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_56VideoOnly = "6E2A6955-81DF-4943-BA50-68A986A708F6";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_FAIRVBRVideo = "3510A862-5850-4886-835F-D78EC6A64042";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_HIGHVBRVideo = "0F10D9D3-3B04-4fb0-A3D3-88D4AC854ACC";

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string WMProfile_V80_BESTVBRVideo = "048439BA-309C-440e-9CB4-3DCCA3756423";

        // ReSharper restore InconsistentNaming
    }
}