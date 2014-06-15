﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Quantizer.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Defines the Quantizer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Encapsulates methods to calculate the color palette of an image.
    /// </summary>
    public abstract class Quantizer
    {
        /// <summary>
        /// The flag used to indicate whether a single pass or two passes are needed for quantization.
        /// </summary>
        private readonly bool singlePass;

        /// <summary>
        /// The size in bytes of the 32 bytes per pixel Color structure.
        /// </summary>
        private readonly int pixelSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ImageProcessor.Imaging.Quantizer">Quantizer</see> class. 
        /// </summary>
        /// <param name="singlePass">
        /// If set to <see langword="true"/>, then the quantizer will loop through the source pixels once; 
        /// otherwise, <see langword="false"/>.
        /// </param>
        protected Quantizer(bool singlePass)
        {
            this.singlePass = singlePass;
            this.pixelSize = Marshal.SizeOf(typeof(Color32));
        }

        /// <summary>
        /// Quantizes the given <see cref="T:System.Drawing.Image">Image</see> and returns the resulting output
        /// <see cref="T:System.Drawing.Bitmap">Bitmap.</see>
        /// </summary>
        /// <param name="source">The image to quantize</param>
        /// <returns>
        /// A quantized <see cref="T:System.Drawing.Bitmap">Bitmap</see> version of the <see cref="T:System.Drawing.Image">Image</see>
        /// </returns>
        public Bitmap Quantize(Image source)
        {
            // Get the size of the source image
            int height = source.Height;
            int width = source.Width;

            // And construct a rectangle from these dimensions
            Rectangle bounds = new Rectangle(0, 0, width, height);

            // First off take a 32bpp copy of the image
            Bitmap copy = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // And construct an 8bpp version
            Bitmap output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // Now lock the bitmap into memory
            using (Graphics g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;

                // Draw the source image onto the copy bitmap,
                // which will effect a widening as appropriate.
                g.DrawImage(source, bounds);
            }

            // Define a pointer to the bitmap data
            BitmapData sourceData = null;

            try
            {
                // Get the source image bits and lock into memory
                sourceData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                // Call the FirstPass function if not a single pass algorithm.
                // For something like an octree quantizer, this will run through
                // all image pixels, build a data structure, and create a palette.
                if (!this.singlePass)
                {
                    this.FirstPass(sourceData, width, height);
                }

                // Then set the color palette on the output bitmap. I'm passing in the current palette 
                // as there's no way to construct a new, empty palette.
                output.Palette = this.GetPalette(output.Palette);

                // Then call the second pass which actually does the conversion
                this.SecondPass(sourceData, output, width, height, bounds);
            }
            finally
            {
                // Ensure that the bits are unlocked
                copy.UnlockBits(sourceData);
            }

            // Last but not least, return the output bitmap
            return output;
        }

        /// <summary>
        /// Execute the first pass through the pixels in the image
        /// </summary>
        /// <param name="sourceData">The source data</param>
        /// <param name="width">The width in pixels of the image</param>
        /// <param name="height">The height in pixels of the image</param>
        protected virtual void FirstPass(BitmapData sourceData, int width, int height)
        {
            // Define the source data pointers. The source row is a byte to
            // keep addition of the stride value easier (as this is in bytes)              
            IntPtr sourceRow = sourceData.Scan0;

            // Loop through each row
            for (int row = 0; row < height; row++)
            {
                // Set the source pixel to the first pixel in this row
                IntPtr sourcePixel = sourceRow;

                // And loop through each column
                for (int col = 0; col < width; col++)
                {
                    this.InitialQuantizePixel(new Color32(sourcePixel));
                    sourcePixel = (IntPtr)((long)sourcePixel + this.pixelSize);
                }

                // Now I have the pixel, call the FirstPassQuantize function...
                // Add the stride to the source row
                sourceRow = (IntPtr)((long)sourceRow + sourceData.Stride);
            }
        }

        /// <summary>
        /// Execute a second pass through the bitmap
        /// </summary>
        /// <param name="sourceData">The source bitmap, locked into memory</param>
        /// <param name="output">The output bitmap</param>
        /// <param name="width">The width in pixels of the image</param>
        /// <param name="height">The height in pixels of the image</param>
        /// <param name="bounds">The bounding rectangle</param>
        protected virtual void SecondPass(BitmapData sourceData, Bitmap output, int width, int height, Rectangle bounds)
        {
            BitmapData outputData = null;

            try
            {
                // Lock the output bitmap into memory
                outputData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                // Define the source data pointers. The source row is a byte to
                // keep addition of the stride value easier (as this is in bytes)
                IntPtr sourceRow = sourceData.Scan0;
                IntPtr sourcePixel = sourceRow;
                IntPtr previousPixel = sourcePixel;

                // Now define the destination data pointers
                IntPtr destinationRow = outputData.Scan0;
                IntPtr destinationPixel = destinationRow;

                // And convert the first pixel, so that I have values going into the loop.
                byte pixelValue = this.QuantizePixel(new Color32(sourcePixel));

                // Assign the value of the first pixel
                Marshal.WriteByte(destinationPixel, pixelValue);

                // Loop through each row
                for (int row = 0; row < height; row++)
                {
                    // Set the source pixel to the first pixel in this row
                    sourcePixel = sourceRow;

                    // And set the destination pixel pointer to the first pixel in the row
                    destinationPixel = destinationRow;

                    // Loop through each pixel on this scan line
                    for (int col = 0; col < width; col++)
                    {
                        // Check if this is the same as the last pixel. If so use that value
                        // rather than calculating it again. This is an inexpensive optimisation.
                        if (Marshal.ReadInt32(previousPixel) != Marshal.ReadInt32(sourcePixel))
                        {
                            // Quantize the pixel
                            pixelValue = this.QuantizePixel(new Color32(sourcePixel));

                            // And setup the previous pointer
                            previousPixel = sourcePixel;
                        }

                        // And set the pixel in the output
                        Marshal.WriteByte(destinationPixel, pixelValue);

                        sourcePixel = (IntPtr)((long)sourcePixel + this.pixelSize);
                        destinationPixel = (IntPtr)((long)destinationPixel + 1);
                    }

                    // Add the stride to the source row
                    sourceRow = (IntPtr)((long)sourceRow + sourceData.Stride);

                    // And to the destination row
                    destinationRow = (IntPtr)((long)destinationRow + outputData.Stride);
                }
            }
            finally
            {
                // Ensure that I unlock the output bits
                output.UnlockBits(outputData);
            }
        }

        /// <summary>
        /// Override this to process the pixel in the first pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <remarks>
        /// This function need only be overridden if your quantize algorithm needs two passes,
        /// such as an Octree quantizer.
        /// </remarks>
        protected virtual void InitialQuantizePixel(Color32 pixel)
        {
        }

        /// <summary>
        /// Override this to process the pixel in the second pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <returns>The quantized value</returns>
        protected abstract byte QuantizePixel(Color32 pixel);

        /// <summary>
        /// Retrieve the palette for the quantized image
        /// </summary>
        /// <param name="original">Any old palette, this is overwritten</param>
        /// <returns>The new color palette</returns>
        protected abstract ColorPalette GetPalette(ColorPalette original);

        /// <summary>
        /// Structure that defines a 32 bit color
        /// </summary>
        /// <remarks>
        /// This structure is used to read data from a 32 bits per pixel image
        /// in memory, and is ordered in this manner as this is the way that
        /// the data is laid out in memory
        /// </remarks>
        [StructLayout(LayoutKind.Explicit)]
        public struct Color32
        {
            /// <summary>
            /// Holds the blue component of the colour
            /// </summary>
            [FieldOffset(0)]
            private byte blue;

            /// <summary>
            /// Holds the green component of the colour
            /// </summary>
            [FieldOffset(1)]
            private byte green;

            /// <summary>
            /// Holds the red component of the colour
            /// </summary>
            [FieldOffset(2)]
            private byte red;

            /// <summary>
            /// Holds the alpha component of the colour
            /// </summary>
            [FieldOffset(3)]
            private byte alpha;

            /// <summary>
            /// Permits the color32 to be treated as a 32 bit integer.
            /// </summary>
            [FieldOffset(0)]
            private int argb;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:ImageProcessor.Imaging.Quantizer.Color32">Color32</see> structure. 
            /// </summary>
            /// <param name="sourcePixel">The pointer to the pixel.</param>
            public Color32(IntPtr sourcePixel)
            {
                this = (Color32)Marshal.PtrToStructure(sourcePixel, typeof(Color32));
            }

            /// <summary>
            /// Gets or sets the blue component of the colour
            /// </summary>
            public byte Blue
            {
                get { return this.blue; }
                set { this.blue = value; }
            }
            
            /// <summary>
            /// Gets or sets the green component of the colour
            /// </summary>
            public byte Green
            {
                get { return this.green; }
                set { this.green = value; }
            }

            /// <summary>
            /// Gets or sets the red component of the colour
            /// </summary>
            public byte Red
            {
                get { return this.red; }
                set { this.red = value; }
            }

            /// <summary>
            /// Gets or sets the alpha component of the colour
            /// </summary>
            public byte Alpha
            {
                get { return this.alpha; }
                set { this.alpha = value; }
            }

            /// <summary>
            /// Gets or sets the ARGB component, permitting the color32 to be treated as a 32 bit integer.
            /// </summary>
            public int Argb
            {
                get { return this.argb; }
                set { this.argb = value; }
            }

            /// <summary>
            /// Gets the color for this Color32 object
            /// </summary>
            public Color Color
            {
                get { return Color.FromArgb(this.Alpha, this.Red, this.Green, this.Blue); }
            }
        }
    }
}