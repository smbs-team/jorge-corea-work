// <copyright file="Odata.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using PTASCRMHelpers;
    using PTASCRMHelpers.Exceptions;
    using PTASCRMHelpers.Utilities;
    using PTASketchFileMigratorConsoleApp;
    using PTASSketchFileMigratorConsoleApp;

    /// <summary>Wrapper class for the OData functionality.</summary>
    /// <seealso cref="PTASExportConnector.SDK.IOdata" />
    public class Odata : IOdata
    {
        private readonly ISettingsManager settings;
        private CrmOdataHelper odata;

        /// <summary>
        /// Initializes a new instance of the <see cref="Odata"/> class.
        /// </summary>
        /// <param name="settings">Settings object.</param>
        /// <param name="tokenManager">Token for ODATA.</param>
        public Odata(ISettingsManager settings, ITokenManager tokenManager)
        {
            this.settings = settings;
            this.odata = new CrmOdataHelper(settings.ReadSetting("crmUri"), settings.ReadSetting("authUri"), settings.ReadSetting("clientId"), settings.ReadSetting("clientSecret"), tokenManager);
        }

        /// <inheritdoc />
        public async Task<OdataResponse> Get(string entityName, string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("query is null or empty.");
            }

            try
            {
                var apiRoute = this.settings.ReadSetting("apiRoute");

                var queryStr = $"{apiRoute}{entityName}s?{query}";
                var response = await this.odata.CrmWebApiFormattedGetRequest(queryStr);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OdataResponse>(json);
            }
            catch (DynamicsHttpRequestException ex)
            {
                throw new DynamicsHttpRequestException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get failed", ex.Message);
                throw new Exception();
            }
        }
    }
}
