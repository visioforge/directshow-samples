//////////////////////////////////////////////////////
// MainDlg.cpp - Main dialog implementation
//
// MFC dialog-based demo for VisioForge VLC Source.
// Shows video playback with seeking, pause/resume,
// and timeline display.
//////////////////////////////////////////////////////
#include "pch.h"
#include "MFCVLCDemo.h"
#include "MainDlg.h"
#include "VLCSourceGuids.h"

#include <dvdmedia.h>

#pragma comment(lib, "strmiids.lib")
#pragma comment(lib, "ole32.lib")
#pragma comment(lib, "oleaut32.lib")
#pragma comment(lib, "uuid.lib")

BEGIN_MESSAGE_MAP(CMainDlg, CDialogEx)
    ON_BN_CLICKED(IDC_BTN_START, &CMainDlg::OnBnClickedStart)
    ON_BN_CLICKED(IDC_BTN_STOP, &CMainDlg::OnBnClickedStop)
    ON_BN_CLICKED(IDC_BTN_PAUSE, &CMainDlg::OnBnClickedPause)
    ON_BN_CLICKED(IDC_BTN_RESUME, &CMainDlg::OnBnClickedResume)
    ON_BN_CLICKED(IDC_BTN_BROWSE, &CMainDlg::OnBnClickedBrowse)
    ON_WM_SIZE()
    ON_WM_DESTROY()
    ON_WM_TIMER()
    ON_WM_HSCROLL()
END_MESSAGE_MAP()

//////////////////////////////////////////////////////
// Utility: Free AM_MEDIA_TYPE
//////////////////////////////////////////////////////

void CMainDlg::FreeMediaType(AM_MEDIA_TYPE& mt)
{
    if (mt.cbFormat != 0)
    {
        CoTaskMemFree((PVOID)mt.pbFormat);
        mt.cbFormat = 0;
        mt.pbFormat = NULL;
    }
    if (mt.pUnk != NULL)
    {
        mt.pUnk->Release();
        mt.pUnk = NULL;
    }
}

void CMainDlg::DeleteMediaType(AM_MEDIA_TYPE* pmt)
{
    if (pmt != NULL)
    {
        FreeMediaType(*pmt);
        CoTaskMemFree(pmt);
    }
}

//////////////////////////////////////////////////////
// Construction / Destruction
//////////////////////////////////////////////////////

CMainDlg::CMainDlg(CWnd* pParent)
    : CDialogEx(IDD_MAINDLG, pParent)
    , m_pGraph(NULL)
    , m_pCapture(NULL)
    , m_pMediaControl(NULL)
    , m_pMediaSeeking(NULL)
    , m_pVideoWindow(NULL)
    , m_pSourceFilter(NULL)
    , m_bSeeking(false)
{
}

CMainDlg::~CMainDlg()
{
}

void CMainDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialogEx::DoDataExchange(pDX);
    DDX_Control(pDX, IDC_EDIT_FILENAME, m_editFilename);
    DDX_Control(pDX, IDC_VIDEO_PANEL, m_videoPanel);
    DDX_Control(pDX, IDC_STATIC_TIME, m_staticTime);
    DDX_Control(pDX, IDC_STATIC_STATUS, m_staticStatus);
    DDX_Control(pDX, IDC_SLIDER_TIMELINE, m_sliderTimeline);
}

//////////////////////////////////////////////////////
// Initialization
//////////////////////////////////////////////////////

BOOL CMainDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();

    m_sliderTimeline.SetRange(0, 1000);
    m_sliderTimeline.SetPos(0);
    m_sliderTimeline.EnableWindow(FALSE);

    SetStatus(_T("Ready. Select a file and click Start."));

    return TRUE;
}

//////////////////////////////////////////////////////
// File Browse
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedBrowse()
{
    CFileDialog dlg(TRUE, NULL, NULL, OFN_FILEMUSTEXIST | OFN_HIDEREADONLY,
        _T("Media Files|*.mp4;*.avi;*.mkv;*.wmv;*.mov;*.flv;*.mpg;*.mpeg;*.ts;*.mp3;*.wav;*.flac;*.ogg|All Files|*.*||"),
        this);

    if (dlg.DoModal() == IDOK)
    {
        m_editFilename.SetWindowText(dlg.GetPathName());
    }
}

