// <copyright file="DynamicsMetadataReader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using D2SSyncHelpers.Exceptions;
    using D2SSyncHelpers.Interfaces;
    using D2SSyncHelpers.Models;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// Loads the relationships for an entity.
    /// </summary>
    public class DynamicsRelationsLoader : DynamicsSecurityBase, IDynamicsRelationsLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsRelationsLoader"/> class.
        /// </summary>
        /// <param name="config">System config.</param>
        public DynamicsRelationsLoader(IConfiguration config)
            : base(config)
        {
        }

        /// <summary>
        /// Gets metada json.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>List of items.</returns>
        public async Task<EntityReference[]> GetRelatedEntities(string entityName)
        {
            if (entityName.EndsWith("s"))
            {
                entityName = entityName.Substring(0, entityName.Length - 1);
            }

            var url = $"{this.CrmUri}/api/data/v9.1/EntityDefinitions(LogicalName='{entityName}')/ManyToOneRelationships?$select=ReferencedAttribute,ReferencedEntity,ReferencingAttribute,ReferencingEntity";
            string returnedJson = await this.GetContent(new Uri(url));
            EntityReference[] i2 = JsonConvert.DeserializeObject<DynamicsWrapper>(returnedJson).Value;
            return i2;
        }
    }
}