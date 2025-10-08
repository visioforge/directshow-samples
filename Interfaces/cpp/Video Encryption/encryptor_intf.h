// Interfaces to configure filter

///////////////////////////////////////////////////////
#pragma once
///////////////////////////////////////////////////////
#include <initguid.h>
///////////////////////////////////////////////////////
#ifdef __cplusplus
extern "C" {
#endif

//////////////////////////////////////////////////////
// {F1D3727A-88DE-49ab-A635-280BEFEFF902}
DEFINE_GUID(CLSID_EncryptMuxer, 
0xf1d3727a, 0x88de, 0x49ab, 0xa6, 0x35, 0x28, 0xb, 0xef, 0xef, 0xf9, 0x2);
//////////////////////////////////////////////////////
// {D2C761F0-9988-4f79-9B0E-FB2B79C65851}
DEFINE_GUID(CLSID_EncryptDemuxer, 
0xd2c761f0, 0x9988, 0x4f79, 0x9b, 0xe, 0xfb, 0x2b, 0x79, 0xc6, 0x58, 0x51);
//////////////////////////////////////////////////////

/// <summary>
/// Filter registration interface.
/// </summary>
DECLARE_INTERFACE_(IVFRegister, IUnknown)
{
	/// <summary>
	/// Sets registered.
	/// </summary>
	/// <param name="licenseKey">
	/// License Key.
	/// </param>
	STDMETHOD(SetLicenseKey)
		(THIS_
		WCHAR * licenseKey
		)PURE;
};

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
// {6F8162B5-778D-42b5-9242-1BBABB24FFC4}
DEFINE_GUID(IID_IPasswordProvider, 
0x6f8162b5, 0x778d, 0x42b5, 0x92, 0x42, 0x1b, 0xba, 0xbb, 0x24, 0xff, 0xc4);
///////////////////////////////////////////////////////
DECLARE_INTERFACE_(IPasswordProvider,IUnknown)
{
	STDMETHOD(QueryPassword)
		(THIS_
			LPCWSTR pszFileName,
			LPBYTE pBuffer,
			LONG * plSize
		)PURE;
};
///////////////////////////////////////////////////////
// {BAA5BD1E-3B30-425e-AB3B-CC20764AC253}
DEFINE_GUID(IID_ICryptoConfig, 
0xbaa5bd1e, 0x3b30, 0x425e, 0xab, 0x3b, 0xcc, 0x20, 0x76, 0x4a, 0xc2, 0x53);
///////////////////////////////////////////////////////
DECLARE_INTERFACE_(ICryptoConfig,IUnknown)
{
	STDMETHOD(put_Provider)
		(THIS_
			IPasswordProvider * pProvider
		)PURE;
	STDMETHOD(get_Provider)
		(THIS_
			IPasswordProvider ** ppProvider
		)PURE;
	STDMETHOD(put_Password)
		(THIS_
			LPBYTE pBuffer,
			LONG lSize
		)PURE;
	STDMETHOD(HavePassword)
		(THIS_
		)PURE;
};
///////////////////////////////////////////////////////

///////////////////////////////////////////////////////
#ifdef __cplusplus
}
#endif
///////////////////////////////////////////////////////