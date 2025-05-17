// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : Roman
// Created          : 02-21-2024
//
// Last Modified By : Roman
// Last Modified On : 02-21-2024
// ***********************************************************************
// <copyright file="IFFmpegSourceSettings.cs" company="VisioForge">
//     Copyright (c) 2025. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Runtime.InteropServices;

namespace VisioForge.DirectShowAPI
{
    /// <summary>
    /// Enum FFMPEG_SOURCE_BUFFERING_MODE
    /// </summary>
    public enum FFMPEG_SOURCE_BUFFERING_MODE
    {
        /// <summary>
        /// The ffmpeg source buffering mode automatic
        /// </summary>
        FFMPEG_SOURCE_BUFFERING_MODE_AUTO,

        /// <summary>
        /// The ffmpeg source buffering mode on
        /// </summary>
        FFMPEG_SOURCE_BUFFERING_MODE_ON,

        /// <summary>
        /// The ffmpeg source buffering mode off
        /// </summary>
        FFMPEG_SOURCE_BUFFERING_MODE_OFF
    }

    /// <summary>
    /// Delegate FFMPEGDataCallbackDelegate.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferSize">Size of the buffer.</param>
    /// <param name="dataType">Type of the data.</param>
    /// <param name="startTime">The start time.</param>
    /// <param name="stopTime">The stop time.</param>
    /// <returns>System.Int32.</returns>
    public delegate int FFMPEGDataCallbackDelegate([In] IntPtr buffer, int bufferSize, int dataType, long startTime, long stopTime);

    /// <summary>
    /// Delegate FFMPEGTimestampCallbackDelegate
    /// </summary>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="pts">The PTS.</param>
    /// <returns>System.Int32.</returns>
    public delegate int FFMPEGTimestampCallbackDelegate(FFMPEGSourceMediaType mediaType, Int64 demuxerStartTime, Int64 streamStartTimr, Int64 timestamp);

    /// <summary>
    /// Specifies the type of media stream in the FFmpeg source.
    /// </summary>
    public enum FFMPEGSourceMediaType
    {
        /// <summary>
        /// Unknown media type.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Video stream.
        /// </summary>
        Video,

        /// <summary>
        /// Audio stream.
        /// </summary>
        Audio,

        /// <summary>
        /// Data stream.
        /// </summary>
        Data,

        /// <summary>
        /// Subtitle stream.
        /// </summary>
        Subtitle,

        /// <summary>
        /// Attachment stream.
        /// </summary>
        Attachment,

        /// <summary>
        /// Number of media types (used internally).
        /// </summary>
        NB
    };

    /// <summary>
    /// Interface IFFMPEGSourceSettings
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("1974D893-83E4-4F89-9908-795C524CC17E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFFMPEGSourceSettings
    {
        /// <summary>
        /// Gets the hw acceleration enabled.
        /// </summary>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool GetHWAccelerationEnabled();

        /// <summary>
        /// Sets the hw acceleration enabled.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetHWAccelerationEnabled([In, MarshalAs(UnmanagedType.Bool)] bool enabled);

        /// <summary>
        /// Gets the load time out.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        [PreserveSig]
        uint GetLoadTimeOut();

        /// <summary>
        /// Sets the load time out.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetLoadTimeOut([In] uint milliseconds);

        /// <summary>
        /// Gets the buffering mode.
        /// </summary>
        /// <returns>FFMPEG_SOURCE_BUFFERING_MODE.</returns>
        [PreserveSig]
        FFMPEG_SOURCE_BUFFERING_MODE GetBufferingMode();

        /// <summary>
        /// Sets the buffering mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetBufferingMode([In] FFMPEG_SOURCE_BUFFERING_MODE mode);

        /// <summary>
        /// Sets the custom option.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetCustomOption([In, MarshalAs(UnmanagedType.LPStr)] string name, [In, MarshalAs(UnmanagedType.LPStr)] string value);

        /// <summary>
        /// Clears the custom options.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int ClearCustomOptions();

        /// <summary>
        /// Sets callback for data buffer.
        /// </summary>
        /// <param name="callback">Callback pointer.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int SetDataCallback([MarshalAs(UnmanagedType.FunctionPtr)] FFMPEGDataCallbackDelegate callback);

        /// <summary>
        /// Sets callback for data buffer.
        /// </summary>
        /// <param name="callback">Callback pointer.</param>
        /// <returns>Returns 0 if the operation has been successful.</returns>
        [PreserveSig]
        int SetTimestampCallback([MarshalAs(UnmanagedType.FunctionPtr)] FFMPEGTimestampCallbackDelegate callback);

        /// <summary>
        /// Sets the audio enabled.
        /// </summary>
        /// <param name="enabled">The enabled.</param>
        /// <returns>HRESULT .</returns>
        [PreserveSig]
        int SetAudioEnabled([MarshalAs(UnmanagedType.Bool)] bool enabled);
    }
}
