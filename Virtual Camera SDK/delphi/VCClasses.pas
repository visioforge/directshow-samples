unit VCClasses;
{$IFDEF VER210}
{$WARN IMPLICIT_STRING_CAST OFF}
{$WARN IMPLICIT_STRING_CAST_LOSS OFF}
{$WARN SYMBOL_DEPRECATED OFF}
{$ENDIF}

interface

uses
  windows,
  classes,
  Dialogs,
  graphics,
  ExtCtrls,
  VFVCDirectShow9,
  VFVCDirect3D9,
  activex,
  comobj,
  sysutils,
  strutils,
  registry,
  math,
  mmsystem,
  contnrs,
  forms,
  multimon,
  VCFiltersAPI,
  // VCVUMeter,
  VideoCaptureTypes
  // VCAudioEffects
    ;

type
  TDSSampleCB = procedure(SampleTime: Double; pSample: IMediaSample) of object;
  TDSBufferCB = procedure(SampleTime: Double; pBuffer: PByte;
    BufferLen: Integer) of object;

  TDSSampleGrabberMediaType = (SG_RGB24, SG_RGB32, SG_PCM);

  TVFStringsDefined = set of (sdDelimiter, sdQuoteChar, sdNameValueSeparator,
    sdLineBreak, sdStrictDelimiter);

  TVFWideStrings = class;

  { TVFWideStrings class }
  TVFWideStrings = class(TPersistent)
  private
    FDefined: TVFStringsDefined;
    FLineBreak: WideString;
    FQuoteChar: WideChar;
    FStrictDelimiter: Boolean;
    function GetLineBreak: WideString;
    function GetQuoteChar: WideChar;
    function GetStrictDelimiter: Boolean;
    procedure SetLineBreak(const Value: WideString);
    procedure SetQuoteChar(const Value: WideChar);
    procedure SetStrictDelimiter(const Value: Boolean);
  protected
    function CompareStrings(const S1, S2: WideString): Integer; virtual;
    function Get(Index: Integer): WideString; virtual; abstract;
    function GetCapacity: Integer; virtual;
    function GetCount: Integer; virtual; abstract;
    function GetObject(Index: Integer): TObject; virtual;
    function GetTextStr: WideString; virtual;
    procedure Put(Index: Integer; const S: WideString); virtual;
    procedure PutObject(Index: Integer; AObject: TObject); virtual;
    procedure SetCapacity(NewCapacity: Integer); virtual;
    procedure SetTextStr(const Value: WideString); virtual;
  public
    destructor Destroy; override;
    function Add(const S: WideString): Integer; virtual;
    function AddObject(const S: WideString; AObject: TObject): Integer; virtual;
    procedure AddStrings(Strings: TStrings); overload; virtual;
    procedure AddStrings(Strings: TVFWideStrings); overload; virtual;
    procedure Clear; virtual; abstract;
    procedure Delete(Index: Integer); virtual; abstract;
    function IndexOf(const S: WideString): Integer; virtual;
    procedure Insert(Index: Integer; const S: WideString); virtual; abstract;
    procedure InsertObject(Index: Integer; const S: WideString;
      AObject: TObject); virtual;
    procedure LoadFromFile(const FileName: WideString); virtual;
    procedure LoadFromStream(Stream: TStream); virtual;
    procedure SaveToFile(const FileName: WideString); virtual;
    procedure SaveToStream(Stream: TStream); virtual;
    property Capacity: Integer read GetCapacity write SetCapacity;
    property Count: Integer read GetCount;
    property LineBreak: WideString read GetLineBreak write SetLineBreak;
    property QuoteChar: WideChar read GetQuoteChar write SetQuoteChar;
    property StrictDelimiter: Boolean read GetStrictDelimiter
      write SetStrictDelimiter;
    property Strings[Index: Integer]: WideString read Get write Put; default;
    property Text: WideString read GetTextStr write SetTextStr;
  end;

  { TVFWideStringList class }

  TVFWideStringList = class;

  PWideStringItem = ^TVFWideStringItem;

  TVFWideStringItem = record
    FString: WideString;
    FObject: TObject;
  end;

  PWideStringItemList = ^TWideStringItemList;
  // TWideStringItemList = array [0 .. MaxListSize] of TVFWideStringItem;
  TWideStringItemList = array [0 .. 1000] of TVFWideStringItem;
  TWideStringListSortCompare = function(List: TVFWideStringList;
    Index1, Index2: Integer): Integer;

  TVFWideStringList = class(TVFWideStrings)
  private
    FCapacity: Integer;
    FCount: Integer;
    FList: PWideStringItemList;
    FOnChange: TNotifyEvent;
    FOnChanging: TNotifyEvent;
    procedure Grow;
  protected
    function Get(Index: Integer): WideString; override;
    function GetCapacity: Integer; override;
    function GetCount: Integer; override;
    function GetObject(Index: Integer): TObject; override;
    procedure InsertItem(Index: Integer; const S: WideString;
      AObject: TObject); virtual;
    procedure Put(Index: Integer; const S: WideString); override;
    procedure PutObject(Index: Integer; AObject: TObject); override;
    procedure SetCapacity(NewCapacity: Integer); override;
  public
    destructor Destroy; override;
    function Add(const S: WideString): Integer; override;
    procedure Clear; override;
    procedure Delete(Index: Integer); override;
    function IndexOf(const S: WideString): Integer; override;
    procedure Insert(Index: Integer; const S: WideString); override;
    function AddObject(const S: WideString; AObject: TObject): Integer;
      override;
    procedure InsertObject(Index: Integer; const S: WideString;
      AObject: TObject); override;
  end;

  PFilCatNode = ^TFilCatNode;

  { @exclude }
  TFilCatNode = record
    FriendlyName: WideString;
    Description: WideString;
    CLSID: TGUID;
    FOURCC: WideString;
    DevicePath: WideString;
    Merit: DWORD;
  end;

  TDEV_BROADCAST_HDR = record
    dbch_size: DWORD;
    dbch_devicetype: DWORD;
    dbch_reserved: DWORD;
  end;

  TVFSysDevEnum = class
  private
    ACategory: PFilCatNode;
    FCategories: TList;
    FFilters: TList;
    FGUID: TGUID;
    FReadMerit: Boolean;
    procedure GetCat(catlist: TList; CatGUID: TGUID);
    function GetCategory(item: Integer): TFilCatNode;
    function GetCountCategories: Integer;
    function GetCountFilters: Integer;
    function GetFilter(item: Integer): TFilCatNode;
  public
    constructor Create; overload;
    constructor Create(guid: TGUID); overload;
    destructor Destroy; override;
    function FilterIndexOfFriendlyName(const FriendlyName: WideString): Integer;
    function GetBaseFilter(Index: Integer): IBaseFilter; overload;
    function GetBaseFilter(guid: TGUID): IBaseFilter; overload;
    function GetFilterMerit(name, guid: WideString): cardinal;
    function GetMoniker(Index: Integer): IMoniker;
    procedure SelectGUIDCategory(guid: TGUID);
    procedure SelectIndexCategory(Index: Integer);
    function SetFilterMerit(name, guid: WideString; Merit: DWORD): Boolean;
    property Categories[item: Integer]: TFilCatNode read GetCategory;
    property CountCategories: Integer read GetCountCategories;
    property CountFilters: Integer read GetCountFilters;
    property Filters[item: Integer]: TFilCatNode read GetFilter;

    property ReadMerit: Boolean read FReadMerit write FReadMerit;
  end;

type
  TDSScreen = class(TObject) // 4.1
  private
    FDebugMode: Boolean;
    FForcedRect: TRect;
    FForceRect: Boolean;
    FAspect_Ratio_Override: Boolean;
    FAspect_Ratio_X: Integer;
    FAspect_Ratio_Y: Integer;
    FBorderColor: TColor;
    FullScreen: Boolean;
    function GetLetterboxRect(ScreenWidth, ScreenHeight, AspectX,
      AspectY: Integer): TRect;
  protected
    pEVRFilterConfig: IEVRFilterConfig;
    pMFGetService: IMFGetService;
    pMFVideoMixerControl: IMFVideoMixerControl;
    pMFVideoRenderer: IMFVideoRenderer;
    pVMRDeinterlaceControl9: IVMRDeinterlaceControl9;
    pVMRFilterConfig: IVMRFilterConfig9;
  public
    FlipHorizontal: Boolean;
    FlipVertical: Boolean;
    FOnError: TVFErrorEvent;
    height: Integer;
    pMFMixerBitmap: IMFVideoMixerBitmap;
    pMFVideoDisplayControl: IMFVideoDisplayControl;
    pVideoRenderer: IBaseFilter;
    pVideoWindow: IVideoWindow;
    pVMRMixerBitmap9: IVMRMixerBitmap9;
    pVMRMixerControl9: IVMRMixerControl9;
    pVMRWindowlessControl: IVMRWindowlessControl9;
    Renderer: TVFVideoRenderer;
    RendererDeinterlaceMode: WideString;
    RendererDeinterlaceUseDefault: Boolean;
    ScreenHandle: cardinal;
    Screen_Zoom_Ratio: Integer;
    Screen_Zoom_ShiftX: Integer;
    Screen_Zoom_ShiftY: Integer;
    Stretch: Boolean;
    VideoInHeight: Integer;
    VideoInWidth: Integer;
    width: Integer;
    constructor Create;
    destructor Destroy; override;
    procedure Clear;
    procedure ClearVideoWindow;
    function ConnectDirectIn(filterGraph: IFilterGraph2; pOut: IPin): HRESULT;
    function ConnectDirectInX(filterGraph: IFilterGraph2; Index: Integer;
      pOut: IPin): HRESULT;
    function ConnectIn(filterGraph: IFilterGraph2; pOut: IPin)
      : HRESULT; virtual;
    function ConnectInX(filterGraph: IFilterGraph2; Index: Integer;
      pOut: IPin): HRESULT;
    procedure GetInPin(Index: Integer; var pPin: IPin);
    procedure Init_EVR(filterGraph: IFilterGraph2; PIP_Count: Integer);
    procedure Init_None(filterGraph: IFilterGraph2);
    procedure Init_Renderer(filterGraph: IFilterGraph2; PIP_Count: Integer);
    procedure Init_VMR9(filterGraph: IFilterGraph2; PIP_Count: Integer);
    procedure Init_VR(filterGraph: IFilterGraph2);
    procedure OnErrorS(const Text: WideString);
    procedure Update_EVR(filterGraph: IFilterGraph2);
    procedure Update_Renderer(filterGraph: IFilterGraph2);
    procedure Update_VMR9(filterGraph: IFilterGraph2);
    procedure Update_VR(filterGraph: IFilterGraph2);
  published
    property DebugMode: Boolean read FDebugMode write FDebugMode;
    property Aspect_Ratio_Override: Boolean read FAspect_Ratio_Override
      write FAspect_Ratio_Override;
    property Aspect_Ratio_X: Integer read FAspect_Ratio_X write FAspect_Ratio_X;
    property Aspect_Ratio_Y: Integer read FAspect_Ratio_Y write FAspect_Ratio_Y;
    property BorderColor: TColor read FBorderColor write FBorderColor;
    property ForcedRect: TRect read FForcedRect write FForcedRect;
    property ForceRect: Boolean read FForceRect write FForceRect;
    // property Filter: IBaseFilter read FFilter write FFilter;
  end;

  // 4.1
const
  MEDIASUBTYPE_MP42: TGUID = '{3234504D-0000-0010-8000-00AA00389B71}';
  MEDIASUBTYPE_DIVX: TGUID = '{58564944-0000-0010-8000-00AA00389B71}';
  MEDIASUBTYPE_VOXWARE: TGUID = '{00000075-0000-0010-8000-00AA00389B71}';
  IID_IPropertyBag: TGUID = '{55272A00-42CB-11CE-8135-00AA004BB851}';
  IID_ISpecifyPropertyPages: TGUID = '{B196B28B-BAB4-101A-B69C-00AA00341D07}';
  IID_IPersistStream: TGUID = '{00000109-0000-0000-C000-000000000046}';
  IID_IServiceProvider: TGUID = '{6D5140C1-7436-11CE-8034-00AA006009FA}';
  MEDIASUBTYPE_MPEG2_TRANSPORT_STRIDE
    : TGUID = '{138AA9A4-1EE2-4c5b-988E-19ABFDBC8A11}';
  dskey = 'CLSID\{083863F1-70DE-11D0-BD40-00A0C911CE86}\Instance\';
{$IFDEF VF_EDIT}

  // MPEG-2 PSI parser
const
  IID_IMpeg2PsiParser: TGUID = '{AE1A2884-540E-4077-B1AB-67A34A72298C}';
  CLSID_VFPSIParser: TGUID = '{DDF7480E-13E2-4481-BA2B-3C17C4FC469F}';

