// <copyright file="GetAllContentsFailException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
  using System;
  using System.Net.Http;

  /// <summary>
  /// Exception when there was a problem moving to dynamics.
  /// </summary>
  public class GetAllContentsFailException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllContentsFailException"/> class.
    /// </summary>
    /// <param name="ex">Inner exception that fired this one.</param>
    public GetAllContentsFailException(Exception ex)
      : base("Getting all contents.", ex)
    {
    }
  }
}