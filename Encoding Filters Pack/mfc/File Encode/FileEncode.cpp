#include "FileEncode.h"

static const GUID kClsidMFMux =
{ 0x6BBF4BCA, 0x9473, 0x4A69, { 0x9B, 0xC2, 0x6F, 0xF6, 0xB3, 0xBB, 0xB0, 0x2B } };

static const GUID kClsidRgb2Yuv =
{ 0x3BDA461E, 0x12DB, 0x4C24, { 0x98, 0x15, 0xB6, 0x8D, 0x1A, 0xA4, 0xD3, 0x4A } };

namespace
{
    void AddEncoder(CComboBox& combo, const wchar_t* name, VFMFVideoEncoder encoder)
    {
        int index = combo.AddString(name);
        combo.SetItemData(index, (DWORD_PTR)encoder);
    }

    void AddMux(CComboBox& combo, const wchar_t* name, VFMFMuxer muxer)
    {
        int index = combo.AddString(name);
        combo.SetItemData(index, (DWORD_PTR)muxer);
    }

    void AddRate(CComboBox& combo, const wchar_t* name, VFMFCommonRateControlMode mode)
    {
        int index = combo.AddString(name);
        combo.SetItemData(index, (DWORD_PTR)mode);
    }

    const wchar_t* ExtensionFor(VFMFMuxer muxer)
    {
        switch (muxer)
        {
        case Muxer_FFMMPEG_MKV: return L".mkv";
        case Muxer_FFMMPEG_MOV: return L".mov";
        case Muxer_FFMMPEG_TS: return L".ts";
        case Muxer_FFMMPEG_HLS: return NULL;
        default: return L".mp4";
        }
    }

    void EnsureParent(const wchar_t* path, bool pathIsFolder)
    {
        wchar_t directory[MAX_PATH];
        StringCchCopyW(directory, MAX_PATH, path);
        if (!pathIsFolder)
        {
            wchar_t* slash = wcsrchr(directory, L'\\');
            if (slash == NULL)
            {
                return;
            }

            *slash = L'\0';
        }

        SHCreateDirectoryExW(NULL, directory, NULL);
    }
}

CFileEncodeApp theApp;

BOOL CFileEncodeApp::InitInstance()
{
    CWinApp::InitInstance();
    CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

    CFileEncodeDlg dialog;
    m_pMainWnd = &dialog;
    dialog.DoModal();

    CoUninitialize();
    return FALSE;
}

CFileEncodeDlg::CFileEncodeDlg()
    : CDialogEx(IDD_FILE_ENCODE)
    , m_graph(NULL)
    , m_capture(NULL)
    , m_control(NULL)
    , m_events(NULL)
    , m_window(NULL)
    , m_running(false)
{
}

BOOL CFileEncodeDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();
    CheckDlgButton(IDC_FILE, BST_CHECKED);
    CheckDlgButton(IDC_AUDIO, BST_CHECKED);
    SetDlgItemInt(IDC_VBITRATE, 4000, FALSE);
    SetDlgItemInt(IDC_ABITRATE, 128, FALSE);

    CComboBox* encoders = (CComboBox*)GetDlgItem(IDC_ENCODER);
    AddEncoder(*encoders, L"H.264 software", VIDEO_ENCODER_MS_H264);
    AddEncoder(*encoders, L"H.264 Intel Quick Sync", VIDEO_ENCODER_QSV_H264);
    AddEncoder(*encoders, L"H.264 NVIDIA NVENC", VIDEO_ENCODER_NVENC_H264);
    AddEncoder(*encoders, L"H.264 AMD", VIDEO_ENCODER_AMD_H264);
    AddEncoder(*encoders, L"H.265 software", VIDEO_ENCODER_MS_H265);
    AddEncoder(*encoders, L"H.265 Intel Quick Sync", VIDEO_ENCODER_QSV_H265);
    AddEncoder(*encoders, L"H.265 NVIDIA NVENC", VIDEO_ENCODER_NVENC_H265);
    AddEncoder(*encoders, L"H.265 AMD", VIDEO_ENCODER_AMD_H265);
    encoders->SetCurSel(0);

    CComboBox* rates = (CComboBox*)GetDlgItem(IDC_RATE);
    AddRate(*rates, L"CBR", VFMFCommonRateControlMode_CBR);
    AddRate(*rates, L"Peak constrained VBR", VFMFCommonRateControlMode_PeakConstrainedVBR);
    AddRate(*rates, L"Quality", VFMFCommonRateControlMode_Quality);
    rates->SetCurSel(0);

    CComboBox* muxes = (CComboBox*)GetDlgItem(IDC_MUX);
    AddMux(*muxes, L"MP4", Muxer_MP4);
    AddMux(*muxes, L"MKV", Muxer_FFMMPEG_MKV);
    AddMux(*muxes, L"MOV", Muxer_FFMMPEG_MOV);
    AddMux(*muxes, L"MPEG-TS", Muxer_FFMMPEG_TS);
    AddMux(*muxes, L"HLS folder", Muxer_FFMMPEG_HLS);
    muxes->SetCurSel(0);

    FillDevices(CLSID_VideoInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE), false);
    FillDevices(CLSID_AudioInputDeviceCategory, *(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE), true);

    wchar_t output[MAX_PATH];
    DocumentsFile(output, MAX_PATH, L"encode.mp4");
    SetDlgItemText(IDC_OUTPUT, output);
    GetDlgItem(IDC_STOP)->EnableWindow(FALSE);
    UpdateSourceMode();
    return TRUE;
}

