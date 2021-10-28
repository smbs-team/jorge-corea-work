// <copyright file="SerializedWithControllerName.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  using Newtonsoft.Json;

  /// <summary>
  /// Class to deserialize to an object with controller name.
  /// </summary>
  public class SerializedWithControllerName
  {
    /// <summary>
    /// Gets or sets field to deserialize from controller.
    /// </summary>
    [JsonProperty("controller")]
    public string ControllerName { get; set; }
  }
}