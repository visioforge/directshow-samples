unit VCFiltersAPI;

// --------------------------- \\
// <<<DO NOT SORT THIS UNIT>>> \\
// --------------------------- \\

interface

uses
  classes,
  windows,
  messages,
  dialogs,
  ActiveX,
  Contnrs,
  sysutils,
  VFVCDirectShow9,
  VFVCDirect3D9,
  VFVCDirectSound,
  VideoCaptureTypes,
  Graphics;
{$MINENUMSIZE 4}
{$IFNDEF conditionalexpressions}
{$DEFINE norecprocs} // Delphi 5 or less
{$ENDIF}
{$IFDEF conditionalexpressions} // Delphi 6+
{$IF compilerversion < 18} // Delphi 2005 or less
{$DEFINE norecprocs}
{$IFEND}
{$ENDIF}

const
  // WAV Dest
  CLSID_VFWavDest: TGUID = '{16EF2357-E074-436d-A37A-20BBE06A5D93}';

  // Matroska Muxer
  CLSID_MatroskaMuxer: TGUID = '{1E1299A2-9D42-4F12-8791-D79E376F4143}';

  // LAME
  CLSID_LAMEDShowFilter: TGUID = '{B8D27088-FF5F-4B7C-98DC-0E91A1696286}';
  IID_IAudioEncoderProperties: TGUID = '{ca7e9ef0-1cbe-11d3-8d29-00a0c94bbfee}';

  //
  // Configuring MPEG audio encoder parameters with unspecified
  // input stream type may lead to misbehaviour and confusing
  // results. In most cases the specified parameters will be
  // overridden by defaults for the input media type.
  // To archive proper results use this interface on the
  // audio encoder filter with input pin connected to the valid
  // source.
  //
type
  IAudioEncoderProperties = interface(IUnknown)
    ['{ca7e9ef0-1cbe-11d3-8d29-00a0c94bbfee}']
    // Is PES output enabled? Return TRUE or FALSE
    function get_PESOutputEnabled(out dwEnabled: DWORD): HRESULT; stdcall;
    // Enable/disable PES output
    function set_PESOutputEnabled(dwEnabled: DWORD): HRESULT; stdcall;
    // Get target compression bitrate in Kbits/s
    function get_Bitrate(out dwBitrate: DWORD): HRESULT; stdcall;
    // Set target compression bitrate in Kbits/s
    // Not all numbers available! See spec for details!
    function set_Bitrate(dwBitrate: DWORD): HRESULT; stdcall;
    // Get variable bitrate flag
    function get_Variable(out dwVariable: DWORD): HRESULT; stdcall;
    // Set variable bitrate flag
    function set_Variable(dwVariable: DWORD): HRESULT; stdcall;
    // Get variable bitrate in Kbits/s
    function get_VariableMin(out dwmin: DWORD): HRESULT; stdcall;
    // Set variable bitrate in Kbits/s
    // Not all numbers available! See spec for details!
    function set_VariableMin(dwmin: DWORD): HRESULT; stdcall;
    // Get variable bitrate in Kbits/s
    function get_VariableMax(out dwmax: DWORD): HRESULT; stdcall;
    // Set variable bitrate in Kbits/s
    // Not all numbers available! See spec for details!
    function set_VariableMax(dwmax: DWORD): HRESULT; stdcall;
    // Get compression quality
    function get_Quality(out dwQuality: DWORD): HRESULT; stdcall;
    // Set compression quality
    // Not all numbers available! See spec for details!
    function set_Quality(dwQuality: DWORD): HRESULT; stdcall;
    // Get VBR quality
    function get_VariableQ(out dwVBRq: DWORD): HRESULT; stdcall;
    // Set VBR quality
    // Not all numbers available! See spec for details!
    function set_VariableQ(dwVBRq: DWORD): HRESULT; stdcall;
    // Get source sample rate. Return E_FAIL if input pin
    // in not connected.
    function get_SourceSampleRate(out dwSampleRate: DWORD): HRESULT; stdcall;
    // Get source number of channels. Return E_FAIL if
    // input pin is not connected.
    function get_SourceChannels(out dwChannels: DWORD): HRESULT; stdcall;
    // Get sample rate for compressed audio bitstream
    function get_SampleRate(out dwSampleRate: DWORD): HRESULT; stdcall;
    // Set sample rate. See genaudio spec for details
    function set_SampleRate(dwSampleRate: DWORD): HRESULT; stdcall;
    // Get channel mode. See genaudio.h for details
    function get_ChannelMode(out dwChannelMode: DWORD): HRESULT; stdcall;
    // Set channel mode
    function set_ChannelMode(dwChannelMode: DWORD): HRESULT; stdcall;
    // Is CRC enabled?
    function get_CRCFlag(out dwFlag: DWORD): HRESULT; stdcall;
    // Enable/disable CRC
    function set_CRCFlag(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'original' flag
    function get_OriginalFlag(out dwFlag: DWORD): HRESULT; stdcall;
    function set_OriginalFlag(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'copyright' flag
    function get_CopyrightFlag(out dwFlag: DWORD): HRESULT; stdcall;
    function set_CopyrightFlag(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'Enforce VBR Minimum bitrate' flag
    function get_EnforceVBRmin(out dwFlag: DWORD): HRESULT; stdcall;
    function set_EnforceVBRmin(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'Voice' flag
    function get_VoiceMode(out dwFlag: DWORD): HRESULT; stdcall;
    function set_VoiceMode(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'Keep All Frequencies' flag
    function get_KeepAllFreq(out dwFlag: DWORD): HRESULT; stdcall;
    function set_KeepAllFreq(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'Strict ISO complience' flag
    function get_StrictISO(out dwFlag: DWORD): HRESULT; stdcall;
    function set_StrictISO(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'Disable short block' flag
    function get_NoShortBlock(out dwDisable: DWORD): HRESULT; stdcall;
    function set_NoShortBlock(dwDisable: DWORD): HRESULT; stdcall;
    // Control 'Xing VBR Tag' flag
    function get_XingTag(out dwXingTag: DWORD): HRESULT; stdcall;
    function set_XingTag(dwXingTag: DWORD): HRESULT; stdcall;
    // Control 'Forced mid/ side stereo' flag
    function get_ForceMS(out dwFlag: DWORD): HRESULT; stdcall;
    function set_ForceMS(dwFlag: DWORD): HRESULT; stdcall;
    // Control 'ModeFixed' flag
    function get_ModeFixed(out dwFlag: DWORD): HRESULT; stdcall;
    function set_ModeFixed(dwFlag: DWORD): HRESULT; stdcall;
    // Receive the block of encoder
    // configuration parametres
    function get_ParameterBlockSize(out pcBlock: Byte; out pdwSize: DWORD)
      : HRESULT; stdcall;
    // Set encoder configuration parametres
    function set_ParameterBlockSize(pcBlock: PByte; dwSize: DWORD)
      : HRESULT; stdcall;
    // Set default audio encoder parameters depending
    // on current input stream type
    function DefaultAudioEncoderProperties: HRESULT; stdcall;
    // By default the modified properties are not saved to
    // registry immediately, so the filter needs to be
    // forced to do this. Omitting this steps may lead to
    // misbehavior and confusing results.
    function LoadAudioEncoderPropertiesFromRegistry: HRESULT; stdcall;
    function SaveAudioEncoderPropertiesToRegistry: HRESULT; stdcall;
    // Determine, whether the filter can be configured. If this
    // functions returs E_FAIL, input format hasn't been
    // specified and filter behavior unpredicated. If S_OK,
    // the filter could be configured with correct values.
    function InputTypeDefined: HRESULT; stdcall;
  end;

  // EFFECTS FILTER
const
  // 6.0
  CLSID_VFVideoEffectsPro: TGUID = '{980B1181-1619-44F9-AEBE-F2D7E5CE1EFE}';
  IID_IVFEffects_45: TGUID = '{5E767DA8-97AF-4607-B95F-8CC6010B84CA}';
  IID_IVFEffects_Pro: TGUID = '{9A794ABE-98AD-45AF-BBB0-042172C74C79}';

type
  CVFTextLogo = record
    x: integer;
    y: integer;
    transparent_bg: longbool;
    font_size: integer;
    font_italic: longbool;
    font_bold: longbool;
    font_underline: longbool;
    font_strikeout: longbool;
    font_color: COLORREF;
    bg_color: COLORREF;

    rightToLeft: longbool;
    vertical: longbool;

    align: DWORD;
    drawQuality: DWORD;
    antialiasing: DWORD;

    rectWidth: integer;
    rectHeight: integer;

    rotationMode: DWORD;
    flipMode: DWORD;

    transp: DWORD;

    gradient: longbool;
    gradientMode: DWORD;
    gradientColor1: COLORREF;
    gradientColor2: COLORREF;

    borderMode: DWORD;
    innerBorderColor: COLORREF;
    outerBorderColor: COLORREF;
    innerBorderSize: integer;
    outerBorderSize: integer;

    bgShape: longbool;
    bgShapeType: DWORD;
    bgShapeX: integer;
    bgShapeY: integer;
    bgShapeWidth: integer;
    bgShapeHeight: integer;
    bgShapeColor: COLORREF;

    text: WideString;
    font_name: WideString;
    dateMode: longbool;
    dateMask: WideString;
  end;

  CVFGraphicLogo = record
    x: cardinal;
    y: cardinal;
    StretchMode: DWORD;
    hBmp: integer;
    TranspLevel: integer;
    ColorKey: COLORREF;
    UseColorKey: longbool;
    Filename: WideString;
  end;

  CVFDenoiseCAST = record
    TemporalDifferenceThreshold: integer; // default 16 - range [0, 255]
    NumberOfMotionPixelsThreshold: integer; // default 0 - range [0, 16]
    StrongEdgeThreshold: integer; // default 8 - range [0, 255]
    BlockWidth: integer; // default 4 - range [1, 16]
    BlockHeight: integer; // default 4 - range [1, 16]
    EdgePixelWeight: integer; // default 128 - range [0, 255]
    NonEdgePixelWeight: integer; // default 16 - range [0, 255]
    GaussianThresholdY: integer; // default 12
    GaussianThresholdUV: integer; // default 6
    HistoryWeight: integer; // default 192 - range [0, 255]
  end;

  CVFDeintBlend = record
    blendThresh1: integer; // default 5 - range [0, 255]
    blendThresh2: integer; // default 9 - range [0, 255]
    blendConstants1: double; // default 0.3 - range [0, 1]
    blendConstants2: double; // default 0.7 - range [0, 1]
  end;

  CVFZoom = record
    zoomX: double;
    zoomY: double;
    shiftX: integer;
    shiftY: integer;
  end;

  CVFPan = record
    startX: integer;
    startY: integer;
    startWidth: integer;
    startHeight: integer;

    stopX: integer;
    stopY: integer;
    stopWidth: integer;
    stopHeight: integer;
  end;

  CVFEffect = record
    _type: integer;
    ID: integer;
    StartTime: int64;
    StopTime: int64;

    Enabled: longbool;

    pAmountI: integer;
    pMinI: integer;
    pMaxI: integer;
    pAmountD: double;
    pScaleD: double;
    pTurbulenceI: integer;
    pSizeI: integer;
    pSeamI: integer;
    pFactorI: integer;
    pInferenceI: integer;
    pStyleI: integer;

    pDenoiseSNRThreshold: integer; // default 20, [0, 255]
    pDeintTriangleWeight: integer; // default 180, [128, 256]
    pDeintCAVTThreshold: integer; // default 20, [0, 255]
    pDenoiseAdaptiveThreshold: integer; // default 20, [0, 255]
    pDenoiseAdaptiveBlurMode: integer; // default - 0, [0, 3]

    TextLogo: CVFTextLogo;
    GraphicLogo: CVFGraphicLogo;
    DenoiseCAST: CVFDenoiseCAST;
    DeintBlend: CVFDeintBlend;

    Zoom: CVFZoom;
    Pan: CVFPan;
  end;

  IVFEffects45 = interface(IUnknown)
    procedure add_effect(pEffect: CVFEffect); stdcall;
    procedure set_effect_settings(pEffect: CVFEffect); stdcall;
    procedure remove_effect(ID: integer); stdcall;
    procedure clear_effects(); stdcall;
  end;

  CVFEffect_Main = class(TObject)
    eff: CVFEffect;
  end;

  // public delegate int BufferCBProc([In] IntPtr handle, [In] IntPtr pBuffer, int BufferLen, int width, int height, Int64 StartTime, Int64 StopTime, [MarshalAs(UnmanagedType.Bool)] ref bool updateFrame);

  IVFEffectsPro = interface(IUnknown)
    procedure set_enabled(effects, motdet, chroma, sg: longbool); stdcall;
    function set_sg_callback_24(Callback: pointer): HRESULT; stdcall;
    function set_sg_callback_32(Callback: pointer): HRESULT; stdcall;
    // function set_settings(allow_vih2_input, force_vih1_output, force_yuy2_output: LongBool): HRESULT; stdcall;
    function put_sg_app_handle(apphandle: pointer): HRESULT; stdcall;
    function put_sg_app_handle_id(apphandleid: DWORD): HRESULT; stdcall;
    function set_sg_callback_24_flip(flip: longbool): HRESULT; stdcall;
  end;

  // Screen capture
const
  // IID_IVFScreenCapture: TGUID = '{259E0009-9963-4a71-91AE-34B96D75486F}';
  // IID_IVFScreenCapture2: TGUID = '{BC91012D-22E0-4091-8C0A-3913BDAB8A42}';
  CLSID_VFScreenCapture: TGUID = '{B74136FB-1F94-4180-8695-C9307810D944}';
  IID_IVFScreenCapture3: TGUID = '{259E0009-9963-4a71-91AE-34B96D754899}';

type
  VFRect = record
    left, top, right, bottom: cardinal;
  end;

  // IVFScreenCapture = interface(IUnknown)
  // function set_fps(fps: double): HRESULT; stdcall;
  // function set_rect(rect: VFRect): HRESULT; stdcall;
  // function set_mouse(draw: boolean): HRESULT; stdcall;
  // function set_display_index(index: integer): HRESULT; stdcall;
  // end;
  //
  // IVFScreenCapture2 = interface(IUnknown)
  // function set_mode(picture_mode: integer): HRESULT; stdcall;
  // // function get_DC(var DC: integer): HRESULT; stdcall;
  // function refresh_pic(): HRESULT; stdcall;
  // function SetStream(Stream: IStream; Length: int64): HRESULT; stdcall;
  // end;

  IVFScreenCapture3 = interface(IUnknown)
    function init(): HRESULT; stdcall;
    function set_fps(fps: double): HRESULT; stdcall;
    function set_rect(rect: VFRect): HRESULT; stdcall;
    function set_mouse(draw: boolean): HRESULT; stdcall;
    function set_display_index(index: integer): HRESULT; stdcall;
    function set_mode(mode: TVFScreenCaptureMode): HRESULT; stdcall;
    function refresh_pic(): HRESULT; stdcall;
    function set_stream(Stream: IStream; Length: int64): HRESULT; stdcall;
    function set_window_handle(handle: HWND): HRESULT; stdcall;
    function get_window_size(handle: HWND; out width: integer;
      out height: integer): HRESULT; stdcall;
  end;

  // Resize filter
const
  IID_IVFResize: TGUID = '{12BC6F20-2812-4660-8684-10F3FD3B4487}';
  CLSID_VFResizer_4: TGUID = '{2E0E7313-71DC-4455-ADB4-F80718B7B727}';

type
  IVFResize = interface(IUnknown)
    function put_Resolution(x, y: cardinal): HRESULT; stdcall;
    function put_ResizeMode(mode: TVFResizeMode; letterbox: boolean)
      : HRESULT; stdcall;
    function put_Crop(left, top, right, bottom: cardinal): HRESULT; stdcall;
    function put_FilterMode(mode: TVFResizeFilterMode): HRESULT; stdcall;
    function put_RotateMode(mode: TVFRotateMode): HRESULT; stdcall;
  end;

  // MPEG-2 PSI parser
const
  IID_IMpeg2PsiParser: TGUID = '{AE1A2884-540E-4077-B1AB-67A34A72298C}';
  CLSID_VFPSIParser: TGUID = '{DDF7480E-13E2-4481-BA2B-3C17C4FC469F}';

type
  IMpeg2PsiParser = interface(IUnknown)
    function GetTransportStreamId(out pStreamId: WORD): HRESULT; stdcall;
    function GetPatVersionNumber(out pPatVersion: Byte): HRESULT; stdcall;
    function GetCountOfPrograms(out pNumOfPrograms: integer): HRESULT; stdcall;
    function GetRecordProgramNumber(dwIndex: DWORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function GetRecordProgramMapPid(dwIndex: DWORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function FindRecordProgramMapPid(wProgramNumber: WORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function GetPmtVersionNumber(wProgramNumber: WORD; out pPmtVersion: Byte)
      : HRESULT; stdcall;
    function GetCountOfElementaryStreams(wProgramNumber: WORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function GetRecordStreamType(wProgramNumber: WORD; dwRecordIndex: DWORD;
      out pbVal: Byte): HRESULT; stdcall;
    function GetRecordElementaryPid(wProgramNumber: WORD; dwRecordIndex: DWORD;
      out pwVal: WORD): HRESULT; stdcall;
  end;

const
  CLSID_VFDump_4: TGUID = '{83DF94EE-5A0A-4730-9818-9726CE117CEC}';

  // Motion detection
  IID_IVFMotDetConfig: TGUID = '{A77713DE-E16F-4f64-AFE4-27F536B3F4EC}';

  // type
  // IVFMotDetCallback = interface(IUnknown)
  // function MatrixCB(pBuffer: PByte; BufferLen: Integer; similarity: integer): HRESULT; stdcall;
  // end;

  // type
  // SAMPLECALLBACK = function(smt: integer): HRESULT of object;

type
  IVFMotDetConfig = interface(IUnknown)
    // function CBFunction( nValue: cardinal): HRESULT; stdcall;
    // function SetCallBack(pCallbackInterface: IVFMotDetCallback): HRESULT; stdcall;
    function motdet_set_callback(Callback: pointer): HRESULT; stdcall;
    // function put_enabled(enabled_: boolean): HRESULT; stdcall;
    // function get_enabled(var enabled_: boolean): HRESULT; stdcall;
    function put_CHL_enabled(enabled_: boolean): HRESULT; stdcall;
    // function get_CHL_enabled(var enabled_: boolean): HRESULT; stdcall;
    function put_CHL_color(color_: integer): HRESULT; stdcall;
    // function get_CHL_color(var color_: integer): HRESULT; stdcall;
    function put_CHL_threshold(threshold_: integer): HRESULT; stdcall;
    // function get_CHL_threshold(var threshold_: integer): HRESULT; stdcall;
    function put_lines_x(value_: integer): HRESULT; stdcall;
    // function get_lines_x(var value_: integer): HRESULT; stdcall;
    function put_lines_y(value_: integer): HRESULT; stdcall;
    // function get_lines_y(var value_: integer): HRESULT; stdcall;
    function put_drop_frames_enabled(enabled_: boolean): HRESULT; stdcall;
    // function get_drop_frames_enabled(var enabled_: boolean): HRESULT; stdcall;
    function put_drop_frames_threshold(value_: integer): HRESULT; stdcall;
    // function get_drop_frames_threshold(var value_: integer): HRESULT; stdcall;
    function put_frame_interval(value_: integer): HRESULT; stdcall;
    // function get_frame_interval(var value_: integer): HRESULT; stdcall;
    function put_compare_mode(red, green, blue, greyscale: boolean)
      : HRESULT; stdcall;
    // function put_sens_matrix(value_: PBYTE): HRESULT; stdcall;
    // function ClearSensMatrix(): HRESULT; stdcall;
    function put_app_handle(handle_: pointer): HRESULT; stdcall;
  end;

  // Audio analyzer
  // const
  // IID_IVFAnalyzer: TGUID = '{BA873A67-681B-4911-BF88-7B175AF14F70}';
  // CLSID_VFAnalyzer_4: TGUID = '{8583FA71-A09B-47da-81D3-C0CF2F3678CB}';

type
  PeakMeterData = record
    value_: integer;
    falloff: integer;
    peak: integer;
  end;

type
  PeakMeterDataEx = record
    value: integer;
    falloff: integer;
    Speed: integer;
  end;

type
  PPeakMeterDataArray = ^TPeakMeterDataArray;
  TPeakMeterDataArray = array [0 .. 22] of PeakMeterData;

  PPeakMeterDataArrayEx = ^TPeakMeterDataArrayEx;
  TPeakMeterDataArrayEx = array [0 .. 22] of PeakMeterDataEx;

  PPeakMeterIntsArray = ^TPeakMeterIntsArray;
  TPeakMeterIntsArray = array [0 .. 22] of integer;

  IVFAnalyzer = interface(IUnknown)
    function SetCallback(Callback: pointer): HRESULT; stdcall;
    function put_app_handle(handle_: pointer): HRESULT; stdcall;
    function put_vu_meter_enabled(Enabled: boolean): HRESULT; stdcall;
    function put_booster_enabled(Enabled: boolean): HRESULT; stdcall;
    function put_booster_level(level: double): HRESULT; stdcall;
  end;

const
  CLSID_VFFFMPEGSource: TGUID = '{F15FF9D9-F69A-43E6-92F7-13268D10F938}';
  CLSID_WMAsfReader: TGUID = '{187463A0-5BB7-11D3-ACBE-0080C75E246E}';

type
  /// <summary>
  /// Camera protocol.
  /// </summary>
  VFIPHTTPCameraProtocol = (

    /// <summary>
    /// Unknown.
    /// </summary>
    ProtocolUnknown,

    /// <summary>
    /// HTTP sunc.
    /// </summary>
    HTTPSync,

    /// <summary>
    /// HTTP async.
    /// </summary>
    HTTPAsync,

    /// <summary>
    /// RTSP via TCP.
    /// </summary>
    RTSPTCP,

    /// <summary>
    /// RTSP via UDP.
    /// </summary>
    RTSPUDP,

    /// <summary>
    /// RTSP via HTTP.
    /// </summary>
    RTSPHTTP,

    /// <summary>
    /// MMS / WMV stream from any device.
    /// </summary>
    MMS_WMV);

  /// <summary>
  /// IP HTTP FFMPEG filter params.
  /// </summary>
  VFIPHTTPFFMPEGFilterParams = record
    /// <summary>
    /// Log enabled.
    /// </summary>
    LogToFile: boolean;

    /// <summary>
    /// Log file name.
    /// </summary>
    LogFilename: WideString;

    /// <summary>
    /// URL.
    /// </summary>
    URL: WideString;

    /// <summary>
    /// Protocol.
    /// </summary>
    Protocol: VFIPHTTPCameraProtocol;

    /// <summary>
    /// Username.
    /// </summary>
    Username: WideString;

    /// <summary>
    /// Password.
    /// </summary>
    Password: WideString;

    /// <summary>
    /// Buffered frames count.
    /// </summary>
    BufferFrames: integer;

    /// <summary>
    /// Video width.
    /// </summary>
    VideoWidth: integer;

    /// <summary>
    /// Video height.
    /// </summary>
    VideoHeight: integer;

    LogCallback: pointer;
  end;

  /// <summary>
  /// IP HTTP Source filter interface.
  /// </summary>
  IVFIPHTTPSource = interface(IUnknown)
    /// <summary>
    /// Sets filter parameters.
    /// </summary>
    /// <param name="FilterParams">
    /// Filter parameters.
    /// </param>
    /// <returns>
    /// Returns HRESULT.
    /// </returns>
    function SetParams(FilterParams: VFIPHTTPFFMPEGFilterParams)
      : HRESULT; stdcall;
  end;

  /// <summary>
  /// IP HTTP FFMPEG Source filter interface.
  /// </summary>
  IVFIPHTTPFFMPEGSource = interface(IUnknown)
    /// <summary>
    /// Sets filter parameters.
    /// </summary>
    /// <param name="FilterParams">
    /// Filter parameters.
    /// </param>
    /// <returns>
    /// Returns HRESULT.
    /// </returns>
    function SetParams(FilterParams: VFIPHTTPFFMPEGFilterParams)
      : HRESULT; stdcall;
  end;

const
  IID_IVFRegisterInt: TGUID = '{1F9EC154-8D0D-457F-A640-7546FC553F42}';
  IID_IVFRegister: TGUID = '{59E82754-B531-4A8E-A94D-57C75F01DA30}';
  IID_IVFPushConfig: TGUID = '{260E28D7-48E6-4ABD-A14A-DD0BBD0AA9F5}';

type
  /// <summary>
  /// Internal filter registration interface.
  /// </summary>
  IVFRegisterInt = interface(IUnknown)
    /// <summary>
    /// Sets registered.
    /// </summary>
    procedure SetRegistered(); stdcall;
  end;

  /// <summary>
  /// Public filter registration interface.
  /// </summary>
  IVFRegister = interface(IUnknown)
    /// <summary>
    /// Sets registered.
    /// </summary>
    /// <param name="licenseKey">
    /// License Key.
    /// </param>
    procedure SetLicenseKey(licenseKey: PWideChar); stdcall;
  end;

  /// <summary>
  /// Push config interface.
  /// </summary>
  IVFPushConfig = interface(IUnknown)

    function HasVideoStream(): BOOL; stdcall;

    function HasAudioStream(): BOOL; stdcall;

    function GetVideoStreamWidth(): integer; stdcall;

    function GetVideoStreamHeight(): integer; stdcall;

    procedure SetVideoFFMPEGFilters(value: PWideChar); stdcall;

    procedure SetAudioFFMPEGFilters(value: PWideChar); stdcall;

    procedure SetThreadsCount(value: integer); stdcall;

    procedure SetInputFFMPEGFormat(value: PWideChar); stdcall;

    procedure SetBufferFramesCount(value: integer); stdcall;

    procedure SetAudioStream(value: integer); stdcall;

    procedure SetSubtitleStream(value: integer); stdcall;

    procedure SetVideoFrameRate(frameRate: double); stdcall;
  end;

  /// <summary>
  /// Encryption filter interface.
  /// </summary>
  IVFCryptoConfig = interface(IUnknown)
    ['{BAA5BD1E-3B30-425e-AB3B-CC20764AC253}']
    function put_Provider(passwordProviderNotUsed: TObject): HRESULT; stdcall;
    function get_Provider(out passwordProviderNotUsed: TObject)
      : HRESULT; stdcall;
    function put_Password(buffer: PWideChar; size: integer): HRESULT; stdcall;
    function HavePassword(): HRESULT; stdcall;
  end;

const
  IID_IH264Encoder: TGUID = '{09FA2EA3-4773-41a8-90DC-9499D4061E9F}';

type
  IH264Encoder = interface(IUnknown)
    ['{09FA2EA3-4773-41a8-90DC-9499D4061E9F}']
    function get_Bitrate(out plValue: integer): HRESULT; stdcall;

    function put_Bitrate(lValue: integer): HRESULT; stdcall;

    function get_RateControl(out pValue: TVFH264RateControl): HRESULT; stdcall;

    function put_RateControl(value: TVFH264RateControl): HRESULT; stdcall;

    function get_MbEncoding(out pValue: TVFH264MBEncoding): HRESULT; stdcall;

    function put_MbEncoding(value: TVFH264MBEncoding): HRESULT; stdcall;

    function get_GOP(out pValue: BOOL): HRESULT; stdcall;

    function put_GOP(value: BOOL): HRESULT; stdcall;

    function get_AutoBitrate(out pValue: BOOL): HRESULT; stdcall;

    function put_AutoBitrate(value: BOOL): HRESULT; stdcall;

    function get_Profile(out pValue: TVFH264Profile): HRESULT; stdcall;

    function put_Profile(value: TVFH264Profile): HRESULT; stdcall;

    function get_Level(out pValue: TVFH264Level): HRESULT; stdcall;

    function put_Level(value: TVFH264Level): HRESULT; stdcall;

    function get_Usage(out pValue: TVFH264TargetUsage): HRESULT; stdcall;

    function put_Usage(value: TVFH264TargetUsage): HRESULT; stdcall;

    function get_SequentialTiming(out pValue: TVFH264TimeType)
      : HRESULT; stdcall;

    function put_SequentialTiming(value: TVFH264TimeType): HRESULT; stdcall;

    function get_SliceIntervals(out piIDR: integer; out piP: integer)
      : HRESULT; stdcall;

    function put_SliceIntervals(var piIDR: integer; var piP: integer)
      : HRESULT; stdcall;
  end;

const
  IID_IMP4MuxerConfig: TGUID = '{99DC9BE5-0AFA-45d4-8370-AB021FB07CF4}';

type
  IMP4MuxerConfig = interface(IUnknown)
    ['{99DC9BE5-0AFA-45d4-8370-AB021FB07CF4}']
    function get_SingleThread(out pValue: BOOL): HRESULT; stdcall;

    function put_SingleThread(value: BOOL): HRESULT; stdcall;

    function get_CorrectTiming(out pValue: BOOL): HRESULT; stdcall;

    function put_CorrectTiming(value: BOOL): HRESULT; stdcall;
  end;

  /// <summary>
  /// AAC info.
  /// </summary>
  VFAACInfo = record
    samplerate: integer;
    channels: integer;
    frame_size: integer;
    frames_done: int64;
  end;

  /// <summary>
  /// AAC config.
  /// </summary>
  VFAACConfig = record
    version: integer;
    object_type: integer;
    output_type: integer;
    bitrate: integer;
  end;

const
  IID_IMonogramAACEncoder: TGUID = '{B2DE30C0-1441-4451-A0CE-A914FD561D7F}';

type
  IMonogramAACEncoder = interface(IUnknown)
    ['{B2DE30C0-1441-4451-A0CE-A914FD561D7F}']
    function GetConfig(var config: VFAACConfig): HRESULT; stdcall;
    function SetConfig(var config: VFAACConfig): HRESULT; stdcall;
  end;

  /// <summary>
  /// Virtual camera sink interface.
  /// </summary>
type
  IVFVirtualCamera = interface(IUnknown)
    ['{A96631D2-4AC9-4F09-9F34-FF8229087DEB}']
    function set_license(license: PWideChar): HRESULT; stdcall;
    function set_instance(instance: WideChar): HRESULT; stdcall;
  end;

  // Windows Media DSP
type
{$EXTERNALSYM _tagpropertykey}
  _tagpropertykey = packed record
    fmtid: TGUID;
    pid: DWORD;
  end;
{$EXTERNALSYM PROPERTYKEY}

  PROPERTYKEY = _tagpropertykey;
  PPropertyKey = ^TPropertyKey;
  TPropertyKey = _tagpropertykey;

const
  PID_FIRST_USABLE = $02;

  IID_IPropertyStore: TGUID = '{886d8eeb-8cf2-4446-8d02-cdba1dbdcf99}';

  MFPKEY_WMAAECMA_SYSTEM_MODE
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 0);
  MFPKEY_WMAAECMA_DMO_SOURCE_MODE
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 1);
  MFPKEY_WMAAECMA_DEVICE_INDEXES
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 2);
  MFPKEY_WMAAECMA_FEATURE_MODE
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 3);
  MFPKEY_WMAAECMA_FEATR_FRAME_SIZE
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 4);
  MFPKEY_WMAAECMA_FEATR_ECHO_LENGTH
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 5);
  MFPKEY_WMAAECMA_FEATR_NS: TPropertyKey =
    (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 6);
  MFPKEY_WMAAECMA_FEATR_AGC
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 7);
  MFPKEY_WMAAECMA_FEATR_AES
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 8);
  MFPKEY_WMAAECMA_FEATR_VAD
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 9);
  MFPKEY_WMAAECMA_FEATR_CENTER_CLIP
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 10);
  MFPKEY_WMAAECMA_FEATR_NOISE_FILL
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 11);
  MFPKEY_WMAAECMA_RETRIEVE_TS_STATS
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 12);
  MFPKEY_WMAAECMA_QUALITY_METRICS
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 13);
  MFPKEY_WMAAECMA_MICARRAY_DESCPTR
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 14);
  MFPKEY_WMAAECMA_DEVICEPAIR_GUID
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 15);
  MFPKEY_WMAAECMA_FEATR_MICARR_MODE
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 16);
  MFPKEY_WMAAECMA_FEATR_MICARR_BEAM
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 17);
  MFPKEY_WMAAECMA_FEATR_MICARR_PREPROC
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 18);
  MFPKEY_WMAAECMA_MIC_GAIN_BOUNDER
    : TPropertyKey = (fmtid: '{6f52c567-0360-4bd2-9617-ccbf1421c939}';
    pid: PID_FIRST_USABLE + 19);

  MFPKEY_CONV_INPUTFRAMERATE
    : TPropertyKey = (fmtid: '{52f8d29b-2e76-43f7-a4f6-1717904e35df}';
    pid: $01);
  MFPKEY_CONV_OUTPUTFRAMERATE
    : TPropertyKey = (fmtid: '{52f8d29b-2e76-43f7-a4f6-1717904e35df}';
    pid: $02);

  CLSID_CResamplerMediaObject: TGUID = '{f447b69e-1884-4a7e-8055-346f74d6edb3}';
  CLSID_CWMAudioAEC: TGUID = '{745057c7-f353-4f2d-a7ee-58434477730e}';
  CLSID_CFrameRateConvertDmo: TGUID = '{01f36ce2-0907-4d8b-979d-f151be91c883}';
  CLSID_CMpeg4DecMediaObject: TGUID = '{f371728a-6052-4d47-827c-d039335dfe0a}';
  CLSID_CMpeg43DecMediaObject: TGUID = '{cba9e78b-49a3-49ea-93d4-6bcba8c4de07}';
  CLSID_CMpeg4sDecMediaObject: TGUID = '{2a11bae2-fe6e-4249-864b-9e9ed6e8dbc2}';
  CLSID_CMpeg4EncMediaObject: TGUID = '{24f258d8-c651-4042-93e4-ca654abb682c}';
  CLSID_CMpeg4sEncMediaObject: TGUID = '{6ec5a7be-d81e-4f9e-ada3-cd1bf262b6d8}';
  CLSID_CMSSCDecMediaObject: TGUID = '{7bafb3b1-d8f4-4279-9253-27da423108de}';
  CLSID_CMSSCEncMediaObject: TGUID = '{8cb9cc06-d139-4ae6-8bb4-41e612e141d5}';
  CLSID_CMSSCEncMediaObject2: TGUID = '{f7ffe0a0-a4f5-44b5-949e-15ed2bc66f9d}';
  CLSID_CWMADecMediaObject: TGUID = '{2eeb4adf-4578-4d10-bca7-bb955f56320a}';
  CLSID_CWMAEncMediaObject: TGUID = '{70f598e9-f4ab-495a-99e2-a7c4d3d89abf}';
  CLSID_CWMSPDecMediaObject: TGUID = '{874131cb-4ecc-443b-8948-746b89595d20}';
  CLSID_CWMSPEncMediaObject: TGUID = '{67841b03-c689-4188-ad3f-4c9ebeec710b}';
  CLSID_CWMSPEncMediaObject2: TGUID = '{1f1f4e1a-2252-4063-84bb-eee75f8856d5}';
  CLSID_CWMVDecMediaObject: TGUID = '{82d353df-90bd-4382-8bc2-3f6192b76e34}';
  CLSID_CWMVEncMediaObject2: TGUID = '{96b57cdd-8966-410c-bb1f-c97eea765c04}';
  CLSID_CWMVXEncMediaObject: TGUID = '{7e320092-596a-41b2-bbeb-175d10504eb6}';
  CLSID_CWMV9EncMediaObject: TGUID = '{d23b90d0-144f-46bd-841d-59e4eb19dc59}';
  CLSID_CWVC1DecMediaObject: TGUID = '{c9bfbccf-e60e-4588-a3df-5a03b1fd9585}';
  CLSID_CWVC1EncMediaObject: TGUID = '{44653D0D-8CCA-41e7-BACA-884337B747AC}';
  CLSID_CDeColorConvMediaObject
    : TGUID = '{49034c05-f43c-400f-84c1-90a683195a3a}';
  CLSID_CDVDecoderMediaObject: TGUID = '{e54709c5-1e17-4c8d-94e7-478940433584}';
  CLSID_CDVEncoderMediaObject: TGUID = '{c82ae729-c327-4cce-914d-8171fefebefb}';
  CLSID_CMpeg2DecMediaObject: TGUID = '{863d66cd-cdce-4617-b47f-c8929cfc28a6}';
  CLSID_CPK_DS_MPEG2Decoder: TGUID = '{9910c5cd-95c9-4e06-865a-efa1c8016bf4}';
  CLSID_CAC3DecMediaObject: TGUID = '{03d7c802-ecfa-47d9-b268-5fb3e310dee4}';
  CLSID_CPK_DS_AC3Decoder: TGUID = '{6c9c69d6-0ffc-4481-afdb-cdf1c79c6f3e}';
  CLSID_CMP3DecMediaObject: TGUID = '{bbeea841-0a63-4f52-a7ab-a9b3a84ed38a}';
  CLSID_CResizerMediaObject: TGUID = '{d3ec8b8b-7728-4fd8-9fe0-7b67d19f73a3}';
  CLSID_CInterlaceMediaObject: TGUID = '{b5a89c80-4901-407b-9abc-90d9a644bb46}';
  CLSID_CWMAudioLFXAPO: TGUID = '{62dc1a93-ae24-464c-a43e-452f824c4250}';
  CLSID_CWMAudioGFXAPO: TGUID = '{637c490d-eee3-4c0a-973f-371958802da2}';
  CLSID_CWMAudioSpdTxDMO: TGUID = '{5210f8e4-b0bb-47c3-a8d9-7b2282cc79ed}';
  CLSID_CClusterDetectorDmo: TGUID = '{36e820c4-165a-4521-863c-619e1160d4d4}';
  CLSID_CColorControlDmo: TGUID = '{798059f0-89ca-4160-b325-aeb48efe4f9a}';
  CLSID_CColorConvertDMO: TGUID = '{98230571-0087-4204-b020-3282538e57d3}';
  CLSID_CColorLegalizerDmo: TGUID = '{fdfaa753-e48e-4e33-9c74-98a27fc6726a}';
  CLSID_CFrameInterpDMO: TGUID = '{0a7cfe1b-6ab5-4334-9ed8-3f97cb37daa1}';
  CLSID_CResizerDMO: TGUID = '{1ea1ea14-48f4-4054-ad1a-e8aee10ac805}';
  CLSID_CShotDetectorDmo: TGUID = '{56aefacd-110c-4397-9292-b0a0c61b6750}';
  CLSID_CSmpteTransformsDmo: TGUID = '{bde6388b-da25-485d-ba7f-fabc28b20318}';
  CLSID_CThumbnailGeneratorDmo
    : TGUID = '{559c6bad-1ea8-4963-a087-8a6810f9218b}';
  CLSID_CTocGeneratorDmo: TGUID = '{4dda1941-77a0-4fb1-a518-e2185041d70c}';
  CLSID_CMPEGAACDecMediaObject
    : TGUID = '{8DDE1772-EDAD-41c3-B4BE-1F30FB4EE0D6}';
  CLSID_CNokiaAACDecMediaObject
    : TGUID = '{3CB2BDE4-4E29-4c44-A73E-2D7C2C46D6EC}';
  CLSID_CVodafoneAACDecMediaObject
    : TGUID = '{7F36F942-DCF3-4d82-9289-5B1820278F7C}';
  CLSID_CXeonAACCCDecMediaObject
    : TGUID = '{A74E98F2-52D6-4b4e-885B-E0A6CA4F187A}';
  CLSID_CNokiaAACCCDecMediaObject
    : TGUID = '{EABF7A6F-CCBA-4d60-8620-B152CC977263}';
  CLSID_CVodafoneAACCCDecMediaObject
    : TGUID = '{7E76BF7F-C993-4e26-8FAB-470A70C0D59C}';
  CLSID_CMPEG2EncoderDS: TGUID = '{5F5AFF4A-2F7F-4279-88C2-CD88EB39D144}';
  CLSID_CMPEG2EncoderVideoDS: TGUID = '{42150cd9-ca9a-4ea5-9939-30ee037f6e74}';
  CLSID_CMPEG2EncoderAudioDS: TGUID = '{acd453bc-c58a-44d1-bbf5-bfb325be2d78}';
  CLSID_CMPEG2AudDecoderDS: TGUID = '{E1F1A0B8-BEEE-490d-BA7C-066C40B5E2B9}';
  CLSID_CMPEG2VidDecoderDS: TGUID = '{212690FB-83E5-4526-8FD7-74478B7939CD}';
  CLSID_CMSAC3Enc: TGUID = '{C6B400E2-20A7-4e58-A2FE-24619682CE6C}';

  SINGLE_CHANNEL_AEC = 0;
  ADAPTIVE_ARRAY_ONLY = SINGLE_CHANNEL_AEC + 1;
  OPTIBEAM_ARRAY_ONLY = ADAPTIVE_ARRAY_ONLY + 1;
  ADAPTIVE_ARRAY_AND_AEC = OPTIBEAM_ARRAY_ONLY + 1;
  OPTIBEAM_ARRAY_AND_AEC = ADAPTIVE_ARRAY_AND_AEC;
  SINGLE_CHANNEL_NSAGC = OPTIBEAM_ARRAY_AND_AEC + 1;
  MODE_NOT_SET = SINGLE_CHANNEL_NSAGC + 1;

  AEC_VAD_DISABLED = 0;
  AEC_VAD_NORMAL = 1;
  AEC_VAD_FOR_AGC = 2;
  AEC_VAD_FOR_SILENCE_SUPPRESSION = 3;

