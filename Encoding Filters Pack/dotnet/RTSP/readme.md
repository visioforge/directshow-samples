# RTSP

Publishes a file or a camera as `rtsp://127.0.0.1:8554/live` (change the URL in the form).

Video is H.264 from the v10 encoder. The bitrate field is kilobits per second, which is what the encoder takes. Audio is AAC in bits per second: the sample multiplies its field by 1000 and sets it before the encoder is connected. The AAC encoder takes 12, 16, 20, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 140, 160, 192, or 224 kbps; any other value encodes at 224 kbps. Both streams go into the RTSP sink. Stop calls `IMediaControl.Stop` and waits until the graph has flushed.

A file source is sent as fast as the machine allows. A camera stays on the graph clock. The sink streams multicast: a player has to accept RTSP over UDP multicast (for example `ffplay -rtsp_transport udp_multicast <url>`). A missing camera is reported on Start. The sample does not require a camera when a file is selected.

`RTSP.csproj` targets `net10.0-windows` x64. `RTSP.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample. Point a player at the URL after Start.