type
  IMpeg2PsiParser = interface(IUnknown)
    function FindRecordProgramMapPid(wProgramNumber: WORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function GetCountOfElementaryStreams(wProgramNumber: WORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function GetCountOfPrograms(out pNumOfPrograms: Integer): HRESULT; stdcall;
    function GetPatVersionNumber(out pPatVersion: byte): HRESULT; stdcall;
    function GetPmtVersionNumber(wProgramNumber: WORD; out pPmtVersion: byte)
      : HRESULT; stdcall;
    function GetRecordElementaryPid(wProgramNumber: WORD; dwRecordIndex: DWORD;
      out pwVal: WORD): HRESULT; stdcall;
    function GetRecordProgramMapPid(dwIndex: DWORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function GetRecordProgramNumber(dwIndex: DWORD; out pwVal: WORD)
      : HRESULT; stdcall;
    function GetRecordStreamType(wProgramNumber: WORD; dwRecordIndex: DWORD;
      out pbVal: byte): HRESULT; stdcall;
    function GetTransportStreamId(out pStreamId: WORD): HRESULT; stdcall;
  end;
{$ENDIF}

type
  // IMediaStore
  (*
    PPROPERTYKEY = ^TPROPERTYKEY;
    TPROPERTYKEY = record
    fmtid: TGUID;
    pid: DWORD;
    end;

    IInitializeWithFile = interface(IUnknown)
    ['{b7d14566-0509-4cce-a71f-0a554233bd9b}']
    function Initialize(pszFilePath: PWideChar; grfMode: DWORD):
    HRESULT; stdcall;
    end;

    IInitializeWithStream = interface(IUnknown)
    ['{b824b49d-22ac-4161-ac8a-9916e8fa3f7f}']
    function Initialize(var pIStream: IStream; grfMode: DWORD):
    HRESULT; stdcall;
    end;

    IPropertyStore = interface(IUnknown)
    ['{886d8eeb-8cf2-4446-8d02-cdba1dbdcf99}']
    function GetCount(out cProps: DWORD): HRESULT; stdcall;
    function GetAt(iProp: DWORD; out PropKey: TPropertyKey): HRESULT;
    stdcall;
    function GetValue(pPropKey: PPropertyKey; out PropVar:
    TPropVariant): HRESULT; stdcall;
    function SetValue(pPropKey: PPropertyKey; pPropVar: PPropVariant):
    HRESULT; stdcall;
    function Commit: HRESULT; stdcall;
    end;

    IPropertyStoreCapabilities = interface(IUnknown)
    ['{c8e2d566-186e-4d49-bf41-6909ead56acc}']
    function IsPropertyWritable(pPropKey: PPropertyKey): HRESULT; stdcall;
    end;
  *)

  // Main

  VF_StreamType = (VF_ST_Audio, VF_ST_Video, VF_ST_Any);

  TVFPinList = class(TInterfaceList)
  private
    Filter: IBaseFilter;
    function GetConnected(Index: Integer): Boolean;
    function GetPin(Index: Integer): IPin;
    function GetPinInfo(Index: Integer): TPinInfo;
    procedure PutPin(Index: Integer; item: IPin);
  public
    constructor Create(BaseFilter: IBaseFilter); overload;
    destructor Destroy; override;
    function Add(item: IPin): Integer;
    procedure Assign(BaseFilter: IBaseFilter);
    function First: IPin;
    function IndexOf(item: IPin): Integer;
    procedure Insert(Index: Integer; item: IPin);
    function Last: IPin;
    function Remove(item: IPin): Integer;
    procedure Update;
    property Connected[Index: Integer]: Boolean read GetConnected;
    property Items[Index: Integer]: IPin read GetPin write PutPin; default;
    property PinInfo[Index: Integer]: TPinInfo read GetPinInfo;
  end;

  TVFFilterList = class(TInterfaceList)
  private
    Graph: IFilterGraph;
    function GetFilter(Index: Integer): IBaseFilter;
    function GetFilterInfo(Index: Integer): TFilterInfo;
    procedure PutFilter(Index: Integer; item: IBaseFilter);
  public
    constructor Create(filterGraph: IFilterGraph); overload;
    destructor Destroy; override;
    function Add(item: IBaseFilter): Integer;
    procedure Assign(filterGraph: IFilterGraph);
    function First: IBaseFilter;
    function IndexOf(item: IBaseFilter): Integer;
    procedure Insert(Index: Integer; item: IBaseFilter);
    function Last: IBaseFilter;
    function Remove(item: IBaseFilter): Integer;
    procedure Update;
    property FilterInfo[Index: Integer]: TFilterInfo read GetFilterInfo;
    property Items[Index: Integer]: IBaseFilter read GetFilter
      write PutFilter; default;
  end;

  TVFMediaType = class(TPersistent)
  private
    function GetFormatType: TGUID;
    function GetMajorType: TGUID;
    function GetSubType: TGUID;
    procedure ReadData(Stream: TStream);
    procedure SetFormatType(const guid: TGUID);
    procedure SetMajorType(MT: TGUID);
    procedure SetSubType(ST: TGUID);
    procedure WriteData(Stream: TStream);
  protected
    { @exclude }
    procedure DefineProperties(Filer: TFiler); override;
  public
    AMMediaType: PAMMEDIATYPE;
    constructor Create; overload;
    constructor Create(MediaType: PAMMEDIATYPE); overload;
    constructor Create(majortype: TGUID); overload;
    constructor Create(MTClass: TVFMediaType); overload;
    destructor Destroy; override;
    function AllocFormatBuffer(length: ULONG): pointer;
    procedure Assign(Source: TPersistent); override;
    function Equal(MTClass: TVFMediaType): Boolean; overload;
    function format: pointer;
    function FormatLength: ULONG;
    function GetSampleSize: ULONG;
    procedure InitMediaType;
    function IsFixedSize: Boolean;
    function IsPartiallySpecified: Boolean;
    function IsTemporalCompressed: Boolean;
    function IsValid: Boolean;
    function MatchesPartial(ppartial: TVFMediaType): Boolean;
    function NotEqual(MTClass: TVFMediaType): Boolean; overload;
    procedure Read(MediaType: PAMMEDIATYPE);
    function ReallocFormatBuffer(length: ULONG): pointer;
    procedure ResetFormatBuffer;
    function SetFormat(pFormat: pointer; length: ULONG): Boolean;
    procedure SetSampleSize(SZ: ULONG);
    procedure SetTemporalCompression(bCompressed: Boolean);
    procedure SetVariableSize;
    property formattype: TGUID read GetFormatType write SetFormatType;
    property majortype: TGUID read GetMajorType write SetMajorType;
    property subtype: TGUID read GetSubType write SetSubType;
  end;

  TVFEnumMediaType = class(TObject)
  private
    FList: TList;
    function GetCount: Integer;
    function GetItem(Index: Integer): TVFMediaType;
    function GetMediaDescription(Index: Integer): WideString;
    procedure SetItem(Index: Integer; item: TVFMediaType);
  public
    constructor Create; overload;
    constructor Create(EnumMT: IEnumMediaTypes); overload;
    constructor Create(Pin: IPin); overload;
    constructor Create(FileName: TFileName); overload;
    destructor Destroy; override;
    function Add(item: TVFMediaType): Integer;
    procedure Assign(EnumMT: IEnumMediaTypes); overload;
    procedure Assign(Pin: IPin); overload;
    procedure Assign(FileName: TFileName); overload;
    procedure Clear;
    procedure Delete(Index: Integer);
    property Count: Integer read GetCount;
    property Items[Index: Integer]: TVFMediaType read GetItem write SetItem;
    property MediaDescription[Index: Integer]: WideString
      read GetMediaDescription;
  end;
{$WARNINGS OFF}

  TVFGraphCallbacks = class(TComponent, IAMGraphBuilderCallback)
  private
    FBlackList: TStringList;
    FBlackListByCLSID: TStringList;
  public
    constructor Create;
    destructor Destroy; override;
    procedure AssignToGraph(pGB: IGraphBuilder);
    function CreatedFilter(pFil: IBaseFilter): HRESULT; stdcall;
    function SelectedFilter(pMon: IMoniker): HRESULT; stdcall;
  published
    property BlackList: TStringList read FBlackList write FBlackList;
    property BlackListByCLSID: TStringList read FBlackListByCLSID
      write FBlackListByCLSID;
  end;
{$WARNINGS ON}

  TVFPin = class(TObject)
  public
    Formats: TObjectList;
    name: WideString;
    constructor Create;
    destructor Destroy; override;
  end;

const
  PixelCountMax = 32768;

type
  pRGBArray = ^TRGBArray;
  TRGBArray = array [0 .. PixelCountMax - 1] of TRGBTriple;
  pRGBAArray = ^TRGBAArray;
  TRGBAArray = array [0 .. PixelCountMax - 1] of TRGBQuad;

function AddGraphToRot(Graph: IFilterGraph; out ID: Integer): HRESULT;
procedure DeleteMediaType(pmt: PAMMEDIATYPE);
procedure FPUMask(enableMask: Boolean);
function DetectVideoCaptureDeviceType(pVideoIn: IBaseFilter;
  device_name: WideString): TVFDeviceType;
procedure FreeMediaType(MT: PAMMEDIATYPE);
function GetErrorCodeAsString(hr: HRESULT): WideString;
function GetFOURCC(FOURCC: cardinal): AnsiString;
function GetFreePinWithMediaType(pFilter: IBaseFilter; pDir: PIN_DIRECTION;
  guid: TGUID; var pPin: IPin): Boolean;
function GetPin(pFilter: IBaseFilter; pDir: PIN_DIRECTION; pIndex: byte;
  var pPin: IPin): Boolean;
function GetZoomPos(SrcWidth, SrcHeight, ShiftX, ShiftY, zoom: Integer): TRect;
function HBITMAPToInt(Value: HBITMAP): Integer;
procedure AddImageOverlay(src: pointer; src_width, src_height: Integer;
  dest: pointer; dest_width, dest_height: Integer);
function IntToHBITMAP(Value: Integer): HBITMAP;
function LoadWMProfileFromFile(FilePath: WideString; ms: TMemoryStream)
  : Boolean;
function PinHaveThisType(Pin: IPin; MediaType: TGUID): Boolean;
function RemoveGraphFromRot(ID: Integer): HRESULT;
// function SampleGrabber_GetBitmap(bitmap: TBitmap; pSample: IMediaSample; SampleGrabber: ISampleGrabber;
// screenshot: Boolean; logo: pointer; logo_width, logo_height: Integer): Boolean;
function GetBitmapFromBuffer(bitmap: TBitmap; buffer: PByte;
  bufferSize: Integer; width, height: Integer; screenshot: Boolean;
  logo: pointer; logo_width, logo_height: Integer): Boolean;
function ConvertBitmapToBuffer(bitmap: TBitmap; out buffer: PByte): Boolean;
function GetMediaTypeDescription(MediaType: PAMMEDIATYPE): WideString;
function HaveFilterPropertyPage(Filter: IBaseFilter;
  PropertyPage: TVFPropertyPage = ppDefault): Boolean;
function ImageCompare(Img1, Img2: TBitmap; Difference: byte;
  var FinalDiff: Integer; ReadDiff: Boolean; var imgDiff: TBitmap;
  var ImgDiffTop, ImgDiffLeft: Integer; var DiffPerc: Double): Boolean;
function LoadGraphFile(pGraph: IGraphBuilder;
  const wszName: WideString): HRESULT;
function SaveGraphFile(pGraph: IGraphBuilder; wszPath: WideString): HRESULT;
procedure SaveGraphAsText(pGraph: IGraphBuilder; FileName: WideString);
function ShowFilterPropertyPage(parent: THandle; Filter: IBaseFilter;
  PropertyPage: TVFPropertyPage = ppDefault): HRESULT;
function VMR9DeinterlaceModeToStr(ds: VMR9DeinterlacePrefs): WideString;
function GetEnvironmentVariableW(const name: WideString): WideString;
function AudioInput_SetFormat(pFilter: IBaseFilter; pOutput: IPin;
  format: WideString; UseBestFormat: Boolean; Line: WideString;
  var pAudioInMixer: IAMAudioInputMixer): Integer;
function PinConnected(pPin: IPin): Boolean;
function GetLetterboxCoordinates(ScreenWidth, ScreenHeight, SrcWidth,
  SrcHeight: Integer): TRect;
function DumpTBitmapToBuffer(bitmap: TBitmap; pBuf: pointer; size: Integer;
  var Depth32b: Boolean): Boolean;

function GetPinVideoInfo(Pin: IPin; out width: Integer;
  out height: Integer): Boolean;
procedure ZoomBitmap(imgInput: TBitmap; var imgOutput: TBitmap; zoom: Double);

function VF_GetFileSize(FileName: WideString): Int64;

function FilterSupportedEVR(dsFilters: TVFSysDevEnum): Boolean;
function FilterSupportedVMR9(dsFilters: TVFSysDevEnum): Boolean;
procedure FindSplitter(pGraph: IGraphBuilder; var FilterName: WideString);
function CountFilterPins(pFilter: IBaseFilter; out pulInPins: cardinal;
  out pulOutPins: cardinal): HRESULT;
function IsFilterConnected(pFilter: IBaseFilter): Boolean;
procedure ShowPinInfo(pPin: IPin);
procedure ShowFilterInfo(pFilter: IBaseFilter);

procedure FilterConnectedTo(pSrc: IBaseFilter; dir: PIN_DIRECTION;
  var pDest: IBaseFilter);
procedure DisconnectFilter(pFilter: IBaseFilter);
function GetFilterVideoOutputPin(pFilter: IBaseFilter; var pPin: IPin): Boolean;

function ReadMediaInfoDeep(FileName: WideString;
  out width, height: Integer): Boolean;

function AddFilterFromCLSID(filterGraph: IFilterGraph2; CLSID: TGUID;
  name: WideString): IBaseFilter;

implementation

// FFMPEG methods
function FFPlayerInit(filterGraph2: IFilterGraph2): LongBool; stdcall;
  external 'VFFFMPEG.dll';
function FFPlayerOpen(FileName: PWideChar; FrameRate: Double;
  StartTime, StopTime: Integer): LongBool; stdcall; external 'VFFFMPEG.dll';
function FFPlayerOpenMemory(memorySource: IStream): LongBool; stdcall;
  external 'VFFFMPEG.dll';
function FFPlayerGetFilter(out Filter: IBaseFilter): Integer; stdcall;
  external 'VFFFMPEG.dll';
function FFPlayerGetPins(out videoPin: IPin; out audioPin: IPin): Integer;
  stdcall; external 'VFFFMPEG.dll';
function FFPlayerUninit: LongBool; stdcall; external 'VFFFMPEG.dll';
function FFPlayerOpenDevice(FileName: PWideChar; FrameRate: Double): LongBool;
  stdcall; external 'VFFFMPEG.dll';
function FFPlayerOpenScreen(screenID: Integer; FrameRate: Double;
  Left, Top, Right, Bottom: Integer): LongBool; stdcall;
  external 'VFFFMPEG.dll';
function FFConverterInit(): LongBool; stdcall; external 'VFFFMPEG.dll';
function FFConverterUninit: LongBool; stdcall; external 'VFFFMPEG.dll';
function FFConverterDirectCapture(sourceFile, outputFile, format: PWideChar)
  : LongBool; stdcall; external 'VFFFMPEG.dll';
function FFConverterStop: LongBool; stdcall; external 'VFFFMPEG.dll';

constructor TDSScreen.Create;
begin
  inherited;

  pVideoRenderer := nil;
  FForceRect := false;
  FAspect_Ratio_Override := false;
  FAspect_Ratio_X := 16;
  FAspect_Ratio_Y := 9;

  FBorderColor := clBlack;
end;

destructor TDSScreen.Destroy;
begin
  Clear;

  inherited;
end;

procedure TDSScreen.Clear;
begin
  try
    if Assigned(pVMRDeinterlaceControl9) then
      pVMRDeinterlaceControl9 := nil;

    if Assigned(pVMRWindowlessControl) then
      pVMRWindowlessControl := nil;
    if Assigned(pVMRFilterConfig) then
      pVMRFilterConfig := nil;
    if Assigned(pVMRMixerBitmap9) then
      pVMRMixerBitmap9 := nil;

    if Assigned(pEVRFilterConfig) then
      pEVRFilterConfig := nil;
    if Assigned(pMFGetService) then
      pMFGetService := nil;
    if Assigned(pMFVideoRenderer) then
      pMFVideoRenderer := nil;
    if Assigned(pMFVideoDisplayControl) then
      pMFVideoDisplayControl := nil;
    if Assigned(pMFMixerBitmap) then
      pMFMixerBitmap := nil;

    ClearVideoWindow;

    if Assigned(pVideoRenderer) then
      pVideoRenderer := nil;

    // if Assigned(pGraphBuilder) then
    // pGraphBuilder := nil;
  except
    ;
  end;
end;

procedure TDSScreen.ClearVideoWindow;
begin
  if Assigned(pVideoWindow) then
  begin
    pVideoWindow.put_Visible(false);
    pVideoWindow.put_Owner(0);
    pVideoWindow.put_MessageDrain(0);
  end;
end;

function TDSScreen.ConnectDirectIn(filterGraph: IFilterGraph2;
  pOut: IPin): HRESULT;
var
  pPin: IPin;
begin
  result := E_FAIL;

  GetPin(pVideoRenderer, PINDIR_INPUT, 1, pPin);

  if pPin <> nil then
  begin
    result := filterGraph.ConnectDirect(pOut, pPin, nil);
    pPin := nil;
  end;
end;

function TDSScreen.ConnectDirectInX(filterGraph: IFilterGraph2; Index: Integer;
  pOut: IPin): HRESULT;
var
  pPin: IPin;
begin
  result := E_FAIL;

  GetPin(pVideoRenderer, PINDIR_INPUT, index, pPin);

  if pPin <> nil then
  begin
    result := filterGraph.ConnectDirect(pOut, pPin, nil);
    pPin := nil;
  end;
end;

function TDSScreen.ConnectIn(filterGraph: IFilterGraph2; pOut: IPin): HRESULT;
var
  pPin: IPin;
begin
  result := E_FAIL;

  GetPin(pVideoRenderer, PINDIR_INPUT, 1, pPin);

  if pPin <> nil then
  begin
    result := filterGraph.Connect(pOut, pPin);
    pPin := nil;
  end;
end;

function TDSScreen.ConnectInX(filterGraph: IFilterGraph2; Index: Integer;
  pOut: IPin): HRESULT;
var
  pPin: IPin;
begin
  result := E_FAIL;

  GetPin(pVideoRenderer, PINDIR_INPUT, index, pPin);

  if pPin <> nil then
  begin
    result := filterGraph.Connect(pOut, pPin);
    pPin := nil;
  end;
end;

procedure TDSScreen.GetInPin(Index: Integer; var pPin: IPin);
begin
  GetPin(pVideoRenderer, PINDIR_INPUT, Index + 1, pPin);
end;

function TDSScreen.GetLetterboxRect(ScreenWidth, ScreenHeight, AspectX,
  AspectY: Integer): TRect;
var
  dc: HDC;
  // pBasicVideo: IBasicVideo2;
  VideoScreenWidth, VideoScreenHeight, ClipWidth, ClipHeight, NewImageWidth,
    NewImageHeight, NewImageLeft, NewImageTop: Integer;
begin
  try
    if ScreenWidth = 0 then
    begin
      if (FullScreen) then
      begin
        dc := CreateDC('DISPLAY', nil, nil, nil);
        VideoScreenWidth := GetDeviceCaps(dc, HORZRES);
        VideoScreenHeight := GetDeviceCaps(dc, VERTRES);
        DeleteDC(dc);
      end
      else
      begin
        VideoScreenWidth := width;
        VideoScreenHeight := height;
      end;
    end
    else
    begin
      VideoScreenWidth := ScreenWidth;
      VideoScreenHeight := ScreenHeight;
    end;

    // if ((DSI.pGraphBuilder.QueryInterface(IID_IBasicVideo2, pBasicVideo) = S_OK)) then
    // begin
    ClipWidth := VideoInWidth;
    ClipHeight := VideoInHeight;

    if (AspectX = 1) and (AspectY = 1) then
    begin
      // pBasicVideo.get_VideoWidth(ClipWidth);
      // pBasicVideo.get_VideoHeight(ClipHeight);
    end
    else
    begin
      ClipWidth := AspectX;
      ClipHeight := AspectY;
    end;

    NewImageWidth := 0;
    NewImageHeight := 0;

    if ((VideoScreenWidth > 0) and (VideoScreenHeight > 0)) then
    begin
      if ((ClipWidth / ClipHeight) > (VideoScreenWidth / VideoScreenHeight))
      then
      begin
        NewImageWidth := VideoScreenWidth;
        NewImageHeight := round(ClipHeight / ClipWidth * VideoScreenWidth);
      end
      else
      begin
        NewImageWidth := round(ClipWidth / ClipHeight * VideoScreenHeight);
        NewImageHeight := VideoScreenHeight;
      end
    end;

    NewImageLeft := round((VideoScreenWidth / 2) - (NewImageWidth / 2));
    NewImageTop := round((VideoScreenHeight / 2) - (NewImageHeight / 2));

    result.Left := NewImageLeft;
    result.Top := NewImageTop;
    result.Right := NewImageWidth + NewImageLeft;
    result.Bottom := NewImageHeight + NewImageTop;

    // pBasicVideo := nil;
    // end;
  except
    result.Left := 0;
    result.Top := 0;
    result.Bottom := ScreenHeight;
    result.Right := ScreenWidth;
  end;
end;

procedure TDSScreen.Init_EVR(filterGraph: IFilterGraph2; PIP_Count: Integer);
var
  dwNumStreams: DWORD; // Number of streams to use.
  hr: HRESULT;
begin
  CoCreateInstance(CLSID_EnhancedVideoRenderer, nil, CLSCTX_INPROC_SERVER,
    IID_IBaseFilter, pVideoRenderer);
  filterGraph.AddFilter(pVideoRenderer, StringToOleStr('EVR'));

  if (filterGraph = nil) or (pVideoRenderer = nil) then
    Exit;

  dwNumStreams := PIP_Count + 1;

  hr := pVideoRenderer.QueryInterface(IID_IMFVideoRenderer, pMFVideoRenderer);
  if hr = S_OK then
  begin
    // FDS_pMFVideoRenderer.InitializeRenderer(dwNumStreams);
  end
  else
    OnErrorS('Unable to query IMFVideoRenderer interface.');

  hr := pVideoRenderer.QueryInterface(IID_IEVRFilterConfig, pEVRFilterConfig);
  if hr = S_OK then
  begin
    // FDS_pEVRFilterConfig.SetNumberOfStreams(dwNumStreams);
  end
  else
    OnErrorS('Unable to get IEVRFilterConfig interface.');

  hr := pVideoRenderer.QueryInterface(IID_IMFGetService, pMFGetService);
  if hr = S_OK then
  begin
    // FDS_pEVRFilterConfig.SetNumberOfStreams(dwNumStreams);
  end
  else
  begin
    OnErrorS('Unable to get IMFGetService interface.');
    Exit;
  end;

  hr := pMFGetService.GetService(MR_VIDEO_RENDER_SERVICE,
    IID_IMFVideoDisplayControl, pMFVideoDisplayControl);
  if hr = S_OK then
  begin
    pMFVideoDisplayControl.SetVideoWindow(ScreenHandle);
  end
  else
    OnErrorS('Unable to get IMFVideoDisplayControl interface.');

  hr := pMFGetService.GetService(MR_VIDEO_MIXER_SERVICE,
    IID_IMFVideoMixerControl, pMFVideoMixerControl);
  if hr <> S_OK then
    OnErrorS('Unable to get IMFVideoMixerControl interface.');

  hr := pMFGetService.GetService(MR_VIDEO_MIXER_SERVICE,
    IID_IMFVideoMixerBitmap, pMFMixerBitmap);
  if hr <> S_OK then
    OnErrorS('Unable to get IMFVideoMixerBitmap interface.');
end;

procedure TDSScreen.Init_None(filterGraph: IFilterGraph2);
begin
  CoCreateInstance(CLSID_NullRenderer, nil, CLSCTX_INPROC_SERVER,
    IID_IBaseFilter, pVideoRenderer);
  filterGraph.AddFilter(pVideoRenderer, StringToOleStr('Screen NULL Renderer'));
end;

procedure TDSScreen.Init_Renderer(filterGraph: IFilterGraph2;
  PIP_Count: Integer);
begin
  if Renderer = VR_VideoRenderer then
    Init_VR(filterGraph)
  else if Renderer = VR_VMR9 then
    Init_VMR9(filterGraph, PIP_Count)
  else if Renderer = VR_EVR then
    Init_EVR(filterGraph, PIP_Count)
  else
    Init_None(filterGraph);
end;

procedure TDSScreen.Init_VMR9(filterGraph: IFilterGraph2; PIP_Count: Integer);
var
  hr: HRESULT;
  dwNumStreams: DWORD;
begin
  CoCreateInstance(CLSID_VideoMixingRenderer9, nil, CLSCTX_INPROC_SERVER,
    IID_IBaseFilter, pVideoRenderer);
  filterGraph.AddFilter(pVideoRenderer, StringToOleStr('VMR9'));

  if (filterGraph = nil) or (pVideoRenderer = nil) then
    Exit;

  hr := pVideoRenderer.QueryInterface(IID_IVMRFilterConfig9, pVMRFilterConfig);

  if hr <> S_OK then
    Exit;

  // Set the rendering mode and number of streams.
  pVMRFilterConfig.SetRenderingMode(VMRMode_Windowless);

  dwNumStreams := PIP_Count + 1;
  pVMRFilterConfig.SetNumberOfStreams(dwNumStreams);

  hr := pVideoRenderer.QueryInterface(IID_IVMRWindowlessControl9,
    pVMRWindowlessControl);

  if hr = S_OK then
    pVMRWindowlessControl.SetVideoClippingWindow(ScreenHandle);

  hr := pVideoRenderer.QueryInterface(IID_IVMRMixerControl9, pVMRMixerControl9);
  if hr <> S_OK then
    OnErrorS('Unable to get IVMRMixerControl9 interface.');

  hr := pVideoRenderer.QueryInterface(IID_IVMRMixerBitmap9, pVMRMixerBitmap9);
  if hr <> S_OK then
    OnErrorS('Unable to get IVMRMixerBitmap9 interface.');

  hr := pVideoRenderer.QueryInterface(IID_IVMRDeinterlaceControl9,
    pVMRDeinterlaceControl9);
  if hr <> S_OK then; // OnErrorS(153);
end;

procedure TDSScreen.Init_VR(filterGraph: IFilterGraph2);
var
  hr: HRESULT;
begin
  CoCreateInstance(CLSID_VideoRenderer, nil, CLSCTX_INPROC_SERVER,
    IID_IBaseFilter, pVideoRenderer);
  filterGraph.AddFilter(pVideoRenderer, StringToOleStr('Video Renderer'));

  hr := pVideoRenderer.QueryInterface(IID_IVideoWindow, pVideoWindow);

  if hr <> S_OK then
    OnErrorS('Unable to get IVideoWindow interface.')
  else
  begin
    pVideoWindow.put_MessageDrain(ScreenHandle);
    pVideoWindow.put_Owner(ScreenHandle);
  end;
end;

procedure TDSScreen.OnErrorS(const Text: WideString);
begin
  try
    if (Assigned(FOnError)) then
      FOnError(Text);
  except
    ;
  end;
end;

procedure TDSScreen.Update_EVR(filterGraph: IFilterGraph2);
var
  rect_dest, rect_tmp: TRect;
  rect_src: TMFVideoNormalizedRect;
  video_size, video_size_ar: TSIZE;
begin
  try
    if ((pMFVideoDisplayControl <> nil) and (filterGraph <> nil) and
      (pMFVideoMixerControl <> nil)) then
    begin
      // if (VideoInWidth = 0) or (VideoInHeight = 0) then
      // begin
      pMFVideoDisplayControl.GetNativeVideoSize(video_size, video_size_ar);
      VideoInWidth := video_size.cx;
      VideoInHeight := video_size.cy;
      // end;

      pMFVideoDisplayControl.SetBorderColor(FBorderColor);

      rect_dest.Left := 0;
      rect_dest.Top := 0;
      rect_dest.Right := width;
      rect_dest.Bottom := height;

      // Custom Aspect Ratio disabeled, waiting for Vista patch, maybe in Win7 it will work correctly...
      // if not Stretch then
      // begin
      // if FAspect_Ratio_Override then
      // rect_dest := GetLetterboxRect(Width, Height, FAspect_Ratio_X, FAspect_Ratio_Y)
      // else
      // begin
      // rect_dest.Left := 0;
      // rect_dest.Top := 0;
      // rect_dest.Right := Width;
      // rect_dest.Bottom := Height;
      // end;
      // end
      // else
      // begin
      // rect_dest.Left := 0;
      // rect_dest.Top := 0;
      // rect_dest.Right := Width;
      // rect_dest.Bottom := Height;
      // end;

      if (Screen_Zoom_Ratio > 0) and (VideoInWidth > 0) and (VideoInHeight > 0)
      then
      begin
        rect_tmp := GetZoomPos(VideoInWidth, VideoInHeight, Screen_Zoom_ShiftX,
          Screen_Zoom_ShiftY, Screen_Zoom_Ratio);
        rect_src.Left := rect_tmp.Left / VideoInWidth;
        rect_src.Right := rect_tmp.Right / VideoInWidth;
        rect_src.Top := rect_tmp.Top / VideoInHeight;
        rect_src.Bottom := rect_tmp.Bottom / VideoInHeight;

        pMFVideoDisplayControl.SetVideoPosition(@rect_src, @rect_dest);
      end
      else
      begin
        rect_src.Left := 0;
        rect_src.Top := 0;
        rect_src.Right := 1;
        rect_src.Bottom := 1;

        pMFVideoDisplayControl.SetVideoPosition(@rect_src, @rect_dest);
      end;

      if Stretch { or FAspect_Ratio_Override } then
        pMFVideoDisplayControl.SetAspectRatioMode(MFVideoARMode_None)
      else
        pMFVideoDisplayControl.SetAspectRatioMode
          (MFVideoARMode_PreservePicture);

      pMFVideoDisplayControl.RepaintVideo();
    end;
  except
    ;
  end;
end;

procedure TDSScreen.Update_Renderer(filterGraph: IFilterGraph2);
begin
  if Renderer = VR_VideoRenderer then
    Update_VR(filterGraph)
  else if Renderer = VR_VMR9 then
    Update_VMR9(filterGraph)
  else if Renderer = VR_EVR then
    Update_EVR(filterGraph);
end;

procedure TDSScreen.Update_VMR9(filterGraph: IFilterGraph2);
var
  rect_dest, rect_source: TRect;
  hr: HRESULT;
  vd: VMR9VideoDesc;
  num: cardinal;
  modes: array [0 .. 10] of TGUID;
  i: Integer;
  caps: VMR9DeinterlaceCaps;
  nrectOld, nrectNew: TVMR9NormalizedRect;
  S: WideString;
  hdc1: HDC;
  ps: PAINTSTRUCT;
  arx, ary: Integer;

  // pBasicVideo2: IBasicVideo2;

  // Ratio: real;
  // TmpX, TmpY: real;
  // TmpLeft, TmpTop: real;
  // SLeft, Stop, SWidth, SHeight: Integer;

begin
  try
    if ((pVMRWindowlessControl <> nil) and (filterGraph <> nil) and
      (pVMRFilterConfig <> nil)) then
    begin
      // if (VideoInWidth = 0) or (VideoInHeight = 0) then
      pVMRWindowlessControl.GetNativeVideoSize(VideoInWidth, VideoInHeight,
        arx, ary);

      // Set the rendering mode and number of streams.
      pVMRFilterConfig.SetRenderingMode(VMRMode_Windowless);
      pVMRWindowlessControl.SetVideoClippingWindow(ScreenHandle);
      pVMRWindowlessControl.SetBorderColor(FBorderColor);

      if Stretch or FAspect_Ratio_Override then
        pVMRWindowlessControl.SetAspectRatioMode(VMR9ARMode_None)
      else
        pVMRWindowlessControl.SetAspectRatioMode(VMR9ARMode_LetterBox);

      // flip
      if pVMRMixerControl9 <> nil then
      begin
        pVMRMixerControl9.GetOutputRect(0, @nrectOld);
        nrectNew := nrectOld;

        if FlipHorizontal then
        begin
          nrectNew.Left := 1;
          nrectNew.Right := 0;
        end
        else
        begin
          nrectNew.Left := 0;
          nrectNew.Right := 1;
        end;

        if FlipVertical then
        begin
          nrectNew.Top := 1;
          nrectNew.Bottom := 0;
        end
        else
        begin
          nrectNew.Top := 0;
          nrectNew.Bottom := 1;
        end;

        pVMRMixerControl9.SetOutputRect(0, @nrectNew);
      end;

      if not Stretch then
      begin
        if FAspect_Ratio_Override then
          rect_dest := GetLetterboxRect(width, height, FAspect_Ratio_X,
            FAspect_Ratio_Y)
        else
        begin
          rect_dest.Left := 0;
          rect_dest.Top := 0;
          rect_dest.Right := width;
          rect_dest.Bottom := height;
        end;
      end
      else
      begin
        rect_dest.Left := 0;
        rect_dest.Top := 0;
        rect_dest.Right := width;
        rect_dest.Bottom := height;
      end;

      if (VideoInWidth > 0) and (VideoInHeight > 0) then
      begin
        if (Screen_Zoom_Ratio > 0) then
        begin
          // pBasicVideo2 := nil;
          //
          // try
          // if (pVideoRenderer.QueryInterface(IID_IBasicVideo2, pBasicVideo2) = S_OK) then
          // begin
          // pBasicVideo2.SetDefaultSourcePosition;
          // pBasicVideo2.get_SourceLeft(SLeft);
          // pBasicVideo2.get_SourceTop(STop);
          // pBasicVideo2.get_SourceWidth(SWidth);
          // pBasicVideo2.get_SourceHeight(SHeight);
          //
          // Ratio := SHeight / SWidth;
          //
          // TmpX := SWidth - ((80 * Swidth) / 100);
          // TmpY := TmpX * Ratio;
          //
          // TmpLeft := (SWidth - TmpX) / 2;
          // TmpTop := (SHeight - TmpY) / 2;
          //
          // pBasicVideo2.put_SourceWidth(Trunc(TmpX));
          // pBasicVideo2.put_SourceHeight(Trunc(TmpY));
          // pBasicVideo2.put_SourceLeft(Trunc(TmpLeft));
          // pBasicVideo2.put_SourceTop(Trunc(TmpTop));
          // end;
          //
          // //FZoom := Value;
          // finally
          // pBasicVideo2 := nil;
          // end;

          // rect_source.Left := 0;
          // rect_source.Top := 0;
          // rect_source.Right := VideoInWidth;
          // rect_source.Bottom := VideoInHeight;

          rect_source := GetZoomPos(VideoInWidth, VideoInHeight,
            Screen_Zoom_ShiftX, Screen_Zoom_ShiftY, Screen_Zoom_Ratio);

          // rect_source.Left := 60;
          // rect_source.Top := 60;
          // rect_source.Right := 300;
          // rect_source.Bottom := 200;

          hr := pVMRWindowlessControl.SetVideoPosition(@rect_source,
            @rect_dest);
          if hr <> S_OK then
            OnErrorS('Can''t zoom');
        end
        else
        begin
          rect_source.Left := 0;
          rect_source.Top := 0;
          rect_source.Right := VideoInWidth;
          rect_source.Bottom := VideoInHeight;

          pVMRWindowlessControl.SetVideoPosition(@rect_source, @rect_dest);
        end;

        // rect_source.Left := 0;
        // rect_source.Top := 0;
        // rect_source.Right := VideoInWidth;
        // rect_source.Bottom := VideoInHeight;

        pVMRWindowlessControl.SetVideoPosition(@rect_source, @rect_dest);
      end
      else
      begin
        pVMRWindowlessControl.SetVideoPosition(nil, @rect_dest);
      end;

      if not RendererDeinterlaceUseDefault then
      begin
        // hr := pVideoRenderer.QueryInterface(IID_IVMRDeinterlaceControl9, pVMRDeinterlaceControl9);
        // if hr <> S_OK then
        // Exit;

        if pVMRDeinterlaceControl9 = nil then
          Exit;

        vd.dwSize := SizeOf(VMR9VideoDesc);
        vd.dwSampleWidth := VideoInWidth;
        vd.dwSampleHeight := VideoInHeight;
        vd.SampleFormat := VMR9_SampleFieldInterleavedEvenFirst;
        vd.dwFourCC := MAKEFOURCC('Y', 'U', 'Y', '2');
        vd.InputSampleFreq.dwNumerator := 25000;
        vd.InputSampleFreq.dwDenominator := 1000;
        vd.OutputFrameFreq.dwNumerator := 50000;
        vd.OutputFrameFreq.dwDenominator := 1000;

        num := 0;
        hr := pVMRDeinterlaceControl9.GetNumberOfDeinterlaceModes(vd, num, nil);

        if (hr = S_OK) and (num > 0) then
          pVMRDeinterlaceControl9.GetNumberOfDeinterlaceModes(vd, num, @modes);

        caps.dwSize := SizeOf(VMR9DeinterlaceCaps);

        if hr = S_OK then
        begin
          for i := 0 to num - 1 do
          begin
            pVMRDeinterlaceControl9.GetDeinterlaceModeCaps(modes[i], @vd, caps);

            S := VMR9DeinterlaceModeToStr(caps.DeinterlaceTechnology);

            if S = trim(RendererDeinterlaceMode) then
            begin
              hr := pVMRDeinterlaceControl9.SetDeinterlaceMode($FFFFFFFF,
                modes[i]);

              if hr <> S_OK then
                OnErrorS('(Warning) Unable to set deinterlace mode.');

              break;
            end;
          end;
        end;

        // pVMRDeinterlaceControl9 := nil;
      end;

      hdc1 := BeginPaint(ScreenHandle, ps);
      pVMRWindowlessControl.RepaintVideo(ScreenHandle, hdc1);
      EndPaint(ScreenHandle, ps);
    end;
  except
    ;
  end;
end;

procedure TDSScreen.Update_VR(filterGraph: IFilterGraph2);
var
  dc: HDC;
  pBasicVideo: IBasicVideo;
  VideoScreenWidth, VideoScreenHeight, NewImageWidth, NewImageHeight,
    NewImageLeft, NewImageTop: Integer;
  lMode: LongBool;
  src_rect: TRect;
  owner: OAHWND;
begin
  VideoScreenWidth := 0;
  VideoScreenHeight := 0;

  try
    if ((pVideoWindow <> nil) and (filterGraph <> nil)) then
    begin
      if (VideoInWidth = 0) or (VideoInHeight = 0) then
      begin
        try
          if (pVideoRenderer.QueryInterface(IID_IBasicVideo, pBasicVideo) = S_OK)
          then
          begin
            pBasicVideo.get_VideoWidth(VideoInWidth);
            pBasicVideo.get_VideoHeight(VideoInHeight);
          end;
        finally
          pBasicVideo := nil;
        end;
      end;

      pVideoWindow.put_BorderColor(FBorderColor);

      // zoom
      if (Screen_Zoom_Ratio >= 0) and (VideoInWidth > 0) and (VideoInHeight > 0)
      then
      begin
        pBasicVideo := nil;
        try
          if (pVideoRenderer.QueryInterface(IID_IBasicVideo, pBasicVideo) = S_OK)
          then
          begin
            pBasicVideo.SetDefaultSourcePosition;

            src_rect := GetZoomPos(VideoInWidth, VideoInHeight,
              Screen_Zoom_ShiftX, Screen_Zoom_ShiftY, Screen_Zoom_Ratio);
            pBasicVideo.put_SourceWidth(src_rect.Right - src_rect.Left);
            pBasicVideo.put_SourceHeight(src_rect.Bottom - src_rect.Top);
            pBasicVideo.put_SourceLeft(src_rect.Left);
            pBasicVideo.put_SourceTop(src_rect.Top);
          end;
        finally
          pBasicVideo := nil;
        end;
      end;

      if (FullScreen) then
      begin
        if pVideoWindow.get_Owner(owner) <> 0 then
        begin
          pVideoWindow.put_Owner(0);
          pVideoWindow.put_MessageDrain(ScreenHandle);
          pVideoWindow.put_WindowStyle((WS_CHILD xor WS_CLIPSIBLINGS) and
            not(WS_BORDER xor WS_CAPTION xor WS_THICKFRAME xor WS_TABSTOP));

          dc := CreateDC('DISPLAY', nil, nil, nil);
          VideoScreenWidth := GetDeviceCaps(dc, HORZRES);
          VideoScreenHeight := GetDeviceCaps(dc, VERTRES);
          DeleteDC(dc);
        end;
      end
      else
      begin
        owner := 0;

        pVideoWindow.get_Owner(owner);

        if owner <> ScreenHandle then
        begin
          pVideoWindow.put_MessageDrain(ScreenHandle);
          pVideoWindow.put_Owner(ScreenHandle);

          pVideoWindow.put_WindowStyle(WS_CHILD xor WS_CLIPSIBLINGS);

          pVideoWindow.put_Visible(true);

          VideoScreenWidth := width;
          VideoScreenHeight := height;
        end;
      end;

      if ((filterGraph.QueryInterface(IID_IBasicVideo, pBasicVideo) = S_OK))
      then
      begin
        // pBasicVideo.get_VideoWidth(ClipWidth);
        // pBasicVideo.get_VideoHeight(ClipHeight);

        if ((VideoScreenWidth > 0) and (VideoScreenHeight > 0) and
          (VideoInWidth > 0) and (VideoInHeight > 0)) then
        begin
          if ((VideoInWidth / VideoInHeight) >
            (VideoScreenWidth / VideoScreenHeight)) then
          begin
            NewImageWidth := VideoScreenWidth;
            NewImageHeight :=
              round(VideoInHeight / VideoInWidth * VideoScreenWidth);
          end
          else
          begin
            NewImageWidth := round(VideoInWidth / VideoInHeight *
              VideoScreenHeight);
            NewImageHeight := VideoScreenHeight;
          end;

          NewImageLeft := round((VideoScreenWidth / 2) - (NewImageWidth / 2));
          NewImageTop := round((VideoScreenHeight / 2) - (NewImageHeight / 2));
        end
        else
        begin
          NewImageLeft := 0;
          NewImageTop := 0;
          NewImageWidth := VideoScreenWidth;
          NewImageHeight := VideoScreenHeight;
        end;

        if (FullScreen) then
        begin
          pVideoWindow.SetWindowPosition(0, 0, VideoScreenWidth,
            VideoScreenHeight);

          if Stretch then
            pBasicVideo.SetDestinationPosition(0, 0, VideoScreenWidth,
              VideoScreenHeight)
          else
            pBasicVideo.SetDestinationPosition(NewImageLeft, NewImageTop,
              NewImageWidth, NewImageHeight);

          pVideoWindow.HideCursor(true);

          lMode := true;
          pVideoWindow.put_FullScreenMode(lMode);

          pVideoWindow.SetWindowForeground(0);
        end
        else
        begin
          if Stretch then
            pVideoWindow.SetWindowPosition(0, 0, width, height)
          else
            pVideoWindow.SetWindowPosition(NewImageLeft, NewImageTop,
              NewImageWidth, NewImageHeight);

          pBasicVideo.SetDefaultDestinationPosition();

          pVideoWindow.HideCursor(false);

          pVideoWindow.SetWindowForeground(-1);

          lMode := false;
          pVideoWindow.put_FullScreenMode(lMode);
        end;

        pBasicVideo := nil;
      end;
    end;
  except
    ;
  end;
end;

const
  FIL_AVIDecompressor = 'AVI Decompressor';
  FIL_PSIParser = 'PSI Parser';
  FIL_PSI = 'PSI';
  Deint_BOBLineReplicate = 'BOB Line Replicate, ';
  Deint_BOBVerticalStretch = 'BOB Vertical Stretch, ';
  Deint_MedianFiltering = 'Median Filtering, ';
  Deint_EdgeFiltering = 'Edge Filtering, ';
  Deint_FieldAdaptive = 'Field Adaptive, ';
  Deint_PixelAdaptive = 'Pixel Adaptive, ';
  Deint_MotionVectorSteered = 'Motion Vector Steered, ';
  Deint_UnknownProprietary = 'Unknown/Proprietary';
  SFilterData = 'FilterData';
  FCC_BITFIELDS = 'BITFIELDS';
  FCC_RLE4 = 'RLE4';
  FCC_RLE8 = 'RLE8';
  FCC_RGB = 'RGB';
  PP_DevicePath = 'DevicePath';
  PP_FccHandler = 'FccHandler';
  PP_Description = 'Description';
  PP_CLSID = 'CLSID';
  PP_FriendlyName = 'FriendlyName';
  SUnknown = 'Unknown';
  ER_ENOTDETERMINED = 'E_NOTDETERMINED';
  ER_VFWEPINALREADYBLOCKED = 'VFW_E_PIN_ALREADY_BLOCKED';
  ER_ENOTIMELINE = 'E_NO_TIMELINE';
  ER_ERENDERENGINEISBROKEN = 'E_RENDER_ENGINE_IS_BROKEN';
  ER_VFWEDDRAWCAPSNOTSUITABLE = 'VFW_E_DDRAW_CAPS_NOT_SUITABLE';
  ER_EMUSTINITRENDERER = 'E_MUST_INIT_RENDERER';
  ER_VFWEVMRNOTINMIXERMODE = 'VFW_E_VMR_NOT_IN_MIXER_MODE';
  ER_SWARNOUTPUTRESET = 'S_WARN_OUTPUTRESET';
  ER_ENOTINTREE = 'E_NOTINTREE';
  ER_VFWEVMRNODEINTERLACEHW = 'VFW_E_VMR_NO_DEINTERLACE_HW';
  ER_VFWEDVDVMR9INCOMPATIBLEDEC = 'VFW_E_DVD_VMR9_INCOMPATIBLEDEC';
  ER_VFWEBADKEY = 'VFW_E_BAD_KEY';
  ER_VFWEVMRNOPROCAMPHW = 'VFW_E_VMR_NO_PROCAMP_HW';
  ER_VFWEVMRNOAPSUPPLIED = 'VFW_E_VMR_NO_AP_SUPPLIED';
  ER_VFWECERTIFICATIONFAILURE = 'VFW_E_CERTIFICATION_FAILURE';
  ER_VFWEPINALREADYBLOCKEDONTHISTHREAD =
    'VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD';
  ER_VFWEDVDNOGOUPPGC = 'VFW_E_DVD_NO_GOUP_PGC';
  ER_VFWEDVDNOTINKARAOKEMODE = 'VFW_E_DVD_NOT_IN_KARAOKE_MODE';
  ER_VFWEDVDSTREAMDISABLED = 'VFW_E_DVD_STREAM_DISABLED';
  ER_VFWEDVDINVALIDDISC = 'VFW_E_DVD_INVALID_DISC';
  ER_VFWEDVDNORESUMEINFORMATION = 'VFW_E_DVD_NO_RESUME_INFORMATION';
  ER_VFWEDVDTITLEUNKNOWN = 'VFW_E_DVD_TITLE_UNKNOWN';
  ER_VFWEFRAMESTEPUNSUPPORTED = 'VFW_E_FRAME_STEP_UNSUPPORTED';
  ER_VFWEDVDLOWPARENTALLEVEL = 'VFW_E_DVD_LOW_PARENTAL_LEVEL';
  ER_VFWEDVDDECNOTENOUGH = 'VFW_E_DVD_DECNOTENOUGH';
  ER_VFWEDVDINCOMPATIBLEREGION = 'VFW_E_DVD_INCOMPATIBLE_REGION';
  ER_VFWEDVDNOATTRIBUTES = 'VFW_E_DVD_NO_ATTRIBUTES';
  ER_VFWEDVDSTATECORRUPT = 'VFW_E_DVD_STATE_CORRUPT';
  ER_VFWEDVDSTATEWRONGDISC = 'VFW_E_DVD_STATE_WRONG_DISC';
  ER_VFWEDVDWRONGSPEED = 'VFW_E_DVD_WRONG_SPEED';
  ER_VFWEDVDCMDCANCELLED = 'VFW_E_DVD_CMD_CANCELLED';
  ER_VFWECOPYPROTFAILED = 'VFW_E_COPYPROT_FAILED';
  ER_VFWEDVDSTATEWRONGVERSION = 'VFW_E_DVD_STATE_WRONG_VERSION';
  ER_VFWEDVDMENUDOESNOTEXIST = 'VFW_E_DVD_MENU_DOES_NOT_EXIST';
  ER_VFWETIMEEXPIRED = 'VFW_E_TIME_EXPIRED';
  ER_VFWEDDRAWVERSIONNOTSUITABLE = 'VFW_E_DDRAW_VERSION_NOT_SUITABLE';
  ER_VFWENOCAPTUREHARDWARE = 'VFW_E_NO_CAPTURE_HARDWARE';
  ER_VFWEDVDINVALIDDOMAIN = 'VFW_E_DVD_INVALIDDOMAIN';
  ER_VFWEDVDGRAPHNOTREADY = 'VFW_E_DVD_GRAPHNOTREADY';
  ER_VFWEDVDRENDERFAIL = 'VFW_E_DVD_RENDERFAIL';
  ER_VFWEDVDNOBUTTON = 'VFW_E_DVD_NO_BUTTON';
  ER_VFWEDVDOPERATIONINHIBITED = 'VFW_E_DVD_OPERATION_INHIBITED';
  ER_VFWENOVPHARDWARE = 'VFW_E_NO_VP_HARDWARE';
  ER_VFWEALREADYCANCELLED = 'VFW_E_ALREADY_CANCELLED';
  ER_VFWEREADONLY = 'VFW_E_READ_ONLY';
  ER_VFWEOUTOFVIDEOMEMORY = 'VFW_E_OUT_OF_VIDEO_MEMORY';
  ER_VFWEVPNEGOTIATIONFAILED = 'VFW_E_VP_NEGOTIATION_FAILED';
  ER_VFWEUNSUPPORTEDSTREAM = 'VFW_E_UNSUPPORTED_STREAM';
  ER_VFWEBADVIDEOCD = 'VFW_E_BAD_VIDEOCD';
  ER_VFWSNOSTOPTIME = 'VFW_S_NO_STOP_TIME';
  ER_VFWENOTRANSPORT = 'VFW_E_NO_TRANSPORT';
  ER_VFWEBUFFERUNDERFLOW = 'VFW_E_BUFFER_UNDERFLOW';
  ER_VFWENOTINGRAPH = 'VFW_E_NOT_IN_GRAPH';
  ER_VFWENOTIMEFORMAT = 'VFW_E_NO_TIME_FORMAT';
  ER_VFWEINVALIDCLSID = 'VFW_E_INVALID_CLSID';
  ER_VFWEPROCESSORNOTSUITABLE = 'VFW_E_PROCESSOR_NOT_SUITABLE';
  ER_VFWEUNSUPPORTEDVIDEO = 'VFW_E_UNSUPPORTED_VIDEO';
  ER_VFWEMPEGNOTCONSTRAINED = 'VFW_E_MPEG_NOT_CONSTRAINED';
  ER_VFWEUNSUPPORTEDAUDIO = 'VFW_E_UNSUPPORTED_AUDIO';
  ER_VFWESAMPLETIMENOTSET = 'VFW_E_SAMPLE_TIME_NOT_SET';
  ER_VFWENOTIMEFORMATSET = 'VFW_E_NO_TIME_FORMAT_SET';
  ER_VFWENOAUDIOHARDWARE = 'VFW_E_NO_AUDIO_HARDWARE';
  ER_VFWERPZA = 'VFW_E_RPZA';
  ER_VFWEMONOAUDIOHW = 'VFW_E_MONO_AUDIO_HW';
  ER_VFWEMEDIATIMENOTSET = 'VFW_E_MEDIA_TIME_NOT_SET';
  ER_VFWEINVALIDMEDIATYPE = 'VFW_E_INVALID_MEDIA_TYPE';
  ER_VFWEUNKNOWNFILETYPE = 'VFW_E_UNKNOWN_FILE_TYPE';
  ER_VFWEFILETOOSHORT = 'VFW_E_FILE_TOO_SHORT';
  ER_VFWEINVALIDFILEVERSION = 'VFW_E_INVALID_FILE_VERSION';
  ER_VFWECANNOTLOADSOURCEFILTER = 'VFW_E_CANNOT_LOAD_SOURCE_FILTER';
  ER_VFWENOADVISESET = 'VFW_E_NO_ADVISE_SET';
  ER_VFWECIRCULARGRAPH = 'VFW_E_CIRCULAR_GRAPH';
  ER_VFWEADVISEALREADYSET = 'VFW_E_ADVISE_ALREADY_SET';
  ER_VFWENOFULLSCREEN = 'VFW_E_NO_FULLSCREEN';
  ER_VFWENOMODEXAVAILABLE = 'VFW_E_NO_MODEX_AVAILABLE';
  ER_VFWECORRUPTGRAPHFILE = 'VFW_E_CORRUPT_GRAPH_FILE';
  ER_VFWETIMEALREADYPASSED = 'VFW_E_TIME_ALREADY_PASSED';
  ER_VFWESAMPLEREJECTEDEOS = 'VFW_E_SAMPLE_REJECTED_EOS';
  ER_VFWEINVALIDFILEFORMAT = 'VFW_E_INVALID_FILE_FORMAT';
  ER_VFWENOTALLOWEDTOSAVE = 'VFW_E_NOT_ALLOWED_TO_SAVE';
  ER_VFWEENUMOUTOFRANGE = 'VFW_E_ENUM_OUT_OF_RANGE';
  ER_VFWETIMEOUT = 'VFW_E_TIMEOUT';
  ER_VFWEINVALIDRECT = 'VFW_E_INVALID_RECT';
  ER_VFWEDUPLICATENAME = 'VFW_E_DUPLICATE_NAME';
  ER_VFWESAMPLEREJECTED = 'VFW_E_SAMPLE_REJECTED';
  ER_VFWEWRONGSTATE = 'VFW_E_WRONG_STATE';
  ER_VFWETYPENOTACCEPTED = 'VFW_E_TYPE_NOT_ACCEPTED';
  ER_VFWESTARTTIMEAFTEREND = 'VFW_E_START_TIME_AFTER_END';
  ER_VFWENOTRUNNING = 'VFW_E_NOT_RUNNING';
  ER_VFWENOTPAUSED = 'VFW_E_NOT_PAUSED';
  ER_VFWENOTSTOPPED = 'VFW_E_NOT_STOPPED';
  ER_VFWESTATECHANGED = 'VFW_E_STATE_CHANGED';
  ER_VFWENODISPLAYPALETTE = 'VFW_E_NO_DISPLAY_PALETTE';
  ER_VFWETOOMANYCOLORS = 'VFW_E_TOO_MANY_COLORS';
  ER_VFWENOCOLORKEYFOUND = 'VFW_E_NO_COLOR_KEY_FOUND';
  ER_VFWENOPALETTEAVAILABLE = 'VFW_E_NO_PALETTE_AVAILABLE';
  ER_VFWECOLORKEYSET = 'VFW_E_COLOR_KEY_SET';
  ER_VFWEPALETTESET = 'VFW_E_PALETTE_SET';
  ER_VFWENOTSAMPLECONNECTION = 'VFW_E_NOT_SAMPLE_CONNECTION';
  ER_VFWENOTOVERLAYCONNECTION = 'VFW_E_NOT_OVERLAY_CONNECTION';
  ER_VFWENOCOLORKEYSET = 'VFW_E_NO_COLOR_KEY_SET';
  ER_VFWECHANGINGFORMAT = 'VFW_E_CHANGING_FORMAT';
  ER_VFWECANNOTRENDER = 'VFW_E_CANNOT_RENDER';
  ER_VFWENOTFOUND = 'VFW_E_NOT_FOUND';
  ER_VFWECANNOTCONNECT = 'VFW_E_CANNOT_CONNECT';
  ER_VFWENOINTERFACE = 'VFW_E_NO_INTERFACE';
  ER_VFWENOSINK = 'VFW_E_NO_SINK';
  ER_VFWENOCLOCK = 'VFW_E_NO_CLOCK';
  ER_VFWESIZENOTSET = 'VFW_E_SIZENOTSET';
  ER_VFWEALREADYCOMMITTED = 'VFW_E_ALREADY_COMMITTED';
  ER_VFWEBADALIGN = 'VFW_E_BADALIGN';
  ER_VFWEBUFFEROVERFLOW = 'VFW_E_BUFFER_OVERFLOW';
  ER_VFWERUNTIMEERROR = 'VFW_E_RUNTIME_ERROR';
  ER_VFWEBUFFERSOUTSTANDING = 'VFW_E_BUFFERS_OUTSTANDING';
  ER_VFWEBUFFERNOTSET = 'VFW_E_BUFFER_NOTSET';
  ER_VFWENOTCOMMITTED = 'VFW_E_NOT_COMMITTED';
  ER_VFWENOALLOCATOR = 'VFW_E_NO_ALLOCATOR';
  ER_VFWENOTCONNECTED = 'VFW_E_NOT_CONNECTED';
  ER_VFWEINVALIDDIRECTION = 'VFW_E_INVALID_DIRECTION';
  ER_SVFWENOACCEPTABLETYPES = 'VFW_E_NO_ACCEPTABLE_TYPES';
  ER_VFWENOTYPES = 'VFW_E_NO_TYPES';
  ER_VFWEFILTERACTIVE = 'VFW_E_FILTER_ACTIVE';
  ER_VFWEALREADYCONNECTED = 'VFW_E_ALREADY_CONNECTED';
  ER_VFWEENUMOUTOFSYNC = 'VFW_E_ENUM_OUT_OF_SYNC';
  ER_VFWENEEDOWNER = 'VFW_E_NEED_OWNER';
  ER_VFWEINVALIDSUBTYPE = 'VFW_E_INVALIDSUBTYPE';
  ER_VFWSDVDNOTACCURATE = 'VFW_S_DVD_NOT_ACCURATE';
  ER_VFWSDVDNONONESEQUENTIAL = 'VFW_S_DVD_NON_ONE_SEQUENTIAL';
  ER_VFWSNOPREVIEWPIN = 'VFW_S_NOPREVIEWPIN';
  ER_VFWSCANTCUE = 'VFW_S_CANT_CUE';
  ER_VFWSSTREAMOFF = 'VFW_S_STREAM_OFF';
  ER_VFWSRESERVED = 'VFW_S_RESERVED';
  ER_VFWSESTIMATED = 'VFW_S_ESTIMATED';
  ER_VFWSDVDCHANNELCONTENTSNOTAVAILABLE =
    'VFW_S_DVD_CHANNEL_CONTENTS_NOT_AVAILABLE';
  ER_VFWSRPZA = 'VFW_S_RPZA';
  ER_VFWSAUDIONOTRENDERED = 'VFW_S_AUDIO_NOT_RENDERED';
  ER_VFWSVIDEONOTRENDERED = 'VFW_S_VIDEO_NOT_RENDERED';
  ER_VFWSMEDIATYPEIGNORED = 'VFW_S_MEDIA_TYPE_IGNORED';
  ER_VFWSRESOURCENOTNEEDED = 'VFW_S_RESOURCE_NOT_NEEDED';
  ER_VFWSCONNECTIONSDEFERRED = 'VFW_S_CONNECTIONS_DEFERRED';
  ER_VFWSSOMEDATAIGNORED = 'VFW_S_SOME_DATA_IGNORED';
  ER_VFWSPARTIALRENDER = 'VFW_S_PARTIAL_RENDER';
  ER_VFWSSTATEINTERMEDIATE = 'VFW_S_STATE_INTERMEDIATE';
  ER_VFWSDUPLICATENAME = 'VFW_S_DUPLICATE_NAME';
  ER_VFWSNOMOREITEMS = 'VFW_S_NO_MORE_ITEMS';
  ER_SFALSE = 'S_FALSE';
  ER_SOK = 'S_OK';
  MT_UnKnown = 'UnKnown ';
  MT_Video = 'Video';
  MT_URLSTREAM = 'URL_STREAM';
  MT_Timecode = 'Timecode';
  MT_Text = 'Text';
  MT_Stream = 'Stream';
  MT_ScriptCommand = 'ScriptCommand';
  MT_MPEG2PES = 'MPEG2_PES';
  MT_Midi = 'Midi';
  MT_LMRT = 'LMRT';
  MT_Interleaved = 'Interleaved';
  MT_AUXLine21Data = 'AUXLine21Data';
  MT_File = 'File';
  MT_Audio = 'Audio';
  MT_Analogvideo = 'Analogvideo';
  MT_AnalogAudio = 'AnalogAudio';
  AUD_Unknown = 'Unknown';
  AUD_NORRIS = 'NORRIS';
  AUD_LHCODEC = 'LH_CODEC';
  AUD_OLIOPR = 'OLIOPR';
  AUD_OLISBC = 'OLISBC';
  AUD_OLICELP = 'OLICELP';
  AUD_OLIADPCM = 'OLIADPCM';
  AUD_BTVDIGITAL = 'BTV_DIGITAL';
  AUD_OLIGSM = 'OLIGSM';
  AUD_FMTOWNSSND = 'FM_TOWNS_SND';
  AUD_QUARTERDECK = 'QUARTERDECK';
  AUD_CREATIVEFASTSPEECH10 = 'CREATIVE_FASTSPEECH10';
  AUD_CREATIVEFASTSPEECH8 = 'CREATIVE_FASTSPEECH8';
  AUD_CREATIVEADPCM = 'CREATIVE_ADPCM';
  AUD_RHETOREXADPCM = 'RHETOREX_ADPCM';
  AUD_SOFTSOUND = 'SOFTSOUND';
  AUD_VOXWARE = 'VOXWARE';
  AUD_DSATDISPLAY = 'DSAT_DISPLAY';
  AUD_DSAT = 'DSAT';
  AUD_G722ADPCM = 'G722_ADPCM';
  AUD_G726ADPCM = 'G726_ADPCM';
  AUD_CANOPUSATRAC = 'CANOPUS_ATRAC';
  AUD_ESPCM = 'ESPCM';
  AUD_CIRRUS = 'CIRRUS';
  AUD_MPEGLAYER3 = 'MPEGLAYER3';
  AUD_MPEG = 'MPEG';
  AUD_G728CELP = 'G728_CELP';
  AUD_G721ADPCM = 'G721_ADPCM';
  AUD_XEBEC = 'XEBEC';
  AUD_ROCKWELLDIGITALK = 'ROCKWELL_DIGITALK';
  AUD_ROCKWELLADPCM = 'ROCKWELL_ADPCM';
  AUD_ECHOSC3 = 'ECHOSC3';
  AUD_CSIMAADPCM = 'CS_IMAADPCM';
  AUD_NMSVBXADPCM = 'NMS_VBXADPCM';
  AUD_CONTROLRESCR10 = 'CONTROL_RES_CR10';
  AUD_DIGIADPCM = 'DIGIADPCM';
  AUD_DIGIREAL = 'DIGIREAL';
  AUD_CONTROLRESVQLPC = 'CONTROL_RES_VQLPC';
  AUD_ANTEXADPCME = 'ANTEX_ADPCME';
  AUD_MSNAUDIO = 'MSNAUDIO';
  AUD_GSM610 = 'GSM610';
  AUD_DOLBYAC2 = 'DOLBY_AC2';
  AUD_AUDIOFILEAF10 = 'AUDIOFILE_AF10';
  AUD_APTX = 'APTX';
  AUD_AUDIOFILEAF36 = 'AUDIOFILE_AF36';
  AUD_ECHOSC1 = 'ECHOSC1';
  AUD_DSPGROUPTRUESPEECH = 'DSPGROUP_TRUESPEECH';
  AUD_SONARC = 'SONARC';
  AUD_YAMAHAADPCM = 'YAMAHA_ADPCM';
  AUD_MEDIAVISIONADPCM = 'MEDIAVISION_ADPCM';
  AUD_DIALOGICOKIADPCM = 'DIALOGIC_OKI_ADPCM';
  AUD_DIGIFIX = 'DIGIFIX';
  AUD_DIGISTD = 'DIGISTD';
  AUD_G723ADPCM = 'G723_ADPCM';
  AUD_SIERRAADPCM = 'SIERRA_ADPCM';
  AUD_MEDIASPACEADPCM = 'MEDIASPACE_ADPCM';
  AUD_DVIADPCM = 'DVI_ADPCM';
  AUD_OKIADPCM = 'OKI_ADPCM';
  AUD_MULAW = 'MULAW';
  AUD_ALAW = 'ALAW';
  AUD_IBMCVSD = 'IBM_CVSD';
  AUD_IEEEFLOAT = 'IEEE_FLOAT';
  AUD_ADPCM = 'ADPCM';
  AUD_PCM = 'PCM';
  MST_UnKnown = 'UnKnown ';
  MST_VOXWAREMetaSound = 'VOXWARE_MetaSound';
  MST_DIVX = 'DIVX';
  MST_MSMPEG4 = 'MS-MPEG4';
  MST_PROVIDER = 'PROVIDER';
  MST_DSI = 'DSI';
  MST_PCI = 'PCI';
  MST_SDDS = 'SDDS';
  MST_DVDLPCMAUDIO = 'DVD_LPCM_AUDIO';
  MST_DTS = 'DTS';
  MST_DVDSUBPICTURE = 'DVD_SUBPICTURE';
  MST_DOLBYAC3 = 'DOLBY_AC3';
  MST_MPEG2AUDIO = 'MPEG2_AUDIO';
  MST_MPEG2TRANSPORT = 'MPEG2_TRANSPORT';
  MST_MPEG2PROGRAM = 'MPEG2_PROGRAM';
  MST_MPEG2VIDEO = 'MPEG2_VIDEO';
  MST_AnalogVideoSECAML = 'AnalogVideo_SECAM_L';
  MST_AnalogVideoSECAMK1 = 'AnalogVideo_SECAM_K1';
  MST_AnalogVideoSECAMK = 'AnalogVideo_SECAM_K';
  MST_AnalogVideoSECAMH = 'AnalogVideo_SECAM_H';
  MST_AnalogVideoSECAMG = 'AnalogVideo_SECAM_G';
  MST_AnalogVideoSECAMD = 'AnalogVideo_SECAM_D';
  MST_AnalogVideoSECAMB = 'AnalogVideo_SECAM_B';
  MST_AnalogVideoPALNCOMBO = 'AnalogVideo_PAL_N_COMBO';
  MST_AnalogVideoPALN = 'AnalogVideo_PAL_N';
  MST_AnalogVideoPALM = 'AnalogVideo_PAL_M';
  MST_AnalogVideoPALI = 'AnalogVideo_PAL_I';
  MST_AnalogVideoPALH = 'AnalogVideo_PAL_H';
  MST_AnalogVideoPALG = 'AnalogVideo_PAL_G';
  MST_AnalogVideoPALD = 'AnalogVideo_PAL_D';
  MST_AnalogVideoPALB = 'AnalogVideo_PAL_B';
  MST_AnalogVideoNTSCM = 'AnalogVideo_NTSC_M';
  MST_VPVBI = 'VPVBI';
  MST_VPVideo = 'VPVideo';
  MST_DssAudio = 'DssAudio';
  MST_DssVideo = 'DssVideo';
  MST_RAWSPORT = 'RAW_SPORT';
  MST_SPDIFTAG241h = 'SPDIF_TAG_241h';
  MST_DOLBYAC3SPDIF = 'DOLBY_AC3_SPDIF';
  MST_IEEEFLOAT = 'IEEE_FLOAT';
  MST_DRMAudio = 'DRM_Audio';
  MST_Line21VBIRawData = 'Line21_VBIRawData';
  MST_Line21GOPPacket = 'Line21_GOPPacket';
  MST_Line21BytePair = 'Line21_BytePair';
  MST_Dvsl = 'dvsl';
  MST_Dvhd = 'dvhd';
  MST_Dvsd = 'dvsd_';
  MST_AIFF = 'AIFF';
  MST_AU = 'AU';
  MST_PCMAudioObsolete = 'PCMAudio_Obsolete';
  MST_WAVE = 'WAVE';
  MST_QTJpeg = 'QTJpeg';
  MST_PCM = 'PCM';
  MST_QTRpza = 'QTRpza';
  MST_QTRle = 'QTRle';
  MST_Asf = 'Asf';
  MST_QTMovie = 'QTMovie';
  MST_QTSmc = 'QTSmc';
  MST_Avi = 'Avi';
  MST_MPEG1Audio = 'MPEG1Audio';
  MST_MPEG1Video = 'MPEG1Video';
  MST_MPEG1VideoCD = 'MPEG1VideoCD';
  MST_MPEG1System = 'MPEG1System';
  MST_MPEG1AudioPayload = 'MPEG1AudioPayload';
  MST_MPEG1Payload = 'MPEG1Payload';
  MST_MPEG1Packet = 'MPEG1Packet';
  MST_Overlay = 'Overlay';
  MST_ARGB32 = 'ARGB32';
  MST_RGB32 = 'RGB32';
  MST_RGB24 = 'RGB24';
  MST_RGB555 = 'RGB555';
  MST_RGB565 = 'RGB565';
  MST_RGB8 = 'RGB8';
  MST_RGB4 = 'RGB4';
  MST_RGB1 = 'RGB1';
  MST_MDVF = 'MDVF';
  MST_IJPG = 'IJPG';
  MST_DVCS = 'DVCS';
  MST_Plum = 'Plum';
  MST_CFCC = 'CFCC';
  MST_TVMJ = 'TVMJ';
  MST_WAKE = 'WAKE';
  MST_MJPG = 'MJPG';
  MST_CLJR = 'CLJR';
  MST_Y211 = 'Y211';
  MST_CPLA = 'CPLA';
  MST_IF09 = 'IF09';
  MST_YV12 = 'YV12';
  MST_UYVY = 'UYVY';
  MST_YVYU = 'YVYU';
  MST_YUY2 = 'YUY2';
  MST_Y41P = 'Y41P';
  MST_Y411 = 'Y411';
  MST_YVU9 = 'YVU9';
  MST_IYUV = 'IYUV';
  MST_YUYV = 'YUYV';
  MST_CLPL = 'CLPL';
  wszStreamName: WideString = 'ActiveMovieGraph';

function GetEnvironmentVariableW(const name: WideString): WideString;
var
  Len: Integer;
  w: WideString;
begin
  result := '';
  SetLength(w, 1);
  Len := windows.GetEnvironmentVariableW(PWideChar(Name), PWideChar(w), 1);
  if Len > 0 then
  begin
    SetLength(result, Len - 1);
    windows.GetEnvironmentVariableW(PWideChar(Name), PWideChar(result), Len);
  end;
end;

constructor TVFGraphCallbacks.Create;
begin
  inherited Create(nil);

  FBlackList := TStringList.Create;
  FBlackListByCLSID := TStringList.Create;
end;

destructor TVFGraphCallbacks.Destroy;
begin
  FBlackList.Free;
  FBlackListByCLSID.Free;

  inherited;
end;

procedure TVFGraphCallbacks.AssignToGraph(pGB: IGraphBuilder);
var
  Obj: IObjectWithSite;
const
  IID_IObjectWithSite: TGUID = '{FC4801A3-2BA9-11CF-A229-00AA003D7352}';
begin
  try
    if Succeeded(pGB.QueryInterface(IID_IObjectWithSite, Obj)) then
    begin
      Obj.SetSite(Self);
      Obj := nil;
    end;
  except
    ;
  end;
end;

function TVFGraphCallbacks.CreatedFilter(pFil: IBaseFilter): HRESULT; stdcall;
begin
  result := NOERROR;
end;

function TVFGraphCallbacks.SelectedFilter(pMon: IMoniker): HRESULT; stdcall;
var
  PropBag: IPropertyBag;
  pFilter: IBaseFilter;
  name: OleVariant;
  CLSID: TGUID;
  clsid_s: WideString;
begin
  result := E_FAIL;

  try
    if pMon = nil then
      Exit;

    pMon.BindToStorage(nil, nil, IID_IPropertyBag, PropBag);
    PropBag.Read(PP_FriendlyName, Name, nil);
    PropBag := nil;

    if FBlackList.IndexOf(Name) <> -1 then
      result := E_FAIL
    else
      result := NOERROR;

    if result = E_FAIL then
      Exit;

    if FBlackListByCLSID.Count > 0 then
    begin
      pMon.BindToObject(nil, nil, IID_IBaseFilter, pFilter);

      if not Assigned(pFilter) then
        Exit;

      pFilter.GetClassID(CLSID);
      clsid_s := GUIDToString(CLSID);

      if FBlackListByCLSID.IndexOf(clsid_s) <> -1 then
        result := E_FAIL
      else
        result := NOERROR;

      if Assigned(pFilter) then
        pFilter := nil;
    end;
  except
    result := NOERROR;
  end;
end;

function LoadWMProfileFromFile(FilePath: WideString; ms: TMemoryStream)
  : Boolean;
var
  fs: TFileStream;
begin
  result := false;

  try
    fs := nil;
    try
      if not Assigned(ms) then
        Exit;
      fs := TFileStream.Create(FilePath, fmOpenRead);
      ms.LoadFromStream(fs);
      result := true;
    finally
      if Assigned(fs) then
      begin
        fs.Free;
      end;
    end;
  except
    ;
    // if DebugMode then
    // ShowMessage('Can''t load WMV profile from file!');
  end;
end;

function AddGraphToRot(Graph: IFilterGraph; out ID: Integer): HRESULT;
var
  Moniker: IMoniker;
  ROT: IRunningObjectTable;
  wsz: WideString;
begin
  result := GetRunningObjectTable(0, ROT);
  if (result <> S_OK) then
    Exit;
  wsz := format('FilterGraph %p pid %x',
    [pointer(Graph), GetCurrentProcessId()]);
  result := CreateItemMoniker('!', PWideChar(wsz), Moniker);
  if (result <> S_OK) then
    Exit;
  result := ROT.Register(0, Graph, Moniker, ID);
  Moniker := nil;
end;

function RemoveGraphFromRot(ID: Integer): HRESULT;
var
  ROT: IRunningObjectTable;
begin
  result := GetRunningObjectTable(0, ROT);
  if (result <> S_OK) then
    Exit;
  result := ROT.Revoke(ID);
  ROT := nil;
end;

procedure SaveGraphAsText(pGraph: IGraphBuilder; FileName: WideString);
var
  fl: TVFFilterList;
  pl: TVFPinList;
  emt: TVFEnumMediaType;
  pFilter: IBaseFilter;
  pPin: IPin;
  i, j, k: Integer;
  lst: TStringList;
  pi: PIN_INFO;
  fi: FILTER_INFO;
begin
  lst := TStringList.Create;
  fl := nil;

  try
    fl := TVFFilterList.Create(pGraph);

    lst.Add('Filters count: ' + IntToStr(fl.Count));
    lst.Add('');

    for i := 0 to fl.Count - 1 do
    begin
      pFilter := fl.Items[i];
      lst.Add('Filter name: ' + fl.FilterInfo[i].achName);

      pl := TVFPinList.Create(pFilter);

      for j := 0 to pl.Count - 1 do
      begin
        if pl.PinInfo[j].dir = PINDIR_INPUT then
          lst.Add('  Pin ' + IntToStr(j) + ' - Input - ' + pl.PinInfo
            [j].achName)
        else
          lst.Add('  Pin ' + IntToStr(j) + ' - Output - ' + pl.PinInfo
            [j].achName);

        pl.Items[j].ConnectedTo(pPin);

        if pPin <> nil then
          try
            pPin.QueryPinInfo(pi);
            pi.pFilter.QueryFilterInfo(fi);

            lst.Add('    Connected to: ' + fi.achName + ', ' + pi.achName);

            pPin := nil;
          except
            pPin := nil;
          end;

        emt := TVFEnumMediaType.Create;
        emt.Assign(pl.Items[j]);

        if emt.Count > 0 then
          lst.Add('    Mediatypes:');

        for k := 0 to emt.Count - 1 do
          lst.Add('      ' + emt.MediaDescription[k]);

        emt.Free;
      end;

      pl.Free;

      lst.Add('');
      lst.Add('');
    end;

    lst.SaveToFile(FileName);
  finally
    pFilter := nil;
    lst.Free;

    if fl <> nil then
    begin
      fl.Free;
    end;
  end;
end;

function SaveGraphFile(pGraph: IGraphBuilder; wszPath: WideString): HRESULT;
var
  Storage: IStorage;
  Stream: IStream;
  Persist: IPersistStream;
begin
  try
    result := StgCreateDocfile(PWideChar(wszPath), STGM_CREATE or
      STGM_TRANSACTED or STGM_READWRITE or STGM_SHARE_EXCLUSIVE, 0, Storage);
    if FAILED(result) then
      Exit;

    result := Storage.CreateStream(PWideChar(wszStreamName),
      STGM_WRITE or STGM_CREATE or STGM_SHARE_EXCLUSIVE, 0, 0, Stream);
    if FAILED(result) then
      Exit;

    pGraph.QueryInterface(IID_IPersistStream, Persist);
    result := Persist.Save(Stream, true);
    Stream := nil;
    Persist := nil;
    if Succeeded(result) then
      result := Storage.Commit(STGC_DEFAULT);
    Storage := nil;
  except
  end;
end;

function LoadGraphFile(pGraph: IGraphBuilder;
  const wszName: WideString): HRESULT;
var
  Storage: IStorage;
  Stream: IStream;
  PersistStream: IPersistStream;
begin
  if (S_OK <> StgIsStorageFile(PWideChar(wszName))) then
  begin
    result := E_FAIL;
    Exit;
  end;

  result := StgOpenStorage(PWideChar(wszName), nil, STGM_TRANSACTED or
    STGM_READ or STGM_SHARE_DENY_WRITE, nil, 0, Storage);

  if FAILED(result) then
    Exit;

  result := pGraph.QueryInterface(IID_IPersistStream, PersistStream);

  if (Succeeded(result)) then
  begin
    result := Storage.OpenStream(PWideChar(wszStreamName), nil,
      STGM_READ or STGM_SHARE_EXCLUSIVE, 0, Stream);
    if Succeeded(result) then
    begin
      result := PersistStream.Load(Stream);
      Stream := nil;
    end;
    PersistStream := nil;
  end;

  Storage := nil;
end;

function MajorTypeToString(guid: TGUID): WideString;
begin
  if IsEqualGUID(guid, MEDIATYPE_AnalogAudio) then
    result := 'AnalogAudio'
  else if IsEqualGUID(guid, MEDIATYPE_AnalogVIDEO) then
    result := 'AnalogVideo'
  else if IsEqualGUID(guid, MEDIATYPE_Audio) then
    result := 'Audio'
  else if IsEqualGUID(guid, MEDIATYPE_AUXLine21Data) then
    result := 'AUXLine21Data'
  else if IsEqualGUID(guid, MEDIATYPE_File) then
    result := 'File'
  else if IsEqualGUID(guid, MEDIATYPE_Interleaved) then
    result := 'Interleaved'
  else if IsEqualGUID(guid, MEDIATYPE_LMRT) then
    result := 'LMRT'
  else if IsEqualGUID(guid, MEDIATYPE_Midi) then
    result := 'Midi'
  else if IsEqualGUID(guid, MEDIATYPE_MPEG2_PES) then
    result := 'MPEG2_PES'
  else if IsEqualGUID(guid, MEDIATYPE_ScriptCommand) then
    result := 'ScriptCommand'
  else if IsEqualGUID(guid, MEDIATYPE_Stream) then
    result := 'Stream'
  else if IsEqualGUID(guid, MEDIATYPE_Text) then
    result := 'Text'
  else if IsEqualGUID(guid, MEDIATYPE_Timecode) then
    result := 'Timecode'
  else if IsEqualGUID(guid, MEDIATYPE_URL_STREAM) then
    result := 'URL_STREAM'
  else if IsEqualGUID(guid, MEDIATYPE_Video) then
    result := 'Video'
  else if IsEqualGUID(guid, MEDIATYPE_VBI) then
    result := 'VBI'
  else if IsEqualGUID(guid, MEDIATYPE_MPEG2_PACK) then
    result := 'MPEG2_PACK'
  else if IsEqualGUID(guid, MEDIATYPE_MPEG2_PES) then
    result := 'MPEG2_PES'
  else if IsEqualGUID(guid, MEDIATYPE_MPEG2_SECTIONS) then
    result := 'MPEG2_SECTIONS'
  else if IsEqualGUID(guid, MEDIATYPE_CONTROL) then
    result := 'CONTROL'
  else
    result := 'UnKnown: ' + GUIDToString(guid);
end;

function SubTypeToString(guid: TGUID): WideString;
begin
  // sub types
  if IsEqualGUID(guid, MEDIASUBTYPE_CLPL) then
    result := 'CLPL'
  else if IsEqualGUID(guid, MEDIASUBTYPE_YUYV) then
    result := 'YUYV'
  else if IsEqualGUID(guid, MEDIASUBTYPE_IYUV) then
    result := 'IYUV'
  else if IsEqualGUID(guid, MEDIASUBTYPE_YVU9) then
    result := 'YVU9'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Y411) then
    result := 'Y411'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Y41P) then
    result := 'Y41P'
  else if IsEqualGUID(guid, MEDIASUBTYPE_YUY2) then
    result := 'YUY2'
  else if IsEqualGUID(guid, MEDIASUBTYPE_YVYU) then
    result := 'YVYU'
  else if IsEqualGUID(guid, MEDIASUBTYPE_UYVY) then
    result := 'UYVY'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Y211) then
    result := 'Y211'
  else if IsEqualGUID(guid, MEDIASUBTYPE_YV12) then
    result := 'YV12'
  else if IsEqualGUID(guid, MEDIASUBTYPE_CLJR) then
    result := 'CLJR'
  else if IsEqualGUID(guid, MEDIASUBTYPE_IF09) then
    result := 'IF09'
  else if IsEqualGUID(guid, MEDIASUBTYPE_CPLA) then
    result := 'CPLA'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MJPG) then
    result := 'MJPG'
  else if IsEqualGUID(guid, MEDIASUBTYPE_TVMJ) then
    result := 'TVMJ'
  else if IsEqualGUID(guid, MEDIASUBTYPE_WAKE) then
    result := 'WAKE'
  else if IsEqualGUID(guid, MEDIASUBTYPE_CFCC) then
    result := 'CFCC'
  else if IsEqualGUID(guid, MEDIASUBTYPE_IJPG) then
    result := 'IJPG'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Plum) then
    result := 'Plum'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DVCS) then
    result := 'DVCS'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DVSD) then
    result := 'DVSD'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MDVF) then
    result := 'MDVF'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RGB1) then
    result := 'RGB1'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RGB4) then
    result := 'RGB4'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RGB8) then
    result := 'RGB8'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RGB565) then
    result := 'RGB565'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RGB555) then
    result := 'RGB555'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RGB24) then
    result := 'RGB24'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RGB32) then
    result := 'RGB32'
  else if IsEqualGUID(guid, MEDIASUBTYPE_ARGB32) then
    result := 'ARGB32'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Overlay) then
    result := 'Overlay'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG1Packet) then
    result := 'MPEG1Packet'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG1Payload) then
    result := 'MPEG1Payload'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG1AudioPayload) then
    result := 'MPEG1AudioPayload'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG1System) then
    result := 'MPEG1System'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG1VideoCD) then
    result := 'MPEG1VideoCD'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG1Video) then
    result := 'MPEG1Video'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG1Audio) then
    result := 'MPEG1Audio'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Avi) then
    result := 'Avi'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Asf) then
    result := 'Asf'
  else if IsEqualGUID(guid, MEDIASUBTYPE_QTMovie) then
    result := 'QTMovie'
  else if IsEqualGUID(guid, MEDIASUBTYPE_QTRpza) then
    result := 'QTRpza'
  else if IsEqualGUID(guid, MEDIASUBTYPE_QTSmc) then
    result := 'QTSmc'
  else if IsEqualGUID(guid, MEDIASUBTYPE_QTRle) then
    result := 'QTRle'
  else if IsEqualGUID(guid, MEDIASUBTYPE_QTJpeg) then
    result := 'QTJpeg'
  else if IsEqualGUID(guid, MEDIASUBTYPE_PCMAudio_Obsolete) then
    result := 'PCMAudio_Obsolete'
  else if IsEqualGUID(guid, MEDIASUBTYPE_PCM) then
    result := 'PCM'
  else if IsEqualGUID(guid, MEDIASUBTYPE_WAVE) then
    result := 'WAVE'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AU) then
    result := 'AU'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AIFF) then
    result := 'AIFF'
  else if IsEqualGUID(guid, MEDIASUBTYPE_dvsd_) then
    result := 'dvsd_'
  else if IsEqualGUID(guid, MEDIASUBTYPE_dvhd) then
    result := 'dvhd'
  else if IsEqualGUID(guid, MEDIASUBTYPE_dvsl) then
    result := 'dvsl'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Line21_BytePair) then
    result := 'Line21_BytePair'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Line21_GOPPacket) then
    result := 'Line21_GOPPacket'
  else if IsEqualGUID(guid, MEDIASUBTYPE_Line21_VBIRawData) then
    result := 'Line21_VBIRawData'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DRM_Audio) then
    result := 'DRM_Audio'
  else if IsEqualGUID(guid, MEDIASUBTYPE_IEEE_FLOAT) then
    result := 'IEEE_FLOAT'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DOLBY_AC3_SPDIF) then
    result := 'DOLBY_AC3_SPDIF'
  else if IsEqualGUID(guid, MEDIASUBTYPE_RAW_SPORT) then
    result := 'RAW_SPORT'
  else if IsEqualGUID(guid, MEDIASUBTYPE_SPDIF_TAG_241h) then
    result := 'SPDIF_TAG_241h'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DssVideo) then
    result := 'DssVideo'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DssAudio) then
    result := 'DssAudio'
  else if IsEqualGUID(guid, MEDIASUBTYPE_VPVideo) then
    result := 'VPVideo'
  else if IsEqualGUID(guid, MEDIASUBTYPE_VPVBI) then
    result := 'VPVBI'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_NTSC_M) then
    result := 'AnalogVideo_NTSC_M'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_B) then
    result := 'AnalogVideo_PAL_B'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_D) then
    result := 'AnalogVideo_PAL_D'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_G) then
    result := 'AnalogVideo_PAL_G'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_H) then
    result := 'AnalogVideo_PAL_H'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_I) then
    result := 'AnalogVideo_PAL_I'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_M) then
    result := 'AnalogVideo_PAL_M'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_N) then
    result := 'AnalogVideo_PAL_N'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_PAL_N_COMBO) then
    result := 'AnalogVideo_PAL_N_COMBO'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_SECAM_B) then
    result := 'AnalogVideo_SECAM_B'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_SECAM_D) then
    result := 'AnalogVideo_SECAM_D'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_SECAM_G) then
    result := 'AnalogVideo_SECAM_G'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_SECAM_H) then
    result := 'AnalogVideo_SECAM_H'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_SECAM_K) then
    result := 'AnalogVideo_SECAM_K'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_SECAM_K1) then
    result := 'AnalogVideo_SECAM_K1'
  else if IsEqualGUID(guid, MEDIASUBTYPE_AnalogVideo_SECAM_L) then
    result := 'AnalogVideo_SECAM_L'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG2_VIDEO) then
    result := 'MPEG2_VIDEO'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG2_PROGRAM) then
    result := 'MPEG2_PROGRAM'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG2_TRANSPORT) then
    result := 'MPEG2_TRANSPORT'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MPEG2_AUDIO) then
    result := 'MPEG2_AUDIO'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DOLBY_AC3) then
    result := 'DOLBY_AC3'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DVD_SUBPICTURE) then
    result := 'DVD_SUBPICTURE'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DVD_LPCM_AUDIO) then
    result := 'DVD_LPCM_AUDIO'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DTS) then
    result := 'DTS'
  else if IsEqualGUID(guid, MEDIASUBTYPE_SDDS) then
    result := 'SDDS'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DVD_NAVIGATION_PCI) then
    result := 'PCI'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DVD_NAVIGATION_DSI) then
    result := 'DSI'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DVD_NAVIGATION_PROVIDER) then
    result := 'PROVIDER'
  else if IsEqualGUID(guid, MEDIASUBTYPE_MP42) then
    result := 'MS-MPEG4'
  else if IsEqualGUID(guid, MEDIASUBTYPE_DIVX) then
    result := 'DIVX'
  else if IsEqualGUID(guid, MEDIASUBTYPE_VOXWARE) then
    result := 'VOXWARE_MetaSound'
  else if IsEqualGUID(guid, MEDIASUBTYPE_VPVBI) then
    result := 'VPVBI'
  else
    result := 'UnKnown: ' + GUIDToString(guid);
