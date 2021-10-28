// <copyright file="SEApplicationManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Implementations
{
    using System;
    using System.Threading.Tasks;
    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Seniour Application manager implementation.
    /// </summary>
    public class SEApplicationManager
    {
        private const string TableName = "ptas_seapplications";
        private const string SelectFields = "seapplicationid";
        private const string KeyField = "seapplicationid";

        /// <summary>
        /// Initializes a new instance of the <see cref="SEApplicationManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public SEApplicationManager(CRMWrapper crmWrapper)
        {
            this.CrmWrapper = crmWrapper;
        }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <summary>
        /// Get the contact info.
        /// </summary>
        /// <param name="seapplicationId">Id to search.</param>
        /// <returns>The contact or null.</returns>
        public async Task<DynamicsSeniorExemptionApplication> GetSEApplicationFromSEApplicationId(string seapplicationId) =>
          await this.ExecuteQuery($"$filter={KeyField} eq '{seapplicationId}'");

        /// <summary>
        /// Gets the contact Id from it's email address.
        /// </summary>
        /// <param name="contactId">contactId to look for.</param>
        /// <returns>Contact Id.</returns>
        public async Task<DynamicsSeniorExemptionApplication> GetSEApplicationFromContactId(string contactId) =>
          await this.ExecuteQuery($"$filter=ptas_contactid eq '{contactId}'");

        /// <summary>
        /// Get contact info by contact Id.
        /// </summary>
        /// <param name="seapplication">Contact to search for.</param>
        /// <returns>The seapplication if found or null.</returns>
        /// <exception cref="Exception">Thrown when any other type of problem.</exception>
        public async Task InsertSEApplication(DynamicsSeniorExemptionApplicationForSave seapplication)
        {
            var x = await this.CrmWrapper.ExecutePost(TableName, seapplication, SelectFields);
            if (!x)
            {
                throw new Exception("Could not create senior exemption application");
            }
        }

        /// <summary>
        /// Tries to update a contact entity.
        /// </summary>
        /// <param name="seapplication">The contact to update.</param>
        /// <returns>void.</returns>
        public async Task<bool> UpdateSEApplication(DynamicsSeniorExemptionApplication seapplication) =>
          await this.CrmWrapper.ExecutePatch(TableName, seapplication, $"ptas_seapplicationId={seapplication.SEAapplicationId}");

        private async Task<DynamicsSeniorExemptionApplication> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsSeniorExemptionApplication>(TableName, query);
            return results == null || results.Length == 0 ? null : results[0];
        }
    }
 }
