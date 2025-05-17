object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'Virtual Camera SDK Demo'
  ClientHeight = 444
  ClientWidth = 865
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 40
    Top = 8
    Width = 302
    Height = 19
    Caption = 'Video source (streaming to virtual camera)'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = 0
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 559
    Top = 8
    Width = 153
    Height = 19
    Caption = 'Virtual camera source'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = 0
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label4: TLabel
    Left = 24
    Top = 114
    Width = 61
    Height = 13
    Caption = 'Video source'
  end
  object Label7: TLabel
    Left = 24
    Top = 162
    Width = 62
    Height = 13
    Caption = 'Audio source'
  end
  object rbFile: TRadioButton
    Left = 8
    Top = 41
    Width = 113
    Height = 17
    Caption = 'Video file'
    Checked = True
    TabOrder = 0
    TabStop = True
  end
  object rbCamera: TRadioButton
    Left = 8
    Top = 91
    Width = 137
    Height = 17
    Caption = 'Video capture device'
    TabOrder = 1
  end
  object Panel1: TPanel
    Left = 367
    Top = 8
    Width = 9
    Height = 418
    TabOrder = 2
  end
  object edSourceFile: TEdit
    Left = 24
    Top = 64
    Width = 306
    Height = 21
    TabOrder = 3
    Text = 'c:\samples\!video.mp4'
  end
  object btOpenFile: TButton
    Left = 336
    Top = 62
    Width = 25
    Height = 25
    Caption = '...'
    TabOrder = 4
  end
  object cbVideoCaptureSource: TComboBox
    Left = 24
    Top = 133
    Width = 337
    Height = 21
    Style = csDropDownList
    TabOrder = 5
  end
  object cbAudioCaptureSource: TComboBox
    Left = 24
    Top = 181
    Width = 337
    Height = 21
    Style = csDropDownList
    TabOrder = 6
  end
  object btSourceStop: TButton
    Left = 305
    Top = 399
    Width = 56
    Height = 25
    Caption = 'Stop'
    TabOrder = 7
    OnClick = btSourceStopClick
  end
  object btSourceStart: TButton
    Left = 243
    Top = 399
    Width = 56
    Height = 25
    Caption = 'Start'
    TabOrder = 8
    OnClick = btSourceStartClick
  end
  object pnScreen1: TPanel
    Left = 382
    Top = 31
    Width = 475
    Height = 352
    Color = clBlack
    ParentBackground = False
    TabOrder = 9
  end
  object btCameraStartPreview1: TButton
    Left = 739
    Top = 399
    Width = 56
    Height = 25
    Caption = 'Start'
    TabOrder = 10
    OnClick = btCameraStartPreview1Click
  end
  object btCameraStop1: TButton
    Left = 801
    Top = 399
    Width = 56
    Height = 25
    Caption = 'Stop'
    TabOrder = 11
    OnClick = btCameraStop1Click
  end
  object rbScreen: TRadioButton
    Left = 8
    Top = 208
    Width = 97
    Height = 17
    Caption = 'Screen'
    TabOrder = 12
  end
  object rbRandom: TRadioButton
    Left = 8
    Top = 360
    Width = 98
    Height = 17
    Caption = 'Random gray'
    TabOrder = 13
  end
  object GroupBox1: TGroupBox
    Left = 24
    Top = 231
    Width = 337
    Height = 122
    TabOrder = 14
    object Label32: TLabel
      Left = 32
      Top = 63
      Width = 22
      Height = 13
      Caption = 'Left'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label38: TLabel
      Left = 32
      Top = 95
      Width = 25
      Height = 13
      Caption = 'Right'
    end
    object Label39: TLabel
      Left = 124
      Top = 95
      Width = 34
      Height = 13
      Caption = 'Bottom'
    end
    object Label37: TLabel
      Left = 124
      Top = 63
      Width = 21
      Height = 13
      Caption = 'Top'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object rbScreenFullScreen: TRadioButton
      Left = 8
      Top = 8
      Width = 113
      Height = 17
      Caption = 'Full screen'
      Checked = True
      TabOrder = 0
      TabStop = True
    end
    object rbScreenCustom: TRadioButton
      Left = 8
      Top = 37
      Width = 113
      Height = 17
      Caption = 'Custom area'
      TabOrder = 1
    end
    object edScreenLeft: TEdit
      Left = 64
      Top = 60
      Width = 41
      Height = 21
      TabOrder = 2
      Text = '0'
    end
    object edScreenRight: TEdit
      Left = 64
      Top = 92
      Width = 41
      Height = 21
      TabOrder = 3
      Text = '640'
    end
    object edScreenBottom: TEdit
      Left = 167
      Top = 92
      Width = 41
      Height = 21
      TabOrder = 4
      Text = '480'
    end
    object edScreenTop: TEdit
      Left = 167
      Top = 60
      Width = 41
      Height = 21
      TabOrder = 5
      Text = '0'
    end
  end
  object rbControlHandle: TRadioButton
    Left = 112
    Top = 360
    Width = 249
    Height = 17
    Caption = 'Window / control (by handle, can be set in code)'
    TabOrder = 15
  end
end
