namespace DeploymentHealthService
{
  using System;
  using System.Web.Http;
  using Swashbuckle.Application;

  /// <summary>
  /// Configure swagger.
  /// </summary>
  public static class SwaggerConfig
  {
    /// <summary>
    /// Configures Swagger.
    /// </summary>
    /// <param name="config">Configuration to load.</param>
    public static void Configure(HttpConfiguration config)
    {
      if (config == null)
      {
        throw new ArgumentNullException(nameof(config));
      }

      // Use http://localhost:5000/swagger/ui/index to inspect API docs
      config
          .EnableSwagger(c =>
          {
            c.SingleApiVersion("v1", "PTAS Document Storage Services");
            c.PrettyPrint();
            c.IncludeXmlComments(string.Format(@"{0}\bin\ReferenceProject.xml", System.AppDomain.CurrentDomain.BaseDirectory));
            c.RootUrl(requestMessage =>
                  {
                    var idx = requestMessage.RequestUri.AbsoluteUri.IndexOf("swagger", StringComparison.InvariantCultureIgnoreCase);
                    return requestMessage.RequestUri.AbsoluteUri.Substring(0, idx - 1);
                  });
          })
          .EnableSwaggerUi();
    }
  }
}