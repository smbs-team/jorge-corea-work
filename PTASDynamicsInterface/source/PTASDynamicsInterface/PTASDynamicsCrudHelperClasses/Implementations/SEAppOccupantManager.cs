// <copyright file="SEAppOccupantManager.cs" company="King County">
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
    public class SEAppOccupantManager : ISEAppOccupantManager
    {
        private const string TableName = "ptas_seappoccupants";

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppOccupantManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public SEAppOccupantManager(CRMWrapper crmWrapper)
        {
            this.CrmWrapper = crmWrapper;
        }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <inheritdoc/>
        public async Task<List<DynamicsSEAppOccupant>> GetSEAppOccupantFromSEAppId(string sEAppId) =>
            await this.ExecuteQuery($"$filter=_ptas_seapplicationid_value eq '{sEAppId}'");

        private async Task<List<DynamicsSEAppOccupant>> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsSEAppOccupant>(TableName, query);
            List<DynamicsSEAppOccupant> resultList = new List<DynamicsSEAppOccupant>();

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
