// <copyright file="ICountry.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// Generic representation of a ptas_year.
    /// </summary>
    public interface ICountry
    {
        /// <summary>
        /// Gets or sets id of the Country.
        /// </summary>
        Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        string Name { get; set; }
    }
}