//////////////////////////////////////////////////////
// Start Playback
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedStart()
{
    CString strFilename;
    m_editFilename.GetWindowText(strFilename);
    if (strFilename.IsEmpty())
    {
        SetStatus(_T("Please select a file first."));
        return;
    }

    SetStatus(_T("Starting playback..."));

    HRESULT hr;

    // Create filter graph
    hr = CoCreateInstance(CLSID_FilterGraph, NULL,
        CLSCTX_INPROC_SERVER, IID_IFilterGraph2, (void**)&m_pGraph);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create FilterGraph."));
        return;
    }

    hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL,
        CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void**)&m_pCapture);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create CaptureGraphBuilder2."));
        ReleaseGraph();
        return;
    }

    hr = m_pCapture->SetFiltergraph(m_pGraph);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to set filter graph."));
        ReleaseGraph();
        return;
    }

    // Create and add VLC source filter
    hr = CoCreateInstance(CLSID_VlcSource, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pSourceFilter);
    if (FAILED(hr))
    {
        SetStatus(_T("VLC Source filter not registered. Run regsvr32 VFVLCSource.ax"));
        ReleaseGraph();
        return;
    }

    hr = m_pGraph->AddFilter(m_pSourceFilter, L"VisioForge VLC Source");
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to add source filter to graph."));
        ReleaseGraph();
        return;
    }

    // Load file via IFileSourceFilter
    IFileSourceFilter* pFileSource = NULL;
    hr = m_pSourceFilter->QueryInterface(IID_IFileSourceFilter, (void**)&pFileSource);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to query IFileSourceFilter."));
        ReleaseGraph();
        return;
    }

    hr = pFileSource->Load(strFilename, NULL);
    pFileSource->Release();
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to load file."));
        ReleaseGraph();
        return;
    }

    // Render video stream
    hr = m_pCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pSourceFilter, NULL, NULL);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to render video stream."));
        ReleaseGraph();
        return;
    }

    // Render audio stream (optional, ignore errors)
    m_pCapture->RenderStream(NULL, &MEDIATYPE_Audio,
        m_pSourceFilter, NULL, NULL);

    // Set up video window
    hr = m_pGraph->QueryInterface(IID_IVideoWindow, (void**)&m_pVideoWindow);
    if (SUCCEEDED(hr))
    {
        m_pVideoWindow->put_Owner((OAHWND)m_videoPanel.GetSafeHwnd());
        m_pVideoWindow->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS);
        ResizeVideoWindow();
        m_pVideoWindow->put_Visible(OATRUE);
    }

    // Get seeking interface
    m_pGraph->QueryInterface(IID_IMediaSeeking, (void**)&m_pMediaSeeking);

    // Run the graph
    hr = m_pGraph->QueryInterface(IID_IMediaControl, (void**)&m_pMediaControl);
    if (SUCCEEDED(hr))
    {
        hr = m_pMediaControl->Run();
        if (SUCCEEDED(hr))
        {
            SetStatus(_T("Playing."));

            GetDlgItem(IDC_BTN_START)->EnableWindow(FALSE);
            GetDlgItem(IDC_BTN_STOP)->EnableWindow(TRUE);
            GetDlgItem(IDC_BTN_PAUSE)->EnableWindow(TRUE);
            GetDlgItem(IDC_BTN_RESUME)->EnableWindow(FALSE);
            GetDlgItem(IDC_BTN_BROWSE)->EnableWindow(FALSE);
            m_editFilename.EnableWindow(FALSE);
            m_sliderTimeline.EnableWindow(TRUE);

            // Start progress timer (500ms)
            SetTimer(TIMER_PROGRESS, 500, NULL);
            return;
        }
    }

    SetStatus(_T("Failed to start playback."));
    ReleaseGraph();
}

//////////////////////////////////////////////////////
// Stop Playback
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedStop()
{
    KillTimer(TIMER_PROGRESS);
    ReleaseGraph();

    GetDlgItem(IDC_BTN_START)->EnableWindow(TRUE);
    GetDlgItem(IDC_BTN_STOP)->EnableWindow(FALSE);
    GetDlgItem(IDC_BTN_PAUSE)->EnableWindow(FALSE);
    GetDlgItem(IDC_BTN_RESUME)->EnableWindow(FALSE);
    GetDlgItem(IDC_BTN_BROWSE)->EnableWindow(TRUE);
    m_editFilename.EnableWindow(TRUE);
    m_sliderTimeline.EnableWindow(FALSE);
    m_sliderTimeline.SetPos(0);
    m_staticTime.SetWindowText(_T("00:00:00 / 00:00:00"));

    SetStatus(_T("Stopped."));
}

//////////////////////////////////////////////////////
// Pause / Resume
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedPause()
{
    if (m_pMediaControl)
    {
        m_pMediaControl->Pause();
        GetDlgItem(IDC_BTN_PAUSE)->EnableWindow(FALSE);
        GetDlgItem(IDC_BTN_RESUME)->EnableWindow(TRUE);
        SetStatus(_T("Paused."));
    }
}

