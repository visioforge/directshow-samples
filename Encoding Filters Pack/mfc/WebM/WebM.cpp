#include "WebM.h"

static const GUID kClsidConverter = { 0x3A65D681, 0x4840, 0x4077, { 0xBC, 0xA6, 0x38, 0x1F, 0x25, 0x64, 0xBE, 0xBC } };
static const GUID kClsidVp8 = { 0x7B2B9E53, 0xF65D, 0x4967, { 0xBE, 0x13, 0x66, 0x29, 0x5E, 0x90, 0x38, 0x2E } };
static const GUID kClsidMux = { 0x25908CCA, 0x6ED3, 0x4FC8, { 0xA3, 0x58, 0x6D, 0x30, 0x39, 0x8D, 0x15, 0xBF } };
static const GUID kClsidVorbis = { 0xF86DC6FF, 0x66F2, 0x4B42, { 0xBE, 0x37, 0x2E, 0xFE, 0x9D, 0xD4, 0xF5, 0x49 } };
static const GUID kI420 = { 0x30323449, 0x0000, 0x0010, { 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 } };
static const GUID kYV12 = { 0x32315659, 0x0000, 0x0010, { 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 } };
static const GUID kIID_Converter = { 0x25AB8F6C, 0x2CB3, 0x46A8, { 0x90, 0xE3, 0xBB, 0x2F, 0x27, 0xA9, 0x9C, 0xEA } };
static const GUID kIID_VPX = { 0xED311152, 0x5211, 0x11DF, { 0x94, 0xAF, 0x00, 0x26, 0xB9, 0x77, 0xEE, 0xAB } };
static const GUID kIID_Vorbis = { 0xA4C6A887, 0x7BD3, 0x4B33, { 0x9A, 0x57, 0xA3, 0xEB, 0x10, 0x92, 0x4D, 0x3A } };

#ifdef _WIN64
static const GUID kIID_VP8 = { 0xED3110FE, 0x5211, 0x11DF, { 0x94, 0xAF, 0x00, 0x26, 0xB9, 0x77, 0xEE, 0xAA } };
#else
static const GUID kIID_VP8 = { 0xED311151, 0x5211, 0x11DF, { 0x94, 0xAF, 0x00, 0x26, 0xB9, 0x77, 0xEE, 0xAA } };
#endif

struct IVFVideoConverter : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetOutputSubtypes(const GUID* subtypes, int count) = 0;
};

struct IVPXEncoderKind : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetEncoderKind(int kind) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetEncoderKind(int* kind) = 0;
};

