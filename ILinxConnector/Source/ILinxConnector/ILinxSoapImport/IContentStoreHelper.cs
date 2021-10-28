// <copyright file="IContentStoreHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport
{
  using ILinxSoapImport.EdmsService;

  /// <summary>
  /// Generic content store helper.
  /// </summary>
  public interface IContentStoreHelper
  {
    /// <summary>
    /// Initialize and retrieve a content store client.
    /// </summary>
    /// <returns>Newly created content store.</returns>
    /// <remarks>Must displose and close.</remarks>
    ContentStoreContractClient GetContentStoreClient();

    /// <summary>
    /// Retrieve security token for further operations.
    /// </summary>
    /// <returns>The token encoded as a string.</returns>
    string GetSecurityToken();
  }
}