#ifndef __iScreenCapture__
#define __iScreenCapture__

#ifdef __cplusplus
extern "C" {
#endif

	// {259E0009-9963-4a71-91AE-34B96D75486F}
	DEFINE_GUID(IID_IVFScreenCapture, 
		0x259e0009, 0x9963, 0x4a71, 0x91, 0xae, 0x34, 0xb9, 0x6d, 0x75, 0x48, 0x6f);

	// {BC91012D-22E0-4091-8C0A-3913BDAB8A42}
	DEFINE_GUID(IID_IVFScreenCapture2, 
		0xbc91012d, 0x22e0, 0x4091, 0x8c, 0xa, 0x39, 0x13, 0xbd, 0xab, 0x8a, 0x42);

	struct VFRect 
	{
		UINT32 left;
		UINT32 top;
		UINT32 right;
		UINT32 bottom;
	};

	DECLARE_INTERFACE_(IVFScreenCapture, IUnknown)
	{
		STDMETHOD(set_fps) (THIS_
			double fps
			) PURE;

		STDMETHOD(set_rect) (THIS_
			VFRect rect
			) PURE;

		STDMETHOD(set_mouse) (THIS_
			bool draw
			) PURE;

		STDMETHOD(set_screen_index) (THIS_
			int index
			) PURE;
	};

	DECLARE_INTERFACE_(IVFScreenCapture2, IUnknown)
	{
		STDMETHOD(set_mode) (THIS_
			int picture_mode
			) PURE;

		STDMETHOD(refresh_pic) (
			) PURE;

		STDMETHOD(set_stream) (THIS_
			IStream* Stream,
			__int64 Length
			) PURE;
	};

#ifdef __cplusplus
}
#endif

#endif // __iScreenCapture__

