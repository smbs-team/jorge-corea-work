// <copyright file="DynamicsFailException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
  using System;

  /// <summary>
  /// Sent when fail on a blob operation.
  /// </summary>
  public class DynamicsFailException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicsFailException"/> class.
    /// </summary>
    /// <param name="route">Route where it failed.</param>
    /// <param name="operation">Operation that failed.</param>
    /// <param name="innerException">Exception that triggered this one.</param>
    public DynamicsFailException(string operation, string route, Exception innerException)
      : base($"Dynamics Operation {operation} failed on route: {route}.", innerException)
    {
    }
  }
}