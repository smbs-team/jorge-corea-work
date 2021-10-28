// <copyright file="ContactManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Implementations
{
    using System.Threading.Tasks;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Contact manager implementation.
    /// </summary>
    public class ContactManager : IContactManager
    {
        private const string TableName = "contacts";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public ContactManager(CRMWrapper crmWrapper)
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
        /// <param name="contactId">Id to search.</param>
        /// <returns>The contact or null.</returns>
        public async Task<DynamicsContact> GetContactFromContactId(string contactId) =>
          await this.ExecuteQuery($"$filter=contactid eq '{contactId}'");

        /// <summary>
        /// Gets the contact Id from it's email address.
        /// </summary>
        /// <param name="email">Email to look for.</param>
        /// <returns>Contact Id.</returns>
        public async Task<DynamicsContact> GetContactFromEmail(string email) =>
          await this.ExecuteQuery($"$filter=emailaddress1 eq '{email}'");

        private async Task<DynamicsContact> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsContact>(TableName, query);
            return results == null || results.Length == 0 ? null : results[0];
        }
    }
}
