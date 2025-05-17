namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using DirectShowLib;

    /// <summary>
    /// Class VideoMixer.
    /// Implements the <see cref="System.IDisposable" />.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "None.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:Field names should not begin with underscore", Justification = "None.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "None.")]
    public class VideoMixer : IDisposable
    {
        /// <summary>
        /// The filter.
        /// </summary>
        private IBaseFilter _filter;

        /// <summary>
        /// Filter interface.
        /// </summary>
        private IVFVideoMixer _intf;

        /// <summary>
        /// Gets filter interface.
        /// </summary>
        /// <value>The intf.</value>
        public IVFVideoMixer Intf
        {
            get
            {
                return _intf;
            }
        }

        /// <summary>
        /// Gets a value indicating whether filter interface is available.
        /// </summary>
        /// <value><c>true</c> if [intf available]; otherwise, <c>false</c>.</value>
        public bool IntfAvailable { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoMixer" /> class.
        /// </summary>
        public VideoMixer()
        {
        }

        #region IDisposable

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Dispose.
        /// </summary>
#pragma warning disable S4136 // Method overloads should be grouped together
        public void Dispose()
#pragma warning restore S4136 // Method overloads should be grouped together
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Disposing parameter.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        // Free other state (managed objects).
                    }

                    Clear();

                    // Free your own state (unmanaged objects).
                    // Set large fields to null.
                    disposed = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, e);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="VideoMixer" /> class.
        /// </summary>
        ~VideoMixer()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <returns>IBaseFilter.</returns>
        public IBaseFilter GetFilter()
        {
            return _filter;
        }

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            if (_intf != null)
            {
                Marshal.ReleaseComObject(_intf);
                _intf = null;
            }

            try
            {
                if (_filter != null)
                {
                    Marshal.FinalReleaseComObject(_filter);
                    _filter = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to release filter. " + e.Message, e);
            }

            IntfAvailable = false;
        }

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="filterGraph">Filter graph.</param>
        /// <returns>Returns true or 0 if the operation was successful.</returns>
        public bool Init(IFilterGraph2 filterGraph)
        {
            _filter = DSHelper.AddFilterFromClsid(filterGraph, Consts.CLSID_VFVideoMixer, "Video mixer");

            return _filter != null;
        }

        /// <summary>
        /// Init interface.
        /// </summary>
        /// <returns>Returns true or 0 if the operation was successful.</returns>
        public bool InitIntf()
        {
            IntfAvailable = false;

            _intf = _filter as IVFVideoMixer;
            if (_intf != null)
            {
                IntfAvailable = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets position.
        /// </summary>
        /// <param name="index">Device index.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void SetLayerPosition(int index, int x, int y, int width, int height)
        {
            if (_intf == null)
            {
                return;
            }

            Intf.GetInputParam(index, out var param);

            param.X = x;
            param.Y = y;
            param.Width = width;
            param.Height = height;

            Intf.SetInputParam(index, param);
        }

        /// <summary>
        /// Sets layer settings.
        /// </summary>
        /// <param name="index">Device index.</param>
        /// <param name="transparency">Transparency level.</param>
        /// <param name="flipX">True to flip horizontally.</param>
        /// <param name="flipY">True to flip vertically.</param>
        /// <param name="disabled">True to disable stream.</param>
        public void SetLayerSettings(int index, int transparency, bool flipX, bool flipY, bool disabled)
        {
            if (_intf == null)
            {
                return;
            }

            Intf.GetInputParam(index, out var param);

            param.Alpha = transparency;
            param.FlipX = flipX;
            param.FlipY = flipY;
            param.Disabled = disabled;

            Intf.SetInputParam(index, param);
        }

        /// <summary>
        /// Sets layer settings.
        /// </summary>
        /// <param name="index">Device index.</param>
        /// <param name="order">Stream order. From 0 to streams count - 1.</param>
        public void SetInputOrder(int index, int order)
        {
            VFPIPVideoInputParam param;

            if (_intf == null)
            {
                return;
            }

            Intf.GetInputParam(index, out param);

            param.OrderID = order;

            Intf.SetInputParam(index, param);
        }

        /// <summary>
        /// Sets output settings.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="color">Background color.</param>
        /// <param name="imageFilename">Background image file name.</param>
        /// <param name="frameRate">Frame rate.</param>
        /// <param name="resizeQuality">Resize quality.</param>
        public void SetOutputSettings(int width, int height, Color color, string imageFilename, double frameRate, VFPIPResizeQuality resizeQuality)
        {
            var param = new VFPIPVideoOutputParam();

            if (_intf == null)
            {
                return;
            }

            param.Width = width;
            param.Height = height;
            param.Backcolor = color.ToArgb();
            param.FrameRateTime = (int)(10000000.0 / frameRate);
            param.Backimage = imageFilename;

            if (param.Backimage == null)
            {
                param.Backimage = string.Empty;
            }

            Intf.SetOutputParam(param);
            Intf.SetResizeQuality(resizeQuality);
        }

        /// <summary>
        /// Sets chroma mode.
        /// </summary>
        /// <param name="enabled">The chroma enabled.</param>
        /// <param name="color">The color.</param>
        /// <param name="tolerance1">The tolerance 1.</param>
        /// <param name="tolerance2">The tolerance 2.</param>
        public void SetChroma(bool enabled, Color color, int tolerance1, int tolerance2)
        {
            if (_intf == null)
            {
                return;
            }

            var colorx = color.ToArgb();
            _intf.SetChromaSettings(enabled, colorx, tolerance1, tolerance2);
        }
    }
}
