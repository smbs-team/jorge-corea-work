// <copyright file="DocusignResponse.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  using Newtonsoft.Json;

  /// <summary>
  /// Client response for DocuSign Request.
  /// </summary>
  public class DocusignResponse
  {
    /// <summary>
    /// Gets return url to where the user has to be redirected.
    /// </summary>
    [JsonProperty("redirectUrl")]
    public string RedirectUrl { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether there was an error.
    /// </summary>
    [JsonProperty("error")]
    public bool Error { get; internal set; } = false;

    /// <summary>
    /// Gets error message if error.
    /// </summary>
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; internal set; } = string.Empty;
  }
}