// FFMPEGSourceMFCSample.cpp - MFC Sample Application for FFMPEG Source Filter
//
// Demonstrates how to use the VisioForge FFMPEG Source DirectShow filter
// in an MFC dialog-based application with EVR video preview, seeking,
// speed control, and stream selection.
//
// Prerequisites:
//   - Register the FFMPEG Source filter
//   - Windows SDK with DirectShow and Media Foundation headers
//   - Link: strmiids.lib ole32.lib oleaut32.lib mfuuid.lib mfplat.lib

#include <afxwin.h>
#include <afxcmn.h>
#include <afxdlgs.h>
#include <afxdialogex.h>
#include "FFMPEGSourceMFCSample.h"

#include <iomanip>
#include <sstream>

// ---- CFFMPEGSourceDlg implementation ----

BEGIN_MESSAGE_MAP(CFFMPEGSourceDlg, CDialogEx)
    ON_BN_CLICKED(IDC_BTN_START, &CFFMPEGSourceDlg::OnBnClickedStart)
    ON_BN_CLICKED(IDC_BTN_PAUSE, &CFFMPEGSourceDlg::OnBnClickedPause)
    ON_BN_CLICKED(IDC_BTN_RESUME, &CFFMPEGSourceDlg::OnBnClickedResume)
    ON_BN_CLICKED(IDC_BTN_STOP, &CFFMPEGSourceDlg::OnBnClickedStop)
    ON_BN_CLICKED(IDC_BTN_BROWSE, &CFFMPEGSourceDlg::OnBnClickedBrowse)
    ON_CBN_SELCHANGE(IDC_COMBO_VIDEO_STREAM, &CFFMPEGSourceDlg::OnCbnSelchangeVideoStream)
    ON_CBN_SELCHANGE(IDC_COMBO_AUDIO_STREAM, &CFFMPEGSourceDlg::OnCbnSelchangeAudioStream)
    ON_WM_HSCROLL()
    ON_WM_TIMER()
    ON_WM_DESTROY()
END_MESSAGE_MAP()

CFFMPEGSourceDlg::CFFMPEGSourceDlg(CWnd* pParent)
    : CDialogEx(IDD_FFMPEGSOURCE_DIALOG, pParent)
    , m_pGraph(NULL)
    , m_pCaptureGraph(NULL)
    , m_pControl(NULL)
    , m_pSeeking(NULL)
    , m_pEvent(NULL)
    , m_pSource(NULL)
    , m_pVideoRenderer(NULL)
    , m_pVideoDisplay(NULL)
    , m_bGraphRunning(FALSE)
    , m_bUpdatingPosition(FALSE)
{
}

void CFFMPEGSourceDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialogEx::DoDataExchange(pDX);
    DDX_Control(pDX, IDC_EDIT_FILENAME, m_editFilename);
    DDX_Control(pDX, IDC_EDIT_TIMEOUT, m_editTimeout);
    DDX_Control(pDX, IDC_COMBO_BUFFERING, m_comboBuffering);
    DDX_Control(pDX, IDC_CHECK_GPU, m_checkGPU);
    DDX_Control(pDX, IDC_COMBO_VIDEO_STREAM, m_comboVideoStream);
    DDX_Control(pDX, IDC_COMBO_AUDIO_STREAM, m_comboAudioStream);
    DDX_Control(pDX, IDC_SLIDER_TIMELINE, m_sliderTimeline);
    DDX_Control(pDX, IDC_SLIDER_SPEED, m_sliderSpeed);
    DDX_Control(pDX, IDC_STATIC_TIME, m_staticTime);
    DDX_Control(pDX, IDC_STATIC_SPEED_VAL, m_staticSpeedVal);
    DDX_Control(pDX, IDC_VIDEO_AREA, m_videoArea);
    DDX_Control(pDX, IDC_LIST_LOG, m_listLog);
}

