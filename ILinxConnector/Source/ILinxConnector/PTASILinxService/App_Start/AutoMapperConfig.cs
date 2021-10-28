// <copyright file="AutoMapperConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
  using System;
  using System.Web.Http;
  using AutoMapper;

  /// <summary>
  /// Configuration for automapper.
  /// </summary>
  public class AutoMapperConfig
  {
    /// <summary>
    /// Gets current mapper.
    /// </summary>
    public static IMapper Mapper { get; private set; }

    /// <summary>
    /// Configure IMapper for the app.
    /// </summary>
    /// <param name="config">Httpconfiguration parameters.</param>
    /// <returns>Created mapper.</returns>
    public static IMapper Configure(HttpConfiguration config)
    {
      Action<IMapperConfigurationExpression> mapperConfigurationExp = cfg =>
      {
        cfg.ConstructServicesUsing(GetResolver(config));
      };

      var mapperConfiguration = new MapperConfiguration(mapperConfigurationExp);
      Mapper = mapperConfiguration.CreateMapper();

      return Mapper;
    }

    private static Func<Type, object> GetResolver(HttpConfiguration config) => type => config.DependencyResolver.GetService(type);
  }
}