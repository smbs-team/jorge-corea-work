namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets class break ranges for a column according to the Standard Deviation algorithm.
    /// </summary>
    public class GetDatasetColumnBreaksStdDeviationService : BaseService
    {
        /// <summary>
        /// The maximum iterations allowed for the interval algorithm.
        /// </summary>
        private const int MaxIterations = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetColumnBreaksStdDeviationService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetColumnBreaksStdDeviationService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets class break ranges for a column according to the Standard Deviation algorithm.
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
        public async Task<GetUserCustomSearchDataResponse> GetDatasetColumnBreaksStdDeviationAsync(
            Guid datasetId,
            bool usePostProcess,
            int? postProcessId,
            string columnName,
            ColumnStandardDeviationBreaksData breaksData,
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

            DatasetHelper.AssertCanReadFromDataset(dataset, usePostProcess);

            GetUserCustomSearchDataResponse response = new GetUserCustomSearchDataResponse();
            response.Results = await this.GeStandardDeviationBreaksAsync(
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
        private async Task<object[]> GeStandardDeviationBreaksAsync(
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            string columnName,
            ColumnStandardDeviationBreaksData breaksData)
        {
            List<object> result = new List<object>();

            string stdDevFunction = breaksData.StandardDeviationFunction == null ? "STDEV" : breaksData.StandardDeviationFunction;
            string selectColumnStatement = await DatasetColumnHelper.GetColumnValuesSelectStatement(
                this.ServiceContext.DbContextFactory,
                dataset,
                columnName,
                usePostProcess,
                datasetPostProcess);

            string commandText =
                $"SELECT {stdDevFunction}({columnName}) AS Dev, AVG({columnName}) as Mean, MIN({columnName}) as MinValue, MAX({columnName}) as MaxValue" +
                $" FROM ({selectColumnStatement}) dv";

            if (breaksData.FilterEmptyValuesExpression != null && string.IsNullOrWhiteSpace(breaksData.FilterEmptyValuesExpression.Script))
            {
                commandText = $"{commandText} WHERE {breaksData.FilterEmptyValuesExpression}";
            }

            double mean = 0;
            double stdDeviation = 0;
            double min = 0;
            double max = 0;

            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                commandText,
                async (DbCommand command, DbDataReader dataReader) =>
                {
                    while (await dataReader.ReadAsync())
                    {
                        mean = System.Convert.ToDouble(dataReader["Mean"]);
                        min = System.Convert.ToDouble(dataReader["MinValue"]);
                        max = System.Convert.ToDouble(dataReader["MaxValue"]);
                        stdDeviation = System.Convert.ToDouble(dataReader["Dev"]);
                    }
                },
                $"Cannot get class break values from column: '{columnName}'.");

            double distanceToLeft = Math.Abs(mean - min);
            double distanceToRight = Math.Abs(max - mean);
            double interval = stdDeviation * breaksData.Interval;

            // Iterations necessary to calculate the breaks.
            int leftIterations = Math.Min(
                interval != 0.0 ? (int)Math.Ceiling(distanceToLeft / interval) : 0,
                GetDatasetColumnBreaksStdDeviationService.MaxIterations);
            int rightIterations = Math.Min(
                interval != 0.0 ? (int)Math.Ceiling(distanceToRight / interval) : 0,
                GetDatasetColumnBreaksStdDeviationService.MaxIterations);

            var results = new List<object>();

            // There could be too many iterations to process, so we put a break on some reasonable limit.
            for (int i = 1; i <= leftIterations; i++)
            {
                double classBreak = mean - (interval * i);
                results.Add(classBreak);
            }

            results.Add(mean);

            for (int i = 1; i <= rightIterations; i++)
            {
                double classBreak = mean + (interval * i);
                results.Add(classBreak);
            }

            return results.ToArray();
        }
    }
}
