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
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Data access methods.
    /// </summary>
    public class DataAccessLibrary : IDataAccessLibrary
    {
        private readonly string connectionString;
        private ClientCredential principalCredentials;

        // private readonly string access = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Im5PbzNaRHJPRFhFSzFqS1doWHNsSFJfS1hFZyIsImtpZCI6Im5PbzNaRHJPRFhFSzFqS1doWHNsSFJfS1hFZyJ9.eyJhdWQiOiJodHRwczovL2RhdGFiYXNlLndpbmRvd3MubmV0LyIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0L2JhZTUwNTlhLTc2ZjAtNDlkNy05OTk2LTcyZGZlOTVkNjljNy8iLCJpYXQiOjE2MjE0NTUwNTksIm5iZiI6MTYyMTQ1NTA1OSwiZXhwIjoxNjIxNDU4OTU5LCJhY3IiOiIxIiwiYWlvIjoiRTJaZ1lGQ3QzNkZzc1htK2RwT2pvZnl4WFRKRnN1c1BPMjNrc2ZYNHp0cDljSGJ1eXdJQSIsImFtciI6WyJwd2QiXSwiYXBwaWQiOiIwNGIwNzc5NS04ZGRiLTQ2MWEtYmJlZS0wMmY5ZTFiZjdiNDYiLCJhcHBpZGFjciI6IjAiLCJmYW1pbHlfbmFtZSI6Ik1vcmFnYSIsImdpdmVuX25hbWUiOiJNYXJpbyIsImdyb3VwcyI6WyJkNmNhODgyMC00NmU4LTRjZjAtYTI2Yi0zYzQzOTQwOTE4ODUiLCI3YjJhNDc1ZS0yMjFiLTQ4YjMtOTZjYy1hZDA3YmE5NWMxNWYiLCIzNTNhNjFjZC00YWRlLTQ3NTItYjdhZC0zYzAwMDdhMzMzZWUiLCJmNmJlZGE2NS00OTQ5LTRjN2YtOWExZi03NTk1ODk4N2Y2MmIiLCJkMmUwMWY2ZC0xNzllLTRhNTUtODRjMS03ZDdjYzZiN2ZlMTgiLCJkMGIxNmE3My1kMTY0LTQ2ZTctOGRjNC1hZjU5NDY1MTk5YzkiLCJmZWY2M2RjMi01NTZkLTQzNDMtYjI4Zi1mYmE4M2JkZTUzYzgiLCIyOWUxMjE4Yi04Y2M0LTQ4M2EtYjMyMi1jOWFjNjMyODAwMDYiLCJkOTQ1YmM4NS1kMWZjLTQzZDQtODM1My0xZGMxYzAyZmJiMGEiLCJhNzY5MDcwOC1iYTRjLTQyNDgtODczZi1hODZjZWVmMWY0NjAiLCIzNjExY2EyNi00ZGJkLTQ5ZDMtODU4ZC0wYzJhMDFhM2U5ZGEiLCI5MzcyOTQ4Zi0yNTkwLTQzMTUtYjJkYS05YjI2ZDRhMmFiNDQiLCI0ZmM1YzRmOC02Y2ZlLTQyODItODVlYS02MTZmNTJlZjdiMDEiLCI5OGMxZDgwMS0xOThlLTRiNmItOTkyZi1kYThlYzI0ZjI1YTgiLCJhZGU1NDFjNi03OWM1LTQ2NzUtOTRjOC1lYjhlNzhiYzY4MjgiLCIwOTdlNWUwNS1lOWM0LTRlYmYtYmFhOC0wZjRkNDc0NzIyOWEiLCJmNmM3OThhYS1iNTdkLTQyOGUtYjk5YS01YjI3N2Y0MWM4ZjMiLCIyYzdhNjBhNC1mZmM3LTQxOGMtYTI2My02ZGUxM2RlNDdiZTgiLCI2NmU5MjY0Yy1kN2ZkLTQzNDUtOTExZS05MTVmMzY5ZTkyNmUiLCIzYmE3MGE5Ni0zZGZjLTQ0ZTUtYTEzNi1iZjU2MTZmYjljMTciLCJmYjAwOTY2ZC0zMDhiLTQ4ZGUtYjA3Yi02ZjM3ZDI3Zjg5M2YiLCJhYTU2MGEzMS00MTRkLTQ2NGItYTU0ZC00Y2U3NWZmMWE0NTgiLCI0MjE3NDk0Yi02YmVhLTRjMjEtYTgyNi1mOTI3ZDM2OTgxNDAiLCI5ZWE2NjkwNC1lZjE1LTQ4YTYtODc2ZS1lYjE3NmY4M2EwNjkiLCIwMTk2MjRjOC0wZWQxLTRlNzYtOTkxMC1jOTVhNjAzNmMxM2EiLCJiMzYzZDM5Zi1jOWJhLTRkM2UtOTYwYy1mYjk2MmEzZjg1ZTYiXSwiaWR0eXAiOiJ1c2VyIiwiaXBhZGRyIjoiMjAxLjE5MS43NC4zMyIsIm5hbWUiOiJNb3JhZ2EsIE1hcmlvIiwib2lkIjoiYzIzYzJiOTItNGYzYy00YjA2LTg4NWUtYTBlMzIxNGY1ODMxIiwib25wcmVtX3NpZCI6IlMtMS01LTIxLTEzMjk4MzAxMjItNDE4NDMzNDM2MC0yODUyMTg5NTctMjA2Nzc4IiwicHVpZCI6IjEwMDMyMDAwN0Y1MkY2RkMiLCJyaCI6IjAuQVFRQW1nWGx1dkIyMTBtWmxuTGY2VjFweDVWM3NBVGJqUnBHdS00Qy1lR19lMFlFQUpZLiIsInNjcCI6InVzZXJfaW1wZXJzb25hdGlvbiIsInN1YiI6IjFrbFFwcGt4bVZSVmswTllsZ0VrdE5mMWhyenlVU3dfRWVfSGVFalNlVTAiLCJ0aWQiOiJiYWU1MDU5YS03NmYwLTQ5ZDctOTk5Ni03MmRmZTk1ZDY5YzciLCJ1bmlxdWVfbmFtZSI6Im4tbW1vcmFnYUBraW5nY291bnR5LmdvdiIsInVwbiI6Im4tbW1vcmFnYUBraW5nY291bnR5LmdvdiIsInV0aSI6IldHV010NTVzYUV1d3R4NV80RmNsQUEiLCJ2ZXIiOiIxLjAifQ.Kjhrq8Eol6M5O7SNoKFxP877CP4bDymVFlfALjBO36PWYc5yGX7salKGdGJQBK178UelNctxfgfPqi0_VpaJYK5sntn3ljAUSUgNxjPowbBvyvKdyynZKyjhHRr7EOF92OA7tTQHxhtldCLvFa0Ux3PH-aiZEiKuAXTp__kHZrB58601V56Yp1fiCQSRQwr56pqh6G0T0MfMHSbNpNgt0dDvn9n1qjHyqdZvm2OVFAg4TWIGhYxVj7ATA0PnqlZCNV-cgNnB3S17W7nXIe362UxekitSci-0Me5M6iJSAeG3NulwoOCkZ4zXJ8_aMpqC1CIp9j_JU_1LfNyJiSL_ow";

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLibrary"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="principalCredentials">Credential for token.</param>
        public DataAccessLibrary(string connectionString, ClientCredential principalCredentials)
        {
            this.connectionString = connectionString;
            this.principalCredentials = principalCredentials;
        }

        private string ConnectionString => this.connectionString;

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
            connection.AccessToken = this.GetToken();

            // connection.AccessToken = this.access;
            var data = await connection.QueryAsync<TDataType>(sql, parameters);
            return data.ToList();
        }

        /// <summary>
        /// Attemp to get the connectionString.
        /// </summary>
        /// <returns>SqlConnection.</returns>
        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection(this.ConnectionString);
            connection.AccessToken = this.GetToken();

            return connection;
        }

        /// <summary>
        /// Attempt to execute a non-query command.
        /// </summary>
        /// <typeparam name="T">Datatype.</typeparam>
        /// <param name="sql">Query to execute.</param>
        /// <param name="parameters">Parameters to apply.</param>
        /// <returns>Void task.</returns>
        public async Task<int> SaveData<T>(string sql, T parameters)
        {
            using var connection = new SqlConnection(this.ConnectionString);
            connection.AccessToken = this.GetToken();

            // connection.AccessToken = this.access;
            var result = await connection.ExecuteAsync(sql, parameters);
            return result;
        }

        /// <summary>Create or update token when expired.</summary>
        /// <param name="sqlConnectionString"> Connection for the token.</param>
        private string GetToken()
        {
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();

            string accessToken = Task.Run(async () =>
            {
                return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantId, this.principalCredentials);
            }).Result;

            return accessToken;
        }
    }
}