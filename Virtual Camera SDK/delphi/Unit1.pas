unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, Vcl.ExtCtrls,
  VFVCDirectShow9, VCFiltersAPI, VCClasses, VirtualCameraIntf,
  VideoCaptureTypes,
  strutils,
  math,
  Contnrs,
  mmsystem,
  activex,
  Registry,
  DateUtils,
  AxCtrls;

type
  TForm1 = class(TForm, ISampleGrabberCB)
    Label1: TLabel;
    Label2: TLabel;
    rbFile: TRadioButton;
    rbCamera: TRadioButton;
    Panel1: TPanel;
    edSourceFile: TEdit;
    btOpenFile: TButton;
    Label4: TLabel;
    cbVideoCaptureSource: TComboBox;
    Label7: TLabel;
    cbAudioCaptureSource: TComboBox;
    btSourceStop: TButton;
    btSourceStart: TButton;
    pnScreen1: TPanel;
    btCameraStartPreview1: TButton;
    btCameraStop1: TButton;
    rbScreen: TRadioButton;
    rbRandom: TRadioButton;
    GroupBox1: TGroupBox;
    rbScreenFullScreen: TRadioButton;
    rbScreenCustom: TRadioButton;
    edScreenLeft: TEdit;
    Label32: TLabel;
    Label38: TLabel;
    edScreenRight: TEdit;
    Label39: TLabel;
    edScreenBottom: TEdit;
    edScreenTop: TEdit;
    Label37: TLabel;
    rbControlHandle: TRadioButton;
    procedure btCameraStartPreview1Click(Sender: TObject);
    procedure btCameraStop1Click(Sender: TObject);
    procedure btSourceStartClick(Sender: TObject);
    procedure btSourceStopClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    audioFilter1: IBaseFilter;
    audioSinkFilter: IBaseFilter;
    cameraEffectsFilter1: IBaseFilter;
    cameraFilter1: IBaseFilter;
    captureGraphCamera1: ICaptureGraphBuilder2;
    captureGraphSource: ICaptureGraphBuilder2;
    filterGraphCamera1: IFilterGraph2;
    filterGraphSource: IFilterGraph2;
    mediaControlCamera1: IMediaControl;
    mediaControlSource: IMediaControl;
    mediaEventExCamera1: IMediaEventEx;
    mediaEventExSource: IMediaEventEx;
    sourceAudioFilter: IBaseFilter;
    sourceSinkIntf: IVFVirtualCameraSink;
    sourceVideoFilter: IBaseFilter;
    sourceScreenIntf: IVFScreenCapture3;
    sampleGrabber: IBaseFilter;
    sampleGrabberIntf: ISampleGrabber;
    videoSinkFilter: IBaseFilter;
    videoWindowCamera1: IVideoWindow;
    videoWindowSource: IVideoWindow;
    procedure CameraFree1;
    procedure CameraInitPreview1;
    function FindSourceVideoCaptureDevice(filterGraph: IFilterGraph2;
      name: WideString): IBaseFilter;
    function FindSourceAudioCaptureDevice(filterGraph: IFilterGraph2;
      name: WideString): IBaseFilter;
    procedure ResizeVideoWindow1;
    procedure SourceFree;
    procedure SourceInitCamera;
    procedure SourceInitFile;
    procedure SourceInitScreen;
    procedure SourceInitRandomGray;
    function BufferCB(SampleTime: Double; pBuffer: PByte; BufferLen: longint)
      : HRESULT; stdcall;
    function SampleCB(SampleTime: Double; pSample: IMediaSample)
      : HRESULT; stdcall;
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

const
  WM_GRAPHNOTIFY = $8000 + 1;

implementation

{$R *.dfm}

procedure SetSampleGrabberMediaType(Intf: ISampleGrabber;
  width, height: Integer);
var
  SampleGrabber_MediaType: TAMMediaType;
  vih: VIDEOINFOHEADER;
