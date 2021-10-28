// <copyright file="ContactManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Entities
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using Microsoft.WindowsAzure.Storage.Table;
    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// Manages cloud tables for contacts.
    /// </summary>
    public class ContactManager
    {
        private readonly CloudTable cloudTable;
        private readonly string dynamicsApiURL;
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactManager"/> class.
        /// </summary>
        /// <param name="cloudTable">Source table.</param>
        /// <param name="dynamicsApiURL">Config dynamics Api URL.</param>
        public ContactManager(CloudTable cloudTable, string dynamicsApiURL)
        {
            this.cloudTable = cloudTable;
            this.dynamicsApiURL = dynamicsApiURL;
        }

        /// <summary>
        /// Attempt to retrieve information for the contact.
        /// </summary>
        /// <param name="email">email to look for.</param>
        /// <returns>contact if found.</returns>
        public async Task<IContactInfo> GetContactIdAsync(string email)
        {
            var t = this.cloudTable.Execute(TableOperation.Retrieve<ContactInfo>(ContactInfo.ContactsVault, email));
            if (t.Result != null)
            {
                return t.Result as IContactInfo;
            }

            // not found... search in dynamics.
            ContactInfo fromDynamics = await this.LoadFromDynamcisAsync(email);
            if (fromDynamics != null)
            {
                this.SaveToTable(email, fromDynamics);
                return fromDynamics;
            }

            return null;
        }

        private void SaveToTable(string email, ContactInfo fromDynamics)
        {
            fromDynamics.RowKey = email;
            this.cloudTable.Execute(TableOperation.InsertOrReplace(fromDynamics));
        }

        private async Task<ContactInfo> LoadFromDynamcisAsync(string email)
        {
            var itemPath = Path.Combine(Path.Combine(this.dynamicsApiURL, "contactlookup"), email);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, itemPath)
            {
                ////Content = new StringContent(jsonToApply, Encoding.UTF8, "application/json"),
            };

            var authHeader = JWTDecoder.GetAuthHeader();
            if (authHeader != null)
            {
                requestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), authHeader);
            }

            var response = await this.client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var s = await response.Content.ReadAsStringAsync();
                var t = Json.Decode(s).contactid;
                if (t != null)
                {
                    return new ContactInfo { ContactId = Guid.Parse(t), RowKey = email };
                }
            }

            return null;
        }
    }
}