//////////////////////////////////////////////////////
// MainDlg.cpp - Main dialog implementation
//
// Two independent filter graphs:
//   Source: real webcam -> Smart Tee -> preview
//                                    -> VF Virtual Camera Sink
//   Camera: VF Virtual Camera Source -> preview or capture
//////////////////////////////////////////////////////
#include "pch.h"
#include "MFCCameraDemo.h"
#include "MainDlg.h"
#include "VCameraGuids.h"

#include <dvdmedia.h>

#pragma comment(lib, "strmiids.lib")
#pragma comment(lib, "ole32.lib")
#pragma comment(lib, "oleaut32.lib")
#pragma comment(lib, "uuid.lib")

BEGIN_MESSAGE_MAP(CMainDlg, CDialogEx)
    // Source
    ON_BN_CLICKED(IDC_BTN_SOURCE_START, &CMainDlg::OnBnClickedSourceStart)
    ON_BN_CLICKED(IDC_BTN_SOURCE_STOP, &CMainDlg::OnBnClickedSourceStop)
    ON_CBN_SELCHANGE(IDC_COMBO_SOURCE_DEVICE, &CMainDlg::OnCbnSelchangeSourceDevice)
    // Camera
    ON_BN_CLICKED(IDC_BTN_CAMERA_PREVIEW, &CMainDlg::OnBnClickedCameraPreview)
    ON_BN_CLICKED(IDC_BTN_CAMERA_CAPTURE, &CMainDlg::OnBnClickedCameraCapture)
    ON_BN_CLICKED(IDC_BTN_CAMERA_STOP, &CMainDlg::OnBnClickedCameraStop)
    ON_BN_CLICKED(IDC_BTN_BROWSE, &CMainDlg::OnBnClickedBrowse)
    // Window
    ON_WM_SIZE()
    ON_WM_DESTROY()
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
    // Source graph
    , m_pSrcGraph(NULL), m_pSrcCapture(NULL)
    , m_pSrcMediaControl(NULL), m_pSrcVideoWindow(NULL)
    , m_pSrcWebcamFilter(NULL), m_pSrcAudioFilter(NULL)
    , m_pSrcVideoSink(NULL), m_pSrcAudioSink(NULL)
    , m_pSrcSmartTee(NULL)
    // Camera graph
    , m_pCamGraph(NULL), m_pCamCapture(NULL)
    , m_pCamMediaControl(NULL), m_pCamVideoWindow(NULL)
    , m_pCamCameraFilter(NULL), m_pCamAudioFilter(NULL)
    , m_pCamVideoSmartTee(NULL), m_pCamAudioSmartTee(NULL)
    , m_pCamVideoCodec(NULL), m_pCamAudioCodec(NULL)
    , m_pCamAVIMux(NULL), m_pCamFileWriter(NULL)
    , m_pCamAudioRenderer(NULL)
{
}

CMainDlg::~CMainDlg()
{
    // Release device monikers
    for (int i = 0; i < m_videoDevices.GetSize(); i++)
        if (m_videoDevices[i].pMoniker)
            m_videoDevices[i].pMoniker->Release();
    for (int i = 0; i < m_audioDevices.GetSize(); i++)
        if (m_audioDevices[i].pMoniker)
            m_audioDevices[i].pMoniker->Release();
}

void CMainDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialogEx::DoDataExchange(pDX);
    // Source
    DDX_Control(pDX, IDC_COMBO_SOURCE_DEVICE, m_comboSourceDevice);
    DDX_Control(pDX, IDC_COMBO_SOURCE_FORMAT, m_comboSourceFormat);
    DDX_Control(pDX, IDC_COMBO_SOURCE_FRAMERATE, m_comboSourceFrameRate);
    DDX_Control(pDX, IDC_SOURCE_VIDEO_PANEL, m_srcVideoPanel);
    // Camera
    DDX_Control(pDX, IDC_COMBO_CAMERA_FORMAT, m_comboCameraFormat);
    DDX_Control(pDX, IDC_COMBO_CAMERA_FRAMERATE, m_comboCameraFrameRate);
    DDX_Control(pDX, IDC_CAMERA_VIDEO_PANEL, m_camVideoPanel);
    // Status
    DDX_Control(pDX, IDC_STATIC_STATUS, m_staticStatus);
}

//////////////////////////////////////////////////////
// Initialization
//////////////////////////////////////////////////////

BOOL CMainDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();

    SetDlgItemText(IDC_EDIT_OUTPUT_FILE, _T("C:\\vf\\output.avi"));

    EnumerateVideoDevices();
    EnumerateCameraFormats();

    SetStatus(_T("Ready."));
    return TRUE;
}

//////////////////////////////////////////////////////
// Device enumeration
//////////////////////////////////////////////////////

