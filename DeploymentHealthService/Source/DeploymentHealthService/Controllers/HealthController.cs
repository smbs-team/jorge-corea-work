namespace DeploymentHealthService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using DeploymentHealthService.Model;
    using DeploymentHealthService.TokenProvider;
    using ILinxSoapImport;
    using ILinxSoapImport.Exceptions;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASCRMHelpers;
    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// Controller that checks environment health.
    /// </summary>
    public class HealthController : ApiController
    {
        /// <summary>
        /// The account key setting.
        /// </summary>
        private const string BlobAccountKeySetting = "accountkey";

        /// <summary>
        /// The API route value.
        /// </summary>
        private const string ApiRouteValue = "/api/data/v9.1/";

        /// <summary>
        /// The i linx helper.
        /// </summary>
        private readonly IILinxHelper iLinxHelper;

        /// <summary>
        /// The configuration parameters.
        /// </summary>
        private readonly IConfigParams configParams;

        /// <summary>
        /// The configuration parameters.
        /// </summary>
        private readonly IDynamicsConfigurationParams dynamicsConfigurationParams;

        /// <summary>
        /// Gets the CRM helper.
        /// </summary>
        /// <value>
        /// The CRM helper.
        /// </value>
        private readonly CrmOdataHelper crmHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController" /> class.
        /// </summary>
        /// <param name="iLinxHelper">The ilinx helper.</param>
        /// <param name="configParams">The configuration parameters.</param>
        /// <param name="dynamicsConfigurationParams">The dynamics configuration parameters.</param>
        public HealthController(IILinxHelper iLinxHelper, IConfigParams configParams, IDynamicsConfigurationParams dynamicsConfigurationParams)
        {
            this.iLinxHelper = iLinxHelper;
            this.configParams = configParams;
            this.dynamicsConfigurationParams = dynamicsConfigurationParams;

            this.crmHelper = new CrmOdataHelper(dynamicsConfigurationParams.CRMUri, dynamicsConfigurationParams.AuthUri, dynamicsConfigurationParams.ClientId, dynamicsConfigurationParams.ClientSecret);
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>A string with the health status.</returns>
        [HttpGet]
        public HealthResults Get()
        {
            HealthResults healthResults = new HealthResults();
            List<HealthResult> resultsList = new List<HealthResult>();
            resultsList.Add(new HealthResult()
            {
                Item = "Health Call",
                Status = "Success"
            });

            this.TestConfigurationParameters(resultsList);
            this.TestBlobStorageConnection(resultsList, "Blob Storage", this.configParams.BlobStorageConnectionString, this.configParams.BlobStorageContainer);
            this.TestBlobStorageConnection(resultsList, "Premium Blob Storage", this.configParams.PremiumBlobStorageConnectionString, "tilecachecontainer");
            this.TestPremiumBlobStorageConnection(resultsList);
            this.TestSqlServerConnection(resultsList);
            this.TestIlinxConnection(resultsList);
            this.TestDynamicsConnection(resultsList);
            this.TestMapboxConnection(resultsList);
            this.TestApiManagementParemeters(resultsList);

            healthResults.Results = resultsList.ToArray();

            return healthResults;
        }

        /// <summary>
        /// Tests the configuration parameters.
        /// </summary>
        /// <param name="results">The results.</param>
        private void TestConfigurationParameters(List<HealthResult> results)
        {
            string missingParameters = string.Empty;
            if (string.IsNullOrWhiteSpace(this.configParams.ActivationId))
            {
                missingParameters += nameof(this.configParams.ActivationId) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.ApplicationName))
            {
                missingParameters += nameof(this.configParams.ApplicationName) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.EdmsSoapServicesEndpoint))
            {
                missingParameters += nameof(this.configParams.EdmsSoapServicesEndpoint) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.Password))
            {
                missingParameters += nameof(this.configParams.Password) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.UserName))
            {
                missingParameters += nameof(this.configParams.UserName) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.BlobStorageConnectionString))
            {
                missingParameters += nameof(this.configParams.BlobStorageConnectionString) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.PremiumBlobStorageConnectionString))
            {
                missingParameters += nameof(this.configParams.PremiumBlobStorageConnectionString) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.SqlServerConnectionString))
            {
                missingParameters += nameof(this.configParams.SqlServerConnectionString) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.BlobStorageConnectionString))
            {
                missingParameters += nameof(this.configParams.BlobStorageConnectionString) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.FinalizerUrl))
            {
                missingParameters += nameof(this.configParams.FinalizerUrl) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.BlobStorageContainer))
            {
                missingParameters += nameof(this.configParams.BlobStorageContainer) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.configParams.CognitiveSubscriptionKey))
            {
                missingParameters += nameof(this.configParams.CognitiveSubscriptionKey) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.dynamicsConfigurationParams.ClientId))
            {
                missingParameters += nameof(this.dynamicsConfigurationParams.ClientId) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.dynamicsConfigurationParams.ClientSecret))
            {
                missingParameters += nameof(this.dynamicsConfigurationParams.ClientSecret) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.dynamicsConfigurationParams.CRMUri))
            {
                missingParameters += nameof(this.dynamicsConfigurationParams.CRMUri) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.dynamicsConfigurationParams.MapboxUri))
            {
                missingParameters += nameof(this.dynamicsConfigurationParams.MapboxUri) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.dynamicsConfigurationParams.MBToken))
            {
                missingParameters += nameof(this.dynamicsConfigurationParams.MBToken) + ",";
            }

            if (string.IsNullOrWhiteSpace(this.dynamicsConfigurationParams.AuthUri))
            {
                missingParameters += nameof(this.dynamicsConfigurationParams.AuthUri) + ",";
            }

            if (string.IsNullOrEmpty(missingParameters))
            {
                results.Add(new HealthResult()
                {
                    Item = "Pipe parameter check",
                    Status = "Success"
                });
            }
            else
            {
                results.Add(new HealthResult()
                {
                    Item = "Pipe parameter check",
                    Status = "Failed",
                    Error = "Missing parameters: " + missingParameters
                });
            }
        }

        private void TestBlobStorageConnection(List<HealthResult> results, string blobStorageName, string connectionString, string blobContainer)
        {
            try
            {
                CloudBlobClient blobClient = Task.Run(async () =>
                {
                        return await this.GetCloudBlobClient(connectionString);
                }).Result;

                CloudBlobContainer container = blobClient.GetContainerReference(blobContainer);
                if (container != null)
                {
                    BlobContainerPermissions permissions = container.GetPermissions();

                    results.Add(new HealthResult()
                    {
                        Item = blobStorageName + " connection",
                        Status = "Success"
                    });
                    return;
                }

                results.Add(new HealthResult()
                {
                    Item = blobStorageName + " connection",
                    Status = "Failure",
                    Error = "Could not retrieve container"
                });
            }
            catch (System.Exception ex)
            {
                results.Add(new HealthResult()
                {
                    Item = blobStorageName + " connection",
                    Status = "Failure",
                    Error = ex.ToString()
                });
            }
        }

        /// <summary>
        /// Gets the cloud BLOB client.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// The blob client.
        /// </returns>
        /// <exception cref="System.FormatException">Thrown in case the connection string is invalid.</exception>
        private async Task<CloudBlobClient> GetCloudBlobClient(string connectionString)
        {
            if (connectionString.ToLower().Contains(HealthController.BlobAccountKeySetting))
            {
                CloudStorageAccount storageAccount;
                if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
                {
                    return storageAccount.CreateCloudBlobClient();
                }

                return null;
            }
            else
            {
                AzureTokenProvider tokenProvider = new AzureTokenProvider();
                string tokenCredential = await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.StorageTokenEndpoint);
                var storageCredentials = new StorageCredentials(new TokenCredential(tokenCredential));

                CloudBlobClient blobClient = new CloudBlobClient(new Uri(connectionString), storageCredentials);
                return blobClient;
            }
        }

        private void TestPremiumBlobStorageConnection(List<HealthResult> results)
        {
            results.Add(new HealthResult()
            {
                Item = "Premium Blob storage connection",
                Status = "Not Tested"
            });
        }

        private void TestSqlServerConnection(List<HealthResult> results)
        {
            string accessToken = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(this.configParams.SqlServerConnectionString))
                {
                    Task.Run(async () =>
                    {
                        AzureTokenProvider tokenProvider = new AzureTokenProvider();
                        await conn.SetBearerToken(this.configParams.SqlServerConnectionString, tokenProvider);
                    }).Wait();

                    accessToken = conn.AccessToken;

                    conn.Open();

                    // WR: added isNull and left outer join to get data when not found on primary.
                    string query = $@"select count(*) from [dbo].[SqlServerPatchManifest]";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var count = reader[0].ToString();
                                results.Add(new HealthResult()
                                {
                                    Item = "Sql Server Connection",
                                    Status = "Success (" + count + " records)"
                                });
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                string error = ex.ToString();
                if (error.Contains("Login failed for user"))
                {
                    error += ", Access token: " + accessToken;
                }

                results.Add(new HealthResult()
                {
                    Item = "Sql Server Connection",
                    Status = "Failed",
                    Error = error
                });
            }
        }

        private void TestIlinxConnection(List<HealthResult> results)
        {
            try
            {
                this.iLinxHelper.FetchDocument(System.Guid.NewGuid().ToString());
            }
            catch (DocumentNotFoundException)
            {
                results.Add(new HealthResult()
                {
                    Item = "Ilinx connection",
                    Status = "Success"
                });
            }
            catch (System.Exception ex)
            {
                results.Add(new HealthResult()
                {
                    Item = "Ilinx connection",
                    Status = "Failed",
                    Error = ex.ToString()
                });
            }
        }

        private void TestMapboxConnection(List<HealthResult> results)
        {
            try
            {
                var mbtoken = this.dynamicsConfigurationParams.MBToken;
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                var searchEncode = System.Web.HttpUtility.UrlEncode("225567");

                var response = Task.Run(async () =>
                {
                    return await client.GetAsync($"{this.dynamicsConfigurationParams.MapboxUri}{searchEncode}.json?limit=3&access_token={mbtoken}");
                }).Result;

                if (!response.IsSuccessStatusCode)
                {
                    results.Add(new HealthResult()
                    {
                        Item = "Mapbox connection",
                        Status = "Failed",
                        Error = "Failure status code"
                    });

                    return;
                }

                results.Add(new HealthResult()
                {
                    Item = "Mapbox connection",
                    Status = "Success"
                });
            }
            catch (System.Exception ex)
            {
                results.Add(new HealthResult()
                {
                    Item = "Mapbox connection",
                    Status = "Failed",
                    Error = ex.ToString()
                });
            }
        }

        private void TestDynamicsConnection(List<HealthResult> results)
        {
            try
            {
                string tableName = "ptas_counties";
                var queryStr = $"{HealthController.ApiRouteValue}{tableName}?$count=true&$top=10";

                var response = Task.Run(async () =>
                {
                    return await this.crmHelper.CrmWebApiFormattedGetRequest(queryStr);
                }).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = Task.Run(async () =>
                    {
                        return await response.Content.ReadAsStringAsync();
                    }).Result;

                    results.Add(new HealthResult()
                    {
                        Item = "Dynamics connection",
                        Status = "Failed",
                        Error = "Failure status code:" + (responseContent ?? "no Content")
                    });

                    return;
                }

                results.Add(new HealthResult()
                {
                    Item = "Dynamics connection",
                    Status = "Success"
                });
            }
            catch (System.Exception ex)
            {
                results.Add(new HealthResult()
                {
                    Item = "Dynamics connection",
                    Status = "Failed",
                    Error = ex.ToString()
                });
            }
        }

        private void TestApiManagementParemeters(List<HealthResult> results)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = this.configParams.DynamicsApiURL + "/Counties";

                var response = Task.Run(async () =>
                {
                    return await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                }).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    results.Add(new HealthResult()
                    {
                        Item = "Service to Service API Management",
                        Status = "Success"
                    });
                }
                else
                {
                    results.Add(new HealthResult()
                    {
                        Item = "Service to Service API Management",
                        Status = "Failed",
                        Error = "Unexpected status code"
                    });
                }
            }
            catch (System.Exception ex)
            {
                results.Add(new HealthResult()
                {
                    Item = "Service to Service API Management",
                    Status = "Failed",
                    Error = ex.ToString()
                });
            }
        }
    }
}