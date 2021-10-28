// <copyright file="AzureBlobResponse.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
  using System.Collections.Generic;
  using Newtonsoft.Json;

  /// <summary>
  /// Response for azure blob operations.
  /// </summary>
  public class AzureBlobResponse
  {
    /// <summary>
    /// Gets or sets a value indicating whether there is an error flag.
    /// </summary>
    [JsonProperty("error")]
    public bool Error { get; set; } = false;

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets file names uploaded.
    /// </summary>
    [JsonProperty("files")]
    public IEnumerable<UploadFileResult> Files { get; set; }
  }
}
