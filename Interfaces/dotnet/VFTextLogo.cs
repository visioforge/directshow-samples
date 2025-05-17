// ***********************************************************************
// Assembly         : VisioForge.ProcessingFilters.Interfaces
// Author           : roman
// Created          : 12-21-2021
//
// Last Modified By : roman
// Last Modified On : 12-21-2021
// ***********************************************************************
// <copyright file="VFTextLogo.cs" company="VisioForge">
// Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowAPI
{
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Text logo.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VFTextLogo
    {
        /// <summary>
        /// X.
        /// </summary>
        public int X;

        /// <summary>
        /// Y.
        /// </summary>
        public int Y;

        /// <summary>
        /// Transparent background.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool TransparentBg;

        /// <summary>
        /// Font size.
        /// </summary>
        public int FontSize;

        /// <summary>
        /// Use italic font.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool FontItalic;

        /// <summary>
        /// Use bold font.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool FontBold;

        /// <summary>
        /// Use underline font.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool FontUnderline;

        /// <summary>
        /// Use strikeout font.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool FontStrikeout;

        /// <summary>
        /// Font color.
        /// </summary>
        public int FontColor;

        /// <summary>
        /// Background color.
        /// </summary>
        public int BGColor;

        /// <summary>
        /// Text is right-to-left.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool RightToLeft;

        /// <summary>
        /// Text located vertically.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool Vertical;

        /// <summary>
        /// Align.
        /// </summary>
        public int Align;

        /// <summary>
        /// Draw quality.
        /// </summary>
        public int DrawQuality;

        /// <summary>
        /// Antialiasing.
        /// </summary>
        public int Antialiasing;

        /// <summary>
        /// Rectangle width.
        /// </summary>
        public int RectWidth;

        /// <summary>
        /// Rectangle height.
        /// </summary>
        public int RectHeight;

        /// <summary>
        /// Rotation mode.
        /// </summary>
        public int RotationMode;

        /// <summary>
        /// Flip mode.
        /// </summary>
        public int FlipMode;

        /// <summary>
        /// Transparency level.
        /// </summary>
        public int Transp;

        /// <summary>
        /// Use gradient.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool Gradient;

        /// <summary>
        /// Gradient mode.
        /// </summary>
        public int GradientMode;

        /// <summary>
        /// Gradient color 1.
        /// </summary>
        public int GradientColor1;

        /// <summary>
        /// Gradient color 2.
        /// </summary>
        public int GradientColor2;

        /// <summary>
        /// Border mode.
        /// </summary>
        public int BorderMode;

        /// <summary>
        /// Inner border color.
        /// </summary>
        public int InnerBorderColor;

        /// <summary>
        /// Outer border color.
        /// </summary>
        public int OuterBorderColor;

        /// <summary>
        /// Inner border size.
        /// </summary>
        public int InnerBorderSize;

        /// <summary>
        /// Outer border size.
        /// </summary>
        public int OuterBorderSize;

        /// <summary>
        /// Use background shape.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool BGShape;

        /// <summary>
        /// Background shape type.
        /// </summary>
        public int BGShapeType;

        /// <summary>
        /// Background shape X.
        /// </summary>
        public int BGShapeX;

        /// <summary>
        /// Background shape Y.
        /// </summary>
        public int BGShapeY;

        /// <summary>
        /// Background shape width.
        /// </summary>
        public int BGShapeWidth;

        /// <summary>
        /// Background shape height.
        /// </summary>
        public int BGShapeHeight;

        /// <summary>
        /// Background shape color.
        /// </summary>
        public int BGShapeColor;

        /// <summary>
        /// Text.
        /// </summary>
        [Localizable(false)]
        [MarshalAs(UnmanagedType.BStr)]
        public string Text;

        /// <summary>
        /// Font name.
        /// </summary>
        [Localizable(false)]
        [MarshalAs(UnmanagedType.BStr)]
        public string FontName;

        /// <summary>
        /// Use Date mode.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool DateMode;

        /// <summary>
        /// Date mask.
        /// </summary>
        [Localizable(false)]
        [MarshalAs(UnmanagedType.BStr)]
        public string DateMask;
    }
}