end;

function FormatTypeToString(guid: TGUID): WideString;
begin
  // sub types
  if IsEqualGUID(guid, FORMAT_None) then
    result := 'None'
  else if IsEqualGUID(guid, FORMAT_VideoInfo) then
    result := 'VideoInfo'
  else if IsEqualGUID(guid, FORMAT_VideoInfo2) then
    result := 'VideoInfo2'
  else if IsEqualGUID(guid, FORMAT_WaveFormatEx) then
    result := 'WaveFormatEx'
  else if IsEqualGUID(guid, FORMAT_MPEGVideo) then
    result := 'MPEG Video'
  else if IsEqualGUID(guid, FORMAT_MPEGStreams) then
    result := 'MPEG Streams'
  else if IsEqualGUID(guid, FORMAT_DvInfo) then
    result := 'DV Info'
  else if IsEqualGUID(guid, FORMAT_AnalogVideo) then
    result := 'AnalogVideo'
  else if IsEqualGUID(guid, FORMAT_MPEG2_VIDEO) then
    result := 'MPEG-2 Video'
  else if IsEqualGUID(guid, FORMAT_MPEG2Video) then
    result := 'MPEG-2 Video'
  else if IsEqualGUID(guid, FORMAT_DolbyAC3) then
    result := 'Dolby AC3'
  else if IsEqualGUID(guid, FORMAT_MPEG2Audio) then
    result := 'MPEG-2 Audio'
  else if IsEqualGUID(guid, FORMAT_DVD_LPCMAudio) then
    result := 'DVD LPCM Audio'
  else
    result := 'UnKnown: ' + GUIDToString(guid);
