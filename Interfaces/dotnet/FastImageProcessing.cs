// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 04-16-2022
//
// Last Modified By : roman
// Last Modified On : 08-26-2022
// ***********************************************************************
// <copyright file="FastImageProcessing.cs" company="VisioForge">
//     Copyright (c) 2006-2022
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    /// <summary>
    /// Fast Image Processing.
    /// </summary>
    [System.Security.SuppressUnmanagedCodeSecurity]
    public static partial class FastImageProcessing
    {
        /// <summary>
        /// The X86.
        /// </summary>
        private static bool X86 = IntPtr.Size == 4;

        /// <summary>
        /// The found.
        /// </summary>
        private static bool _found = false;

        /// <summary>
        /// The maximum thread count.
        /// </summary>
        private const int MAX_THREAD_COUNT = 6;

        /// <summary>
        /// The smart resource limit.
        /// </summary>
        private const int SMART_RES_LIMIT = 200 * 200;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Init()
        {
            try
            {
                if (X86)
                {
                    MFPCoreX86.Init();
                }
                else
                {
                    MFPCoreX64.Init();
                }

                _found = true;
            }
            catch (Exception e)
            {
                _found = false;
            }
        }

        /// <summary>
        /// Determines whether this instance is found.
        /// </summary>
        /// <returns><c>true</c> if this instance is found; otherwise, <c>false</c>.</returns>
        public static bool IsFound() => _found;

        /// <summary>
        /// Draws the RGB24 image on RGB24 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="destStride">The destination stride.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public static void Draw_RGB24OnRGB24S(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            int srcStride,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int destStride,
            int x,
            int y)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB24OnRGB24S(srcPixels, srcWidth, srcHeight, srcStride, destPixels, destWidth, destHeight, destStride, x, y);
            }
            else
            {
                MFPCoreX64.Draw_RGB24OnRGB24S(srcPixels, srcWidth, srcHeight, srcStride, destPixels, destWidth, destHeight, destStride, x, y);
            }
        }

        /// <summary>
        /// Draws the RGB24 image on RGB24 image (legacy).
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public static void Draw_RGB24OnRGB24Old(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int x,
            int y)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB24OnRGB24(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y);
            }
            else
            {
                MFPCoreX64.Draw_RGB24OnRGB24(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y);
            }
        }

        /// <summary>
        /// Draws the RGB24 image on RGB32 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public static void Draw_RGB24OnRGB32(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int x,
            int y)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB24OnRGB32(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y);
            }
            else
            {
                MFPCoreX64.Draw_RGB24OnRGB32(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y);
            }
        }

        /// <summary>
        /// Delegate D_Draw_RGB32OnRGB24S.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="destStride">The destination stride.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private delegate void D_Draw_RGB32OnRGB24S(
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
        /// Draws the RGB32 image on RGB24 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Draw_RGB32OnRGB24(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int x,
            int y,
            bool smartMultithreading = true)
        {
            Draw_RGB32OnRGB24S(
                srcPixels,
                srcWidth,
                srcHeight,
                ImageHelper.GetStrideRGB32(srcWidth),
                destPixels,
                destWidth,
                destHeight,
                ImageHelper.GetStrideRGB24(destWidth),
                x,
                y,
                smartMultithreading);
        }

        /// <summary>
        /// Draws the RGB32 image on RGB24 image.
        /// </summary>
        /// <param name="inPixels">The in pixels.</param>
        /// <param name="inWidth">Width of the in.</param>
        /// <param name="inHeight">Height of the in.</param>
        /// <param name="inStride">The in stride.</param>
        /// <param name="srcX">The source x.</param>
        /// <param name="srcY">The source y.</param>
        /// <param name="destPixels">The dest pixels.</param>
        /// <param name="destWidth">Width of the dest.</param>
        /// <param name="destHeight">Height of the dest.</param>
        /// <param name="destStride">The dest stride.</param>
        /// <param name="destX">The dest x.</param>
        /// <param name="destY">The dest y.</param>
        public static void Draw_RGB32OnRGB24POS(
            IntPtr inPixels,
            int inWidth,
            int inHeight,
            int inStride,
            int srcX,
            int srcY,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int destStride,
            int destX,
            int destY)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB32OnRGB24POS(
                    inPixels,
                    inWidth,
                    inHeight,
                    inStride,
                    srcX,
                    srcY,
                    destPixels,
                    destWidth,
                    destHeight,
                    destStride,
                    destX,
                    destY);
            }
            else
            {
                MFPCoreX64.Draw_RGB32OnRGB24POS(
                    inPixels,
                    inWidth,
                    inHeight,
                    inStride,
                    srcX,
                    srcY,
                    destPixels,
                    destWidth,
                    destHeight,
                    destStride,
                    destX,
                    destY);
            }
        }

        /// <summary>
        /// Draws the RGB32 image on RGB24 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="destStride">The destination stride.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Draw_RGB32OnRGB24S(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            int srcStride,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int destStride,
            int x,
            int y,
            bool smartMultithreading = true)
        {
            D_Draw_RGB32OnRGB24S CALL;
            if (X86)
            {
                CALL = MFPCoreX86.Draw_RGB32OnRGB24S;
            }
            else
            {
                CALL = MFPCoreX64.Draw_RGB32OnRGB24S;
            }

            int srcWidthFix = srcWidth;
            if (srcWidthFix > destWidth - x)
            {
                srcWidthFix = destWidth - x;
            }

            int srcHeightFix = srcHeight;
            if (srcHeightFix > destHeight - y)
            {
                srcHeightFix = destHeight - y;
            }

            if (!smartMultithreading || (srcWidthFix * srcHeightFix < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidthFix,
                    srcHeight,
                    srcStride,
                    destPixels,
                    destWidth,
                    destHeight,
                    destStride,
                    x,
                    y);
            }
            else
            {
                var threads = Environment.ProcessorCount;
                int partHeight = srcHeightFix / threads;

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * srcStride),
                            srcWidthFix,
                            partHeight,
                            srcStride,
                            destPixels + (k * partHeight * destStride),
                            destWidth,
                            destHeight,
                            destStride,
                            x,
                            y);
                    });

                if (partHeight * threads != srcHeightFix)
                {
                    int specPartHeight = srcHeightFix - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * srcStride),
                        srcWidthFix,
                        specPartHeight,
                        srcStride,
                        destPixels + (threads * partHeight * destStride),
                        destWidth,
                        destHeight,
                        destStride,
                        x,
                        y);
                }
            }
        }

        /// <summary>
        /// Draws the RGB32 image on RGB24 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="srcStride">The source stride.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="destStride">The destination stride.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Draw_RGB32OnRGB24SX(
            IntPtr srcPixels,
            Rectangle srcRect,
            int srcStride,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int destStride,
            int x,
            int y)
        {
            int srcWidthFix = srcRect.Width;
            if (srcWidthFix > destWidth - x)
            {
                srcWidthFix = destWidth - x;
            }

            int srcHeightFix = srcRect.Height;
            if (srcHeightFix > destHeight - y)
            {
                srcHeightFix = destHeight - y;
            }

            var srcPixelsX = srcPixels + srcRect.Top * srcStride + srcRect.Left * 4;
            if (X86)
            {
                MFPCoreX86.Draw_RGB32OnRGB24S(
                    srcPixelsX,
                    srcWidthFix,
                    srcHeightFix,
                    srcStride,
                    destPixels,
                    destWidth,
                    destHeight,
                    destStride,
                    x,
                    y);
            }
            else
            {
                MFPCoreX64.Draw_RGB32OnRGB24S(
                    srcPixelsX,
                    srcWidthFix,
                    srcHeightFix,
                    srcStride,
                    destPixels,
                    destWidth,
                    destHeight,
                    destStride,
                    x,
                    y);
            }
        }

        /// <summary>
        /// Draws the RGB32 image on RGB32 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="tmpPixels">The temporary data.</param>
        /// <param name="tmpWidth">Width of the temporary data.</param>
        /// <param name="tmpHeight">Height of the temporary data.</param>
        public static void Draw_RGB32OnRGB32(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int x,
            int y,
            IntPtr tmpPixels,
            int tmpWidth,
            int tmpHeight)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB32OnRGB32(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y, tmpPixels, tmpWidth, tmpHeight);
            }
            else
            {
                MFPCoreX64.Draw_RGB32OnRGB32(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y, tmpPixels, tmpWidth, tmpHeight);
            }
        }

        /// <summary>
        /// Draws the RGB24 image on RGB24 image with transparency.
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
        public static void Draw_RGB24OnRGB24_Transp(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int x,
            int y,
            int transp)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB24OnRGB24_Transp(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y, transp);
            }
            else
            {
                MFPCoreX64.Draw_RGB24OnRGB24_Transp(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y, transp);
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// Draws the RGB32 image on RGB24 image with transparency.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="destWidth">Width of the destination.</param>
        /// <param name="destHeight">Height of the destination.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="transp">The transparency.</param>
        public static void Draw_RGB32OnRGB24_Transp(
            IntPtr srcPixels,
            int srcWidth,
            int srcHeight,
            IntPtr destPixels,
            int destWidth,
            int destHeight,
            int x,
            int y,
            int transp)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB32OnRGB24_Transp(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y, transp);
            }
            else
            {
                MFPCoreX64.Draw_RGB32OnRGB24_Transp(srcPixels, srcWidth, srcHeight, destPixels, destWidth, destHeight, x, y, transp);
            }
        }

        /// <summary>
        /// Change the RGB24 image transparency.
        /// </summary>
        /// <param name="pixels">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="transp">The transparency.</param>
        public static void Draw_RGB24_TraspChange(IntPtr pixels, int width, int height, int transp)
        {
            if (X86)
            {
                MFPCoreX86.Draw_RGB24_TraspChange(pixels, width, height, transp);
            }
            else
            {
                MFPCoreX64.Draw_RGB24_TraspChange(pixels, width, height, transp);
            }
        }

        /// <summary>
        /// Cuts the RGB24 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="cutX">The cut x.</param>
        /// <param name="cutY">The cut y.</param>
        public static void ImageCutRGB24(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int cutX, int cutY)
        {
            if (X86)
            {
                MFPCoreX86.ImageCutRGB24(srcPixels, srcWidth, srcHeight, destPixels, cutX, cutY);
            }
            else
            {
                MFPCoreX64.ImageCutRGB24(srcPixels, srcWidth, srcHeight, destPixels, cutX, cutY);
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// Cuts the RGB32 image.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="destPixels">The destination data.</param>
        /// <param name="cutX">The cut x.</param>
        /// <param name="cutY">The cut y.</param>
        public static void ImageCutRGB32(
            IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr destPixels, int cutX, int cutY)
        {
            if (X86)
            {
                MFPCoreX86.ImageCutRGB32(srcPixels, srcWidth, srcHeight, destPixels, cutX, cutY);
            }
            else
            {
                MFPCoreX64.ImageCutRGB32(srcPixels, srcWidth, srcHeight, destPixels, cutX, cutY);
            }
        }

        /// <summary>
        /// Delegate EffectBlueDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private delegate void EffectBlueDelegate(IntPtr srcPixels, int width, int height);

        /// <summary>
        /// Blurs the specified source data.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="tmpArray">The temporary array.</param>
        /// <param name="tmpArrayLen">Length of the temporary array.</param>
        public static void Blur(
            IntPtr srcPixels,
            int width,
            int height,
            IntPtr tmpArray,
            int tmpArrayLen)
        {
            if (X86)
            {
                MFPCoreX86.EffectBlur(srcPixels, width, height, tmpArray, tmpArrayLen);
            }
            else
            {
                MFPCoreX64.EffectBlur(srcPixels, width, height, tmpArray, tmpArrayLen);
            }
        }

        /// <summary>
        /// Blurs the source data.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="range">The range.</param>
        /// <param name="vertical">if set to <c>true</c> [vertical].</param>
        /// <param name="horizontal">if set to <c>true</c> [horizontal].</param>
        /// <param name="tmpArray">The temporary array.</param>
        /// <param name="tmpArrayLen">Length of the temporary array.</param>
        public static void BlurEx(
            IntPtr srcPixels,
            int width,
            int height,
            int range,
            bool vertical,
            bool horizontal,
            IntPtr tmpArray,
            int tmpArrayLen)
        {
            if (X86)
            {
                MFPCoreX86.EffectBlurEx(srcPixels, width, height, range, vertical, horizontal, tmpArray, tmpArrayLen);
            }
            else
            {
                MFPCoreX64.EffectBlurEx(srcPixels, width, height, range, vertical, horizontal, tmpArray, tmpArrayLen);
            }
        }

        /// <summary>
        /// Delegate EffectColorNoiseDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        private delegate void EffectColorNoiseDelegate(IntPtr srcPixels, int width, int height, int amount);

        /// <summary>
        /// Delegate EffectContrastelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        private delegate void EffectContrastelegate(IntPtr srcPixels, int width, int height, int amount);

        /// <summary>
        /// Contrast.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Contrast(IntPtr srcPixels, int srcWidth, int srcHeight, int amount, bool smartMultithreading = true)
        {
            EffectContrastelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectContrast;
            }
            else
            {
                CALL = MFPCoreX64.EffectContrast;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    amount);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            amount);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        amount);
                }
            }
        }

        /// <summary>
        /// Delegate EffectFilterBlueDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        private delegate void EffectFilterBlueDelegate(IntPtr srcPixels, int width, int height, int min, int max);

        /// <summary>
        /// Filter blue.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void FilterBlue(IntPtr srcPixels, int srcWidth, int srcHeight, int min, int max, bool smartMultithreading = true)
        {
            EffectFilterBlueDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectFilterBlue;
            }
            else
            {
                CALL = MFPCoreX64.EffectFilterBlue;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    min,
                    max);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            min,
                            max);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        min,
                        max);
                }
            }
        }

        /// <summary>
        /// Delegate EffectFilterGreenDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        private delegate void EffectFilterGreenDelegate(IntPtr srcPixels, int width, int height, int min, int max);

        /// <summary>
        /// Filter green.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void FilterGreen(IntPtr srcPixels, int srcWidth, int srcHeight, int min, int max, bool smartMultithreading = true)
        {
            EffectFilterGreenDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectFilterGreen;
            }
            else
            {
                CALL = MFPCoreX64.EffectFilterGreen;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    min,
                    max);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            min,
                            max);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        min,
                        max);
                }
            }
        }

        /// <summary>
        /// Delegate EffectFilterRedDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        private delegate void EffectFilterRedDelegate(IntPtr srcPixels, int width, int height, int min, int max);

        /// <summary>
        /// Filter red.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void FilterRed(IntPtr srcPixels, int srcWidth, int srcHeight, int min, int max, bool smartMultithreading = true)
        {
            EffectFilterRedDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectFilterRed;
            }
            else
            {
                CALL = MFPCoreX64.EffectFilterRed;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    min,
                    max);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            min,
                            max);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        min,
                        max);
                }
            }
        }

        /// <summary>
        /// Delegate EffectGreenDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private delegate void EffectGreenDelegate(IntPtr srcPixels, int width, int height);

        /// <summary>
        /// Green.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Green(IntPtr srcPixels, int srcWidth, int srcHeight, bool smartMultithreading = true)
        {
            EffectGreenDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectGreen;
            }
            else
            {
                CALL = MFPCoreX64.EffectGreen;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight);
                }
            }
        }


        /// <summary>
        /// Sharpen.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="temp">The temporary data.</param>
        public static void Sharpen(IntPtr srcPixels, int width, int height, IntPtr temp)
        {
            // MT not required
            if (X86)
            {
                MFPCoreX86.EffectSharpen(srcPixels, width, height, temp);
            }
            else
            {
                MFPCoreX64.EffectSharpen(srcPixels, width, height, temp);
            }
        }

        /// <summary>
        /// Invert.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="temp">The temporary data.</param>
        public static void Invert(IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr temp)
        {
            // MT not required
            if (X86)
            {
                MFPCoreX86.EffectInvert(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    temp);
            }
            else
            {
                MFPCoreX64.EffectInvert(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    temp);
            }
        }

        /// <summary>
        /// Delegate EffectLightnessDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        private delegate void EffectLightnessDelegate(IntPtr srcPixels, int width, int height, int amount);

        /// <summary>
        /// Brightness.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Brightness(IntPtr srcPixels, int srcWidth, int srcHeight, int amount, bool smartMultithreading = true)
        {
            EffectLightnessDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectLightness;
            }
            else
            {
                CALL = MFPCoreX64.EffectLightness;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    amount);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            amount);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        amount);
                }
            }
        }

        /// <summary>
        /// Delegate EffectDarknessDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        private delegate void EffectDarknessDelegate(IntPtr srcPixels, int width, int height, int amount);

        /// <summary>
        /// Darkness.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Darkness(IntPtr srcPixels, int srcWidth, int srcHeight, int amount, bool smartMultithreading = true)
        {
            EffectDarknessDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectDarkness;
            }
            else
            {
                CALL = MFPCoreX64.EffectDarkness;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    amount);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            amount);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        amount);
                }
            }
        }

        /// <summary>
        /// Marble.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="turbulence">The turbulence.</param>
        public static void Marble(IntPtr srcPixels, int width, int height, double scale, int turbulence)
        {
            if (X86)
            {
                MFPCoreX86.EffectMarble(srcPixels, width, height, scale, turbulence);
            }
            else
            {
                MFPCoreX64.EffectMarble(srcPixels, width, height, scale, turbulence);
            }
        }

        //public static void EffectMirrorDown(IntPtr srcPixels, int width, int height)
        //{
        //    if (X86)
        //    {
        //        MFPCoreX86.EffectMirrorDown(srcPixels, width, height);
        //    }
        //    else
        //    {
        //        MFPCoreX64.EffectMirrorDown(srcPixels, width, height);
        //    }
        //}

        /// <summary>
        /// Mirror right.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void MirrorRight(IntPtr srcPixels, int width, int height)
        {
            if (X86)
            {
                MFPCoreX86.EffectMirrorRight(srcPixels, width, height);
            }
            else
            {
                MFPCoreX64.EffectMirrorRight(srcPixels, width, height);
            }
        }


        /// <summary>
        /// Delegate EffectMonoNoiseFilter.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        private delegate void EffectMonoNoiseFilter(IntPtr srcPixels, int width, int height, int amount);

        /// <summary>
        /// Mono noise.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void MonoNoise(IntPtr srcPixels, int srcWidth, int srcHeight, int amount, bool smartMultithreading = true)
        {
            EffectMonoNoiseFilter CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectMonoNoise;
            }
            else
            {
                CALL = MFPCoreX64.EffectMonoNoise;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    amount);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            amount);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        amount);
                }
            }
        }

        /// <summary>
        /// Mosaic.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="size">The size.</param>
        public static void Mosaic(IntPtr srcPixels, int width, int height, int size)
        {
            if (X86)
            {
                MFPCoreX86.EffectMosaic(srcPixels, width, height, size);
            }
            else
            {
                MFPCoreX64.EffectMosaic(srcPixels, width, height, size);
            }
        }

        /// <summary>
        /// Posterize.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        public static void Posterize(IntPtr srcPixels, int width, int height, int amount)
        {
            if (X86)
            {
                MFPCoreX86.EffectPosterize(srcPixels, width, height, amount);
            }
            else
            {
                MFPCoreX64.EffectPosterize(srcPixels, width, height, amount);
            }
        }

        /// <summary>
        /// Delegate EffectRedDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private delegate void EffectRedDelegate(IntPtr srcPixels, int width, int height);

        /// <summary>
        /// Red.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Red(IntPtr srcPixels, int srcWidth, int srcHeight, bool smartMultithreading = true)
        {
            EffectRedDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectRed;
            }
            else
            {
                CALL = MFPCoreX64.EffectRed;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight);
                }
            }
        }

        /// <summary>
        /// Delegate EffectSaturationDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        private delegate void EffectSaturationDelegate(IntPtr srcPixels, int width, int height, int amount);

        /// <summary>
        /// Saturation.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Saturation(IntPtr srcPixels, int srcWidth, int srcHeight, int amount, bool smartMultithreading = true)
        {
            EffectSaturationDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectSaturation;
            }
            else
            {
                CALL = MFPCoreX64.EffectSaturation;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    amount);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            amount);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        amount);
                }
            }
        }

        /// <summary>
        /// Shake down.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="factor">The factor.</param>
        public static void ShakeDown(IntPtr srcPixels, int width, int height, int factor)
        {
            if (X86)
            {
                MFPCoreX86.EffectShakeDown(srcPixels, width, height, factor);
            }
            else
            {
                MFPCoreX64.EffectShakeDown(srcPixels, width, height, factor);
            }
        }

        /// <summary>
        /// Delegate EffectSolorizeDelegate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        private delegate void EffectSolorizeDelegate(IntPtr srcPixels, int width, int height, int amount);

        /// <summary>
        /// Solorize.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="smartMultithreading">if set to <c>true</c> use smart multithreading.</param>
        public static void Solorize(IntPtr srcPixels, int srcWidth, int srcHeight, int amount, bool smartMultithreading = true)
        {
            EffectSolorizeDelegate CALL;
            if (X86)
            {
                CALL = MFPCoreX86.EffectSolorize;
            }
            else
            {
                CALL = MFPCoreX64.EffectSolorize;
            }

            if (!smartMultithreading || (srcWidth * srcHeight < SMART_RES_LIMIT))
            {
                CALL(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    amount);
            }
            else
            {
                var threads = Math.Min(Environment.ProcessorCount, MAX_THREAD_COUNT);
                int partHeight = srcHeight / threads;
                partHeight = MathHelper.RoundToSpecial(partHeight, 4);

                Parallel.For(
                    0,
                    threads,
                    (k) =>
                    {
                        CALL(
                            srcPixels + (k * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                            srcWidth,
                            partHeight,
                            amount);
                    });

                if (partHeight * threads != srcHeight)
                {
                    int specPartHeight = srcHeight - (partHeight * threads);
                    CALL(
                        srcPixels + (threads * partHeight * ImageHelper.GetStrideRGB24(srcWidth)),
                        srcWidth,
                        specPartHeight,
                        amount);
                }
            }

            //if (X86)
            //{
            //    MFPCoreX86.EffectSolorize(srcPixels, width, height, amount);
            //}
            //else
            //{
            //    MFPCoreX64.EffectSolorize(srcPixels, width, height, amount);
            //}
        }

        /// <summary>
        /// Spray.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="amount">The amount.</param>
        public static void Spray(IntPtr srcPixels, int width, int height, int amount)
        {
            if (X86)
            {
                MFPCoreX86.EffectSpray(srcPixels, width, height, amount);
            }
            else
            {
                MFPCoreX64.EffectSpray(srcPixels, width, height, amount);
            }
        }

        /// <summary>
        /// CAVT deinterlace.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="temp">The temporary data.</param>
        /// <param name="threshold">The threshold.</param>
        public static void DeinterlaceCAVT(IntPtr srcPixels, int width, int height, IntPtr temp, int threshold)
        {
            if (X86)
            {
                MFPCoreX86.EffectDeinterlaceCAVT(srcPixels, width, height, temp, threshold);
            }
            else
            {
                MFPCoreX64.EffectDeinterlaceCAVT(srcPixels, width, height, temp, threshold);
            }
        }

        /// <summary>
        /// Triangle deinterlace.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="temp">The temporary data.</param>
        /// <param name="weight">The weight.</param>
        public static void DeinterlaceTriangle(IntPtr srcPixels, int width, int height, IntPtr temp, int weight)
        {
            if (X86)
            {
                MFPCoreX86.EffectDeinterlaceTriangle(srcPixels, width, height, temp, weight);
            }
            else
            {
                MFPCoreX64.EffectDeinterlaceTriangle(srcPixels, width, height, temp, weight);
            }
        }

        /// <summary>
        /// SNR denoise.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="temp">The temporary data.</param>
        /// <param name="threshold">The threshold.</param>
        public static void DenoiseSNR(IntPtr srcPixels, int width, int height, IntPtr temp, int threshold)
        {
            if (X86)
            {
                MFPCoreX86.EffectDenoiseSNR(srcPixels, width, height, temp, threshold);
            }
            else
            {
                MFPCoreX64.EffectDenoiseSNR(srcPixels, width, height, temp, threshold);
            }
        }

        /// <summary>
        /// Motion detection, build matrix.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="linesX">The lines x.</param>
        /// <param name="linesY">The lines y.</param>
        /// <param name="matrixRes">The matrix resource.</param>
        /// <param name="matrix">The matrix.</param>
        public static void MotionDetectionBuildMatrix(
            int width, int height, int linesX, int linesY, byte[] matrixRes, IntPtr matrix)
        {
            if (X86)
            {
                MFPCoreX86.MotionDetectionBuildMatrix(width, height, linesX, linesY, matrixRes, matrix);
            }
            else
            {
                MFPCoreX64.MotionDetectionBuildMatrix(width, height, linesX, linesY, matrixRes, matrix);
            }
        }

        /// <summary>
        /// Motion detection, compare images.
        /// </summary>
        /// <param name="pic1">The pic 1.</param>
        /// <param name="pic2">The pic 2.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="compareGreyscale">if set to <c>true</c> compare greyscale.</param>
        /// <param name="compareRed">if set to <c>true</c> compare red.</param>
        /// <param name="compareGreen">if set to <c>true</c> compare green.</param>
        /// <param name="compareBlue">if set to <c>true</c> compare blue.</param>
        /// <param name="numPixels">The number data.</param>
        /// <returns>System.Int32.</returns>
        public static int MotionDetectionCompareImages(
            IntPtr pic1,
            IntPtr pic2,
            IntPtr matrix,
            bool compareGreyscale,
            bool compareRed,
            bool compareGreen,
            bool compareBlue,
            int numPixels)
        {
            if (X86)
            {
                return MFPCoreX86.MotionDetectionCompareImages(
                    pic1, pic2, matrix, compareGreyscale, compareRed, compareGreen, compareBlue, numPixels);
            }
            else
            {
                return MFPCoreX64.MotionDetectionCompareImages(
                    pic1, pic2, matrix, compareGreyscale, compareRed, compareGreen, compareBlue, numPixels);
            }
        }

        /// <summary>
        /// Motion detection, highlight.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="numPixels">The number data.</param>
        /// <param name="color">The color.</param>
        /// <param name="chlThreshold">The CHL threshold.</param>
        public static void MotionDetectionHighlight(
            IntPtr frame, IntPtr matrix, int numPixels, int color, int chlThreshold)
        {
            if (X86)
            {
                MFPCoreX86.MotionDetectionHighlight(frame, matrix, numPixels, color, chlThreshold);
            }
            else
            {
                MFPCoreX64.MotionDetectionHighlight(frame, matrix, numPixels, color, chlThreshold);
            }
        }

        /// <summary>
        /// Fade-in/out.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="stopTime">The stop time.</param>
        /// <param name="currentTime">The current time.</param>
        /// <param name="fadeIn">if set to <c>true</c> use fade-in.</param>
        public static void FadeInOut(
            IntPtr data,
            int width,
            int height,
            long startTime,
            long stopTime,
            long currentTime,
            bool fadeIn)
        {
            if (X86)
            {
                MFPCoreX86.EffectFadeInOut(data, width, height, startTime, stopTime, currentTime, fadeIn);
            }
            else
            {
                MFPCoreX64.EffectFadeInOut(data, width, height, startTime, stopTime, currentTime, fadeIn);
            }
        }

        /// <summary>
        /// Rotate.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="tempPixels">The temporary data.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="stretch">if set to <c>true</c> stretch.</param>
        /// <param name="workingBuffer">The working buffer.</param>
        /// <param name="workingBufferSize">Size of the working buffer.</param>
        public static void RotateIn(IntPtr srcPixels, int width, int height, IntPtr tempPixels, double angle, bool stretch, IntPtr workingBuffer, ref int workingBufferSize)
        {
            if (X86)
            {
                MFPCoreX86.RotateIn(srcPixels, width, height, tempPixels, angle, stretch, workingBuffer, ref workingBufferSize);
            }
            else
            {
                MFPCoreX64.RotateIn(srcPixels, width, height, tempPixels, angle, stretch, workingBuffer, ref workingBufferSize);
            }
        }

        //public static void ColorKeyRGB24(
        //    IntPtr pSrcChroma, int srcChromaStride, IntPtr pSrcBg, int srcBgStride, IntPtr pDest, int destStride, int width, int height, int color)
        //{
        //    if (X86)
        //    {
        //        MFPCoreX86.ColorKeyRGB24(pSrcChroma, srcChromaStride, pSrcBg, srcBgStride, pDest, destStride, width, height, color);
        //    }
        //    else
        //    {
        //        MFPCoreX64.ColorKeyRGB24(pSrcChroma, srcChromaStride, pSrcBg, srcBgStride, pDest, destStride, width, height, color);
        //    }
        //}

        /// <summary>
        /// Decode JPEG file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="output">The output.</param>
        /// <param name="outputSize">Size of the output.</param>
        /// <param name="bgr">if set to <c>true</c> use BGR.</param>
        /// <returns>System.Int32.</returns>
        public static int JPEGFileDecodeToRGB(
            string source,
            IntPtr output,
            int outputSize,
            bool bgr)
        {
            if (X86)
            {
                return MFPCoreX86.JPEGFileDecodeToRGB(source, output, outputSize, bgr);
            }
            else
            {
                return MFPCoreX64.JPEGFileDecodeToRGB(source, output, outputSize, bgr);
            }
        }

        /// <summary>
        /// Decode JPEG data to RGB.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceSize">Size of the source.</param>
        /// <param name="output">The output.</param>
        /// <param name="outputSize">Size of the output.</param>
        /// <param name="bgr">if set to <c>true</c> use BGR.</param>
        /// <returns>System.Int32.</returns>
        public static int JPEGDataDecodeToRGB(
            IntPtr source,
            int sourceSize,
            IntPtr output,
            int outputSize,
            [MarshalAs(UnmanagedType.Bool)] bool bgr)
        {
            if (X86)
            {
                return MFPCoreX86.JPEGDataDecodeToRGB(source, sourceSize, output, outputSize, bgr);
            }
            else
            {
                return MFPCoreX64.JPEGDataDecodeToRGB(source, sourceSize, output, outputSize, bgr);
            }
        }

        /// <summary>
        /// Encode RGB data to JPEG file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="source">The source.</param>
        /// <param name="sourceSize">Size of the source.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bgr">if set to <c>true</c> use BGR.</param>
        /// <returns>System.Int32.</returns>
        public static int JPEGFileEncodeFromRGB(
            [MarshalAs(UnmanagedType.LPWStr)] string filename,
            int quality,
            IntPtr source,
            int sourceSize,
            int width,
            int height,
            bool bgr)
        {
            if (X86)
            {
                return MFPCoreX86.JPEGFileEncodeFromRGB(filename, quality, source, sourceSize, width, height, bgr);
            }
            else
            {
                return MFPCoreX64.JPEGFileEncodeFromRGB(filename, quality, source, sourceSize, width, height, bgr);
            }
        }

        /// <summary>
        /// Encode RGB data to JPEG data.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceSize">Size of the source.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="output">The output.</param>
        /// <param name="outputSize">Size of the output.</param>
        /// <param name="bgr">if set to <c>true</c> use BGR.</param>
        /// <returns>System.Int32.</returns>
        public static int JPEGDataEncodeFromRGB(
            IntPtr source,
            int sourceSize,
            int quality,
            int width,
            int height,
            IntPtr output,
            out int outputSize,
            bool bgr)
        {
            if (X86)
            {
                return MFPCoreX86.JPEGDataEncodeFromRGB(source, sourceSize, quality, width, height, output, out outputSize, bgr);
            }
            else
            {
                return MFPCoreX64.JPEGDataEncodeFromRGB(source, sourceSize, quality, width, height, output, out outputSize, bgr);
            }
        }

        /// <summary>
        /// Horizontal flip, RGB24.
        /// </summary>
        /// <param name="pixels">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void FlipHorizontalRGB24(IntPtr pixels, int width, int height)
        {
            if (X86)
            {
                MFPCoreX86.FlipHorizontalRGB24(pixels, width, height);
            }
            else
            {
                MFPCoreX64.FlipHorizontalRGB24(pixels, width, height);
            }
        }

        /// <summary>
        /// Horizontal flip, RGB32.
        /// </summary>
        /// <param name="pixels">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void FlipHorizontalRGB32(IntPtr pixels, int width, int height)
        {
            if (X86)
            {
                MFPCoreX86.FlipHorizontalRGB32(pixels, width, height);
            }
            else
            {
                MFPCoreX64.FlipHorizontalRGB32(pixels, width, height);
            }
        }

        /// <summary>
        /// Vertical flip, RGB24.
        /// </summary>
        /// <param name="pixels">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void FlipVerticalRGB24(IntPtr pixels, int width, int height)
        {
            if (X86)
            {
                MFPCoreX86.FlipVerticalRGB24(pixels, width, height);
            }
            else
            {
                MFPCoreX64.FlipVerticalRGB24(pixels, width, height);
            }
        }

        /// <summary>
        /// Vertical flip, RGB32.
        /// </summary>
        /// <param name="pixels">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void FlipVerticalRGB32(IntPtr pixels, int width, int height)
        {
            if (X86)
            {
                MFPCoreX86.FlipVerticalRGB32(pixels, width, height);
            }
            else
            {
                MFPCoreX64.FlipVerticalRGB32(pixels, width, height);
            }
        }

        /// <summary>
        /// Greyscale.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="temp">The temporary data.</param>
        public static void Greyscale(IntPtr srcPixels, int srcWidth, int srcHeight, IntPtr temp)
        {
            // MT is not required
            if (X86)
            {
                MFPCoreX86.EffectGreyscale(
                       srcPixels,
                       srcWidth,
                       srcHeight,
                       temp);
            }
            else
            {
                MFPCoreX64.EffectGreyscale(
                    srcPixels,
                    srcWidth,
                    srcHeight,
                    temp);
            }
        }

        /// <summary>
        /// Mirror down.
        /// </summary>
        /// <param name="srcPixels">The source data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="tempData">The temporary data.</param>
        public static void MirrorDown(IntPtr srcPixels, int width, int height, IntPtr tempData)
        {
            if (X86)
            {
                MFPCoreX86.EffectMirrorDownEx(srcPixels, width, height, tempData);
            }
            else
            {
                MFPCoreX64.EffectMirrorDownEx(srcPixels, width, height, tempData);
            }
        }
    }
}
