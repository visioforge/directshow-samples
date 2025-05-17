// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-22-2021
//
// Last Modified By : roman
// Last Modified On : 12-22-2021
// ***********************************************************************
// <copyright file="VideoRendererEVR.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using DirectShowLib;
    using MediaFoundation;
    using MediaFoundation.EVR;
    using MediaFoundation.Misc;

    /// <summary>
    /// Class VideoRendererEVR.
    /// Implements the <see cref="System.IDisposable" />.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "None.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:Field names should not begin with underscore", Justification = "None.")]
    public class VideoRendererEVR : IDisposable
    {
        /// <summary>
        /// The filter.
        /// </summary>
        private IBaseFilter _filter;

        /// <summary>
        /// MF video mixer control.
        /// </summary>
        private IMFVideoMixerControl2 dsMFVideoMixerControl;

        /// <summary>
        /// MF video processor interface.
        /// </summary>
        private IMFVideoProcessor dsMFVideoProcessor;

        /// <summary>
        /// MF video display control interface.
        /// </summary>
        private IMFVideoDisplayControl dsMFVideoDisplayControl;

        /// <summary>
        /// Gets or sets background color.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoRendererEVR"/> is letterbox.
        /// </summary>
        /// <value><c>true</c> if letterbox; otherwise, <c>false</c>.</value>
        public bool Letterbox { get; set; } = true;

        /// <summary>
        /// Gets or sets the screen handle.
        /// </summary>
        /// <value>The screen handle.</value>
        public IntPtr ScreenHandle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoRendererEVR" /> class.
        /// </summary>
        public VideoRendererEVR()
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
        public void Dispose()
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
        /// Finalizes an instance of the <see cref="VideoRendererEVR" /> class.
        /// </summary>
        ~VideoRendererEVR()
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
        private void Clear()
        {
            // EVR
            try
            {
                if (dsMFVideoDisplayControl != null)
                {
                    Marshal.ReleaseComObject(dsMFVideoDisplayControl);
                    dsMFVideoDisplayControl = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, e);
            }

            try
            {
                if (dsMFVideoMixerControl != null)
                {
                    Marshal.ReleaseComObject(dsMFVideoMixerControl);
                    dsMFVideoMixerControl = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, e);
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
                Debug.WriteLine(e.Message, e);
            }
        }

        /// <summary>
        /// Makes the colorref.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>System.Int32.</returns>
        private static int MakeCOLORREF(Color color)
        {
            return (int)(((uint)color.R) | (((uint)color.G) << 8) | (((uint)color.B) << 16));
        }

        /// <summary>
        /// Updates video renderer.
        /// </summary>
        /// <param name="filterGraph">Filter graph.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void Update(IFilterGraph2 filterGraph, int width, int height)
        {
            MFRect rectDest = new MFRect();
            MFVideoNormalizedRect rectSrc = new MFVideoNormalizedRect();

            try
            {
                if ((dsMFVideoDisplayControl != null) && (filterGraph != null))
                {
                    if (dsMFVideoProcessor != null)
                    {
                        dsMFVideoProcessor.SetBackgroundColor(MakeCOLORREF(BackgroundColor));
                    }

                    rectDest.left = 0;
                    rectDest.top = 0;
                    rectDest.right = width;
                    rectDest.bottom = height;

                    rectSrc.left = 0;
                    rectSrc.top = 0;
                    rectSrc.right = 1;
                    rectSrc.bottom = 1;

                    dsMFVideoDisplayControl.SetVideoPosition(rectSrc, rectDest);

                    if (!Letterbox)
                    {
                        dsMFVideoDisplayControl.SetAspectRatioMode(MFVideoAspectRatioMode.None);
                    }
                    else
                    {
                        dsMFVideoDisplayControl.SetAspectRatioMode(MFVideoAspectRatioMode.PreservePicture);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, e);
            }
        }

        /// <summary>
        /// Inits video renderer.
        /// </summary>
        /// <param name="filterGraph">Filter graph.</param>
        public void Init(IFilterGraph2 filterGraph)
        {
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
            Guid CLSID_EnhancedVideoRenderer = new Guid("FA10746C-9B63-4b6c-BC49-FC300EA5F256");
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
            _filter = DSHelper.AddFilterFromClsid(
                filterGraph,
                CLSID_EnhancedVideoRenderer,
                "EVR");

            if ((filterGraph == null) || (_filter == null))
            {
                return;
            }

            var pConfig = (IEVRFilterConfig)_filter;
            if (pConfig != null)
            {
                pConfig.SetNumberOfStreams(1);
            }
            else
            {
                Debug.WriteLine("Unable to query IEVRFilterConfig interface.");
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            var mfGetService = (MediaFoundation.IMFGetService)_filter;
            if (mfGetService == null)
            {
                Debug.WriteLine("Unable to query IMFGetService interface.");
                return;
            }

            mfGetService.GetService(
                MFServices.MR_VIDEO_RENDER_SERVICE,
                typeof(IMFVideoDisplayControl).GUID,
                out var videoDisplayControlObj);
            if (videoDisplayControlObj != null)
            {
                dsMFVideoDisplayControl = videoDisplayControlObj as IMFVideoDisplayControl;
                if (dsMFVideoDisplayControl != null && ScreenHandle != IntPtr.Zero)
                {
                    dsMFVideoDisplayControl.SetVideoWindow(ScreenHandle);
                }
            }
            else
            {
                Debug.WriteLine("Unable to query IMFVideoDisplayControl interface.");
            }

            mfGetService.GetService(
                MFServices.MR_VIDEO_MIXER_SERVICE,
                typeof(IMFVideoMixerControl).GUID,
                out var videoMixerControlObj);
            if (videoMixerControlObj != null)
            {
                dsMFVideoMixerControl = videoMixerControlObj as IMFVideoMixerControl2;
            }
            else
            {
                Debug.WriteLine("Unable to get EVR Video Mixer Control interface.");
            }

            mfGetService.GetService(MFServices.MR_VIDEO_MIXER_SERVICE, typeof(IMFVideoProcessor).GUID, out var videoProcessorObj);
            dsMFVideoProcessor = videoProcessorObj as IMFVideoProcessor;
        }
    }
}
