namespace CustomSearchesServicesLibrary.CustomSearches.Model.RScript
{
    /// <summary>
    /// Types of files that RScript can return.
    /// </summary>
    public enum RScriptResultFileType
    {
        /// <summary>
        /// The report file type.
        /// </summary>
        ReportFileType,

        /// <summary>
        /// The chart file type
        /// </summary>
        ChartFileType
    }

    /// <summary>
    /// Model for RScript results.
    /// </summary>
    public class RScriptFileResult
    {
        /// <summary>
        /// Gets or sets the title of the result.  Title can be used to group several outputs for the same report.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the status of the R script execution.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the result file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the description of the file.
        /// </summary>
        public string Description { get; set; }
    }
}
