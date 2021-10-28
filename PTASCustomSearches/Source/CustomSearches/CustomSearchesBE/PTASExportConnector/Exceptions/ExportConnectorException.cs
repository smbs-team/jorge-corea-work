// <copyright file="ExportConnectorException.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.Exceptions
{
    using System;

    /// <summary>Used when something goes wrong within the exporting process.</summary>
    /// <seealso cref="System.Exception" />
    public class ExportConnectorException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ExportConnectorException"/> class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">Equivalent status code of the given error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (<span class="keyword"><span class="languageSpecificText"><span class="cs">null</span><span class="vb">Nothing</span><span class="cpp">nullptr</span></span></span><span class="nu">a null reference (<span class="keyword">Nothing</span> in Visual Basic)</span> in Visual Basic) if no inner exception is specified.
        /// </param>
        public ExportConnectorException(string message, int statusCode, Exception innerException = null)
            : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>Gets the status code.</summary>
        /// <value>The status code.</value>
        public int StatusCode { get; }
    }
}