BOOL CFFMPEGSourceDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();

    // Default values
    m_editFilename.SetWindowText(_T("c:\\Samples\\!video.mp4"));
    m_editTimeout.SetWindowText(_T("10000"));

    m_comboBuffering.AddString(_T("Auto"));
    m_comboBuffering.AddString(_T("On"));
    m_comboBuffering.AddString(_T("Off"));
    m_comboBuffering.SetCurSel(0);

    m_checkGPU.SetCheck(BST_UNCHECKED);

    // Speed slider: 5..25 => 0.5x..2.5x
    m_sliderSpeed.SetRange(5, 25);
    m_sliderSpeed.SetPos(10);
    m_staticSpeedVal.SetWindowText(_T("1.0x"));

    // Timeline slider
    m_sliderTimeline.SetRange(0, 100);
    m_sliderTimeline.SetPos(0);
    m_staticTime.SetWindowText(_T("00:00:00 / 00:00:00"));

    Log(_T("Ready. Select a file and click Start."));

    return TRUE;
}

void CFFMPEGSourceDlg::Log(const CString& msg)
{
    m_listLog.AddString(msg);
    m_listLog.SetTopIndex(m_listLog.GetCount() - 1);
}

// ---- Playback helpers ----

LONGLONG CFFMPEGSourceDlg::GetDurationMs()
{
    if (m_pSeeking)
    {
        REFERENCE_TIME duration = 0;
        if (SUCCEEDED(m_pSeeking->GetDuration(&duration)))
            return duration / 10000;
    }
    return 0;
}

LONGLONG CFFMPEGSourceDlg::GetPositionMs()
{
    if (m_pSeeking)
    {
        REFERENCE_TIME position = 0;
        if (SUCCEEDED(m_pSeeking->GetCurrentPosition(&position)))
            return position / 10000;
    }
    return 0;
}

BOOL CFFMPEGSourceDlg::SetPositionMs(LONGLONG ms)
{
    if (!m_pSeeking)
        return FALSE;

    REFERENCE_TIME pos = ms * 10000;
    REFERENCE_TIME stop = 0;
    HRESULT hr = m_pSeeking->SetPositions(
        &pos, AM_SEEKING_AbsolutePositioning,
        &stop, AM_SEEKING_NoPositioning);
    return SUCCEEDED(hr);
}

void CFFMPEGSourceDlg::UpdateVideoWindow()
{
    if (m_pVideoDisplay)
    {
        CRect rc;
        m_videoArea.GetClientRect(&rc);
        MFVideoNormalizedRect srcRect = { 0.0f, 0.0f, 1.0f, 1.0f };
        RECT destRect = { 0, 0, rc.Width(), rc.Height() };
        m_pVideoDisplay->SetVideoPosition(&srcRect, &destRect);
    }
}

// ---- Graph building ----

