namespace CustomSearchesServicesLibrary.ServiceFramework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using DocumentFormat.OpenXml.Drawing.Charts;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Database transient retry policy.
    /// </summary>
    public static class DbTransientRetryPolicy
    {
        /// <summary>
        /// The enable dynamic SQL logging flag. If enabled, dynamic sql code executed through
        /// DbTransientRetryPolicy Execute methods will be logged to the database.
        /// </summary>
        private static bool enableDynamicSqlLoggingInDb = false;

        /// <summary>
        /// Executes a non query database command.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The sql parameters.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public static async Task<int> ExecuteNonQueryAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            SqlParameter[] parameters,
            int[] retryDelays = null)
            where TDbContext : DbContext
        {
            int result = -1;
            await DbTransientRetryPolicy.LogDynamicScript(serviceContext, script, parameters);
            await RetryPolicyAsync(
                async () =>
                {
                    using (var context = dbContextFactory.Create())
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        await context.Database.OpenConnectionAsync();
                        command.CommandTimeout = 7200;
                        command.CommandText = script;

                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters.Select(p => ((ICloneable)p).Clone()).ToArray());
                        }

                        result = await command.ExecuteNonQueryAsync();
                    }
                },
                serviceContext.Logger,
                retryDelays);

            return result;
        }

        /// <summary>
        /// Executes a scalar database command.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The sql parameters.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public static async Task<object> ExecuteScalarAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string script,
            SqlParameter[] parameters,
            int[] retryDelays = null)
            where TDbContext : DbContext
        {
            object result = null;
            await DbTransientRetryPolicy.LogDynamicScript(serviceContext, script, parameters);
            await RetryPolicyAsync(
                async () =>
                {
                    using (var context = dbContextFactory.Create())
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        await context.Database.OpenConnectionAsync();
                        command.CommandTimeout = 7200;
                        command.CommandText = script;

                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters.Select(p => ((ICloneable)p).Clone()).ToArray());
                        }

                        result = await command.ExecuteScalarAsync();
                    }
                },
                serviceContext.Logger,
                retryDelays);

            return result;
        }

        /// <summary>
        /// Executes a data reader command against database.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="lambda">The lambda to execute.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The sql parameters.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public static async Task ExecuteReaderAsync<TDbContext>(
            IServiceContext serviceContext,
            Func<DbCommand, DbDataReader, Task> lambda,
            IFactory<TDbContext> dbContextFactory,
            string script,
            SqlParameter[] parameters,
            int[] retryDelays = null)
            where TDbContext : DbContext
        {
            await DbTransientRetryPolicy.LogDynamicScript(serviceContext, script, parameters);
            await RetryPolicyAsync(
                async () =>
                {
                    using (var context = dbContextFactory.Create())
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        await context.Database.OpenConnectionAsync();
                        command.CommandTimeout = 7200;
                        command.CommandText = script;

                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters.Select(p => ((ICloneable)p).Clone()).ToArray());
                        }

                        using (var dataReader = await command.ExecuteReaderAsync())
                        {
                            await lambda(command, dataReader);
                        }
                    }
                },
                serviceContext.Logger,
                retryDelays);
        }

        /// <summary>
        /// Adds aworker job queue.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="jobType">The dataset id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="jobPayload">The job payload.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="retryDelays">The retry delays.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        public static async Task<int> AddWorkerJobQueueAsync(
            IServiceContext serviceContext,
            string queueName,
            string jobType,
            Guid userId,
            object jobPayload,
            int timeout,
            int[] retryDelays = null)
        {
            string payload = JsonHelper.SerializeObject(jobPayload);
            string commandText = "INSERT INTO [dbo].[WorkerJobQueue] ([QueueName], [JobType], [UserId], [TimeoutInSeconds], [JobPayLoad])\n";
            commandText += "OUTPUT INSERTED.JobId\n";

            commandText += string.Format("VALUES('{0}', '{1}', '{2}', '{3}', @payload)\n", queueName, jobType, userId, timeout);
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@payload", payload);
            int result = (int)(await DbTransientRetryPolicy.ExecuteScalarAsync(
                serviceContext,
                serviceContext.WorkerJobDbContextFactory,
                commandText,
                parameters,
                retryDelays));

            return result;
        }

        /// <summary>
        /// Gets the worker job result.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="jobId">The job id.</param>
        /// <param name="retryDelays">The retry delays.</param>
        /// <returns>
        /// The job result.
        /// </returns>
        public static async Task<string> GetWorkerJobResultAsync(
            IServiceContext serviceContext,
            int jobId,
            int[] retryDelays = null)
        {
            string commandText = "SELECT JobResult FROM [dbo].[WorkerJobQueue] where JobId = " + jobId;
            object result = await DbTransientRetryPolicy.ExecuteScalarAsync(
                serviceContext,
                serviceContext.WorkerJobDbContextFactory,
                commandText,
                parameters: null,
                retryDelays);

            return ((result == null) || (result.GetType() == typeof(System.DBNull))) ? string.Empty : result.ToString();
        }

        /// <summary>
        /// Gets the worker job.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="jobId">The queue name.</param>
        /// <param name="includePayload">A value indicating whether the worker job should include the payload.</param>
        /// <param name="retryDelays">The retry delays.</param>
        /// <returns>
        /// The worker job.
        /// </returns>
        public static async Task<WorkerJobQueue> GetWorkerJobAsync(
            IServiceContext serviceContext,
            int jobId,
            bool includePayload,
            int[] retryDelays = null)
        {
            WorkerJobQueue result = null;
            string commandText = "SELECT [JobId],[UserId],[StartedTimestamp],[CreatedTimestamp],[QueueName],[JobType],[JobResult],[ExecutionTime]";
            if (includePayload)
            {
                commandText += ",[JobPayLoad]";
            }

            commandText += $" FROM [dbo].[WorkerJobQueue] WHERE [JobId] = {jobId}";

            await DbTransientRetryPolicy.ExecuteReaderAsync(
                serviceContext,
                async (command, dataReader) =>
                {
                    await dataReader.ReadAsync();
                    result = DbTransientRetryPolicy.JobFromReader(dataReader, includePayload);
                },
                serviceContext.WorkerJobDbContextFactory,
                commandText,
                parameters: null,
                retryDelays);

            return result;
        }

        /// <summary>
        /// Gets the pending worker jobs.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="includePayload">A value indicating whether the worker job should include the payload.</param>
        /// <param name="maxPayloadSize">Maximum size of the payload.</param>
        /// <param name="maxJobs">The maximun amount of jobs to return.</param>
        /// <param name="retryDelays">The retry delays.</param>
        /// <returns>
        /// The worker job.
        /// </returns>
        public static async Task<IEnumerable<WorkerJobQueue>> GetPendingWorkerJobsAsync(
            IServiceContext serviceContext,
            Guid userId,
            bool includePayload,
            int maxPayloadSize,
            int maxJobs = 10,
            int[] retryDelays = null)
        {
            var results = new List<WorkerJobQueue>();
            string commandText = $"SELECT TOP {maxJobs} [JobId],[UserId],[StartedTimestamp],[CreatedTimestamp],[QueueName],[JobType],[JobResult],[ExecutionTime]";
            if (includePayload)
            {
                commandText += $",IIF(LEN([JobPayLoad]) < {maxPayloadSize}, [JobPayLoad], NULL) AS [JobPayLoad]";
            }

            commandText += $" FROM [dbo].[WorkerJobQueue] WHERE [UserId] = '{userId}' AND [JobResult] IS NULL";
            commandText += " ORDER BY CreatedTimestamp DESC";

            await DbTransientRetryPolicy.ExecuteReaderAsync(
                serviceContext,
                async (command, dataReader) =>
                {
                    while (await dataReader.ReadAsync())
                    {
                        var workerJob = DbTransientRetryPolicy.JobFromReader(dataReader, includePayload);
                        results.Add(workerJob);
                    }
                },
                serviceContext.WorkerJobDbContextFactory,
                commandText,
                parameters: null,
                retryDelays);

            return results;
        }

        /// <summary>
        /// Determines whether the custom search table exists in database.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>
        /// The column schema.
        /// </returns>
        public static async Task<bool> ExistCustomSearchTableAsync(
            IServiceContext serviceContext,
            string tableName)
        {
            string script = string.Empty;
            script += "SELECT Count (*)\n";
            script += "FROM INFORMATION_SCHEMA.TABLES\n";
            script += "WHERE TABLE_SCHEMA = 'cus'\n";
            script += "AND  TABLE_NAME = '" + tableName + "'\n";

            var foundTemplate = (int)await DynamicSqlStatementHelper.ExecuteScalarWithRetriesAsync(
                serviceContext,
                serviceContext.DataDbContextFactory,
                script,
                $"Cannot verify if the table exists: '{tableName}'.");

            return foundTemplate > 0;
        }

        /// <summary>
        /// Gets the table column schema.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="tableFullName">The table full name.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// The column schema.
        /// </returns>
        public static async Task<ReadOnlyCollection<DbColumn>> GetTableColumnSchemaAsync(
            IServiceContext serviceContext,
            string tableFullName,
            int[] retryDelays = null)
        {
            ReadOnlyCollection<DbColumn> result = null;
            string commandText = $"SELECT TOP(1) * FROM {tableFullName}";

            await DbTransientRetryPolicy.ExecuteReaderAsync(
                serviceContext,
                async (command, dataReader) =>
                {
                    result = dataReader.GetColumnSchema();
                },
                serviceContext.DataDbContextFactory,
                commandText,
                parameters: null,
                retryDelays);

            return result;
        }

        /// <summary>
        /// Gets the dataset column schema.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// The column schema.
        /// </returns>
        public static async Task<ReadOnlyCollection<DbColumn>> GetDatasetColumnSchemaAsync(
            IServiceContext serviceContext,
            Dataset dataset,
            int[] retryDelays = null)
        {
            Guid datasetId = (dataset.SourceDatasetId == null || dataset.SourceDatasetId == Guid.Empty) ? dataset.DatasetId : (Guid)dataset.SourceDatasetId;
            string tableName = string.Format(CustomSearchesDataDbContext.CustomSearchResultFullTableFormat, datasetId.ToString().Replace("-", "_"));

            return await GetTableColumnSchemaAsync(serviceContext, tableName, retryDelays);
        }

        /// <summary>
        /// Gets the dataset post process view column schema.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="datasetView">The dataset view.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// The column schema.
        /// </returns>
        public static async Task<ReadOnlyCollection<DbColumn>> GetDatasetViewColumnSchemaAsync(
            IServiceContext serviceContext,
            string datasetView,
            int[] retryDelays = null)
        {
            ReadOnlyCollection<DbColumn> result = null;
            string commandText = "SELECT TOP(1) * FROM ";
            if (datasetView.TrimStart().ToLower().StartsWith("select"))
            {
                commandText += $"({datasetView}) b";
            }
            else
            {
                commandText += $"{datasetView}";
            }

            await DbTransientRetryPolicy.ExecuteReaderAsync(
                serviceContext,
                async (command, dataReader) =>
                {
                    result = dataReader.GetColumnSchema();
                },
                serviceContext.DataDbContextFactory,
                commandText,
                parameters: null,
                retryDelays);

            return result;
        }

        /// <summary>
        /// Gets the dataset update view script.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetView">The dataset view.</param>
        /// <param name="isPostProcess">Value indicating whether it is a post process.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// The update view.
        /// </returns>
        public static async Task<string> GetDatasetUpdateViewScriptAsync(
            IServiceContext serviceContext,
            Dataset dataset,
            string datasetView,
            bool isPostProcess,
            CustomSearchesDbContext dbContext,
            int[] retryDelays = null)
        {
            int rowVersion = isPostProcess ? int.MaxValue : 0;
            string updateTableFullName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset);
            var dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(serviceContext, datasetView, retryDelays);
            var columns = await dbContext.CustomSearchColumnDefinition
                .Where(d => d.CustomSearchDefinitionId == dataset.CustomSearchDefinitionId && d.IsEditable == true)
                .ToDictionaryAsync(d => d.ColumnName.ToLower(), d => d);

            string script = "SELECT ";
            if (!isPostProcess)
            {
                script += "u.[ErrorMessage], u.[IsValid], u.[Validated], u.[BackendExportState], u.[ExportedToBackEndErrorMessage],\n";
            }

            foreach (var dbColumn in dbColumns)
            {
                if (columns.ContainsKey(dbColumn.ColumnName.ToLower()))
                {
                    script += $"ISNULL(u.[{dbColumn.ColumnName}], s.[{dbColumn.ColumnName}]) [{dbColumn.ColumnName}],\n";
                }
                else
                {
                    script += $"s.[{dbColumn.ColumnName}],\n";
                }
            }

            script = script.TrimEnd(new char[] { ',', '\n' }) + "\n";
            script += $"FROM ({datasetView}) s\n";
            script += $"LEFT JOIN {updateTableFullName} u ON (s.[CustomSearchResultId] = u.[CustomSearchResultId])";
            script += $" AND (u.[RowVersion] = {rowVersion})";

            return script;
        }

        /// <summary>
        /// Gets the dataset view script.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="expressions">The custom search expressions.</param>
        /// <param name="applyUpdates">Value indicating whether the view should include the dataset updates.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// The dataset base view script.
        /// </returns>
        public static async Task<string> GetDatasetViewScriptAsync(
            IServiceContext serviceContext,
            Dataset dataset,
            List<CustomSearchExpression> expressions,
            bool applyUpdates,
            CustomSearchesDbContext dbContext,
            int[] retryDelays = null)
        {
            string datasetTableName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string fromScript;
            if (applyUpdates)
            {
                string datasetBaseViewScript = $"SELECT * FROM {datasetTableName} WHERE [FilterState] != 'FilteredOut'";
                fromScript = await DbTransientRetryPolicy.GetDatasetUpdateViewScriptAsync(serviceContext, dataset, datasetBaseViewScript, isPostProcess: false, dbContext, retryDelays);
            }
            else
            {
                fromScript = datasetTableName;
            }

            string script = "SELECT *";
            if (expressions?.Count > 0)
            {
                foreach (var expression in expressions)
                {
                    script += $", {expression.Script} AS [{expression.ColumnName}]";
                }
            }

            script += "\n";
            script += $"FROM ({fromScript}) v";

            return script;
        }

        /// <summary>
        /// Gets the dynamic results.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="retryDelays">An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.</param>
        /// <returns>
        /// The dynamic results.
        /// </returns>
        public static async Task<IEnumerable<dynamic>> GetDynamicResultAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string commandText,
            int[] retryDelays = null)
            where TDbContext : DbContext
        {
            List<dynamic> results = new List<dynamic>();
            await DbTransientRetryPolicy.ExecuteReaderAsync(
                serviceContext,
                async (command, dataReader) =>
                {
                    results.Clear();

                    // List for column names
                    var columnNames = new List<string>();

                    if (dataReader.HasRows)
                    {
                        // Add column names to list
                        for (var i = 0; i < dataReader.VisibleFieldCount; i++)
                        {
                            columnNames.Add(dataReader.GetName(i));
                        }

                        while (await dataReader.ReadAsync())
                        {
                            // Create the dynamic result for each row
                            var result = new ExpandoObject() as IDictionary<string, object>;

                            foreach (var columnName in columnNames)
                            {
                                result.Add(columnName, dataReader[columnName]);
                            }

                            results.Add(result);
                        }
                    }
                },
                dbContextFactory,
                commandText,
                parameters: null,
                retryDelays);

            return results;
        }

        /// <summary>
        /// Drops a table in database.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="retryDelays">The retry delays.</param>
        /// <returns>The async task.</returns>
        public static async Task DropTableAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string tableName,
            int[] retryDelays = null)
            where TDbContext : DbContext
        {
            string script = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + tableName + "') AND type in (N'U'))" + "\n";
            script += "DROP TABLE " + tableName;

            await DbTransientRetryPolicy.ExecuteNonQueryAsync(serviceContext, dbContextFactory, script, parameters: null, retryDelays);
        }

        /// <summary>
        /// Drops dataset tables in database.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="retryDelays">The retry delays.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public static async Task<int> DropDatasetTablesAsync<TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            Dataset dataset,
            int[] retryDelays = null)
            where TDbContext : DbContext
        {
            // This script drops dataset tables and views based in current naming convention. If the naming convention changes this script should be revised.
            string datasetView = CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, usePostProcess: false);
            string datasetPostProcessView = CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, usePostProcess: true);
            string datasetTable = CustomSearchesDataDbContext.GetDatasetTableName(dataset.DatasetId);
            string datasetUpdateTable = CustomSearchesDataDbContext.GetDatasetUpdateTableName(dataset);
            string datasetPostProcessTableStarts = datasetTable + "_PostProcess_";
            string script = $"IF OBJECT_ID('{datasetPostProcessView}') IS NOT NULL BEGIN DROP VIEW {datasetPostProcessView} END;\n";
            script += $"IF OBJECT_ID('{datasetView}') IS NOT NULL BEGIN DROP VIEW {datasetView} END;\n";

            script += "DECLARE @cmd varchar(4000)\n";
            script += "DECLARE cmds CURSOR FOR\n";
            script += "SELECT 'drop table [cus].[' + Table_Name + ']'\n";
            script += "FROM INFORMATION_SCHEMA.TABLES\n";
            script += $"WHERE Table_Name LIKE '{datasetPostProcessTableStarts}%'\n";

            script += "OPEN cmds\n";
            script += "WHILE 1 = 1\n";
            script += "BEGIN\n";
            script += "    FETCH cmds INTO @cmd\n";
            script += "    IF @@fetch_status != 0 BREAK\n";
            script += "    EXEC(@cmd)\n";
            script += "END\n";
            script += "CLOSE cmds;\n";
            script += "DEALLOCATE cmds\n";

            script += $"IF OBJECT_ID('[cus].[{datasetUpdateTable}]') IS NOT NULL BEGIN DROP TABLE [cus].[{datasetUpdateTable}] END;\n";

            if (dataset.SourceDatasetId == null)
            {
                script += $"IF OBJECT_ID('[cus].[{datasetTable}]') IS NOT NULL BEGIN DROP TABLE [cus].[{datasetTable}] END;\n";
            }

            foreach (var datasetPostProcess in dataset.DatasetPostProcess)
            {
                script += DatasetHelper.GetDeleteBackendUpdateScript(datasetPostProcess.DatasetPostProcessId);
            }

            return await DbTransientRetryPolicy.ExecuteNonQueryAsync(serviceContext, dbContextFactory, script, parameters: null, retryDelays);
        }

        /// <summary>
        /// Sets the value of the DynamicSqlLoggingEnabled flag.  If enabled, dynamic sql code executed through
        /// DbTransientRetryPolicy Execute methods will be logged to the database.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> the flag is enabled.  Disabled otherwise.</param>
        public static void SetDynamicSqlLoggingInDbEnabled(bool enabled)
        {
            DbTransientRetryPolicy.enableDynamicSqlLoggingInDb = enabled;
        }

        /// <summary>
        /// Wraps the script with logging code (if enableDynamicSqlLogging is turned on). Modifies the parameter list
        /// to add new parameters needed for the logging.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The parameters.</param>
        private static async Task LogDynamicScript(IServiceContext serviceContext, string script, SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(script) || !DbTransientRetryPolicy.enableDynamicSqlLoggingInDb)
            {
                return;
            }

            try
            {
                using (var context = serviceContext.DbContextFactory.Create())
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    string serializedSqlParameters = string.Empty;

                    if (parameters != null)
                    {
                        var parametersToSerialize = from p in parameters select new { p.ParameterName, p.SqlDbType, p.Value };
                        serializedSqlParameters = JsonHelper.SerializeObject(parametersToSerialize);
                    }

                    List<SqlParameter> newParameters = new List<SqlParameter>();
                    Guid userId = serviceContext.AuthProvider.UserInfoData?.Id ?? Guid.Empty;
                    int jobId = -1;

                    newParameters.Add(new SqlParameter("@LogUserId", userId));
                    newParameters.Add(new SqlParameter("@SqlLog", script));
                    newParameters.Add(new SqlParameter("@SqlParametersLog", serializedSqlParameters));
                    newParameters.Add(new SqlParameter("@JobId", jobId));

                    string logScript =
                        "INSERT INTO[cus].[DynamicSqlLog]\n" +
                        "  ([UserId], [SqlLog], [Parameters], [JobId])\n" +
                        "  VALUES (@LogUserId, @SqlLog, @SqlParametersLog, @JobId);\n";

                    await context.Database.OpenConnectionAsync();
                    command.CommandTimeout = 7200;
                    command.CommandText = logScript;

                    command.Parameters.AddRange(newParameters.Select(p => ((ICloneable)p).Clone()).ToArray());

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                // We never want to throw because loggging failed.
            }
        }

        /// <summary>
        /// Retries the lamda expression when a transient sql exception occurs.
        /// </summary>
        /// <param name="lambda">The lambda to execute.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="retryDelays">
        /// An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.
        /// </param>
        /// <returns>The output response for the input exception.</returns>
        private static async Task RetryPolicyAsync(Func<Task> lambda, ILogger logger, int[] retryDelays = null)
        {
            retryDelays ??= new[] { 5000, 25000, 60000 };

            for (int i = 0; i <= retryDelays.Length; i++)
            {
                try
                {
                    await lambda();
                    return;
                }
                catch (Exception ex)
                {
                    if (DynamicSqlStatementHelper.ContainTransientSqlException(ex))
                    {
                        if (i < retryDelays.Length)
                        {
                            logger.LogWarning(ex, $"Transient exception found while executing sql statement: {ex}");
                            await Task.Delay(retryDelays[i]);
                            continue;
                        }
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Instantiates an EF job entity from the contents of a data reader.
        /// </summary>
        /// <param name="dataReader">The reader.</param>
        /// <returns>The job object.</returns>
        private static WorkerJobQueue JobFromReader(DbDataReader dataReader, bool includePayload)
        {
            var job = new WorkerJobQueue()
            {
                JobId = (int)dataReader["JobId"],
                UserId = (Guid)dataReader["UserId"],
                StartedTimestamp = dataReader["StartedTimestamp"] is DBNull ? null : (DateTime?)dataReader["StartedTimestamp"],
                CreatedTimestamp = dataReader["CreatedTimestamp"] is DBNull ? null : (DateTime?)dataReader["CreatedTimestamp"],
                QueueName = dataReader["QueueName"] is DBNull ? null : (string)dataReader["QueueName"],
                JobType = (string)dataReader["JobType"],
                JobResult = dataReader["JobResult"] is DBNull ? null : (string)dataReader["JobResult"],
                ExecutionTime = dataReader["ExecutionTime"] is DBNull ? null : (decimal?)dataReader["ExecutionTime"]
            };

            if (includePayload)
            {
                job.JobPayload = dataReader["JobPayload"] is DBNull ? null : (string)dataReader["JobPayload"];
            }

            return job;
        }
    }
}