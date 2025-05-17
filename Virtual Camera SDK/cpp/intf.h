//////////////////////////////////////////////////////
#pragma once
//////////////////////////////////////////////////////
#include <initguid.h>
//////////////////////////////////////////////////////
// {AA6AB4DF-9670-4913-88BB-2CB381C19340}
DEFINE_GUID(CLSID_VFVirtualCameraSink, 
0xaa6ab4df, 0x9670, 0x4913, 0x88, 0xbb, 0x2c, 0xb3, 0x81, 0xc1, 0x93, 0x40);
//////////////////////////////////////////////////////
// {AA4DA14E-644B-487a-A7CB-517A390B4BB8}
DEFINE_GUID(CLSID_VFVirtualCameraSource, 
0xaa4da14e, 0x644b, 0x487a, 0xa7, 0xcb, 0x51, 0x7a, 0x39, 0xb, 0x4b, 0xb8);
//////////////////////////////////////////////////////

//////////////////////////////////////////////////////
// {1A2673B0-553E-4027-AECC-839405468950}
DEFINE_GUID(CLSID_VFVirtualAudioSink, 
0x1a2673b0, 0x553e, 0x4027, 0xae, 0xcc, 0x83, 0x94, 0x5, 0x46, 0x89, 0x50);
//////////////////////////////////////////////////////
// {B5A463DF-4016-4c34-AA4F-48EC1B51C73F}
DEFINE_GUID(CLSID_VFVirtualAudioSource, 
0xb5a463df, 0x4016, 0x4c34, 0xaa, 0x4f, 0x48, 0xec, 0x1b, 0x51, 0xc7, 0x3f);
//////////////////////////////////////////////////////

// {A96631D2-4AC9-4F09-9F34-FF8229087DEB}
DEFINE_GUID(IID_IVFVirtualCameraSink, 
0xa96631d2, 0x4ac9, 0x4f09, 0x9f, 0x34, 0xff, 0x82, 0x29, 0x8, 0x7d, 0xeb);

DECLARE_INTERFACE_(IVFVirtualCameraSink, IUnknown)
{
	// register
	STDMETHOD(set_license) (THIS_
		LPCWSTR license          
		) PURE;
};

// {9D91D91F-3D2A-4127-9719-AF39DAD4A473}
DEFINE_GUID(IID_IVFVirtualCameraSource,
	0x9d91d91f, 0x3d2a, 0x4127, 0x97, 0x19, 0xaf, 0x39, 0xda, 0xd4, 0xa4, 0x73);

DECLARE_INTERFACE_(IVFVirtualCameraSource, IUnknown)
{
	STDMETHOD(SetCustomVideoSize) (THIS_
		int width,
		int height
		) PURE;

	STDMETHOD(FixResolution) () PURE;
};