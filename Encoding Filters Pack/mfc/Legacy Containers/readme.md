# Legacy containers (MFC)

Records a file or a camera with the legacy FFmpeg encoder to FLV, MPEG-1, MPEG-1 VCD, MPEG-2, MPEG-2 SVCD, or MPEG-2 DVD. MPEG-TS is in the file-encode sample.

The settings struct is passed by value. Video and audio bitrates are bits per second. The form asks for kilobits per second and multiplies. Width, height, sample rate, and channel count are the encoder output, which the filter resamples to.

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