void CMainDlg::EnumerateVideoDevices()
{
    m_comboSourceDevice.ResetContent();

    // Enumerate video capture devices
    ICreateDevEnum* pDevEnum = NULL;
    HRESULT hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL,
        CLSCTX_INPROC_SERVER, IID_ICreateDevEnum, (void**)&pDevEnum);
    if (FAILED(hr)) return;

    IEnumMoniker* pEnum = NULL;
    hr = pDevEnum->CreateClassEnumerator(CLSID_VideoInputDeviceCategory, &pEnum, 0);
    if (hr == S_OK && pEnum)
    {
        IMoniker* pMoniker = NULL;
        while (pEnum->Next(1, &pMoniker, NULL) == S_OK)
        {
            IPropertyBag* pPropBag = NULL;
            hr = pMoniker->BindToStorage(0, 0, IID_IPropertyBag, (void**)&pPropBag);
            if (SUCCEEDED(hr))
            {
                VARIANT varName;
                VariantInit(&varName);
                hr = pPropBag->Read(L"FriendlyName", &varName, 0);
                if (SUCCEEDED(hr))
                {
                    DeviceInfo info;
                    info.name = varName.bstrVal;
                    info.pMoniker = pMoniker;
                    pMoniker->AddRef();

                    m_videoDevices.Add(info);
                    m_comboSourceDevice.AddString(info.name);

                    VariantClear(&varName);
                }
                pPropBag->Release();
            }
            pMoniker->Release();
        }
        pEnum->Release();
    }
    pDevEnum->Release();

    if (m_comboSourceDevice.GetCount() > 0)
    {
        m_comboSourceDevice.SetCurSel(0);
        EnumerateSourceFormats();
    }
}

//////////////////////////////////////////////////////
// Source format enumeration (from selected webcam)
//////////////////////////////////////////////////////

void CMainDlg::EnumerateSourceFormats()
{
    m_comboSourceFormat.ResetContent();
    m_comboSourceFrameRate.ResetContent();

    int sel = m_comboSourceDevice.GetCurSel();
    if (sel < 0 || sel >= m_videoDevices.GetSize()) return;

    // Bind moniker to filter
    IBaseFilter* pFilter = NULL;
    HRESULT hr = m_videoDevices[sel].pMoniker->BindToObject(
        0, 0, IID_IBaseFilter, (void**)&pFilter);
    if (FAILED(hr)) return;

    // Find output pin
    IEnumPins* pEnumPins = NULL;
    IPin* pOutputPin = NULL;
    hr = pFilter->EnumPins(&pEnumPins);
    if (SUCCEEDED(hr))
    {
        IPin* pPin = NULL;
        while (pEnumPins->Next(1, &pPin, NULL) == S_OK)
        {
            PIN_DIRECTION dir;
            pPin->QueryDirection(&dir);
            if (dir == PINDIR_OUTPUT)
            {
                pOutputPin = pPin;
                break;
            }
            pPin->Release();
        }
        pEnumPins->Release();
    }

    if (pOutputPin == NULL)
    {
        pFilter->Release();
        return;
    }

    IAMStreamConfig* pConfig = NULL;
    hr = pOutputPin->QueryInterface(IID_IAMStreamConfig, (void**)&pConfig);
    if (SUCCEEDED(hr))
    {
        int count = 0, size = 0;
        hr = pConfig->GetNumberOfCapabilities(&count, &size);
        if (SUCCEEDED(hr) && count > 0 && size > 0)
        {
            BYTE* pSCC = new BYTE[size];
            bool hasCapsInfo = (size >= (int)sizeof(VIDEO_STREAM_CONFIG_CAPS));
            bool addedFrameRates = false;

            for (int i = 0; i < count; i++)
            {
                AM_MEDIA_TYPE* pmt = NULL;
                hr = pConfig->GetStreamCaps(i, &pmt, pSCC);
                if (FAILED(hr)) continue;

                if (pmt->formattype == FORMAT_VideoInfo &&
                    pmt->cbFormat >= sizeof(VIDEOINFOHEADER))
                {
                    VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)pmt->pbFormat;

                    const WCHAR* subtype = L"Unknown";
                    if (pmt->subtype == MEDIASUBTYPE_YUY2) subtype = L"YUY2";
                    else if (pmt->subtype == MEDIASUBTYPE_RGB24) subtype = L"RGB24";
                    else if (pmt->subtype == MEDIASUBTYPE_RGB32) subtype = L"RGB32";
                    else if (pmt->subtype == MEDIASUBTYPE_YV12) subtype = L"YV12";
                    else if (pmt->subtype == MEDIASUBTYPE_NV12) subtype = L"NV12";
                    else if (pmt->subtype == MEDIASUBTYPE_MJPG) subtype = L"MJPG";

                    CString strFormat;
                    strFormat.Format(_T("%dx%d %s"),
                        (int)pVih->bmiHeader.biWidth,
                        (int)labs(pVih->bmiHeader.biHeight),
                        subtype);

                    int idx = m_comboSourceFormat.AddString(strFormat);
                    m_comboSourceFormat.SetItemData(idx, (DWORD_PTR)i);

                    if (!addedFrameRates && hasCapsInfo)
                    {
                        VIDEO_STREAM_CONFIG_CAPS* pCaps = (VIDEO_STREAM_CONFIG_CAPS*)pSCC;
                        long long minInterval = pCaps->MinFrameInterval;
                        long long maxInterval = pCaps->MaxFrameInterval;
                        int maxFps = (minInterval > 0) ? (int)(10000000LL / minInterval) : 30;
                        int minFps = (maxInterval > 0) ? (int)(10000000LL / maxInterval) : 1;

                        int commonRates[] = { 5, 10, 15, 20, 25, 30, 50, 60 };
                        for (int r = 0; r < 8; r++)
                        {
                            if (commonRates[r] >= minFps && commonRates[r] <= maxFps)
                            {
                                CString strRate;
                                strRate.Format(_T("%d"), commonRates[r]);
                                m_comboSourceFrameRate.AddString(strRate);
                            }
                        }
                        addedFrameRates = true;
                    }
                }
                DeleteMediaType(pmt);
            }
            delete[] pSCC;
        }
        pConfig->Release();
    }

    pOutputPin->Release();
    pFilter->Release();

    if (m_comboSourceFormat.GetCount() > 0)
        m_comboSourceFormat.SetCurSel(0);
    if (m_comboSourceFrameRate.GetCount() > 0)
    {
        for (int i = 0; i < m_comboSourceFrameRate.GetCount(); i++)
        {
            CString text;
            m_comboSourceFrameRate.GetLBText(i, text);
            if (text == _T("25") || text == _T("30"))
            {
                m_comboSourceFrameRate.SetCurSel(i);
                break;
            }
        }
        if (m_comboSourceFrameRate.GetCurSel() < 0)
            m_comboSourceFrameRate.SetCurSel(0);
    }
}

