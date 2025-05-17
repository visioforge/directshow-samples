program Virtual_Camera_Demo;

uses
  Vcl.Forms,
  Unit1 in 'Unit1.pas' {Form1},
  VCClasses in 'VCClasses.pas',
  VCFiltersAPI in 'VCFiltersAPI.pas',
  VFVCDirect3D9 in 'VFVCDirect3D9.pas',
  VFVCDirectDraw in 'VFVCDirectDraw.pas',
  VFVCDirectShow9 in 'VFVCDirectShow9.pas',
  VFVCDirectSound in 'VFVCDirectSound.pas',
  VFVCDXTypes in 'VFVCDXTypes.pas',
  VideoCaptureTypes in 'VideoCaptureTypes.pas',
  VirtualCameraIntf in 'VirtualCameraIntf.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TForm1, Form1);
  Application.Run;

end.
