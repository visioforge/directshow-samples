//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "Unit1.h"

#include <uuids.h>
#include <mfapi.h>
#include <evr.h>

#include <stdio.h>
#include <string.h>
#include <iostream>
#include <sstream>
#include <iomanip>

#include "ivlcsrc.h"

//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1* Form1;
//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner) : TForm(Owner) {}
//---------------------------------------------------------------------------

void TForm1::AddGraph()
{
    HRESULT hr = CoInitialize(NULL); // Ensure COM library is initialized

    // Create the Filter Graph Manager
    hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER,
        IID_IFilterGraph2, (void**)&filterGraph);
    if (FAILED(hr)) {
        // Handle error
    }

    // Create the Capture Graph Builder
    hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL,
        CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void**)&captureGraph);
    if (FAILED(hr)) {
        // Handle error
    }

    // Set the filter graph to the capture graph builder
    hr = captureGraph->SetFiltergraph(filterGraph);
    if (FAILED(hr)) {
        // Handle error
    }

    // Query for the Media Control for playback control
    hr = filterGraph->QueryInterface(IID_IMediaControl, (void**)&mediaControl);
    if (FAILED(hr)) {
        // Handle error
    }

    // Query for the Media Seeking interface
    hr = filterGraph->QueryInterface(IID_IMediaSeeking, (void**)&mediaSeeking);
    if (FAILED(hr)) {
        // Handle error
    }

    // Query for the Media Event interface
    hr = filterGraph->QueryInterface(IID_IMediaEventEx, (void**)&mediaEventEx);
    if (FAILED(hr)) {
        // Handle error
    }

    // Set the event notification window
    hr = mediaEventEx->SetNotifyWindow((OAHWND)Handle, WM_GRAPHNOTIFY, NULL);
    if (FAILED(hr)) {
        // Handle error
    }
}

void TForm1::AddSource()
{
    HRESULT hr = CoInitialize(NULL); // Ensure COM library is initialized

    // Add the source filter to the graph
	hr = CoCreateInstance(CLSID_VlcSource, NULL, CLSCTX_INPROC_SERVER,
        IID_IBaseFilter, (void**)&sourceFilter);
    if (FAILED(hr)) {
        // Handle error
    }
	hr = filterGraph->AddFilter(sourceFilter, L"VLC Source");
    if (FAILED(hr)) {
        // Handle error
    }

    // Load media
    IFileSourceFilter* pFileSource = NULL;
    hr = sourceFilter->QueryInterface(
        IID_IFileSourceFilter, (void**)&pFileSource);
    if (SUCCEEDED(hr) && pFileSource) {
        hr = pFileSource->Load(StringToOleStr(edFilename->Text), NULL);
        pFileSource->Release(); // Release the interface after use
        if (FAILED(hr)) {
            // Handle error
        }
    }
}

void TForm1::AddVideoRenderer()
{
    CLSID CLSID_EVR = { 0xFA10746C, 0x9B63, 0x4B6C,
        { 0xBC, 0x49, 0xFC, 0x30, 0x0E, 0xA5, 0xF2, 0x56 } };
    videoRenderer = nullptr;
    CoCreateInstance(CLSID_EVR, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter,
        (void**)&videoRenderer);
    filterGraph->AddFilter(videoRenderer, L"EVR");

    IEVRFilterConfig* pConfig = nullptr;
    videoRenderer->QueryInterface(IID_IEVRFilterConfig, (void**)&pConfig);
    if (pConfig != nullptr) {
        pConfig->SetNumberOfStreams(1);
        pConfig->Release();
    } else {
        throw std::exception("Unable to query IEVRFilterConfig interface.");
    }

    IMFGetService* getService = nullptr;
    videoRenderer->QueryInterface(IID_IMFGetService, (void**)&getService);
    if (getService != nullptr) {
        IMFVideoDisplayControl* dsMFVideoDisplayControl = nullptr;
        getService->GetService(MR_VIDEO_RENDER_SERVICE,
            IID_IMFVideoDisplayControl, (void**)&dsMFVideoDisplayControl);
        if (dsMFVideoDisplayControl != nullptr) {
            dsMFVideoDisplayControl->SetVideoWindow(
                pnScreen->Handle); // Assuming method GetHandle
            MFVideoNormalizedRect rectSrc = { 0, 0, 1, 1 };
            RECT rectDest = { 0, 0, pnScreen->Width,
                pnScreen->Height }; // Assuming methods GetWidth/GetHeight

            dsMFVideoDisplayControl->SetVideoPosition(&rectSrc, &rectDest);
            dsMFVideoDisplayControl->Release();
        }
        getService->Release();
    } else {
        throw std::exception("Unable to query IMFGetService interface.");
    }

    HRESULT hr = captureGraph->RenderStream(
        nullptr, &MEDIATYPE_Video, sourceFilter, nullptr, videoRenderer);
    if (FAILED(hr)) {
        throw std::exception("Failed to render video stream.");
    }

    hr = captureGraph->RenderStream(
        nullptr, &MEDIATYPE_Audio, sourceFilter, nullptr, nullptr);
    // Typically handle or log the error here too
}

void __fastcall TForm1::btStartClick(TObject* Sender)
{
    AddGraph();

    AddSource();

    AddVideoRenderer();

    mediaControl->Run();

    tmProgress->Enabled = true;
}

REFERENCE_TIME TForm1::GetDuration()
{
    if (mediaSeeking != nullptr) {
        REFERENCE_TIME duration = 0;
        HRESULT hr = mediaSeeking->GetDuration(&duration);
        if (SUCCEEDED(hr)) {
            // Convert REFERENCE_TIME (100-nanosecond units) to milliseconds
            return duration / 10000;
        }
    }
    return 0; // Return 0 if failed or mediaSeeking is nullptr
}

