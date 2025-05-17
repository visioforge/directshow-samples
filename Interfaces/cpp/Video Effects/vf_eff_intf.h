
#pragma once

#include <initguid.h>

#ifndef __vf_eff_intf__
#define __vf_eff_intf__

enum CVFEffectType {ef_text_logo, ef_graphic_logo, ef_blue, ef_blur, ef_color_noise, ef_contrast, ef_darkness,  
					  ef_filter_blue, ef_filter_blue_2, ef_filter_green, ef_filter_green2, ef_filter_red, ef_filter_red2, 
					  ef_flip_down, ef_flip_right, ef_green, ef_greyscale, ef_invert, ef_lightness, 
					  ef_marble, ef_mirror_down, ef_mirror_right, ef_mono_noise, ef_mosaic, ef_posterize, ef_red,
					  ef_saturation, ef_shake_down, ef_solorize, ef_spray, ef_denoise_cast, ef_denoise_adaptive, 
					  ef_denoise_mosquito, ef_deint_blend, ef_deint_triangle, ef_deint_cavt};

enum CVFStretchMode {sm_stretch, sm_letterbox, sm_crop, sm_none};
enum CVFTextAntialiasingMode {am_system_default, am_SingleBitPerPixelGridFit, am_SingleBitPerPixel, am_AntiAliasGridFit, am_AntiAlias, am_ClearTypeGridFit};
enum CVFTextGradientMode {gm_horizontal, gm_vertical, gm_forward_diagonal, gm_backward_diagonal};
enum CVFTextBorderMode {bm_none, bm_inner, bm_outer, bm_inner_and_outer, bm_embossed, bm_outline, bm_filled_outline, bm_halo};
enum CVFTextAlign {al_left, al_center, al_right};
enum CVFTextDrawMode {dq_bicubic_hq, dq_bilinear_hq, dq_nearest_neighbor, dq_bicubic, dq_bilinear, dq_standard_hq, dq_standard_lq, dq_system_default};
enum CVFTextShapeType {st_rectangle, st_ellipse};
enum CVFTextFlipMode {fm_none, fm_x, fm_y, fm_x_and_y};
enum CVFTextRotationMode {rm_none, rm_90, rm_180, rm_270};


// {980B1181-1619-44F9-AEBE-F2D7E5CE1EFE}
DEFINE_GUID(CLSID_VF_EffectsPro_45, 
0x980b1181, 0x1619, 0x44f9, 0xae, 0xbe, 0xf2, 0xd7, 0xe5, 0xce, 0x1e, 0xfe);

// {5E767DA8-97AF-4607-B95F-8CC6010B84CA}
DEFINE_GUID(IID_IVFEffects_45, 
0x5e767da8, 0x97af, 0x4607, 0xb9, 0x5f, 0x8c, 0xc6, 0x1, 0xb, 0x84, 0xca);

struct CVFTextLogoMain
{
	int x;
	int y;

	BOOL transparent_bg;

	int font_size;

	BOOL font_italic;
	BOOL font_bold;
	BOOL font_underline;
	BOOL font_strikeout;

	COLORREF font_color;
	COLORREF bg_color;

	BOOL rightToLeft;
	BOOL vertical;

	DWORD align;
	DWORD drawQuality;
	DWORD antialiasing;
	int rectWidth;
	int rectHeight;
	DWORD rotationMode;
	DWORD flipMode;
	DWORD transp;

	BOOL gradient;

	DWORD gradientMode;
	COLORREF gradientColor1;
	COLORREF gradientColor2;
	DWORD borderMode;
	COLORREF innerBorderColor;
	COLORREF outerBorderColor;
	int innerBorderSize;
	int outerBorderSize;
	
	BOOL bgShape;

	DWORD bgShapeType;
	int bgShapeX;
	int bgShapeY;
	int bgShapeWidth;
	int bgShapeHeight;
	COLORREF bgShapeColor;

	BSTR text;
	BSTR font_name;

	BOOL DateMode;
	BSTR DateMask;
};

struct CVFGraphicLogoMain
{
	UINT32 x;
	UINT32 y;
	DWORD StretchMode;
	int hBmp;
	int TranspLevel;
	COLORREF ColorKey;
	BOOL UseColorKey;
	BSTR Filename;
};

struct CVFDenoiseCAST
{
	int TemporalDifferenceThreshold;		// default 16 - range [0, 255]
	int NumberOfMotionPixelsThreshold;		// default 0 - range [0, 16]
	int StrongEdgeThreshold;				// default 8 - range [0, 255]
	int BlockWidth;						// default 4 - range [1, 16]
	int BlockHeight;						// default 4 - range [1, 16]
	int EdgePixelWeight;					// default 128 - range [0, 255]
	int NonEdgePixelWeight;				// default 16 - range [0, 255]
	int GaussianThresholdY;				// default 12
	int GaussianThresholdUV;				// default 6
	int HistoryWeight;						// default 192 - range [0, 255]
};

struct CVFDeintBlend
{
	int blendThresh1;		// default 5 - range [0, 255]
	int blendThresh2;		// default 9 - range [0, 255]
	double blendConstants1;		// default 0.3 - range [0, 1]
	double blendConstants2;		// default 0.7 - range [0, 1]
};

struct CVFEffect
{
	int Type;
	int ID;	
	__int64 StartTime;
	__int64 StopTime;

	BOOL Enabled;

