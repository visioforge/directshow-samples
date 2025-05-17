object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'VLC Player'
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
  object lbTime: TLabel
    Left = 360
    Top = 107
    Width = 89
    Height = 15
    Caption = '00:00:00/00:00:00'
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
  object tbTimeline: TScrollBar
    Left = 8
    Top = 70
    Width = 441
    Height = 17
    PageSize = 0
    TabOrder = 2
    OnChange = tbTimelineChange
  end
  object btStart: TButton
    Left = 8
    Top = 103
    Width = 75
    Height = 25
    Caption = 'Start'
    TabOrder = 3
    OnClick = btStartClick
  end
  object btPause: TButton
    Left = 170
    Top = 103
    Width = 75
    Height = 25
    Caption = 'Pause'
    TabOrder = 4
    OnClick = btPauseClick
  end
  object btStop: TButton
    Left = 251
    Top = 103
    Width = 75
    Height = 25
    Caption = 'Stop'
    TabOrder = 5
    OnClick = btStopClick
  end
  object btResume: TButton
    Left = 89
    Top = 103
    Width = 75
    Height = 25
    Caption = 'Resume'
    TabOrder = 6
    OnClick = btResumeClick
  end
  object pnScreen: TPanel
    Left = 464
    Top = 8
    Width = 657
    Height = 497
    Caption = 'pnScreen'
    Color = clBlack
    ParentBackground = False
    TabOrder = 7
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
