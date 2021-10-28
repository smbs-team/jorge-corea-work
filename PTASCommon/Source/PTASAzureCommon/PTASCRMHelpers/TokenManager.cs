// <copyright file="TokenManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers
{
    using System;

    /// <summary>
    /// Cache token manager.
    /// </summary>
    public class TokenManager : ITokenManager
    {
        private string tokenStr = string.Empty;
        private DateTime expires = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenManager"/> class.
        /// </summary>
        public TokenManager()
        {
        }

        /// <inheritdoc/>
        public string TokenStr => this.expires < DateTime.Now ? null : this.tokenStr;

        /// <inheritdoc/>
        public void SetToken(string tokenStr, DateTime expires)
        {
            this.tokenStr = tokenStr;
            this.expires = expires;
        }
    }
}