void CMainDlg::OnBnClickedResume()
{
    if (m_pMediaControl)
    {
        m_pMediaControl->Run();
        GetDlgItem(IDC_BTN_PAUSE)->EnableWindow(TRUE);
        GetDlgItem(IDC_BTN_RESUME)->EnableWindow(FALSE);
        SetStatus(_T("Playing."));
    }
}

//////////////////////////////////////////////////////
// Timer - Update timeline and time display
//////////////////////////////////////////////////////

void CMainDlg::OnTimer(UINT_PTR nIDEvent)
{
    if (nIDEvent == TIMER_PROGRESS)
    {
        UpdateTimeDisplay();
    }

    CDialogEx::OnTimer(nIDEvent);
}

void CMainDlg::UpdateTimeDisplay()
{
    if (!m_pMediaSeeking || m_bSeeking)
        return;

    LONGLONG duration = 0, position = 0;
    m_pMediaSeeking->GetDuration(&duration);
    m_pMediaSeeking->GetCurrentPosition(&position);

    // Convert 100-nanosecond units to seconds
    int durSec = (int)(duration / 10000000LL);
    int posSec = (int)(position / 10000000LL);

    // Update slider
    if (durSec > 0)
    {
        int sliderPos = (int)((position * 1000LL) / duration);
        m_sliderTimeline.SetPos(sliderPos);
    }

    // Format time strings
    int posH = posSec / 3600;
    int posM = (posSec % 3600) / 60;
    int posS = posSec % 60;

    int durH = durSec / 3600;
    int durM = (durSec % 3600) / 60;
    int durS = durSec % 60;

    CString strTime;
    strTime.Format(_T("%02d:%02d:%02d / %02d:%02d:%02d"),
        posH, posM, posS, durH, durM, durS);
    m_staticTime.SetWindowText(strTime);
}

//////////////////////////////////////////////////////
// Seeking via slider
//////////////////////////////////////////////////////

void CMainDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
    if (pScrollBar && pScrollBar->GetSafeHwnd() == m_sliderTimeline.GetSafeHwnd())
    {
        if (nSBCode == TB_THUMBTRACK || nSBCode == TB_THUMBPOSITION)
        {
            if (m_pMediaSeeking)
            {
                m_bSeeking = true;

                LONGLONG duration = 0;
                m_pMediaSeeking->GetDuration(&duration);

                int pos = m_sliderTimeline.GetPos();
                LONGLONG newPos = (duration * pos) / 1000LL;
                LONGLONG stopPos = 0;

                m_pMediaSeeking->SetPositions(
                    &newPos, AM_SEEKING_AbsolutePositioning,
                    &stopPos, AM_SEEKING_NoPositioning);

                m_bSeeking = false;
            }
        }
    }

    CDialogEx::OnHScroll(nSBCode, nPos, pScrollBar);
}

//////////////////////////////////////////////////////
// Video window resize
//////////////////////////////////////////////////////

void CMainDlg::ResizeVideoWindow()
{
    if (m_pVideoWindow && m_videoPanel.GetSafeHwnd())
    {
        CRect rc;
        m_videoPanel.GetClientRect(&rc);
        m_pVideoWindow->SetWindowPosition(0, 0, rc.Width(), rc.Height());
    }
}

void CMainDlg::OnSize(UINT nType, int cx, int cy)
{
    CDialogEx::OnSize(nType, cx, cy);
    ResizeVideoWindow();
}

//////////////////////////////////////////////////////
// Cleanup
//////////////////////////////////////////////////////

void CMainDlg::ReleaseGraph()
{
    if (m_pMediaControl)
    {
        m_pMediaControl->Stop();
        m_pMediaControl->Release();
        m_pMediaControl = NULL;
    }

    if (m_pMediaSeeking)
    {
        m_pMediaSeeking->Release();
        m_pMediaSeeking = NULL;
    }

    if (m_pVideoWindow)
    {
        m_pVideoWindow->put_Visible(OAFALSE);
        m_pVideoWindow->put_Owner(NULL);
        m_pVideoWindow->Release();
        m_pVideoWindow = NULL;
    }

    if (m_pSourceFilter)
    {
        m_pSourceFilter->Release();
        m_pSourceFilter = NULL;
    }

    if (m_pCapture)
    {
        m_pCapture->Release();
        m_pCapture = NULL;
    }

    if (m_pGraph)
    {
        m_pGraph->Release();
        m_pGraph = NULL;
    }
}

void CMainDlg::OnDestroy()
{
    KillTimer(TIMER_PROGRESS);
    ReleaseGraph();
    CDialogEx::OnDestroy();
}

//////////////////////////////////////////////////////
// Status helper
//////////////////////////////////////////////////////

void CMainDlg::SetStatus(LPCTSTR text)
{
    CString str;
    str.Format(_T("Status: %s"), text);
    m_staticStatus.SetWindowText(str);
}
