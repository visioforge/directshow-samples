// {59E82754-B531-4A8E-A94D-57C75F01DA30}
DEFINE_GUID(IID_IVFRegister,
	0x59E82754, 0xB531, 0x4A8E, 0xA9, 0x4D, 0x57, 0xC7, 0x5F, 0x01, 0xDA, 0x30);


/// <summary>
/// Filter registration interface.
/// </summary>
DECLARE_INTERFACE_(IVFRegister, IUnknown)
{
	/// <summary>
	/// Sets registered.
	/// </summary>
	/// <param name="licenseKey">
	/// License Key.
	/// </param>
	STDMETHOD(SetLicenseKey)
		(THIS_
			WCHAR * licenseKey
			)PURE;
};