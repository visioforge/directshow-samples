using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VisioForge.DirectShowAPI;

namespace Main_Demo
{
    public class BitmapFrame
    {
        public Bitmap Image { get; private set; }

        public long StartTime { get; private set; }

        public long StopTime { get; private set; }

        public BitmapFrame(Bitmap image, long startTime, long stopTime)
        {
            Image = image;
            StartTime = startTime;
            StopTime = stopTime;
        }
    }

    public class FrameCache
    {
        private readonly int _width;

        private readonly int _height;

        private readonly ConcurrentQueue<BitmapFrame> _bitmaps;

        private IntPtr _currentFrame;

        private int _currentFrameSize;

        private long _currentFrameStartTime;

        private long _currentFrameStopTime;

        public FrameCache(int width, int height)
        {
            _width = width;
            _height = height;

            _bitmaps = new ConcurrentQueue<BitmapFrame>();
            _currentFrame = IntPtr.Zero;
            _currentFrameStartTime = 0;
            _currentFrameStopTime = 0;
        }

        public void Add(Bitmap bmp, long startTime, long stopTime)
        {
            if (bmp.Width == _width && bmp.Height == _height && bmp.PixelFormat == PixelFormat.Format24bppRgb)
            {
                _bitmaps.Enqueue(new BitmapFrame((Bitmap)bmp.Clone(), startTime, stopTime));
            }
            else
            {
                _bitmaps.Enqueue(new BitmapFrame(ResizeWithAspectRatio(bmp, _width, _height), startTime, stopTime));
            }
        }

        private static Bitmap ResizeWithAspectRatio(Bitmap image, int width, int height)
        {
            var brush = new SolidBrush(Color.Black);

            float scale = Math.Min((float)width / image.Width, (float)height / image.Height);

            var bmp = new Bitmap(width, height);
            var graph = Graphics.FromImage(bmp);

            graph.InterpolationMode = InterpolationMode.High;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
            graph.DrawImage(image, (width - scaleWidth) / 2, (height - scaleHeight) / 2, scaleWidth, scaleHeight);

            return bmp;
        }

        private void GetFrame()
        {
            bool res = _bitmaps.TryDequeue(out var bmp);

            if (res)
            {
                _currentFrameStartTime = bmp.StartTime;
                _currentFrameStopTime = bmp.StopTime;
                _currentFrameSize = ImageHelper.GetStrideRGB24(bmp.Image.Width) * bmp.Image.Height;
                _currentFrame = Marshal.AllocCoTaskMem(_currentFrameSize);
                ImageHelper.BitmapToIntPtr(bmp.Image, _currentFrame, bmp.Image.Width, bmp.Image.Height, PixelFormat.Format24bppRgb);

                FastImageProcessing.FlipHorizontalRGB24(_currentFrame, bmp.Image.Width, bmp.Image.Height);
            }
        }

        public IntPtr GetFrame(long timestamp)
        {
            if (_currentFrame == IntPtr.Zero)
            {
                GetFrame();
            }

            if (timestamp > _currentFrameStopTime)
            {
                if (_currentFrame != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(_currentFrame);
                    _currentFrame = IntPtr.Zero;
                }

                GetFrame();
            }

            return _currentFrame;
        }
    }
}