end;

function AudioTagToString(code: Integer): WideString;
begin
  case code of
    $0001:
      result := 'PCM'; // common
    $0002:
      result := 'ADPCM';
    $0003:
      result := 'IEEE_FLOAT';
    $0005:
      result := 'IBM_CVSD';
    $0006:
      result := 'ALAW';
    $0007:
      result := 'MULAW';
    $0010:
      result := 'OKI_ADPCM';
    $0011:
      result := 'DVI_ADPCM';
    $0012:
      result := 'MEDIASPACE_ADPCM';
    $0013:
      result := 'SIERRA_ADPCM';
    $0014:
      result := 'G723_ADPCM';
    $0015:
      result := 'DIGISTD';
    $0016:
      result := 'DIGIFIX';
    $0017:
      result := 'DIALOGIC_OKI_ADPCM';
    $0018:
      result := 'MEDIAVISION_ADPCM';
    $0020:
      result := 'YAMAHA_ADPCM';
    $0021:
      result := 'SONARC';
    $0022:
      result := 'DSPGROUP_TRUESPEECH';
    $0023:
      result := 'ECHOSC1';
    $0024:
      result := 'AUDIOFILE_AF36';
    $0025:
      result := 'APTX';
    $0026:
      result := 'AUDIOFILE_AF10';
    $0030:
      result := 'DOLBY_AC2';
    $0031:
      result := 'GSM610';
    $0032:
      result := 'MSNAUDIO';
    $0033:
      result := 'ANTEX_ADPCME';
    $0034:
      result := 'CONTROL_RES_VQLPC';
    $0035:
      result := 'DIGIREAL';
    $0036:
      result := 'DIGIADPCM';
    $0037:
      result := 'CONTROL_RES_CR10';
    $0038:
      result := 'NMS_VBXADPCM';
    $0039:
      result := 'CS_IMAADPCM';
    $003A:
      result := 'ECHOSC3';
    $003B:
      result := 'ROCKWELL_ADPCM';
    $003C:
      result := 'ROCKWELL_DIGITALK';
    $003D:
      result := 'XEBEC';
    $0040:
      result := 'G721_ADPCM';
    $0041:
      result := 'G728_CELP';
    $0050:
      result := 'MPEG';
    $0055:
      result := 'MPEGLAYER3';
    $0060:
      result := 'CIRRUS';
    $0061:
      result := 'ESPCM';
    $0062:
      result := 'VOXWARE';
    $0063:
      result := 'CANOPUS_ATRAC';
    $0064:
      result := 'G726_ADPCM';
    $0065:
      result := 'G722_ADPCM';
    $0066:
      result := 'DSAT';
    $0067:
      result := 'DSAT_DISPLAY';
    $0075:
      result := 'VOXWARE'; // aditionnal  ???
    $0080:
      result := 'SOFTSOUND';
    $0100:
      result := 'RHETOREX_ADPCM';
    $0200:
      result := 'CREATIVE_ADPCM';
    $0202:
      result := 'CREATIVE_FASTSPEECH8';
    $0203:
      result := 'CREATIVE_FASTSPEECH10';
    $0220:
      result := 'QUARTERDECK';
    $0300:
      result := 'FM_TOWNS_SND';
    $0400:
      result := 'BTV_DIGITAL';
    $1000:
      result := 'OLIGSM';
    $1001:
      result := 'OLIADPCM';
    $1002:
      result := 'OLICELP';
    $1003:
      result := 'OLISBC';
    $1004:
      result := 'OLIOPR';
    $1100:
      result := 'LH_CODEC';
    $1400:
      result := 'NORRIS';
  else
    result := 'Unknown';
  end;
end;

function GetMediaTypeDescription(MediaType: PAMMEDIATYPE): WideString;
begin
  result := '';

  // format
  if IsEqualGUID(MediaType.formattype, FORMAT_VideoInfo) then
  begin
    // result := result + 'VideoInfo ';
    if ((MediaType.cbFormat > 0) and Assigned(MediaType.pbFormat)) then
      with PVIDEOINFOHEADER(MediaType.pbFormat)^.bmiHeader do
        result := result + format('%dx%d %s, %d bits',
          [biWidth, biHeight, GetFOURCC(biCompression), biBitCount]);
  end
  else if IsEqualGUID(MediaType.formattype, FORMAT_VideoInfo2) then
  begin
    // result := result + 'VideoInfo2 ';
    if ((MediaType.cbFormat > 0) and Assigned(MediaType.pbFormat)) then
      with PVIDEOINFOHEADER2(MediaType.pbFormat)^.bmiHeader do
        result := result + format('%dx%d %s, %d bits',
          [biWidth, biHeight, GetFOURCC(biCompression), biBitCount]);
  end
  else if IsEqualGUID(MediaType.formattype, FORMAT_WaveFormatEx) then
  begin
    // result := result + 'WaveFormatEx: ';
    if ((MediaType.cbFormat > 0) and Assigned(MediaType.pbFormat)) then
    begin
      result := result + AudioTagToString(PWAVEFORMATEX(MediaType.pbFormat)
        ^.wFormatTag);

      with PWAVEFORMATEX(MediaType.pbFormat)^ do
        result := result + format(', %d Hz, %d Bits, %d Channels',
          [nSamplesPerSec, wBitsPerSample, nChannels]);
    end;
  end
  else if IsEqualGUID(MediaType.formattype, FORMAT_MPEGVideo) then
  begin
    result := result + 'MPEG1 ';
    if ((MediaType.cbFormat > 0) and Assigned(MediaType.pbFormat)) then
      with PMPEG1VideoInfo(MediaType.pbFormat)^.hdr.bmiHeader do
        result := result + format('%s %dX%d, %d bits',
          [GetFOURCC(biCompression), biWidth, biHeight, biBitCount]);
  end
  else if IsEqualGUID(MediaType.formattype, FORMAT_MPEG2Video) then
  begin
    result := result + 'MPEG2 ';
    if ((MediaType.cbFormat > 0) and Assigned(MediaType.pbFormat)) then
      with PMPEG2VideoInfo(MediaType.pbFormat)^.hdr.bmiHeader do
        result := result + format('%s %dX%d, %d bits',
          [GetFOURCC(biCompression), biWidth, biHeight, biBitCount]);
  end
  else if IsEqualGUID(MediaType.formattype, FORMAT_DvInfo) then
    result := result + 'DV'
  else if IsEqualGUID(MediaType.formattype, FORMAT_MPEGStreams) then
    result := result + 'MPEGStreams'
  else if IsEqualGUID(MediaType.formattype, FORMAT_DolbyAC3) then
    result := result + 'DolbyAC3'
  else if IsEqualGUID(MediaType.formattype, FORMAT_MPEG2Audio) then
    result := result + 'MPEG2 Audio'
  else if IsEqualGUID(MediaType.formattype, FORMAT_DVD_LPCMAudio) then
    result := result + 'DVD_LPCMAudio'
  else
  begin
    result := 'MajorType: ' + MajorTypeToString(MediaType.majortype);
    result := result + ', SubType: ' + SubTypeToString(MediaType.majortype);
    result := result + ', FormatType: ' + FormatTypeToString
      (MediaType.formattype);
  end;