void CFileEncodeDlg::OnOK()
{
}

void CFileEncodeDlg::OnCancel()
{
    StopGraph();
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_CAMERA_DEVICE));
    ReleaseDevices(*(CComboBox*)GetDlgItem(IDC_AUDIO_DEVICE));
    CDialogEx::OnCancel();
}

void CFileEncodeDlg::UpdateSourceMode()
{
    const BOOL file = IsDlgButtonChecked(IDC_FILE) == BST_CHECKED;
    GetDlgItem(IDC_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_BROWSE_SOURCE)->EnableWindow(file);
    GetDlgItem(IDC_CAMERA_DEVICE)->EnableWindow(!file);
    GetDlgItem(IDC_AUDIO_DEVICE)->EnableWindow(!file);
}

void CFileEncodeDlg::UpdateOutputName()
{
    CComboBox* muxes = (CComboBox*)GetDlgItem(IDC_MUX);
    VFMFMuxer muxer = (VFMFMuxer)muxes->GetItemData(muxes->GetCurSel());
    const wchar_t* extension = ExtensionFor(muxer);
    wchar_t current[MAX_PATH];
    GetDlgItemText(IDC_OUTPUT, current, MAX_PATH);
    wchar_t directory[MAX_PATH];
    StringCchCopyW(directory, MAX_PATH, current);
    wchar_t* slash = wcsrchr(directory, L'\\');
    if (slash == NULL)
    {
        return;
    }

    if (extension == NULL)
    {
        *slash = L'\0';
        wchar_t folder[MAX_PATH];
        StringCchPrintfW(folder, MAX_PATH, L"%s\\hls", directory);
        SetDlgItemText(IDC_OUTPUT, folder);
        return;
    }

    wchar_t path[MAX_PATH];
    *slash = L'\0';
    StringCchPrintfW(path, MAX_PATH, L"%s\\encode%s", directory, extension);
    SetDlgItemText(IDC_OUTPUT, path);
}

void CFileEncodeDlg::OnSourceMode()
{
    UpdateSourceMode();
}

void CFileEncodeDlg::OnMuxChanged()
{
    UpdateOutputName();
}

void CFileEncodeDlg::OnBrowseSource()
{
    CFileDialog dialog(TRUE, NULL, NULL, OFN_FILEMUSTEXIST, L"Media files|*.mp4;*.mkv;*.mov;*.avi;*.wmv|All files|*.*||", this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_SOURCE, dialog.GetPathName());
    }
}

void CFileEncodeDlg::OnBrowseOutput()
{
    CComboBox* muxes = (CComboBox*)GetDlgItem(IDC_MUX);
    VFMFMuxer muxer = (VFMFMuxer)muxes->GetItemData(muxes->GetCurSel());
    if (muxer == Muxer_FFMMPEG_HLS)
    {
        CFolderPickerDialog dialog(NULL, 0, this);
        if (dialog.DoModal() == IDOK)
        {
            SetDlgItemText(IDC_OUTPUT, dialog.GetPathName());
        }

        return;
    }

    const wchar_t* extension = ExtensionFor(muxer);
    wchar_t filter[64];
    StringCchPrintfW(filter, 64, L"Output|*%s|All files|*.*||", extension);
    CFileDialog dialog(FALSE, extension + 1, L"encode", OFN_OVERWRITEPROMPT, filter, this);
    if (dialog.DoModal() == IDOK)
    {
        SetDlgItemText(IDC_OUTPUT, dialog.GetPathName());
    }
}

bool CFileEncodeDlg::Failed(HRESULT hr, const wchar_t* step)
{
    if (SUCCEEDED(hr))
    {
        return false;
    }

    ShowFailure(hr, step);
    StopGraph();
    return true;
}

