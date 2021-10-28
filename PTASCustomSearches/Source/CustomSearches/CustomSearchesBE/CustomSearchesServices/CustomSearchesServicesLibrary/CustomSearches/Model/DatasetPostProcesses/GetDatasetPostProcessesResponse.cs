namespace CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;

    /// <summary>
    /// Model for the response of the GetDatasetPostProcesses service.
    /// </summary>
    public class GetDatasetPostProcessesResponse
    {
        /// <summary>
        /// Gets or sets the post processes.
        /// </summary>
        public DatasetPostProcessData[] PostProcesses { get; set; }
    }
}
