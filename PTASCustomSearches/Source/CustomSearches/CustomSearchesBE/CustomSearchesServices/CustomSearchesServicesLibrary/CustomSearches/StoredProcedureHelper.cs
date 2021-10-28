namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Helper for stored procedures.
    /// </summary>
    public static class StoredProcedureHelper
    {
        /// <summary>
        /// Validates if the stored procedure exists.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure name.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task ValidateStoredProcedureExistsAsync(string storedProcedureName, IServiceContext serviceContext)
        {
            SqlParameter[] commandParameters = new SqlParameter[]
            {
                new SqlParameter(@"storedProcedureName", storedProcedureName),
            };

            string script =
                "SELECT [name] FROM sys.procedures\n" +
                "where [type_desc] = 'SQL_STORED_PROCEDURE' AND [name] = @storedProcedureName and OBJECT_SCHEMA_NAME([object_id])= 'cus'";

            var result = await DbTransientRetryPolicy.ExecuteScalarAsync(
                serviceContext,
                serviceContext.DataDbContextFactory,
                script,
                commandParameters);

            if (result == null || result.Equals(DBNull.Value))
            {
                throw new CustomSearchesEntityNotFoundException(
                    $"Stored procedure '{storedProcedureName}' was not found.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Gets the stored procedure parameters.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure name.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task<HashSet<string>> GetStoredProcedureParametersAsync(string storedProcedureName, IServiceContext serviceContext)
        {
            SqlParameter[] sqlStoredProcedureParameters = new SqlParameter[]
            {
                new SqlParameter(@"procedurename", storedProcedureName),
            };

            string script = $"select name from sys.parameters where object_id = object_id('cus.' + @procedurename)";

            HashSet<string> storedProcedureParameters = new HashSet<string>();

            await DbTransientRetryPolicy.ExecuteReaderAsync(
                serviceContext,
                async (command, dataReader) =>
                {
                    while (dataReader.Read())
                    {
                        storedProcedureParameters.Add(dataReader.GetString(0).TrimStart('@'));
                    }
                },
                serviceContext.DataDbContextFactory,
                script,
                sqlStoredProcedureParameters);

            return storedProcedureParameters;
        }

        /// <summary>
        /// Gets the stored procedure result schema.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure name.</param>
        /// <param name="commandParameters">The part of the script with the parameters.</param>
        /// <param name="sqlParameters">The sql parameters.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>The stored procedure result schema.</returns>
        public static async Task<ReadOnlyCollection<DbColumn>> GetStoredProcedureResultSchemaAsync(
            string storedProcedureName,
            string commandParameters,
            SqlParameter[] sqlParameters,
            IServiceContext serviceContext)
        {
            string commandText = StoredProcedureHelper.GetExecuteScript(storedProcedureName, commandParameters);
            ReadOnlyCollection<DbColumn> result = null;
            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(
                serviceContext,
                serviceContext.DataDbContextFactory,
                commandText,
                sqlParameters,
                async (DbCommand command, DbDataReader dataReader) =>
                {
                    result = dataReader.GetColumnSchema();
                    command.Cancel();

                    // This is not awaited on purpose to avoid blocking the service.
                    dataReader.DisposeAsync();
                },
                $"Cannot get the column schema from Stored Procedure: '{storedProcedureName}'.");

            return result;
        }

        /// <summary>
        /// Gets the script for execute a stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure name.</param>
        /// <param name="commandParameters">The part of the script with the parameters.</param>
        /// <returns>The stored procedure result schema.</returns>
        public static string GetExecuteScript(
            string storedProcedureName,
            string commandParameters)
        {
            return $"EXECUTE [cus].[{storedProcedureName}] {commandParameters}";
        }
    }
}
