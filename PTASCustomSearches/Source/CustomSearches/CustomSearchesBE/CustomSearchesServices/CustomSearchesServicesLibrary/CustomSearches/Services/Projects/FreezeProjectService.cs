namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Executor;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that freezes a user project.
    /// </summary>
    public class FreezeProjectService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreezeProjectService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public FreezeProjectService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Freezes a user project.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The freeze project response.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't be execute because the datasets of the project are still being generated.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Cannot get the column schema or create the dataset data view.</exception>
        public async Task<FreezeProjectResponse> FreezeProjectAsync(int userProjectId, CustomSearchesDbContext dbContext)
        {
            UserProject userProject = await dbContext.UserProject
                .Where(p => p.UserProjectId == userProjectId)
                .Include(p => p.UserProjectDataset)
                .ThenInclude(ups => ups.Dataset)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userProject, "UserProject", userProjectId);

            this.ServiceContext.AuthProvider.AuthorizeProjectItemOperation(userProject, "FreezeProject");

            var service = new CreateNewProjectVersionService(this.ServiceContext);

            UserProjectGenerationPayloadData adjustmentsProjectPayload;
            UserProjectGenerationPayloadData whatIfProjectPayload;

            using (CustomSearchesDbContext customSearchesDbContext = this.ServiceContext.DbContextFactory.Create())
            {
                adjustmentsProjectPayload = await service.PrepareCreateNewProjectVersionAsync(userProjectId, UserProjectVersionType.Adjustments, customSearchesDbContext);
            }

            using (CustomSearchesDbContext customSearchesDbContext = this.ServiceContext.DbContextFactory.Create())
            {
                whatIfProjectPayload = await service.PrepareCreateNewProjectVersionAsync(userProjectId, UserProjectVersionType.WhatIf, customSearchesDbContext);
            }

            userProject.VersionType = UserProjectVersionType.Frozen.ToString();

            var sourceApplyModelDatasets =
                userProject.UserProjectDataset
                    .Where(upd => upd.DatasetRole?.Trim().ToLower() == ApplyModelService.ApplyModelDatasetRole.ToLower())
                    .Select(upd => upd.Dataset)
                    .ToDictionary(d => d.DatasetId, d => d);

            var newTargetIdSourceApplyModelDatasets = new Dictionary<Guid, Dataset>();

            for (int i = 0; i < adjustmentsProjectPayload.NewDatasets.Length; i++)
            {
                if (sourceApplyModelDatasets.ContainsKey(adjustmentsProjectPayload.SourceDatasets[i]))
                {
                    newTargetIdSourceApplyModelDatasets.Add(adjustmentsProjectPayload.NewDatasets[i], sourceApplyModelDatasets[adjustmentsProjectPayload.SourceDatasets[i]]);
                }
            }

            for (int i = 0; i < whatIfProjectPayload.NewDatasets.Length; i++)
            {
                if (sourceApplyModelDatasets.ContainsKey(whatIfProjectPayload.SourceDatasets[i]))
                {
                    newTargetIdSourceApplyModelDatasets.Add(whatIfProjectPayload.NewDatasets[i], sourceApplyModelDatasets[whatIfProjectPayload.SourceDatasets[i]]);
                }
            }

            int? rootProjectId = userProject.RootVersionUserProjectId == null ? userProject.UserProjectId : userProject.RootVersionUserProjectId;

            var userProjects = await dbContext.UserProject
                    .Where(p => p.RootVersionUserProjectId == rootProjectId || p.UserProjectId == rootProjectId)
                    .Include(p => p.UserProjectDataset)
                    .ToArrayAsync();

            List<Guid> newApplyModelDatasetIds = new List<Guid>();
            foreach (var currentProject in userProjects)
            {
                currentProject.IsFrozen = true;
                currentProject.IsLocked = true;

                if (currentProject.VersionType == UserProjectVersionType.Adjustments.ToString() ||
                    currentProject.VersionType == UserProjectVersionType.WhatIf.ToString())
                {
                    newApplyModelDatasetIds.AddRange(
                        currentProject.UserProjectDataset
                            .Where(upd => upd.DatasetRole?.Trim().ToLower() == ApplyModelService.ApplyModelDatasetRole.ToLower())
                            .Select(upd => upd.DatasetId));
                }
            }

            var newApplyModelDatasets = await DatasetHelper.LoadDatasetsWithDependenciesAsync(
                dbContext,
                newApplyModelDatasetIds.ToArray(),
                includeRelatedExpressions: true,
                includeParentFolder: false,
                includeInverseSourceDatasets: false,
                includeUserProject: false,
                includeDatasetUserClientState: false);

            // Loads source apply model datasets and post processes.
            await DatasetHelper.LoadDatasetsWithDependenciesAsync(
                dbContext,
                newTargetIdSourceApplyModelDatasets.Values.Select(d => d.DatasetId).ToArray(),
                includeRelatedExpressions: false,
                includeParentFolder: false,
                includeInverseSourceDatasets: false,
                includeUserProject: false,
                includeDatasetUserClientState: false);

            foreach (var applyModelDataset in newApplyModelDatasets)
            {
                if (!newTargetIdSourceApplyModelDatasets.ContainsKey(applyModelDataset.DatasetId))
                {
                    continue;
                }

                applyModelDataset.DataSetState = DatasetStateType.Processed.ToString();
                applyModelDataset.DataSetPostProcessState = DatasetPostProcessStateType.Processed.ToString();
                foreach (var datasetPostProcess in applyModelDataset.DatasetPostProcess)
                {
                    datasetPostProcess.IsDirty = false;
                    SucceededJobResult succeededJobResult = new SucceededJobResult
                    {
                        Status = "Success"
                    };

                    datasetPostProcess.ResultPayload = JsonHelper.SerializeObject(succeededJobResult);

                    string targetTableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(datasetPostProcess);

                    var sourceDatasetPostProcess =
                        newTargetIdSourceApplyModelDatasets[applyModelDataset.DatasetId].DatasetPostProcess
                        .FirstOrDefault(dpp => dpp.PostProcessRole == datasetPostProcess.PostProcessRole);

                    string sourceTableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(sourceDatasetPostProcess);

                    var columnNames = new List<string> { "CustomSearchResultId" };
                    columnNames.AddRange(datasetPostProcess.CustomSearchExpression
                        .Where(e => e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())
                        .Select(e => e.ColumnName));

                    var columnsScript = string.Join(", ", columnNames.Select(cn => $"[{cn}]"));

                    var script = StoredProcedureUpdatePostProcessExecutor.GetCreateTableScript(datasetPostProcess);

                    script += $"INSERT INTO {targetTableFullName} ({columnsScript}) SELECT {columnsScript} FROM {sourceTableFullName} \n";

                    await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        script,
                        parameters: null,
                        $"Error inserting data into the table: '{targetTableFullName}'.");
                }
            }

            await dbContext.SaveChangesAsync();

            ImportUserProjectResponse adjustmentsProjectResponse;
            ImportUserProjectResponse whatIfProjectResponse;
            using (CustomSearchesDbContext customSearchesDbContext = this.ServiceContext.DbContextFactory.Create())
            {
                adjustmentsProjectResponse = await service.QueueCreateNewProjectVersionAsync(adjustmentsProjectPayload);
            }

            using (CustomSearchesDbContext customSearchesDbContext = this.ServiceContext.DbContextFactory.Create())
            {
                whatIfProjectResponse = await service.QueueCreateNewProjectVersionAsync(whatIfProjectPayload);
            }

            return new FreezeProjectResponse(
                adjustmentsProjectResponse.QueuedDatasets.Concat(whatIfProjectResponse.QueuedDatasets).ToArray(),
                adjustmentsProjectResponse.JobIds.Concat(whatIfProjectResponse.JobIds).ToArray());
        }
    }
}
