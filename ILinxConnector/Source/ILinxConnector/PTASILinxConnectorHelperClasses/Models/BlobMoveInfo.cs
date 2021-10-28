// <copyright file="BlobMoveInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
    using System;

    /// <summary>
    /// Information necesary to move a blob.
    /// </summary>
    public class BlobMoveInfo
    {
        /// <summary>
        /// Gets or sets id of the blob.
        /// </summary>
        public Guid BlobId { get; set; }

        /// <summary>
        /// Gets or sets account of the iLinx container.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets roll year of the ilinx document.
        /// </summary>
        public string RollYear { get; set; }

        /// <summary>
        /// Gets or sets document type of the ilinx document.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Gets or sets recId for the ilinx document.
        /// </summary>
        public string RecId { get; set; }
    }
}