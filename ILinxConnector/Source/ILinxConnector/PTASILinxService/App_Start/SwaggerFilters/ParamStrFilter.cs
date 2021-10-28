// <copyright file="ParamStrFilter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.App_Start.SwaggerFilters
{
  using System.Collections.Generic;
  using System.Linq;
  using Swashbuckle.Swagger;

  /// <summary>
  /// Generic class with only string parameters.
  /// </summary>
  public abstract class ParamStrFilter : ParamsFilter
  {
    /// <summary>
    /// Gets get all parameters as a comma separated string.
    /// </summary>
    protected abstract string ParamStr { get; }

    /// <inheritdoc/>
    protected override IEnumerable<Parameter> GetParams()
    {
      foreach (var item in this.ParamStr.Split(','))
      {
        var parts = item.Split(':');
        yield return new Parameter()
        {
          name = parts.First(),
          @in = "formData",
          description = parts.Last(),
          required = true,
          type = "string",
        };
      }
    }
  }
}