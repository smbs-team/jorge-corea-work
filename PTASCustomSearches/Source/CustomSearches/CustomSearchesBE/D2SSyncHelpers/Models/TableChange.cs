// <copyright file="TableChange.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents the last change saved for a table.
    /// </summary>
    public class TableChange
    {
        /// <summary>
        /// Gets or sets name of the table is the ID.
        /// </summary>
        [Column("tableId")]
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets date of last processing.
        /// </summary>
        [Column("LastDateProcessed")]
        public DateTime LastDateProcessed { get; set; }

        /// <summary>
        /// Gets or sets guid of last processing.
        /// </summary>
        [Column("LastGuidProcessed")]
        public Guid LastGuidProcessed { get; set; }
    }
}