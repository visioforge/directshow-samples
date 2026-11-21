# Legacy containers (MFC)

Records a file or a camera with the FFmpeg encoder to FLV (Sorenson Spark video, MP3 audio), MPEG-1 (MPEG-1 video, MP2 audio), or MPEG-2 (MPEG-2 video, MP2 audio). MPEG-TS is in the file-encode sample.

The container is set on the filter with `IFFmpegSinkSettings.SetFormat`, and the output path with `IFileSinkFilter`. The encoder adds an input pin for each connection. Once the graph is connected, the sample sets codec, bitrate, size, sample rate, and channels on each pin through `IFFmpegSinkPinSettings`. Video and audio bitrates are bits per second. The form asks for kilobits per second and multiplies. The bitrate is an average target, and an easy picture encodes below it. Width and height are the encoded size. The picture is scaled to it, so pick a size with the aspect ratio of the source.

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
