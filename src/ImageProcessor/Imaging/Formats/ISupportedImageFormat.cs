// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISupportedImageFormat.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The SupportedImageFormat interface providing information about image formats to ImageProcessor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging.Formats
{
    using System.Drawing.Imaging;

    /// <summary>
    /// The SupportedImageFormat interface providing information about image formats to ImageProcessor.
    /// </summary>
    public interface ISupportedImageFormat
    {
        /// <summary>
        /// Gets the file header.
        /// </summary>
        byte[] FileHeader { get; }

        /// <summary>
        /// Gets the list of file extensions.
        /// </summary>
        string[] FileExtensions { get; }

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains. 
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Gets the <see cref="ImageFormat"/>.
        /// </summary>
        ImageFormat ImageFormat { get; }

        /// <summary>
        /// Gets a value indicating whether the image format is indexed.
        /// </summary>
        bool IsIndexed { get; }

        /// <summary>
        /// Gets a value indicating whether the image format is animated.
        /// </summary>
        bool IsAnimated { get; }
    }
}
