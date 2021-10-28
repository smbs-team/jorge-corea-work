// <copyright file="ImageUtilities.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImageManipulation
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Image static utilities.
    /// </summary>
    public static class ImageUtilities
    {
        /// <summary>
        /// MedWidth.
        /// </summary>
        public const int MedWidth = 768;

        /// <summary>
        /// SmallWidth.
        /// </summary>
        public const int SmallWidth = 200;

        /// <summary>
        /// File extensions and formatters for image types.
        /// </summary>
        public static readonly IEnumerable<(string extension, ImageFormat format)> Extensions = new (string extension, ImageFormat format)[]
       {
      ("bmp", ImageFormat.Bmp),
      ("gif", ImageFormat.Gif),
      ("jpeg", ImageFormat.Jpeg),
      ("jpg", ImageFormat.Jpeg),
      ("png", ImageFormat.Png),
      ("tif", ImageFormat.Tiff),
      ("tiff", ImageFormat.Tiff),
      ("wmf", ImageFormat.Png),
       };

        /// <summary>
        /// Get image stream sized.
        /// </summary>
        /// <param name="img">Img.</param>
        /// <param name="maxWidth">max Width.</param>
        /// <param name="imageExtension">Image Extension.</param>
        /// <returns>Stream with the image ready to save.</returns>
        public static Stream GetImageStream(Image img, int maxWidth, string imageExtension)
        {
            var formatter =
                GetFormatter(imageExtension);
            return formatter is null ? null : GetImageStream(img, maxWidth, formatter);
        }

        /// <summary>
        /// Get image stream sized.
        /// </summary>
        /// <param name="img">Img.</param>
        /// <param name="maxWidth">max Width.</param>
        /// <param name="formatter">Image formatterF.</param>
        /// <returns>Stream with the image ready to save.</returns>
        public static Stream GetImageStream(Image img, int maxWidth, ImageFormat formatter)
        {
            var newHeight = (maxWidth * img.Height) / img.Width;
            var bmp = ResizeImage(img, maxWidth, newHeight);
            var result = new MemoryStream();
            bmp.Save(result, formatter);

            result.Position = 0;
            return result;
        }

        /// <summary>
        /// Is this extension image related.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <returns>If the extension is an image.</returns>
        public static bool IsImageExtension(this string fileName) => GetFormatter(Path.GetExtension(fileName)) != null;

        /// <summary>
        /// Is this extension image related.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <returns>If the extension is an svg.</returns>
        public static bool IsSVGExtension(this string fileName) => fileName.ToLower().EndsWith("svg");

        private static ImageFormat GetFormatter(string imageExtension)
        {
            var ext = imageExtension.ToLower().Replace(".", string.Empty);
            return Extensions
                            .Where(y => y.extension.Equals(ext))
                            .Select(y => y.format)
                            .FirstOrDefault();
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }
    }
}