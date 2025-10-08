unit encryptor_intf;

interface

uses
  classes,
  windows,
  messages,
  dialogs,
  ActiveX,
  Contnrs,
  sysutils;

const
  IID_IVFCryptoConfig: TGUID = '{BAA5BD1E-3B30-425e-AB3B-CC20764AC253}';

type
  IVFCryptoConfig = interface(IUnknown)
    ['{BAA5BD1E-3B30-425e-AB3B-CC20764AC253}']
    function put_Provider(pProvider: IUnknown): HResult; stdcall;
    function get_Provider(out pProvider: IUnknown): HResult; stdcall;
    function put_Password(pBuffer: PByte; lSize: Integer): HResult; stdcall;
    function HavePassword(): HResult; stdcall;
  end;

const
  IID_IH264Encoder: TGUID = '{09FA2EA3-4773-41a8-90DC-9499D4061E9F}';

type
  IH264Encoder = interface(IUnknown)
    ['{09FA2EA3-4773-41a8-90DC-9499D4061E9F}']
    function get_Bitrate(out plValue: Integer): HResult; stdcall;

    function put_Bitrate(lValue: Integer): HResult; stdcall;

    function get_RateControl(out pValue: Integer): HResult; stdcall;

    function put_RateControl(value: Integer): HResult; stdcall;

    function get_MbEncoding(out pValue: Integer): HResult; stdcall;

    function put_MbEncoding(value: Integer): HResult; stdcall;

    function get_GOP(out pValue: BOOL): HResult; stdcall;

    function put_GOP(value: BOOL): HResult; stdcall;

    function get_AutoBitrate(out pValue: BOOL): HResult; stdcall;

    function put_AutoBitrate(value: BOOL): HResult; stdcall;

    function get_Profile(out pValue: Integer): HResult; stdcall;

    function put_Profile(value: Integer): HResult; stdcall;

    function get_Level(out pValue: Integer): HResult; stdcall;

    function put_Level(value: Integer): HResult; stdcall;

    function get_Usage(out pValue: Integer): HResult; stdcall;

    function put_Usage(value: Integer): HResult; stdcall;

    function get_SequentialTiming(out pValue: Integer): HResult; stdcall;

    function put_SequentialTiming(value: Integer): HResult; stdcall;

    function get_SliceIntervals(out piIDR: Integer; out piP: Integer)
      : HResult; stdcall;

    function put_SliceIntervals(var piIDR: Integer; var piP: Integer)
      : HResult; stdcall;

    function get_MaxBitrate(out value: Integer): HResult; stdcall;

    function put_MaxBitrate(value: Integer): HResult; stdcall;

    function get_MinBitrate(out value: Integer): HResult; stdcall;

    function put_MinBitrate(value: Integer): HResult; stdcall;
  end;

  // profile
type
  IntelVideoEncoderProfile = (PF_AUTOSELECT = 0,

    // H.264 values
    PF_H264_BASELINE = 66, PF_H264_MAIN = 77, PF_H264_HIGH = 100,
    PF_H264_HIGH10 = 110, PF_H264_HIGH422 = 122,

    // MPEG2 values
    PF_MPEG2_SIMPLE = 80, PF_MPEG2_MAIN = 64, PF_MPEG2_SNR = 3,
    PF_MPEG2_SPATIAL = 2, PF_MPEG2_HIGH = 16);

  // level values
type
  IntelVideoEncoderLevel = (LL_AUTOSELECT = 0,

    // H.264 values
    LL_1 = 10, LL_1b = 9, LL_11 = 11, LL_12 = 12, LL_13 = 13, LL_2 = 20,
    LL_21 = 21, LL_22 = 22, LL_3 = 30, LL_31 = 31, LL_32 = 32, LL_4 = 40,
    LL_41 = 41, LL_42 = 42, LL_5 = 50, LL_51 = 51,

    // MPEG2 values
    LL_LOW = 10, LL_MAIN = 8, LL_HIGH1440 = 6, LL_HIGH = 4);

  // TargetUsages: from 1 to 7 inclusive
type
  IntelVideoEncoderTargetUsage = (TARGETUSAGE_1 = 1, TARGETUSAGE_2 = 2,
    TARGETUSAGE_3 = 3, TARGETUSAGE_4 = 4, TARGETUSAGE_5 = 5, TARGETUSAGE_6 = 6,
    TARGETUSAGE_7 = 7,

    TARGETUSAGE_UNKNOWN = 0, TARGETUSAGE_BEST_QUALITY = TARGETUSAGE_1,
    TARGETUSAGE_BALANCED = TARGETUSAGE_4,
    TARGETUSAGE_BEST_SPEED = TARGETUSAGE_7);

  // picture sequence control
