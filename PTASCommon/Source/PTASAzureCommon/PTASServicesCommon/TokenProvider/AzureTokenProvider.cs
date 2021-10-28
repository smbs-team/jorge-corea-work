namespace PTASServicesCommon.TokenProvider
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Provides access tokens for azure services.
    /// </summary>
    /// <seealso cref="PTASServicesCommon.TokenProvider.IServiceTokenProvider" />
    public class AzureTokenProvider : IServiceTokenProvider
    {
        /// <summary>
        /// The tenant URL.
        /// </summary>
        public const string TenantUrl = "kingcounty.gov";

        /// <summary>
        /// The tenant Id.
        /// </summary>
        public const string TenantId = "bae5059a-76f0-49d7-9996-72dfe95d69c7";

        /// <summary>
        /// The database token endpoint.
        /// </summary>
        public const string DbTokenEndpoint = "https://database.windows.net/";

        /// <summary>
        /// The storage token endpoint.
        /// </summary>
        public const string StorageTokenEndpoint = "https://storage.azure.com/";

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="tokenEndpoint">The token endpoint.</param>
        /// <returns>A token that can be used to access cloud services.</returns>
        public async Task<string> GetAccessTokenAsync(string tokenEndpoint)
        {
            var tokenProvider = new AzureServiceTokenProvider();
            string tokenCredential = await tokenProvider.GetAccessTokenAsync(tokenEndpoint);
            return tokenCredential;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="tokenEndpoint">The token endpoint.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="clientCredential">The client credential.</param>
        /// <returns>A token that can be used to access cloud services.</returns>
        public async Task<string> GetAccessTokenAsync(string tokenEndpoint, string tenantId, ClientCredential clientCredential)
        {
            string aadInstance = "https://login.windows.net/{0}";
            AuthenticationContext authenticationContext = new AuthenticationContext(string.Format(aadInstance, tenantId));
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(tokenEndpoint, clientCredential);
            return authenticationResult.AccessToken;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="tokenEndpoint">The token endpoint.</param>
        /// <param name="tenantUrl">The tenant URL.</param>
        /// <returns>
        /// A token that can be used to access cloud services.
        /// </returns>
        public async Task<string> GetAccessTokenAsync(string tokenEndpoint, string tenantUrl)
        {
            var tokenProvider = new AzureServiceTokenProvider();
            string tokenCredential = await tokenProvider.GetAccessTokenAsync(tokenEndpoint, tenantUrl);
            return tokenCredential;
        }

        /// <summary>
        /// Gets a service to service access token by using client credentials flow.
        /// </summary>
        /// <param name="apimEndpoint">The apim endpoint url.</param>
        /// <param name="resourceId">The resource identifier of the resource to grant access for.</param>
        /// <param name="subscriptionKey">The subscription key for apim oauth endpoint.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>
        /// A token that can be used to access cloud services.
        /// </returns>
        public async Task<string> GetKcServiceAccessTokenAsync(string apimEndpoint, string resourceId, string subscriptionKey, string clientId, string clientSecret)
        {
            var client = new HttpClient();
            Uri apimUri = new Uri(apimEndpoint);
            Uri tokenServiceUri = new Uri(apimUri, "/oauth2token/v1/");

            // Request headers.
            client.DefaultRequestHeaders.Add("Client-Id", clientId);
            client.DefaultRequestHeaders.Add("Client-Secret", clientSecret);
            client.DefaultRequestHeaders.Add("Resource-Id", resourceId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var uri = tokenServiceUri.ToString();
            var tokenresponse = await client.GetAsync(uri);
            var tokencontent = await tokenresponse.Content.ReadAsStringAsync();
            return tokencontent;
        }
    }
}
