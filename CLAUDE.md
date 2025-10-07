# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

This repository contains sample projects demonstrating DirectShow filters from VisioForge SDKs. The samples showcase video/audio processing, encoding, source filtering, and virtual camera functionality for Windows platforms.

## Repository Structure

The repository is organized by product category:

- **Interfaces/** - Shared DirectShow COM interface definitions
  - `cpp/` - C++ header files (~32 interfaces) for DirectShow filter integration
  - `dotnet/` - .NET interop layer (VisioForge.DirectShowAPI NuGet package)
- **VLC Source Filter/** - VLC-based media source samples
- **FFMPEG Source Filter/** - FFmpeg-based media source samples
- **Virtual Camera SDK/** - Virtual camera implementation samples
- **Processing Filters Pack/** - Video effects and mixing filters (VideoEffects, VideoMixer)
- **Encoding Filters Pack/** - Encoding filter demonstrations

Each product directory typically contains:
- `dotnet/` - .NET samples (C#, VB.NET) targeting .NET 8.0 or .NET Framework 4.8
- `cpp/` or `cpp_builder/` - C++ samples (Visual Studio or C++Builder)
- `delphi/` - Delphi samples (where applicable)

## Building

### .NET Samples

Build individual solutions:
```bash
dotnet build "<Product Directory>/dotnet/<Sample Name>/<Sample Name>.sln"
```

Examples:
```bash
dotnet build "VLC Source Filter/dotnet/cs/VLC Source Demo.sln"
dotnet build "Processing Filters Pack/dotnet/Processing Filters Samples.sln"
dotnet build "Virtual Camera SDK/dotnet/Virtual Camera SDK Samples.sln"
```

### Interfaces Library

Build the shared DirectShow API library:
```bash
dotnet build "Interfaces/dotnet/VisioForge.DirectShowAPI.sln"
```

The library targets netstandard2.0 and is distributed as a NuGet package (VisioForge.DirectShowAPI).

### Platform-Specific Builds

.NET samples typically support x64 and x86 platforms. Specify platform when needed:
```bash
dotnet build "VLC Source Filter/dotnet/cs/VLC Source Demo.sln" -p:Platform=x64
```

## Architecture

### COM Interop Pattern

All DirectShow filters are COM-based. The .NET samples use P/Invoke and COM interop through:

1. **Interface Definitions** - COM interfaces defined in `Interfaces/dotnet/` with proper marshaling attributes
2. **Helper Classes** - `DSHelper.cs` and `FilterHelper.cs` provide filter management utilities
3. **DirectShowLib** - Third-party library for core DirectShow functionality
4. **Native Interfaces** - C++ headers in `Interfaces/cpp/` for native integration

### Key Interop Classes

- **DSHelper** - Filter creation, adding to graph, format configuration
- **FilterHelper** - Pin connection/disconnection, filter connectivity checks
- **FilterGraphHelper** - Graph manipulation utilities
- **Helpful** - General DirectShow utilities and helper functions

### Filter Configuration Pattern

VisioForge filters typically expose custom configuration interfaces (e.g., `IFFmpegSourceSettings`, `IVFPushConfig`) that must be queried and configured before connecting filters in the graph.

Common pattern:
1. Create filter from CLSID using `DSHelper.AddFilterFromClsid()`
2. Query custom configuration interface
3. Configure filter settings
4. Connect filter in DirectShow graph
5. Run graph

### Product-Specific Architectures

- **FFMPEG/VLC Source** - Source filters with hardware acceleration, buffering modes, custom FFmpeg options
- **Virtual Camera** - Push-based streaming with configurable video/audio streams and FFmpeg filters
- **Processing Filters** - Video effects (chroma key, enhancement) and multi-source mixing
- **Encoding Filters** - Hardware-accelerated encoding (NVENC, CUDA, H.264) with custom configuration

## Common Development Patterns

When working with samples:

1. Filters use COM interfaces - always check HRESULT return values
2. Memory management - release COM objects with `Marshal.ReleaseComObject()`
3. Filter graphs must be properly connected before running
4. Configuration interfaces must be set before filter connection
5. Hardware acceleration settings (when available) must be configured before graph connection

## Dependencies

- DirectShow SDK (included in Windows SDK)
- .NET 8.0 SDK or .NET Framework 4.8 (depending on sample)
- VisioForge.DirectShowAPI NuGet package (v2025.5.4)
- MediaFoundationCore NuGet package (v3.1.2)
- System.Drawing.Common NuGet package (v9.0.5)

## Notes

- Samples demonstrate commercial DirectShow filters - runtime requires appropriate VisioForge SDK licenses
- C++ samples may require Windows SDK and DirectShow base classes
- All .NET projects use strong-name signing
- The repository uses git for version control with main branch as primary
