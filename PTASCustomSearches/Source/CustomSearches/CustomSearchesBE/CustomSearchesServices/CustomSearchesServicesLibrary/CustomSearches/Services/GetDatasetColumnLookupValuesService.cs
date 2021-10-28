namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the dataset column lookup values.
    /// </summary>
    public class GetDatasetColumnLookupValuesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetColumnLookupValuesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetColumnLookupValuesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the dataset column lookup values.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The dataset column lookup values.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process or column was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        public async Task<GetUserCustomSearchDataResponse> GetDatasetColumnLookupValuesAsync(Guid datasetId, bool usePostProcess, int? postProcessId, string columnName, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(columnName, nameof(columnName));
            columnName = columnName.Trim();
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

            DatasetHelper.AssertCanReadFromDataset(dataset, usePostProcess);

            GetUserCustomSearchDataResponse response = new GetUserCustomSearchDataResponse();
            response.Results = await this.GetDistinctValuesAsync(dataset, usePostProcess, datasetPostProcess, columnName);

            return response;
        }

        /// <summary>
        /// Gets the distinct values from database.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>>The distinct values.</returns>
        private async Task<object[]> GetDistinctValuesAsync(
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            string columnName)
        {
            List<object> result = new List<object>();

            string selectColumnStatement = await DatasetColumnHelper.GetColumnValuesSelectStatement(
                this.ServiceContext.DbContextFactory,
                dataset,
                columnName,
                usePostProcess,
                datasetPostProcess);

            var dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, selectColumnStatement);

            var dbColumn = dbColumns.FirstOrDefault(c => c.ColumnName.ToLower() == columnName.ToLower());
            InputValidationHelper.AssertEntityExists(dbColumn, "Column", columnName);

            string commandText = $"SELECT DISTINCT [{columnName}] FROM ({selectColumnStatement}) dv";

            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                commandText,
                async (DbCommand command, DbDataReader dataReader) =>
                {
                    while (await dataReader.ReadAsync())
                    {
                        result.Add(dataReader[columnName]);
                    }
                },
                $"Cannot get lookup values for column: '{columnName}'.");

            return result.ToArray();
        }
    }
}
