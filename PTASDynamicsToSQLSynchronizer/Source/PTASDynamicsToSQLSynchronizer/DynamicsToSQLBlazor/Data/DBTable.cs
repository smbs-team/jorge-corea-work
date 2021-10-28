// <copyright file="DBTable.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Representation of a db table.
    /// </summary>
    public class DBTable
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Column("table_name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets created.
        /// </summary>
        [Column("create_date")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets modified.
        /// </summary>
        [Column("modify_date")]
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets field list.
        /// </summary>
        public IEnumerable<DBField> Fields { get; internal set; }
    }
}
