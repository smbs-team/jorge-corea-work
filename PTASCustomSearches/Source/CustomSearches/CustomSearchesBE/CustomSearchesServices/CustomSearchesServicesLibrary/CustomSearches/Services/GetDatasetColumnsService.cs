namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the dataset columns.
    /// </summary>
    public class GetDatasetColumnsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetColumnsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetColumnsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the dataset columns.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId">The post process id.</param>
        /// <param name="includeDependencies">if set to <c>true</c> [include dependencies].</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The dataset columns.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process was not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        public async Task<GetDatasetColumnsResponse> GetDatasetColumnsAsync(
            Guid datasetId,
            bool usePostProcess,
            int? postProcessId,
            bool includeDependencies,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset
                .Where(d => (d.DatasetId == datasetId))
                .Include(d => d.CustomSearchDefinition)
                .FirstOrDefaultAsync();

            var columnDefinitions = await (from cd in dbContext.CustomSearchColumnDefinition
                                           join d in dbContext.Dataset
                                           on cd.CustomSearchDefinitionId equals d.CustomSearchDefinitionId
                                           where d.DatasetId == datasetId
                                           select cd).ToListAsync();

            var columnDefinitionIds = columnDefinitions.Select(cscd => cscd.CustomSearchColumnDefinitionId);

            await (from e in dbContext.CustomSearchExpression
                   where e.DatasetId == datasetId || columnDefinitionIds.Contains((int)e.CustomSearchColumnDefinitionId)
                   select e).LoadAsync();

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

            GetDatasetColumnsResponse response = new GetDatasetColumnsResponse();

            Dictionary<string, CustomSearchExpression> calculatedExpressions = dataset.CustomSearchExpression
                .Where(c =>
                    (c.ExpressionRole == CustomSearchExpressionRoleType.CalculatedColumn.ToString())
                    && (c.ExpressionType == CustomSearchExpressionType.TSQL.ToString()))
                .ToDictionary(e => e.ColumnName.ToLower());

            string datasetView = DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess);
            ReadOnlyCollection<DbColumn> dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

            response.DatasetColumns = new CustomSearchColumnDefinitionData[dbColumns.Count];

            for (int i = 0; i < dbColumns.Count; i++)
            {
                DbColumn dbColumn = dbColumns[i];
                CustomSearchColumnDefinition colDef = columnDefinitions.FirstOrDefault(c => c.ColumnName.ToLower() == dbColumn.ColumnName.ToLower());

                bool foundColDef = colDef != null;
                bool isIndexed = this.IsDatabaseColumnIndexable(dbColumn);

                bool hasEditLookupExpression = false;

                bool isEditable = false;

                if (foundColDef)
                {
                    hasEditLookupExpression = colDef.CustomSearchExpression
                        .FirstOrDefault(c => (c.ExpressionRole == CustomSearchExpressionRoleType.EditLookupExpression.ToString())) != null;

                    if (this.ServiceContext.AuthProvider.IsAuthorizedToAnyRole(dataset.CustomSearchDefinition.DatasetEditRoles) &&
                        this.ServiceContext.AuthProvider.IsAuthorizedToAnyRole(colDef.ColumnEditRoles))
                    {
                        isEditable = colDef.IsEditable;
                    }
                }
                else
                {
                    isEditable = DatasetColumnHelper.IsSelectionOrFilterColumn(dbColumn.ColumnName);
                }

                bool isCalculatedColumn = calculatedExpressions.ContainsKey(dbColumn.ColumnName.ToLower());

                response.DatasetColumns[i] = new CustomSearchColumnDefinitionData
                {
                    CustomSearchDefinitionId = dataset.CustomSearchDefinitionId,
                    ColumnName = dbColumn.ColumnName,
                    ColumnType = dbColumn.DataType.Name.ToString(),
                    ColumnTypeLength = dbColumn.ColumnSize ?? 0,
                    CanBeUsedAsLookup = foundColDef ? (bool)colDef.CanBeUsedAsLookup : false,
                    ColumnCategory = foundColDef ? colDef.ColumnCategory : null,
                    IsEditable = isEditable,
                    HasEditLookupExpression = hasEditLookupExpression,
                    ForceEditLookupExpression = foundColDef ? colDef.ForceEditLookupExpression : false,
                    ColumDefinitionExtensions = JsonHelper.DeserializeObject(colDef?.ColumDefinitionExtensions),
                    IsIndexed = isIndexed,
                    IsCalculatedColumn = isCalculatedColumn,
                };

                if (includeDependencies)
                {
                    var toIncludeExpressions = new List<CustomSearchExpressionData>();

                    if (isCalculatedColumn)
                    {
                        CustomSearchExpression customSearchExpression = calculatedExpressions[dbColumn.ColumnName.ToLower()];
                        CustomSearchExpressionData customSearchExpressionData = new CustomSearchExpressionData(customSearchExpression, ModelInitializationType.Summary);
                        toIncludeExpressions.Add(customSearchExpressionData);
                    }

                    if (foundColDef)
                    {
                        foreach (var columnExpression in colDef.CustomSearchExpression)
                        {
                            CustomSearchExpressionData colDefExpressionData = new CustomSearchExpressionData(columnExpression, ModelInitializationType.Summary);
                            toIncludeExpressions.Add(colDefExpressionData);
                        }
                    }

                    response.DatasetColumns[i].Expressions = toIncludeExpressions.ToArray();
                }
            }

            return response;
        }

        /// <summary>
        /// Checks if the column can be indexed.
        /// </summary>
        /// <param name="column">The database column.</param>
        /// <returns>Return true if the column can be indexed, otherwise false.</returns>
        private bool IsDatabaseColumnIndexable(DbColumn column)
        {
            // The maximum key length for a nonclustered index is 1700 bytes. Columns that are of the large object (LOB) data types
            // ntext, text, varchar(max), nvarchar(max), varbinary(max), xml, or image cannot be specified as key columns for an index.
            string colType = column.DataTypeName;
            int? colSize = column.ColumnSize;

            if (colSize > 1700)
            {
                return false;
            }

            if ((colType == "ntext") || (colType == "text") || (colType == "image") || (colType == "xml"))
            {
                return false;
            }

            if ((colSize > 850) && ((colType == "nchar") || (colType == "nvarchar")))
            {
                return false;
            }

            return true;
        }
    }
}
