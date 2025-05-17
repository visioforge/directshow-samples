#ifndef __IRESIZE__
#define __IRESIZE__

enum CVFResizeMode {rm_CropOnly, rm_NearestNeighbor, rm_Bilinear, rm_Bicubic, rm_Lancroz}; 
enum CVFRotateMode {rt_None, rt_90, rt_180, rt_270};
enum CVFResizeFilterMode {rf_Resize, rf_Rotate};

#ifdef __cplusplus
extern "C" {
#endif

	// {2E0E7313-71DC-4455-ADB4-F80718B7B727}
	DEFINE_GUID(CLSID_VFResizer_4, 
		0x2e0e7313, 0x71dc, 0x4455, 0xad, 0xb4, 0xf8, 0x7, 0x18, 0xb7, 0xb7, 0x27);

	// {12BC6F20-2812-4660-8684-10F3FD3B4487}
	DEFINE_GUID(IID_IVFResize, 
		0x12bc6f20, 0x2812, 0x4660, 0x86, 0x84, 0x10, 0xf3, 0xfd, 0x3b, 0x44, 0x87);

	

    DECLARE_INTERFACE_(IVFResize, IUnknown)
    {
        STDMETHOD(put_Resolution) (THIS_
            UINT32 x,
			UINT32 y
        ) PURE;

		STDMETHOD(put_ResizeMode) (THIS_
			CVFResizeMode mode,
			bool letterbox
			) PURE;

		STDMETHOD(put_Crop) (THIS_
			UINT32 left,
			UINT32 top,
			UINT32 right,
			UINT32 bottom
			) PURE;

		STDMETHOD(put_FilterMode) (THIS_
			CVFResizeFilterMode mode
			) PURE;

		STDMETHOD(put_RotateMode) (THIS_
			CVFRotateMode mode
			) PURE;
    };

#ifdef __cplusplus
}
#endif

#endif // __IRESIZE__

