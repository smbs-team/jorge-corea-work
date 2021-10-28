namespace PTASCRMHelpers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;
    using PTASCRMHelpers.Exception;

    /// <summary>
    /// CrmOdataHelper: Makes the connection with CRM Dynamics using Odata.
    /// </summary>
    public class CrmOdataHelper
    {
        private readonly string crmUri;
        private readonly string authUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmOdataHelper"/> class.
        /// </summary>
        /// <param name="crmUri">URI to the dynamics CRM interface.</param>
        /// <param name="authUri">URI to the authorization interface.</param>
        /// <param name="clientId">Client ID to connect to.</param>
        /// <param name="clientSecret">Client secret for connections.</param>
        public CrmOdataHelper(string crmUri, string authUri, string clientId, string clientSecret)
        {
            this.crmUri = crmUri;
            this.authUri = authUri;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        private string ClientId { get; }

        private string ClientSecret { get; }

        /// <summary>
        /// CrmWebApiFormattedGetRequest:It formats the get request.
        /// </summary>
        /// <param name="apiRequest">The API Request.</param>
        /// <returns>The result of  httpClient.GetAsync.</returns>
        public async Task<HttpResponseMessage> CrmWebApiFormattedGetRequest(string apiRequest)
        {
            string fullURL = this.crmUri + apiRequest;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    this.SetupHeaders(httpClient);
                    return await httpClient.GetAsync(fullURL);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new DynamicsHttpRequestException($"Error trying to get data from Dynamics w.  Can't connect to the URL: {fullURL}", ex);
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
                var credentials = new ClientCredential(this.ClientId, this.ClientSecret);
                var tokenResult = authContext.AcquireTokenAsync(this.crmUri, credentials).Result;
                return tokenResult.AccessToken;
            }
            catch (AdalServiceException ex)
            {
                string error = string.Format($"Error trying to authenticate credentials from Dynamics odata service. (Error code: {ex.ErrorCode}, error message: {ex.Message})");
                throw new DynamicsHttpRequestException(error, ex);
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