type
  IntelVideoEncoderPSControl = record

    GopPicSize: cardinal; // I-frame interval in frames

    GopRefDist: cardinal;
    // Distance between I- or P- key frames;If GopRefDist = 1, there are no B-frames used
    NumSlice: cardinal; // Number of slices

    BufferSizeInKB: cardinal; // vbv buffer size
  end;

  // picture coding control
type
  IntelVideoEncoderPCControl = (PC_FRAME = 1, PC_FIELD_TFF = 2,
    PC_FIELD_BFF = 4);

type
  IntelVideoEncoderFrameControl = record
    width: cardinal; // output frame width
    height: cardinal; // output frame height
  end;

  // See Requirement 34
  IntelVideoEncoderThrottlePolicy = (
    // no throttle handling
    TT_NA,

    // auto throttling
    TT_AUTO);

  IntelVideoEncoderThrottleControl = record
    throttle_policy: IntelVideoEncoderThrottlePolicy;
    bitrate_up: cardinal; // range of bitrate increase adjustment.
    bitrate_down: cardinal; // range of bitrate decrease adjustment.
    qp_up: cardinal; // range of qp increase adjustment
    qp_down: cardinal; // range of qp decrease adjustment
  end;

  IntelVideoEncoderRCMethod = (RC_CBR = 1, // Constant Bitrate
    RC_VBR = 2 // Variable Bitrate
    );

  // rate control
  IntelVideoEncoderRCControl = record
    rc_method: IntelVideoEncoderRCMethod;
    // specify bit rate in bps
    bitrate: cardinal;
  end;

  IntelVideoEncoderPreset = (
    // undefined preset
    PRESET_UNKNOWN = -1,

    // User Defined (by setting encoder parameters)
    PRESET_USER_DEFINED,

    // Optimal speed and quality
    PRESET_BALANCED,

    // Best quality setting
    PRESET_BEST_QUALITY,

    // Fast encoding with reasonable quality
    PRESET_FAST,

    PRESET_LOW_LATENCY);

  IntelVideoEncoderParams = record
    profile_idc: IntelVideoEncoderProfile;

    level_idc: IntelVideoEncoderLevel;

    ps_control: IntelVideoEncoderPSControl;

    pc_control: IntelVideoEncoderPCControl;

    frame_control: IntelVideoEncoderFrameControl;

    throttle_control: IntelVideoEncoderThrottleControl;

    rc_control: IntelVideoEncoderRCControl;

    preset: IntelVideoEncoderPreset;

    target_usage: IntelVideoEncoderTargetUsage;
  end;

  IntelVideoEncoderStatistics = record
    // size of the Statistics structure
    struct_size: cardinal;
    // frame width
    width: cardinal;
    // frame height
    height: cardinal;
    // frame rate
    frame_rate: cardinal;

    // horizontal pixel aspect ratio
    horizontal: cardinal;
    // vertical pixel aspect ratio
    vertical: cardinal;

    // average bitrate
    real_bitrate: cardinal;
    // number of frames encoded
    frames_encoded: cardinal;
    // requested bit rate
    requested_bitrate: cardinal;
    // number of frames received
    frames_received: cardinal;
  end;

const
  IID_IIntelConfigureVideoEncoder
    : TGUID = '{2A483975-C5B2-4D1F-8DBB-23A653AF5E39}';

type
  IIntelConfigureVideoEncoder = interface(IUnknown)
    ['{2A483975-C5B2-4D1F-8DBB-23A653AF5E39}']
    function SetParams(parameters: IntelVideoEncoderParams): HResult; stdcall;

    function GetParams(out parameters: IntelVideoEncoderParams)
      : HResult; stdcall;

    function GetRunTimeStatistics(out statistics: IntelVideoEncoderStatistics)
      : HResult; stdcall;

    function SetParams2(parameters: IntelVideoEncoderParams): HResult; stdcall;
  end;

const
  IID_IMP4MuxerConfig: TGUID = '{99DC9BE5-0AFA-45d4-8370-AB021FB07CF4}';

type
  IMP4MuxerConfig = interface(IUnknown)
    ['{99DC9BE5-0AFA-45d4-8370-AB021FB07CF4}']
    function get_SingleThread(out pValue: BOOL): HResult; stdcall;

    function put_SingleThread(value: BOOL): HResult; stdcall;

    function get_CorrectTiming(out pValue: BOOL): HResult; stdcall;

    function put_CorrectTiming(value: BOOL): HResult; stdcall;
  end;

  /// <summary>
  /// AAC info.
  /// </summary>
  VFAACInfo = record
    samplerate: Integer;
    channels: Integer;
    frame_size: Integer;
    frames_done: int64;
  end;

  /// <summary>
  /// AAC config.
  /// </summary>
  VFAACConfig = record
    version: Integer;
    object_type: Integer;
    output_type: Integer;
    bitrate: Integer;
  end;

