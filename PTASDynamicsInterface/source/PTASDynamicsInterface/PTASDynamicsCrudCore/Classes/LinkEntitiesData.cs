// <copyright file="LinkEntitiesData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Data to link 2 dynamics entities.
    /// </summary>
    public class LinkEntitiesData
    {
        /// <summary>
        /// Gets or sets counterpartTableName.
        /// </summary>
        public string CounterpartEntityName { get; set; }

        /// <summary>
        /// Gets or sets counterparId.
        /// </summary>
        public string CounterparId { get; set; }

        /// <summary>
        /// Gets or sets tableName.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets entityId.
        /// </summary>
        public string EntityId { get; set; }
    }
}