#pragma once
#include <rpcndr.h>
#include <intsafe.h>
#include <objbase.h>

#ifdef __cplusplus
extern "C" {
#endif 

// {2A741FB6-6DE1-460B-8FCA-76DB478C9357}
DEFINE_GUID(IID_INVEncConfig2,
		0x2a741fb6, 0x6de1, 0x460b, 0x8f, 0xca, 0x76, 0xdb, 0x47, 0x8c, 0x93, 0x57);


DECLARE_INTERFACE_(INVEncConfig2, IUnknown)
{
	STDMETHOD(CheckNVENCAvailable) (THIS_ BOOL* result, /*NVENCSTATUS*/ int* status) PURE;
};


#ifdef __cplusplus
}
#endif