begin
  if not Assigned(Intf) then
    Exit;

  ZeroMemory(@SampleGrabber_MediaType, SizeOf(TAMMediaType));

  SampleGrabber_MediaType.majortype := MEDIATYPE_Video;
  SampleGrabber_MediaType.subtype := MEDIASUBTYPE_RGB24;
  SampleGrabber_MediaType.formattype := FORMAT_VideoInfo;

  ZeroMemory(@vih, SizeOf(VIDEOINFOHEADER));
  vih.rcSource.Right := width;
  vih.rcSource.Bottom := height;
  vih.rcTarget := vih.rcSource;

  vih.bmiHeader.biBitCount := 24;
  vih.bmiHeader.biCompression := BI_RGB;
  vih.bmiHeader.biWidth := width;
  vih.bmiHeader.biHeight := height;
  vih.bmiHeader.biPlanes := 1;
  vih.bmiHeader.biSize := SizeOf(BITMAPINFOHEADER);
  vih.bmiHeader.biSizeImage := width * height * 3;

  SampleGrabber_MediaType.pbFormat := @vih;
  SampleGrabber_MediaType.cbFormat := SizeOf(VIDEOINFOHEADER);
  SampleGrabber_MediaType.lSampleSize := width * height * 3;

  Intf.SetMediaType(SampleGrabber_MediaType);
end;

function TForm1.BufferCB(SampleTime: Double; pBuffer: PByte; BufferLen: longint)
  : HRESULT; stdcall;
begin
  FillChar(pBuffer^, BufferLen, random(255));

  Result := 0;
end;

function TForm1.SampleCB(SampleTime: Double; pSample: IMediaSample)
  : HRESULT; stdcall;
begin
  Result := 0;
end;

procedure TForm1.btCameraStartPreview1Click(Sender: TObject);
begin
  btCameraStartPreview1.Enabled := false;
  btCameraStop1.Enabled := true;

  CameraInitPreview1();
end;

procedure TForm1.btCameraStop1Click(Sender: TObject);
begin
  btCameraStartPreview1.Enabled := true;
  btCameraStop1.Enabled := false;

  CameraFree1();
end;

procedure TForm1.btSourceStartClick(Sender: TObject);
begin
  btSourceStart.Enabled := false;
  btSourceStop.Enabled := true;

  if (rbFile.Checked) then
  begin
    SourceInitFile();
  end
  else if rbCamera.Checked then
  begin
    SourceInitCamera();
  end
  else if rbScreen.Checked or rbControlHandle.Checked then
  begin
    SourceInitScreen();
  end
  else if rbRandom.Checked then
  begin
    SourceInitRandomGray();
  end;
end;

procedure TForm1.btSourceStopClick(Sender: TObject);
begin
  btSourceStop.Enabled := false;
  btSourceStart.Enabled := true;

  SourceFree();
end;

procedure TForm1.FormCreate(Sender: TObject);
var
  videoCaptureDevices, audioCaptureDevices: TVFSysDevEnum;
  i: Integer;
begin
  // Get all video / audio input devices
  videoCaptureDevices := TVFSysDevEnum.Create(CLSID_VideoInputDeviceCategory);

  for i := 0 to videoCaptureDevices.CountFilters - 1 do
  begin
    cbVideoCaptureSource.Items.Add(videoCaptureDevices.Filters[i].FriendlyName);
  end;

  if (cbVideoCaptureSource.Items.Count > 0) then
  begin
    cbVideoCaptureSource.ItemIndex := 0;
  end;

  videoCaptureDevices.Free;

  audioCaptureDevices := TVFSysDevEnum.Create(CLSID_AudioInputDeviceCategory);

  for i := 0 to audioCaptureDevices.CountFilters - 1 do
  begin
    cbAudioCaptureSource.Items.Add(audioCaptureDevices.Filters[i].FriendlyName);
  end;

  if (cbAudioCaptureSource.Items.Count > 0) then
  begin
    cbAudioCaptureSource.ItemIndex := 0;
  end;

  audioCaptureDevices.Free;

  // EnumCameraFormats1();
  // EnumCameraFormats2();
end;

