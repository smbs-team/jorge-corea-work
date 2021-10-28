namespace CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses
{
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;

    /// <summary>
    /// Model for the response of GetDatasetPostProcess service.
    /// </summary>
    public class GetDatasetPostProcessResponse
    {
        /// <summary>
        /// Gets or sets the post process.
        /// </summary>
        public DatasetPostProcessData PostProcess { get; set; }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public UserInfoData[] UsersDetails { get; set; }
    }
}
