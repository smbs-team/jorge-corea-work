namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the project types.
    /// </summary>
    public class GetProjectTypesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProjectTypesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetProjectTypesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the project types.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The projects types.
        /// </returns>
        public async Task<GetProjectTypesResponse> GetProjectTypesAsync(CustomSearchesDbContext dbContext)
        {
            var response = new GetProjectTypesResponse();

            response.ProjectTypes = await
                (from pt in dbContext.ProjectType.Include(p => p.ProjectTypeCustomSearchDefinition)
                 select new ProjectTypeData(pt, ModelInitializationType.FullObject)).
                 ToArrayAsync();

            return response;
        }
    }
}
