# RTSP (MFC)

Publishes a file or a camera. The default URL is `rtsp://127.0.0.1:8554/live`.

Video is the H.264 v10 encoder. The bitrate field is kilobits per second, which is what the encoder takes. Audio is the v10 AAC encoder in bits per second: the sample multiplies its field by 1000 and sets it before the encoder is connected. The AAC encoder takes 12, 16, 20, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 140, 160, 192, or 224 kbps; any other value encodes at 224 kbps. Stop calls `IMediaControl::Stop` and waits until the graph has flushed.

A file source is sent as fast as the machine allows. A camera stays on the graph clock. The sink streams multicast: a player has to accept RTSP over UDP multicast (for example `ffplay -rtsp_transport udp_multicast <url>`). A missing camera is reported on Start.

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
