// <copyright file="SEAppNoteManager.cs" company="King County">
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
    public class SEAppNoteManager : ISEAppNoteManager
    {
        private const string TableName = "ptas_seappnotes";

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppNoteManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public SEAppNoteManager(CRMWrapper crmWrapper)
        {
            this.CrmWrapper = crmWrapper;
        }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <inheritdoc/>
        public async Task<List<DynamicsSEAppNote>> GetSEAppNoteFromSEAppId(string sEAppId) =>
            await this.ExecuteQuery($"$filter=_ptas_seapplicationid_value eq '{sEAppId}' and ptas_showonportal eq true");

        private async Task<List<DynamicsSEAppNote>> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsSEAppNote>(TableName, query);
            List<DynamicsSEAppNote> resultList = new List<DynamicsSEAppNote>();

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