procedure TForm1.CameraFree1;
begin
  // Stop previewing data
  if (mediaControlCamera1 <> nil) then
    mediaControlCamera1.StopWhenReady();

  // Stop receiving events
  if (mediaEventExCamera1 <> nil) then
    mediaEventExCamera1.SetNotifyWindow(0, WM_GRAPHNOTIFY, 0);

  // Relinquish ownership (IMPORTANT!) of the video window.
  // Failing to call put_Owner can lead to assert failures within
  // the video renderer, as it still assumes that it has a valid
  // parent window.
  if (videoWindowCamera1 <> nil) then
  begin
    videoWindowCamera1.put_Visible(false);
    videoWindowCamera1.put_Owner(0);
  end;

  // Release DirectShow interfaces
  mediaControlCamera1 := nil;
  mediaEventExCamera1 := nil;
  videoWindowCamera1 := nil;
  filterGraphCamera1 := nil;
  captureGraphCamera1 := nil;
end;

procedure TForm1.CameraInitPreview1;
var
  outputPin, pin: IPin;
const
  WM_GRAPHNOTIFY = $8000 + 1;
begin
  // An exception is thrown if cast fail
  CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC, IID_IFilterGraph2,
    filterGraphCamera1);
  CoCreateInstance(CLSID_CaptureGraphBuilder2, nil, CLSCTX_INPROC,
    IID_ICaptureGraphBuilder2, captureGraphCamera1);
  captureGraphCamera1.SetFiltergraph(filterGraphCamera1);

  filterGraphCamera1.QueryInterface(IID_IMediaControl, mediaControlCamera1);
  filterGraphCamera1.QueryInterface(IID_IVideoWindow, videoWindowCamera1);
  filterGraphCamera1.QueryInterface(IID_IMediaEventEx, mediaEventExCamera1);

  mediaEventExCamera1.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, 0);

  // Attach the filter graph to the capture graph
  captureGraphCamera1.SetFiltergraph(filterGraphCamera1);

  cameraFilter1 := AddFilterFromCLSID(filterGraphCamera1,
    CLSID_VFVirtualCameraSource, 'VisioForge Virtual Camera');

  GetFreePinWithMediaType(cameraFilter1, PINDIR_OUTPUT, MEDIATYPE_Video,
    outputPin);

  if (outputPin = nil) then
  begin
    cameraFilter1 := nil;

    Exit;
  end;

  // Render the preview pin on the video capture filter
  // Use this instead of this.graphBuilder.RenderFile
  captureGraphCamera1.RenderStream(nil, // @PinCategory_Capture,
    @MEDIATYPE_Video, cameraFilter1, cameraEffectsFilter1, nil);
  // DsError.ThrowExceptionForHR(hr);

  // Set the video window to be a child of the main window
  videoWindowCamera1.put_Owner(pnScreen1.Handle);
  // DsError.ThrowExceptionForHR(hr);

  videoWindowCamera1.put_WindowStyle(WS_CHILD xor WS_ClipChildren);
  // DsError.ThrowExceptionForHR(hr);

  // Use helper function to position video window in client rect
  // of main application window
  ResizeVideoWindow1();

  // Make the video window visible, now that it is properly positioned
  videoWindowCamera1.put_Visible(true);
  // DsError.ThrowExceptionForHR(hr);

  audioFilter1 := AddFilterFromCLSID(filterGraphCamera1,
    CLSID_VFVirtualAudioCardSource, 'VisioForge Virtual Audio Card');
  GetFreePinWithMediaType(audioFilter1, PINDIR_OUTPUT, MEDIATYPE_Audio, pin);

  filterGraphCamera1.Render(pin);

  // Marshal.ReleaseComObject(sourceVideoFilter);
  // sourceVideoFilter = null;

  mediaControlCamera1.Run();
end;

function TForm1.FindSourceVideoCaptureDevice(filterGraph: IFilterGraph2;
  name: WideString): IBaseFilter;
var
  k: Integer;
  Filters: TVFSysDevEnum;