type
  IPropertyStore = interface
    ['{886d8eeb-8cf2-4446-8d02-cdba1dbdcf99}']
    function GetCount(out cProps: DWORD): HRESULT; stdcall;
    function GetAt(iProp: DWORD; out pkey: TPropertyKey): HRESULT; stdcall;
    function GetValue(const key: TPropertyKey; out pv: TPropVariant)
      : HRESULT; stdcall;
    function SetValue(const key: TPropertyKey; const propvar: TPropVariant)
      : HRESULT; stdcall;
    function Commit: HRESULT; stdcall;
  end;

  TVFVUMeterEventImage = procedure(ID: integer; Frame: TBitmap) of object;
  TVFVUMeterEventImageH = procedure(ID: integer; Frame: HBitmap) of object;
  TVFVUMeterEventImageI = procedure(ID: integer; Frame: integer) of object;
  TVFVUMeterEvent = procedure(ID: integer; values: WideString) of object;

  // // PIP
  // const
  // CLSID_VFFrameJoin: TGUID = '{BD072821-F05F-4569-87F4-82864E2067FC}';
  // IID_IVFFrameJoin: TGUID = '{0C43C9AD-1F2B-47f5-B563-CB6461589C33}';
  //
  // type
  // IVFFrameJoin = interface(IUnknown)
  // function set_mode(value_: integer): HRESULT; stdcall;
  // function set_pipX(value_: DWORD): HRESULT; stdcall;
  // function set_pipY(value_: DWORD): HRESULT; stdcall;
  // function set_pipWidth(value_: DWORD): HRESULT; stdcall;
  // function set_pipHeight(value_: DWORD): HRESULT; stdcall;
  // function set_transp(value_: DWORD): HRESULT; stdcall;
  // function set_flipX(value_: LongBool): HRESULT; stdcall;
  // function set_flipY(value_: LongBool): HRESULT; stdcall;
  // end;

  // Video mixer
type
  VFVideoInputParam = record

    // output position and size
    x: integer;
    y: integer;
    w: integer;
    h: integer;

    // alpha 0(fill) - 255(transparent)
    alpha: integer;

    flipX: longbool;
    flipY: longbool;
  end;

type
  VFVideoOutputParam = record

    w: integer;
    h: integer;

    frame_rate: integer;
    back_color: integer;

    back_image: WideString;
  end;

const
  CLSID_VFVideoMixer: TGUID = '{3F4B9E80-436F-4f97-AD39-4656943278D2}';
  IID_IVFVideoMixer: TGUID = '{3318300E-F6F1-4d81-8BC3-9DB06B09F77A}';

type
  IVFVideoMixer = interface(IUnknown)
    function SetInputParam(pin_index: integer; param: VFVideoInputParam)
      : HRESULT; stdcall;
    function GetInputParam(pin_index: integer; var param: VFVideoInputParam)
      : HRESULT; stdcall;
    function GetInputParam2(pin: IPin; var param: VFVideoInputParam)
      : HRESULT; stdcall;
    function SetOutputParam(param: VFVideoOutputParam): HRESULT; stdcall;
    function GetOutputParam(var param: VFVideoOutputParam): HRESULT; stdcall;
  end;

  // Chroma Key
const
  IID_IVFChromaKey: TGUID = '{AF6E8208-30E3-44f0-AAFE-787A6250BAB3}';

type
  IVFChromaKey = interface(IUnknown)
    function put_contrast(low, high: integer): HRESULT; stdcall;
    function put_color(color: integer): HRESULT; stdcall;
    function put_image(Filename: WideString): HRESULT; stdcall;
  end;

procedure ClearEffect(var eff: CVFEffect);

// =============================================================================
// Description:
//
// Service GUID used by IMFGetService::GetService to retrieve interfaces from
// the renderer or the presenter.
//
const
  SID_IMFVideoProcessor = '{6AB0000C-FECE-4d1f-A2AC-A9573530656E}';
  SID_IMFVideoMixerBitmap = '{814C7B20-0FDB-4eec-AF8F-F957C8F69EDC}';
  SID_IMFStreamSink = '{6ef2a660-47c0-4666-b13d-cbb717f2fa2c}';
  SID_IEVRFilterConfig = '{83E91E85-82C1-4EA7-801D-85DC50B75086}';
  SID_IMFVideoDisplayControl = '{A490B1E4-AB84-4D31-A1B2-181E03B1077A}';
  SID_IMFDesiredSample = '{56C294D0-753E-4260-8D61-A3D8820B1D54}';
  SID_IMFVideoPositionMapper = '{1F6A9F17-E70B-4E24-8AE4-0B2C3BA7A4AE}';
  SID_IMFVideoDeviceID = '{A38D9567-5A9C-4F3C-B293-8EB415B279BA}';
  SID_IMFVideoMixerControl = '{A5C6C53F-C202-4AA5-9695-175BA8C508A5}';
  SID_IMFGetService = '{FA993888-4383-415A-A930-DD472A8CF6F7}';
  SID_IMFVideoRenderer = '{DFDFD197-A9CA-43D8-B341-6AF3503792CD}';
  SID_IMFTrackedSample = '{245BF8E9-0755-40F7-88A5-AE0F18D55E17}';
  SID_IMFTopologyServiceLookup = '{fa993889-4383-415a-a930-dd472a8cf6f7}';
  SID_IMFTopologyServiceLookupClient = '{fa99388a-4383-415a-a930-dd472a8cf6f7}';
  SID_IEVRTrustedVideoPlugin = '{83A4CE40-7710-494b-A893-A472049AF630}';
  SID_IMFVideoPresenter = '{29AFF080-182A-4A5D-AF3B-448F3A6346CB}';
  SID_IMFMediaType = '{44ae0fa8-ea31-4109-8d2e-4cae4997c555}';

  SID_IMFAttributes = '{2cd2d921-c447-44a7-a13c-4adabfc247e3}';
  SID_IMFMediaBuffer = '{045FA593-8799-42b8-BC8D-8968C6453507}';
  SID_IMFSample = '{c40a00f2-b93a-4d80-ae8c-5a1c634f58e4}';
  SID_IMFMediaEvent = '{DF598932-F10C-4E39-BBA2-C308F101DAA3}';
  SID_IMFCollection = '{5BC8A76B-869A-46a3-9B03-FA218A66AEBE}';
  SID_IMFTransform = '{0a9ccdbc-d797-4563-9667-94ec5d79292d}';
  SID_IMFVideoMediaType = '{b99f381f-a8f9-47a2-a5af-ca3a225a3890}';
  SID_IMFAsyncResult = '{ac6b7889-0740-4d51-8619-905994a55cc6}';
  SID_IMFAsyncCallback = '{a27003cf-2354-4f2a-8d6a-ab7cff15437e}';
  SID_IMFClockStateSink = '{F6696E82-74F7-4f3d-A178-8A5E09C3659F}';
  SID_IMFRateSupport = '{0a9ccdbc-d797-4563-9667-94ec5d79292d}';
  SID_IMFCLOCK = '{2eb1e945-18b8-4139-9b1a-d5d584818530}';
  SID_IDirectXVideoProcessor = '{8c3a39f0-916e-4690-804f-4c8001355d25}';

