// <copyright file="IlinxLoginFailedException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when there is no file to process.
    /// </summary>
    public class IlinxLoginFailedException : Exception
    {
        private const string IlinxLoginFailedMessage = "Login to Ilinx failed.";

        /// <summary>
        /// Initializes a new instance of the <see cref="IlinxLoginFailedException"/> class.
        /// </summary>
        public IlinxLoginFailedException()
            : base(IlinxLoginFailedException.IlinxLoginFailedMessage)
        {
        }
    }
}
