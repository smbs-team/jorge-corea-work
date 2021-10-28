namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System.Data.Common;

    /// <summary>
    /// Helper for database column.
    /// </summary>
    public static class DatabaseColumnHelper
    {
        /// <summary>
        /// Gets the data type definition, based on schema definition for a column.
        /// </summary>
        /// <param name="column">The database column.</param>
        /// <returns>Return T-SQL data type definition.</returns>
        public static string GetDatabaseType(DbColumn column)
        {
            switch (column.DataTypeName)
            {
                case "nvarchar":
                    return $"{column.DataTypeName}(" + ((column.ColumnSize == -1) ? "255" : (column.ColumnSize > 4000) ? "MAX" : column.ColumnSize.ToString()) + ")";

                case "varbinary":
                case "varchar":
                    return $"{column.DataTypeName}(" + ((column.ColumnSize == -1) ? "255" : (column.ColumnSize > 8000) ? "MAX" : column.ColumnSize.ToString()) + ")";

                case "binary":
                case "char":
                case "nchar":
                    return $"{column.DataTypeName}({column.ColumnSize})";

                case "decimal":
                case "numeric":
                    return $"{column.DataTypeName}({column.NumericPrecision},{column.NumericScale})";

                default:
                    return column.DataTypeName;
            }
        }

        /// <summary>
        /// Checks if the column can be indexed.
        /// </summary>
        /// <param name="column">The database column.</param>
        /// <returns>Return true if the column can be indexed, otherwise false.</returns>
        public static bool IsDatabaseColumnIndexable(DbColumn column)
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