var
  IID_IMFTrackedSample: TGUID = SID_IMFTrackedSample;
  IID_IMFVideoDisplayControl: TGUID = SID_IMFVideoDisplayControl;
  // GetService MR_VIDEO_RENDER_SERVICE
  IID_IMFVideoPresenter: TGUID = '{29AFF080-182A-4A5D-AF3B-448F3A6346CB}';
  IID_IMFVideoPositionMapper: TGUID = SID_IMFVideoPositionMapper;
  // GetService MR_VIDEO_RENDER_SERVICE
  IID_IMFDesiredSample: TGUID = SID_IMFDesiredSample;
  IID_IMFVideoMixerControl: TGUID = SID_IMFVideoMixerControl;
  // GetService MR_VIDEO_MIXER_SERVICE
  IID_IMFVideoRenderer: TGUID = SID_IMFVideoRenderer;
  IID_IMFVideoDeviceID: TGUID = SID_IMFVideoDeviceID;
  IID_IEVRFilterConfig: TGUID = SID_IEVRFilterConfig;
  IID_IMFTopologyServiceLookup: TGUID = SID_IMFTopologyServiceLookup;
  IID_IMFTopologyServiceLookupClient
    : TGUID = SID_IMFTopologyServiceLookupClient;
  IID_IEVRTrustedVideoPlugin: TGUID = SID_IEVRTrustedVideoPlugin;
  IID_IMFGetService: TGUID = SID_IMFGetService;
  IID_IDirectXVideoProcessor: TGUID = SID_IDirectXVideoProcessor;
  IID_IMFVideoMixerBitmap: TGUID = SID_IMFVideoMixerBitmap;

  CLSID_EnhancedVideoRenderer: TGUID = '{FA10746C-9B63-4B6C-BC49-FC300EA5F256}';
  CLSID_MFVideoMixer9: TGUID = '{E474E05A-AB65-4f6A-827C-218B1BAAF31F}';
  CLSID_MFVideoPresenter9: TGUID = '{98455561-5136-4D28-AB08-4CEE40EA2781}';
  CLSID_EVRTearlessWindowPresenter9
    : TGUID = '{A0A7A57B-59B2-4919-A694-ADD0A526C373}';

  MR_VIDEO_RENDER_SERVICE: TGUID = '{1092A86c-AB1A-459A-A336-831FBC4D11FF}';
  MR_VIDEO_MIXER_SERVICE: TGUID = '{073cd2fc-6cf4-40b7-8859-e89552c841f8}';
  MR_VIDEO_ACCELERATION_SERVICE
    : TGUID = '{efef5175-5c7d-4ce2-bbbd-34ff8bca6554}';
  MR_BUFFER_SERVICE: TGUID = '{a562248c-9ac6-4ffc-9fba-3af8f8ad1a4d}';

  VIDEO_ZOOM_RECT: TGUID = '{7aaa1638-1b7f-4c93-bd89-5b9c9fb6fcf0}';

type
  MFTIME = int64;
{$IF compilerversion < 18.5}
  ULONG_PTR = DWORD; // This is available in Delphi2007
{$IFEND}
  MediaEventType = IUnknown;

type
  IMFVideoPositionMapper = interface(IUnknown)
    [SID_IMFVideoPositionMapper]
    function MapOutputCoordinateToInputStream(xOut: Single; yOut: Single;
      dwOutputStreamIndex: DWORD; dwInputStreamIndex: DWORD; out pxIn: Single;
      out pyIn: Single): HRESULT; stdcall;
  end;

  IMFVideoDeviceID = interface(IUnknown)
    [SID_IMFVideoDeviceID]
    function GetDeviceID(out pDeviceID: TIID): HRESULT; stdcall;
  end;

const
  MFVideoARMode_None = $00000000;
  MFVideoARMode_PreservePicture = $00000001;
  MFVideoARMode_PreservePixel = $00000002;
  MFVideoARMode_NonLinearStretch = $00000004;
  MFVideoARMode_Mask = $00000007;

  // =============================================================================
  // Description:
  //
  // The rendering preferences used by the video presenter object.
  //
const // MFVideoRenderPrefs
  // Do not paint color keys (default off)
  MFVideoRenderPrefs_DoNotRenderBorder = $00000001;
  // Do not clip to monitor that has largest amount of video (default off)
  MFVideoRenderPrefs_DoNotClipToDevice = $00000002;
  MFVideoRenderPrefs_Mask = $00000003;

type
  PMFVideoNormalizedRect = ^TMFVideoNormalizedRect;

  TMFVideoNormalizedRect = record
    left: Single;
    top: Single;
    right: Single;
    bottom: Single;
{$IFNDEF norecprocs}
    procedure init(ALeft, ATop, ARight, ABottom: Single);
{$ENDIF}
  end;

type
  IMFVideoDisplayControl = interface(IUnknown)
    [SID_IMFVideoDisplayControl]
    function GetNativeVideoSize( { unique } out pszVideo: TSIZE;
      { unique } out pszARVideo: TSIZE): HRESULT; stdcall;
    function GetIdealVideoSize( { unique } out pszMin: TSIZE;
      { unique } out pszMax: TSIZE): HRESULT; stdcall;
    function SetVideoPosition( { unique } pnrcSource: PMFVideoNormalizedRect;
      { unique } prcDest: PRECT): HRESULT; stdcall;
    function GetVideoPosition(out pnrcSource: TMFVideoNormalizedRect;
      out prcDest: TRECT): HRESULT; stdcall;
    function SetAspectRatioMode(dwAspectRatioMode: DWORD): HRESULT; stdcall;
    function GetAspectRatioMode(out pdwAspectRatioMode: DWORD)
      : HRESULT; stdcall;
    function SetVideoWindow(hwndVideo: HWND): HRESULT; stdcall;
    function GetVideoWindow(out phwndVideo: HWND): HRESULT; stdcall;
    function RepaintVideo: HRESULT; stdcall;
    function GetCurrentImage(pBih: PBITMAPINFOHEADER; out lpDib;
      out pcbDib: DWORD;
      { unique } pTimeStamp: PInt64): HRESULT; stdcall;
    function SetBorderColor(Clr: COLORREF): HRESULT; stdcall;
    function GetBorderColor(out pClr: COLORREF): HRESULT; stdcall;
    function SetRenderingPrefs(dwRenderFlags: DWORD): HRESULT; stdcall;
    // a combination of MFVideoRenderPrefs
    function GetRenderingPrefs(out pdwRenderFlags: DWORD): HRESULT; stdcall;
    function SetFullscreen(fFullscreen: boolean): HRESULT; stdcall;
    function GetFullscreen(out pfFullscreen: boolean): HRESULT; stdcall;
  end;

type
  IDirectXVideoProcessor = interface(IUnknown)
    [SID_IDirectXVideoProcessor]
    function GetVideoProcessorService(out IDirectXVideoProcessorService)
      : HRESULT; stdcall;

    // virtual HRESULT STDMETHODCALLTYPE GetVideoProcessorService(
    // /* [out] */
    // __deref_out  IDirectXVideoProcessorService **ppService) = 0;
    //
    // virtual HRESULT STDMETHODCALLTYPE GetCreationParameters(
    // /* [out] */
    // __out_opt  GUID *pDeviceGuid,
    // /* [out] */
    // __out_opt  DXVA2_VideoDesc *pVideoDesc,
    // /* [out] */
    // __out_opt  D3DFORMAT *pRenderTargetFormat,
    // /* [out] */
    // __out_opt  UINT *pMaxNumSubStreams) = 0;
    //
    // virtual HRESULT STDMETHODCALLTYPE GetVideoProcessorCaps(
    // /* [out] */
    // __out  DXVA2_VideoProcessorCaps *pCaps) = 0;
    //
    // virtual HRESULT STDMETHODCALLTYPE GetProcAmpRange(
    // /* [in] */
    // __in  UINT ProcAmpCap,
    // /* [out] */
    // __out  DXVA2_ValueRange *pRange) = 0;
    //
    // virtual HRESULT STDMETHODCALLTYPE GetFilterPropertyRange(
    // /* [in] */
    // __in  UINT FilterSetting,
    // /* [out] */
    // __out  DXVA2_ValueRange *pRange) = 0;
    //
    // virtual HRESULT STDMETHODCALLTYPE VideoProcessBlt(
    // /* [in] */
    // __in  IDirect3DSurface9 *pRenderTarget,
    // /* [in] */
    // __in  const DXVA2_VideoProcessBltParams *pBltParams,
    // /* [size_is][in] */
    // __in_ecount(NumSamples)  const DXVA2_VideoSample *pSamples,
    // /* [in] */
    // __in  UINT NumSamples,
    // /* [out] */
    // __inout_opt  HANDLE *pHandleComplete) = 0;

  end;

  // =============================================================================
  // Description:
  //
  // The different message types that can be passed to the video presenter via
  // IMFVideoPresenter::ProcessMessage.
  //
  TMFVP_MESSAGE_TYPE = (
    // Called by the video renderer when a flush request is received on the
    // reference video stream. In response, the presenter should clear its
    // queue of samples waiting to be presented.
    // ulParam is unused and should be set to zero.
    MFVP_MESSAGE_FLUSH = $00000000,
    // Indicates to the presenter that the current output media type on the
    // mixer has changed. In response, the presenter may now wish to renegotiate
    // the media type of the video mixer.
    // Return Values:
    // S_OK - successful completion
    // MF_E_INVALIDMEDIATYPE - The presenter and mixer could not agree on
    // a media type.
    // ulParam is unused and should be set to zero.
    MFVP_MESSAGE_INVALIDATEMEDIATYPE = $00000001,
    // Indicates that a sample has been delivered to the video mixer object,
    // and there may now be a sample now available on the mixer's output. In
    // response, the presenter may want to draw frames out of the mixer's
    // output.
    // ulParam is unused and should be set to zero.
    MFVP_MESSAGE_PROCESSINPUTNOTIFY = $00000002,
    // Called when streaming is about to begin. In
    // response, the presenter should allocate any resources necessary to begin
    // streaming.
    // ulParam is unused and should be set to zero.
    MFVP_MESSAGE_BEGINSTREAMING = $00000003,
    // Called when streaming has completed. In
    // response, the presenter should release any resources that were
    // previously allocated for streaming.
    // ulParam is unused and should be set to zero.
    MFVP_MESSAGE_ENDSTREAMING = $00000004,
    // Indicates that the end of this segment has been reached.
    // When the last frame has been rendered, EC_COMPLETE should be sent
    // on the IMediaEvent interface retrieved from the renderer
    // during IMFTopologyServiceLookupClient::InitServicePointers method.
    // ulParam is unused and should be set to zero.
    MFVP_MESSAGE_ENDOFSTREAM = $00000005,
    // The presenter should step the number frames indicated by the lower DWORD
    // of ulParam.
    // The first n-1 frames should be skipped and only the nth frame should be
    // shown. Note that this message should only be received while in the pause
    // state or while in the started state when the rate is 0.
    // Otherwise, MF_E_INVALIDREQUEST should be returned.
    // When the nth frame has been shown EC_STEP_COMPLETE
    // should be sent on the IMediaEvent interface.
    // Additionally, if stepping is being done while the rate is set to 0
    // (a.k.a. "scrubbing"), the frame should be displayed immediately when
    // it is received, and EC_SCRUB_TIME should be sent right away after
    // sending EC_STEP_COMPLETE.
    MFVP_MESSAGE_STEP = $00000006,
    // The currently queued step operation should be cancelled. The presenter
    // should remain in the pause state following the cancellation.
    // ulParam is unused and should be set to zero.
    MFVP_MESSAGE_CANCELSTEP = $00000007);

  TMFRatio = record
    Numerator: DWORD;
    Denominator: DWORD;
  end;
{$WARNINGS OFF}

  TMFVideoChromaSubsampling = (MFVideoChromaSubsampling_Unknown = 0,
    MFVideoChromaSubsampling_ProgressiveChroma = $8,
    MFVideoChromaSubsampling_Horizontally_Cosited = $4,
    MFVideoChromaSubsampling_Vertically_Cosited = $2,
    MFVideoChromaSubsampling_Vertically_AlignedChromaPlanes = $1,

    MFVideoChromaSubsampling_MPEG2 =
    MFVideoChromaSubsampling_Horizontally_Cosited or
    MFVideoChromaSubsampling_Vertically_AlignedChromaPlanes,

    MFVideoChromaSubsampling_MPEG1 =
    MFVideoChromaSubsampling_Vertically_AlignedChromaPlanes,

    MFVideoChromaSubsampling_DV_PAL =
    MFVideoChromaSubsampling_Horizontally_Cosited or
    MFVideoChromaSubsampling_Vertically_Cosited,

    MFVideoChromaSubsampling_Cosited =
    MFVideoChromaSubsampling_Horizontally_Cosited or
    MFVideoChromaSubsampling_Vertically_Cosited or
    MFVideoChromaSubsampling_Vertically_AlignedChromaPlanes,

    MFVideoChromaSubsampling_Last = MFVideoChromaSubsampling_Cosited + 1,
    MFVideoChromaSubsampling_ForceDWORD = $7FFFFFFF);
{$WARNINGS ON}
  TMFVideoInterlaceMode = (MFVideoInterlace_Unknown = 0,
    MFVideoInterlace_Progressive = 2,
    MFVideoInterlace_FieldInterleavedUpperFirst = 3,
    MFVideoInterlace_FieldInterleavedLowerFirst = 4,
    MFVideoInterlace_FieldSingleUpper = 5,
    MFVideoInterlace_FieldSingleLower = 6,
    MFVideoInterlace_MixedInterlaceOrProgressive = 7, MFVideoInterlace_Last,
    MFVideoInterlace_ForceDWORD = $7FFFFFFF);

  TMFVideoTransferFunction = (MFVideoTransFunc_Unknown = 0,
    MFVideoTransFunc_10 = 1, MFVideoTransFunc_18 = 2, MFVideoTransFunc_20 = 3,
    MFVideoTransFunc_22 = 4, MFVideoTransFunc_709 = 5,
    MFVideoTransFunc_240M = 6, MFVideoTransFunc_sRGB = 7,
    MFVideoTransFunc_28 = 8, MFVideoTransFunc_Last,
    MFVideoTransFunc_ForceDWORD = $7FFFFFFF);

  TMFVideoPrimaries = (MFVideoPrimaries_Unknown = 0,
    MFVideoPrimaries_reserved = 1, MFVideoPrimaries_BT709 = 2,
    MFVideoPrimaries_BT470_2_SysM = 3, MFVideoPrimaries_BT470_2_SysBG = 4,
    MFVideoPrimaries_SMPTE170M = 5, MFVideoPrimaries_SMPTE240M = 6,
    MFVideoPrimaries_EBU3213 = 7, MFVideoPrimaries_SMPTE_C = 8,
    MFVideoPrimaries_Last, MFVideoPrimaries_ForceDWORD = $7FFFFFFF);

  TMFVideoTransferMatrix = (MFVideoTransferMatrix_Unknown = 0,
    MFVideoTransferMatrix_BT709 = 1, MFVideoTransferMatrix_BT601 = 2,
    MFVideoTransferMatrix_SMPTE240M = 3, MFVideoTransferMatrix_Last,
    MFVideoTransferMatrix_ForceDWORD = $7FFFFFFF);

  TMFVideoLighting = (MFVideoLighting_Unknown = 0, MFVideoLighting_bright = 1,
    MFVideoLighting_office = 2, MFVideoLighting_dim = 3,
    MFVideoLighting_dark = 4, MFVideoLighting_Last,
    MFVideoLighting_ForceDWORD = $7FFFFFFF);

  TMFNominalRange = (MFNominalRange_Unknown = 0, MFNominalRange_Normal = 1,
    MFNominalRange_Wide = 2,

    MFNominalRange_0_255 = 1, MFNominalRange_16_235 = 2,
    MFNominalRange_48_208 = 3);

  TMFT_MESSAGE_TYPE = (MFT_MESSAGE_COMMAND_FLUSH = $00000000,
    MFT_MESSAGE_COMMAND_DRAIN = $00000001,
    MFT_MESSAGE_SET_D3D_MANAGER = $00000002,
    MFT_MESSAGE_NOTIFY_BEGIN_STREAMING = $10000000,
    MFT_MESSAGE_NOTIFY_END_STREAMING = $10000001,
    MFT_MESSAGE_NOTIFY_END_OF_STREAM = $10000002,
    MFT_MESSAGE_NOTIFY_START_OF_STREAM = $10000003);

  TMFOffset = record
    fract: WORD;
    value: SHORT;
  end;

  TMFVideoArea = record
    OffsetX: TMFOffset;
    OffsetY: TMFOffset;
    Area: TSIZE;
  end;

  TMFVideoInfo = record
    dwWidth: DWORD;
    dwHeight: DWORD;
    PixelAspectRatio: TMFRatio;
    SourceChromaSubsampling: TMFVideoChromaSubsampling;
    InterlaceMode: TMFVideoInterlaceMode;
    TransferFunction: TMFVideoTransferFunction;
    ColorPrimaries: TMFVideoPrimaries;
    TransferMatrix: TMFVideoTransferMatrix;
    SourceLighting: TMFVideoLighting;
    FramesPerSecond: TMFRatio;
    NominalRange: TMFNominalRange;
    GeometricAperture: TMFVideoArea;
    MinimumDisplayAperture: TMFVideoArea;
    PanScanAperture: TMFVideoArea;
    VideoFlags: int64;
  end;

  TMFVideoCompressedInfo = record
    AvgBitrate: int64;
    AvgBitErrorRate: int64;
    MaxKeyFrameSpacing: DWORD;
  end;

  TMFARGB = record
    rgbBlue: Byte;
    rgbGreen: Byte;
    rgbRed: Byte;
    rgbAlpha: Byte;
  end;

  TMFAYUVSample = record
    bCrValue: Byte;
    bCbValue: Byte;
    bYValue: Byte;
    bSampleAlpha8: Byte;
  end;

  TMFPaletteEntry = record
    ARGB: TMFARGB;
    AYCbCr: TMFAYUVSample;
  end;

  TMFVideoSurfaceInfo = record
    Format: DWORD;
    PaletteEntries: DWORD;
    Palette: array of TMFPaletteEntry;
  end;

  TMFVIDEOFORMAT = record
    dwSize: DWORD;
    videoInfo: TMFVideoInfo;
    guidFormat: TGUID;
    compressedInfo: TMFVideoCompressedInfo;
    surfaceInfo: TMFVideoSurfaceInfo;
  end;

  TMFT_INPUT_STREAM_INFO = record
    hnsMaxLatency: LONGLONG;
    dwFlags: DWORD;
    cbSize: DWORD;
    cbMaxLookahead: DWORD;
    cbAlignment: DWORD;
  end;

  TMFT_OUTPUT_STREAM_INFO = record
    dwFlags: DWORD;
    cbSize: DWORD;
    cbAlignment: DWORD;
  end;

  TMF_ATTRIBUTES_MATCH_TYPE = (MF_ATTRIBUTES_MATCH_OUR_ITEMS = 0,
    MF_ATTRIBUTES_MATCH_THEIR_ITEMS = 1, MF_ATTRIBUTES_MATCH_ALL_ITEMS = 2,
    MF_ATTRIBUTES_MATCH_INTERSECTION = 3, MF_ATTRIBUTES_MATCH_SMALLER = 4);

  TMF_ATTRIBUTE_TYPE = (MF_ATTRIBUTE_UINT32 = VT_UI4,
    MF_ATTRIBUTE_UINT64 = VT_UI8, MF_ATTRIBUTE_DOUBLE = VT_R8,
    MF_ATTRIBUTE_GUID = VT_CLSID, MF_ATTRIBUTE_STRING = VT_LPWSTR,
    MF_ATTRIBUTE_BLOB = VT_VECTOR or VT_UI1,
    MF_ATTRIBUTE_IUNKNOWN = VT_UNKNOWN);

  TMF_RATE_DIRECTION = (MFRATE_FORWARD, MFRATE_REVERSE);

  TMF_CLOCK_PROPERTIES = record
    qwCorrelationRate: int64;
    guidClockId: TGUID;
    dwClockFlags: DWORD;
    qwClockFrequency: int64;
    dwClockTolerance: DWORD;
    dwClockJitter: DWORD;
  end;

  TMF_CLOCK_STATE = (MFCLOCK_STATE_INVALID, MFCLOCK_STATE_RUNNING,
    MFCLOCK_STATE_STOPPED, MFCLOCK_STATE_PAUSED);

  IMFClockStateSink = interface(IUnknown)
    [SID_IMFClockStateSink]
    function OnClockPause(hnsSystemTime: MFTIME): HRESULT; stdcall;
    function OnClockRestart(hnsSystemTime: MFTIME): HRESULT; stdcall;
    function OnClockSetRate(hnsSystemTime: MFTIME; flRate: Single)
      : HRESULT; stdcall;
    function OnClockStart(hnsSystemTime: MFTIME; llClockStartOffset: int64)
      : HRESULT; stdcall;
    function OnClockStop(hnssSystemTime: MFTIME): HRESULT; stdcall;
  end;

  IMFVideoMediaType = interface(IUnknown)
    [SID_IMFVideoMediaType]
    function GetVideoFormat(): TMFVIDEOFORMAT; stdcall;
    function GetVideoRepresentation(guidRepresentation: TGUID;
      ppvRepresentation: pointer; lStride: Longint): HRESULT; stdcall;
  end;

  IMFVideoPresenter = interface(IMFClockStateSink)
    [SID_IMFVideoPresenter]
    function ProcessMessage(eMessage: TMFVP_MESSAGE_TYPE; ulParam: ULONG_PTR)
      : HRESULT; stdcall;
    function GetCurrentMediaType(out ppMediaType: IMFVideoMediaType)
      : HRESULT; stdcall;
  end;

  IMFDesiredSample = interface(IUnknown)
    [SID_IMFDesiredSample]
    function GetDesiredSampleTimeAndDuration(out phnsSampleTime: int64;
      out phnsSampleDuration: int64): HRESULT; stdcall;
    function SetDesiredSampleTimeAndDuration(hnsSampleTime: int64;
      hnsSampleDuration: int64): HRESULT; stdcall;
    procedure Clear; stdcall;
  end;

  IMFAsyncResult = interface(IUnknown)
    [SID_IMFAsyncResult]
    function GetObject(out ppObject: IUnknown): HRESULT; stdcall;
    function GetState(out ppunkState: IUnknown): HRESULT; stdcall;
    function GetStateNoAddRef(): IUnknown; stdcall;
    function GetStatus(): HRESULT; stdcall;
    function SetStatus(hrStatus: HRESULT): HRESULT; stdcall;
  end;

  IMFAsyncCallback = interface(IUnknown)
    [SID_IMFAsyncCallback]
    function GetParameters(out pdwFlags: DWORD; out pdwQueue: DWORD)
      : HRESULT; stdcall;
    function Invoke(out pAsyncResult: IMFAsyncResult): HRESULT; stdcall;
  end;

  IMFTrackedSample = interface(IUnknown)
    [SID_IMFTrackedSample]
    function SetAllocator(pSampleAllocator: IMFAsyncCallback;
      { unique } pUnkState: IUnknown): HRESULT; stdcall;
  end;

  IMFVideoMixerControl = interface(IUnknown)
    [SID_IMFVideoMixerControl]
    function SetStreamZOrder(dwStreamID: DWORD; dwZ: DWORD): HRESULT; stdcall;
    function GetStreamZOrder(dwStreamID: DWORD; out pdwZ: DWORD)
      : HRESULT; stdcall;
    function SetStreamOutputRect(dwStreamID: DWORD;
      pnrcOutput: PMFVideoNormalizedRect): HRESULT; stdcall;
    function GetStreamOutputRect(dwStreamID: DWORD;
      out pnrcOutput: TMFVideoNormalizedRect): HRESULT; stdcall;
  end;

  IMFAttributes = interface(IUnknown)
    [SID_IMFAttributes]
    function Compare(pTheirs: IMFAttributes;
      MatchType: TMF_ATTRIBUTES_MATCH_TYPE; out pbResult: boolean)
      : HRESULT; stdcall;
    function CompareItem(guidKey: PGuid; value: PPROPVARIANT;
      out pbResult: BOOL): HRESULT; stdcall;
    function CopyAllItems(pDest: IMFAttributes): HRESULT; stdcall;
    function DeleteAllItems(): HRESULT; stdcall;
    function DeleteItem(guidKey: PGuid): HRESULT; stdcall;
    function GetAllocatedBlob(guidKey: PGuid; out ppBuf: PByte;
      out pcbSize: PDWord): HRESULT; stdcall;
    function GetAllocatedString(guidKey: PGuid; ppwszValue: PWideChar;
      out pcchLength: DWORD): HRESULT; stdcall;
    function GetBlob(guidKey: PGuid; pBuf: PByte; cbBufsize: DWORD;
      pcbBlobSize: PDWord): HRESULT; stdcall;
    function GetBlobSize(guidKey: PGuid; out pcbBlobSize: DWORD)
      : HRESULT; stdcall;
    function GetCount(out pcItems: DWORD): HRESULT; stdcall;
    function GetDouble(guidKey: PGuid; out pfValue: double): HRESULT; stdcall;
    function GetGUID(guidKey: PGuid; out pguidValue: TGUID): HRESULT; stdcall;
    function GetItem(guidKey: PGuid; out pValue: PROPVARIANT): HRESULT; stdcall;
    function GetItemByIndex(unIndex: DWORD; Guid: TGUID;
      out pValue: PROPVARIANT): HRESULT; stdcall;
    function GetItemType(guidKey: PGuid; out pType: TMF_ATTRIBUTE_TYPE)
      : HRESULT; stdcall;
    function GetString(guidKey: PGuid; pwszValue: PWideChar; cchBufSize: DWORD;
      out pcchLength: DWORD): HRESULT; stdcall;
    function GetStringLength(guidKey: PGuid; out pcchLength: DWORD)
      : HRESULT; stdcall;
    function GetUINT32(guidKey: PGuid; out punValue: DWORD): HRESULT; stdcall;
    function GetUINT64(guidKey: PGuid; out punValue: int64): HRESULT; stdcall;
    function GetUnknown(guidKey: PGuid; riid: TGUID; out ppV: pointer)
      : HRESULT; stdcall;
    function LockStore(): HRESULT; stdcall;
    function SetBlob(guidKey: PGuid; pBuf: PByte; cbBufsize: DWORD)
      : HRESULT; stdcall;
    function SetDouble(guidKey: PGuid; fValue: double): HRESULT; stdcall;
    function SetGUID(guidKey: PGuid; guidValue: PGuid): HRESULT; stdcall;
    function SetItem(guidKey: PGuid; value: PPROPVARIANT): HRESULT; stdcall;
    function SetString(guidKey: PGuid; wszValue: PWideChar): HRESULT; stdcall;
    function SetUINT32(guidKey: PGuid; unValue: DWORD): HRESULT; stdcall;
    function SetUINT64(guidKey: PGuid; unValue: int64): HRESULT; stdcall;
    function SetUnknown(guidKey: PGuid; pUnknown: IUnknown): HRESULT; stdcall;
    function UnlockStore(): HRESULT; stdcall;
  end;

  IMFMediaType = interface(IMFAttributes)
    [SID_IMFMediaType]
    function GetMajorType(out pguidMajorType: TGUID): HRESULT; stdcall;
    function IsCompressedFormat(out pfCompressed: BOOL): HRESULT; stdcall;
    function IsEqual(pIMediaType: IMFMediaType; out pdwFlags: DWORD)
      : HRESULT; stdcall;
    function GetRepresentation(const guidRepresentation: TGUID;
      out ppvRepresentation: pointer): HRESULT; stdcall;
    function FreeRepresentation(const guidRepresentation: TGUID;
      pvRepresentation: pointer): HRESULT; stdcall;
  end;

  IMFMediaBuffer = interface(IUnknown)
    [SID_IMFMediaBuffer]
    function GetCurrentLength(out pcbCurrentLength: DWORD): HRESULT; stdcall;
    function GetMaxLength(out pcbMaxLength: DWORD): HRESULT; stdcall;
    function Lock(out ppbBuffer: PByte; out pcbMaxLength: DWORD;
      out pcbCurrentLength: DWORD): HRESULT; stdcall;
    function SetCurrentLength(cbCurrentLength: DWORD): HRESULT; stdcall;
    function Unlock(): HRESULT; stdcall;
  end;

  IMFSample = interface(IUnknown)
    [SID_IMFSample]
    function AddBuffer(pBuffer: IMFMediaBuffer): HRESULT; stdcall;
    function ConvertToContiguousBuffer(out ppBuffer: IMFMediaBuffer)
      : HRESULT; stdcall;
    function CopyToBuffer(pBuffer: IMFMediaBuffer): HRESULT; stdcall;
    function GetBufferByIndex(dwIndex: DWORD; out ppBuffer: IMFMediaBuffer)
      : HRESULT; stdcall;
    function GetBufferCount(out pdwBufferCount: DWORD): HRESULT; stdcall;
    function GetSampleDuration(out phnsSampleDuration: LONGLONG)
      : HRESULT; stdcall;
    function GetSampleFlags(out pdwSampleFlags: DWORD): HRESULT; stdcall;
    function GetSampleTime(out phnsSampleTime: LONGLONG): HRESULT; stdcall;
    function GetTotalLength(out pcbTotalLength: DWORD): HRESULT; stdcall;
    function RemoveAllBuffers(): HRESULT; stdcall;
    function RemoveBufferByIndex(dwIndex: DWORD): HRESULT; stdcall;
    function SetSampleDuration(hnsSampleDuration: LONGLONG): HRESULT; stdcall;
    function SetSampleFlags(dwSampleFlags: DWORD): HRESULT; stdcall;
    function SetSampleTime(hnsSampleTime: LONGLONG): HRESULT; stdcall;
  end;

  IMFMediaEvent = interface(IUnknown)
    [SID_IMFMediaEvent]
    function GetExtendedType(out pguidExtendedType: TGUID): HRESULT; stdcall;
    function GetStatus(out phrStatus: HRESULT): HRESULT; stdcall;
    function GetType(out pmet: MediaEventType): HRESULT; stdcall;
    function GetValue(pvValue: PROPVARIANT): HRESULT; stdcall;
  end;

  IMFCollection = interface(IUnknown)
    [SID_IMFCollection]
    function AddElement(pUnkElement: IUnknown): HRESULT; stdcall;
    function GetElement(dwElementIndex: DWORD; out ppUnkElement: IUnknown)
      : HRESULT; stdcall;
    function GetElementCount(out pcElements: DWORD): HRESULT; stdcall;
    function InsertElementAt(dwIndex: DWORD; pUnknown: IUnknown)
      : HRESULT; stdcall;
    function RemoveAllElements(): HRESULT; stdcall;
    function RemoveElement(dwElementIndex: DWORD; out ppUnkElement: IUnknown)
      : HRESULT; stdcall;
  end;

  IMFRateSupport = interface(IUnknown)
    [SID_IMFRateSupport]
    function GetFastestRate(eDirection: TMF_RATE_DIRECTION; fThin: BOOL;
      out pflRate: double): HRESULT; stdcall;
    function GetSlowestRate(eDirection: TMF_RATE_DIRECTION; fThin: BOOL;
      out pflRate: double): HRESULT; stdcall;
    function IsRateSupported(fThin: BOOL; flRate: double;
      out pflNearestSupportedRate: double): HRESULT; stdcall;
  end;

  IMFClock = interface(IUnknown)
    [SID_IMFCLOCK]
    function GetClockCharacteristics(out pdwCharacteristics: DWORD)
      : HRESULT; stdcall;
    function GetContinuityKey(out pdwContinuityKey: DWORD): HRESULT; stdcall;
    function GetCorrelatedTime(dwReserved: DWORD; out pllClockTime: int64;
      out phnsSystemTime: MFTIME): HRESULT; stdcall;
    function GetProperties(out pClockProperties: TMF_CLOCK_PROPERTIES)
      : HRESULT; stdcall;
    function GetState(dwReserved: DWORD; out peClockState: TMF_CLOCK_STATE)
      : HRESULT; stdcall;
  end;

  TMFT_OUTPUT_DATA_BUFFER = record
    dwStreamID: DWORD;
    pSample: IMFSample;
    dwStatus: DWORD;
    pEvents: IMFCollection;
  end;

  IMFTransform = interface(IUnknown)
    [SID_IMFTransform]
    function AddInputStreams(cStreams: DWORD; out adwStreamIDs: DWORD)
      : HRESULT; stdcall;
    function DeleteInputStream(dwStreamID: DWORD): HRESULT; stdcall;
    function GetAttributes(out pAttributes: IMFAttributes): HRESULT; stdcall;
    function GetInputAvailableType(dwInputStreamID: DWORD; dwTypeIndex: DWORD;
      out ppType: IMFMediaType): HRESULT; stdcall;
    function GetInputCurrentType(dwInputStreamID: DWORD;
      out ppType: IMFMediaType): HRESULT; stdcall;
    function GetInputStatus(dwInputStreamID: DWORD; out pdwFlags: DWORD)
      : HRESULT; stdcall;
    function GetInputStreamAttributes(dwInputStreamID: DWORD;
      out ppAttributes: IMFAttributes): HRESULT; stdcall;
    function GetInputStreamInfo(dwInputStreamID: DWORD;
      pStreamInfo: TMFT_INPUT_STREAM_INFO): HRESULT; stdcall;
    function GetOutputAvailableType(dwOutputStreamID: DWORD; dwTypeIndex: DWORD;
      out ppType: IMFMediaType): HRESULT; stdcall;
    function GetOutputCurrentType(dwOutputStreamID: DWORD;
      out ppType: IMFMediaType): HRESULT; stdcall;
    function GetOutputStatus(out pdwFlags: DWORD): HRESULT; stdcall;
    function GetOutputStreamAttributes(dwOutputStreamID: DWORD;
      out ppAttributes: IMFAttributes): HRESULT; stdcall;
    function GetOutputStreamInfo(dwOutputStreamID: DWORD;
      out pStreamInfo: TMFT_OUTPUT_STREAM_INFO): HRESULT; stdcall;
    function GetStreamCount(out pcInputStreams: DWORD;
      out pcOutputStreams: DWORD): HRESULT; stdcall;
    function GetStreamIDs(dwInputIDArraySize: DWORD; out pdwInputIDs: DWORD;
      dwOutputIDArraySize: DWORD; out pdwOutputIDs: DWORD): HRESULT; stdcall;
    function GetStreamLimits(out pdwInputMinimum: DWORD;
      out pdwInputMaximum: DWORD; out pdwOutputMinimum: DWORD;
      out pdwOutputMaximum: DWORD): HRESULT; stdcall;
    function ProcessEvent(dwInputStreamID: DWORD; pEvent: IMFMediaEvent)
      : HRESULT; stdcall;
    function ProcessInput(dwInputStreamID: DWORD; pSample: IMFSample;
      dwFlags: DWORD): HRESULT; stdcall;
    function ProcessMessage(eMessage: TMFT_MESSAGE_TYPE; ulParam: ULONG_PTR)
      : HRESULT; stdcall;
    function ProcessOutput(dwFlags: DWORD; cOutputBufferCount: DWORD;
      out pOutputSamples: TMFT_OUTPUT_DATA_BUFFER; out pdwStatus: DWORD)
      : HRESULT; stdcall;
    function SetInputType(dwInputStreamID: DWORD; pType: IMFMediaType;
      dwFlags: DWORD): HRESULT; stdcall;
    function SetOutputBounds(hnsLowerBound: LONGLONG; hnsUpperBound: LONGLONG)
      : HRESULT; stdcall;
    function SetOutputType(dwOutputStreamID: DWORD; pType: IMFMediaType;
      dwFlags: DWORD): HRESULT; stdcall;
  end;

  IMFVideoRenderer = interface(IUnknown)
    [SID_IMFVideoRenderer]
    function InitializeRenderer(pVideoMixer: IMFTransform;
      pVideoPresenter: IMFVideoPresenter): HRESULT; stdcall;
  end;

  /// ////////////////////////////////////////////////////////////////////////////
  /// ////////////////////////////////////////////////////////////////////////////

