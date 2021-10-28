// <copyright file="SEAppOtherPropManager.cs" company="King County">
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
    public class SEAppOtherPropManager : ISEAppOtherPropManager
    {
        private const string TableName = "ptas_seappotherprops";

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppOtherPropManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public SEAppOtherPropManager(CRMWrapper crmWrapper)
        {
            this.CrmWrapper = crmWrapper;
        }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <inheritdoc/>
        public async Task<List<DynamicsSEAppOtherProp>> GetSEAppOtherPropFromSEAppId(string sEAppId) =>
            await this.ExecuteQuery($"$filter=_ptas_seapplicationid_value eq '{sEAppId}'");

        private async Task<List<DynamicsSEAppOtherProp>> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsSEAppOtherProp>(TableName, query);
            List<DynamicsSEAppOtherProp> resultList = new List<DynamicsSEAppOtherProp>();

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