void CFFMPEGSourceDlg::BuildGraph()
{
    HRESULT hr;

    // 1. Create FilterGraph2
    hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER,
        IID_IFilterGraph2, (void**)&m_pGraph);
    if (FAILED(hr))
    {
        CString msg; msg.Format(_T("ERROR: Cannot create FilterGraph (0x%08X)"), hr);
        Log(msg);
        return;
    }

    // 2. Create CaptureGraphBuilder2
    hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC_SERVER,
        IID_ICaptureGraphBuilder2, (void**)&m_pCaptureGraph);
    if (FAILED(hr))
    {
        CString msg; msg.Format(_T("ERROR: Cannot create CaptureGraphBuilder2 (0x%08X)"), hr);
        Log(msg);
        TearDownGraph();
        return;
    }
    m_pCaptureGraph->SetFiltergraph(m_pGraph);

    // 3. Query control interfaces
    m_pGraph->QueryInterface(IID_IMediaControl, (void**)&m_pControl);
    m_pGraph->QueryInterface(IID_IMediaSeeking, (void**)&m_pSeeking);
    m_pGraph->QueryInterface(IID_IMediaEventEx, (void**)&m_pEvent);

    // 4. Create FFMPEG source filter
    hr = CoCreateInstance(CLSID_VFFFMPEGSource, NULL, CLSCTX_INPROC_SERVER,
        IID_IBaseFilter, (void**)&m_pSource);
    if (FAILED(hr))
    {
        CString msg; msg.Format(_T("ERROR: Cannot create FFMPEG Source filter (0x%08X). Is it registered?"), hr);
        Log(msg);
        TearDownGraph();
        return;
    }
    hr = m_pGraph->AddFilter(m_pSource, L"FFMPEG Source");
    if (FAILED(hr))
    {
        CString msg; msg.Format(_T("ERROR: Cannot add source filter to graph (0x%08X)"), hr);
        Log(msg);
        TearDownGraph();
        return;
    }

    // 5. Configure IFFmpegSourceSettings BEFORE loading
    IFFmpegSourceSettings* pSettings = NULL;
    hr = m_pSource->QueryInterface(IID_IFFmpegSourceSettings, (void**)&pSettings);
    if (SUCCEEDED(hr) && pSettings)
    {
        // Buffering mode
        int bufIdx = m_comboBuffering.GetCurSel();
        if (bufIdx >= 0)
        {
            pSettings->SetBufferingMode((FFMPEG_SOURCE_BUFFERING_MODE)bufIdx);
            CString msg; msg.Format(_T("Buffering mode: %d"), bufIdx);
            Log(msg);
        }

        // GPU acceleration
        BOOL useGPU = (m_checkGPU.GetCheck() == BST_CHECKED);
        pSettings->SetHWAccelerationEnabled(useGPU);
        Log(useGPU ? _T("GPU acceleration: enabled") : _T("GPU acceleration: disabled"));

        // Timeout
        CString strTimeout;
        m_editTimeout.GetWindowText(strTimeout);
        DWORD timeout = (DWORD)_ttoi(strTimeout);
        if (timeout > 0)
        {
            pSettings->SetLoadTimeOut(timeout);
            CString msg; msg.Format(_T("Load timeout: %u ms"), timeout);
            Log(msg);
        }

        pSettings->Release();
    }

    // 6. Load file via IFileSourceFilter
    CString strFilename;
    m_editFilename.GetWindowText(strFilename);
    if (strFilename.IsEmpty())
    {
        Log(_T("ERROR: No filename specified."));
        TearDownGraph();
        return;
    }

    IFileSourceFilter* pFileSource = NULL;
    hr = m_pSource->QueryInterface(IID_IFileSourceFilter, (void**)&pFileSource);
    if (SUCCEEDED(hr) && pFileSource)
    {
        hr = pFileSource->Load(CT2W(strFilename), NULL);
        pFileSource->Release();
        if (FAILED(hr))
        {
            CString msg; msg.Format(_T("ERROR: Cannot load '%s' (0x%08X)"), (LPCTSTR)strFilename, hr);
            Log(msg);
            TearDownGraph();
            return;
        }
        CString msg; msg.Format(_T("Loaded: %s"), (LPCTSTR)strFilename);
        Log(msg);
    }

    // 7. Enumerate streams (IAMStreamSelect)
    m_comboVideoStream.ResetContent();
    m_comboAudioStream.ResetContent();

    IAMStreamSelect* pStreamSelect = NULL;
    hr = m_pSource->QueryInterface(IID_IAMStreamSelect, (void**)&pStreamSelect);
    if (SUCCEEDED(hr) && pStreamSelect)
    {
        DWORD streamCount = 0;
        pStreamSelect->Count(&streamCount);
        for (DWORD i = 0; i < streamCount; i++)
        {
            AM_MEDIA_TYPE* pmt = NULL;
            WCHAR* szName = NULL;
            hr = pStreamSelect->Info(i, &pmt, NULL, NULL, NULL, &szName, NULL, NULL);
            if (SUCCEEDED(hr))
            {
                if (pmt && pmt->majortype == MEDIATYPE_Video)
                    m_comboVideoStream.AddString(CString(szName));
                else if (pmt && pmt->majortype == MEDIATYPE_Audio)
                    m_comboAudioStream.AddString(CString(szName));

                if (pmt)
                {
                    if (pmt->pbFormat) CoTaskMemFree(pmt->pbFormat);
                    if (pmt->pUnk) pmt->pUnk->Release();
                    CoTaskMemFree(pmt);
                }
                if (szName) CoTaskMemFree(szName);
            }
        }
        pStreamSelect->Release();
    }

    if (m_comboVideoStream.GetCount() > 0)
        m_comboVideoStream.SetCurSel(0);
    if (m_comboAudioStream.GetCount() > 0)
        m_comboAudioStream.SetCurSel(0);

    CString strStreams;
    strStreams.Format(_T("Streams: %d video, %d audio"),
        m_comboVideoStream.GetCount(), m_comboAudioStream.GetCount());
    Log(strStreams);

    // 8. Create EVR and configure
    hr = CoCreateInstance(CLSID_EnhancedVideoRenderer, NULL, CLSCTX_INPROC_SERVER,
        IID_IBaseFilter, (void**)&m_pVideoRenderer);
    if (FAILED(hr))
    {
        CString msg; msg.Format(_T("ERROR: Cannot create EVR (0x%08X)"), hr);
        Log(msg);
        TearDownGraph();
        return;
    }
    m_pGraph->AddFilter(m_pVideoRenderer, L"EVR");

    IEVRFilterConfig* pEVRConfig = NULL;
    hr = m_pVideoRenderer->QueryInterface(IID_IEVRFilterConfig, (void**)&pEVRConfig);
    if (SUCCEEDED(hr) && pEVRConfig)
    {
        pEVRConfig->SetNumberOfStreams(1);
        pEVRConfig->Release();
    }

    // Get IMFVideoDisplayControl for video window management
    IMFGetService* pGetService = NULL;
    hr = m_pVideoRenderer->QueryInterface(IID_IMFGetService, (void**)&pGetService);
    if (SUCCEEDED(hr) && pGetService)
    {
        hr = pGetService->GetService(MR_VIDEO_RENDER_SERVICE,
            IID_IMFVideoDisplayControl, (void**)&m_pVideoDisplay);
        if (SUCCEEDED(hr) && m_pVideoDisplay)
        {
            m_pVideoDisplay->SetVideoWindow(m_videoArea.GetSafeHwnd());
            UpdateVideoWindow();
        }
        pGetService->Release();
    }

    // 9. Render streams
    hr = m_pCaptureGraph->RenderStream(NULL, &MEDIATYPE_Video,
        m_pSource, NULL, m_pVideoRenderer);
    if (FAILED(hr))
    {
        CString msg; msg.Format(_T("ERROR: Cannot render video stream (0x%08X)"), hr);
        Log(msg);
        TearDownGraph();
        return;
    }
    Log(_T("Video stream connected."));

    hr = m_pCaptureGraph->RenderStream(NULL, &MEDIATYPE_Audio,
        m_pSource, NULL, NULL);
    if (SUCCEEDED(hr))
        Log(_T("Audio stream connected."));

    // 10. Run
    hr = m_pControl->Run();
    if (FAILED(hr))
    {
        CString msg; msg.Format(_T("ERROR: Run failed (0x%08X)"), hr);
        Log(msg);
        TearDownGraph();
        return;
    }

    m_bGraphRunning = TRUE;
    SetTimer(TIMER_PROGRESS, 250, NULL);
    Log(_T("Playback started."));
}

