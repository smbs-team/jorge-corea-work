// <copyright file="DocumentsController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Http;
    using ILinxSoapImport;
    using ILinxSoapImport.EdmsService;
    using PTASILinxConnectorHelperClasses.Models;
    using PTASIlinxService;
    using PTASIlinxService.App_Start.SwaggerFilters;
    using PTASIlinxService.Classes;
    using PTASLinxConnectorHelperClasses.Models;
    using Swashbuckle.Swagger.Annotations;

    /// <summary>
    /// Controller to fetch and add documents.
    /// </summary>
    public class DocumentsController : ApiController
    {
        private readonly IILinxHelper iLinxHelper;
        private readonly ICacheManager<DocumentResult> cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsController"/> class.
        /// </summary>
        /// <param name="iLinxHelper">Inyected iLinxHelper.</param>
        /// <param name="cacheManager">Current cache manager.</param>
        public DocumentsController(IILinxHelper iLinxHelper, ICacheManager<DocumentResult> cacheManager)
        {
            this.iLinxHelper = iLinxHelper
              ?? throw new ArgumentNullException(nameof(iLinxHelper));
            this.cacheManager = cacheManager
              ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        /// <summary>
        /// Gets document info for document Id. Note Id is a Guid but here we don't care.
        /// </summary>
        /// <param name="id">Id to search for.</param>
        /// <param name="tiffsAsTiffs">Should tiffs be returned as tiffs.</param>
        /// <returns>Document information if found.</returns>
        [HttpGet]
        public DocumentResult Get(string id, bool tiffsAsTiffs = false) =>
          this.cacheManager
            .Get($"doc{tiffsAsTiffs}-{id}", 0.5, ()
              => this.iLinxHelper.FetchDocument(id, tiffsAsTiffs));

        /// <summary>
        /// Update the images in a document.
        /// </summary>
        /// <remarks>Params in form: Files, DocumentId.</remarks>
        /// <returns>Status of request.</returns>
        [HttpPatch]
        [SwaggerOperationFilter(typeof(NeedsFilesFilter))]
        [SwaggerOperationFilter(typeof(DocumentControllerPatchFilter))]
        public InsertResponse Patch()
        {
            NameValueCollection formValues = this.GetFormValues();
            string id = formValues["documentId"];
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid documentGuid))
            {
                throw new ArgumentException("Value must be a non-empty guid.", "DocumentId");
            }

            this.cacheManager.RemoveItem($"doc-{id}");
            this.cacheManager.RemoveItem($"doc-{id}");
            return this.iLinxHelper.UpdateDocument(documentGuid, this.GetFiles());
        }

        /// <summary>
        /// Attempt to insert a document in ILinx.
        /// </summary>
        /// <returns>Response to insertion request.</returns>
        [HttpPost]
        [SwaggerOperationFilter(typeof(NeedsFilesFilter))]
        [SwaggerOperationFilter(typeof(DocumentControllerPostFilter))]
        public InsertResponse Post()
        {
            NameValueCollection form = this.GetFormValues();
            return this.iLinxHelper.SaveDocument(
              form["accountNumber"],
              form["rollYear"],
              form["docType"],
              form["recId"],
              this.GetFiles());
        }

        /// <summary>
        /// Returns a name value collection with the form values.
        /// </summary>
        /// <returns>All form values.</returns>
        protected virtual NameValueCollection GetFormValues()
        {
            return HttpContext.Current.Request.Form;
        }

        /// <summary>
        /// Attempt to get the files from the request.
        /// </summary>
        /// <returns>List of files.</returns>
        protected virtual ReceivedFileInfo[] GetFiles() => StaticHelpers.GetFilesFromRequest(HttpContext.Current.Request);
    }
}