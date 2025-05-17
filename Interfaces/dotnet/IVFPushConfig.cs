// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="IVFPushConfig.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// Push config interface.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("260E28D7-48E6-4ABD-A14A-DD0BBD0AA9F5")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVFPushConfig
    {
        /// <summary>
        /// Determines whether has video stream.
        /// </summary>
        /// <returns><c>true</c> if has video stream; otherwise, <c>false</c>.</returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [PreserveSig]
        bool HasVideoStream();

        /// <summary>
        /// Determines whether [has audio stream].
        /// </summary>
        /// <returns><c>true</c> if has audio stream; otherwise, <c>false</c>.</returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [PreserveSig]
        bool HasAudioStream();

        /// <summary>
        /// Gets the width of the video stream.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetVideoStreamWidth();

        /// <summary>
        /// Gets the height of the video stream.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetVideoStreamHeight();

        /// <summary>
        /// Sets the video ffmpeg filters.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetVideoFFMPEGFilters([MarshalAs(UnmanagedType.LPWStr)] string value);

        /// <summary>
        /// Sets the audio ffmpeg filters.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetAudioFFMPEGFilters([MarshalAs(UnmanagedType.LPWStr)] string value);

        /// <summary>
        /// Sets the threads count.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetThreadsCount(int value);

        /// <summary>
        /// Sets the input ffmpeg format.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetInputFFMPEGFormat([MarshalAs(UnmanagedType.LPWStr)] string value);

        /// <summary>
        /// Sets the buffer frames count.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetBufferFramesCount(int value);

        /// <summary>
        /// Sets the audio stream.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetAudioStream(int value);

        /// <summary>
        /// Sets the subtitle stream.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetSubtitleStream(int value);

        /// <summary>
        /// Sets the video frame rate.
        /// </summary>
        /// <param name="frameRate">The frame rate.</param>
        [PreserveSig]
        void SetVideoFrameRate(double frameRate);

        /// <summary>
        /// Sets the memory source stream.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="extension">The extension.</param>
        [PreserveSig]
        void SetMemorySourceStream(
            IStream sourceStream,
            [MarshalAs(UnmanagedType.LPWStr)] string extension);

        /// <summary>
        /// Sets the on finished event.
        /// </summary>
        /// <param name="callback">The callback.</param>
        [PreserveSig]
        void SetOnFinishedEvent([MarshalAs(UnmanagedType.FunctionPtr)] object callback);

        /// <summary>
        /// Sets the network timeout.
        /// </summary>
        /// <param name="value">The value.</param>
        [PreserveSig]
        void SetNetworkTimeout(int value);
    }
}