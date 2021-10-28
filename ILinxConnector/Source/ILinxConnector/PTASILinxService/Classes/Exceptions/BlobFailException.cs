// <copyright file="BlobFailException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
  using System;

  /// <summary>
  /// Sent when fail on a blob operation.
  /// </summary>
  public class BlobFailException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BlobFailException"/> class.
    /// </summary>
    /// <param name="route">Route where it failed.</param>
    /// <param name="operation">Operation that failed.</param>
    /// <param name="innerException">Exception that triggered this one.</param>
    public BlobFailException(string operation, string route, Exception innerException)
      : base($"Operation {operation} failed on route: {route}.", innerException)
    {
    }
  }
}