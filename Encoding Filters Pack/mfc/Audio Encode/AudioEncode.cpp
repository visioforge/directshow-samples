#include "AudioEncode.h"

static const GUID kClsidLame =
{ 0xA3744173, 0x10B0, 0x4F23, { 0xB8, 0xD7, 0x63, 0xA6, 0x8B, 0x20, 0xEB, 0x53 } };

static const GUID kIidLame =
{ 0x595EB9D1, 0xF454, 0x41AD, { 0xA1, 0xFA, 0xEC, 0x23, 0x2A, 0xD9, 0xDA, 0x52 } };

static const GUID kClsidAac =
{ 0x763CAC70, 0x373C, 0x4892, { 0x89, 0x8B, 0xAC, 0x80, 0x66, 0x1B, 0x15, 0xF3 } };

static const GUID kIidAac =
{ 0x0BEF7533, 0x39E6, 0x42A5, { 0x86, 0x3F, 0xE0, 0x87, 0xFA, 0xB5, 0xD8, 0x4F } };

static const GUID kClsidMp4 =
{ 0x0B0D654C, 0x7AC1, 0x441E, { 0x9C, 0x4D, 0x3C, 0x29, 0xAB, 0xED, 0xB6, 0xA8 } };

static const GUID kIidMp4 =
{ 0x9E26CE8B, 0x6708, 0x4535, { 0xAA, 0xA4, 0x23, 0xF9, 0xA9, 0x7C, 0x79, 0x37 } };

// Methods before set_Bitrate stay in the vtable so the bitrate call lands on the right slot.
struct IAudioEncoderProperties : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE get_PESOutputEnabled(DWORD* enabled) = 0;
    virtual HRESULT STDMETHODCALLTYPE set_PESOutputEnabled(DWORD enabled) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_Bitrate(DWORD* bitrate) = 0;
    virtual HRESULT STDMETHODCALLTYPE set_Bitrate(DWORD bitrate) = 0;
};

struct IVFAACEncoder : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetInputSampleRate(UINT32 sampleRate) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetInputSampleRate(UINT32* sampleRate) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetInputChannels(SHORT channels) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetInputChannels(SHORT* channels) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetBitRate(UINT32 bitRate) = 0;
};

struct IMP4V10MuxerConfig : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetFlags(UINT32 value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetFlags(UINT32* value) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetLiveDisabled(BOOL liveDisabled) = 0;
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

CAudioEncodeApp theApp;

BOOL CAudioEncodeApp::InitInstance()
{
    CWinApp::InitInstance();
    CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

    CAudioEncodeDlg dialog;
    m_pMainWnd = &dialog;
    dialog.DoModal();

    CoUninitialize();
    return FALSE;
}

CAudioEncodeDlg::CAudioEncodeDlg()
    : CDialogEx(IDD_AUDIO)
    , m_graph(NULL)
    , m_capture(NULL)
    , m_control(NULL)
    , m_events(NULL)
    , m_running(false)
{
}

BOOL CAudioEncodeDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();
    CheckDlgButton(IDC_FILE, BST_CHECKED);
    SetDlgItemInt(IDC_BITRATE, 128, FALSE);

    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    int mp3 = formats->AddString(L"MP3 (LAME)");
    formats->SetItemData(mp3, 1);
    int aac = formats->AddString(L"AAC in MP4");
    formats->SetItemData(aac, 0);
    formats->SetCurSel(0);

    FillDevices(CLSID_AudioInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE), false);

    wchar_t output[MAX_PATH];
    DocumentsFile(output, MAX_PATH, L"audio.mp3");
    SetDlgItemText(IDC_OUTPUT, output);
    GetDlgItem(IDC_STOP)->EnableWindow(FALSE);
    UpdateSourceMode();
    return TRUE;
}

void CAudioEncodeDlg::OnOK()
{
}

void CAudioEncodeDlg::OnCancel()
{
    StopGraph();
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE));
    CDialogEx::OnCancel();
}

void CAudioEncodeDlg::UpdateSourceMode()
{
    const BOOL file = IsDlgButtonChecked(IDC_FILE) == BST_CHECKED;
    GetDlgItem(IDC_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_BROWSE_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_AUDIO_DEVICE)->EnableWindow(!file);
}

