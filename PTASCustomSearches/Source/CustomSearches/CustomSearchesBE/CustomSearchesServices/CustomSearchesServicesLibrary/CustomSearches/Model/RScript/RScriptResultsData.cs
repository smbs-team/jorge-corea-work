namespace CustomSearchesServicesLibrary.CustomSearches.Model.RScript
{
    /// <summary>
    /// Model for RScript results.
    /// </summary>
    public class RScriptResultsData
    {
        /// <summary>
        /// Gets or sets the status of the R script execution.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///  Gets or sets the result of the execution.
        /// </summary>
        public object[] Results { get; set; }

        /// <summary>
        /// Gets or sets the file results.
        /// </summary>
        public RScriptFileResult[] FileResults { get; set; }
    }
}
