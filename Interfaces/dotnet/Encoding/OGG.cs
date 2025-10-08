using System;
using System.Collections.Generic;
using System.Text;

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Vorbis encoder filter interface.
    /// </summary>
    [ComImport, Guid("A4C6A887-7BD3-4b33-9A57-A3EB10924D3A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    ComConversionLoss]
    public interface IXiphVorbisEncodeSettings
    {
        /// <summary>
        /// Gets encoder settings.
        /// </summary>
        /// <returns>
        /// Returns encoder settings.
        /// </returns>
        [PreserveSig]
        object GetEncoderSettings();

        /// <summary>
        /// Sets quality.
        /// </summary>
        /// <param name="inQuality">
        /// Quality.
        /// </param>
        /// <returns>
        /// Returns true if the operation was successful.
        /// </returns>
        [PreserveSig]
        bool SetQuality(int inQuality);

        /// <summary>
        /// Sets encoding mode.
        /// </summary>
        /// <param name="inBitrate">
        /// Encoding mode.
        /// </param>
        /// <returns>
        /// Returns true if the operation was successful.
        /// </returns>
        [PreserveSig]
        bool SetBitrateQualityMode(int inBitrate);

        /// <summary>
        /// Sets bitrate.
        /// </summary>
        /// <param name="inBitrate">
        /// Bitrate.
        /// </param>
        /// <param name="inMinBitrate">
        /// Min bitrate.
        /// </param>
        /// <param name="inMaxBitrate">
        /// Max bitrate.
        /// </param>
        /// <returns>
        /// Returns true if the operation was successful.
        /// </returns>
        [PreserveSig]
        bool SetManaged(int inBitrate, int inMinBitrate, int inMaxBitrate);
    }

}
