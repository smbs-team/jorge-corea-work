namespace CustomSearchesServicesLibrary.CustomSearches.Model.Payload
{
    using CustomSearchesServicesLibrary.Enumeration;

    /// <summary>
    /// Base model for dataset import/export SignalR payloads.
    /// </summary>
    public class DatasetFileImportExportSignalRPayloadData : DatasetPayloadData
    {
        /// <summary>
        /// Gets or sets the file type.
        /// </summary>
        public DatasetFileImportExportType FileType { get; set; }
    }
}