type
  MF_SERVICE_LOOKUP_TYPE = (MF_SERVICE_LOOKUP_UPSTREAM,
    MF_SERVICE_LOOKUP_UPSTREAM_DIRECT, MF_SERVICE_LOOKUP_DOWNSTREAM,
    MF_SERVICE_LOOKUP_DOWNSTREAM_DIRECT, MF_SERVICE_LOOKUP_ALL,
    // lookup service on any components of the graph
    MF_SERVICE_LOOKUP_GLOBAL); // lookup global objects

  IMFTopologyServiceLookup = interface(IUnknown)
    [SID_IMFTopologyServiceLookup]
    function LookupService(_type: MF_SERVICE_LOOKUP_TYPE; dwIndex: DWORD;
      const guidService: TIID;
      { in } const riid: TIID; out ppvObjects; var pnObjects: DWORD)
      : HRESULT; stdcall;
  end;

  IMFTopologyServiceLookupClient = interface(IUnknown)
    [SID_IMFTopologyServiceLookupClient]
    function InitServicePointers(pLookup: IMFTopologyServiceLookup)
      : HRESULT; stdcall;
  end;

  IEVRTrustedVideoPlugin = interface(IUnknown)
    [SID_IEVRTrustedVideoPlugin]
    function IsInTrustedVideoMode(out pYes: BOOL): HRESULT; stdcall;
    function CanConstrict(out pYes: BOOL): HRESULT; stdcall;
    function SetConstriction(dwKPix: DWORD): HRESULT; stdcall;
    function DisableImageExport(bDisable: BOOL): HRESULT; stdcall;
  end;

type
  IEVRFilterConfig = interface(IUnknown)
    [SID_IEVRFilterConfig]
    function SetNumberOfStreams(dwMaxStreams: DWORD): HRESULT; stdcall;
    function GetNumberOfStreams(out pdwMaxStreams: DWORD): HRESULT; stdcall;
  end;

  IMFGetService = interface(IUnknown)
    [SID_IMFGetService]
    function GetService(const guidService: TGUID; const IID: TIID;
      out ppvObject): HRESULT; stdcall;
  end;

type
  D3DPOOL = DWORD;

  TDXVA2_Fixed32 = record
    case integer of
      0:
        (Fraction: WORD;
          // USHORT;  (Unsigned SmallInt = Word)
          value: SHORT);
      1:
        (ll: Longint)
  end;

  TDXVA2_VideoProcessorCaps = record
    DeviceCaps: UINT; // see DXVA2_VPDev_Xxxx
    InputPool: D3DPOOL;
    NumForwardRefSamples: UINT;
    NumBackwardRefSamples: UINT;
    Reserved: UINT;
    DeinterlaceTechnology: UINT; // see DXVA2_DeinterlaceTech_Xxxx
    ProcAmpControlCaps: UINT; // see DXVA2_ProcAmp_Xxxx
    VideoProcessorOperations: UINT; // see DXVA2_VideoProcess_Xxxx
    NoiseFilterTechnology: UINT; // see DXVA2_NoiseFilterTech_Xxxx
    DetailFilterTechnology: UINT; // see DXVA2_DetailFilterTech_Xxxx
  end;

  TDXVA2_ValueRange = record
    MinValue: TDXVA2_Fixed32;
    MaxValue: TDXVA2_Fixed32;
    DefaultValue: TDXVA2_Fixed32;
    StepSize: TDXVA2_Fixed32;
  end;

  TDXVA2_ProcAmpValues = record
    Brightness: TDXVA2_Fixed32;
    Contrast: TDXVA2_Fixed32;
    Hue: TDXVA2_Fixed32;
    Saturation: TDXVA2_Fixed32;
  end;

const
  DXVA2_ProcAmp_None = $0000;
  DXVA2_ProcAmp_Brightness = $0001;
  DXVA2_ProcAmp_Contrast = $0002;
  DXVA2_ProcAmp_Hue = $0004;
  DXVA2_ProcAmp_Saturation = $0008;
  DXVA2_ProcAmp_Mask = $000F;

type
  IMFVideoProcessor = interface(IUnknown)
    [SID_IMFVideoProcessor]
    function GetAvailableVideoProcessorModes(var lpdwNumProcessingModes: UINT;
      { [size_is][size_is][out] } out
      ppVideoProcessingModes { Pointer to Array of GUID } ): HRESULT; stdcall;
    function GetVideoProcessorCaps(lpVideoProcessorMode: PGuid;
      { [out] } out lpVideoProcessorCaps: TDXVA2_VideoProcessorCaps)
      : HRESULT; stdcall;
    function GetVideoProcessorMode(out lpMode: TGUID): HRESULT; stdcall;
    function SetVideoProcessorMode(lpMode: PGuid): HRESULT; stdcall;
    function GetProcAmpRange(dwProperty: DWORD;
      out pPropRange: TDXVA2_ValueRange): HRESULT; stdcall;
    function GetProcAmpValues(dwFlags: DWORD; out values: TDXVA2_ProcAmpValues)
      : HRESULT; stdcall;
    function SetProcAmpValues(dwFlags: DWORD;
      { in } const pValues: TDXVA2_ProcAmpValues): HRESULT; stdcall;
    function GetFilteringRange(dwProperty: DWORD;
      out pPropRange: TDXVA2_ValueRange): HRESULT; stdcall;
    function GetFilteringValue(dwProperty: DWORD; out pValue: TDXVA2_Fixed32)
      : HRESULT; stdcall;
    function SetFilteringValue(dwProperty: DWORD; const pValue: TDXVA2_Fixed32)
      : HRESULT; stdcall;
    function GetBackgroundColor(out lpClrBkg: COLORREF): HRESULT; stdcall;
    function SetBackgroundColor(ClrBkg: COLORREF): HRESULT; stdcall;
  end;

  // TMFVideoAlphaBitmapFlags = (
const
  MFVideoAlphaBitmap_EntireDDS = $1;
  MFVideoAlphaBitmap_SrcColorKey = $2;
  MFVideoAlphaBitmap_SrcRect = $4;
  MFVideoAlphaBitmap_DestRect = $8;
  MFVideoAlphaBitmap_FilterMode = $10;
  MFVideoAlphaBitmap_Alpha = $20;
  MFVideoAlphaBitmap_BitMask = $3F;

type
  TMFVideoAlphaBitmapParams = record
    dwFlags: DWORD;
    clrSrcKey: COLORREF;
    rcSrc: TRECT;
    nrcDest: TMFVideoNormalizedRect;
    fAlpha: Single;
    dwFilterMode: DWORD;
  end;

  // IDirect3DSurface9 = Pointer; // TODO (for now use a pointer to avoid dependencies to DirectX 9 units)

  // TMFVideoAlphaBitmap = record
  // GetBitmapFromDC: Boolean;
  // case Boolean of
  // True: (hdc: HDC; params: TMFVideoAlphaBitmapParams);
  // False: (pDDS: IDirect3DSurface9; params2: TMFVideoAlphaBitmapParams; );
  // end;

  PMFVideoAlphaBitmap = ^TMFVideoAlphaBitmap;

  TMFVideoAlphaBitmap = record
    GetBitmapFromDC: boolean;
    case boolean of
      True:
        (hdc_: HDC; params: TMFVideoAlphaBitmapParams);
      False:
        (pDDS: pointer; params2: TMFVideoAlphaBitmapParams;);
  end;

  // PMFVideoAlphaBitmap = ^TMFVideoAlphaBitmap;
  // TMFVideoAlphaBitmap = record
  // GetBitmapFromDC: Boolean;
  // hdc_: HDC;
  // pDDS: IDirect3DSurface9;
  // params: TMFVideoAlphaBitmapParams;
  // end;

  IMFVideoMixerBitmap = interface(IUnknown)
    [SID_IMFVideoMixerBitmap]
    function SetAlphaBitmap(const pBmpParms: PMFVideoAlphaBitmap)
      : HRESULT; stdcall;
    function ClearAlphaBitmap: HRESULT; stdcall;
    function UpdateAlphaBitmapParameters(const pBmpParms
      : TMFVideoAlphaBitmapParams): HRESULT; stdcall;
    function GetAlphaBitmapParameters(out pBmpParms: TMFVideoAlphaBitmapParams)
      : HRESULT; stdcall;
  end;

function MFVideoNormalizedRect(const ALeft, ATop, ARight, ABottom: Single)
  : TMFVideoNormalizedRect;

// C++ CodecAPI.h ->> Delphi

//
// Common Parameters
//

// AVEncCommonFormatConstraint (GUID)

const
  AVEncCommonFormatConstraint: TGUID = '{57cbb9b8-116f-4951-b40c-c2a035ed8f17}';

  AVEncCommonFormatUnSpecified
    : TGUID = '{af46a35a-6024-4525-a48a-094b97f5b3c2}';
  AVEncCommonFormatDVD_V: TGUID = '{cc9598c4-e7fe-451d-b1ca-761bc840b7f3}';
  AVEncCommonFormatDVD_DashVR: TGUID = '{e55199d6-044c-4dae-a488-531ed306235b}';
  AVEncCommonFormatDVD_PlusVR: TGUID = '{e74c6f2e-ec37-478d-9af4-a5e135b6271c}';
  AVEncCommonFormatVCD: TGUID = '{95035bf7-9d90-40ff-ad5c-5cf8cf71ca1d}';
  AVEncCommonFormatSVCD: TGUID = '{51d85818-8220-448c-8066-d69bed16c9ad}';
  AVEncCommonFormatATSC: TGUID = '{8d7b897c-a019-4670-aa76-2edcac7ac296}';
  AVEncCommonFormatDVB: TGUID = '{71830d8f-6c33-430d-844b-c2705baae6db}';
  AVEncCommonFormatMP3: TGUID = '{349733cd-eb08-4dc2-8197-e49835ef828b}';
  AVEncCommonFormatHighMAT: TGUID = '{1eabe760-fb2b-4928-90d1-78db88eee889}';
  AVEncCommonFormatHighMPV: TGUID = '{a2d25db8-b8f9-42c2-8bc7-0b93cf604788}';

  AVEncCodecType: TGUID = '{08af4ac1-f3f2-4c74-9dcf-37f2ec79f826}';

  AVEncMPEG1Video: TGUID = '{c8dafefe-da1e-4774-b27d-11830c16b1fe}';
  AVEncMPEG2Video: TGUID = '{046dc19a-6677-4aaa-a31d-c1ab716f4560}';
  AVEncMPEG1Audio: TGUID = '{d4dd1362-cd4a-4cd6-8138-b94db4542b04}';
  AVEncMPEG2Audio: TGUID = '{ee4cbb1f-9c3f-4770-92b5-fcb7c2a8d381}';
  AVEncWMV: TGUID = '{4e0fef9b-1d43-41bd-b8bd-4d7bf7457a2a}';
  AVEndMPEG4Video: TGUID = '{dd37b12a-9503-4f8b-b8d0-324a00c0a1cf}';
  AVEncH264Video: TGUID = '{95044eab-31b3-47de-8e75-38a42bb03e28}';
  AVEncDV: TGUID = '{09b769c7-3329-44fb-8954-fa30937d3d5a}';
  AVEncWMAPro: TGUID = '{1955f90c-33f7-4a68-ab81-53f5657125c4}';
  AVEncWMALossless: TGUID = '{55ca7265-23d8-4761-9031-b74fbe12f4c1}';
  AVEncWMAVoice: TGUID = '{13ed18cb-50e8-4276-a288-a6aa228382d9}';
  AVEncDolbyDigitalPro: TGUID = '{f5be76cc-0ff8-40eb-9cb1-bba94004d44f}';
  AVEncDolbyDigitalConsumer: TGUID = '{c1a7bf6c-0059-4bfa-94ef-ef747a768d52}';
  AVEncDolbyDigitalPlus: TGUID = '{698d1b80-f7dd-415c-971c-42492a2056c6}';
  AVEncDTSHD: TGUID = '{2052e630-469d-4bfb-80ca-1d656e7e918f}';
  AVEncDTS: TGUID = '{45fbcaa2-5e6e-4ab0-8893-5903bee93acf}';
  AVEncMLP: TGUID = '{05f73e29-f0d1-431e-a41c-a47432ec5a66}';
  AVEncPCM: TGUID = '{844be7f4-26cf-4779-b386-cc05d187990c}';
  AVEncSDDS: TGUID = '{1dc1b82f-11c8-4c71-b7b6-ee3eb9bc2b94}';

  // AVEncCommonRateControlMode (UINT32)
  AVEncCommonRateControlMode: TGUID = '{1c0608e9-370c-4710-8a58-cb6181c42423}';

