// <copyright file="DataAccessLibrary.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using D2SSyncHelpers.Interfaces;

    using Dapper;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Data access methods.
    /// </summary>
    public class DataAccessLibrary : IDataAccessLibrary
    {
        private readonly string connectionString;
        private readonly string token;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLibrary"/> class.
        /// </summary>
        /// <param name="config">App configuration.</param>
        public DataAccessLibrary(IConfiguration config)
        {
            this.connectionString = config.GetConnectionString("default") ?? config["connectionString"];
            this.token = !this.connectionString.Contains("User ID") ? config["BearerToken"]?.Trim() : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLibrary"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="token">Token to send.</param>
        public DataAccessLibrary(string connectionString, string token = null)
        {
            this.connectionString = connectionString;
            this.token = token?.Trim();
        }

        private string ConnectionString => this.connectionString;

        /// <summary>
        /// Load data into objects.
        /// </summary>
        /// <typeparam name="TDataType">Datatype to process.</typeparam>
        /// <typeparam name="TParams">Parameters dataype.</typeparam>
        /// <param name="sql">Query to execute.</param>
        /// <param name="parameters">Actual parameters.</param>
        /// <param name="log">Logger.</param>
        /// <returns>List of loaded objects.</returns>
        public async Task<List<TDataType>> LoadData<TDataType, TParams>(string sql, TParams parameters, ILogger log = null)
        {
            using var connection = this.GetConnection();

            var data = await connection.QueryAsync<TDataType>(sql, parameters);
            log?.LogInformation(sql);
            return data.ToList();
        }

        /// <summary>
        /// Attemp to get the connectionString.
        /// </summary>
        /// <returns>SqlConnection.</returns>
        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection(this.ConnectionString);
            if (!string.IsNullOrEmpty(this.token))
            {
                connection.AccessToken = this.token;
            }

            return connection;
        }

        /// <summary>
        /// Attempt to execute a non-query command.
        /// </summary>
        /// <typeparam name="T">Datatype.</typeparam>
        /// <param name="sql">Query to execute.</param>
        /// <param name="parameters">Parameters to apply.</param>
        /// <param name="log">Logger.</param>
        /// <returns>Void task.</returns>
        public async Task<int> SaveData<T>(string sql, T parameters, ILogger log = null)
        {
            using var connection = this.GetConnection();
            var result = await connection.ExecuteAsync(sql, parameters);
            log?.LogInformation(sql);
            return result;
        }
    }
}