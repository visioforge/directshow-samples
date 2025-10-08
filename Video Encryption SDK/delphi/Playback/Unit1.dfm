object Form1: TForm1
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu, biMinimize]
  Caption = 'Video Encryptor SDK Demo'
  ClientHeight = 439
  ClientWidth = 500
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 45
    Height = 13
    Caption = 'File name'
  end
  object Label2: TLabel
    Left = 8
    Top = 410
    Width = 46
    Height = 13
    Caption = 'Password'
  end
  object edFilename: TEdit
    Left = 8
    Top = 21
    Width = 450
    Height = 21
    TabOrder = 0
    Text = 'c:\vf\encrypted!.enc'
  end
  object btSelectFile: TButton
    Left = 464
    Top = 19
    Width = 25
    Height = 25
    Caption = '...'
    TabOrder = 1
    OnClick = btSelectFileClick
  end
  object btStart: TButton
    Left = 335
    Top = 406
    Width = 75
    Height = 25
    Caption = 'Start'
    TabOrder = 2
    OnClick = btStartClick
  end
  object btStop: TButton
    Left = 416
    Top = 407
    Width = 75
    Height = 25
    Caption = 'Stop'
    TabOrder = 3
    OnClick = btStopClick
  end
  object edPassword: TEdit
    Left = 60
    Top = 407
    Width = 53
    Height = 21
    TabOrder = 4
    Text = '100'
  end
  object pnVideoView: TPanel
    Left = 8
    Top = 50
    Width = 481
    Height = 321
    Caption = 'pnVideoView'
    ParentBackground = False
    TabOrder = 5
  end
  object OpenDialog1: TOpenDialog
    Left = 56
    Top = 72
  end
end
