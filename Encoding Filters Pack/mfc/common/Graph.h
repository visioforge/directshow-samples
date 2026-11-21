#pragma once

#include <dshow.h>
#include <shlobj.h>
#include <strsafe.h>

// Empty string runs the trial. Replace it with the key from the purchase.
static const wchar_t* const kLicenseKey = L"";

static const GUID kIID_IVFRegister =
{ 0x59E82754, 0xB531, 0x4A8E, { 0xA9, 0x4D, 0x57, 0xC7, 0x5F, 0x01, 0xDA, 0x30 } };

struct IVFRegister : public IUnknown
{
    virtual HRESULT STDMETHODCALLTYPE SetLicenseKey(WCHAR* licenseKey) = 0;
};

inline void ApplyLicense(IBaseFilter* filter)
{
    if (filter == NULL || kLicenseKey[0] == L'\0')
    {
        return;
    }

    IVFRegister* registration = NULL;
    if (SUCCEEDED(filter->QueryInterface(kIID_IVFRegister, (void**)&registration)) && registration != NULL)
    {
        registration->SetLicenseKey(const_cast<WCHAR*>(kLicenseKey));
        registration->Release();
    }
}

inline void ShowFailure(HRESULT hr, const wchar_t* step)
{
    wchar_t text[320];
    StringCchPrintfW(text, 320, L"%s failed (0x%08X).", step, (unsigned)hr);
    AfxMessageBox(text);
}

inline HRESULT AddFilter(IGraphBuilder* graph, IBaseFilter* filter, const wchar_t* name)
{
    HRESULT hr = graph->AddFilter(filter, name);
    if (SUCCEEDED(hr))
    {
        ApplyLicense(filter);
    }

    return hr;
}

inline HRESULT AddByClsid(IGraphBuilder* graph, const GUID& clsid, const wchar_t* name, IBaseFilter** filter)
{
    *filter = NULL;
    IBaseFilter* created = NULL;
    HRESULT hr = CoCreateInstance(clsid, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&created);
    if (FAILED(hr))
    {
        return hr;
    }

    hr = AddFilter(graph, created, name);
    if (FAILED(hr))
    {
        created->Release();
        return hr;
    }

    *filter = created;
    return S_OK;
}

inline void ReleaseFilter(IBaseFilter** filter)
{
    if (filter != NULL && *filter != NULL)
    {
        (*filter)->Release();
        *filter = NULL;
    }
}

inline void ReleaseDevices(CComboBox& combo)
{
    for (int i = 0; i < combo.GetCount(); ++i)
    {
        IMoniker* moniker = (IMoniker*)combo.GetItemDataPtr(i);
        if (moniker != NULL && moniker != (IMoniker*)(INT_PTR)-1)
        {
            moniker->Release();
            combo.SetItemDataPtr(i, NULL);
        }
    }
}

inline void FillDevices(const GUID& category, CComboBox& combo, bool includeNone)
{
    ReleaseDevices(combo);
    combo.ResetContent();
    if (includeNone)
    {
        int none = combo.AddString(L"(none)");
        combo.SetItemDataPtr(none, NULL);
    }

    ICreateDevEnum* enumeratorFactory = NULL;
    HRESULT hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER, IID_ICreateDevEnum, (void**)&enumeratorFactory);
    if (FAILED(hr))
    {
        return;
    }

    IEnumMoniker* enumerator = NULL;
    hr = enumeratorFactory->CreateClassEnumerator(category, &enumerator, 0);
    enumeratorFactory->Release();
    if (hr != S_OK || enumerator == NULL)
    {
        if (combo.GetCount() > 0)
        {
            combo.SetCurSel(0);
        }

        return;
    }

    IMoniker* moniker = NULL;
    while (enumerator->Next(1, &moniker, NULL) == S_OK)
    {
        wchar_t name[256];
        StringCchCopyW(name, 256, L"Device");
        IPropertyBag* bag = NULL;
        if (SUCCEEDED(moniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void**)&bag)))
        {
            VARIANT value;
            VariantInit(&value);
            if (SUCCEEDED(bag->Read(L"FriendlyName", &value, NULL)) && value.vt == VT_BSTR && value.bstrVal != NULL)
            {
                StringCchCopyW(name, 256, value.bstrVal);
            }

            VariantClear(&value);
            bag->Release();
        }

        int index = combo.AddString(name);
        combo.SetItemDataPtr(index, moniker);
    }

    enumerator->Release();
    if (combo.GetCount() > 0)
    {
        combo.SetCurSel(includeNone && combo.GetCount() > 1 ? 1 : 0);
    }
}

inline HRESULT BindDevice(IMoniker* moniker, IBaseFilter** filter)
{
    *filter = NULL;
    if (moniker == NULL)
    {
        return E_POINTER;
    }

    return moniker->BindToObject(NULL, NULL, IID_IBaseFilter, (void**)filter);
}

inline void DocumentsFile(wchar_t* path, size_t count, const wchar_t* fileName)
{
    wchar_t documents[MAX_PATH];
    documents[0] = L'\0';
    SHGetFolderPathW(NULL, CSIDL_PERSONAL, NULL, 0, documents);
    StringCchPrintfW(path, count, L"%s\\VisioForge\\%s", documents, fileName);
}