void CFFMPEGSourceDlg::TearDownGraph()
{
    KillTimer(TIMER_PROGRESS);

    if (m_pControl)
    {
        m_pControl->Stop();
        m_pControl->Release();
        m_pControl = NULL;
    }

    if (m_pEvent)
    {
        m_pEvent->Release();
        m_pEvent = NULL;
    }

    if (m_pSeeking)
    {
        m_pSeeking->Release();
        m_pSeeking = NULL;
    }

    if (m_pVideoDisplay)
    {
        m_pVideoDisplay->Release();
        m_pVideoDisplay = NULL;
    }

    if (m_pVideoRenderer)
    {
        m_pVideoRenderer->Release();
        m_pVideoRenderer = NULL;
    }

    if (m_pSource)
    {
        m_pSource->Release();
        m_pSource = NULL;
    }

    if (m_pCaptureGraph)
    {
        m_pCaptureGraph->Release();
        m_pCaptureGraph = NULL;
    }

    if (m_pGraph)
    {
        m_pGraph->Release();
        m_pGraph = NULL;
    }

    m_bGraphRunning = FALSE;
}

// ---- Button handlers ----

void CFFMPEGSourceDlg::OnBnClickedStart()
{
    if (m_bGraphRunning)
    {
        Log(_T("Already running."));
        return;
    }

    BuildGraph();
}

