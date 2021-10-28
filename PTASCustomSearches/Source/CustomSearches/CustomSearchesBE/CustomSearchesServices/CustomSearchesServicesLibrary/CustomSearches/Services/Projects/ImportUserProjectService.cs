namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Services;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that imports a user project.
    /// </summary>
    public class ImportUserProjectService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportUserProjectService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportUserProjectService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the user project.
        /// </summary>
        /// <param name="userProjectData">The user project to import.</param>
        /// <param name="allowUpdate">Value indicating whether updates are allowed.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The id of the imported project.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset, custom search definition or parameter was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<ImportUserProjectResponse> ImportUserProjectAsync(
            ProjectData userProjectData,
            bool allowUpdate,
            CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertZero(userProjectData.UserProjectId, nameof(UserProject), nameof(userProjectData.UserProjectId));
            InputValidationHelper.AssertShouldBeGreaterThan(userProjectData.ModelArea, 0, nameof(userProjectData.ModelArea));

            if (!string.IsNullOrWhiteSpace(userProjectData.SplitModelProperty))
            {
                await this.ValidateSplitModel(userProjectData.SplitModelProperty, userProjectData.SplitModelValues, dbContext);
            }

            var existingProject = await
                (from p in dbContext.UserProject
                 where p.ProjectName == userProjectData.ProjectName
                 select p).
                 FirstOrDefaultAsync();

            if ((!allowUpdate) && (existingProject != null))
            {
                throw new CustomSearchesRequestBodyException("Project already exists.", innerException: null);
            }

            var projectTypes = await
                (from pt in dbContext.ProjectType
                 select new { Key = pt.ProjectTypeName, Value = pt.ProjectTypeId }).
                ToDictionaryAsync(e => e.Key, e => e.Value);

            List<Guid> datasetIds = new List<Guid>();
            List<int> jobIds = new List<int>();
            UserProject projectToSave = null;
            if (existingProject == null)
            {
                // Check the datasets to see if they need to be executed
                foreach (var projectDataset in userProjectData.ProjectDatasets)
                {
                    projectDataset.UserProjectId = 0;
                    projectDataset.OwnsDataset = true;

                    // If found a dataset in data then it creates a new dataset otherwise it duplicates an existing dataset.
                    if (projectDataset.Dataset != null)
                    {
                        ExecuteCustomSearchData customSearchData = new ExecuteCustomSearchData();
                        customSearchData.Parameters = projectDataset.Dataset.ParameterValues;
                        customSearchData.DatasetName = projectDataset.Dataset.DatasetName;

                        var latestCustomSearchDefinition =
                            await CustomSearchDefinitionHelper.GetCustomSearchDefinitionLatestVersion(projectDataset.Dataset.CustomSearchDefinitionId, dbContext);

                        // Need to make sure this happens before any other modification, because the data context is shared.
                        var service = new ExecuteCustomSearchService(this.ServiceContext);
                        var response = await service.QueueExecuteCustomSearchAsync(
                            latestCustomSearchDefinition.CustomSearchDefinitionId,
                            customSearchData,
                            validate: false,
                            dbContext,
                            assignFolder: false,
                            checkExecutionRoles: false);

                        // Setup the attachment to the new dataset.
                        projectDataset.Dataset = null;
                        projectDataset.DatasetId = response.DatasetId;
                        datasetIds.Add(response.DatasetId);
                        jobIds.Add(response.JobId);
                    }
                    else
                    {
                        var service = new DuplicateDatasetService(this.ServiceContext);

                        // In this case Duplicate method should receive an user project with the AssessmentYear that is used for the authorization.
                        UserProject project = new UserProject() { AssessmentYear = DateTime.UtcNow.Year };
                        ExecuteCustomSearchResponse response = await service.DuplicateDatasetAsync(
                            projectDataset.DatasetId,
                            project,
                            applyRowFilter: false,
                            applyUserSelection: false,
                            duplicateDatasetData: null,
                            duplicatePostProcess: true,
                            needsPostProcessExecution: true,
                            dbContext);
                        projectDataset.DatasetId = response.DatasetId;
                        datasetIds.Add(response.DatasetId);
                        jobIds.Add(response.JobId);
                    }
                }

                var newProject = userProjectData.ToEfModel(projectTypes);
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
                newProject.UserId = userId;
                newProject.LastModifiedBy = userId;
                newProject.LastModifiedTimestamp = DateTime.Now;
                newProject.CreatedBy = newProject.LastModifiedBy;
                newProject.CreatedTimestamp = newProject.LastModifiedTimestamp;
                newProject.VersionType = UserProjectVersionType.Draft.ToString();

                projectToSave = newProject;

                // Add new project if it was not found.
                dbContext.UserProject.Add(newProject);
            }
            else
            {
                // Update project if it was found.  Only comments can be updated
                projectToSave = existingProject;
                projectToSave.Comments = userProjectData.Comments;
            }

            await dbContext.ValidateAndSaveChangesAsync();
            return new ImportUserProjectResponse(projectToSave.UserProjectId, datasetIds.ToArray(), jobIds.ToArray());
        }

        private async Task ValidateSplitModel(string splitModelProperty, string[] splitModelValues, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertArrayNotEmpty(
                splitModelValues,
                nameof(splitModelValues),
                "If SplitModelProperty is provided, SplitModelValues can't be empty");

            var metadataService = new GetMetadataStoreItemsService(this.ServiceContext);
            var metadataResponse = await metadataService.GetMetadataStoreItemAsync(
                GetMetadataStoreItemsService.GlobalConstantStoreType,
                "SplitModelValues",
                latestVersion: true,
                dbContext);

            var splitModelProperties = (JArray)metadataResponse.MetadataStoreItems[0].Value;

            // We don't check values for SubArea since they are not part of the metadata. Should check from dynamics schema?
            if (splitModelProperty == "SubArea")
            {
                return;
            }

            JArray propertyValues =
                (from p in splitModelProperties
                where p["SplitModelProperty"].Value<string>() == splitModelProperty
                select p["SplitModelValues"].Value<JArray>()).FirstOrDefault();

            if (propertyValues == null)
            {
                throw new CustomSearchesRequestBodyException(
                    $"Split model property: '{splitModelProperty}' not supported",
                    innerException: null);
            }

            foreach (var splitModelValue in splitModelValues)
            {
                var metadataValue = (from v in propertyValues where v.Value<string>() == splitModelValue select v).FirstOrDefault();
                if (metadataValue == null)
                {
                    throw new CustomSearchesRequestBodyException(
                      $"Split model property value: '{splitModelValue}' not supported",
                      innerException: null);
                }
            }
        }
    }
}
