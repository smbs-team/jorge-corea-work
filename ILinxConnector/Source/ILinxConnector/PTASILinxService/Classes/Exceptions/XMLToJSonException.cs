// <copyright file="XMLToJSonException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
  using System;

  /// <summary>
  /// Uncaught exception during XML to Json conversion.
  /// </summary>
  [Serializable]
  public class XMLToJSonException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="XMLToJSonException"/> class.
    /// </summary>
    /// <param name="innerException">Inner exception.</param>
    public XMLToJSonException(Exception innerException)
      : base("Failed on converting to json.", innerException)
    {
    }
  }
}