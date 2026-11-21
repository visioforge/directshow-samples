# WebM (MFC)

Records a file or a camera to WebM: VP8 video with Vorbis audio, or VP9 video with Opus audio.

The container is set on the FFmpeg encoder with `IFFmpegSinkSettings.SetFormat` (WebM), and the output path with `IFileSinkFilter`. The encoder adds an input pin for each connection. Once the graph is connected, the sample sets the codecs and the video bitrate on each pin through `IFFmpegSinkPinSettings`. The bitrate is bits per second. The form asks for kilobits per second and multiplies. The picture keeps the source size. The preview and the encoder share the video through an Infinite Pin Tee.

The encoder takes the frame rate from the video type. A source that leaves it unset fails in Run with "The parameter is incorrect". The Windows WMV decoder is one such source. A 32-bit process also needs a 32-bit splitter or source filter for MP4.

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
