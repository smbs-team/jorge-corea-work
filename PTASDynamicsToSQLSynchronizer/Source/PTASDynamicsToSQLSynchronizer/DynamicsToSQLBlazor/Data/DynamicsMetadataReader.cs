// <copyright file="DynamicsMetadataReader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Read metadata from dynamics.
    /// </summary>
    public class DynamicsMetadataReader
    {
        private readonly string crmUri;
        private readonly string authUri;
        private readonly string clientId;
        private readonly string clientSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsMetadataReader"/> class.
        /// </summary>
        /// <param name="config">App configuration.</param>
        public DynamicsMetadataReader(IConfiguration config)
        {
            this.crmUri = config["MetadataUri"];
            this.authUri = config["AuthUri"];
            this.clientId = config["clientId"];
            this.clientSecret = config["clientSecret"];
        }

        /// <summary>
        /// Load metadata from dynamics.
        /// </summary>
        /// <returns>Loaded XML.</returns>
        public async Task<string> LoadMetadataAsync()
        {
            try
            {
                var client = new HttpClient();
                this.SetupHeaders(client);
                var response = await client.GetAsync(this.crmUri + "/api/data/v9.1/$metadata");
                var text = await response.Content.ReadAsStringAsync();
                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return File.ReadAllText("$metadata.xml");
            }
        }

        /// <summary>
        /// GetTokenUsingClientIdSecret:It gets the toker for the connection with OData/Dynamics.
        /// </summary>
        /// <returns>The token for the connection.</returns>
        private string GetTokenUsingClientIdSecret()
        {
            try
            {
                var authority = this.authUri;
                var authContext = new AuthenticationContext(authority, false);
                var credentials = new ClientCredential(this.clientId, this.clientSecret);
                var tokenResult = authContext.AcquireTokenAsync(this.crmUri, credentials).Result;
                return tokenResult.AccessToken;
            }
            catch (AdalServiceException ex)
            {
                string error = string.Format($"Error trying to authenticate credentials from Dynamics odata service. (Error code: {ex.ErrorCode}, error message: {ex.Message})");
                throw new DynamicsHttpRequestException(error, ex);
            }
            catch (AggregateException ex) when (ex.InnerException is AdalServiceException)
            {
                var ex1 = ex.InnerException as AdalServiceException;
                string error = string.Format($"Azure Active Directory Autentication Exception. (Error code: {ex1.ErrorCode}, error message: {ex1.Message})");
                throw new DynamicsHttpRequestException(error, ex1);
            }
        }

        private void SetupHeaders(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(this.crmUri);
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations = *");
            httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(@"*/*"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                this.GetTokenUsingClientIdSecret());
        }
    }
}