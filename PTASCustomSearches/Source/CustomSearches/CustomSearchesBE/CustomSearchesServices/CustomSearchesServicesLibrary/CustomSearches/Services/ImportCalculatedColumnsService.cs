namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service that imports the calculated column to the dataset view.
    /// </summary>
    public class ImportCalculatedColumnsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCalculatedColumnsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportCalculatedColumnsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the calculated columns to the dataset view.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="importCalculatedColumnsData">The calculated columns data to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter dataset or column already exists in table.</exception>
        public async Task ImportCalculatedColumnsAsync(Guid datasetId, ImportCalculatedColumnsData importCalculatedColumnsData, CustomSearchesDbContext dbContext, ILogger logger)
        {
            Dataset dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                    dbContext,
                    datasetId,
                    includeRelatedExpressions: true,
                    includeParentFolder: true,
                    includeInverseSourceDatasets: true,
                    includeUserProject: false,
                    includeDatasetUserClientState: false);

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            // Should be applied over source dataset.
            if (dataset.SourceDatasetId != null)
            {
                dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                    dbContext,
                    (Guid)dataset.SourceDatasetId,
                    includeRelatedExpressions: true,
                    includeParentFolder: true,
                    includeInverseSourceDatasets: true,
                    includeUserProject: false,
                    includeDatasetUserClientState: false);
            }

            InputValidationHelper.AssertDatasetDataNotLocked(dataset);
            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "ImportCalculatedColumns");

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    List<CustomSearchExpression> expressionsToAdd = new List<CustomSearchExpression>();
                    List<CustomSearchExpression> expressionsToRemove =
                    dataset.CustomSearchExpression
                        .Where(
                            e =>
                                (e.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()) &&
                                (e.ExpressionType.ToLower() == CustomSearchExpressionType.TSQL.ToString().ToLower()))
                        .ToList();

                    dbContext.CustomSearchExpression.RemoveRange(expressionsToRemove);
                    expressionsToRemove.ForEach(e => dataset.CustomSearchExpression.Remove(e));

                    ReadOnlyCollection<DbColumn> dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);

                    if ((importCalculatedColumnsData != null) &&
                        (importCalculatedColumnsData.CalculatedColumns != null) &&
                        (importCalculatedColumnsData.CalculatedColumns.Length > 0))
                    {
                        int executionOrder = 0;
                        foreach (var calculatedColumnData in importCalculatedColumnsData.CalculatedColumns)
                        {
                            InputValidationHelper.AssertNotEmpty(calculatedColumnData.ColumnName, nameof(calculatedColumnData.ColumnName));
                            InputValidationHelper.AssertNotEmpty(calculatedColumnData.Script, nameof(calculatedColumnData.Script));

                            DbColumn dbColumn = dbColumns.FirstOrDefault(c => c.ColumnName.ToLower() == calculatedColumnData.ColumnName.ToLower());
                            if (dbColumn != null)
                            {
                                throw new CustomSearchesConflictException(string.Format("Column '{0}' already exists in table.", calculatedColumnData.ColumnName), null);
                            }

                            CustomSearchExpression newExpression = new CustomSearchExpression()
                            {
                                DatasetId = dataset.DatasetId,
                                ColumnName = calculatedColumnData.ColumnName,
                                ExpressionRole = CustomSearchExpressionRoleType.CalculatedColumn.ToString(),
                                OwnerType = CustomSearchExpressionOwnerType.Dataset.ToString(),
                                ExpressionType = CustomSearchExpressionType.TSQL.ToString(),
                                Script = calculatedColumnData.Script,
                                ExecutionOrder = executionOrder
                            };

                            executionOrder++;
                            dbContext.CustomSearchExpression.Add(newExpression);
                            dataset.CustomSearchExpression.Add(newExpression);
                            expressionsToAdd.Add(newExpression);
                        }

                        int notRepeatedColumnsCount = importCalculatedColumnsData.CalculatedColumns.Select(c => c.ColumnName.Trim().ToLower()).ToHashSet().Count;

                        if (notRepeatedColumnsCount != importCalculatedColumnsData.CalculatedColumns.Length)
                        {
                            throw new CustomSearchesRequestBodyException(
                                $"Calculated columns have repeated names.",
                                innerException: null);
                        }
                    }

                    var oldColumns = expressionsToRemove.Select(e => e.ColumnName);
                    var updatedColumns = expressionsToAdd.Select(e => e.ColumnName).Intersect(oldColumns);

                    foreach (var updatedColumn in updatedColumns)
                    {
                        var oldExpression = expressionsToRemove.First(e => e.ColumnName.ToLower() == updatedColumn.ToLower());
                        var newExpression = expressionsToAdd.First(e => e.ColumnName.ToLower() == updatedColumn.ToLower());

                        if (oldExpression.Script != newExpression.Script)
                        {
                            foreach (var currentDatasetPostProcess in dataset.DatasetPostProcess)
                            {
                                if (!currentDatasetPostProcess.IsDirty)
                                {
                                    var postProcessExpressions = CustomSearchExpressionEvaluator.GetOrderedCustomSearchExpressions(currentDatasetPostProcess);

                                    if (CustomSearchExpressionEvaluator.IsColumnUsedInExpressions(updatedColumn, postProcessExpressions))
                                    {
                                        currentDatasetPostProcess.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
                                        currentDatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                                        currentDatasetPostProcess.IsDirty = true;
                                        dbContext.DatasetPostProcess.Update(currentDatasetPostProcess);
                                        datasetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                                    }
                                }
                            }

                            foreach (var referenceDataset in dataset.InverseSourceDataset)
                            {
                                foreach (var currentDatasetPostProcess in referenceDataset.DatasetPostProcess)
                                {
                                    if (!currentDatasetPostProcess.IsDirty)
                                    {
                                        var postProcessExpressions = CustomSearchExpressionEvaluator.GetOrderedCustomSearchExpressions(currentDatasetPostProcess);

                                        if (CustomSearchExpressionEvaluator.IsColumnUsedInExpressions(updatedColumn, postProcessExpressions))
                                        {
                                            currentDatasetPostProcess.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
                                            currentDatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                                            currentDatasetPostProcess.IsDirty = true;
                                            dbContext.DatasetPostProcess.Update(currentDatasetPostProcess);
                                        }
                                    }
                                }

                                referenceDataset.DataSetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                            }
                        }
                    }

                    // Validates if the expression can be removed.
                    CustomSearchExpressionEvaluator.AssertExpressionReferencesToColumnsAreValid(dataset, dbColumns: null, oldColumns);

                    await dbContext.ValidateAndSaveChangesAsync();

                    string updateView = await DbTransientRetryPolicy.GetDatasetViewScriptAsync(
                        this.ServiceContext,
                        dataset,
                        expressionsToAdd,
                        applyUpdates: true,
                        dbContext);

                    string commandText = "ALTER VIEW " + CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, false /*usePostProcess*/) + "\n";
                    commandText += " AS \n";
                    commandText += updateView;

                    await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        commandText,
                        parameters: null);

                    // Update post process view
                    DatasetPostProcessExecutionPayloadData payload = new DatasetPostProcessExecutionPayloadData
                    {
                        OnlyView = true,
                        DatasetId = dataset.DatasetId
                    };

                    ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
                    await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"ImportCalculatedColumns: {message}"); });

                    foreach (var referenceDataset in dataset.InverseSourceDataset)
                    {
                        payload.DatasetId = referenceDataset.DatasetId;
                        await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"ImportCalculatedColumns: {message}"); });
                    }

                    return (datasetState, datasetPostProcessState);
                },
                dataset,
                isRootLock: true,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext);
        }
    }
}
