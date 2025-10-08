unit encryptor_intf;

interface

const
  IID_IVFCryptoConfig: TGUID = '{BAA5BD1E-3B30-425e-AB3B-CC20764AC253}';

type
  IVFCryptoConfig = interface(IUnknown)
    ['{BAA5BD1E-3B30-425e-AB3B-CC20764AC253}']
    function put_Provider(pProvider: IUnknown): HResult; stdcall;
    function get_Provider(out pProvider: IUnknown): HResult; stdcall;
    function put_Password(pBuffer: PByte; lSize: Integer): HResult; stdcall;
    function HavePassword(): HResult; stdcall;
  end;

implementation

end.
