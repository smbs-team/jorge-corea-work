namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Model for the response of the GetProjectTypes service.
    /// </summary>
    public class GetProjectTypesResponse
    {
        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        public ProjectTypeData[] ProjectTypes { get; set; }
    }
}
