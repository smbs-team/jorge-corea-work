// <copyright file="PDFConvertController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Http;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.App_Start.SwaggerFilters;
    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.Exceptions;

    using PTASLinxConnectorHelperClasses.Models;

    using Swashbuckle.Swagger.Annotations;

    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Controller to convert pdf files to images.
    /// </summary>
    public class PDFConvertController : ApiController
    {
        private readonly IConfigParams config;

        /// <summary>
        /// Initializes a new instance of the <see cref="PDFConvertController"/> class.
        /// </summary>
        /// <param name="config">Configuration params.</param>
        public PDFConvertController(IConfigParams config)
        {
            this.config = config;
        }

        /// <summary>
        /// Receive PDF file, convert it to images, save in temporary storage and return urls.
        /// </summary>
        /// <returns>Result of coversion.</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        [SwaggerOperationFilter(typeof(NeedsFilesFilter))]
        public ConvertFileResults Post()
        {
            var files = this.GetFiles();
            if (files?.Count != 1)
            {
                throw new WrongNumberOfFilesException(1, files?.Count ?? 0);
            }

            var file = files[0];
            return file.InputStream.GetPDFFiles(this.config.SautinLicense);
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
    }
}