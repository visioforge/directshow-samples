// FFMPEGSourceMFCSample.h - Main dialog header
#pragma once

#include "resource.h"
#include <dshow.h>
#include <initguid.h>
#include <evr.h>
#include <mfapi.h>
#include <mfidl.h>

// ---- Filter CLSID ----
// {C5255DE3-50A7-4714-B763-D99E96E4CD52}
DEFINE_GUID(CLSID_VFFFMPEGSource,
    0xc5255de3, 0x50a7, 0x4714, 0xb7, 0x63, 0xd9, 0x9e, 0x96, 0xe4, 0xcd, 0x52);

// ---- EVR CLSID ----
// {FA10746C-9B63-4B6C-BC49-FC300EA5F256}
DEFINE_GUID(CLSID_EnhancedVideoRenderer,
    0xfa10746c, 0x9b63, 0x4b6c, 0xbc, 0x49, 0xfc, 0x30, 0x0e, 0xa5, 0xf2, 0x56);

// ---- IFFmpegSourceSettings IID ----
// {1974D893-83E4-4F89-9908-795C524CC17E}
DEFINE_GUID(IID_IFFmpegSourceSettings,
    0x1974d893, 0x83e4, 0x4f89, 0x99, 0x08, 0x79, 0x5c, 0x52, 0x4c, 0xc1, 0x7e);

// ---- Interface enum and typedefs ----
enum FFMPEG_SOURCE_BUFFERING_MODE {
    FFMPEG_SOURCE_BUFFERING_MODE_AUTO,
    FFMPEG_SOURCE_BUFFERING_MODE_ON,
    FFMPEG_SOURCE_BUFFERING_MODE_OFF
};

typedef HRESULT(_stdcall* FFMPEGDataCallbackDelegate)(
    BYTE* buffer, int bufferLen, int dataType, LONGLONG startTime, LONGLONG stopTime);

typedef HRESULT(_stdcall* FFMPEGTimestampCallbackDelegate)(
    int mediaType, __int64 demuxerStartTime, __int64 streamStartTime, __int64 timestamp);

// ---- IFFmpegSourceSettings interface ----
DECLARE_INTERFACE_(IFFmpegSourceSettings, IUnknown)
{
    STDMETHOD_(BOOL, GetHWAccelerationEnabled)(THIS) PURE;
    STDMETHOD(SetHWAccelerationEnabled)(THIS_ BOOL enabled) PURE;
    STDMETHOD_(DWORD, GetLoadTimeOut)(THIS) PURE;
    STDMETHOD(SetLoadTimeOut)(THIS_ DWORD milliseconds) PURE;
    STDMETHOD_(FFMPEG_SOURCE_BUFFERING_MODE, GetBufferingMode)(THIS) PURE;
    STDMETHOD(SetBufferingMode)(THIS_ FFMPEG_SOURCE_BUFFERING_MODE mode) PURE;
    STDMETHOD(SetCustomOption)(THIS_ LPSTR name, LPSTR value) PURE;
    STDMETHOD(ClearCustomOptions)(THIS) PURE;
    STDMETHOD(SetDataCallback)(THIS_ FFMPEGDataCallbackDelegate callback) PURE;
    STDMETHOD(SetTimestampCallback)(THIS_ FFMPEGTimestampCallbackDelegate callback) PURE;
    STDMETHOD(SetAudioEnabled)(THIS_ BOOL enabled) PURE;
};

// ---- Timer ID ----
#define TIMER_PROGRESS 1

// ---- Dialog class ----
class CFFMPEGSourceDlg : public CDialogEx
{
public:
    CFFMPEGSourceDlg(CWnd* pParent = nullptr);

#ifdef AFX_DESIGN_TIME
    enum { IDD = IDD_FFMPEGSOURCE_DIALOG };
#endif

protected:
    virtual void DoDataExchange(CDataExchange* pDX);
    virtual BOOL OnInitDialog();

    afx_msg void OnBnClickedStart();
    afx_msg void OnBnClickedPause();
    afx_msg void OnBnClickedResume();
    afx_msg void OnBnClickedStop();
    afx_msg void OnBnClickedBrowse();
    afx_msg void OnCbnSelchangeVideoStream();
    afx_msg void OnCbnSelchangeAudioStream();
    afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
    afx_msg void OnTimer(UINT_PTR nIDEvent);
    afx_msg void OnDestroy();
    DECLARE_MESSAGE_MAP()

private:
    // Graph building
    void BuildGraph();
    void TearDownGraph();
    void Log(const CString& msg);

    // Playback helpers
    LONGLONG GetDurationMs();
    LONGLONG GetPositionMs();
    BOOL SetPositionMs(LONGLONG ms);
    void UpdateVideoWindow();

    // DirectShow interfaces
    IFilterGraph2*          m_pGraph;
    ICaptureGraphBuilder2*  m_pCaptureGraph;
    IMediaControl*          m_pControl;
    IMediaSeeking*          m_pSeeking;
    IMediaEventEx*          m_pEvent;
    IBaseFilter*            m_pSource;
    IBaseFilter*            m_pVideoRenderer;
    IMFVideoDisplayControl* m_pVideoDisplay;

    // State
    BOOL m_bGraphRunning;
    BOOL m_bUpdatingPosition;

    // Controls
    CEdit       m_editFilename;
    CEdit       m_editTimeout;
    CComboBox   m_comboBuffering;
    CButton     m_checkGPU;
    CComboBox   m_comboVideoStream;
    CComboBox   m_comboAudioStream;
    CSliderCtrl m_sliderTimeline;
    CSliderCtrl m_sliderSpeed;
    CStatic     m_staticTime;
    CStatic     m_staticSpeedVal;
    CStatic     m_videoArea;
    CListBox    m_listLog;
};
