# File encode (MFC)

Same recording path as the WinForms file-encode sample: a file or a camera, a preview, and the MF Mux filter.

Video encoders are H.264 and H.265, each as software, Intel Quick Sync, NVIDIA NVENC, or AMD. This dialog lists every encoder. Start reports the failure when that encoder is not installed. Software H.264 and H.265 stop above 1920x1080; a larger picture needs a GPU encoder. Audio is AAC. The bitrate fields are kilobits per second.

The preview and the encoder share the video through an Infinite Pin Tee. A file source is encoded as fast as the machine allows. A camera stays on the graph clock, with low latency turned on. HLS writes a folder (`stream.m3u8` and `segment`).

The project is Visual Studio 2022 (`v143`), MFC in a shared DLL, Unicode, Win32 and x64. Put a license key in `kLicenseKey` in `../common/Graph.h` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample. HLS needs the Encoding Filters Pack redist with `VisioForge_MFT64.dll` 2026.11.10 or later (`VisioForge_MFT.dll` for 32-bit); an older one crashes when HLS starts.
