using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PTASServicesCommon.TokenProvider;

namespace PTASSyncService.Utilities
{
    /// <summary>
    /// Utility related to SQL Token
    /// </summary>
    public static class SQLTokenUtility
    {
        /// <summary>
        /// Returns the token needed for the sql connection
        /// </summary>
        /// <param name="cnnString">the connection string</param>
        /// <returns>the token.</returns>
        public static string GetSQLToken(string cnnString)
        {
            string accessToken = null;
            if (!cnnString.ToLowerInvariant().Contains("password"))
            {
                string token = Task.Run(async () =>
                {
                    return await new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider().GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                }).Result;

                accessToken = token;
            }

            return accessToken;
        }
    }
}
