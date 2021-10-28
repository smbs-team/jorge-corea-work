namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that updates the dataset data.
    /// </summary>
    public class UpdateDatasetDataService : BaseService
    {
        /// <summary>
        /// The update batch size.
        /// </summary>
        public const int UpdateBatchSize = 250;

        /// <summary>
        /// The max amount of rows to return as part of a single revert.
        /// </summary>
        public const int MaxRowsToReturn = 250;

        /// <summary>
        /// The standard validation error message.
        /// </summary>
        private const string StandardValidationErrorMessage = "Error in update validation.";

        /// <summary>
        /// The validation script dictionary (per dataset).
        /// </summary>
        private IDictionary<string, string> validationScriptDictionary = new Dictionary<string, string>();

        /// <summary>
        /// DbColumns in the dataset.
        /// </summary>
        private IDictionary<string, DbColumn> dbColumns = new Dictionary<string, DbColumn>();

        /// <summary>
        /// The authorized editable columns.
        /// </summary>
        private HashSet<string> authorizedEditableColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDatasetDataService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UpdateDatasetDataService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Updates the dataset data.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="rows">The rows to update.</param>
        /// <param name="includeUpdatedRows">Whether to include the updated rows as part of the result.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The rows if includeUpdatedRows is true.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter dataset.</exception>
        public async Task<GetUserCustomSearchDataResponse> UpdateDatasetDataAsync(
            Guid datasetId,
            JObject[] rows,
            bool includeUpdatedRows,
            string clientId,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetId)
                .Include(d => d.ParentFolder)
                .Include(d => d.CustomSearchDefinition)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);
            InputValidationHelper.AssertDatasetDataNotLocked(dataset);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "UpdateDatasetData");

            this.ServiceContext.AuthProvider.AuthorizeAnyRole(
                dataset.CustomSearchDefinition.DatasetEditRoles,
                $"Current user doesn't have permissions to update the dataset data of a custom search with id '{dataset.CustomSearchDefinitionId}'. Required roles: '{dataset.CustomSearchDefinition.DatasetEditRoles}'.");

            await this.AuthorizeRowLevelUpdateAsync(dataset, rows, dbContext);

            return await this.UpdateDatasetDataAsync(dataset, rows, includeUpdatedRows, clientId, dbContext);
        }

        /// <summary>
        /// Authorizes the row level update.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns> The async task.</returns>
        public async Task AuthorizeRowLevelUpdateAsync(
           Dataset dataset,
           JObject[] rows,
           CustomSearchesDbContext dbContext)
        {
            string authColumn = dataset.CustomSearchDefinition.RowLevelEditRolesColumn;
            if (!string.IsNullOrWhiteSpace(authColumn))
            {
                var keysToValidate = new List<(bool keyFound, bool hasNonDefaultColumns, int customSearchResultId, string major, string minor)>();
                string whereStatement = string.Empty;

                // Gathers keys for each row.
                foreach (var row in rows)
                {
                    var key = this.GetRowKey(row);
                    if (key.keyFound && key.hasNonDefaultColumns)
                    {
                        if (key.customSearchResultId != -1)
                        {
                            whereStatement += $"CustomSearchResultId = {key.customSearchResultId} OR ";
                        }
                        else
                        {
                            whereStatement += $"(Major = {key.major} AND Minor = {key.minor}) OR ";
                        }

                        keysToValidate.Add(key);
                    }
                }

                if (keysToValidate.Count > 0)
                {
                    whereStatement = whereStatement.Substring(0, whereStatement.Length - " OR ".Length); // Removes last OR
                    string sourceTableName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
                    string authorizationScript = $"SELECT CustomSearchResultId, {authColumn} FROM {sourceTableName} WHERE {whereStatement}";

                    await DbTransientRetryPolicy.ExecuteReaderAsync(
                        this.ServiceContext,
                        async (command, dataReader) =>
                        {
                            while (dataReader.Read())
                            {
                                int resultId = dataReader.GetInt32(0);
                                string roles = dataReader.GetString(1);
                                this.ServiceContext.AuthProvider.AuthorizeAnyRole(
                                    roles,
                                    $"Current user doesn't have permissions to update the dataset data for the row with CustomSearchResultId '{resultId}'. Required roles: '{roles}'.");
                            }
                        },
                        this.ServiceContext.DataDbContextFactory,
                        authorizationScript,
                        null);
                }
            }
        }

        /// <summary>
        /// Updates the dataset data.  Doesn't validate dataset existence.  Performs a lock.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="rows">The rows to update.</param>
        /// <param name="includeUpdatedRows">Whether to include the updated rows as part of the result.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        ///  The rows if includeUpdatedRows is true.
        /// </returns>
        public async Task<GetUserCustomSearchDataResponse> UpdateDatasetDataAsync(
            Dataset dataset,
            JObject[] rows,
            bool includeUpdatedRows,
            string clientId,
            CustomSearchesDbContext dbContext)
        {
            GetUserCustomSearchDataResponse toReturn = null;
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    toReturn = await this.UpdateDatasetDataNoLockAsync(dataset, rows, includeUpdatedRows, clientId, dbContext);
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
        /// Updates the dataset data without using a lock or validating dataset existence.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="rows">The rows to update.</param>
        /// <param name="includeUpdatedRows">Whether to include the updated rows as part of the result.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        public async Task<GetUserCustomSearchDataResponse> UpdateDatasetDataNoLockAsync(
            Dataset dataset,
            JObject[] rows,
            bool includeUpdatedRows,
            string clientId,
            CustomSearchesDbContext dbContext)
        {
            string targetTableName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset);
            string sourceTableName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            var scriptBuilder = new StringBuilder();
            var dbColumns = await this.GetDbColumnsAsync(dataset);
            await this.LoadAuthorizedEditableColumnsAsync(dataset, dbContext);

            List<DatasetRowSelectionData> datasetRowSelectionList = new List<DatasetRowSelectionData>();
            foreach (var row in rows)
            {
                scriptBuilder.Append(this.GetUpdateScriptForRow(row, sourceTableName, targetTableName, dbColumns));

                if (row.Properties().FirstOrDefault(p => DatasetColumnHelper.IsSelectionColumn(p.Name)) != null)
                {
                    datasetRowSelectionList.Add(row.ToObject<DatasetRowSelectionData>());
                }
            }

            string updateScript = scriptBuilder.ToString();
            if (string.IsNullOrWhiteSpace(updateScript))
            {
                return null;
            }

            if (includeUpdatedRows)
            {
                updateScript += this.GetSelectUpdatesScriptPreValidate(targetTableName);
            }

            updateScript += await this.GetValidateScriptAsync(dataset, targetTableName, dbContext);

            if (includeUpdatedRows)
            {
                updateScript += await this.GetSelectUpdatesScriptPostValidateAsync(dataset, dbContext);
            }

            string errorMessage = "Update statement failed in the database.  Check that all column names and values are valid for this dataset.";

            GetUserCustomSearchDataResponse toReturn = null;
            if (includeUpdatedRows)
            {
                toReturn = new GetUserCustomSearchDataResponse();
                toReturn.Results = (await DynamicSqlStatementHelper.GetDynamicResultWithRetriesAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    updateScript,
                    errorMessage)).ToArray();

                toReturn.TotalRows = toReturn.Results.Length;
            }
            else
            {
                await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    updateScript,
                    errorMessage);
            }

            if (datasetRowSelectionList.Count > 0)
            {
                await this.ServiceContext.SendRealTimeNotificationAsync(
                    "UIClientHub",
                    this.ServiceContext.AuthProvider.UserInfoData.Id.ToString(),
                    "DatasetSelectionChanged",
                    dataset.DatasetId,
                    clientId,
                    datasetRowSelectionList);
            }

            return toReturn;
        }

        /// <summary>
        /// Determines whether the type can be cast to decimal in TSQL.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type can be cast to decimal in TSQL; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanSqlCastToDecimal(Type type)
        {
            return type != typeof(Guid);
        }

        /// <summary>
        /// Gets the database columns.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>
        /// A dictionary with the db columns.
        /// </returns>
        private async Task<IDictionary<string, DbColumn>> GetDbColumnsAsync(Dataset dataset)
        {
            if (this.dbColumns == null || this.dbColumns.Count == 0)
            {
                var columns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);
                this.dbColumns = (from c in columns select c).ToDictionary(c => c.ColumnName.ToLower(), c => c);
            }

            return this.dbColumns;
        }

        /// <summary>
        /// Loads the columns that the user can edit.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        private async Task LoadAuthorizedEditableColumnsAsync(Dataset dataset, CustomSearchesDbContext dbContext)
        {
            if (this.authorizedEditableColumns == null)
            {
                this.authorizedEditableColumns = new HashSet<string>();
                var columnDefinitions = await (
                    from d in dbContext.CustomSearchColumnDefinition
                    where d.CustomSearchDefinitionId == dataset.CustomSearchDefinitionId
                    select d).ToArrayAsync();

                foreach (var columnDefinition in columnDefinitions)
                {
                    if (this.ServiceContext.AuthProvider.IsAuthorizedToAnyRole(columnDefinition.ColumnEditRoles))
                    {
                        this.authorizedEditableColumns.Add(columnDefinition.ColumnName.ToLower());
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the user has permissions to edit the column.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        private void AuthorizeEditableColumn(string columnName)
        {
            if (!this.authorizedEditableColumns.Contains(columnName, StringComparer.OrdinalIgnoreCase))
            {
                throw new AuthorizationException(
                    $"Current user doesn't have permissions to edit the column '{columnName}'.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Gets the script to select the updated rows. This should run prior to validations.
        /// </summary>
        /// <param name="targetTableName">Name of the target table.</param>
        /// <returns>A string with the validation script.</returns>
        private string GetSelectUpdatesScriptPreValidate(string targetTableName)
        {
            return $"CREATE TABLE #ToValidate (CustomSearchResultId int); " +
                $"INSERT INTO #ToValidate " +
                $"SELECT CustomSearchResultId " +
                $"FROM {targetTableName} " +
                $"WHERE Validated = 0;" + Environment.NewLine;
        }

        /// <summary>
        /// Gets the script to select the updated rows.  This should run after validations.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A string with the validation script.</returns>
        private async Task<string> GetSelectUpdatesScriptPostValidateAsync(Dataset dataset, CustomSearchesDbContext dbContext)
        {
            bool usePostProcess = await DatasetHelper.CanUsePostProcessAsync(dataset, datasetPostProcess: null, usePostProcess: true, dbContext);
            string datasetViewName = DatasetHelper.GetDatasetView(dataset, usePostProcess, null);

            return $"SELECT s.* FROM {datasetViewName} s " +
                $"INNER JOIN #ToValidate t " +
                $"ON s.CustomSearchResultId = t.CustomSearchResultId; " + Environment.NewLine;
        }

        /// <summary>
        /// Gets the validation script.  Validation script is cached per dataset.
        /// </summary>
        /// <param name="targetTableName">Name of the target table.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A string with the validation script.</returns>
        private async Task<string> GetValidateScriptAsync(Dataset dataset, string targetTableName, CustomSearchesDbContext dbContext)
        {
            string validationScript = string.Empty;

            if (this.validationScriptDictionary.ContainsKey(dataset.DatasetId.ToString()))
            {
                validationScript = this.validationScriptDictionary[dataset.DatasetId.ToString()];
            }
            else
            {
                string sourceView = CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, false);

                var validationRules = await
                    (from vr in dbContext.CustomSearchValidationRule
                     where vr.CustomSearchDefinitionId == dataset.CustomSearchDefinitionId
                     orderby vr.ExecutionOrder descending
                     select vr).Include(d => d.CustomSearchExpression).ToListAsync();

                var validConditionScriptBuilder = new StringBuilder();
                string errorValueScript = string.Empty;

                int validationConditionCount = 0;
                string conditionConcatenator = "AND";

                foreach (var rule in validationRules)
                {
                    var conditionExp =
                        (from exp in rule.CustomSearchExpression
                         where exp.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.ValidationConditionExpression.ToString().ToLower()
                         select exp).FirstOrDefault();

                    if (conditionExp == null)
                    {
                        continue;
                    }

                    validationConditionCount++;

                    var errorExp =
                        (from exp in rule.CustomSearchExpression
                         where exp.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.ValidationErrorExpression.ToString().ToLower()
                         select exp).FirstOrDefault();

                    validConditionScriptBuilder.Append($"({conditionExp.Script}) {conditionConcatenator}");

                    string ruleErrorValueScript = errorExp != null ? errorExp.Script : $"'{UpdateDatasetDataService.StandardValidationErrorMessage}'";

                    if (string.IsNullOrEmpty(errorValueScript))
                    {
                        errorValueScript = $"IIF(NOT({conditionExp.Script}), {ruleErrorValueScript}, '')";
                    }
                    else
                    {
                        errorValueScript = $"IIF(NOT({conditionExp.Script}), {ruleErrorValueScript}, {errorValueScript})";
                    }
                }

                if (validationConditionCount == 0)
                {
                    return $";UPDATE {targetTableName} " +
                    $"SET Validated = 1, IsValid = 1, ErrorMessage = NULL " +
                    $"WHERE Validated = 0 " + Environment.NewLine;
                }

                string validConditionScript = validConditionScriptBuilder.ToString();
                validConditionScript = validConditionScript.Remove(validConditionScript.Length - conditionConcatenator.Length);

                validationScript =
                    $";WITH cte AS( " +
                    $"SELECT IIF({validConditionScript}, 1, 0) AS IsValid, {errorValueScript} AS ErrorMessage, CustomSearchResultId " +
                    $"from {sourceView} " +
                    $") " +
                    $"UPDATE t " +
                    $"SET t.Validated = 1, t.IsValid = cte.IsValid, t.ErrorMessage = cte.ErrorMessage " +
                    $"FROM cte INNER JOIN {targetTableName} t ON cte.CustomSearchResultId = t.CustomSearchResultId " +
                    $"WHERE t.Validated = 0 " + Environment.NewLine;

                validationScript = this.ReplaceCurrentUserRoles(validationScript);
                this.validationScriptDictionary[dataset.DatasetId.ToString()] = validationScript;
            }

            return validationScript;
        }

        /// <summary>
        /// Replaces the current user roles in the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>
        /// The replaced text.
        /// </returns>
        private string ReplaceCurrentUserRoles(string script)
        {
            var userRoleArray = this.ServiceContext.AuthProvider.UserInfoData.Roles;
            string currentUserRoles = string.Empty;
            if (userRoleArray != null && userRoleArray.Length > 0)
            {
                currentUserRoles = string.Join(", ", userRoleArray.Select(r => r.Name.ToLower().Trim()).ToHashSet());
            }

            return script.Replace("{CurrentUserRoles}", $"'{currentUserRoles}'");
        }

        /// <summary>
        /// Gets the data key for a given row.
        /// </summary>
        /// <param name="row">The row.</param>
        private (bool keyFound, bool hasNonDefaultColumns, int customSearchResultId, string major, string minor) GetRowKey(JObject row)
        {
            (bool keyFound, bool hasNonDefaultColumns, int customSearchResultId, string major, string minor) toReturn = (false, false, -1, null, null);

            foreach (var item in row)
            {
                string name = item.Key.ToLower();
                JToken token = item.Value;
                object tokenValue = ((JValue)item.Value).Value;

                if (tokenValue != null)
                {
                    // 'CustomSearchResultId' is the key of row and 'RowNum' is not part of the table
                    if (name == "customsearchresultid")
                    {
                        try
                        {
                            toReturn.customSearchResultId = System.Convert.ToInt32(((JValue)item.Value).Value);
                            toReturn.keyFound = true;
                        }
                        catch
                        {
                            // No CustomSearchResultId.  Move on.
                        }
                    }
                    else if (name == "major")
                    {
                        try
                        {
                            toReturn.major = ((JValue)item.Value).Value.ToString();
                            if (!string.IsNullOrWhiteSpace(toReturn.minor))
                            {
                                toReturn.keyFound = true;
                            }
                        }
                        catch
                        {
                            // No major, move on.
                        }
                    }
                    else if (name == "minor")
                    {
                        try
                        {
                            toReturn.minor = ((JValue)item.Value).Value.ToString();
                            if (!string.IsNullOrWhiteSpace(toReturn.major))
                            {
                                toReturn.keyFound = true;
                            }
                        }
                        catch
                        {
                            // No minor, move on.
                        }
                    }
                    else if (!DatasetColumnHelper.IsDefaultEditableColumn(name))
                    {
                        toReturn.hasNonDefaultColumns = true;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(toReturn.major) || string.IsNullOrWhiteSpace(toReturn.minor))
            {
                toReturn.major = null;
                toReturn.minor = null;
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the update script for a data row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="sourceTableName">Name of the source table.</param>
        /// <param name="targetTableName">Name of the target table.</param>
        /// <param name="dbColumns">The database columns.</param>
        /// <returns>
        /// The update script.
        /// </returns>
        private string GetUpdateScriptForRow(
            JObject row,
            string sourceTableName,
            string targetTableName,
            IDictionary<string, DbColumn> dbColumns)
        {
            var updateFieldConditionBuilder = new StringBuilder();
            var insertFieldConditionBuilder = new StringBuilder();
            var updateSetSentenceBuilder = new StringBuilder(
                "t.[IsValid] = 0, t.[Validated] = 0, t.[BackendExportState] = 'NotExported', t.[ErrorMessage] = '', t.[ExportedToBackEndErrorMessage] = '',");
            var insertFieldsBuilder = new StringBuilder("[RowVersion],[CustomSearchResultId],");
            var insertValuesBuilder = new StringBuilder("'0',s.[CustomSearchResultId],");
            int updateColumnCount = 0;

            var key = this.GetRowKey(row);

            if (!key.keyFound)
            {
                return string.Empty;
            }

            foreach (var item in row)
            {
                string name = item.Key.ToLower();
                JToken token = item.Value;

                object tokenValue = ((JValue)item.Value).Value;

                if (tokenValue != null)
                {
                    // 'CustomSearchResultId' is the key of row and 'RowNum' is not part of the table
                    if (name == "customsearchresultid" || name == "major" || name == "minor")
                    {
                        // Key is ignored here.
                    }
                    else if (DatasetColumnHelper.IsSelectionOrFilterColumn(name))
                    {
                        // Selected is ignored here.  Update is done separately.
                    }
                    else if (!DatasetColumnHelper.IsDefaultNotEditableColumn(name) && dbColumns.ContainsKey(name))
                    {
                        this.AuthorizeEditableColumn(name);

                        string colValue = $"'{DatasetColumnHelper.ColumnValueToString(tokenValue, true)}'";

                        string mergedTableValue = $"ISNULL(t.[{name}], s.[{name}]) ";
                        string updateNumericCondition = $"ISNUMERIC({mergedTableValue}) = 1 AND ISNUMERIC({colValue}) = 1 ";
                        string insertNumericCondition = $"ISNUMERIC(s.[{name}]) = 1 AND ISNUMERIC({colValue}) = 1 ";

                        string asDecimalColumnValue = colValue;
                        string asDecimalMergedValue = mergedTableValue;
                        string asDecimalSourceValue = $"s.[{name}]";

                        Type columnType = dbColumns[name].DataType;

                        if (UpdateDatasetDataService.CanSqlCastToDecimal(columnType))
                        {
                            asDecimalColumnValue = $"CONVERT(DECIMAL(30,10), {asDecimalColumnValue}) ";
                            asDecimalMergedValue = $"CONVERT(DECIMAL(30,10), {asDecimalMergedValue}) ";
                            asDecimalSourceValue = $"CONVERT(DECIMAL(30,10), {asDecimalSourceValue}) ";
                        }

                        string numericalUpdateComparison = $"IIF({asDecimalMergedValue} != {asDecimalColumnValue}, 1, 0)";
                        string nonNumericalUpdateComparison = $"IIF({mergedTableValue} != {colValue}, 1, 0)";

                        string numericalInsertComparison = $"IIF({asDecimalSourceValue} != {asDecimalColumnValue}, 1, 0)";
                        string nonNumericalInsertComparison = $"IIF(s.[{name}] != {colValue}, 1, 0)";

                        updateFieldConditionBuilder.Append($" IIF({updateNumericCondition}, {numericalUpdateComparison}, {nonNumericalUpdateComparison}) = 1 {Environment.NewLine} OR");
                        insertFieldConditionBuilder.Append($" IIF({insertNumericCondition}, {numericalInsertComparison}, {nonNumericalInsertComparison}) = 1 {Environment.NewLine} OR");
                        updateColumnCount++;

                        insertFieldsBuilder.Append($" [{name}] ,");
                        insertValuesBuilder.Append(
                            $" IIF(IIF({insertNumericCondition}, {numericalInsertComparison}, {nonNumericalInsertComparison}) = 1, {colValue}, NULL) {Environment.NewLine} ,");

                        updateSetSentenceBuilder.Append(
                            $" t.[{name}] = IIF(IIF({updateNumericCondition}, {numericalInsertComparison}, {nonNumericalInsertComparison}) = 1, {colValue}, NULL) ,");
                    }
                }
            }

            string matchStatement = null;
            if (!string.IsNullOrWhiteSpace(key.major) && (!string.IsNullOrWhiteSpace(key.minor)))
            {
                matchStatement = $"s.Major = {key.major} AND s.Minor = {key.minor}";
            }
            else if (key.customSearchResultId >= 0)
            {
                matchStatement = $"s.CustomSearchResultId = {key.customSearchResultId}";
            }

            string script = string.Empty;
            if (updateColumnCount > 0)
            {
                string updateSetSentence = updateSetSentenceBuilder.ToString().TrimEnd(new char[] { ',' });

                string updateFieldConditionSentece = updateFieldConditionBuilder.ToString();
                updateFieldConditionSentece = updateFieldConditionSentece.Substring(0, updateFieldConditionSentece.Length - 2);

                string insertFieldConditionSentece = insertFieldConditionBuilder.ToString();
                insertFieldConditionSentece = insertFieldConditionSentece.Substring(0, insertFieldConditionSentece.Length - 2);

                string insertFields = insertFieldsBuilder.ToString().TrimEnd(new char[] { ',' });
                string insertValues = insertValuesBuilder.ToString().TrimEnd(new char[] { ',' });

                script +=
                    $"MERGE {targetTableName} t USING {sourceTableName} s " + Environment.NewLine +
                    $"ON t.CustomSearchResultId = s.CustomSearchResultId " + Environment.NewLine +
                    $"WHEN MATCHED AND {matchStatement} AND ({Environment.NewLine} {updateFieldConditionSentece} {Environment.NewLine})" + Environment.NewLine +
                    $"  THEN UPDATE SET " + Environment.NewLine +
                    $"  {updateSetSentence} " + Environment.NewLine +
                    $"WHEN NOT MATCHED BY TARGET AND {matchStatement} AND ({Environment.NewLine} {insertFieldConditionSentece} {Environment.NewLine})" + Environment.NewLine +
                    $"  THEN INSERT({insertFields}) " +
                    $"  VALUES({insertValues});" + Environment.NewLine;
            }

            var selectionOrFilterUpdate = from p in row.Properties() where DatasetColumnHelper.IsSelectionOrFilterColumn(p.Name) select p;
            foreach (var property in selectionOrFilterUpdate)
            {
                string colValue = $"'{DatasetColumnHelper.ColumnValueToString(property.Value, true)}'";

                script +=
                    $"UPDATE s " +
                    $"  SET s.{property.Name} = {colValue} " +
                    $"  FROM {sourceTableName} s " +
                    $"  WHERE {matchStatement} " + Environment.NewLine;
            }

            return script;
        }
    }
}