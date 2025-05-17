using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisioForge.DirectShowLib
{
    /// <summary>
    /// Deinterlace technology.
    /// </summary>
    public static class DeinterlaceTech9
    {
        // ReSharper disable RedundantCast
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// E_FAIL code.
        /// </summary>
        public const int E_FAIL = unchecked((int)0x80004005);

        /// <summary>
        /// the algorithm is unknown or proprietary.
        /// </summary>
        public const int Unknown = unchecked((int)0x0000);

        /// <summary>
        /// the algorithm creates the missing lines by repeating
        /// the line either above or below it - this method will look very jaggy and
        /// isn't recommended.
        /// </summary>
        public const int BOBLineReplicate = unchecked((int)0x0001);

        /// <summary>
        /// the algorithm creates the missing lines by vertically stretching each
        /// video field by a factor of two, for example by averaging two lines or
        /// using a [-1, 9, 9, -1]/16 filter across four lines.
        /// Slight vertical adjustments are made to ensure that the resulting image
        /// does not "bob" up and down.
        /// </summary>
        public const int BOBVerticalStretch = unchecked((int)0x0002);

        /// <summary>
        /// the pixels in the missing line are recreated by a median filtering operation.
        /// </summary>
        public const int MedianFiltering = unchecked((int)0x0004);

        /// <summary>
        /// the pixels in the missing line are recreated by an edge filter.
        /// In this process, spatial directional filters are applied to determine
        /// the orientation of edges in the picture content, and missing
        /// pixels are created by filtering along (rather than across) the
        /// detected edges.
        /// </summary>
        public const int EdgeFiltering = unchecked((int)0x0010);

        /// <summary>
        /// the pixels in the missing line are recreated by switching on a field by
        /// field basis between using either spatial or temporal interpolation
        /// depending on the amount of motion.
        /// </summary>
        public const int FieldAdaptive = unchecked((int)0x0020);

        /// <summary>
        /// the pixels in the missing line are recreated by switching on a pixel by pixel
        /// basis between using either spatial or temporal interpolation depending on
        /// the amount of motion.
        /// </summary>
        public const int PixelAdaptive = unchecked((int)0x0040);

        /// <summary>
        /// Motion Vector Steering  identifies objects within a sequence of video 
        /// fields.  The missing pixels are recreated after first aligning the
        /// movement axes of the individual objects in the scene to make them
        /// parallel with the time axis.
        /// </summary>
        public const int MotionVectorSteered = unchecked((int)0x0080);

        // ReSharper restore RedundantCast
        // ReSharper restore InconsistentNaming
    }

}
