namespace CustomSearchesServicesLibrary.CustomSearches.Executor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Post process view phases.
    /// </summary>
    public enum PostProcessViewPhase
    {
        /// <summary>
        /// Post process view phase pre commit.
        /// </summary>
        PreCommit,

        /// <summary>
        /// Post process view phase post commit.
        /// </summary>
        PostCommit
    }

    /// <summary>
    /// Executor for dataset post process.
    /// </summary>
    public class DatasetPostProcessExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetPostProcessExecutor"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previousViewScript.</param>
        /// <param name="singleRowExecutionData">The single row execution data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="serviceContext">The service context.</param>
        public DatasetPostProcessExecutor(
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            string previousViewScript,
            SingleRowExecutionData singleRowExecutionData,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext)
            : base()
        {
            this.ViewScript = previousViewScript;
            this.Dataset = dataset;
            this.DatasetPostProcess = datasetPostProcess;
            this.SingleRowExecutionData = singleRowExecutionData;
            this.DbContext = dbContext;
            this.ServiceContext = serviceContext;
        }

        /// <summary>
        /// Gets or sets the security principal.
        /// </summary>
        public IServiceContext ServiceContext { get; set; }

        /// <summary>
        /// Gets or sets the view script.
        /// </summary>
        public string ViewScript { get; set; }

        /// <summary>
        /// Gets or sets the dataset.
        /// </summary>
        protected Dataset Dataset { get; set; }

        /// <summary>
        /// Gets or sets the dataset post process.
        /// </summary>
        protected DatasetPostProcess DatasetPostProcess { get; set; }

        /// <summary>
        /// Gets or sets the single row execution data.
        /// </summary>
        protected SingleRowExecutionData SingleRowExecutionData { get; set; }

        /// <summary>
        /// Gets or sets the database context.
        /// </summary>
        protected CustomSearchesDbContext DbContext { get; set; }

        /// <summary>
        /// Deletes the previous state.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public virtual async Task DeletePreviousStateAsync()
        {
        }

        /// <summary>
        /// Commits the new state.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public virtual async Task CommitNewStateAsync()
        {
        }

        /// <summary>
        /// Calculates the view.
        /// </summary>
        /// <param name="postProcessViewPhase">The post process view phase.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public virtual async Task CalculateViewAsync(PostProcessViewPhase postProcessViewPhase)
        {
        }

        /// <summary>
        /// Commits the view.
        /// </summary>
        /// <param name="postProcessViewPhase">The post process view phase.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public virtual async Task CommitViewAsync(PostProcessViewPhase postProcessViewPhase)
        {
            if (this.SingleRowExecutionData.IsSingleRowExecutionMode)
            {
                return;
            }

            string updateView = await DbTransientRetryPolicy.GetDatasetUpdateViewScriptAsync(
                this.ServiceContext,
                this.Dataset,
                this.ViewScript,
                isPostProcess: true,
                this.DbContext);

            this.DatasetPostProcess.CalculatedView = updateView;
            string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.Dataset, usePostProcess: true);
            string alterScript = "CREATE OR ALTER VIEW " + viewName + " AS\n";

            await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                alterScript + updateView,
                $"Cannot commit dataset post process view: '{viewName}'.");

            // Saves dataset post process in order to store the calculated view.
            this.DatasetPostProcess.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            this.DatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
            this.DatasetPostProcess.LastExecutionTimestamp = DateTime.UtcNow;
            this.DbContext.DatasetPostProcess.Update(this.DatasetPostProcess);
            await this.DbContext.ValidateAndSaveChangesAsync();
        }

        /// <summary>
        /// Update the view script using the post process table.
        /// </summary>
        /// <param name="expressionColumnNames">The expression column names to use in the post process table.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected async Task UpdateViewScriptWithPostProcessTableAsync(HashSet<string> expressionColumnNames)
        {
            string datasetView = this.ViewScript;
            if (string.IsNullOrWhiteSpace(datasetView))
            {
                datasetView = DatasetHelper.GetDatasetView(this.Dataset, usePostProcess: false, datasetPostProcess: null);
            }

            var dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

            HashSet<string> schemaColumnsName = dbColumns.Select(c => c.ColumnName.Trim()).ToHashSet();

            string columnScript = string.Empty;
            foreach (string columnName in schemaColumnsName)
            {
                if (expressionColumnNames.Contains(columnName, StringComparer.OrdinalIgnoreCase))
                {
                    columnScript += $"CASE WHEN b.[CustomSearchResultId] IS NULL THEN a.[{columnName}] ELSE b.[{columnName}] END [{columnName}], ";
                }
                else
                {
                    columnScript += $"a.[{columnName}], ";
                }
            }

            HashSet<string> columnNamesNotFoundInSchema = expressionColumnNames.Except(schemaColumnsName, StringComparer.OrdinalIgnoreCase).ToHashSet();
            foreach (string columnName in columnNamesNotFoundInSchema)
            {
                columnScript += $"b.[{columnName}], ";
            }

            columnScript = columnScript.TrimEnd(new char[] { ',', ' ' });

            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.Dataset, !string.IsNullOrWhiteSpace(this.ViewScript) /*usePostProcess*/);
            string previousViewScript = string.IsNullOrWhiteSpace(this.ViewScript) ? ("SELECT * FROM " + viewName) : this.ViewScript;

            string script = "SELECT " + columnScript + "\n";
            script += "FROM (" + previousViewScript + ") a\n";
            script += "LEFT JOIN " + tableName + " b\n";
            script += "ON a.CustomSearchResultId = b.CustomSearchResultId" + "\n";
            this.ViewScript = script;
        }

        /// <summary>
        /// Calculates the view using the post process table.
        /// </summary>
        /// <param name="customSearchExpressionType">The custom search expression type.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected async Task CalculateViewWithPostProcessTableAsync(CustomSearchExpressionType customSearchExpressionType)
        {
            var expressionColumnNames = this.DatasetPostProcess
                .CustomSearchExpression.Where(c => c.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower() &&
                    c.ExpressionType.Trim().ToLower() == customSearchExpressionType.ToString().ToLower())
                .OrderBy(c => c.ExecutionOrder)
                .Select(e => e.ColumnName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (expressionColumnNames.Count() == 0)
            {
                return;
            }

            await this.UpdateViewScriptWithPostProcessTableAsync(expressionColumnNames);

            string updateView = await DbTransientRetryPolicy.GetDatasetUpdateViewScriptAsync(
                this.ServiceContext,
                this.Dataset,
                this.ViewScript,
                isPostProcess: true,
                this.DbContext);

            this.DatasetPostProcess.CalculatedView = updateView;
        }
    }
}
