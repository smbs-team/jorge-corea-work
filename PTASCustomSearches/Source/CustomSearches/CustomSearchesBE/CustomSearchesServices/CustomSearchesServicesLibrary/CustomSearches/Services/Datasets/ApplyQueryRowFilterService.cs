namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that applies the query row filter in the dataset data.
    /// </summary>
    public class ApplyQueryRowFilterService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyQueryRowFilterService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ApplyQueryRowFilterService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Applies the query row filter in the dataset data.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="queryRowFilterData">The filter parameters.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The result objects.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process was not found.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Apply row filter failed in the database.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task ApplyQueryRowFilterAsync(
            Guid datasetId,
            bool usePostProcess,
            int? postProcessId,
            QueryRowFilterData queryRowFilterData,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            DatasetPostProcess datasetPostProcess = null;
            if (usePostProcess && (postProcessId > 0))
            {
                datasetPostProcess = await dbContext.DatasetPostProcess.FirstOrDefaultAsync(d => (d.DatasetPostProcessId == postProcessId) && d.DatasetId == datasetId);
                InputValidationHelper.AssertEntityExists(datasetPostProcess, "DatasetPostProcess", postProcessId);
            }

            usePostProcess = await DatasetHelper.CanUsePostProcessAsync(dataset, datasetPostProcess, usePostProcess, dbContext);
            DatasetHelper.AssertCanUsePostProcess(datasetPostProcess, usePostProcess);

            string tableName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string viewName = DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess);

            string whereScript = GetDatasetDataService.GenerateFilterScript(queryRowFilterData.FilterModel, tableAlias: "S");

            string script =
                $"UPDATE U " +
                $"SET [{DatasetColumnHelper.FilterStateColumnName}] = 'NotFilteredOut' " +
                $"FROM {tableName} U " +
                $"INNER JOIN {viewName} S ON U.[CustomSearchResultId] = S.[CustomSearchResultId] ";

            if (!string.IsNullOrWhiteSpace(whereScript))
            {
                script += $"WHERE {whereScript}\n";
                script +=
                $"UPDATE U " +
                $"SET [{DatasetColumnHelper.FilterStateColumnName}] = 'FilteredOut' " +
                $"FROM {tableName} U " +
                $"LEFT JOIN {viewName} S ON U.[CustomSearchResultId] = S.[CustomSearchResultId] " +
                $"WHERE S.[CustomSearchResultId] IS NULL OR NOT({whereScript})\n";
            }

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        script,
                        "Update statement failed in the database while applying the query row filter.");

                    return (datasetState, datasetPostProcessState);
                },
                dataset,
                isRootLock: false,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext);
        }
    }
}