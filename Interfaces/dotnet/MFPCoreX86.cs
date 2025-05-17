// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : Roman
// Created          : 08-15-2023
//
// Last Modified By : Roman
// Last Modified On : 02-16-2023
// ***********************************************************************
// <copyright file="CoreX86.cs" company="VisioForge">
//     VisioForge (c) 2006 - 2021
// </copyright>
// <summary>Media Framework Primitives Core (x86).</summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Media Framework Primitives Core (x86).
    /// </summary>
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal static class MFPCoreX86
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Init();

        /// <summary>
        /// Draws the rg B24 on rg B24.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB24OnRGB24(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int destWidth, int destHeight, int x, int y);

        /// <summary>
        /// Draw RGB24 on RGB24.
        /// </summary>
        /// <param name="srcPixels">The src pixels.</param>
        /// <param name="srcWidth">The src width.</param>
        /// <param name="srcHeight">The src height.</param>
        /// <param name="srcStride">The src stride.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">The dest width.</param>
        /// <param name="destHeight">The dest height.</param>
        /// <param name="destStride">The dest stride.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB24OnRGB24S(
            IntPtr srcPixels, int srcWidth, int srcHeight, int srcStride, IntPtr destPixels, int destWidth, int destHeight, int destStride, int x, int y);

        /// <summary>
        /// Draws the rg B24 on rg B32.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB24OnRGB32(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int destWidth, int destHeight, int x, int y);

        /// <summary>
        /// Draws the rg B32 on rg B24.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB32OnRGB24(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int destWidth, int destHeight, int x, int y);

        /// <summary>
        /// Draws the rg B32 on rg B24 s.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="destStride">The dest stride.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB32OnRGB24S(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            int srcStride,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int destStride,
            int x,
            int y);

        /// <summary>
        /// Draws the rg B32 on rg B24 position.
        /// </summary>
        /// <param name="inPixels">The in pixels.</param>
        /// <param name="inWidth">Width of the in.</param>
        /// <param name="inHeight">Height of the in.</param>
        /// <param name="inStride">The in stride.</param>
        /// <param name="srcX">The source x.</param>
        /// <param name="srcY">The source y.</param>
        /// <param name="bgPixels">The bg pixels.</param>
        /// <param name="bgWidth">Width of the bg.</param>
        /// <param name="bgHeight">Height of the bg.</param>
        /// <param name="bgStride">The bg stride.</param>
        /// <param name="dstX">The DST x.</param>
        /// <param name="dstY">The DST y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB32OnRGB24POS(
            IntPtr inPixels,
            int inWidth,
            int inHeight,
            int inStride,
            int srcX,
            int srcY,
            IntPtr bgPixels,
            int bgWidth,
            int bgHeight,
            int bgStride,
            int dstX,
            int dstY);

        /// <summary>
        /// Draws the rg B32 on rg B32.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="destX">The dest x.</param>
        /// <param name="destY">The dest y.</param>
        /// <param name="tmpPixels">The temporary pixels.</param>
        /// <param name="tmpWidth">Width of the temporary.</param>
        /// <param name="tmpHeight">Height of the temporary.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB32OnRGB32(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int destX,
            int destY,
            IntPtr tmpPixels,
            int tmpWidth,
            int tmpHeight);

        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB32OnRGB32POS(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            int srcX,
            int srcY,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int destX,
            int destY,
            IntPtr tmpPixels,
            int tmpWidth,
            int tmpHeight);

        /// <summary>
        /// Draws the rg B24 on rg B24 transp.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="transp">The transp.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB24OnRGB24_Transp(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int destWidth, int destHeight, int x, int y, int transp);

        /// <summary>
        /// Draws the rg B32 on rg B24 transp.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="transp">The transp.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB32OnRGB24_Transp(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int destWidth, int destHeight, int x, int y, int transp);

        /// <summary>
        /// Draws the rg B24 trasp change.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="transp">The transp.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Draw_RGB24_TraspChange(IntPtr pixels, int width, int height, int transp);

        /// <summary>
        /// Images the cut rg B24.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="cutX">The cut x.</param>
        /// <param name="cutY">The cut y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void ImageCutRGB24(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int cutX, int cutY);

        /// <summary>
        /// Images the cut rg B32.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="cutX">The cut x.</param>
        /// <param name="cutY">The cut y.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void ImageCutRGB32(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int cutX, int cutY);


        /// <summary>
        /// Yus the y2 to rg B24.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="dstPixels">The DST pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_YUY2ToRGB24")]
        public static extern void YUY2ToRGB24(IntPtr srcPixels, IntPtr dstPixels, int width, int height);

        /// <summary>
        /// Rgs the B24 to yu y2.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="dstPixels">The DST pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="dstStride">The DST stride.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB24ToYUY2")]
        public static extern void RGB24ToYUY2(IntPtr srcPixels, IntPtr dstPixels, int width, int height, int srcStride, int dstStride);

        /// <summary>
        /// Rgs the B32 to rg B24.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="dstPixels">The DST pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB32ToRGB24")]
        public static extern void RGB32ToRGB24(IntPtr srcPixels, IntPtr dstPixels, int width, int height);

        /// <summary>
        /// Rgs the B24 to rg B32.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="dstPixels">The DST pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB24ToRGB32")]
        public static extern void RGB24ToRGB32(IntPtr srcPixels, IntPtr dstPixels, int width, int height);

        /// <summary>
        /// Rgs the B24 to bg R24.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="pDest">The p dest.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="stride">The stride.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB24ToBGR24")]
        public static extern void RGB24ToBGR24(IntPtr pSource, IntPtr pDest, int width, int height, int stride);

        /// <summary>
        /// Rgs the B24 to rgba.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="dstPixels">The DST pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="alpha">The alpha.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB24ToRGBA")]
        public static extern void RGB24ToRGBA(IntPtr srcPixels, IntPtr dstPixels, int width, int height, int alpha);

        /// <summary>
        /// ies the V12 to rg B24.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="dstPixels">The DST pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_YV12ToRGB24")]
        public static extern void YV12ToRGB24(IntPtr srcPixels, IntPtr dstPixels, int width, int height);

        /// <summary>
        /// Rgs the B24 to y V12.
        /// </summary>
        /// <param name="srcPixels">The source pixels.</param>
        /// <param name="dstPixels">The DST pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="srcStride">The source stride.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB24ToYV12")]
        public static extern void RGB24ToYV12(IntPtr srcPixels, IntPtr dstPixels, int width, int height, int srcStride);

        /// <summary>
        /// Rgs the B24 to uyvy hdyc.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="pDest">The p dest.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destStride">The dest stride.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB24ToUYVY_HDYC")]
        public static extern void RGB24ToUYVY_HDYC(IntPtr pSource, IntPtr pDest, int width, int height, int srcStride, int destStride);

        /// <summary>
        /// Rgs the B32 to uyvy hdyc.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="pDest">The p dest.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destStride">The dest stride.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB32ToUYVY_HDYC")]
        public static extern void RGB32ToUYVY_HDYC(IntPtr pSource, IntPtr pDest, int width, int height, int srcStride, int destStride);

        /// <summary>
        /// Rgs the B32 to yu y2.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="pDest">The p dest.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destStride">The dest stride.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_RGB32ToYUY2")]
        public static extern void RGB32ToYUY2(IntPtr pSource, IntPtr pDest, int width, int height, int srcStride, int destStride);


        /// <summary>
        /// Effects the blue.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectBlue(IntPtr pSource, int width, int height);

        /// <summary>
        /// Effects the blur.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="tmpArray">The temporary array.</param>
        /// <param name="tmpArrayLen">Length of the temporary array.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectBlur(
            IntPtr pSource,
            int width,
            int height,
            IntPtr tmpArray,
            int tmpArrayLen);

        /// <summary>
        /// Effects the blur ex.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="range">The range.</param>
        /// <param name="vertical">if set to <c>true</c> [vertical].</param>
        /// <param name="horizontal">if set to <c>true</c> [horizontal].</param>
        /// <param name="tmpArray">The temporary array.</param>
        /// <param name="tmpArrayLen">Length of the temporary array.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectBlurEx(
            IntPtr pSource,
            int width,
            int height,
            int range,
            [In][MarshalAs(UnmanagedType.Bool)] bool vertical,
            [In][MarshalAs(UnmanagedType.Bool)] bool horizontal,
            IntPtr tmpArray,
            int tmpArrayLen);

        /// <summary>
        /// Effects the color noise.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectColorNoise(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the contrast.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectContrast(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the filter blue.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectFilterBlue(IntPtr pSource, int width, int height, int min, int max);

        /// <summary>
        /// Effects the filter green.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectFilterGreen(IntPtr pSource, int width, int height, int min, int max);

        /// <summary>
        /// Effects the filter red.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectFilterRed(IntPtr pSource, int width, int height, int min, int max);

        /// <summary>
        /// Effects the green.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectGreen(IntPtr pSource, int width, int height);

        /// <summary>
        /// Effects the sharpen.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pTemp">The p temporary.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectSharpen(IntPtr pSource, int width, int height, IntPtr pTemp);

        /// <summary>
        /// Effects the lightness.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectLightness(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the darkness.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectDarkness(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the marble.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="turbulence">The turbulence.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectMarble(IntPtr pSource, int width, int height, double scale, int turbulence);

        /// <summary>
        /// Effects the mirror down.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectMirrorDown(IntPtr pSource, int width, int height);

        /// <summary>
        /// Effects the mirror right.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectMirrorRight(IntPtr pSource, int width, int height);

        /// <summary>
        /// Effects the mono noise.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectMonoNoise(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the mosaic.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="size">The size.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectMosaic(IntPtr pSource, int width, int height, int size);

        /// <summary>
        /// Effects the posterize.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectPosterize(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the red.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectRed(IntPtr pSource, int width, int height);

        /// <summary>
        /// Effects the saturation.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectSaturation(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the shake down.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="factor">The factor.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectShakeDown(IntPtr pSource, int width, int height, int factor);

        /// <summary>
        /// Effects the solorize.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectSolorize(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the spray.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectSpray(IntPtr pSource, int width, int height, int amount);

        /// <summary>
        /// Effects the deinterlace triangle.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pTemp">The p temporary.</param>
        /// <param name="weight">The weight.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectDeinterlaceTriangle(IntPtr pSource, int width, int height, IntPtr pTemp, int weight);

        /// <summary>
        /// Effects the denoise SNR.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pTemp">The p temporary.</param>
        /// <param name="threshold">The threshold.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void EffectDenoiseSNR(IntPtr pSource, int width, int height, IntPtr pTemp, int threshold);

        /// <summary>
        /// Effects the denoise adaptive.
        /// </summary>
        /// <param name="frame0">The frame0.</param>
        /// <param name="frame1">The frame1.</param>
        /// <param name="frame2">The frame2.</param>
        /// <param name="output">The output.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="srcPitch">The source pitch.</param>
        /// <param name="destPitch">The dest pitch.</param>
        /// <param name="threshold">The threshold.</param>
        /// <param name="blurType">Type of the blur.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void EffectDenoiseAdaptive(
            IntPtr frame0,
            IntPtr frame1,
            IntPtr frame2,
            IntPtr output,
            int width,
            int height,
            int srcPitch,
            int destPitch,
            byte threshold,
            byte blurType);

        /// <summary>
        /// Effects the denoise mosquito.
        /// </summary>
        /// <param name="frame0">The frame0.</param>
        /// <param name="frame1">The frame1.</param>
        /// <param name="frame2">The frame2.</param>
        /// <param name="output">The output.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="srcPitch">The source pitch.</param>
        /// <param name="destPitch">The dest pitch.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void EffectDenoiseMosquito(
            IntPtr frame0,
            IntPtr frame1,
            IntPtr frame2,
            IntPtr output,
            int width,
            int height,
            int srcPitch,
            int destPitch);

        /// <summary>
        /// Motions the detection build matrix.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="linesX">The lines x.</param>
        /// <param name="linesY">The lines y.</param>
        /// <param name="matrixRes">The matrix resource.</param>
        /// <param name="matrix">The matrix.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void MotionDetectionBuildMatrix(
            int width, int height, int linesX, int linesY, byte[] matrixRes, IntPtr matrix);

        /// <summary>
        /// Motions the detection compare images.
        /// </summary>
        /// <param name="pic1">The pic1.</param>
        /// <param name="pic2">The pic2.</param>
        /// <param name="Matrix">The matrix.</param>
        /// <param name="compareGreyscale">if set to <c>true</c> [compare greyscale].</param>
        /// <param name="compareRed">if set to <c>true</c> [compare red].</param>
        /// <param name="compareGreen">if set to <c>true</c> [compare green].</param>
        /// <param name="compareBlue">if set to <c>true</c> [compare blue].</param>
        /// <param name="numPixels">The number pixels.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int MotionDetectionCompareImages(
             IntPtr pic1,
             IntPtr pic2,
             IntPtr Matrix,
             bool compareGreyscale,
             bool compareRed,
             bool compareGreen,
             bool compareBlue,
             int numPixels);

        /// <summary>
        /// Motions the detection highlight.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="numPixels">The number pixels.</param>
        /// <param name="color">The color.</param>
        /// <param name="chlThreshold">The CHL threshold.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void MotionDetectionHighlight(IntPtr frame, IntPtr matrix, int numPixels, int color, int chlThreshold);

        /// <summary>
        /// Effects the fade in out.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="stopTime">The stop time.</param>
        /// <param name="currentTime">The current time.</param>
        /// <param name="fadeIn">if set to <c>true</c> [fade in].</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void EffectFadeInOut(
            IntPtr data,
            int width,
            int height,
            long startTime,
            long stopTime,
            long currentTime,
            [In][MarshalAs(UnmanagedType.Bool)] bool fadeIn);

        /// <summary>
        /// Rotates the in.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pTemp">The p temporary.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="stretch">if set to <c>true</c> [stretch].</param>
        /// <param name="pWorkBuffer">The p work buffer.</param>
        /// <param name="pWorkBufferSize">Size of the p work buffer.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static void RotateIn(
            IntPtr pSource,
            int width,
            int height,
            IntPtr pTemp,
            double angle,
            [In][MarshalAs(UnmanagedType.Bool)] bool stretch,
            IntPtr pWorkBuffer,
            ref int pWorkBufferSize);

        /// <summary>
        /// Colors the key rg B24.
        /// </summary>
        /// <param name="pSrcChroma">The p source chroma.</param>
        /// <param name="srcChromaStride">The source chroma stride.</param>
        /// <param name="pSrcBg">The p source bg.</param>
        /// <param name="srcBgStride">The source bg stride.</param>
        /// <param name="pDest">The p dest.</param>
        /// <param name="destStride">The dest stride.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void ColorKeyRGB24(
            IntPtr pSrcChroma, int srcChromaStride, IntPtr pSrcBg, int srcBgStride, IntPtr pDest, int destStride, int width, int height, int color);

        /// <summary>
        /// JPEGs the file decode to RGB.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="output">The output.</param>
        /// <param name="outputSize">Size of the output.</param>
        /// <param name="bgr">if set to <c>true</c> [BGR].</param>
        /// <returns>System.Int32.</returns>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int JPEGFileDecodeToRGB(
            [MarshalAs(UnmanagedType.LPWStr)] string source,
            IntPtr output,
            int outputSize,
            [MarshalAs(UnmanagedType.Bool)] bool bgr);

        /// <summary>
        /// JPEGs the data decode to RGB.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceSize">Size of the source.</param>
        /// <param name="output">The output.</param>
        /// <param name="outputSize">Size of the output.</param>
        /// <param name="bgr">if set to <c>true</c> [BGR].</param>
        /// <returns>System.Int32.</returns>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int JPEGDataDecodeToRGB(
            IntPtr source,
            int sourceSize,
            IntPtr output,
            int outputSize,
            [MarshalAs(UnmanagedType.Bool)] bool bgr);

        /// <summary>
        /// JPEGs the file encode from RGB.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="source">The source.</param>
        /// <param name="sourceSize">Size of the source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bgr">if set to <c>true</c> [BGR].</param>
        /// <returns>System.Int32.</returns>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int JPEGFileEncodeFromRGB(
            [MarshalAs(UnmanagedType.LPWStr)] string filename,
            int quality,
            IntPtr source,
            int sourceSize,
            int width,
            int height,
            bool bgr);

        /// <summary>
        /// JPEGs the data encode from RGB.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceSize">Size of the source.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="output">The output.</param>
        /// <param name="outputSize">Size of the output.</param>
        /// <param name="bgr">if set to <c>true</c> [BGR].</param>
        /// <returns>System.Int32.</returns>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int JPEGDataEncodeFromRGB(
            IntPtr source,
            int sourceSize,
            int quality,
            int width,
            int height,
            IntPtr output,
            out int outputSize,
            bool bgr);

        /// <summary>
        /// Lavs the is available.
        /// </summary>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LAV_IsAvailable();


        /// <summary>
        /// Flips the horizontal rg B24.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_FlipHorizontalRGB24")]
        public static extern void FlipHorizontalRGB24(IntPtr pixels, int width, int height);

        /// <summary>
        /// Flips the horizontal rg B32.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_FlipHorizontalRGB32")]
        public static extern void FlipHorizontalRGB32(IntPtr pixels, int width, int height);

        /// <summary>
        /// Flips the vertical rg B24.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_FlipVerticalRGB24")]
        public static extern void FlipVerticalRGB24(IntPtr pixels, int width, int height);

        /// <summary>
        /// Flips the vertical rg B32.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_FlipVerticalRGB32")]
        public static extern void FlipVerticalRGB32(IntPtr pixels, int width, int height);

        /// <summary>
        /// Effects the greyscale.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pTemp">The p temporary.</param>
        [DllImport("VisioForge_MFP.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "EffectGreyscale")]
        public extern static void EffectGreyscale(IntPtr pSource, int width, int height, IntPtr pTemp);

        /// <summary>
        /// Effects the mirror down ex.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="tempData">The temporary data.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_EffectMirrorDownEx")]
        public extern static void EffectMirrorDownEx(IntPtr pSource, int width, int height, IntPtr tempData);

        /// <summary>
        /// Effects the deinterlace cavt.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pTemp">The p temporary.</param>
        /// <param name="threshold">The threshold.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_EffectDeinterlaceCAVT")]
        public extern static void EffectDeinterlaceCAVT(IntPtr pSource, int width, int height, IntPtr pTemp, int threshold);

        /// <summary>
        /// Effects the invert.
        /// </summary>
        /// <param name="pSource">The p source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pTemp">The p temporary.</param>
        [DllImport("VisioForge_MFPX.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "MFPX_EffectInvert")]
        public extern static void EffectInvert(IntPtr pSource, int width, int height, IntPtr pTemp);
    }
}
