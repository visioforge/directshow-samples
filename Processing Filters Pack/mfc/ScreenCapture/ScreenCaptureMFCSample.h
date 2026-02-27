// ScreenCaptureMFCSample.h - Main dialog header
#pragma once

#include "resource.h"
#include <dshow.h>
#include <initguid.h>

// ---- Filter CLSID ----
// {0118D5CC-77E4-4199-81B0-548988688261}
DEFINE_GUID(CLSID_VFScreenCapture_4,
    0x118d5cc, 0x77e4, 0x4199, 0x81, 0xb0, 0x54, 0x89, 0x88, 0x68, 0x82, 0x61);

// ---- Interface IIDs ----
// {259E0009-9963-4a71-91AE-34B96D75486F}
DEFINE_GUID(IID_IVFScreenCapture,
    0x259e0009, 0x9963, 0x4a71, 0x91, 0xae, 0x34, 0xb9, 0x6d, 0x75, 0x48, 0x6f);

// {BC91012D-22E0-4091-8C0A-3913BDAB8A42}
DEFINE_GUID(IID_IVFScreenCapture2,
    0xbc91012d, 0x22e0, 0x4091, 0x8c, 0xa, 0x39, 0x13, 0xbd, 0xab, 0x8a, 0x42);

// {D612C76D-4821-4107-A83F-63512CE7EBD7}
DEFINE_GUID(IID_IVFScreenCaptureDD,
    0xd612c76d, 0x4821, 0x4107, 0xa8, 0x3f, 0x63, 0x51, 0x2c, 0xe7, 0xeb, 0xd7);

// ---- Interface structures and enums ----
struct VFRect
{
    INT32 left;
    INT32 top;
    INT32 right;
    INT32 bottom;
};

enum VFScreenCaptureMode { scm_screen, scm_picture, scm_color, scm_window, scm_buffer };

// ---- Interface declarations ----
DECLARE_INTERFACE_(IVFScreenCapture, IUnknown)
{
    STDMETHOD(set_fps)(THIS_ double fps) PURE;
    STDMETHOD(set_rect)(THIS_ VFRect rect) PURE;
    STDMETHOD(set_mouse)(THIS_ bool draw) PURE;
    STDMETHOD(set_screen_index)(THIS_ int index) PURE;
};

DECLARE_INTERFACE_(IVFScreenCapture2, IUnknown)
{
    STDMETHOD(set_mode)(THIS_ VFScreenCaptureMode mode) PURE;
    STDMETHOD(refresh_pic)() PURE;
    STDMETHOD(set_stream)(THIS_ IStream* Stream, __int64 Length) PURE;
    STDMETHOD(set_window_handle)(HWND handle) PURE;
    STDMETHOD(get_window_size)(HWND handle, int* width, int* height) PURE;
};

DECLARE_INTERFACE_(IVFScreenCaptureDD, IUnknown)
{
    STDMETHOD(dd_check)(THIS_ int displayID) PURE;
};

// ---- Dialog class ----
class CScreenCaptureDlg : public CDialogEx
{
public:
    CScreenCaptureDlg(CWnd* pParent = nullptr);

#ifdef AFX_DESIGN_TIME
    enum { IDD = IDD_SCREENCAPTURE_DIALOG };
#endif

protected:
    virtual void DoDataExchange(CDataExchange* pDX);
    virtual BOOL OnInitDialog();

    afx_msg void OnBnClickedStart();
    afx_msg void OnBnClickedStop();
    afx_msg void OnBnClickedPick();
    afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
    afx_msg void OnDestroy();
    DECLARE_MESSAGE_MAP()

private:
    void BuildGraph();
    void TearDownGraph();
    void Log(const CString& msg);

    IGraphBuilder*    m_pGraph;
    IMediaControl*    m_pControl;
    IVideoWindow*     m_pVideoWindow;
    IBaseFilter*      m_pScreenCapture;
    IBaseFilter*      m_pVideoRenderer;

    CComboBox   m_comboMode;
    CEdit       m_editFPS;
    CEdit       m_editLeft;
    CEdit       m_editTop;
    CEdit       m_editRight;
    CEdit       m_editBottom;
    CButton     m_checkMouse;
    CListBox    m_listLog;
    CStatic     m_videoArea;
    BOOL        m_bGraphRunning;
    HWND        m_hTargetWnd;
    BOOL        m_bPicking;
};
