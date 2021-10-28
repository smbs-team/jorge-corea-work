// <copyright file="DataServices.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using PTASSketchFileMigratorConsoleApp;

    /// <summary>Defines methods for PTAS data services.</summary>
    public class DataServices : IDataServices
    {
        private readonly ISettingsManager settings;

        /// <summary>Initializes a new instance of the <see cref="DataServices"/> class.</summary>
        /// <param name="settings">The settings.</param>
        public DataServices(ISettingsManager settings)
        {
            this.settings = settings;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SetIsOfficial(Dictionary<string, string> data)
        {
            var dataServiceURL = this.settings.ReadSetting("dataServiceURL");
            var apiRoute = this.settings.ReadSetting("setOfficialSketchAPIRoute");
            var isOfficialToken = this.settings.ReadSetting("isOfficialToken");
            var stringNewMetadata = JsonConvert.SerializeObject(data);
            var fullRoute = $"{dataServiceURL}{apiRoute}";
            try
            {
                var httpContent = new StringContent(stringNewMetadata, Encoding.UTF8, "application/json");
                using var httpClient = new HttpClient();
                this.SetupHeaders(httpClient, isOfficialToken, dataServiceURL);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(fullRoute, httpContent);
                return httpResponseMessage;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        private void SetupHeaders(HttpClient httpClient, string tokenStr, string baseAddress)
        {
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                tokenStr);
        }
    }
}
