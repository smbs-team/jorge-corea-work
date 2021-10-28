namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Dataset column helper.  Contains helper functions for manipulating dataset columns.
    /// </summary>
    public class DatasetColumnHelper
    {
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
        /// The default editable column names.
        /// </summary>
        public static readonly string[] DefaultEditableColumnNames =
            {
                DatasetColumnHelper.SelectionColumnName, DatasetColumnHelper.FilterStateColumnName
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
        ///   <c>true</c> if the column name is a default non editable column; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDefaultNotEditableColumn(string columName)
        {
            return
                (from nec in DatasetColumnHelper.DefaultNotEditableColumnNames
                 where nec.ToLower() == columName.ToLower()
                 select nec).
                 FirstOrDefault() != null;
        }

        /// <summary>
        /// Determines whether the passed column name is one of the default editable columns for the dataset.
        /// </summary>
        /// <param name="columName">Name of the column.</param>
        /// <returns>
        ///   <c>true</c> if the column name is a default editable column; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDefaultEditableColumn(string columName)
        {
            return
                (from nec in DatasetColumnHelper.DefaultEditableColumnNames
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
            return await DatasetColumnHelper.GetEditableColumnsAsync(dataset.CustomSearchDefinitionId, dbContext);
        }

        /// <summary>
        /// Gets the editable columns for the dataset.
        /// </summary>
        /// <param name="customSearchDefinitionId">The custom search definitino id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The list of editable columns.</returns>
        public static async Task<List<CustomSearchColumnDefinition>> GetEditableColumnsAsync(int customSearchDefinitionId, CustomSearchesDbContext dbContext)
        {
            return await (from ec in dbContext.CustomSearchColumnDefinition
                          where ec.CustomSearchDefinitionId == customSearchDefinitionId && ec.IsEditable == true
                          select ec).ToListAsync();
        }

        /// <summary>
        /// Gets the select statement that can be used to retrieve all the values for a dataset column, taking into account any override.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>
        /// The select statement used for retrieving all column values.
        /// </returns>
        public static async Task<string> GetColumnValuesSelectStatement(
            IFactory<CustomSearchesDbContext> dbContextFactory,
            Dataset dataset,
            string columnName,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess)
        {
            if (!usePostProcess)
            {
                using (CustomSearchesDbContext dbContext = dbContextFactory.Create())
                {
                    return await GetColumnValuesSelectStatement(dbContext, dataset, columnName, usePostProcess, datasetPostProcess);
                }
            }

            return DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess);
        }

        /// <summary>
        /// Gets the select statement that can be used to retrieve all the values for a dataset column, taking into account any override.
        /// </summary>
        /// <param name="dbContext">The custom searches db context.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>
        /// The select statement used for retrieving all column values.
        /// </returns>
        public static async Task<string> GetColumnValuesSelectStatement(
            CustomSearchesDbContext dbContext,
            Dataset dataset,
            string columnName,
            bool usePostProcess,
            DatasetPostProcess datasetPostProcess)
        {
            if (!usePostProcess)
            {
                CustomSearchExpression columnDefinitionRangeOverride = await
                    (from cse in dbContext.CustomSearchExpression
                     join cd in dbContext.CustomSearchColumnDefinition
                         on cse.CustomSearchColumnDefinitionId equals cd.CustomSearchColumnDefinitionId
                     join csd in dbContext.CustomSearchDefinition
                         on cd.CustomSearchDefinitionId equals csd.CustomSearchDefinitionId
                     where cd.CustomSearchDefinitionId == dataset.CustomSearchDefinitionId && cd.ColumnName.ToLower() == columnName.ToLower()
                        && cse.ExpressionRole == CustomSearchExpressionRoleType.RangedValuesOverrideExpression.ToString()
                     select cse).FirstOrDefaultAsync();

                if (columnDefinitionRangeOverride != null)
                {
                    return columnDefinitionRangeOverride.Script;
                }
            }

            return $"SELECT [{columnName}] FROM {DatasetHelper.GetDatasetView(dataset, usePostProcess, datasetPostProcess)}";
        }
    }
}
