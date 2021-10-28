// <copyright file="CheckException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
    using System;

    /// <summary>
    /// Exception raised when problems parsing JSON.
    /// </summary>
    [Serializable]
    public class CheckException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public CheckException(string message)
            : base(message)
        {
        }
    }
}