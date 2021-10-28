namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that reverts updates to the dataset data.
    /// </summary>
    public class RevertDatasetUpdatesService : BaseService
    {
        /// <summary>
        /// The standard validation error message.
        /// </summary>
        private const string StandardValidationErrorMessage = "Error in update validation.";

        /// <summary>
        /// Initializes a new instance of the <see cref="RevertDatasetUpdatesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public RevertDatasetUpdatesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Reverts the dataset data updates.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="rowsToRevert">The rows to revert.</param>
        /// <param name="includeRevertedRows">if set to <c>true</c> return the reverted rows.  This flag is ignored if the request includes
        /// specific rows to revert.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>If includeRevertedRows is true and the whole dataset isn't reverted, returns the reverted rows.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset with id '{datasetId}' not found. - null</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<GetUserCustomSearchDataResponse> RevertDatasetUpdatesAsync(
            Guid datasetId,
            RevertDatasetUpdatesData rowsToRevert,
            bool includeRevertedRows,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset =
                await (from d in dbContext.Dataset where d.DatasetId == datasetId select d).
                    Include(d => d.ParentFolder).
                    FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);
            InputValidationHelper.AssertDatasetDataNotLocked(dataset);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "RevertDatasetUpdates");

            GetUserCustomSearchDataResponse toReturn = null;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    toReturn = await this.RevertDatasetDataNoLockAsync(dataset, rowsToRevert, includeRevertedRows, dbContext);
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

            return toReturn;
        }

        /// <summary>
        /// Reverts the dataset data updates without using a lock or validating dataset existence.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="rowsToRevert">The rows to revert.</param>
        /// <param name="includeRevertedRows">if set to <c>true</c> return the reverted rows.  This flag is ignored if the request includes.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>If includeRevertedRows is true and the whole dataset isn't reverted, returns the reverted rows.</returns>
        /// <exception cref="CustomSearchesDatabaseException">Revert statement failed in the database.</exception>
        public async Task<GetUserCustomSearchDataResponse> RevertDatasetDataNoLockAsync(
            Dataset dataset,
            RevertDatasetUpdatesData rowsToRevert,
            bool includeRevertedRows,
            CustomSearchesDbContext dbContext)
        {
            string datasetUpdateTableFullName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset);
            string datasetTableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            var scriptBuilder = new StringBuilder();

            bool revartAllDataset = !(rowsToRevert != null && rowsToRevert.RowIds != null && rowsToRevert.RowIds.Length > 0);

            if (!revartAllDataset)
            {
                foreach (var row in rowsToRevert.RowIds)
                {
                    scriptBuilder.Append(this.GetRevertScriptForRow(row, datasetUpdateTableFullName, datasetTableFullName));
                }

                if (includeRevertedRows)
                {
                    scriptBuilder.Append(await this.GetSelectUpdatesScriptAsync(dataset, rowsToRevert.RowIds, dbContext));
                }
            }
            else
            {
                scriptBuilder.Append(this.GetRevertScriptForRow(null, datasetUpdateTableFullName, datasetTableFullName));
            }

            GetUserCustomSearchDataResponse toReturn = null;
            if (!includeRevertedRows || revartAllDataset)
            {
                await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    scriptBuilder.ToString(),
                    "Revert statement failed in the database.");
            }
            else
            {
                toReturn = new GetUserCustomSearchDataResponse();
                toReturn.Results = (await DbTransientRetryPolicy.GetDynamicResultAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    scriptBuilder.ToString())).ToArray();
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the update script for a data row.
        /// </summary>
        /// <param name="datasetRowId">The row (or null if the full dataset is to be reverted).</param>
        /// <param name="targetTableName">Name of the target table.</param>
        /// <param name="datasetTableFullName">The full name of the dataset table.</param>
        /// <returns>
        /// The update script.
        /// </returns>
        private string GetRevertScriptForRow(DatasetRowIdData datasetRowId, string targetTableName, string datasetTableFullName)
        {
            string toReturn = $"DELETE FROM {targetTableName} ";

            if (datasetRowId != null)
            {
                if (datasetRowId.CustomSearchResultId > 0)
                {
                    toReturn = $"DELETE FROM {targetTableName} WHERE CustomSearchResultId = {datasetRowId.CustomSearchResultId} ";
                }
                else if (!string.IsNullOrWhiteSpace(datasetRowId.Major) && !string.IsNullOrWhiteSpace(datasetRowId.Minor))
                {
                    toReturn =
                        $"DELETE {targetTableName} " +
                        $"FROM {targetTableName} " +
                        $"INNER JOIN {datasetTableFullName} " +
                        $"ON {targetTableName}.[CustomSearchResultId] = {datasetTableFullName}.[CustomSearchResultId] " +
                        $"WHERE {datasetTableFullName}.Major = '{datasetRowId.Major}' AND {datasetTableFullName}.Minor = '{datasetRowId.Minor}' ";
                }
            }

            toReturn += Environment.NewLine;

            return toReturn;
        }

        /// <summary>
        /// Gets the script to select the reverted rows.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="rowsToRevert">The rows to revert.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// A string with the validation script.
        /// </returns>
        private async Task<string> GetSelectUpdatesScriptAsync(Dataset dataset, DatasetRowIdData[] rowsToRevert, CustomSearchesDbContext dbContext)
        {
            bool usePostProcess = await DatasetHelper.CanUsePostProcessAsync(dataset, datasetPostProcess: null, usePostProcess: true, dbContext);
            string datasetViewName = DatasetHelper.GetDatasetView(dataset, usePostProcess, null);

            var rowFilterBuilder = new StringBuilder();

            for (int i = 0; i < UpdateDatasetDataService.MaxRowsToReturn && i < rowsToRevert.Length; i++)
            {
                var rowKey = rowsToRevert[i];

                if (rowKey.CustomSearchResultId > 0)
                {
                    rowFilterBuilder.Append($" CustomSearchResultId = {rowKey.CustomSearchResultId} OR");
                }
                else if (!string.IsNullOrWhiteSpace(rowKey.Major) && !string.IsNullOrWhiteSpace(rowKey.Minor))
                {
                    rowFilterBuilder.Append($" (Major = '{rowKey.Major}' AND Minor = '{rowKey.Minor}') OR");
                }
            }

            string rowFilterString = rowFilterBuilder.ToString();
            rowFilterString = rowFilterString.Substring(0, rowFilterString.Length - 2);

            return $"SELECT s.* FROM {datasetViewName} s " +
                $"WHERE {rowFilterString}";
        }
    }
}
