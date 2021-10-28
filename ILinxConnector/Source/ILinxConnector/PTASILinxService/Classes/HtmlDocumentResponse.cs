// <copyright file="HtmlDocumentResponse.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
  /// <summary>
  /// response that contains an html.
  /// </summary>
  public class HtmlDocumentResponse
  {
    /// <summary>
    /// Gets html content.
    /// </summary>
    public string HtmlResponse { get; internal set; }
  }
}