void CMainDlg::OnCbnSelchangeSourceDevice()
{
    EnumerateSourceFormats();
}

//////////////////////////////////////////////////////
// Set source video format on webcam filter
//////////////////////////////////////////////////////

bool CMainDlg::SetSourceVideoFormat()
{
    if (m_pSrcWebcamFilter == NULL) return false;

    int comboSel = m_comboSourceFormat.GetCurSel();
    if (comboSel < 0) return false;

    int capsIndex = (int)m_comboSourceFormat.GetItemData(comboSel);

    CString strRate;
    m_comboSourceFrameRate.GetWindowText(strRate);
    double fps = _ttof(strRate);
    if (fps <= 0) fps = 25.0;

    IEnumPins* pEnumPins = NULL;
    IPin* pOutputPin = NULL;
    HRESULT hr = m_pSrcWebcamFilter->EnumPins(&pEnumPins);
    if (FAILED(hr)) return false;

    IPin* pPin = NULL;
    while (pEnumPins->Next(1, &pPin, NULL) == S_OK)
    {
        PIN_DIRECTION dir;
        pPin->QueryDirection(&dir);
        if (dir == PINDIR_OUTPUT)
        {
            pOutputPin = pPin;
            break;
        }
        pPin->Release();
    }
    pEnumPins->Release();
    if (pOutputPin == NULL) return false;

    IAMStreamConfig* pConfig = NULL;
    hr = pOutputPin->QueryInterface(IID_IAMStreamConfig, (void**)&pConfig);
    pOutputPin->Release();
    if (FAILED(hr)) return false;

    int count = 0, size = 0;
    hr = pConfig->GetNumberOfCapabilities(&count, &size);
    if (FAILED(hr) || capsIndex >= count || size == 0)
    {
        pConfig->Release();
        return false;
    }

    BYTE* pSCC = new BYTE[size];
    AM_MEDIA_TYPE* pmt = NULL;
    hr = pConfig->GetStreamCaps(capsIndex, &pmt, pSCC);
    if (SUCCEEDED(hr))
    {
        if (pmt->formattype == FORMAT_VideoInfo &&
            pmt->cbFormat >= sizeof(VIDEOINFOHEADER) && fps > 0)
        {
            VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)pmt->pbFormat;
            pVih->AvgTimePerFrame = (LONGLONG)(10000000.0 / fps);
        }
        hr = pConfig->SetFormat(pmt);
        DeleteMediaType(pmt);
    }

    delete[] pSCC;
    pConfig->Release();
    return SUCCEEDED(hr);
}

//////////////////////////////////////////////////////
// Start Source (webcam -> virtual camera sink + preview)
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedSourceStart()
{
    int devSel = m_comboSourceDevice.GetCurSel();
    if (devSel < 0 || devSel >= m_videoDevices.GetSize())
    {
        SetStatus(_T("No video device selected."));
        return;
    }

    SetStatus(_T("Starting source..."));

    HRESULT hr;

    // Create filter graph
    hr = CoCreateInstance(CLSID_FilterGraph, NULL,
        CLSCTX_INPROC_SERVER, IID_IFilterGraph2, (void**)&m_pSrcGraph);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create source FilterGraph."));
        return;
    }

    hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL,
        CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void**)&m_pSrcCapture);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create source CaptureGraphBuilder2."));
        ReleaseSourceGraph();
        return;
    }

    m_pSrcCapture->SetFiltergraph(m_pSrcGraph);

    // Add webcam filter
    hr = m_videoDevices[devSel].pMoniker->BindToObject(
        0, 0, IID_IBaseFilter, (void**)&m_pSrcWebcamFilter);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create webcam filter."));
        ReleaseSourceGraph();
        return;
    }

    hr = m_pSrcGraph->AddFilter(m_pSrcWebcamFilter, L"Webcam");
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to add webcam to graph."));
        ReleaseSourceGraph();
        return;
    }

    // Set format on webcam
    SetSourceVideoFormat();

    // Add Smart Tee to split webcam output -> preview + sink
    hr = CoCreateInstance(CLSID_SmartTee, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pSrcSmartTee);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create Smart Tee for source."));
        ReleaseSourceGraph();
        return;
    }
    m_pSrcGraph->AddFilter(m_pSrcSmartTee, L"Source Smart Tee");

    // Webcam -> Smart Tee
    hr = m_pSrcCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pSrcWebcamFilter, NULL, m_pSrcSmartTee);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to connect webcam to Smart Tee."));
        ReleaseSourceGraph();
        return;
    }

    // Add Virtual Camera Sink
    hr = CoCreateInstance(CLSID_VFVirtualCameraSink, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pSrcVideoSink);
    if (FAILED(hr))
    {
        SetStatus(_T("Virtual Camera Sink not registered."));
        ReleaseSourceGraph();
        return;
    }
    m_pSrcGraph->AddFilter(m_pSrcVideoSink, L"Virtual Camera Sink");

    // Smart Tee capture -> Virtual Camera Sink
    hr = m_pSrcCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pSrcSmartTee, NULL, m_pSrcVideoSink);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to connect Smart Tee to Virtual Camera Sink."));
        ReleaseSourceGraph();
        return;
    }

    // Smart Tee preview -> video renderer (for source preview)
    hr = m_pSrcCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pSrcSmartTee, NULL, NULL);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to render source preview."));
        ReleaseSourceGraph();
        return;
    }

    // Set up source video window
    hr = m_pSrcGraph->QueryInterface(IID_IVideoWindow, (void**)&m_pSrcVideoWindow);
    if (SUCCEEDED(hr))
    {
        m_pSrcVideoWindow->put_Owner((OAHWND)m_srcVideoPanel.GetSafeHwnd());
        m_pSrcVideoWindow->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS);
        ResizeSourceVideoWindow();
        m_pSrcVideoWindow->put_Visible(OATRUE);
    }

    // Add Virtual Audio Card Sink (optional)
    hr = CoCreateInstance(CLSID_VFVirtualAudioCardSink, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pSrcAudioSink);
    if (SUCCEEDED(hr))
    {
        m_pSrcGraph->AddFilter(m_pSrcAudioSink, L"Virtual Audio Card Sink");
    }

    // Run source graph
    hr = m_pSrcGraph->QueryInterface(IID_IMediaControl, (void**)&m_pSrcMediaControl);
    if (SUCCEEDED(hr))
    {
        hr = m_pSrcMediaControl->Run();
        if (SUCCEEDED(hr))
        {
            GetDlgItem(IDC_BTN_SOURCE_START)->EnableWindow(FALSE);
            GetDlgItem(IDC_BTN_SOURCE_STOP)->EnableWindow(TRUE);
            m_comboSourceDevice.EnableWindow(FALSE);
            m_comboSourceFormat.EnableWindow(FALSE);
            m_comboSourceFrameRate.EnableWindow(FALSE);

            CString devName;
            m_comboSourceDevice.GetLBText(devSel, devName);
            CString status;
            status.Format(_T("Source streaming: %s"), (LPCTSTR)devName);
            SetStatus(status);
            return;
        }
    }

    SetStatus(_T("Failed to start source graph."));
    ReleaseSourceGraph();
}

