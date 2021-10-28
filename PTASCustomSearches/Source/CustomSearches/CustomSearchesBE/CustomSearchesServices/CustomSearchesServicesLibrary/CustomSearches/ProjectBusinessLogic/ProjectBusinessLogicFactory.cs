namespace CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic
{
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Project business logic factory class.
    /// </summary>
    /// <seealso cref="IProjectBusinessLogicFactory" />
    public class ProjectBusinessLogicFactory : IProjectBusinessLogicFactory
    {
        /// <summary>
        /// Creates project business logic instances.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A new project business logic.</returns>
        public DefaultProjectBusinessLogic CreateProjectBusinessLogic(UserProject userProject, CustomSearchesDbContext dbContext)
        {
            switch (userProject?.ProjectType.ProjectTypeName.ToLower())
            {
                case "physical inspection":
                    return new PhysicalInspectionProjectBusinessLogic(userProject, dbContext);
                case "annual update":
                    return new AnnualUpdateProjectBusinessLogic(userProject, dbContext);
                default:
                    return new DefaultProjectBusinessLogic(dbContext);
            }
        }
    }
}
