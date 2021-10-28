// <copyright file="MoveDocResults.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
    using System.Collections.Generic;
    using PTASILinxConnectorHelperClasses.Models;

    /// <summary>
    /// Results returned after moving a file to/from ILinx.
    /// </summary>
    public class MoveDocResults
  {
    /// <summary>
    /// Gets or sets a value indicating whether error state.
    /// </summary>
    public bool Error { get; set; }

    /// <summary>
    /// Gets results of each insert.
    /// </summary>
    public IEnumerable<InsertResponse> InsertResults { get; internal set; }

    /// <summary>
    /// Gets consolidated message.
    /// </summary>
    public string Message { get; internal set; }
  }
}