//////////////////////////////////////////////////////
// Stop Source
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedSourceStop()
{
    ReleaseSourceGraph();

    GetDlgItem(IDC_BTN_SOURCE_START)->EnableWindow(TRUE);
    GetDlgItem(IDC_BTN_SOURCE_STOP)->EnableWindow(FALSE);
    m_comboSourceDevice.EnableWindow(TRUE);
    m_comboSourceFormat.EnableWindow(TRUE);
    m_comboSourceFrameRate.EnableWindow(TRUE);

    SetStatus(_T("Source stopped."));
}

void CMainDlg::ResizeSourceVideoWindow()
{
    if (m_pSrcVideoWindow && m_srcVideoPanel.GetSafeHwnd())
    {
        CRect rc;
        m_srcVideoPanel.GetClientRect(&rc);
        m_pSrcVideoWindow->SetWindowPosition(0, 0, rc.Width(), rc.Height());
    }
}

void CMainDlg::ReleaseSourceGraph()
{
    if (m_pSrcMediaControl)
    {
        m_pSrcMediaControl->Stop();
        m_pSrcMediaControl->Release();
        m_pSrcMediaControl = NULL;
    }

    if (m_pSrcVideoWindow)
    {
        m_pSrcVideoWindow->put_Visible(OAFALSE);
        m_pSrcVideoWindow->put_Owner(NULL);
        m_pSrcVideoWindow->Release();
        m_pSrcVideoWindow = NULL;
    }

    if (m_pSrcAudioSink) { m_pSrcAudioSink->Release(); m_pSrcAudioSink = NULL; }
    if (m_pSrcAudioFilter) { m_pSrcAudioFilter->Release(); m_pSrcAudioFilter = NULL; }
    if (m_pSrcVideoSink) { m_pSrcVideoSink->Release(); m_pSrcVideoSink = NULL; }
    if (m_pSrcSmartTee) { m_pSrcSmartTee->Release(); m_pSrcSmartTee = NULL; }
    if (m_pSrcWebcamFilter) { m_pSrcWebcamFilter->Release(); m_pSrcWebcamFilter = NULL; }
    if (m_pSrcCapture) { m_pSrcCapture->Release(); m_pSrcCapture = NULL; }
    if (m_pSrcGraph) { m_pSrcGraph->Release(); m_pSrcGraph = NULL; }
}

//////////////////////////////////////////////////////
// Camera format enumeration (virtual camera)
//////////////////////////////////////////////////////

