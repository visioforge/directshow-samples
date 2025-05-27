#pragma once

#include <initguid.h>
#include <comdef.h>

// {C5255DE3-50A7-4714-B763-D99E96E4CD52}
DEFINE_GUID(CLSID_FFmpegSourceFilter,
	0xc5255de3, 0x50a7, 0x4714, 0xb7, 0x63, 0xd9, 0x9e, 0x96, 0xe4, 0xcd, 0x52);

DEFINE_GUID(IID_IFFmpegSourceSettings,
0x1974D893, 0x83E4, 0x4F89, 0x99, 0x08, 0x79, 0x5C, 0x52, 0x4C, 0xC1, 0x7E);

enum FFMPEG_SOURCE_BUFFERING_MODE {
	FFMPEG_SOURCE_BUFFERING_MODE_AUTO,
	FFMPEG_SOURCE_BUFFERING_MODE_ON,
	FFMPEG_SOURCE_BUFFERING_MODE_OFF
};

// Data callback Interface
typedef HRESULT(_stdcall* FFMPEGDataCallbackDelegate) (
	BYTE* buffer, int bufferLen, int dataType, LONGLONG startTime, LONGLONG stopTime);

// Timestamp callback Interface
typedef HRESULT(_stdcall* FFMPEGTimestampCallbackDelegate) (
	int mediaType, __int64 demuxerStartTime, __int64 streamStartTimr, __int64 timestamp);

MIDL_INTERFACE("1974D893-83E4-4F89-9908-795C524CC17E")
IFFmpegSourceSettings : IUnknown
{
	STDMETHOD_(BOOL, GetHWAccelerationEnabled)() = 0;

	/// <summary>
	/// If hardware acceleration is enabled, the filter will try to use 
	/// hardware video decoding when possible.
	/// It must be set before connecting downstream video filter.
	/// Default value is TRUE.
	/// </summary>
	/// <returns></returns>
	STDMETHOD(SetHWAccelerationEnabled)(BOOL enabled) = 0;

	STDMETHOD_(DWORD, GetLoadTimeOut)() = 0;

	/// <summary>
	/// Sets the time out during the source loading in milliseconds.
	/// It must be set before loading the source.
	/// Default value is 15000 (15 seconds).
	/// </summary>
	/// <param name="milliseconds"></param>
	/// <returns></returns>
	STDMETHOD(SetLoadTimeOut)(DWORD milliseconds) = 0;

	STDMETHOD_(FFMPEG_SOURCE_BUFFERING_MODE, GetBufferingMode)() = 0;
	/// <summary>
	/// Sets the buffering mode for live sources. 
	/// It must be set before loading the source.
	/// AUTO: Detect if buffering is required by the source.
	/// ON: Allow buffering.
	/// OFF: Do not allow buffering.
	/// Default value is AUTO.
	/// </summary>
	/// <param name="mode"></param>
	/// <returns></returns>
	STDMETHOD(SetBufferingMode)(FFMPEG_SOURCE_BUFFERING_MODE mode) = 0;

	/// <summary>
	/// Sets a custom option for the source.
	/// It must be set before loading the source.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	STDMETHOD(SetCustomOption)(LPSTR name, LPSTR value) = 0;

	/// <summary>
	/// Clears all custom options.
	/// It must be set before loading the source.
	/// </summary>
	STDMETHOD(ClearCustomOptions)() = 0;
	
	/// <summary>
	/// Sets the data callback.
	/// </summary>
	/// <param name="callback">The callback.</param>
	/// <returns>HRESULT .</returns>
	STDMETHOD(SetDataCallback) (FFMPEGDataCallbackDelegate callback) = 0;

	/// <summary>
	/// Sets the timestamp callback.
	/// </summary>
	/// <param name="callback">The callback.</param>
	/// <returns>HRESULT .</returns>
	STDMETHOD(SetTimestampCallback) (FFMPEGTimestampCallbackDelegate callback) = 0;
	
	/// <summary>
	/// Sets the audio enabled.
	/// </summary>
	/// <param name="enabled">The enabled.</param>
	/// <returns>HRESULT .</returns>
	STDMETHOD(SetAudioEnabled) (BOOL enabled) = 0;
};

_COM_SMARTPTR_TYPEDEF(IFFmpegSourceSettings, __uuidof(IFFmpegSourceSettings));
