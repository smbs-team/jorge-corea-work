// <copyright file="EmptyDocException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport.Exceptions
{
  using System;

  /// <summary>
  /// Exception thrown when there is no file to process.
  /// </summary>
  public class EmptyDocException : Exception
  {
    private const string EmptyDocMessage = "Document is empty.";

    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyDocException"/> class.
    /// </summary>
    public EmptyDocException()
      : base(EmptyDocMessage)
    {
    }
  }
}