void CMainDlg::EnumerateCameraFormats()
{
    m_comboCameraFormat.ResetContent();
    m_comboCameraFrameRate.ResetContent();

    IBaseFilter* pFilter = NULL;
    HRESULT hr = CoCreateInstance(CLSID_VFVirtualCameraSource, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&pFilter);
    if (FAILED(hr))
    {
        SetStatus(_T("Virtual Camera Source not registered."));
        return;
    }

    IEnumPins* pEnumPins = NULL;
    IPin* pOutputPin = NULL;
    hr = pFilter->EnumPins(&pEnumPins);
    if (SUCCEEDED(hr))
    {
        IPin* pPin = NULL;
        while (pEnumPins->Next(1, &pPin, NULL) == S_OK)
        {
            PIN_DIRECTION dir;
            pPin->QueryDirection(&dir);
            if (dir == PINDIR_OUTPUT)
            {
                pOutputPin = pPin;
                break;
            }
            pPin->Release();
        }
        pEnumPins->Release();
    }

    if (pOutputPin == NULL)
    {
        pFilter->Release();
        return;
    }

    IAMStreamConfig* pConfig = NULL;
    hr = pOutputPin->QueryInterface(IID_IAMStreamConfig, (void**)&pConfig);
    if (SUCCEEDED(hr))
    {
        int count = 0, size = 0;
        hr = pConfig->GetNumberOfCapabilities(&count, &size);
        if (SUCCEEDED(hr) && count > 0 && size > 0)
        {
            BYTE* pSCC = new BYTE[size];
            bool hasCapsInfo = (size >= (int)sizeof(VIDEO_STREAM_CONFIG_CAPS));
            bool addedFrameRates = false;

            for (int i = 0; i < count; i++)
            {
                AM_MEDIA_TYPE* pmt = NULL;
                hr = pConfig->GetStreamCaps(i, &pmt, pSCC);
                if (FAILED(hr)) continue;

                if (pmt->formattype == FORMAT_VideoInfo &&
                    pmt->cbFormat >= sizeof(VIDEOINFOHEADER))
                {
                    VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)pmt->pbFormat;

                    const WCHAR* subtype = L"Unknown";
                    if (pmt->subtype == MEDIASUBTYPE_YUY2) subtype = L"YUY2";
                    else if (pmt->subtype == MEDIASUBTYPE_RGB24) subtype = L"RGB24";
                    else if (pmt->subtype == MEDIASUBTYPE_RGB32) subtype = L"RGB32";
                    else if (pmt->subtype == MEDIASUBTYPE_YV12) subtype = L"YV12";

                    CString strFormat;
                    strFormat.Format(_T("%dx%d %s"),
                        (int)pVih->bmiHeader.biWidth,
                        (int)labs(pVih->bmiHeader.biHeight),
                        subtype);

                    int idx = m_comboCameraFormat.AddString(strFormat);
                    m_comboCameraFormat.SetItemData(idx, (DWORD_PTR)i);

                    if (!addedFrameRates && hasCapsInfo)
                    {
                        VIDEO_STREAM_CONFIG_CAPS* pCaps = (VIDEO_STREAM_CONFIG_CAPS*)pSCC;
                        long long minInterval = pCaps->MinFrameInterval;
                        long long maxInterval = pCaps->MaxFrameInterval;
                        int maxFps = (minInterval > 0) ? (int)(10000000LL / minInterval) : 30;
                        int minFps = (maxInterval > 0) ? (int)(10000000LL / maxInterval) : 1;

                        int commonRates[] = { 5, 10, 15, 20, 25, 30, 50, 60 };
                        for (int r = 0; r < 8; r++)
                        {
                            if (commonRates[r] >= minFps && commonRates[r] <= maxFps)
                            {
                                CString strRate;
                                strRate.Format(_T("%d"), commonRates[r]);
                                m_comboCameraFrameRate.AddString(strRate);
                            }
                        }
                        addedFrameRates = true;
                    }
                }
                DeleteMediaType(pmt);
            }
            delete[] pSCC;
        }
        pConfig->Release();
    }

    pOutputPin->Release();
    pFilter->Release();

    if (m_comboCameraFormat.GetCount() > 0)
        m_comboCameraFormat.SetCurSel(0);
    if (m_comboCameraFrameRate.GetCount() > 0)
    {
        for (int i = 0; i < m_comboCameraFrameRate.GetCount(); i++)
        {
            CString text;
            m_comboCameraFrameRate.GetLBText(i, text);
            if (text == _T("25"))
            {
                m_comboCameraFrameRate.SetCurSel(i);
                break;
            }
        }
        if (m_comboCameraFrameRate.GetCurSel() < 0)
            m_comboCameraFrameRate.SetCurSel(0);
    }
}

//////////////////////////////////////////////////////
// Camera format configuration
//////////////////////////////////////////////////////

bool CMainDlg::SetCameraVideoFormat(int formatIndex, double frameRate)
{
    if (m_pCamCameraFilter == NULL) return false;

    IEnumPins* pEnumPins = NULL;
    IPin* pOutputPin = NULL;
    HRESULT hr = m_pCamCameraFilter->EnumPins(&pEnumPins);
    if (FAILED(hr)) return false;

    IPin* pPin = NULL;
    while (pEnumPins->Next(1, &pPin, NULL) == S_OK)
    {
        PIN_DIRECTION dir;
        pPin->QueryDirection(&dir);
        if (dir == PINDIR_OUTPUT)
        {
            pOutputPin = pPin;
            break;
        }
        pPin->Release();
    }
    pEnumPins->Release();
    if (pOutputPin == NULL) return false;

    IAMStreamConfig* pConfig = NULL;
    hr = pOutputPin->QueryInterface(IID_IAMStreamConfig, (void**)&pConfig);
    pOutputPin->Release();
    if (FAILED(hr)) return false;

    int count = 0, size = 0;
    hr = pConfig->GetNumberOfCapabilities(&count, &size);
    if (FAILED(hr) || formatIndex >= count || size == 0)
    {
        pConfig->Release();
        return false;
    }

    BYTE* pSCC = new BYTE[size];
    AM_MEDIA_TYPE* pmt = NULL;
    hr = pConfig->GetStreamCaps(formatIndex, &pmt, pSCC);
    if (SUCCEEDED(hr))
    {
        if (pmt->formattype == FORMAT_VideoInfo &&
            pmt->cbFormat >= sizeof(VIDEOINFOHEADER) && frameRate > 0)
        {
            VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)pmt->pbFormat;
            pVih->AvgTimePerFrame = (LONGLONG)(10000000.0 / frameRate);
        }
        hr = pConfig->SetFormat(pmt);
        DeleteMediaType(pmt);
    }

    delete[] pSCC;
    pConfig->Release();
    return SUCCEEDED(hr);
}

