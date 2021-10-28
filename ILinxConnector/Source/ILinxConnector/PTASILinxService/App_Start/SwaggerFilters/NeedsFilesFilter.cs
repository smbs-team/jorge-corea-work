// <copyright file="NeedsFilesFilter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.App_Start.SwaggerFilters
{
  using System.Collections.Generic;
  using System.Web.Http.Description;
  using Swashbuckle.Swagger;

  /// <summary>
  /// Filter added to a method that needs a file to be posted.
  /// </summary>
  public class NeedsFilesFilter : IOperationFilter
  {
    /// <inheritdoc/>
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    {
      if (operation.parameters == null)
      {
        operation.parameters = new List<Parameter>();
      }

      if (operation.consumes.Count == 0)
      {
        operation.consumes.Add("multipart/form-data");
      }

      operation.parameters.Add(new Parameter()
      {
        name = "Files",
        @in = "formData",
        description = "Attached file",
        required = true,
        type = "file",
      });
    }
  }
}