namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that imports the filter post process.
    /// </summary>
    public class ImportFilterPostProcessService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFilterPostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportFilterPostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the filter post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The post process id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<IdResult> ImportFilterPostProcessAsync(DatasetPostProcessData datasetPostProcessData, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertZero(datasetPostProcessData.DatasetPostProcessId, nameof(DatasetPostProcess), nameof(datasetPostProcessData.DatasetPostProcessId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.DatasetId, nameof(datasetPostProcessData.DatasetId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.PostProcessName, nameof(datasetPostProcessData.PostProcessName));
            InputValidationHelper.AssertEmpty(datasetPostProcessData.ExceptionPostProcessRules, nameof(datasetPostProcessData.ExceptionPostProcessRules));
            InputValidationHelper.AssertZero(datasetPostProcessData.RscriptModelId ?? 0, nameof(DatasetPostProcess), nameof(datasetPostProcessData.RscriptModelId));
            InputValidationHelper.AssertEmpty(datasetPostProcessData.SecondaryDatasets, nameof(datasetPostProcessData.SecondaryDatasets));

            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetPostProcessData.DatasetId)
                .Include(d => d.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, nameof(Dataset), datasetPostProcessData.DatasetId);
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.CustomSearchExpressions, nameof(datasetPostProcessData.CustomSearchExpressions));
            InputValidationHelper.AssertEmpty(datasetPostProcessData.ExceptionPostProcessRules, nameof(datasetPostProcessData.ExceptionPostProcessRules));

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "ImportFilterPostProcess");
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            var allExpressions = new List<CustomSearchExpression>();
            DatasetPostProcess newDatasetPostProcess = datasetPostProcessData.ToEfModel(out allExpressions);

            // Validate all expressions
            CustomSearchExpressionValidator.ValidateTypes(
                allExpressions,
                validRoles: new CustomSearchExpressionRoleType[] { CustomSearchExpressionRoleType.FilterExpression });

            // Assign explicit order to expressions
            int executionOrder = 0;
            foreach (var expression in allExpressions)
            {
                expression.ExecutionOrder = executionOrder;
                executionOrder++;
            }

            await CustomSearchExpressionValidator.ValidateExpressionScriptsAsync(
                allExpressions,
                this.ServiceContext,
                dataset,
                DatasetHelper.GetPreviousDatasetPostProcess(dataset, newDatasetPostProcess),
                newDatasetPostProcess,
                chartTypeContext: null,
                throwOnFail: true);

            int datasetPostProcessId = 0;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    string postprocessNameLower =
                       datasetPostProcessData.PostProcessName == null ? string.Empty : datasetPostProcessData.PostProcessName.ToLower();
                    string postprocessRoleLower =
                        datasetPostProcessData.PostProcessRole == null ? string.Empty : datasetPostProcessData.PostProcessRole.ToLower();
                    string postprocessTypeLower =
                        DatasetPostProcessType.FilterPostProcess.ToString().ToLower();

                    DatasetPostProcess existingDatasetPostProcess = await dbContext.DatasetPostProcess
                        .Where(p =>
                        (p.DatasetId == datasetPostProcessData.DatasetId)
                         && ((p.PostProcessName == null ? string.Empty : p.PostProcessName.ToLower()) == postprocessNameLower)
                         && ((p.PostProcessRole == null ? string.Empty : p.PostProcessRole.ToLower()) == postprocessRoleLower)
                         && ((p.PostProcessType == null ? string.Empty : p.PostProcessType.ToLower()) == postprocessTypeLower))
                        .Include(p => p.CustomSearchExpression)
                        .FirstOrDefaultAsync();

                    var dataSetPostProcessToSave = existingDatasetPostProcess;

                    if (dataSetPostProcessToSave != null)
                    {
                        dbContext.CustomSearchExpression.RemoveRange(existingDatasetPostProcess.CustomSearchExpression);
                        existingDatasetPostProcess.CustomSearchExpression.Clear();

                        // Add new expressions
                        foreach (var newExpression in newDatasetPostProcess.CustomSearchExpression)
                        {
                            existingDatasetPostProcess.CustomSearchExpression.Add(newExpression);
                        }

                        dbContext.DatasetPostProcess.Update(existingDatasetPostProcess);
                    }
                    else
                    {
                        dataSetPostProcessToSave = newDatasetPostProcess;

                        dataSetPostProcessToSave.PostProcessType = DatasetPostProcessType.FilterPostProcess.ToString();
                        dataSetPostProcessToSave.CreatedBy = userId;
                        dataSetPostProcessToSave.CreatedTimestamp = DateTime.UtcNow;
                        dataSetPostProcessToSave.ExecutionOrder = int.MaxValue;

                        // Priority is only supported on creation. Priority updates could break column expressions.
                        dataSetPostProcessToSave.Priority = datasetPostProcessData.Priority;

                        dbContext.DatasetPostProcess.Add(newDatasetPostProcess);
                    }

                    dataSetPostProcessToSave.IsDirty = true;
                    dataSetPostProcessToSave.LastModifiedBy = userId;
                    dataSetPostProcessToSave.LastModifiedTimestamp = DateTime.UtcNow;

                    await dbContext.ValidateAndSaveChangesAsync();
                    datasetPostProcessId = dataSetPostProcessToSave.DatasetPostProcessId;
                    datasetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                    return (datasetState, datasetPostProcessState);
                },
                dataset,
                isRootLock: false,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext);

            return new IdResult(datasetPostProcessId);
        }
    }
}