end;

procedure CopyMediaType(pmtTarget: PAMMEDIATYPE; pmtSource: PAMMEDIATYPE);
begin
  // We'll leak if we copy onto one that already exists - there's one
  // case we can check like that - copying to itself.
  ASSERT(pmtSource <> pmtTarget);
  // pmtTarget^ := pmtSource^;
  move(pmtSource^, pmtTarget^, SizeOf(TAMMediaType));
  if (pmtSource.cbFormat <> 0) then
  begin
    ASSERT(pmtSource.pbFormat <> nil);
    pmtTarget.pbFormat := CoTaskMemAlloc(pmtSource.cbFormat);
    if (pmtTarget.pbFormat = nil) then
      pmtTarget.cbFormat := 0
    else
      CopyMemory(pmtTarget.pbFormat, pmtSource.pbFormat, pmtTarget.cbFormat);
  end;
  if (pmtTarget.pUnk <> nil) then
    pmtTarget.pUnk._AddRef;
end;

// copy constructor does a deep copy of the format block

constructor TVFMediaType.Create;
begin
  InitMediaType;
end;

constructor TVFMediaType.Create(MediaType: PAMMEDIATYPE);
begin
  InitMediaType;
  CopyMediaType(AMMediaType, MediaType);
end;

constructor TVFMediaType.Create(majortype: TGUID);
begin
  InitMediaType;
  AMMediaType.majortype := majortype;
end;

constructor TVFMediaType.Create(MTClass: TVFMediaType);
begin
  InitMediaType;
  CopyMediaType(AMMediaType, MTClass.AMMediaType);
end;

destructor TVFMediaType.Destroy;
begin
  FreeMediaType(AMMediaType);
  dispose(AMMediaType);
  inherited Destroy;
end;

// allocate length bytes for the format and return a read/write pointer
// If we cannot allocate the new block of memory we return NULL leaving
// the original block of memory untouched (as does ReallocFormatBuffer)

function TVFMediaType.AllocFormatBuffer(length: ULONG): pointer;
var
  pNewFormat: pointer;
begin
  ASSERT(length <> 0);

  // do the types have the same buffer size
  if (AMMediaType.cbFormat = length) then
  begin
    result := AMMediaType.pbFormat;
    Exit;
  end;

  // allocate the new format buffer
  pNewFormat := CoTaskMemAlloc(length);
  if (pNewFormat = nil) then
  begin
    if (length <= AMMediaType.cbFormat) then
    begin
      result := AMMediaType.pbFormat; // reuse the old block anyway.
      Exit;
    end
    else
    begin
      result := nil;
      Exit;
    end;
  end;

  // delete the old format
  if (AMMediaType.cbFormat <> 0) then
  begin
    ASSERT(AMMediaType.pbFormat <> nil);
    CoTaskMemFree(AMMediaType.pbFormat);
  end;

  AMMediaType.cbFormat := length;
  AMMediaType.pbFormat := pNewFormat;
  result := AMMediaType.pbFormat;
end;

// copy MTClass.AMMediaType to current AMMediaType

procedure TVFMediaType.Assign(Source: TPersistent);
begin
  if Source is TVFMediaType then
  begin
    if (Source <> Self) then
    begin
      FreeMediaType(AMMediaType);
      CopyMediaType(AMMediaType, TVFMediaType(Source).AMMediaType);
    end;
  end
  else
    inherited Assign(Source);
end;

procedure TVFMediaType.DefineProperties(Filer: TFiler);

  function DoWrite: Boolean;
  begin
    result := true;
    if Filer.Ancestor <> nil then
    begin
      result := true;
      if Filer.Ancestor is TVFMediaType then
        result := not Equal(TVFMediaType(Filer.Ancestor))
    end;
  end;

begin
  Filer.DefineBinaryProperty('data', ReadData, WriteData, DoWrite);
end;

function TVFMediaType.Equal(MTClass: TVFMediaType): Boolean;
begin
  // I don't believe we need to check sample size or
  // temporal compression flags, since I think these must
  // be represented in the type, subtype and format somehow. They
  // are pulled out as separate flags so that people who don't understand
  // the particular format representation can still see them, but
  // they should duplicate information in the format block.
  result := ((IsEqualGUID(AMMediaType.majortype, MTClass.AMMediaType.majortype)
    = true) and (IsEqualGUID(AMMediaType.subtype, MTClass.AMMediaType.subtype)
    = true) and (IsEqualGUID(AMMediaType.formattype,
    MTClass.AMMediaType.formattype) = true) and
    (AMMediaType.cbFormat = MTClass.AMMediaType.cbFormat) and
    ((AMMediaType.cbFormat = 0) or (CompareMem(AMMediaType.pbFormat,
    MTClass.AMMediaType.pbFormat, AMMediaType.cbFormat))));
end;

// Retrieves a pointer to the format block.

function TVFMediaType.format: pointer;
begin
  result := AMMediaType.pbFormat;
end;

// Retrieves the length of the format block.

function TVFMediaType.FormatLength: ULONG;
begin
  result := AMMediaType.cbFormat;
end;

function TVFMediaType.GetFormatType: TGUID;
begin
  result := AMMediaType.formattype;
end;

function TVFMediaType.GetMajorType: TGUID;
begin
  result := AMMediaType.majortype;
end;

// If the sample size is fixed, returns the sample size in bytes. Otherwise,
// returns zero.

function TVFMediaType.GetSampleSize: ULONG;
begin
  if IsFixedSize then
    result := AMMediaType.lSampleSize
  else
    result := 0;
end;

function TVFMediaType.GetSubType: TGUID;
begin
  result := AMMediaType.subtype;
end;

// initialise a media type structure

procedure TVFMediaType.InitMediaType;
begin
  new(AMMediaType);
  ZeroMemory(AMMediaType, SizeOf(TAMMediaType));
  AMMediaType.lSampleSize := 1;
  AMMediaType.bFixedSizeSamples := true;
end;

// Determines if the samples have a fixed size or a variable size.

function TVFMediaType.IsFixedSize: Boolean;
begin
  result := AMMediaType.bFixedSizeSamples;
end;

// a partially specified media type can be passed to IPin::Connect
// as a constraint on the media type used in the connection.
// the type, subtype or format type can be null.

function TVFMediaType.IsPartiallySpecified: Boolean;
begin
  if (IsEqualGUID(AMMediaType.majortype, GUID_NULL) or
    IsEqualGUID(AMMediaType.formattype, GUID_NULL)) then
  begin
    result := true;
    Exit;
  end
  else
  begin
    result := false;
    Exit;
  end;
end;

// Determines if the stream uses temporal compression.

function TVFMediaType.IsTemporalCompressed: Boolean;
begin
  result := AMMediaType.bTemporalCompression;
end;

// By default, TDSMediaType objects are initialized with a major type of GUID_NULL.
// Call this method to determine whether the object has been correctly initialized.

function TVFMediaType.IsValid: Boolean;
begin
  result := not IsEqualGUID(AMMediaType.majortype, GUID_NULL);
end;

// Determines if this media type matches a partially specified media type.

function TVFMediaType.MatchesPartial(ppartial: TVFMediaType): Boolean;
begin
  if (not IsEqualGUID(ppartial.AMMediaType.majortype, GUID_NULL) and
    not IsEqualGUID(AMMediaType.majortype, ppartial.AMMediaType.majortype)) then
  begin
    result := false;
    Exit;
  end;
  if (not IsEqualGUID(ppartial.AMMediaType.subtype, GUID_NULL) and
    not IsEqualGUID(AMMediaType.subtype, ppartial.AMMediaType.subtype)) then
  begin
    result := false;
    Exit;
  end;

  if not IsEqualGUID(ppartial.AMMediaType.formattype, GUID_NULL) then
  begin
    // if the format block is specified then it must match exactly
    if not IsEqualGUID(AMMediaType.formattype, ppartial.AMMediaType.formattype)
    then
    begin
      result := false;
      Exit;
    end;
    if (AMMediaType.cbFormat <> ppartial.AMMediaType.cbFormat) then
    begin
      result := false;
      Exit;
    end;
    if ((AMMediaType.cbFormat <> 0) and (CompareMem(AMMediaType.pbFormat,
      ppartial.AMMediaType.pbFormat, AMMediaType.cbFormat) <> false)) then
    begin
      result := false;
      Exit;
    end;
  end;
  result := true;
end;

// Check to see if they are equal

function TVFMediaType.NotEqual(MTClass: TVFMediaType): Boolean;
begin
  if (Self = MTClass) then
    result := false
  else
    result := true;
end;

// this class inherits publicly from AM_MEDIA_TYPE so the compiler could generate
// the following assignment operator itself, however it could introduce some
// memory conflicts and leaks in the process because the structure contains
// a dynamically allocated block (pbFormat) which it will not copy correctly

procedure TVFMediaType.Read(MediaType: PAMMEDIATYPE);
begin
  if (MediaType <> Self.AMMediaType) then
  begin
    FreeMediaType(AMMediaType);
    CopyMediaType(AMMediaType, MediaType);
  end;
end;

procedure TVFMediaType.ReadData(Stream: TStream);
begin
  ResetFormatBuffer;
  Stream.Read(AMMediaType^, SizeOf(TAMMediaType));
  if FormatLength > 0 then
  begin
    AMMediaType.pbFormat := CoTaskMemAlloc(FormatLength);
    Stream.Read(AMMediaType.pbFormat^, FormatLength)
  end;
end;

// reallocate length bytes for the format and return a read/write pointer
// to it. We keep as much information as we can given the new buffer size
// if this fails the original format buffer is left untouched. The caller
// is responsible for ensuring the size of memory required is non zero

function TVFMediaType.ReallocFormatBuffer(length: ULONG): pointer;
var
  pNewFormat: pointer;
begin
  ASSERT(length <> 0);

  // do the types have the same buffer size
  if (AMMediaType.cbFormat = length) then
  begin
    result := AMMediaType.pbFormat;
    Exit;
  end;

  // allocate the new format buffer
  pNewFormat := CoTaskMemAlloc(length);
  if (pNewFormat = nil) then
  begin
    if (length <= AMMediaType.cbFormat) then
    begin
      result := AMMediaType.pbFormat; // reuse the old block anyway.
      Exit;
    end
    else
    begin
      result := nil;
      Exit;
    end;
  end;

  // copy any previous format (or part of if new is smaller)
  // delete the old format and replace with the new one
  if (AMMediaType.cbFormat <> 0) then
  begin
    ASSERT(AMMediaType.pbFormat <> nil);
    CopyMemory(pNewFormat, AMMediaType.pbFormat,
      Min(length, AMMediaType.cbFormat));
    CoTaskMemFree(AMMediaType.pbFormat);
  end;

  AMMediaType.cbFormat := length;
  AMMediaType.pbFormat := pNewFormat;
  result := pNewFormat;
end;

// reset the format buffer

procedure TVFMediaType.ResetFormatBuffer;
begin
  if (AMMediaType.cbFormat <> 0) then
    CoTaskMemFree(AMMediaType.pbFormat);
  AMMediaType.cbFormat := 0;
  AMMediaType.pbFormat := nil;
end;

function TVFMediaType.SetFormat(pFormat: pointer; length: ULONG): Boolean;
begin
  if (nil = AllocFormatBuffer(length)) then
  begin
    result := false;
    Exit;
  end;
  ASSERT(AMMediaType.pbFormat <> nil);
  CopyMemory(AMMediaType.pbFormat, pFormat, length);
  result := true;
end;

// set the type of the media type format block, this type defines what you
// will actually find in the format pointer. For example FORMAT_VideoInfo or
// FORMAT_WaveFormatEx. In the future this may be an interface pointer to a
// property set. Before sending out media types this should be filled in.

procedure TVFMediaType.SetFormatType(const guid: TGUID);
begin
  AMMediaType.formattype := guid;
end;

procedure TVFMediaType.SetMajorType(MT: TGUID);
begin
  AMMediaType.majortype := MT;
end;

// If value of sz is zero, the media type uses variable sample sizes. Otherwise,
// the sample size is fixed at sz bytes.

procedure TVFMediaType.SetSampleSize(SZ: ULONG);
begin
  if (SZ = 0) then
  begin
    SetVariableSize;
  end
  else
  begin
    AMMediaType.bFixedSizeSamples := true;
    AMMediaType.lSampleSize := SZ;
  end;
end;

procedure TVFMediaType.SetSubType(ST: TGUID);
begin
  AMMediaType.subtype := ST;
end;

// Specifies whether samples are compressed using temporal compression

procedure TVFMediaType.SetTemporalCompression(bCompressed: Boolean);
begin
  AMMediaType.bTemporalCompression := bCompressed;
end;

// Specifies that samples do not have a fixed size.

procedure TVFMediaType.SetVariableSize;
begin
  AMMediaType.bFixedSizeSamples := false;
end;

procedure TVFMediaType.WriteData(Stream: TStream);
begin
  Stream.Write(AMMediaType^, SizeOf(TAMMediaType));
  if FormatLength > 0 then
    Stream.Write(AMMediaType.pbFormat^, FormatLength);
end;

constructor TVFEnumMediaType.Create;
begin
  FList := TList.Create;
end;

constructor TVFEnumMediaType.Create(EnumMT: IEnumMediaTypes);
var
  pmt: PAMMEDIATYPE;
begin
  if (FList = nil) then
    FList := TList.Create;
  ASSERT(EnumMT <> nil, 'IEnumMediaType not assigned');
  while (EnumMT.Next(1, pmt, nil) = S_OK) do
  begin
    FList.Add(TVFMediaType.Create(pmt));
  end;
end;

constructor TVFEnumMediaType.Create(Pin: IPin);
var
  EnumMT: IEnumMediaTypes;
  hr: HRESULT;
begin
  FList := TList.Create;
  ASSERT(Pin <> nil, 'IPin not assigned');
  hr := Pin.EnumMediaTypes(EnumMT);
  if (hr <> S_OK) then
    Exit;
  Create(EnumMT);
end;

constructor TVFEnumMediaType.Create(FileName: TFileName);
begin
  FList := TList.Create;
  Assign(FileName);
end;

destructor TVFEnumMediaType.Destroy;
begin
  Clear;
  FList.Free;
end;

function TVFEnumMediaType.Add(item: TVFMediaType): Integer;
begin
  result := FList.Add(item);
end;

procedure TVFEnumMediaType.Assign(EnumMT: IEnumMediaTypes);
var
  pmt: PAMMEDIATYPE;
begin
  if (Count <> 0) then
    Clear;
  ASSERT(EnumMT <> nil, 'IEnumMediaType not assigned');
  while (EnumMT.Next(1, pmt, nil) = S_OK) do
  begin
    FList.Add(TVFMediaType.Create(pmt));
  end;
end;

procedure TVFEnumMediaType.Assign(Pin: IPin);
var
  EnumMT: IEnumMediaTypes;
  hr: HRESULT;
begin
  Clear;
  ASSERT(Pin <> nil, 'IPin not assigned');
  hr := Pin.EnumMediaTypes(EnumMT);
  if (hr <> S_OK) then
    Exit;
  Assign(EnumMT);
end;

procedure TVFEnumMediaType.Assign(FileName: TFileName);
var
  MediaDet: IMediaDet;
  KeyProvider: IServiceProvider;
  hr: HRESULT;
  Streams: longint;
  i: longint;
  MediaType: TAMMediaType;
begin
  Clear;
  hr := CoCreateInstance(CLSID_MediaDet, nil, CLSCTX_INPROC, IID_IMediaDet,
    MediaDet);
  // milenko start get rid of compiler warnings ...
  if (hr = S_OK) then
  begin
  end;
  // milenko end;
  ASSERT(hr = S_OK, 'Media Detector not available');
  hr := MediaDet.put_Filename(FileName);
  if hr <> S_OK then
  begin
    MediaDet := nil;
    Exit;
  end;
  MediaDet.get_OutputStreams(Streams);
  if Streams > 0 then
  begin
    for i := 0 to (Streams - 1) do
    begin
      MediaDet.put_CurrentStream(i);
      MediaDet.get_StreamMediaType(MediaType);
      FList.Add(TVFMediaType.Create(@MediaType));
    end;
  end;
  KeyProvider := nil;
  MediaDet := nil;
end;

procedure TVFEnumMediaType.Clear;
var
  i: Integer;
begin
  if Count <> 0 then
    for i := 0 to (Count - 1) do
    begin
      if (FList.Items[i] <> nil) then
        TVFMediaType(FList.Items[i]).Free;
    end;
  FList.Clear;
end;

procedure TVFEnumMediaType.Delete(Index: Integer);
begin
  if (FList.Items[index] <> nil) then
    TVFMediaType(FList.Items[index]).Free;
  FList.Delete(index);
end;

function TVFEnumMediaType.GetCount: Integer;
begin
  ASSERT(FList <> nil, 'TDSEnumMediaType not created');
  if (FList <> nil) then
    result := FList.Count
  else
    result := 0;
end;

function TVFEnumMediaType.GetItem(Index: Integer): TVFMediaType;
begin
  result := TVFMediaType(FList.Items[index]);
end;

function TVFEnumMediaType.GetMediaDescription(Index: Integer): WideString;
begin
  result := '';
  if ((index < Count) and (index > -1)) then
    result := GetMediaTypeDescription(TVFMediaType(FList.Items[index])
      .AMMediaType);
end;

procedure TVFEnumMediaType.SetItem(Index: Integer; item: TVFMediaType);
begin
  TVFMediaType(FList.Items[index]).Assign(item);
end;

constructor TVFFilterList.Create(filterGraph: IFilterGraph);
begin
  inherited Create;
  Graph := filterGraph;
  Update;
end;

destructor TVFFilterList.Destroy;
begin
  inherited Destroy;
end;

function TVFFilterList.Add(item: IBaseFilter): Integer;
begin
  result := inherited Add(item);
end;

procedure TVFFilterList.Assign(filterGraph: IFilterGraph);
begin
  Clear;
  Graph := filterGraph;
  Update;
end;

function TVFFilterList.First: IBaseFilter;
begin
  result := GetFilter(0);
end;

function TVFFilterList.GetFilter(Index: Integer): IBaseFilter;
begin
  result := Get(index) as IBaseFilter;
end;

function TVFFilterList.GetFilterInfo(Index: Integer): TFilterInfo;
begin
  if Assigned(Items[index]) then
    Items[index].QueryFilterInfo(result);
end;

function TVFFilterList.IndexOf(item: IBaseFilter): Integer;
begin
  result := inherited IndexOf(item);
end;

procedure TVFFilterList.Insert(Index: Integer; item: IBaseFilter);
begin
  inherited Insert(index, item);
end;

function TVFFilterList.Last: IBaseFilter;
begin
  result := inherited Last as IBaseFilter;
end;

procedure TVFFilterList.PutFilter(Index: Integer; item: IBaseFilter);
begin
  Put(index, item);
end;

function TVFFilterList.Remove(item: IBaseFilter): Integer;
begin
  result := inherited Remove(item);
end;

procedure TVFFilterList.Update;
var
  EnumFilters: IEnumFilters;
  Filter: IBaseFilter;
begin
  if Assigned(Graph) then
    Graph.EnumFilters(EnumFilters);
  while (EnumFilters.Next(1, Filter, nil) = S_OK) do
    Add(Filter);
  EnumFilters := nil;
end;

constructor TVFPinList.Create(BaseFilter: IBaseFilter);
begin
  inherited Create;
  Filter := BaseFilter;
  Update;
end;

destructor TVFPinList.Destroy;
begin
  Filter := nil;
  inherited Destroy;
end;

function TVFPinList.Add(item: IPin): Integer;
begin
  result := inherited Add(item);
end;

procedure TVFPinList.Assign(BaseFilter: IBaseFilter);
begin
  Clear;
  Filter := BaseFilter;
  if Filter <> nil then
    Update;
end;

function TVFPinList.First: IPin;
begin
  result := GetPin(0);
end;

function TVFPinList.GetConnected(Index: Integer): Boolean;
var
  Pin: IPin;
begin
  Items[Index].ConnectedTo(Pin);
  result := (Pin <> nil);
end;

function TVFPinList.GetPin(Index: Integer): IPin;
begin
  result := Get(index) as IPin;
end;

function TVFPinList.GetPinInfo(Index: Integer): TPinInfo;
begin
  if Assigned(Items[index]) then
    Items[index].QueryPinInfo(result);
end;

function TVFPinList.IndexOf(item: IPin): Integer;
begin
  result := inherited IndexOf(item);
end;

procedure TVFPinList.Insert(Index: Integer; item: IPin);
begin
  inherited Insert(index, item);
end;

function TVFPinList.Last: IPin;
begin
  result := inherited Last as IPin;
end;

procedure TVFPinList.PutPin(Index: Integer; item: IPin);
begin
  Put(index, item);
end;

function TVFPinList.Remove(item: IPin): Integer;
begin
  result := inherited Remove(item);
end;

procedure TVFPinList.Update;
var
  EnumPins: IEnumPins;
  Pin: IPin;
begin
  Clear;
  if Assigned(Filter) then
    Filter.EnumPins(EnumPins)
  else
    Exit;
  while (EnumPins.Next(1, Pin, nil) = S_OK) do
    Add(Pin);
  EnumPins := nil;
end;

function ShowFilterPropertyPage(parent: THandle; Filter: IBaseFilter;
  PropertyPage: TVFPropertyPage = ppDefault): HRESULT;
var
  SpecifyPropertyPages: ISpecifyPropertyPages;
  CaptureDialog: IAMVfwCaptureDialogs;
  CompressDialog: IAMVfwCompressDialogs;
  CAGUID: TCAGUID;
  FilterInfo: TFilterInfo;
  code: Integer;
begin
  result := S_FALSE;
  code := 0;
  if Filter = nil then
    Exit;

  ZeroMemory(@FilterInfo, SizeOf(TFilterInfo));

  case PropertyPage of
    ppVFWCapDisplay:
      code := VfwCaptureDialog_Display;
    ppVFWCapFormat:
      code := VfwCaptureDialog_Format;
    ppVFWCapSource:
      code := VfwCaptureDialog_Source;
    ppVFWCompConfig:
      code := VfwCompressDialog_Config;
    ppVFWCompAbout:
      code := VfwCompressDialog_About;
  end;

  case PropertyPage of
    ppDefault:
      begin
        result := Filter.QueryInterface(IID_ISpecifyPropertyPages,
          SpecifyPropertyPages);
        if result <> S_OK then
          Exit;
        result := SpecifyPropertyPages.GetPages(CAGUID);
        if result <> S_OK then
          Exit;
        result := Filter.QueryFilterInfo(FilterInfo);
        if result = S_OK then
        begin
          result := OleCreatePropertyFrame(parent, 0, 0, FilterInfo.achName, 1,
            @Filter, CAGUID.cElems, CAGUID.pElems, 0, 0, nil);
          FilterInfo.pGraph := nil;
        end;
        if Assigned(CAGUID.pElems) then
          CoTaskMemFree(CAGUID.pElems);
        SpecifyPropertyPages := nil;
      end;
    ppVFWCapDisplay .. ppVFWCapSource:
      begin
        result := Filter.QueryInterface(IID_IAMVfwCaptureDialogs,
          CaptureDialog);
        if (result <> S_OK) then
          Exit;
        result := CaptureDialog.HasDialog(code);
        if result <> S_OK then
          Exit;
        result := CaptureDialog.ShowDialog(code, parent);
        CaptureDialog := nil;
      end;
    ppVFWCompConfig .. ppVFWCompAbout:
      begin
        result := Filter.QueryInterface(IID_IAMVfwCompressDialogs,
          CompressDialog);
        if (result <> S_OK) then
          Exit;
        case PropertyPage of
          ppVFWCompConfig:
            result := CompressDialog.ShowDialog
              (VfwCompressDialog_QueryConfig, 0);
          ppVFWCompAbout:
            result := CompressDialog.ShowDialog
              (VfwCompressDialog_QueryAbout, 0);
        end;
        if result = S_OK then
          result := CompressDialog.ShowDialog(code, parent);
        CompressDialog := nil;
      end;
  end;
end;

function HaveFilterPropertyPage(Filter: IBaseFilter;
  PropertyPage: TVFPropertyPage = ppDefault): Boolean;
var
  SpecifyPropertyPages: ISpecifyPropertyPages;
  CaptureDialog: IAMVfwCaptureDialogs;
  CompressDialog: IAMVfwCompressDialogs;
  code: Integer;
  hr: HRESULT;
begin
  result := false;
  code := 0;
  if Filter = nil then
    Exit;

  case PropertyPage of
    ppVFWCapDisplay:
      code := VfwCaptureDialog_Display;
    ppVFWCapFormat:
      code := VfwCaptureDialog_Format;
    ppVFWCapSource:
      code := VfwCaptureDialog_Source;
    ppVFWCompConfig:
      code := VfwCompressDialog_QueryConfig;
    ppVFWCompAbout:
      code := VfwCompressDialog_QueryAbout;
  end;

  case PropertyPage of
    ppDefault:
      begin
        result := Succeeded(Filter.QueryInterface(IID_ISpecifyPropertyPages,
          SpecifyPropertyPages));
        SpecifyPropertyPages := nil;
      end;
    ppVFWCapDisplay .. ppVFWCapSource:
      begin
        hr := Filter.QueryInterface(IID_IAMVfwCaptureDialogs, CaptureDialog);
        if (hr <> S_OK) then
          Exit;
        result := Succeeded(CaptureDialog.HasDialog(code));
        CaptureDialog := nil;
      end;
    ppVFWCompConfig .. ppVFWCompAbout:
      begin
        hr := Filter.QueryInterface(IID_IAMVfwCompressDialogs, CompressDialog);
        if (hr <> S_OK) then
          Exit;
        result := Succeeded(CompressDialog.ShowDialog(code, 0));
        CompressDialog := nil;
      end;
  end;
end;

procedure FreeAndNil(var Obj);
var
  Temp: TObject;
begin
  Temp := TObject(Obj);
  pointer(Obj) := nil;
  Temp.Free;
end;

constructor TVFSysDevEnum.Create;
begin
  FReadMerit := false;
  FCategories := TList.Create;
  FFilters := TList.Create;
  GetCat(FCategories, CLSID_ActiveMovieCategories);
end;

constructor TVFSysDevEnum.Create(guid: TGUID);
begin
  FReadMerit := false;
  FCategories := TList.Create;
  FFilters := TList.Create;
  GetCat(FCategories, CLSID_ActiveMovieCategories);
  SelectGUIDCategory(guid);
end;

destructor TVFSysDevEnum.Destroy;
var
  i: Integer;
begin
  inherited Destroy;
  if FCategories.Count > 0 then
    for i := 0 to (FCategories.Count - 1) do
      if Assigned(FCategories.Items[i]) then
        dispose(FCategories.Items[i]);
  FCategories.Clear;
  FreeAndNil(FCategories);
  if FFilters.Count > 0 then
    for i := 0 to (FFilters.Count - 1) do
      if Assigned(FFilters.Items[i]) then
        dispose(FFilters.Items[i]);
  FFilters.Clear;
  FreeAndNil(FFilters);
end;

// Find filter index by FriendlyName; -1, if not found

function TVFSysDevEnum.FilterIndexOfFriendlyName(const FriendlyName
  : WideString): Integer;
begin
  result := FFilters.Count - 1;
  while (result >= 0) and
    (AnsiCompareText(PFilCatNode(FFilters.Items[result])^.FriendlyName,
    FriendlyName) <> 0) do
    dec(result);
end;

function TVFSysDevEnum.GetBaseFilter(Index: Integer): IBaseFilter;
var
  SysDevEnum: ICreateDevEnum;
  EnumCat: IEnumMoniker;
  Moniker: IMoniker;
