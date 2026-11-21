# H.264 v10 (MFC)

Records a file or a camera to an MP4 file. Video is the H.264 v10 encoder, which accepts a picture larger than 1920x1080. The software H.264 encoder in the File Encode sample stops above that size.

The bitrate field is kilobits per second, which is what the encoder takes. Audio is optional AAC in bits per second: the sample multiplies its field by 1000 and sets it before the encoder is connected. The AAC encoder takes 12, 16, 20, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 140, 160, 192, or 224 kbps; any other value encodes at 224 kbps. Both streams go into the v10 MP4 muxer. A file source turns the muxer's live mode off and is encoded as fast as the machine allows. A camera stays on the graph clock. A missing camera is reported on Start.

The default output is `Documents\VisioForge\encode.mp4`.

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
