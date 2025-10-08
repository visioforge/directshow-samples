unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, StrUtils, ActiveX, ENCDSPack, ENCDirectShow9, encryptor_intf,
  Vcl.ExtCtrls, DirectShowPlayer;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    edFilename: TEdit;
    btSelectFile: TButton;
    btStart: TButton;
    btStop: TButton;
    OpenDialog1: TOpenDialog;
    Label2: TLabel;
    edPassword: TEdit;
    pnVideoView: TPanel;
    procedure btSelectFileClick(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure btStopClick(Sender: TObject);
  private
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  Player: TDirectShowPlayer;

implementation

{$R *.dfm}

procedure TForm1.btSelectFileClick(Sender: TObject);
begin
  if OpenDialog1.Execute then
  begin
    edFilename.Text := OpenDialog1.FileName;
  end;
end;

procedure TForm1.btStartClick(Sender: TObject);
begin
  Player := TDirectShowPlayer.Create(pnVideoView.Handle, edFilename.Text,
    edPassword.Text);
  Player.Play;
end;

procedure TForm1.btStopClick(Sender: TObject);
begin
  Player.Stop;
end;

end.
