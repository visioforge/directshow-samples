#include "Legacy.h"

static const GUID kClsidFfmpeg =
{ 0x69F244AB, 0x31A9, 0x4B74, { 0x81, 0xCB, 0xCF, 0x8C, 0x9F, 0xAC, 0xB2, 0xEC } };

static const GUID kIidFfmpeg =
{ 0x17B8FF7D, 0xA67F, 0x45CE, { 0xB4, 0x25, 0x0E, 0x4F, 0x60, 0x7D, 0x8C, 0x60 } };

// Passed by value. Every field is a 4-byte int or a pointer so the layout stays blittable.
struct CVFOutputSettings
{
    wchar_t* filename;
    int audioAvailable;
    int audioBitrate;
    int audioSamplerate;
    int audioChannels;
    int videoWidth;
    int videoHeight;
    int aspectRatioW;
    int aspectRatioH;
    int videoBitrate;
    int videoMaxRate;
    int videoMinRate;
    int videoBufferSize;
    int interlace;
    int videoGopSize;
    int tvSystem;
    int outputFormat;
};

struct IVFFFMPEGEncoder : public IUnknown
{
    virtual void STDMETHODCALLTYPE set_settings(CVFOutputSettings settings) = 0;
};

namespace
{
    struct FormatItem
    {
        const wchar_t* name;
        int format;
        const wchar_t* extension;
    };

    const FormatItem kFormats[] =
    {
        { L"FLV", 0, L".flv" },
        { L"MPEG-1", 1, L".mpg" },
        { L"MPEG-1 VCD", 2, L".mpg" },
        { L"MPEG-2", 3, L".mpg" },
        { L"MPEG-2 SVCD", 5, L".mpg" },
        { L"MPEG-2 DVD", 6, L".mpg" }
    };

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

    int PositiveOr(int value, int fallback)
    {
        return value > 0 ? value : fallback;
    }

    // VCD is 2, SVCD is 5, DVD is 6. Those discs are 4:3. Other containers follow the picture.
    void DisplayAspect(int format, int width, int height, int* ratioW, int* ratioH)
    {
        if (format == 2 || format == 5 || format == 6)
        {
            *ratioW = 4;
            *ratioH = 3;
            return;
        }

        if (width < 1)
        {
            width = 1;
        }

        if (height < 1)
        {
            height = 1;
        }

        int divisor = width;
        int rest = height;
        while (rest != 0)
        {
            int next = divisor % rest;
            divisor = rest;
            rest = next;
        }

        *ratioW = width / divisor;
        *ratioH = height / divisor;
    }
}

CLegacyApp theApp;

BOOL CLegacyApp::InitInstance()
{
    CWinApp::InitInstance();
    CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

    CLegacyDlg dialog;
    m_pMainWnd = &dialog;
    dialog.DoModal();

    CoUninitialize();
    return FALSE;
}

CLegacyDlg::CLegacyDlg()
    : CDialogEx(IDD_LEGACY)
    , m_graph(NULL)
    , m_capture(NULL)
    , m_control(NULL)
    , m_events(NULL)
    , m_window(NULL)
    , m_running(false)
{
}

BOOL CLegacyDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();
    CheckDlgButton(IDC_FILE, BST_CHECKED);
    CheckDlgButton(IDC_AUDIO, BST_CHECKED);
    SetDlgItemInt(IDC_WIDTH, 1280, FALSE);
    SetDlgItemInt(IDC_HEIGHT, 720, FALSE);
    SetDlgItemInt(IDC_VBITRATE, 4000, FALSE);
    SetDlgItemInt(IDC_ABITRATE, 128, FALSE);
    SetDlgItemInt(IDC_RATE, 48000, FALSE);
    SetDlgItemInt(IDC_CHANNELS, 2, FALSE);

    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    for (int i = 0; i < (int)(sizeof(kFormats) / sizeof(kFormats[0])); ++i)
    {
        int index = formats->AddString(kFormats[i].name);
        formats->SetItemData(index, (DWORD_PTR)kFormats[i].format);
    }

    formats->SetCurSel(0);

    CComboBox* tv = (CComboBox*)GetDlgItem(IDC_TV);
    tv->AddString(L"Default");
    tv->AddString(L"PAL");
    tv->AddString(L"NTSC");
    tv->SetCurSel(0);

    FillDevices(CLSID_VideoInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE), false);
    FillDevices(CLSID_AudioInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE), true);

    wchar_t output[MAX_PATH];
    DocumentsFile(output, MAX_PATH, L"encode.flv");
    SetDlgItemText(IDC_OUTPUT, output);
    GetDlgItem(IDC_STOP)->EnableWindow(FALSE);
    UpdateSourceMode();
    return TRUE;
}

