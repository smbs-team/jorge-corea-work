// <copyright file="FilesController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Http;

    using ILinxSoapImport;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// Controller to retrieve images from Linx.
    /// </summary>
    public class FilesController : ApiController
    {
        private readonly DocumentsController docsController;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesController"/> class.
        /// </summary>
        /// <param name="iLinx">Inyected iLinx wrapper.</param>
        /// <param name="cacheManager">Current cache manager.</param>
        public FilesController(IILinxHelper iLinx, ICacheManager<DocumentResult> cacheManager)
        {
            if (iLinx is null)
            {
                throw new ArgumentNullException(nameof(iLinx));
            }

            if (cacheManager is null)
            {
                throw new ArgumentNullException(nameof(cacheManager));
            }

            this.docsController = new DocumentsController(iLinx, cacheManager);
        }

        /// <summary>
        /// Tries to fetch an image based on a 3 part id.
        /// </summary>
        /// <param name="id">DocumentId.FileId.FileExtension.</param>
        /// <returns>Bytes of the found file, if found.</returns>
        [HttpGet]
        public FileStreamResult Get(string id)
        {
            var format = "{id}.{fileID}.{index}.{extension}";
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(nameof(id), innerException: null);
            }

            var parts = id.Split('.');
            if (parts.Length != format.Split('.').Length)
            {
                throw new ArgumentException("Wrong filename.");
            }

            (string docId, string fileId, string index, string extension) = (parts[0], parts[1], parts[2], parts[3]);

            if (!Guid.TryParse(docId, out _))
            {
                throw new ArgumentException("Document Id not a guid.");
            }

            FileDetails file = this.GetFileDetails(docId, fileId, index, extension);
            if (file == null)
            {
                throw new FileNotFoundException();
            }

            return new FileStreamResult(new MemoryStream(file.FileBytes), $"image/{extension}");
        }

        private FileDetails GetFileDetails(string part0, string part1, string index, string extension)
        {
            var doc = this.docsController.Get(part0, extension.Contains("tif"));
            if (doc == null)
            {
                throw new FileNotFoundException();
            }

            var file = doc.Files.Where(f => f.FileId == $"{part1}.{index}").FirstOrDefault();
            return file;
        }
    }
}