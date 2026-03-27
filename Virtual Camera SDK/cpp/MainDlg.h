//////////////////////////////////////////////////////
// MainDlg.h - Main dialog header
//
// MFC dialog-based demo for VisioForge Virtual Camera.
// Two independent filter graphs:
//   Source: real webcam -> VF Virtual Camera Sink
//   Camera: VF Virtual Camera Source -> preview/capture
//////////////////////////////////////////////////////
#pragma once

#include <dshow.h>

class CMainDlg : public CDialogEx
{
public:
    CMainDlg(CWnd* pParent = nullptr);
    virtual ~CMainDlg();

    enum { IDD = IDD_MAINDLG };

protected:
    virtual void DoDataExchange(CDataExchange* pDX);
    virtual BOOL OnInitDialog();

    // Source section handlers
    afx_msg void OnBnClickedSourceStart();
    afx_msg void OnBnClickedSourceStop();
    afx_msg void OnCbnSelchangeSourceDevice();

    // Camera section handlers
    afx_msg void OnBnClickedCameraPreview();
    afx_msg void OnBnClickedCameraCapture();
    afx_msg void OnBnClickedCameraStop();
    afx_msg void OnBnClickedBrowse();

    afx_msg void OnSize(UINT nType, int cx, int cy);
    afx_msg void OnDestroy();

    DECLARE_MESSAGE_MAP()

private:
    // ======== Source graph (webcam -> virtual camera sink) ========
    IFilterGraph2*          m_pSrcGraph;
    ICaptureGraphBuilder2*  m_pSrcCapture;
    IMediaControl*          m_pSrcMediaControl;
    IVideoWindow*           m_pSrcVideoWindow;
    IBaseFilter*            m_pSrcWebcamFilter;
    IBaseFilter*            m_pSrcAudioFilter;
    IBaseFilter*            m_pSrcVideoSink;
    IBaseFilter*            m_pSrcAudioSink;
    IBaseFilter*            m_pSrcSmartTee;

    CComboBox m_comboSourceDevice;
    CComboBox m_comboSourceFormat;
    CComboBox m_comboSourceFrameRate;
    CStatic   m_srcVideoPanel;

    struct DeviceInfo
    {
        CString name;
        IMoniker* pMoniker;
    };
    CArray<DeviceInfo> m_videoDevices;
    CArray<DeviceInfo> m_audioDevices;

    void EnumerateVideoDevices();
    void EnumerateSourceFormats();
    bool SetSourceVideoFormat();
    void ResizeSourceVideoWindow();
    void ReleaseSourceGraph();

    // ======== Camera graph (virtual camera -> preview/capture) ========
    IFilterGraph2*          m_pCamGraph;
    ICaptureGraphBuilder2*  m_pCamCapture;
    IMediaControl*          m_pCamMediaControl;
    IVideoWindow*           m_pCamVideoWindow;
    IBaseFilter*            m_pCamCameraFilter;
    IBaseFilter*            m_pCamAudioFilter;

    // Capture-specific filters
    IBaseFilter*            m_pCamVideoSmartTee;
    IBaseFilter*            m_pCamAudioSmartTee;
    IBaseFilter*            m_pCamVideoCodec;
    IBaseFilter*            m_pCamAudioCodec;
    IBaseFilter*            m_pCamAVIMux;
    IBaseFilter*            m_pCamFileWriter;
    IBaseFilter*            m_pCamAudioRenderer;

    CComboBox m_comboCameraFormat;
    CComboBox m_comboCameraFrameRate;
    CStatic   m_camVideoPanel;
    CStatic   m_staticStatus;

    void EnumerateCameraFormats();
    bool SetCameraVideoFormat(int formatIndex, double frameRate);
    bool BuildCameraCommonGraph(double fps, int capsIndex);
    void ResizeCameraVideoWindow();
    void ReleaseCameraGraph();

    void SetStatus(LPCTSTR text);

    static void FreeMediaType(AM_MEDIA_TYPE& mt);
    static void DeleteMediaType(AM_MEDIA_TYPE* pmt);
};
