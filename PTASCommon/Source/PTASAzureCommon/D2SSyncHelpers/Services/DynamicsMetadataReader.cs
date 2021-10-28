// <copyright file="DynamicsMetadataReader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using D2SSyncHelpers.Exceptions;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// Read metadata from dynamics.
    /// </summary>
    public class DynamicsMetadataReader : DynamicsSecurityBase, IAsyncStringReader
    {
        private const string MetadataPath = "MetadataPath";
        private readonly string metadataPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsMetadataReader"/> class.
        /// </summary>
        /// <param name="config">App configuration.</param>
        public DynamicsMetadataReader(IConfiguration config)
            : base(config)
        {
            this.metadataPath = config[MetadataPath] ?? throw new ArgumentNullException(MetadataPath);
        }

        /// <summary>
        /// Load metadata from dynamics.
        /// </summary>
        /// <returns>Loaded XML.</returns>
        /// <exception cref="DynamicsHttpRequestException">Thrown for transport error.</exception>
        public async Task<string> GetContentAsync()
        {
            try
            {
                return await this.LoadFromClient();
            }
            catch (HttpRequestException ex)
            {
                throw new DynamicsHttpRequestException("Error loading metadata from dynamics.", ex);
            }
        }

        /// <summary>
        /// Loads from the metadata physical repository.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected virtual async Task<string> LoadFromClient()
        {
            Uri baseUri = new Uri(this.CrmUri);
            Uri myUri = new Uri(baseUri, this.metadataPath);
            return await this.GetContent(myUri);
        }
    }
}