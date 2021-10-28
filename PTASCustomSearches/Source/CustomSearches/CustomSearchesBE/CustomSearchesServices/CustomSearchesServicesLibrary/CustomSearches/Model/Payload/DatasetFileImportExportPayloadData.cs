namespace CustomSearchesServicesLibrary.CustomSearches.Model.Payload
{
    using CustomSearchesServicesLibrary.Enumeration;

    /// <summary>
    /// Base model for dataset import/export payloads.
    /// </summary>
    public class DatasetFileImportExportPayloadData : DatasetPayloadData
    {
        /// <summary>
        /// Gets or sets the dataset data.
        /// </summary>
        public string DatasetData { get; set; }

        /// <summary>
        /// Gets or sets the file type.
        /// </summary>
        public DatasetFileImportExportType FileType { get; set; }
    }
}
