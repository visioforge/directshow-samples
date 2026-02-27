// ScreenCaptureMFCSample.cpp - MFC Sample Application for ScreenCaptureFilterDD
//
// Demonstrates how to use the VisioForge Screen Capture DD DirectShow filter
// in an MFC dialog-based application with video preview.
//
// Prerequisites:
//   - Register ScreenCaptureFilterDD.ax: regsvr32 ScreenCaptureFilterDD.ax
//   - Windows SDK with DirectShow headers
//   - Link: strmiids.lib ole32.lib oleaut32.lib

#include <afxwin.h>
#include <afxcmn.h>
#include <afxdialogex.h>
#include <gdiplus.h>
#pragma comment(lib, "gdiplus.lib")
#include "ScreenCaptureMFCSample.h"

// ---- Resource IDs (normally in resource.h, inlined here for simplicity) ----
#ifndef IDD_SCREENCAPTURE_DIALOG
#define IDD_SCREENCAPTURE_DIALOG  100
#define IDC_COMBO_MODE            1001
#define IDC_EDIT_FPS              1002
#define IDC_EDIT_LEFT             1003
#define IDC_EDIT_TOP              1004
#define IDC_EDIT_RIGHT            1005
#define IDC_EDIT_BOTTOM           1006
#define IDC_CHECK_MOUSE           1007
#define IDC_BTN_START             1008
#define IDC_BTN_STOP              1009
#define IDC_LIST_LOG              1010
#define IDC_VIDEO_AREA            1011
#endif

// ---- CScreenCaptureDlg implementation ----

BEGIN_MESSAGE_MAP(CScreenCaptureDlg, CDialogEx)
    ON_BN_CLICKED(IDC_BTN_START, &CScreenCaptureDlg::OnBnClickedStart)
    ON_BN_CLICKED(IDC_BTN_STOP, &CScreenCaptureDlg::OnBnClickedStop)
    ON_BN_CLICKED(IDC_BTN_PICK, &CScreenCaptureDlg::OnBnClickedPick)
    ON_WM_LBUTTONUP()
    ON_WM_DESTROY()
END_MESSAGE_MAP()

CScreenCaptureDlg::CScreenCaptureDlg(CWnd* pParent)
    : CDialogEx(IDD_SCREENCAPTURE_DIALOG, pParent)
    , m_pGraph(NULL)
    , m_pControl(NULL)
    , m_pVideoWindow(NULL)
    , m_pScreenCapture(NULL)
    , m_pVideoRenderer(NULL)
    , m_bGraphRunning(FALSE)
    , m_hTargetWnd(NULL)
    , m_bPicking(FALSE)
{
}

void CScreenCaptureDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialogEx::DoDataExchange(pDX);
    DDX_Control(pDX, IDC_COMBO_MODE, m_comboMode);
    DDX_Control(pDX, IDC_EDIT_FPS, m_editFPS);
    DDX_Control(pDX, IDC_EDIT_LEFT, m_editLeft);
    DDX_Control(pDX, IDC_EDIT_TOP, m_editTop);
    DDX_Control(pDX, IDC_EDIT_RIGHT, m_editRight);
    DDX_Control(pDX, IDC_EDIT_BOTTOM, m_editBottom);
    DDX_Control(pDX, IDC_CHECK_MOUSE, m_checkMouse);
    DDX_Control(pDX, IDC_LIST_LOG, m_listLog);
    DDX_Control(pDX, IDC_VIDEO_AREA, m_videoArea);
}

BOOL CScreenCaptureDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();

    SetWindowText(_T("ScreenCaptureFilterDD - MFC Sample"));

    // Populate capture mode combo
    m_comboMode.AddString(_T("Screen (GDI)"));
    m_comboMode.AddString(_T("Picture"));
    m_comboMode.AddString(_T("Color"));
    m_comboMode.AddString(_T("Window"));
    m_comboMode.AddString(_T("Screen (DXGI)"));
    m_comboMode.SetCurSel(0);

    // Default values
    m_editFPS.SetWindowText(_T("15"));
    m_editLeft.SetWindowText(_T("0"));
    m_editTop.SetWindowText(_T("0"));
    m_editRight.SetWindowText(_T("1920"));
    m_editBottom.SetWindowText(_T("1080"));
    m_checkMouse.SetCheck(BST_CHECKED);

    Log(_T("Ready. Click Start to begin screen capture."));

    return TRUE;
}

void CScreenCaptureDlg::Log(const CString& msg)
{
    m_listLog.AddString(msg);
    m_listLog.SetTopIndex(m_listLog.GetCount() - 1);
}

