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
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Service that imports the stored procedure update post process.
    /// </summary>
    public class ImportStoredProcedureUpdatePostProcessService : BaseService
    {
        /// <summary>
        /// The 'isSchemaProbe' stored procedure parameter name.
        /// </summary>
        public static readonly string IsSchemaProbeParameterName = "isSchemaProbe";

        /// <summary>
        /// The 'major' stored procedure parameter name.
        /// </summary>
        public static readonly string MajorParameterName = "major";

        /// <summary>
        /// The 'minor' stored procedure parameter name.
        /// </summary>
        public static readonly string MinorParameterName = "minor";

        /// <summary>
        /// The 'backendUpdates' stored procedure parameter name.
        /// </summary>
        public static readonly string BackendUpdatesParameterName = "backendUpdates";

        /// <summary>
        /// The required stored procedure parameters.
        /// </summary>
        private static readonly HashSet<string> RequiredSPParameters =
            new HashSet<string> { IsSchemaProbeParameterName, MajorParameterName, MinorParameterName, BackendUpdatesParameterName };

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportStoredProcedureUpdatePostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportStoredProcedureUpdatePostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the schema of the stored procedure results.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure name.</param>
        /// <param name="sqlParameterNames">The input parameters names of the stored procedure.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="throwIfNotRow">if set to <c>true</c> throws an exception if can't get the top row from database.</param>
        /// <returns>
        /// The schema of the stored procedure results.
        /// </returns>
        public static async Task<ReadOnlyCollection<DbColumn>> GetStoredProcedureResultSchemaAsync(
            string storedProcedureName,
            HashSet<string> sqlParameterNames,
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            IServiceContext serviceContext,
            bool throwIfNotRow)
        {
            var prevDatasetPostProcess = DatasetHelper.GetPreviousDatasetPostProcess(dataset, datasetPostProcess);

            // Get top dataset row
            var datasetRowResponse = await GetDatasetDataService.GetDatasetDataAsync(
                            dataset,
                            startRowIndex: 0,
                            rowCount: 1,
                            usePostProcess: prevDatasetPostProcess != null,
                            prevDatasetPostProcess,
                            serviceContext);

            if (datasetRowResponse.Results == null || datasetRowResponse.Results.Count() == 0)
            {
                if (throwIfNotRow)
                {
                    throw new CustomSearchesRowsRequiredException(
                        $"No row was found in the database view to use as input for the stored procedure '{storedProcedureName}'.",
                        innerException: null);
                }

                return null;
            }

            var datasetRowDictionary = (IDictionary<string, object>)datasetRowResponse.Results.FirstOrDefault();
            datasetRowDictionary = datasetRowDictionary.ToDictionary(kvp => kvp.Key.Trim().ToLower(), kvp => kvp.Value);

            string commandParameters = string.Empty;
            List<SqlParameter> dbParameters = new List<SqlParameter>();

            var notFoundParameters = ImportStoredProcedureUpdatePostProcessService.RequiredSPParameters.Except(sqlParameterNames, StringComparer.OrdinalIgnoreCase);

            if (notFoundParameters.Count() > 0)
            {
                string notFoundValues = string.Join(" or ", notFoundParameters.Select(v => $"'{v}'"));
                throw new CustomSearchesRequestBodyException(
                    $"The stored procedure should have the following parameter(s): {notFoundValues}.",
                    innerException: null);
            }

            foreach (var sqlParameterName in sqlParameterNames)
            {
                string parameterNameToLower = sqlParameterName.ToLower();
                SqlParameter dbParameter = new SqlParameter();

                if (!datasetRowDictionary.ContainsKey(parameterNameToLower))
                {
                    if (parameterNameToLower == ImportStoredProcedureUpdatePostProcessService.IsSchemaProbeParameterName.ToLower())
                    {
                        dbParameter.Value = true;
                    }
                    else if (parameterNameToLower == ImportStoredProcedureUpdatePostProcessService.BackendUpdatesParameterName.ToLower())
                    {
                        dbParameter.Direction = ParameterDirection.Output;
                        dbParameter.SqlDbType = SqlDbType.NVarChar;
                        dbParameter.Size = -1;
                    }
                    else
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"The stored procedure of the post process '{datasetPostProcess.DatasetPostProcessId}' ({datasetPostProcess.PostProcessRole}) " +
                            $"has the parameter '{sqlParameterName}' that doesn't correspond to any column in the dataset view.",
                            innerException: null);
                    }
                }
                else
                {
                    dbParameter.Value = datasetRowDictionary[parameterNameToLower];
                }

                dbParameter.ParameterName = $"{sqlParameterName}Value";
                dbParameters.Add(dbParameter);

                commandParameters += $" @{sqlParameterName} = @{dbParameter.ParameterName},";
            }

            commandParameters = commandParameters.TrimEnd(new char[] { ',' });

            return await StoredProcedureHelper.GetStoredProcedureResultSchemaAsync(
                storedProcedureName,
                commandParameters,
                dbParameters.ToArray(),
                serviceContext);
        }

        /// <summary>
        /// Imports the stored procedure update post process.
        /// The name, role and type are used as the key to identify the post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The post process id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<IdResult> ImportStoredProcedureUpdatePostProcessAsync(DatasetPostProcessData datasetPostProcessData, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertZero(datasetPostProcessData.DatasetPostProcessId, nameof(DatasetPostProcess), nameof(datasetPostProcessData.DatasetPostProcessId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.DatasetId, nameof(datasetPostProcessData.DatasetId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.PostProcessName, nameof(datasetPostProcessData.PostProcessName));
            InputValidationHelper.AssertEmpty(datasetPostProcessData.ExceptionPostProcessRules, nameof(datasetPostProcessData.ExceptionPostProcessRules));
            InputValidationHelper.AssertZero(datasetPostProcessData.RscriptModelId ?? 0, nameof(DatasetPostProcess), nameof(datasetPostProcessData.RscriptModelId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.CustomSearchExpressions, nameof(datasetPostProcessData.CustomSearchExpressions));
            InputValidationHelper.AssertEmpty(datasetPostProcessData.SecondaryDatasets, nameof(datasetPostProcessData.SecondaryDatasets));

            var dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                dbContext,
                datasetPostProcessData.DatasetId,
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: false,
                includeUserProject: false,
                includeDatasetUserClientState: false);

            InputValidationHelper.AssertEntityExists(dataset, nameof(Dataset), datasetPostProcessData.DatasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(
                dataset.UserId,
                dataset.ParentFolder,
                dataset.IsLocked,
                userProject,
                "ImportStoredProcedureUpdatePostProcess");

            int datasetPostProcessId = 0;
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    string postprocessNameLower =
                        datasetPostProcessData.PostProcessName == null ? string.Empty : datasetPostProcessData.PostProcessName.ToLower();
                    string postprocessRoleLower =
                        datasetPostProcessData.PostProcessRole == null ? string.Empty : datasetPostProcessData.PostProcessRole.ToLower();
                    string postprocessTypeLower =
                        DatasetPostProcessType.StoredProcedureUpdatePostProcess.ToString().ToLower();

                    DatasetPostProcess datasetPostProcess = dataset.DatasetPostProcess
                        .Where(p =>
                            (p.DatasetId == datasetPostProcessData.DatasetId)
                             && ((p.PostProcessName == null ? string.Empty : p.PostProcessName.ToLower()) == postprocessNameLower)
                             && ((p.PostProcessRole == null ? string.Empty : p.PostProcessRole.ToLower()) == postprocessRoleLower)
                             && ((p.PostProcessType == null ? string.Empty : p.PostProcessType.ToLower()) == postprocessTypeLower))
                        .FirstOrDefault();

                    IEnumerable<string> columnsToValidate = null;

                    if (datasetPostProcess != null)
                    {
                        columnsToValidate = CustomSearchExpressionEvaluator.GetCalculatedColumnNames(datasetPostProcess);
                        dbContext.CustomSearchExpression.RemoveRange(datasetPostProcess.CustomSearchExpression);
                        datasetPostProcess.CustomSearchExpression.Clear();
                        dbContext.DatasetPostProcess.Update(datasetPostProcess);
                    }
                    else
                    {
                        datasetPostProcess = new DatasetPostProcess()
                        {
                            DatasetId = datasetPostProcessData.DatasetId,
                            PostProcessName = datasetPostProcessData.PostProcessName,
                            PostProcessRole = datasetPostProcessData.PostProcessRole,
                            PostProcessType = DatasetPostProcessType.StoredProcedureUpdatePostProcess.ToString(),
                            CreatedBy = userId,
                            CreatedTimestamp = DateTime.UtcNow,
                            ExecutionOrder = int.MaxValue,

                            // Priority is only supported on creation. Priority updates could break column expressions.
                            Priority = datasetPostProcessData.Priority
                        };

                        dataset.DatasetPostProcess.Add(datasetPostProcess);
                        dbContext.DatasetPostProcess.Add(datasetPostProcess);
                    }

                    datasetPostProcess.PostProcessSubType = null;
                    datasetPostProcess.IsDirty = true;
                    datasetPostProcess.PostProcessDefinition = datasetPostProcessData.PostProcessDefinition;
                    datasetPostProcess.LastModifiedBy = userId;
                    datasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;

                    await this.AddStoredProcedureExpressionsAsync(dataset, datasetPostProcess, datasetPostProcessData);

                    // Validates if the expressions can be removed.
                    if ((columnsToValidate != null) && (columnsToValidate.Count() > 0))
                    {
                        var dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);

                        CustomSearchExpressionEvaluator.AssertExpressionReferencesToColumnsAreValid(dataset, dbColumns, columnsToValidate);
                    }

                    await dbContext.ValidateAndSaveChangesAsync();
                    datasetPostProcessId = datasetPostProcess.DatasetPostProcessId;
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

        /// <summary>
        /// Adds the stored procedure expressions.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="datasetPostProcessData">The dataset post process data.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        private async Task AddStoredProcedureExpressionsAsync(Dataset dataset, DatasetPostProcess datasetPostProcess, DatasetPostProcessData datasetPostProcessData)
        {
            CustomSearchExpression storedProcedureExpression = null;
            var allExpressions = new List<CustomSearchExpression>();
            foreach (var expressionData in datasetPostProcessData.CustomSearchExpressions)
            {
                expressionData.CustomSearchExpressionId = 0;
                storedProcedureExpression = expressionData.ToEfModel();
                storedProcedureExpression.ExecutionOrder = 0;
                datasetPostProcess.CustomSearchExpression.Add(storedProcedureExpression);
                allExpressions.Add(storedProcedureExpression);
            }

            CustomSearchExpressionValidator.AssertHasOneExpression(datasetPostProcess.CustomSearchExpression, CustomSearchExpressionRoleType.UpdateStoredProcedure);

            CustomSearchExpressionValidator.ValidateTypes(
                allExpressions,
                validRoles: new CustomSearchExpressionRoleType[] { CustomSearchExpressionRoleType.UpdateStoredProcedure });

            var prevDatasetPostProcess = DatasetHelper.GetPreviousDatasetPostProcess(dataset, datasetPostProcess);

            await CustomSearchExpressionValidator.ValidateExpressionScriptsAsync(
                allExpressions,
                this.ServiceContext,
                dataset,
                prevDatasetPostProcess,
                datasetPostProcess,
                chartTypeContext: null,
                throwOnFail: true);

            string storedProcedureName = storedProcedureExpression.Script;

            HashSet<string> storedProcedureParameters =
                await StoredProcedureHelper.GetStoredProcedureParametersAsync(storedProcedureName, this.ServiceContext);

            var columnSchema = await ImportStoredProcedureUpdatePostProcessService.GetStoredProcedureResultSchemaAsync(
                storedProcedureName,
                storedProcedureParameters,
                dataset,
                datasetPostProcess,
                this.ServiceContext,
                throwIfNotRow: true);
            int executionOrder = 1;

            storedProcedureParameters.RemoveWhere(p => p.ToLower() == ImportStoredProcedureUpdatePostProcessService.IsSchemaProbeParameterName.ToLower());
            string parametersScript = string.Join(", ", storedProcedureParameters.Select(p => $"[{p}]"));

            // Adding autogenerated columns
            foreach (DbColumn column in columnSchema)
            {
                DatabaseColumnExtensionData databaseColumnExtensionData = new DatabaseColumnExtensionData()
                {
                    DatabaseType = DatabaseColumnHelper.GetDatabaseType(column),
                    IsDatabaseColumnIndexable = DatabaseColumnHelper.IsDatabaseColumnIndexable(column)
                };

                var calculatedExpression = new CustomSearchExpression()
                {
                    ExpressionType = CustomSearchExpressionType.TSQL.ToString(),
                    ExpressionRole = CustomSearchExpressionRoleType.CalculatedColumn.ToString(),
                    ColumnName = column.ColumnName,
                    ExecutionOrder = executionOrder,
                    IsAutoGenerated = true,
                    Script = parametersScript,
                    ExpressionExtensions = JsonHelper.SerializeObject(databaseColumnExtensionData)
                };

                datasetPostProcess.CustomSearchExpression.Add(calculatedExpression);
                executionOrder++;
            }
        }
    }
}
