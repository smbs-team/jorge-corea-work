// <copyright file="IILinxHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport
{
    using System;
    using ILinxSoapImport.EdmsService;
    using PTASILinxConnectorHelperClasses.Models;
    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// iLinx helper interface.
    /// </summary>
    public interface IILinxHelper
    {
        /// <summary>
        /// Get a document from iLinx.
        /// </summary>
        /// <param name="id">Id of the document to fetch.</param>
        /// <param name="returnTiffs">Should it return tiffs as tiffs.</param>
        /// <returns>The document information.</returns>
        DocumentResult FetchDocument(string id, bool returnTiffs = false);

        /// <summary>
        /// Attempts to insert a new document.
        /// </summary>
        /// <param name="accountNumber">Account number.</param>
        /// <param name="rollYear">Roll Year.</param>
        /// <param name="docType">Document type.</param>
        /// <param name="recId">RecId: note we don't know what rec stands for and is not documented.</param>
        /// <param name="files">Files to save.</param>
        /// <returns>Result of insertion.</returns>
        InsertResponse SaveDocument(string accountNumber, string rollYear, string docType, string recId, ReceivedFileInfo[] files);

        /// <summary>
        /// Attempts to update an iLinx document.
        /// </summary>
        /// <param name="documentId">Id of the document.</param>
        /// <param name="files">Files to update.</param>
        /// <returns>Result of update.</returns>
        InsertResponse UpdateDocument(Guid documentId, ReceivedFileInfo[] files);
    }
}