unit VirtualCameraIntf;

interface

const
  IID_IVFVirtualCameraSink
    : TGUID = '{A96631D2-4AC9-4F09-9F34-FF8229087DEB}';

type
  /// <summary>
  /// Virtual camera sink interface.
  ///
  /// </summary>
  IVFVirtualCameraSink = interface(IUnknown)
    ['{A96631D2-4AC9-4F09-9F34-FF8229087DEB}']
    function set_license(license: PAnsiChar): HRESULT; stdcall;
  end;

implementation

/// <summary>
/// Virtual audio card sink CLSID.
/// </summary>
const
  CLSID_VFVirtualAudioCardSink
    : TGUID = '{1A2673B0-553E-4027-AECC-839405468950}';

  /// <summary>
  /// Virtual audio card source CLSID.
  /// </summary>
const
  CLSID_VFVirtualAudioCardSource
    : TGUID = '{B5A463DF-4016-4C34-AA4F-48EC1B51C73F}';

  /// <summary>
  /// Virtual camera sink CLSID.
  /// </summary>
const
  CLSID_VFVirtualCameraSink: TGUID = '{AA6AB4DF-9670-4913-88BB-2CB381C19340}';

  /// <summary>
  /// Virtual camera source CLSID.
  /// </summary>
const
  CLSID_VFVirtualCameraSource: TGUID = '{AA4DA14E-644B-487a-A7CB-517A390B4BB8}';

end.
