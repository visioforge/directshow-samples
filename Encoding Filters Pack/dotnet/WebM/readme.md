# WebM

Records a file or a camera to WebM: VP8 video with Vorbis audio, or VP9 video with Opus audio.

The container is set on the FFmpeg encoder with `IFFmpegSinkSettings.SetFormat` (WebM), and the output path with `IFileSinkFilter`. The encoder adds an input pin for each connection. Once the graph is connected, the sample sets the codecs and the video bitrate on each pin through `IFFmpegSinkPinSettings`. The bitrate is bits per second. The form asks for kilobits per second and multiplies. The picture keeps the source size. The preview and the encoder share the video through an Infinite Pin Tee.

The encoder takes the frame rate from the video type. A source that leaves it unset fails in Run with "The parameter is incorrect". The Windows WMV decoder is one such source. A 32-bit process also needs a 32-bit splitter or source filter for MP4.

`WebM.csproj` targets `net10.0-windows` and `WebM.net48.csproj` targets `net48`. Both compile the same source for x64 or x86: pick the platform in Visual Studio, or pass `-p:Platform=x64` or `-p:Platform=x86` to `dotnet build`. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
