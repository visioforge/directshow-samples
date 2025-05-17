//////////////////////////////////////////////////////
#pragma once
//////////////////////////////////////////////////////
#include <initguid.h>
//////////////////////////////////////////////////////
// {80DF6C2F-83E7-4217-801B-29BB4BF0C77F}
DEFINE_GUID(IID_IRTSPSinkConfig, 
0x80df6c2f, 0x83e7, 0x4217, 0x80, 0x1b, 0x29, 0xbb, 0x4b, 0xf0, 0xc7, 0x7f);
//////////////////////////////////////////////////////
DECLARE_INTERFACE_(IRTSPSinkConfig,IUnknown)
{
	STDMETHOD(set_URL)(LPCOLESTR pszURL)PURE;
	STDMETHOD(get_URL)(LPOLESTR * ppszURL)PURE;
	STDMETHOD(set_Info)(LPCOLESTR pszInfo)PURE;
	STDMETHOD(get_Info)(LPOLESTR * ppszInfo)PURE;
	STDMETHOD(set_Description)(LPCOLESTR pszDescription)PURE;
	STDMETHOD(get_Description)(LPOLESTR * ppszDescription)PURE;
	STDMETHOD(set_TTL)(int _ttl)PURE;
	STDMETHOD(get_TTL)(int * _ttl)PURE;
	STDMETHOD(set_SSM)(BOOL _ssm)PURE;
	STDMETHOD(get_SSM)(BOOL * _ssm)PURE;
	STDMETHOD(set_StartRTPPort)(int _port)PURE;
	STDMETHOD(get_StartRTPPort)(int * _port)PURE;
	STDMETHOD(set_Port)(int _port)PURE;
	STDMETHOD(get_Port)(int * _port)PURE;
	STDMETHOD(get_BroadcastIP)(LPOLESTR * ppszIP)PURE;
	STDMETHOD(set_BroadcastIP)(LPOLESTR pszIP)PURE;
};
//////////////////////////////////////////////////////
