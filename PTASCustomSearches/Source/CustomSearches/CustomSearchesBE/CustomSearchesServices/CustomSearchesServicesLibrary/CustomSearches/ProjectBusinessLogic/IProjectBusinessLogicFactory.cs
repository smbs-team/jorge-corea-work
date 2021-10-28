namespace CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic
{
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Project business logic factory interface.
    /// </summary>
    public interface IProjectBusinessLogicFactory
    {
        /// <summary>
        /// Creates project business logic instances.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A new project business logic.</returns>
        DefaultProjectBusinessLogic CreateProjectBusinessLogic(
            UserProject userProject,
            CustomSearchesDbContext dbContext);
    }
}
