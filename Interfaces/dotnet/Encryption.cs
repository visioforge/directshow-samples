namespace VisioForge.DirectShowAPI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security.Cryptography;

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("6F8162B5-778D-42b5-9242-1BBABB24FFC4"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFPasswordProvider
    {
        [PreserveSig]
        int QueryPassword(
            [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pBuffer,
            [In, Out] ref int plSize);
    }

    /// <summary>
    /// Encryption filter interface.
    /// </summary>
    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
           Guid("BAA5BD1E-3B30-425e-AB3B-CC20764AC253"),
           InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFCryptoConfig
    {
        [PreserveSig]
        int put_Provider([In] IVFPasswordProvider passwordProviderNotUsed);

        [PreserveSig]
        int get_Provider([Out] IVFPasswordProvider passwordProviderNotUsed);

        [PreserveSig]
        int put_Password(IntPtr buffer, [In] int size);

        [PreserveSig]
        int HavePassword();
    }

    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("99DC9BE5-0AFA-45d4-8370-AB021FB07CF4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFMuxerConfig
    {
        [PreserveSig]
        int get_SingleThread([Out] [MarshalAs(UnmanagedType.Bool)] out bool pValue);

        [PreserveSig]
        int put_SingleThread([In] [MarshalAs(UnmanagedType.Bool)] bool value);

        [PreserveSig]
        int get_CorrectTiming([Out] [MarshalAs(UnmanagedType.Bool)] out bool pValue);

        [PreserveSig]
        int put_CorrectTiming([In] [MarshalAs(UnmanagedType.Bool)] bool value);
    }

    public static class VFCryptoConfigHelper
    {
        public static int ApplyString(this IVFCryptoConfig cryptoConfig, string key)
        {
            string encryptionKey = key;

            if (string.IsNullOrEmpty(encryptionKey))
            {
                throw new Exception("Encryption key not set!");
            }

            IntPtr ptr = Marshal.StringToCoTaskMemUni(encryptionKey);
            cryptoConfig.put_Password(ptr, encryptionKey.Length * 2);
            Marshal.FreeCoTaskMem(ptr);

            return 0;
        }

        public static int ApplyFile(this IVFCryptoConfig cryptoConfig, string key)
        {
            if (!File.Exists(key))
            {
                throw new FileNotFoundException("Unable to open file key for encryptor.");
            }

            SHA256 shaM = new SHA256Managed();
            FileStream file = new FileStream(key, FileMode.Open);
            var sha = shaM.ComputeHash(file);
            string hex = BitConverter.ToString(sha).Replace("-", string.Empty).ToLowerInvariant();
            file.Close();
            shaM.Clear();

            ApplyString(cryptoConfig, hex);

            return 0;
        }

        public static int ApplyBinary(this IVFCryptoConfig cryptoConfig, byte[] key)
        {
            SHA256 shaM = new SHA256Managed();
            var sha = shaM.ComputeHash(key);
            string hex = BitConverter.ToString(sha).Replace("-", string.Empty).ToLowerInvariant();
            shaM.Clear();

            ApplyString(cryptoConfig, hex);

            return 0;
        }
    }
}