void CAudioEncodeDlg::UpdateOutputName()
{
    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    const bool mp3 = formats->GetItemData(formats->GetCurSel()) == 1;
    wchar_t current[MAX_PATH];
    GetDlgItemText(IDC_OUTPUT, current, MAX_PATH);
    wchar_t directory[MAX_PATH];
    StringCchCopyW(directory, MAX_PATH, current);
    wchar_t* slash = wcsrchr(directory, L'\\');
    if (slash == NULL)
    {
        return;
    }

    *slash = L'\0';
    wchar_t path[MAX_PATH];
    StringCchPrintfW(path, MAX_PATH, L"%s\\audio%s", directory, mp3 ? L".mp3" : L".mp4");
    SetDlgItemText(IDC_OUTPUT, path);
}

void CAudioEncodeDlg::OnSourceMode()
{
    UpdateSourceMode();
}

void CAudioEncodeDlg::OnFormatChanged()
{
    UpdateOutputName();
}

void CAudioEncodeDlg::OnBrowseSource()
{
    CFileDialog dialog(TRUE, NULL, NULL, OFN_FILEMUSTEXIST, L"Media files|*.mp4;*.mkv;*.mov;*.avi;*.mp3;*.wav;*.m4a|All files|*.*||", this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_SOURCE, dialog.GetPathName());
    }
}

void CAudioEncodeDlg::OnBrowseOutput()
{
    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    const bool mp3 = formats->GetItemData(formats->GetCurSel()) == 1;
    const wchar_t* extension = mp3 ? L".mp3" : L".mp4";
    wchar_t filter[64];
    StringCchPrintfW(filter, 64, L"%s|*%s|All files|*.*||", mp3 ? L"MP3" : L"MP4", extension);
    CFileDialog dialog(FALSE, extension + 1, mp3 ? L"audio.mp3" : L"audio.mp4", OFN_OVERWRITEPROMPT, filter, this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_OUTPUT, dialog.GetPathName());
    }
}

bool CAudioEncodeDlg::Failed(HRESULT hr, const wchar_t* step)
{
    if (SUCCEEDED(hr))
    {
        return false;
    }

    ShowFailure(hr, step);
    StopGraph();
    return true;
}

