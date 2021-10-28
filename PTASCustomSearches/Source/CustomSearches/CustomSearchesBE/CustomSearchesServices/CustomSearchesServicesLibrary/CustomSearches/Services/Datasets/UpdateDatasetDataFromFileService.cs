namespace CustomSearchesServicesLibrary.CustomSearches.Services.Datasets
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that updates a dataset from the contents of a file.  Only editable columns are updated.
    /// </summary>
    public class UpdateDatasetDataFromFileService : BaseService
    {
        /// <summary>
        /// Dictionary with the dataset db columns hashed by name (lowercase).
        /// </summary>
        private Dictionary<string, DbColumn> datasetDbColumns = new Dictionary<string, DbColumn>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDatasetDataFromFileService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UpdateDatasetDataFromFileService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Queues a job that updates the dataset from a file.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="data">The data to upload.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Dataset is used by a worker job.</exception>
        public async Task<IdResult> QueueUpdateDatasetDataFromFileAsync(
            Guid datasetId,
            Stream data,
            DatasetFileImportExportType fileType,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await
                (from d in dbContext.Dataset where d.DatasetId == datasetId select d).
                Include(d => d.ParentFolder).
                Include(d => d.CustomSearchDefinition).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);
            InputValidationHelper.AssertDatasetDataNotLocked(dataset);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "QueueUpdateDatasetDataFromFile");
            this.ServiceContext.AuthProvider.AuthorizeAnyRole(
                dataset.CustomSearchDefinition.DatasetEditRoles,
                $"Current user doesn't have permissions to update the dataset data from file of a custom search with id '{dataset.CustomSearchDefinitionId}'. Required roles: '{dataset.CustomSearchDefinition.DatasetEditRoles}'.");

            await DatasetHelper.TestAlterDatasetLockAsync(
                dataset.DatasetId,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                isRootLock: false,
                userId,
                lockingJobId: null,
                dbContext);

            var payload = new DatasetFileImportExportPayloadData();

            payload.DatasetId = datasetId;
            payload.FileType = fileType;

            switch (fileType)
            {
                case DatasetFileImportExportType.CSV:
                    using (var reader = new StreamReader(data))
                    {
                        payload.DatasetData = await reader.ReadToEndAsync();
                    }

                    break;

                case DatasetFileImportExportType.XLSX:
                    byte[] bytes = new byte[data.Length];
                    await data.ReadAsync(bytes);
                    payload.DatasetData = System.Convert.ToBase64String(bytes);
                    break;
            }

            return new IdResult(await this.ServiceContext.AddWorkerJobQueueAsync(
                "DatasetBackendUpdate",
                "UpdateDatasetDataFromFileJobType",
                userId,
                payload,
                WorkerJobTimeouts.UpdateDatasetDataFromFileTimeout));
        }

        /// <summary>
        /// Updates the dataset data by using the file contents.
        /// </summary>
        /// <param name="payload">The dataset payload.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset with id '{datasetId}' not found. - null</exception>
        public async Task UpdateDatasetDataFromFileAsync(
            DatasetFileImportExportPayloadData payload,
            CustomSearchesDbContext dbContext,
            int jobId,
            Action<string> logAction)
        {
            Dataset dataset = await
                (from d in dbContext.Dataset where d.DatasetId == payload.DatasetId select d).
                Include(d => d.ParentFolder).
                Include(d => d.CustomSearchDefinition).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", payload.DatasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "UpdateDatasetDataFromFile");

            string datasetView = DatasetHelper.GetDatasetView(dataset, false, datasetPostProcess: null);
            ReadOnlyCollection<DbColumn> viewColumns =
                await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

            this.datasetDbColumns = (from vc in viewColumns select vc).ToDictionary(vc => vc.ColumnName.ToLower(), vc => vc);

            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (string datasetState, string datasetPostProcessState) =>
                {
                    switch (payload.FileType)
                    {
                        case DatasetFileImportExportType.CSV:
                            await this.ProcessCsvAsync(dataset, payload, dbContext, logAction);
                            break;

                        case DatasetFileImportExportType.XLSX:
                            await this.ProcessXlsxAsync(dataset, payload, dbContext, logAction);
                            break;
                    }

                    return (datasetState, datasetPostProcessState);
                },
                dataset,
                isRootLock: false,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                userId,
                lockingJobId: jobId,
                dbContext,
                this.ServiceContext);
        }

        /// <summary>
        /// Processes the CSV file.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="payloadData">The payload data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private async Task ProcessCsvAsync(
            Dataset dataset,
            DatasetFileImportExportPayloadData payloadData,
            CustomSearchesDbContext dbContext,
            Action<string> logAction)
        {
            int i = 0;
            string[] headers = null;
            var editableFieldIndices = new List<int>();
            int[] keyIndices = null;

            var updates = new List<JObject>();

            var updateService = new UpdateDatasetDataService(this.ServiceContext);
            bool noChanges = false;

            await CsvReader.ProcessCsvAsync(
                async (string[] headers, int[] indexKeys) =>
                {
                    // Calculate editable fields.
                    var editableColumnsDictionary =
                        (from ec in await DatasetColumnHelper.GetEditableColumnsAsync(dataset, dbContext)
                         select ec).ToDictionary(ec => ec.ColumnName, ec => ec);

                    if (editableColumnsDictionary.Values.Count == 0)
                    {
                        return false;
                    }

                    int fieldIndex = 0;
                    foreach (var header in headers)
                    {
                        var editableColumn = editableColumnsDictionary.GetValueOrDefault(header);
                        if (editableColumn != null)
                        {
                            editableFieldIndices.Add(fieldIndex);
                        }

                        fieldIndex++;
                    }

                    if (editableFieldIndices.Count == 0)
                    {
                        return false;
                    }

                    return true;
                },
                async (string[] fields) =>
                {
                    // Updates the dataset using the read values.
                    await this.GatherUpdatesAsync(
                        dataset,
                        i,
                        headers,
                        editableFieldIndices,
                        keyIndices,
                        fields,
                        updates,
                        dbContext,
                        updateService,
                        logAction);
                },
                payloadData.DatasetData);

            if (!noChanges)
            {
                await this.ApplyUpdatesAsync(dataset, updates, dbContext, updateService, logAction);
            }
            else
            {
                logAction.Invoke("No changes to process.");
            }
        }

        /// <summary>
        /// Processes the CSV file.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="payloadData">The payload data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private async Task ProcessXlsxAsync(
            Dataset dataset,
            DatasetFileImportExportPayloadData payloadData,
            CustomSearchesDbContext dbContext,
            Action<string> logAction)
        {
            byte[] bytes = System.Convert.FromBase64String(payloadData.DatasetData);
            using (Stream stream = new MemoryStream(bytes))
            {
                XLWorkbook workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                {
                    throw new ArgumentOutOfRangeException("payloadData", "The excel in the payload doesn't contain any worksheet to import.");
                }

                bool noChanges = false;
                var updates = new List<JObject>();
                var updateService = new UpdateDatasetDataService(this.ServiceContext);

                string[] headers = null;
                string[] fields = null;
                var editableFieldIndices = new List<int>();
                int customSearchResultIdIndex = -1;
                int majorFieldIndex = -1;
                int minorFieldIndex = -1;

                int[] keyIndices = null;

                foreach (var row in worksheet.Rows())
                {
                    if (row.RowNumber() == 1)
                    {
                        var headersList = new List<string>();

                        // Store headers
                        foreach (var cell in row.Cells())
                        {
                            headersList.Add(cell.Value.ToString());
                        }

                        headers = headersList.ToArray();

                        // Calculate editable fields.
                        var editableColumnsDictionary =
                            (from ec in await DatasetColumnHelper.GetEditableColumnsAsync(dataset, dbContext)
                                select ec).ToDictionary(ec => ec.ColumnName, ec => ec);

                        if (editableColumnsDictionary.Values.Count == 0)
                        {
                            noChanges = true;
                        }

                        if (!noChanges)
                        {
                            int fieldIndex = 0;
                            foreach (var header in headers)
                            {
                                var editableColumn = editableColumnsDictionary.GetValueOrDefault(header);
                                if (editableColumn != null)
                                {
                                    editableFieldIndices.Add(fieldIndex);
                                }

                                if (header.ToLower() == "customsearchresultid")
                                {
                                    customSearchResultIdIndex = fieldIndex;
                                }
                                else if (header.ToLower() == "major")
                                {
                                    majorFieldIndex = fieldIndex;
                                }
                                else if (header.ToLower() == "minor")
                                {
                                    minorFieldIndex = fieldIndex;
                                }

                                fieldIndex++;
                            }

                            if ((majorFieldIndex != -1) && (minorFieldIndex != -1))
                            {
                                keyIndices = new int[] { majorFieldIndex, minorFieldIndex };
                            }
                            else if (customSearchResultIdIndex != -1)
                            {
                                keyIndices = new int[] { customSearchResultIdIndex };
                            }

                            if (editableFieldIndices.Count == 0 || keyIndices == null)
                            {
                                noChanges = true;
                            }
                        }
                    }
                    else
                    {
                        var fieldList = new List<string>();

                        // Store values
                        for (int i = 0; i < headers.Length; i++)
                        {
                            // Column 0 is RowNum.
                            DbColumn dbColumn = null;
                            this.datasetDbColumns.TryGetValue(headers[i].ToLower(), out dbColumn);
                            bool isStringDbType = dbColumn?.DataType == typeof(string);
                            var cellValue = row.Cell(i + 1).GetValue<string>();

                            if (!isStringDbType)
                            {
                                cellValue = DatasetColumnHelper.ColumnValueToString(cellValue, true);
                            }

                            fieldList.Add(cellValue);
                        }

                        fields = fieldList.ToArray();

                        // Updates the dataset using the read values.
                        await this.GatherUpdatesAsync(
                            dataset,
                            row.RowNumber(),
                            headers,
                            editableFieldIndices,
                            keyIndices,
                            fields,
                            updates,
                            dbContext,
                            updateService,
                            logAction);
                    }
                }

                if (!noChanges)
                {
                    await this.ApplyUpdatesAsync(dataset, updates, dbContext, updateService, logAction);
                }
                else
                {
                    logAction.Invoke("No changes to process.");
                }
            }
        }

        /// <summary>
        /// Update a row in the backend.  Updates are accumulated until a batch of sufficient size can be created for the update service.
        /// </summary>
        /// <param name="dataset">The dataset to update.</param>
        /// <param name="rowNumber">The row number.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="editableFieldIndices">The editable fields.</param>
        /// <param name="keyIndices">Indices for the key fields (either CustomSearchResultId or Major/Minor).</param>
        /// <param name="fields">The fields.</param>
        /// <param name="updates">The updates.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="updateService">The update service.</param>
        /// <param name="logAction">The log action.</param>
        private async Task GatherUpdatesAsync(
            Dataset dataset,
            int rowNumber,
            string[] headers,
            List<int> editableFieldIndices,
            int[] keyIndices,
            string[] fields,
            List<JObject> updates,
            CustomSearchesDbContext dbContext,
            UpdateDatasetDataService updateService,
            Action<string> logAction)
        {
            JObject update = new JObject();
            foreach (var fieldIndex in editableFieldIndices)
            {
                string fieldValue = fields[fieldIndex];

                // Update system does not accept nulls.  Nulls are ignored.
                if (fieldValue != null && fieldValue.ToLower() != "system.dbnull")
                {
                    update[headers[fieldIndex]] = fieldValue;
                }
            }

            try
            {
                if (keyIndices.Length == 1)
                {
                    string key = fields[keyIndices[0]];
                    if (string.IsNullOrEmpty(key))
                    {
                        // Skip if no key.
                        return;
                    }

                    update["CustomSearchResultId"] = System.Convert.ToInt32(fields[keyIndices[0]]);
                }
                else if (keyIndices.Length == 2)
                {
                    string key0 = fields[keyIndices[0]];
                    string key1 = fields[keyIndices[1]];
                    if (string.IsNullOrEmpty(key0) || string.IsNullOrEmpty(key1))
                    {
                        // Skip if no key.
                        return;
                    }

                    update["Major"] = System.Convert.ToInt32(fields[keyIndices[0]]);
                    update["Minor"] = System.Convert.ToInt32(fields[keyIndices[1]]);
                }
                else
                {
                    // Shouldn't arrive here.
                    logAction.Invoke("GatherUpdatesAsync found a wrong number of keys.  Skipping row from update.");
                    return;
                }
            }
            catch (System.FormatException)
            {
                // Skip when key is not recognizable.
                logAction.Invoke($"GatherUpdatesAsync found a corrupt key in for row {rowNumber}.  Skipping row from update.");
                return;
            }

            updates.Add(update);

            if (updates.Count == UpdateDatasetDataService.UpdateBatchSize)
            {
                await this.ApplyUpdatesAsync(dataset, updates, dbContext, updateService, logAction);
            }
        }

        /// <summary>
        /// Applies the updates asynchronous.
        /// </summary>
        /// <param name="dataset">The dataset to update.</param>
        /// <param name="updates">The updates.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="updateService">The update service.</param>
        /// <returns>The task.</returns>
        private async Task ApplyUpdatesAsync(
            Dataset dataset,
            List<JObject> updates,
            CustomSearchesDbContext dbContext,
            UpdateDatasetDataService updateService,
            Action<string> logAction)
        {
            if (updates != null && updates.Count > 0)
            {
                logAction.Invoke($"Starting update for {updates.Count} rows...");
                await updateService.UpdateDatasetDataNoLockAsync(dataset, updates.ToArray(), false, clientId: null, dbContext);
                logAction.Invoke($"Successfully updated {updates.Count} rows.");
                updates.Clear();
            }
        }
    }
}