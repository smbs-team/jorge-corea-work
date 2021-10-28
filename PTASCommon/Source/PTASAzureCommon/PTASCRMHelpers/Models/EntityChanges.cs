// <copyright file="EntityChanges.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Changes to a dynamics entity.
    /// </summary>
    public class EntityChanges
    {
        /// <summary>
        /// Gets or sets entity Name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets entity Id.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets changes.
        /// </summary>
        public Dictionary<string, object> Changes { get; set; }
    }
}