void CScreenCaptureDlg::BuildGraph()
{
    HRESULT hr;

    // Create the filter graph manager
    hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER,
        IID_IGraphBuilder, (void**)&m_pGraph);
    if (FAILED(hr))
    {
        Log(_T("ERROR: Cannot create FilterGraph."));
        return;
    }

    // Create the screen capture filter
    hr = CoCreateInstance(CLSID_VFScreenCapture_4, NULL, CLSCTX_INPROC_SERVER,
        IID_IBaseFilter, (void**)&m_pScreenCapture);
    if (FAILED(hr))
    {
        Log(_T("ERROR: Cannot create ScreenCaptureFilterDD. Is it registered?"));
        TearDownGraph();
        return;
    }

    // Configure capture settings via IVFScreenCapture
    IVFScreenCapture* pCapture = NULL;
    hr = m_pScreenCapture->QueryInterface(IID_IVFScreenCapture, (void**)&pCapture);
    if (SUCCEEDED(hr))
    {
        // FPS
        CString strFPS;
        m_editFPS.GetWindowText(strFPS);
        double fps = _ttof(strFPS);
        if (fps < 0.4) fps = 0.4;
        if (fps > 30.0) fps = 30.0;
        hr = pCapture->set_fps(fps);
        if (SUCCEEDED(hr))
        {
            CString msg;
            msg.Format(_T("FPS set to %.1f"), fps);
            Log(msg);
        }

        // Capture rectangle
        VFRect rect;
        CString str;
        m_editLeft.GetWindowText(str);   rect.left = _ttoi(str);
        m_editTop.GetWindowText(str);    rect.top = _ttoi(str);
        m_editRight.GetWindowText(str);  rect.right = _ttoi(str);
        m_editBottom.GetWindowText(str); rect.bottom = _ttoi(str);
        hr = pCapture->set_rect(rect);
        if (SUCCEEDED(hr))
        {
            CString msg;
            msg.Format(_T("Capture rect: (%d,%d)-(%d,%d)"), rect.left, rect.top, rect.right, rect.bottom);
            Log(msg);
        }

        // Mouse cursor
        bool drawMouse = (m_checkMouse.GetCheck() == BST_CHECKED);
        pCapture->set_mouse(drawMouse);
        Log(drawMouse ? _T("Mouse cursor: enabled") : _T("Mouse cursor: disabled"));

        // Screen index
        pCapture->set_screen_index(0);

        pCapture->Release();
    }

    // Set capture mode via IVFScreenCapture2
    IVFScreenCapture2* pCapture2 = NULL;
    hr = m_pScreenCapture->QueryInterface(IID_IVFScreenCapture2, (void**)&pCapture2);
    if (SUCCEEDED(hr))
    {
        int modeIndex = m_comboMode.GetCurSel();
        VFScreenCaptureMode mode = (VFScreenCaptureMode)modeIndex;
        pCapture2->set_mode(mode);

        // For window mode, use the picked external window
        if (mode == scm_window)
        {
            if (!m_hTargetWnd || !::IsWindow(m_hTargetWnd))
            {
                Log(_T("ERROR: No target window selected. Use 'Pick Window' first."));
                pCapture2->Release();
                TearDownGraph();
                return;
            }
            pCapture2->set_window_handle(m_hTargetWnd);

            TCHAR title[256] = {};
            ::GetWindowText(m_hTargetWnd, title, 256);
            CString msg;
            msg.Format(_T("Window mode: capturing '%s'"), title);
            Log(msg);
        }

        // For picture mode, decode image.jpg to raw BGRA32 pixels
        if (mode == scm_picture)
        {
            // Look for image.jpg: exe dir, then two levels up (project dir)
            TCHAR exePath[MAX_PATH] = {};
            ::GetModuleFileName(NULL, exePath, MAX_PATH);
            CString exeDir(exePath);
            exeDir = exeDir.Left(exeDir.ReverseFind(_T('\\')));

            CString imagePath = exeDir + _T("\\image.jpg");
            if (::GetFileAttributes(imagePath) == INVALID_FILE_ATTRIBUTES)
                imagePath = exeDir + _T("\\..\\..\\image.jpg");

            // Use GDI+ to decode and resize image to capture rect dimensions
            Gdiplus::GdiplusStartupInput gdiplusInput;
            ULONG_PTR gdiplusToken = 0;
            Gdiplus::GdiplusStartup(&gdiplusToken, &gdiplusInput, NULL);

            Gdiplus::Bitmap* pSrcBitmap = new Gdiplus::Bitmap(imagePath);
            if (pSrcBitmap->GetLastStatus() == Gdiplus::Ok)
            {
                // Target size from capture rect edit boxes
                CString str;
                m_editRight.GetWindowText(str);  int targetW = _ttoi(str);
                m_editBottom.GetWindowText(str); int targetH = _ttoi(str);
                m_editLeft.GetWindowText(str);   targetW -= _ttoi(str);
                m_editTop.GetWindowText(str);    targetH -= _ttoi(str);
                if (targetW <= 0) targetW = (int)pSrcBitmap->GetWidth();
                if (targetH <= 0) targetH = (int)pSrcBitmap->GetHeight();

                // Create target bitmap and draw source scaled into it
                Gdiplus::Bitmap targetBitmap(targetW, targetH, PixelFormat32bppARGB);
                {
                    Gdiplus::Graphics g(&targetBitmap);
                    g.SetInterpolationMode(Gdiplus::InterpolationModeHighQualityBicubic);
                    g.DrawImage(pSrcBitmap, 0, 0, targetW, targetH);
                }

                // Lock bits as BGRA32
                Gdiplus::BitmapData bitmapData = {};
                Gdiplus::Rect lockRect(0, 0, targetW, targetH);
                targetBitmap.LockBits(&lockRect, Gdiplus::ImageLockModeRead,
                    PixelFormat32bppARGB, &bitmapData);

                DWORD bufSize = (DWORD)((size_t)targetW * targetH * 4);
                HGLOBAL hMem = ::GlobalAlloc(GMEM_MOVEABLE, bufSize);
                if (hMem)
                {
                    LPBYTE pDest = (LPBYTE)::GlobalLock(hMem);
                    for (int y = 0; y < targetH; y++)
                    {
                        memcpy(pDest + (size_t)y * targetW * 4,
                            (LPBYTE)bitmapData.Scan0 + (size_t)y * bitmapData.Stride,
                            (size_t)targetW * 4);
                    }
                    ::GlobalUnlock(hMem);

                    IStream* pStream = NULL;
                    if (SUCCEEDED(::CreateStreamOnHGlobal(hMem, TRUE, &pStream)))
                    {
                        pCapture2->set_stream(pStream, (__int64)bufSize);
                        pCapture2->refresh_pic();
                        pStream->Release();
                        CString msg;
                        msg.Format(_T("Picture mode: loaded '%s' (%dx%d -> %dx%d BGRA)"),
                            (LPCTSTR)imagePath,
                            pSrcBitmap->GetWidth(), pSrcBitmap->GetHeight(),
                            targetW, targetH);
                        Log(msg);
                    }
                    else
                    {
                        ::GlobalFree(hMem);
                        Log(_T("ERROR: Failed to create stream for image."));
                    }
                }

                targetBitmap.UnlockBits(&bitmapData);
            }
            else
            {
                Log(_T("ERROR: image.jpg not found or cannot be decoded."));
            }

            delete pSrcBitmap;
            Gdiplus::GdiplusShutdown(gdiplusToken);
        }

        CString msg;
        msg.Format(_T("Capture mode: %d"), modeIndex);
        Log(msg);

        pCapture2->Release();
    }

    // Check DXGI availability
    IVFScreenCaptureDD* pCaptureDD = NULL;
    hr = m_pScreenCapture->QueryInterface(IID_IVFScreenCaptureDD, (void**)&pCaptureDD);
    if (SUCCEEDED(hr))
    {
        hr = pCaptureDD->dd_check(0);
        Log(hr == S_OK ? _T("DXGI Desktop Duplication: available")
                       : _T("DXGI Desktop Duplication: not available"));
        pCaptureDD->Release();
    }

    // Add capture filter to graph
    hr = m_pGraph->AddFilter(m_pScreenCapture, L"Screen Capture DD");
    if (FAILED(hr))
    {
        Log(_T("ERROR: Cannot add capture filter to graph."));
        TearDownGraph();
        return;
    }

    // Create and add Video Renderer
    hr = CoCreateInstance(CLSID_VideoRenderer, NULL, CLSCTX_INPROC_SERVER,
        IID_IBaseFilter, (void**)&m_pVideoRenderer);
    if (FAILED(hr))
    {
        Log(_T("ERROR: Cannot create Video Renderer."));
        TearDownGraph();
        return;
    }

    hr = m_pGraph->AddFilter(m_pVideoRenderer, L"Video Renderer");
    if (FAILED(hr))
    {
        Log(_T("ERROR: Cannot add renderer to graph."));
        TearDownGraph();
        return;
    }

    // Connect capture output to renderer input
    IPin* pOutPin = NULL;
    IPin* pInPin = NULL;
    IEnumPins* pEnum = NULL;
    ULONG fetched;

    m_pScreenCapture->EnumPins(&pEnum);
    if (pEnum) { pEnum->Next(1, &pOutPin, &fetched); pEnum->Release(); }

    m_pVideoRenderer->EnumPins(&pEnum);
    if (pEnum) { pEnum->Next(1, &pInPin, &fetched); pEnum->Release(); }

    if (pOutPin && pInPin)
    {
        hr = m_pGraph->Connect(pOutPin, pInPin);
        if (FAILED(hr))
        {
            Log(_T("ERROR: Cannot connect filters. Check capture settings."));
            if (pOutPin) pOutPin->Release();
            if (pInPin) pInPin->Release();
            TearDownGraph();
            return;
        }
    }
    if (pOutPin) pOutPin->Release();
    if (pInPin) pInPin->Release();

    // Set up video window inside our dialog's video area
    hr = m_pGraph->QueryInterface(IID_IVideoWindow, (void**)&m_pVideoWindow);
    if (SUCCEEDED(hr))
    {
        CRect rc;
        m_videoArea.GetClientRect(&rc);
        m_videoArea.ClientToScreen(&rc);
        ScreenToClient(&rc);

        m_pVideoWindow->put_Owner((OAHWND)m_videoArea.GetSafeHwnd());
        m_pVideoWindow->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS);
        m_pVideoWindow->SetWindowPosition(0, 0, rc.Width(), rc.Height());
        m_pVideoWindow->put_Visible(OATRUE);
    }

    // Get media control
    m_pGraph->QueryInterface(IID_IMediaControl, (void**)&m_pControl);

    Log(_T("Graph built successfully."));
}

