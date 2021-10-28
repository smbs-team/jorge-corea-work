// <copyright file="AzureFileBlobControllerFilter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.App_Start.SwaggerFilters
{
  using Swashbuckle.Swagger;

  /// <summary>
  /// Add parameters to AzureFileBlobController.
  /// </summary>
  public class AzureFileBlobControllerFilter : ParamStrFilter, IOperationFilter
  {
    private const string StringParams = "seniorApplicationId,seniorApplicationDetailsId,section,document,checkImage";

    /// <inheritdoc/>
    protected override string ParamStr => StringParams;
  }
}