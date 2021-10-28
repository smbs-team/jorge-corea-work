// <copyright file="ImportConnectorException.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector.Exceptions
{
    using System;

    /// <summary>Used when something goes wrong withing the importing process.</summary>
    /// <seealso cref="System.Exception" />
    public class ImportConnectorException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ImportConnectorException"/> class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (<span class="keyword"><span class="languageSpecificText"><span class="cs">null</span><span class="vb">Nothing</span><span class="cpp">nullptr</span></span></span><span class="nu">a null reference (<span class="keyword">Nothing</span> in Visual Basic)</span> in Visual Basic) if no inner exception is specified.
        /// </param>
        public ImportConnectorException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}