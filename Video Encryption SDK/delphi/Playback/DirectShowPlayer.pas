unit DirectShowPlayer;

interface

uses
  Windows, ActiveX, DirectShow9, encryptor_intf, Vcl.Forms, Vcl.Dialogs;

type
  TDirectShowPlayer = class
  private
    FGraph: IGraphBuilder;
    FCaptureGraph: ICaptureGraphBuilder2;
    FMediaControl: IMediaControl;
    FVideoWindow: IVideoWindow;
    procedure SetupVideoWindow(WindowHandle: HWND);
    procedure SetupFilters(const FileName: WideString; password: WideString);
  public
    constructor Create(WindowHandle: HWND; const FileName: WideString;
      const password: WideString);
    destructor Destroy; override;
    procedure Play;
    procedure Stop;
  end;

implementation

constructor TDirectShowPlayer.Create(WindowHandle: HWND;
  const FileName: WideString; const password: WideString);
begin
  CoInitialize(nil);

  // Create the Filter Graph Manager
  CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC_SERVER,
    IID_IGraphBuilder, FGraph);

  CoCreateInstance(CLSID_CaptureGraphBuilder2, nil, CLSCTX_INPROC_SERVER,
    IID_ICaptureGraphBuilder2, FCaptureGraph);

  FCaptureGraph.SetFiltergraph(FGraph);

  // Set up filters
  SetupFilters(FileName, password);

  // Set up the video window
  SetupVideoWindow(WindowHandle);

  FGraph.QueryInterface(IMediaControl, FMediaControl);
end;

destructor TDirectShowPlayer.Destroy;
begin
  FMediaControl := nil;
  FVideoWindow := nil;
  FGraph := nil;
  CoUninitialize;
  inherited;
end;

procedure TDirectShowPlayer.SetupVideoWindow(WindowHandle: HWND);
var
  Rect: TRect;
begin
  FGraph.QueryInterface(IVideoWindow, FVideoWindow);
  FVideoWindow.put_Owner(OAHWnd(WindowHandle));
  FVideoWindow.put_WindowStyle(WS_CHILD or WS_CLIPCHILDREN);

  // Get the client area of the specified window
  GetClientRect(WindowHandle, Rect);

  // Set the video window size to match the client area of the parent window
  FVideoWindow.SetWindowPosition(Rect.Left, Rect.Top, Rect.Right - Rect.Left, Rect.Bottom - Rect.Top);

  FVideoWindow.put_MessageDrain(OAHWnd(WindowHandle));
end;


procedure TDirectShowPlayer.SetupFilters(const FileName: WideString;
  password: WideString);
const
  LAVVideoDecoder: TGUID = '{EE30215D-164F-4A92-A4EB-9D4C13390F9F}';
  LAVAudioDecoder: TGUID = '{E8E73B6B-4CB3-44A4-BE99-4F7BCB96E491}';
  Decryptor: TGUID = '{D2C761F0-9988-4F79-9B0E-FB2B79C65851}';
var
  SourceFilter: IBaseFilter;
  DecryptorFilter: IBaseFilter;
  VideoDecoderFilter: IBaseFilter;
  AudioDecoderFilter: IBaseFilter;
  config: IVFCryptoConfig;
  passwordPtr: Pointer;
  passwordLen: Integer;
  hr: HRESULT;
begin
  // Add the file source filter
  FGraph.AddSourceFilter(PWideChar(FileName), PWideChar(FileName),
    SourceFilter);

  // Add demuxer
  CoCreateInstance(Decryptor, nil, CLSCTX_INPROC_SERVER, IID_IBaseFilter,
    DecryptorFilter);
  FGraph.AddFilter(DecryptorFilter, 'Decryptor');

  DecryptorFilter.QueryInterface(IID_IVFCryptoConfig, config);
  if config <> nil then
  begin
    passwordPtr := System.StringToOleStr(password);
    passwordLen := Length(password) * 2;
    config.put_Password(passwordPtr, passwordLen);

    config := nil;
  end;

  hr := FCaptureGraph.RenderStream(nil, nil, SourceFilter, nil,
    DecryptorFilter);
  if hr <> 0 then
    ShowMessage('Unable to render decryptor.');

  // Add the video decoder filter
  CoCreateInstance(LAVVideoDecoder, nil, CLSCTX_INPROC_SERVER, IID_IBaseFilter,
    VideoDecoderFilter);
  FGraph.AddFilter(VideoDecoderFilter, 'LAV Video Decoder');

  hr := FCaptureGraph.RenderStream(nil, @MEDIATYPE_Video, DecryptorFilter, nil,
    VideoDecoderFilter);
  if hr <> 0 then
    ShowMessage('Unable to render video.');

  // Add the audio decoder filter
  CoCreateInstance(LAVAudioDecoder, nil, CLSCTX_INPROC_SERVER, IID_IBaseFilter,
    AudioDecoderFilter);
  FGraph.AddFilter(AudioDecoderFilter, 'LAV Audio Decoder');

  hr := FCaptureGraph.RenderStream(nil, @MEDIATYPE_Audio, DecryptorFilter, nil,
    AudioDecoderFilter);
  if hr <> 0 then
    ShowMessage('Unable to render audio.');

  // render audio/video streams
  hr := FCaptureGraph.RenderStream(nil, @MEDIATYPE_Video, VideoDecoderFilter,
    nil, nil);
  if hr <> 0 then
    ShowMessage('Unable to render video stream to video view.');

  hr := FCaptureGraph.RenderStream(nil, @MEDIATYPE_Audio, AudioDecoderFilter,
    nil, nil);
  if hr <> 0 then
    ShowMessage('Unable to render audio stream to audio renderer.');
end;

procedure TDirectShowPlayer.Play;
begin
  FMediaControl.Run;
end;

procedure TDirectShowPlayer.Stop;
begin
  FMediaControl.Stop;
end;

end.
