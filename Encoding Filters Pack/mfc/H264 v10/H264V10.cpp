#include "H264V10.h"

static const GUID kClsidRgb = { 0x3BDA461E, 0x12DB, 0x4C24, { 0x98, 0x15, 0xB6, 0x8D, 0x1A, 0xA4, 0xD3, 0x4A } };
static const GUID kClsidH264 = { 0xEA1FED6B, 0xB876, 0x4DB0, { 0xB7, 0xB1, 0x77, 0x84, 0x63, 0xE5, 0x99, 0x78 } };
static const GUID kClsidAac = { 0x763CAC70, 0x373C, 0x4892, { 0x89, 0x8B, 0xAC, 0x80, 0x66, 0x1B, 0x15, 0xF3 } };
static const GUID kClsidMux = { 0x0B0D654C, 0x7AC1, 0x441E, { 0x9C, 0x4D, 0x3C, 0x29, 0xAB, 0xED, 0xB6, 0xA8 } };
static const GUID kIID_H264 = { 0x2A483975, 0xC5B2, 0x4D1F, { 0x8D, 0xBB, 0x23, 0xA6, 0x53, 0xAF, 0x5E, 0x39 } };
static const GUID kIID_Aac = { 0x0BEF7533, 0x39E6, 0x42A5, { 0x86, 0x3F, 0xE0, 0x87, 0xFA, 0xB5, 0xD8, 0x4F } };
static const GUID kIID_Mux = { 0x9E26CE8B, 0x6708, 0x4535, { 0xAA, 0xA4, 0x23, 0xF9, 0xA9, 0x7C, 0x79, 0x37 } };

struct IntelPsControl
{
    unsigned GopPicSize;
    unsigned GopRefDist;
    unsigned NumSlice;
    unsigned BufferSizeInKB;
};

struct IntelFrameControl
{
    unsigned width;
    unsigned height;
};

struct IntelThrottle
{
    int policy;
    unsigned bitrateUp;
    unsigned bitrateDown;
    unsigned qpUp;
    unsigned qpDown;
};

struct IntelRate
{
    int method;
    unsigned bitrate;
};

struct IntelParams
{
    int profile;
    int level;
    IntelPsControl pictures;
    int pictureControl;
    IntelFrameControl frame;
    IntelThrottle throttle;
    IntelRate rate;
    int preset;
    int targetUsage;
};

struct IIntelConfigureVideoEncoder : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetParams(IntelParams parameters) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetParams(IntelParams* parameters) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetRunTimeStatistics(void* statistics) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetParams2(IntelParams parameters) = 0;
};

struct IVFAACEncoder : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetInputSampleRate(unsigned rate) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetInputSampleRate(unsigned* rate) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetInputChannels(short channels) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetInputChannels(short* channels) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetBitRate(unsigned bitrate) = 0;
};

struct IMP4V10MuxerConfig : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetFlags(unsigned value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetFlags(unsigned* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetLiveDisabled(BOOL liveDisabled) = 0;
};

static void EnsureParent(const wchar_t* path)
{
    wchar_t directory[MAX_PATH];
    StringCchCopyW(directory, MAX_PATH, path);
    wchar_t* slash = wcsrchr(directory, L'\\');
    if (slash == NULL)
    {
        return;
    }

    *slash = L'\0';
    SHCreateDirectoryExW(NULL, directory, NULL);
}

CH264V10App theApp;

BOOL CH264V10App::InitInstance()
{
    CWinApp::InitInstance();
    CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
    CH264V10Dlg dialog;
    m_pMainWnd = &dialog;
    dialog.DoModal();
    CoUninitialize();
    return FALSE;
}

CH264V10Dlg::CH264V10Dlg()
    : CDialogEx(IDD_H264V10), m_graph(NULL), m_capture(NULL), m_control(NULL), m_events(NULL), m_window(NULL), m_running(false)
{
}

BOOL CH264V10Dlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();
    HICON icon = AfxGetApp()->LoadIcon(IDI_ICON);
    SetIcon(icon, TRUE);
    SetIcon(icon, FALSE);
    CheckDlgButton(IDC_FILE, BST_CHECKED);
    CheckDlgButton(IDC_AUDIO, BST_CHECKED);
    SetDlgItemInt(IDC_VBITRATE, 4000, FALSE);
    SetDlgItemInt(IDC_ABITRATE, 128, FALSE);
    wchar_t output[MAX_PATH];
    DocumentsFile(output, MAX_PATH, L"encode.mp4");
    SetDlgItemText(IDC_OUTPUT, output);
    FillDevices(CLSID_VideoInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE), false);
    FillDevices(CLSID_AudioInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE), true);
    GetDlgItem(IDC_STOP)->EnableWindow(FALSE);
    UpdateSourceMode();
    return TRUE;
}

