using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisioForge.DirectShowLib
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Error codes.
    /// </summary>
    public static class DSErrorCodes
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

        public const int E_OUT_OF_MEMORY = unchecked((int)0x8007000E);

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
        public const int E_UNEXPECTED = unchecked((int)0x8000FFFF);

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

}
