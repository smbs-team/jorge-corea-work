namespace CustomSearchesServicesLibrary.CustomSearches
{
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Dataset script builder.
    /// </summary>
    public static class DatasetScriptBuilder
    {
        /// <summary>
        /// Generates the script that gets total updated rows.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>
        /// The search script.
        /// </returns>
        public static string BuildGetUpdatedRowCountScript(Dataset dataset)
        {
            string updateTableName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset);

            return $"SELECT COUNT(u.Validated) FROM {updateTableName} u WHERE ISNULL(u.Validated, 0) != 0";
        }

        /// <summary>
        /// Generates the script that gets total exported rows.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>
        /// The search script.
        /// </returns>
        public static string BuildGetExportedRowCountScript(Dataset dataset)
        {
            string script = string.Empty;

            string updateTableName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset);

            return $"SELECT COUNT(u.BackendExportState) FROM {updateTableName} u WHERE ISNULL(u.BackendExportState, '') != 'NotExported'";
        }
    }
}
