// <copyright file="ICounty.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

  /// <summary>
  /// Generic county info. Only name, no other fields.
  /// </summary>
    public interface ICounty
  {
        /// <summary>
        /// Gets or sets id of the County.
        /// </summary>
        Guid CountyId { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        string Name { get; set; }
  }
}