begin
  result := nil;
  if ((index < CountFilters) and (index >= 0)) then
  begin
    CoCreateInstance(CLSID_SystemDeviceEnum, nil, CLSCTX_INPROC,
      IID_ICreateDevEnum, SysDevEnum);
    SysDevEnum.CreateClassEnumerator(FGUID, EnumCat, 0);
    EnumCat.Skip(index);
    EnumCat.Next(1, Moniker, nil);
    Moniker.BindToObject(nil, nil, IID_IBaseFilter, result);
    EnumCat.Reset;
    SysDevEnum := nil;
    EnumCat := nil;
    Moniker := nil;
  end
end;

function TVFSysDevEnum.GetBaseFilter(guid: TGUID): IBaseFilter;
var
  i: Integer;
begin
  result := nil;
  if CountFilters > 0 then
    for i := 0 to CountFilters - 1 do
      if IsEqualGUID(guid, Filters[i].CLSID) then
      begin
        result := GetBaseFilter(i);
        Exit;
      end;
end;

// *****************************************************************************
// TSysDevEnum
// *****************************************************************************

procedure TVFSysDevEnum.GetCat(catlist: TList; CatGUID: TGUID);
var
  SysDevEnum: ICreateDevEnum;
  EnumCat: IEnumMoniker;
  names: TStringList;
  Moniker: IMoniker;
  Fetched: ULONG;
  PropBag: IPropertyBag;
  name: OleVariant;
  S: WideString;
  hr: HRESULT;
  i, k: Integer;
begin
  try
    if catlist.Count > 0 then
      for i := 0 to (catlist.Count - 1) do
        if Assigned(catlist.Items[i]) then
          dispose(catlist.Items[i]);

    catlist.Clear;
    names := TStringList.Create;

    CoCreateInstance(CLSID_SystemDeviceEnum, nil, CLSCTX_INPROC,
      IID_ICreateDevEnum, SysDevEnum);
    hr := SysDevEnum.CreateClassEnumerator(CatGUID, EnumCat, 0);
    if (hr = S_OK) then
    begin
      while (EnumCat.Next(1, Moniker, @Fetched) = S_OK) do
      begin
        try
          Name := '';
          Moniker.BindToStorage(nil, nil, IID_IPropertyBag, PropBag);

          PropBag.Read('FriendlyName', Name, nil);

          new(ACategory);

          if IsEqualGUID(CatGUID, CLSID_AudioRendererCategory) then
          begin
            if (Pos('DirectSound', Name) = 0) then
            begin
              Name := '<ignore>';
            end
            else
            begin
              Name := trim(AnsiReplaceStr(Name, 'DirectSound:', ''));
            end;
          end;

          // new(ACategory);

          if trim(Name) = '' then
          begin
            Name := '[no name]';
          end;

          ACategory^.FriendlyName := Name;

          Name := '';
          if (PropBag.Read('CLSID', Name, nil) = S_OK) then
          begin
            try
              ACategory^.CLSID := StringToGUID(Name);
            except
              ACategory^.CLSID := GUID_NULL;
            end;
          end
          else
            ACategory^.CLSID := GUID_NULL;

          Name := '';
          if (PropBag.Read('FccHandler', Name, nil) = S_OK) then
            ACategory^.FOURCC := Name
          else
            ACategory^.FOURCC := '';

          Name := '';
          PropBag.Read('Description', Name, nil);
          ACategory^.Description := Name;

          if Pos(WideString('Microsoft DV Camera and VCR'),
            ACategory^.FriendlyName) <> 0 then
          begin
            S := AnsiReplaceStr(ACategory^.FriendlyName,
              'Microsoft DV Camera and VCR', '(DV)');
            ACategory^.FriendlyName := ACategory^.Description + ' ' + S;
          end;

          if Pos(WideString('Microsoft AV/C Tape Subunit Device'),
            ACategory^.FriendlyName) <> 0 then
          begin
            S := AnsiReplaceStr(ACategory^.FriendlyName,
              'Microsoft AV/C Tape Subunit Device', '(DV/MPEG/Tape Device)');
            ACategory^.FriendlyName := ACategory^.Description + ' ' + S;
          end;

          if names.IndexOf(ACategory^.FriendlyName) <> -1 then
          begin
            for k := 2 to 10 do
              if names.IndexOf(ACategory^.FriendlyName + ' (' + IntToStr(k) +
                ')') = -1 then
              begin
                ACategory^.FriendlyName := ACategory^.FriendlyName + ' (' +
                  IntToStr(k) + ')';
                break;
              end;
          end;

          names.Add(ACategory^.FriendlyName);

          Name := '';
          if (PropBag.Read('DevicePath', Name, nil) = S_OK) then
            ACategory^.DevicePath := Name
          else
            ACategory^.DevicePath := '';

          if FReadMerit then
            ACategory^.Merit := GetFilterMerit('',
              GUIDToString(ACategory^.CLSID));

          catlist.Add(ACategory);
          PropBag := nil;
          Moniker := nil;
        except
          ;
        end;
      end;
    end;

    names.Free;
    EnumCat := nil;
    SysDevEnum := nil;
  except
    ;
  end;
end;

function TVFSysDevEnum.GetCategory(item: Integer): TFilCatNode;
var
  PCategory: PFilCatNode;
begin
  PCategory := FCategories.Items[item];
  result := PCategory^;
end;

function TVFSysDevEnum.GetCountCategories: Integer;
begin
  result := FCategories.Count;
end;

function TVFSysDevEnum.GetCountFilters: Integer;
begin
  result := FFilters.Count;
end;

function TVFSysDevEnum.GetFilter(item: Integer): TFilCatNode;
var
  PCategory: PFilCatNode;
begin
  PCategory := FFilters.Items[item];
  result := PCategory^;
end;

function TVFSysDevEnum.GetFilterMerit(name, guid: WideString): cardinal;
const
  bufsize = 4096;
var
  reg: TRegistry;
  i, size: Integer;
  S: AnsiString;
  res: cardinal;
  buff: array [0 .. bufsize - 1] of byte;
begin
  result := 0;

  try
    res := 0;

    if guid <> '' then
      S := dskey + guid + '\'
    else
      for i := 0 to CountFilters - 1 do
        if Filters[i].FriendlyName = name then
        begin
          S := dskey + GUIDToString(Filters[i].CLSID) + '\';
          break;
        end;

    if S = '' then
      Exit;

    reg := TRegistry.Create;
    reg.RootKey := HKEY_CLASSES_ROOT;

    if reg.OpenKey(S, false) then
    begin
      size := reg.GetDataSize(SFilterData);
      if (reg.GetDataType(SFilterData) = rdBinary) then // and (size = 128) then
      begin
        // reg.ReadBinaryData('FilterData', buff, bufsize);
        reg.ReadBinaryData(SFilterData, buff, size);
        try
          res := buff[7] + 256 * buff[6] + 256 * 256 * buff[5] + 256 * 256 * 256
            * buff[4];
        except
          res := 0;
        end;
      end;
      reg.CloseKey;
    end;

    result := res;

    reg.Free;
  except
    ;
  end;
end;

function TVFSysDevEnum.GetMoniker(Index: Integer): IMoniker;
var
  SysDevEnum: ICreateDevEnum;
  EnumCat: IEnumMoniker;
begin
  result := nil;
  if ((index < CountFilters) and (index >= 0)) then
  begin
    CoCreateInstance(CLSID_SystemDeviceEnum, nil, CLSCTX_INPROC,
      IID_ICreateDevEnum, SysDevEnum);
    SysDevEnum.CreateClassEnumerator(FGUID, EnumCat, 0);
    EnumCat.Skip(index);
    EnumCat.Next(1, result, nil);
    EnumCat.Reset;
    SysDevEnum := nil;
    EnumCat := nil;
  end
end;

procedure TVFSysDevEnum.SelectGUIDCategory(guid: TGUID);
begin
  FGUID := guid;
  GetCat(FFilters, FGUID);
end;

procedure TVFSysDevEnum.SelectIndexCategory(Index: Integer);
begin
  SelectGUIDCategory(Categories[index].CLSID);
end;

function TVFSysDevEnum.SetFilterMerit(name, guid: WideString;
  Merit: DWORD): Boolean;
const
  bufsize = 128;
var
  reg: TRegistry;
  i: Integer;
  S: AnsiString;
  b1, b2, b3, b4: byte;
  mer, mer2: DWORD;
  buff: array [0 .. 127] of byte;
begin
  result := false;
  reg := nil;

  try
    if guid <> '' then
      S := dskey + guid + '\'
    else
      for i := 0 to CountFilters - 1 do
        if Filters[i].FriendlyName = name then
        begin
          S := dskey + GUIDToString(Filters[i].CLSID) + '\';
          break;
        end;

    if S = '' then
      Exit;

    reg := TRegistry.Create;
    reg.RootKey := HKEY_CLASSES_ROOT;

    if reg.OpenKey(S, false) then
    begin
      reg.ReadBinaryData(SFilterData, buff, bufsize);

      mer2 := Merit;
      mer := mer2 div (256 * 256 * 256);
      b1 := mer;

      mer2 := Merit - mer * 256 * 256 * 256;
      mer := mer2 div (256 * 256);
      b2 := mer;

      mer2 := mer2 - mer * 256 * 256;
      mer := mer2 div 256;
      b3 := mer;

      b4 := mer2 - mer * 256;

      buff[7] := b1;
      buff[6] := b2;
      buff[5] := b3;
      buff[4] := b4;

      try
        reg.WriteBinaryData(SFilterData, buff, bufsize);
        result := true;
      except
        result := false;
      end;

      reg.CloseKey;
    end;

  except
    ;
  end;

  if Assigned(reg) then
  begin
    reg.Free;
  end;
end;

function GetFOURCC(FOURCC: cardinal): AnsiString;
type
  TFOURCC = array [0 .. 3] of AnsiChar;
var
  CC: TFOURCC;
begin
  case FOURCC of
    0:
      result := FCC_RGB;
    1:
      result := FCC_RLE8;
    2:
      result := FCC_RLE4;
    3:
      result := FCC_BITFIELDS;
  else
    PDWORD(@CC)^ := FOURCC;
    result := CC;
  end;
end;

function GetErrorCodeAsString(hr: HRESULT): WideString;
begin
  case hr of
    // DirectShow
    S_OK:
      result := ER_SOK;
    S_FALSE:
      result := ER_SFALSE;
    VFW_S_NO_MORE_ITEMS:
      result := ER_VFWSNOMOREITEMS;
    VFW_S_DUPLICATE_NAME:
      result := ER_VFWSDUPLICATENAME;
    VFW_S_STATE_INTERMEDIATE:
      result := ER_VFWSSTATEINTERMEDIATE;
    VFW_S_PARTIAL_RENDER:
      result := ER_VFWSPARTIALRENDER;
    VFW_S_SOME_DATA_IGNORED:
      result := ER_VFWSSOMEDATAIGNORED;
    VFW_S_CONNECTIONS_DEFERRED:
      result := ER_VFWSCONNECTIONSDEFERRED;
    VFW_S_RESOURCE_NOT_NEEDED:
      result := ER_VFWSRESOURCENOTNEEDED;
    VFW_S_MEDIA_TYPE_IGNORED:
      result := ER_VFWSMEDIATYPEIGNORED;
    VFW_S_VIDEO_NOT_RENDERED:
      result := ER_VFWSVIDEONOTRENDERED;
    VFW_S_AUDIO_NOT_RENDERED:
      result := ER_VFWSAUDIONOTRENDERED;
    VFW_S_RPZA:
      result := ER_VFWSRPZA;
    VFW_S_ESTIMATED:
      result := ER_VFWSESTIMATED;
    VFW_S_RESERVED:
      result := ER_VFWSRESERVED;
    VFW_S_STREAM_OFF:
      result := ER_VFWSSTREAMOFF;
    VFW_S_CANT_CUE:
      result := ER_VFWSCANTCUE;
    VFW_S_NOPREVIEWPIN:
      result := ER_VFWSNOPREVIEWPIN;
    VFW_S_DVD_NON_ONE_SEQUENTIAL:
      result := ER_VFWSDVDNONONESEQUENTIAL;
    VFW_S_DVD_CHANNEL_CONTENTS_NOT_AVAILABLE:
      result := ER_VFWSDVDCHANNELCONTENTSNOTAVAILABLE;
    VFW_S_DVD_NOT_ACCURATE:
      result := ER_VFWSDVDNOTACCURATE;
    VFW_E_INVALIDMEDIATYPE:
      result := ER_VFWEINVALIDMEDIATYPE;
    VFW_E_INVALIDSUBTYPE:
      result := ER_VFWEINVALIDSUBTYPE;
    VFW_E_NEED_OWNER:
      result := ER_VFWENEEDOWNER;
    VFW_E_ENUM_OUT_OF_SYNC:
      result := ER_VFWEENUMOUTOFSYNC;
    VFW_E_ALREADY_CONNECTED:
      result := ER_VFWEALREADYCONNECTED;
    VFW_E_FILTER_ACTIVE:
      result := ER_VFWEFILTERACTIVE;
    VFW_E_NO_TYPES:
      result := ER_VFWENOTYPES;
    VFW_E_NO_ACCEPTABLE_TYPES:
      result := ER_SVFWENOACCEPTABLETYPES;
    VFW_E_INVALID_DIRECTION:
      result := ER_VFWEINVALIDDIRECTION;
    VFW_E_NOT_CONNECTED:
      result := ER_VFWENOTCONNECTED;
    VFW_E_NO_ALLOCATOR:
      result := ER_VFWENOALLOCATOR;

    VFW_E_RUNTIME_ERROR:
      result := ER_VFWERUNTIMEERROR;
    VFW_E_BUFFER_NOTSET:
      result := ER_VFWEBUFFERNOTSET;
    VFW_E_BUFFER_OVERFLOW:
      result := ER_VFWEBUFFEROVERFLOW;
    VFW_E_BADALIGN:
      result := ER_VFWEBADALIGN;
    VFW_E_ALREADY_COMMITTED:
      result := ER_VFWEALREADYCOMMITTED;
    VFW_E_BUFFERS_OUTSTANDING:
      result := ER_VFWEBUFFERSOUTSTANDING;
    VFW_E_NOT_COMMITTED:
      result := ER_VFWENOTCOMMITTED;
    VFW_E_SIZENOTSET:
      result := ER_VFWESIZENOTSET;
    VFW_E_NO_CLOCK:
      result := ER_VFWENOCLOCK;
    VFW_E_NO_SINK:
      result := ER_VFWENOSINK;
    VFW_E_NO_INTERFACE:
      result := ER_VFWENOINTERFACE;
    VFW_E_NOT_FOUND:
      result := ER_VFWENOTFOUND;
    VFW_E_CANNOT_CONNECT:
      result := ER_VFWECANNOTCONNECT;
    VFW_E_CANNOT_RENDER:
      result := ER_VFWECANNOTRENDER;
    VFW_E_CHANGING_FORMAT:
      result := ER_VFWECHANGINGFORMAT;
    VFW_E_NO_COLOR_KEY_SET:
      result := ER_VFWENOCOLORKEYSET;
    VFW_E_NOT_OVERLAY_CONNECTION:
      result := ER_VFWENOTOVERLAYCONNECTION;
    VFW_E_NOT_SAMPLE_CONNECTION:
      result := ER_VFWENOTSAMPLECONNECTION;
    VFW_E_PALETTE_SET:
      result := ER_VFWEPALETTESET;
    VFW_E_COLOR_KEY_SET:
      result := ER_VFWECOLORKEYSET;
    VFW_E_NO_COLOR_KEY_FOUND:
      result := ER_VFWENOCOLORKEYFOUND;
    VFW_E_NO_PALETTE_AVAILABLE:
      result := ER_VFWENOPALETTEAVAILABLE;
    VFW_E_NO_DISPLAY_PALETTE:
      result := ER_VFWENODISPLAYPALETTE;
    VFW_E_TOO_MANY_COLORS:
      result := ER_VFWETOOMANYCOLORS;
    VFW_E_STATE_CHANGED:
      result := ER_VFWESTATECHANGED;
    VFW_E_NOT_STOPPED:
      result := ER_VFWENOTSTOPPED;
    VFW_E_NOT_PAUSED:
      result := ER_VFWENOTPAUSED;
    VFW_E_NOT_RUNNING:
      result := ER_VFWENOTRUNNING;

    VFW_E_WRONG_STATE:
      result := ER_VFWEWRONGSTATE;
    VFW_E_START_TIME_AFTER_END:
      result := ER_VFWESTARTTIMEAFTEREND;
    VFW_E_INVALID_RECT:
      result := ER_VFWEINVALIDRECT;
    VFW_E_TYPE_NOT_ACCEPTED:
      result := ER_VFWETYPENOTACCEPTED;
    VFW_E_SAMPLE_REJECTED:
      result := ER_VFWESAMPLEREJECTED;
    VFW_E_SAMPLE_REJECTED_EOS:
      result := ER_VFWESAMPLEREJECTEDEOS;
    VFW_E_DUPLICATE_NAME:
      result := ER_VFWEDUPLICATENAME;
    VFW_E_TIMEOUT:
      result := ER_VFWETIMEOUT;
    VFW_E_INVALID_FILE_FORMAT:
      result := ER_VFWEINVALIDFILEFORMAT;
    VFW_E_ENUM_OUT_OF_RANGE:
      result := ER_VFWEENUMOUTOFRANGE;
    VFW_E_CIRCULAR_GRAPH:
      result := ER_VFWECIRCULARGRAPH;
    VFW_E_NOT_ALLOWED_TO_SAVE:
      result := ER_VFWENOTALLOWEDTOSAVE;
    VFW_E_TIME_ALREADY_PASSED:
      result := ER_VFWETIMEALREADYPASSED;
    VFW_E_ALREADY_CANCELLED:
      result := ER_VFWEALREADYCANCELLED;
    VFW_E_CORRUPT_GRAPH_FILE:
      result := ER_VFWECORRUPTGRAPHFILE;
    VFW_E_ADVISE_ALREADY_SET:
      result := ER_VFWEADVISEALREADYSET;
    VFW_E_NO_MODEX_AVAILABLE:
      result := ER_VFWENOMODEXAVAILABLE;
    VFW_E_NO_ADVISE_SET:
      result := ER_VFWENOADVISESET;
    VFW_E_NO_FULLSCREEN:
      result := ER_VFWENOFULLSCREEN;
    // VFW_E_IN_FULLSCREEN_MODE:         result:= 'VFW_E_IN_FULLSCREEN_MODE';
    VFW_E_UNKNOWN_FILE_TYPE:
      result := ER_VFWEUNKNOWNFILETYPE;
    VFW_E_CANNOT_LOAD_SOURCE_FILTER:
      result := ER_VFWECANNOTLOADSOURCEFILTER;
    VFW_E_FILE_TOO_SHORT:
      result := ER_VFWEFILETOOSHORT;
    VFW_E_INVALID_FILE_VERSION:
      result := ER_VFWEINVALIDFILEVERSION;
    VFW_E_INVALID_CLSID:
      result := ER_VFWEINVALIDCLSID;
    VFW_E_INVALID_MEDIA_TYPE:
      result := ER_VFWEINVALIDMEDIATYPE;
    VFW_E_SAMPLE_TIME_NOT_SET:
      result := ER_VFWESAMPLETIMENOTSET;

    VFW_E_MEDIA_TIME_NOT_SET:
      result := ER_VFWEMEDIATIMENOTSET;
    VFW_E_NO_TIME_FORMAT_SET:
      result := ER_VFWENOTIMEFORMATSET;
    VFW_E_MONO_AUDIO_HW:
      result := ER_VFWEMONOAUDIOHW;
    // VFW_E_NO_DECOMPRESSOR:            result:= 'VFW_E_NO_DECOMPRESSOR';
    VFW_E_NO_AUDIO_HARDWARE:
      result := ER_VFWENOAUDIOHARDWARE;
    VFW_E_RPZA:
      result := ER_VFWERPZA;
    VFW_E_PROCESSOR_NOT_SUITABLE:
      result := ER_VFWEPROCESSORNOTSUITABLE;
    VFW_E_UNSUPPORTED_AUDIO:
      result := ER_VFWEUNSUPPORTEDAUDIO;
    VFW_E_UNSUPPORTED_VIDEO:
      result := ER_VFWEUNSUPPORTEDVIDEO;
    VFW_E_MPEG_NOT_CONSTRAINED:
      result := ER_VFWEMPEGNOTCONSTRAINED;
    VFW_E_NOT_IN_GRAPH:
      result := ER_VFWENOTINGRAPH;
    VFW_E_NO_TIME_FORMAT:
      result := ER_VFWENOTIMEFORMAT;
    VFW_E_READ_ONLY:
      result := ER_VFWEREADONLY;
    VFW_E_BUFFER_UNDERFLOW:
      result := ER_VFWEBUFFERUNDERFLOW;
    VFW_E_UNSUPPORTED_STREAM:
      result := ER_VFWEUNSUPPORTEDSTREAM;
    VFW_E_NO_TRANSPORT:
      result := ER_VFWENOTRANSPORT;
    VFW_E_BAD_VIDEOCD:
      result := ER_VFWEBADVIDEOCD;
    VFW_S_NO_STOP_TIME:
      result := ER_VFWSNOSTOPTIME;
    VFW_E_OUT_OF_VIDEO_MEMORY:
      result := ER_VFWEOUTOFVIDEOMEMORY;
    VFW_E_VP_NEGOTIATION_FAILED:
      result := ER_VFWEVPNEGOTIATIONFAILED;
    VFW_E_DDRAW_CAPS_NOT_SUITABLE:
      result := ER_VFWEDDRAWCAPSNOTSUITABLE;
    VFW_E_NO_VP_HARDWARE:
      result := ER_VFWENOVPHARDWARE;
    VFW_E_NO_CAPTURE_HARDWARE:
      result := ER_VFWENOCAPTUREHARDWARE;
    VFW_E_DVD_OPERATION_INHIBITED:
      result := ER_VFWEDVDOPERATIONINHIBITED;
    VFW_E_DVD_INVALIDDOMAIN:
      result := ER_VFWEDVDINVALIDDOMAIN;
    VFW_E_DVD_NO_BUTTON:
      result := ER_VFWEDVDNOBUTTON;
    VFW_E_DVD_GRAPHNOTREADY:
      result := ER_VFWEDVDGRAPHNOTREADY;
    VFW_E_DVD_RENDERFAIL:
      result := ER_VFWEDVDRENDERFAIL;
    VFW_E_DVD_DECNOTENOUGH:
      result := ER_VFWEDVDDECNOTENOUGH;

    VFW_E_DDRAW_VERSION_NOT_SUITABLE:
      result := ER_VFWEDDRAWVERSIONNOTSUITABLE;
    VFW_E_COPYPROT_FAILED:
      result := ER_VFWECOPYPROTFAILED;
    VFW_E_TIME_EXPIRED:
      result := ER_VFWETIMEEXPIRED;
    VFW_E_DVD_WRONG_SPEED:
      result := ER_VFWEDVDWRONGSPEED;
    VFW_E_DVD_MENU_DOES_NOT_EXIST:
      result := ER_VFWEDVDMENUDOESNOTEXIST;
    VFW_E_DVD_CMD_CANCELLED:
      result := ER_VFWEDVDCMDCANCELLED;
    VFW_E_DVD_STATE_WRONG_VERSION:
      result := ER_VFWEDVDSTATEWRONGVERSION;
    VFW_E_DVD_STATE_CORRUPT:
      result := ER_VFWEDVDSTATECORRUPT;
    VFW_E_DVD_STATE_WRONG_DISC:
      result := ER_VFWEDVDSTATEWRONGDISC;
    VFW_E_DVD_INCOMPATIBLE_REGION:
      result := ER_VFWEDVDINCOMPATIBLEREGION;
    VFW_E_DVD_NO_ATTRIBUTES:
      result := ER_VFWEDVDNOATTRIBUTES;
    VFW_E_DVD_NO_GOUP_PGC:
      result := ER_VFWEDVDNOGOUPPGC;
    VFW_E_DVD_LOW_PARENTAL_LEVEL:
      result := ER_VFWEDVDLOWPARENTALLEVEL;
    VFW_E_DVD_NOT_IN_KARAOKE_MODE:
      result := ER_VFWEDVDNOTINKARAOKEMODE;
    VFW_E_FRAME_STEP_UNSUPPORTED:
      result := ER_VFWEFRAMESTEPUNSUPPORTED;
    VFW_E_DVD_STREAM_DISABLED:
      result := ER_VFWEDVDSTREAMDISABLED;
    VFW_E_DVD_TITLE_UNKNOWN:
      result := ER_VFWEDVDTITLEUNKNOWN;
    VFW_E_DVD_INVALID_DISC:
      result := ER_VFWEDVDINVALIDDISC;
    VFW_E_DVD_NO_RESUME_INFORMATION:
      result := ER_VFWEDVDNORESUMEINFORMATION;
    VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD:
      result := ER_VFWEPINALREADYBLOCKEDONTHISTHREAD;
    VFW_E_PIN_ALREADY_BLOCKED:
      result := ER_VFWEPINALREADYBLOCKED;
    VFW_E_CERTIFICATION_FAILURE:
      result := ER_VFWECERTIFICATIONFAILURE;
    VFW_E_VMR_NOT_IN_MIXER_MODE:
      result := ER_VFWEVMRNOTINMIXERMODE;
    VFW_E_VMR_NO_AP_SUPPLIED:
      result := ER_VFWEVMRNOAPSUPPLIED;
    VFW_E_VMR_NO_DEINTERLACE_HW:
      result := ER_VFWEVMRNODEINTERLACEHW;
    VFW_E_VMR_NO_PROCAMP_HW:
      result := ER_VFWEVMRNOPROCAMPHW;
    VFW_E_DVD_VMR9_INCOMPATIBLEDEC:
      result := ER_VFWEDVDVMR9INCOMPATIBLEDEC;
    VFW_E_BAD_KEY:
      result := ER_VFWEBADKEY;

    // DES
    S_WARN_OUTPUTRESET:
      result := ER_SWARNOUTPUTRESET;
    E_NOTINTREE:
      result := ER_ENOTINTREE;
    E_RENDER_ENGINE_IS_BROKEN:
      result := ER_ERENDERENGINEISBROKEN;
    E_MUST_INIT_RENDERER:
      result := ER_EMUSTINITRENDERER;
    E_NOTDETERMINED:
      result := ER_ENOTDETERMINED;
    E_NO_TIMELINE:
      result := ER_ENOTIMELINE;
  else
    result := SUnknown;
  end;
end;

function GetPin(pFilter: IBaseFilter; pDir: PIN_DIRECTION; pIndex: byte;
  var pPin: IPin): Boolean;
var
  pl: TVFPinList;
  i, k: Integer;
begin
  result := false;

  try
    pl := TVFPinList.Create;
    pl.Assign(pFilter);

    k := 0;
    for i := 0 to pl.Count - 1 do
      if pl.PinInfo[i].dir = pDir then
      begin
        k := k + 1;
        if k = pIndex then
        begin
          pPin := pl.Items[i];
          pl.Free;
          result := true;
          Exit;
        end;
      end;

    pl.Free;
  except
    ;
  end;
end;

function PinHaveThisType(Pin: IPin; MediaType: TGUID): Boolean;
var
  mts: IEnumMediaTypes;
  MT: PAMMEDIATYPE;
