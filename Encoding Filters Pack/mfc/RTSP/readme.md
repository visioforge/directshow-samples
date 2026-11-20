# RTSP (MFC)

Publishes a file or a camera. The default URL is `rtsp://127.0.0.1:8554/live`.

Video is the H.264 v10 encoder. The bitrate field is kilobits per second; the encoder takes bits per second, so the sample multiplies by 1000. Audio is the v10 AAC encoder, also in bits per second, set after its input is connected. Stop calls `IMediaControl::Stop` and waits until the graph has flushed.

A file source is sent as fast as the machine allows. A camera stays on the graph clock. A missing camera is reported on Start.

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