begin
  Filters := nil;

  try
    Filters := TVFSysDevEnum.Create(CLSID_VideoInputDeviceCategory);

    if Filters = nil then
      Exit;

    if filterGraph = nil then
      Exit;

    if trim(name) = '' then
      Exit;

    k := Filters.FilterIndexOfFriendlyName(name);

    if k = -1 then
      Exit;

    Filters.GetMoniker(Filters.FilterIndexOfFriendlyName(name))
      .BindToObject(nil, nil, IID_IBaseFilter, Result);

    if Assigned(Result) then
    begin
      filterGraph.AddFilter(Result, StringToOleStr(name));
    end;
  finally
    if Assigned(Filters) then
      Filters.Free;
  end;
end;

function TForm1.FindSourceAudioCaptureDevice(filterGraph: IFilterGraph2;
  name: WideString): IBaseFilter;
var
  k: Integer;
  Filters: TVFSysDevEnum;
begin
  Filters := nil;
  try
    Filters := TVFSysDevEnum.Create(CLSID_AudioInputDeviceCategory);

    if Filters = nil then
      Exit;

    if filterGraph = nil then
      Exit;

    if trim(name) = '' then
      Exit;

    k := Filters.FilterIndexOfFriendlyName(name);

    if k = -1 then
      Exit;

    Filters.GetMoniker(Filters.FilterIndexOfFriendlyName(name))
      .BindToObject(nil, nil, IID_IBaseFilter, Result);

    if Assigned(Result) then
    begin
      filterGraph.AddFilter(Result, StringToOleStr(name));
    end;
  finally
    if Assigned(Filters) then
      Filters.Free;
  end;
end;

procedure TForm1.ResizeVideoWindow1;
begin
  // Resize the video preview window to match owner window size
  if (videoWindowCamera1 <> nil) then
  begin
    videoWindowCamera1.SetWindowPosition(0, 0, pnScreen1.width,
      pnScreen1.height);
  end;
end;

procedure TForm1.SourceFree;
begin
  // Stop previewing data
  if (mediaControlSource <> nil) then
  begin
    mediaControlSource.StopWhenReady();
  end;

  // Stop receiving events
  if (mediaEventExSource <> nil) then
  begin
    mediaEventExSource.SetNotifyWindow(0, WM_GRAPHNOTIFY, 0);
  end;

  // Relinquish ownership (IMPORTANT!) of the video window.
  // Failing to call put_Owner can lead to assert failures within
  // the video renderer, as it still assumes that it has a valid
  // parent window.
  if (videoWindowSource <> nil) then
  begin
    videoWindowSource.put_Visible(false);
    videoWindowSource.put_Owner(0);
  end;

  // Release DirectShow interfaces
  mediaControlSource := nil;
  mediaEventExSource := nil;
  videoWindowSource := nil;
  filterGraphSource := nil;
  captureGraphSource := nil;
  sourceVideoFilter := nil;
  videoSinkFilter := nil;
  audioSinkFilter := nil;
end;

procedure TForm1.SourceInitCamera;
var
  outputPin: IPin;
