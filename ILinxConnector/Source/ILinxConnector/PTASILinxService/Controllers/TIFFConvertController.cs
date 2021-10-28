// <copyright file="TIFFConvertController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.Exceptions;

    /// <summary>
    /// Controller to convert tiffs to images.
    /// </summary>
    public class TIFFConvertController : ApiController
    {
        private readonly ICacheManager<ConvertFileResults> cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TIFFConvertController"/> class.
        /// </summary>
        /// <param name="cacheManager">Cache manager.</param>
        public TIFFConvertController(ICacheManager<ConvertFileResults> cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Receive TIFF file, convert it to images, save in temporary storage and return urls.
        /// </summary>
        /// <returns>Result of coversion.</returns>
        [HttpPost]
        public ConvertFileResults Post()
        {
            HttpContext currContext = this.GetHttpContext();
            var contentRequest = currContext.Request;
            HttpFileCollectionBase files = this.GetFiles();
            if (files == null || files.Count != 1)
            {
                throw new WrongNumberOfFilesException(1, 0);
            }

            var hash = StaticHelpers.GetHash(files[0]);
            return this.cacheManager.Get(hash, 10, () =>
            {
                Stream inputStream = files[0].InputStream;
                ConvertFileResults convertFileResults = this.GetTiffFiles(inputStream);
                return convertFileResults;
            });
        }

        /// <summary>
        /// Get a list of posted files.
        /// </summary>
        /// <returns>The files from the request.</returns>
        protected virtual HttpFileCollectionBase GetFiles()
        {
            HttpContext currContext = this.GetHttpContext();
            var contentRequest = currContext.Request;

            HttpFileCollection files = contentRequest.Files;
            return new HttpFileCollectionWrapper(files);
        }

        /// <summary>
        /// Returns current http context.
        /// </summary>
        /// <returns>Current http context.</returns>
        protected virtual HttpContext GetHttpContext()
        {
            return HttpContext.Current;
        }

        /// <summary>
        /// Get Current URI.
        /// </summary>
        /// <returns>Current request URI.</returns>
        protected virtual Uri GetUri()
        {
            return this.Request.RequestUri;
        }

        private static byte[] GetPageBytes(int i, Image img)
        {
            img.SelectActiveFrame(FrameDimension.Page, i);
            var m = new MemoryStream();
            img.Save(m, ImageFormat.Png);
            return m.ToArray();
        }

        /// <summary>
        /// Return a list of files from a tiff.
        /// </summary>
        /// <param name="inputStream">Tiff stream to convert to files.</param>
        /// <returns>A list of file bytes and urls.</returns>
        private ConvertFileResults GetTiffFiles(Stream inputStream)
        {
            var img = Image.FromStream(inputStream);
            int nCount = img.GetFrameCount(FrameDimension.Page);
            var r = new int[nCount];
            var images = r.Select((_, i) => GetPageBytes(i, img)).ToArray();

            return new ConvertFileResults
            {
                Images = images,
            };
        }
    }
}