object Form1: TForm1
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu, biMinimize]
  Caption = 'Video Encryptor SDK Demo'
  ClientHeight = 205
  ClientWidth = 512
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
    Width = 50
    Height = 13
    Caption = 'Source file'
  end
  object Label2: TLabel
    Left = 8
    Top = 162
    Width = 46
    Height = 13
    Caption = 'Password'
  end
  object Label3: TLabel
    Left = 8
    Top = 48
    Width = 51
    Height = 13
    Caption = 'Output file'
  end
  object edSourceFile: TEdit
    Left = 8
    Top = 21
    Width = 450
    Height = 21
    TabOrder = 0
    Text = 'c:\Samples\!video.avi'
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
    Left = 333
    Top = 159
    Width = 75
    Height = 25
    Caption = 'Start'
    TabOrder = 2
    OnClick = btStartClick
  end
  object btStop: TButton
    Left = 414
    Top = 159
    Width = 75
    Height = 25
    Caption = 'Stop'
    TabOrder = 3
    OnClick = btStopClick
  end
  object edPassword: TEdit
    Left = 60
    Top = 159
    Width = 85
    Height = 21
    TabOrder = 4
    Text = '100'
  end
  object edOutputFile: TEdit
    Left = 8
    Top = 64
    Width = 450
    Height = 21
    TabOrder = 5
    Text = 'c:\vf\encrypted!.enc'
  end
  object btSaveOutput: TButton
    Left = 464
    Top = 62
    Width = 25
    Height = 25
    Caption = '...'
    TabOrder = 6
    OnClick = btSaveOutputClick
  end
  object ProgressBar1: TProgressBar
    Left = 151
    Top = 162
    Width = 176
    Height = 17
    TabOrder = 7
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 91
    Width = 481
    Height = 62
    Caption = 'Decoding engine'
    TabOrder = 8
    object rbDecodeAuto: TRadioButton
      Left = 16
      Top = 24
      Width = 57
      Height = 17
      Caption = 'Auto'
      TabOrder = 0
    end
    object rbDecodeLAV: TRadioButton
      Left = 95
      Top = 24
      Width = 42
      Height = 17
      Caption = 'LAV'
      Checked = True
      TabOrder = 1
      TabStop = True
    end
    object rbNoReencoding: TRadioButton
      Left = 168
      Top = 24
      Width = 201
      Height = 17
      BiDiMode = bdLeftToRight
      Caption = 'No reencoding (source is H264/AAC)'
      ParentBiDiMode = False
      TabOrder = 2
    end
  end
  object OpenDialog1: TOpenDialog
    Left = 336
    Top = 8
  end
  object SaveDialog1: TSaveDialog
    Left = 368
    Top = 8
  end
  object Timer1: TTimer
    Enabled = False
    Interval = 500
    OnTimer = Timer1Timer
    Left = 408
    Top = 8
  end
end
