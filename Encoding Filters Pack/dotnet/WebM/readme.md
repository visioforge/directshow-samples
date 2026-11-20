# WebM

Records a file or a camera to WebM: VP8 video and Vorbis audio.

The VP8 encoder is not configured through `VisioForge.DirectShowAPI`. This sample declares the x64 `IVP8Encoder` and `IVPXEncoder` COM interfaces itself. `SetTargetBitrate` is kilobits per second. A file source uses the best-quality deadline. A camera uses the realtime deadline and no frame lag.

The WebM muxer is not the file sink. The sample connects it to a standard File Writer. Video is converted to I420 (or YV12) before VP8. The preview and the encoder share the video through an Infinite Pin Tee.

`WebM.csproj` targets `net10.0-windows` x64. `WebM.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample. This sample encodes VP8. It does not encode VP9.
