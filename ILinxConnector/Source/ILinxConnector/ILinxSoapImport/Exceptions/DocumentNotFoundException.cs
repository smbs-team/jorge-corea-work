// <copyright file="DocumentNotFoundException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when a document is not found in Ilinx.
    /// </summary>
    public class DocumentNotFoundException : Exception
    {
        private const string EmptyDocMessage = "Document was not found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNotFoundException"/> class.
        /// </summary>
        public DocumentNotFoundException()
            : base(EmptyDocMessage)
        {
        }
    }
}
