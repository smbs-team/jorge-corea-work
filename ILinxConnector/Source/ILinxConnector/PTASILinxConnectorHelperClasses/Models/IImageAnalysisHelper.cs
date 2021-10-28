// <copyright file="IImageAnalysisHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
  using System.IO;
  using System.Threading.Tasks;

  /// <summary>
  /// Generic image analyzer.
  /// </summary>
  public interface IImageAnalysisHelper
  {
    /// <summary>
    /// Async function to check for image acceptability.
    /// </summary>
    /// <param name="buffer">Actual image bytes.</param>
    /// <returns>True if acceptable.</returns>
    Task<(bool, string)> ImageIsAcceptable(byte[] buffer);
  }
}