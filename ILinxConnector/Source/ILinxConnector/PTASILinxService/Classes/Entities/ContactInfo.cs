// <copyright file="ContactInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Information stored about the contact.
    /// </summary>
    public class ContactInfo : TableEntity, IContactInfo
    {
        /// <summary>
        /// vault for contacts.
        /// </summary>
        public const string ContactsVault = "contact-vault";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactInfo"/> class.
        /// </summary>
        public ContactInfo()
        {
            this.PartitionKey = ContactsVault;
        }

        /// <summary>
        /// Gets or sets customer contactId.
        /// </summary>
        public Guid ContactId { get; set; }
    }
}