//////////////////////////////////////////////////////
// Build common camera graph (filter graph + camera source)
//////////////////////////////////////////////////////

bool CMainDlg::BuildCameraCommonGraph(double fps, int capsIndex)
{
    HRESULT hr;

    hr = CoCreateInstance(CLSID_FilterGraph, NULL,
        CLSCTX_INPROC_SERVER, IID_IFilterGraph2, (void**)&m_pCamGraph);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create camera FilterGraph."));
        return false;
    }

    hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL,
        CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void**)&m_pCamCapture);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to create camera CaptureGraphBuilder2."));
        ReleaseCameraGraph();
        return false;
    }

    m_pCamCapture->SetFiltergraph(m_pCamGraph);

    hr = CoCreateInstance(CLSID_VFVirtualCameraSource, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamCameraFilter);
    if (FAILED(hr))
    {
        SetStatus(_T("Virtual Camera Source not registered."));
        ReleaseCameraGraph();
        return false;
    }

    hr = m_pCamGraph->AddFilter(m_pCamCameraFilter, L"VisioForge Virtual Camera");
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to add virtual camera to graph."));
        ReleaseCameraGraph();
        return false;
    }

    if (capsIndex >= 0)
        SetCameraVideoFormat(capsIndex, fps);

    return true;
}

//////////////////////////////////////////////////////
// Camera Preview (preview only)
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedCameraPreview()
{
    SetStatus(_T("Starting camera preview..."));

    int comboSel = m_comboCameraFormat.GetCurSel();
    CString strRate;
    m_comboCameraFrameRate.GetWindowText(strRate);
    double fps = _ttof(strRate);
    if (fps <= 0) fps = 25.0;
    int capsIndex = (comboSel >= 0) ? (int)m_comboCameraFormat.GetItemData(comboSel) : -1;

    if (!BuildCameraCommonGraph(fps, capsIndex))
        return;

    HRESULT hr;

    hr = m_pCamCapture->RenderStream(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video,
        m_pCamCameraFilter, NULL, NULL);
    if (FAILED(hr))
    {
        SetStatus(_T("Failed to render camera video."));
        ReleaseCameraGraph();
        return;
    }

    hr = m_pCamGraph->QueryInterface(IID_IVideoWindow, (void**)&m_pCamVideoWindow);
    if (SUCCEEDED(hr))
    {
        m_pCamVideoWindow->put_Owner((OAHWND)m_camVideoPanel.GetSafeHwnd());
        m_pCamVideoWindow->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS);
        ResizeCameraVideoWindow();
        m_pCamVideoWindow->put_Visible(OATRUE);
    }

    // Optional audio
    hr = CoCreateInstance(CLSID_VFVirtualAudioCardSource, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamAudioFilter);
    if (SUCCEEDED(hr))
    {
        m_pCamGraph->AddFilter(m_pCamAudioFilter, L"Virtual Audio Card");
        IEnumPins* pEnumPins = NULL;
        m_pCamAudioFilter->EnumPins(&pEnumPins);
        if (pEnumPins)
        {
            IPin* pPin = NULL;
            while (pEnumPins->Next(1, &pPin, NULL) == S_OK)
            {
                PIN_DIRECTION dir;
                pPin->QueryDirection(&dir);
                if (dir == PINDIR_OUTPUT)
                {
                    m_pCamGraph->Render(pPin);
                    pPin->Release();
                    break;
                }
                pPin->Release();
            }
            pEnumPins->Release();
        }
    }

    hr = m_pCamGraph->QueryInterface(IID_IMediaControl, (void**)&m_pCamMediaControl);
    if (SUCCEEDED(hr))
    {
        hr = m_pCamMediaControl->Run();
        if (SUCCEEDED(hr))
        {
            GetDlgItem(IDC_BTN_CAMERA_PREVIEW)->EnableWindow(FALSE);
            GetDlgItem(IDC_BTN_CAMERA_CAPTURE)->EnableWindow(FALSE);
            GetDlgItem(IDC_BTN_CAMERA_STOP)->EnableWindow(TRUE);
            GetDlgItem(IDC_BTN_BROWSE)->EnableWindow(FALSE);
            GetDlgItem(IDC_EDIT_OUTPUT_FILE)->EnableWindow(FALSE);
            m_comboCameraFormat.EnableWindow(FALSE);
            m_comboCameraFrameRate.EnableWindow(FALSE);

            CString strFormat;
            if (comboSel >= 0)
                m_comboCameraFormat.GetLBText(comboSel, strFormat);
            else
                strFormat = _T("default");

            CString status;
            status.Format(_T("Camera preview: %s @ %s fps"), (LPCTSTR)strFormat, (LPCTSTR)strRate);
            SetStatus(status);
            return;
        }
    }

    SetStatus(_T("Failed to start camera preview."));
    ReleaseCameraGraph();
}

