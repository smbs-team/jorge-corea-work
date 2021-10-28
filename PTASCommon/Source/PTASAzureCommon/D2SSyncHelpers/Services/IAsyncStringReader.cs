// <copyright file="IAsyncStringReader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Any object that reads a string from any media async.
    /// </summary>
    public interface IAsyncStringReader
    {
        /// <summary>
        /// Gets content.
        /// </summary>
        /// <returns>A task that yields a string.</returns>
        Task<string> GetContentAsync();
    }
}