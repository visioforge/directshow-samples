unit VideoCaptureTypes;

interface

uses
  windows,
  classes,
  graphics,
  VFVCDirectDraw;

type
  /// <summary>
  /// Error event.
  /// </summary>
  /// <param name="ErrorText">
  /// Error text.
  /// </param>
  TVFErrorEvent = procedure(ErrorText: WideString) of object;

  /// <summary>
  /// Filter event.
  /// </summary>
  /// <param name="Name">Filter name.</param>
  /// <param name="Intf">Filter interface.</param>
  TVFFilterEvent = procedure(Name: WideString; Intf: IUnknown) of object;

  /// <summary>
  /// VU meter event image.
  /// </summary>
  /// <param name="Frame">Frame.</param>
  TVFVUMeterEventImage = procedure(Frame: TBitmap) of object;

  /// <summary>
  /// VU meter event image.
  /// </summary>
  /// <param name="Frame">Frame.</param>
  TVFVUMeterEventImageH = procedure(Frame: HBitmap) of object;

  /// <summary>
  /// VU meter event image.
  /// </summary>
  /// <param name="Frame">Frame.</param>
  TVFVUMeterEventImageI = procedure(Frame: Integer) of object;

  /// <summary>
  /// VU meter event.
  /// </summary>
  /// <param name="Values">Values.</param>
  TVFVUMeterEvent = procedure(Values: WideString) of object;

  /// <summary>
  /// Frame event.
  /// </summary>
  /// type:
  /// <param name="Frame">Frame.</param>
  /// <param name="StartTime">Start time.</param>
  /// <param name="StopTime">Stop time.</param>
  /// <param name="Updated">Set true to make changes in video frame.</param>
  TVFFrameEvent = procedure(var Frame: TBitmap; StartTime, StopTime: Integer;
    out Updated: Boolean) of object;

  /// <summary>
  /// Frame event.
  /// </summary>
  /// type:
  /// <param name="Frame">Frame.</param>
  /// <param name="StartTime">Start time.</param>
  /// <param name="StopTime">Stop time.</param>
  TVFFrameEventH = procedure(Frame: HBitmap; StartTime, StopTime: Integer)
    of object;

  /// <summary>
  /// Frame event.
  /// </summary>
  /// type:
  /// <param name="Frame">Frame.</param>
  /// <param name="StartTime">Start time.</param>
  /// <param name="StopTime">Stop time.</param>
  TVFFrameEventI = procedure(Frame: Integer; StartTime, StopTime: Integer)
    of object;

  /// <summary>
  /// Audio frame event.
  /// </summary>
  /// <param name="Frame">Frame buffer.</param>
  /// <param name="FrameSize">Frame buffer size.</param>
  /// <param name="SampleRate">Sample rate.</param>
  /// <param name="Channels">Channels count.</param>
  /// <param name="BPS">BPS.</param>
  /// <param name="StartTime">Start time.</param>
  /// <param name="StopTime">Stop time.</param>
  TVFAudioFrameEvent = procedure(Frame: PByte; FrameSize, SampleRate, Channels,
    BPS, StartTime, StopTime: Integer) of object;

  /// <summary>
  /// On changed frame event .
  /// </summary>
  /// type:
  /// <param name="HFrame">Frame.</param>
  /// <param name="HDiff">Difference frame.</param>
  /// <param name="DiffTop">Difference image top.</param>
  /// <param name="DiffLeft">Difference image left.</param>
  /// <param name="DiffHeight">Difference image height.</param>
  /// <param name="DiffWidth">Difference image width.</param>
  /// <param name="FrameHeight">Frame height.</param>
  /// <param name="FrameWidth">Frame width.</param>
  /// <param name="DiffPercent">Difference %.</param>
  /// <param name="ChangedPerc">Changed %.</param>
  TVFChangedFrameEventI = procedure(HFrame: Integer; HDiff: Integer;
    DiffTop, DiffLeft, DiffHeight, DiffWidth, FrameHeight, FrameWidth: Integer;
    DiffPercent: double; ChangedPerc: double) of object;

  /// <summary>
  /// On changed frame event .
  /// </summary>
  /// type:
  /// <param name="HFrame">Frame.</param>
  /// <param name="HDiff">Difference frame.</param>
  /// <param name="DiffTop">Difference image top.</param>
  /// <param name="DiffLeft">Difference image left.</param>
  /// <param name="DiffHeight">Difference image height.</param>
  /// <param name="DiffWidth">Difference image width.</param>
  /// <param name="FrameHeight">Frame height.</param>
  /// <param name="FrameWidth">Frame width.</param>
  /// <param name="DiffPercent">Difference %.</param>
  /// <param name="ChangedPerc">Changed %.</param>
  TVFChangedFrameEvent = procedure(HDiff, HFrame: HBitmap;
    DiffTop, DiffLeft, DiffHeight, DiffWidth, FrameHeight, FrameWidth: Integer;
    DiffPercent, ChangedPerc: double) of object;

  /// <summary>
  /// Start event.
  /// </summary>
  TVFStartEvent = procedure of object;

  /// <summary>
  /// Stop event.
  /// </summary>
  TVFStopEvent = procedure() of object;

  /// <summary>
  /// Motion event.
  /// </summary>
  /// <param name="Level">Level.</param>
  /// <param name="Matrix">Matrix.</param>
  TVFMotionEvent = procedure(Level: Integer; Matrix: WideString) of object;

  /// <summary>
  /// TV tuner tune channels event.
  /// </summary>
  /// <param name="SignalPresent">Signal present flag.</param>
  /// <param name="Channel">Channel.</param>
  /// <param name="Frequency">Frequency.</param>
  /// <param name="Progress">Progress.</param>
  TVFTVTunerTuneChannelsEvent = procedure(SignalPresent: Boolean;
    Channel: Integer; Frequency: Integer; Progress: Integer) of object;

  /// <summary>
  /// DV device event.
  /// </summary>
  /// <param name="EventCode">Event code.</param>
  TVFDVDeviceEvent = procedure(EventCode: Integer) of object;

  /// <summary>
  /// Mouse left down event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseLeftDownEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse left up event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseLeftUpEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse left double click event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseLeftDoubleClickEvent = procedure(X, Y: Integer;
    Ctrl, Shift, Alt: Boolean) of object;

  /// <summary>
  /// Mouse right down event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseRightDownEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse right up event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseRightUpEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse right double click event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseRightDoubleClickEvent = procedure(X, Y: Integer;
    Ctrl, Shift, Alt: Boolean) of object;

  /// <summary>
  /// Mouse middle down event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseMiddleDownEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse middle up event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseMiddleUpEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse middle double click event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseMiddleDoubleClickEvent = procedure(X, Y: Integer;
    Ctrl, Shift, Alt: Boolean) of object;

  /// <summary>
  /// Mouse X1 down event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseX1DownEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse X1 up event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseX1UpEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse X1 double click event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseX1DoubleClickEvent = procedure(X, Y: Integer;
    Ctrl, Shift, Alt: Boolean) of object;

  /// <summary>
  /// Mouse X2 down event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseX2DownEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse X2 up event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseX2UpEvent = procedure(X, Y: Integer; Ctrl, Shift, Alt: Boolean)
    of object;

  /// <summary>
  /// Mouse X2 double click event.
  /// </summary>
  /// <param name="X">X.</param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  TVFMouseX2DoubleClickEvent = procedure(X, Y: Integer;
    Ctrl, Shift, Alt: Boolean) of object;

  /// <summary>
  /// Mouse wheel up event.
  /// </summary>
  /// <param name="X">X. </param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  /// <param name="MouseLeft">Mouse left button pressed.</param>
  /// <param name="MouseRight">Mouse right button pressed.</param>
  /// <param name="MouseMiddle">Mouse middle button pressed.</param>
  /// <param name="MouseX1">Mouse X1 button pressed.</param>
  /// <param name="MouseX2">Mouse X2 button pressed.</param>
  /// <param name="Value">Value.</param>
  TVFMouseWheelUpEvent = procedure(X, Y: Integer;
    Ctrl, Shift, Alt, MouseLeft, MouseRight, MouseMiddle, MouseX1,
    MouseX2: Boolean; Value: Integer) of object;

  /// <summary>
  /// Mouse wheel down event.
  /// </summary>
  /// <param name="X">X. </param>
  /// <param name="Y">Y.</param>
  /// <param name="Ctrl">Ctrl pressed.</param>
  /// <param name="Shift">Shift pressed.</param>
  /// <param name="Alt">Alt pressed.</param>
  /// <param name="MouseLeft">Mouse left button pressed.</param>
  /// <param name="MouseRight">Mouse right button pressed.</param>
  /// <param name="MouseMiddle">Mouse middle button pressed.</param>
  /// <param name="MouseX1">Mouse X1 button pressed.</param>
  /// <param name="MouseX2">Mouse X2 button pressed.</param>
  /// <param name="Value">Value.</param>
  TVFMouseWheelDownEvent = procedure(X, Y: Integer;
    Ctrl, Shift, Alt, MouseLeft, MouseRight, MouseMiddle, MouseX1,
    MouseX2: Boolean; Value: Integer) of object;

  /// <summary>
  /// Key up event.
  /// </summary>
  /// <param name="Key">Key.</param>
  /// <param name="PressedCount">Pressed count.</param>
  TVFKeyUp = procedure(Key: AnsiChar; PressedCount: Integer) of object;

  /// <summary>
  /// Key down event.
  /// </summary>
  /// <param name="Key">Key.</param>
  /// <param name="PressedCount">Pressed count.</param>
  TVFKeyDown = procedure(Key: AnsiChar; PressedCount: Integer) of object;

  /// <summary>
  /// LAME channels mode.
  /// </summary>
  TVFLameChannelsMode = (
    /// <summary>
    /// Standard stereo.
    /// </summary>
    CH_Standard_Stereo,
    /// <summary>
    /// Joint stereo.
    /// </summary>
    CH_Joint_Stereo,
    /// <summary>
    /// Dual stereo.
    /// </summary>
    CH_Dual_Stereo,
    /// <summary>
    /// Mono.
    /// </summary>
    CH_Mono);

  /// <summary>
  /// Interleaving.
  /// </summary>
  TVFInterleaving = (
    /// <summary>
    /// None.
    /// </summary>
    Int_None,
    /// <summary>
    /// Capture.
    /// </summary>
    Int_Capture,
    /// <summary>
    /// Full.
    /// </summary>
    Int_Full,
    /// <summary>
    /// None (Buffered).
    /// </summary>
    Int_None_Buffered);

  /// <summary>
  /// DV video format.
  /// </summary>
  TVFDVVideoFormat = (
    /// <summary>
    /// PAL.
    /// </summary>
    DVF_PAL,
    /// <summary>
    /// NTSC.
    /// </summary>
    DVF_NTSC);

  /// <summary>
  /// DV video resolution.
  /// </summary>
  TVFDVVideoResolution = (
    /// <summary>
    /// Full.
    /// </summary>
    DVR_FULL,
    /// <summary>
    /// Half.
    /// </summary>
    DVR_HALF,
    /// <summary>
    /// Quarter.
    /// </summary>
    DVR_QUARTER,
    /// <summary>
    /// DC.
    /// </summary>
    DVR_DC);

  /// <summary>
  /// Output format.
  /// </summary>
  TVFOutputFormat = (
    /// <summary>
    /// AVI.
    /// </summary>
    Format_AVI,
    /// <summary>
    /// WMV.
    /// </summary>
    Format_WMV,
    /// <summary>
    /// DV.
    /// </summary>
    Format_DV,
    /// <summary>
    /// MKV.
    /// </summary>
    Format_MKV,
    /// <summary>
    /// PCM / ACM.
    /// </summary>
    Format_PCM_ACM,
    /// <summary>
    /// WMA.
    /// </summary>
    Format_WMA,
    /// <summary>
    /// LAME.
    /// </summary>
    Format_LAME,
    /// <summary>
    /// Custom.
    /// </summary>
    Format_Custom,
    /// <summary>
    /// DirectStream DV.
    /// </summary>
    Format_DirectStream_DV,
    /// <summary>
    /// DirectStream MPEG.
    /// </summary>
    Format_DirectStream_MPEG,
    /// <summary>
    /// FFMPEG.
    /// </summary>
    Format_FFMPEG,
    /// <summary>
    /// WebM.
    /// </summary>
    Format_WebM,

    /// <summary>
    /// MP4 H264 / AAC.
    /// </summary>
    Format_MP4,

    /// <summary>
    /// Encrypted video.
    /// </summary>
    Format_Encrypted);

  /// <summary>
  /// Status.
  /// </summary>
  TVFStatus = (
    /// <summary>
    /// Work.
    /// </summary>
    Status_WORK,
    /// <summary>
    /// Free.
    /// </summary>
    Status_FREE);

  /// <summary>
  /// Device type.
  /// </summary>
  TVFDeviceType = (
    /// <summary>
    /// Standard.
    /// </summary>
    DT_Standard,
    /// <summary>
    /// Standard + MPEG-2.
    /// </summary>
    DT_STD_AND_MPEG2,
    /// <summary>
    /// MPEG-2.
    /// </summary>
    DT_MPEG2_ONLY,
    /// <summary>
    /// DV.
    /// </summary>
    DT_DV,
    /// <summary>
    /// MPEG-2 HDV.
    /// </summary>
    DT_MPEG2_HDV,
    /// <summary>
    /// Unknown.
    /// </summary>
    DT_Unknown);

  /// <summary>
  /// VU meter style.
  /// </summary>
  TVFVUMeterStyle = (
    /// <summary>
    /// Lines - horizontal.
    /// </summary>
    VU_LinesHorizontal,
    /// <summary>
    /// Lines- vertical.
    /// </summary>
    VU_LinesVertical);

  /// <summary>
  /// Mode.
  /// </summary>
  TVFMode = (
    /// <summary>
    /// Video capture.
    /// </summary>
    Mode_Video_Capture,
    /// <summary>
    /// Video preview.
    /// </summary>
    Mode_Video_Preview,
    /// <summary>
    /// Audio capture.
    /// </summary>
    Mode_Audio_Capture,
    /// <summary>
    /// Audio preview.
    /// </summary>
    Mode_Audio_Preview,
    /// <summary>
    /// Screen capture.
    /// </summary>
    Mode_Screen_Capture,
    /// <summary>
    /// Screen preview.
    /// </summary>
    Mode_Screen_Preview,
    /// <summary>
    /// IP capture.
    /// </summary>
    Mode_IP_Capture,
    /// <summary>
    /// IP preview.
    /// </summary>
    Mode_IP_Preview);

  /// <summary>
  /// Video adjustment.
  /// </summary>
  TVFVideoCapAdjust = (
    /// <summary>
    /// Brightess.
    /// </summary>
    adj_Brightness,
    /// <summary>
    /// Contast.
    /// </summary>
    adj_Contrast,
    /// <summary>
    /// Hue.
    /// </summary>
    adj_Hue,
    /// <summary>
    /// Saturation.
    /// </summary>
    adj_Saturation,
    /// <summary>
    /// Sharpness.
    /// </summary>
    adj_Sharpness,
    /// <summary>
    /// Gamma.
    /// </summary>
    adj_Gamma,
    /// <summary>
    /// Color enable.
    /// </summary>
    adj_ColorEnable,
    /// <summary>
    /// White balance.
    /// </summary>
    adj_WhiteBalance,
    /// <summary>
    /// Backlight compensations.
    /// </summary>
    adj_BacklightCompensation,
    /// <summary>
    /// Gain.
    /// </summary>
    adj_Gain);

  /// <summary>
  /// Video renderer.
  /// </summary>
  TVFVideoRenderer = (
    /// <summary>
    /// Video renderer.
    /// </summary>
    VR_VideoRenderer,
    /// <summary>
    /// VMR-9.
    /// </summary>
    VR_VMR9,
    /// <summary>
    /// EVR.
    /// </summary>
    VR_EVR,
    /// <summary>
    /// None.
    /// </summary>
    VR_NONE);

  /// <summary>
  /// DV command.
  /// </summary>
  TVFDVCommand = (
    /// <summary>
    /// Play.
    /// </summary>
    DV_PLAY,
    /// <summary>
    /// Stop.
    /// </summary>
    DV_STOP,
    /// <summary>
    /// Pause.
    /// </summary>
    DV_PAUSE,
    /// <summary>
    /// Rewind.
    /// </summary>
    DV_REW,
    /// <summary>
    /// Fast forward.
    /// </summary>
    DV_FF,
    /// <summary>
    /// Step forward.
    /// </summary>
    DV_STEP_FW,
    /// <summary>
    /// Step reverse.
    /// </summary>
    DV_STEP_REV,
    /// <summary>
    /// Fastest forward.
    /// </summary>
    DV_FASTEST_FWD,
    /// <summary>
    /// Slowest forward.
    /// </summary>
    DV_SLOWEST_FWD,
    /// <summary>
    /// Fastest reverse.
    /// </summary>
    DV_FASTEST_REV,
    /// <summary>
    /// Slowest reverse.
    /// </summary>
    DV_SLOWEST_REV,
    /// <summary>
    /// Record.
    /// </summary>
    DV_RECORD,
    /// <summary>
    /// Record pause.
    /// </summary>
    DV_RECORD_PAUSE,
    /// <summary>
    /// Play slow forward 6.
    /// </summary>
    DV_PLAY_SLOW_FWD_6,
    /// <summary>
    /// Play slow forward 5.
    /// </summary>
    DV_PLAY_SLOW_FWD_5,
    /// <summary>
    /// Play slow forward 4.
    /// </summary>
    DV_PLAY_SLOW_FWD_4,
    /// <summary>
    /// Play slow forward 3.
    /// </summary>
    DV_PLAY_SLOW_FWD_3,
    /// <summary>
    /// Play slow forward 2.
    /// </summary>
    DV_PLAY_SLOW_FWD_2,
    /// <summary>
    /// Play slow forward 1.
    /// </summary>
    DV_PLAY_SLOW_FWD_1,
    /// <summary>
    /// Play fast forward 1.
    /// </summary>
    DV_PLAY_FAST_FWD_1,
    /// <summary>
    /// Play fast forward 2.
    /// </summary>
    DV_PLAY_FAST_FWD_2,
    /// <summary>
    /// Play fast forward 3.
    /// </summary>
    DV_PLAY_FAST_FWD_3,
    /// <summary>
    /// Play fast forward 4.
    /// </summary>
    DV_PLAY_FAST_FWD_4,
    /// <summary>
    /// Play fast forward 5.
    /// </summary>
    DV_PLAY_FAST_FWD_5,
    /// <summary>
    /// Play fast forward 6.
    /// </summary>
    DV_PLAY_FAST_FWD_6,
    /// <summary>
    /// Play slow reverse 6.
    /// </summary>
    DV_PLAY_SLOW_REV_6,
    /// <summary>
    /// Play slow reverse 5.
    /// </summary>
    DV_PLAY_SLOW_REV_5,
    /// <summary>
    /// Play slow reverse 4.
    /// </summary>
    DV_PLAY_SLOW_REV_4,
    /// <summary>
    /// Play slow reverse 3.
    /// </summary>
    DV_PLAY_SLOW_REV_3,
    /// <summary>
    /// Play slow reverse 2.
    /// </summary>
    DV_PLAY_SLOW_REV_2,
    /// <summary>
    /// Play slow reverse 1.
    /// </summary>
    DV_PLAY_SLOW_REV_1,
    /// <summary>
    /// Play fast reverse 1.
    /// </summary>
    DV_PLAY_FAST_REV_1,
    /// <summary>
    /// Play fast reverse 2.
    /// </summary>
    DV_PLAY_FAST_REV_2,
    /// <summary>
    /// Play fast reverse 3.
    /// </summary>
    DV_PLAY_FAST_REV_3,
    /// <summary>
    /// Play fast reverse 4.
    /// </summary>
    DV_PLAY_FAST_REV_4,
    /// <summary>
    /// Play fast reverse 5.
    /// </summary>
    DV_PLAY_FAST_REV_5,
    /// <summary>
    /// Play fast reverse 6.
    /// </summary>
    DV_PLAY_FAST_REV_6,
    /// <summary>
    /// Reverse.
    /// </summary>
    DV_REVERSE,
    /// <summary>
    /// Reverse pause.
    /// </summary>
    DV_REVERSE_PAUSE);

  /// <summary>
  /// Special filter type.
  /// </summary>
  TVFSpecFilterType = (
    /// <summary>
    /// MPEG-1/2 muxer.
    /// </summary>
    SF_MPEG12_Muxer,
    /// <summary>
    /// MPEG-1/2 splitter.
    /// </summary>
    SF_MPEG12_Splitter,
    /// <summary>
    /// MPEG-1/2 video decoder.
    /// </summary>
    SF_MPEG12_Video_Decoder,
    /// <summary>
    /// MPEG-1 audio decoder.
    /// </summary>
    SF_MPEG1_Audio_Decoder,
    /// <summary>
    /// Muxers.
    /// </summary>
    SF_Muxers,
    /// <summary>
    /// Audio encoders.
    /// </summary>
    SF_AudioEncoders,
    /// <summary>
    /// Video encoders.
    /// </summary>
    SF_VideoEncoders,
    /// <summary>
    /// Hardware video encoder.
    /// </summary>
    SF_Hardware_Video_Encoder);

  /// <summary>
  /// Image format.
  /// </summary>
  TVFImageFormat = (
    /// <summary>
    /// BMP.
    /// </summary>
    IM_BMP,
    /// <summary>
    /// JPEG.
    /// </summary>
    IM_JPEG,
    /// <summary>
    /// PNG.
    /// </summary>
    IM_PNG,
    /// <summary>
    /// GIF.
    /// </summary>
    IM_GIF,
    /// <summary>
    /// TIFF.
    /// </summary>
    IM_TIFF);

  /// <summary>
  /// Background image draw mode.
  /// </summary>
  TVFBGImageDrawMode = (
    /// <summary>
    /// Center.
    /// </summary>
    DM_Center,
    /// <summary>
    /// Stretch.
    /// </summary>
    DM_Stretch,
    /// <summary>
    /// Repeat.
    /// </summary>
    DM_Repeat);

  /// <summary>
  /// Audio effect type.
  /// </summary>
  TVFAudioEffectType = (

    /// <summary>
    /// Amplify.
    /// </summary>
    AE_Amplify,

    /// <summary>
    /// Bandpass.
    /// </summary>
    AE_BandPass,

    /// <summary>
    /// Channel order.
    /// </summary>
    AE_ChannelOrder,

    /// <summary>
    /// Down mix.
    /// </summary>
    AE_DownMix,

    /// <summary>
    /// Dynamic amplify.
    /// </summary>
    AE_DynamicAmplify,

    /// <summary>
    /// Echo delay.
    /// </summary>
    AE_EchoDelay,

    /// <summary>
    /// Equalizer.
    /// </summary>
    AE_Equalizer,

    /// <summary>
    /// Flanger.
    /// </summary>
    AE_Flanger,

    /// <summary>
    /// High pass.
    /// </summary>
    AE_HighPass,

    /// <summary>
    /// Low pass.
    /// </summary>
    AE_LowPass,

    /// <summary>
    /// Notch.
    /// </summary>
    AE_Notch,

    /// <summary>
    /// Phase invert.
    /// </summary>
    AE_PhaseInvert,

    /// <summary>
    /// Phaser.
    /// </summary>
    AE_Phaser,

    /// <summary>
    /// Pitch scale.
    /// </summary>
    AE_PitchScale,

    /// <summary>
    /// Pitch shift.
    /// </summary>
    AE_PitchShift,

    /// <summary>
    /// Sound3D.
    /// </summary>
    AE_Sound3D,

    /// <summary>
    /// Tempo.
    /// </summary>
    AE_Tempo,

    /// <summary>
    /// Treble enhancer.
    /// </summary>
    AE_TrebleEnhancer,

    /// <summary>
    /// True bass.
    /// </summary>
    AE_TrueBass,

    /// <summary>
    /// DMO chorus.
    /// </summary>
    AE_DMOChorus,

    /// <summary>
    /// DMO compress.
    /// </summary>
    AE_DMOCompressor,

    /// <summary>
    /// DMO distortion.
    /// </summary>
    AE_DMODistortion,

    /// <summary>
    /// DMO echo.
    /// </summary>
    AE_DMOEcho,

    /// <summary>
    /// DMO flanger.
    /// </summary>
    AE_DMOFlanger,

    /// <summary>
    /// DMO gargle.
    /// </summary>
    AE_DMOGargle,

    /// <summary>
    /// DMO I3DL2 reverb.
    /// </summary>
    AE_DMOI3DL2Reverb,

    /// <summary>
    /// DMO parametric equalizer.
    /// </summary>
    AE_DMOParamEQ,

    /// <summary>
    /// DMO wave reverb.
    /// </summary>
    AE_DMOWavesReverb,

    /// <summary>
    /// Parametric equalizer.
    /// </summary>
    AE_ParametricEQ);

  /// <summary>
  /// FFT size.
  /// </summary>
  TVFFFTSize = (

    /// <summary>
    /// 2.
    /// </summary>
    fts2,

    /// <summary>
    /// 4.
    /// </summary>
    fts4,

    /// <summary>
    /// 8.
    /// </summary>
    fts8,

    /// <summary>
    /// 16.
    /// </summary>
    fts16,

    /// <summary>
    /// 32.
    /// </summary>
    fts32,

    /// <summary>
    /// 64.
    /// </summary>
    fts64,

    /// <summary>
    /// 128.
    /// </summary>
    fts128,

    /// <summary>
    /// 256.
    /// </summary>
    fts256,

    /// <summary>
    /// 512.
    /// </summary>
    fts512,

    /// <summary>
    /// 1024.
    /// </summary>
    fts1024,

    /// <summary>
    /// 2048.
    /// </summary>
    fts2048,

    /// <summary>
    /// 4096.
    /// </summary>
    fts4096,

    /// <summary>
    /// 8192.
    /// </summary>
    fts8192);

  /// <summary>
  /// IP camera source type.
  /// </summary>
  TVFIPSource = (
    /// <summary>
    /// Auto.
    /// </summary>
    IP_Auto,

    /// <summary>
    /// HTTP MJPEG / MPEG-4 / H264.
    /// </summary>
    IP_HTTP,

    /// <summary>
    /// RTSP TCP.
    /// </summary>
    IP_RTSP_TCP,

    /// <summary>
    /// RTSP UDP.
    /// </summary>
    IP_RTSP_UDP,

    /// <summary>
    /// RTSP HTTP.
    /// </summary>
    IP_RTSP_HTTP,

    /// <summary>
    /// MMS / WMV.
    /// </summary>
    IP_MMS_WMV);

  /// <summary>
  /// Screen capture mode.
  /// </summary>
  TVFScreenCaptureMode = (
    /// <summary>
    /// Screen.
    /// </summary>
    SCM_Screen,

    /// <summary>
    /// Picture. Not implemented yet.
    /// </summary>
    SCM_Picture,

    /// <summary>
    /// Color. Not implemented yet.
    /// </summary>
    SCM_Color,

    /// <summary>
    /// Window.
    /// </summary>
    SCM_Window,

    /// <summary>
    /// Screen (FFMPEG).
    /// </summary>
    SCM_Screen_FFMPEG);

  /// <summary>
  /// PIP source type.
  /// </summary>
  TVFPIPSource = (
    /// <summary>
    /// Video capture device.
    /// </summary>
    PIP_VideoCaptureDevice,

    /// <summary>
    /// IP camera.
    /// </summary>
    PIP_IP_Camera,

    /// <summary>
    /// Screen_source.
    /// </summary>
    PIP_Screen);

  /// <summary>
  /// Picture-In-Picture mode.
  /// </summary>
  TVFPIPMode = (

    /// <summary>
    /// Custom mode. You can define output size and video sources positions manually.
    /// </summary>
    PM_Custom,

    /// <summary>
    /// Horizontal.
    /// </summary>
    PM_Horizontal,

    /// <summary>
    /// Vertical.
    /// </summary>
    PM_Vertical);

  /// <summary>
  /// Video effect type.
  /// </summary>
  TVFEffectType = (
    /// <summary>
    /// Text logo.
    /// </summary>
    ef_text_logo,
    /// <summary>
    /// Graphical logo.
    /// </summary>
    ef_graphic_logo,
    /// <summary>
    /// Blue.
    /// </summary>
    ef_blue,
    /// <summary>
    /// Blur.
    /// </summary>
    ef_blur,
    /// <summary>
    /// Color noise.
    /// </summary>
    ef_color_noise,
    /// <summary>
    /// Contrast.
    /// </summary>
    ef_contrast,
    /// <summary>
    /// Darkness.
    /// </summary>
    ef_darkness,

    /// <summary>
    /// Filter blue.
    /// </summary>
    ef_filter_blue,

    /// <summary>
    /// Filter blue 2.
    /// </summary>
    ef_filter_blue_2,

    /// <summary>
    /// Filter green.
    /// </summary>
    ef_filter_green,

    /// <summary>
    /// Filter green 2.
    /// </summary>
    ef_filter_green2,

    /// <summary>
    /// Filter red.
    /// </summary>
    ef_filter_red,

    /// <summary>
    /// Filter red 2.
    /// </summary>
    ef_filter_red2,

    /// <summary>
    /// Flip down.
    /// </summary>
    ef_flip_down,

    /// <summary>
    /// Flip right.
    /// </summary>
    ef_flip_right,

    /// <summary>
    /// Green.
    /// </summary>
    ef_green,

    /// <summary>
    /// Grayscale.
    /// </summary>
    ef_greyscale,

    /// <summary>
    /// Invert.
    /// </summary>
    ef_invert,

    /// <summary>
    /// Lightness.
    /// </summary>
    ef_lightness,

    /// <summary>
    /// Marble.
    /// </summary>
    ef_marble,

    /// <summary>
    /// Mirror down.
    /// </summary>
    ef_mirror_down,

    /// <summary>
    /// Mirror right.
    /// </summary>
    ef_mirror_right,

    /// <summary>
    /// Mono noise.
    /// </summary>
    ef_mono_noise,

    /// <summary>
    /// Mosaic.
    /// </summary>
    ef_mosaic,

    /// <summary>
    /// Posterize.
    /// </summary>
    ef_posterize,

    /// <summary>
    /// Red.
    /// </summary>
    ef_red,

    /// <summary>
    /// Saturation.
    /// </summary>
    ef_saturation,

    /// <summary>
    /// Shake down.
    /// </summary>
    ef_shake_down,

    /// <summary>
    /// Solorize.
    /// </summary>
    ef_solorize,

    /// <summary>
    /// Spray.
    /// </summary>
    ef_spray,

    /// <summary>
    /// Denoise CAST.
    /// </summary>
    ef_denoise_cast,

    /// <summary>
    /// Denoise adaptive.
    /// </summary>
    ef_denoise_adaptive,

    /// <summary>
    /// Denoise mosquito.
    /// </summary>
    ef_denoise_mosquito,

    /// <summary>
    /// Deinterlace blend.
    /// </summary>
    ef_deint_blend,

    /// <summary>
    /// Deinterlace triangle.
    /// </summary>
    ef_deint_triangle,

    /// <summary>
    /// Deinterlace CAVT.
    /// </summary>
    ef_deint_cavt,

    /// <summary>
    /// Zoom.
    /// </summary>
    ef_zoom,

    /// <summary>
    /// Pan.
    /// </summary>
    ef_pan);

  /// <summary>
  /// Stretch mode.
  /// </summary>
  TVFStretchMode = (

    /// <summary>
    /// Stretch.
    /// </summary>
    sm_stretch,

    /// <summary>
    /// Letterbox.
    /// </summary>
    sm_letterbox,

    /// <summary>
    /// Crop.
    /// </summary>
    sm_crop,

    /// <summary>
    /// None.
    /// </summary>
    sm_none);

  /// <summary>
  /// Text antialiasing mode.
  /// </summary>
  TVFTextAntialiasingMode = (

    /// <summary>
    /// System default.
    /// </summary>
    am_system_default,

    /// <summary>
    /// Single bit per pixel grid fit.
    /// </summary>
    am_SingleBitPerPixelGridFit,

    /// <summary>
    /// Single bit per pixel.
    /// </summary>
    am_SingleBitPerPixel,

    /// <summary>
    /// Antialias grid fit.
    /// </summary>
    am_AntiAliasGridFit,

    /// <summary>
    /// Antialias.
    /// </summary>
    am_AntiAlias,

    /// <summary>
    /// Cleartype grid fit.
    /// </summary>
    am_ClearTypeGridFit);

  /// <summary>
  /// Text gradient mode.
  /// </summary>
  TVFTextGradientMode = (

    /// <summary>
    /// Horizontal.
    /// </summary>
    gm_horizontal,

    /// <summary>
    /// Vertical.
    /// </summary>
    gm_vertical,

    /// <summary>
    /// Forward diagonal.
    /// </summary>
    gm_forward_diagonal,

    /// <summary>
    /// Backward diagonal.
    /// </summary>
    gm_backward_diagonal);

  /// <summary>
  /// Text effect mode.
  /// </summary>
  TVFTextEffectMode = (

    /// <summary>
    /// None.
    /// </summary>
    bm_none,

    /// <summary>
    /// Inner.
    /// </summary>
    bm_inner,

    /// <summary>
    /// Outer.
    /// </summary>
    bm_outer,

    /// <summary>
    /// Inne and outer.
    /// </summary>
    bm_inner_and_outer,

    /// <summary>
    /// Embossed.
    /// </summary>
    bm_embossed,

    /// <summary>
    /// Outline.
    /// </summary>
    bm_outline,

    /// <summary>
    /// Filled outline.
    /// </summary>
    bm_filled_outline,

    /// <summary>
    /// Halo.
    /// </summary>
    bm_halo);

  /// <summary>
  /// Text align.
  /// </summary>
  TVFTextAlign = (

    /// <summary>
    /// Left.
    /// </summary>
    al_left,

    /// <summary>
    /// Center.
    /// </summary>
    al_center,

    /// <summary>
    /// Right.
    /// </summary>
    al_right);

  /// <summary>
  /// Text draw mode.
  /// </summary>
  TVFTextDrawMode = (

    /// <summary>
    /// Bicubic HQ.
    /// </summary>
    dq_bicubic_hq,

    /// <summary>
    /// Bilinear HQ.
    /// </summary>
    dq_bilinear_hq,

    /// <summary>
    /// Nearest neighbor.
    /// </summary>
    dq_nearest_neighbor,

    /// <summary>
    /// Bicubic.
    /// </summary>
    dq_bicubic,

    /// <summary>
    /// Bilinear.
    /// </summary>
    dq_bilinear,

    /// <summary>
    /// Standard HQ.
    /// </summary>
    dq_standard_hq,

    /// <summary>
    /// Standard LQ.
    /// </summary>
    dq_standard_lq,

    /// <summary>
    /// System default.
    /// </summary>
    dq_system_default);

  /// <summary>
  /// Text shape type.
  /// </summary>
  TVFTextShapeType = (

    /// <summary>
    /// Rectangle.
    /// </summary>
    st_rectangle,

    /// <summary>
    /// Ellipse.
    /// </summary>
    st_ellipse);

  /// <summary>
  /// Text flip mode.
  /// </summary>
  TVFTextFlipMode = (

    /// <summary>
    /// None.
    /// </summary>
    fm_none,

    /// <summary>
    /// X.
    /// </summary>
    fm_x,

    /// <summary>
    /// Y.
    /// </summary>
    fm_y,

    /// <summary>
    /// X and Y.
    /// </summary>
    fm_x_and_y);

  /// <summary>
  /// Text rotation mode.
  /// </summary>
  TVFTextRotationMode = (

    /// <summary>
    /// None.
    /// </summary>
    rm_none,

    /// <summary>
    /// 90.
    /// </summary>
    rm_90,

    /// <summary>
    /// 180.
    /// </summary>
    rm_180,

    /// <summary>
    /// 270.
    /// </summary>
    rm_270);

  /// <summary>
  /// Chroma color.
  /// </summary>
  TVFChromaColor = (

    /// <summary>
    /// Red.
    /// </summary>
    Chroma_Red,

    /// <summary>
    /// Green.
    /// </summary>
    Chroma_Green,

    /// <summary>
    /// Blue.
    /// </summary>
    Chroma_Blue);

  /// <summary>
  /// Resize mode.
  /// </summary>
  TVFResizeMode = (
    /// <summary>
    /// Crop only.
    /// </summary>
    rm_CropOnly,

    /// <summary>
    /// Nearest neighbor.
    /// </summary>
    rm_NearestNeighbor,

    /// <summary>
    /// Bilinear.
    /// </summary>
    rm_Bilinear,

    /// <summary>
    /// Bicubic.
    /// </summary>
    rm_Bicubic,

    /// <summary>
    /// Lancroz.
    /// </summary>
    rm_Lancroz);

  /// <summary>
  /// Rotate mode.
  /// </summary>
  TVFRotateMode = (
    /// <summary>
    /// No rotation.
    /// </summary>
    rt_None,

    /// <summary>
    /// 90 degrees.
    /// </summary>
    rt_90,

    /// <summary>
    /// 180 degrees.
    /// </summary>
    rt_180,

    /// <summary>
    /// 270 degrees.
    /// </summary>
    rt_270);

  /// <summary>
  /// Resize/Crop/Rotate filter mode. For internal usage only.
  /// </summary>
  TVFResizeFilterMode = (
    /// <summary>
    /// Resize mode.
    /// </summary>
    fm_Resize,

    /// <summary>
    /// Rotate mode.
    /// </summary>
    fm_Rotate);

  /// <summary>
  /// Vorbis mode.
  /// </summary>
  TVFVorbisMode = (
    /// <summary>
    /// Quality.
    /// </summary>
    VorbisMode_Quality,

    /// <summary>
    /// Bitrate.
    /// </summary>
    VorbisMode_Bitrate);

  /// <summary>
  /// VP8 quality mode.
  /// </summary>
  TVP8QualityMode = (
    /// <summary>
    /// Best quality.
    /// </summary>
    VP8Quality_BestQualityBetaDoNotUse = 0,

    /// <summary>
    /// Good quality.
    /// </summary>
    VP8Quality_GoodQuality = 1,

    /// <summary>
    /// Real-time.
    /// </summary>
    VP8Quality_RealtimeMode = 2);

  /// <summary>
  /// VP8 end usage mode.
  /// </summary>
  TVP8EndUsageMode = (
    /// <summary>
    /// CBR.
    /// </summary>
    VP8EndUsageMode_CBR = 0,

    /// <summary>
    /// Default.
    /// </summary>
    VP8EndUsageMode_Default = 1,

    /// <summary>
    /// VBR.
    /// </summary>
    VP8EndUsageMode_VBR = 2);

  /// <summary>
  /// FFMPEG output format.
  /// </summary>
  TVFFFMPEGOutputFormat = (
    /// <summary>
    /// Flash video.
    /// </summary>
    FFMPEGOutputFormat_FLV,

    /// <summary>
    /// MPEG-1.
    /// </summary>
    FFMPEGOutputFormat_MPEG1,

    /// <summary>
    /// MPEG-1 VCD.
    /// </summary>
    FFMPEGOutputFormat_MPEG1VCD,

    /// <summary>
    /// MPEG-2.
    /// </summary>
    FFMPEGOutputFormat_MPEG2,

    /// <summary>
    /// MPEG-2 Transport Stream.
    /// </summary>
    FFMPEGOutputFormat_MPEG2TS,

    /// <summary>
    /// MPEG-2 SVCD.
    /// </summary>
    FFMPEGOutputFormat_MPEG2SVCD,

    /// <summary>
    /// MPEG-2 DVD.
    /// </summary>
    FFMPEGOutputFormat_MPEG2DVD,

    /// <summary>
    /// MPEG-4 ASP.
    /// </summary>
    FFMPEGOutputFormat_MPEG4);

  /// <summary>
  /// FFMPEG TV System.
  /// </summary>
  TVFFFMPEGTVSystem = (
    /// <summary>
    /// Unknown or default.
    /// </summary>
    FFMPEGTVSystem_None,

    /// <summary>
    /// PAL.
    /// </summary>
    FFMPEGTVSystem_PAL,

    /// <summary>
    /// NTSC.
    /// </summary>
    FFMPEGTVSystem_NTSC,

    /// <summary>
    /// Film.
    /// </summary>
    FFMPEGTVSystem_Film);

  /// <summary>
  /// Property page.
  /// </summary>
  TVFPropertyPage = (

    /// <summary>
    /// Simple property page.
    /// </summary>
    ppDefault,

    /// <summary>
    /// Capture Video source dialog box.
    /// </summary>
    ppVFWCapDisplay,

    /// <summary>
    /// Capture Video format dialog box.
    /// </summary>
    ppVFWCapFormat,

    /// <summary>
    /// Capture Video source dialog box.
    /// </summary>
    ppVFWCapSource,

    /// <summary>
    /// Compress Configure dialog box.
    /// </summary>
    ppVFWCompConfig,

    /// <summary>
    /// Compress About Dialog box.
    /// </summary>
    ppVFWCompAbout);

  /// <summary>
  /// WMV stream mode.
  /// </summary>
  TVFWMVStreamMode = (
    /// <summary>
    /// CBR.
    /// </summary>
    VF_SM_CBR,

    /// <summary>
    /// VBR Quality.
    /// </summary>
    VF_SM_VBR_Quality,

    /// <summary>
    /// VBR Bitrate.
    /// </summary>
    VF_SM_VBR_Bitrate,

    /// <summary>
    /// VBR Peak Bitrate.
    /// </summary>
    VF_SM_VBR_PeakBitrate);

  /// <summary>
  /// WMV mode.
  /// </summary>
  TVFWMVMode = (
    /// <summary>
    /// WM external profile.
    /// </summary>
    VF_WM_ExternalProfile,

    /// <summary>
    /// WM internal profile.
    /// </summary>
    VF_WM_InternalProfile,

    /// <summary>
    /// WM custom settings.
    /// </summary>
    VF_WM_CustomSettings,

    /// <summary>
    /// WM system profile (version 8).
    /// </summary>
    VF_WM_V8_SystemProfile);

  /// <summary>
  /// TV system.
  /// </summary>
  TVFTVSystem = (
    /// <summary>
    /// PAL.
    /// </summary>
    VF_TS_PAL,
    /// <summary>
    /// NTSC.
    /// </summary>
    VF_TS_NTSC,
    /// <summary>
    /// Other.
    /// </summary>
    VF_TS_Other);

  /// <summary>
  /// H264 rate control.
  /// </summary>
  TVFH264RateControl = (
    /// <summary>
    /// Constant bit rate.
    /// </summary>
    H264RateControl_CBR = 0,

    /// <summary>
    /// Variable bit rate.
    /// </summary>
    H264RateControl_VBR = 1);

  /// <summary>
  /// H264 MB encoding.
  /// </summary>
  TVFH264MBEncoding = (
    /// <summary>
    /// CAVLC.
    /// </summary>
    H264MBEncoding_CAVLC = 0,

    /// <summary>
    /// CABAC.
    /// </summary>
    H264MBEncoding_CABAC = 1);

  /// <summary>
  /// H264 profile.
  /// </summary>
  TVFH264Profile = (
    /// <summary>
    /// Auto.
    /// </summary>
    H264Profile_Auto = 0,

    /// <summary>
    /// Baseline.
    /// </summary>
    H264Profile_Baseline = 66,

    /// <summary>
    /// Main.
    /// </summary>
    H264Profile_Main = 77,

    /// <summary>
    /// High.
    /// </summary>
    H264Profile_High = 100,

    /// <summary>
    /// High 10.
    /// </summary>
    H264Profile_High10 = 110,

    /// <summary>
    /// High 422.
    /// </summary>
    H264Profile_High422 = 122);

  /// <summary>
  /// H264 profile level.
  /// </summary>
  TVFH264Level = (
    /// <summary>
    /// Auto.
    /// </summary>
    H264Level_LevelAuto = 0,

    /// <summary>
    /// 1.0.
    /// </summary>
    H264Level_Level1 = 10,

    /// <summary>
    /// 1.1.
    /// </summary>
    H264Level_Level11 = 11,

    /// <summary>
    /// 1.2.
    /// </summary>
    H264Level_Level12 = 12,

    /// <summary>
    /// 1.3.
    /// </summary>
    H264Level_Level13 = 13,

    /// <summary>
    /// 2.0.
    /// </summary>
    H264Level_Level2 = 20,

    /// <summary>
    /// 2.1.
    /// </summary>
    H264Level_Level21 = 21,

    /// <summary>
    /// 2.2.
    /// </summary>
    H264Level_Level22 = 22,

    /// <summary>
    /// 3.0.
    /// </summary>
    H264Level_Level3 = 30,

    /// <summary>
    /// 3.1.
    /// </summary>
    H264Level_Level31 = 31,

    /// <summary>
    /// 3.2.
    /// </summary>
    H264Level_Level32 = 32,

    /// <summary>
    /// 4.0.
    /// </summary>
    H264Level_Level4 = 40,

    /// <summary>
    /// 4.1.
    /// </summary>
    H264Level_Level41 = 41,

    /// <summary>
    /// 4.2.
    /// </summary>
    H264Level_Level42 = 42,

    /// <summary>
    /// 5.0.
    /// </summary>
    H264Level_Level5 = 50,

    /// <summary>
    /// 5.1.
    /// </summary>
    H264Level_Level51 = 51);

  /// <summary>
  /// H264 target usage.
  /// </summary>
  TVFH264TargetUsage = (
    /// <summary>
    /// Auto.
    /// </summary>
    H264TargetUsage_Auto = 0,

    /// <summary>
    /// Best quality.
    /// </summary>
    H264TargetUsage_BestQuality = 1,

    /// <summary>
    /// Balanced.
    /// </summary>
    H264TargetUsage_Balanced = 4,

    /// <summary>
    /// Best speed.
    /// </summary>
    H264TargetUsage_BestSpeed = 7);

  /// <summary>
  /// H264 picture type.
  /// </summary>
  TVFH264PictureType = (
    /// <summary>
    /// Auto.
    /// </summary>
    H264PictureType_Auto = 0,

    /// <summary>
    /// Frame.
    /// </summary>
    H264PictureType_Frame = 1,

    /// <summary>
    /// TFF.
    /// </summary>
    H264PictureType_TFF = 2,

    /// <summary>
    /// BFF.
    /// </summary>
    H264PictureType_BFF = 3);

  /// <summary>
  /// H264 time type.
  /// </summary>
  TVFH264TimeType = (
    /// <summary>
    /// Default / As input.
    /// </summary>
    H264TimeType_Default = 0,

    /// <summary>
    /// Sequental.
    /// </summary>
    H264TimeType_Sequental = 1,

    /// <summary>
    /// Encoded.
    /// </summary>
    H264TimeType_Encoded = 2);

  /// <summary>
  /// Monogram AAC encoder AAC stream object type.
  /// </summary>
  TVFAACObject = (
    /// <summary>
    /// Do not use.
    /// </summary>
    AACObject_Undefined = 0,

    /// <summary>
    /// Main.
    /// </summary>
    AACObject_Main = 1,

    /// <summary>
    /// Low.
    /// </summary>
    AACObject_Low = 2,

    /// <summary>
    /// SSR.
    /// </summary>
    AACObject_SSR = 3,

    /// <summary>
    /// LTP.
    /// </summary>
    AACObject_LTP = 4);

  /// <summary>
  /// Monogram AAC encoder AAC stream output type.
  /// </summary>
  TVFAACOutput = (
    /// <summary>
    /// RAW.
    /// </summary>
    AACObject_RAW = 0,

    /// <summary>
    /// ADTS.
    /// </summary>
    AACObject_ADTS = 1);

  /// <summary>
  /// Monogram AAC encoder AAC stream version.
  /// </summary>
  TVFAACVersion = (
    /// <summary>
    /// MPEG-4.
    /// </summary>
    AACVersion_MPEG4 = 0,

    /// <summary>
    /// MPEG-2.
    /// </summary>
    AACVersion_MPEG2);

  /// <summary>
  /// Network streaming format.
  /// </summary>
  TVFNetworkStreamingFormat = (
    /// <summary>
    /// WMV (Windows Media Video).
    /// </summary>
    NSF_WMV,

    /// <summary>
    /// External.
    /// </summary>
    NSF_External);

implementation

end.
