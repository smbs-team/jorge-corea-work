// <copyright file="Entity.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    /// <summary>POCO for a entity.</summary>
    public class Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">Name of entity.</param>
        /// <param name="syncOrder">SyncOrder of entity.</param>
        public Entity(string name, int syncOrder)
        {
            this.Name = name;
            this.SyncOrder = syncOrder;
        }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the synchronization order.</summary>
        /// <value>The synchronize order.</value>
        public int SyncOrder { get; set; }
    }
}
