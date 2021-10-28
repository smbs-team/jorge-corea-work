// <copyright file="NoControllerException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
  using System;

  /// <summary>
  /// When a json string does not have a controller field.
  /// </summary>
  public class NoControllerException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="NoControllerException"/> class.
    /// </summary>
    /// <param name="generatingJson">Json that produced the error.</param>
    public NoControllerException(string generatingJson)
      : base($"No controller in text: {generatingJson.Substring(0, 100)}")
    {
    }
  }
}