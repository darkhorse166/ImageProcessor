// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TiffFormat.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Provides the necessary information to support tiff images.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging.Formats
{
    using System.Drawing.Imaging;

    /// <summary>
    /// Provides the necessary information to support tiff images.
    /// </summary>
    public class TiffFormat : FormatBase
    {
        /// <summary>
        /// Gets the file header.
        /// </summary>
        public override byte[] FileHeader
        {
            get
            {
                return new byte[] { 77, 77, 42 };
            }
        }

        /// <summary>
        /// Gets the list of file extensions.
        /// </summary>
        public override string[] FileExtensions
        {
            get
            {
                return new[] { "tif", "tiff" };
            }
        }

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains. 
        /// </summary>
        public override string MimeType
        {
            get
            {
                return "image/tiff";
            }
        }

        /// <summary>
        /// Gets the <see cref="ImageFormat" />.
        /// </summary>
        public override ImageFormat ImageFormat
        {
            get
            {
                return ImageFormat.Tiff;
            }
        }
    }
}