void CH264V10Dlg::OnOK() {}

void CH264V10Dlg::OnCancel()
{
    StopGraph();
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE));
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE));
    CDialogEx::OnCancel();
}

void CH264V10Dlg::UpdateSourceMode()
{
    const BOOL file = IsDlgButtonChecked(IDC_FILE) == BST_CHECKED;
    GetDlgItem(IDC_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_BROWSE_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_CAMERA_DEVICE)->EnableWindow(!file);
    GetDlgItem(IDC_AUDIO_DEVICE)->EnableWindow(!file);
}

void CH264V10Dlg::OnSourceMode() { UpdateSourceMode(); }

void CH264V10Dlg::OnBrowseSource()
{
    CFileDialog dialog(TRUE, NULL, NULL, OFN_FILEMUSTEXIST, L"Media files|*.mp4;*.mkv;*.mov;*.avi;*.wmv|All files|*.*||", this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_SOURCE, dialog.GetPathName());
    }
}

void CH264V10Dlg::OnBrowseOutput()
{
    CFileDialog dialog(FALSE, L"mp4", L"encode.mp4", OFN_OVERWRITEPROMPT, L"MP4|*.mp4|All files|*.*||", this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_OUTPUT, dialog.GetPathName());
    }
}

bool CH264V10Dlg::Failed(HRESULT hr, const wchar_t* step)
{
    if (SUCCEEDED(hr)) return false;
    ShowFailure(hr, step);
    StopGraph();
    return true;
}

