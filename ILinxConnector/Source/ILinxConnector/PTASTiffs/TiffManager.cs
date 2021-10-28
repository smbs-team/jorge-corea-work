namespace PTASTiffs
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.IO;
  using System.Linq;

  /// <summary>
  /// Generic tiff manipulation.
  /// </summary>
  public static class TiffManager
  {
    private const EncoderValue Compression = EncoderValue.CompressionLZW;

    /// <summary>
    /// Convert a tiff image into an array of images.
    /// </summary>
    /// <param name="inputTiff">Tiff to convert.</param>
    /// <returns>Array of image bytes.</returns>
    public static byte[][] TiffToImages(Stream inputTiff)
    {
      try
      {
        Image image = Image.FromStream(inputTiff);
        int pageCount = image.GetFrameCount(FrameDimension.Page);
        return (new int[pageCount]).Select((_, i)
          => GetPageBytes(i, image)).ToArray();
      }
      catch (ArgumentException ex)
      {
        throw new ArgumentException("Argument exception.", nameof(inputTiff), ex);
      }
    }

    /// <summary>
    /// Creates a tiff from an array of images.
    /// </summary>
    /// <param name="allBytes">Raw image bytes, doesn't matter the typoe.</param>
    /// <returns>Tiff image composed by all the images.</returns>
    public static byte[] ImagesToTiff(this IEnumerable<byte[]> allBytes)
    {
      if (!allBytes.Any())
      {
        throw new NeedAtLeastOneFileException();
      }

      var imageStreams = allBytes.Select(bytes
        => Image.FromStream(new MemoryStream(bytes))).ToList();

      // 1: create the tiff image based on the first image on the list.
      MemoryStream outputStream = new MemoryStream();
      ((Bitmap)imageStreams[0]).Save(outputStream, ImageFormat.Tiff);

      // this is a one paged tiff in memory with the first image.
      var tiff = Image.FromStream(outputStream);

      using (var resultStream = new MemoryStream())
      {
        // start saving into result stream.
        tiff.Save(resultStream, GetCodecInfo("image/tiff"), GetFirstPageEncoderParams());

        // process the rest of the images excluding the first.
        imageStreams
          .Where((_, i) => i > 0).ToList()
          .ForEach(img => tiff.SaveAdd(img, GetNextPagesEncoderParams()));

        // finish saving.
        tiff.SaveAdd(GetFinishingEnconderParams());

        return resultStream.ToArray();
      }
    }

    /// <summary>
    /// Creates the encoder parameters for ending an image.
    /// </summary>
    /// <returns>Created parameters.</returns>
    private static EncoderParameters GetFinishingEnconderParams()
      => new EncoderParameters
      {
        Param = new EncoderParameter[]
        {
          new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush),
        },
      };

    /// <summary>
    /// Creates the encoder parameters for pages after the first one.
    /// </summary>
    /// <returns>Created parameters.</returns>
    private static EncoderParameters GetNextPagesEncoderParams()
      => new EncoderParameters
      {
        Param = new EncoderParameter[]
        {
          new EncoderParameter(Encoder.Compression, (long)Compression),
          new EncoderParameter(
            Encoder.SaveFlag,
            (long)EncoderValue.FrameDimensionPage),
        },
      };

    /// <summary>
    /// Creates the encoder parameters for the first page of the image.
    /// </summary>
    /// <returns>Created parameters.</returns>
    private static EncoderParameters GetFirstPageEncoderParams()
      => new EncoderParameters
      {
        Param = new EncoderParameter[]
        {
          new EncoderParameter(Encoder.Compression, (long)Compression),
          new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame),
        },
      };

    private static byte[] GetPageBytes(int i, Image img)
    {
      var parms = new EncoderParameters
      {
        Param = new EncoderParameter[]
        {
          new EncoderParameter(Encoder.Quality, 50L),
        },
      };
      img.SelectActiveFrame(FrameDimension.Page, i);
      var memStream = new MemoryStream();
      img.Save(memStream, GetCodecInfo("image/png"), parms);
      return memStream.ToArray();
    }

    private static ImageCodecInfo GetCodecInfo(string png)
      => ImageCodecInfo.GetImageEncoders().Where(c => c.MimeType == png).Single();
  }
}
