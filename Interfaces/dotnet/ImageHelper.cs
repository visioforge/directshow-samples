namespace VisioForge.DirectShowAPI
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    public static class ImageHelper
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern void CopyMemory(IntPtr pDest, IntPtr pSrc, int length);

        public static int MakeCOLORREF(byte r, byte g, byte b)
        {
            return (int)(((uint)r) | (((uint)g) << 8) | (((uint)b) << 16));
        }

        public static int MakeCOLORREF(Color color)
        {
            return (int)(((uint)color.R) | (((uint)color.G) << 8) | (((uint)color.B) << 16));
        }

        public static int GetStrideRGB(int width, PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return ((width * 3) - 1) / 4 * 4 + 4;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return ((width * 4) - 1) / 4 * 4 + 4;
            }

            return 0;
        }

        public static int GetStrideRGB24(int width)
        {
            int stride = ((width * 3) - 1) / 4 * 4 + 4;
            return stride;
        }

        public static int GetStrideRGB32(int width)
        {
            int stride = ((width * 4) - 1) / 4 * 4 + 4;
            return stride;
        }

        public static int GetStrideYUY2(int width)
        {
            int stride = width * 2;
            return stride;
        }

        public static Bitmap Crop(Bitmap source, Rectangle cropRect)
        {
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }

            return target;
        }

        public static void BitmapToIntPtr(Bitmap sourceBmp, IntPtr destArray)
        {
            BitmapData bitmapData = new BitmapData();
            Rectangle rect = new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height);

            if (destArray == IntPtr.Zero)
            {
                throw new Exception("destArray is NULL, you must allocate memory before usage!");
            }

            if (sourceBmp.PixelFormat == PixelFormat.Format24bppRgb)
            {
                sourceBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb, bitmapData);

                IntPtr src = bitmapData.Scan0;

                int strideSrc = bitmapData.Stride;
                int strideDest = GetStrideRGB24(sourceBmp.Width);

                for (int i = 0; i < sourceBmp.Height; i++)
                {
                    IntPtr tmpDest = new IntPtr(destArray.ToInt64() + (strideDest * i));
                    IntPtr tmpSrc = new IntPtr(src.ToInt64() + (strideSrc * i));

                    CopyMemory(tmpDest, tmpSrc, strideDest);
                }

                sourceBmp.UnlockBits(bitmapData);
                // ReSharper disable once RedundantAssignment
                bitmapData = null;
            }
            else if (sourceBmp.PixelFormat == PixelFormat.Format32bppArgb) 
            {
                sourceBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb, bitmapData);

                IntPtr src = bitmapData.Scan0;

                int strideSrc = bitmapData.Stride;
                int strideDest = GetStrideRGB32(sourceBmp.Width);

                for (int i = 0; i < sourceBmp.Height; i++)
                {
                    IntPtr tmpDest = new IntPtr(destArray.ToInt64() + (strideDest * i));
                    IntPtr tmpSrc = new IntPtr(src.ToInt64() + (strideSrc * i));

                    CopyMemory(tmpDest, tmpSrc, strideDest);
                }

                sourceBmp.UnlockBits(bitmapData);
                // ReSharper disable once RedundantAssignment
                bitmapData = null;
            }
        }

        public static void BitmapToIntPtr(
            Bitmap sourceBmp, IntPtr destArray, int width, int height, PixelFormat pixelFormat)
        {
            if (destArray == IntPtr.Zero)
            {
                throw new Exception("destArray is NULL, you must allocate memory before usage!");
            }

            BitmapData bitmapData = new BitmapData();
            Rectangle rect = new Rectangle(0, 0, width, height);

            Bitmap bitmap;
            bool disposeBitmap;

            if (sourceBmp == null)
            {
                return;
            }

            if (width != sourceBmp.Width || height != sourceBmp.Height)
            {
                bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(sourceBmp, new Rectangle(Point.Empty, bitmap.Size));
                }

                disposeBitmap = true;
            }
            else
            {
                bitmap = sourceBmp;

                disposeBitmap = false;
            }

            if (pixelFormat == PixelFormat.Format24bppRgb)
            {
                bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb, bitmapData);

                IntPtr src = bitmapData.Scan0;

                int strideSrc = bitmapData.Stride;
                int strideDest = GetStrideRGB24(bitmap.Width);

                for (int i = 0; i < bitmap.Height; i++)
                {
                    IntPtr tmpDest = new IntPtr(destArray.ToInt64() + (strideDest * i));
                    IntPtr tmpSrc = new IntPtr(src.ToInt64() + (strideSrc * i));

                    CopyMemory(tmpDest, tmpSrc, strideDest);
                }

                bitmap.UnlockBits(bitmapData);
                // ReSharper disable once RedundantAssignment
                bitmapData = null;
            }
            else if (pixelFormat == PixelFormat.Format32bppArgb)
            {
                bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb, bitmapData);

                IntPtr src = bitmapData.Scan0;

                int strideSrc = bitmapData.Stride;
                int strideDest = GetStrideRGB32(bitmap.Width);

                for (int i = 0; i < bitmap.Height; i++)
                {
                    IntPtr tmpDest = new IntPtr(destArray.ToInt64() + (strideDest * i));
                    IntPtr tmpSrc = new IntPtr(src.ToInt64() + (strideSrc * i));

                    CopyMemory(tmpDest, tmpSrc, strideDest);
                }

                bitmap.UnlockBits(bitmapData);
                // ReSharper disable once RedundantAssignment
                bitmapData = null;
            }

            if (disposeBitmap)
            {
                bitmap.Dispose();
            }
        }

        /// <summary>
        /// Gets encoder info.
        /// </summary>
        /// <param name="mimeType">
        /// Mime type.
        /// </param>
        /// <returns>
        /// Returns image info structure.
        /// </returns>
        [Localizable(false)]
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                {
                    return encoders[j];
                }
            }

            return null;
        }

        ///// <summary>
        ///// Converts IntPtr to Bitmap.
        ///// </summary>
        ///// <param name="source">
        ///// Source.
        ///// </param>
        ///// <param name="width">
        ///// Width.
        ///// </param>
        ///// <param name="height">
        ///// Height.
        ///// </param>
        ///// <param name="pixelFormat">
        ///// Pixel format.
        ///// </param>
        ///// <param name="outputBitmap">
        ///// Output bitmap.
        ///// </param>
        ///// <param name="horizontalFlip">
        ///// Horizontal flip.
        ///// </param>
        //public static void IntPtrToBitmap(IntPtr source, int width, int height, PixelFormat pixelFormat, ref Bitmap outputBitmap, bool horizontalFlip = false)
        //{
        //    if (pixelFormat != PixelFormat.Format24bppRgb && pixelFormat != PixelFormat.Format32bppArgb)
        //    {
        //        return;
        //    }

        //    if (outputBitmap != null)
        //    {
        //        outputBitmap.Dispose();
        //        // ReSharper disable RedundantAssignment
        //        outputBitmap = null;
        //        // ReSharper restore RedundantAssignment
        //    }

        //    int depth = 0;
        //    switch (pixelFormat)
        //    {
        //        case PixelFormat.Format24bppRgb:
        //            depth = 3;
        //            break;
        //        case PixelFormat.Format32bppArgb:
        //            depth = 4;
        //            break;
        //    }

        //    int bufSize = width * height * depth;

        //    outputBitmap = new Bitmap(width, height);
        //    BitmapData bitmapData = outputBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);

        //    CopyMemory(bitmapData.Scan0, source, bufSize);

        //    if (horizontalFlip)
        //    {
        //        if (pixelFormat == PixelFormat.Format24bppRgb)
        //        {
        //            MFP.FlipHorizontalRGB24(bitmapData.Scan0, width, height);
        //        }
        //        else
        //        {
        //            MFP.FlipHorizontalRGB32(bitmapData.Scan0, width, height);
        //        }
        //    }

        //    outputBitmap.UnlockBits(bitmapData);
        //}

        /// <summary>
        /// Converts byte array to bitmap with horizontal flip.
        /// </summary>
        /// <param name="source">
        /// Source data.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="pixelFormat">
        /// Pixel format.
        /// </param>
        /// <param name="preDefinedOutput">
        /// Pre-defined output bitmap.
        /// </param>
        public static void ByteArrayToBitmapFlip(byte[] source, int width, int height, PixelFormat pixelFormat, ref Bitmap preDefinedOutput)
        {
            if (source == null || preDefinedOutput == null || preDefinedOutput.Width != width || preDefinedOutput.Height != height ||
                preDefinedOutput.PixelFormat != pixelFormat)
            {
                return;
            }

            BitmapData bitmapData = preDefinedOutput.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
            Marshal.Copy(source, 0, bitmapData.Scan0, source.Length);
            preDefinedOutput.UnlockBits(bitmapData);
            preDefinedOutput.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        /// <summary>
        /// Converts byte array to bitmap.
        /// </summary>
        /// <param name="source">
        /// Source data.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="pixelFormat">
        /// Pixel format.
        /// </param>
        /// <param name="preDefinedOutput">
        /// Pre-defined output bitmap.
        /// </param>
        /// <param name="flip">
        /// Flip.
        /// </param>
        public static void ByteArrayToBitmap(byte[] source, int width, int height, PixelFormat pixelFormat, ref Bitmap preDefinedOutput, bool flip = false)
        {
            if (source == null || preDefinedOutput == null || preDefinedOutput.Width != width || preDefinedOutput.Height != height ||
                preDefinedOutput.PixelFormat != pixelFormat)
            {
                return;
            }

            BitmapData bitmapData = preDefinedOutput.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
            Marshal.Copy(source, 0, bitmapData.Scan0, source.Length);
            preDefinedOutput.UnlockBits(bitmapData);

            if (flip)
            {
                preDefinedOutput.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
        }

        /// <summary>
        /// Converts byte array to Jpeg.
        /// </summary>
        /// <param name="source">
        /// Source data.
        /// </param>
        /// <param name="sourceLen">
        /// Source length.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="pixelFormat">
        /// Pixel format.
        /// </param>
        /// <param name="jpeg">
        /// JPEG byte array.
        /// </param>
        /// <param name="flip">
        /// Flip.
        /// </param>
        public static void IntPtrToJpeg(IntPtr source, int sourceLen, int width, int height, PixelFormat pixelFormat, out byte[] jpeg, bool flip = false)
        {
            jpeg = null;

            if (source == IntPtr.Zero || sourceLen == 0)
            {
                return;
            }

            var imageTmp = new Bitmap(width, height, pixelFormat);

            BitmapData bitmapData = imageTmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
            CopyMemory(bitmapData.Scan0, source, sourceLen);
            imageTmp.UnlockBits(bitmapData);

            if (flip)
            {
                imageTmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            using (var ms = new MemoryStream())
            {
                imageTmp.Save(ms, ImageFormat.Jpeg);
                jpeg = ms.ToArray();
            }

            imageTmp.Dispose();
            imageTmp = null;
        }

        /// <summary>
        /// Converts byte array to Jpeg.
        /// </summary>
        /// <param name="source">
        /// Source data.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="pixelFormat">
        /// Pixel format.
        /// </param>
        /// <param name="jpeg">
        /// JPEG byte array.
        /// </param>
        /// <param name="flip">
        /// Flip.
        /// </param>
        public static void ByteArrayToJpeg(byte[] source, int width, int height, PixelFormat pixelFormat, out byte[] jpeg, bool flip = false)
        {
            jpeg = null;
            
            if (source == null)
            {
                return;
            }

            var imageTmp = new Bitmap(width, height, pixelFormat);

            BitmapData bitmapData = imageTmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
            Marshal.Copy(source, 0, bitmapData.Scan0, source.Length);
            imageTmp.UnlockBits(bitmapData);

            if (flip)
            {
                imageTmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            using (var ms = new MemoryStream())
            {
                imageTmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                jpeg = ms.ToArray();
            }

            imageTmp.Dispose();
            imageTmp = null;
        }

        /// <summary>
        /// Converts Jpeg to byte array.
        /// </summary>
        /// <param name="jpeg">
        /// JPEG byte array.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="pixelFormat">
        /// Pixel format.
        /// </param>
        /// <param name="output">
        /// Output data.
        /// </param>
        /// <param name="outputSize">
        /// Output size.
        /// </param>
        /// <param name="flip">
        /// Flip.
        /// </param>
        public static void JpegToByteArray(byte[] jpeg, int width, int height, PixelFormat pixelFormat, out IntPtr output, out int outputSize, bool flip = false)
        {
            output = IntPtr.Zero;
            outputSize = 0;

            if (jpeg == null)
            {
                return;
            }

            Bitmap imageTmp;

            using (var ms = new MemoryStream())
            {
                ms.Write(jpeg, 0, jpeg.Length);
                imageTmp = new Bitmap(ms);

                if (flip)
                {
                    imageTmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
            }

            BitmapData bitmapData = imageTmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);

            outputSize = bitmapData.Stride * bitmapData.Height;
            output = Marshal.AllocCoTaskMem(outputSize);
            CopyMemory(output, bitmapData.Scan0, outputSize);

            imageTmp.UnlockBits(bitmapData);
            
            imageTmp.Dispose();
            imageTmp = null;
        }

        /// <summary>
        /// Converts Jpeg to byte array.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="sourceSize">
        /// The source Size.
        /// </param>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="pixelFormat">
        /// Pixel format.
        /// </param>
        /// <param name="output">
        /// Output data.
        /// </param>
        /// <param name="outputSize">
        /// Output size.
        /// </param>
        /// <param name="flip">
        /// Flip.
        /// </param>
        public static void JpegToByteArray(IntPtr source, int sourceSize, int width, int height, PixelFormat pixelFormat, out IntPtr output, out int outputSize, bool flip = false)
        {
            output = IntPtr.Zero;
            outputSize = 0;

            if (source == IntPtr.Zero)
            {
                return;
            }

            Bitmap imageTmp;

            byte[] jpeg = new byte[sourceSize];
            Marshal.Copy(source, jpeg, 0, sourceSize);

            using (var ms = new MemoryStream())
            {
                ms.Write(jpeg, 0, jpeg.Length);
                imageTmp = new Bitmap(ms);

                if (flip)
                {
                    imageTmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
            }

            jpeg = null;

            BitmapData bitmapData = imageTmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);

            outputSize = bitmapData.Stride * bitmapData.Height;
            output = Marshal.AllocCoTaskMem(outputSize);
            CopyMemory(output, bitmapData.Scan0, outputSize);

            imageTmp.UnlockBits(bitmapData);
            
            imageTmp.Dispose();
            imageTmp = null;
        }

        public static void ByteArrayToBitmap(IntPtr srcArray, int srcWidth, int srcHeight, PixelFormat pixelFormat, ref Bitmap destBmp)
        {
            if (srcArray == IntPtr.Zero || destBmp == null || destBmp.Width != srcWidth || destBmp.Height != srcHeight || destBmp.PixelFormat != pixelFormat)
            {
                return;
            }

            BitmapData bitmapData = new BitmapData();
            Rectangle rect = new Rectangle(0, 0, srcWidth, srcHeight);

            destBmp.LockBits(rect, ImageLockMode.WriteOnly, pixelFormat, bitmapData);

            IntPtr pixels2 = bitmapData.Scan0;

            int srcStride = 0;
            if (pixelFormat == PixelFormat.Format32bppArgb)
            {
                srcStride = GetStrideRGB32(srcWidth);
            }
            else if (pixelFormat == PixelFormat.Format24bppRgb)
            {
                srcStride = GetStrideRGB24(srcWidth);
            }

            CopyMemory(pixels2, srcArray, srcStride * srcHeight);

            destBmp.UnlockBits(bitmapData);
            bitmapData = null;
        }

        public static void BitmapToIntPtrFlip(Bitmap sourceBitmap, int width, int height, PixelFormat pixelFormat, IntPtr destPtr, int destLen)
        {
            if (destPtr == IntPtr.Zero || sourceBitmap == null || sourceBitmap.Width != width || sourceBitmap.Height != height ||
                sourceBitmap.PixelFormat != pixelFormat)
            {
                return;
            }

            sourceBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            BitmapData bitmapData = sourceBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, pixelFormat);
            CopyMemory(destPtr, bitmapData.Scan0, destLen);
            sourceBitmap.UnlockBits(bitmapData);

            sourceBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">
        /// The image to resize.
        /// </param>
        /// <param name="width">
        /// The width to resize to.
        /// </param>
        /// <param name="height">
        /// The height to resize to.
        /// </param>
        /// <param name="fast">
        /// Fast but low quality resize.
        /// </param>
        /// <returns>
        /// The resized image.
        /// </returns>
        public static Bitmap ResizeImage(this Image image, int width, int height, bool fast = false)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                if (fast)
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                }
                else
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                }
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            image.Dispose();

            return destImage;
        }

        public static Rectangle RectangleConv(this RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}
