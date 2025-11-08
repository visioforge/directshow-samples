using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VLC_Source_Demo
{
    /// <summary>
    /// Helper class for string marshaling to native code.
    /// </summary>
    internal static class StringHelper
    {
        /// <summary>
        /// Converts a managed string to a native UTF-8 encoded string pointer.
        /// </summary>
        /// <param name="managedString">The managed string to convert.</param>
        /// <returns>An IntPtr pointing to the native UTF-8 string. Caller is responsible for freeing the memory.</returns>
        public static IntPtr NativeUtf8FromString(string managedString)
        {
            if (managedString == null)
            {
                return IntPtr.Zero;
            }

            int len = Encoding.UTF8.GetByteCount(managedString);
            byte[] buffer = new byte[len + 1]; // +1 for null terminator
            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
            
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            
            return nativeUtf8;
        }

        /// <summary>
        /// Frees memory allocated by NativeUtf8FromString.
        /// </summary>
        /// <param name="nativeUtf8">The pointer to free.</param>
        public static void FreeNativeUtf8(IntPtr nativeUtf8)
        {
            if (nativeUtf8 != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeUtf8);
            }
        }
    }
}
