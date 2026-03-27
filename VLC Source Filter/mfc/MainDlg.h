//////////////////////////////////////////////////////
// MainDlg.h - Main dialog header
//
// MFC dialog-based demo for VisioForge VLC Source.
// Demonstrates: file/stream playback, video preview,
// pause/resume, and timeline seeking.
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

    afx_msg void OnBnClickedStart();
    afx_msg void OnBnClickedStop();
    afx_msg void OnBnClickedPause();
    afx_msg void OnBnClickedResume();
    afx_msg void OnBnClickedBrowse();
    afx_msg void OnSize(UINT nType, int cx, int cy);
    afx_msg void OnDestroy();
    afx_msg void OnTimer(UINT_PTR nIDEvent);
    afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);

    DECLARE_MESSAGE_MAP()

private:
    // UI controls
    CEdit       m_editFilename;
    CStatic     m_videoPanel;
    CStatic     m_staticTime;
    CStatic     m_staticStatus;
    CSliderCtrl m_sliderTimeline;

    // DirectShow interfaces
    IFilterGraph2*          m_pGraph;
    ICaptureGraphBuilder2*  m_pCapture;
    IMediaControl*          m_pMediaControl;
    IMediaSeeking*          m_pMediaSeeking;
    IVideoWindow*           m_pVideoWindow;
    IBaseFilter*            m_pSourceFilter;

    // Timer
    static const UINT_PTR TIMER_PROGRESS = 1;
    bool m_bSeeking;

    // Helpers
    void SetStatus(LPCTSTR text);
    void ResizeVideoWindow();
    void ReleaseGraph();
    void UpdateTimeDisplay();

    static void FreeMediaType(AM_MEDIA_TYPE& mt);
    static void DeleteMediaType(AM_MEDIA_TYPE* pmt);
};
