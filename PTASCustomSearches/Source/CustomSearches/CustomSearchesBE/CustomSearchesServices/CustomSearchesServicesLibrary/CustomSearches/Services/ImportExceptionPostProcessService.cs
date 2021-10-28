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
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that imports the exception post process.
    /// </summary>
    public class ImportExceptionPostProcessService : BaseService
    {
        /// <summary>
        /// RScript blob container name.
        /// </summary>
        public const string RScriptBlobContainerName = "rscript";

        /// <summary>
        /// The request body error message.
        /// </summary>
        public const string ExceptionPostProcessRolesErrorMessage = "Each exception post process rule must have one role of '" +
            nameof(CustomSearchExpressionRoleType.FilterExpression) +
            "' and one role of '" + nameof(CustomSearchExpressionRoleType.CalculatedColumn) + "'.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportExceptionPostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportExceptionPostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the exception post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="validateOnlyFilterExpressions">Value indicating whether only the filter expressions should be validated.</param>
        /// <returns>
        /// The post process id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<IdResult> ImportExceptionPostProcessAsync(
            DatasetPostProcessData datasetPostProcessData,
            CustomSearchesDbContext dbContext,
            bool validateOnlyFilterExpressions = false)
        {
            InputValidationHelper.AssertZero(datasetPostProcessData.DatasetPostProcessId, nameof(DatasetPostProcess), nameof(datasetPostProcessData.DatasetPostProcessId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.DatasetId, nameof(datasetPostProcessData.DatasetId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.PostProcessName, nameof(datasetPostProcessData.PostProcessName));
            InputValidationHelper.AssertZero(datasetPostProcessData.RscriptModelId ?? 0, nameof(DatasetPostProcess), nameof(datasetPostProcessData.RscriptModelId));
            InputValidationHelper.AssertEmpty(datasetPostProcessData.CustomSearchExpressions, nameof(datasetPostProcessData.CustomSearchExpressions));

            List<Guid> secondaryDatasetIds = new List<Guid>();
            List<Guid> datasetIds = new List<Guid>();
            datasetIds.Add(datasetPostProcessData.DatasetId);
            if (datasetPostProcessData.SecondaryDatasets?.Length > 0)
            {
                datasetIds.AddRange(datasetPostProcessData.SecondaryDatasets);
                secondaryDatasetIds.AddRange(datasetPostProcessData.SecondaryDatasets);
            }

            var datasets = await DatasetHelper.LoadDatasetsWithDependenciesAsync(
                dbContext,
                datasetIds.ToArray(),
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: false,
                includeUserProject: false,
                includeDatasetUserClientState: false);

            var dataset = datasets.FirstOrDefault(d => d.DatasetId == datasetPostProcessData.DatasetId);

            InputValidationHelper.AssertEntityExists(dataset, nameof(Dataset), datasetPostProcessData.DatasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "ImportExceptionPostProcess");
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            List<DatasetPostProcess> primaryAndSecondariesPostProcesses =
                await DatasetHelper.LoadPrimaryAndSecondariesPostProcessesAsync(userProject, dataset, datasetPostProcessData, DatasetPostProcessType.ExceptionPostProcess, dbContext);

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
            projectBusinessLogic.ValidateImportPostProcess(datasetPostProcessData, datasets);

            List<DatasetPostProcess> importedPostProcesses = new List<DatasetPostProcess>();
            DatasetPostProcess primaryDatasetPostProcess = null;

            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    foreach (var currentDataset in datasets)
                    {
                        datasetPostProcessData.DatasetId = currentDataset.DatasetId;

                        var datasetPostProcess = primaryAndSecondariesPostProcesses.FirstOrDefault(dpp => dpp.DatasetId == currentDataset.DatasetId);

                        if (datasetPostProcess != null)
                        {
                            primaryAndSecondariesPostProcesses.Remove(datasetPostProcess);
                        }

                        datasetPostProcess = await this.ImportExceptionPostProcessAsync(currentDataset, datasetPostProcessData, datasetPostProcess, dbContext, validateOnlyFilterExpressions);

                        datasetPostProcess.PrimaryDatasetPostProcess = primaryDatasetPostProcess;
                        if (primaryDatasetPostProcess == null)
                        {
                            primaryDatasetPostProcess = datasetPostProcess;
                        }

                        importedPostProcesses.Add(datasetPostProcess);
                    }

                    DeleteDatasetPostProcessService service = new DeleteDatasetPostProcessService(this.ServiceContext);
                    CloudBlobContainer blobContainer = await this.ServiceContext.CloudStorageProvider.GetCloudBlobContainer(ImportExceptionPostProcessService.RScriptBlobContainerName, this.ServiceContext.AppCredential);

                    // Remove unused post processes.
                    await service.DeleteDatasetPostProcessesAsync(primaryAndSecondariesPostProcesses, dbContext, bypassPostProcessBundleCheck: true, checkPostProcessStackOnPivot: false);
                    await service.DeleteDependenciesAsync(primaryAndSecondariesPostProcesses, blobContainer);

                    await dbContext.ValidateAndSaveChangesAsync();
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

            return new IdResult(primaryDatasetPostProcess.DatasetPostProcessId);
        }

        /// <summary>
        /// Imports the exception post process.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcessData">The dataset post process data .</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="validateOnlyFilterExpressions">Value indicating whether only the filter expressions should be validated.</param>
        /// <returns>
        /// The post process.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<DatasetPostProcess> ImportExceptionPostProcessAsync(
            Dataset dataset,
            DatasetPostProcessData datasetPostProcessData,
            DatasetPostProcess datasetPostProcess,
            CustomSearchesDbContext dbContext,
            bool validateOnlyFilterExpressions = false)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            IEnumerable<string> columnsToValidate = null;

            if (datasetPostProcess != null)
            {
                columnsToValidate = CustomSearchExpressionEvaluator.GetCalculatedColumnNames(datasetPostProcess);
                dbContext.CustomSearchExpression.RemoveRange(datasetPostProcess.CustomSearchExpression);
                datasetPostProcess.CustomSearchExpression.Clear();

                foreach (var exceptionPostProcessRule in datasetPostProcess.ExceptionPostProcessRule)
                {
                    dbContext.CustomSearchExpression.RemoveRange(exceptionPostProcessRule.CustomSearchExpression);
                }

                dbContext.ExceptionPostProcessRule.RemoveRange(datasetPostProcess.ExceptionPostProcessRule);
                datasetPostProcess.ExceptionPostProcessRule.Clear();
                dbContext.DatasetPostProcess.Update(datasetPostProcess);
            }
            else
            {
                datasetPostProcess = new DatasetPostProcess()
                {
                    DatasetId = datasetPostProcessData.DatasetId,
                    PostProcessName = datasetPostProcessData.PostProcessName,
                    PostProcessRole = datasetPostProcessData.PostProcessRole,
                    PostProcessType = DatasetPostProcessType.ExceptionPostProcess.ToString(),
                    CreatedBy = userId,
                    CreatedTimestamp = DateTime.UtcNow,
                    ExecutionOrder = int.MaxValue,

                    // Priority is only supported on creation. Priority updates could break column expressions.
                    Priority = datasetPostProcessData.Priority
                };

                dataset.DatasetPostProcess.Add(datasetPostProcess);
                dbContext.DatasetPostProcess.Add(datasetPostProcess);
            }

            ExceptionMethodType exceptionMethod = ExceptionMethodType.UniqueConditionSelector;
            if (!string.IsNullOrWhiteSpace(datasetPostProcessData.PostProcessSubType))
            {
                exceptionMethod =
                    InputValidationHelper.ValidateEnum<ExceptionMethodType>(datasetPostProcessData.PostProcessSubType, nameof(datasetPostProcessData.PostProcessSubType));
            }

            datasetPostProcess.TraceEnabledFields = JsonHelper.SerializeObject(datasetPostProcessData.TraceEnabledFields);

            datasetPostProcess.PostProcessSubType = exceptionMethod.ToString();
            datasetPostProcess.IsDirty = true;
            datasetPostProcess.PostProcessDefinition = datasetPostProcessData.PostProcessDefinition;
            datasetPostProcess.LastModifiedBy = userId;
            datasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;

            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.ExceptionPostProcessRules, nameof(datasetPostProcessData.ExceptionPostProcessRules));

            var allExpressions = new List<CustomSearchExpression>();
            if ((datasetPostProcessData.CustomSearchExpressions != null) && (datasetPostProcessData.CustomSearchExpressions.Length > 0))
            {
                int executionOrder = 0;
                foreach (var expressionData in datasetPostProcessData.CustomSearchExpressions)
                {
                    expressionData.CustomSearchExpressionId = 0;
                    CustomSearchExpression expression = expressionData.ToEfModel();
                    expression.ExecutionOrder = executionOrder;
                    datasetPostProcess.CustomSearchExpression.Add(expression);
                    allExpressions.Add(expression);
                    executionOrder++;
                }

                CustomSearchExpressionValidator.ValidateTypes(
                    datasetPostProcess.CustomSearchExpression,
                    validRoles: new CustomSearchExpressionRoleType[] { CustomSearchExpressionRoleType.GroupFilterExpression },
                    message: $"The expressions that belong directly to the exception post process should have the role of '{CustomSearchExpressionRoleType.GroupFilterExpression}'.");
            }

            for (int i = 0; i < datasetPostProcessData.ExceptionPostProcessRules.Length; i++)
            {
                var exceptionPostProcessRuleData = datasetPostProcessData.ExceptionPostProcessRules[i];
                ExceptionPostProcessRule exceptionPostProcessRule = new ExceptionPostProcessRule()
                {
                    Description = exceptionPostProcessRuleData.Description,
                    GroupName = exceptionPostProcessRuleData.GroupName,
                    ExecutionOrder = i
                };

                datasetPostProcess.ExceptionPostProcessRule.Add(exceptionPostProcessRule);

                InputValidationHelper.AssertNotEmpty(
                    exceptionPostProcessRuleData.CustomSearchExpressions,
                    nameof(exceptionPostProcessRuleData.CustomSearchExpressions),
                    ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage);

                if (exceptionPostProcessRuleData.CustomSearchExpressions.Length != 2)
                {
                    throw new CustomSearchesRequestBodyException(ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage, innerException: null);
                }

                var ruleExpressions = exceptionPostProcessRuleData.CustomSearchExpressions;
                CustomSearchExpressionData filterExpression = ruleExpressions
                    .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower());
                CustomSearchExpressionData valueExpression = ruleExpressions
                    .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower());

                for (int j = 0; j < exceptionPostProcessRuleData.CustomSearchExpressions.Length; j++)
                {
                    var item = exceptionPostProcessRuleData.CustomSearchExpressions[j];
                    CustomSearchExpression customSearchExpression = new CustomSearchExpression()
                    {
                        ColumnName = item.ColumnName,
                        ExpressionRole = item.ExpressionRole,
                        ExpressionType = item.ExpressionType,
                        OwnerType = CustomSearchExpressionOwnerType.ExceptionPostProcessRule.ToString(),
                        Script = item.Script,
                        IsAutoGenerated = item.IsAutoGenerated,
                        ExecutionOrder = j
                    };

                    customSearchExpression.ExpressionExtensions = JsonHelper.SerializeObject(item.ExpressionExtensions);

                    exceptionPostProcessRule.CustomSearchExpression.Add(customSearchExpression);
                    allExpressions.Add(customSearchExpression);
                }

                CustomSearchExpressionValidator.AssertHasOneExpression(
                    exceptionPostProcessRule.CustomSearchExpression,
                    CustomSearchExpressionRoleType.FilterExpression,
                    ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage);

                CustomSearchExpressionValidator.AssertHasOneExpression(
                    exceptionPostProcessRule.CustomSearchExpression,
                    CustomSearchExpressionRoleType.CalculatedColumn,
                    ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage);
            }

            CustomSearchExpressionValidator.ValidateTypes(
                allExpressions,
                validRoles: null);

            await CustomSearchExpressionValidator.ValidateExpressionScriptsAsync(
                validateOnlyFilterExpressions ?
                allExpressions.Where(e => e.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower()).ToList() :
                allExpressions,
                this.ServiceContext,
                dataset,
                DatasetHelper.GetPreviousDatasetPostProcess(dataset, datasetPostProcess),
                datasetPostProcess,
                chartTypeContext: null,
                throwOnFail: true);

            // Validates if the expressions can be removed.
            if ((columnsToValidate != null) && (columnsToValidate.Count() > 0))
            {
                var dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);

                CustomSearchExpressionEvaluator.AssertExpressionReferencesToColumnsAreValid(dataset, dbColumns, columnsToValidate);
            }

            return datasetPostProcess;
        }
    }
}
