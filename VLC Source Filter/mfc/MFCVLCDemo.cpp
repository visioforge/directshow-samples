//////////////////////////////////////////////////////
// MFCVLCDemo.cpp - Application class implementation
//////////////////////////////////////////////////////
#include "pch.h"
#include "MFCVLCDemo.h"
#include "MainDlg.h"

BEGIN_MESSAGE_MAP(CMFCVLCDemoApp, CWinApp)
END_MESSAGE_MAP()

CMFCVLCDemoApp::CMFCVLCDemoApp()
{
}

CMFCVLCDemoApp theApp;

BOOL CMFCVLCDemoApp::InitInstance()
{
    CWinApp::InitInstance();

    HRESULT hr = CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
    if (FAILED(hr))
    {
        AfxMessageBox(_T("COM initialization failed."), MB_ICONERROR);
        return FALSE;
    }

    CMainDlg dlg;
    m_pMainWnd = &dlg;
    dlg.DoModal();
    m_pMainWnd = NULL;

    CoUninitialize();

    return FALSE;
}
