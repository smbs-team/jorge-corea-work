namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
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
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASCRMHelpers;

    /// <summary>
    /// Service that applies the user project model.
    /// </summary>
    public class ApplyModelService : BaseService
    {
        /// <summary>
        /// The bulk update post process role.
        /// </summary>
        public const string BulkUpdatePostProcessRole = "ApplyModelBulkUpdate";

        /// <summary>
        /// The apply model dataset role.
        /// </summary>
        public const string ApplyModelDatasetRole = "ApplyModel";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyModelService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ApplyModelService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the custom search parameter that indicates if the table type should be used in the stored procedure.
        /// </summary>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="projectBusinessLogic">The project business logic.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The custom search parameter.</returns>
        public static async Task<CustomSearchParameterValueData[]> GetApplyModelDatasetParametersAsync(
            CustomSearchDefinition customSearchDefinition,
            DefaultProjectBusinessLogic projectBusinessLogic,
            Guid userId,
            string major,
            string minor,
            CustomSearchesDbContext dbContext)
        {
            var applyModelDatasetParameters = new List<CustomSearchParameterValueData>();

            var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetId: null);
            await dbContext.CustomSearchDefinition.
                Where(csd => csd.CustomSearchDefinitionId == pivotDataset.CustomSearchDefinitionId).
                Include(csd => csd.CustomSearchParameter).
                LoadAsync();

            var pivotDefinitionParameters = pivotDataset.CustomSearchDefinition.CustomSearchParameter;
            var customSearchDefinitionParameters = await dbContext.CustomSearchParameter
                .Where(csp => csp.CustomSearchDefinitionId == customSearchDefinition.CustomSearchDefinitionId)
                .ToListAsync();

            var datasetParameterValueDataList = JsonHelper.DeserializeObject<List<CustomSearchParameterValueData>>(pivotDataset.ParameterValues);

            foreach (var datasetParameterValueData in datasetParameterValueDataList)
            {
                var parameterName = datasetParameterValueData.Name;
                if (string.IsNullOrWhiteSpace(parameterName))
                {
                    parameterName = pivotDefinitionParameters.
                        Where(p => p.CustomSearchParameterId == datasetParameterValueData.Id).
                        Select(p => p.ParameterName).
                        FirstOrDefault();

                    datasetParameterValueData.Name = parameterName;
                    datasetParameterValueData.Id = null;
                }

                if (customSearchDefinitionParameters.FirstOrDefault(p => p.ParameterName.ToLower() == parameterName.ToLower()) != null)
                {
                    // We only send parameters that exist in both searches.
                    applyModelDatasetParameters.Add(datasetParameterValueData);
                }
            }

            CustomSearchesValidationHelper.AssertParameterValuesAreValid(customSearchDefinition, applyModelDatasetParameters.ToArray());
            var useModelParameter = ApplyModelService.GetUseTableTypeInputParameter(projectBusinessLogic);

            if (useModelParameter != null)
            {
                applyModelDatasetParameters.Add(useModelParameter);
            }

            var userGuidParameter = new CustomSearchParameterValueData
            {
                Name = "UserGuid",
                Value = userId.ToString(),
            };

            applyModelDatasetParameters.Add(userGuidParameter);

            if (!string.IsNullOrWhiteSpace(major) && !string.IsNullOrWhiteSpace(minor))
            {
                var majorParameter = new CustomSearchParameterValueData
                {
                    Name = "IsRecalculation",
                    Value = "True",
                };

                applyModelDatasetParameters.Add(majorParameter);
            }

            return applyModelDatasetParameters.ToArray();
        }

        /// <summary>
        /// Queues the apply model job.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>
        /// The apply user project response.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">UserProjectId should be the id of the root project.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<ExecuteCustomSearchResponse> QueueApplyModelAsync(int userProjectId, string major, string minor, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            var userProject = await dbContext.UserProject
                .Where(up => (up.UserProjectId == userProjectId))
                .Include(up => up.UserProjectDataset)
                .Include(up => up.ProjectType)
                .ThenInclude(pt => pt.ProjectTypeCustomSearchDefinition)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userProject, nameof(UserProject), userProjectId);

            this.ServiceContext.AuthProvider.AuthorizeProjectItemOperation(userProject, "ApplyModel");

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
            await projectBusinessLogic.ValidateApplyModelAsync();

            var customSearchDefinition = await this.GetApplyModelCustomSearchDefinition(userProject, dbContext);

            var applyModelDataset = await this.GetApplyModelDatasetAsync(userProject, dbContext);

            if (applyModelDataset != null)
            {
                DeleteDatasetService service = new DeleteDatasetService(this.ServiceContext);
                using (CustomSearchesDbContext deleteDbContext = this.ServiceContext.DbContextFactory.Create())
                {
                    await service.DeleteDatasetWithLockAsync(applyModelDataset.DatasetId, dbContext, blobContainer);
                }
            }

            var parameters = await ApplyModelService.GetApplyModelDatasetParametersAsync(customSearchDefinition, projectBusinessLogic, userId, major, minor, dbContext);
            string datasetName = customSearchDefinition.CustomSearchName + " (" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + ")";

            Dataset entity = new Dataset()
            {
                DatasetId = Guid.NewGuid(),
                UserId = userId,
                CustomSearchDefinitionId = customSearchDefinition.CustomSearchDefinitionId,
                ParameterValues = JsonHelper.SerializeObject(parameters),
                CreatedTimestamp = DateTime.UtcNow,
                DatasetName = datasetName,
                Comments = "Apply model dataset.",
                IsLocked = false,
                GenerateIndexesElapsedMs = 0,
                ExecuteStoreProcedureElapsedMs = 0,
                GenerateSchemaElapsedMs = 0,
                DataSetState = DatasetStateType.NotProcessed.ToString(),
                DataSetPostProcessState = DatasetPostProcessStateType.NotProcessed.ToString(),
                CreatedBy = userId,
                LastModifiedBy = userId,
                LastModifiedTimestamp = DateTime.UtcNow,
                LastExecutionTimestamp = DateTime.UtcNow
            };

            entity.GeneratedTableName = CustomSearchesDataDbContext.GetDatasetTableName(entity);

            dbContext.Dataset.Add(entity);

            UserProjectDataset newUserProjectDataset = new UserProjectDataset
            {
                UserProject = userProject,
                Dataset = entity,
                DatasetRole = ApplyModelService.ApplyModelDatasetRole,
                OwnsDataset = true,
            };

            userProject.UserProjectDataset.Add(newUserProjectDataset);

            await dbContext.ValidateAndSaveChangesAsync();

            // Returns the dataset id and job id.
            return await this.AddApplyModelToJobQueueAsync(userProject, entity, parameters, major: null, minor: null);
        }

        /// <summary>
        /// Adds the single row execution apply model to the job queue.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="projectBusinessLogic">The project business logic.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The apply user project response.
        /// </returns>
        public async Task<ExecuteCustomSearchResponse> AddSingleRowExecutionApplyModelToJobQueueAsync(
            UserProject userProject,
            DefaultProjectBusinessLogic projectBusinessLogic,
            string major,
            string minor,
            CustomSearchesDbContext dbContext)
        {
            var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetId: null);
            var pivotDatasetTableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(pivotDataset);

            string commandText =
                $"SELECT TOP(1) u.systemuserid from {pivotDatasetTableFullName} d\n" +
                $"LEFT JOIN [dynamics].[systemuser] u\n" +
                $"ON d.[{userProject.ProjectType.ApplyModelUserFilterColumnName}] = u.[internalemailaddress]\n" +
                $"WHERE [Major] = '{major}' AND [Minor] = '{minor}'\n";

            object result = await DbTransientRetryPolicy.ExecuteScalarAsync(
                this.ServiceContext,
                this.ServiceContext.DbContextFactory,
                commandText,
                parameters: null);

            string userResult = ((result == null) || (result.GetType() == typeof(System.DBNull))) ? string.Empty : result.ToString();

            Guid userId = new Guid(userResult);
            var customSearchDefinition = await this.GetApplyModelCustomSearchDefinition(userProject, dbContext);
            var applyModelDataset = await this.GetApplyModelDatasetAsync(userProject, dbContext);
            var parameters = await ApplyModelService.GetApplyModelDatasetParametersAsync(customSearchDefinition, projectBusinessLogic, userId, major, minor, dbContext);
            return await this.AddApplyModelToJobQueueAsync(userProject, applyModelDataset, parameters, major, minor, "FastQueue");
        }

        /// <summary>
        /// Adds the apply model to the job queue.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="parameters">The custom search paramaters.</param>.
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>
        /// The apply user project response.
        /// </returns>
        public async Task<ExecuteCustomSearchResponse> AddApplyModelToJobQueueAsync(
            UserProject userProject,
            Dataset dataset,
            CustomSearchParameterValueData[] parameters,
            string major,
            string minor,
            string queueName = "DatasetGeneration")
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            ApplyModelPayloadData payload = new ApplyModelPayloadData();
            payload.CustomSearchDefinitionId = dataset.CustomSearchDefinitionId;
            payload.Parameters = parameters;
            payload.UserProjectId = userProject.UserProjectId;
            payload.DatasetId = dataset.DatasetId;
            payload.SingleRowExecutionData.Major = major;
            payload.SingleRowExecutionData.Minor = minor;

            int jobId = await this.ServiceContext.AddWorkerJobQueueAsync(
                queueName,
                "ApplyModelJobType",
                userId,
                payload,
                WorkerJobTimeouts.DatasetGenerationTimeout);

            // Returns the dataset id and job id.
            return new ExecuteCustomSearchResponse
            {
                DatasetId = payload.DatasetId,
                JobId = jobId
            };
        }

        /// <summary>
        /// Gets the apply model custom search definition.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The apply model custom search definition.
        /// </returns>
        public async Task<CustomSearchDefinition> GetApplyModelCustomSearchDefinition(
            UserProject userProject,
            CustomSearchesDbContext dbContext)
        {
            var projectTypeApplyModelCustomSearchDefinition = userProject.ProjectType.ProjectTypeCustomSearchDefinition.
                Where(pc => pc.DatasetRole.Trim().ToLower() == ApplyModelService.ApplyModelDatasetRole.ToLower()).FirstOrDefault();

            InputValidationHelper.AssertEntityExists(
                projectTypeApplyModelCustomSearchDefinition,
                nameof(projectTypeApplyModelCustomSearchDefinition),
                userProject.UserProjectId,
                message: $"The project should be related to the custom search definition through the role '{ApplyModelService.ApplyModelDatasetRole}'.");

            int customSearchDefinitionId = projectTypeApplyModelCustomSearchDefinition.CustomSearchDefinitionId;

            var customSearchDefinition = await dbContext.CustomSearchDefinition
                .Where(c => (c.CustomSearchDefinitionId == customSearchDefinitionId) && (!c.IsDeleted))
                .Include(c => c.CustomSearchParameter)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(customSearchDefinition, nameof(CustomSearchDefinition), customSearchDefinitionId);

            return customSearchDefinition;
        }

        /// <summary>
        /// Gets the apply model dataset.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The apply model dataset.
        /// </returns>
        public async Task<Dataset> GetApplyModelDatasetAsync(
            UserProject userProject,
            CustomSearchesDbContext dbContext)
        {
            Dataset applyModelDataset = null;
            var userProjectApplyModelDataset = userProject.UserProjectDataset.
                FirstOrDefault(upd => upd.DatasetRole.ToLower() == ApplyModelService.ApplyModelDatasetRole.ToLower());

            if (userProjectApplyModelDataset != null)
            {
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

                applyModelDataset =
                    await (from upd in dbContext.UserProjectDataset
                          join d in dbContext.Dataset
                          on upd.DatasetId equals d.DatasetId
                          where (upd.UserProjectId == userProject.UserProjectId) &&
                              (d.UserId == userId) &&
                              (upd.DatasetRole.ToLower() == ApplyModelService.ApplyModelDatasetRole.ToLower())
                          select d).FirstOrDefaultAsync();
            }

            return applyModelDataset;
        }

        /// <summary>
        /// Applies the user project model.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>
        /// The validation warning list.
        /// </returns>
        public async Task<List<string>> ApplyModelAsync(
            ApplyModelPayloadData payload,
            Guid userId,
            CustomSearchesDbContext dbContext,
            GenericDynamicsHelper dynamicsODataHelper,
            Action<string> logAction)
        {
            var userProject = await DatasetHelper.GetOwnerProjectFromDbContextAsync(payload.DatasetId, dbContext);

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
            var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetId: null);

            string pivotDatasetPostProcessView = DatasetHelper.GetDatasetView(pivotDataset, usePostProcess: true, datasetPostProcess: null);
            var dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(
                this.ServiceContext,
                pivotDatasetPostProcessView);

            string columnsSelectScript = ApplyModelService.GetColumnsSelectScript(dbColumns, projectBusinessLogic);
            payload.TableTypeInputParameterScript = $"{columnsSelectScript}\n" +
                $"FROM {pivotDatasetPostProcessView}\n";

            if (payload.IsSingleRowExecutionMode)
            {
                payload.TableTypeInputParameterScript += $"WHERE [Major] = '{payload.SingleRowExecutionData.Major}' AND [Minor] = '{payload.SingleRowExecutionData.Minor}';\n";
                payload.ExecutionMode = DatasetExecutionMode.Refresh;
            }
            else
            {
                string email = this.ServiceContext.AuthProvider.UserInfoData.Email;
                payload.TableTypeInputParameterScript += $"WHERE [{userProject.ProjectType.ApplyModelUserFilterColumnName}] = '{email}'\n";
                payload.ExecutionMode = DatasetExecutionMode.Generate;
            }

            // We update the CustomSearchDefinitionId with the latest version.
            var latestCustomSearchDefinition =
                await CustomSearchDefinitionHelper.GetCustomSearchDefinitionLatestVersion(payload.CustomSearchDefinitionId, dbContext);
            payload.CustomSearchDefinitionId = latestCustomSearchDefinition.CustomSearchDefinitionId;

            List<string> warnings = null;
            ExecuteCustomSearchService service = new ExecuteCustomSearchService(this.ServiceContext);
            using (CustomSearchesDbContext executeCustomSearchDbContext = this.ServiceContext.DbContextFactory.Create())
            {
                warnings = await service.ExecuteCustomSearchAsync(
                    payload,
                    executeCustomSearchDbContext,
                    dynamicsODataHelper,
                    logAction);
            }

            if (!payload.IsSingleRowExecutionMode)
            {
                DatasetPostProcessData datasetPostProcessData = new DatasetPostProcessData()
                {
                    CreatedBy = userId,
                    CreatedTimestamp = DateTime.UtcNow,
                    DatasetId = payload.DatasetId,
                    IsDirty = true,
                    LastModifiedBy = userId,
                    PostProcessDefinition = "Apply Model Bulk Update",
                    PostProcessName = "Apply Model Bulk Update",
                    PostProcessRole = ApplyModelService.BulkUpdatePostProcessRole,
                    LastModifiedTimestamp = DateTime.UtcNow,
                    PostProcessType = DatasetPostProcessType.StoredProcedureUpdatePostProcess.ToString(),
                    Priority = 1000,
                };

                CustomSearchExpressionData expressiondata = new CustomSearchExpressionData()
                {
                    ExpressionRole = CustomSearchExpressionRoleType.UpdateStoredProcedure.ToString(),
                    ExpressionType = CustomSearchExpressionType.TSQL.ToString(),
                    IsAutoGenerated = true,
                    Script = userProject.ProjectType.BulkUpdateProcedureName,
                };

                datasetPostProcessData.CustomSearchExpressions = new CustomSearchExpressionData[] { expressiondata };

                var importPostProcessService = new ImportStoredProcedureUpdatePostProcessService(this.ServiceContext);
                using (CustomSearchesDbContext importPostProcessDbContext = this.ServiceContext.DbContextFactory.Create())
                {
                    try
                    {
                        await importPostProcessService.ImportStoredProcedureUpdatePostProcessAsync(datasetPostProcessData, importPostProcessDbContext);
                    }
                    catch (CustomSearchesRowsRequiredException customSearchesRowsRequiredException)
                    {
                        throw new CustomSearchesConflictException(
                            "There are no parcels assigned to the current user.  Apply Model operation requires the user to have parcels assigned.",
                            customSearchesRowsRequiredException);
                    }
                }
            }

            return warnings;
        }

        /// <summary>
        /// Gets the script for the column to use in the sql select.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="dbColumns">The db columns.</param>
        /// <returns>The script for the column to use in the sql select.</returns>
        private static string GetColumnSelectScript(string columnName, ReadOnlyCollection<DbColumn> dbColumns)
        {
            return dbColumns.FirstOrDefault(c => c.ColumnName.ToLower() == columnName.ToLower()) != null ?
                $"[{columnName}]" :
                $"NULL [{columnName}]";
        }

        /// <summary>
        /// Gets the script for the columns to use in the sql select.
        /// </summary>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="projectBusinessLogic">The project business logic.</param>
        /// <returns>The table input parameter script.</returns>
        private static string GetColumnsSelectScript(ReadOnlyCollection<DbColumn> dbColumns, DefaultProjectBusinessLogic projectBusinessLogic)
        {
            string tableTypeInputParameterScript = $"SELECT RIGHT('000000' + LTRIM(STR(Major)), 6) + RIGHT('0000' + LTRIM(STR(Minor)), 4) [ParcelId], Major, Minor";

            string[] tableTypeInputParameterNames = projectBusinessLogic.GetApplyModelTableTypeInputParameterNames();

            if (tableTypeInputParameterNames?.Length > 0)
            {
                foreach (var tableTypeInputParameterName in tableTypeInputParameterNames)
                {
                    tableTypeInputParameterScript += $", {GetColumnSelectScript(tableTypeInputParameterName, dbColumns)}";
                }
            }

            return tableTypeInputParameterScript;
        }

        /// <summary>
        /// Gets the custom search parameter that indicates if the table type should be used in the stored procedure.
        /// </summary>
        /// <param name="projectBusinessLogic">The project business logic.</param>
        /// <returns>The custom search parameter.</returns>
        private static CustomSearchParameterValueData GetUseTableTypeInputParameter(DefaultProjectBusinessLogic projectBusinessLogic)
        {
            string parameterName = projectBusinessLogic.GetApplyModelUseTableTypeInputParameterName();

            if (string.IsNullOrWhiteSpace(parameterName))
            {
                return null;
            }

            return new CustomSearchParameterValueData
            {
                Name = parameterName,
                Value = "1",
            };
        }
    }
}