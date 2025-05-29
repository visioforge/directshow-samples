#ifndef __IVLCSRC__
#define __IVLCSRC__

#ifdef __cplusplus
extern "C" {
#endif

	// {77493EB7-6D00-41C5-9535-7C593824E892}
	DEFINE_GUID(IID_IVlcSrc, 
	0x77493eb7, 0x6d00, 0x41c5, 0x95, 0x35, 0x7c, 0x59, 0x38, 0x24, 0xe8, 0x92);


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

	// {CCE122C0-172C-4626-B4B6-42B039E541CB}
	DEFINE_GUID(IID_IVlcSrc2,
		0xcce122c0, 0x172c, 0x4626, 0xb4, 0xb6, 0x42, 0xb0, 0x39, 0xe5, 0x41, 0xcb);

	DECLARE_INTERFACE_(IVlcSrc2, IVlcSrc)
	{
		STDMETHOD(SetCustomCommandLine) (THIS_
			char* params[],
			int length
			) PURE;
	};

	// {3DFBED0C-E4A8-401C-93EF-CBBFB65223DD}
	DEFINE_GUID(IID_IVlcSrc3,
		0x3dfbed0c, 0xe4a8, 0x401c, 0x93, 0xef, 0xcb, 0xbf, 0xb6, 0x52, 0x23, 0xdd);

	DECLARE_INTERFACE_(IVlcSrc3, IVlcSrc2)
	{
		STDMETHOD(SetDefaultFrameRate) (THIS_			
			double frameRate
			) PURE;
	};

#ifdef __cplusplus
}
#endif

#endif // __IVLCSRC__

