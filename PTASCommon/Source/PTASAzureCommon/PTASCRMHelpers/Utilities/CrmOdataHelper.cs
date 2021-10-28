// <copyright file="CrmOdataHelper.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers.Utilities
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using Newtonsoft.Json;

    using PTASCRMHelpers.Exceptions;

    /// <summary>
    /// CrmOdataHelper: Makes the connection with CRM Dynamics using Odata.
    /// </summary>
    public class CrmOdataHelper
    {
        private readonly string authUri;
        private readonly string crmUri;
        private readonly ITokenManager tokenManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmOdataHelper"/> class.
        /// </summary>
        /// <param name="crmUri">URI to the dynamics CRM interface.</param>
        /// <param name="authUri">URI to the authorization interface.</param>
        /// <param name="clientId">Client ID to connect to.</param>
        /// <param name="clientSecret">Client secret for connections.</param>
        /// <param name="tokenManager">Token manager.</param>
        public CrmOdataHelper(string crmUri, string authUri, string clientId, string clientSecret, ITokenManager tokenManager = null)
        {
            this.crmUri = crmUri;
            this.authUri = authUri;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.tokenManager = tokenManager;
        }

        private string ClientId { get; }

        private string ClientSecret { get; }

        /// <summary>
        /// GetResponseString: It gets the message response from a HttpResponseMessage.
        /// </summary>
        /// <param name="responseMessage">It contains the response message.</param>
        /// <returns>A string with the response.</returns>
        public static async Task<string> GetResponseString(HttpResponseMessage responseMessage)
        {
            string responseStr = await responseMessage.Content.ReadAsStringAsync();
            return responseMessage.IsSuccessStatusCode ? responseStr : null;
        }

        /// <summary>
        /// CrmWebApiFormattedDeleteRequest:It formats the delete request.
        /// </summary>
        /// <param name="apiRequest">The API Request.</param>
        /// <returns>The result of  httpClient.GetAsync.</returns>
        public async Task<HttpResponseMessage> CrmWebApiFormattedDeleteRequest(string apiRequest)
        {
            string fullURL = this.crmUri + apiRequest;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    this.SetupHeaders(httpClient);
                    return await httpClient.DeleteAsync(fullURL);
                }
            }
            catch (HttpRequestException ex)
            {
                string error = string.Format("Error trying to delete data from Dynamics.  Can't connect to the URL: {0}", fullURL);
                throw new DynamicsHttpRequestException(error, ex);
            }
        }

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

                    // Console.WriteLine($"Calling: {fullURL}");
                    return await httpClient.GetAsync(fullURL);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new DynamicsHttpRequestException($"Error trying to get data from Dynamics.  Can't connect to the URL: {fullURL}", ex);
            }
        }

        /// <summary>
        /// CrmWebApiFormattedNtoNPostRequest:It formats the get request for N to N relationships.
        /// </summary>
        /// <typeparam name="T">The JSON class of the entity.</typeparam>
        /// <param name="queryStr">The query string.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="keyValue">The keystr to be used in the filter.</param>
        /// <param name="navigationPropertyStr">The navigationPropertyStr to be used as name of the navigation property.</param>
        /// <returns>The result of httpClient.GetAsync.</returns>
        public async Task<HttpResponseMessage> CrmWebApiFormattedNtoNPostRequest<T>(string queryStr, T entity, string keyValue, string navigationPropertyStr)
        {
            // Serialize our concrete class into a JSON String
            var stringNewMetadata = JsonConvert.SerializeObject(entity);
            var fullRoute = $"{this.crmUri}{queryStr}({keyValue})/{navigationPropertyStr}/$ref";

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringNewMetadata, Encoding.UTF8, "application/json");
            try
            {
                using (var httpClient = new HttpClient())
                {
                    this.SetupHeaders(httpClient);
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(fullRoute, httpContent);
                    return httpResponseMessage;
                }
            }
            catch (HttpRequestException ex)
            {
                string error = string.Format("Error trying to post an N to N relationship data from Dynamics.  Can't connect to the URL: {0}", fullRoute);
                throw new DynamicsHttpRequestException(error, ex);
            }
        }

        /// <summary>
        /// This method formats the query string about the entity T, it filters by keystr.
        /// </summary>
        /// <typeparam name="T">The JSON class of the entity.</typeparam>
        /// <param name="queryStr">The query string.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="keyStr">The keystr to be used in the filter.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> CrmWebApiFormattedPatchRequest<T>(string queryStr, T entity, string keyStr)
        {
            // Serialize our concrete class into a JSON String
            var stringNewMetadata = JsonConvert.SerializeObject(entity);

            var fullRoute = $"{this.crmUri}{queryStr}({keyStr})";

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringNewMetadata, Encoding.UTF8, "application/json");
            try
            {
                using (var httpClient = new HttpClient())
                {
                    this.SetupHeaders(httpClient);
                    HttpResponseMessage httpResponseMessage = await httpClient.PatchAsync(fullRoute, httpContent);
                    return httpResponseMessage;
                }
            }
            catch (HttpRequestException ex)
            {
                string error = string.Format("Error trying to patch data from Dynamics.  Can't connect to the URL: {0}", fullRoute);
                throw new DynamicsHttpRequestException(error, ex);
            }
        }

        /// <summary>
        /// CrmWebApiFormattedGetRequest:It formats the get request.
        /// </summary>
        /// <param name = "apiRequest" > The API Request.</param>
        /// <param name = "toSave" > The Object to save.</param>
        /// <param name = "selectFields" > The selected fields in the api request.</param>
        /// <returns>The result of httpClient.PostAsync.</returns>
        public async Task<HttpResponseMessage> CrmWebApiFormattedPostRequest(string apiRequest, object toSave, string selectFields)
        {
            // Serialize our concrete class into a JSON String
            var stringNewMetadata = JsonConvert.SerializeObject(toSave);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            if (string.IsNullOrEmpty(selectFields))
            {
                selectFields = string.Empty;
            }
            else
            {
                selectFields = $"?$select={selectFields}";
            }

            string fullRoute = this.crmUri + apiRequest + selectFields;
            try
            {
                var httpContent = new StringContent(stringNewMetadata, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    this.SetupHeaders(httpClient);
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(fullRoute, httpContent);
                    return httpResponseMessage;
                }
            }
            catch (HttpRequestException ex)
            {
                string error = string.Format("Error trying to post data from Dynamics.  Can't connect to the URL: {0}", fullRoute);
                throw new DynamicsHttpRequestException(error, ex);
            }
        }

        /// <summary>
        /// Sets up the header for an HTTP client.
        /// </summary>
        /// <param name="httpClient">Client to setup.</param>
        public void SetupHeaders(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(this.crmUri);
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations = *");
            httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(@"*/*"));
            string tokenStr = this.GetTokenUsingClientIdSecret();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                tokenStr);
        }

        /// <summary>
        /// GetTokenUsingClientIdSecret: It gets the token for the connection with OData/Dynamics.
        /// </summary>
        /// <returns>The token for the connection.</returns>
        private string GetTokenUsingClientIdSecret()
        {
            var tk = this.tokenManager?.TokenStr;
            if (!string.IsNullOrEmpty(tk))
            {
                return tk;
            }

            try
            {
                var authority = this.authUri;
                var authContext = new AuthenticationContext(authority, false);
                var credentials = new ClientCredential(this.ClientId, this.ClientSecret);
                var tokenResult = authContext.AcquireTokenAsync(this.crmUri, credentials).Result;
                this.tokenManager?.SetToken(tokenResult.AccessToken, tokenResult.ExpiresOn.DateTime);
                return tokenResult.AccessToken;
            }
            catch (AdalServiceException ex)
            {
                string error = string.Format($"Error trying to authenticate credentials from Dynamics odata service. (Error code: {ex.ErrorCode}, error message: {ex.Message})");
                throw new DynamicsHttpRequestException(error, ex);
            }
            catch (System.AggregateException ex) when (ex.InnerException is AdalServiceException)
            {
                var ex1 = ex.InnerException as AdalServiceException;
                string error = string.Format($"Azure Active Directory Authentication Exception. (Error code: {ex1.ErrorCode}, error message: {ex1.Message})");
                throw new DynamicsHttpRequestException(error, ex1);
            }
        }
    }
}