void CFileEncodeDlg::OnStart()
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

    CComboBox* encoders = (CComboBox*)GetDlgItem(IDC_ENCODER);
    CComboBox* rates = (CComboBox*)GetDlgItem(IDC_RATE);
    CComboBox* muxes = (CComboBox*)GetDlgItem(IDC_MUX);
    VFMFVideoEncoder encoder = (VFMFVideoEncoder)encoders->GetItemData(encoders->GetCurSel());
    VFMFCommonRateControlMode rate = (VFMFCommonRateControlMode)rates->GetItemData(rates->GetCurSel());
    VFMFMuxer muxer = (VFMFMuxer)muxes->GetItemData(muxes->GetCurSel());
    const bool encodeAudio = IsDlgButtonChecked(IDC_AUDIO) == BST_CHECKED;
    const int videoBitrate = (int)GetDlgItemInt(IDC_VBITRATE, NULL, FALSE);
    const int audioBitrate = (int)GetDlgItemInt(IDC_ABITRATE, NULL, FALSE);
    EnsureParent(output, muxer == Muxer_FFMMPEG_HLS);

    IBaseFilter* source = NULL;
    IBaseFilter* audioSource = NULL;
    IBaseFilter* tee = NULL;
    IBaseFilter* renderer = NULL;
    IBaseFilter* rgb = NULL;
    IBaseFilter* mux = NULL;

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

    hr = AddByClsid(m_graph, kClsidRgb2Yuv, L"RGB to YUV", &rgb);
    if (Failed(hr, L"Add the color converter. Install the Encoding Filters Pack."))
    {
        goto cleanup;
    }

    hr = AddByClsid(m_graph, kClsidMFMux, L"MF Mux", &mux);
    if (Failed(hr, L"Add the MF muxer. Install the Encoding Filters Pack."))
    {
        goto cleanup;
    }

    IMFMuxConfig* config = NULL;
    hr = mux->QueryInterface(__uuidof(IMFMuxConfig), (void**)&config);
    if (Failed(hr, L"Get the muxer settings"))
    {
        goto cleanup;
    }

    VFMFVideoEncoderSettings video;
    ZeroMemory(&video, sizeof(video));
    VFMFVideoEncoderSettings::SetDefaults(&video);
    video.Encoder = encoder;
    video.AvgBitrate = videoBitrate;
    video.MaxBitrate = videoBitrate * 2;
    video.RateControlMode = rate;
    video.LowLatencyMode = fileSource ? FALSE : TRUE;
    hr = config->SetVideoEncoderSettings(video);
    if (SUCCEEDED(hr))
    {
        hr = config->SetMuxer(muxer);
    }

    if (SUCCEEDED(hr) && encodeAudio)
    {
        VFMFAudioEncoderSettings audio;
        ZeroMemory(&audio, sizeof(audio));
        VFMFAudioEncoderSettings::SetDefaults(&audio);
        audio.Bitrate = (UINT32)audioBitrate;
        hr = config->SetAudioEncoderSettings(audio);
    }

    if (SUCCEEDED(hr) && muxer == Muxer_FFMMPEG_HLS)
    {
        VFHLSSettings hls;
        ZeroMemory(&hls, sizeof(hls));
        hls.PlaylistType = fileSource ? HLS_PLAYLIST_VOD : HLS_PLAYLIST_LIVE;
        hls.SegmentDuration = 4;
        hls.NumSegments = fileSource ? 0 : 5;
        wcscpy_s(hls.PlaylistName, L"stream.m3u8");
        wcscpy_s(hls.PartName, L"segment");
        hr = config->SetHLSMuxerSettings(hls);
    }

    config->Release();
    if (Failed(hr, L"Configure the muxer"))
    {
        goto cleanup;
    }

    IFileSinkFilter* sink = NULL;
    hr = mux->QueryInterface(IID_IFileSinkFilter, (void**)&sink);
    if (SUCCEEDED(hr))
    {
        hr = sink->SetFileName(output, NULL);
        sink->Release();
    }

    if (Failed(hr, L"Set the output path"))
    {
        goto cleanup;
    }

    hr = renderer->QueryInterface(IID_IVideoWindow, (void**)&m_window);
    if (Failed(hr, L"Get the preview window"))
    {
        goto cleanup;
    }

    CWnd* preview = GetDlgItem(IDC_PREVIEW);
    RECT previewRect;
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

    hr = m_capture->RenderStream(NULL, &MEDIATYPE_Video, tee, rgb, mux);
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

        hr = m_capture->RenderStream(NULL, &MEDIATYPE_Audio, audioSource, NULL, mux);
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
    ReleaseFilter(&rgb);
    ReleaseFilter(&mux);
}

void CFileEncodeDlg::OnStop()
{
    SetDlgItemText(IDC_STATUS, L"Stopping...");
    StopGraph();
    SetDlgItemText(IDC_STATUS, L"Stopped.");
}

void CFileEncodeDlg::StopGraph()
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

LRESULT CFileEncodeDlg::OnGraphNotify(WPARAM, LPARAM)
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

BEGIN_MESSAGE_MAP(CFileEncodeDlg, CDialogEx)
    ON_BN_CLICKED(IDC_START, OnStart)
    ON_BN_CLICKED(IDC_STOP, OnStop)
    ON_BN_CLICKED(IDC_BROWSE_SOURCE, OnBrowseSource)
    ON_BN_CLICKED(IDC_BROWSE_OUTPUT, OnBrowseOutput)
    ON_BN_CLICKED(IDC_FILE, OnSourceMode)
    ON_BN_CLICKED(IDC_CAMERA, OnSourceMode)
    ON_CBN_SELCHANGE(IDC_MUX, OnMuxChanged)
    ON_MESSAGE(WM_GRAPH_NOTIFY, OnGraphNotify)
END_MESSAGE_MAP()
