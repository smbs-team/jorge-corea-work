// <copyright file="BadJsonException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
    using System;

    /// <summary>
    /// Exception raised when problems parsing JSON.
    /// </summary>
    [Serializable]
    public class BadJsonException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadJsonException"/> class.
        /// </summary>
        /// <param name="jsonStr">JSON tried to parse.</param>
        /// <param name="innerException">Exception generated.</param>
        public BadJsonException(string jsonStr, Exception innerException)
          : base($"Failed parsing JSON: {jsonStr}", innerException)
        {
        }
    }
}