// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngFormat.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Provides the necessary information to support png images.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging.Formats
{
    using System.Drawing.Imaging;

    /// <summary>
    /// Provides the necessary information to support png images.
    /// </summary>
    public class PngFormat : FormatBase
    {
        /// <summary>
        /// Gets the file header.
        /// </summary>
        public override byte[] FileHeader
        {
            get
            {
                return new byte[] { 137, 80, 78, 71 };
            }
        }

        /// <summary>
        /// Gets the list of file extensions.
        /// </summary>
        public override string[] FileExtensions
        {
            get
            {
                return new[] { "png" };
            }
        }

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains. 
        /// </summary>
        public override string MimeType
        {
            get
            {
                return "image/png";
            }
        }

        /// <summary>
        /// Gets the <see cref="ImageFormat" />.
        /// </summary>
        public override ImageFormat ImageFormat
        {
            get
            {
                return ImageFormat.Png;
            }
        }
    }
}
