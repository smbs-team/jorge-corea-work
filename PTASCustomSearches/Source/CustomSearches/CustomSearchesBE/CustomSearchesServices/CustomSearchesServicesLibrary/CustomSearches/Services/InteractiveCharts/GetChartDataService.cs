namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    /// <summary>
    /// Service that gets the interactive chart data.
    /// </summary>
    public class GetChartDataService : BaseService
    {
        /// <summary>
        /// Gets the max items count to return.
        /// </summary>
        private const int MaxItemsCount = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChartDataService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetChartDataService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the chart template data.
        /// </summary>
        /// <param name="chartId">The chart id.</param>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="continuationToken">The continuation token.</param>
        /// <param name="isPlot">Value indicating whether return the plot part of the chart. Only applies for ScatterPlot.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The chart template data.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        public async Task<GetChartDataResponse> GetChartTemplateDataAsync(
            int chartId,
            Guid datasetId,
            bool usePostProcess,
            int? postProcessId,
            string continuationToken,
            bool isPlot,
            CustomSearchesDbContext dbContext)
        {
            ChartTemplate chartTemplate = await dbContext.ChartTemplate
                .Where(c => c.ChartTemplateId == chartId)
                .Include(c => c.CustomSearchExpression)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(chartTemplate, nameof(chartTemplate), chartId);

            return await this.GetChartDataAsync(
                chartTemplate.ChartType,
                chartTemplate.ChartTitle,
                chartTemplate.CustomSearchExpression,
                datasetId,
                usePostProcess,
                postProcessId,
                continuationToken,
                isPlot,
                dbContext);
        }

        /// <summary>
        /// Gets the interactive chart data.
        /// </summary>
        /// <param name="chartId">The chart id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="continuationToken">The continuation token.</param>
        /// <param name="isPlot">Value indicating whether return the plot part of the chart. Only applies for ScatterPlot.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The interactive chart data.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        public async Task<GetChartDataResponse> GetInteractiveChartDataAsync(
            int chartId,
            bool usePostProcess,
            int? postProcessId,
            string continuationToken,
            bool isPlot,
            CustomSearchesDbContext dbContext)
        {
            InteractiveChart interactiveChart = await dbContext.InteractiveChart
                .Where(c => c.InteractiveChartId == chartId)
                .Include(c => c.CustomSearchExpression)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(interactiveChart, nameof(interactiveChart), chartId);

            return await this.GetChartDataAsync(
                interactiveChart.ChartType,
                interactiveChart.ChartTitle,
                interactiveChart.CustomSearchExpression,
                interactiveChart.DatasetId,
                usePostProcess,
                postProcessId,
                continuationToken,
                isPlot,
                dbContext);
        }

        /// <summary>
        /// Gets the chart data.
        /// </summary>
        /// <param name="chartType">The chart type.</param>
        /// <param name="chartTitle">The chart title.</param>
        /// <param name="expressions">The chart expressions.</param>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="continuationToken">The continuation token.</param>
        /// <param name="isPlot">Value indicating whether return the plot part of the chart. Only applies for ScatterPlot.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The chart data.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        public async Task<GetChartDataResponse> GetChartDataAsync(
            string chartType,
            string chartTitle,
            ICollection<CustomSearchExpression> expressions,
            Guid datasetId,
            bool usePostProcess,
            int? postProcessId,
            string continuationToken,
            bool isPlot,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            DatasetPostProcess datasetPostProcess = null;
            if (usePostProcess && (postProcessId > 0))
            {
                datasetPostProcess = await dbContext.DatasetPostProcess.FirstOrDefaultAsync(d => d.DatasetPostProcessId == postProcessId && d.DatasetId == dataset.DatasetId);
                InputValidationHelper.AssertEntityExists(datasetPostProcess, "Dataset post process", postProcessId);
            }

            usePostProcess = await DatasetHelper.CanUsePostProcessAsync(dataset, datasetPostProcess, usePostProcess, dbContext);
            DatasetHelper.AssertCanUsePostProcess(datasetPostProcess, usePostProcess);

            DatasetHelper.AssertCanReadFromDataset(dataset, usePostProcess);

            GetChartDataResponse response = null;

            InteractiveChartType interactiveChartType =
                InputValidationHelper.ValidateEnum<InteractiveChartType>(chartType, nameof(chartType));

            int continuationTokenVar = 0;
            int offset = 0;
            if (!string.IsNullOrWhiteSpace(continuationToken))
            {
                string[] continuationTokenArray = continuationToken.Split("_");
                continuationTokenVar = int.Parse(continuationTokenArray[0]);
                offset = int.Parse(continuationTokenArray[1]);
            }

            if (((interactiveChartType == InteractiveChartType.ScatterPlot) && isPlot) ||
                (interactiveChartType == InteractiveChartType.ScatterPlotMatrix) ||
                (interactiveChartType == InteractiveChartType.BoxPlot))
            {
                response = await this.ExecuteScatterChartDataScriptAsync(
                    interactiveChartType,
                    expressions.ToArray(),
                    dataset,
                    usePostProcess,
                    datasetPostProcess,
                    offset);
            }
            else if (interactiveChartType == InteractiveChartType.Histogram)
            {
                response = await this.ExecuteHistogramChartDataScriptAsync(
                    expressions.ToArray(),
                    dataset,
                    usePostProcess,
                    datasetPostProcess);
            }
            else
            {
                response = await this.ExecuteChartDataScriptAsync(
                    interactiveChartType,
                    expressions.ToArray(),
                    dataset,
                    usePostProcess,
                    datasetPostProcess,
                    offset,
                    continuationTokenVar);
            }

            response.ChartTitle = chartTitle;
            response.ChartType = chartType;

            return response;
        }

        /// <summary>
        /// Executes the interactive chart data script.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        /// <returns>The chart data.</returns>
        private async Task<IEnumerable<dynamic>> ExecuteInteractiveChartDataScriptAsync(string script)
        {
            // Create the dynamic result for each row
            var results = new List<dynamic>();
            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                async (command, dataReader) =>
                {
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
                            var result = new ExpandoObject() as IDictionary<string, object>;
                            foreach (var columnName in columnNames)
                            {
                                result.Add(columnName, dataReader[columnName]);
                            }

                            results.Add(result);
                        }
                    }
                },
                "Read failed in the database while getting chart data.");

            return results;
        }

        /// <summary>
        /// Executes the scatter chart data script.
        /// </summary>
        /// <param name="chartType">The chart type.</param>
        /// <param name="expressions">The chart expressions.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The get chart data response.</returns>
        private async Task<GetChartDataResponse> ExecuteScatterChartDataScriptAsync(
            InteractiveChartType chartType,
            ICollection<CustomSearchExpression> expressions,
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            int offset)
        {
            GetChartDataResponse response = new GetChartDataResponse();

            int maxItemsToReturn = GetChartDataService.MaxItemsCount;
            ChartPayloadData chartData = new ChartPayloadData();
            chartData.IndependentVariable = "NA";
            string script = this.GetScatterChartDataScript(
                dataset,
                usePostProcess,
                datasetPostProcess,
                offset,
                maxItemsToReturn,
                chartType,
                expressions.ToArray());

            chartData.Values = (await this.ExecuteInteractiveChartDataScriptAsync(script)).ToArray();

            if (chartData.Values.Length > 0)
            {
                if (chartData.Values.Length == maxItemsToReturn)
                {
                    if ((offset + chartData.Values.Length) < 25000)
                    {
                        response.ContinuationToken = "0_" + (offset + chartData.Values.Length).ToString();
                    }
                }

                response.Results = new ChartPayloadData[] { chartData };
            }

            return response;
        }

        /// <summary>
        /// Executes the histogram chart data script.
        /// </summary>
        /// <param name="expressions">The chart expressions.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>The get chart data response.</returns>
        private async Task<GetChartDataResponse> ExecuteHistogramChartDataScriptAsync(
            ICollection<CustomSearchExpression> expressions,
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess)
        {
            GetChartDataResponse response = new GetChartDataResponse();
            List<ChartPayloadData> chartDataList = new List<ChartPayloadData>();
            CustomSearchExpression[] independentExpressions = expressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.IndependentVariable.ToString()).ToArray();
            CustomSearchExpression[] groupByExpressions = expressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.GroupByVariable.ToString()).ToArray();

            for (int i = 0; i < independentExpressions.Length; i++)
            {
                CustomSearchExpression independentExpression = independentExpressions[i];

                var groupByExpression = groupByExpressions.Where(e => e.ExpressionGroup == independentExpression.ExpressionGroup).FirstOrDefault();

                ChartPayloadData chartData = new ChartPayloadData();
                chartData.IndependentVariable = independentExpression.ColumnName;

                string script = this.GetHistogramChartDataScript(
                    dataset,
                    usePostProcess,
                    datasetPostProcess,
                    independentExpression,
                    groupByExpression);

                var results = (await this.ExecuteInteractiveChartDataScriptAsync(script)).ToList();

                if (results.Count() > 0)
                {
                    // Trimming at start and end of the data of each category where BinObservations is 0.
                    string stringResults = JsonHelper.SerializeObject(results);
                    List<HistogramChartData> histogramChartDataList = JsonHelper.DeserializeObject<List<HistogramChartData>>(stringResults);
                    var validResults = new List<HistogramChartData>();
                    var categories = histogramChartDataList.Select(d => d.BinCategory).ToHashSet();
                    foreach (var category in categories)
                    {
                        var categoryDataList = histogramChartDataList.Where(d => d.BinCategory == category);
                        var firstValidDataInCategory = categoryDataList.FirstOrDefault(d => d.BinObservations > 0);
                        var lastValidDataInCategory = categoryDataList.LastOrDefault(d => d.BinObservations > 0);
                        int firstValidIndex = firstValidDataInCategory == null ? 0 : histogramChartDataList.IndexOf(firstValidDataInCategory);
                        int lastValidIndex = lastValidDataInCategory == null ? 0 : histogramChartDataList.IndexOf(lastValidDataInCategory);
                        int validCount = lastValidIndex - firstValidIndex + 1;
                        validResults.AddRange(histogramChartDataList.GetRange(firstValidIndex, validCount).ToArray());
                    }

                    if (validResults.Count() > 0)
                    {
                        chartData.Values = validResults.ToArray();
                        chartDataList.Add(chartData);
                    }
                }
            }

            response.Results = chartDataList.ToArray();

            return response;
        }

        /// <summary>
        /// Executes the interactive chart data script.
        /// </summary>
        /// <param name="chartType">The chart type.</param>
        /// <param name="expressions">The chart expressions.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="continuationTokenVarIndex">The continuation token variable index.</param>
        /// <returns>The get chart data response.</returns>
        private async Task<GetChartDataResponse> ExecuteChartDataScriptAsync(
            InteractiveChartType chartType,
            ICollection<CustomSearchExpression> expressions,
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            int offset,
            int continuationTokenVarIndex)
        {
            GetChartDataResponse response = new GetChartDataResponse();

            int maxItemsToReturn = GetChartDataService.MaxItemsCount;
            List<ChartPayloadData> chartDataList = new List<ChartPayloadData>();
            CustomSearchExpression[] independentExpressions = expressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.IndependentVariable.ToString()).ToArray();
            CustomSearchExpression[] dependentExpressions = expressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.DependentVariable.ToString()).ToArray();
            CustomSearchExpression[] groupByExpressions = expressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.GroupByVariable.ToString()).ToArray();
            for (int i = continuationTokenVarIndex; i < independentExpressions.Length; i++)
            {
                CustomSearchExpression independentExpression = independentExpressions[i];

                var currentGroupByExpressions = groupByExpressions.Where(e => e.ExpressionGroup == independentExpression.ExpressionGroup).ToArray();

                ChartPayloadData chartData = new ChartPayloadData();
                chartData.IndependentVariable = independentExpression.ColumnName;

                string script = this.GetChartDataScript(
                    dataset,
                    usePostProcess,
                    datasetPostProcess,
                    offset,
                    maxItemsToReturn,
                    chartType,
                    independentExpression,
                    currentGroupByExpressions,
                    dependentExpressions);
                chartData.Values = (await this.ExecuteInteractiveChartDataScriptAsync(script)).ToArray();

                if (chartData.Values.Length > 0)
                {
                    chartDataList.Add(chartData);
                }

                maxItemsToReturn -= chartData.Values.Length;

                if (maxItemsToReturn == 0)
                {
                    response.ContinuationToken = i.ToString() + "_" + (offset + chartData.Values.Length).ToString();
                    break;
                }

                offset = 0;
            }

            response.Results = chartDataList.ToArray();

            return response;
        }

        /// <summary>
        /// Gets the chart data script.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="maxItemsToReturn">The max items to return.</param>
        /// <param name="chartType">The chart type.</param>
        /// <param name="independentExpression">The independent expression.</param>
        /// <param name="groupByExpressions">The group by expressions.</param>
        /// <param name="dependentExpressions">The dependent expressions.</param>
        /// <returns>The chart data script.</returns>
        private string GetChartDataScript(
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            int offset,
            int maxItemsToReturn,
            InteractiveChartType chartType,
            CustomSearchExpression independentExpression,
            CustomSearchExpression[] groupByExpressions,
            CustomSearchExpression[] dependentExpressions)
        {
            string independentColumnName = independentExpression.Script.Split(" ")[0];

            string selectScript = $"SELECT {independentColumnName} AS [{independentExpression.ColumnName}]";
            string groupByScript = $"GROUP BY [{independentColumnName}]";
            string orderByScript = $"ORDER BY {independentExpression.Script}";
            foreach (var groupByExpression in groupByExpressions)
            {
                string groupByColumnName = groupByExpression.Script.Split(" ")[0];
                groupByScript += $", [{groupByColumnName}]";
                orderByScript += $", {groupByExpression.Script}";
                selectScript += $", {groupByColumnName} AS [{groupByExpression.ColumnName}]";
            }

            foreach (var dependentExpression in dependentExpressions)
            {
                // Filter out plotted style dependent expressions for scatter plot.
                if (chartType == InteractiveChartType.ScatterPlot && CustomSearchExpressionValidator.IsPlottedStyleExpression(dependentExpression))
                {
                    continue;
                }

                selectScript += $", {dependentExpression.Script} AS [{dependentExpression.ColumnName}]";
            }

            string datasetView = DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess);

            string commandText =
                $"{selectScript}\n" +
                $"FROM\n" +
                $"{datasetView}\n" +
                $"{groupByScript}\n" +
                $"{orderByScript}\n" +
                $"OFFSET {offset} ROWS FETCH NEXT {maxItemsToReturn} ROWS ONLY\n";

            return commandText;
        }

        /// <summary>
        /// Gets the scatter chart data script.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="maxItemsToReturn">The max items to return.</param>
        /// <param name="chartType">The chart type.</param>
        /// <param name="expressions">The custom search expressions.</param>
        /// <returns>The scatter chart data script.</returns>
        private string GetScatterChartDataScript(
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            int offset,
            int maxItemsToReturn,
            InteractiveChartType chartType,
            CustomSearchExpression[] expressions)
        {
            string expressionsScript = string.Empty;

            foreach (var expression in expressions)
            {
                // Filter out not plotted style dependent expressions for scatter plot.
                if (chartType == InteractiveChartType.ScatterPlot)
                {
                    CustomSearchExpressionRoleType role =
                        InputValidationHelper.ValidateEnum<CustomSearchExpressionRoleType>(expression.ExpressionRole, nameof(expression.ExpressionRole));
                    if (role == CustomSearchExpressionRoleType.DependentVariable && !CustomSearchExpressionValidator.IsPlottedStyleExpression(expression))
                    {
                        continue;
                    }
                }

                expressionsScript += " " + expression.Script.Split(" ")[0] + " AS [" + expression.ColumnName + "],";
            }

            expressionsScript = expressionsScript.TrimEnd(new char[] { ',' });

            string commandText = "SELECT Major, Minor," + expressionsScript +
                " FROM " +
                DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess) +
                " order by Major" +
                " OFFSET " + offset + " ROWS FETCH NEXT " + maxItemsToReturn + " ROWS ONLY";

            return commandText;
        }

        /// <summary>
        /// Gets the histogram chart data script.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="independentExpression">The independent custom search expression.</param>
        /// <param name="groupByExpression">The group by custom search expression.</param>
        /// <returns>The histogram chart data script.</returns>
        private string GetHistogramChartDataScript(
            Dataset dataset,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            CustomSearchExpression independentExpression,
            CustomSearchExpression groupByExpression)
        {
            HistogramChartExtensionData histogramChartExtensionData =
                JsonHelper.DeserializeObject<HistogramChartExtensionData>(independentExpression.ExpressionExtensions);

            bool useCategory = false;
            string categoryColumn = string.Empty;
            if (groupByExpression != null)
            {
                useCategory = true;
                categoryColumn = groupByExpression.Script;
            }

            string commandText =
                $"Exec [cus].[SP_GetHistogram] '{DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess)}', " +
                $"'{independentExpression.Script}', {histogramChartExtensionData.AutoBins}, {histogramChartExtensionData.NumBins}, " +
                $"{histogramChartExtensionData.UseDiscreteBins}, {useCategory}, '{categoryColumn}'";

            return commandText;
        }
    }
}