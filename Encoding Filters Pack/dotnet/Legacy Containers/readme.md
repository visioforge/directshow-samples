# Legacy containers

Records a file or a camera with the FFmpeg encoder to FLV (Sorenson Spark video, MP3 audio), MPEG-1 (MPEG-1 video, MP2 audio), or MPEG-2 (MPEG-2 video, MP2 audio).

MPEG-TS is not in this list. The file-encode sample writes MPEG-TS with the MF muxer.

The container is set on the filter with `IFFmpegSinkSettings.SetFormat`, and the output path with `IFileSinkFilter`. The encoder adds an input pin for each connection. Once the graph is connected, the sample sets codec, bitrate, size, sample rate, and channels on each pin through `IFFmpegSinkPinSettings`. Video and audio bitrates are bits per second. The form asks for kilobits per second and multiplies. The bitrate is an average target, and an easy picture encodes below it. Width and height are the encoded size. The picture is scaled to it, so pick a size with the aspect ratio of the source.

`LegacyContainers.csproj` targets `net10.0-windows` x64. `LegacyContainers.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
