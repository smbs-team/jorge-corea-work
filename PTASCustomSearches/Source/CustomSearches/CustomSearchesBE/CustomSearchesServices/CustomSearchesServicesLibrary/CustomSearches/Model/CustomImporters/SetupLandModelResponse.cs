namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;

    /// <summary>
    /// Model for the response of the SetupLandModel service.
    /// </summary>
    public class SetupLandModelResponse
    {
        /// <summary>
        /// Gets or sets the id of the result.
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the post processes.
        /// </summary>
        public DatasetPostProcessData[] PostProcesses { get; set; }
    }
}
