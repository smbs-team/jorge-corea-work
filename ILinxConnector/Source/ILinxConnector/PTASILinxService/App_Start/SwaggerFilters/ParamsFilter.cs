// <copyright file="ParamsFilter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.App_Start.SwaggerFilters
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Http.Description;
  using Swashbuckle.Swagger;

  /// <summary>
  /// Filter that adds parameters.
  /// </summary>
  public abstract class ParamsFilter : IOperationFilter
  {
    /// <inheritdoc/>
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    {
      IEnumerable<Parameter> @params = this.GetParams();
      if (operation.parameters == null)
      {
        operation.parameters = @params.ToList();
      }
      else
      {
        (operation.parameters as List<Parameter>).AddRange(@params);
      }
    }

    /// <summary>
    /// Get parameters.
    /// </summary>
    /// <returns>List of parameters.</returns>
    protected abstract IEnumerable<Parameter> GetParams();
  }
}