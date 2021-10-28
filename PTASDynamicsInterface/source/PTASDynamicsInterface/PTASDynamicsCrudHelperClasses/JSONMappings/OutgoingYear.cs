// <copyright file="OutgoingYear.cs" company="King County">
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
  public class OutgoingYear : IYear
  {
    /// <inheritdoc/>
    [JsonProperty("yearid")]
    public Guid YearId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonProperty("startdate", NullValueHandling = NullValueHandling.Include)]
    public DateTime? StartDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("enddate", NullValueHandling = NullValueHandling.Include)]
    public DateTime? EndDate { get; set; }
  }
}
