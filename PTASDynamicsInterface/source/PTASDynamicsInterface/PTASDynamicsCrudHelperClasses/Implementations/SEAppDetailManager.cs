// <copyright file="SEAppDetailManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Implementations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Parcel manager implementation.
    /// </summary>
    public class SEAppDetailManager : ISEAppDetailManager
    {
        private const string TableName = "ptas_seappdetails";

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppDetailManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public SEAppDetailManager(CRMWrapper crmWrapper)
        {
            this.CrmWrapper = crmWrapper;
        }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <inheritdoc/>
        public virtual async Task<List<DynamicsSeniorExemptionApplicationDetail>> GetSEAppDetailFromSEAppId(string sEAppId) =>
            await this.ExecuteQuery($"$filter=_ptas_seapplicationid_value eq '{sEAppId}'");

        private async Task<List<DynamicsSeniorExemptionApplicationDetail>> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsSeniorExemptionApplicationDetail>(TableName, query);
            List<DynamicsSeniorExemptionApplicationDetail> resultList = new List<DynamicsSeniorExemptionApplicationDetail>();

            if (results != null)
            {
                foreach (var item in results)
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }
    }
}
