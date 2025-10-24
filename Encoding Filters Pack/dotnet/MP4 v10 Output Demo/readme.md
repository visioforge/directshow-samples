# MP4 v10 Output Demo

This sample demonstrates how to create MP4 video files using DirectShow Editing Services (DES) and the VisioForge_MP4_Muxer_v10 DirectShow filter.

## Features

- Add multiple input video files
- Add multiple input audio files  
- Combine video and audio files into a single MP4 output
- Uses DirectShow Editing Services (DES) API directly
- Uses VisioForge_MP4_Muxer_v10 DirectShow filter for MP4 muxing
- Progress tracking during conversion
- Simple and intuitive WinForms interface

## Requirements

- .NET 8.0-windows
- Windows operating system
- VisioForge.Core library (for DirectShow interfaces)
- VisioForge.Libs.External library (for DES interfaces)
- VisioForge_MP4_Muxer_v10 DirectShow filter installed

## How to Use

1. Click "Add Video File" to add video files to the input list
2. Click "Add Audio File" to add audio files to the input list
3. Specify the output MP4 file path or use the default
4. Click "Start" to begin the conversion process
5. Monitor the progress bar and status messages
6. Click "Stop" to cancel the conversion if needed

## Technical Details

### DirectShow Filter Used

- **Filter Name**: VisioForge MP4 Muxer v10
- **CLSID**: {0B0D654C-7AC1-441E-9C4D-3C29ABEDB6A8}
- **Filter Files**: 
  - VisioForge_MP4_Muxer_v10.ax (32-bit)
  - VisioForge_MP4_Muxer_v10_x64.ax (64-bit)

### DirectShow Editing Services API

This sample uses DES (DirectShow Editing Services) directly through COM interfaces:
- **IAMTimeline**: Creates and manages the timeline
- **IRenderEngine**: Renders the timeline to a DirectShow filter graph
- **IAMTimelineObj**: Represents timeline objects (groups, tracks, sources)
- **IAMTimelineGroup**: Represents a group in the timeline
- **IAMTimelineTrack**: Represents a track containing clips
- **IAMTimelineSrc**: Represents a source clip
- **IMediaDet**: Detects media file properties like duration
- **IGraphBuilder**: DirectShow filter graph builder
- **IMediaControl**: Controls playback/rendering
- **IFileSinkFilter**: Configures output file

### Implementation Details

The sample creates a DirectShow Editing Services timeline with:
1. Timeline creation using AMTimeline class
2. Video group with track for video sources
3. Audio group with track for audio sources (planned)
4. Source clips added to tracks with proper timing
5. Render engine connects the timeline to a filter graph
6. MP4 Muxer v10 filter added and configured
7. File sink filter configured for output
8. Graph execution for rendering

### Supported Input Formats

**Video Files**:
- MP4, AVI, WMV, MKV, MOV, MPG, MPEG, M4V

**Audio Files**:
- MP3, WAV, AAC, M4A, WMA, OGG, FLAC

## Code Structure

- `MainForm.cs`: Main application form with UI and DirectShow/DES logic
- `Program.cs`: Application entry point
- `MP4 v10 Output Demo.csproj`: Project configuration

## Building

1. Open the solution in Visual Studio 2022 or later
2. Ensure all NuGet packages are restored
3. Build the project in Release or Debug mode
4. The application requires x64 platform

## Notes

- The sample uses DirectShow Editing Services (DES) COM API directly without VideoEditCore wrapper
- The MP4 v10 muxer is configured through the IMP4V10MuxerConfig interface
- Multiple video and audio files can be added and will be concatenated in sequence
- The output will use H.264 video codec and AAC audio codec by default
- Progress is reported based on timeline position

## See Also

- DirectShow Editing Services (DES) documentation
- DirectShow documentation
- VisioForge.Core DirectShow interfaces
