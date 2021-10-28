namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using CustomSearchesServicesLibrary.Auth.Model;

    /// <summary>
    /// Model for the response of the GetDatase service.
    /// </summary>
    public class GetDatasetResponse
    {
        /// <summary>
        /// Gets or sets the dataset project role.
        /// </summary>
        public string DatasetProjectRole { get; set; }

        /// <summary>
        /// Gets or sets the dataset data.
        /// </summary>
        public DatasetData Dataset { get; set; }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public UserInfoData[] UsersDetails { get; set; }
    }
}
