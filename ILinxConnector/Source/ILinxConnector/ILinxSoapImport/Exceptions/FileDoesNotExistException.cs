// <copyright file="FileDoesNotExistException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport.Exceptions
{
  using System;

  /// <summary>
  /// Exception thrown when there is no file to process.
  /// </summary>
  public class FileDoesNotExistException : Exception
  {
    private const string FileDoesNotExistMessage = "Files does not exist.";

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDoesNotExistException"/> class.
    /// </summary>
    public FileDoesNotExistException()
      : base(FileDoesNotExistMessage)
    {
    }
  }
}
