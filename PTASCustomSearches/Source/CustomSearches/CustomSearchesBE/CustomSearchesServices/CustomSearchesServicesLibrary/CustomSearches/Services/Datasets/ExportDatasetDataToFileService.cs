namespace CustomSearchesServicesLibrary.CustomSearches.Services.Datasets
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Misc;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service that exports a dataset to a file.
    /// </summary>
    public class ExportDatasetDataToFileService : BaseService
    {
        /// <summary>
        /// The default data CSV file name.
        /// </summary>
        public const string CsvSeparator = ",";

        /// <summary>
        /// The batch size used to read elements from the dataset.
        /// </summary>
        private const int ReadBatchSize = 30000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDatasetDataToFileService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ExportDatasetDataToFileService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the dataset work sheet name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The dataset work sheet name.</returns>
        public static string GetDatasetWorksheetName(Dataset dataset)
        {
            // Worksheet names cannot contain any of the following characters: :\/?*[]
            // Worksheet names cannot be more than 31 characters
            return $"{dataset.DatasetId.ToString().Substring(0, 8)}_{dataset.DatasetName}".Replace(":", "_").Remove(31);
        }

        /// <summary>
        /// Exports the dataset to a CSV File.
        /// </summary>
        /// <param name="datasetId">The dataset post process id.</param>
        /// <param name="fileName">The file name as an output parameter.  Created from the dataset.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="usePostProcess">if set to <c>true</c> exports data with post-processes applied.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="includeSecondaryDatasets">Value indicating whether the export should include the secondary datasets.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The bytes of the exported file.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        public async Task<byte[]> ExportDatasetDataToFileAsync(
            Guid datasetId,
            Ref<string> fileName,
            DatasetFileImportExportType fileType,
            CustomSearchesDbContext dbContext,
            bool usePostProcess,
            int? postProcessId,
            bool includeSecondaryDatasets,
            ILogger log)
        {
            Dataset dataset = await (from d in dbContext.Dataset where d.DatasetId == datasetId select d).FirstOrDefaultAsync();
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            DatasetPostProcess datasetPostProcess = null;
            if (usePostProcess && postProcessId != null)
            {
                var datasetPostProcessQuery =
                    dbContext.DatasetPostProcess
                    .Where(dpp => dpp.DatasetId == datasetId && dpp.DatasetPostProcessId == (int)postProcessId);

                if (includeSecondaryDatasets)
                {
                    datasetPostProcessQuery =
                        datasetPostProcessQuery
                        .Include(dpp => dpp.InversePrimaryDatasetPostProcess)
                            .ThenInclude(dpp => dpp.Dataset);
                }

                datasetPostProcess = await datasetPostProcessQuery.FirstOrDefaultAsync();
            }

            var canUsePostProcess = await DatasetHelper.CanUsePostProcessAsync(dataset, datasetPostProcess, usePostProcess, dbContext);

            switch (fileType)
            {
                case DatasetFileImportExportType.CSV:

                    fileName.Value = $"{dataset.DatasetName}.csv";
                    using (var stream = new MemoryStream())
                    using (var writer = new StreamWriter(stream))
                    {
                        var processCsvAction = new Func<GetUserCustomSearchDataResponse, int, ReadOnlyCollection<DbColumn>, Task>(
                            async (GetUserCustomSearchDataResponse datasetResponse, int i, ReadOnlyCollection<DbColumn> columns) =>
                            {
                                await this.WriteCsvToStreamAsync(datasetResponse, i, writer);
                            });

                        await this.ProcessDatasetAsync(
                            dataset,
                            processCsvAction,
                            canUsePostProcess,
                            datasetPostProcess,
                            (string message) => { log.LogInformation(message); });

                        writer.Flush();
                        stream.Position = 0;
                        byte[] b = stream.ToArray();
                        return b;
                    }

                case DatasetFileImportExportType.XLSX:

                    fileName.Value = $"{dataset.DatasetName}.xlsx";
                    var workbook = new XLWorkbook();
                    List<Dataset> datasets = new List<Dataset>() { dataset };

                    if (usePostProcess && postProcessId != null && includeSecondaryDatasets)
                    {
                        foreach (var secondaryPostProcess in datasetPostProcess.InversePrimaryDatasetPostProcess)
                        {
                            datasets.Add(secondaryPostProcess.Dataset);
                        }
                    }

                    foreach (var currentDataset in datasets)
                    {
                        string worksheetName = ExportDatasetDataToFileService.GetDatasetWorksheetName(currentDataset);
                        IXLWorksheet worksheet = workbook.Worksheets.Add(worksheetName);

                        var processAction = new Func<GetUserCustomSearchDataResponse, int, ReadOnlyCollection<DbColumn>, Task>(
                        async (GetUserCustomSearchDataResponse datasetResponse, int i, ReadOnlyCollection<DbColumn> columns) =>
                        {
                            await this.WriteXlsxRowsAsync(datasetResponse, i, worksheet, columns);
                        });

                        await this.ProcessDatasetAsync(
                            currentDataset,
                            processAction,
                            canUsePostProcess,
                            currentDataset.DatasetPostProcess.FirstOrDefault(),
                            (string message) => { log.LogInformation(message); });

                        worksheet.Columns().AdjustToContents();
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
            }

            return null;
        }

        /// <summary>
        /// Reads the dataset data, calling a function for each piece of data.
        /// </summary>
        /// <param name="dataset">The dataset to process.</param>
        /// <param name="processBatchResponse">Action that processes the response for a batch of rows.</param>
        /// <param name="usePostProcess">if set to <c>true</c> processes data with post-processes applied.</param>
        /// <param name="postProcess">The post process.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>The async task.</returns>
        public async Task ProcessDatasetAsync(
            Dataset dataset,
            Func<GetUserCustomSearchDataResponse, int, ReadOnlyCollection<DbColumn>, Task> processBatchResponse,
            bool usePostProcess,
            DatasetPostProcess postProcess,
            Action<string> logAction)
        {
            string datasetView = DatasetHelper.GetDatasetView(dataset, usePostProcess, postProcess);
            ReadOnlyCollection<DbColumn> datasetContextDbColumns =
                await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

            bool finished = false;
            int i = 0;

            while (!finished)
            {
                logAction.Invoke($"Reading dataset batch from SQL...");

                var response = await GetDatasetDataService.GetDatasetDataAsync(
                    dataset,
                    i * ExportDatasetDataToFileService.ReadBatchSize,
                    ExportDatasetDataToFileService.ReadBatchSize,
                    usePostProcess,
                    postProcess,
                    this.ServiceContext);

                logAction($"Batch retrieved from SQL...");

                if (response.Results.Length > 0)
                {
                    await processBatchResponse.Invoke(response, i, datasetContextDbColumns);
                }

                logAction.Invoke($"Processed {(i * ExportDatasetDataToFileService.ReadBatchSize) + response.Results.Length} results from CSV.");
                finished = response.Results.Length < ExportDatasetDataToFileService.ReadBatchSize;
                i++;
            }
        }

        /// <summary>
        /// Writes the CSV to stream asynchronous.
        /// </summary>
        /// <param name="results">The results to write.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <param name="writer">The writer.</param>
        /// <returns>The task.</returns>
        public async Task WriteCsvToStreamAsync(GetUserCustomSearchDataResponse results, int batchNumber, StreamWriter writer)
        {
            // Write columns
            if (batchNumber == 0)
            {
                IDictionary<string, object> firstResult = results.Results[0] as IDictionary<string, object>;
                StringBuilder columnsStringBuilder = new StringBuilder();
                int j = 0;
                foreach (var key in firstResult.Keys)
                {
                    columnsStringBuilder.Append(key);
                    if (j < (firstResult.Keys.Count - 1))
                    {
                        columnsStringBuilder.Append(ExportDatasetDataToFileService.CsvSeparator);
                    }

                    j++;
                }

                string columnsString = columnsStringBuilder.ToString();
                await writer.WriteLineAsync(columnsString);
            }

            // Write  values
            foreach (var rowValue in results.Results)
            {
                IDictionary<string, object> firstResult = rowValue as IDictionary<string, object>;
                StringBuilder valuesStringBuilder = new StringBuilder();
                int j = 0;
                foreach (var columnValue in firstResult.Values)
                {
                    string stringColumnValue = DatasetColumnHelper.ColumnValueToString(columnValue, columnValue.GetType() != typeof(string));
                    if (stringColumnValue.Contains(ExportDatasetDataToFileService.CsvSeparator) || stringColumnValue.Contains("\""))
                    {
                        var escapedQuotes = stringColumnValue.Replace("\"", "\"\"");
                        stringColumnValue = $"\"{escapedQuotes}\"";
                    }

                    stringColumnValue = stringColumnValue.Replace("\n", string.Empty).Replace("\r", string.Empty);

                    valuesStringBuilder.Append(stringColumnValue);

                    if (j < (firstResult.Values.Count - 1))
                    {
                        valuesStringBuilder.Append(ExportDatasetDataToFileService.CsvSeparator);
                    }

                    j++;
                }

                string valuesString = valuesStringBuilder.ToString();
                await writer.WriteLineAsync(valuesString);
            }
        }

        /// <summary>
        /// Writes the response to the Xlsx rows.
        /// </summary>
        /// <param name="results">The results to write.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <param name="worksheet">The worksheet where the values will be inserted.</param>
        /// <param name="dbColumns">The db columns.</param>
        private async Task WriteXlsxRowsAsync(
            GetUserCustomSearchDataResponse results,
            int batchNumber,
            IXLWorksheet worksheet,
            ReadOnlyCollection<DbColumn> dbColumns)
        {
            // Write headers
            if (batchNumber == 0)
            {
                IDictionary<string, object> firstResult = results.Results[0] as IDictionary<string, object>;

                int columnIndex = 1;
                foreach (var key in firstResult.Keys)
                {
                    if (key.ToLower() == "rownum")
                    {
                        continue;
                    }

                    worksheet.Cell(1, columnIndex).SetValue(key);
                    columnIndex++;
                }
            }

            int rowIndex = worksheet.RowsUsed().Count() + 1;

            // Write  values
            foreach (var rowValue in results.Results)
            {
                int columnIndex = 0;

                IDictionary<string, object> firstResult = rowValue as IDictionary<string, object>;

                foreach (var column in firstResult)
                {
                    // RowNum is not present in dbcolumns, so we skip that index.
                    if (column.Key.ToLower() == "rownum")
                    {
                        continue;
                    }

                    DbColumn dbColumn = dbColumns[columnIndex];

                    var sanitizedValue = column.Value;
                    int format = -1; // Don't format.
                    bool isDbColumnNumeric = TypeManagementHelper.IsNumericType(dbColumn.DataType);

                    if (column.Value.GetType() == typeof(System.DBNull))
                    {
                        sanitizedValue = null;
                    }

                    if (!isDbColumnNumeric)
                    {
                        if (dbColumn.DataType == typeof(string))
                        {
                            format = (int)XLPredefinedFormat.Number.Text;
                        }

                        sanitizedValue = sanitizedValue?.ToString();
                    }
                    else
                    {
                        if (TypeManagementHelper.IsDecimalType(dbColumn.DataType))
                        {
                            if (sanitizedValue != null)
                            {
                                sanitizedValue = Convert.ToDecimal(sanitizedValue);
                            }

                            format = (int)XLPredefinedFormat.Number.Precision2;
                        }
                        else
                        {
                            if (sanitizedValue != null)
                            {
                                sanitizedValue = Convert.ToInt64(sanitizedValue);
                            }

                            format = (int)XLPredefinedFormat.Number.Integer;
                        }
                    }

                    worksheet.Cell(rowIndex, columnIndex + 1).SetValue(sanitizedValue);

                    if (format != -1)
                    {
                        worksheet.Cell(rowIndex, columnIndex + 1).Style.NumberFormat.SetNumberFormatId(format);
                    }

                    columnIndex++;
                }

                rowIndex++;
            }
        }
    }
}
