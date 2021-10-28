// <copyright file="DataAccessLibrary.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Data access methods.
    /// </summary>
    public class DataAccessLibrary : IDataAccessLibrary
    {
        private readonly IConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLibrary"/> class.
        /// </summary>
        /// <param name="config">App configuration.</param>
        public DataAccessLibrary(IConfiguration config)
        {
            this.config = config;
        }

        private string ConnectionString => this.config.GetConnectionString("default");

        /// <summary>
        /// Load data into objects.
        /// </summary>
        /// <typeparam name="TDataType">Datatype to process.</typeparam>
        /// <typeparam name="TParams">Parameters dataype.</typeparam>
        /// <param name="sql">Query to execute.</param>
        /// <param name="parameters">Actual parameters.</param>
        /// <returns>List of loaded objects.</returns>
        public async Task<List<TDataType>> LoadData<TDataType, TParams>(string sql, TParams parameters)
        {
            using var connection = new SqlConnection(this.ConnectionString);
            var data = await connection.QueryAsync<TDataType>(sql, parameters);
            return data.ToList();
        }

        /// <summary>
        /// Attempt to execute a non-query command.
        /// </summary>
        /// <typeparam name="T">Datatype.</typeparam>
        /// <param name="sql">Query to execute.</param>
        /// <param name="parameters">Parameters to apply.</param>
        /// <returns>Void task.</returns>
        public async Task SaveData<T>(string sql, T parameters)
        {
            using var connection = new SqlConnection(this.ConnectionString);
            await connection.ExecuteAsync(sql, parameters);
        }
    }
}