begin
  // An exception is thrown if cast fail
  CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC, IID_IFilterGraph2,
    filterGraphSource);
  CoCreateInstance(CLSID_CaptureGraphBuilder2, nil, CLSCTX_INPROC,
    IID_ICaptureGraphBuilder2, captureGraphSource);
  captureGraphSource.SetFiltergraph(filterGraphSource);

  filterGraphSource.QueryInterface(IID_IMediaControl, mediaControlSource);
  filterGraphSource.QueryInterface(IID_IVideoWindow, videoWindowSource);
  filterGraphSource.QueryInterface(IID_IMediaEventEx, mediaEventExSource);

  mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, 0);

  // Attach the filter graph to the capture graph
  captureGraphSource.SetFiltergraph(filterGraphSource);

  // video source
  sourceVideoFilter := FindSourceVideoCaptureDevice(filterGraphSource,
    cbVideoCaptureSource.Text);
  filterGraphSource.AddFilter(sourceVideoFilter,
    StringToOleStr(cbVideoCaptureSource.Text));

  GetFreePinWithMediaType(sourceVideoFilter, PINDIR_OUTPUT, MEDIATYPE_Video,
    outputPin);

  if (outputPin = nil) then
  begin
    sourceVideoFilter := nil;

    Exit;
  end;

  // audio source
  sourceAudioFilter := FindSourceAudioCaptureDevice(filterGraphSource,
    cbAudioCaptureSource.Text);
  filterGraphSource.AddFilter(sourceAudioFilter,
    StringToOleStr(cbAudioCaptureSource.Text));

  // sinks
  videoSinkFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFVirtualCameraSink, 'VisioForge Virtual Camera Sink - Video');

  videoSinkFilter.QueryInterface(IID_IVFVirtualCameraSink, sourceSinkIntf);
  sourceSinkIntf.set_license('TRIAL');

  audioSinkFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFVirtualAudioCardSink, 'VisioForge Virtual Camera Sink - Audio');

  // Render the preview pin on the video capture filter
  // Use this instead of this.graphBuilder.RenderFile
  captureGraphSource.RenderStream(nil, nil, sourceVideoFilter, nil,
    videoSinkFilter);

  captureGraphSource.RenderStream(nil, nil, sourceAudioFilter, nil,
    audioSinkFilter);

  mediaControlSource.Run();
end;

procedure TForm1.SourceInitFile;
begin
  // An exception is thrown if cast fail
  CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC, IID_IFilterGraph2,
    filterGraphSource);
  CoCreateInstance(CLSID_CaptureGraphBuilder2, nil, CLSCTX_INPROC,
    IID_ICaptureGraphBuilder2, captureGraphSource);
  captureGraphSource.SetFiltergraph(filterGraphSource);

  filterGraphSource.QueryInterface(IID_IMediaControl, mediaControlSource);
  filterGraphSource.QueryInterface(IID_IVideoWindow, videoWindowSource);
  filterGraphSource.QueryInterface(IID_IMediaEventEx, mediaEventExSource);

  mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, 0);

  // Attach the filter graph to the capture graph
  captureGraphSource.SetFiltergraph(filterGraphSource);

  // Guid sinkGuid = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340");
  videoSinkFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFVirtualCameraSink, 'VisioForge Virtual Camera Sink - Video');

  sourceSinkIntf := videoSinkFilter as IVFVirtualCameraSink;
  sourceSinkIntf.set_license('TRIAL');

  audioSinkFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFVirtualAudioCardSink, 'VisioForge Virtual Camera Sink - Audio');

  filterGraphSource.AddSourceFilter(StringToOleStr(edSourceFile.Text),
    'Source file', sourceVideoFilter);

  // Render the preview pin on the video capture filter
  captureGraphSource.RenderStream(nil, nil, sourceVideoFilter, nil,
    videoSinkFilter);

  captureGraphSource.RenderStream(nil, nil, sourceVideoFilter, nil,
    audioSinkFilter);

  mediaControlSource.Run();
end;

procedure TForm1.SourceInitScreen;
var
  rect: VFRect;
  controlWidth, controlHeight: Integer;
begin
  // An exception is thrown if cast fail
  CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC, IID_IFilterGraph2,
    filterGraphSource);
  CoCreateInstance(CLSID_CaptureGraphBuilder2, nil, CLSCTX_INPROC,
    IID_ICaptureGraphBuilder2, captureGraphSource);
  captureGraphSource.SetFiltergraph(filterGraphSource);

  filterGraphSource.QueryInterface(IID_IMediaControl, mediaControlSource);
  filterGraphSource.QueryInterface(IID_IVideoWindow, videoWindowSource);
  filterGraphSource.QueryInterface(IID_IMediaEventEx, mediaEventExSource);

  mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, 0);

  // Attach the filter graph to the capture graph
  captureGraphSource.SetFiltergraph(filterGraphSource);

  // Guid sinkGuid = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340");
  videoSinkFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFVirtualCameraSink, 'VisioForge Virtual Camera Sink - Video');

  sourceSinkIntf := videoSinkFilter as IVFVirtualCameraSink;
  sourceSinkIntf.set_license('TRIAL');

  sourceVideoFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFScreenCapture, 'VisioForge Screen Source');

  sourceVideoFilter.QueryInterface(IID_IVFScreenCapture3, sourceScreenIntf);
  if Assigned(sourceScreenIntf) then
  begin
    if rbScreen.Checked then
    begin
      sourceScreenIntf.set_mode(SCM_Screen);

      if rbScreenCustom.Checked then
      begin
        rect.left := StrToInt(edScreenLeft.Text);
        rect.top := StrToInt(edScreenTop.Text);
        rect.Right := StrToInt(edScreenRight.Text);
        rect.Bottom := StrToInt(edScreenBottom.Text);
        sourceScreenIntf.set_rect(rect);
      end;
    end
    else
    begin
      sourceScreenIntf.set_mode(SCM_Window);
      sourceScreenIntf.set_window_handle(self.Handle);
