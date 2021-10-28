namespace CustomSearchesServicesLibrary.CustomSearches.Executor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Executor for custom modeling step post process.
    /// </summary>
    public class CustomModelingStepPostProcessExecutor : DatasetPostProcessExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomModelingStepPostProcessExecutor"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previousViewScript.</param>
        /// <param name="payload">The dataset generation payload data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="serviceContext">The service context.</param>
        public CustomModelingStepPostProcessExecutor(
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            string previousViewScript,
            DatasetPostProcessExecutionPayloadData payload,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext)
            : base(dataset, datasetPostProcess, previousViewScript, singleRowExecutionData: null,  dbContext, serviceContext)
        {
            this.Payload = payload;
        }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        public DatasetPostProcessExecutionPayloadData Payload { get; set; }

        /// <summary>
        /// Deletes the previous state.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task DeletePreviousStateAsync()
        {
            if (this.Payload.AdditionalData == null)
            {
                return;
            }

            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            try
            {
                await DbTransientRetryPolicy.DropTableAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    tableName);
            }
            catch (SqlException ex)
            {
                throw new CustomSearchesDatabaseException(string.Format("Cannot drop dataset table: '{0}'.", tableName), ex);
            }
        }

        /// <summary>
        /// Commits the new state.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task CommitNewStateAsync()
        {
            if (this.Payload.AdditionalData == null)
            {
                return;
            }

            var calculatedColumnNames = this.DatasetPostProcess
                .CustomSearchExpression.Where(
                    cse => cse.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower() &&
                    cse.ExpressionType.Trim().ToLower() == CustomSearchExpressionType.Imported.ToString().ToLower())
                .OrderBy(cse => cse.ExecutionOrder)
                .Select(cse => cse.ColumnName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            calculatedColumnNames.Add("CustomSearchResultId");

            IOrderedEnumerable<KeyValuePair<string, int>> orderedHeaders = null;
            List<string[]> rows = new List<string[]>();

            byte[] bytes = System.Convert.FromBase64String(this.Payload.AdditionalData);
            using (Stream stream = new MemoryStream(bytes))
            {
                CloudBlobContainer blobContainer = await this.ServiceContext.CloudStorageProvider.GetCloudBlobContainer(
                    GetDatasetFileService.BlobResultsFolderName,
                    this.ServiceContext.AppCredential);

                CloudBlockBlob blockBlob =
                    blobContainer.GetBlockBlobReference($"{this.Dataset.DatasetId}/{this.DatasetPostProcess.DatasetPostProcessId}/AdditionalData.xlsx");

                await blockBlob.UploadFromStreamAsync(stream);

                string worksheetName = ExportDatasetDataToFileService.GetDatasetWorksheetName(this.Dataset);

                XLWorkbook workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Name == worksheetName);

                if (worksheet == null)
                {
                    throw new ArgumentOutOfRangeException(
                        "payloadData",
                        $"The excel in the payload doesn't contain a worksheet named '{worksheetName}' to import.");
                }

                Dictionary<string, int> headers = new Dictionary<string, int>();

                foreach (var row in worksheet.Rows())
                {
                    if (row.RowNumber() == 1)
                    {
                        // Store headers
                        var cellHeaders = row.Cells().ToArray();
                        for (int i = 0; i < cellHeaders.Length; i++)
                        {
                            string header = cellHeaders[i].Value.ToString();
                            var columnName = calculatedColumnNames.FirstOrDefault(ecn => ecn.Equals(header, StringComparison.OrdinalIgnoreCase));
                            if (columnName != null)
                            {
                                if (!headers.ContainsKey(columnName))
                                {
                                    headers[columnName] = i + 1;
                                }
                            }
                        }

                        orderedHeaders = headers.OrderBy(kvp => kvp.Value);
                    }
                    else
                    {
                        var currentFields = new List<string>();

                        foreach (var header in orderedHeaders)
                        {
                            var cellValue = row.Cell(header.Value).GetValue<string>();
                            cellValue = DatasetHelper.ColumnValueToString(cellValue, true);
                            currentFields.Add(cellValue);
                        }

                        rows.Add(currentFields.ToArray());
                    }
                }
            }

            string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.DatasetPostProcess.Dataset, usePostProcess: true);

            string tableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.DatasetPostProcess.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableName(this.DatasetPostProcess.Dataset, this.DatasetPostProcess.DatasetPostProcessId);

            string createTableScript = $"CREATE TABLE {tableFullName}(\n" +
                "[CustomSearchResultId] int NOT NULL PRIMARY KEY,\n";

            foreach (var header in orderedHeaders)
            {
                var columnName = header.Key;

                if (columnName == "CustomSearchResultId")
                {
                    continue;
                }

                string databaseType = "float";

                createTableScript += $"[{columnName}] {databaseType} NULL,\n";

                if (databaseType == "float")
                {
                    createTableScript += $"INDEX [IX_{tableName}_{columnName}] NONCLUSTERED ({columnName}),\n";
                }
            }

            createTableScript = createTableScript.TrimEnd(new char[] { ',', '\n' });
            createTableScript += ")";

            await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                createTableScript,
                parameters: null);

            string batchScript = string.Empty;

            while (rows.Count > 0)
            {
                int batchSize = Math.Min(rows.Count, UpdateDatasetDataService.UpdateBatchSize);
                var rowsBatch = rows.GetRange(0, batchSize);
                foreach (var row in rowsBatch)
                {
                    batchScript += $"INSERT INTO {tableFullName} " + $"VALUES ({string.Join(", ", row)})\n";
                }

                await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    batchScript,
                    parameters: null);

                batchScript = string.Empty;
                rows.RemoveRange(0, batchSize);
            }

            this.DatasetPostProcess.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            this.DatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
            this.DbContext.DatasetPostProcess.Update(this.DatasetPostProcess);
            await this.DbContext.ValidateAndSaveChangesAsync();
        }

        /// <summary>
        /// Calculates the view.
        /// </summary>
        /// <param name="postProcessViewPhase">The post process view phase.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task CalculateViewAsync(PostProcessViewPhase postProcessViewPhase)
        {
            if (postProcessViewPhase == PostProcessViewPhase.PostCommit)
            {
                if (this.Payload.AdditionalData == null)
                {
                    string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
                    bool foundTable = await DbTransientRetryPolicy.ExistCustomSearchTableAsync(this.ServiceContext, tableName);
                    if (!foundTable)
                    {
                        return;
                    }

                    string tableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
                    var tableDbColumns = await DbTransientRetryPolicy.GetTableColumnSchemaAsync(this.ServiceContext, tableFullName);

                    var calculatedColumnNames = this.DatasetPostProcess
                        .CustomSearchExpression.Where(c => c.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower() &&
                            c.ExpressionType.Trim().ToLower() == CustomSearchExpressionType.Imported.ToString().ToLower())
                        .Select(c => c.ColumnName.Trim().ToLower())
                        .ToHashSet();

                    if (!calculatedColumnNames.IsSubsetOf(tableDbColumns.Select(c => c.ColumnName.Trim().ToLower())))
                    {
                        return;
                    }
                }

                await this.CalculateViewWithPostProcessTableAsync(CustomSearchExpressionType.Imported);
            }

            bool usePostProcess = !string.IsNullOrWhiteSpace(this.ViewScript);
            string datasetView = DatasetHelper.GetDatasetView(
                this.Dataset,
                usePostProcess,
                datasetPostProcess: postProcessViewPhase == PostProcessViewPhase.PostCommit ? this.DatasetPostProcess : null);

            var dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

            int datasetPostProcessId = this.DatasetPostProcess.DatasetPostProcessId;
            string expressionRole = postProcessViewPhase == PostProcessViewPhase.PostCommit ?
                "CalculatedColumnPostCommit".ToLower() : "CalculatedColumnPreCommit".ToLower();

            var customSearchExpressions = (await this.DbContext.DatasetPostProcess
                .Where(d => d.DatasetPostProcessId == datasetPostProcessId)
                .Include(d => d.CustomSearchExpression)
                .FirstOrDefaultAsync())
                .CustomSearchExpression.Where(c => c.ExpressionRole.ToLower() == expressionRole &&
                    c.ExpressionType == CustomSearchExpressionType.TSQL.ToString())
                .OrderBy(c => c.ExecutionOrder).ToArray();

            if (customSearchExpressions.Length > 0)
            {
                Dictionary<string, string> replacementDictionary = new Dictionary<string, string>();

                string columnScript = string.Empty;
                Dictionary<string, string> expressionsToAdd = new Dictionary<string, string>();
                foreach (var customSearchExpression in customSearchExpressions)
                {
                    string customSearchExpressionScript = customSearchExpression.Script;
                    customSearchExpressionScript =
                        await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                            customSearchExpressionScript,
                            replacementDictionary,
                            customSearchExpression.ColumnName,
                            this.ServiceContext,
                            this.Dataset);

                    expressionsToAdd.Add(customSearchExpression.ColumnName, customSearchExpressionScript);
                }

                foreach (DbColumn column in dbColumns)
                {
                    string colName = column.ColumnName;
                    if (expressionsToAdd.ContainsKey(colName))
                    {
                        columnScript += $"({expressionsToAdd[colName]}) AS [{colName}]";
                        expressionsToAdd.Remove(colName);
                    }
                    else
                    {
                        columnScript += $"[{colName}]";
                    }

                    columnScript += ", ";
                }

                foreach (var expression in expressionsToAdd)
                {
                    columnScript += "(" + expression.Value + ") AS [" + expression.Key + "], ";
                }

                columnScript = columnScript.TrimEnd(new char[] { ',', ' ' });

                string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.Dataset, !string.IsNullOrWhiteSpace(this.ViewScript) /*usePostProcess*/);
                string previousViewScript = string.IsNullOrWhiteSpace(this.ViewScript) ? ("SELECT * FROM " + viewName) : this.ViewScript;

                string script = "SELECT " + columnScript + "\n";
                script += "FROM (" + previousViewScript + ") a\n";
                this.ViewScript = script;
            }
        }
    }
}
