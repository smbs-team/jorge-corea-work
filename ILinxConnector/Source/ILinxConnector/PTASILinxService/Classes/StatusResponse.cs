// <copyright file="StatusResponse.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
  /// <summary>
  /// Response status of api calls.
  /// </summary>
  public class StatusResponse
  {
    /// <summary>
    /// Gets or sets result of the call.
    /// </summary>
    public string Result { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether did we have an error.
    /// </summary>
    public bool Error { get; set; } = false;

    /// <summary>
    /// Gets or sets error message, if there is one.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
  }
}