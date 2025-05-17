#pragma once

#include <initguid.h>
#include "types.h"

#ifndef _vf_ffmpeg_intf_
#define _vf_ffmpeg_intf_

enum FFOutputFormat {of_FLV, of_MPEG1, of_MPEG1VCD, of_MPEG2, of_MPEG2TS, of_MPEG2SVCD, of_MPEG2DVD, of_MPEG4, of_MPEG4_MP3};

enum video_tv_system_t {video_norm_unknown, video_norm_pal, video_norm_ntsc, video_norm_film};

// {17B8FF7D-A67F-45CE-B425-0E4F607D8C60}
DEFINE_GUID(IID_IFFMPEGEncoder,
			0x17b8ff7d, 0xa67f, 0x45ce, 0xb4, 0x25, 0xe, 0x4f, 0x60, 0x7d, 0x8c, 0x60);

#ifdef __cplusplus
extern "C" {
#endif

	struct CVFOutputSettings
	{
		wchar_t* filename;

		BOOL audioAvailable;
		int audioBitrate;
		int audioSamplerate;
		int audioChannels;

		int videoWidth;
		int videoHeight;
		int aspectRatioW;
		int aspectRatioH;
		int videoBitrate;
		int videoMaxRate;
		int videoMinRate;
		int videoBufferSize;
		BOOL interlace;
		int videoGopSize;
		int tvSystem;

		int outputFormat;
	};

	DECLARE_INTERFACE_(IVFFFMPEGEncoder, IUnknown)
	{
		STDMETHOD(set_settings) (THIS_
			CVFOutputSettings settings // set effect settings
			) PURE;
	};

#ifdef __cplusplus
}
#endif




#endif // _IEZ_
