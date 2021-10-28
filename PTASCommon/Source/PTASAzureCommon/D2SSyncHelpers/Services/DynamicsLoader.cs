// <copyright file="DynamicsLoader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Loads data from dynamics.
    /// </summary>
    public class DynamicsLoader : DynamicsSecurityBase
    {
        private readonly string dynamicsPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsLoader"/> class.
        /// </summary>
        /// <param name="config">Sys configuration.</param>
        public DynamicsLoader(IConfiguration config)
            : base(config)
        {
            ////this.dynamicsPath = $"{config["DynamicsURL"]}{config["DataPath"]}";
            this.dynamicsPath = $"{config["DynamicsURL"]}/api/data/v9.1/";
        }

        /// <summary>
        /// Attempt to load data from table.
        /// </summary>
        /// <param name="entityName">Table to process.</param>
        /// <param name="pluralName">Pluralized name.</param>
        /// <param name="pkFieldName">Name of the PK field.</param>
        /// <param name="lastProcessedGuid">Last guid processed.</param>
        /// <param name="chunkSize">Number of records to read.</param>
        /// <returns>Content.</returns>
        public async Task<string> LoadTableDataAsync(string entityName, string pluralName, string pkFieldName, Guid lastProcessedGuid, int chunkSize)
        {
            // removed the filter by date ==> and createdon ge '{lastProcessedDate}' and also the order by date ==> createdon asc,
            var path = $"{this.dynamicsPath}{pluralName}?$top={chunkSize}&$filter={pkFieldName} gt {lastProcessedGuid}&$orderby={pkFieldName} asc";
            var uri = new Uri(path);
            var s = await this.GetContent(uri);
            return s;
        }

        /// <summary>
        /// Entity Definition of a entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>The entity set name and the name of the primary id.</returns>
        public async Task<JToken> EntityDefinition(string entityName)
        {
            var path = $"{this.dynamicsPath}EntityDefinitions?$select=EntitySetName, PrimaryIdAttribute&$filter=LogicalName eq '{entityName}'";
            var uri = new Uri(path);
            var s = await this.GetContent(uri);
            dynamic deserialized = JsonConvert.DeserializeObject(s);
            JArray actualValues = deserialized.value;
            return actualValues[0];
        }

        /// <summary>
        /// Saves entity.
        /// </summary>
        /// <param name="entityName">Entity to save.</param>
        /// <param name="payload">Data to save.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task SaveEntity(string entityName, object payload)
        {
            var path = this.dynamicsPath + entityName;
            var uri = new Uri(path);
            await this.PostContent(uri, JsonConvert.SerializeObject(payload));
        }
    }
}