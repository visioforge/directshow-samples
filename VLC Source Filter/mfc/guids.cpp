//////////////////////////////////////////////////////
// guids.cpp
//
// GUID definitions for VisioForge VLC Source filter.
// This file must be compiled in exactly one translation
// unit to avoid multiply-defined symbol linker errors.
//////////////////////////////////////////////////////

#include "pch.h"
#include <initguid.h>

// {3FC97748-7CB6-4195-89DE-0717582A4863}
DEFINE_GUID(CLSID_VlcSource,
0x3fc97748, 0x7cb6, 0x4195, 0x89, 0xde, 0x07, 0x17, 0x58, 0x2a, 0x48, 0x63);

// {77493EB7-6D00-41C5-9535-7C593824E892}
DEFINE_GUID(IID_IVlcSrc,
0x77493eb7, 0x6d00, 0x41c5, 0x95, 0x35, 0x7c, 0x59, 0x38, 0x24, 0xe8, 0x92);

// {CCE122C0-172C-4626-B4B6-42B039E541CB}
DEFINE_GUID(IID_IVlcSrc2,
0xcce122c0, 0x172c, 0x4626, 0xb4, 0xb6, 0x42, 0xb0, 0x39, 0xe5, 0x41, 0xcb);
