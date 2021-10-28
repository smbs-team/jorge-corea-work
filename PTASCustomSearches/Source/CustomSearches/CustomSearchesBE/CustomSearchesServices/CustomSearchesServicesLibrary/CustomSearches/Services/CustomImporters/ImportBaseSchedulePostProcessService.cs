﻿namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomImporters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Base service to imports a base schedule post process.
    /// </summary>
    public abstract class ImportBaseSchedulePostProcessService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportBaseSchedulePostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportBaseSchedulePostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Updates the the dataset post process data with the information of the extensions in the custom search expressions.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="columnName">The database column to update.</param>
        public abstract void UpdateDatasetPostProcess(DatasetPostProcessData datasetPostProcessData, string columnName);

        /// <summary>
        /// Creates and adds the default land model exception rule to the dataset post process data.
        /// </summary>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="datasetPostProcessData">The dataset post process data.</param>
        public virtual void AddDefaultLandModelExceptionRule(
            int datasetPostProcessId,
            string columnName,
            DatasetPostProcessData datasetPostProcessData)
        {
            var defaultExceptionPostProcessRule =
                SetupLandModelService.CreateDefaultLandModelExceptionRule(datasetPostProcessId, columnName, datasetPostProcessData.PostProcessRole);

            var defaultExceptionPostProcessRuleData =
                new ExceptionPostProcessRuleData(defaultExceptionPostProcessRule, ModelInitializationType.FullObjectWithDepedendencies, includeAutoGenerated: true);

            var exceptionPostProcessRuleList = new List<ExceptionPostProcessRuleData>();
            if (datasetPostProcessData.ExceptionPostProcessRules?.Length > 0)
            {
                exceptionPostProcessRuleList.AddRange(datasetPostProcessData.ExceptionPostProcessRules);
            }

            exceptionPostProcessRuleList.Add(defaultExceptionPostProcessRuleData);
            datasetPostProcessData.ExceptionPostProcessRules = exceptionPostProcessRuleList.ToArray();
        }

        /// <summary>
        /// Imports and execute the schedule post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="columnName">The name of database column to update.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<IdResult> ImportBaseSchedulePostProcessAsync(
            DatasetPostProcessData datasetPostProcessData,
            string columnName,
            CustomSearchesDbContext dbContext)
        {
            // Updates the dataset post process data using the expression extensions.
            this.UpdateDatasetPostProcess(datasetPostProcessData, columnName);

            var dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                dbContext,
                datasetPostProcessData.DatasetId,
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: false,
                includeUserProject: false,
                includeDatasetUserClientState: false);

            // Gets DatasetPostProcess
            string postprocessNameLower =
                datasetPostProcessData.PostProcessName == null ? string.Empty : datasetPostProcessData.PostProcessName.ToLower();
            string postprocessRoleLower =
                datasetPostProcessData.PostProcessRole == null ? string.Empty : datasetPostProcessData.PostProcessRole.ToLower();
            string postprocessTypeLower =
                DatasetPostProcessType.ExceptionPostProcess.ToString().ToLower();

            DatasetPostProcess datasetPostProcess = dataset.DatasetPostProcess
                .Where(p =>
                    (p.DatasetId == datasetPostProcessData.DatasetId)
                     && ((p.PostProcessName == null ? string.Empty : p.PostProcessName.ToLower()) == postprocessNameLower)
                     && ((p.PostProcessRole == null ? string.Empty : p.PostProcessRole.ToLower()) == postprocessRoleLower)
                     && ((p.PostProcessType == null ? string.Empty : p.PostProcessType.ToLower()) == postprocessTypeLower))
                .FirstOrDefault();

            InputValidationHelper.AssertEntityExists(datasetPostProcess, nameof(DatasetPostProcess), datasetPostProcessData.PostProcessName);

            this.AddDefaultLandModelExceptionRule(datasetPostProcess.DatasetPostProcessId, columnName, datasetPostProcessData);

            // Keep default values in properties that should not change.
            datasetPostProcessData.TraceEnabledFields = JsonHelper.DeserializeObject(datasetPostProcess.TraceEnabledFields);
            datasetPostProcessData.PostProcessSubType = datasetPostProcess.PostProcessSubType;
            datasetPostProcessData.PostProcessDefinition = datasetPostProcess.PostProcessDefinition;

            // Imports the exception post process.
            ImportExceptionPostProcessService importService = new ImportExceptionPostProcessService(this.ServiceContext);
            await importService.ImportExceptionPostProcessAsync(datasetPostProcessData, dbContext, validateOnlyFilterExpressions: true);

            // Queues the exception post process.
            ExecuteDatasetPostProcessService executeService = new ExecuteDatasetPostProcessService(this.ServiceContext);

            var lastLandModelPostProcessRole = PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRoleOrder.Last();
            DatasetPostProcess lastLandModelPostProcess = dataset.DatasetPostProcess
                .Where(p =>
                    ((p.PostProcessRole == null ? string.Empty : p.PostProcessRole.ToLower()) == lastLandModelPostProcessRole.ToLower())
                     && ((p.PostProcessType == null ? string.Empty : p.PostProcessType.ToLower()) == postprocessTypeLower))
                .FirstOrDefault();

            InputValidationHelper.AssertEntityExists(
                lastLandModelPostProcess,
                nameof(DatasetPostProcess),
                lastLandModelPostProcessRole,
                $"The dataset should have a exception post process with role '{lastLandModelPostProcessRole}'");

            return await executeService.QueueExecuteDatasetPostProcessAsync(
                datasetPostProcess.DatasetId,
                lastLandModelPostProcess.DatasetPostProcessId,
                major: null,
                minor: null,
                parameters: null,
                dataStream: null,
                dbContext);
        }
    }
}
