//////////////////////////////////////////////////////
// VCameraGuids.h
//
// CLSID/IID declarations and COM interface definitions
// for the VisioForge Virtual Camera filters and standard
// DirectShow helpers used in the MFC demo.
//////////////////////////////////////////////////////
#pragma once
#include <guiddef.h>
//////////////////////////////////////////////////////

// Virtual Camera Source filter
// {AA4DA14E-644B-487a-A7CB-517A390B4BB8}
EXTERN_C const GUID CLSID_VFVirtualCameraSource;

// Virtual Camera Sink filter
// {AA6AB4DF-9670-4913-88BB-2CB381C19340}
EXTERN_C const GUID CLSID_VFVirtualCameraSink;

// Virtual Audio Card Source filter
// {B5A463DF-4016-4C34-AA4F-48EC1B51C73F}
EXTERN_C const GUID CLSID_VFVirtualAudioCardSource;

// Virtual Audio Card Sink filter
// {1A2673B0-553E-4027-AECC-839405468950}
EXTERN_C const GUID CLSID_VFVirtualAudioCardSink;

// IVFVirtualCameraSource interface
// {9D91D91F-3D2A-4127-9719-AF39DAD4A473}
EXTERN_C const GUID IID_IVFVirtualCameraSource;

DECLARE_INTERFACE_(IVFVirtualCameraSource, IUnknown)
{
    STDMETHOD(SetCustomVideoSize) (THIS_
        int width,
        int height
        ) PURE;

    STDMETHOD(FixResolution) () PURE;
};

// Smart Tee filter
// {CC58E280-8AA1-11D1-B3F1-00AA003761C5}
EXTERN_C const GUID CLSID_SmartTee;

// MJPEG Compressor
// {B80AB0A0-7416-11D2-9EEB-006008039E37}
EXTERN_C const GUID CLSID_MJPEGCompressor;

// AVI Mux
// {E2510970-F137-11CE-8B67-00AA00A3F1A6}
EXTERN_C const GUID CLSID_AVIMux;

// File Writer
// {8596E5F0-0DA5-11D0-BD21-00A0C911CE86}
EXTERN_C const GUID CLSID_FileWriter;

// PCM Audio Codec (ACM Wrapper)
// {6A08CF80-0E18-11CF-A24D-0020AFD79767}
EXTERN_C const GUID CLSID_PCMAudioCodec;

// DirectSound Audio Renderer
// {79376820-07D0-11CF-A24D-0020AFD79767}
EXTERN_C const GUID CLSID_DSoundRenderer;
