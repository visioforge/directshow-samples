
#pragma once

EXTERN_GUID(CLSID_PushSourceFilter, 0x4ea6930a, 0x2c8a, 0x4ae6, 0xa5, 0x61, 0x56, 0xe4, 0xb5, 0x04, 0x44, 0x37);

// {473F35BF-259B-4e0d-B04D-C6CF061314BC}
EXTERN_GUID(IID_IAVFileReader,		0x473f35bf, 0x259b, 0x4e0d, 0xb0, 0x4d, 0xc6, 0xcf, 0x06, 0x13, 0x14, 0xbc);

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
MIDL_INTERFACE("473F35BF-259B-4e0d-B04D-C6CF061314BC")
IAVFileReader : public IUnknown
{
public:
	virtual HRESULT STDMETHODCALLTYPE SetParam(AVFileReaderParam avfile_reader_param) = 0;
};
