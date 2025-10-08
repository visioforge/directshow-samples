program EncryptorEncoderSDKDemo;

uses
  Vcl.Forms,
  Unit1 in 'Unit1.pas' {Form1},
  encryptor_intf in 'encryptor_intf.pas',
  ENCDirect3D9 in 'ENCDirect3D9.pas',
  ENCDirectDraw in 'ENCDirectDraw.pas',
  ENCDirectShow9 in 'ENCDirectShow9.pas',
  ENCDirectSound in 'ENCDirectSound.pas',
  ENCDXTypes in 'ENCDXTypes.pas',
  ENCGDIPAPI in 'ENCGDIPAPI.pas',
  ENCGDIPOBJ in 'ENCGDIPOBJ.pas',
  ENCWMF9 in 'ENCWMF9.pas',
  helpers in 'helpers.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