void CScreenCaptureDlg::TearDownGraph()
{
    if (m_pControl)
    {
        m_pControl->Stop();
        m_pControl->Release();
        m_pControl = NULL;
    }

    if (m_pVideoWindow)
    {
        m_pVideoWindow->put_Visible(OAFALSE);
        m_pVideoWindow->put_Owner(NULL);
        m_pVideoWindow->Release();
        m_pVideoWindow = NULL;
    }

    if (m_pVideoRenderer)
    {
        m_pVideoRenderer->Release();
        m_pVideoRenderer = NULL;
    }

    if (m_pScreenCapture)
    {
        m_pScreenCapture->Release();
        m_pScreenCapture = NULL;
    }

    if (m_pGraph)
    {
        m_pGraph->Release();
        m_pGraph = NULL;
    }

    m_bGraphRunning = FALSE;
}

void CScreenCaptureDlg::OnBnClickedStart()
{
    if (m_bGraphRunning)
    {
        Log(_T("Already running."));
        return;
    }

    BuildGraph();

    if (m_pControl)
    {
        HRESULT hr = m_pControl->Run();
        if (SUCCEEDED(hr))
        {
            m_bGraphRunning = TRUE;
            Log(_T("Capture started."));
        }
        else
        {
            CString msg;
            msg.Format(_T("ERROR: Run failed (0x%08X)"), hr);
            Log(msg);
            TearDownGraph();
        }
    }
}

