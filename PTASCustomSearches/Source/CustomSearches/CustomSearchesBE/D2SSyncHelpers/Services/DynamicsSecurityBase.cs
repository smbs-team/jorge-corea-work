// <copyright file="DynamicsSecurityBase.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using D2SSyncHelpers.Exceptions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Base for any object that needs to access dynamics.
    /// </summary>
    public class DynamicsSecurityBase
    {
        private const string AuthUri = "AuthUri";

        private const string ClientId = "ClientId";

        private const string ClientSecret = "ClientSecret";

        private const string DynamicsURL = "DynamicsURL";

        private readonly string authUri;

        private readonly string clientId;

        private readonly string clientSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsSecurityBase"/> class.
        /// </summary>
        /// <param name="config">App config.</param>
        public DynamicsSecurityBase(IConfiguration config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.CrmUri = config[DynamicsURL] ?? throw new ArgumentNullException(DynamicsURL);
            this.authUri = config[AuthUri] ?? throw new ArgumentNullException(AuthUri);
            this.clientId = config[ClientId] ?? throw new ArgumentNullException(ClientId);
            this.clientSecret = config[ClientSecret] ?? throw new ArgumentNullException(ClientSecret);
        }

        /// <summary>
        /// Gets or sets uri of the dynamics rest service.
        /// </summary>
        public string CrmUri { get; set; }

        /// <summary>
        /// Attempts to query dynamics using the provided URI.
        /// </summary>
        /// <param name="myUri">Uri for the request.</param>
        /// <returns>Response String.</returns>
        protected async Task<string> GetContent(Uri myUri)
        {
            Console.WriteLine("Getting content from Uri: " + myUri.ToString());
            var client = new HttpClient();
            this.SetupHeaders(client);

            var response = await client.GetAsync(myUri);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        /// <summary>
        /// GetTokenUsingClientIdSecret:It gets the toker for the connection with OData/Dynamics.
        /// </summary>
        /// <returns>The token for the connection.</returns>
        protected string GetTokenUsingClientIdSecret()
        {
            try
            {
                var authContext = new AuthenticationContext(this.authUri, false);
                var credentials = new ClientCredential(this.clientId, this.clientSecret);
                var tokenResult = authContext.AcquireTokenAsync(this.CrmUri, credentials).Result;
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

        /// <summary>
        /// Setup request headers.
        /// </summary>
        /// <param name="httpClient">Client to setup.</param>
        protected void SetupHeaders(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(this.CrmUri);
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