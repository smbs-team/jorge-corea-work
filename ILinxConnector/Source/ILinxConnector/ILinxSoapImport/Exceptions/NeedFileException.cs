// <copyright file="NeedFileException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport.Exceptions
{
  using System;

  /// <summary>
  /// Exception thrown when there is no file to process.
  /// </summary>
  public class NeedFileException : Exception
  {
    private const string NeedFileMessage = "Need to upload at least one file.";

    /// <summary>
    /// Initializes a new instance of the <see cref="NeedFileException"/> class.
    /// </summary>
    public NeedFileException()
      : base(NeedFileMessage)
    {
    }
  }
}
