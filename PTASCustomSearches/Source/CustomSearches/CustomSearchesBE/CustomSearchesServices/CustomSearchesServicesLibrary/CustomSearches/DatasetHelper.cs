namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;

    /// <summary>
    /// Dataset state helper.
    /// </summary>
    public class DatasetHelper
    {
        /// <summary>
        /// Error message for dataset failed.
        /// </summary>
        public const string DatasetFailedErrorMessage = "Can't execute this service because the dataset '{0}' failed to generate or refresh.";

        /// <summary>
        /// The filter state column name.
        /// </summary>
        public static readonly string FilterStateColumnName = "FilterState";

        /// <summary>
        /// The selection column name.
        /// </summary>
        public static readonly string SelectionColumnName = "Selection";

        /// <summary>
        /// The default not editable column names.
        /// </summary>
        public static readonly string[] DefaultNotEditableColumnNames =
            {
                "RowNum", "ErrorMessage", "ExportedToBackEndErrorMessage", "BackendExportState", "IsValid", "Validated", "CustomSearchResultId"
            };

        /// <summary>
        /// Takes a column value and returns the exact string value for that column.
        /// That is, avoiding exponent for numeric values and using 29 digits of precision.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="normalizeNumbers">if set to <c>true</c> numbers will be normalized (G29 format), even if they come a as a string.</param>
        /// <returns>
        /// String representing the column value.  If value is Null or DbNull, '"System.DBNull' is returned.  Null is returned if the conversion was not possible.
        /// </returns>
        public static string ColumnValueToString(object value, bool normalizeNumbers)
        {
            if ((value == null) || value.GetType() == typeof(System.DBNull))
            {
                return "System.DBNull";
            }

            if (Information.IsNumeric(value) && normalizeNumbers)
            {
                try
                {
                    decimal decimalValue = System.Convert.ToDecimal(value);
                    return decimalValue.ToString("G29");
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Determines whether the passed column name is one of the default non editable columns for the dataset.
        /// </summary>
        /// <param name="columName">Name of the column.</param>
        /// <returns>
        ///   <c>true</c> if [is default not editable column] [the specified column name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDefaultNotEditableColumn(string columName)
        {
            return
                (from nec in DatasetHelper.DefaultNotEditableColumnNames
                 where nec.ToLower() == columName.ToLower()
                 select nec).
                 FirstOrDefault() != null;
        }

        /// <summary>
        /// Determines whether the passed column name is the selection or filter column for the dataset.
        /// </summary>
        /// <param name="columName">Name of the column.</param>
        /// <returns>
        ///   <c>true</c> if [is the selection or filter column] [the specified column name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSelectionOrFilterColumn(string columName)
        {
            string name = columName.ToLower();
            return name == SelectionColumnName.ToLower() || name == FilterStateColumnName.ToLower();
        }

        /// <summary>
        /// Determines whether the passed column name is the selection or filter column for the dataset.
        /// </summary>
        /// <param name="columName">Name of the column.</param>
        /// <returns>
        ///   <c>true</c> if [is the selection or filter column] [the specified column name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSelectionColumn(string columName)
        {
            string name = columName.ToLower();
            return name == SelectionColumnName.ToLower();
        }

        /// <summary>
        /// Gets the editable columns for the dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The list of editable columns.</returns>
        public static async Task<List<CustomSearchColumnDefinition>> GetEditableColumnsAsync(Dataset dataset, CustomSearchesDbContext dbContext)
        {
            return await (from ec in dbContext.CustomSearchColumnDefinition
                          where ec.CustomSearchDefinitionId == dataset.CustomSearchDefinitionId && ec.IsEditable == true
                          select ec).ToListAsync();
        }

        /// <summary>
        /// Tests if it is possible to lock the dataset for altering.  Returns true if it is possible to lock. False otherwise.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="newState">The new state.</param>
        /// <param name="newPostProcessState">New state of the post process.</param>
        /// <param name="isRootLock">if set to <c>true</c> [is root lock].</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="lockingJobId">The locking job identifier.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The locked dataset.
        /// </returns>
        public static async Task<bool> TestAlterDatasetLockAsync(
            Guid datasetId,
            string newState,
            string newPostProcessState,
            bool isRootLock,
            Guid userId,
            int? lockingJobId,
            CustomSearchesDbContext dbContext)
        {
            int resolvedJobId = lockingJobId.HasValue ? lockingJobId.Value : -1;
            Dataset dataset = await dbContext.TestAlterDatasetLockAsync(datasetId, newState, newPostProcessState, isRootLock, userId, resolvedJobId);

            if (dataset == null)
            {
                dataset = await dbContext.Dataset.Where(d => d.DatasetId == datasetId).FirstOrDefaultAsync();
                throw new CustomSearchesConflictException(
                    $"Can't execute this service while the dataset '{datasetId}' is locked in the database." +
                    $" Please retry the operation after a short period of time. If the problem persists contact your administrator.",
                    null,
                    dataset);
            }

            DatasetHelper.AssertNotFailedDataset(dataset);

            return true;
        }

        /// <summary>
        /// Gets the lock to alter dataset.
        /// </summary>
        /// <param name="lambda">The lambda to execute.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="isRootLock">if set to <c>true</c> [is root lock].</param>
        /// <param name="processingDatasetState">State of the processing dataset.</param>
        /// <param name="processingDatasetPostProcessState">State of the processing dataset post process.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="lockingJobId">The locking job id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="allowFailed">If set to allows locks on failed datasets.</param>
        /// <param name="skipAlterDatasetLock">If set to skips the alter dataset lock.</param>
        /// <exception cref="CustomSearchesConflictException">Can't execute this function while dataset state is '{dataset.DataSetState}' and post process state is '{dataset.DataSetPostProcessState}'.</exception>
        /// <returns>The async task.</returns>
        public static async Task GetAlterDatasetLockAsyncV2(
            Func<string, string, Task<(string, string)>> lambda,
            Dataset dataset,
            bool isRootLock,
            string processingDatasetState,
            string processingDatasetPostProcessState,
            Guid userId,
            int? lockingJobId,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext,
            bool allowFailed = false,
            bool skipAlterDatasetLock = false)
        {
            if ((dataset.SourceDatasetId != null) && isRootLock)
            {
                throw new CustomSearchesConflictException(
                    $"Can't root lock a dataset when it is not root.",
                    null);
            }

            /*if (!allowFailed)
            {
                DatasetHelper.AssertNotFailedDataset(dataset);
            }*/

            if (skipAlterDatasetLock)
            {
                await lambda(dataset.DataSetState, dataset.DataSetPostProcessState);
                return;
            }

            int resolvedJobId = lockingJobId.HasValue ? lockingJobId.Value : -1;
            var toReturn = await dbContext.GetAlterDatasetLockAsyncV2(
                dataset.DatasetId,
                processingDatasetState,
                processingDatasetPostProcessState,
                isRootLock,
                userId,
                resolvedJobId);

            if (toReturn == null)
            {
                throw new CustomSearchesConflictException(
                    $"Can't execute this service while the dataset '{dataset.DatasetId}' is locked in the database." +
                    $" Please retry the operation after a short period of time. If the problem persists contact your administrator.",
                    null,
                    dataset);
            }

            string datasetPostProcessState = toReturn.DataSetPostProcessState;
            string datasetState = toReturn.DataSetState;

            try
            {
                await dbContext.Entry<Dataset>(dataset).ReloadAsync();

                (string state, string postProcessState) newState = await lambda(datasetState, datasetPostProcessState);
                if (!string.IsNullOrWhiteSpace(newState.state))
                {
                    datasetState = newState.state;
                }

                if (!string.IsNullOrWhiteSpace(newState.postProcessState))
                {
                    datasetPostProcessState = newState.postProcessState;
                }
            }
            catch
            {
                datasetPostProcessState = await DatasetHelper.CalculateDatasetPostProcessStateAsync(dataset.DatasetId, serviceContext);
                throw;
            }
            finally
            {
                await DatasetHelper.ReleaseAlterDatasetLockAsyncV2(
                    dataset,
                    datasetState,
                    datasetPostProcessState,
                    userId,
                    dbContext);
            }
        }

        /// <summary>
        /// Releases the lock to alter dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcessState">The dataset post process state type.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public static async Task ReleaseAlterDatasetLockAsync(Dataset dataset, string datasetPostProcessState, Guid userId, CustomSearchesDbContext dbContext)
        {
            int retry = 0;
            while (retry < 3)
            {
                try
                {
                    if (retry > 0)
                    {
                        await Task.Delay(10000 * retry);
                    }

                    await dbContext.ReleaseAlterDatasetLockAsync(dataset.DatasetId, userId, datasetPostProcessState);
                    break;
                }
                catch (Exception ex)
                {
                    retry++;
                }
            }
        }

        /// <summary>
        /// Releases the lock to alter dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetState">The dataset state.</param>
        /// <param name="datasetPostProcessState">The dataset post process state type.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public static async Task ReleaseAlterDatasetLockAsyncV2(
            Dataset dataset,
            string datasetState,
            string datasetPostProcessState,
            Guid userId,
            CustomSearchesDbContext dbContext)
        {
            int retry = 0;
            while (retry < 3)
            {
                try
                {
                    if (retry > 0)
                    {
                        await Task.Delay(10000 * retry);
                    }

                    await dbContext.ReleaseAlterDatasetLockAsyncV2(dataset.DatasetId, userId, datasetState, datasetPostProcessState);
                    break;
                }
                catch (Exception ex)
                {
                    retry++;
                }
            }
        }

        /// <summary>
        /// Verifies if it is allowed to read from dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <returns>
        /// True if can read from dataset, otherwise false.
        /// </returns>
        public static bool CanReadFromDataset(Dataset dataset, bool usePostProcess)
        {
            DatasetStateType datasetState = Enum.Parse<DatasetStateType>(dataset.DataSetState);

            if ((datasetState == DatasetStateType.GeneratingIndexes) || (datasetState == DatasetStateType.Processed))
            {
                DatasetPostProcessStateType datasetPostProcessStateType = Enum.Parse<DatasetPostProcessStateType>(dataset.DataSetPostProcessState);
                if ((!usePostProcess) || (datasetPostProcessStateType != DatasetPostProcessStateType.Processing))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Asserts that it is allowed to read from dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <exception cref="CustomSearchesConflictException">Can't execute this function while dataset state is '{dataset.DataSetState}' and post process state is '{dataset.DataSetPostProcessState}'.</exception>
        public static void AssertCanReadFromDataset(Dataset dataset, bool usePostProcess)
        {
            if (!DatasetHelper.CanReadFromDataset(dataset, usePostProcess))
            {
                DatasetHelper.AssertNotFailedDataset(dataset);

                throw new CustomSearchesConflictException(
                    $"The dataset '{dataset.DatasetId}' is currently executing a process that makes reading data from it unreliable" +
                    $" Please retry the operation after a short period of time. If the problem persists contact your administrator.",
                    innerException: null,
                    dataset: dataset);
            }
        }

        /// <summary>
        /// Asserts that the dataset has not failed.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <exception cref="CustomSearchesConflictException">Can't execute this service because the dataset failed to generate or refresh'.</exception>
        public static void AssertNotFailedDataset(Dataset dataset)
        {
            if (dataset.DataSetState == DatasetStateType.Failed.ToString())
            {
                throw new CustomSearchesConflictException(
                    string.Format(DatasetHelper.DatasetFailedErrorMessage, dataset.DatasetId),
                    innerException: null,
                    dataset);
            }
        }

        /// <summary>
        /// Asserts that the dataset name is unique in the user project.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="datasetName">The dataset name.</param>
        /// <param name="dbContext">The database context.</param>
        /// <exception cref="CustomSearchesConflictException">The user project already has a dataset with the same name.</exception>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public static async Task AssertUniqueUserProjectDatasetName(int userProjectId, string datasetName, CustomSearchesDbContext dbContext)
        {
            if (userProjectId > 0)
            {
                InputValidationHelper.AssertNotEmpty(datasetName, nameof(datasetName));

                bool repeatedName =
                    (await (from up in dbContext.UserProject
                            join upd in dbContext.UserProjectDataset
                                     on up.UserProjectId equals upd.UserProjectId
                            join d in dbContext.Dataset
                                     on upd.DatasetId equals d.DatasetId
                            where upd.UserProjectId == userProjectId && d.DatasetName.Trim().ToLower() == datasetName.Trim().ToLower()
                            select d.DatasetName).CountAsync()) > 0;

                if (repeatedName)
                {
                    throw new CustomSearchesConflictException(
                        $"The user project already has a dataset with the same name '{datasetName}'.",
                        null);
                }
            }
        }

        /// <summary>
        /// Verifies if can use post process.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// True if can read from dataset, otherwise false.
        /// </returns>
        public static async Task<bool> CanUsePostProcessAsync(Dataset dataset, DatasetPostProcess datasetPostProcess, bool usePostProcess, CustomSearchesDbContext dbContext)
        {
            if (usePostProcess)
            {
                if (datasetPostProcess != null)
                {
                    if (datasetPostProcess.LastExecutionTimestamp == null)
                    {
                        return false;
                    }
                }
                else
                {
                    if (dataset.DataSetPostProcessState == DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString())
                    {
                        var result = await (from d in dbContext.DatasetPostProcess
                                            where d.DatasetId == dataset.DatasetId && d.LastExecutionTimestamp != null
                                            select d).FirstOrDefaultAsync();
                        return result != null;
                    }
                    else
                    {
                        return dataset.DataSetPostProcessState != DatasetPostProcessStateType.NotProcessed.ToString();
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Asserts that it is allowed to use the post process.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <exception cref="CustomSearchesConflictException">Can't get columns of the post process with status failed.'.</exception>
        public static void AssertCanUsePostProcess(DatasetPostProcess datasetPostProcess, bool usePostProcess)
        {
            if (usePostProcess && (datasetPostProcess != null) && (datasetPostProcess.DatasetPostProcessId > 0))
            {
                var failedJobResult = JsonHelper.DeserializeObject<FailedJobResult>(datasetPostProcess.ResultPayload);
                if (failedJobResult == null || failedJobResult?.Status.ToLower() != "success")
                {
                    throw new CustomSearchesConflictException(
                        $"Can't use the post process {datasetPostProcess.DatasetPostProcessId} with status different of success.",
                        null);
                }
            }
        }

        /// <summary>
        /// Calculates the dataset post process state.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>The dataset post process state.</returns>
        public static async Task<string> CalculateDatasetPostProcessStateAsync(Guid datasetId, IServiceContext serviceContext)
        {
            using (CustomSearchesDbContext dbContext = serviceContext.DbContextFactory.Create())
            {
                // We just read the post process that is required to figure out the state.
                var dirtyOrFirstPostProcess =
                    await (from d in dbContext.Dataset
                           join dpp in dbContext.DatasetPostProcess
                                on d.DatasetId equals dpp.DatasetId
                           where dpp.DatasetId == datasetId
                           orderby dpp.IsDirty descending
                           select dpp).FirstOrDefaultAsync();

                var dppList = new List<DatasetPostProcess>();
                if (dirtyOrFirstPostProcess != null)
                {
                    dppList.Add(dirtyOrFirstPostProcess);
                }

                return DatasetHelper.CalculateDatasetPostProcessState(dppList);
            }
        }

        /// <summary>
        /// Calculates the dataset post process state.
        /// </summary>
        /// <param name="datasetPostProcesses">The dataset post processes.</param>
        /// <returns>The dataset post process state.</returns>
        public static string CalculateDatasetPostProcessState(ICollection<DatasetPostProcess> datasetPostProcesses)
        {
            var dirtyOrFirstPostProcess = datasetPostProcesses.OrderByDescending(dpp => dpp.IsDirty).FirstOrDefault();

            if (dirtyOrFirstPostProcess == null)
            {
                return DatasetPostProcessStateType.NotProcessed.ToString();
            }

            return dirtyOrFirstPostProcess.IsDirty ?
                DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString() :
                DatasetPostProcessStateType.Processed.ToString();
        }

        /// <summary>
        /// Gets the dataset view.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>The dataset view.</returns>
        public static string GetDatasetView(Dataset dataset, bool usePostProcess, DatasetPostProcess datasetPostProcess)
        {
            if (datasetPostProcess == null)
            {
                return CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, usePostProcess);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(datasetPostProcess.CalculatedView))
                {
                    throw new CustomSearchesConflictException(
                        $"DatasetPostProcess with id '{datasetPostProcess.DatasetPostProcessId}' needs to be executed.",
                        null);
                }

                return $"({datasetPostProcess.CalculatedView}) PostProcessView{datasetPostProcess.DatasetPostProcessId}";
            }
        }

        /// <summary>
        /// Gets a value indicating whether it is an user dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>True if it is an user dataset. False if it is a reference dataset or the dataset belongs to a user project.</returns>
        public static bool IsUserDataset(Dataset dataset)
        {
            return dataset.FolderId != null;
        }

        /// <summary>
        /// Gets a value indicating whether it is a reference dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>True if it is a reference dataset, otherwise false.</returns>
        public static bool IsReferenceDataset(Dataset dataset)
        {
            return dataset.SourceDatasetId != null;
        }

        /// <summary>
        /// Gets the source dataset id.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>Returns the SourceDatasetId if it is a reference dataset, otherwise returns the DatasetId.</returns>
        public static Guid GetSourceDatasetId(Dataset dataset)
        {
            return dataset.SourceDatasetId != null ? (Guid)dataset.SourceDatasetId : dataset.DatasetId;
        }

        /// <summary>
        /// Updates a not tracked dataset to be used as reference dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public static void UpdateNotTrackedDatasetToReference(ref Dataset dataset)
        {
            Guid sourceDatasetId = DatasetHelper.GetSourceDatasetId(dataset);
            dataset.DatasetId = Guid.NewGuid();
            dataset.SourceDatasetId = sourceDatasetId;
            dataset.IsLocked = false;

            dataset.DataSetState = DatasetStateType.Processed.ToString();
            dataset.DataSetPostProcessState = DatasetPostProcessStateType.NotProcessed.ToString();
            dataset.FolderId = null;
        }

        /// <summary>
        /// Gets the cached owner project from the dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The user project.</returns>
        public static UserProject GetCachedOwnerProject(Dataset dataset)
        {
            return dataset.UserProjectDataset
                .Where(pd => (pd.DatasetId == dataset.DatasetId) && pd.OwnsDataset)
                .Select(pd => pd.UserProject)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the owner project from the database context.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The user project.</returns>
        public static async Task<UserProject> GetOwnerProjectFromDbContextAsync(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            return await (from up in dbContext.UserProject
                          join upd in dbContext.UserProjectDataset
                               on up.UserProjectId equals upd.UserProjectId
                          join d in dbContext.Dataset
                               on upd.DatasetId equals d.DatasetId
                          where upd.DatasetId == datasetId && upd.OwnsDataset
                          select up).
                    Include(up => up.ProjectType).
                    Include(up => up.UserProjectDataset).
                    FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the user project that owns the dataset.
        /// Looks first if the project is already cached in the entity, otherwise loads it from the DB.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The user project.</returns>
        public static async Task<UserProject> GetOwnerProjectAsync(Dataset dataset, CustomSearchesDbContext dbContext)
        {
            // First look in the cached information.
            var project = DatasetHelper.GetCachedOwnerProject(dataset);

            if (project == null)
            {
                project = await GetOwnerProjectFromDbContextAsync(dataset.DatasetId, dbContext);
            }

            return project;
        }

        /// <summary>
        /// Gets the user project that owns the dataset.
        /// Looks first if the project is already cached in the entity, otherwise loads it from the DB.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>The user project.</returns>
        public static async Task<UserProject> GetOwnerProjectAsync(Dataset dataset, IServiceContext serviceContext)
        {
            // First look in the cached information.
            var project = DatasetHelper.GetCachedOwnerProject(dataset);

            if (project == null)
            {
                using (var dbContext = serviceContext.DbContextFactory.Create())
                {
                    project = await GetOwnerProjectFromDbContextAsync(dataset.DatasetId, dbContext);
                }
            }

            return project;
        }

        /// <summary>
        /// Gets the previous dataset post process.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>The previous dataset post process.</returns>
        public static DatasetPostProcess GetPreviousDatasetPostProcess(Dataset dataset, DatasetPostProcess datasetPostProcess)
        {
            var orderedPostProcesses = dataset.DatasetPostProcess.OrderBy(p => p.Priority).ThenBy(p => p.ExecutionOrder).ThenBy(p => p.CreatedBy).ToList();

            int previousPostProcessIndex = orderedPostProcesses.IndexOf(datasetPostProcess) - 1;

            return previousPostProcessIndex >= 0 ? orderedPostProcesses[previousPostProcessIndex] : null;
        }

        /// <summary>
        /// Loads the dataset with dependencies.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="includeRelatedExpressions">if set to <c>true</c> [include related expressions].</param>
        /// <param name="includeParentFolder">if set to <c>true</c> [include parent folder].</param>
        /// <param name="includeInverseSourceDatasets">if set to <c>true</c> [include inverse source datasets].</param>
        /// <param name="includeUserProject">if set to <c>true</c> [include user project].</param>
        /// <param name="includeDatasetUserClientState">if set to <c>true</c> [include dataset user client state].</param>
        /// <returns>
        /// The dataset with the dependencies.
        /// </returns>
        public static async Task<Dataset> LoadDatasetWithDependenciesAsync(
            CustomSearchesDbContext dbContext,
            Guid datasetId,
            bool includeRelatedExpressions,
            bool includeParentFolder,
            bool includeInverseSourceDatasets,
            bool includeUserProject,
            bool includeDatasetUserClientState)
        {
            IQueryable<Dataset> datasetQuery = dbContext.Dataset
             .Where(d => d.DatasetId == datasetId)
             .Include(d => d.DatasetPostProcess)
             .Include(d => d.InteractiveChart);

            if (includeParentFolder)
            {
                datasetQuery = datasetQuery.Include(d => d.ParentFolder);
            }

            if (includeInverseSourceDatasets)
            {
                datasetQuery = datasetQuery.Include(d => d.InverseSourceDataset);
            }

            if (includeDatasetUserClientState)
            {
                datasetQuery = datasetQuery.Include(d => d.DatasetUserClientState);
            }

            if (includeUserProject)
            {
                datasetQuery = datasetQuery.Include(d => d.UserProjectDataset).ThenInclude(upd => upd.UserProject);
            }

            var dataset = await datasetQuery.FirstOrDefaultAsync();

            List<int> chartIds =
               (from ch in dataset.InteractiveChart select ch.InteractiveChartId).ToList();

            List<int> postProcessIds =
                (from pp in dataset.DatasetPostProcess select pp.DatasetPostProcessId).ToList();

            var rules =
                await (from ppr in dbContext.ExceptionPostProcessRule
                       join pp in dbContext.DatasetPostProcess
                            on ppr.DatasetPostProcessId equals pp.DatasetPostProcessId
                       where pp.DatasetId == datasetId
                       select ppr).ToListAsync();

            List<int> ruleIds = (from ppr in rules select ppr.ExceptionPostProcessRuleId).ToList();

            if (includeRelatedExpressions)
            {
                await (from e in dbContext.CustomSearchExpression
                       where postProcessIds.Contains((int)e.DatasetPostProcessId) ||
                            ruleIds.Contains((int)e.ExceptionPostProcessRuleId) ||
                            chartIds.Contains((int)e.DatasetChartId) ||
                            e.DatasetId == dataset.DatasetId
                       select e).LoadAsync();
            }

            return dataset;
        }

        /// <summary>
        /// Loads the datasets with dependencies.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datasetIds">The dataset identifier.</param>
        /// <param name="includeRelatedExpressions">if set to <c>true</c> [include related expressions].</param>
        /// <param name="includeParentFolder">if set to <c>true</c> [include parent folder].</param>
        /// <param name="includeInverseSourceDatasets">if set to <c>true</c> [include inverse source datasets].</param>
        /// <param name="includeUserProject">if set to <c>true</c> [include user project].</param>
        /// <param name="includeDatasetUserClientState">if set to <c>true</c> [include dataset user client state].</param>
        /// <returns>
        /// The datasets with the dependencies.
        /// </returns>
        public static async Task<List<Dataset>> LoadDatasetsWithDependenciesAsync(
            CustomSearchesDbContext dbContext,
            Guid[] datasetIds,
            bool includeRelatedExpressions,
            bool includeParentFolder,
            bool includeInverseSourceDatasets,
            bool includeUserProject,
            bool includeDatasetUserClientState)
        {
            IQueryable<Dataset> datasetQuery = dbContext.Dataset
             .Where(d => datasetIds.Contains(d.DatasetId))
             .Include(d => d.DatasetPostProcess)
             .Include(d => d.InteractiveChart);

            if (includeParentFolder)
            {
                datasetQuery = datasetQuery.Include(d => d.ParentFolder);
            }

            if (includeInverseSourceDatasets)
            {
                datasetQuery = datasetQuery.Include(d => d.InverseSourceDataset);
            }

            if (includeDatasetUserClientState)
            {
                datasetQuery = datasetQuery.Include(d => d.DatasetUserClientState);
            }

            if (includeUserProject)
            {
                datasetQuery = datasetQuery.Include(d => d.UserProjectDataset).ThenInclude(upd => upd.UserProject);
            }

            var datasets = await datasetQuery.ToDictionaryAsync(d => d.DatasetId);

            List<int> chartIds = new List<int>();
            List<int> postProcessIds = new List<int>();

            foreach (var dataset in datasets.Values)
            {
                chartIds.AddRange((from ch in dataset.InteractiveChart select ch.InteractiveChartId).ToList());
                postProcessIds.AddRange((from pp in dataset.DatasetPostProcess select pp.DatasetPostProcessId).ToList());
            }

            var rules =
                await (from ppr in dbContext.ExceptionPostProcessRule
                       join pp in dbContext.DatasetPostProcess
                            on ppr.DatasetPostProcessId equals pp.DatasetPostProcessId
                       where datasetIds.Contains(pp.DatasetId)
                       select ppr).ToListAsync();

            List<int> ruleIds = (from ppr in rules select ppr.ExceptionPostProcessRuleId).ToList();

            if (includeRelatedExpressions)
            {
                await (from e in dbContext.CustomSearchExpression
                       where postProcessIds.Contains((int)e.DatasetPostProcessId) ||
                            ruleIds.Contains((int)e.ExceptionPostProcessRuleId) ||
                            chartIds.Contains((int)e.DatasetChartId) ||
                           datasetIds.Contains((Guid)e.DatasetId)
                       select e).LoadAsync();
            }

            List<Dataset> sortedDatasets = new List<Dataset>();
            foreach (var datasetId in datasetIds)
            {
                if (datasets.ContainsKey(datasetId))
                {
                    sortedDatasets.Add(datasets[datasetId]);
                }
            }

            return sortedDatasets;
        }

        /// <summary>
        /// Detaches the dataset and its dependencies.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataset">The dataset.</param>
        public static void DetachDatasetWithDependencies(CustomSearchesDbContext dbContext, Dataset dataset)
        {
            dbContext.Entry(dataset).State = EntityState.Detached;

            foreach (var userProjectDataset in dataset.UserProjectDataset)
            {
                dbContext.Entry(userProjectDataset).State = EntityState.Detached;
            }

            List<CustomSearchExpression> expressions = dataset.CustomSearchExpression.ToList();

            foreach (var interactiveChart in dataset.InteractiveChart)
            {
                dbContext.Entry(interactiveChart).State = EntityState.Detached;
                expressions.AddRange(interactiveChart.CustomSearchExpression);
            }

            foreach (var datasetPostProcess in dataset.DatasetPostProcess)
            {
                dbContext.Entry(datasetPostProcess).State = EntityState.Detached;
                expressions.AddRange(datasetPostProcess.CustomSearchExpression);

                foreach (var exceptionPostProcessRule in datasetPostProcess.ExceptionPostProcessRule)
                {
                    dbContext.Entry(exceptionPostProcessRule).State = EntityState.Detached;
                    expressions.AddRange(exceptionPostProcessRule.CustomSearchExpression);
                }
            }

            foreach (var expression in expressions)
            {
                dbContext.Entry(expression).State = EntityState.Detached;
            }
        }

        /// <summary>
        /// Loads the users of the dataset and its dependencies.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataset">The dataset.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public static async Task LoadDatasetUsersAsync(CustomSearchesDbContext dbContext, Dataset dataset)
        {
            HashSet<Guid> userIds = new HashSet<Guid>() { dataset.CreatedBy, dataset.LastModifiedBy, dataset.UserId };
            if (dataset.LastExecutedBy != null)
            {
                userIds.Add((Guid)dataset.LastExecutedBy);
            }

            foreach (var datasetPostProcess in dataset.DatasetPostProcess)
            {
                userIds.Add(datasetPostProcess.CreatedBy);
                userIds.Add(datasetPostProcess.LastModifiedBy);
            }

            foreach (var interactiveChart in dataset.InteractiveChart)
            {
                userIds.Add(interactiveChart.CreatedBy);
                userIds.Add(interactiveChart.LastModifiedBy);
            }

            if (dataset.ParentFolder?.UserId != null)
            {
                userIds.Add((Guid)dataset.ParentFolder.UserId);
            }

            await (from u in dbContext.Systemuser
                   where userIds.Contains(u.Systemuserid)
                   select u).LoadAsync();
        }

        /// <summary>
        /// Gets the delete the backend update script.
        /// </summary>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="excludeFailed">Value indicating whether the failed backend updates should be excluded of the delete.</param>
        /// <returns>
        /// The delete backend update script.
        /// </returns>
        public static string GetDeleteBackendUpdateScript(int datasetPostProcessId, string major = null, string minor = null, bool excludeFailed = false)
        {
            string deleteScript = $"DELETE FROM [cus].[BackendUpdate] WHERE ([DatasetPostProcessId] = {datasetPostProcessId})";

            if (excludeFailed)
            {
                deleteScript += " AND ([ExportError] IS NULL)";
            }

            if (!string.IsNullOrWhiteSpace(major) && !string.IsNullOrWhiteSpace(minor))
            {
                deleteScript += $" AND ([Major] = {major}) AND ([Minor] = {minor})";
            }

            deleteScript += "\n";

            return deleteScript;
        }

        /// <summary>
        /// Loads the primary and secondaries dataset post processes.
        /// </summary>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The primary and secondaries dataset post processes.
        /// </returns>
        public static async Task<List<DatasetPostProcess>> LoadPrimaryAndSecondariesPostProcessesAsync(int datasetPostProcessId, CustomSearchesDbContext dbContext)
        {
            DatasetPostProcess datasetPostProcess = await dbContext.DatasetPostProcess.
                Where(dpp => dpp.DatasetPostProcessId == datasetPostProcessId).
                Include(dpp => dpp.Dataset).
                Include(dpp => dpp.PrimaryDatasetPostProcess).
                    ThenInclude(pdpp => pdpp.InversePrimaryDatasetPostProcess).
                Include(dpp => dpp.InversePrimaryDatasetPostProcess).
                FirstOrDefaultAsync();

            return DatasetHelper.GetSortedPrimaryAndSecondariesPostProcesses(datasetPostProcess);
        }

        /// <summary>
        /// Loads the primary and secondaries dataset post processes.
        /// </summary>
        /// <param name="userProject">The dataset post process.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcessData">The dataset post process data.</param>
        /// <param name="postProcessType">The dataset post process type.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The primary and secondaries dataset post processes.
        /// </returns>
        public static async Task<List<DatasetPostProcess>> LoadPrimaryAndSecondariesPostProcessesAsync(
            UserProject userProject,
            Dataset dataset,
            DatasetPostProcessData datasetPostProcessData,
            DatasetPostProcessType postProcessType,
            CustomSearchesDbContext dbContext)
        {
            List<DatasetPostProcess> postProcesses = new List<DatasetPostProcess>();
            if (userProject != null)
            {
                postProcesses.AddRange(
                    await DatasetHelper.LoadPrimaryAndSecondariesPostProcessesAsync(
                        userProject.UserProjectId,
                        datasetPostProcessData.PostProcessRole,
                        dbContext));
            }
            else
            {
                var datasetPostProcess = DatasetHelper.GetDatasetPostProcess(dataset, datasetPostProcessData, postProcessType);
                if (datasetPostProcess != null)
                {
                    postProcesses.Add(datasetPostProcess);
                }
            }

            return postProcesses;
        }

        /// <summary>
        /// Gets the dataset post process.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcessData">The dataset post process data.</param>
        /// <param name="postProcessType">The dataset post process type.</param>
        /// <returns>
        /// The dataset post process.
        /// </returns>
        public static DatasetPostProcess GetDatasetPostProcess(Dataset dataset, DatasetPostProcessData datasetPostProcessData, DatasetPostProcessType postProcessType)
        {
            string postprocessNameLower =
                datasetPostProcessData.PostProcessName == null ? string.Empty : datasetPostProcessData.PostProcessName.ToLower();
            string postprocessRoleLower =
                datasetPostProcessData.PostProcessRole == null ? string.Empty : datasetPostProcessData.PostProcessRole.ToLower();
            string postprocessTypeLower =
                postProcessType.ToString().ToLower();

            return dataset.DatasetPostProcess
                .Where(p =>
                    (p.DatasetId == datasetPostProcessData.DatasetId)
                     && ((p.PostProcessName == null ? string.Empty : p.PostProcessName.ToLower()) == postprocessNameLower)
                     && ((p.PostProcessRole == null ? string.Empty : p.PostProcessRole.ToLower()) == postprocessRoleLower)
                     && ((p.PostProcessType == null ? string.Empty : p.PostProcessType.ToLower()) == postprocessTypeLower))
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the project type related to the dataset.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The project type.
        /// </returns>
        public static async Task<ProjectType> GetProjectTypeAsync(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            var projectType =
                await (from pt in dbContext.ProjectType
                       join up in dbContext.UserProject
                            on pt.ProjectTypeId equals up.ProjectTypeId
                       join upd in dbContext.UserProjectDataset
                            on up.UserProjectId equals upd.UserProjectId
                       join d in dbContext.Dataset
                            on upd.DatasetId equals d.DatasetId
                       where d.DatasetId == datasetId
                       select pt).FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(
                projectType,
                nameof(projectType),
                datasetId,
                $"There is no project type for {nameof(datasetId)} '{datasetId}'");

            return projectType;
        }

        /// <summary>
        /// Loads the primary and secondaries dataset post processes.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="datasetPostProcessRole">The dataset post process role.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The primary and secondaries dataset post processes.
        /// </returns>
        private static async Task<List<DatasetPostProcess>> LoadPrimaryAndSecondariesPostProcessesAsync(int userProjectId, string datasetPostProcessRole, CustomSearchesDbContext dbContext)
        {
            DatasetPostProcess datasetPostProcess =
                await (from up in dbContext.UserProject
                       join upd in dbContext.UserProjectDataset
                            on up.UserProjectId equals upd.UserProjectId
                       join d in dbContext.Dataset
                            on upd.DatasetId equals d.DatasetId
                       join dpp in dbContext.DatasetPostProcess
                            on d.DatasetId equals dpp.DatasetId
                       where (up.UserProjectId == userProjectId) && (dpp.PostProcessRole.Trim().ToLower() == datasetPostProcessRole.Trim().ToLower())
                       select dpp).
                Include(dpp => dpp.Dataset).
                Include(dpp => dpp.PrimaryDatasetPostProcess).
                    ThenInclude(pdpp => pdpp.InversePrimaryDatasetPostProcess).
                Include(dpp => dpp.InversePrimaryDatasetPostProcess).
                FirstOrDefaultAsync();

            return DatasetHelper.GetSortedPrimaryAndSecondariesPostProcesses(datasetPostProcess);
        }

        /// <summary>
        /// Loads the primary and secondaries dataset post processes.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>
        /// The primary and secondaries dataset post processes.
        /// </returns>
        private static List<DatasetPostProcess> GetSortedPrimaryAndSecondariesPostProcesses(DatasetPostProcess datasetPostProcess)
        {
            List<DatasetPostProcess> sortedPostProcesses = new List<DatasetPostProcess>();

            if (datasetPostProcess != null)
            {
                if (datasetPostProcess.PrimaryDatasetPostProcess == null)
                {
                    sortedPostProcesses.Add(datasetPostProcess);
                    sortedPostProcesses.AddRange(datasetPostProcess.InversePrimaryDatasetPostProcess);
                }
                else
                {
                    sortedPostProcesses.Add(datasetPostProcess.PrimaryDatasetPostProcess);
                    sortedPostProcesses.AddRange(datasetPostProcess.PrimaryDatasetPostProcess.InversePrimaryDatasetPostProcess);
                }
            }

            return sortedPostProcesses;
        }
    }
}