void CScreenCaptureDlg::OnBnClickedStop()
{
    if (!m_bGraphRunning)
    {
        Log(_T("Not running."));
        return;
    }

    TearDownGraph();
    Log(_T("Capture stopped."));
}

void CScreenCaptureDlg::OnBnClickedPick()
{
    SetCapture();
    ::SetCursor(::LoadCursor(NULL, IDC_CROSS));
    m_bPicking = TRUE;
    Log(_T("Click on a window to select it for capture..."));
}

void CScreenCaptureDlg::OnLButtonUp(UINT nFlags, CPoint point)
{
    if (m_bPicking)
    {
        ReleaseCapture();
        m_bPicking = FALSE;

        CPoint screenPt = point;
        ClientToScreen(&screenPt);
        HWND hwnd = ::WindowFromPoint(screenPt);

        // Get top-level parent window
        HWND hwndTop = ::GetAncestor(hwnd, GA_ROOT);

        if (hwndTop == GetSafeHwnd())
        {
            Log(_T("Cannot capture own window (would deadlock). Pick a different window."));
            m_hTargetWnd = NULL;
            return;
        }

        if (hwndTop && ::IsWindow(hwndTop))
        {
            m_hTargetWnd = hwndTop;
            TCHAR title[256] = {};
            ::GetWindowText(hwndTop, title, 256);
            CString msg;
            msg.Format(_T("Selected: '%s' (0x%p)"), title, hwndTop);
            Log(msg);
        }
        else
        {
            Log(_T("No valid window found at that point."));
        }
        return;
    }
    CDialogEx::OnLButtonUp(nFlags, point);
}

void CScreenCaptureDlg::OnDestroy()
{
    TearDownGraph();
    CDialogEx::OnDestroy();
}

// ---- Application class ----
class CScreenCaptureApp : public CWinApp
{
public:
    virtual BOOL InitInstance()
    {
        CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

        CScreenCaptureDlg dlg;
        m_pMainWnd = &dlg;
        dlg.DoModal();

        CoUninitialize();
        return FALSE;
    }
};

CScreenCaptureApp theApp;
