// <copyright file="IContact.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    /// <summary>
    /// Contact information for crud.
    /// </summary>
    public interface IContact
    {
        /// <summary>
        /// Gets or sets alternate key.
        /// </summary>
        int? AlternateKey { get; set; }

        /// <summary>
        /// Gets or sets id of the contact.
        /// </summary>
        string ContactId { get; set; }

        /// <summary>
        /// Gets or sets contact email.
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets contact middle name.
        /// </summary>
        string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets contact phone.
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// Gets or sets if the phone is sms capable.
        /// </summary>
        bool? SMSCapable { get; set; }

        /// <summary>
        /// Gets or sets if the Suffix value.
        /// </summary>
        string Suffix { get; set; }

        /// <summary>
        /// Gets or sets if the Birth date value.
        /// </summary>
        System.DateTime? BirthDate { get; set; }
    }
}