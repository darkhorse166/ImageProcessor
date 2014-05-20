// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebPFormat.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Provides the necessary information to support webp images.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging.Formats
{
    using System.Drawing.Imaging;
    using System.Text;

    /// <summary>
    /// Provides the necessary information to support webp images.
    /// </summary>
    public class WebPFormat : FormatBase
    {
        /// <summary>
        /// Gets the file header.
        /// </summary>
        public override byte[] FileHeader
        {
            get
            {
                return Encoding.ASCII.GetBytes("RIFF");
            }
        }

        /// <summary>
        /// Gets the list of file extensions.
        /// </summary>
        public override string[] FileExtensions
        {
            get
            {
                return new[] { "webp" };
            }
        }

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains. 
        /// </summary>
        public override string MimeType
        {
            get
            {
                return "image/webp";
            }
        }

        /// <summary>
        /// Gets the <see cref="ImageFormat" />.
        /// </summary>
        public override ImageFormat ImageFormat
        {
            get
            {
                return null;
            }
        }
    }
}