void CLegacyDlg::OnOK()
{
}

void CLegacyDlg::OnCancel()
{
    StopGraph();
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE));
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE));
    CDialogEx::OnCancel();
}

void CLegacyDlg::UpdateSourceMode()
{
    const BOOL file = IsDlgButtonChecked(IDC_FILE) == BST_CHECKED;
    GetDlgItem(IDC_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_BROWSE_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_CAMERA_DEVICE)->EnableWindow(!file);
    GetDlgItem(IDC_AUDIO_DEVICE)->EnableWindow(!file);
}

void CLegacyDlg::UpdateOutputName()
{
    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    const int format = (int)formats->GetItemData(formats->GetCurSel());
    const wchar_t* extension = format == 0 ? L".flv" : L".mpg";
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
    StringCchPrintfW(path, MAX_PATH, L"%s\\encode%s", directory, extension);
    SetDlgItemText(IDC_OUTPUT, path);
}

void CLegacyDlg::OnSourceMode()
{
    UpdateSourceMode();
}

void CLegacyDlg::OnFormatChanged()
{
    UpdateOutputName();
}

void CLegacyDlg::OnBrowseSource()
{
    CFileDialog dialog(TRUE, NULL, NULL, OFN_FILEMUSTEXIST, L"Media files|*.mp4;*.mkv;*.mov;*.avi;*.wmv|All files|*.*||", this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_SOURCE, dialog.GetPathName());
    }
}

void CLegacyDlg::OnBrowseOutput()
{
    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    const int format = (int)formats->GetItemData(formats->GetCurSel());
    const wchar_t* extension = format == 0 ? L".flv" : L".mpg";
    wchar_t filter[64];
    StringCchPrintfW(filter, 64, L"Output|*%s|All files|*.*||", extension);
    CFileDialog dialog(FALSE, extension + 1, format == 0 ? L"encode.flv" : L"encode.mpg", OFN_OVERWRITEPROMPT, filter, this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_OUTPUT, dialog.GetPathName());
    }
}

bool CLegacyDlg::Failed(HRESULT hr, const wchar_t* step)
{
    if (SUCCEEDED(hr))
    {
        return false;
    }

    ShowFailure(hr, step);
    StopGraph();
    return true;
}

