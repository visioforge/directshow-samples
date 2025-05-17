object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'FFMPEG Player'
  ClientHeight = 518
  ClientWidth = 1129
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -12
  Font.Name = 'Segoe UI'
  Font.Style = []
  TextHeight = 15
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 87
    Height = 15
    Caption = 'URL or file name'
  end
  object Label2: TLabel
    Left = 8
    Top = 66
    Width = 134
    Height = 15
    Caption = 'Connection timeout (ms)'
  end
  object Label3: TLabel
    Left = 203
    Top = 66
    Width = 83
    Height = 15
    Caption = 'Buffering mode'
  end
  object lbTime: TLabel
    Left = 360
    Top = 267
    Width = 89
    Height = 15
    Caption = '00:00:00/00:00:00'
  end
  object Label5: TLabel
    Left = 8
    Top = 107
    Width = 32
    Height = 15
    Caption = 'Speed'
  end
  object Label4: TLabel
    Left = 8
    Top = 147
    Width = 69
    Height = 15
    Caption = 'Video stream'
  end
  object Label6: TLabel
    Left = 8
    Top = 176
    Width = 71
    Height = 15
    Caption = 'Audio stream'
  end
  object edFilename: TEdit
    Left = 8
    Top = 29
    Width = 409
    Height = 23
    TabOrder = 0
    Text = 'c:\Samples\!video.mp4'
  end
  object btOpenFile: TButton
    Left = 423
    Top = 28
    Width = 26
    Height = 25
    Caption = '...'
    TabOrder = 1
    OnClick = btOpenFileClick
  end
  object edConnectionTimeOut: TEdit
    Left = 148
    Top = 63
    Width = 49
    Height = 23
    TabOrder = 2
    Text = '10000'
  end
  object cbBufferingMode: TComboBox
    Left = 292
    Top = 63
    Width = 73
    Height = 23
    Style = csDropDownList
    ItemIndex = 0
    TabOrder = 3
    Text = 'Auto'
    Items.Strings = (
      'Auto'
      'On'
      'Off')
  end
  object cbUseGPU: TCheckBox
    Left = 371
    Top = 66
    Width = 78
    Height = 17
    Caption = 'Use GPU'
    TabOrder = 4
  end
  object tbTimeline: TScrollBar
    Left = 8
    Top = 230
    Width = 441
    Height = 17
    PageSize = 0
    TabOrder = 5
    OnChange = tbTimelineChange
  end
  object btStart: TButton
    Left = 8
    Top = 263
    Width = 75
    Height = 25
    Caption = 'Start'
    TabOrder = 6
    OnClick = btStartClick
  end
  object btPause: TButton
    Left = 170
    Top = 263
    Width = 75
    Height = 25
    Caption = 'Pause'
    TabOrder = 7
    OnClick = btPauseClick
  end
  object btStop: TButton
    Left = 251
    Top = 263
    Width = 75
    Height = 25
    Caption = 'Stop'
    TabOrder = 8
    OnClick = btStopClick
  end
  object btResume: TButton
    Left = 89
    Top = 263
    Width = 75
    Height = 25
    Caption = 'Resume'
    TabOrder = 9
    OnClick = btResumeClick
  end
  object tbSpeed: TScrollBar
    Left = 100
    Top = 105
    Width = 133
    Height = 17
    PageSize = 0
    TabOrder = 10
    OnChange = tbSpeedChange
  end
  object cbVideoStream: TComboBox
    Left = 100
    Top = 144
    Width = 349
    Height = 23
    Style = csDropDownList
    TabOrder = 11
    OnChange = cbVideoStreamChange
  end
  object cbAudioStream: TComboBox
    Left = 100
    Top = 173
    Width = 349
    Height = 23
    Style = csDropDownList
    TabOrder = 12
    OnChange = cbAudioStreamChange
  end
  object pnScreen: TPanel
    Left = 464
    Top = 8
    Width = 657
    Height = 497
    Caption = 'pnScreen'
    Color = clBlack
    ParentBackground = False
    TabOrder = 13
  end
  object tmProgress: TTimer
    Enabled = False
    OnTimer = tmProgressTimer
    Left = 376
    Top = 416
  end
  object FileOpenDialog1: TFileOpenDialog
    FavoriteLinks = <>
    FileTypes = <>
    Options = []
    Left = 336
    Top = 416
  end
end
