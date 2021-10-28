// <copyright file="MoveToDynamicsFailException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
    using System;
    using System.Net.Http;

    /// <summary>
    /// Exception when there was a problem moving to dynamics.
    /// </summary>
    public class MoveToDynamicsFailException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveToDynamicsFailException"/> class.
        /// </summary>
        /// <param name="causingJson">Json that caused the problem.</param>
        /// <param name="inner">Inner exception if present.</param>
        public MoveToDynamicsFailException(string causingJson, Exception inner)
          : base($"Error moving to dynamics, JSON: \n {causingJson}", inner)
        {
        }
    }
}