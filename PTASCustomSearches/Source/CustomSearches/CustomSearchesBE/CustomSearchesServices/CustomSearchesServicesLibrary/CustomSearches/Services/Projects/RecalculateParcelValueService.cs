namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that recalculates the parcel value.
    /// </summary>
    public class RecalculateParcelValueService : BaseService
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
        /// Initializes a new instance of the <see cref="RecalculateParcelValueService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public RecalculateParcelValueService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Recalculates the parcel value.
        /// </summary>
        /// <param name="recalculateParcelValueData">The recalculate parcel value data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// An error message if the parcel recalculation fails otherwise null.
        /// </returns>
        public async Task<string> RecalculateParcelValueAsync(RecalculateParcelValueData recalculateParcelValueData, CustomSearchesDbContext dbContext)
        {
            string areaNumberScript =
                $"SELECT TOP(1) pa.[ptas_areanumber] FROM [dynamics].[ptas_parceldetail] ppd\n" +
                $"LEFT JOIN [dynamics].[ptas_area] pa\n" +
                $"ON ppd.[_ptas_areaid_value] = pa.[ptas_areaid]\n" +
                $"WHERE ppd.[ptas_major] = '{recalculateParcelValueData.Major}' AND ppd.[ptas_minor] = '{recalculateParcelValueData.Minor}'\n";

            object areaNumberResult = await DbTransientRetryPolicy.ExecuteScalarAsync(
                this.ServiceContext,
                this.ServiceContext.DbContextFactory,
                areaNumberScript,
                parameters: null);
            string areaNumberText = ((areaNumberResult == null) || (areaNumberResult.GetType() == typeof(System.DBNull))) ? string.Empty : areaNumberResult.ToString();

            if (string.IsNullOrWhiteSpace(areaNumberText))
            {
                throw new CustomSearchesRequestBodyException(
                    $"AreaNumber for Major '{recalculateParcelValueData.Major}' and Minor '{recalculateParcelValueData.Minor}' was not found.",
                    innerException: null);
            }

            int areaNumber = int.Parse(areaNumberText);

            var versionType = recalculateParcelValueData.IsWhatIf ? UserProjectVersionType.WhatIf : UserProjectVersionType.Adjustments;

            var lastProjects =
                await (from up in dbContext.UserProject
                       join pt in dbContext.ProjectType
                            on up.ProjectTypeId equals pt.ProjectTypeId
                       where up.ModelArea == areaNumber &&
                            up.IsFrozen &&
                            up.VersionType == versionType.ToString() &&
                            (pt.ProjectTypeName.ToLower() == "annual update" || pt.ProjectTypeName.ToLower() == "physical inspection")
                       orderby up.AssessmentYear descending
                       select up)
                       .Take(10)
                       .Include(up => up.ProjectType)
                       .ToListAsync();

            lastProjects.Reverse();

            var physicalInspection = lastProjects.LastOrDefault(up => up.ProjectType.ProjectTypeName.ToLower() == "physical inspection");
            lastProjects.RemoveRange(0, lastProjects.IndexOf(physicalInspection));

            foreach (var userProject in lastProjects)
            {
                if (recalculateParcelValueData.SelectedYears?.Length > 0 && recalculateParcelValueData.SelectedYears.Contains(userProject.AssessmentYear) == false)
                {
                    continue;
                }

                string errorMessage = await this.RecalculateParcelValueInUserProjectAsync(recalculateParcelValueData, userProject.UserProjectId, dbContext);

                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    // TODO: Instead should wait until the update reaches the report database.
                    await Task.Delay(10000);
                }
                else
                {
                    return errorMessage;
                }
            }

            return null;
        }

        /// <summary>
        /// Recalculates the parcel value.
        /// </summary>
        /// <param name="recalculateParcelValueData">The recalculate parcel valu data.</param>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// An error message if the parcel recalculation fails otherwise null.
        /// </returns>
        public async Task<string> RecalculateParcelValueInUserProjectAsync(RecalculateParcelValueData recalculateParcelValueData, int userProjectId, CustomSearchesDbContext dbContext)
        {
            var userProject = await dbContext.UserProject
                .Where(up => up.UserProjectId == userProjectId)
                .Include(up => up.UserProjectDataset)
                .Include(up => up.ProjectType)
                .ThenInclude(pt => pt.ProjectTypeCustomSearchDefinition)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userProject, nameof(userProject), userProjectId);

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);

            var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetId: null);

            RefreshDatasetService refreshDatasetService = new RefreshDatasetService(this.ServiceContext);
            var refreshDatasetIdResult = await refreshDatasetService.AddRefreshDatasetToJobQueueAsync(
                pivotDataset, recalculateParcelValueData.Major, recalculateParcelValueData.Minor, dbContext, needsPostProcessExecution: true, "FastQueue");

            var refreshDatasetJobResult = await this.ServiceContext.WaitForJobResultAsync((int)refreshDatasetIdResult.Id);
            var failedJobResult = JsonHelper.DeserializeObject<FailedJobResult>(refreshDatasetJobResult);

            if (failedJobResult?.Status?.ToLower() == "failed")
            {
                return "Failed updating the parcel data.";
            }

            ApplyModelService applyModelService = new ApplyModelService(this.ServiceContext);
            var applyModelResult = await applyModelService.AddSingleRowExecutionApplyModelToJobQueueAsync(
                userProject,
                projectBusinessLogic,
                recalculateParcelValueData.Major,
                recalculateParcelValueData.Minor,
                dbContext);

            var applyModelJobResult = await this.ServiceContext.WaitForJobResultAsync(applyModelResult.JobId);
            failedJobResult = JsonHelper.DeserializeObject<FailedJobResult>(applyModelJobResult);

            if (failedJobResult?.Status?.ToLower() == "failed")
            {
                return "Failed applying the model to the parcel data.";
            }

            BulkUpdateService bulkUpdateService = new BulkUpdateService(this.ServiceContext);
            var idResult = await bulkUpdateService.QueueBulkUpdateAsync(applyModelResult.DatasetId, recalculateParcelValueData.Major, recalculateParcelValueData.Minor, dbContext);

            var bulkUpdateJobResult = await this.ServiceContext.WaitForJobResultAsync((int)idResult.Id);
            failedJobResult = JsonHelper.DeserializeObject<FailedJobResult>(bulkUpdateJobResult);

            if (failedJobResult?.Status?.ToLower() == "failed")
            {
                return "Failed applying the bulk update to the parcel data.";
            }

            return null;
        }
    }
}