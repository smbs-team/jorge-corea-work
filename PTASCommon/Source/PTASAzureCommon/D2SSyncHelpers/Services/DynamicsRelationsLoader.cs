// <copyright file="DynamicsRelationsLoader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Threading.Tasks;

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
        /// Get metadata for entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>Task that yields the metadata.</returns>
        public async Task<DynamicsEntityInfo> GetMetadataAsync(string entityName)
        {
            var url = $"{this.CrmUri}/api/data/v9.1/EntityDefinitions(LogicalName='{entityName}')?$expand=Attributes&$select=EntitySetName";
            var returnedJson = await this.GetContent(new Uri(url));
            var metadata = JsonConvert.DeserializeObject<DynamicsEntityInfo>(returnedJson);
            return metadata;
        }

        /// <summary>
        /// Gets metada json.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>List of items.</returns>
        public async Task<EntityManyToManyReference[]> GetNMRelatedEntities(string entityName) => await this.LoadRelated<EntityManyToManyReference>(entityName, "ManyToManyRelationships", string.Empty);

        /// <summary>
        /// Gets metada json.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>List of items.</returns>
        public async Task<EntityReference[]> GetRelatedEntities(string entityName) => await this.LoadRelated<EntityReference>(entityName, "ManyToOneRelationships", "ReferencedAttribute,ReferencedEntity,ReferencingAttribute,ReferencingEntity");

        /// <summary>
        /// Gets defauls sync url for entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>Url.</returns>
        public string GetSyncUrlForEntity(string entityName) => $"{this.CrmUri}/api/data/v9.1/{entityName}";

        /// <summary>
        /// Runs a uri.
        /// </summary>
        /// <param name="syncUrl">Url to run.</param>
        /// <returns>result of fetch.</returns>
        public async Task<string> RunUri(string syncUrl) => await this.GetContent(new Uri(syncUrl), true);

        private async Task<string> GetRelationships(string entityName, string relationshipType, string select)
        {
            var nent = entityName.EndsWith("s") ? entityName[0..^1] : entityName;
            string ns = !string.IsNullOrEmpty(select) ? "?$select=" + select : string.Empty;
            var url = $"{this.CrmUri}/api/data/v9.1/EntityDefinitions(LogicalName='{nent}')/{relationshipType}{ns}";
            string returnedJson = await this.GetContent(new Uri(url));
            return returnedJson;
        }

        private async Task<T[]> LoadRelated<T>(string entityName, string relationshipType, string select)
        {
            string returnedJson = await this.GetRelationships(entityName, relationshipType, select);
            T[] i2 = JsonConvert.DeserializeObject<DynamicsWrapper<T>>(returnedJson).Value;
            return i2;
        }
    }
}