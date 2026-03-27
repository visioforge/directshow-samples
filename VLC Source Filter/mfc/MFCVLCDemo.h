//////////////////////////////////////////////////////
// MFCVLCDemo.h - Application header
//////////////////////////////////////////////////////
#pragma once

#ifndef __AFXWIN_H__
    #error "include 'pch.h' before including this file for PCH"
#endif

#include "resource.h"

class CMFCVLCDemoApp : public CWinApp
{
public:
    CMFCVLCDemoApp();
    virtual BOOL InitInstance();

    DECLARE_MESSAGE_MAP()
};
