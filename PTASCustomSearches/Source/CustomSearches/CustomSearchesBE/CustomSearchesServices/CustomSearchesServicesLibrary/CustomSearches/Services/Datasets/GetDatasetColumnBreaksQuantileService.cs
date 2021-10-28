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
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets class break ranges for a column according to the Quantile algorithm.
    /// </summary>
    public class GetDatasetColumnBreaksQuantileService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetColumnBreaksQuantileService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetColumnBreaksQuantileService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets class break ranges for a column according to the Quantile algorithm.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="breaksData">Data necessary to generate the quantiles.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The quantile ranges.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or postprocess was not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Verifies if it is allowed to read from dataset.</exception>
        public async Task<GetUserCustomSearchDataResponse> GetDatasetColumnBreaksQuantileAsync(
            Guid datasetId,
            bool usePostProcess,
            int? postProcessId,
            string columnName,
            ColumnQuantileBreaksData breaksData,
            CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(columnName, nameof(columnName));
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
            response.Results = await this.GeQuantileBreaksAsync(
                dataset,
                usePostProcess,
                datasetPostProcess,
                columnName,
                breaksData);

            response.TotalRows = response.Results != null ? response.Results.Length : 0;

            return response;
        }

        /// <summary>
        /// Gets the range values from database.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="breaksData">Data necessary to generate the quantiles.</param>
        /// <returns>>The range values.</returns>
        private async Task<object[]> GeQuantileBreaksAsync(
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            string columnName,
            ColumnQuantileBreaksData breaksData)
        {
            List<object> result = new List<object>();

            string selectColumnStatement = await DatasetColumnHelper.GetColumnValuesSelectStatement(
                this.ServiceContext.DbContextFactory,
                dataset,
                columnName,
                usePostProcess,
                datasetPostProcess);

            // Since this will be sent quoted to the SP, we need to escape quotes.
            selectColumnStatement = selectColumnStatement.Replace("'", "''");

            string commandText =
                $"Exec [cus].[SP_GetQuantileBreaks] '{selectColumnStatement}', " +
                $"'{columnName}', '{breaksData.FilterEmptyValuesExpression?.Script}', {breaksData.ClassBreakCount} ";

            var breaks = new List<(object MinValue, object MaxValue)>();

            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                commandText,
                async (DbCommand command, DbDataReader dataReader) =>
                {
                    breaks.Clear();
                    while (await dataReader.ReadAsync())
                    {
                        object minValue = dataReader["MinValue"];
                        object maxValue = dataReader["MaxValue"];
                        breaks.Add((minValue, maxValue));
                    }
                },
                $"Cannot get class break values from column: '{columnName}'.");

            var results = new List<object>();

            if (breaks.Count > 0)
            {
                foreach (var classBreak in breaks)
                {
                    if (results.Count() == 0)
                    {
                        results.Add(classBreak.MinValue);
                        if (!classBreak.MaxValue.Equals(classBreak.MinValue))
                        {
                            results.Add(classBreak.MaxValue);
                        }
                    }
                    else
                    {
                        var lastResult = results.Last();
                        if (!classBreak.MaxValue.Equals(lastResult))
                        {
                            results.Add(classBreak.MaxValue);
                        }
                    }
                }
            }

            return results.ToArray();
        }
    }
}
