//////////////////////////////////////////////////////
// VLCSourceGuids.h
//
// CLSID/IID declarations and COM interface definitions
// for the VisioForge VLC Source filter.
//
// Note: GUID values are defined in guids.cpp. This
// header only declares them so it can be safely
// included from multiple translation units.
//////////////////////////////////////////////////////
#pragma once
//////////////////////////////////////////////////////
#include <guiddef.h>
//////////////////////////////////////////////////////

// CLSID for the VLC Source filter
// {3FC97748-7CB6-4195-89DE-0717582A4863}
EXTERN_C const GUID CLSID_VlcSource;

// IID for IVlcSrc interface
// {77493EB7-6D00-41C5-9535-7C593824E892}
EXTERN_C const GUID IID_IVlcSrc;

DECLARE_INTERFACE_(IVlcSrc, IUnknown)
{
    STDMETHOD(SetFile) (THIS_
        WCHAR *file
        ) PURE;

    STDMETHOD(GetAudioTracksCount) (THIS_
        int *count
        ) PURE;

    STDMETHOD(GetAudioTrackInfo) (THIS_
        int number,
        int *id,
        WCHAR *name
        ) PURE;

    STDMETHOD(GetAudioTrack) (THIS_
        int *id
        ) PURE;

    STDMETHOD(SetAudioTrack) (THIS_
        int id
        ) PURE;

    STDMETHOD(GetSubtitlesCount) (THIS_
        int *count
        ) PURE;

    STDMETHOD(GetSubtitleInfo) (THIS_
        int number,
        int *id,
        WCHAR *name
        ) PURE;

    STDMETHOD(GetSubtitle) (THIS_
        int *id
        ) PURE;

    STDMETHOD(SetSubtitle) (THIS_
        int id
        ) PURE;
};

// IID for IVlcSrc2 interface
// {CCE122C0-172C-4626-B4B6-42B039E541CB}
EXTERN_C const GUID IID_IVlcSrc2;

DECLARE_INTERFACE_(IVlcSrc2, IVlcSrc)
{
    STDMETHOD(SetCustomCommandLine) (THIS_
        char* params[],
        int length
        ) PURE;
};
