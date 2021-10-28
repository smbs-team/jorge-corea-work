// <copyright file="CRMHeader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
  using Newtonsoft.Json;

  /// <summary>
  /// Header for all CRM responses.
  /// </summary>
  /// <typeparam name="T">Type of values to fetch.</typeparam>
  public class CRMHeader<T>
  {
    /// <summary>
    /// Gets or sets number of records returned.
    /// </summary>
    [JsonProperty("@odata.count")]
    public long Count { get; set; }

    /// <summary>
    /// Gets or sets values returned.
    /// </summary>
    [JsonProperty("value")]
    public T[] Values { get; set; }
  }
}
