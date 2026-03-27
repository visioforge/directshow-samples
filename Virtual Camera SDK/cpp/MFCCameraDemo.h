//////////////////////////////////////////////////////
// MFCCameraDemo.h - Application header
//////////////////////////////////////////////////////
#pragma once

#ifndef __AFXWIN_H__
    #error "include 'pch.h' before including this file for PCH"
#endif

#include "resource.h"

class CMFCCameraDemoApp : public CWinApp
{
public:
    CMFCCameraDemoApp();
    virtual BOOL InitInstance();

    DECLARE_MESSAGE_MAP()
};
