// <copyright file="CrmOdataHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers
{
    using System;

    /// <summary>
    /// Token manager interface.
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// Gets a token.
        /// </summary>
        string TokenStr { get; }

        /// <summary>
        /// Sets the token.
        /// </summary>
        /// <param name="tokenStr">Token.</param>
        /// <param name="expires">Expiration.</param>
        /// <param name="utcTicks">Expiration on utc ticks.</param>
        void SetToken(string tokenStr, DateTime expires, long utcTicks);
    }
}