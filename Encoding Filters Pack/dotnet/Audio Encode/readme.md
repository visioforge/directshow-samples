# Audio encode

Writes a playable audio file from a media file or a capture device.

MP3 is the LAME encoder into a standard File Writer. The bitrate is kilobits per second and is set after the encoder input is connected. AAC is the v10 AAC encoder into the v10 MP4 muxer, so the file plays in a normal player. That bitrate is bits per second: the sample multiplies the field by 1000 and sets it before the encoder is connected. The AAC encoder takes 12, 16, 20, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 140, 160, 192, or 224 kbps; any other value encodes at 224 kbps. A file source turns the muxer's live mode off and is encoded as fast as the machine allows. FLAC is the FFmpeg encoder writing Matroska audio (`.mka`): the container is set with `IFFmpegSinkSettings.SetFormat`, and the FLAC codec on the connected input pin with `IFFmpegSinkPinSettings.SetAudioCodec`. FLAC is lossless, so the bitrate field is off.

`AudioEncode.csproj` targets `net10.0-windows` x64. `AudioEncode.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
