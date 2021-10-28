// <copyright file="UploadFileResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
  using Newtonsoft.Json;

  /// <summary>
  /// Result of uploading a file.
  /// </summary>
  public class UploadFileResult
  {
    /// <summary>
    /// Gets or sets route to the file.
    /// </summary>
    [JsonProperty("fileUrl")]
    public string FileRoute { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether error state.
    /// </summary>
    [JsonProperty("error")]
    public bool Error { get; set; } = false;

    /// <summary>
    /// Gets or sets error detail.
    /// </summary>
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;
  }
}