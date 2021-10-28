// <copyright file="SecurityChecker.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Security checker class.
    /// </summary>
    public static class SecurityChecker
    {
        /// <summary>
        /// Checks security token.
        /// </summary>
        /// <param name="mustContainContactId">string to check.</param>
        /// <param name="table">table cache to look from.</param>
        /// <param name="dynamicsApiURL">dynamics url.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task CheckSecurityAsync(string mustContainContactId, CloudTable table, string dynamicsApiURL)
        {
            string contactEmail = JWTDecoder.GetEmailFromHeader();
            if (string.IsNullOrEmpty(mustContainContactId) || contactEmail == null)
            {
                throw new System.UnauthorizedAccessException();
            }

            var cmgr = new ContactManager(table, dynamicsApiURL);
            IContactInfo contactInfo = await cmgr.GetContactIdAsync(contactEmail);

            string cid = (contactInfo?.ContactId ?? Guid.NewGuid()).ToString();
            if (contactInfo == null || !mustContainContactId.Contains(cid))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}