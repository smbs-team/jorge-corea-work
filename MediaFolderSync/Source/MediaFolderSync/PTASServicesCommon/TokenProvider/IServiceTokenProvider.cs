namespace PTASServicesCommon.TokenProvider
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that provides for tokens that can en used to access cloud services.
    /// </summary>
    public interface IServiceTokenProvider
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="tokenEndpoint">The token endpoint.</param>
        /// <returns>A token that can be used to access cloud services.</returns>
        Task<string> GetAccessTokenAsync(string tokenEndpoint);

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="tokenEndpoint">The token endpoint.</param>
        /// <param name="tenantUrl">The tenant URL.</param>
        /// <returns>
        /// A token that can be used to access cloud services.
        /// </returns>
        Task<string> GetAccessTokenAsync(string tokenEndpoint, string tenantUrl);
    }
}