const
  IID_IMonogramAACEncoder: TGUID = '{B2DE30C0-1441-4451-A0CE-A914FD561D7F}';

type
  IMonogramAACEncoder = interface(IUnknown)
    ['{B2DE30C0-1441-4451-A0CE-A914FD561D7F}']
    function GetConfig(var config: VFAACConfig): HResult; stdcall;
    function SetConfig(var config: VFAACConfig): HResult; stdcall;
  end;

const
  IID_IVFAACEncoder: TGUID = '{0BEF7533-39E6-42a5-863F-E087FAB5D84F}';

type
  IVFAACEncoder = interface(IUnknown)
    ['{0BEF7533-39E6-42a5-863F-E087FAB5D84F}']
    function SetInputSampleRate(samplerate: cardinal): HResult; stdcall;
    function GetInputSampleRate(out samplerate: cardinal): HResult; stdcall;
    function SetInputChannels(nChannels: short): HResult; stdcall;
    function GetInputChannels(out pnChannels: short): HResult; stdcall;
    function SetBitRate(ulBitRate: cardinal): HResult; stdcall;
    function GetBitRate(out pulBitRate: cardinal): HResult; stdcall;
    function SetProfile(uProfile: cardinal): HResult; stdcall;
    function GetProfile(out puProfile: cardinal): HResult; stdcall;
    function SetOutputFormat(uFormat: cardinal): HResult; stdcall;
    function GetOutputFormat(out puFormat: cardinal): HResult; stdcall;
    function SetTimeShift(timeShift: Integer): HResult; stdcall;
    function GetTimeShift(out ptimeShift: cardinal): HResult; stdcall;
    function SetLFE(lfe: cardinal): HResult; stdcall;
    function GetLFE(out lfe: cardinal): HResult; stdcall;
    function SetTNS(tns: cardinal): HResult; stdcall;
    function GetTNS(out tns: cardinal): HResult; stdcall;
    function SetMidSide(v: cardinal): HResult; stdcall;
    function GetMidSide(out v: cardinal): HResult; stdcall;
  end;

const
  /// <summary>
  /// VisioForge AAC encoder.
  /// </summary>
  CLSID_VFAACEncoderLegacy: TGUID = '{C6499EA5-481A-4B62-BDDE-A556CD27B258}';

  /// <summary>
  /// VisioForge H264 encoder.
  /// </summary>
  CLSID_VFH264EncoderLegacy: TGUID = '{8252B38C-A72A-4E2D-B2B3-3AEE94C3B58B}';

  /// <summary>
  /// VisioForge MP4 muxer.
  /// </summary>
  CLSID_VFMP4DestLegacy: TGUID = '{BC5FB713-8E81-442A-8A5E-CA21FC7274C8}';

  /// <summary>
  /// VisioForge AAC encoder.
  /// </summary>
  CLSID_VFAACEncoderV10: TGUID = '{763CAC70-373C-4892-898B-AC80661B15F3}';

  /// <summary>
  /// VisioForge H264 encoder.
  /// </summary>
  CLSID_VFH264EncoderV10: TGUID = '{EA1FED6B-B876-4DB0-B7B1-778463E59978}';

  /// <summary>
  /// VisioForge MP4 muxer.
  /// </summary>
  CLSID_VFMP4DestV10: TGUID = '{0B0D654C-7AC1-441E-9C4D-3C29ABEDB6A8}';

  /// <summary>
  /// VisioForge CUDA encoder.
  /// </summary>
  CLSID_H264CUDA: TGUID = '{F3FBEAE6-B7DE-425D-88EA-E4D9D3DAFC96}';

  /// <summary>
  /// VisioForge encryptor.
  /// </summary>
  CLSID_VFEncryptor: TGUID = '{F1D3727A-88DE-49AB-A635-280BEFEFF902}';

  /// <summary>
  /// RGB2YUV filter CLSID.
  /// </summary>
  CLSID_VFRGB2YUV: TGUID = '{3BDA461E-12DB-4C24-9815-B68D1AA4D34A}';

  /// <summary>
  /// YUV2RGB filter CLSID.
  /// </summary>
  CLSID_VFYUV2RGB: TGUID = '{CB54D323-9327-49F5-8147-859FE8FAEFF5}';

const
  IID_IVFRegister: TGUID = '{59E82754-B531-4A8E-A94D-57C75F01DA30}';

type
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

implementation

end.
