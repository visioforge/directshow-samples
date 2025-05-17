
#ifndef YKSOURCEFILTERDEFINE
#define YKSOURCEFILTERDEFINE

// {44E1B67B-4BCD-481b-ABF7-67E680E740E0}
static const GUID IID_IAVFileReader = 
{ 0x44e1b67b, 0x4bcd, 0x481b, { 0xab, 0xf7, 0x67, 0xe6, 0x80, 0xe7, 0x40, 0xe0 } };

// {9CF575AF-D47F-4472-9686-184EC5B29EA8}
static const GUID IID_IFilterProperty = 
{ 0x9cf575af, 0xd47f, 0x4472, { 0x96, 0x86, 0x18, 0x4e, 0xc5, 0xb2, 0x9e, 0xa8 } };

// {A58231A1-AD59-4c82-9F2E-D681D541A3E7}
static const GUID CLSID_PropertyPage = 
{ 0xa58231a1, 0xad59, 0x4c82, { 0x9f, 0x2e, 0xd6, 0x81, 0xd5, 0x41, 0xa3, 0xe7 } };

enum AVFileReaderProtocol
{
	AVFileReaderProtocolUnknown,
	AVFileReaderProtocolHTTPSync,
	AVFileReaderProtocolHTTPAsync,
};

struct AVFileReaderParam
{
	BOOL								log_to_file;
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

#endif