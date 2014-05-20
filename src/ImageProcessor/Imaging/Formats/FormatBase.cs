// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatBase.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The supported format base implement this class when building a supported format.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging.Formats
{
    using System.Drawing.Imaging;

    /// <summary>
    /// The supported format base. Implement this class when building a supported format.
    /// </summary>
    public abstract class FormatBase : ISupportedImageFormat
    {
        /// <summary>
        /// Gets the file header.
        /// </summary>
        public abstract byte[] FileHeader { get; }

        /// <summary>
        /// Gets the list of file extensions.
        /// </summary>
        public abstract string[] FileExtensions { get; }

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains.
        /// </summary>
        public abstract string MimeType { get; }

        /// <summary>
        /// Gets the <see cref="ImageFormat" />.
        /// </summary>
        public abstract ImageFormat ImageFormat { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the image format is indexed.
        /// </summary>
        public bool IsIndexed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image format is animated.
        /// </summary>
        public bool IsAnimated { get; set; }
    }
}