	int pAmountI;
	int pMinI;
	int pMaxI;
	double pAmountD;
	double pScaleD;
	int pTurbulenceI;
	int pSizeI;
	int pSeamI;
	int pFactorI;
	int pInferenceI;
	int pStyleI;

	int pDenoiseSNRThreshold;
	int pDeintTriangleWeight;
	int pDeintCAVTThreshold;
	int pDenoiseAdaptiveThreshold;
	int pDenoiseAdaptiveBlurMode;

	CVFTextLogoMain TextLogo;
	CVFGraphicLogoMain GraphicLogo;
	CVFDenoiseCAST DenoiseCAST;
	CVFDeintBlend DeintBlend;
};

#ifdef __cplusplus
extern "C" {
#endif

	DECLARE_INTERFACE_(IVFEffects45, IUnknown)
	{
		// add effect
		STDMETHOD(add_effect) (THIS_
					CVFEffect pEffect         
				 ) PURE;

		STDMETHOD(set_effect_settings) (THIS_
			CVFEffect pEffect         // set effect settings
			) PURE;

		STDMETHOD(remove_effect) (THIS_
			int id		         // remove effect
			) PURE;

		STDMETHOD(clear_effects) (void //clear effect list
			) PURE;
	};

	//Callback Interface
	typedef HRESULT (_stdcall *BUFFERCALLBACK) ( 
		void* handle, DWORD handle_id, BYTE * buffer, int bufferLen, int width, int height, LONGLONG startTime, LONGLONG stopTime, BOOL * updateFrame );

	// {9A794ABE-98AD-45AF-BBB0-042172C74C79}
	DEFINE_GUID(IID_IVFEffectsPro, 
		0x9a794abe, 0x98ad, 0x45af, 0xbb, 0xb0, 0x4, 0x21, 0x72, 0xc7, 0x4c, 0x79);
	

	DECLARE_INTERFACE_(IVFEffectsPro, IUnknown)
	{
		STDMETHOD(set_enabled) (THIS_
					BOOL effects,
					BOOL motdet, 
					BOOL chroma, 
					BOOL sg
				 ) PURE;

		STDMETHOD(set_sg_callback_24) ( BUFFERCALLBACK Callback) PURE;
		STDMETHOD(set_sg_callback_32) ( BUFFERCALLBACK Callback) PURE;

		//STDMETHOD(set_settings) (THIS_
		//			BOOL _allow_vih2_input,
		//			BOOL _force_vih1_output,
		//			BOOL _force_yuy2_output
		//		 ) PURE;

		STDMETHOD (put_sg_app_handle) ( void* handle ) PURE;
		STDMETHOD (put_sg_app_handle_id) ( DWORD handle_id ) PURE;
	};
	

	//Callback Interface	
	typedef int (__stdcall *SAMPLECALLBACK) ( void* handle, BYTE * buffer, int bufferLen, int similarity );

	// {A77713DE-E16F-4f64-AFE4-27F536B3F4EC}
	DEFINE_GUID(IID_IVFMotDetConfig45, 
		0xa77713de, 0xe16f, 0x4f64, 0xaf, 0xe4, 0x27, 0xf5, 0x36, 0xb3, 0xf4, 0xec);


	DECLARE_INTERFACE_(IVFMotDetConfig45, IUnknown)
	{
		//STDMETHOD (SetCallBack) ( IVFMotDetCallback * pCallbackInterface ) PURE;
		STDMETHOD (motdet_set_callback) ( SAMPLECALLBACK Callback) PURE;
		//STDMETHOD (put_enabled) ( BOOL enabled_ ) PURE;
		STDMETHOD (motdet_put_CHL_enabled) ( BOOL enabled_ ) PURE;
		STDMETHOD (motdet_put_CHL_color) ( int color_ ) PURE;
		STDMETHOD (motdet_put_CHL_threshold) ( int threshold_ ) PURE;
		STDMETHOD (motdet_put_lines_x) ( int value_ ) PURE;
		STDMETHOD (motdet_put_lines_y) ( int value_ ) PURE;
		STDMETHOD (motdet_put_drop_frames_enabled) ( BOOL enabled_ ) PURE;
		STDMETHOD (motdet_put_drop_frames_threshold) ( int value_ ) PURE;
		STDMETHOD (motdet_put_frame_interval) ( int value_ ) PURE;
		STDMETHOD (motdet_put_compare_mode) ( BOOL red, BOOL green, BOOL blue, BOOL greyscale ) PURE;
		STDMETHOD (motdet_put_app_handle) ( void* handle ) PURE;
	};

enum CVFChromaColor
{
	Chroma_Red,
	Chroma_Green,
	Chroma_Blue
};


	// {AF6E8208-30E3-44f0-AAFE-787A6250BAB3}
	DEFINE_GUID(IID_IVFChromaKey, 
		0xaf6e8208, 0x30e3, 0x44f0, 0xaa, 0xfe, 0x78, 0x7a, 0x62, 0x50, 0xba, 0xb3);
	
	DECLARE_INTERFACE_(IVFChromaKey, IUnknown)
	{
		STDMETHOD(chroma_put_contrast) (THIS_
			int low,
			int high
			) PURE;

		STDMETHOD(chroma_put_color) (THIS_
			int color
			) PURE;

		STDMETHOD(chroma_put_image) (THIS_
			BSTR filename
			) PURE;
	};

#ifdef __cplusplus
}
#endif


#endif // __IEZ__

