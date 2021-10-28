// <copyright file="NeedsJsonBody.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.App_Start.SwaggerFilters
{
  using System.Collections.Generic;
  using System.Web.Http.Description;
  using Swashbuckle.Swagger;

  /// <summary>
  /// Filter to add JSON body to request.
  /// </summary>
  public class NeedsJsonBody : IOperationFilter
  {
    /// <inheritdoc/>
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    {
      if (operation.parameters == null)
      {
        operation.parameters = new List<Parameter>();
      }

      operation.parameters.Add(new Parameter()
      {
        name = "JSON",
        @in = "body",
        description = "JSON Body",
        required = true,
        type = "text/json",
      });
    }
  }
}