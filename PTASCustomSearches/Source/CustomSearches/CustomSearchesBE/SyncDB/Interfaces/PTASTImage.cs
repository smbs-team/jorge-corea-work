using PTASDBFramework.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace SyncDataBase.Interfaces
{
    public class PTASTImage : IPTASTImage
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            Image image = null;
            using (MemoryStream memory = new MemoryStream(imageData))
            {
                image = Image.FromStream(memory);
            }

            if (image != null)
            {
                var ratioX = (double)width / image.Width;
                var ratioY = (double)height / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newImage = new Bitmap(newWidth, newHeight);

                using (var graphics = Graphics.FromImage(newImage))
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);

                using (var stream = new MemoryStream())
                {
                    newImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }
            return null;
        }
    }
}