type
  eAVEncCommonRateControlMode = (eAVEncCommonRateControlMode_CBR,
    eAVEncCommonRateControlMode_PeakConstrainedVBR,
    eAVEncCommonRateControlMode_UnconstrainedVBR,
    eAVEncCommonRateControlMode_Quality);

const
  // AVEncCommonLowLatency (BOOL)
  AVEncCommonLowLatency: TGUID = '{9d3ecd55-89e8-490a-970a-0c9548d5a56e}';

  // AVEncCommonMultipassMode (UINT32)
  AVEncCommonMultipassMode: TGUID = '{22533d4c-47e1-41b5-9352-a2b7780e7ac4}';
  // AVEncCommonPassStart (UINT32)
  AVEncCommonPassStart: TGUID = '{6a67739f-4eb5-4385-9928-f276a939ef95}';
  // AVEncCommonPassEnd (UINT32)
  AVEncCommonPassEnd: TGUID = '{0e3d01bc-c85c-467d-8b60-c41012ee3bf6}';

  // AVEncCommonRealTime (BOOL)
  AVEncCommonRealTime: TGUID = '{143a0ff6-a131-43da-b81e-98fbb8ec378e}';

  // AVEncCommonQuality (UINT32)
  AVEncCommonQuality: TGUID = '{fcbf57a3-7ea5-4b0c-9644-69b40c39c391}';

  // AVEncCommonQualityVsSpeed (UINT32)
  AVEncCommonQualityVsSpeed: TGUID = '{98332df8-03cd-476b-89fa-3f9e442dec9f}';

  // AVEncCommonMeanBitRate (UINT32)
  AVEncCommonMeanBitRate: TGUID = '{f7222374-2144-4815-b550-a37f8e12ee52}';

  // AVEncCommonMeanBitRateInterval (UINT64)
  AVEncCommonMeanBitRateInterval
    : TGUID = '{bfaa2f0c-cb82-4bc0-8474-f06a8a0d0258}';

  // AVEncCommonMaxBitRate (UINT32)
  AVEncCommonMaxBitRate: TGUID = '{9651eae4-39b9-4ebf-85ef-d7f444ec7465}';

  // AVEncCommonMinBitRate (UINT32)
  AVEncCommonMinBitRate: TGUID = '{101405b2-2083-4034-a806-efbeddd7c9ff}';

  // AVEncCommonBufferSize (UINT32)
  AVEncCommonBufferSize: TGUID = '{0db96574-b6a4-4c8b-8106-3773de0310cd}';

  // AVEncCommonBufferInLevel (UINT32)
  AVEncCommonBufferInLevel: TGUID = '{d9c5c8db-fc74-4064-94e9-cd19f947ed45}';

  // AVEncCommonBufferOutLevel (UINT32)
  AVEncCommonBufferOutLevel: TGUID = '{ccae7f49-d0bc-4e3d-a57e-fb5740140069}';

  // AVEncCommonStreamEndHandling (UINT32)
  AVEncCommonStreamEndHandling
    : TGUID = '{6aad30af-6ba8-4ccc-8fca-18d19beaeb1c}';

type
  eAVEncCommonStreamEndHandling = (eAVEncCommonStreamEndHandling_DiscardPartial,
    eAVEncCommonStreamEndHandling_EnsureComplete);

  //
  // Common Post Encode Statistical Parameters
  //

  // AVEncStatCommonCompletedPasses (UINT32)
const
  AVEncStatCommonCompletedPasses
    : TGUID = '{3e5de533-9df7-438c-854f-9f7dd3683d34}';

  //
  // Common Video Parameters
  //

  // AVEncVideoOutputFrameRate (UINT32)
  AVEncVideoOutputFrameRate: TGUID = '{ea85e7c3-9567-4d99-87c4-02c1c278ca7c}';

  // AVEncVideoOutputFrameRateConversion (UINT32)
  AVEncVideoOutputFrameRateConversion
    : TGUID = '{8c068bf4-369a-4ba3-82fd-b2518fb3396e}';

type
  eAVEncVideoOutputFrameRateConversion =
    (eAVEncVideoOutputFrameRateConversion_Disable,
    eAVEncVideoOutputFrameRateConversion_Enable,
    eAVEncVideoOutputFrameRateConversion_Alias);

const
  // AVEncVideoPixelAspectRatio (UINT32 as UINT16/UNIT16) <---- You have WORD in the doc
  AVEncVideoPixelAspectRatio: TGUID = '{3cdc718f-b3e9-4eb6-a57f-cf1f1b321b87}';

  // AVEncVideoForceSourceScanType (UINT32)
  AVEncVideoForceSourceScanType
    : TGUID = '{1ef2065f-058a-4765-a4fc-8a864c103012}';

type
  eAVEncVideoSourceScanType = (eAVEncVideoSourceScan_Automatic,
    eAVEncVideoSourceScan_Interlaced, eAVEncVideoSourceScan_Progressive);

const
  // AVEncVideoNoOfFieldsToEncode (UINT64)
  AVEncVideoNoOfFieldsToEncode
    : TGUID = '{61e4bbe2-4ee0-40e7-80ab-51ddeebe6291}';

  // AVEncVideoNoOfFieldsToSkip (UINT64)
  AVEncVideoNoOfFieldsToSkip: TGUID = '{a97e1240-1427-4c16-a7f7-3dcfd8ba4cc5}';

  // AVEncVideoEncodeDimension (UINT32)
  AVEncVideoEncodeDimension: TGUID = '{1074df28-7e0f-47a4-a453-cdd73870f5ce}';

  // AVEncVideoEncodeOffsetOrigin (UINT32)
  AVEncVideoEncodeOffsetOrigin
    : TGUID = '{6bc098fe-a71a-4454-852e-4d2ddeb2cd24}';

  // AVEncVideoDisplayDimension (UINT32)
  AVEncVideoDisplayDimension: TGUID = '{de053668-f4ec-47a9-86d0-836770f0c1d5}';

  // AVEncVideoOutputScanType (UINT32)
  AVEncVideoOutputScanType: TGUID = '{460b5576-842e-49ab-a62d-b36f7312c9db}';

type
  eAVEncVideoOutputScanType = (eAVEncVideoOutputScan_Progressive,
    eAVEncVideoOutputScan_Interlaced, eAVEncVideoOutputScan_SameAsInput,
    eAVEncVideoOutputScan_Automatic);

const
  // AVEncVideoInverseTelecineEnable (BOOL)
  AVEncVideoInverseTelecineEnable
    : TGUID = '{2ea9098b-e76d-4ccd-a030-d3b889c1b64c}';

  // AVEncVideoInverseTelecineThreshold (UINT32)
  AVEncVideoInverseTelecineThreshold
    : TGUID = '{40247d84-e895-497f-b44c-b74560acfe27}';

  // AVEncVideoSourceFilmContent (UINT32)
  AVEncVideoSourceFilmContent: TGUID = '{1791c64b-ccfc-4827-a0ed-2557793b2b1c}';

type
  eAVEncVideoFilmContent = (eAVEncVideoFilmContent_VideoOnly,
    eAVEncVideoFilmContent_FilmOnly, eAVEncVideoFilmContent_Mixed);

const
  // AVEncVideoSourceIsBW (BOOL)
  AVEncVideoSourceIsBW: TGUID = '{42ffc49b-1812-4fdc-8d24-7054c521e6eb}';

  // AVEncVideoFieldSwap (BOOL)
  AVEncVideoFieldSwap: TGUID = '{fefd7569-4e0a-49f2-9f2b-360ea48c19a2}';

  // AVEncVideoInputChromaResolution (UINT32)
  // AVEncVideoOutputChromaSubsamplingFormat (UINT32)
  AVEncVideoInputChromaResolution
    : TGUID = '{bb0cec33-16f1-47b0-8a88-37815bee1739}';
  AVEncVideoOutputChromaResolution
    : TGUID = '{6097b4c9-7c1d-4e64-bfcc-9e9765318ae7}';

type
  eAVEncVideoChromaResolution = (eAVEncVideoChromaResolution_SameAsSource,
    eAVEncVideoChromaResolution_444, eAVEncVideoChromaResolution_422,
    eAVEncVideoChromaResolution_420, eAVEncVideoChromaResolution_411);

const
  // AVEncVideoInputChromaSubsampling (UINT32)
  // AVEncVideoOutputChromaSubsampling (UINT32)
  AVEncVideoInputChromaSubsampling
    : TGUID = '{a8e73a39-4435-4ec3-a6ea-98300f4b36f7}';
  AVEncVideoOutputChromaSubsampling
    : TGUID = '{fa561c6c-7d17-44f0-83c9-32ed12e96343}';

type
  eAVEncVideoChromaSubsampling =
    (eAVEncVideoChromaSubsamplingFormat_SameAsSource = 0,
    eAVEncVideoChromaSubsamplingFormat_ProgressiveChroma = 8,
    eAVEncVideoChromaSubsamplingFormat_Horizontally_Cosited = 4,
    eAVEncVideoChromaSubsamplingFormat_Vertically_Cosited = 2,
    eAVEncVideoChromaSubsamplingFormat_Vertically_AlignedChromaPlanes = 1);

const
  // AVEncVideoInputColorPrimaries (UINT32)
  // AVEncVideoOutputColorPrimaries (UINT32)
  AVEncVideoInputColorPrimaries
    : TGUID = '{c24d783f-7ce6-4278-90ab-28a4f1e5f86c}';
  AVEncVideoOutputColorPrimaries
    : TGUID = '{be95907c-9d04-4921-8985-a6d6d87d1a6c}';

type
  eAVEncVideoColorPrimaries = (eAVEncVideoColorPrimaries_SameAsSource = 0,
    eAVEncVideoColorPrimaries_Reserved = 1, eAVEncVideoColorPrimaries_BT709 = 2,
    eAVEncVideoColorPrimaries_BT470_2_SysM = 3,
    eAVEncVideoColorPrimaries_BT470_2_SysBG = 4,
    eAVEncVideoColorPrimaries_SMPTE170M = 5,
    eAVEncVideoColorPrimaries_SMPTE240M = 6,
    eAVEncVideoColorPrimaries_EBU3231 = 7,
    eAVEncVideoColorPrimaries_SMPTE_C = 8);

const
  // AVEncVideoInputColorTransferFunction (UINT32)
  // AVEncVideoOutputColorTransferFunction (UINT32)
  AVEncVideoInputColorTransferFunction
    : TGUID = '{8c056111-a9c3-4b08-a0a0-ce13f8a27c75}';
  AVEncVideoOutputColorTransferFunction
    : TGUID = '{4a7f884a-ea11-460d-bf57-b88bc75900de}';

type
  eAVEncVideoColorTransferFunction =
    (eAVEncVideoColorTransferFunction_SameAsSource,
    eAVEncVideoColorTransferFunction_10, // (Linear scRGB)
    eAVEncVideoColorTransferFunction_18, eAVEncVideoColorTransferFunction_20,
    eAVEncVideoColorTransferFunction_22,
    // (BT470-2 SysM)
    eAVEncVideoColorTransferFunction_22_709,
    // (BT709  SMPTE296M SMPTE170M BT470 SMPTE274M BT.1361)
    eAVEncVideoColorTransferFunction_22_240M, // (SMPTE240M interim 274M)
    eAVEncVideoColorTransferFunction_22_8bit_sRGB, // (sRGB)
    eAVEncVideoColorTransferFunction_28);

const

  // AVEncVideoInputColorTransferMatrix (UINT32)
  // AVEncVideoOutputColorTransferMatrix (UINT32)
  AVEncVideoInputColorTransferMatrix
    : TGUID = '{52ed68b9-72d5-4089-958d-f5405d55081c}';
  AVEncVideoOutputColorTransferMatrix
    : TGUID = '{a9b90444-af40-4310-8fbe-ed6d933f892b}';

type
  eAVEncVideoColorTransferMatrix = (eAVEncVideoColorTransferMatrix_SameAsSource,
    eAVEncVideoColorTransferMatrix_BT709, eAVEncVideoColorTransferMatrix_BT601,
    // (601 BT470-2 BB 170M)
    eAVEncVideoColorTransferMatrix_SMPTE240M);

const
  // AVEncVideoInputColorLighting (UINT32)
  // AVEncVideoOutputColorLighting (UINT32)
  AVEncVideoInputColorLighting
    : TGUID = '{46a99549-0015-4a45-9c30-1d5cfa258316}';
  AVEncVideoOutputColorLighting
    : TGUID = '{0e5aaac6-ace6-4c5c-998e-1a8c9c6c0f89}';

type
  eAVEncVideoColorLighting = (eAVEncVideoColorLighting_SameAsSource,
    eAVEncVideoColorLighting_Unknown, eAVEncVideoColorLighting_Bright,
    eAVEncVideoColorLighting_Office, eAVEncVideoColorLighting_Dim,
    eAVEncVideoColorLighting_Dark);

const
  // AVEncVideoInputColorNominalRange (UINT32)
  // AVEncVideoOutputColorNominalRange (UINT32)
  AVEncVideoInputColorNominalRange
    : TGUID = '{16cf25c6-a2a6-48e9-ae80-21aec41d427e}';
  AVEncVideoOutputColorNominalRange
    : TGUID = '{972835ed-87b5-4e95-9500-c73958566e54}';

type
  eAVEncVideoColorNominalRange = (eAVEncVideoColorNominalRange_SameAsSource,
    eAVEncVideoColorNominalRange_0_255,
    // (8 bit: 0..255 10 bit: 0..1023)
    eAVEncVideoColorNominalRange_16_235, // (16..235 64..940 (16*4...235*4)
    eAVEncVideoColorNominalRange_48_208 // (48..208)
    );

const
  // AVEncInputVideoSystem (UINT32)
  AVEncInputVideoSystem: TGUID = '{bede146d-b616-4dc7-92b2-f5d9fa9298f7}';

type
  eAVEncInputVideoSystem = (eAVEncInputVideoSystem_Unspecified,
    eAVEncInputVideoSystem_PAL, eAVEncInputVideoSystem_NTSC,
    eAVEncInputVideoSystem_SECAM, eAVEncInputVideoSystem_MAC,
    eAVEncInputVideoSystem_HDV, eAVEncInputVideoSystem_Component);

const
  // AVEncVideoHeaderDropFrame (UINT32)
  AVEncVideoHeaderDropFrame: TGUID = '{6ed9e124-7925-43fe-971b-e019f62222b4}';

  // AVEncVideoHeaderHours (UINT32)
  AVEncVideoHeaderHours: TGUID = '{2acc7702-e2da-4158-bf9b-88880129d740}';

  // AVEncVideoHeaderMinutes (UINT32)
  AVEncVideoHeaderMinutes: TGUID = '{dc1a99ce-0307-408b-880b-b8348ee8ca7f}';

  // AVEncVideoHeaderSeconds (UINT32)
  AVEncVideoHeaderSeconds: TGUID = '{4a2e1a05-a780-4f58-8120-9a449d69656b}';

  // AVEncVideoHeaderFrames (UINT32)
  AVEncVideoHeaderFrames: TGUID = '{afd5f567-5c1b-4adc-bdaf-735610381436}';

  // AVEncVideoDefaultUpperFieldDominant (BOOL)
  AVEncVideoDefaultUpperFieldDominant
    : TGUID = '{810167c4-0bc1-47ca-8fc2-57055a1474a5}';

  // AVEncVideoCBRMotionTradeoff (UINT32)
  AVEncVideoCBRMotionTradeoff: TGUID = '{0d49451e-18d5-4367-a4ef-3240df1693c4}';

  // AVEncVideoCodedVideoAccessUnitSize (UINT32)
  AVEncVideoCodedVideoAccessUnitSize
    : TGUID = '{b4b10c15-14a7-4ce8-b173-dc90a0b4fcdb}';

  // AVEncVideoMaxKeyframeDistance (UINT32)
  AVEncVideoMaxKeyframeDistance
    : TGUID = '{2987123a-ba93-4704-b489-ec1e5f25292c}';

  //
  // Common Post-Encode Video Statistical Parameters
  //

  // AVEncStatVideoOutputFrameRate (UINT32/UINT32)
  AVEncStatVideoOutputFrameRate
    : TGUID = '{be747849-9ab4-4a63-98fe-f143f04f8ee9}';

  // AVEncStatVideoCodedFrames (UINT32)
  AVEncStatVideoCodedFrames: TGUID = '{d47f8d61-6f5a-4a26-bb9f-cd9518462bcd}';

  // AVEncStatVideoTotalFrames (UINT32)
  AVEncStatVideoTotalFrames: TGUID = '{fdaa9916-119a-4222-9ad6-3f7cab99cc8b}';

  //
  // Common Audio Parameters
  //

  // AVEncAudioIntervalToEncode (UINT64)
  AVEncAudioIntervalToEncode: TGUID = '{866e4b4d-725a-467c-bb01-b496b23b25f9}';

  // AVEncAudioIntervalToSkip (UINT64)
  AVEncAudioIntervalToSkip: TGUID = '{88c15f94-c38c-4796-a9e8-96e967983f26}';

  // AVEncAudioDualMono (UINT32) - Read/Write
  // Some audio encoders can encode 2 channel input as 'dual mono'. Use this
  // property to set the appropriate field in the bitstream header to indicate that the
  // 2 channel bitstream is or isn't dual mono.
  // For encoding MPEG audio use the DualChannel option in AVEncMPACodingMode instead
  AVEncAudioDualMono: TGUID = '{3648126b-a3e8-4329-9b3a-5ce566a43bd3}';

type
  eAVEncAudioDualMono = (eAVEncAudioDualMono_SameAsInput,
    // As indicated by input media type
    eAVEncAudioDualMono_Off, // 2-ch output bitstream should not be dual mono
    eAVEncAudioDualMono_On // 2-ch output bitstream should be dual mono
    );

