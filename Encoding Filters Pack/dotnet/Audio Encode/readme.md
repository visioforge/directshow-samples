# Audio encode

Writes a playable audio file from a media file or a capture device.

MP3 is the LAME encoder into a standard File Writer. The bitrate is kilobits per second and is set after the encoder input is connected. AAC is the v10 AAC encoder into the v10 MP4 muxer, so the file plays in a normal player. That bitrate is bits per second: the sample multiplies the field by 1000. A file source turns the muxer's live mode off and is encoded as fast as the machine allows. FLAC is the Xiph encoder into the Dump filter. The level runs from 0 (fastest) to 8, and 5 is the filter default. The Dump filter stores the stream; this sample does not rewrite the duration in the header.

`AudioEncode.csproj` targets `net10.0-windows` x64. `AudioEncode.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
