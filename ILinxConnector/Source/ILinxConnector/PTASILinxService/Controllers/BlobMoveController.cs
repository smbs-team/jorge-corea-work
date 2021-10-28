// <copyright file="BlobMoveController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using ILinxSoapImport;
    using ILinxSoapImport.EdmsService;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json.Linq;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.Exceptions;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    using Swashbuckle.Swagger.Annotations;

    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Controller to move a blob to ilinx and then mark it as done in dynamics.
    /// </summary>
    public class BlobMoveController : BlobMoveAbstractController
    {
        private readonly IILinxHelper ilinx;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobMoveController"/> class.
        /// </summary>
        /// <param name="cloud">Cloud storage provider.</param>
        /// <param name="linx">Linx storage provider.</param>
        /// <param name="config">System configuration.</param>
        public BlobMoveController(ICloudStorageProvider cloud, IILinxHelper linx, IConfigParams config)
            : base(cloud, config)
        {
            this.ilinx = linx;
        }

        /// <summary>
        /// Inserts uten into container target.
        /// </summary>
        /// <param name="info">Info to move.</param>
        /// <param name="blobs">Blobs to apply to.</param>
        /// <returns>List of responses.</returns>
        protected override (BlobDocumentContainer blobContainer, InsertResponse insertResponse)[] InsertIntoTarget(BlobMoveInfo info, List<BlobDocumentContainer> blobs) => blobs
              .Select(blobContainer => this.SaveToIlinx(info, blobContainer)).ToArray();

        /// <summary>
        /// Always returns false.
        /// </summary>
        /// <returns>False, always, it's not sharepoint.</returns>
        protected override bool IsSharePoint() => false;

        private (BlobDocumentContainer blobContainer, InsertResponse insertResponse) SaveToIlinx(BlobMoveInfo info, BlobDocumentContainer blobContainer)
        {
            ReceivedFileInfo[] files = GetFilesForContainer(blobContainer);
            InsertResponse insertResponse = this.ilinx.SaveDocument(
                                                          info.AccountNumber,
                                                          info.RollYear,
                                                          info.DocType,
                                                          info.RecId,
                                                          files);
            return (blobContainer, insertResponse);
        }
    }
}