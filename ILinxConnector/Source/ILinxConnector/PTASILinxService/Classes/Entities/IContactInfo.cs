// <copyright file="IContactInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Entities
{
    using System;

    /// <summary>
    /// Information stored about the contact.
    /// </summary>
    public interface IContactInfo
    {
        /// <summary>
        /// Gets or sets customer contactId.
        /// </summary>
        Guid ContactId { get; set; }
    }
}