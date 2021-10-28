namespace DeploymentHealthService.TokenProvider
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Services.AppAuthentication;

    /// <summary>
    /// Provides access tokens for azure services.
    /// </summary>
    public class AzureTokenProvider
    {
        /// <summary>
        /// The tenant URL.
        /// </summary>
        public const string TenantUrl = "kingcounty.gov";

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
    }
}
