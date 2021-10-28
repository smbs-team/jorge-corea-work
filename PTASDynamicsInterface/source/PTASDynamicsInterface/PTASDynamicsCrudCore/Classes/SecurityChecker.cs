// <copyright file="SecurityChecker.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Classes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Class to check contact id matches security header.
    /// </summary>
    public class SecurityChecker
    {
        private readonly IContactManager contactManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityChecker"/> class.
        /// </summary>
        /// <param name="contactManager">Current contact manager.</param>
        public SecurityChecker(IContactManager contactManager)
        {
            this.contactManager = contactManager;
        }

        /// <summary>
        /// Check if containing string has the contact id in it.
        /// </summary>
        /// <param name="context">Current http context.</param>
        /// <param name="containingString">String that must contain the security id.</param>
        /// <returns>True if found.</returns>
        public async Task<bool> CheckSecurity(HttpContext context, string containingString)
        {
            if (context == null || this.contactManager == null)
            {
                return true;
            }

            var emailAddressInHeader = JWTDecoder.GetEmailFromHeader(context);
            var foundContact = await this.contactManager.GetContactFromEmail(emailAddressInHeader);
            string contactId = foundContact?.ContactId;
            return contactId != null && containingString.Contains(contactId);
        }
    }
}