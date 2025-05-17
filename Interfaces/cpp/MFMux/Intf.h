#pragma once
#include <unknwn.h>

#include "ExportsTypes.h"

MIDL_INTERFACE("3D7A8BA3-AB22-44E2-9CF1-6F98F4CBACEC")
IMFMuxConfig : public IUnknown
{
public:
	virtual HRESULT STDMETHODCALLTYPE SetVideoEncoderSettings(VFMFVideoEncoderSettings settings) = 0;
	virtual HRESULT STDMETHODCALLTYPE SetAudioEncoderSettings(VFMFAudioEncoderSettings settings) = 0;
	virtual HRESULT STDMETHODCALLTYPE SetErrorCallback(ERRORCALLBACK callback) = 0;
	virtual HRESULT STDMETHODCALLTYPE SetMuxer(VFMFMuxer muxer) = 0;
	virtual HRESULT STDMETHODCALLTYPE SetHLSMuxerSettings(VFHLSSettings settings) = 0;
	virtual HRESULT STDMETHODCALLTYPE SeparateCaptureSetMode(BOOL enable, BOOL autostart) = 0;
	virtual HRESULT STDMETHODCALLTYPE SeparateCaptureStart() = 0;
	virtual HRESULT STDMETHODCALLTYPE SeparateCaptureStop() = 0;
	virtual HRESULT STDMETHODCALLTYPE SeparateCaptureStartNewFile(LPCWSTR filename) = 0;
	virtual HRESULT STDMETHODCALLTYPE SeparateCapturePause() = 0;
	virtual HRESULT STDMETHODCALLTYPE SeparateCaptureResume() = 0;
};