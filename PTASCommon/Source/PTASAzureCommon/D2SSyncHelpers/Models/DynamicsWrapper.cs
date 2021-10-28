// <copyright file="DynamicsWrapper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Models
{
    /// <summary>
    /// Dynamics wrapper, used to decode json.
    /// </summary>
    /// <typeparam name="T">Type to get.</typeparam>
    public class DynamicsWrapper<T>
    {
        /// <summary>
        /// Gets or sets list of entity references.
        /// </summary>
        public T[] Value { get; set; }
    }
}