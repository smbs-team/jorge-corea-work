// <copyright file="SEAppFinancialManager.cs" company="King County">
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
    public class SEAppFinancialManager : ISEAppFinancialManager
    {
        private const string TableName = "ptas_sefinancialformses";

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppFinancialManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public SEAppFinancialManager(CRMWrapper crmWrapper)
        {
            this.CrmWrapper = crmWrapper;
        }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <inheritdoc/>
        public async Task<List<DynamicsSeniorExemptionApplicationFinancial>> GetSEAppFinancialFromSEAppDetailId(string sEAppDetailId) =>
            await this.ExecuteQuery($"$filter=_ptas_seappdetailid_value eq '{sEAppDetailId}'");

        private async Task<List<DynamicsSeniorExemptionApplicationFinancial>> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(TableName, query);
            List<DynamicsSeniorExemptionApplicationFinancial> resultList = new List<DynamicsSeniorExemptionApplicationFinancial>();

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