void CFFMPEGSourceDlg::OnBnClickedPause()
{
    if (m_pControl)
    {
        HRESULT hr = m_pControl->Pause();
        if (SUCCEEDED(hr))
            Log(_T("Paused."));
    }
}

void CFFMPEGSourceDlg::OnBnClickedResume()
{
    if (m_pControl)
    {
        HRESULT hr = m_pControl->Run();
        if (SUCCEEDED(hr))
            Log(_T("Resumed."));
    }
}

void CFFMPEGSourceDlg::OnBnClickedStop()
{
    if (!m_bGraphRunning)
    {
        Log(_T("Not running."));
        return;
    }

    TearDownGraph();

    m_sliderTimeline.SetPos(0);
    m_staticTime.SetWindowText(_T("00:00:00 / 00:00:00"));

    Log(_T("Stopped."));
}

void CFFMPEGSourceDlg::OnBnClickedBrowse()
{
    CFileDialog dlg(TRUE, NULL, NULL, OFN_FILEMUSTEXIST | OFN_HIDEREADONLY,
        _T("Media Files|*.mp4;*.avi;*.mkv;*.wmv;*.mov;*.flv;*.ts;*.mpg;*.webm|All Files|*.*||"),
        this);

    if (dlg.DoModal() == IDOK)
    {
        m_editFilename.SetWindowText(dlg.GetPathName());
    }
}

// ---- Stream selection ----

void CFFMPEGSourceDlg::OnCbnSelchangeVideoStream()
{
    if (!m_pSource)
        return;

    IAMStreamSelect* pStreamSelect = NULL;
    HRESULT hr = m_pSource->QueryInterface(IID_IAMStreamSelect, (void**)&pStreamSelect);
    if (SUCCEEDED(hr) && pStreamSelect)
    {
        DWORD streamCount = 0;
        pStreamSelect->Count(&streamCount);
        int k = 0;
        for (DWORD i = 0; i < streamCount; i++)
        {
            AM_MEDIA_TYPE* pmt = NULL;
            WCHAR* szName = NULL;
            hr = pStreamSelect->Info(i, &pmt, NULL, NULL, NULL, &szName, NULL, NULL);
            if (SUCCEEDED(hr))
            {
                if (pmt && pmt->majortype == MEDIATYPE_Video)
                {
                    if (k == m_comboVideoStream.GetCurSel())
                        pStreamSelect->Enable(i, AMSTREAMSELECTENABLE_ENABLE);
                    else
                        pStreamSelect->Enable(i, 0);
                    k++;
                }

                if (pmt)
                {
                    if (pmt->pbFormat) CoTaskMemFree(pmt->pbFormat);
                    if (pmt->pUnk) pmt->pUnk->Release();
                    CoTaskMemFree(pmt);
                }
                if (szName) CoTaskMemFree(szName);
            }
        }
        pStreamSelect->Release();
    }
}

