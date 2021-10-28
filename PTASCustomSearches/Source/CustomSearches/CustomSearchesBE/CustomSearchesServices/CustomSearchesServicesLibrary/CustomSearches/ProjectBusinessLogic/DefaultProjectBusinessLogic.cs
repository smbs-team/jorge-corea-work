namespace CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class to manage the default project business logic.
    /// </summary>
    public class DefaultProjectBusinessLogic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProjectBusinessLogic"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public DefaultProjectBusinessLogic(CustomSearchesDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets or sets the database context.
        /// </summary>
        protected CustomSearchesDbContext DbContext { get; set; }

        /// <summary>
        /// Gets or sets the dataset used as a reference for the business logic.
        /// </summary>
        protected Dataset PivotDataset { get; set; }

        /// <summary>
        /// Gets or sets the user project.
        /// </summary>
        protected UserProject UserProject { get; set; }

        /// <summary>
        /// Gets a value indicating whether the project business logic supports the pivot dataset.
        /// </summary>
        protected virtual bool SupportsPivotDataset
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the pivot dataset role.
        /// </summary>
        protected virtual string PivotDatasetRole
        {
            get
            {
                return "Population";
            }
        }

        /// <summary>
        /// Loads the dataset used as a reference for the business logic.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <returns>
        /// The reference dataset.
        /// </returns>
        public virtual async Task<Dataset> LoadPivotDatasetAsync(Guid? datasetId)
        {
            if (this.PivotDataset == null)
            {
                if (this.SupportsPivotDataset)
                {
                    var datasets =
                        await (from up in this.DbContext.UserProject
                               join upd in this.DbContext.UserProjectDataset
                                    on up.UserProjectId equals upd.UserProjectId
                               join d in this.DbContext.Dataset
                                    on upd.DatasetId equals d.DatasetId
                               where up.UserProjectId == this.UserProject.UserProjectId
                               select d).
                           Include(d => d.DatasetPostProcess).
                           Include(d => d.UserProjectDataset).
                           ToListAsync();

                    this.PivotDataset =
                        datasets.FirstOrDefault(d => d.UserProjectDataset.FirstOrDefault(upd => upd.DatasetRole?.Trim().ToLower() == this.PivotDatasetRole.ToLower()) != null);
                }
                else
                {
                    this.PivotDataset = await this.DbContext.Dataset.
                        Where(d => d.DatasetId == datasetId).
                        Include(d => d.DatasetPostProcess).
                        Include(d => d.UserProjectDataset).
                        FirstOrDefaultAsync();
                }
            }

            return this.PivotDataset;
        }

        /// <summary>
        /// Validates the import custom modeling step post process operation.
        /// </summary>
        /// <param name="datasetPostProcessData">The post process data.</param>
        /// <param name="datasets">The datasets.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        public virtual void ValidateImportPostProcess(DatasetPostProcessData datasetPostProcessData, List<Dataset> datasets, bool bypassPostProcessBundleCheck = false)
        {
        }

        /// <summary>
        /// Validates the import custom modeling step post process operation.
        /// </summary>
        /// <param name="datasetPostProcessData">The post process data.</param>
        /// <param name="datasets">The datasets.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        public virtual void ValidateImportCustomModelingStepPostProcess(DatasetPostProcessData datasetPostProcessData, List<Dataset> datasets, bool bypassPostProcessBundleCheck)
        {
        }

        /// <summary>
        /// Validates the execute post process operation.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        public virtual void ValidateExecutePostProcess(DatasetPostProcess datasetPostProcess)
        {
        }

        /// <summary>
        /// Validates the delete post process operation.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        /// <param name="checkPostProcessStackOnPivot">Value indicating whether the stack check is over the current dataset or should be over the pivot.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task ValidateDeletePostProcessAsync(DatasetPostProcess datasetPostProcess, bool bypassPostProcessBundleCheck, bool checkPostProcessStackOnPivot)
        {
        }

        /// <summary>
        /// Indicates whether the post process uses multidataset pipeline.
        /// </summary>
        /// <param name="postProcessRole">The post process role.</param>
        /// <returns>
        /// A value to indicates whether the post process uses multidataset pipeline.
        /// </returns>
        public virtual bool UseMultiDatasetPipeline(string postProcessRole)
        {
            return false;
        }

        /// <summary>
        /// Validates the apply model operation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task ValidateApplyModelAsync()
        {
        }

        /// <summary>
        /// Gets the use table type input parameter name used in the apply model operation.
        /// </summary>
        /// <returns>
        /// The use table type input parameter name.
        /// </returns>
        public virtual string GetApplyModelUseTableTypeInputParameterName()
        {
            return null;
        }

        /// <summary>
        /// Gets the table type input parameter names used in the apply model operation.
        /// </summary>
        /// <returns>
        /// The table type input parameter names.
        /// </returns>
        public virtual string[] GetApplyModelTableTypeInputParameterNames()
        {
            return null;
        }
    }
}
