#ifndef __ICUDAH264__
#define __ICUDAH264__
#include <combaseapi.h>

#ifdef __cplusplus
extern "C" {
	extern "C" {
#endif

		enum rate_control : int
		{
			VBR = 1,
			CBR = 2,
		};

		enum mb_encoding : int
		{
			CAVLC = 0,
			CABAC = 1,
		};

		enum profile_idc : int
		{
			PROFILE_AUTO = 0,
			PROFILE_BASELINE = 66,
			PROFILE_MAIN = 77,
			PROFILE_HIGH = 100,
		};

		enum level_idc : int
		{
			LEVEL_AUTO = 0,
			LEVEL_1 = 10,
			LEVEL_1_1 = 11,
			LEVEL_1_2 = 12,
			LEVEL_1_3 = 13,
			LEVEL_2 = 20,
			LEVEL_2_1 = 21,
			LEVEL_2_2 = 22,
			LEVEL_3 = 30,
			LEVEL_3_1 = 31,
			LEVEL_3_2 = 32,
			LEVEL_4 = 40,
			LEVEL_4_1 = 41,
			LEVEL_4_2 = 42,
			LEVEL_5 = 50,
			LEVEL_5_1 = 51
		};

		// { 8D3913F9-C92D-4361-81A7-07C1E36232EC }
		DEFINE_GUID(IID_ICUDAH264Encoder,
			0x8D3913F9, 0xC92D, 0x4361, 0x81, 0xA7, 0x07, 0xC1, 0xE3, 0x62, 0x32, 0xEC);

		// { F3FBEAE6-B7DE-425D-88EA-E4D9D3DAFC96 }
		DEFINE_GUID(CLSID_VFCUDAH264Encoder,
			0xF3FBEAE6, 0xB7DE, 0x425D, 0x88, 0xEA, 0xE4, 0xD9, 0xD3, 0xDA, 0xFC, 0x96);

		DECLARE_INTERFACE_(ICUDAH264Encoder, IUnknown)
		{
			STDMETHOD(get_Bitrate) (THIS_
				int* plValue
				) PURE;

			STDMETHOD(put_Bitrate) (THIS_
				int lValue
				) PURE;

			STDMETHOD(get_RateControl) (THIS_
				rate_control* plValue
				) PURE;

			STDMETHOD(put_RateControl) (THIS_
				rate_control lValue
				) PURE;

			STDMETHOD(get_MbEncoding) (THIS_
				mb_encoding* plValue
				) PURE;

			STDMETHOD(put_MbEncoding) (THIS_
				mb_encoding lValue
				) PURE;

			STDMETHOD(get_Deblocking) (THIS_
				BOOL* plValue
				) PURE;

			STDMETHOD(put_Deblocking) (THIS_
				BOOL lValue
				) PURE;

			STDMETHOD(get_GOP) (THIS_
				BOOL* plValue
				) PURE;

			STDMETHOD(put_GOP) (THIS_
				BOOL lValue
				) PURE;

			STDMETHOD(get_AutoBitrate) (THIS_
				BOOL* plValue
				) PURE;

			STDMETHOD(put_AutoBitrate) (THIS_
				BOOL lValue
				) PURE;

			STDMETHOD(get_Profile) (THIS_
				profile_idc* plValue
				) PURE;

			STDMETHOD(put_Profile) (THIS_
				profile_idc lValue
				) PURE;

			STDMETHOD(get_Level) (THIS_
				level_idc* plValue
				) PURE;

			STDMETHOD(put_Level) (THIS_
				level_idc lValue
				) PURE;

			STDMETHOD(get_SliceIntervals) (THIS_
				int* piIDR, int* piP
				) PURE;

			STDMETHOD(put_SliceIntervals) (THIS_
				int *piIDR, int *piP
				) PURE;
		};
	}

#ifdef __cplusplus
}
#endif

#endif // __ICUDAH264__