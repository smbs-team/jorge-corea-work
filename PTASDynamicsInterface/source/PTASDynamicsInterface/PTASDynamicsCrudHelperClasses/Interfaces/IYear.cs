// <copyright file="IYear.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
  using System;

  /// <summary>
  /// Generic representation of a ptas_year.
  /// </summary>
  public interface IYear
  {
    /// <summary>
    /// Gets or sets id of the year.
    /// </summary>
    Guid YearId { get; set; }

    /// <summary>
    /// Gets or sets ending date.
    /// </summary>
    DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets name of the year.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets starting date of this year.
    /// </summary>
    DateTime? StartDate { get; set; }
  }
}