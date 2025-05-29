// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 05-23-2017
// ***********************************************************************
// <copyright file="IVlcSrc.cs" company="VisioForge">
//     Copyright (c) 2025 VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Interface IVlcSrc.
    /// </summary>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("77493EB7-6D00-41C5-9535-7C593824E892")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVlcSrc
    {
        /// <summary>
        /// Sets the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetFile([MarshalAs(UnmanagedType.LPWStr)] string file);

        /// <summary>
        /// Gets the audio tracks count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetAudioTracksCount(out int count);

        /// <summary>
        /// Gets the audio track information.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetAudioTrackInfo(
                    int number,
                    out int id,
                    [MarshalAs(UnmanagedType.LPWStr)] string name);

        /// <summary>
        /// Gets the audio track.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetAudioTrack(out int id);

        /// <summary>
        /// Sets the audio track.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetAudioTrack(int id);

        /// <summary>
        /// Gets the subtitles count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetSubtitlesCount(out int count);

        /// <summary>
        /// Gets the subtitle information.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetSubtitleInfo(
                    int number,
                    out int id,
                    [MarshalAs(UnmanagedType.LPWStr)] string name);

        /// <summary>
        /// Gets the subtitle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int GetSubtitle(out int id);

        /// <summary>
        /// Sets the subtitle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetSubtitle(int id);
    }

    /// <summary>
    /// Interface IVlcSrc2
    /// Implements the <see cref="VisioForge.Core.Interfaces.IVlcSrc" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.Interfaces.IVlcSrc" />
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("CCE122C0-172C-4626-B4B6-42B039E541CB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVlcSrc2 : IVlcSrc
    {
        /// <summary>
        /// Sets the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int SetFile([MarshalAs(UnmanagedType.LPWStr)] string file);

        /// <summary>
        /// Gets the audio tracks count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetAudioTracksCount(out int count);

        /// <summary>
        /// Gets the audio track information.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetAudioTrackInfo(
                    int number,
                    out int id,
                    [MarshalAs(UnmanagedType.LPWStr)] string name);

        /// <summary>
        /// Gets the audio track.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetAudioTrack(out int id);

        /// <summary>
        /// Sets the audio track.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int SetAudioTrack(int id);

        /// <summary>
        /// Gets the subtitles count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetSubtitlesCount(out int count);

        /// <summary>
        /// Gets the subtitle information.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetSubtitleInfo(
                    int number,
                    out int id,
                    [MarshalAs(UnmanagedType.LPWStr)] string name);

        /// <summary>
        /// Gets the subtitle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetSubtitle(out int id);

        /// <summary>
        /// Sets the subtitle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int SetSubtitle(int id);

        //[PreserveSig]
        //int SetCustomCommandLine(
        //    [In][Out][MarshalAsAttribute(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr)] string[] params_, 
        //    int size);

        /// <summary>
        /// Sets the custom command line.
        /// </summary>
        /// <param name="params_">The parameters.</param>
        /// <param name="size">The size.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetCustomCommandLine(
            [In][Out][MarshalAsAttribute(UnmanagedType.LPArray)] IntPtr[] params_,
            int size);
    }

    /// <summary>
    /// Interface IVlcSrc3.
    /// Implements the <see cref="VisioForge.Core.Interfaces.IVlcSrc2" />.
    /// </summary>
    /// <seealso cref="VisioForge.Core.Interfaces.IVlcSrc2" />
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("3DFBED0C-E4A8-401C-93EF-CBBFB65223DD")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IVlcSrc3 : IVlcSrc2
    {
        /// <summary>
        /// Sets the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int SetFile([MarshalAs(UnmanagedType.LPWStr)] string file);

        /// <summary>
        /// Gets the audio tracks count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetAudioTracksCount(out int count);

        /// <summary>
        /// Gets the audio track information.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetAudioTrackInfo(
                    int number,
                    out int id,
                    [MarshalAs(UnmanagedType.LPWStr)] string name);

        /// <summary>
        /// Gets the audio track.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetAudioTrack(out int id);

        /// <summary>
        /// Sets the audio track.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int SetAudioTrack(int id);

        /// <summary>
        /// Gets the subtitles count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetSubtitlesCount(out int count);

        /// <summary>
        /// Gets the subtitle information.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetSubtitleInfo(
                    int number,
                    out int id,
                    [MarshalAs(UnmanagedType.LPWStr)] string name);

        /// <summary>
        /// Gets the subtitle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int GetSubtitle(out int id);

        /// <summary>
        /// Sets the subtitle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int SetSubtitle(int id);

        //[PreserveSig]
        //int SetCustomCommandLine(
        //    [In][Out][MarshalAsAttribute(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr)] string[] params_, 
        //    int size);

        /// <summary>
        /// Sets the custom command line.
        /// </summary>
        /// <param name="params_">The parameters.</param>
        /// <param name="size">The size.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        new int SetCustomCommandLine(
            [In][Out][MarshalAsAttribute(UnmanagedType.LPArray)] IntPtr[] params_,
            int size);

        /// <summary>
        /// Sets the default frame rate.
        /// </summary>
        /// <param name="frameRate">The frame rate.</param>
        /// <returns>System.Int32.</returns>
        [PreserveSig]
        int SetDefaultFrameRate(           
           double frameRate);
    }
}
