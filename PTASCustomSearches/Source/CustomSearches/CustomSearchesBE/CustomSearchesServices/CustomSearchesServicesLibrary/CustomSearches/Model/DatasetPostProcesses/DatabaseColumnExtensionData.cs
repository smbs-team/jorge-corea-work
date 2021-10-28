namespace CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses
{
    /// <summary>
    /// Model for database column extension data.
    /// </summary>
    public class DatabaseColumnExtensionData
    {
        /// <summary>
        /// Gets or sets the database type.
        /// </summary>
        public string DatabaseType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the database column is indexable.
        /// </summary>
        public bool IsDatabaseColumnIndexable { get; set; }
    }
}
