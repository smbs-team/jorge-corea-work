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
    /// Service that adds a calculated column to the dataset view.
    /// </summary>
    public class AddCalculatedColumnService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddCalculatedColumnService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public AddCalculatedColumnService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Adds a calculated column to the dataset view with a dataset lock.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="calculatedColumnData">The calculated column data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="executePostProcess">Value indicating if should execute the post process.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Empty ColumnName or Script.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter dataset or can't execute this function in a reference dataset or Column already exists in table.</exception>
        public async Task AddCalculatedColumnLockAsync(Guid datasetId, CalculatedColumnData calculatedColumnData, CustomSearchesDbContext dbContext, bool executePostProcess, ILogger logger)
        {
            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetId)
                .Include(d => d.InverseSourceDataset)
                .Include(d => d.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            // Should be applied over source dataset.
            if (dataset.SourceDatasetId != null)
            {
                dataset = await dbContext.Dataset
                 .Where(d => d.DatasetId == dataset.SourceDatasetId)
                 .Include(d => d.InverseSourceDataset)
                 .FirstOrDefaultAsync();
            }

            InputValidationHelper.AssertDatasetDataNotLocked(dataset);
            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "AddCalculatedColumn");

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    datasetPostProcessState = await this.AddCalculatedColumnAsync(dataset, calculatedColumnData, dbContext, datasetPostProcessState, executePostProcess, logger);
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

        /// <summary>
        /// Adds a calculated column to the dataset view.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="calculatedColumnData">The calculated column data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datasetPostProcessState">The dataset post process state.</param>
        /// <param name="executePostProcess">Value indicating if should execute the post process.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The new dataset post process state.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">Empty ColumnName or Script.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't execute this function in a reference dataset or column already exists in table.</exception>
        public async Task<string> AddCalculatedColumnAsync(Dataset dataset, CalculatedColumnData calculatedColumnData, CustomSearchesDbContext dbContext, string datasetPostProcessState, bool executePostProcess, ILogger logger)
        {
            // Should be applied over source dataset.
            if (dataset.SourceDatasetId != null)
            {
                throw new CustomSearchesConflictException(
                    $"Can't execute this function in a reference dataset.",
                    null);
            }

            InputValidationHelper.AssertNotEmpty(calculatedColumnData.ColumnName, nameof(calculatedColumnData.ColumnName));
            InputValidationHelper.AssertNotEmpty(calculatedColumnData.Script, nameof(calculatedColumnData.Script));

            ReadOnlyCollection<DbColumn> dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);

            DbColumn dbColumn = dbColumns.FirstOrDefault(c => c.ColumnName.ToLower() == calculatedColumnData.ColumnName.ToLower());
            if (dbColumn != null)
            {
                throw new CustomSearchesConflictException(string.Format("Column '{0}' already exists in table.", calculatedColumnData.ColumnName), null);
            }

            List<CustomSearchExpression> expressions =
                await dbContext.CustomSearchExpression
                .Where(c =>
                (c.DatasetId == dataset.DatasetId)
                && (c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())
                && (c.OwnerType.ToLower() == CustomSearchExpressionOwnerType.Dataset.ToString().ToLower())
                && (c.ExpressionType.ToLower() == CustomSearchExpressionType.TSQL.ToString().ToLower()))
                .ToListAsync();

            CustomSearchExpression newExpression = expressions.FirstOrDefault(e => e.ColumnName.ToLower() == calculatedColumnData.ColumnName.ToLower());

            if (newExpression == null)
            {
                newExpression = new CustomSearchExpression()
                {
                    DatasetId = dataset.DatasetId,
                    ColumnName = calculatedColumnData.ColumnName,
                    ExpressionRole = CustomSearchExpressionRoleType.CalculatedColumn.ToString(),
                    OwnerType = CustomSearchExpressionOwnerType.Dataset.ToString(),
                    ExpressionType = CustomSearchExpressionType.TSQL.ToString(),
                    Script = calculatedColumnData.Script
                };
                dbContext.CustomSearchExpression.Add(newExpression);
                expressions.Add(newExpression);
            }
            else
            {
                newExpression.Script = calculatedColumnData.Script;
                dbContext.CustomSearchExpression.Update(newExpression);
                var datasetPostProcesses = await dbContext.DatasetPostProcess.Where(p => p.DatasetId == dataset.DatasetId).ToArrayAsync();

                foreach (var currentDatasetPostProcess in datasetPostProcesses)
                {
                    var postProcessExpressions = from e in dbContext.CustomSearchExpression
                                                 join r in dbContext.ExceptionPostProcessRule
                                                 on e.DatasetPostProcessId equals r.DatasetPostProcessId into xp
                                                 from r in xp.DefaultIfEmpty()
                                                 where ((e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()) || (e.ExpressionType == CustomSearchExpressionType.RScript.ToString()))
                                                 && ((e.DatasetPostProcessId == currentDatasetPostProcess.DatasetPostProcessId) || (r != null && e.ExceptionPostProcessRuleId == r.ExceptionPostProcessRuleId))
                                                 select e;

                    var postProcessExpressionsArray = await postProcessExpressions.ToArrayAsync();
                    if (CustomSearchExpressionEvaluator.IsColumnUsedInExpressions(calculatedColumnData.ColumnName, postProcessExpressionsArray))
                    {
                        currentDatasetPostProcess.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
                        currentDatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                        currentDatasetPostProcess.IsDirty = true;
                        dbContext.DatasetPostProcess.Update(currentDatasetPostProcess);
                        datasetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                    }
                }
            }

            await dbContext.ValidateAndSaveChangesAsync();

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

            if (executePostProcess)
            {
                // Update post process view
                DatasetPostProcessExecutionPayloadData payload = new DatasetPostProcessExecutionPayloadData
                {
                    OnlyView = true,
                    DatasetId = dataset.DatasetId
                };

                ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
                await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"AddCalculatedColumn: {message}"); });

                foreach (var referenceDataset in dataset.InverseSourceDataset)
                {
                    payload.DatasetId = referenceDataset.DatasetId;
                    await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"AddCalculatedColumn: {message}"); });
                }
            }

            return datasetPostProcessState;
        }
    }
}