void CLegacyDlg::OnStart()
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

        int audioIndex = audioDevices->GetCurSel();
        if (audioIndex > 0)
        {
            audioMoniker = (IMoniker*)audioDevices->GetItemDataPtr(audioIndex);
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

    CComboBox* formats = (CComboBox*)GetDlgItem(IDC_FORMAT);
    CComboBox* tv = (CComboBox*)GetDlgItem(IDC_TV);
    const int format = (int)formats->GetItemData(formats->GetCurSel());
    const int tvSystem = tv->GetCurSel() < 0 ? 0 : tv->GetCurSel();
    const bool encodeAudio = IsDlgButtonChecked(IDC_AUDIO) == BST_CHECKED;
    const int width = PositiveOr((int)GetDlgItemInt(IDC_WIDTH, NULL, FALSE), 1280);
    const int height = PositiveOr((int)GetDlgItemInt(IDC_HEIGHT, NULL, FALSE), 720);
    const int videoBitrate = PositiveOr((int)GetDlgItemInt(IDC_VBITRATE, NULL, FALSE), 4000) * 1000;
    const int audioBitrate = PositiveOr((int)GetDlgItemInt(IDC_ABITRATE, NULL, FALSE), 128) * 1000;
    const int sampleRate = PositiveOr((int)GetDlgItemInt(IDC_RATE, NULL, FALSE), 48000);
    const int channels = PositiveOr((int)GetDlgItemInt(IDC_CHANNELS, NULL, FALSE), 2);
    EnsureParent(output);

    IBaseFilter* source = NULL;
    IBaseFilter* audioSource = NULL;
    IBaseFilter* tee = NULL;
    IBaseFilter* renderer = NULL;
    IBaseFilter* encoder = NULL;
    IVFFFMPEGEncoder* ffmpeg = NULL;
    wchar_t filename[MAX_PATH];
    CVFOutputSettings settings;
    CWnd* preview = NULL;
    RECT previewRect;

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

        audioSource = source;
        audioSource->AddRef();
    }
    else
    {
        hr = BindDevice(videoMoniker, &source);
        if (Failed(hr, L"Open the camera"))
        {
            goto cleanup;
        }

        hr = AddFilter(m_graph, source, L"Camera");
        if (Failed(hr, L"Add the camera"))
        {
            goto cleanup;
        }

        if (audioMoniker != NULL)
        {
            hr = BindDevice(audioMoniker, &audioSource);
            if (Failed(hr, L"Open the audio device"))
            {
                goto cleanup;
            }

            hr = AddFilter(m_graph, audioSource, L"Audio device");
            if (Failed(hr, L"Add the audio device"))
            {
                goto cleanup;
            }
        }
    }

    hr = AddByClsid(m_graph, CLSID_InfTee, L"Preview tee", &tee);
    if (Failed(hr, L"Add the preview tee"))
    {
        goto cleanup;
    }

    hr = AddByClsid(m_graph, CLSID_VideoRenderer, L"Preview", &renderer);
    if (Failed(hr, L"Add the preview"))
    {
        goto cleanup;
    }

    hr = AddByClsid(m_graph, kClsidFfmpeg, L"FFmpeg encoder", &encoder);
    if (Failed(hr, L"Add the FFmpeg encoder. Install the Encoding Filters Pack."))
    {
        goto cleanup;
    }

    hr = encoder->QueryInterface(kIidFfmpeg, (void**)&ffmpeg);
    if (Failed(hr, L"Get the encoder settings"))
    {
        goto cleanup;
    }

    StringCchCopyW(filename, MAX_PATH, output);
    ZeroMemory(&settings, sizeof(settings));
    settings.filename = filename;
    settings.audioAvailable = encodeAudio ? 1 : 0;
    settings.audioBitrate = audioBitrate;
    settings.audioSamplerate = sampleRate;
    settings.audioChannels = channels;
    settings.videoWidth = width;
    settings.videoHeight = height;
    DisplayAspect(format, width, height, &settings.aspectRatioW, &settings.aspectRatioH);
    settings.videoBitrate = videoBitrate;
    settings.videoMaxRate = videoBitrate;
    settings.videoMinRate = videoBitrate / 2;
    settings.videoBufferSize = videoBitrate / 8;
    settings.interlace = 0;
    settings.videoGopSize = 18;
    settings.tvSystem = tvSystem;
    settings.outputFormat = format;
    ffmpeg->set_settings(settings);
    ffmpeg->Release();

    hr = renderer->QueryInterface(IID_IVideoWindow, (void**)&m_window);
    if (Failed(hr, L"Get the preview window"))
    {
        goto cleanup;
    }

    preview = GetDlgItem(IDC_PREVIEW);
    preview->GetClientRect(&previewRect);
    m_window->put_Owner((OAHWND)preview->GetSafeHwnd());
    m_window->put_WindowStyle(WS_CHILD | WS_CLIPCHILDREN);
    m_window->SetWindowPosition(0, 0, previewRect.right, previewRect.bottom);
    m_window->put_Visible(OATRUE);

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, source, NULL, tee);
    if (Failed(hr, L"Connect the source to the tee"))
    {
        goto cleanup;
    }

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, tee, NULL, renderer);
    if (Failed(hr, L"Connect the preview"))
    {
        goto cleanup;
    }

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, tee, NULL, encoder);
    if (Failed(hr, L"Connect the encoder"))
    {
        goto cleanup;
    }

    if (encodeAudio)
    {
        if (audioSource == NULL)
        {
            AfxMessageBox(L"Select an audio device, or turn audio off.");
            StopGraph();
            goto cleanup;
        }

        hr = m_capture->RenderStream(NULL, &MEDIATYPE_Audio, audioSource, NULL, encoder);
        if (Failed(hr, L"Connect audio"))
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
    ReleaseFilter(&audioSource);
    ReleaseFilter(&tee);
    ReleaseFilter(&renderer);
    ReleaseFilter(&encoder);
}

void CLegacyDlg::OnStop()
{
    SetDlgItemText(IDC_STATUS, L"Stopping...");
    StopGraph();
    SetDlgItemText(IDC_STATUS, L"Stopped.");
}

void CLegacyDlg::StopGraph()
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

LRESULT CLegacyDlg::OnGraphNotify(WPARAM, LPARAM)
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

BEGIN_MESSAGE_MAP(CLegacyDlg, CDialogEx)
    ON_BN_CLICKED(IDC_START, OnStart)
    ON_BN_CLICKED(IDC_STOP, OnStop)
    ON_BN_CLICKED(IDC_BROWSE_SOURCE, OnBrowseSource)
    ON_BN_CLICKED(IDC_BROWSE_OUTPUT, OnBrowseOutput)
    ON_BN_CLICKED(IDC_FILE, OnSourceMode)
    ON_BN_CLICKED(IDC_CAMERA, OnSourceMode)
    ON_CBN_SELCHANGE(IDC_FORMAT, OnFormatChanged)
    ON_MESSAGE(WM_GRAPH_NOTIFY, OnGraphNotify)
END_MESSAGE_MAP()
