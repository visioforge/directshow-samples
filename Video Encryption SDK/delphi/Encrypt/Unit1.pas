unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, StrUtils, ActiveX, ENCDirectShow9, encryptor_intf,
  Vcl.ComCtrls, Vcl.ExtCtrls, helpers;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    edSourceFile: TEdit;
    btSelectFile: TButton;
    btStart: TButton;
    btStop: TButton;
    OpenDialog1: TOpenDialog;
    Label2: TLabel;
    edPassword: TEdit;
    Label3: TLabel;
    edOutputFile: TEdit;
    btSaveOutput: TButton;
    SaveDialog1: TSaveDialog;
    ProgressBar1: TProgressBar;
    Timer1: TTimer;
    GroupBox1: TGroupBox;
    rbDecodeAuto: TRadioButton;
    rbDecodeLAV: TRadioButton;
    rbNoReencoding: TRadioButton;
    procedure btSaveOutputClick(Sender: TObject);
    procedure btSelectFileClick(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure btStopClick(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
  private
    procedure WndProc(var Message: TMessage); override;
  public
    procedure AddEncoders;
    procedure AddNoReencode;
    procedure CreateGraph;
    procedure ClearGraph;
    procedure AddSource();
    procedure ConfigureEncryptor;
    procedure OnComplete;
    procedure StartGraph;
    procedure StopGraph;
    { Public declarations }
  end;

var
  Form1: TForm1;
  dsFilterGraph: IFilterGraph2;
  dsCaptureGraph: ICaptureGraphBuilder2;
  dsSource, dsSourceVideo, dsSourceAudio, dsVideoEncoder, dsAudioEncoder,
    dsEncryptor, dsRGB2YUV, dsFilewriter: IBaseFilter;
  dsMediaControl: IMediaControl;
  dsMediaEvent: IMediaEventEx;
  dsMediaSeeking: IMediaSeeking;

const
  SDKLicenseKey = '';

const
  MSG_GRAPH_EVENT = WM_USER + 12346;
  CLSID_LAVSplitterSource: TGUID = '{B98D13E7-55DB-4385-A33D-09FD1BA26338}';
  CLSID_LAVVideoDecoder: TGUID = '{EE30215D-164F-4A92-A4EB-9D4C13390F9F}';
  CLSID_LAVAudioDecoder: TGUID = '{E8E73B6B-4CB3-44A4-BE99-4F7BCB96E491}';

implementation

{$R *.dfm}

procedure TForm1.btSelectFileClick(Sender: TObject);
begin
  if OpenDialog1.Execute then
  begin
    edSourceFile.Text := OpenDialog1.FileName;
  end;
end;

function AddFilter(filterGraph: IFilterGraph2; guid: TGUID; name: WideString;
  var Filter: IBaseFilter): HRESULT;
var
  hr: HRESULT;
begin
  result := 0;

  try
    hr := CoCreateInstance(guid, nil, CLSCTX_INPROC_SERVER,
      IID_IBaseFilter, Filter);

    if hr <> S_OK then
      result := hr;

    if hr = S_OK then
    begin
      hr := filterGraph.AddFilter(Filter, StringToOleStr(name));
      result := hr;
    end;
  except
    result := -1;
  end;
end;

procedure TForm1.AddEncoders;
var
  hr: HRESULT;
  sink: IFileSinkFilter;
begin
  // encryptor
  hr := AddFilter(dsFilterGraph, CLSID_VFEncryptor, 'VisioForge Encryptor',
    dsEncryptor);
  if hr <> 0 then
    ShowMessage('Unable to add VisioForge Encryptor filter.');
  ConfigureEncryptor();

  dsEncryptor.QueryInterface(IID_IMediaSeeking, dsMediaSeeking);

  // video
  hr := AddFilter(dsFilterGraph, CLSID_VFH264EncoderV10,
    'VisioForge H264 Encoder', dsVideoEncoder);
  if hr <> 0 then
    ShowMessage('Unable to add VisioForge H264 Encoder filter.');

  hr := AddFilter(dsFilterGraph, CLSID_VFRGB2YUV, 'VisioForge RGB2YUV',
    dsRGB2YUV);
  if hr <> 0 then
    ShowMessage('Unable to add VisioForge RGB2YUV filter.');

  hr := dsCaptureGraph.RenderStream(nil, @MEDIATYPE_Video, dsSourceVideo,
    dsRGB2YUV, dsVideoEncoder);
  if hr <> 0 then
    ShowMessage('Unable to render video.');

  hr := dsCaptureGraph.RenderStream(nil, @MEDIATYPE_Video, dsVideoEncoder, nil,
    dsEncryptor);
  if hr <> 0 then
    ShowMessage('Unable to render video (2).');

  // audio
  hr := AddFilter(dsFilterGraph, CLSID_VFAACEncoderV10,
    'VisioForge AAC Encoder', dsAudioEncoder);
  if hr <> 0 then
    ShowMessage('Unable to add VisioForge AAC Encoder filter.');

  hr := dsCaptureGraph.RenderStream(nil, @MEDIATYPE_Audio, dsSourceAudio, nil,
    dsAudioEncoder);
  if hr <> 0 then
    ShowMessage('Unable to render audio.');

  hr := dsCaptureGraph.RenderStream(nil, @MEDIATYPE_Audio, dsAudioEncoder, nil,
    dsEncryptor);
  if hr <> 0 then
    ShowMessage('Unable to render audio (2).');

  // filewriter
  hr := AddFilter(dsFilterGraph, CLSID_FileWriter, 'Filewriter', dsFilewriter);
  if hr <> 0 then
    ShowMessage('Unable to add Filewriter filter.');

  dsFilewriter.QueryInterface(IID_IFileSinkFilter, sink);
  sink.SetFileName(StringToOleStr(edOutputFile.Text), nil);

  hr := dsCaptureGraph.RenderStream(nil, nil, dsEncryptor, nil, dsFilewriter);
  if hr <> 0 then
    ShowMessage('Unable to connect file writer.');

  dsEncryptor.QueryInterface(IID_IMediaSeeking, dsMediaSeeking);
end;

procedure TForm1.AddNoReencode;
var
  hr: HRESULT;
  sink: IFileSinkFilter;
  pins: TVFPinList;
  videoOutPin, audioOutPin, tmpPin: IPin;
  i: integer;
begin
  // encryptor
  hr := AddFilter(dsFilterGraph, CLSID_VFEncryptor, 'VisioForge Encryptor',
    dsEncryptor);
  if hr <> 0 then
    ShowMessage('Unable to add VisioForge Encryptor filter.');
  ConfigureEncryptor();

  dsEncryptor.QueryInterface(IID_IMediaSeeking, dsMediaSeeking);

  pins := TVFPinList.Create(dsSource);
  videoOutPin := nil;
  audioOutPin := nil;

  for i := 0 to pins.Count - 1 do
  begin
    if PinHaveThisType(pins[i], MEDIATYPE_Video) then
    begin
      videoOutPin := pins[i];
      continue;
    end;

    if PinHaveThisType(pins[i], MEDIATYPE_Audio) then
    begin
      audioOutPin := pins[i];
      continue;
    end;
  end;

  // video
  if (videoOutPin <> nil) then
  begin
    GetPin(dsEncryptor, PINDIR_INPUT, 1, tmpPin);

    hr := dsFilterGraph.ConnectDirect(videoOutPin, tmpPin, nil);
    if hr <> 0 then
      ShowMessage('Unable to render video.');

    videoOutPin := nil;
    tmpPin := nil;
  end;

  // audio
  if (audioOutPin <> nil) then
  begin
    GetPin(dsEncryptor, PINDIR_INPUT, 2, tmpPin);

    hr := dsFilterGraph.ConnectDirect(audioOutPin, tmpPin, nil);
    if hr <> 0 then
    begin
      // audio is not compatible and will be reencoded
      hr := AddFilter(dsFilterGraph, CLSID_VFAACEncoderV10,
        'VisioForge AAC Encoder', dsAudioEncoder);
      if hr <> 0 then
        ShowMessage('Unable to add VisioForge AAC Encoder filter.');

      hr := dsCaptureGraph.RenderStream(nil, @MEDIATYPE_Audio, dsSourceAudio,
        nil, dsAudioEncoder);
      if hr <> 0 then
        ShowMessage('Unable to render audio.');

      hr := dsCaptureGraph.RenderStream(nil, @MEDIATYPE_Audio, dsAudioEncoder,
        nil, dsEncryptor);
      if hr <> 0 then
        ShowMessage('Unable to render audio (2).');
    end;

    audioOutPin := nil;
    tmpPin := nil;
  end;

  // filewriter
  hr := AddFilter(dsFilterGraph, CLSID_FileWriter, 'Filewriter', dsFilewriter);
  if hr <> 0 then
    ShowMessage('Unable to add Filewriter filter.');

  dsFilewriter.QueryInterface(IID_IFileSinkFilter, sink);
  sink.SetFileName(StringToOleStr(edOutputFile.Text), nil);

  hr := dsCaptureGraph.RenderStream(nil, nil, dsEncryptor, nil, dsFilewriter);
  if hr <> 0 then
    ShowMessage('Unable to connect file writer.');

  dsEncryptor.QueryInterface(IID_IMediaSeeking, dsMediaSeeking);

  pins.Free;
end;

procedure TForm1.WndProc(var Message: TMessage);
var
  EventCode, Param1, Param2: integer;
begin
  if csDesigning in ComponentState then
  begin
    inherited;
    exit;
  end;

  if Message.Msg = MSG_GRAPH_EVENT then
  begin
    if dsMediaEvent <> nil then
    begin
      while (dsMediaEvent.GetEvent(EventCode, Param1, Param2, 0) = 0) do
      begin
        // Free event parameters to prevent memory leaks associated with
        // event parameter data.  While this application is not interested
        // in the received events, applications should always process them.

        case (EventCode) of
          EC_COMPLETE:
            begin
              dsMediaEvent.FreeEventParams(EventCode, Param1, Param2);
              inherited;
              OnComplete();
              exit;
            end;
        end;

        if (dsMediaEvent = nil) then
        begin
          exit;
        end;

        dsMediaEvent.FreeEventParams(EventCode, Param1, Param2);
      end;

    end;
  end;

  inherited;
end;

procedure TForm1.CreateGraph;
var
  hr: HRESULT;
begin
  CoInitialize(nil);

  // Start by making an empty timeline.  Add a media detector as well.
  hr := CoCreateInstance(CLSID_FilterGraph, nil, CLSCTX_INPROC_SERVER,
    IID_IFilterGraph2, dsFilterGraph);
  hr := CoCreateInstance(CLSID_CaptureGraphBuilder2, nil, CLSCTX_INPROC_SERVER,
    IID_ICaptureGraphBuilder2, dsCaptureGraph);
  dsCaptureGraph.SetFiltergraph(dsFilterGraph);

  hr := dsFilterGraph.QueryInterface(IID_IMediaControl, dsMediaControl);
  hr := dsFilterGraph.QueryInterface(IID_IMediaEventEx, dsMediaEvent);

  dsMediaEvent.SetNotifyWindow(self.Handle, MSG_GRAPH_EVENT, 0);
end;

procedure TForm1.ClearGraph;
begin
  dsFilterGraph.RemoveFilter(dsSource);
  dsSource := nil;

  dsFilterGraph.RemoveFilter(dsSourceVideo);
  dsSourceVideo := nil;

  dsFilterGraph.RemoveFilter(dsSourceAudio);
  dsSourceAudio := nil;

  dsFilterGraph.RemoveFilter(dsVideoEncoder);
  dsVideoEncoder := nil;

  dsFilterGraph.RemoveFilter(dsAudioEncoder);
  dsAudioEncoder := nil;

  dsFilterGraph.RemoveFilter(dsEncryptor);
  dsEncryptor := nil;

  dsFilterGraph.RemoveFilter(dsRGB2YUV);
  dsRGB2YUV := nil;

  dsFilterGraph.RemoveFilter(dsFilewriter);
  dsFilewriter := nil;

  dsCaptureGraph := nil;

  dsMediaControl := nil;
  dsMediaEvent := nil;
  dsMediaSeeking := nil;

  dsFilterGraph := nil;
end;

procedure TForm1.AddSource();
var
  hr: HRESULT;
  source: IFileSourceFilter;
begin
  if rbDecodeAuto.Checked then
  begin
    hr := dsFilterGraph.AddSourceFilter(StringToOleStr(edSourceFile.Text),
      'SRC', dsSource);
    if hr <> 0 then
      ShowMessage('Unable to add source file');
    dsSourceVideo := dsSource;
    dsSourceAudio := dsSource;
  end
  else if rbDecodeLAV.Checked then
  begin
    hr := AddFilter(dsFilterGraph, CLSID_LAVSplitterSource,
      'LAV Splitter Source', dsSource);
    if hr <> 0 then
      ShowMessage('Unable to add LAV Splitter Source');

    hr := dsSource.QueryInterface(IID_IFileSourceFilter, source);
    source.Load(StringToOleStr(edSourceFile.Text), nil);

    hr := AddFilter(dsFilterGraph, CLSID_LAVVideoDecoder, 'LAV Video Decoder',
      dsSourceVideo);

    hr := dsCaptureGraph.RenderStream(nil, nil, dsSource, nil, dsSourceVideo);

    hr := AddFilter(dsFilterGraph, CLSID_LAVAudioDecoder, 'LAV Audio Decoder',
      dsSourceAudio);
    hr := dsCaptureGraph.RenderStream(nil, nil, dsSource, nil, dsSourceAudio);
  end
  else
  begin
    hr := AddFilter(dsFilterGraph, CLSID_LAVSplitterSource,
      'LAV Splitter Source', dsSource);
    if hr <> 0 then
      ShowMessage('Unable to add LAV Splitter Source');

    hr := dsSource.QueryInterface(IID_IFileSourceFilter, source);
    source.Load(StringToOleStr(edSourceFile.Text), nil);

    dsSourceVideo := dsSource;
    dsSourceAudio := dsSource;
  end;
end;

procedure TForm1.btSaveOutputClick(Sender: TObject);
begin
  if SaveDialog1.Execute then
  begin
    edOutputFile.Text := SaveDialog1.FileName;
  end;
end;

procedure TForm1.btStartClick(Sender: TObject);
begin
  CreateGraph();
  AddSource();

  if rbNoReencoding.Checked then
  begin
    AddNoReencode();
  end
  else
  begin
    AddEncoders();
  end;

  StartGraph();

  Timer1.Enabled := true;
end;

procedure TForm1.btStopClick(Sender: TObject);
begin
  Timer1.Enabled := false;

  StopGraph();
  ClearGraph();
end;

procedure TForm1.ConfigureEncryptor;
var
  info: TFilterInfo;
  config: IVFCryptoConfig;
  password: Pointer;
  passwordLen: integer;
  filterName: String;

  reg: IVFRegister;
begin
  if SDKLicenseKey <> '' then
  begin
    dsEncryptor.QueryInterface(IID_IVFRegister, reg);
    reg.SetLicenseKey(StringToOleStr(SDKLicenseKey));

    reg := nil;
  end;

  dsEncryptor.QueryInterface(IID_IVFCryptoConfig, config);
  if config <> nil then
  begin
    password := System.StringToOleStr(edPassword.Text);
    passwordLen := Length(edPassword.Text) * 2;
    config.put_Password(password, passwordLen);

    config := nil;
  end;
end;

procedure TForm1.OnComplete;
begin
  Timer1.Enabled := false;

  StopGraph();
  ClearGraph();

  ProgressBar1.Position := 0;
  ShowMessage('Complete');
end;

procedure TForm1.StartGraph;
var
  pMediaFilter: IMediaFilter;
begin
  dsFilterGraph.QueryInterface(IID_IMediaFilter, pMediaFilter);
  pMediaFilter.SetSyncSource(nil);
  pMediaFilter := nil;

  dsMediaControl.Run;
end;

procedure TForm1.StopGraph;
begin
  dsMediaControl.Stop;
end;

procedure TForm1.Timer1Timer(Sender: TObject);
var
  dur, pos, temp: Int64;
begin
  if dsMediaSeeking <> nil then
  begin
    dsMediaSeeking.GetDuration(dur);
    dsMediaSeeking.GetPositions(pos, temp);

    ProgressBar1.Position := Round(pos * 100 / dur);
  end;
end;

end.
