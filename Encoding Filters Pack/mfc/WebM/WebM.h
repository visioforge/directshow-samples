#pragma once

#include <afxwin.h>
#include <afxdlgs.h>
#include <afxdialogex.h>
#include "../common/Graph.h"
#include "resource.h"

#define WM_GRAPH_NOTIFY (WM_APP + 1)

class CWebMDlg : public CDialogEx
{
public:
    CWebMDlg();
    enum { IDD = IDD_WEBM };

protected:
    virtual BOOL OnInitDialog();
    virtual void OnOK();
    virtual void OnCancel();
    afx_msg void OnStart();
    afx_msg void OnStop();
    afx_msg void OnBrowseSource();
    afx_msg void OnBrowseOutput();
    afx_msg void OnSourceMode();
    afx_msg LRESULT OnGraphNotify(WPARAM wparam, LPARAM lparam);
    DECLARE_MESSAGE_MAP()

private:
    void StopGraph();
    void UpdateSourceMode();
    bool Failed(HRESULT hr, const wchar_t* step);
    HRESULT ConnectFree(IBaseFilter* from, IBaseFilter* to);

    IGraphBuilder* m_graph;
    ICaptureGraphBuilder2* m_capture;
    IMediaControl* m_control;
    IMediaEventEx* m_events;
    IVideoWindow* m_window;
    bool m_running;
};

class CWebMApp : public CWinApp
{
public:
    virtual BOOL InitInstance();
};