//      sourceScreenIntf.get_window_size(self.Handle, controlWidth,
//        controlHeight);
//      rect.top := 0;
//      rect.left := 0;
//      rect.Right := controlWidth;
//      rect.Bottom := controlHeight;
//      sourceScreenIntf.set_rect(rect);
    end;
  end;

  // Render the preview pin on the video capture filter
  captureGraphSource.RenderStream(nil, @MEDIATYPE_Video, sourceVideoFilter, nil,
    videoSinkFilter);

  mediaControlSource.Run();
end;

procedure TForm1.SourceInitRandomGray;
var
  colorSourceIntf: IVFScreenCapture3;
  rectx: VFRect;
const
  width = 640;
  height = 480;
begin
  // An exception is thrown if cast fail
  CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC, IID_IFilterGraph2,
    filterGraphSource);
  CoCreateInstance(CLSID_CaptureGraphBuilder2, nil, CLSCTX_INPROC,
    IID_ICaptureGraphBuilder2, captureGraphSource);
  captureGraphSource.SetFiltergraph(filterGraphSource);

  filterGraphSource.QueryInterface(IID_IMediaControl, mediaControlSource);
  filterGraphSource.QueryInterface(IID_IVideoWindow, videoWindowSource);
  filterGraphSource.QueryInterface(IID_IMediaEventEx, mediaEventExSource);

  mediaEventExSource.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, 0);

  // Attach the filter graph to the capture graph
  captureGraphSource.SetFiltergraph(filterGraphSource);

  // Guid sinkGuid = new Guid("AA6AB4DF-9670-4913-88BB-2CB381C19340");
  videoSinkFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFVirtualCameraSink, 'VisioForge Virtual Camera Sink - Video');

  sourceSinkIntf := videoSinkFilter as IVFVirtualCameraSink;
  sourceSinkIntf.set_license('TRIAL');

  sourceVideoFilter := AddFilterFromCLSID(filterGraphSource,
    CLSID_VFScreenCapture, 'VisioForge Screen Source');
  sourceVideoFilter.QueryInterface(IID_IVFScreenCapture3, colorSourceIntf);

  if colorSourceIntf <> nil then
  begin
    colorSourceIntf.set_mode(SCM_Color);
    colorSourceIntf.set_fps(10);

    rectx.left := 0;
    rectx.top := 0;
    rectx.Right := width;
    rectx.Bottom := height;
    colorSourceIntf.set_rect(rectx);
  end;

  // sample grabber
  sampleGrabber := AddFilterFromCLSID(filterGraphSource, CLSID_SampleGrabber,
    'Sample Grabber');

  sampleGrabber.QueryInterface(IID_ISampleGrabber, sampleGrabberIntf);
  if sampleGrabberIntf <> nil then
  begin
    SetSampleGrabberMediaType(sampleGrabberIntf, width, height);
    sampleGrabberIntf.SetCallback(self, 1);
  end;

  // Render the preview pin on the video capture filter
  captureGraphSource.RenderStream(nil, @MEDIATYPE_Video, sourceVideoFilter,
    sampleGrabber, videoSinkFilter);

  mediaControlSource.Run();
end;

end.
