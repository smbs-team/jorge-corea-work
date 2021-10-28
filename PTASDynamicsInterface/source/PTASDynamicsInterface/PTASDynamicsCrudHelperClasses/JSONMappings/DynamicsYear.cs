// <copyright file="DynamicsYear.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
  using System;
  using Newtonsoft.Json;
  using PTASDynamicsCrudHelperClasses.Interfaces;

  /// <summary>
  /// Year to read from dynamics.
  /// </summary>
  public class DynamicsYear : IYear
  {
    /// <inheritdoc/>
    [JsonProperty("ptas_yearid")]
    public Guid YearId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_name")]
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_startdate")]
    public DateTime? StartDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_enddate")]
    public DateTime? EndDate { get; set; }
  }
}
