namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using CustomSearchesServicesLibrary.Auth.Model;

    /// <summary>
    /// Model for the response of the GetUserProject service.
    /// </summary>
    public class GetUserProjectResponse
    {
        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        public ProjectData Project { get; set; }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public UserInfoData[] UsersDetails { get; set; }
    }
}
