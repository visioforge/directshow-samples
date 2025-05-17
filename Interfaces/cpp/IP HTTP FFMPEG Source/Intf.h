
#pragma once

// {1269EA71-85DB-40c6-AD87-35AD707C821C}
static const GUID CLSID_PushSourceFilter = 
{ 0x1269ea71, 0x85db, 0x40c6, { 0xad, 0x87, 0x35, 0xad, 0x70, 0x7c, 0x82, 0x1c } };

// {44E1B67B-4BCD-481b-ABF7-67E680E740E0}
static const GUID IID_IAVFileReader = 
{ 0x44e1b67b, 0x4bcd, 0x481b, { 0xab, 0xf7, 0x67, 0xe6, 0x80, 0xe7, 0x40, 0xe0 } };

enum AVFileReaderProtocol
{
	AVFileReaderProtocolUnknown,
	AVFileReaderProtocolHTTPSync,
	AVFileReaderProtocolHTTPAsync,
};

struct AVFileReaderParam
{
	bool								log_to_file;
	tchar*								log_file_path;
	tchar*								file_path;
	AVFileReaderProtocol				protocol;
	tchar*								user;
	tchar*								pass;
	int									buffer_frames;
	int									video_width;
	int									video_height;
	fLogCallback						log_callback;
};

// We define the interface the app can use to program us
MIDL_INTERFACE("44E1B67B-4BCD-481b-ABF7-67E680E740E0")
IAVFileReader : public IUnknown
{
public:
	virtual HRESULT STDMETHODCALLTYPE SetParam(AVFileReaderParam avfile_reader_param) = 0;
};

