# RTSP

Publishes a file or a camera as `rtsp://127.0.0.1:8554/live` (change the URL in the form).

Video is H.264 from the v10 encoder. The bitrate field is kilobits per second; the encoder takes bits per second, so the sample multiplies by 1000. Audio is AAC in bits per second, set after the encoder input is connected. Both streams go into the RTSP sink. Stop calls `IMediaControl.Stop` and waits until the graph has flushed.

A file source is sent as fast as the machine allows. A camera stays on the graph clock. A missing camera is reported on Start. The sample does not require a camera when a file is selected.

`RTSP.csproj` targets `net10.0-windows` x64. `RTSP.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample. Point a player at the URL after Start.
