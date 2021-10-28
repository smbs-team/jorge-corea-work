namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using DocumentFormat.OpenXml;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the dataset data.
    /// </summary>
    public class GetDatasetDataService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetDataService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetDataService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Generates the script that gets the updates for a limited number of records.
        /// Care needs to be taken so this is not called with too many records.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="rowKeys">Row keys to get the data for (CustomSearchResultId).</param>
        /// <returns>
        /// The search script.
        /// </returns>
        public static string GenerateGetUpdatesScript(Dataset dataset, int[] rowKeys)
        {
            string targetTableName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset);
            string viewName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);

            var inExpressionBuilder = new StringBuilder();
            foreach (var key in rowKeys)
            {
                inExpressionBuilder.Append($"{key.ToString()},");
            }

            string inExpression = inExpressionBuilder.ToString().TrimEnd(new char[] { ',' });

            return $"SELECT t.* FROM {viewName} s " +
                $"INNER JOIN {targetTableName} t " +
                $"ON s.CustomSearchResultId = t.CustomSearchResultId " +
                $"WHERE t.CustomSearchResultId IN ({inExpression}) ";
        }

        /// <summary>
        /// Generates the script for the search.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process scope.</param>
        /// <param name="addConnectionOptimizationStatements">if set to <c>true</c> adds arithabort and transnaction isolation level statements.</param>
        /// <returns>
        /// The search script.
        /// </returns>
        public static string GenerateSearchScript(
            Dataset dataset,
            int startIndex,
            int rowCount,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            bool addConnectionOptimizationStatements = true)
        {
            string script = string.Empty;
            StringValue viewScript;
            if (datasetPostProcess == null)
            {
                string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, usePostProcess);
                viewScript = $"SELECT * FROM {viewName}";
            }
            else
            {
                viewScript = datasetPostProcess.CalculatedView;
            }

            if (addConnectionOptimizationStatements)
            {
                script += "set arithabort on\n";
                script += "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED\n";
            }

            script += "Select *";
            script += "FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY CustomSearchResultId ) AS RowNum, *\n";
            script += $"FROM ({viewScript}) dv ) Ordered\n";
            script += $"where RowNum > {startIndex} and RowNum  <= {startIndex + rowCount}";

            return script;
        }

        /// <summary>
        /// Generates the filter script.
        /// </summary>
        /// <param name="filterModel">The filter model.</param>
        /// <param name="tableAlias">The table alias.</param>
        /// <returns>The search script.</returns>
        public static string GenerateFilterScript(Dictionary<string, CustomSearchGridFilterConditionData> filterModel, string tableAlias)
        {
            string script = string.Empty;

            if ((filterModel != null) && (filterModel.Count > 0))
            {
                foreach (KeyValuePair<string, CustomSearchGridFilterConditionData> entry in filterModel)
                {
                    // 'empty' is a special filter option. When Empty is displayed, it means the filter is not active.
                    if (entry.Value.Type != "empty")
                    {
                        string databaseOperator = GetDatabaseOperator(entry.Value.Type);
                        string databaseFilterValue = GetDatabaseFilterValue(entry.Value);
                        if ((databaseOperator != null) && (databaseFilterValue != null))
                        {
                            if (string.IsNullOrWhiteSpace(script) == false)
                            {
                                script += " AND ";
                            }

                            string entryKey = string.IsNullOrWhiteSpace(tableAlias) ? $"[{entry.Key}]" : $"{tableAlias}.[{entry.Key}]";

                            script += $"{entryKey} {databaseOperator} '{databaseFilterValue}'";
                            if (databaseOperator == "BETWEEN")
                            {
                                if (string.IsNullOrWhiteSpace(entry.Value.FilterTo) == false)
                                {
                                    script += " AND " + "'" + entry.Value.FilterTo + "'";
                                }
                            }

                            script += "\n";
                        }
                    }
                }
            }

            return script;
        }

        /// <summary>
        /// Gets the sql operator.
        /// </summary>
        /// <param name="filterOperator">The filter operator.</param>
        /// <returns>The sql operator.</returns>
        public static string GetDatabaseOperator(string filterOperator)
        {
            switch (filterOperator.ToLower())
            {
                case "startswith":
                case "endswith":
                case "contains":
                    return "LIKE";
                case "notcontains":
                    return "NOT LIKE";
                case "equals":
                    return "=";
                case "notequal":
                    return "<>";
                case "lessthan":
                    return "<";
                case "lessthanorequal":
                    return "<=";
                case "greaterthan":
                    return ">";
                case "greaterthanorequal":
                    return ">=";
                case "inrange":
                    return "BETWEEN";
            }

            return null;
        }

        /// <summary>
        /// Gets the sql operator.
        /// </summary>
        /// <param name="conditionData">The filter condition.</param>
        /// <returns>The sql operator.</returns>
        public static string GetDatabaseFilterValue(CustomSearchGridFilterConditionData conditionData)
        {
            switch (conditionData.Type.ToLower())
            {
                case "startswith":
                    return conditionData.Filter + "%";
                case "endswith":
                    return "%" + conditionData.Filter;
                case "contains":
                case "notcontains":
                    return "%" + conditionData.Filter + "%";
            }

            return conditionData.Filter;
        }

        /// <summary>
        /// Gets the dataset data.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="startRowIndex">Start index of the row.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>
        /// The result objects.
        /// </returns>
        public static async Task<GetUserCustomSearchDataResponse> GetDatasetDataAsync(
            Dataset dataset,
            int startRowIndex,
            int rowCount,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess,
            IServiceContext serviceContext)
        {
            GetUserCustomSearchDataResponse response = new GetUserCustomSearchDataResponse();
            string script = GetDatasetDataService.GenerateSearchScript(dataset, startRowIndex, rowCount, usePostProcess, datasetPostProcess);

            response.Results = (await DbTransientRetryPolicy.GetDynamicResultAsync(
                serviceContext,
                serviceContext.DataDbContextFactory,
                script)).ToArray();

            return response;
        }

        /// <summary>
        /// Gets the dataset data.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="includeUpdateInfo">Value indicating whether to include information on what rows and fields were updated.</param>
        /// <param name="customSearchGridData">The filter parameters.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The result objects.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        public async Task<GetUserCustomSearchDataResponse> GetDatasetDataAsync(
            Guid datasetId,
            bool usePostProcess,
            int? postProcessId,
            bool includeUpdateInfo,
            CustomSearchGridData customSearchGridData,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            DatasetPostProcess datasetPostProcess = null;
            if (usePostProcess && (postProcessId > 0))
            {
                datasetPostProcess = await dbContext.DatasetPostProcess.FirstOrDefaultAsync(d => (d.DatasetPostProcessId == postProcessId) && d.DatasetId == datasetId);
                InputValidationHelper.AssertEntityExists(datasetPostProcess, "DatasetPostProcess", postProcessId);
            }

            usePostProcess = await DatasetHelper.CanUsePostProcessAsync(dataset, datasetPostProcess, usePostProcess, dbContext);
            DatasetHelper.AssertCanUsePostProcess(datasetPostProcess, usePostProcess);

            DatasetHelper.AssertCanReadFromDataset(dataset, usePostProcess);

            GetUserCustomSearchDataResponse response = new GetUserCustomSearchDataResponse();

            int rowGroupColsCount = customSearchGridData.RowGroupCols?.Length ?? 0;
            int groupKeysCount = customSearchGridData.GroupKeys?.Count ?? 0;
            bool searchGroups = rowGroupColsCount > 0 && rowGroupColsCount > groupKeysCount;
            string script = this.GenerateSearchScript(dataset, usePostProcess, datasetPostProcess, customSearchGridData, searchGroups);

            // This has several tasks that may execute in parallel.
            var tasks = new List<Task>();

            Func<Task> getRowsAction = async () =>
            {
                response.Results = (await DbTransientRetryPolicy.GetDynamicResultAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    script)).ToArray();

                if (includeUpdateInfo && !searchGroups && response.Results.Length > 0)
                {
                    int[] rowKeys = (from r in response.Results select (int)(r as IDictionary<string, object>)["CustomSearchResultId"]).ToArray();
                    string updatesScript = GetDatasetDataService.GenerateGetUpdatesScript(dataset, rowKeys);
                    dynamic[] updates = (await DbTransientRetryPolicy.GetDynamicResultAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        updatesScript)).ToArray();

                    response.UpdatedColumns = this.GetUpdatedColumns(updates);
                }
            };

            tasks.Add(getRowsAction.Invoke());

            if (includeUpdateInfo && !searchGroups)
            {
                Func<Task> getUpdateCount = async () =>
                {
                    response.TotalUpdatedRows = (int)(await DynamicSqlStatementHelper.ExecuteScalarWithRetriesAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        DatasetScriptBuilder.BuildGetUpdatedRowCountScript(dataset),
                        "Error retrieving total updated rows."));
                };

                Func<Task> getExportCount = async () =>
                {
                    response.TotalExportedRows = (int)(await DynamicSqlStatementHelper.ExecuteScalarWithRetriesAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        DatasetScriptBuilder.BuildGetExportedRowCountScript(dataset),
                        "Error retrieving total exported rows."));
                };

                tasks.Add(getUpdateCount.Invoke());
                tasks.Add(getExportCount.Invoke());
            }

            Task.WaitAll(tasks.ToArray());

            response.TotalRows = dataset.TotalRows;

            return response;
        }

        /// <summary>
        /// Generates the script for the search.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="customSearchGridData">The parameter of the search.</param>
        /// <param name="searchGroups">Search for groups.</param>
        /// <returns>The search script.</returns>
        /// <exception cref = "CustomSearchesRequestBodyException" > Required parameter was not added or parameter has an invalid value.</exception>
        public string GenerateSearchScript(Dataset dataset, bool usePostProcess, DatasetPostProcess datasetPostProcess, CustomSearchGridData customSearchGridData, bool searchGroups)
        {
            string script = string.Empty;

            string viewName = DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess);

            string sortScript = this.GenerateSortScript(customSearchGridData, searchGroups);

            script += "SELECT  *\n";
            script += "FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY " + sortScript + " ) AS RowNum, *\n";

            string groupsString = string.Empty;

            if (searchGroups)
            {
                groupsString = this.GenerateGroupsScript(customSearchGridData);
                script += "FROM(\n";
                script += "SELECT  " + groupsString + "\n";
                script += "FROM    ( SELECT    *\n";
            }

            script += "          FROM      " + viewName + "\n";

            string whereScript = string.Empty;
            if ((customSearchGridData.RowGroupCols?.Length > 0) && (customSearchGridData.GroupKeys?.Count > 0))
            {
                int i = 0;
                foreach (var rowGroupCol in customSearchGridData.RowGroupCols)
                {
                    if (customSearchGridData.GroupKeys.Count > i)
                    {
                        if (string.IsNullOrWhiteSpace(whereScript) == false)
                        {
                            whereScript += " AND ";
                        }

                        whereScript += "                    " + rowGroupCol.Field + " = '" + customSearchGridData.GroupKeys[i] + "'";
                    }
                    else
                    {
                        break;
                    }

                    i++;
                }
            }

            string filterScript = GenerateFilterScript(customSearchGridData.FilterModel, tableAlias: string.Empty);
            if (!string.IsNullOrWhiteSpace(filterScript) && !string.IsNullOrWhiteSpace(whereScript))
            {
                whereScript += " AND ";
            }

            whereScript += filterScript;

            if (!string.IsNullOrWhiteSpace(whereScript))
            {
                script += "          WHERE\n";
                script += whereScript;
            }

            script += "        ) AS RowConstrainedResult\n";

            if (searchGroups)
            {
                script += "GROUP BY " + groupsString + "\n";
                script += "	) GroupSelect) RowNumberSelect\n";
            }

            InputValidationHelper.AssertShouldBeGreaterThan(customSearchGridData.StartRow, -1, nameof(customSearchGridData.StartRow));
            InputValidationHelper.AssertShouldBeGreaterThan(customSearchGridData.EndRow, customSearchGridData.StartRow, nameof(customSearchGridData.EndRow));

            script += "WHERE   RowNum > " + customSearchGridData.StartRow + "\n";
            script += "    AND RowNum <= " + customSearchGridData.EndRow + "\n";

            return script;
        }

        /// <summary>
        /// Generates the groups script for the search.
        /// </summary>
        /// <param name="customSearchGridData">The parameter of the search.</param>
        /// <returns>The groups script.</returns>
        public string GenerateGroupsScript(CustomSearchGridData customSearchGridData)
        {
            string groupsScript = string.Empty;

            if ((customSearchGridData.RowGroupCols != null) &&
                (customSearchGridData.RowGroupCols.Length > 0) &&
                (customSearchGridData.GroupKeys != null))
            {
                int i = 0;
                foreach (var rowGroupCol in customSearchGridData.RowGroupCols)
                {
                    if (i <= customSearchGridData.GroupKeys.Count)
                    {
                        if (string.IsNullOrWhiteSpace(groupsScript) == false)
                        {
                            groupsScript += ", ";
                        }

                        groupsScript += rowGroupCol.Field;
                    }

                    i++;
                }
            }

            return groupsScript;
        }

        /// <summary>
        /// Generates the sort script for the search.
        /// </summary>
        /// <param name="customSearchGridData">The parameter of the search.</param>
        /// <param name="searchGroups">Search for groups.</param>
        /// <returns>The sort script.</returns>
        public string GenerateSortScript(CustomSearchGridData customSearchGridData, bool searchGroups)
        {
            string sortScript = string.Empty;

            if (searchGroups)
            {
                sortScript = this.GenerateGroupsScript(customSearchGridData);
            }
            else
            {
                if ((customSearchGridData.SortModel != null) && (customSearchGridData.SortModel.Length > 0))
                {
                    foreach (CustomSearchGridSortingData entry in customSearchGridData.SortModel)
                    {
                        if (string.IsNullOrWhiteSpace(sortScript) == false)
                        {
                            sortScript += ", ";
                        }

                        sortScript += entry.ColId + " " + entry.Sort;
                    }
                }
                else
                {
                    sortScript = "CustomSearchResultId";
                }
            }

            return sortScript;
        }

        /// <summary>
        /// Gets the updated columns per row from the raw array of updates.
        /// </summary>
        /// <param name="rowUpdates">The row updates.</param>
        /// <returns>An array with updated columns.</returns>
        private RowUpdatedColumnsData[] GetUpdatedColumns(dynamic[] rowUpdates)
        {
            var toReturn = new List<RowUpdatedColumnsData>();
            foreach (var update in rowUpdates)
            {
                var updateDictionary = update as IDictionary<string, object>;
                RowUpdatedColumnsData currentRowUpdates = new RowUpdatedColumnsData();
                currentRowUpdates.CustomSearchResultId = (int)updateDictionary["CustomSearchResultId"];
                var updatedColumns = new List<string>();

                foreach (var updatedColumn in updateDictionary.Keys)
                {
                    if (!DatasetColumnHelper.IsDefaultNotEditableColumn(updatedColumn))
                    {
                        var value = updateDictionary[updatedColumn];
                        if (value != null && value.GetType() != typeof(DBNull))
                        {
                            updatedColumns.Add(updatedColumn);
                        }
                    }
                }

                if (updatedColumns.Count > 0)
                {
                    currentRowUpdates.UpdatedColumns = updatedColumns.ToArray();
                    toReturn.Add(currentRowUpdates);
                }
            }

            return toReturn.ToArray();
        }
    }
}