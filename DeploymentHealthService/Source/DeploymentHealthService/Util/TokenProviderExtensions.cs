namespace DeploymentHealthService.TokenProvider
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods to support token access in specific classes.
    /// </summary>
    public static class TokenProviderExtensions
    {
        /// <summary>
        /// Constant to identify password in a connection string.
        /// </summary>
        private const string ConnectionStringPasswordSection = "password";

        /// <summary>
        /// Constant to identify password in a connection string.
        /// </summary>
        private const string ConnectionStringPwdSection = "pwd";

        /// <summary>
        /// Sets the bearer token.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <returns>A Task.</returns>
        public static async Task SetBearerToken(this SqlConnection sqlConnection, string connectionString, AzureTokenProvider tokenProvider)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException(nameof(sqlConnection));
            }

            if (tokenProvider == null)
            {
                throw new ArgumentNullException(nameof(tokenProvider));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (sqlConnection != null)
            {
                // We only used bearer token when password is not provided
                if (!sqlConnection.ConnectionString.ToLower().Contains(TokenProviderExtensions.ConnectionStringPasswordSection) &&
                    !sqlConnection.ConnectionString.ToLower().Contains(TokenProviderExtensions.ConnectionStringPwdSection))
                {
                    sqlConnection.AccessToken = await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                }
            }
        }
    }
}
