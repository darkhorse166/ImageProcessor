// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JpegFormat.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Provides the necessary information to support jpeg images.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging.Formats
{
    using System.Drawing.Imaging;

    /// <summary>
    /// Provides the necessary information to support jpeg images.
    /// </summary>
    public class JpegFormat : FormatBase
    {
        /// <summary>
        /// Gets the file header.
        /// </summary>
        public override byte[] FileHeader
        {
            get
            {
                return new byte[] { 255, 216, 255, 224 };
            }
        }

        /// <summary>
        /// Gets the list of file extensions.
        /// </summary>
        public override string[] FileExtensions
        {
            get
            {
                return new[] { "jpeg", "jpg" };
            }
        }

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains. 
        /// </summary>
        public override string MimeType
        {
            get
            {
                return "image/jpeg";
            }
        }

        /// <summary>
        /// Gets the <see cref="ImageFormat" />.
        /// </summary>
        public override ImageFormat ImageFormat
        {
            get
            {
                return ImageFormat.Jpeg;
            }
        }
    }
}