//////////////////////////////////////////////////////
// Camera Capture (preview + AVI file)
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedCameraCapture()
{
    CString outputPath;
    GetDlgItemText(IDC_EDIT_OUTPUT_FILE, outputPath);
    if (outputPath.IsEmpty())
    {
        AfxMessageBox(_T("Please specify an output file path."));
        return;
    }

    SetStatus(_T("Starting camera capture..."));

    int comboSel = m_comboCameraFormat.GetCurSel();
    CString strRate;
    m_comboCameraFrameRate.GetWindowText(strRate);
    double fps = _ttof(strRate);
    if (fps <= 0) fps = 25.0;
    int capsIndex = (comboSel >= 0) ? (int)m_comboCameraFormat.GetItemData(comboSel) : -1;

    if (!BuildCameraCommonGraph(fps, capsIndex))
        return;

    HRESULT hr;

    // Video Smart Tee
    hr = CoCreateInstance(CLSID_SmartTee, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamVideoSmartTee);
    if (FAILED(hr)) { SetStatus(_T("Failed to create video Smart Tee.")); ReleaseCameraGraph(); return; }
    m_pCamGraph->AddFilter(m_pCamVideoSmartTee, L"Video Smart Tee");

    // Camera -> Smart Tee
    hr = m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pCamCameraFilter, NULL, m_pCamVideoSmartTee);
    if (FAILED(hr)) { SetStatus(_T("Failed to connect camera to Smart Tee.")); ReleaseCameraGraph(); return; }

    // MJPEG compressor
    hr = CoCreateInstance(CLSID_MJPEGCompressor, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamVideoCodec);
    if (FAILED(hr)) { SetStatus(_T("Failed to create MJPEG compressor.")); ReleaseCameraGraph(); return; }
    m_pCamGraph->AddFilter(m_pCamVideoCodec, L"MJPEG Compressor");

    // Smart Tee capture -> MJPEG
    hr = m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pCamVideoSmartTee, NULL, m_pCamVideoCodec);
    if (FAILED(hr)) { SetStatus(_T("Failed to connect Smart Tee to MJPEG.")); ReleaseCameraGraph(); return; }

    // AVI Mux
    hr = CoCreateInstance(CLSID_AVIMux, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamAVIMux);
    if (FAILED(hr)) { SetStatus(_T("Failed to create AVI Mux.")); ReleaseCameraGraph(); return; }
    m_pCamGraph->AddFilter(m_pCamAVIMux, L"AVI Mux");

    // MJPEG -> AVI Mux
    hr = m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pCamVideoCodec, NULL, m_pCamAVIMux);
    if (FAILED(hr)) { SetStatus(_T("Failed to connect MJPEG to AVI Mux.")); ReleaseCameraGraph(); return; }

    // Smart Tee preview -> video renderer
    hr = m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Video,
        m_pCamVideoSmartTee, NULL, NULL);
    if (FAILED(hr)) { SetStatus(_T("Failed to render capture preview.")); ReleaseCameraGraph(); return; }

    // Video window
    hr = m_pCamGraph->QueryInterface(IID_IVideoWindow, (void**)&m_pCamVideoWindow);
    if (SUCCEEDED(hr))
    {
        m_pCamVideoWindow->put_Owner((OAHWND)m_camVideoPanel.GetSafeHwnd());
        m_pCamVideoWindow->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS);
        ResizeCameraVideoWindow();
        m_pCamVideoWindow->put_Visible(OATRUE);
    }

    // Audio (optional): Smart Tee -> PCM -> AVI Mux + DirectSound
    hr = CoCreateInstance(CLSID_VFVirtualAudioCardSource, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamAudioFilter);
    if (SUCCEEDED(hr))
    {
        m_pCamGraph->AddFilter(m_pCamAudioFilter, L"Virtual Audio Card");

        hr = CoCreateInstance(CLSID_SmartTee, NULL,
            CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamAudioSmartTee);
        if (SUCCEEDED(hr))
        {
            m_pCamGraph->AddFilter(m_pCamAudioSmartTee, L"Audio Smart Tee");
            m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Audio,
                m_pCamAudioFilter, NULL, m_pCamAudioSmartTee);

            hr = CoCreateInstance(CLSID_PCMAudioCodec, NULL,
                CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamAudioCodec);
            if (SUCCEEDED(hr))
            {
                m_pCamGraph->AddFilter(m_pCamAudioCodec, L"PCM Audio Codec");
                m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Audio,
                    m_pCamAudioSmartTee, NULL, m_pCamAudioCodec);
                m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Audio,
                    m_pCamAudioCodec, NULL, m_pCamAVIMux);
            }

            hr = CoCreateInstance(CLSID_DSoundRenderer, NULL,
                CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamAudioRenderer);
            if (SUCCEEDED(hr))
            {
                m_pCamGraph->AddFilter(m_pCamAudioRenderer, L"DirectSound Renderer");
                m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Audio,
                    m_pCamAudioSmartTee, NULL, m_pCamAudioRenderer);
            }
        }
    }

    // File Writer
    hr = CoCreateInstance(CLSID_FileWriter, NULL,
        CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_pCamFileWriter);
    if (FAILED(hr)) { SetStatus(_T("Failed to create File Writer.")); ReleaseCameraGraph(); return; }
    m_pCamGraph->AddFilter(m_pCamFileWriter, L"File Writer");

    IFileSinkFilter* pSink = NULL;
    hr = m_pCamFileWriter->QueryInterface(IID_IFileSinkFilter, (void**)&pSink);
    if (SUCCEEDED(hr))
    {
        pSink->SetFileName(CT2W(outputPath), NULL);
        pSink->Release();
    }

    // AVI Mux -> File Writer
    hr = m_pCamCapture->RenderStream(NULL, &MEDIATYPE_Stream,
        m_pCamAVIMux, NULL, m_pCamFileWriter);
    if (FAILED(hr)) { SetStatus(_T("Failed to connect AVI Mux to File Writer.")); ReleaseCameraGraph(); return; }

    // Run
    hr = m_pCamGraph->QueryInterface(IID_IMediaControl, (void**)&m_pCamMediaControl);
    if (SUCCEEDED(hr))
    {
        hr = m_pCamMediaControl->Run();
        if (SUCCEEDED(hr))
        {
            GetDlgItem(IDC_BTN_CAMERA_PREVIEW)->EnableWindow(FALSE);
            GetDlgItem(IDC_BTN_CAMERA_CAPTURE)->EnableWindow(FALSE);
            GetDlgItem(IDC_BTN_CAMERA_STOP)->EnableWindow(TRUE);
            GetDlgItem(IDC_BTN_BROWSE)->EnableWindow(FALSE);
            GetDlgItem(IDC_EDIT_OUTPUT_FILE)->EnableWindow(FALSE);
            m_comboCameraFormat.EnableWindow(FALSE);
            m_comboCameraFrameRate.EnableWindow(FALSE);

            CString status;
            status.Format(_T("Capturing to: %s"), (LPCTSTR)outputPath);
            SetStatus(status);
            return;
        }
    }

    SetStatus(_T("Failed to start capture."));
    ReleaseCameraGraph();
}

