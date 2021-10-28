// <copyright file="DapperConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsTranfer
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using Dapper;

    /// <summary>
    /// Dapper config.
    /// </summary>
    public static class DapperConfig
    {
        /// <summary>
        /// Configure dappper mapping.
        /// </summary>
        /// <param name="types">Types to map.</param>
        public static void ConfigureMapper(params Type[] types)
        {
            types.ToList().ForEach(tableType =>
            {
                SqlMapper.SetTypeMap(
                    tableType,
                    new CustomPropertyTypeMap(
                        tableType,
                        (type, columnName)
                            => type.GetProperties()
                                .FirstOrDefault(prop
                                => prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            });
        }
    }
}