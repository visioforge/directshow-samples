# Legacy containers

Records a file or a camera with the legacy FFmpeg encoder to FLV, MPEG-1, MPEG-1 VCD, MPEG-2, MPEG-2 SVCD, or MPEG-2 DVD.

MPEG-TS is not in this list. The file-encode sample writes MPEG-TS with the MF muxer.

The settings struct is passed by value and is blittable: the file name is an unmanaged UTF-16 pointer, and the flags are 4-byte integers. Video and audio bitrates are bits per second. The form asks for kilobits per second and multiplies. Width, height, sample rate, and channel count are the encoder output, which the filter resamples to. Passing 0 would replace the filter's own defaults, so the sample always sends the values from the form.

`LegacyContainers.csproj` targets `net10.0-windows` x64. `LegacyContainers.net48.csproj` targets `net48` x64. Both compile the same source. Put a license key in `Session.LicenseKey` after purchase. An empty key runs the trial.

Install the Encoding Filters Pack before running the sample.
