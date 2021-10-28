// <copyright file="StaticHelpers.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    using ILinxSoapImport.EdmsService;

    using PTASILinxConnectorHelperClasses.Models;

    using SautinSoft;

    /// <summary>
    /// Helpers to accumulate static functions.
    /// </summary>
    public static class StaticHelpers
    {
        /// <summary>
        /// Not available flag.
        /// </summary>
        public const string NotAvailable = "NA";

        private const string BetweenPercentSigns = "%.*?%";
        private const int DPI = 160;
        private const int MaxPages = 100;
        private const PdfFocus.CImageOptions.eColorDepth ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale24bpp;

        /// <summary>
        /// Adds a parameter at the end of an URL.
        /// </summary>
        /// <param name="url">original value.</param>
        /// <param name="key">parameter key.</param>
        /// <param name="value">parameter value.</param>
        /// <returns>new url.</returns>
        public static string AddParametersToUrl(this string url, string key, string value)
      =>
      url.Contains("?")
        ? $"{url}&{$"{key}={value}"}".Replace("&&", "&")
        : $"{url}?{$"{key}={value}"}";

        /// <summary>
        /// returns all elements but the last one.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="src">Enumeration of elements.</param>
        /// <returns>All elements except last one.</returns>
        public static IEnumerable<T> ButLast<T>(this IEnumerable<T> src)
          => src.Take(src.Count() - 1);

        /// <summary>
        /// Shorthand method to convert files from content request
        /// into a list of content store files.
        /// </summary>
        /// <param name="contentRequest">Received http request.</param>
        /// <returns>Extracted files or empty list.</returns>
        public static ReceivedFileInfo[] GetFilesFromRequest(HttpRequest contentRequest) =>
          GetFileContents(contentRequest.Files)
            .Select((bytes, index) =>
            {
                string fileName = contentRequest.Files[index].FileName;
                return new ReceivedFileInfo
                {
                    FileBits = bytes,
                    FileExtension = Path.GetExtension(fileName),
                    FileName = fileName,
                };
            }).ToArray();

        /// <summary>
        /// Calculates a hash number for a posted file.
        /// </summary>
        /// <param name="file">File to calc the hash for.</param>
        /// <returns>A hash number converted to string.</returns>
        public static string GetHash(HttpPostedFileBase file) => (file.FileName + file.ContentLength.ToString() + file.ContentType).GetHashCode().ToString();

        /// <summary>
        /// Converts a stream to files.
        /// </summary>
        /// <param name="inputStream">Read stream.</param>
        /// <param name="serial">Product serial number.</param>
        /// <returns>Converted files.</returns>
        public static ConvertFileResults GetPDFFiles(this Stream inputStream, string serial)
        {
            PdfFocus pdf = new SautinSoft.PdfFocus
            {
                Serial = serial,
            };
            pdf.OpenPdf(inputStream);
            pdf.ImageOptions.ImageFormat = ImageFormat.Png;
            pdf.OptimizeImages = true;
            pdf.ImageOptions.ColorDepth = ColorDepth;
            pdf.ImageOptions.Dpi = DPI;
            var pageCount = pdf.PageCount;
            var more = pageCount > MaxPages;
            IEnumerable<byte[]> images = GetImages(pdf, 1, MaxPages);
            return new ConvertFileResults
            {
                Images = images,
                MoreItems = more,
            };
        }

        /// <summary>
        /// Verifies that a string is a valid email address.
        /// </summary>
        /// <param name="email">string to verify.</param>
        /// <returns>valid email condition.</returns>
        public static bool IsValidEmail(this string email) =>
            new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);

        /// <summary>
        /// Remove all double spaces.
        /// </summary>
        /// <param name="s">Source string.</param>
        /// <returns>String withouth double blanks.</returns>
        public static string RemoveDoubleSpaces(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            while (s.IndexOf("  ") > 0)
            {
                s = s.Replace("  ", " ");
            }

            return s;
        }

        /// <summary>
        /// Removes leftover replacements in text.
        /// </summary>
        /// <param name="src">Source text.</param>
        /// <returns>Clean text.</returns>
        public static string RemoveEmptyTemplateVars(this string src) => Regex
                 .Matches(src, BetweenPercentSigns, RegexOptions.RightToLeft)
                 .Cast<Match>()
                 .Union(Regex
                   .Matches(src, BetweenPercentSigns, RegexOptions.None)
                   .Cast<Match>())
                 .Select(itm => itm.Value)
                 .Where(itm => !itm.Contains(">") && !itm.Contains("<"))
                 .Distinct()
                 .OrderBy(itm => itm)
                 .Aggregate(src, (a, b) => a.Replace(b, NotAvailable));

        /// <summary>
        /// Remove new lines from text.
        /// </summary>
        /// <param name="s">source string.</param>
        /// <returns>source with no new lines.</returns>
        public static string RemoveNewLines(this string s)
        {
            return string.Join(string.Empty, s.Split("\r\n".ToCharArray())).RemoveDoubleSpaces().Replace("> ", ">");
        }

        /// <summary>
        /// Removes first slash if present.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>String with no starting slash.</returns>
        public static string RemoveStartingSlash(this string source)
          => source.StartsWith("/") ? source.Substring(1) : source;

        /// <summary>
        /// converts a parameter blob name to a qualified path.
        /// </summary>
        /// <param name="imageId">Received full name.</param>
        /// <returns>Blob path to the item.</returns>
        public static string RewireName(this string imageId)
        {
            if (string.IsNullOrEmpty(imageId))
            {
                throw new ArgumentException("Image Id should not be null.", nameof(imageId));
            }

            var parts = imageId.Split('.');

            if (parts.Length < 2)
            {
                throw new ArgumentException("Image id should have at least 2 parts separated by periods.", nameof(imageId));
            }

            return string.Join(@"/", parts.ButLast().ToList()) + "." + parts.Last();
        }

        /// <summary>
        /// Copies a bitmap to an array.
        /// </summary>
        /// <param name="bitmap">bitmap to convert.</param>
        /// <returns>converted byte array.</returns>
        public static byte[] ToByteArray(this Bitmap bitmap)
        {
            ImageCodecInfo encoder = GetEncoder(ImageFormat.Png);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var m = new MemoryStream();
            bitmap.Save(m, encoder, myEncoderParameters);
            var r = m.ToArray();
            return r;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the contents of the files as a byte array.
        /// </summary>
        /// <param name="files">List of files to convert to bytes.</param>
        /// <returns>Array of byte arrays with files contents.</returns>
        private static IEnumerable<byte[]> GetFileContents(HttpFileCollection files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile currFile = files[i];
                using (MemoryStream ms = new MemoryStream())
                {
                    currFile.InputStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    yield return bytes;
                }
            }
        }

        private static IEnumerable<byte[]> GetImages(PdfFocus pdf, int startPage, int maxPages)
        {
            var lastPage = Math.Min(pdf.PageCount, startPage + maxPages);

            for (int i = startPage; i < lastPage; i += 1)
            {
                yield return pdf.ToImage(i);
            }
        }
    }
}