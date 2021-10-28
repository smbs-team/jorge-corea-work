// <copyright file="FileStreamResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  using System;
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Headers;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Web.Http;

  /// <summary>
  /// File stream HTTP action result.
  /// </summary>
  public class FileStreamResult : IHttpActionResult
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FileStreamResult" /> class.
    /// </summary>
    /// <param name="contentStream">The content stream.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <exception cref="System.ArgumentNullException">contentStream.</exception>
    public FileStreamResult(System.IO.Stream contentStream, string contentType)
    {
      this.ContentStream = contentStream ?? throw new ArgumentNullException(nameof(contentStream));
      this.ContentType = contentType;
    }

    /// <summary>
    /// Gets the content stream.
    /// </summary>
    public System.IO.Stream ContentStream { get; }

    /// <summary>
    /// Gets the type of the content.
    /// </summary>
    public string ContentType { get; }

    /// <summary>
    /// Executes this task.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The response.</returns>
    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
      var response = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new StreamContent(this.ContentStream),
      };
      if (!string.IsNullOrEmpty(this.ContentType))
      {
        response.Content.Headers.ContentType = new MediaTypeHeaderValue(this.ContentType);
      }

      return Task.FromResult(response);
    }
  }
}