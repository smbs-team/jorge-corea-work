// <copyright file="EntityChanges.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers.Models
{
    using System.Collections.Generic;
    using System.Collections.Generic;

    /// <summary>
    /// Request for a dynamics entity.
    /// </summary>
    public class EntityRequest
    {
        /// <summary>
        /// Gets or sets entity Name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets entity Id.
        /// </summary>
        public string EntityId { get; set; }
    }
}
