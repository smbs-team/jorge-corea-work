// <copyright file="FilesDoNotMatchException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport.Exceptions
{
  using System;

  /// <summary>
  /// Exception thrown when there is no file to process.
  /// </summary>
  public class FilesDoNotMatchException : Exception
  {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilesDoNotMatchException"/> class.
        /// </summary>
        /// <param name="expected">Expected number of files.</param>
        /// <param name="received">Actual number of files.</param>
        public FilesDoNotMatchException(int expected, int received)
      : base($"File count does not match. Should be {expected} but is {received}")
    {
    }
  }
}
