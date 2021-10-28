namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service that deletes a calculated column to the dataset view.
    /// </summary>
    public class DeleteCalculatedColumnService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCalculatedColumnService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteCalculatedColumnService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes a calculated column to the dataset view..
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Calculated column should not be used in expressions of: charts, datasets, post processes or exception post process rules.</exception>
        public async Task DeleteCalculatedColumnAsync(Guid datasetId, string columnName, CustomSearchesDbContext dbContext, ILogger logger)
        {
            InputValidationHelper.AssertNotEmpty(columnName, nameof(columnName));

            Dataset dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                dbContext,
                datasetId,
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: true,
                includeUserProject: false,
                includeDatasetUserClientState: false);

            InputValidationHelper.AssertEntityExists(dataset, nameof(Dataset), datasetId);

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

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "DeleteCalculatedColumn");

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    List<CustomSearchExpression> expressions =
                    dataset.CustomSearchExpression
                    .Where(c =>
                    (c.DatasetId == datasetId)
                    && (c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())
                    && (c.OwnerType.ToLower() == CustomSearchExpressionOwnerType.Dataset.ToString().ToLower())
                    && (c.ExpressionType.ToLower() == CustomSearchExpressionType.TSQL.ToString().ToLower())).ToList();

                    CustomSearchExpression expressionToDelete = expressions.FirstOrDefault(e => e.ColumnName.ToLower() == columnName.ToLower());

                    if (expressionToDelete == null)
                    {
                        throw new CustomSearchesEntityNotFoundException(
                            $"Calculated column '{columnName}' was not found in table.",
                            null);
                    }

                    dbContext.CustomSearchExpression.Remove(expressionToDelete);

                    // Validates if the expression can be removed.
                    dataset.CustomSearchExpression.Remove(expressionToDelete);
                    CustomSearchExpressionEvaluator.AssertExpressionReferencesToColumnsAreValid(dataset, dbColumns: null, new string[] { expressionToDelete.ColumnName });

                    await dbContext.SaveChangesAsync();
                    expressions.Remove(expressionToDelete);

                    string updateView = await DbTransientRetryPolicy.GetDatasetViewScriptAsync(
                        this.ServiceContext,
                        dataset,
                        expressions,
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
                        DatasetId = datasetId
                    };

                    ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
                    await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"DeleteCalculatedColumn: {message}"); });

                    foreach (var referenceDataset in dataset.InverseSourceDataset)
                    {
                        payload.DatasetId = referenceDataset.DatasetId;
                        await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"DeleteCalculatedColumn: {message}"); });
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
