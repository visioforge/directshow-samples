///////////////////////////////////////////////////////
#pragma once
///////////////////////////////////////////////////////
#include <initguid.h>
///////////////////////////////////////////////////////
#ifdef __cplusplus
extern "C" {
#endif
///////////////////////////////////////////////////////
#define WAVE_FORMAT_AAC		0x00FF
///////////////////////////////////////////////////////
// {B2DE30C0-1441-4451-A0CE-A914FD561D7F}
DEFINE_GUID(IID_IAACEncoder, 
0xb2de30c0, 0x1441, 0x4451, 0xa0, 0xce, 0xa9, 0x14, 0xfd, 0x56, 0x1d, 0x7f);
///////////////////////////////////////////////////////
#define AAC_VERSION_MPEG4		0
#define AAC_VERSION_MPEG2		1
///////////////////////////////////////////////////////
#define AAC_OBJECT_MAIN			1
#define AAC_OBJECT_LOW			2
#define AAC_OBJECT_SSR			3
#define AAC_OBJECT_LTP			4
///////////////////////////////////////////////////////
#define AAC_OUTPUT_RAW			0
#define AAC_OUTPUT_ADTS			1
#define AAC_OUTPUT_LATM			2
///////////////////////////////////////////////////////
struct AACConfig
{
	int			version;
	int			object_type;
	int			output_type;
	int			bitrate;
};
///////////////////////////////////////////////////////
DECLARE_INTERFACE_(IAACEncoder, IUnknown)
{
	STDMETHOD(GetConfig)(AACConfig *config) PURE;
	STDMETHOD(SetConfig)(AACConfig *config) PURE;
};
///////////////////////////////////////////////////////
typedef enum rate_control
{
	CBR = 0,
	VBR = 1,
}_rate_control_t,* p_rate_control_t;

typedef enum mb_encoding
{
	CAVLC = 0,
	CABAC = 1,
}_mb_encoding_t,* p_mb_encoding_t;

typedef enum profile_idc
{
	PROFILE_AUTO	 = 0,
	PROFILE_BASELINE = 66,
	PROFILE_MAIN     = 77,
	PROFILE_HIGH     = 100,
	PROFILE_HIGH10   = 110,
	PROFILE_HIGH422  = 122,
} _profile_idc_t,* p_profile_idc_t;

typedef enum level_idc
{
	LEVEL_AUTO		= 0,
	LEVEL_1			= 10,
	LEVEL_1_1		= 11,
	LEVEL_1_2		= 12,
	LEVEL_1_3		= 13,
	LEVEL_2			= 20,
	LEVEL_2_1		= 21,
	LEVEL_2_2		= 22,
	LEVEL_3			= 30,
	LEVEL_3_1		= 31,
	LEVEL_3_2		= 32,
	LEVEL_4			= 40,
	LEVEL_4_1		= 41,
	LEVEL_4_2		= 42,
	LEVEL_5			= 50,
	LEVEL_5_1		= 51
} _level_idc_t,* p_level_idc_t;

typedef enum target_usage
{
	USAGE_AUTO         = 0,
    USAGE_BEST_QUALITY = 1,
    USAGE_BALANCED     = 4,
    USAGE_BEST_SPEED   = 7
}_usage_t,* p_usage_t;

typedef enum picture_type
{
	PICTURE_AUTO	= 0,
	PICTURE_FRAME	= 1,
	PICTURE_TFF		= 2,
	PICTURE_BFF		= 3,
}_picture_type_t, *p_picture_type_t;

typedef enum time_type
{
	TIME_DEFAULT	= 0,
	TIME_SEQUENTAL	= 1,
	TIME_ENCODED	= 2,
	TIME_AS_INPUT	= TIME_DEFAULT,
} _time_type_t,* p_time_type_t;
///////////////////////////////////////////////////////
// {09FA2EA3-4773-41a8-90DC-9499D4061E9F}
DEFINE_GUID(IID_IH264Encoder, 
0x9fa2ea3, 0x4773, 0x41a8, 0x90, 0xdc, 0x94, 0x99, 0xd4, 0x6, 0x1e, 0x9f);
///////////////////////////////////////////////////////
// IH264Encoder
DECLARE_INTERFACE_(IH264Encoder,IUnknown)
{
	STDMETHOD(get_Bitrate)
		(THIS_
			LONG * plValue
		)PURE;
	STDMETHOD(put_Bitrate)
		(THIS_
			LONG lValue
		)PURE;
	STDMETHOD(get_RateControl)
		(THIS_
			LONG *pValue
		)PURE;
	STDMETHOD(put_RateControl)
		(THIS_
			LONG value
		)PURE;
	STDMETHOD(get_MbEncoding)
		(THIS_
			LONG *pValue
		)PURE;
	STDMETHOD(put_MbEncoding)
		(THIS_
		LONG value
		)PURE;
	STDMETHOD(get_GOP)
		(THIS_
		BOOL * pValue
		)PURE;
	STDMETHOD(put_GOP)
		(THIS_
		BOOL value
		)PURE;
	STDMETHOD(get_AutoBitrate)
		(THIS_
		BOOL * pValue
		)PURE;
	STDMETHOD(put_AutoBitrate)
		(THIS_
		BOOL value
		)PURE;
	STDMETHOD(get_Profile)
		(THIS_
		LONG *pValue
		)PURE;
	STDMETHOD(put_Profile)
		(THIS_
		LONG value
		)PURE;
	STDMETHOD(get_Level)
		(THIS_
		LONG *pValue
		)PURE;
	STDMETHOD(put_Level)
		(THIS_
		LONG value
		)PURE;
	STDMETHOD(get_Usage)
		(THIS_
		LONG *pValue
		)PURE;
	STDMETHOD(put_Usage)
		(THIS_
		LONG value
		)PURE;
	STDMETHOD(get_SequentalTiming)
		(THIS_
		LONG *pTiming
		)PURE;
	STDMETHOD(put_SequentalTiming)
		(THIS_
		LONG _timing
		)PURE;
	STDMETHOD(get_SliceIntervals)
		(THIS_
		int * piIDR,
		int * piP
		)PURE;
	STDMETHOD(put_SliceIntervals)
		(THIS_
		int * piIDR,
		int * piP
		)PURE;
	STDMETHOD(get_MaxBitrate)
		(THIS_
		LONG * plValue
		)PURE;
	STDMETHOD(put_MaxBitrate)
		(THIS_
		LONG lValue
		)PURE;
	STDMETHOD(get_MinBitrate)
		(THIS_
		LONG * plValue
		)PURE;
	STDMETHOD(put_MinBitrate)
		(THIS_
		LONG lValue
		)PURE;
};
///////////////////////////////////////////////////////
#define DECLARE_IH264ENCODER								\
	STDMETHOD(get_Bitrate)(LONG * plValue);					\
	STDMETHOD(put_Bitrate)(LONG lValue);					\
	STDMETHOD(get_RateControl)(LONG *pValue);				\
	STDMETHOD(put_RateControl)(LONG value);					\
	STDMETHOD(get_MbEncoding)(LONG *pValue);					\
	STDMETHOD(put_MbEncoding)(LONG value);					\
	STDMETHOD(get_GOP)(BOOL * pValue);						\
	STDMETHOD(put_GOP)(BOOL value);							\
	STDMETHOD(get_AutoBitrate)(BOOL * pValue);				\
	STDMETHOD(put_AutoBitrate)(BOOL value);					\
	STDMETHOD(get_Profile)(LONG *pValue);			\
	STDMETHOD(put_Profile)(LONG value);			\
	STDMETHOD(get_Level)(LONG *pValue);				\
	STDMETHOD(put_Level)(LONG value);				\
	STDMETHOD(get_Usage)(LONG *pValue);					\
	STDMETHOD(put_Usage)(LONG value);					\
	STDMETHOD(get_SequentalTiming)(LONG *pbValue);	\
	STDMETHOD(put_SequentalTiming)(LONG _value);	\
	STDMETHOD(get_SliceIntervals)(int * piIDR,int * piP);	\
	STDMETHOD(put_SliceIntervals)(int * piIDR,int * piP);   \
	STDMETHOD(get_MaxBitrate)(LONG * plValue);				\
	STDMETHOD(put_MaxBitrate)(LONG lValue);					\
	STDMETHOD(get_MinBitrate)(LONG * plValue);				\
	STDMETHOD(put_MinBitrate)(LONG lValue);

///////////////////////////////////////////////////////
// {99DC9BE5-0AFA-45d4-8370-AB021FB07CF4}
DEFINE_GUID(IID_IMuxerConfig, 
0x99dc9be5, 0xafa, 0x45d4, 0x83, 0x70, 0xab, 0x2, 0x1f, 0xb0, 0x7c, 0xf4);
///////////////////////////////////////////////////////
// IMuxerConfig
DECLARE_INTERFACE_(IMuxerConfig,IUnknown)
{
	STDMETHOD(get_SingleThread)
		(THIS_
			BOOL * pValue
		)PURE;
	STDMETHOD(put_SingleThread)
		(THIS_
			BOOL value
		)PURE;
	STDMETHOD(get_CorrectTiming)
		(THIS_
			BOOL * pValue
		)PURE;
	STDMETHOD(put_CorrectTiming)
		(THIS_
			BOOL value
		)PURE;
};
///////////////////////////////////////////////////////
#ifdef __cplusplus
}
#endif
///////////////////////////////////////////////////////