//////////////////////////////////////////////////////
// Camera Stop
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedCameraStop()
{
    ReleaseCameraGraph();

    GetDlgItem(IDC_BTN_CAMERA_PREVIEW)->EnableWindow(TRUE);
    GetDlgItem(IDC_BTN_CAMERA_CAPTURE)->EnableWindow(TRUE);
    GetDlgItem(IDC_BTN_CAMERA_STOP)->EnableWindow(FALSE);
    GetDlgItem(IDC_BTN_BROWSE)->EnableWindow(TRUE);
    GetDlgItem(IDC_EDIT_OUTPUT_FILE)->EnableWindow(TRUE);
    m_comboCameraFormat.EnableWindow(TRUE);
    m_comboCameraFrameRate.EnableWindow(TRUE);

    SetStatus(_T("Camera stopped."));
}

//////////////////////////////////////////////////////
// Browse for output file
//////////////////////////////////////////////////////

void CMainDlg::OnBnClickedBrowse()
{
    CFileDialog dlg(FALSE, _T("avi"), _T("output.avi"),
        OFN_OVERWRITEPROMPT,
        _T("AVI Files (*.avi)|*.avi|All Files (*.*)|*.*||"),
        this);

    if (dlg.DoModal() == IDOK)
        SetDlgItemText(IDC_EDIT_OUTPUT_FILE, dlg.GetPathName());
}

//////////////////////////////////////////////////////
// Camera video window resize
//////////////////////////////////////////////////////

void CMainDlg::ResizeCameraVideoWindow()
{
    if (m_pCamVideoWindow && m_camVideoPanel.GetSafeHwnd())
    {
        CRect rc;
        m_camVideoPanel.GetClientRect(&rc);
        m_pCamVideoWindow->SetWindowPosition(0, 0, rc.Width(), rc.Height());
    }
}

void CMainDlg::OnSize(UINT nType, int cx, int cy)
{
    CDialogEx::OnSize(nType, cx, cy);
    ResizeSourceVideoWindow();
    ResizeCameraVideoWindow();
}

//////////////////////////////////////////////////////
// Camera graph cleanup
//////////////////////////////////////////////////////

void CMainDlg::ReleaseCameraGraph()
{
    if (m_pCamMediaControl)
    {
        m_pCamMediaControl->Stop();
        m_pCamMediaControl->Release();
        m_pCamMediaControl = NULL;
    }

    if (m_pCamVideoWindow)
    {
        m_pCamVideoWindow->put_Visible(OAFALSE);
        m_pCamVideoWindow->put_Owner(NULL);
        m_pCamVideoWindow->Release();
        m_pCamVideoWindow = NULL;
    }

    if (m_pCamAudioRenderer) { m_pCamAudioRenderer->Release(); m_pCamAudioRenderer = NULL; }
    if (m_pCamFileWriter) { m_pCamFileWriter->Release(); m_pCamFileWriter = NULL; }
    if (m_pCamAVIMux) { m_pCamAVIMux->Release(); m_pCamAVIMux = NULL; }
    if (m_pCamAudioCodec) { m_pCamAudioCodec->Release(); m_pCamAudioCodec = NULL; }
    if (m_pCamVideoCodec) { m_pCamVideoCodec->Release(); m_pCamVideoCodec = NULL; }
    if (m_pCamAudioSmartTee) { m_pCamAudioSmartTee->Release(); m_pCamAudioSmartTee = NULL; }
    if (m_pCamVideoSmartTee) { m_pCamVideoSmartTee->Release(); m_pCamVideoSmartTee = NULL; }
    if (m_pCamAudioFilter) { m_pCamAudioFilter->Release(); m_pCamAudioFilter = NULL; }
    if (m_pCamCameraFilter) { m_pCamCameraFilter->Release(); m_pCamCameraFilter = NULL; }
    if (m_pCamCapture) { m_pCamCapture->Release(); m_pCamCapture = NULL; }
    if (m_pCamGraph) { m_pCamGraph->Release(); m_pCamGraph = NULL; }
}

void CMainDlg::OnDestroy()
{
    ReleaseSourceGraph();
    ReleaseCameraGraph();
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
