namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using CustomSearchesServicesLibrary.Auth.Model;

    /// <summary>
    /// Model for the response of the GetUserProjects service.
    /// </summary>
    public class GetUserProjectsResponse
    {
        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        public ProjectData[] Projects { get; set; }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public UserInfoData[] UsersDetails { get; set; }
    }
}