void CAudioEncodeDlg::OnStart()
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

    CComboBox* devices = (CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE);
    IMoniker* audioMoniker = NULL;
    if (!fileSource)
    {
        if (devices->GetCurSel() < 0)
        {
            AfxMessageBox(L"No audio device is selected.");
            return;
        }

        audioMoniker = (IMoniker*)devices->GetItemDataPtr(devices->GetCurSel());
        if (audioMoniker == NULL || audioMoniker == (IMoniker*)(INT_PTR)-1)
        {
            AfxMessageBox(L"No audio device is selected.");
            return;
        }
    }
    else if (GetFileAttributesW(sourcePath) == INVALID_FILE_ATTRIBUTES)
    {
        AfxMessageBox(L"Choose a source file.");
        return;
    }

    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    const bool mp3 = formats->GetItemData(formats->GetCurSel()) == 1;
    int bitrate = (int)GetDlgItemInt(IDC_BITRATE, NULL, FALSE);
    if (bitrate <= 0)
    {
        bitrate = 128;
    }

    EnsureParent(output);

    IBaseFilter* source = NULL;
    IBaseFilter* encoder = NULL;
    IBaseFilter* sink = NULL;
    IFileSinkFilter* fileSink = NULL;

    HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void**)&m_graph);
    if (Failed(hr, L"Create the filter graph"))
    {
        goto cleanup;
    }

    hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void**)&m_capture);
    if (Failed(hr, L"Create the capture graph"))
    {
        goto cleanup;
    }

    hr = m_capture->SetFiltergraph(m_graph);
    if (Failed(hr, L"Attach the capture graph"))
    {
        goto cleanup;
    }

    hr = m_graph->QueryInterface(IID_IMediaControl, (void**)&m_control);
    if (Failed(hr, L"Get the media control"))
    {
        goto cleanup;
    }

    hr = m_graph->QueryInterface(IID_IMediaEventEx, (void**)&m_events);
    if (Failed(hr, L"Get the media events"))
    {
        goto cleanup;
    }

    hr = m_events->SetNotifyWindow((OAHWND)m_hWnd, WM_GRAPH_NOTIFY, 0);
    if (Failed(hr, L"Set the notify window"))
    {
        goto cleanup;
    }

    if (fileSource)
    {
        hr = m_graph->AddSourceFilter(sourcePath, L"Source", &source);
        if (Failed(hr, L"Add the source file"))
        {
            goto cleanup;
        }
    }
    else
    {
        hr = BindDevice(audioMoniker, &source);
        if (Failed(hr, L"Open the audio device"))
        {
            goto cleanup;
        }

        hr = AddFilter(m_graph, source, L"Audio device");
        if (Failed(hr, L"Add the audio device"))
        {
            goto cleanup;
        }
    }

    if (mp3)
    {
        hr = AddByClsid(m_graph, kClsidLame, L"LAME encoder", &encoder);
        if (Failed(hr, L"Add the LAME encoder. Install the Encoding Filters Pack."))
        {
            goto cleanup;
        }

        hr = AddByClsid(m_graph, CLSID_FileWriter, L"File writer", &sink);
        if (Failed(hr, L"Add the file writer"))
        {
            goto cleanup;
        }
    }
    else
    {
        hr = AddByClsid(m_graph, kClsidAac, L"AAC encoder", &encoder);
        if (Failed(hr, L"Add the AAC encoder. Install the Encoding Filters Pack."))
        {
            goto cleanup;
        }

        hr = AddByClsid(m_graph, kClsidMp4, L"MP4 muxer", &sink);
        if (Failed(hr, L"Add the MP4 muxer. Install the Encoding Filters Pack."))
        {
            goto cleanup;
        }

        if (fileSource)
        {
            IMP4V10MuxerConfig* live = NULL;
            hr = sink->QueryInterface(kIidMp4, (void**)&live);
            if (SUCCEEDED(hr) && live != NULL)
            {
                hr = live->SetLiveDisabled(TRUE);
                live->Release();
            }

            if (Failed(hr, L"Turn live mode off"))
            {
                goto cleanup;
            }
        }
    }

    hr = sink->QueryInterface(IID_IFileSinkFilter, (void**)&fileSink);
    if (SUCCEEDED(hr))
    {
        hr = fileSink->SetFileName(output, NULL);
        fileSink->Release();
    }

    if (Failed(hr, L"Set the output path"))
    {
        goto cleanup;
    }

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Audio, source, encoder, sink);
    if (Failed(hr, mp3 ? L"Connect LAME" : L"Connect AAC"))
    {
        goto cleanup;
    }

    if (mp3)
    {
        IAudioEncoderProperties* lame = NULL;
        hr = encoder->QueryInterface(kIidLame, (void**)&lame);
        if (SUCCEEDED(hr) && lame != NULL)
        {
            hr = lame->set_Bitrate((DWORD)bitrate);
            lame->Release();
        }

        if (Failed(hr, L"Set MP3 bitrate"))
        {
            goto cleanup;
        }
    }
    else
    {
        IVFAACEncoder* aac = NULL;
        hr = encoder->QueryInterface(kIidAac, (void**)&aac);
        if (SUCCEEDED(hr) && aac != NULL)
        {
            hr = aac->SetBitRate((UINT32)bitrate * 1000);
            aac->Release();
        }

        if (Failed(hr, L"Set AAC bitrate"))
        {
            goto cleanup;
        }
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

        if (Failed(hr, L"Encode the file as fast as the machine allows"))
        {
            goto cleanup;
        }
    }

    m_running = true;
    hr = m_control->Run();
    if (Failed(hr, L"Run"))
    {
        goto cleanup;
    }

    GetDlgItem(IDC_START)->EnableWindow(FALSE);
    GetDlgItem(IDC_STOP)->EnableWindow(TRUE);
    SetDlgItemText(IDC_STATUS, L"Encoding.");

cleanup:
    ReleaseFilter(&source);
    ReleaseFilter(&encoder);
    ReleaseFilter(&sink);
}

void CAudioEncodeDlg::OnStop()
{
    SetDlgItemText(IDC_STATUS, L"Stopping...");
    StopGraph();
    SetDlgItemText(IDC_STATUS, L"Stopped.");
}

void CAudioEncodeDlg::StopGraph()
{
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

LRESULT CAudioEncodeDlg::OnGraphNotify(WPARAM, LPARAM)
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

BEGIN_MESSAGE_MAP(CAudioEncodeDlg, CDialogEx)
    ON_BN_CLICKED(IDC_START, OnStart)
    ON_BN_CLICKED(IDC_STOP, OnStop)
    ON_BN_CLICKED(IDC_BROWSE_SOURCE, OnBrowseSource)
    ON_BN_CLICKED(IDC_BROWSE_OUTPUT, OnBrowseOutput)
    ON_BN_CLICKED(IDC_FILE, OnSourceMode)
    ON_BN_CLICKED(IDC_DEVICE, OnSourceMode)
    ON_CBN_SELCHANGE(IDC_FORMAT, OnFormatChanged)
    ON_MESSAGE(WM_GRAPH_NOTIFY, OnGraphNotify)
END_MESSAGE_MAP()
