// <copyright file="ParcelManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Parcel manager implementation.
    /// </summary>
    public class ParcelManager : IParcelManager
    {
        private const string TableName = "ptas_parceldetails";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParcelManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        /// <param name="config">configuration params from dependency.</param>
        public ParcelManager(CRMWrapper crmWrapper, IConfigurationParams config)
        {
            this.CrmWrapper = crmWrapper;
            this.Config = config;
        }

        /// <summary>
        /// Gets the Global configuration params.
        /// </summary>
        private IConfigurationParams Config { get; }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <inheritdoc/>
        public async Task<List<ParcelLookupResult>> GetParcelFromAccountNumber(string accountNumber) =>
          await this.ExecuteQueryParcelLookup($"$select=ptas_parceldetailid,ptas_name,ptas_acctnbr,ptas_address,ptas_streettype,ptas_district,ptas_zipcode,ptas_namesonaccount&$filter=startswith(ptas_acctnbr,'{accountNumber}') and statecode eq 0 and statuscode eq 1&$top=5", "ptas_acctnbr");

        /// <inheritdoc/>
        public async Task<List<ParcelLookupResult>> GetParcelFromAddress(string[] addressParts)
        {
            var conditions = addressParts.Select(addrPart => $"contains(ptas_address,'{addrPart}')").ToList();
            var allConditions = $"({string.Join(" and ", conditions)}) and statecode eq 0 and statuscode eq 1";
            return await this.ExecuteQueryParcelLookup($"$select=ptas_parceldetailid,ptas_name,ptas_acctnbr,ptas_address,ptas_streettype,ptas_district,ptas_zipcode,ptas_namesonaccount&$filter={allConditions}&$top=5", "ptas_address");
        }

        /// <inheritdoc />
        public async Task<List<ParcelLookupResult>> GetParcelFromCommercialProject(string[] searchParts)
        {
            var conditions = searchParts.Select(addrPart => $"contains(ptas_name,'{addrPart}')").ToList();
            conditions.Add("ptas_snapshottype eq null");
            var cond = string.Join(" and ", conditions);
            var expand = "$expand=ptas_parcelid";
            var x = await this.CrmWrapper.ExecuteGet<IntermediateResult>("ptas_condocomplexes", $"$filter={cond}&{expand}&$top=5");
            if (x == null)
            {
                return new List<ParcelLookupResult>();
            }

            return x.Select(r => new ParcelLookupResult(r.Ptas_parcelid, "ptas_condocomplex", r.Name)).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<ParcelLookupResult>> GetParcelFromParcelId(string parcelId) =>
            await this.ExecuteQueryParcelLookup($"$select=ptas_parceldetailid,ptas_name,ptas_acctnbr,ptas_address,ptas_streettype,ptas_district,ptas_zipcode,ptas_namesonaccount&$filter=startswith(ptas_name,'{parcelId}') and statecode eq 0 and statuscode eq 1&$top=5", "ptas_name");

        /// <summary>
        /// Gets a list of matches for the address by geocoding of the addres using mapbox service.
        /// </summary>
        /// <param name="searchValue">serach criteria to lookup.</param>
        /// <returns>list of FormattedAddress with the resulting parced features of mapbox, or null if not found any. This method return the 3 closest matches.</returns>
        public async Task<List<FormattedAddress>> LookupAddress(string searchValue)
        {
            List<FormattedAddress> result = new List<FormattedAddress>();

            var mbtoken = this.Config.MBToken;
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            var searchEncode = System.Web.HttpUtility.UrlEncode(searchValue);
            var response = await client.GetAsync($"{this.Config.MapboxUri}{searchEncode}.json?limit=3&access_token={mbtoken}");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stringResp = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(stringResp))
                {
                    var respJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(stringResp);
                    if (respJson["features"] is Newtonsoft.Json.Linq.JArray features && features.Count > 0)
                    {
                        foreach (Newtonsoft.Json.Linq.JObject item in features)
                        {
                            result.Add(new FormattedAddress(item));
                        }
                    }

                    return result;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<List<ParcelLookupResult>> LookupParcel(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return null;
            }

            var searchParts = searchValue.Split(new char[] { ' ', ',', '.', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            List<ParcelLookupResult> found = null;
            if (searchParts.Length == 1 && searchValue.CouldBeId())
            {
                // note the couldbeid eliminates obvious cases
                // where the string is not in the id format.
                found = await this.GetParcelFromParcelId(searchValue);
                if ((found?.Count ?? 0) > 0)
                {
                    return found;
                }

                found = await this.GetParcelFromAccountNumber(searchValue);

                if ((found?.Count ?? 0) > 0)
                {
                    return found;
                }
            }

            Task<List<ParcelLookupResult>> t1 = this.GetParcelFromAddress(searchParts);
            Task<List<ParcelLookupResult>> t2 = this.GetParcelFromCommercialProject(searchParts);
            var results = (await Task.WhenAll(t1, t2)).SelectMany(t => t).Distinct(new CompareParcels());
            return results.ToList();
        }

        /// <summary>
        /// Execute a query over Parcel Details entity.
        /// </summary>
        /// <param name="query">the query to be executed.</param>
        /// <param name="source">the query to result source.</param>
        /// <returns>Parcel detail instance.</returns>
        private async Task<List<ParcelLookupResult>> ExecuteQueryParcelLookup(string query, string source)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsParcelDetail>(TableName, query);
            List<ParcelLookupResult> resultList = new List<ParcelLookupResult>();

            if (results != null)
            {
                foreach (var item in results)
                {
                    resultList.Add(new ParcelLookupResult(item, source, null));
                }
            }

            return resultList;
        }

        private class CompareParcels : EqualityComparer<ParcelLookupResult>
        {
            public override bool Equals(ParcelLookupResult x, ParcelLookupResult y) =>
                x.ParcelDetailId == y.ParcelDetailId;

            public override int GetHashCode(ParcelLookupResult obj) => obj.ParcelDetailId.GetHashCode();
        }

        private class IntermediateResult
        {
            [JsonProperty("ptas_name")]
            public string Name { get; set; }

            [JsonProperty("ptas_parcelid")]
            public DynamicsParcelDetail Ptas_parcelid { get; set; }
        }

        /////// <summary>
        /////// Execute a query over Parcel Details entity.
        /////// </summary>
        /////// <param name="query">the query to be executed.</param>
        /////// <returns>Parcel detail instance.</returns>
        ////private async Task<DynamicsParcelDetail> ExecuteQuery(string query)
        ////{
        ////    var results = await this.CrmWrapper.ExecuteGet<DynamicsParcelDetail>(TableName, query);
        ////    return results?.FirstOrDefault();
        ////}
    }
}