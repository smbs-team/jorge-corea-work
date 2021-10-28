// <copyright file="DocumentControllerPatchFilter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.App_Start.SwaggerFilters
{
  using Swashbuckle.Swagger;

  /// <summary>
  /// Add parameters to Documents Controller.
  /// </summary>
  public class DocumentControllerPatchFilter : ParamStrFilter, IOperationFilter
  {
    private const string StringParams = "documentId:Document Id";

    /// <inheritdoc/>
    protected override string ParamStr => StringParams;
  }
}