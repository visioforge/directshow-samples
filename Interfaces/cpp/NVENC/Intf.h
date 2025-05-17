

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0603 */
/* at Fri Jun 17 18:26:16 2016
 */
/* Compiler settings for Inc\nv264.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.00.0603 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __nv264_h_h__
#define __nv264_h_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __INVEncConfig_FWD_DEFINED__
#define __INVEncConfig_FWD_DEFINED__
typedef interface INVEncConfig INVEncConfig;

#endif 	/* __INVEncConfig_FWD_DEFINED__ */


#ifndef __INVEncConfig_FWD_DEFINED__
#define __INVEncConfig_FWD_DEFINED__
typedef interface INVEncConfig INVEncConfig;

#endif 	/* __INVEncConfig_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "strmif.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __INVEncConfig_INTERFACE_DEFINED__
#define __INVEncConfig_INTERFACE_DEFINED__

/* interface INVEncConfig */
/* [unique][helpstring][uuid][object] */ 


//EXTERN_C const IID IID_INVEncConfig;

// {9A2AC42C-3E3D-4E6A-84E5-D097292D496B}
static const GUID IID_INVEncConfig =
{ 0x9a2ac42c, 0x3e3d, 0x4e6a,{ 0x84, 0xe5, 0xd0, 0x97, 0x29, 0x2d, 0x49, 0x6b } };