begin
  try
    result := false;
    Pin.EnumMediaTypes(mts);
    mts.Reset;
    while mts.Next(1, MT, nil) = S_OK do
    begin
      if MT <> nil then
      begin
        if (IsEqualGUID(MT.majortype, MediaType)) then
        begin
          result := true;
          Exit;
        end;
      end;
    end;
    if Assigned(mts) then
      mts := nil;
  except
    result := false;
  end;
end;

function GetFreePinWithMediaType(pFilter: IBaseFilter; pDir: PIN_DIRECTION;
  guid: TGUID; var pPin: IPin): Boolean;
var
  pl: TVFPinList;
  i: Integer;
  pPin2: IPin;
  MT: _AMMediaType;
begin
  result := false;

  try
    pl := TVFPinList.Create;
    pl.Assign(pFilter);

    for i := 0 to pl.Count - 1 do
      if pl.PinInfo[i].dir = pDir then
      begin
        pl.Items[i].ConnectedTo(pPin2);

        if pPin2 <> nil then
        begin
          pPin2 := nil;
          continue;
        end;

        if PinHaveThisType(pl.Items[i], guid) then
        begin
          FreeMediaType(@MT);
          pPin := pl.Items[i];
          pl.Free;
          result := true;
          Exit;
        end;

        FreeMediaType(@MT);
      end;

    pl.Free;
  except
    ;
  end;
end;

procedure FreeMediaType(MT: PAMMEDIATYPE);
begin
  if (MT^.cbFormat <> 0) then
  begin
    CoTaskMemFree(MT^.pbFormat);
    // Strictly unnecessary but tidier
    MT^.cbFormat := 0;
    MT^.pbFormat := nil;
  end;
  if (MT^.pUnk <> nil) then
    MT^.pUnk := nil;
end;

procedure DeleteMediaType(pmt: PAMMEDIATYPE);
begin
  try
    // allow nil pointers for coding simplicity
    if (pmt = nil) then
      Exit;
    FreeMediaType(pmt);
    CoTaskMemFree(pmt);
  except
    ;
  end;
end;

function ReadMediaInfoDeep(FileName: WideString;
  out width, height: Integer): Boolean;
var
  pGraphBuilder: IFilterGraph2;
  pBasicVideo: IBasicVideo;
  hr: Integer;
begin
  try
    result := false;

    try
      CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC, IID_IGraphBuilder,
        pGraphBuilder);

      hr := pGraphBuilder.RenderFile(StringToOleStr(FileName), nil);

      pGraphBuilder.QueryInterface(IID_IBasicVideo, pBasicVideo);

      if Assigned(pBasicVideo) then
      begin
        pBasicVideo.get_VideoWidth(width);
        pBasicVideo.get_VideoHeight(height);

        result := true;

        pBasicVideo := nil;
      end;
    finally
      pGraphBuilder := nil;
    end;
  except

  end;
end;

function ImageCompare(Img1, Img2: TBitmap; Difference: byte;
  var FinalDiff: Integer; ReadDiff: Boolean; var imgDiff: TBitmap;
  var ImgDiffTop, ImgDiffLeft: Integer; var DiffPerc: Double): Boolean;
var
  i, j: Integer;
  RowOriginal, RowProcessed: pRGBArray;
  RowOriginal32, RowProcessed32: pRGBAArray;

  Fullsize, errors: Int64;

  MLeft, MTop, MRight, MBottom: Integer;
begin
  result := true;

  MLeft := 0;
  MTop := 0;
  MRight := 0;
  MBottom := 0;
  errors := 0;

  if Img1.width <> Img2.width then
    Exit;
  if Img1.height <> Img2.height then
    Exit;
  if (Img1.PixelFormat <> pf24bit) and (Img1.PixelFormat <> pf32bit) and
    (Img1.PixelFormat <> pf16bit) and (Img1.PixelFormat <> pf8bit) then
    Exit;

  Fullsize := Img1.width * Img1.height;

  if Fullsize = 0 then
    Exit;

  try
    DiffPerc := 0;

    errors := 0;

    if ReadDiff then
    begin
      MLeft := Img1.width;
      MRight := 0;
      MTop := Img1.height;
      MBottom := 0;
    end;

    if Img1.PixelFormat = pf24bit then
    // 24bit picture
    begin
      for j := Img1.height - 1 downto 0 do
      begin
        RowOriginal := pRGBArray(Img1.Scanline[j]);
        RowProcessed := pRGBArray(Img2.Scanline[j]);

        if (not ReadDiff) and ((100 - ((errors / Fullsize) * 100)) < Difference)
        then
        begin
          result := false;
          Exit;
        end;

        for i := Img1.width - 1 downto 0 do
        begin
          if (RowProcessed[i].rgbtRed <> RowOriginal[i].rgbtRed) or
            (RowProcessed[i].rgbtGreen <> RowOriginal[i].rgbtGreen) or
            (RowProcessed[i].rgbtBlue <> RowOriginal[i].rgbtBlue) then
          begin
            errors := errors + 1;
            if ReadDiff then
            begin
              if i < MLeft then
                MLeft := i;
              if i > MRight then
                MRight := i;
              if j < MTop then
                MTop := j;
              if j > MBottom then
                MBottom := j;
            end;
          end;
        end
      end;
    end
    else if Img1.PixelFormat = pf32bit then
    // 32bit picture
    begin
      for j := Img1.height - 1 downto 0 do
      begin
        RowOriginal32 := pRGBAArray(Img1.Scanline[j]);
        RowProcessed32 := pRGBAArray(Img2.Scanline[j]);

        if (100 - ((errors / Fullsize) * 100)) < Difference then
        begin
          result := false;
          Exit;
        end;

        for i := Img1.width - 1 downto 0 do
        begin
          if (RowProcessed32[i].rgbRed <> RowOriginal32[i].rgbRed) or
            (RowProcessed32[i].rgbGreen <> RowOriginal32[i].rgbGreen) or
            (RowProcessed32[i].rgbBlue <> RowOriginal32[i].rgbBlue) then
            errors := errors + 1;
        end
      end;
    end;

  finally
    FinalDiff := round(100 - ((errors / Fullsize) * 100));

    if (ReadDiff) and (FinalDiff < Difference) then
      result := false;

    if (ReadDiff) and (not result) then
      try
        try
          imgDiff.PixelFormat := Img2.PixelFormat;
          imgDiff.width := abs(MRight - MLeft);
          imgDiff.height := abs(MTop - MBottom);

          ImgDiffTop := MTop;
          ImgDiffLeft := MLeft;

          DiffPerc := ((imgDiff.width * imgDiff.height) / Fullsize) * 100;

          imgDiff.Canvas.CopyRect(rect(0, 0, abs(MRight - MLeft),
            abs(MBottom - MTop)), Img2.Canvas, rect(MLeft, MTop, MRight,
            MBottom));
        except
          ;
        end;
      except
        ;
      end;

  end;
end;

function VMR9DeinterlaceModeToStr(ds: VMR9DeinterlacePrefs): WideString;
var
  S: WideString;
begin
  S := '';

  if ds = DeinterlaceTech9_Unknown then
  begin
    S := S + Deint_UnknownProprietary;
    ds := ds - DeinterlaceTech9_Unknown;
  end;

  if ds div DeinterlaceTech9_MotionVectorSteered = 1 then
  begin
    S := S + Deint_MotionVectorSteered;
    ds := ds - DeinterlaceTech9_MotionVectorSteered;
  end;

  if ds div DeinterlaceTech9_PixelAdaptive = 1 then
  begin
    S := S + Deint_PixelAdaptive;
    ds := ds - DeinterlaceTech9_PixelAdaptive;
  end;

  if ds div DeinterlaceTech9_FieldAdaptive = 1 then
  begin
    S := S + Deint_FieldAdaptive;
    ds := ds - DeinterlaceTech9_FieldAdaptive;
  end;

  if ds div DeinterlaceTech9_EdgeFiltering = 1 then
  begin
    S := S + Deint_EdgeFiltering;
    ds := ds - DeinterlaceTech9_EdgeFiltering;
  end;

  if ds div DeinterlaceTech9_MedianFiltering = 1 then
  begin
    S := S + Deint_MedianFiltering;
    ds := ds - DeinterlaceTech9_MedianFiltering;
  end;

  if ds div DeinterlaceTech9_BOBVerticalStretch = 1 then
  begin
    S := S + Deint_BOBVerticalStretch;
    ds := ds - DeinterlaceTech9_BOBVerticalStretch;
  end;

  if ds div DeinterlaceTech9_BOBLineReplicate = 1 then
  begin
    S := S + Deint_BOBLineReplicate;
    // ds := ds - DeinterlaceTech9_BOBLineReplicate;
  end;

  S := trim(S);
  if S[length(S)] = ',' then
    S[length(S)] := ' ';

  result := trim(S);
end;

function IntToHBITMAP(Value: Integer): HBITMAP;
var
  res: HBITMAP;
begin
  CopyMemory(@res, @Value, 4);
  result := res;
end;

function HBITMAPToInt(Value: HBITMAP): Integer;
var
  res: Integer;
begin
  CopyMemory(@res, @Value, 4);
  result := res;
end;

procedure AddImageOverlay(src: pointer; src_width, src_height: Integer;
  dest: pointer; dest_width, dest_height: Integer);
var
  i: Integer;
  j, stride1, stride2: cardinal;
  tmp_dest, tmp_src: pointer;
begin
  if (src <> nil) and (dest <> nil) then
  begin
    stride1 := dest_width * 3;
    stride2 := src_width * 3;

    for i := 0 to src_height - 1 do
    begin
      j := i;
      tmp_dest := pointer(cardinal(dest) + stride1 * j);
      tmp_src := pointer(cardinal(src) + stride2 * j);
      CopyMemory(tmp_dest, tmp_src, stride2);
    end;
  end;
end;

function GetZoomPos(SrcWidth, SrcHeight, ShiftX, ShiftY, zoom: Integer): TRect;
var
  Ratio: real;
  TmpX, TmpY: real;
  TmpLeft, TmpTop: real;
begin
  result.Left := 0;
  result.Top := 0;
  result.Right := SrcWidth;
  result.Bottom := SrcHeight;

  try
    Ratio := SrcHeight / SrcWidth;

    TmpX := SrcWidth - ((zoom * SrcWidth) / 100);
    TmpY := TmpX * Ratio;

    // TmpX := srcWidth * (zoom / 100);//  srcWidth - ((zoom * srcWidth) / 100);
    // TmpY := TmpX * Ratio;

    TmpLeft := (SrcWidth - TmpX) / 2;
    TmpTop := (SrcHeight - TmpY) / 2;

    result.Left := Trunc(TmpLeft) + ShiftX;
    result.Top := Trunc(TmpTop) + ShiftY;
    result.Right := Trunc(TmpX) + Trunc(TmpLeft) + ShiftX;
    result.Bottom := Trunc(TmpY) + Trunc(TmpTop) + ShiftY;
  except
    result.Left := 0;
    result.Top := 0;
    result.Right := SrcWidth;
    result.Bottom := SrcHeight;
  end;
end;

function ConvertBitmapToBuffer(bitmap: TBitmap; out buffer: PByte): Boolean;
var
  i: Integer;
begin
  result := false;

  try

    if bitmap = nil then
      Exit;

    for i := 0 to bitmap.height - 1 do
    begin
      CopyMemory(PAnsiChar(buffer) + i * bitmap.width * 3,
        bitmap.Scanline[bitmap.height - i - 1], bitmap.width * 3);
    end;

    result := true;
  except
    result := false;
  end;
end;

function GetBitmapFromBuffer(bitmap: TBitmap; buffer: PByte;
  bufferSize: Integer; width, height: Integer; screenshot: Boolean;
  logo: pointer; logo_width, logo_height: Integer): Boolean;

  function GetDIBLineSize(BitCount, width: Integer): Integer;
  begin
    if BitCount = 15 then
      BitCount := 16;
    result := ((BitCount * width + 31) div 32) * 4;
  end;

var
  BufferLen: Integer;
  BitmapHandle: HBITMAP;
  DIBPtr: pointer;
  DIBSize: longint;
  BIHeader: BITMAPINFOHEADER;
  // BIHeaderPtr: BitmapInfoHeader;
begin
  result := false;

  try
    if not Assigned(bitmap) then
      Exit;

    if not Assigned(buffer) then
      Exit;

    // buffer := nil;
    BufferLen := bufferSize;

    try
      if (logo <> nil) and (buffer <> nil) and (width > logo_width) and
        (height > logo_height) then
        AddImageOverlay(logo, logo_width, logo_height, buffer, width, height);

      ZeroMemory(@BIHeader, SizeOf(BITMAPINFOHEADER));
      BIHeader.biBitCount := 24;
      BIHeader.biCompression := BI_RGB;
      BIHeader.biWidth := width;
      BIHeader.biHeight := height;
      BIHeader.biPlanes := 1;
      BIHeader.biSize := SizeOf(BITMAPINFOHEADER);
      BIHeader.biSizeImage := width * height * 3;

      // BIHeaderPtr := @BIHeader;
      // BufferLen := BIHeaderPtr^.biWidth * BIHeaderPtr^.biHeight * 3;

      BitmapHandle := CreateDIBSection(0, PBitmapInfo(@BIHeader)^,
        DIB_RGB_COLORS, DIBPtr, 0, 0);

      if BitmapHandle <> 0 then
      begin
        try
          if DIBPtr = nil then
            Exit;
          // get DIB size
          DIBSize := BIHeader.biSizeImage;
          if DIBSize = 0 then
          begin
            with BIHeader do
              DIBSize := GetDIBLineSize(biBitCount, biWidth) * biHeight
                * biPlanes;
          end;

          if BufferLen > DIBSize then // copy Min(BufferLen, DIBSize)
            BufferLen := DIBSize;
          move(buffer^, DIBPtr^, BufferLen);

          bitmap.Handle := BitmapHandle;
        finally
          if bitmap.Handle <> BitmapHandle then
            // preserve for any changes in Graphics.pas
            DeleteObject(BitmapHandle);
        end;
      end;
    finally
      result := true
    end;
  except
    result := false;
  end;
end;

// function SampleGrabber_GetBitmap(bitmap: TBitmap; pSample: IMediaSample; SampleGrabber: ISampleGrabber;
// screenshot: Boolean; logo: pointer; logo_width, logo_height: Integer): Boolean;
//
// function GetDIBLineSize(BitCount, width: Integer): Integer;
// begin
// if BitCount = 15 then
// BitCount := 16;
// result := ((BitCount * width + 31) div 32) * 4;
// end;
//
// procedure AddLogo(src: pointer; src_width, src_height: Integer; dest: pointer; dest_width, dest_height: Integer);
// var
// i, stride1, stride2: Integer;
// tmp_dest, tmp_src: pointer;
// begin
// if logo <> nil then
// begin
// stride1 := dest_width * 3;
// stride2 := src_width * 3;
//
// for i := 0 to src_height - 1 do
// begin
// tmp_dest := pointer(cardinal(dest) + stride1 * i);
// tmp_src := pointer(cardinal(src) + stride2 * i);
// CopyMemory(tmp_dest, tmp_src, stride2);
// end;
// end;
// end;
//
// var
// hr: HRESULT;
// BIHeaderPtr: PBitmapInfoHeader;
// MediaType: TAMMediaType;
// BitmapHandle: HBITMAP;
// BufferLen, i: Integer;
// DIBPtr, dest, src: pointer;
// DIBSize: longint;
// buffer: PByte;
// stride1, stride2: Integer;
// begin
// result := false;
//
// try
// if not Assigned(bitmap) then
// Exit;
//
// buffer := nil;
// BufferLen := 0;
//
// hr := SampleGrabber.GetConnectedMediaType(MediaType);
// if hr <> S_OK then
// Exit;
//
// if not screenshot then
// begin
// hr := pSample.GetPointer(buffer);
//
// if hr <> S_OK then
// Exit;
// end;
//
// try
// if IsEqualGUID(MediaType.majortype, MEDIATYPE_Video) then
// begin
// BIHeaderPtr := nil;
// if IsEqualGUID(MediaType.formattype, FORMAT_VideoInfo) then
// begin
// BIHeaderPtr := @(PVIDEOINFOHEADER(MediaType.pbFormat)^.bmiHeader);
// BufferLen := BIHeaderPtr^.biWidth * BIHeaderPtr^.biHeight * 3;
// end
// else if IsEqualGUID(MediaType.formattype, FORMAT_VideoInfo2) then
// begin
// BIHeaderPtr := @(PVIDEOINFOHEADER2(MediaType.pbFormat)^.bmiHeader);
// BufferLen := BIHeaderPtr^.biWidth * BIHeaderPtr^.biHeight * 3;
// end;
//
// if (logo <> nil) and (buffer <> nil) and (BIHeaderPtr^.biWidth > logo_width) and
// (BIHeaderPtr^.biHeight > logo_height) then
// AddLogo(logo, logo_width, logo_height, buffer, BIHeaderPtr^.biWidth, BIHeaderPtr^.biHeight);
//
// BitmapHandle := CreateDIBSection(0, PBitmapInfo(BIHeaderPtr)^, DIB_RGB_COLORS, DIBPtr, 0, 0);
//
// if BitmapHandle <> 0 then
// begin
// try
// if DIBPtr = nil then
// Exit;
// // get DIB size
// DIBSize := BIHeaderPtr^.biSizeImage;
// if DIBSize = 0 then
// begin
// with BIHeaderPtr^ do
// DIBSize := GetDIBLineSize(biBitCount, biWidth) * biHeight * biPlanes;
// end;
// // copy DIB
// if not Assigned(buffer) then
// begin
// // get buffer size
// BufferLen := 0;
// hr := SampleGrabber.GetCurrentBuffer(BufferLen, nil);
// if (hr <> S_OK) or (BufferLen <= 0) then
// Exit;
// // copy buffer to DIB
// if BufferLen > DIBSize then // copy Min(BufferLen, DIBSize)
// BufferLen := DIBSize;
// hr := SampleGrabber.GetCurrentBuffer(BufferLen, DIBPtr);
// if hr <> S_OK then
// Exit;
//
// if (logo <> nil) and (DIBPtr <> nil) and (BIHeaderPtr^.biWidth > logo_width) and
// (BIHeaderPtr^.biHeight > logo_height) then
// AddLogo(logo, logo_width, logo_height, DIBPtr, BIHeaderPtr^.biWidth, BIHeaderPtr^.biHeight);
// end
// else
// begin
// if BufferLen > DIBSize then // copy Min(BufferLen, DIBSize)
// BufferLen := DIBSize;
// move(buffer^, DIBPtr^, BufferLen);
// end;
// bitmap.Handle := BitmapHandle;
// finally
// if bitmap.Handle <> BitmapHandle then // preserve for any changes in Graphics.pas
// DeleteObject(BitmapHandle);
// end;
// end;
// end;
// finally
// FreeMediaType(@MediaType);
// result := true
// end;
// except
// result := false;
// end;
// end;
{$WARNINGS OFF}

procedure FPUMask(enableMask: Boolean);
var
  cw: WORD; // = $1332;
begin
  cw := $1332;
  if enableMask then
  begin
    cw := Get8087CW;
    Set8087CW($133F); // alle Exceptions maskieren
  end
  else
    Set8087CW(cw);
end;
{$WARNINGS ON}

function DetectVideoCaptureDeviceType(pVideoIn: IBaseFilter;
  device_name: WideString): TVFDeviceType;
var
  // hr: HRESULT;
  pl: TVFPinList;
  emt: TVFEnumMediaType;
  i, j: Integer;
  f: Boolean;

  standard_found, mpeg_found: Boolean;
begin
  result := DT_Unknown;

  try
    if Pos(WideString('(DV/MPEG/Tape Device)'), device_name) <> 0 then
    begin
      result := DT_MPEG2_HDV;
      Exit;
    end;

    if Pos(WideString('(DV)'), device_name) <> 0 then
    begin
      result := DT_DV;
      Exit;
    end;

    standard_found := false;
    mpeg_found := false;

    pl := TVFPinList.Create(pVideoIn);

    f := false;
    for i := 0 to pl.Count - 1 do
      if pl.PinInfo[i].dir = PINDIR_OUTPUT then
      begin
        emt := TVFEnumMediaType.Create;
        emt.Assign(pl.Items[i]);

        for j := 0 to emt.Count - 1 do
        begin
          if ((IsEqualGUID(emt.Items[j].subtype,
            MEDIASUBTYPE_MPEG2_TRANSPORT_STRIDE)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2_PROGRAM)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2_TRANSPORT)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2_VIDEO)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2DATA)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1Payload)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1Packet)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1System)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1Video))) then
          begin
            f := true;
            mpeg_found := true;

            break;
          end;
        end;

        emt.Free;

        if f then
          break;
      end;

    for i := 0 to pl.Count - 1 do
      if Pos('656', pl.PinInfo[i].achName) <> 0 then
      begin
        mpeg_found := true;
        break;
      end;

    f := false;
    for i := 0 to pl.Count - 1 do
      if pl.PinInfo[i].dir = PINDIR_OUTPUT then
      begin
        emt := TVFEnumMediaType.Create;
        emt.Assign(pl.Items[i]);

        for j := 0 to emt.Count - 1 do
        begin
          if ((not((IsEqualGUID(emt.Items[j].subtype,
            MEDIASUBTYPE_MPEG2_TRANSPORT_STRIDE)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2_PROGRAM)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2_TRANSPORT)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2_VIDEO)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG2DATA)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1Payload)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1Packet)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1System)) or
            (IsEqualGUID(emt.Items[j].subtype, MEDIASUBTYPE_MPEG1Video)))) and
            (IsEqualGUID(emt.Items[j].majortype, MEDIATYPE_Video))) then
          begin
            f := true;
            standard_found := true;

            break;
          end;
        end;

        emt.Free;

        if f then
          break;
      end;

    pl.Free;

    if standard_found and mpeg_found then
      result := DT_STD_AND_MPEG2
    else if standard_found then
      result := DT_Standard
    else if mpeg_found then
      result := DT_MPEG2_ONLY;
  except
    result := DT_Unknown;
  end;
end;

function AudioInput_SetFormat(pFilter: IBaseFilter; pOutput: IPin;
  format: WideString; UseBestFormat: Boolean; Line: WideString;
  var pAudioInMixer: IAMAudioInputMixer): Integer;
var
  best_channels: Integer;
  pAudioInMixerPin: IPin;
  alloc_prop: _AllocatorProperties;
  AudioMediaTypes: TVFEnumMediaType;
  pAMBufferNegotiation: IAMBufferNegotiation;
  pl: TVFPinList;
  i: Integer;
  lst: TVFWideStringList;
  j: Integer;
  pwav: PWAVEFORMATEX;
  best_index: Integer;
  best_samplerate: Integer;
  best_bps: Integer;
  cur_channels, cur_bps, cur_samplerate: Integer;
  pAMStreamConfig: IAMStreamConfig;
  hr: HRESULT;
begin
  result := 0;

  cur_samplerate := 0;
  cur_channels := 0;
  cur_bps := 0;

  AudioMediaTypes := nil;

  hr := pOutput.QueryInterface(IID_IAMStreamConfig, pAMStreamConfig);

  if hr = S_OK then
  begin
    AudioMediaTypes := TVFEnumMediaType.Create(pOutput);

    if UseBestFormat then
    begin
      best_bps := 0;
      best_samplerate := 0;
      best_channels := 0;
      best_index := 0;

      for j := 0 to AudioMediaTypes.Count - 1 do
      begin
        pwav := AudioMediaTypes.Items[j].AMMediaType.pbFormat;
        if ((best_bps <= pwav.wBitsPerSample) and
          (best_channels <= pwav.nChannels) and
          (best_channels <= pwav.nChannels)) then
        begin
          best_bps := pwav.wBitsPerSample;
          best_channels := pwav.nChannels;
          best_samplerate := pwav.nSamplesPerSec;
          best_index := j;
        end;
      end;

      j := best_index;
      if ((j = -1) or (pAMStreamConfig.SetFormat(AudioMediaTypes.Items[j]
        .AMMediaType^) <> S_OK)) then
      begin
        result := 47;
      end;

      cur_channels := best_channels;
      cur_bps := best_bps;
      cur_samplerate := best_samplerate;

      pAMStreamConfig := nil;
    end
    else
    begin
      lst := TVFWideStringList.Create;
      for j := 0 to AudioMediaTypes.Count - 1 do
        lst.Add(AudioMediaTypes.MediaDescription[j]);
      j := lst.IndexOf(format);
      lst.Free;
      if ((j = -1) or (pAMStreamConfig.SetFormat(AudioMediaTypes.Items[j]
        .AMMediaType^) <> S_OK)) then
      begin
        result := 48;
      end;

      pwav := AudioMediaTypes.Items[j].AMMediaType.pbFormat;
      cur_channels := pwav.nChannels;
      cur_bps := pwav.wBitsPerSample;
      cur_samplerate := pwav.nSamplesPerSec;
      pAMStreamConfig := nil;
    end;
  end;

  i := 0;
  pl := TVFPinList.Create(pFilter);
  while i < pl.Count do
  begin
    if pl.PinInfo[i].dir = PINDIR_OUTPUT then
      pl.Delete(i)
    else
      inc(i);
  end;

  if pl.Count > 0 then
  begin
    for i := 0 to pl.Count - 1 do
      if trim(pl.PinInfo[i].achName) = trim(Line) then
      begin
        try
          pAudioInMixerPin := nil;
          pAudioInMixerPin := pl.Items[i];
          hr := pAudioInMixerPin.QueryInterface(IID_IAMAudioInputMixer,
            pAudioInMixer);
          if hr = S_OK then
          begin
            pAudioInMixer.put_Enable(true);
          end;
        except
          ;
        end;

        break;
      end;
  end;

  pl.Free;

  if ((cur_samplerate = 0) or (cur_bps = 0) or (cur_channels = 0) or
    (AudioMediaTypes = nil)) then
  begin
    try
      // MT := AudioMediaTypes.Items[j].AMMediaType;
      if pOutput.QueryInterface(IID_IAMBufferNegotiation, pAMBufferNegotiation)
        = S_OK then
      begin
        // pAMBufferNegotiation.GetAllocatorProperties(alloc_prop);
        alloc_prop.cBuffers := -1;
        // alloc_prop.cBuffers := 1;      :=
        // alloc_prop.cbBuffer := round(PWAVEFORMATEX(mt.pbFormat).nSamplesPerSec*PWAVEFORMATEX(mt.pbFormat).nChannels*round(PWAVEFORMATEX(mt.pbFormat).wBitsPerSample/8)*0.05); //50ms
        if cur_bps = 0 then
          cur_bps := 16;
        alloc_prop.cbBuffer := round(cur_samplerate * round(cur_bps / 8) *
          cur_channels * 0.05);
        // bug, проверить внимательно отставание звука при захвате
        // 50ms
        alloc_prop.cbAlign := -1;
        alloc_prop.cbPrefix := -1;
        pAMBufferNegotiation.SuggestAllocatorProperties(alloc_prop);
        pAMBufferNegotiation := nil;
      end;
      // AudioMediaTypes.Free;
    except
    end;
  end;

  if AudioMediaTypes <> nil then
  begin
    AudioMediaTypes.Free;
  end;
end;

function PinConnected(pPin: IPin): Boolean;
var
  pPin2: IPin;
begin
  result := false;

  if pPin = nil then
    Exit;

  pPin.ConnectedTo(pPin2);
  if pPin2 <> nil then
  begin
    pPin2 := nil;
    result := true;
  end;
end;

function GetLetterboxCoordinates(ScreenWidth, ScreenHeight, SrcWidth,
  SrcHeight: Integer): TRect;
var
  NewScrWidth, NewScrHeight, NewScrLeft, NewScrTop: Integer;
