// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageProcessorConfiguration.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The image processor configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ImageProcessor.Core.Common.Exceptions;
    using ImageProcessor.Imaging.Formats;

    /// <summary>
    /// The image processor configuration.
    /// </summary>
    public class ImageProcessorConfiguration
    {
        /// <summary>
        /// A new instance Initializes a new instance of the <see cref="ImageProcessorConfiguration"/> class.
        /// with lazy initialization.
        /// </summary>
        private static readonly Lazy<ImageProcessorConfiguration> Lazy =
                        new Lazy<ImageProcessorConfiguration>(() => new ImageProcessorConfiguration());

        /// <summary>
        /// Prevents a default instance of the <see cref="ImageProcessorConfiguration"/> class from being created.
        /// </summary>
        private ImageProcessorConfiguration()
        {
            this.LoadSupportedImageFormats();
        }

        /// <summary>
        /// Gets the current instance of the <see cref="ImageProcessorConfiguration"/> class.
        /// </summary>
        public static ImageProcessorConfiguration Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        /// <summary>
        /// Gets the supported image formats.
        /// </summary>
        public IEnumerable<ISupportedImageFormat> SupportedImageFormats { get; private set; }

        /// <summary>
        /// Creates a list, using reflection, of supported image formats that ImageProcessor can run.
        /// </summary>
        private void LoadSupportedImageFormats()
        {
            if (this.SupportedImageFormats == null)
            {
                try
                {
                    Type type = typeof(ISupportedImageFormat);
                    List<Type> availableTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(t => t != null && type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .ToList();

                    this.SupportedImageFormats = availableTypes
                        .Select(x => (Activator.CreateInstance(x) as ISupportedImageFormat)).ToList();
                }
                catch (Exception ex)
                {
                    throw new ImageFormatException(ex.Message, ex.InnerException);
                }
            }
        }
    }
}
