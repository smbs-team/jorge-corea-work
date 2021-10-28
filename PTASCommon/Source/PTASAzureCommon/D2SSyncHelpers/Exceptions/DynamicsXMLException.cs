// <copyright file="DynamicsXMLException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Exceptions
{
    using System;

    /// <summary>
    /// Error when dynamics XML is erroneous.
    /// </summary>
    public class DynamicsXMLException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsXMLException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Generating exception.</param>
        public DynamicsXMLException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
