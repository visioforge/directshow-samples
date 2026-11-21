# File encode

Records a file or a camera to MP4, MKV, MOV, MPEG-TS, or HLS with the VisioForge MF Mux filter.

Video encoders are H.264 and H.265, each as software, Intel QuickSync, NVIDIA NVENC, or AMD. An encoder that is not installed is marked in the list and cannot be started. H.265 software is listed only when HEVC Video Extensions (`HEVCVideoExtensionEncoder`) are installed. H.265 Intel and NVIDIA are listed from the installed hardware encoder names. Software H.264 and H.265 stop above 1920x1080; a larger picture needs a GPU encoder. Audio is AAC. `VFMFVideoEncoderSettings.AvgBitrate` is kilobits per second.

The preview and the encoder share the video through an Infinite Pin Tee. A file source is encoded as fast as the machine allows. A camera stays on the graph clock, with low latency turned on.

`FileEncode.csproj` targets `net10.0-windows` x64. `FileEncode.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample. HLS needs the Encoding Filters Pack redist with `VisioForge_MFT64.dll` 2026.11.10 or later (`VisioForge_MFT.dll` for 32-bit); an older one crashes when HLS starts.