#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("9A2AC42C-3E3D-4E6A-84E5-D097292D496B")
    INVEncConfig : public IAMVideoCompression
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetDeviceType( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetDeviceType( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetPictureStructure( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetPictureStructure( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetNumBuffers( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetNumBuffers( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetRateControl( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetRateControl( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetPreset( 
            /* [in] */ GUID v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetPreset( 
            /* [out] */ GUID *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetQp( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetQp( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetBFrames( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetBFrames( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetGOP( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetGOP( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetBitrate( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetBitrate( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetVbvBitrate( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVbvBitrate( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetVbvSize( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVbvSize( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetProfile( 
            /* [in] */ GUID v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetProfile( 
            /* [out] */ GUID *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetLevel( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetLevel( 
            /* [out] */ int *v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetCodec( 
            /* [in] */ int v) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetCodec( 
            /* [out] */ int *v) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct INVEncConfigVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            INVEncConfig * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            INVEncConfig * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            INVEncConfig * This);
        
        HRESULT ( STDMETHODCALLTYPE *put_KeyFrameRate )( 
            INVEncConfig * This,
            /* [in] */ long KeyFrameRate);
        
        HRESULT ( STDMETHODCALLTYPE *get_KeyFrameRate )( 
            INVEncConfig * This,
            /* [annotation][out] */ 
            _Out_  long *pKeyFrameRate);
        
        HRESULT ( STDMETHODCALLTYPE *put_PFramesPerKeyFrame )( 
            INVEncConfig * This,
            /* [in] */ long PFramesPerKeyFrame);
        
        HRESULT ( STDMETHODCALLTYPE *get_PFramesPerKeyFrame )( 
            INVEncConfig * This,
            /* [annotation][out] */ 
            _Out_  long *pPFramesPerKeyFrame);
        
        HRESULT ( STDMETHODCALLTYPE *put_Quality )( 
            INVEncConfig * This,
            /* [in] */ double Quality);
        
        HRESULT ( STDMETHODCALLTYPE *get_Quality )( 
            INVEncConfig * This,
            /* [annotation][out] */ 
            _Out_  double *pQuality);
        
        HRESULT ( STDMETHODCALLTYPE *put_WindowSize )( 
            INVEncConfig * This,
            /* [in] */ DWORDLONG WindowSize);
        
        HRESULT ( STDMETHODCALLTYPE *get_WindowSize )( 
            INVEncConfig * This,
            /* [annotation][out] */ 
            _Out_  DWORDLONG *pWindowSize);
        
        HRESULT ( STDMETHODCALLTYPE *GetInfo )( 
            INVEncConfig * This,
            /* [annotation][size_is][out] */ 
            _Out_writes_bytes_opt_(*pcbVersion)  LPWSTR pszVersion,
            /* [annotation][out][in] */ 
            _Inout_opt_  int *pcbVersion,
            /* [annotation][size_is][out] */ 
            _Out_writes_bytes_opt_(*pcbDescription)  LPWSTR pszDescription,
            /* [annotation][out][in] */ 
            _Inout_opt_  int *pcbDescription,
            /* [annotation][out] */ 
            _Out_opt_  long *pDefaultKeyFrameRate,
            /* [annotation][out] */ 
            _Out_opt_  long *pDefaultPFramesPerKey,
            /* [annotation][out] */ 
            _Out_opt_  double *pDefaultQuality,
            /* [annotation][out] */ 
            _Out_opt_  long *pCapabilities);
        
        HRESULT ( STDMETHODCALLTYPE *OverrideKeyFrame )( 
            INVEncConfig * This,
            /* [in] */ long FrameNumber);
        
        HRESULT ( STDMETHODCALLTYPE *OverrideFrameSize )( 
            INVEncConfig * This,
            /* [in] */ long FrameNumber,
            /* [in] */ long Size);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetDeviceType )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetDeviceType )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetPictureStructure )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetPictureStructure )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetNumBuffers )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetNumBuffers )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetRateControl )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetRateControl )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetPreset )( 
            INVEncConfig * This,
            /* [in] */ GUID v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetPreset )( 
            INVEncConfig * This,
            /* [out] */ GUID *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetQp )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetQp )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetBFrames )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetBFrames )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetGOP )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetGOP )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetBitrate )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetBitrate )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetVbvBitrate )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetVbvBitrate )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetVbvSize )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetVbvSize )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetProfile )( 
            INVEncConfig * This,
            /* [in] */ GUID v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetProfile )( 
            INVEncConfig * This,
            /* [out] */ GUID *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetLevel )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetLevel )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetCodec )( 
            INVEncConfig * This,
            /* [in] */ int v);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetCodec )( 
            INVEncConfig * This,
            /* [out] */ int *v);
        
        END_INTERFACE
    } INVEncConfigVtbl;

    interface INVEncConfig
    {
        CONST_VTBL struct INVEncConfigVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define INVEncConfig_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define INVEncConfig_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define INVEncConfig_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define INVEncConfig_put_KeyFrameRate(This,KeyFrameRate)	\
    ( (This)->lpVtbl -> put_KeyFrameRate(This,KeyFrameRate) ) 

#define INVEncConfig_get_KeyFrameRate(This,pKeyFrameRate)	\
    ( (This)->lpVtbl -> get_KeyFrameRate(This,pKeyFrameRate) ) 

#define INVEncConfig_put_PFramesPerKeyFrame(This,PFramesPerKeyFrame)	\
    ( (This)->lpVtbl -> put_PFramesPerKeyFrame(This,PFramesPerKeyFrame) ) 

#define INVEncConfig_get_PFramesPerKeyFrame(This,pPFramesPerKeyFrame)	\
    ( (This)->lpVtbl -> get_PFramesPerKeyFrame(This,pPFramesPerKeyFrame) ) 

#define INVEncConfig_put_Quality(This,Quality)	\
    ( (This)->lpVtbl -> put_Quality(This,Quality) ) 

#define INVEncConfig_get_Quality(This,pQuality)	\
    ( (This)->lpVtbl -> get_Quality(This,pQuality) ) 

#define INVEncConfig_put_WindowSize(This,WindowSize)	\
    ( (This)->lpVtbl -> put_WindowSize(This,WindowSize) ) 

#define INVEncConfig_get_WindowSize(This,pWindowSize)	\
    ( (This)->lpVtbl -> get_WindowSize(This,pWindowSize) ) 

#define INVEncConfig_GetInfo(This,pszVersion,pcbVersion,pszDescription,pcbDescription,pDefaultKeyFrameRate,pDefaultPFramesPerKey,pDefaultQuality,pCapabilities)	\
    ( (This)->lpVtbl -> GetInfo(This,pszVersion,pcbVersion,pszDescription,pcbDescription,pDefaultKeyFrameRate,pDefaultPFramesPerKey,pDefaultQuality,pCapabilities) ) 

#define INVEncConfig_OverrideKeyFrame(This,FrameNumber)	\
    ( (This)->lpVtbl -> OverrideKeyFrame(This,FrameNumber) ) 

#define INVEncConfig_OverrideFrameSize(This,FrameNumber,Size)	\
    ( (This)->lpVtbl -> OverrideFrameSize(This,FrameNumber,Size) ) 


#define INVEncConfig_SetDeviceType(This,v)	\
    ( (This)->lpVtbl -> SetDeviceType(This,v) ) 

#define INVEncConfig_GetDeviceType(This,v)	\
    ( (This)->lpVtbl -> GetDeviceType(This,v) ) 

#define INVEncConfig_SetPictureStructure(This,v)	\
    ( (This)->lpVtbl -> SetPictureStructure(This,v) ) 

#define INVEncConfig_GetPictureStructure(This,v)	\
    ( (This)->lpVtbl -> GetPictureStructure(This,v) ) 

#define INVEncConfig_SetNumBuffers(This,v)	\
    ( (This)->lpVtbl -> SetNumBuffers(This,v) ) 

#define INVEncConfig_GetNumBuffers(This,v)	\
    ( (This)->lpVtbl -> GetNumBuffers(This,v) ) 

#define INVEncConfig_SetRateControl(This,v)	\
    ( (This)->lpVtbl -> SetRateControl(This,v) ) 

#define INVEncConfig_GetRateControl(This,v)	\
    ( (This)->lpVtbl -> GetRateControl(This,v) ) 

#define INVEncConfig_SetPreset(This,v)	\
    ( (This)->lpVtbl -> SetPreset(This,v) ) 

#define INVEncConfig_GetPreset(This,v)	\
    ( (This)->lpVtbl -> GetPreset(This,v) ) 

#define INVEncConfig_SetQp(This,v)	\
    ( (This)->lpVtbl -> SetQp(This,v) ) 

#define INVEncConfig_GetQp(This,v)	\
    ( (This)->lpVtbl -> GetQp(This,v) ) 

#define INVEncConfig_SetBFrames(This,v)	\
    ( (This)->lpVtbl -> SetBFrames(This,v) ) 

#define INVEncConfig_GetBFrames(This,v)	\
    ( (This)->lpVtbl -> GetBFrames(This,v) ) 

#define INVEncConfig_SetGOP(This,v)	\
    ( (This)->lpVtbl -> SetGOP(This,v) ) 

#define INVEncConfig_GetGOP(This,v)	\
    ( (This)->lpVtbl -> GetGOP(This,v) ) 

#define INVEncConfig_SetBitrate(This,v)	\
    ( (This)->lpVtbl -> SetBitrate(This,v) ) 

#define INVEncConfig_GetBitrate(This,v)	\
    ( (This)->lpVtbl -> GetBitrate(This,v) ) 

#define INVEncConfig_SetVbvBitrate(This,v)	\
    ( (This)->lpVtbl -> SetVbvBitrate(This,v) ) 

#define INVEncConfig_GetVbvBitrate(This,v)	\
    ( (This)->lpVtbl -> GetVbvBitrate(This,v) ) 

#define INVEncConfig_SetVbvSize(This,v)	\
    ( (This)->lpVtbl -> SetVbvSize(This,v) ) 

#define INVEncConfig_GetVbvSize(This,v)	\
    ( (This)->lpVtbl -> GetVbvSize(This,v) ) 

#define INVEncConfig_SetProfile(This,v)	\
    ( (This)->lpVtbl -> SetProfile(This,v) ) 

#define INVEncConfig_GetProfile(This,v)	\
    ( (This)->lpVtbl -> GetProfile(This,v) ) 

#define INVEncConfig_SetLevel(This,v)	\
    ( (This)->lpVtbl -> SetLevel(This,v) ) 

#define INVEncConfig_GetLevel(This,v)	\
    ( (This)->lpVtbl -> GetLevel(This,v) ) 

#define INVEncConfig_SetCodec(This,v)	\
    ( (This)->lpVtbl -> SetCodec(This,v) ) 

#define INVEncConfig_GetCodec(This,v)	\
    ( (This)->lpVtbl -> GetCodec(This,v) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __INVEncConfig_INTERFACE_DEFINED__ */



#ifndef __NVEncLib_LIBRARY_DEFINED__
#define __NVEncLib_LIBRARY_DEFINED__

/* library NVEncLib */
/* [helpstring][version][uuid] */ 



EXTERN_C const IID LIBID_NVEncLib;
#endif /* __NVEncLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