void CFFMPEGSourceDlg::OnCbnSelchangeAudioStream()
{
    if (!m_pSource)
        return;

    IAMStreamSelect* pStreamSelect = NULL;
    HRESULT hr = m_pSource->QueryInterface(IID_IAMStreamSelect, (void**)&pStreamSelect);
    if (SUCCEEDED(hr) && pStreamSelect)
    {
        DWORD streamCount = 0;
        pStreamSelect->Count(&streamCount);
        int k = 0;
        for (DWORD i = 0; i < streamCount; i++)
        {
            AM_MEDIA_TYPE* pmt = NULL;
            WCHAR* szName = NULL;
            hr = pStreamSelect->Info(i, &pmt, NULL, NULL, NULL, &szName, NULL, NULL);
            if (SUCCEEDED(hr))
            {
                if (pmt && pmt->majortype == MEDIATYPE_Audio)
                {
                    if (k == m_comboAudioStream.GetCurSel())
                        pStreamSelect->Enable(i, AMSTREAMSELECTENABLE_ENABLE);
                    else
                        pStreamSelect->Enable(i, 0);
                    k++;
                }

                if (pmt)
                {
                    if (pmt->pbFormat) CoTaskMemFree(pmt->pbFormat);
                    if (pmt->pUnk) pmt->pUnk->Release();
                    CoTaskMemFree(pmt);
                }
                if (szName) CoTaskMemFree(szName);
            }
        }
        pStreamSelect->Release();
    }
}

// ---- Slider / scroll handling ----

void CFFMPEGSourceDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
    CSliderCtrl* pSlider = (CSliderCtrl*)pScrollBar;

    if (pSlider == &m_sliderTimeline)
    {
        if (!m_bUpdatingPosition && m_pSeeking)
        {
            int pos = m_sliderTimeline.GetPos();
            SetPositionMs((LONGLONG)pos * 1000);
        }
    }
    else if (pSlider == &m_sliderSpeed)
    {
        if (m_pSeeking)
        {
            double rate = m_sliderSpeed.GetPos() / 10.0;
            m_pSeeking->SetRate(rate);

            CString strRate;
            strRate.Format(_T("%.1fx"), rate);
            m_staticSpeedVal.SetWindowText(strRate);
        }
    }

    CDialogEx::OnHScroll(nSBCode, nPos, pScrollBar);
}

// ---- Timer for position updates ----

static CString FormatTimeSeconds(int totalSeconds)
{
    int h = totalSeconds / 3600;
    int m = (totalSeconds % 3600) / 60;
    int s = totalSeconds % 60;
    CString str;
    str.Format(_T("%02d:%02d:%02d"), h, m, s);
    return str;
}

void CFFMPEGSourceDlg::OnTimer(UINT_PTR nIDEvent)
{
    if (nIDEvent == TIMER_PROGRESS && m_pSeeking)
    {
        m_bUpdatingPosition = TRUE;

        int durationSec = (int)(GetDurationMs() / 1000);
        int positionSec = (int)(GetPositionMs() / 1000);

        m_sliderTimeline.SetRange(0, durationSec);

        if (positionSec >= 0 && positionSec <= durationSec)
            m_sliderTimeline.SetPos(positionSec);

        CString strTime;
        strTime.Format(_T("%s / %s"),
            (LPCTSTR)FormatTimeSeconds(positionSec),
            (LPCTSTR)FormatTimeSeconds(durationSec));
        m_staticTime.SetWindowText(strTime);

        m_bUpdatingPosition = FALSE;
    }

    CDialogEx::OnTimer(nIDEvent);
}

// ---- Cleanup ----

void CFFMPEGSourceDlg::OnDestroy()
{
    TearDownGraph();
    CDialogEx::OnDestroy();
}

// ---- Application class ----
class CFFMPEGSourceApp : public CWinApp
{
public:
    virtual BOOL InitInstance()
    {
        CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

        CFFMPEGSourceDlg dlg;
        m_pMainWnd = &dlg;
        dlg.DoModal();

        CoUninitialize();
        return FALSE;
    }
};

CFFMPEGSourceApp theApp;