// Vtable order matches the filter through SetTargetBitrate. Bitrate is kilobits per second.
struct IVP8Encoder : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE ApplySettings() = 0;
    virtual HRESULT STDMETHODCALLTYPE ResetSettings() = 0;
    virtual HRESULT STDMETHODCALLTYPE SetDeadline(int deadline) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetDeadline(int* deadline) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetThreadCount(int threads) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetThreadCount(int* threads) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetErrorResilient(int value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetErrorResilient(int* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetDropframeThreshold(int value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetDropframeThreshold(int* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetResizeAllowed(int value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetResizeAllowed(int* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetResizeUpThreshold(int value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetResizeUpThreshold(int* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetResizeDownThreshold(int value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetResizeDownThreshold(int* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetEndUsage(int endUsage) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetEndUsage(int* endUsage) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetLagInFrames(int frames) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetLagInFrames(int* frames) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetTokenPartitions(int value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetTokenPartitions(int* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetTargetBitrate(int bitrateKbps) = 0;
};

struct IVorbisEncode : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE GetEncoderSettings() = 0;
    virtual BOOL STDMETHODCALLTYPE SetQuality(int quality) = 0;
};

namespace
{
    void EnsureParent(const wchar_t* path)
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
}

CWebMApp theApp;

BOOL CWebMApp::InitInstance()
{
    CWinApp::InitInstance();
    CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
    CWebMDlg dialog;
    m_pMainWnd = &dialog;
    dialog.DoModal();
    CoUninitialize();
    return FALSE;
}

CWebMDlg::CWebMDlg()
    : CDialogEx(IDD_WEBM)
    , m_graph(NULL)
    , m_capture(NULL)
    , m_control(NULL)
    , m_events(NULL)
    , m_window(NULL)
    , m_running(false)
{
}

BOOL CWebMDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();
    HICON icon = AfxGetApp()->LoadIcon(IDI_ICON);
    SetIcon(icon, TRUE);
    SetIcon(icon, FALSE);
    CheckDlgButton(IDC_FILE, BST_CHECKED);
    CheckDlgButton(IDC_AUDIO, BST_CHECKED);
    SetDlgItemInt(IDC_VBITRATE, 2000, FALSE);

    CComboBox* rates = (CComboBox*)GetDlgItem(IDC_RATE);
    int vbr = rates->AddString(L"VBR");
    rates->SetItemData(vbr, 0);
    int cbr = rates->AddString(L"CBR");
    rates->SetItemData(cbr, 1);
    rates->SetCurSel(0);

    FillDevices(CLSID_VideoInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE), false);
    FillDevices(CLSID_AudioInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE), true);

    wchar_t output[MAX_PATH];
    DocumentsFile(output, MAX_PATH, L"encode.webm");
    SetDlgItemText(IDC_OUTPUT, output);
    GetDlgItem(IDC_STOP)->EnableWindow(FALSE);
    UpdateSourceMode();
    return TRUE;
}

void CWebMDlg::OnOK()
{
}

void CWebMDlg::OnCancel()
{
    StopGraph();
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE));
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE));
    CDialogEx::OnCancel();
}

void CWebMDlg::UpdateSourceMode()
{
    const BOOL file = IsDlgButtonChecked(IDC_FILE) == BST_CHECKED;
    GetDlgItem(IDC_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_BROWSE_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_CAMERA_DEVICE)->EnableWindow(!file);
    GetDlgItem(IDC_AUDIO_DEVICE)->EnableWindow(!file);
}

void CWebMDlg::OnSourceMode()
{
    UpdateSourceMode();
}

void CWebMDlg::OnBrowseSource()
{
    CFileDialog dialog(TRUE, NULL, NULL, OFN_FILEMUSTEXIST, L"Media files|*.mp4;*.mkv;*.mov;*.avi;*.wmv;*.webm|All files|*.*||", this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_SOURCE, dialog.GetPathName());
    }
}

void CWebMDlg::OnBrowseOutput()
{
    CFileDialog dialog(FALSE, L"webm", L"encode", OFN_OVERWRITEPROMPT, L"WebM|*.webm|All files|*.*||", this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_OUTPUT, dialog.GetPathName());
    }
}

bool CWebMDlg::Failed(HRESULT hr, const wchar_t* step)
{
    if (SUCCEEDED(hr))
    {
        return false;
    }

    ShowFailure(hr, step);
    StopGraph();
    return true;
}

HRESULT CWebMDlg::ConnectFree(IBaseFilter* from, IBaseFilter* to)
{
    IEnumPins* pins = NULL;
    HRESULT hr = from->EnumPins(&pins);
    if (FAILED(hr))
    {
        return hr;
    }

    IPin* output = NULL;
    IPin* fetched = NULL;
    while (output == NULL && pins->Next(1, &fetched, NULL) == S_OK)
    {
        PIN_DIRECTION direction;
        fetched->QueryDirection(&direction);
        IPin* peer = NULL;
        const HRESULT connected = fetched->ConnectedTo(&peer);
        if (peer != NULL)
        {
            peer->Release();
        }

        if (direction == PINDIR_OUTPUT && connected != S_OK)
        {
            output = fetched;
        }
        else
        {
            fetched->Release();
        }
    }

    pins->Release();
    if (output == NULL)
    {
        return VFW_E_NOT_FOUND;
    }

    hr = to->EnumPins(&pins);
    if (FAILED(hr))
    {
        output->Release();
        return hr;
    }

    IPin* input = NULL;
    while (input == NULL && pins->Next(1, &fetched, NULL) == S_OK)
    {
        PIN_DIRECTION direction;
        fetched->QueryDirection(&direction);
        IPin* peer = NULL;
        const HRESULT connected = fetched->ConnectedTo(&peer);
        if (peer != NULL)
        {
            peer->Release();
        }

        if (direction == PINDIR_INPUT && connected != S_OK)
        {
            input = fetched;
        }
        else
        {
            fetched->Release();
        }
    }

    pins->Release();
    if (input == NULL)
    {
        output->Release();
        return VFW_E_NOT_FOUND;
    }

    hr = m_graph->Connect(output, input);
    output->Release();
    input->Release();
    return hr;
}

void CWebMDlg::OnStart()
{
    if (m_running)
    {
        return;
    }

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
            if (audioMoniker == (IMoniker*)(INT_PTR)-1)
            {
                audioMoniker = NULL;
            }
        }
    }
    else if (GetFileAttributesW(sourcePath) == INVALID_FILE_ATTRIBUTES)
    {
        AfxMessageBox(L"Choose a source file.");
        return;
    }

    CComboBox* rates = (CComboBox*)GetDlgItem(IDC_RATE);
    const int endUsage = (int)rates->GetItemData(rates->GetCurSel());
    const int bitrate = (int)GetDlgItemInt(IDC_VBITRATE, NULL, FALSE);
    const bool encodeAudio = IsDlgButtonChecked(IDC_AUDIO) == BST_CHECKED;
    EnsureParent(output);

    IBaseFilter* source = NULL;
    IBaseFilter* audioSource = NULL;
    IBaseFilter* tee = NULL;
    IBaseFilter* renderer = NULL;
    IBaseFilter* converter = NULL;
    IBaseFilter* vp8 = NULL;
    IBaseFilter* vorbis = NULL;
    IBaseFilter* mux = NULL;
    IBaseFilter* writer = NULL;

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
    hr = AddByClsid(m_graph, kClsidConverter, L"Video converter", &converter);
    if (Failed(hr, L"Add the video converter. Install the Encoding Filters Pack.")) goto cleanup;
    hr = AddByClsid(m_graph, kClsidVp8, L"VP8 encoder", &vp8);
    if (Failed(hr, L"Add the VP8 encoder. Install the Encoding Filters Pack.")) goto cleanup;
    hr = AddByClsid(m_graph, kClsidMux, L"WebM muxer", &mux);
    if (Failed(hr, L"Add the WebM muxer. Install the Encoding Filters Pack.")) goto cleanup;
    hr = AddByClsid(m_graph, CLSID_FileWriter, L"File writer", &writer);
    if (Failed(hr, L"Add the file writer")) goto cleanup;

    IVFVideoConverter* converterSettings = NULL;
    hr = converter->QueryInterface(kIID_Converter, (void**)&converterSettings);
    if (SUCCEEDED(hr))
    {
        const GUID subtypes[2] = { kI420, kYV12 };
        hr = converterSettings->SetOutputSubtypes(subtypes, 2);
        converterSettings->Release();
    }
    if (Failed(hr, L"Set I420 output")) goto cleanup;

    IVPXEncoderKind* kind = NULL;
    if (SUCCEEDED(vp8->QueryInterface(kIID_VPX, (void**)&kind)) && kind != NULL)
    {
        kind->SetEncoderKind(0);
        kind->Release();
    }

    IVP8Encoder* encoder = NULL;
    hr = vp8->QueryInterface(kIID_VP8, (void**)&encoder);
    if (Failed(hr, L"Get the VP8 encoder interface")) goto cleanup;
    SYSTEM_INFO info;
    GetSystemInfo(&info);
    hr = encoder->SetThreadCount((int)info.dwNumberOfProcessors);
    if (SUCCEEDED(hr)) hr = encoder->SetDeadline(fileSource ? 0 : 1);
    if (SUCCEEDED(hr)) hr = encoder->SetEndUsage(endUsage);
    if (SUCCEEDED(hr) && !fileSource) hr = encoder->SetLagInFrames(0);
    if (SUCCEEDED(hr)) hr = encoder->SetTargetBitrate(bitrate);
    encoder->Release();
    if (Failed(hr, L"Set the VP8 bitrate")) goto cleanup;

    if (encodeAudio)
    {
        hr = AddByClsid(m_graph, kClsidVorbis, L"Vorbis encoder", &vorbis);
        if (Failed(hr, L"Add the Vorbis encoder. Install the Encoding Filters Pack.")) goto cleanup;
        IVorbisEncode* vorbisSettings = NULL;
        if (SUCCEEDED(vorbis->QueryInterface(kIID_Vorbis, (void**)&vorbisSettings)) && vorbisSettings != NULL)
        {
            vorbisSettings->SetQuality(80);
            vorbisSettings->Release();
        }
    }

    IFileSinkFilter* sink = NULL;
    hr = writer->QueryInterface(IID_IFileSinkFilter, (void**)&sink);
    if (SUCCEEDED(hr))
    {
        hr = sink->SetFileName(output, NULL);
        sink->Release();
    }
    if (Failed(hr, L"Set the output path")) goto cleanup;

    hr = renderer->QueryInterface(IID_IVideoWindow, (void**)&m_window);
    if (Failed(hr, L"Get the preview window")) goto cleanup;
    CWnd* preview = GetDlgItem(IDC_PREVIEW);
    RECT previewRect;
    preview->GetClientRect(&previewRect);
    m_window->put_Owner((OAHWND)preview->GetSafeHwnd());
    m_window->put_WindowStyle(WS_CHILD | WS_CLIPCHILDREN);
    m_window->SetWindowPosition(0, 0, previewRect.right, previewRect.bottom);
    m_window->put_Visible(OATRUE);

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, source, NULL, tee);
    if (Failed(hr, L"Connect the source to the tee")) goto cleanup;
    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, tee, NULL, renderer);
    if (Failed(hr, L"Connect the preview")) goto cleanup;
    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, tee, converter, vp8);
    if (Failed(hr, L"Connect VP8")) goto cleanup;
    hr = ConnectFree(vp8, mux);
    if (Failed(hr, L"Connect VP8 to the muxer")) goto cleanup;
    hr = ConnectFree(mux, writer);
    if (Failed(hr, L"Connect the muxer to the file writer")) goto cleanup;

    if (encodeAudio)
    {
        if (audioSource == NULL)
        {
            AfxMessageBox(L"Select an audio device, or turn audio off.");
            StopGraph();
            goto cleanup;
        }

        hr = m_capture->RenderStream(NULL, &MEDIATYPE_Audio, audioSource, vorbis, mux);
        if (Failed(hr, L"Connect Vorbis")) goto cleanup;
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
    ReleaseFilter(&converter);
    ReleaseFilter(&vp8);
    ReleaseFilter(&vorbis);
    ReleaseFilter(&mux);
    ReleaseFilter(&writer);
}

void CWebMDlg::OnStop()
{
    SetDlgItemText(IDC_STATUS, L"Stopping...");
    StopGraph();
    SetDlgItemText(IDC_STATUS, L"Stopped.");
}

void CWebMDlg::StopGraph()
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

LRESULT CWebMDlg::OnGraphNotify(WPARAM, LPARAM)
{
    if (!m_running || m_events == NULL)
    {
        return 0;
    }

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

BEGIN_MESSAGE_MAP(CWebMDlg, CDialogEx)
    ON_BN_CLICKED(IDC_START, OnStart)
    ON_BN_CLICKED(IDC_STOP, OnStop)
    ON_BN_CLICKED(IDC_BROWSE_SOURCE, OnBrowseSource)
    ON_BN_CLICKED(IDC_BROWSE_OUTPUT, OnBrowseOutput)
    ON_BN_CLICKED(IDC_FILE, OnSourceMode)
    ON_BN_CLICKED(IDC_CAMERA, OnSourceMode)
    ON_MESSAGE(WM_GRAPH_NOTIFY, OnGraphNotify)
END_MESSAGE_MAP()
