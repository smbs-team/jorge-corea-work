namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Context in which expression validation occurs.
    /// </summary>
    public class ExpressionValidationContext
    {
        /// <summary>
        /// Value indicating whether the dataset context has valid state.  Works as a cache for the information.
        /// </summary>
        private bool? datasetContextHasValidState;

        /// <summary>
        /// The db columns for the dataset context.  Works as a cache for the information.
        /// </summary>
        private ReadOnlyCollection<DbColumn> datasetContextDbColumns;

        /// <summary>
        /// Value indicating whether post-processes view can be used to query the dataset.  Works as a cache for the information.
        /// </summary>
        private bool? canUsePostProcess;

        /// <summary>
        /// Gets or sets the dataset context.  This is used when a real dataset is needed for the validations.
        /// </summary>
        public Dataset DatasetContext { get; set; }

        /// <summary>
        /// Gets or sets the previous dataset post process context.  This is the dataset that would execute just before this one.
        /// </summary>
        public DatasetPostProcess PreviousPostProcessContext { get; set; }

        /// <summary>
        /// Gets or sets the dataset post process context.  This is used to determine which version of the view should be used
        /// for the validations.
        /// </summary>
        public DatasetPostProcess PostProcessContext { get; set; }

        /// <summary>
        /// Gets or sets the last executed statement.  This is used by error reporting to show the sql statement that failed executing.
        /// </summary>
        public string LastExecutedStatement { get; set; }

        /// <summary>
        /// Gets or sets the service context.
        /// </summary>
        public IServiceContext ServiceContext { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [throw on fail].
        /// </summary>
        public bool ThrowOnFail { get; set; }

        /// <summary>
        /// Gets or sets the chart type context.  Used to give context to evaluate chart related expressions.
        /// </summary>
        public string ChartTypeContext { get; set; }

        /// <summary>
        /// Asserts the DatasetContext property is not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">DatasetContext.</exception>
        public void AssertDatasetContextNotNull()
        {
            if (this.DatasetContext == null)
            {
                throw new ArgumentNullException(nameof(this.DatasetContext));
            }
        }

        /// <summary>
        /// Determines whether the dataset context can use the post-processes flag for queries.
        /// </summary>
        /// <returns>True if the dataset context can use the post-processes flag for queries .</returns>
        public async Task<bool> CanUsePostProcessAsync()
        {
            if (!this.canUsePostProcess.HasValue)
            {
                using (var dbContext = this.ServiceContext.DbContextFactory.Create())
                {
                    this.canUsePostProcess = await DatasetHelper.CanUsePostProcessAsync(
                        this.DatasetContext,
                        this.PreviousPostProcessContext,
                        usePostProcess: this.PreviousPostProcessContext != null,
                        dbContext);
                }
            }

            return this.canUsePostProcess.Value;
        }

        /// <summary>
        /// Gets the dataset script.  This script is used as a base for TSQL expression validation.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// The script that selects the records from the dataset.
        /// </returns>
        public async Task<string> GetDatasetScript()
        {
            this.AssertDatasetContextNotNull();

            var usePostProcess = await this.CanUsePostProcessAsync();
            return GetDatasetDataService.GenerateSearchScript(
                this.DatasetContext,
                startIndex: 0,
                rowCount: 1,
                usePostProcess,
                this.PreviousPostProcessContext,
                false);
        }

        /// <summary>
        /// Gets collection of db columns for a given context.
        /// </summary>
        /// <returns>The database columns for the dataset.</returns>
        public async Task<ReadOnlyCollection<DbColumn>> GetDatasetContextDbColumns()
        {
            this.AssertDatasetContextNotNull();

            if (this.datasetContextDbColumns == null)
            {
                bool usePostProcess = await this.CanUsePostProcessAsync();
                string datasetView = DatasetHelper.GetDatasetView(this.DatasetContext, usePostProcess, this.PreviousPostProcessContext);
                this.datasetContextDbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(
                    this.ServiceContext,
                    datasetView);
            }

            return this.datasetContextDbColumns;
        }

        /// <summary>
        /// Gets a value indicating whether the dataset context has a valid state (i.e. not locked or dirty).
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The validation results.
        /// </returns>
        public async Task<(bool result, string message)> DatasetContextHasValidState()
        {
            this.AssertDatasetContextNotNull();

            if (!this.datasetContextHasValidState.HasValue)
            {
                using (var dbContext = this.ServiceContext.DbContextFactory.Create())
                {
                    var usePostProcess = await this.CanUsePostProcessAsync();

                    this.datasetContextHasValidState =
                        DatasetHelper.CanReadFromDataset(this.DatasetContext, usePostProcess);

                    if (!this.datasetContextHasValidState.Value)
                    {
                        string message;
                        if (this.DatasetContext.DataSetState == DatasetStateType.Failed.ToString())
                        {
                            message = string.Format(DatasetHelper.DatasetFailedErrorMessage, this.DatasetContext.DatasetId);
                        }
                        else
                        {
                            message = $"The dataset '{this.DatasetContext.DatasetId}' is currently executing another process." +
                            $" Please retry the operation after a short period of time. If the problem persists contact your administrator.";
                        }

                        return (false, message);
                    }

                    var prevPostProcess = this.PreviousPostProcessContext;
                    if (prevPostProcess != null)
                    {
                        if (!this.PreviousPostProcessContext.IsDirty && !string.IsNullOrWhiteSpace(this.PreviousPostProcessContext.CalculatedView))
                        {
                            var dirtyPostProcess =
                                await (from pp in dbContext.DatasetPostProcess
                                       where ((pp.IsDirty == true) || string.IsNullOrWhiteSpace(pp.CalculatedView)) &&
                                             ((pp.DatasetId == this.DatasetContext.DatasetId) &&
                                              ((pp.Priority < prevPostProcess.Priority) ||
                                               ((pp.Priority == prevPostProcess.Priority) && (pp.ExecutionOrder < prevPostProcess.ExecutionOrder)) ||
                                               ((pp.Priority == prevPostProcess.Priority) && (pp.ExecutionOrder == prevPostProcess.ExecutionOrder) && (pp.CreatedTimestamp < prevPostProcess.CreatedTimestamp))))
                                       select pp).FirstOrDefaultAsync();

                            this.datasetContextHasValidState = dirtyPostProcess == null;
                        }
                        else
                        {
                            this.datasetContextHasValidState = false;
                        }

                        if (!this.datasetContextHasValidState.Value)
                        {
                            string message = $"The dataset '{this.DatasetContext.DatasetId}' has another post process in a dirty state." +
                                $" Make sure all existing post-processes have executed with no errors before trying this operation.";
                            return (false, message);
                        }
                    }
                }
            }

            return (this.datasetContextHasValidState.Value, string.Empty);
        }
    }
}
