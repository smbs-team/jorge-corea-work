// <copyright file="AlreadyExistedException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Implementations
{
    using System;

    /// <summary>
    /// Exception to denote duplicate record.
    /// </summary>
    public class AlreadyExistedException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyExistedException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public AlreadyExistedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Gets the http status of this exception.
    /// </summary>
    public int StatusCode => 409;
  }
}