void CH264V10Dlg::OnStart()
{
    if (m_running) return;

    const bool fileSource = IsDlgButtonChecked(IDC_FILE) == BST_CHECKED;
    wchar_t sourcePath[MAX_PATH];
    wchar_t output[MAX_PATH];
    GetDlgItemText(IDC_SOURCE, sourcePath, MAX_PATH);
    GetDlgItemText(IDC_OUTPUT, output, MAX_PATH);
    if (output[0] == L'\0')
    {
        AfxMessageBox(L"Choose an output path.");
        return;
    }

    CComboBox* cameras = (CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE);
    CComboBox* audioDevices = (CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE);
    IMoniker* videoMoniker = NULL;
    IMoniker* audioMoniker = NULL;
    if (!fileSource)
    {
        if (cameras->GetCurSel() < 0)
        {
            AfxMessageBox(L"No camera is selected.");
            return;
        }
        videoMoniker = (IMoniker*)cameras->GetItemDataPtr(cameras->GetCurSel());
        if (videoMoniker == NULL || videoMoniker == (IMoniker*)(INT_PTR)-1)
        {
            AfxMessageBox(L"No camera is selected.");
            return;
        }
        if (audioDevices->GetCurSel() > 0)
        {
            audioMoniker = (IMoniker*)audioDevices->GetItemDataPtr(audioDevices->GetCurSel());
            if (audioMoniker == (IMoniker*)(INT_PTR)-1) audioMoniker = NULL;
        }
    }
    else if (GetFileAttributesW(sourcePath) == INVALID_FILE_ATTRIBUTES)
    {
        AfxMessageBox(L"Choose a source file.");
        return;
    }

    const int videoKbps = (int)GetDlgItemInt(IDC_VBITRATE, NULL, FALSE);
    const int audioKbps = (int)GetDlgItemInt(IDC_ABITRATE, NULL, FALSE);
    const bool encodeAudio = IsDlgButtonChecked(IDC_AUDIO) == BST_CHECKED;
    EnsureParent(output);

    IBaseFilter* source = NULL;
    IBaseFilter* audioSource = NULL;
    IBaseFilter* tee = NULL;
    IBaseFilter* renderer = NULL;
    IBaseFilter* rgb = NULL;
    IBaseFilter* h264 = NULL;
    IBaseFilter* aac = NULL;
    IBaseFilter* mux = NULL;
    IFileSinkFilter* fileSink = NULL;
    IIntelConfigureVideoEncoder* encoder = NULL;
    CWnd* preview = NULL;
    RECT previewRect;
    ZeroMemory(&previewRect, sizeof(previewRect));

    HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void**)&m_graph);
    if (Failed(hr, L"Create the filter graph")) goto cleanup;
    hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void**)&m_capture);
    if (Failed(hr, L"Create the capture graph")) goto cleanup;
    hr = m_capture->SetFiltergraph(m_graph);
    if (Failed(hr, L"Attach the capture graph")) goto cleanup;
    hr = m_graph->QueryInterface(IID_IMediaControl, (void**)&m_control);
    if (Failed(hr, L"Get the media control")) goto cleanup;
    hr = m_graph->QueryInterface(IID_IMediaEventEx, (void**)&m_events);
    if (Failed(hr, L"Get the media events")) goto cleanup;
    hr = m_events->SetNotifyWindow((OAHWND)m_hWnd, WM_GRAPH_NOTIFY, 0);
    if (Failed(hr, L"Set the notify window")) goto cleanup;

    if (fileSource)
    {
        hr = m_graph->AddSourceFilter(sourcePath, L"Source", &source);
        if (Failed(hr, L"Add the source file")) goto cleanup;
        audioSource = source;
        audioSource->AddRef();
    }
    else
    {
        hr = BindDevice(videoMoniker, &source);
        if (Failed(hr, L"Open the camera")) goto cleanup;
        hr = AddFilter(m_graph, source, L"Camera");
        if (Failed(hr, L"Add the camera")) goto cleanup;
        if (audioMoniker != NULL)
        {
            hr = BindDevice(audioMoniker, &audioSource);
            if (Failed(hr, L"Open the audio device")) goto cleanup;
            hr = AddFilter(m_graph, audioSource, L"Audio device");
            if (Failed(hr, L"Add the audio device")) goto cleanup;
        }
    }

    hr = AddByClsid(m_graph, CLSID_InfTee, L"Preview tee", &tee);
    if (Failed(hr, L"Add the preview tee")) goto cleanup;
    hr = AddByClsid(m_graph, CLSID_VideoRenderer, L"Preview", &renderer);
    if (Failed(hr, L"Add the preview")) goto cleanup;
    hr = AddByClsid(m_graph, kClsidRgb, L"RGB to YUV", &rgb);
    if (Failed(hr, L"Add the color converter. Install the Encoding Filters Pack.")) goto cleanup;
    hr = AddByClsid(m_graph, kClsidH264, L"H.264 encoder", &h264);
    if (Failed(hr, L"Add the H.264 encoder. Install the Encoding Filters Pack.")) goto cleanup;
    hr = AddByClsid(m_graph, kClsidMux, L"MP4 muxer", &mux);
    if (Failed(hr, L"Add the MP4 muxer. Install the Encoding Filters Pack.")) goto cleanup;

    hr = h264->QueryInterface(kIID_H264, (void**)&encoder);
    if (SUCCEEDED(hr))
    {
        IntelParams parameters;
        ZeroMemory(&parameters, sizeof(parameters));
        hr = encoder->GetParams(&parameters);
        if (SUCCEEDED(hr))
        {
            parameters.rate.method = 1;
            // Kilobits per second.
            parameters.rate.bitrate = (unsigned)videoKbps;
            hr = encoder->SetParams2(parameters);
        }
        encoder->Release();
    }
    if (Failed(hr, L"Set the H.264 bitrate")) goto cleanup;

    if (fileSource)
    {
        IMP4V10MuxerConfig* live = NULL;
        hr = mux->QueryInterface(kIID_Mux, (void**)&live);
        if (SUCCEEDED(hr) && live != NULL)
        {
            hr = live->SetLiveDisabled(TRUE);
            live->Release();
        }
        if (Failed(hr, L"Turn live mode off")) goto cleanup;
    }

    hr = mux->QueryInterface(IID_IFileSinkFilter, (void**)&fileSink);
    if (SUCCEEDED(hr) && fileSink != NULL)
    {
        hr = fileSink->SetFileName(output, NULL);
        fileSink->Release();
        fileSink = NULL;
    }
    if (Failed(hr, L"Set the output path")) goto cleanup;

    if (encodeAudio)
    {
        hr = AddByClsid(m_graph, kClsidAac, L"AAC encoder", &aac);
        if (Failed(hr, L"Add the AAC encoder. Install the Encoding Filters Pack.")) goto cleanup;

        // The AAC encoder takes its bitrate only before its output pin is connected.
        IVFAACEncoder* aacSettings = NULL;
        hr = aac->QueryInterface(kIID_Aac, (void**)&aacSettings);
        if (SUCCEEDED(hr))
        {
            hr = aacSettings->SetBitRate((unsigned)audioKbps * 1000);
            aacSettings->Release();
        }
        if (Failed(hr, L"Set the AAC bitrate")) goto cleanup;
    }

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, source, NULL, tee);
    if (Failed(hr, L"Connect the source to the tee")) goto cleanup;
    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, tee, NULL, renderer);
    if (Failed(hr, L"Connect the preview")) goto cleanup;

    // The renderer has a window only once its input pin is connected.
    hr = renderer->QueryInterface(IID_IVideoWindow, (void**)&m_window);
    if (Failed(hr, L"Get the preview window")) goto cleanup;
    preview = GetDlgItem(IDC_PREVIEW);
    preview->GetClientRect(&previewRect);
    m_window->put_Owner((OAHWND)preview->GetSafeHwnd());
    m_window->put_WindowStyle(WS_CHILD | WS_CLIPCHILDREN);
    m_window->SetWindowPosition(0, 0, previewRect.right, previewRect.bottom);
    m_window->put_Visible(OATRUE);

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, tee, rgb, h264);
    if (Failed(hr, L"Connect H.264")) goto cleanup;
    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, h264, NULL, mux);
    if (Failed(hr, L"Connect MP4 video")) goto cleanup;

    if (encodeAudio)
    {
        if (audioSource == NULL)
        {
            AfxMessageBox(L"Select an audio device, or turn audio off.");
            StopGraph();
            goto cleanup;
        }
        hr = m_capture->RenderStream(NULL, &MEDIATYPE_Audio, audioSource, aac, mux);
        if (Failed(hr, L"Connect AAC")) goto cleanup;
    }

    if (fileSource)
    {
        IMediaFilter* clock = NULL;
        hr = m_graph->QueryInterface(IID_IMediaFilter, (void**)&clock);
        if (SUCCEEDED(hr))
        {
            hr = clock->SetSyncSource(NULL);
            clock->Release();
        }
        if (Failed(hr, L"Encode the file as fast as the machine allows")) goto cleanup;
    }

    m_running = true;
    hr = m_control->Run();
    if (Failed(hr, L"Run")) goto cleanup;
    GetDlgItem(IDC_START)->EnableWindow(FALSE);
    GetDlgItem(IDC_STOP)->EnableWindow(TRUE);
    SetDlgItemText(IDC_STATUS, L"Encoding.");