const
  // AVEncAudioMapDestChannel0..15 (UINT32)
  AVEncAudioMapDestChannel0: TGUID = '{bc5d0b60-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel1: TGUID = '{bc5d0b61-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel2: TGUID = '{bc5d0b62-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel3: TGUID = '{bc5d0b63-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel4: TGUID = '{bc5d0b64-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel5: TGUID = '{bc5d0b65-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel6: TGUID = '{bc5d0b66-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel7: TGUID = '{bc5d0b67-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel8: TGUID = '{bc5d0b68-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel9: TGUID = '{bc5d0b69-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel10: TGUID = '{bc5d0b6a-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel11: TGUID = '{bc5d0b6b-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel12: TGUID = '{bc5d0b6c-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel13: TGUID = '{bc5d0b6d-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel14: TGUID = '{bc5d0b6e-df6a-4e16-9803-b82007a30c8d}';
  AVEncAudioMapDestChannel15: TGUID = '{bc5d0b6f-df6a-4e16-9803-b82007a30c8d}';

  // AVEncAudioInputContent (UINT32) <---- You have type in the doc
  AVEncAudioInputContent: TGUID = '{3e226c2b-60b9-4a39-b00b-a7b40f70d566}';

type
  eAVEncAudioInputContent = (AVEncAudioInputContent_Unknown,
    AVEncAudioInputContent_Voice, AVEncAudioInputContent_Music);

  //
  // Common Post-Encode Audio Statistical Parameters
  //

const
  // AVEncStatAudioPeakPCMValue (UINT32)
  AVEncStatAudioPeakPCMValue: TGUID = '{dce7fd34-dc00-4c16-821b-35d9eb00fb1a}';

  // AVEncStatAudioAveragePCMValue (UINT32)
  AVEncStatAudioAveragePCMValue
    : TGUID = '{979272f8-d17f-4e32-bb73-4e731c68ba2d}';

  // AVEncStatAudioAverageBPS (UINT32)
  AVEncStatAudioAverageBPS: TGUID = '{ca6724db-7059-4351-8b43-f82198826a14}';

  //
  // MPEG Video Encoding Interface
  //

  //
  // MPV Encoder Specific Parameters
  //

  // AVEncMPVGOPSize (UINT32)
  AVEncMPVGOPSize: TGUID = '{95f31b26-95a4-41aa-9303-246a7fc6eef1}';

  // AVEncMPVGOPOpen (BOOL)
  AVEncMPVGOPOpen: TGUID = '{b1d5d4a6-3300-49b1-ae61-a09937ab0e49}';

  // AVEncMPVDefaultBPictureCount (UINT32)
  AVEncMPVDefaultBPictureCount
    : TGUID = '{8d390aac-dc5c-4200-b57f-814d04babab2}';

  // AVEncMPVProfile (UINT32) <---- You have GUID in the doc
  AVEncMPVProfile: TGUID = '{dabb534a-1d99-4284-975a-d90e2239baa1}';

type
  eAVEncMPVProfile = (eAVEncMPVProfile_unknown, eAVEncMPVProfile_Simple,
    eAVEncMPVProfile_Main, eAVEncMPVProfile_High, eAVEncMPVProfile_422);

const
  // AVEncMPVLevel (UINT32) <---- You have GUID in the doc
  AVEncMPVLevel: TGUID = '{6ee40c40-a60c-41ef-8f50-37c2249e2cb3}';

type
  eAVEncMPVLevel = (eAVEncMPVLevel_Low = 1, eAVEncMPVLevel_Main = 2,
    eAVEncMPVLevel_High1440 = 3, eAVEncMPVLevel_High = 4);

const
  // AVEncMPVFrameFieldMode (UINT32)
  AVEncMPVFrameFieldMode: TGUID = '{acb5de96-7b93-4c2f-8825-b0295fa93bf4}';

type
  eAVEncMPVFrameFieldMode = (eAVEncMPVFrameFieldMode_FieldMode,
    eAVEncMPVFrameFieldMode_FrameMode);

  //
  // Advanced MPV Encoder Specific Parameters
  //

const
  // AVEncMPVAddSeqEndCode (BOOL)
  AVEncMPVAddSeqEndCode: TGUID = '{a823178f-57df-4c7a-b8fd-e5ec8887708d}';

  // AVEncMPVGOPSInSeq (UINT32)
  AVEncMPVGOPSInSeq: TGUID = '{993410d4-2691-4192-9978-98dc2603669f}';

  // AVEncMPVUseConcealmentMotionVectors (BOOL)
  AVEncMPVUseConcealmentMotionVectors
    : TGUID = '{ec770cf3-6908-4b4b-aa30-7fb986214fea}';
  // AVEncMPVSceneDetection (UINT32)
  AVEncMPVSceneDetection: TGUID = '{552799f1-db4c-405b-8a3a-c93f2d0674dc}';

type
  eAVEncMPVSceneDetection = (eAVEncMPVSceneDetection_None,
    eAVEncMPVSceneDetection_InsertIPicture, eAVEncMPVSceneDetection_StartNewGOP,
    eAVEncMPVSceneDetection_StartNewLocatableGOP);

const
  // AVEncMPVGenerateHeaderSeqExt (BOOL)
  AVEncMPVGenerateHeaderSeqExt
    : TGUID = '{d5e78611-082d-4e6b-98af-0f51ab139222}';

  // AVEncMPVGenerateHeaderSeqDispExt (BOOL)
  AVEncMPVGenerateHeaderSeqDispExt
    : TGUID = '{6437aa6f-5a3c-4de9-8a16-53d9c4ad326f}';

  // AVEncMPVGenerateHeaderPicExt (BOOL)
  AVEncMPVGenerateHeaderPicExt
    : TGUID = '{1b8464ab-944f-45f0-b74e-3a58dad11f37}';

  // AVEncMPVGenerateHeaderPicDispExt (BOOL)
  AVEncMPVGenerateHeaderPicDispExt
    : TGUID = '{c6412f84-c03f-4f40-a00c-4293df8395bb}';

  // AVEncMPVGenerateHeaderSeqScaleExt (BOOL)
  AVEncMPVGenerateHeaderSeqScaleExt
    : TGUID = '{0722d62f-dd59-4a86-9cd5-644f8e2653d8}';

  // AVEncMPVScanPattern (UINT32)
  AVEncMPVScanPattern: TGUID = '{7f8a478e-7bbb-4ae2-b2fc-96d17fc4a2d6}';

type
  eAVEncMPVScanPattern = (eAVEncMPVScanPattern_Auto,
    eAVEncMPVScanPattern_ZigZagScan, eAVEncMPVScanPattern_AlternateScan);

const
  // AVEncMPVIntraDCPrecision (UINT32)
  AVEncMPVIntraDCPrecision: TGUID = '{a0116151-cbc8-4af3-97dc-d00cceb82d79}';

  // AVEncMPVQScaleType (UINT32)
  AVEncMPVQScaleType: TGUID = '{2b79ebb7-f484-4af7-bb58-a2a188c5cbbe}';

type
  eAVEncMPVQScaleType = (eAVEncMPVQScaleType_Auto, eAVEncMPVQScaleType_Linear,
    eAVEncMPVQScaleType_NonLinear);

const
  // AVEncMPVIntraVLCTable (UINT32)
  AVEncMPVIntraVLCTable: TGUID = '{a2b83ff5-1a99-405a-af95-c5997d558d3a}';

type
  eAVEncMPVIntraVLCTable = (eAVEncMPVIntraVLCTable_Auto,
    eAVEncMPVIntraVLCTable_MPEG1, eAVEncMPVIntraVLCTable_Alternate);

const
  // AVEncMPVQuantMatrixIntra (BYTE[64] encoded as a string of 128 hex digits)
  AVEncMPVQuantMatrixIntra: TGUID = '{9bea04f3-6621-442c-8ba1-3ac378979698}';

  // AVEncMPVQuantMatrixNonIntra (BYTE[64] encoded as a string of 128 hex digits)
  AVEncMPVQuantMatrixNonIntra: TGUID = '{87f441d8-0997-4beb-a08e-8573d409cf75}';

  // AVEncMPVQuantMatrixChromaIntra (BYTE[64] encoded as a string of 128 hex digits)
  AVEncMPVQuantMatrixChromaIntra
    : TGUID = '{9eb9ecd4-018d-4ffd-8f2d-39e49f07b17a}';

  // AVEncMPVQuantMatrixChromaNonIntra (BYTE[64] encoded as a string of 128 hex digits)
  AVEncMPVQuantMatrixChromaNonIntra
    : TGUID = '{1415b6b1-362a-4338-ba9a-1ef58703c05b}';

  //
  // MPEG1 Audio Encoding Interface
  //

  //
  // MPEG1 Audio Specific Parameters
  //

  // AVEncMPALayer (UINT)
  AVEncMPALayer: TGUID = '{9d377230-f91b-453d-9ce0-78445414c22d}';

type
  eAVEncMPALayer = (eAVEncMPALayer_1 = 1, eAVEncMPALayer_2 = 2,
    eAVEncMPALayer_3 = 3);

const
  // AVEncMPACodingMode (UINT)
  AVEncMPACodingMode: TGUID = '{b16ade03-4b93-43d7-a550-90b4fe224537}';

type
  eAVEncMPACodingMode = (eAVEncMPACodingMode_Mono, eAVEncMPACodingMode_Stereo,
    eAVEncMPACodingMode_DualChannel, eAVEncMPACodingMode_JointStereo,
    eAVEncMPACodingMode_Surround);

const
  // AVEncMPACopyright (BOOL) - default state to encode into the stream (may be overridden by input)
  // 1 (true)  - copyright protected
  // 0 (false) - not copyright protected
  AVEncMPACopyright: TGUID = '{a6ae762a-d0a9-4454-b8ef-f2dbeefdd3bd}';

  // AVEncMPAOriginalBitstream (BOOL) - default value to encode into the stream (may be overridden by input)
  // 1 (true)  - for original bitstream
  // 0 (false) - for copy bitstream
  AVEncMPAOriginalBitstream: TGUID = '{3cfb7855-9cc9-47ff-b829-b36786c92346}';

  // AVEncMPAEnableRedundancyProtection (BOOL)
  // 1 (true)  -  Redundancy should be added to facilitate error detection and concealment (CRC)
  // 0 (false) -  No redundancy should be added
  AVEncMPAEnableRedundancyProtection
    : TGUID = '{5e54b09e-b2e7-4973-a89b-0b3650a3beda}';

  // AVEncMPAPrivateUserBit (UINT) - User data bit value to encode in the stream
  AVEncMPAPrivateUserBit: TGUID = '{afa505ce-c1e3-4e3d-851b-61b700e5e6cc}';

  // AVEncMPAEmphasisType (UINT)
  // Indicates type of de-emphasis filter to be used
  AVEncMPAEmphasisType: TGUID = '{2d59fcda-bf4e-4ed6-b5df-5b03b36b0a1f}';

type
  eAVEncMPAEmphasisType = (eAVEncMPAEmphasisType_None,
    eAVEncMPAEmphasisType_50_15, eAVEncMPAEmphasisType_Reserved,
    eAVEncMPAEmphasisType_CCITT_J17);

  //
  // Dolby Digital(TM) Audio Encoding Interface
  //

  //
  // Dolby Digital(TM) Audio Specific Parameters
  //

const
  // AVEncDDService (UINT)
  AVEncDDService: TGUID = '{d2e1bec7-5172-4d2a-a50e-2f3b82b1ddf8}';

type
  eAVEncDDService = (eAVEncDDService_CM, // (Main Service: Complete Main)
    eAVEncDDService_ME, // (Main Service: Music and Effects (ME))
    eAVEncDDService_VI, // (Associated Service: Visually-Impaired (VI)
    eAVEncDDService_HI, // (Associated Service: Hearing-Impaired (HI))
    eAVEncDDService_D, // (Associated Service: Dialog (D))
    eAVEncDDService_C, // (Associated Service: Commentary (C))
    eAVEncDDService_E, // (Associated Service: Emergency (E))
    eAVEncDDService_VO // (Associated Service: Voice Over (VO) / Karaoke)
    );

const
  // AVEncDDDialogNormalization (UINT32)
  AVEncDDDialogNormalization: TGUID = '{d7055acf-f125-437d-a704-79c79f0404a8}';

  // AVEncDDCentreDownMixLevel (UINT32)
  AVEncDDCentreDownMixLevel: TGUID = '{e285072c-c958-4a81-afd2-e5e0daf1b148}';

  // AVEncDDSurroundDownMixLevel (UINT32)
  AVEncDDSurroundDownMixLevel: TGUID = '{7b20d6e5-0bcf-4273-a487-506b047997e9}';

  // AVEncDDProductionInfoExists (BOOL)
  AVEncDDProductionInfoExists: TGUID = '{b0b7fe5f-b6ab-4f40-964d-8d91f17c19e8}';

  // AVEncDDProductionRoomType (UINT32)
  AVEncDDProductionRoomType: TGUID = '{dad7ad60-23d8-4ab7-a284-556986d8a6fe}';

type
  eAVEncDDProductionRoomType = (eAVEncDDProductionRoomType_NotIndicated,
    eAVEncDDProductionRoomType_Large, eAVEncDDProductionRoomType_Small);

const
  // AVEncDDProductionMixLevel (UINT32)
  AVEncDDProductionMixLevel: TGUID = '{301d103a-cbf9-4776-8899-7c15b461ab26}';

  // AVEncDDCopyright (BOOL)
  AVEncDDCopyright: TGUID = '{8694f076-cd75-481d-a5c6-a904dcc828f0}';

  // AVEncDDOriginalBitstream (BOOL)
  AVEncDDOriginalBitstream: TGUID = '{966ae800-5bd3-4ff9-95b9-d30566273856}';

  // AVEncDDDigitalDeemphasis (BOOL)
  AVEncDDDigitalDeemphasis: TGUID = '{e024a2c2-947c-45ac-87d8-f1030c5c0082}';

  // AVEncDDDCHighPassFilter (BOOL)
  AVEncDDDCHighPassFilter: TGUID = '{9565239f-861c-4ac8-bfda-e00cb4db8548}';

  // AVEncDDChannelBWLowPassFilter (BOOL)
  AVEncDDChannelBWLowPassFilter
    : TGUID = '{e197821d-d2e7-43e2-ad2c-00582f518545}';

  // AVEncDDLFELowPassFilter (BOOL)
  AVEncDDLFELowPassFilter: TGUID = '{d3b80f6f-9d15-45e5-91be-019c3fab1f01}';

  // AVEncDDSurround90DegreeePhaseShift (BOOL)
  AVEncDDSurround90DegreeePhaseShift
    : TGUID = '{25ecec9d-3553-42c0-bb56-d25792104f80}';

  // AVEncDDSurround3dBAttenuation (BOOL)
  AVEncDDSurround3dBAttenuation
    : TGUID = '{4d43b99d-31e2-48b9-bf2e-5cbf1a572784}';

  // AVEncDDDynamicRangeCompressionControl (UINT32)
  AVEncDDDynamicRangeCompressionControl
    : TGUID = '{cfc2ff6d-79b8-4b8d-a8aa-a0c9bd1c2940}';

type
  eAVEncDDDynamicRangeCompressionControl =
    (eAVEncDDDynamicRangeCompressionControl_None,
    eAVEncDDDynamicRangeCompressionControl_FilmStandard,
    eAVEncDDDynamicRangeCompressionControl_FilmLight,
    eAVEncDDDynamicRangeCompressionControl_MusicStandard,
    eAVEncDDDynamicRangeCompressionControl_MusicLight,
    eAVEncDDDynamicRangeCompressionControl_Speech);

const
  // AVEncDDRFPreEmphasisFilter (BOOL)
  AVEncDDRFPreEmphasisFilter: TGUID = '{21af44c0-244e-4f3d-a2cc-3d3068b2e73f}';

  // AVEncDDSurroundExMode (UINT32)
  AVEncDDSurroundExMode: TGUID = '{91607cee-dbdd-4eb6-bca2-aadfafa3dd68}';

type
  eAVEncDDSurroundExMode = (eAVEncDDSurroundExMode_NotIndicated,
    eAVEncDDSurroundExMode_No, eAVEncDDSurroundExMode_Yes);

const
  // AVEncDDPreferredStereoDownMixMode (UINT32)
  AVEncDDPreferredStereoDownMixMode
    : TGUID = '{7f4e6b31-9185-403d-b0a2-763743e6f063}';

type
  eAVEncDDPreferredStereoDownMixMode = (eAVEncDDPreferredStereoDownMixMode_LtRt,
    eAVEncDDPreferredStereoDownMixMode_LoRo);

const
  // AVEncDDLtRtCenterMixLvl_x10 (INT32)
  AVEncDDLtRtCenterMixLvl_x10: TGUID = '{dca128a2-491f-4600-b2da-76e3344b4197}';

  // AVEncDDLtRtSurroundMixLvl_x10 (INT32)
  AVEncDDLtRtSurroundMixLvl_x10
    : TGUID = '{212246c7-3d2c-4dfa-bc21-652a9098690d}';

  // AVEncDDLoRoCenterMixLvl (INT32)
  AVEncDDLoRoCenterMixLvl_x10: TGUID = '{1cfba222-25b3-4bf4-9bfd-e7111267858c}';

  // AVEncDDLoRoSurroundMixLvl_x10 (INT32)
  AVEncDDLoRoSurroundMixLvl_x10
    : TGUID = '{e725cff6-eb56-40c7-8450-2b9367e91555}';

  // AVEncDDAtoDConverterType (UINT32)
  AVEncDDAtoDConverterType: TGUID = '{719f9612-81a1-47e0-9a05-d94ad5fca948}';

type
  eAVEncDDAtoDConverterType = (eAVEncDDAtoDConverterType_Standard,
    eAVEncDDAtoDConverterType_HDCD);

const
  // AVEncDDHeadphoneMode (UINT32)
  AVEncDDHeadphoneMode: TGUID = '{4052dbec-52f5-42f5-9b00-d134b1341b9d}';

type
  eAVEncDDHeadphoneMode = (eAVEncDDHeadphoneMode_NotIndicated,
    eAVEncDDHeadphoneMode_NotEncoded, eAVEncDDHeadphoneMode_Encoded);

  //
  // WMV Video Encoding Interface
  //

  //
  // WMV Video Specific Parameters
  //

const
  // AVEncWMVKeyFrameDistance (UINT32)
  AVEncWMVKeyFrameDistance: TGUID = '{5569055e-e268-4771-b83e-9555ea28aed3}';

  // AVEncWMVInterlacedEncoding (UINT32)
  AVEncWMVInterlacedEncoding: TGUID = '{e3d00f8a-c6f5-4e14-a588-0ec87a726f9b}';

  // AVEncWMVDecoderComplexity (UINT32)
  AVEncWMVDecoderComplexity: TGUID = '{f32c0dab-f3cb-4217-b79f-8762768b5f67}';

  // AVEncWMVHasKeyFrameBufferLevelMarker (BOOL)
  AVEncWMVKeyFrameBufferLevelMarker
    : TGUID = '{51ff1115-33ac-426c-a1b1-09321bdf96b4}';

  // AVEncWMVProduceDummyFrames (UINT32)
  AVEncWMVProduceDummyFrames: TGUID = '{d669d001-183c-42e3-a3ca-2f4586d2396c}';

  //
  // WMV Post-Encode Statistical Parameters
  //

  // AVEncStatWMVCBAvg (UINT32/UINT32)
  AVEncStatWMVCBAvg: TGUID = '{6aa6229f-d602-4b9d-b68c-c1ad78884bef}';

  // AVEncStatWMVCBMax (UINT32/UINT32)
  AVEncStatWMVCBMax: TGUID = '{e976bef8-00fe-44b4-b625-8f238bc03499}';

  // AVEncStatWMVDecoderComplexityProfile (UINT32)
  AVEncStatWMVDecoderComplexityProfile
    : TGUID = '{89e69fc3-0f9b-436c-974a-df821227c90d}';

  // AVEncStatMPVSkippedEmptyFrames (UINT32)
  AVEncStatMPVSkippedEmptyFrames
    : TGUID = '{32195fd3-590d-4812-a7ed-6d639a1f9711}';

  //
  // MPEG1/2 Multiplexer Interfaces
  //

  //
  // MPEG1/2 Packetizer Interface
  //

  // Shared with Mux:
  // AVEncMP12MuxEarliestPTS (UINT32)
  // AVEncMP12MuxLargestPacketSize (UINT32)
  // AVEncMP12MuxSysSTDBufferBound (UINT32)

  // AVEncMP12PktzSTDBuffer (UINT32)
  AVEncMP12PktzSTDBuffer: TGUID = '{0b751bd0-819e-478c-9435-75208926b377}';

  // AVEncMP12PktzStreamID (UINT32)
  AVEncMP12PktzStreamID: TGUID = '{c834d038-f5e8-4408-9b60-88f36493fedf}';

  // AVEncMP12PktzInitialPTS (UINT32)
  AVEncMP12PktzInitialPTS: TGUID = '{2a4f2065-9a63-4d20-ae22-0a1bc896a315}';

  // AVEncMP12PktzPacketSize (UINT32)
  AVEncMP12PktzPacketSize: TGUID = '{ab71347a-1332-4dde-a0e5-ccf7da8a0f22}';

  // AVEncMP12PktzCopyright (BOOL)
  AVEncMP12PktzCopyright: TGUID = '{c8f4b0c1-094c-43c7-8e68-a595405a6ef8}';

  // AVEncMP12PktzOriginal (BOOL)
  AVEncMP12PktzOriginal: TGUID = '{6b178416-31b9-4964-94cb-6bff866cdf83}';

  //
  // MPEG1/2 Multiplexer Interface
  //

  // AVEncMP12MuxPacketOverhead (UINT32)
  AVEncMP12MuxPacketOverhead: TGUID = '{e40bd720-3955-4453-acf9-b79132a38fa0}';

  // AVEncMP12MuxNumStreams (UINT32)
  AVEncMP12MuxNumStreams: TGUID = '{f7164a41-dced-4659-a8f2-fb693f2a4cd0}';

  // AVEncMP12MuxEarliestPTS (UINT32)
  AVEncMP12MuxEarliestPTS: TGUID = '{157232b6-f809-474e-9464-a7f93014a817}';

  // AVEncMP12MuxLargestPacketSize (UINT32)
  AVEncMP12MuxLargestPacketSize
    : TGUID = '{35ceb711-f461-4b92-a4ef-17b6841ed254}';

  // AVEncMP12MuxInitialSCR (UINT32)
  AVEncMP12MuxInitialSCR: TGUID = '{3433ad21-1b91-4a0b-b190-2b77063b63a4}';

  // AVEncMP12MuxMuxRate (UINT32)
  AVEncMP12MuxMuxRate: TGUID = '{ee047c72-4bdb-4a9d-8e21-41926c823da7}';

  // AVEncMP12MuxPackSize (UINT32)
  AVEncMP12MuxPackSize: TGUID = '{f916053a-1ce8-4faf-aa0b-ba31c80034b8}';

  // AVEncMP12MuxSysSTDBufferBound (UINT32)
  AVEncMP12MuxSysSTDBufferBound
    : TGUID = '{35746903-b545-43e7-bb35-c5e0a7d5093c}';

  // AVEncMP12MuxSysRateBound (UINT32)
  AVEncMP12MuxSysRateBound: TGUID = '{05f0428a-ee30-489d-ae28-205c72446710}';

  // AVEncMP12MuxTargetPacketizer (UINT32)
  AVEncMP12MuxTargetPacketizer
    : TGUID = '{d862212a-2015-45dd-9a32-1b3aa88205a0}';

  // AVEncMP12MuxSysFixed (UINT32)
  AVEncMP12MuxSysFixed: TGUID = '{cefb987e-894f-452e-8f89-a4ef8cec063a}';

  // AVEncMP12MuxSysCSPS (UINT32)
  AVEncMP12MuxSysCSPS: TGUID = '{7952ff45-9c0d-4822-bc82-8ad772e02993}';

  // AVEncMP12MuxSysVideoLock (BOOL)
  AVEncMP12MuxSysVideoLock: TGUID = '{b8296408-2430-4d37-a2a1-95b3e435a91d}';

  // AVEncMP12MuxSysAudioLock (BOOL)
  AVEncMP12MuxSysAudioLock: TGUID = '{0fbb5752-1d43-47bf-bd79-f2293d8ce337}';

  // AVEncMP12MuxDVDNavPacks (BOOL)
  AVEncMP12MuxDVDNavPacks: TGUID = '{c7607ced-8cf1-4a99-83a1-ee5461be3574}';

  //
  // Decoding Interface
  //

  // format values are GUIDs as VARIANT BSTRs
  AVDecCommonInputFormat: TGUID = '{E5005239-BD89-4be3-9C0F-5DDE317988CC}';
  AVDecCommonOutputFormat: TGUID = '{3c790028-c0ce-4256-b1a2-1b0fc8b1dcdc}';

  // AVDecCommonMeanBitRate - Mean bitrate in mbits/sec (UINT32)
  AVDecCommonMeanBitRate: TGUID = '{59488217-007A-4f7a-8E41-5C48B1EAC5C6}';
  // AVDecCommonMeanBitRateInterval - Mean bitrate interval (in 100ns) (UINT64)
  AVDecCommonMeanBitRateInterval
    : TGUID = '{0EE437C6-38A7-4c5c-944C-68AB42116B85}';

  //
  // Audio Decoding Interface
  //

  // Value GUIDS
  // The following 6 GUIDs are values of the AVDecCommonOutputFormat property
  //
  // Stereo PCM output using matrix-encoded stereo down mix (aka Lt/Rt)
  GUID_AVDecAudioOutputFormat_PCM_Stereo_MatrixEncoded
    : TGUID = '{696E1D30-548F-4036-825F-7026C60011BD}';
  //
  // Regular PCM output (any number of channels)
  GUID_AVDecAudioOutputFormat_PCM
    : TGUID = '{696E1D31-548F-4036-825F-7026C60011BD}';
  //
  // SPDIF PCM (IEC 60958) stereo output. Type of stereo down mix should
  // be specified by the application.
  GUID_AVDecAudioOutputFormat_SPDIF_PCM
    : TGUID = '{696E1D32-548F-4036-825F-7026C60011BD}';
  //
  // SPDIF bitstream (IEC 61937) output such as AC3 MPEG or DTS.
  GUID_AVDecAudioOutputFormat_SPDIF_Bitstream
    : TGUID = '{696E1D33-548F-4036-825F-7026C60011BD}';
  //
  // Stereo PCM output using regular stereo down mix (aka Lo/Ro)
  GUID_AVDecAudioOutputFormat_PCM_Headphones
    : TGUID = '{696E1D34-548F-4036-825F-7026C60011BD}';

  // Stereo PCM output using automatic selection of stereo down mix
  // mode (Lo/Ro or Lt/Rt). Use this when the input stream includes
  // information about the preferred downmix mode (such as Annex D of AC3).
  // Default down mix mode should be specified by the application.
  GUID_AVDecAudioOutputFormat_PCM_Stereo_Auto
    : TGUID = '{696E1D35-548F-4036-825F-7026C60011BD}';

  //
  // Video Decoder properties
  //

  // AVDecVideoImageSize (UINT32) - High UINT16 width low UINT16 height
  AVDecVideoImageSize: TGUID = '{5EE5747C-6801-4cab-AAF1-6248FA841BA4}';

  // AVDecVideoPixelAspectRatio (UINT32 as UINT16/UNIT16) - High UINT16 width low UINT16 height
  AVDecVideoPixelAspectRatio: TGUID = '{B0CF8245-F32D-41df-B02C-87BD304D12AB}';

  // AVDecVideoInputScanType (UINT32)
  AVDecVideoInputScanType: TGUID = '{38477E1F-0EA7-42cd-8CD1-130CED57C580}';

type
  eAVDecVideoInputScanType = (eAVDecVideoInputScan_Unknown,
    eAVDecVideoInputScan_Progressive,
    eAVDecVideoInputScan_Interlaced_UpperFieldFirst,
    eAVDecVideoInputScan_Interlaced_LowerFieldFirst);

  //
  // Audio Decoder properties
  //

const
  GUID_AVDecAudioInputWMA: TGUID = '{C95E8DCF-4058-4204-8C42-CB24D91E4B9B}';
  GUID_AVDecAudioInputWMAPro: TGUID = '{0128B7C7-DA72-4fe3-BEF8-5C52E3557704}';
  GUID_AVDecAudioInputDolby: TGUID = '{8E4228A0-F000-4e0b-8F54-AB8D24AD61A2}';
  GUID_AVDecAudioInputDTS: TGUID = '{600BC0CA-6A1F-4e91-B241-1BBEB1CB19E0}';
  GUID_AVDecAudioInputPCM: TGUID = '{F2421DA5-BBB4-4cd5-A996-933C6B5D1347}';
  GUID_AVDecAudioInputMPEG: TGUID = '{91106F36-02C5-4f75-9719-3B7ABF75E1F6}';

  // AVDecAudioDualMono (UINT32) - Read only
  // The input bitstream header might have a field indicating whether the 2-ch bitstream
  // is dual mono or not. Use this property to read this field.
  // If it's dual mono the application can set AVDecAudioDualMonoReproMode to determine
  // one of 4 reproduction modes
  AVDecAudioDualMono: TGUID = '{4a52cda8-30f8-4216-be0f-ba0b2025921d}';

type
  eAVDecAudioDualMono = (eAVDecAudioDualMono_IsNotDualMono,
    // 2-ch bitstream input is not dual mono
    eAVDecAudioDualMono_IsDualMono, // 2-ch bitstream input is dual mono
    eAVDecAudioDualMono_UnSpecified // There is no indication in the bitstream
    );

  // AVDecAudioDualMonoReproMode (UINT32)
  // Reproduction modes for programs containing two independent mono channels (Ch1 & Ch2).
  // In case of 2-ch input the decoder should get AVDecAudioDualMono to check if the input
  // is regular stereo or dual mono. If dual mono the application can ask the user to set the playback
  // mode by setting AVDecAudioDualReproMonoMode. If output is not stereo use AVDecDDMatrixDecodingMode or
  // equivalent.

const
  AVDecAudioDualMonoReproMode: TGUID = '{a5106186-cc94-4bc9-8cd9-aa2f61f6807e}';

type
  eAVDecAudioDualMonoReproMode = (eAVDecAudioDualMonoReproMode_STEREO,
    // Ch1+Ch2 for mono output (Ch1 left     Ch2 right) for stereo output
    eAVDecAudioDualMonoReproMode_LEFT_MONO,
    // Ch1 for mono output     (Ch1 left     Ch1 right) for stereo output
    eAVDecAudioDualMonoReproMode_RIGHT_MONO,
    // Ch2 for mono output     (Ch2 left     Ch2 right) for stereo output
    eAVDecAudioDualMonoReproMode_MIX_MONO
    // Ch1+Ch2 for mono output (Ch1+Ch2 left Ch1+Ch2 right) for stereo output
    );

  //
  // Audio Common Properties
  //

const
  // AVAudioChannelCount (UINT32)
  // Total number of audio channels including LFE if it exists.
  AVAudioChannelCount: TGUID = '{1d3583c4-1583-474e-b71a-5ee463c198e4}';

  // AVAudioChannelConfig (UINT32)
  // A bit-wise OR of any number of type values specified by eAVAudioChannelConfig
  AVAudioChannelConfig: TGUID = '{17f89cb3-c38d-4368-9ede-63b94d177f9f}';

  // typeerated values for  AVAudioChannelConfig are identical
  // to the speaker positions defined in ksmedia.h and used
  // in WAVE_FORMAT_EXTENSIBLE. Configurations for 5.1 and
  // 7.1 channels should be identical to KSAUDIO_SPEAKER_5POINT1_SURROUND
  // and KSAUDIO_SPEAKER_7POINT1_SURROUND in ksmedia.h. This means:
  // 5.1 ch -> LOW_FREQUENCY | FRONT_LEFT | FRONT_RIGHT | FRONT_CENTER | SIDE_LEFT | SIDE_RIGHT
  // 7.1 ch -> LOW_FREQUENCY | FRONT_LEFT | FRONT_RIGHT | FRONT_CENTER | SIDE_LEFT | SIDE_RIGHT | BACK_LEFT | BACK_RIGHT
  //
type
  eAVAudioChannelConfig = (eAVAudioChannelConfig_FRONT_LEFT = $1,
    eAVAudioChannelConfig_FRONT_RIGHT = $2,
    eAVAudioChannelConfig_FRONT_CENTER = $4,
    eAVAudioChannelConfig_LOW_FREQUENCY = $8, // aka LFE
    eAVAudioChannelConfig_BACK_LEFT = $10,
    eAVAudioChannelConfig_BACK_RIGHT = $20,
    eAVAudioChannelConfig_FRONT_LEFT_OF_CENTER = $40,
    eAVAudioChannelConfig_FRONT_RIGHT_OF_CENTER = $80,
    eAVAudioChannelConfig_BACK_CENTER = $100, // aka Mono Surround
    eAVAudioChannelConfig_SIDE_LEFT = $200, // aka Left Surround
    eAVAudioChannelConfig_SIDE_RIGHT = $400, // aka Right Surround
    eAVAudioChannelConfig_TOP_CENTER = $800,
    eAVAudioChannelConfig_TOP_FRONT_LEFT = $1000,
    eAVAudioChannelConfig_TOP_FRONT_CENTER = $2000,
    eAVAudioChannelConfig_TOP_FRONT_RIGHT = $4000,
    eAVAudioChannelConfig_TOP_BACK_LEFT = $8000,
    eAVAudioChannelConfig_TOP_BACK_CENTER = $10000,
    eAVAudioChannelConfig_TOP_BACK_RIGHT = $20000);

const
  // AVAudioSampleRate (UINT32)
  // In samples per second (Hz)
  AVAudioSampleRate: TGUID = '{971d2723-1acb-42e7-855c-520a4b70a5f2}';

  //
  // Dolby Digital(TM) Audio Specific Parameters
  //

  // AVDDSurroundMode (UINT32) common to encoder/decoder
  AVDDSurroundMode: TGUID = '{99f2f386-98d1-4452-a163-abc78a6eb770}';

type
  eAVDDSurroundMode = (eAVDDSurroundMode_NotIndicated, eAVDDSurroundMode_No,
    eAVDDSurroundMode_Yes);

const
  // AVDecDDOperationalMode (UINT32)
  AVDecDDOperationalMode: TGUID = '{d6d6c6d1-064e-4fdd-a40e-3ecbfcb7ebd0}';

type
  eAVDecDDOperationalMode = (eAVDecDDOperationalMode_NONE,
    eAVDecDDOperationalMode_LINE,
    // Dialnorm enabled dialogue at -31dBFS dynrng used high/low scaling allowed
    eAVDecDDOperationalMode_RF,
    // Dialnorm enabled dialogue at -20dBFS dynrng & compr used high/low scaling NOT allowed (always fully compressed)
    eAVDecDDOperationalMode_CUSTOM0,
    // Analog dialnorm (dialogue normalization not part of the decoder)
    eAVDecDDOperationalMode_CUSTOM1
    // Digital dialnorm (dialogue normalization is part of the decoder)
    );

const
  // AVDecDDMatrixDecodingMode(UINT32)
  // A ProLogic decoder has a built-in auto-detection feature. When the Dolby Digital decoder
  // is set to the 6-channel output configuration and it is fed a 2/0 bit stream to decode it can
  // do one of the following:
  // a) decode the bit stream and output it on the two front channels (eAVDecDDMatrixDecodingMode_OFF)
  // b) decode the bit stream followed by ProLogic decoding to create 6-channels (eAVDecDDMatrixDecodingMode_ON).
  // c) the decoder will look at the Surround bit ('dsurmod') in the bit stream to determine whether
  // apply ProLogic decoding or not (eAVDecDDMatrixDecodingMode_AUTO).
  AVDecDDMatrixDecodingMode: TGUID = '{ddc811a5-04ed-4bf3-a0ca-d00449f9355f}';

type
  eAVDecDDMatrixDecodingMode = (eAVDecDDMatrixDecodingMode_OFF,
    eAVDecDDMatrixDecodingMode_ON, eAVDecDDMatrixDecodingMode_AUTO);

const
  // AVDecDDDynamicRangeScaleHigh (UINT32)
  // Indicates what fraction of the dynamic range compression
  // to apply. Relevant for negative values of dynrng only.
  // Linear range 0-100 where:
  // 0 - No dynamic range compression (preserve full dynamic range)
  // 100 - Apply full dynamic range compression
  AVDecDDDynamicRangeScaleHigh
    : TGUID = '{50196c21-1f33-4af5-b296-11426d6c8789}';

  // AVDecDDDynamicRangeScaleLow (UINT32)
  // Indicates what fraction of the dynamic range compression
  // to apply. Relevant for positive values of dynrng only.
  // Linear range 0-100 where:
  // 0 - No dynamic range compression (preserve full dynamic range)
  // 100 - Apply full dynamic range compression
  AVDecDDDynamicRangeScaleLow: TGUID = '{044e62e4-11a5-42d5-a3b2-3bb2c7c2d7cf}';

type
  /// <summary>
  /// VP8 deadline.
  /// </summary>
  VP8Deadline = (
    /// <summary>
    /// Best quality.
    /// </summary>
    kDeadlineBestQuality = 0,

    /// <summary>
    /// Good quality.
    /// </summary>
    kDeadlineGoodQuality = $F4240,

    /// <summary>
    /// Real-time.
    /// </summary>
    kDeadlineRealtime = 1);

  /// <summary>
  /// VP8 end usage.
  /// </summary>
  VP8EndUsage = (
    /// <summary>
    /// CBR.
    /// </summary>
    kEndUsageCBR = 1,

    /// <summary>
    /// Default.
    /// </summary>
    kEndUsageDefault = -1,

    /// <summary>
    /// VBR.
    /// </summary>
    kEndUsageVBR = 0);

  /// <summary>
  /// Keyframe placement mode.
  /// Specifies whether keyframes are placed automatically
  /// by the encoder, or whether this behavior is disabled.
  /// </summary>
  VP8KeyframeMode = (
    /// <summary>
    /// Encoder determines optimal placement.
    /// </summary>
    kKeyframeModeAuto = 1,

    /// <summary>
    /// Use encoder default.
    /// </summary>
    kKeyframeModeDefault = -1,

    /// <summary>
    /// Encoder does not place keyframes.
    /// </summary>
    kKeyframeModeDisabled = 0);

  /// <summary>
  /// VP8 pass mode.
  /// </summary>
  VP8PassMode = (
    /// <summary>
    /// One pass.
    /// </summary>
    kPassModeOnePass,

    /// <summary>
    /// First pass.
    /// </summary>
    kPassModeFirstPass,

    /// <summary>
    /// Last pass.
    /// </summary>
    kPassModeLastPass);

const
  IID_IVP8Encoder: TGUID = '{ED3110FE-5211-11DF-94AF-0026B977EEAA}';

  /// <summary>
  /// VP8 encoder filter interface.
  /// </summary>
type
  IVP8Encoder = interface(IUnknown)
    ['{ED3110FE-5211-11DF-94AF-0026B977EEAA}']
    /// <summary>
    /// The filter maintains a set of encoder configuration values, held
    /// in cache.  Any parameters set (using the methods below) are always
    /// applied to the cached value, irrespective of the state of the graph.
    /// <para></para>
    /// When the graph is started, the filter initializes the VP8 encoder
    /// using the cached configuration values.  This is done automatically,
    /// as part of the activities associated with transitioning the filter
    /// from the stopped state.
    /// <para></para>
    /// If the graph has been started, then any parameters set by the user
    /// are still applied to the cache (as before).  However, to apply the
    /// configuration values in cache to the VP8 encoder, the user must also
    /// call ApplySettings.
    /// <para></para>
    /// It is harmless to call ApplySettings while the graph is stopped.
    /// </summary>
    procedure ApplySettings(); stdcall;

    /// <summary>
    /// Sets the configuration values in cache to their defaults, the same
    /// as they had when the filter instance was originally created.
    /// </summary>
    procedure ResetSettings();

    /// <summary>
    /// Time to spend encoding, in microseconds.
    /// </summary>
    /// <param name="Deadline">
    /// Deadline time. 0 - infinite.
    /// </param>
    procedure SetDeadline(Deadline: integer); stdcall;

    /// <summary>
    /// Time to spend encoding, in microseconds.
    /// </summary>
    /// <param name="Deadline">
    /// Deadline time.
    /// </param>
    procedure GetDeadline(out Deadline: integer); stdcall;

    /// <summary>
    /// For multi-threaded implementations, use no more than this number of threads.
    /// The codec may use fewer threads than allowed. The value 0 is equivalent to the value 1.
    /// </summary>
    /// <param name="Threads">
    /// Threads count.
    /// </param>
    procedure SetThreadCount(Threads: integer); stdcall;

    /// <summary>
    /// For multi-threaded implementations, use no more than this number of threads.
    /// The codec may use fewer threads than allowed. The value 0 is equivalent to the value 1.
    /// </summary>
    /// <param name="Threads">
    /// Threads count.
    /// </param>
    procedure GetThreadCount(out Threads: integer); stdcall;

    /// <summary>
    /// Error resilient mode indicates to the encoder that it should take
    /// measures appropriate for streaming over lossy or noisy links, if
    /// possible.  The value 0 means feature is disabled (the default),
    /// and any positive value means the feature is enabled.
    /// </summary>
    /// <param name="ErrorResilient">
    /// Error resilient parameter.
    /// </param>
    procedure SetErrorResilient(ErrorResilient: integer); stdcall;

    /// <summary>
    /// Error resilient mode indicates to the encoder that it should take
    /// measures appropriate for streaming over lossy or noisy links, if
    /// possible.  The value 0 means feature is disabled (the default),
    /// and any positive value means the feature is enabled.
    /// </summary>
    /// <param name="ErrorResilient">
    /// Error resilient parameter.
    /// </param>
    procedure GetErrorResilient(out ErrorResilient: integer); stdcall;

    /// <summary>
    /// Temporal resampling allows the codec to "drop" frames as a strategy to
    /// meet its target data rate. This can cause temporal discontinuities in
    /// the encoded video, which may appear as stuttering during playback. This
    /// trade-off is often acceptable, but for many applications is not. It can
    /// be disabled in these cases.
    /// This threshold is described as a percentage of the target data buffer.
    /// When the data buffer falls below this percentage of fullness, a
    /// dropped frame is indicated. Set the threshold to zero (0) to disable
    /// this feature.
    /// </summary>
    /// <param name="DropframeThreshold">
    /// Dropframe threshold.
    /// </param>
    procedure SetDropframeThreshold(DropframeThreshold: integer); stdcall;

    /// <summary>
    /// Temporal resampling allows the codec to "drop" frames as a strategy to
    /// meet its target data rate. This can cause temporal discontinuities in
    /// the encoded video, which may appear as stuttering during playback. This
    /// trade-off is often acceptable, but for many applications is not. It can
    /// be disabled in these cases.
    /// This threshold is described as a percentage of the target data buffer.
    /// When the data buffer falls below this percentage of fullness, a
    /// dropped frame is indicated. Set the threshold to zero (0) to disable
    /// this feature.
    /// </summary>
    /// <param name="DropframeThreshold">
    /// Dropframe threshold.
    /// </param>
    procedure GetDropframeThreshold(out DropframeThreshold: integer); stdcall;

    /// <summary>
    /// Enable/disable spatial resampling
    /// Spatial resampling allows the codec to compress a lower resolution
    /// version of the frame, which is then upscaled by the encoder to the
    /// correct presentation resolution. This increases visual quality at
    /// low data rates, at the expense of CPU time on the encoder/decoder.
    /// </summary>
    /// <param name="ResizeAllowed">
    /// Resize allowed.
    /// </param>
    procedure SetResizeAllowed(ResizeAllowed: integer); stdcall;

    /// <summary>
    /// Enable/disable spatial resampling
    /// Spatial resampling allows the codec to compress a lower resolution
    /// version of the frame, which is then upscaled by the encoder to the
    /// correct presentation resolution. This increases visual quality at
    /// low data rates, at the expense of CPU time on the encoder/decoder.
    /// </summary>
    /// <param name="ResizeAllowed">
    /// Resize allowed.
    /// </param>
    procedure GetResizeAllowed(out ResizeAllowed: integer); stdcall;

    /// <summary>
    /// Spatial resampling up watermark.
    /// This threshold is described as a percentage of the target data buffer.
    /// When the data buffer rises above this percentage of fullness, the
    /// encoder will step up to a higher resolution version of the frame.
    /// </summary>
    /// <param name="ResizeUpThreshold">
    /// Resize up threshold.
    /// </param>
    procedure SetResizeUpThreshold(ResizeUpThreshold: integer); stdcall;

    /// <summary>
    /// Spatial resampling up watermark.
    /// This threshold is described as a percentage of the target data buffer.
    /// When the data buffer rises above this percentage of fullness, the
    /// encoder will step up to a higher resolution version of the frame.
    /// </summary>
    /// <param name="ResizeUpThreshold">
    /// Resize up threshold.
    /// </param>
    procedure GetResizeUpThreshold(out ResizeUpThreshold: integer); stdcall;

    /// <summary>
    /// Spatial resampling down watermark.
    /// This threshold is described as a percentage of the target data buffer.
    /// When the data buffer falls below this percentage of fullness, the
    /// encoder will step down to a lower resolution version of the frame.
    /// </summary>
    /// <param name="ResizeDownThreshold">
    /// Resize down threshold.
    /// </param>
    procedure SetResizeDownThreshold(ResizeDownThreshold: integer); stdcall;

    /// <summary>
    /// Spatial resampling down watermark.
    /// This threshold is described as a percentage of the target data buffer.
    /// When the data buffer falls below this percentage of fullness, the
    /// encoder will step down to a lower resolution version of the frame.
    /// </summary>
    /// <param name="ResizeDownThreshold">
    /// Resize down threshold.
    /// </param>
    procedure GetResizeDownThreshold(out ResizeDownThreshold: integer); stdcall;

    /// <summary>
    /// Indicates whether the end usage of this stream is to be streamed over
    /// a bandwidth constrained link (kEndUsageCBR), or whether it will be
    /// played back on a high bandwidth link, as from a local disk, where
    /// higher variations in bitrate are acceptable (kEndUsageVBR, the default).
    /// </summary>
    /// <param name="EndUsage">
    /// End usage parameter.
    /// </param>
    procedure SetEndUsage(EndUsage: VP8EndUsage); stdcall;

    /// <summary>
    /// Indicates whether the end usage of this stream is to be streamed over
    /// a bandwidth constrained link (kEndUsageCBR), or whether it will be
    /// played back on a high bandwidth link, as from a local disk, where
    /// higher variations in bitrate are acceptable (kEndUsageVBR, the default).
    /// </summary>
    /// <param name="EndUsage">
    /// End usage parameter.
    /// </param>
    procedure GetEndUsage(out EndUsage: VP8EndUsage); stdcall;

    /// <summary>
    /// If set, this value allows the encoder to consume a number of input
    /// frames before producing output frames. This allows the encoder to
    /// base decisions for the current frame on future frames. This does
    /// increase the latency of the encoding pipeline, so it is not appropriate
    /// in all situations (ex: realtime encoding).
    /// This is a maximum value - the encoder may produce frames
    /// sooner than the given limit. Set this value to 0 to disable this feature.
    /// </summary>
    /// <param name="LagInFrames">
    /// Lag in frames.
    /// </param>
    procedure SetLagInFrames(LagInFrames: integer); stdcall;

    /// <summary>
    /// If set, this value allows the encoder to consume a number of input
    /// frames before producing output frames. This allows the encoder to
    /// base decisions for the current frame on future frames. This does
    /// increase the latency of the encoding pipeline, so it is not appropriate
    /// in all situations (ex: realtime encoding).
    /// This is a maximum value - the encoder may produce frames
    /// sooner than the given limit. Set this value to 0 to disable this feature.
    /// </summary>
    /// <param name="LagInFrames">
    /// Lag in frames.
    /// </param>
    procedure GetLagInFrames(out LagInFrames: integer); stdcall;

    /// <summary>
    /// VP8 token partition mode.
    /// This defines VP8 partitioning mode for compressed data, i.e., the number
    /// of sub-streams in the bitstream. Used for parallelized decoding.
    /// Value 0 = one token partition.
    /// Value 1 = two token partitions.
    /// Value 2 = four token partitions.
    /// Value 3 = eight token partitions.
    /// </summary>
    /// <param name="TokenPartition">
    /// Token partition.
    /// </param>
    procedure SetTokenPartitions(TokenPartition: integer); stdcall;

    /// <summary>
    /// VP8 token partition mode.
    /// This defines VP8 partitioning mode for compressed data, i.e., the number
    /// of sub-streams in the bitstream. Used for parallelized decoding.
    /// Value 0 = one token partition.
    /// Value 1 = two token partitions.
    /// Value 2 = four token partitions.
    /// Value 3 = eight token partitions.
    /// </summary>
    /// <param name="TokenPartition">
    /// Token partition.
    /// </param>
    procedure GetTokenPartitions(out TokenPartition: integer); stdcall;

    /// <summary>
    /// Target data rate.
    /// Target bandwidth to use for this stream, in kilobits per second.
    /// The value 0 means "use the codec default".
    /// </summary>
    /// <param name="Bitrate">
    /// Bitrate.
    /// </param>
    procedure SetTargetBitrate(bitrate: integer); stdcall;

    /// <summary>
    /// Target data rate.
    /// Target bandwidth to use for this stream, in kilobits per second.
    /// The value 0 means "use the codec default".
    /// </summary>
    /// <param name="Bitrate">
    /// Bitrate.
    /// </param>
    procedure GetTargetBitrate(out bitrate: integer); stdcall;

    /// <summary>
    /// Minimum (Best Quality) Quantizer.
    /// The quantizer is the most direct control over the quality of the
    /// encoded image.  The quantizer range is [0, 63].
    /// </summary>
    /// <param name="MinQuantizer">
    /// Minimal quantizer.
    /// </param>
    procedure SetMinQuantizer(MinQuantizer: integer); stdcall;

    /// <summary>
    /// Minimum (Best Quality) Quantizer.
    /// The quantizer is the most direct control over the quality of the
    /// encoded image.  The quantizer range is [0, 63].
    /// </summary>
    /// <param name="MinQuantizer">
    /// Minimal quantizer.
    /// </param>
    procedure GetMinQuantizer(out MinQuantizer: integer); stdcall;

    /// <summary>
    /// Maximum (Worst Quality) Quantizer.
    /// The quantizer is the most direct control over the quality of the
    /// encoded image.  The quantizer range is [0, 63].
    /// </summary>
    /// <param name="MaxQuantizer">
    /// Maximal quantizer.
    /// </param>
    procedure SetMaxQuantizer(MaxQuantizer: integer); stdcall;

    /// <summary>
    /// Maximum (Worst Quality) Quantizer.
    /// The quantizer is the most direct control over the quality of the
    /// encoded image.  The quantizer range is [0, 63].
    /// </summary>
    /// <param name="MaxQuantizer">
    /// Maximal quantizer.
    /// </param>
    procedure GetMaxQuantizer(out MaxQuantizer: integer); stdcall;

    /// <summary>
    /// Rate control undershoot tolerance.
    /// This value, expressed as a percentage of the target bitrate, describes
    /// the target bitrate for easier frames, allowing bits to be saved for
    /// harder frames. Set to zero to use the codec default.
    /// </summary>
    /// <param name="UndershootPct">
    /// Undershoot pct.
    /// </param>
    procedure SetUndershootPct(UndershootPct: integer); stdcall;

    /// <summary>
    /// Rate control undershoot tolerance.
    /// This value, expressed as a percentage of the target bitrate, describes
    /// the target bitrate for easier frames, allowing bits to be saved for
    /// harder frames. Set to zero to use the codec default.
    /// </summary>
    /// <param name="UndershootPct">
    /// Undershoot pct.
    /// </param>
    procedure GetUndershootPct(out UndershootPct: integer); stdcall;

    /// <summary>
    /// Rate control overshoot tolerance.
    /// This value, expressed as a percentage of the target bitrate, describes
    /// the maximum allowed bitrate for a given frame.  Set to zero to use the
    /// codec default.
    /// </summary>
    /// <param name="OvershootPct">
    /// Overshoot pct.
    /// </param>
    procedure SetOvershootPct(OvershootPct: integer); stdcall;

    /// <summary>
    /// Rate control overshoot tolerance.
    /// This value, expressed as a percentage of the target bitrate, describes
    /// the maximum allowed bitrate for a given frame.  Set to zero to use the
    /// codec default.
    /// </summary>
    /// <param name="OvershootPct">
    /// Overshoot pct.
    /// </param>
    procedure GetOvershootPct(out OvershootPct: integer); stdcall;

    /// <summary>
    /// Decoder Buffer Size.
    /// This value indicates the amount of data that may be buffered by the
    /// decoding application. This value is expressed in units of
    /// time (milliseconds). For example, a value of 5000 indicates that the
    /// client will buffer (at least) 5000 ms worth of encoded data.
    /// </summary>
    /// <param name="TimeInMilliseconds">
    /// Time, in milliseconds.
    /// </param>
    procedure SetDecoderBufferSize(TimeInMilliseconds: integer); stdcall;

    /// <summary>
    /// Decoder Buffer Size.
    /// This value indicates the amount of data that may be buffered by the
    /// decoding application. This value is expressed in units of
    /// time (milliseconds). For example, a value of 5000 indicates that the
    /// client will buffer (at least) 5000 ms worth of encoded data.
    /// </summary>
    /// <param name="TimeInMilliseconds">
    /// Time, in milliseconds.
    /// </param>
    procedure GetDecoderBufferSize(out TimeInMilliseconds: integer); stdcall;

    /// <summary>
    /// Decoder Buffer Initial Size.
    /// This value indicates the amount of data that will be buffered by the
    /// decoding application prior to beginning playback. This value is
    /// expressed in units of time (milliseconds).
    /// </summary>
    /// <param name="TimeInMilliseconds">
    /// Time in milliseconds.
    /// </param>
    procedure SetDecoderBufferInitialSize(TimeInMilliseconds: integer); stdcall;

    /// <summary>
    /// Decoder Buffer Initial Size.
    /// This value indicates the amount of data that will be buffered by the
    /// decoding application prior to beginning playback. This value is
    /// expressed in units of time (milliseconds).
    /// </summary>
    /// <param name="TimeInMilliseconds">
    /// Time in milliseconds.
    /// </param>
    procedure GetDecoderBufferInitialSize(out TimeInMilliseconds
      : integer); stdcall;

    /// <summary>
    /// Decoder Buffer Optimal Size.
    /// This value indicates the amount of data that the encoder should try
    /// to maintain in the decoder's buffer. This value is expressed in units
    /// of time (milliseconds).
    /// </summary>
    /// <param name="TimeInMilliseconds">
    /// Time, in milliseconds.
    /// </param>
    procedure SetDecoderBufferOptimalSize(TimeInMilliseconds: integer); stdcall;

    /// <summary>
    /// Decoder Buffer Optimal Size.
    /// This value indicates the amount of data that the encoder should try
    /// to maintain in the decoder's buffer. This value is expressed in units
    /// of time (milliseconds).
    /// </summary>
    /// <param name="TimeInMilliseconds">
    /// Time, in milliseconds.
    /// </param>
    procedure GetDecoderBufferOptimalSize(out TimeInMilliseconds
      : integer); stdcall;

    /// <summary>
    /// Keyframe placement mode.
    /// This value indicates whether the encoder should place keyframes at a
    /// fixed interval, or determine the optimal placement automatically
    /// (as governed by the KeyframeMinInterval and KeyframeMaxInterval).
    /// </summary>
    /// <param name="Mode">
    /// Mode.
    /// </param>
    procedure SetKeyframeMode(mode: VP8KeyframeMode); stdcall;

    /// <summary>
    /// Keyframe placement mode.
    /// This value indicates whether the encoder should place keyframes at a
    /// fixed interval, or determine the optimal placement automatically
    /// (as governed by the KeyframeMinInterval and KeyframeMaxInterval).
    /// </summary>
    /// <param name="Mode">
    /// Mode.
    /// </param>
    procedure GetKeyframeMode(out mode: VP8KeyframeMode); stdcall;

    /// <summary>
    /// Keyframe minimum interval.
    /// This value, expressed as a number of frames, prevents the encoder from
    /// placing a keyframe nearer than MinInterval to the previous keyframe. At
    /// least MinInterval frames non-keyframes will be coded before the next
    /// keyframe. Set MinInterval equal to MaxInterval for a fixed interval.
    /// </summary>
    /// <param name="MinInterval">
    /// Minimal interval.
    /// </param>
    procedure SetKeyframeMinInterval(MinInterval: integer); stdcall;

    /// <summary>
    /// Keyframe minimum interval.
    /// This value, expressed as a number of frames, prevents the encoder from
    /// placing a keyframe nearer than MinInterval to the previous keyframe. At
    /// least MinInterval frames non-keyframes will be coded before the next
    /// keyframe. Set MinInterval equal to MaxInterval for a fixed interval.
    /// </summary>
    /// <param name="MinInterval">
    /// Minimal interval.
    /// </param>
    procedure GetKeyframeMinInterval(out MinInterval: integer); stdcall;

    /// <summary>
    /// Keyframe maximum interval.
    /// This value, expressed as a number of frames, forces the encoder to code
    /// a keyframe if one has not been coded in the last MaxInterval frames.
    /// A value of 0 implies all frames will be keyframes. Set MinInterval
    /// equal to MaxInterval for a fixed interval.
    /// </summary>
    /// <param name="MaxInterval">
    /// Maximal interval.
    /// </param>
    procedure SetKeyframeMaxInterval(MaxInterval: integer); stdcall;

    /// <summary>
    /// Keyframe maximum interval.
    /// This value, expressed as a number of frames, forces the encoder to code
    /// a keyframe if one has not been coded in the last MaxInterval frames.
    /// A value of 0 implies all frames will be keyframes. Set MinInterval
    /// equal to MaxInterval for a fixed interval.
    /// </summary>
    /// <param name="MaxInterval">
    /// Maximal interval.
    /// </param>
    procedure GetKeyframeMaxInterval(out MaxInterval: integer); stdcall;

    /// <summary>
    /// Multi-pass Encoding Mode
    /// This value should be set to the current phase for multi-pass encoding.
    /// </summary>
    /// <param name="PassMode">
    /// Pass mode.
    /// </param>
    procedure SetPassMode(PassMode: VP8PassMode); stdcall;

    /// <summary>
    /// Multi-pass Encoding Mode
    /// This value should be set to the current phase for multi-pass encoding.
    /// </summary>
    /// <param name="PassMode">
    /// Pass mode.
    /// </param>
    procedure GetPassMode(out PassMode: VP8PassMode); stdcall;

    /// <summary>
    /// Stats Buffer
    /// The stats buffer is the buffer containing all of the stats packets from
    /// the first pass, concatenated together.  It is only used during the last
    /// pass of a multi-pass (really, two-pass) mode.
    /// </summary>
    /// <param name="Buffer">
    /// Buffer.
    /// </param>
    /// <param name="Length">
    /// Buffer length.
    /// </param>
    procedure SetTwoPassStatsBuf(var buffer: PByte; Length: int64); stdcall;

    /// <summary>
    /// Stats Buffer
    /// The stats buffer is the buffer containing all of the stats packets from
    /// the first pass, concatenated together.  It is only used during the last
    /// pass of a multi-pass (really, two-pass) mode.
    /// </summary>
    /// <param name="Buffer">
    /// Buffer.
    /// </param>
    /// <param name="Length">
    /// Buffer length.
    /// </param>
    procedure GetTwoPassStatsBuf(buffer: PByte; var Length: int64); stdcall;

    /// <summary>
    /// Two-pass mode CBR/VBR bias
    /// Bias, expressed on a scale of 0 to 100, for determining target size
    /// for the current frame. The value 0 indicates the optimal CBR mode
    /// value should be used. The value 100 indicates the optimal VBR mode
    /// value should be used. Values in between indicate which way the
    /// encoder should "lean."
    /// RC mode bias between CBR and VBR(0-100: 0->CBR, 100->VBR)
    /// </summary>
    /// <param name="Bias">
    /// Bias.
    /// </param>
    procedure SetTwoPassVbrBiasPct(Bias: integer); stdcall;

    /// <summary>
    /// Two-pass mode CBR/VBR bias
    /// Bias, expressed on a scale of 0 to 100, for determining target size
    /// for the current frame. The value 0 indicates the optimal CBR mode
    /// value should be used. The value 100 indicates the optimal VBR mode
    /// value should be used. Values in between indicate which way the
    /// encoder should "lean."
    /// RC mode bias between CBR and VBR(0-100: 0->CBR, 100->VBR)
    /// </summary>
    /// <param name="Bias">
    /// Bias.
    /// </param>
    procedure GetTwoPassVbrBiasPct(out Bias: integer); stdcall;

    /// <summary>
    /// Two-pass mode per-GOP minimum bitrate
    /// This value, expressed as a percentage of the target bitrate, indicates
    /// the minimum bitrate to be used for a single GOP (aka "section").
    /// </summary>
    /// <param name="Bitrate">
    /// Bitrate.
    /// </param>
    procedure SetTwoPassVbrMinsectionPct(bitrate: integer); stdcall;

    /// <summary>
    /// Two-pass mode per-GOP minimum bitrate
    /// This value, expressed as a percentage of the target bitrate, indicates
    /// the minimum bitrate to be used for a single GOP (aka "section").
    /// </summary>
    /// <param name="Bitrate">
    /// Bitrate.
    /// </param>
    procedure GetTwoPassVbrMinsectionPct(out bitrate: integer); stdcall;

    /// <summary>
    /// Two-pass mode per-GOP maximum bitrate
    /// This value, expressed as a percentage of the target bitrate, indicates
    /// the maximum bitrate to be used for a single GOP (aka "section").
    /// </summary>
    /// <param name="Bitrate">
    /// Bitrate.
    /// </param>
    procedure SetTwoPassVbrMaxsectionPct(bitrate: integer); stdcall;

    /// <summary>
    /// Two-pass mode per-GOP maximum bitrate
    /// This value, expressed as a percentage of the target bitrate, indicates
    /// the maximum bitrate to be used for a single GOP (aka "section").
    /// </summary>
    /// <param name="Bitrate">
    /// Bitrate.
    /// </param>
    procedure GetTwoPassVbrMaxsectionPct(out bitrate: integer); stdcall;

    /// <summary>
    /// Force Keyframe
    /// Set a flag to request that a keyframe be created from the next frame
    /// encoded.  When the frame is encoded, automatically clear the flag.
    /// If the graph is stopped, then this flag will only be applied when the
    /// graph transitions out of stopped.  You can also clear the flag manually.
    /// </summary>
    procedure SetForceKeyframe(); stdcall;

    /// <summary>
    /// Force Keyframe
    /// Set a flag to request that a keyframe be created from the next frame
    /// encoded.  When the frame is encoded, automatically clear the flag.
    /// If the graph is stopped, then this flag will only be applied when the
    /// graph transitions out of stopped.  You can also clear the flag manually.
    /// </summary>
    procedure ClearForceKeyframe(); stdcall;
  end;

const
  IID_IVorbisEncodeSettings: TGUID = '{A4C6A887-7BD3-4b33-9A57-A3EB10924D3A}';

type
  /// <summary>
  /// Vorbis encoder filter interface.
  /// </summary>
  IVorbisEncodeSettings = interface(IUnknown)
    ['{A4C6A887-7BD3-4b33-9A57-A3EB10924D3A}']
    /// <summary>
    /// Gets encoder settings.
    /// </summary>
    /// <returns>
    /// Returns encoder settings.
    /// </returns>
    function GetEncoderSettings(): pointer; stdcall;

    /// <summary>
    /// Sets quality.
    /// </summary>
    /// <param name="inQuality">
    /// Quality.
    /// </param>
    /// <returns>
    /// Returns true if the operation was successful.
    /// </returns>
    function SetQuality(inQuality: integer): boolean; stdcall;

    /// <summary>
    /// Sets encoding mode.
    /// </summary>
    /// <param name="inBitrate">
    /// Encoding mode.
    /// </param>
    /// <returns>
    /// Returns true if the operation was successful.
    /// </returns>
    function SetBitrateQualityMode(inBitrate: integer): boolean; stdcall;

    /// <summary>
    /// Sets bitrate.
    /// </summary>
    /// <param name="inBitrate">
    /// Bitrate.
    /// </param>
    /// <param name="inMinBitrate">
    /// Min bitrate.
    /// </param>
    /// <param name="inMaxBitrate">
    /// Max bitrate.
    /// </param>
    /// <returns>
    /// Returns true if the operation was successful.
    /// </returns>
    function SetManaged(inBitrate, inMinBitrate, inMaxBitrate: integer)
      : boolean; stdcall;
  end;

const
  CLSID_WebmSource: TGUID = '{ED3110F7-5211-11DF-94AF-0026B977EEAA}';
  CLSID_WebmSplit: TGUID = '{ED3110F8-5211-11DF-94AF-0026B977EEAA}';
  CLSID_WebmMuxer: TGUID = '{ED3110F0-5211-11DF-94AF-0026B977EEAA}';
  CLSID_VP8Encoder: TGUID = '{ED3110F5-5211-11DF-94AF-0026B977EEAA}';
  CLSID_VP8Decoder: TGUID = '{ED3110F3-5211-11DF-94AF-0026B977EEAA}';
  CLSID_VorbisEncoder: TGUID = '{5C94FE86-B93B-467F-BFC3-BD6C91416F9B}';
  CLSID_VorbisDecoder: TGUID = '{05A1D945-A794-44EF-B41A-2F851A117155}';
  MEDIASUBTYPE_WEBM: TGUID = '{ED3110EB-5211-11DF-94AF-0026B977EEAA}';
  MEDIASUBTYPE_VP8_STATS: TGUID = '{ED3110EC-5211-11DF-94AF-0026B977EEAA}';
  MEDIASUBTYPE_VP80: TGUID = '{30385056-0000-0010-8000-00AA00389B71}';
  MEDIASUBTYPE_I420: TGUID = '{30323449-0000-0010-8000-00AA00389B71}';
  CLSID_OGGMuxer: TGUID = '{1F3EFFE4-0E70-47C7-9C48-05EB99E20011}';
  IID_IVFFFMPEGEncoder: TGUID = '{17B8FF7D-A67F-45CE-B425-0E4F607D8C60}';
  CLSID_FFMPEGEncoder: TGUID = '{554AB365-B293-4C1D-9245-E8DB01F027F7}';

  /// <summary>
  /// VisioForge AAC encoder.
  /// </summary>
  CLSID_VFAAC: TGUID = '{C6499EA5-481A-4B62-BDDE-A556CD27B258}';

  /// <summary>
  /// VisioForge H264 encoder.
  /// </summary>
  CLSID_VFH264Encoder: TGUID = '{8252B38C-A72A-4E2D-B2B3-3AEE94C3B58B}';

  /// <summary>
  /// VisioForge MP4 muxer.
  /// </summary>
  CLSID_VFMP4Dest: TGUID = '{4649106F-A772-4345-8971-6100F6EEA1F2}';

  /// <summary>
  /// VisioForge CUDA encoder.
  /// </summary>
  CLSID_H264CUDA: TGUID = '{F3FBEAE6-B7DE-425D-88EA-E4D9D3DAFC96}';

  /// <summary>
  /// VisioForge encryptor.
  /// </summary>
  CLSID_VFEncryptor: TGUID = '{F1D3727A-88DE-49AB-A635-280BEFEFF902}';

  /// <summary>
  /// VisioForge decryptor.
  /// </summary>
  CLSID_VFDecryptor: TGUID = '{D2C761F0-9988-4F79-9B0E-FB2B79C65851}';

  /// <summary>
  /// RGB2YUV filter CLSID.
  /// </summary>
  CLSID_VFRGB2YUV: TGUID = '{3BDA461E-12DB-4C24-9815-B68D1AA4D34A}';

  /// <summary>
  /// YUV2RGB filter CLSID.
  /// </summary>
  CLSID_VFYUV2RGB: TGUID = '{CB54D323-9327-49F5-8147-859FE8FAEFF5}';

  /// <summary>
  /// Virtual audio card sink CLSID.
  /// </summary>
  CLSID_VFVirtualAudioCardSink
    : TGUID = '{1A2673B0-553E-4027-AECC-839405468950}';

  /// <summary>
  /// Virtual audio card source CLSID.
  /// </summary>
  CLSID_VFVirtualAudioCardSource
    : TGUID = '{B5A463DF-4016-4C34-AA4F-48EC1B51C73F}';

  /// <summary>
  /// Virtual camera sink CLSID.
  /// </summary>
  CLSID_VFVirtualCameraSink: TGUID = '{AA6AB4DF-9670-4913-88BB-2CB381C19340}';

  /// <summary>
  /// Virtual camera source CLSID.
  /// </summary>
  CLSID_VFVirtualCameraSource: TGUID = '{AA4DA14E-644B-487a-A7CB-517A390B4BB8}';

  /// <summary>
  /// Network streamer video sink CLSID.
  /// </summary>
  CLSID_VFBridgeVideoSink: TGUID = '{AA6AB4DF-9670-4913-40BB-2CB381C193EE}';

  /// <summary>
  /// Network streamer audio sink CLSID.
  /// </summary>
  CLSID_VFBridgeAudioSink: TGUID = '{1A2673B0-553E-4040-AECC-839405468951}';

  /// <summary>
  /// Network streamer video source CLSID.
  /// </summary>
  CLSID_VFBridgeVideoSource: TGUID = '{AA4DA14E-644B-487a-40CB-517A390B4BEE}';

  /// <summary>
  /// Network streamer audio source CLSID.
  /// </summary>
  CLSID_VFBridgeAudioSource: TGUID = '{B5A463DF-4016-4c40-AA4F-48EC1B51C731}';

  /// <summary>
  /// Network streamer video sink CLSID.
  /// </summary>
  CLSID_VFNetworkStreamerVideoSink
    : TGUID = '{AA6AB4DF-9670-4913-88BB-2CB381C19340}';

  /// <summary>
  /// Network streamer audio sink CLSID.
  /// </summary>
  CLSID_VFNetworkStreamerAudioSink
    : TGUID = '{1A2673B0-553E-4027-AECC-839405468951}';

  /// <summary>
  /// Network streamer video source CLSID.
  /// </summary>
  CLSID_VFNetworkStreamerVideoSource
    : TGUID = '{AA4DA14E-644B-487a-A7CB-517A390B4BB8}';

  /// <summary>
  /// Network streamer audio source CLSID.
  /// </summary>
  CLSID_VFNetworkStreamerAudioSource
    : TGUID = '{B5A463DF-4016-4C34-AA4F-48EC1B51C731}';

type
  FFMPEGOutputSettings = record
    Filename: PWideChar;
    AudioAvailable: longbool;
    AudioBitrate: integer;
    AudioSamplerate: integer;
    AudioChannels: integer;
    VideoWidth: integer;
    VideoHeight: integer;
    AspectRatioW: integer;
    AspectRatioH: integer;
    VideoBitrate: integer;
    VideoMaxRate: integer;
    VideoMinRate: integer;
    VideoBufferSize: integer;
    Interlace: longbool;
    VideoGopSize: integer;
    TVSystem: integer;
    // TVFFFMPEGTVSystem;
    OutputFormat: integer; // TVFFFMPEGOutputFormat;
  end;

  IVFFFMPEGEncoder = interface(IUnknown)
    ['{17B8FF7D-A67F-45CE-B425-0E4F607D8C60}']
    function set_settings(settings: FFMPEGOutputSettings): HRESULT; stdcall;
  end;

  // AsyncEx
const
  CLSID_AsyncEx: TGUID = '{B173D0A0-F669-4f7a-8C40-CF46A1ED04C6}';
  CLSID_PropMonitor: TGUID = '{40362300-F764-4a02-9078-A69FC173DDD2}';
  CLSID_PropPage: TGUID = '{3AD624CA-B19B-407a-8055-81F7890A4000}';
  // IID_IAsyncExControl: TGUID = '{422CA922-D7BD-40c8-ABFF-CF64D4F6B15F}';
  // IID_IAsyncExCallBack: TGUID = '{EC60A9BD-CDA8-462a-BBBF-FCA95A0AA9FE}';
  IID_IAsyncExControl: TGUID = '{3E0FA056-926C-43d9-BA18-EF16E980913B}';
  IID_IAsyncExCallBack: TGUID = '{3E0FB667-956C-43d9-BA18-EF16E980913B}';

  // Interfaces, that can Queried on the Filter
type
  IAsyncExCallBack = interface(IUnknown)
    // ['{EC60A9BD-CDA8-462a-BBBF-FCA95A0AA9FE}']
    ['{3E0FB667-956C-43d9-BA18-EF16E980913B}']
    function AsyncExFilterState(Buffering: longbool; PreBuffering: longbool;
      Connecting: longbool; Playing: longbool; BufferState: integer)
      : HRESULT; stdcall;
    function AsyncExICYNotice(IcyItemName: PChar; ICYItem: PChar)
      : HRESULT; stdcall;
    function AsyncExMetaData(Title: PChar; URL: PChar): HRESULT; stdcall;
    function AsyncExSockError(ErrString: PChar): HRESULT; stdcall;
  end;

type
  IAsyncExControl = interface(IUnknown)
    // ['{422CA922-D7BD-40c8-ABFF-CF64D4F6B15F}']
    ['{3E0FA056-926C-43d9-BA18-EF16E980913B}']
    function SetLoadFromStream(Stream: IStream; Length: int64)
      : HRESULT; stdcall;
    function SetConnectToIp(Host: PChar; Port: PChar; Location: PChar;
      PreBuffersize: integer; MetaData: longbool): HRESULT; stdcall;
    function SetConnectToURL(URL: PChar; PreBuffersize: integer;
      MetaData: longbool): HRESULT; stdcall;
    function SetBuffersize(BufferSize: integer): HRESULT; stdcall;
    function GetBuffersize(out BufferSize: integer): HRESULT; stdcall;
    function SetRipStream(Ripstream: longbool; Path: PChar; Filename: PChar)
      : HRESULT; stdcall;
    function GetRipStream(out Ripstream: longbool; out FileO: PChar)
      : HRESULT; stdcall;
    function SetCallback(Callback: IAsyncExCallBack): HRESULT; stdcall;
    function FreeCallback(): HRESULT; stdcall;
    function ExitAllLoops(): HRESULT; stdcall;
  end;

const
  CLSID_DirectVobSub: TGUID = '{9852A670-F845-491B-9BE6-EBD841B8A613}';
  // Autoloading
  CLSID_DirectVobSubManual: TGUID = '{93A22E7A-5091-45EF-BA61-6DA26156A5D0}';
  // Manual

implementation

uses ComObj;

procedure ClearEffect(var eff: CVFEffect);
begin
  with eff do
  begin
    ID := 0;
    Enabled := False;
    StartTime := 0;
    StopTime := 0;
    pAmountI := 0;
    pMinI := 0;
    pMaxI := 0;
    pAmountD := 0;
    pScaleD := 0;
    pTurbulenceI := 0;
    pSizeI := 0;
    pSeamI := 0;
    pFactorI := 0;
    pInferenceI := 0;
    pStyleI := 0;

    with TextLogo do
    begin
      x := 0;
      y := 0;
      transparent_bg := True;
      text := '';
      font_name := 'Arial';
      font_size := 32;
      font_italic := False;
      font_bold := False;
      font_underline := False;
      font_strikeout := False;
      font_color := 0;
      bg_color := 0;
      rightToLeft := False;
      vertical := False;
      align := 0;
      drawQuality := 0;
      antialiasing := 0;
      rectWidth := 0;
      rectHeight := 0;
      rotationMode := 0;
      flipMode := 0;
      transp := 255;
      gradient := False;
      gradientMode := 0;
      gradientColor1 := 0;
      gradientColor2 := 0;
      borderMode := 0;
      innerBorderColor := 0;
      innerBorderSize := 1;
      outerBorderColor := 0;
      outerBorderSize := 1;
      bgShape := False;
      bgShapeType := 0;
      bgShapeX := 0;
      bgShapeY := 0;
      bgShapeWidth := 0;
      bgShapeHeight := 0;
      bgShapeColor := 0;
    end;
  end;
end;

function MFVideoNormalizedRect(const ALeft, ATop, ARight, ABottom: Single)
  : TMFVideoNormalizedRect;
begin
  Result.left := ALeft;
  Result.top := ATop;
  Result.right := ARight;
  Result.bottom := ABottom;
end;

{ TMFVideoNormalizedRect }
{$IFNDEF norecprocs}

procedure TMFVideoNormalizedRect.init(ALeft, ATop, ARight, ABottom: Single);
begin
  left := ALeft;
  top := ATop;
  right := ARight;
  bottom := ABottom;
end;
{$ENDIF}

end.
