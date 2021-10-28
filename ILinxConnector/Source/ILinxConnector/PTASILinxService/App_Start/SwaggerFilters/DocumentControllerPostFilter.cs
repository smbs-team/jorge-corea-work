// <copyright file="DocumentControllerPostFilter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.App_Start.SwaggerFilters
{
  using Swashbuckle.Swagger;

  /// <summary>
  /// Add parameters to Documents Controller.
  /// </summary>
  public class DocumentControllerPostFilter : ParamStrFilter, IOperationFilter
  {
    private const string StringParams = "accountNumber:Account number,rollYear:Roll Year,docType:Document Type,recId:RecId";

    /// <inheritdoc/>
    protected override string ParamStr => StringParams;
  }
}