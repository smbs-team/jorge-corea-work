namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using CustomSearchesServicesLibrary.Auth.Model;

    /// <summary>
    /// Model for the response of the GetDatasetsForUser service.
    /// </summary>
    public class GetDatasetsForUserResponse
    {
        /// <summary>
        /// Gets or sets the datasts.
        /// </summary>
        public DatasetData[] Datasets { get; set; }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public UserInfoData[] UsersDetails { get; set; }
    }
}
