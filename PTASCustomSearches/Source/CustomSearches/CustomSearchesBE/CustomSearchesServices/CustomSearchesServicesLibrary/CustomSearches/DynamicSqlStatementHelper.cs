namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Helpers that helps building and executing dynamic SQL statements.
    /// </summary>
    public static class DynamicSqlStatementHelper
    {
        private static List<int> transientErrorNumbers = new List<int> { 4060, 40197, 40501, 40613, 49918, 49919, 49920, 11001 };

        /// <summary>
        /// Executes the non query.  Generates a db exception if a SqlException is caught.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="dbError">The database error.</param>
        /// <returns>
        /// The db command result.
        /// </returns>
        /// <exception cref="CustomSearchesDatabaseException">Revert statement failed in the database.</exception>
        public static async Task<int> ExecuteNonQueryWithRetriesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            string dbError)
            where TDbContext : DbContext
        {
            return await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(serviceContext, dbContextFactory, script, parameters: null, dbError);
        }

        /// <summary>
        /// Executes the non query.  Generates a db exception if a SqlException is caught.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="dbError">The database error.</param>
        /// <returns>
        /// The db command result.
        /// </returns>
        /// <exception cref="CustomSearchesDatabaseException">Revert statement failed in the database.</exception>
        public static async Task<int> ExecuteNonQueryWithRetriesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            SqlParameter[] parameters,
            string dbError)
            where TDbContext : DbContext
        {
            try
            {
                return await DbTransientRetryPolicy.ExecuteNonQueryAsync(serviceContext, dbContextFactory, script, parameters);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new CustomSearchesDatabaseException(dbError, ex);
            }
        }

        /// <summary>
        /// Executes the scalar query.  Generates a db exception if a SqlException is caught.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="dbError">The database error.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// The db command result.
        /// </returns>
        /// <exception cref="CustomSearchesDatabaseException">Revert statement failed in the database.</exception>
        public static async Task<object> ExecuteScalarWithRetriesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            string dbError)
            where TDbContext : DbContext
        {
            return await DynamicSqlStatementHelper.ExecuteScalarWithRetriesAsync(serviceContext, dbContextFactory, script, parameters: null, dbError);
        }

        /// <summary>
        /// Executes the scalar query.  Generates a db exception if a SqlException is caught.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="dbError">The database error.</param>
        /// <returns>
        /// The db command result.
        /// </returns>
        /// <exception cref="CustomSearchesDatabaseException">Revert statement failed in the database.</exception>
        public static async Task<object> ExecuteScalarWithRetriesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            SqlParameter[] parameters,
            string dbError)
            where TDbContext : DbContext
            {
            try
            {
                return await DbTransientRetryPolicy.ExecuteScalarAsync<TDbContext>(serviceContext, dbContextFactory, script, parameters);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new CustomSearchesDatabaseException(dbError, ex);
            }
        }

        /// <summary>
        /// Executes the scalar query.  Generates a db exception if a SqlException is caught.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="processReader">The process reader.</param>
        /// <param name="dbError">The database error.</param>
        /// <returns>The async task.</returns>
        public static async Task ExecuteReaderWithRetriesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            Func<DbCommand, DbDataReader, Task> processReader,
            string dbError)
            where TDbContext : DbContext
        {
            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(serviceContext, dbContextFactory, script, parameters: null, processReader, dbError);
        }

        /// <summary>
        /// Executes the scalar query.  Generates a db exception if a SqlException is caught.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="processReader">The process reader.</param>
        /// <param name="dbError">The database error.</param>
        /// <exception cref="CustomSearchesDatabaseException">Revert statement failed in the database.</exception>
        /// <returns>The async task.</returns>
        public static async Task ExecuteReaderWithRetriesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            SqlParameter[] parameters,
            Func<DbCommand, DbDataReader, Task> processReader,
            string dbError)
            where TDbContext : DbContext
        {
            try
            {
                await DbTransientRetryPolicy.ExecuteReaderAsync(
                    serviceContext,
                    async (command, dataReader) =>
                    {
                        await processReader(command, dataReader);
                    },
                    dbContextFactory,
                    script,
                    parameters);
            }
            catch (SqlException ex)
            {
                throw new CustomSearchesDatabaseException(dbError, ex);
            }
        }

        /// <summary>
        /// Gets the dynamic results asynchronously.  Note:  IEnumerable is resolved. May
        /// have performance issues with large datasets.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="dbError">The database error.</param>
        /// <returns>
        /// The dynamic results.
        /// </returns>
        public static async Task<IEnumerable<dynamic>> GetDynamicResultWithRetriesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string commandText,
            string dbError)
            where TDbContext : DbContext
        {
            try
            {
                return await DbTransientRetryPolicy.GetDynamicResultAsync(
                    serviceContext,
                    dbContextFactory,
                    commandText);
            }
            catch (SqlException ex)
            {
                throw new CustomSearchesDatabaseException(dbError, ex);
            }
        }

        /// <summary>
        /// Indicates if the exception is or contain sql exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// True if the exception is or contain sql exception.
        /// </returns>
        public static bool ContainTransientSqlException(Exception exception)
        {
            return DynamicSqlStatementHelper.GetSqlExceptions(exception).FirstOrDefault(sqlEx => DynamicSqlStatementHelper.IsTransientSqlException(sqlEx)) != null;
        }

        /// <summary>
        /// Indicates if the exception is or contain sql exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// True if the exception is or contain sql exception.
        /// </returns>
        public static bool ContainSqlException(Exception exception)
        {
            return DynamicSqlStatementHelper.GetSqlExceptions(exception).Count() > 0;
        }

        /// <summary>
        /// Gets the sql exceptions.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// The sql exceptions.
        /// </returns>
        private static IEnumerable<SqlException> GetSqlExceptions(Exception exception)
        {
            List<SqlException> sqlExceptions = new List<SqlException>();

            if (exception is AggregateException aggregateException)
            {
                sqlExceptions.AddRange(aggregateException.InnerExceptions.OfType<SqlException>());
            }
            else if (exception is SqlException sqlException)
            {
                sqlExceptions.Add(sqlException);
            }

            return sqlExceptions;
        }

        /// <summary>
        /// Indicates if the exception is or contain sql exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// True if the exception is or contain sql exception.
        /// </returns>
        private static bool IsTransientSqlException(SqlException exception)
        {
            return transientErrorNumbers.Contains(exception.Number);
        }
    }
}
