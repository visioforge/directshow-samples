# WebM (MFC)

Records a file or a camera to WebM: VP8 video and Vorbis audio. Win32 uses the 32-bit `IVP8Encoder` interface. x64 uses the 64-bit one. The method order is the same, through `SetTargetBitrate`, which is kilobits per second.

The WebM muxer is not the file sink. The sample connects it to a standard File Writer. Video is converted to I420 or YV12 before VP8. A file source uses the best-quality deadline. A camera uses the realtime deadline and no frame lag.

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample. This sample encodes VP8.
