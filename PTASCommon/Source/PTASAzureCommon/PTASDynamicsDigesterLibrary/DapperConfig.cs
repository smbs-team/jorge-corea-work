// <copyright file="DapperConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsDigesterLibrary
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using Dapper;

    /// <summary>
    /// Configure Dapper mapping for sql tables.
    /// </summary>
    public static class DapperConfig
    {
        /// <summary>
        /// Configures dapper mapper with received types.
        /// </summary>
        /// <param name="types">List of types to configure.</param>
        public static void ConfigureMapper(params Type[] types) =>
            types.ToList().ForEach(tableType =>
                SqlMapper.SetTypeMap(
                    tableType,
                    new CustomPropertyTypeMap(
                        tableType,
                        (type, columnName) =>
                            type.GetProperties().FirstOrDefault(prop =>
                                prop.GetCustomAttributes(false)
                                    .OfType<ColumnAttribute>()
                                    .Any(attr => attr.Name == columnName)))));
    }
}