// <copyright file="ErrorMessage.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Error message to be posted to error reporting.
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// Gets or sets html body of the message.
        /// </summary>
        public string MessageBody { get; set; }

        /// <summary>
        /// Gets or sets subject of the message.
        /// </summary>
        public string MessageSubject { get; set; }
    }
}