# H.264 v10

Records a file or a camera to an MP4 file. Video is the H.264 v10 encoder, which accepts a picture larger than 1920x1080. The software H.264 encoder in the File Encode sample stops above that size.

The bitrate field is kilobits per second; the encoder takes bits per second, so the sample multiplies by 1000. Audio is optional AAC, also in bits per second, set after the encoder input is connected. Both streams go into the v10 MP4 muxer. A file source turns the muxer's live mode off and is encoded as fast as the machine allows. A camera stays on the graph clock. A missing camera is reported on Start.

The default output is `Documents\VisioForge\encode.mp4`.

`H264V10.csproj` targets `net10.0-windows` x64. `H264V10.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