cleanup:
    ReleaseFilter(&source);
    ReleaseFilter(&audioSource);
    ReleaseFilter(&tee);
    ReleaseFilter(&renderer);
    ReleaseFilter(&rgb);
    ReleaseFilter(&h264);
    ReleaseFilter(&aac);
    ReleaseFilter(&mux);
}

void CH264V10Dlg::OnStop()
{
    SetDlgItemText(IDC_STATUS, L"Stopping...");
    StopGraph();
    SetDlgItemText(IDC_STATUS, L"Stopped.");
}

void CH264V10Dlg::StopGraph()
{
    if (m_window != NULL)
    {
        m_window->put_Visible(OAFALSE);
        m_window->put_Owner(NULL);
        m_window->Release();
        m_window = NULL;
    }
    if (m_control != NULL)
    {
        m_control->Stop();
        m_control->Release();
        m_control = NULL;
    }
    if (m_events != NULL)
    {
        m_events->SetNotifyWindow(NULL, 0, 0);
        m_events->Release();
        m_events = NULL;
    }
    if (m_graph != NULL)
    {
        m_graph->Release();
        m_graph = NULL;
    }
    if (m_capture != NULL)
    {
        m_capture->Release();
        m_capture = NULL;
    }
    m_running = false;
    if (GetSafeHwnd() != NULL)
    {
        GetDlgItem(IDC_START)->EnableWindow(TRUE);
        GetDlgItem(IDC_STOP)->EnableWindow(FALSE);
    }
}

LRESULT CH264V10Dlg::OnGraphNotify(WPARAM, LPARAM)
{
    if (!m_running || m_events == NULL) return 0;
    long code = 0;
    LONG_PTR param1 = 0;
    LONG_PTR param2 = 0;
    while (SUCCEEDED(m_events->GetEvent(&code, &param1, &param2, 0)))
    {
        const bool finished = code == EC_COMPLETE || code == EC_ERRORABORT || code == EC_USERABORT;
        m_events->FreeEventParams(code, param1, param2);
        if (finished)
        {
            StopGraph();
            SetDlgItemText(IDC_STATUS, code == EC_COMPLETE ? L"Finished." : L"The graph stopped.");
            break;
        }
    }
    return 0;
}

BEGIN_MESSAGE_MAP(CH264V10Dlg, CDialogEx)
    ON_BN_CLICKED(IDC_START, OnStart)
    ON_BN_CLICKED(IDC_STOP, OnStop)
    ON_BN_CLICKED(IDC_BROWSE_SOURCE, OnBrowseSource)
    ON_BN_CLICKED(IDC_BROWSE_OUTPUT, OnBrowseOutput)
    ON_BN_CLICKED(IDC_FILE, OnSourceMode)
    ON_BN_CLICKED(IDC_CAMERA, OnSourceMode)
    ON_MESSAGE(WM_GRAPH_NOTIFY, OnGraphNotify)
END_MESSAGE_MAP()
