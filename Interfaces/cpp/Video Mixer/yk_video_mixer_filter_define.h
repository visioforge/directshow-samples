
#ifndef YKVIDEOMIXERFILTERDEFINE
#define YKVIDEOMIXERFILTERDEFINE

#include "yk_define.h"

// {3318300E-F6F1-4d81-8BC3-9DB06B09F77A}
static const GUID IID_IVideoMixer = 
{ 0x3318300e, 0xf6f1, 0x4d81, { 0x8b, 0xc3, 0x9d, 0xb0, 0x6b, 0x9, 0xf7, 0x7a } };

struct VideoInputParam
{
	// output position and size
	int			x;
	int			y;
	int			w;
	int			h;

	// alpha 0(fill) - 255(transparent)
	int			alpha;

	BOOL flipX;
	BOOL flipY;
};

struct VideoOutputParam
{
	int 		w;
	int 		h;

	int			frame_rate;
	int			back_color;
	YK_TCHAR*	back_image;
};

// We define the interface the app can use to program us
MIDL_INTERFACE("3318300E-F6F1-4d81-8BC3-9DB06B09F77A")
IVideoMixer : public IUnknown
{
public:
	virtual HRESULT STDMETHODCALLTYPE SetInputParam(int pin_index,VideoInputParam param) = 0;
	virtual HRESULT STDMETHODCALLTYPE GetInputParam(int pin_index,VideoInputParam* param) = 0;
	virtual HRESULT STDMETHODCALLTYPE GetInputParam2(IPin* pin,VideoInputParam* param) = 0;
	virtual HRESULT STDMETHODCALLTYPE SetOutputParam(VideoOutputParam param) = 0;
	virtual HRESULT STDMETHODCALLTYPE GetOutputParam(VideoOutputParam* param) = 0;
};

#endif