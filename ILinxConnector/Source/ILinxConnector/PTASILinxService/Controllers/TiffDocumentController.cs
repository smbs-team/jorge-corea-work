// <copyright file="TiffDocumentController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Web.Http;

    using ILinxSoapImport;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// TiffDocumentController.
    /// </summary>
    public class TiffDocumentController : ApiController
    {
        private readonly IILinxHelper iLinxHelper;
        private readonly ICacheManager<DocumentResult> cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffDocumentController"/> class.
        /// </summary>
        /// <param name="iLinxHelper">ILinx Helper.</param>
        /// <param name="cacheManager">Cache Manager.</param>
        public TiffDocumentController(IILinxHelper iLinxHelper, ICacheManager<DocumentResult> cacheManager)
        {
            this.iLinxHelper = iLinxHelper
              ?? throw new ArgumentNullException(nameof(iLinxHelper));
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Gets document info for document Id. Note Id is a Guid but here we don't care.
        /// </summary>
        /// <param name="id">Id to search for.</param>
        /// <returns>Document information if found.</returns>
        [HttpGet]
        public DocumentResult Get(string id)
        {
            return this.cacheManager.Get($"tiff-{id}", 0.5, () =>
            {
                return this.iLinxHelper.FetchDocument(id, true);
            });
        }
    }
}