namespace CustomSearchesServicesLibrary.CustomSearches.Services.DatasetPostProcesses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Executor;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScript;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that applies the land regression to schedule.
    /// </summary>
    public class ApplyLandRegressionToScheduleService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyLandRegressionToScheduleService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ApplyLandRegressionToScheduleService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Queues the land schedule execution.
        /// </summary>
        /// <param name="rScriptPostProcessId">The RScript post process id.</param>
        /// <param name="exceptionPostProcessId">The exception post process id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The id of the apply land regression to schedule job.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User Project, dataset or post process not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Dataset is used by a worker job.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">RScript post process and exception post process should belong to the same dataset.</exception>
        public async Task<int> QueueApplyLandRegressionToScheduleAsync(
            int rScriptPostProcessId,
            int exceptionPostProcessId,
            CustomSearchesDbContext dbContext)
        {
            var postProcesses = await dbContext.DatasetPostProcess
                .Where(p => (p.PostProcessType == DatasetPostProcessType.RScriptPostProcess.ToString() && p.DatasetPostProcessId == rScriptPostProcessId) ||
                    (p.PostProcessType == DatasetPostProcessType.ExceptionPostProcess.ToString() && p.DatasetPostProcessId == exceptionPostProcessId))
                .Include(p => p.Dataset)
                .ToArrayAsync();

            var rScriptPostProcess = postProcesses.FirstOrDefault(p => p.DatasetPostProcessId == rScriptPostProcessId);
            var exceptionPostProcess = postProcesses.FirstOrDefault(p => p.DatasetPostProcessId == exceptionPostProcessId);

            InputValidationHelper.AssertEntityExists(rScriptPostProcess, nameof(rScriptPostProcess), rScriptPostProcessId);

            InputValidationHelper.AssertEntityExists(exceptionPostProcess, nameof(exceptionPostProcess), exceptionPostProcessId);
            Dataset dataset = exceptionPostProcess.Dataset;

            InputValidationHelper.AssertEntityExists(dataset, nameof(dataset), rScriptPostProcess.DatasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            InputValidationHelper.AssertEntityExists(userProject, nameof(userProject), entityId: null, $"Dataset should belong to an user project.");

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "ApplyLandRegressionToSchedule");

            ApplyLandRegressionToSchedulePayloadData payload = new ApplyLandRegressionToSchedulePayloadData();
            payload.DatasetId = dataset.DatasetId;
            payload.RScriptPostProcessId = rScriptPostProcessId;
            payload.ExceptionPostProcessId = exceptionPostProcessId;
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            await DatasetHelper.TestAlterDatasetLockAsync(
                dataset.DatasetId,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                isRootLock: false,
                userId,
                lockingJobId: null,
                dbContext);

            return await this.ServiceContext.AddWorkerJobQueueAsync(
                "DatasetPostProcessExecution",
                "ApplyLandRegressionToScheduleJobType",
                userId,
                payload,
                WorkerJobTimeouts.ApplyLandRegressionToScheduleTimeout);
        }

        /// <summary>
        /// Applies the land regression to schedule.
        /// </summary>
        /// <param name="rScriptPostProcessId">The RScript post process id.</param>
        /// <param name="exceptionPostProcessId">The exception post process id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The async task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">RScript post process execution failed.</exception>
        public async Task ApplyLandRegressionToScheduleAsync(
            int rScriptPostProcessId,
            int exceptionPostProcessId,
            CustomSearchesDbContext dbContext,
            ILogger logger)
        {
            var postProcesses = await dbContext.DatasetPostProcess
                .Where(p => (p.PostProcessType == DatasetPostProcessType.RScriptPostProcess.ToString() && p.DatasetPostProcessId == rScriptPostProcessId) ||
                    (p.PostProcessType == DatasetPostProcessType.ExceptionPostProcess.ToString() && p.DatasetPostProcessId == exceptionPostProcessId))
                .Include(p => p.Dataset)
                .Include(p => p.CustomSearchExpression)
                .Include(p => p.ExceptionPostProcessRule)
                    .ThenInclude(e => e.CustomSearchExpression)
                .ToArrayAsync();

            var rScriptPostProcess = postProcesses.FirstOrDefault(p => p.DatasetPostProcessId == rScriptPostProcessId);
            var exceptionPostProcess = postProcesses.FirstOrDefault(p => p.DatasetPostProcessId == exceptionPostProcessId);

            InputValidationHelper.AssertEntityExists(rScriptPostProcess, nameof(rScriptPostProcess), rScriptPostProcessId);
            InputValidationHelper.AssertEntityExists(exceptionPostProcess, nameof(exceptionPostProcess), exceptionPostProcessId);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            DatasetPostProcessExecutionPayloadData payload = new DatasetPostProcessExecutionPayloadData
            {
                OnlyView = false,
                DatasetId = rScriptPostProcess.DatasetId,
                DatasetPostProcessId = rScriptPostProcess.DatasetPostProcessId
            };

            ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);

            if (rScriptPostProcess.IsDirty || rScriptPostProcess.LastExecutionTimestamp == null)
            {
                await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"ApplyLandRegressionToSchedule: {message}"); });

                await dbContext.Entry<DatasetPostProcess>(rScriptPostProcess).ReloadAsync();
            }

            RScriptResultsData rScriptResultsData = JsonHelper.DeserializeObject<RScriptResultsData>(rScriptPostProcess.ResultPayload);
            if (rScriptResultsData.Status?.ToLower() != "success")
            {
                throw new CustomSearchesConflictException(
                    "RScript post process execution failed (couldn't retrieve RScript results).",
                    innerException: null);
            }

            Dictionary<string, string> replacementDictionary = new Dictionary<string, string>();
            var results = rScriptResultsData.Results;
            foreach (JObject rscriptResult in results)
            {
                var resultName = (string)rscriptResult["name"];
                if (!string.IsNullOrWhiteSpace(resultName) && resultName.ToLower() == "regression_results")
                {
                    var properties = rscriptResult.Properties().Where(p => p.Name.ToLower() != "name");
                    foreach (var property in properties)
                    {
                        var propertyValue = RScriptDatasetPostProcessExecutor.CheckNotNA(rscriptResult, property.Name);
                        replacementDictionary.Add(property.Name, propertyValue.ToString());
                    }
                }
            }

            CustomSearchExpression landScheduleStepValueExpression = rScriptPostProcess.CustomSearchExpression
                .Where(e => e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.DynamicEvaluator.ToString().ToLower() &&
                      (!string.IsNullOrWhiteSpace(e.ColumnName) && e.ColumnName.Trim().ToLower() == "LandScheduleStepValue".ToLower()))
                .FirstOrDefault();

            InputValidationHelper.AssertEntityExists(
                landScheduleStepValueExpression,
                nameof(landScheduleStepValueExpression),
                $"RScript post process should have a expression with role '{CustomSearchExpressionRoleType.DynamicEvaluator}' and ColumnName 'LandScheduleStepValue'");

            foreach (var exceptionRule in exceptionPostProcess.ExceptionPostProcessRule)
            {
                CustomSearchExpression filterExpression = exceptionRule.CustomSearchExpression
                    .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower());

                if (!string.IsNullOrWhiteSpace(filterExpression.ExpressionExtensions))
                {
                    var expressionExtensions = JsonHelper.DeserializeObject<JObject>(filterExpression.ExpressionExtensions);
                    var scheduleMaxProperty = expressionExtensions.Properties().FirstOrDefault(p => p.Name.ToLower() == "schedulemax");
                    if (scheduleMaxProperty != null)
                    {
                        var scheduleMaxValue = RScriptDatasetPostProcessExecutor.CheckNotNA(expressionExtensions, scheduleMaxProperty.Name);
                        CustomSearchExpression valueExpression = exceptionRule.CustomSearchExpression
                            .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower());

                        replacementDictionary["ScheduleMaxSqFt"] = scheduleMaxValue.ToString();

                        valueExpression.Script =
                            await CustomSearchExpressionEvaluator.EvaluateFormulaAsync<string>(
                                landScheduleStepValueExpression.Script,
                                replacementDictionary,
                                this.ServiceContext,
                                exceptionPostProcess.Dataset);
                    }
                }
            }

            exceptionPostProcess.LastModifiedBy = userId;
            exceptionPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
            exceptionPostProcess.IsDirty = true;
            await dbContext.ValidateAndSaveChangesAsync();

            payload.DatasetPostProcessId = exceptionPostProcess.DatasetPostProcessId;
            await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"ApplyLandRegressionToSchedule: {message}"); });
        }
    }
}
