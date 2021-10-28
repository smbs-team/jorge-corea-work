// <copyright file="WrongNumberOfFilesException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Exceptions
{
  using System;

  /// <summary>
  /// Exception raised when no files present.
  /// </summary>
  public class WrongNumberOfFilesException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="WrongNumberOfFilesException"/> class.
    /// </summary>
    /// <param name="expected">expected number of files.</param>
    /// <param name="received">received number of files.</param>
    public WrongNumberOfFilesException(int expected, int received)
      : base($"Wrong number of files, expected {expected} but received {received}.")
    {
    }
  }
}
