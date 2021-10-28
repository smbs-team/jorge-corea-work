// <copyright file="BlobToIlinxFinalizerParams.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;

    /// <summary>
    /// Params received to finalize the moving of blob to iLinx.
    /// </summary>
    public class BlobToIlinxFinalizerParams
    {
        /// <summary>
        /// Gets or sets item Id.
        /// </summary>
        public Guid SEApplicationid { get; set; }

        /// <summary>
        /// Gets or sets detail Id to look for.
        /// </summary>
        public Guid SEAppDetailId { get; set; }

        /// <summary>
        /// Gets or sets document assigned to by iLinx.
        /// </summary>
        public Guid AssignedDocumentId { get; set; }

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets document assigned to by iLinx.
        /// </summary>
        public string Document { get; set; }
    }
}