begin
  FillChar(result, SizeOf(TRect), 0);

  NewScrWidth := 0;
  NewScrHeight := 0;

  if (SrcHeight = 0) or (SrcWidth = 0) or (ScreenHeight = 0) or (ScreenWidth = 0)
  then
    Exit;

  if ((ScreenWidth > 0) and (ScreenHeight > 0)) then
  begin
    if ((SrcWidth / SrcHeight) > (ScreenWidth / ScreenHeight)) then
    begin
      NewScrWidth := ScreenWidth;
      NewScrHeight := round(SrcHeight / SrcWidth * ScreenWidth);
    end
    else
    begin
      NewScrWidth := round(SrcWidth / SrcHeight * ScreenHeight);
      NewScrHeight := ScreenHeight;
    end
  end;

  NewScrLeft := round((ScreenWidth / 2) - (NewScrWidth / 2));
  NewScrTop := round((ScreenHeight / 2) - (NewScrHeight / 2));

  result.Left := NewScrLeft;
  result.Top := NewScrTop;
  result.Right := NewScrLeft + NewScrWidth;
  result.Bottom := NewScrTop + NewScrHeight;
end;

function DumpTBitmapToBuffer(bitmap: TBitmap; pBuf: pointer; size: Integer;
  var Depth32b: Boolean): Boolean;
var
  bmp: TBitmap;
  i: Integer;
  dest: pointer;
  stride: cardinal;
begin
  result := false;

  if (pBuf = nil) or (bitmap = nil) or (bitmap.width = 0) or (bitmap.height = 0)
  then
    Exit;

  if (bitmap.PixelFormat = pf24bit) and
    (bitmap.width * bitmap.height * 3 <> size) then
    Exit;

  if (bitmap.PixelFormat = pf32bit) and
    (bitmap.width * bitmap.height * 4 <> size) then
    Exit;

  // convert if needed
  if (bitmap.PixelFormat = pf24bit) or (bitmap.PixelFormat = pf32bit) then
    bmp := bitmap
  else
  begin
    bmp := TBitmap.Create;
    bmp.width := bitmap.width;
    bmp.height := bitmap.height;
    bmp.PixelFormat := pf24bit;

    bmp.Canvas.Draw(0, 0, bitmap);
  end;

  // dump
  if bmp.PixelFormat = pf24bit then
    stride := bmp.width * 3
  else
    stride := bmp.width * 4;

  for i := 0 to bmp.height - 1 do
  begin
    dest := pointer(cardinal(pBuf) + stride);
    CopyMemory(dest, bmp.Scanline[i], stride);
  end;

  if bmp <> bitmap then
    bmp.Free;

  result := true;
end;

constructor TVFPin.Create;
begin
  Formats := TObjectList.Create;
end;

destructor TVFPin.Destroy;
begin
  Formats.Free;
end;

type
  CharNextWFunc = function(lpsz: PWideChar): PWideChar; stdcall;
  CharSet = set of AnsiChar;

var
  CharNextW: CharNextWFunc;

function CharNextW95(lpsz: PWideChar): PWideChar; stdcall;
begin
  result := lpsz + 1;
end;

procedure InitCharNextWFunc;
begin
  if (Win32Platform = VER_PLATFORM_WIN32_NT) then
    CharNextW := @windows.CharNextW
  else
    CharNextW := @CharNextW95;
end;

function WStrMove(dest: PWideChar; const Source: PWideChar; Count: cardinal)
  : PWideChar;
begin
  result := dest;
  move(Source^, dest^, Count * SizeOf(WideChar));
end;

function WStrLen(const Str: PWideChar): cardinal;
var
  P: PWideChar;
begin
  P := Str;
  while (P^ <> #0) do
    inc(P);
  result := (P - Str);
end;

function WStrAlloc(size: cardinal): PWideChar;
begin
  size := size * SizeOf(WideChar);
  inc(size, SizeOf(cardinal));
  GetMem(result, size);
  cardinal(pointer(result)^) := size;
  inc(PAnsiChar(result), SizeOf(cardinal));
end;

function WStrNew(const Str: PWideChar): PWideChar;
var
  size: cardinal;
begin
  if Str = nil then
    result := nil
  else
  begin
    size := WStrLen(Str) + 1;
    result := WStrMove(WStrAlloc(size), Str, size);
  end;
end;

function WStrScan(Str: PWideChar; Chr: WideChar): PWideChar;
begin
  result := Str;
  while result^ <> Chr do
  begin
    if result^ = #0 then
    begin
      result := nil;
      Exit;
    end;
    inc(result);
  end;
end;

function WStrEnd(const Str: PWideChar): PWideChar;
begin
  result := Str;
  while (result^ <> #0) do
    inc(result);
end;

function WideQuotedStr(const S: WideString; Quote: WideChar): WideString;
var
  P, src, dest: PWideChar;
  AddCount: Integer;
begin
  AddCount := 0;
  P := WStrScan(PWideChar(S), Quote);
  while P <> nil do
  begin
    inc(P);
    inc(AddCount);
    P := WStrScan(P, Quote);
  end;
  if AddCount = 0 then
  begin
    result := Quote + S + Quote;
    Exit;
  end;
  SetLength(result, length(S) + AddCount + 2);
  dest := pointer(result);
  dest^ := Quote;
  inc(dest);
  src := pointer(S);
  P := WStrScan(src, Quote);
  repeat
    inc(P);
    move(src^, dest^, (P - src) * 2);
    inc(dest, P - src);
    dest^ := Quote;
    inc(dest);
    src := P;
    P := WStrScan(src, Quote);
  until P = nil;
  P := WStrEnd(src);
  move(src^, dest^, (P - src) * 2);
  inc(dest, P - src);
  dest^ := Quote;
end;

function inOpSet(w: WideChar; sets: CharSet): Boolean;
begin
  if w <= #$FF then
    result := AnsiChar(w) in sets
  else
    result := false;
end;

function WideExtractQuotedStr(var src: PWideChar; Quote: WideChar): WideString;
var
  P, dest: PWideChar;
  DropCount: Integer;
begin
  result := '';
  if (src = nil) or (src^ <> Quote) then
    Exit;
  inc(src);
  DropCount := 1;
  P := src;
  src := WStrScan(src, Quote);
  while src <> nil do
  begin
    inc(src);
    if src^ <> Quote then
      break;
    inc(src);
    inc(DropCount);
    src := WStrScan(src, Quote);
  end;
  if src = nil then
    src := WStrEnd(P);
  if ((src - P) <= 1) then
    Exit;
  if DropCount = 1 then
    SetString(result, P, src - P - 1)
  else
  begin
    SetLength(result, src - P - DropCount);
    dest := PWideChar(result);
    src := WStrScan(P, Quote);
    while src <> nil do
    begin
      inc(src);
      if src^ <> Quote then
        break;
      move(P^, dest^, (src - P) * SizeOf(WideChar));
      inc(dest, src - P);
      inc(src);
      P := src;
      src := WStrScan(src, Quote);
    end;
    if src = nil then
      src := WStrEnd(P);
    move(P^, dest^, (src - P - 1) * SizeOf(WideChar));
  end;
end;

{ TVFWideStrings }

destructor TVFWideStrings.Destroy;
begin
  inherited Destroy;
end;

function TVFWideStrings.Add(const S: WideString): Integer;
begin
  result := GetCount;
  Insert(result, S);
end;

function TVFWideStrings.AddObject(const S: WideString;
  AObject: TObject): Integer;
begin
  result := Add(S);
  PutObject(result, AObject);
end;

procedure TVFWideStrings.AddStrings(Strings: TStrings);
var
  i: Integer;
begin
  try
    for i := 0 to Strings.Count - 1 do
      AddObject(Strings[i], Strings.Objects[i]);
  finally;
  end;
end;

procedure TVFWideStrings.AddStrings(Strings: TVFWideStrings);
var
  i: Integer;
begin
  try
    for i := 0 to Strings.Count - 1 do
      AddObject(Strings[i], Strings.GetObject(i));
  finally;
  end;
end;

function TVFWideStrings.CompareStrings(const S1, S2: WideString): Integer;
begin
  result := WideCompareText(S1, S2);
end;

function TVFWideStrings.GetCapacity: Integer;
begin
  result := Count;
end;

function TVFWideStrings.GetLineBreak: WideString;
begin
  if not(sdLineBreak in FDefined) then
    LineBreak := sLineBreak;
  result := FLineBreak;
end;

function TVFWideStrings.GetObject(Index: Integer): TObject;
begin
  result := nil;
end;

function TVFWideStrings.GetQuoteChar: WideChar;
begin
  if not(sdQuoteChar in FDefined) then
    QuoteChar := '"';
  result := FQuoteChar;
end;

function TVFWideStrings.GetStrictDelimiter: Boolean;
begin
  if not(sdStrictDelimiter in FDefined) then
    StrictDelimiter := false;
  result := FStrictDelimiter;
end;

function TVFWideStrings.GetTextStr: WideString;
var
  i, L, size, Count: Integer;
  P: PWideChar;
  S, LB: WideString;
begin
  Count := GetCount;
  size := 0;
  LB := sLineBreak;
  for i := 0 to Count - 1 do
    inc(size, length(Get(i)) + length(LB));
  SetString(result, nil, size);
  P := pointer(result);
  for i := 0 to Count - 1 do
  begin
    S := Get(i);
    L := length(S);
    if L <> 0 then
    begin
      System.move(pointer(S)^, P^, L * SizeOf(WideChar));
      inc(P, L);
    end;
    L := length(LB);
    if L <> 0 then
    begin
      System.move(pointer(LB)^, P^, L * SizeOf(WideChar));
      inc(P, L);
    end;
  end;
end;

function TVFWideStrings.IndexOf(const S: WideString): Integer;
begin
  for result := 0 to GetCount - 1 do
    if CompareStrings(Get(result), S) = 0 then
      Exit;
  result := -1;
end;

procedure TVFWideStrings.InsertObject(Index: Integer; const S: WideString;
  AObject: TObject);
begin
  Insert(Index, S);
  PutObject(Index, AObject);
end;

procedure TVFWideStrings.LoadFromFile(const FileName: WideString);
var
  Stream: TStream;
begin
  Stream := TFileStream.Create(FileName, fmOpenRead or fmShareDenyWrite);
  try
    LoadFromStream(Stream);
  finally
    Stream.Free;
  end;
end;

procedure TVFWideStrings.LoadFromStream(Stream: TStream);
var
  size: Integer;
  S: WideString;
begin
  try
    size := Stream.size - Stream.Position;
    SetString(S, nil, size);
    Stream.Read(pointer(S)^, size);
    SetTextStr(S);
  finally;
  end;
end;

procedure TVFWideStrings.Put(Index: Integer; const S: WideString);
var
  TempObject: TObject;
begin
  TempObject := GetObject(Index);
  Delete(Index);
  InsertObject(Index, S, TempObject);
end;

procedure TVFWideStrings.PutObject(Index: Integer; AObject: TObject);
begin
end;

procedure TVFWideStrings.SaveToFile(const FileName: WideString);
var
  Stream: TStream;
begin
  Stream := TFileStream.Create(FileName, fmCreate);
  try
    SaveToStream(Stream);
  finally
    Stream.Free;
  end;
end;

procedure TVFWideStrings.SaveToStream(Stream: TStream);
var
  S: WideString;
begin
  S := GetTextStr;
  Stream.WriteBuffer(pointer(S)^, length(S) * SizeOf(WideChar));
end;

procedure TVFWideStrings.SetCapacity(NewCapacity: Integer);
begin;
end;

procedure TVFWideStrings.SetLineBreak(const Value: WideString);
begin
  if (FLineBreak <> Value) or not(sdLineBreak in FDefined) then
  begin
    Include(FDefined, sdLineBreak);
    FLineBreak := Value;
  end
end;

procedure TVFWideStrings.SetQuoteChar(const Value: WideChar);
begin
  if (FQuoteChar <> Value) or not(sdQuoteChar in FDefined) then
  begin
    Include(FDefined, sdQuoteChar);
    FQuoteChar := Value;
  end
end;

procedure TVFWideStrings.SetStrictDelimiter(const Value: Boolean);
begin
  if (FStrictDelimiter <> Value) or not(sdStrictDelimiter in FDefined) then
  begin
    Include(FDefined, sdStrictDelimiter);
    FStrictDelimiter := Value;
  end
end;

procedure TVFWideStrings.SetTextStr(const Value: WideString);
var
  P, Start: PWideChar;
  S: WideString;
begin
  try
    Clear;
    P := pointer(Value);
    if P <> nil then
      while P^ <> #0 do
      begin
        Start := P;
        while not inOpSet(P^, [#0, #10, #13]) do
          inc(P);
        SetString(S, Start, P - Start);
        Add(S);
        if P^ = #13 then
          inc(P);
        if P^ = #10 then
          inc(P);
      end;
  finally;
  end;
end;

{ TVFWideStringList }

destructor TVFWideStringList.Destroy;
begin
  FOnChange := nil;
  FOnChanging := nil;
  inherited Destroy;
  if FCount <> 0 then
    Finalize(FList^[0], FCount);
  FCount := 0;
  SetCapacity(0);
end;

function TVFWideStringList.Add(const S: WideString): Integer;
begin
  result := AddObject(S, nil);
end;

function TVFWideStringList.AddObject(const S: WideString;
  AObject: TObject): Integer;
begin
  result := FCount;
  InsertItem(result, S, AObject);
end;

procedure TVFWideStringList.Clear;
begin
  if FCount <> 0 then
  begin
    Finalize(FList^[0], FCount);
    FCount := 0;
    SetCapacity(0);
  end;
end;

procedure TVFWideStringList.Delete(Index: Integer);
begin
  if (Index < 0) or (Index >= FCount) then
    Exit;
  Finalize(FList^[Index]);
  dec(FCount);
  if Index < FCount then
    System.move(FList^[Index + 1], FList^[Index],
      (FCount - Index) * SizeOf(TStringItem));
end;

function TVFWideStringList.Get(Index: Integer): WideString;
begin
  if (Index < 0) or (Index >= FCount) then
    Exit;
  result := FList^[Index].FString;
end;

function TVFWideStringList.GetCapacity: Integer;
begin
  result := FCapacity;
end;

function TVFWideStringList.GetCount: Integer;
begin
  result := FCount;
end;

function TVFWideStringList.GetObject(Index: Integer): TObject;
begin
  result := nil;

  if (Index < 0) or (Index >= FCount) then
    Exit;

  result := FList^[Index].FObject;
end;

procedure TVFWideStringList.Grow;
var
  Delta: Integer;
begin
  if FCapacity > 64 then
    Delta := FCapacity div 4
  else if FCapacity > 8 then
    Delta := 16
  else
    Delta := 4;
  SetCapacity(FCapacity + Delta);
end;

function TVFWideStringList.IndexOf(const S: WideString): Integer;
begin
  result := inherited IndexOf(S);
end;

procedure TVFWideStringList.Insert(Index: Integer; const S: WideString);
begin
  InsertObject(Index, S, nil);
end;

procedure TVFWideStringList.InsertItem(Index: Integer; const S: WideString;
  AObject: TObject);
begin
  if FCount = FCapacity then
    Grow;
  if Index < FCount then
    System.move(FList^[Index], FList^[Index + 1],
      (FCount - Index) * SizeOf(TStringItem));
  with FList^[Index] do
  begin
    pointer(FString) := nil;
    FObject := AObject;
    FString := S;
  end;
  inc(FCount);
end;

procedure TVFWideStringList.InsertObject(Index: Integer; const S: WideString;
  AObject: TObject);
begin
  if (Index < 0) or (Index > FCount) then
    Exit;
  InsertItem(Index, S, AObject);
end;

procedure TVFWideStringList.Put(Index: Integer; const S: WideString);
begin
  if (Index < 0) or (Index >= FCount) then
    Exit;
  FList^[Index].FString := S;
end;

procedure TVFWideStringList.PutObject(Index: Integer; AObject: TObject);
begin
  if (Index < 0) or (Index >= FCount) then
    Exit;
  FList^[Index].FObject := AObject;
end;

procedure TVFWideStringList.SetCapacity(NewCapacity: Integer);
begin
  ReallocMem(FList, NewCapacity * SizeOf(TStringItem));
  FCapacity := NewCapacity;
end;

function StringListCompareStrings(List: TVFWideStringList;
  Index1, Index2: Integer): Integer;
begin
  result := List.CompareStrings(List.FList^[Index1].FString,
    List.FList^[Index2].FString);
end;

function GetPinVideoInfo(Pin: IPin; out width: Integer;
  out height: Integer): Boolean;
var
  mts: TVFEnumMediaType;
  i: Integer;
  MT: TVFMediaType;
  header: PVIDEOINFOHEADER;
begin
  width := 0;
  height := 0;
  result := false;

  mts := TVFEnumMediaType.Create(Pin);

  for i := 0 to mts.Count - 1 do
  begin
    MT := mts.Items[i];

    if ((MT.AMMediaType.cbFormat > 0) and (MT.AMMediaType.pbFormat <> nil)) then
    begin
      if IsEqualGUID(MT.formattype, FORMAT_VideoInfo) then
      begin
        header := PVIDEOINFOHEADER(MT.AMMediaType.pbFormat);
        width := header.bmiHeader.biWidth;
        height := abs(header.bmiHeader.biHeight);

        mts.Clear;
        mts.Free;

        result := true;
        Exit;
      end;
    end;
  end;

  mts.Free;
end;

procedure ZoomBitmap(imgInput: TBitmap; var imgOutput: TBitmap; zoom: Double);
begin
  try
    imgOutput.PixelFormat := imgInput.PixelFormat;
    imgOutput.width := round(imgInput.width * zoom);
    imgOutput.height := round(imgInput.height * zoom);
    StretchBlt(imgOutput.Canvas.Handle, 0, 0, round(imgInput.width * zoom),
      round(imgInput.height * zoom), imgInput.Canvas.Handle, 0, 0,
      imgInput.width, imgInput.height, SRCCOPY);
  except
    ;
  end;
end;

function VF_GetFileSize(FileName: WideString): Int64;
var
  FindData: _WIN32_FIND_DATAW;
  hFind: THandle;
begin
  try
    result := -1;
    hFind := FindFirstFileW(StringToOleStr(FileName), FindData);
    if hFind <> INVALID_HANDLE_VALUE then
    begin
      windows.FindClose(hFind);
      // if (FindData.dwFileAttributes and FILE_ATTRIBUTE_DIRECTORY) = 0 then
      result := FindData.nFileSizeHigh * MAXDWORD + FindData.nFileSizeLow;
    end;
  except
    result := -1;
  end;
end;

function FilterSupportedEVR(dsFilters: TVFSysDevEnum): Boolean;
var
  pGraph: IGraphBuilder;
  pVideoRenderer: IBaseFilter;
  hr: HRESULT;
begin
  try
    result := false;

    if dsFilters.FilterIndexOfFriendlyName('Enhanced Video Renderer') <> -1 then
    begin
      try
        CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC_SERVER,
          IID_IGraphBuilder, pGraph);
        hr := CoCreateInstance(CLSID_EnhancedVideoRenderer, nil,
          CLSCTX_INPROC_SERVER, IID_IBaseFilter, pVideoRenderer);

        if hr = S_OK then
        begin
          hr := pGraph.AddFilter(pVideoRenderer, 'EVR');

          if hr = S_OK then
            result := true;

          pVideoRenderer := nil;
        end;

        pGraph := nil;
      except

        result := false;
      end;
    end
  except
    result := false;
  end;
end;

function FilterSupportedVMR9(dsFilters: TVFSysDevEnum): Boolean;
var
  pGraph: IGraphBuilder;
  pVideoRenderer: IBaseFilter;
  hr: HRESULT;
begin
  try
    result := false;

    if dsFilters.FilterIndexOfFriendlyName('Video Mixing Renderer 9') <> -1 then
    begin
      try
        CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC_SERVER,
          IID_IGraphBuilder, pGraph);
        hr := CoCreateInstance(CLSID_VideoMixingRenderer9, nil,
          CLSCTX_INPROC_SERVER, IID_IBaseFilter, pVideoRenderer);

        if hr = S_OK then
        begin
          hr := pGraph.AddFilter(pVideoRenderer, 'VMR9');

          if hr = S_OK then
            result := true;

          pVideoRenderer := nil;
        end;

        pGraph := nil;
      except

        result := false;
      end;
    end
  except
    result := false;
  end;
end;

procedure FindSplitter(pGraph: IGraphBuilder; var FilterName: WideString);
var
  pEnum: IEnumFilters;
  pFilter: IBaseFilter;
  cFetched: DWORD;
  audio_found, video_found: Boolean;
  hr: HRESULT;
  inpins, outpins: DWORD;
  i: Integer;
  pPin: IPin;
  info: FILTER_INFO;
begin
  hr := pGraph.EnumFilters(pEnum);

  if (hr <> S_OK) then
    Exit;

  while (pEnum.Next(1, pFilter, @cFetched) = S_OK) do
  begin
    inpins := 0;
    outpins := 0;

    audio_found := false;
    video_found := false;

    CountFilterPins(pFilter, inpins, outpins);

    for i := 0 to outpins - 1 do
    begin
      GetPin(pFilter, PINDIR_OUTPUT, i + 1, pPin);

      if (PinHaveThisType(pPin, MEDIATYPE_Audio)) then
      begin
        if Assigned(pPin) then
          pPin := nil;

        audio_found := true;
        // break;
      end
      else if (PinHaveThisType(pPin, MEDIATYPE_Video)) then
      begin
        if Assigned(pPin) then
          pPin := nil;

        video_found := true;
        // break;
      end;

      if Assigned(pPin) then
        pPin := nil;

      if audio_found and video_found then
      begin
        pFilter.QueryFilterInfo(info);
        FilterName := info.achName;
        break;
      end;
    end;

    if Assigned(pFilter) then
      pFilter := nil;

    if audio_found and video_found then
      break;
  end;

  if Assigned(pEnum) then
    pEnum := nil;
end;

function CountFilterPins(pFilter: IBaseFilter; out pulInPins: cardinal;
  out pulOutPins: cardinal): HRESULT;
var
  Enum: IEnumPins;
  Found: cardinal;
  Pin: IPin;
  PinDir: TPinDirection;
begin
  // Verify input
  if (not Assigned(pFilter) or not Assigned(@pulInPins) or
    not Assigned(@pulOutPins)) then
  begin
    result := E_POINTER;
    Exit;
  end;

  // Clear number of pins found
  pulInPins := 0;
  pulOutPins := 0;

  // Get pin enumerator
  result := pFilter.EnumPins(Enum);
  if FAILED(result) then
    Exit;

  Enum.Reset;

  // Count every pin on the filter
  while (S_OK = Enum.Next(1, Pin, @Found)) do
  begin
    result := Pin.QueryDirection(PinDir);
    if (PinDir = PINDIR_INPUT) then
      inc(pulInPins)
    else
      inc(pulOutPins);
    Pin := nil;
  end;

  Enum := nil;
end;

function IsFilterConnected(pFilter: IBaseFilter): Boolean;
var
  pPin1, pPin2: IPin;
begin
  try
    result := false;
    GetPin(pFilter, PINDIR_INPUT, 1, pPin1);
    if pPin1 = nil then
      Exit;

    pPin1.ConnectedTo(pPin2);
    if pPin2 <> nil then
    begin
      result := true;
      pPin2 := nil;
    end;
    pPin1 := nil;
  except
    result := false;
  end;
end;

procedure ShowPinInfo(pPin: IPin);
var
  pi: PIN_INFO;
begin
  if pPin <> nil then
  begin
    pPin.QueryPinInfo(pi);
    ShowMessage(pi.achName);
  end;
end;

procedure ShowFilterInfo(pFilter: IBaseFilter);
var
  fi: FILTER_INFO;
begin
  if pFilter <> nil then
  begin
    pFilter.QueryFilterInfo(fi);
    ShowMessage(fi.achName);
  end;
end;

procedure DisconnectFilter(pFilter: IBaseFilter);
var
  pPin1, pPin2: IPin;
begin
  GetPin(pFilter, PINDIR_INPUT, 1, pPin1);
  if pPin1 <> nil then
  begin
    pPin1.ConnectedTo(pPin2);
    if pPin2 <> nil then
    begin
      pPin1.Disconnect;
      pPin2.Disconnect;

      pPin2 := nil;
    end;
    pPin1 := nil;
  end;
end;

procedure FilterConnectedTo(pSrc: IBaseFilter; dir: PIN_DIRECTION;
  var pDest: IBaseFilter);
var
  pPin1, pPin2: IPin;
  pi: PIN_INFO;
begin
  GetPin(pSrc, dir, 1, pPin1);
  if pPin1 <> nil then
  begin
    pPin1.ConnectedTo(pPin2);
    if pPin2 <> nil then
    begin
      pPin2.QueryPinInfo(pi);
      pDest := pi.pFilter;

      pPin2 := nil;
    end;
    pPin1 := nil;
  end;
end;

function GetFilterVideoOutputPin(pFilter: IBaseFilter; var pPin: IPin): Boolean;
var
  pl1: TVFPinList;
  f: Boolean;
  VideoMediaTypes: TVFEnumMediaType;
  i, j: Integer;
begin
  pl1 := nil;
  VideoMediaTypes := nil;

  try
    result := false;

    pl1 := TVFPinList.Create(pFilter);
    f := false;

    VideoMediaTypes := TVFEnumMediaType.Create;
    for i := 0 to pl1.Count - 1 do
      if (pl1.PinInfo[i].dir = PINDIR_OUTPUT) then
      begin
        VideoMediaTypes.Assign(pl1.Items[i]);
        for j := 0 to VideoMediaTypes.Count - 1 do
          if ((IsEqualGUID(VideoMediaTypes.Items[j].majortype, MEDIATYPE_Video))
            or (IsEqualGUID(VideoMediaTypes.Items[j].majortype,
            MEDIATYPE_Interleaved)) or
            (IsEqualGUID(VideoMediaTypes.Items[j].majortype, MEDIATYPE_Stream))
            or (IsEqualGUID(VideoMediaTypes.Items[j].majortype,
            MEDIATYPE_AnalogVIDEO)) or
            (IsEqualGUID(VideoMediaTypes.Items[j].majortype,
            MEDIATYPE_MPEG2_PACK)) or
            (IsEqualGUID(VideoMediaTypes.Items[j].majortype,
            MEDIATYPE_MPEG2_PES)) or
            (IsEqualGUID(VideoMediaTypes.Items[j].majortype,
            MEDIATYPE_MPEG2_SECTIONS)) or
            (IsEqualGUID(VideoMediaTypes.Items[j].majortype,
            MEDIATYPE_MPEG1SystemStream))) or
            (IsEqualGUID(VideoMediaTypes.Items[j].majortype, MEDIATYPE_NULL))
          then
          begin
            pPin := pl1.Items[i];
            f := true;
            break;
          end;

        if f then
          break
      end;

    if pPin <> nil then
      result := true;
  finally
    if VideoMediaTypes <> nil then
    begin
      VideoMediaTypes.Free;
    end;

    if pl1 <> nil then
    begin
      pl1.Free;
    end;
  end;
end;

function AddFilterFromCLSID(filterGraph: IFilterGraph2; CLSID: TGUID;
  name: WideString): IBaseFilter;
var
  hr: HRESULT;
begin
  result := nil;

  try
    hr := CoCreateInstance(CLSID, nil, CLSCTX_INPROC_SERVER,
      IID_IBaseFilter, result);

    if hr = S_OK then
    begin
      hr := filterGraph.AddFilter(result, StringToOleStr(name));
      if hr < 0 then
        result := nil;
    end;
  except
    ;
  end;
end;

initialization

InitCharNextWFunc;

end.