REFERENCE_TIME TForm1::GetPosition()
{
    HRESULT hr = mediaSeeking->SetTimeFormat(&TIME_FORMAT_MEDIA_TIME);
    if (FAILED(hr)) {
        throw std::exception("Unable to set seeking time format to MediaTime");
    }

    REFERENCE_TIME position = 0;
    hr = mediaSeeking->GetCurrentPosition(&position);
    if (FAILED(hr)) {
        // Log error using your preferred logging method
        return 0;
    }

    return position /
           10000; // Convert from 100-nanosecond units to milliseconds
}

bool TForm1::PositionSet(long long milliseconds)
{
    static bool seekingFlag = false;
    if (seekingFlag) {
        return true; // Early return if already seeking
    }

    bool result = false;
    seekingFlag = true; // Set flag to indicate seeking operation is in progress

    try {
        REFERENCE_TIME position =
            milliseconds *
            10000; // Convert milliseconds to 100-nanosecond units
        REFERENCE_TIME stopPosition = 0; // No specific stop position needed

        HRESULT hr = mediaSeeking->SetTimeFormat(&TIME_FORMAT_MEDIA_TIME);
        if (FAILED(hr)) {
            std::cerr << "Unable to set seeking time format to MediaTime"
                      << std::endl;
        }

        hr = mediaSeeking->SetPositions(&position,
            AM_SEEKING_AbsolutePositioning, &stopPosition,
            AM_SEEKING_NoPositioning);
        if (FAILED(hr)) {
            std::cerr << "Unable to set current position" << std::endl;
        } else {
            result = true;
        }

        // Handle specific formats that may need a delay
        //auto filename = edFilename->Text;
        //if (StrStrW(&filename, L".wmv")) {
        //	Sleep(300);
        //	}

    } catch (...) {
        result = false;
    }

    seekingFlag = false;

    return result;
}

//---------------------------------------------------------------------------

void __fastcall TForm1::btResumeClick(TObject* Sender)
{
    mediaControl->Run();
}
//---------------------------------------------------------------------------

void __fastcall TForm1::btPauseClick(TObject* Sender)
{
    mediaControl->Pause();
}
//---------------------------------------------------------------------------

std::string formatTime(long long total_seconds)
{
    // Extract hours, minutes and seconds from total seconds
    int hours = total_seconds / 3600;
    int minutes = (total_seconds % 3600) / 60;
    int seconds = total_seconds % 60;

    // Use ostringstream to format the string with leading zeros if necessary
    std::ostringstream oss;
    oss << std::setw(2) << std::setfill('0') << hours << ":" << std::setw(2)
        << std::setfill('0') << minutes << ":" << std::setw(2)
        << std::setfill('0') << seconds;

    return oss.str();
}

void __fastcall TForm1::tmProgressTimer(TObject* Sender)
{
    tmProgress->Tag = 1;

    auto duration = (int)(GetDuration() / 1000);
    auto position = (int)(GetPosition() / 1000);
    tbTimeline->Max = duration;

    int value = position;
    if ((value > 0) && (value < tbTimeline->Max)) {
        tbTimeline->Position = value;
    }

    auto str = formatTime(position) + std::string("/") + formatTime(duration);
    lbTime->Caption = str.c_str();

    tmProgress->Tag = 0;
}
//---------------------------------------------------------------------------

void __fastcall TForm1::btStopClick(TObject* Sender)
{
    // Assuming tmProgress is a timer or similar object, stop it.
    tmProgress->Enabled = false;

    // Stop previewing data
    if (mediaControl != nullptr) {
        mediaControl->StopWhenReady();
        mediaControl->Stop();
    }

    // Stop receiving events
    if (mediaEventEx != nullptr) {
        mediaEventEx->SetNotifyWindow(NULL, WM_GRAPHNOTIFY, NULL);
    }

    // Relinquish ownership of the video window
    if (videoWindow != nullptr) {
        videoWindow->put_Visible(
            FALSE); // Assuming OAFALSE is defined as a conversion of OABool.False
        videoWindow->put_Owner(
            NULL); // Important to prevent assert failures in the video renderer
    }

    // Remove all filters from the filter graph
    //FilterGraphTools::RemoveAllFilters(filterGraph);

    // Release DirectShow interfaces
    if (mediaControl != nullptr) {
        mediaControl->Release();
        mediaControl = nullptr;
    }

    if (mediaEventEx != nullptr) {
        mediaEventEx->Release();
        mediaEventEx = nullptr;
    }

    if (videoWindow != nullptr) {
        videoWindow->Release();
        videoWindow = nullptr;
    }

    if (filterGraph != nullptr) {
        filterGraph->Release();
        filterGraph = nullptr;
    }

    if (captureGraph != nullptr) {
        captureGraph->Release();
        captureGraph = nullptr;
    }

    if (sourceFilter != nullptr) {
        sourceFilter->Release();
        sourceFilter = nullptr;
    }

    if (videoRenderer != nullptr) {
        videoRenderer->Release();
        videoRenderer = nullptr;
    }
}
//---------------------------------------------------------------------------

void __fastcall TForm1::tbTimelineChange(TObject* Sender)
{
    if (tmProgress->Tag == 0) {
        PositionSet(tbTimeline->Position * 1000);
    }
}
//---------------------------------------------------------------------------

void __fastcall TForm1::btOpenFileClick(TObject* Sender)
{
    if (FileOpenDialog1->Execute(Handle)) {
        edFilename->Text = FileOpenDialog1->FileName;
    }
}
//---------------------------------------------------------------------------

