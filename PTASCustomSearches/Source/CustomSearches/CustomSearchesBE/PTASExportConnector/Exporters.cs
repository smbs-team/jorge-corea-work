// <copyright file="Exporters.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security;
    using System.Threading.Tasks;
    using ConnectorService;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PTASCRMHelpers.Exceptions;
    using PTASExportConnector.Exceptions;
    using PTASExportConnector.SDK;
    using PTASServicesCommon.TokenProvider;

    /// <summary>It has the code for exporting from the device to the back-end.</summary>
    /// <seealso cref="PTASExportConnector.IExporters" />
    public class Exporters : IExporters
    {
        private readonly IDbService dbService;
        private readonly IOdata oData;
        private string crmUri;
        private string authUri;
        private string clientId;
        private string clientSecret;
        private string sqlConnectionString;
        private string webApi;
        private string token;
        private string pkName;
        private ClientCredential principalCredentials;

        /// <summary>Initializes a new instance of the <see cref="Exporters"/> class.</summary>
        /// <param name="dbService">  It has back-end related code.</param>
        /// <param name="fileSystem">  Abstraction of the file system for easier testing.</param>
        /// <param name="oData">  It has the OData related code.</param>
        /// <param name="webApi">Web Api.</param>
        /// <param name="crmUri">Dynamics URI.</param>
        /// <param name="authUri">Auth URI.</param>
        /// <param name="clientId">ClientID.</param>
        /// <param name="clientSecret">ClientSecret.</param>
        /// <param name="sqlConnectionString">SQL Connection.</param>
        public Exporters(DbService dbService, Odata oData, string crmUri, string authUri, string clientId, string clientSecret, string sqlConnectionString, string webApi, ClientCredential principalCredentials)
        {
            if (dbService == null)
            {
                throw new ExportConnectorException("Error while initiating the export process.", StatusCodes.Status500InternalServerError, new ArgumentNullException("dbService"));
            }

            if (oData == null)
            {
                throw new ExportConnectorException("Error while initiating the export process.", StatusCodes.Status500InternalServerError, new ArgumentNullException("oData"));
            }

            this.dbService = dbService;
            this.oData = oData;
            this.crmUri = crmUri;
            this.authUri = authUri;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.sqlConnectionString = sqlConnectionString;
            this.webApi = webApi;
            this.principalCredentials = principalCredentials;

            this.oData.Init(this.crmUri, this.authUri, this.clientId, this.clientSecret);
        }

        /// <summary>Exports the retrieved data from the related device GUID and entity name.</summary>
        /// <param name="connector">Used to get the entity data.</param>
        /// <param name="deviceGuid">The device GUID to filter.</param>
        /// <param name="entityName">The name of the entity to retrieve the data.</param>
        /// <returns>A list of change-sets ids.</returns>
        public async System.Threading.Tasks.Task<List<string>> ExportAsync(Connector connector, Guid deviceGuid, string entityName)
        {
            if (connector == null)
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentNullException("connector"));
            }

            if (deviceGuid == Guid.Empty)
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentException("Device GUID is empty."));
            }

            if (string.IsNullOrEmpty(entityName))
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentException("Entity name is null or empty."));
            }

            var webapiurl = Environment.GetEnvironmentVariable("webapiurl");

            if (string.IsNullOrEmpty(webapiurl))
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentException("webapiurl is null or empty."));
            }

            var changesetIds = new List<string>();
            var model = this.dbService.GetDatabaseModel();
            var entityDefinition = model.Entities.Where(e => e.Name == entityName).FirstOrDefault();

            var entityList = connector.GetModifiedEntityData(null, entityName, deviceGuid);
            if (entityList.Count != 0)
            {
            var fieldName = entityDefinition.Attributes.Where(a => a.IsClientKey == true).Select(a => a.Name).FirstOrDefault().ToString();

            foreach (var entity in entityList)
            {
                var primaryKey = this.GetGuid((Guid)entity["guid_mb"], entityName, fieldName);
                var json = this.CreateJson(entity, entityDefinition, fieldName, entityName, (string)entity["rowStatus_mb"]);

                if ((string)entity["rowStatus_mb"] == "I")
                {
                    var response = this.oData.Create(webapiurl + await this.EntityDefinition(entityName,  webapiurl), json, string.Empty);
                    var r = response.Result.Headers.Location.LocalPath.ToString().Split("(")[1].Replace(")", string.Empty).ToString();
                    this.dbService.UpdateEntityKeys((Guid)entity["guid_mb"], entityName, fieldName, Guid.Parse(r));
                }
                else if ((string)entity["rowStatus_mb"] == "U")
                {
                    var response = await this.oData.Update(webapiurl + await this.EntityDefinition(entityName, webapiurl), json, primaryKey);
                }

                #region DELETE
                // else
                // {
                //    await this.oData.Delete(webapiurl + await this.EntityDefinition(entityName, webapiurl) + "(" + primaryKey + ")");
                // } 
                #endregion
                if (!changesetIds.Contains(entity["changesetId_mb"].ToString()))
                {
                    changesetIds.Add(entity["changesetId_mb"].ToString());
                }
            }
            }

            return changesetIds;
        }

        private string CreateJson(Dictionary<string, object> rowsNews, EntityModel entityDefinition, string fieldName, string entityName, string rowStatus)
        {
            JObject o = new JObject();
            Dictionary<string, object> rows = rowsNews;
            var primaryKey = this.GetGuid((Guid)rowsNews["guid_mb"], entityName, fieldName);
            Dictionary<string, object> oldValues = this.GetOldFields(entityName, primaryKey);

            if (rowStatus == "U")
            {
            foreach (var newRow in rows)
            {
                foreach (var oldRow in oldValues)
                {
                    if (oldRow.Key.ToLower().Equals(newRow.Key.ToLower()))
                    {
                        if (oldRow.Value.ToString().ToLower().Equals(newRow.Value.ToString().ToLower()))
                        {
                            rows.Remove(newRow.Key);
                        }
                    }
                }
            }
            }

            foreach (var pair in rows)
            {
                if (pair.Key.Contains("fk_r") || pair.Key.ToLower().Equals(this.pkName) || !pair.Key.Contains("ptas_"))
                {
                    continue;
                }

                #region FK CODE
                // if (pair.Key.Contains("fk_r_"))
                // {
                //    var relationship = entityDefinition.Relationships.Where(r => r.Name == pair.Key.Replace("fk_", string.Empty)).FirstOrDefault();
                //    if (relationship == null)
                //    {
                //        relationship = entityDefinition.Relationships.Where(r => r.InverseName == pair.Key.Replace("fk_", string.Empty)).FirstOrDefault();
                //    }

                // var keyName = this.GetGuid((Guid)pair.Value, relationship.DestinationEntity, relationship.DestinationKey);
                // o.Add(relationship.SourceKey + "@odata.bind", "/" + this.entityNames[relationship.DestinationEntity] + "(" + keyName + ")");
                // Hay que construir la linea para enviar el valor obtenido por el GetGuid como si fuese el campo _Value
                // } 
                #endregion
                else if (!(pair.Key.Contains("_mb") || (entityDefinition.Name.ToLower().Equals("ptas_parceldetail") && (pair.Key.Equals("centerx") || pair.Key.Equals("centery")))))
                {
                    var attribute = entityDefinition.Attributes.Where(a => a.Name == pair.Key).FirstOrDefault();
                    if (attribute != null && !Convert.IsDBNull(pair.Value))
                    {
                        switch (attribute.AttributeType)
                        {
                            case "Integer 16":
                            case "Integer 32":
                                o.Add(pair.Key, (int)pair.Value);
                                break;
                            case "Integer 64":
                                o.Add(pair.Key, (long)pair.Value);
                                break;
                            case "Double":
                                o.Add(pair.Key, (double)pair.Value);
                                break;
                            case "Money":
                                o.Add(pair.Key, (double)pair.Value);
                                break;
                            case "Boolean":
                                o.Add(pair.Key, (bool)pair.Value);
                                break;
                            case "GUID":
                                o.Add(pair.Key, pair.Value.ToString());
                                break;
                            case "Date":
                                o.Add(pair.Key, (DateTime)pair.Value);
                                break;
                            default:
                                o.Add(pair.Key, (string)pair.Value);
                                break;
                        }
                    }
                    else
                    {
                        o.Add(pair.Key, null);
                    }
                }
            }

            return o.ToString();
        }

        private string GetGuid(Guid entityGuid, string entityKind, string keyName)
        {
            var guid = this.dbService.GetEntityKeys(entityGuid, entityKind);
            return guid[keyName].ToString();
        }

        /// <summary>
        /// Entity Definition of a entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="dynamicsPath">Path destiny.</param>
        /// <returns>The entity set name and the name of the primary id.</returns>
        private async Task<string> EntityDefinition(string entityName, string dynamicsPath)
        {
            var path = $"{this.crmUri + dynamicsPath}EntityDefinitions?$select=EntitySetName&$filter=LogicalName%20eq%20%27{entityName.ToLower()}%27";
            var uri = new Uri(path);
            var s = await this.GetContent(uri);
            dynamic deserialized = JsonConvert.DeserializeObject(s);
            JArray actualValues = deserialized.value;

            Dictionary<string, object> pluralD = new Dictionary<string, object>();

            foreach (JObject content in actualValues.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    pluralD.Add(prop.Name, prop.Value);
                }
            }

            string plural = pluralD.Values.FirstOrDefault().ToString();
            return plural;
        }

        /// <summary>
        /// Attempts to query dynamics using the provided URI.
        /// </summary>
        /// <param name="myUri">Uri for the request.</param>
        /// <returns>Response String.</returns>
        private async Task<string> GetContent(Uri myUri)
        {
            Console.WriteLine("Getting content from Uri: " + myUri.ToString());
            var client = new HttpClient();
            this.SetupHeaders(client);

            var response = await client.GetAsync(myUri);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        /// <summary>
        /// Setup request headers.
        /// </summary>
        /// <param name="httpClient">Client to setup.</param>
        private void SetupHeaders(HttpClient httpClient)
        {
            var crmUri = Environment.GetEnvironmentVariable("crmUri");
            httpClient.BaseAddress = new Uri(crmUri);
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

        /// <summary>
        /// GetTokenUsingClientIdSecret:It gets the toker for the connection with OData/Dynamics.
        /// </summary>
        /// <returns>The token for the connection.</returns>
        private string GetTokenUsingClientIdSecret()
        {
            try
            {
                var authContext = new AuthenticationContext(this.authUri, false);
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

        private Dictionary<string, object> GetOldFields(string entityName, string guid)
        {
            this.token = this.GetToken();

            using (SqlConnection con = new SqlConnection(this.sqlConnectionString))
            {
                con.AccessToken = this.token;
                con.Open();
                string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + entityName + "_Key" + "' AND TABLE_SCHEMA = 'dbo' AND COLUMN_NAME != 'guid_mb';";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    this.pkName = command.ExecuteScalar().ToString();
                }
            }



            Dictionary<string, object> entity = new Dictionary<string, object>();

            using (SqlConnection middleTierConnection = new SqlConnection(this.sqlConnectionString))
            {
                try
                {
                    middleTierConnection.AccessToken = this.token;
                    middleTierConnection.Open();
                    string query = $"SELECT * FROM {entityName}_Data d INNER JOIN {entityName}_Key k ON d.guid_mb = k.guid_mb AND k.{this.pkName} = '{guid}' INNER JOIN Changesets_mb c ON c.id_cgs = d.changesetId_mb AND c.changesetTypeId_cgs = 2 AND c.statusTypeId_cgs = 3";
                    using (SqlCommand command = new SqlCommand(query, middleTierConnection))
                    {
                        SqlDataReader middletierReader = command.ExecuteReader();
                        if (middletierReader.Read())
                        {
                            for (int i = 0; i < middletierReader.FieldCount; i++)
                            {
                                if (!entity.ContainsKey(middletierReader.GetName(i)))
                                {
                                    if (middletierReader.GetValue(i).GetType().Name.ToLower().Equals("double"))
                                    {
                                        double value = Math.Round(middletierReader.GetDouble(i), 15, MidpointRounding.AwayFromZero);
                                        entity.Add(middletierReader.GetName(i), value);
                                    }
                                    else
                                    {
                                        entity.Add(middletierReader.GetName(i), middletierReader.GetValue(i));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return entity;
            }

        /// <summary>Create or update token when expired.</summary>
        /// <param name="sqlConnectionString"> Connection for the token.</param>
        private string GetToken()
        {
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();

            string accessToken = Task.Run(async () =>
            {
                return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantId, this.principalCredentials);
            }).Result;

            return accessToken;
        }
    }
}
