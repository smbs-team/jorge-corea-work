namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets all the projects.
    /// </summary>
    public class GetProjectsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProjectsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetProjectsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the projects for all users.
        /// </summary>
        /// <param name="includedYears">Limits the result to the projects of the last n years.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The projects for all users.
        /// </returns>
        public async Task<GetUserProjectsResponse> GetProjectsAsync(int? includedYears, CustomSearchesDbContext dbContext)
        {
            GetUserProjectsResponse response = new GetUserProjectsResponse();

            DateTime startFrom = DateTime.UtcNow.AddYears(-(includedYears ?? 0));

            var projects = await (from p in dbContext.UserProject where (includedYears == null) || (p.CreatedTimestamp >= startFrom) select p).
                Include(p => p.ProjectType).
                ThenInclude(pt => pt.ProjectTypeCustomSearchDefinition).
                Include(p => p.CreatedByNavigation).
                Include(p => p.LastModifiedByNavigation).
                Include(p => p.User).
                ToArrayAsync();

            if (projects != null)
            {
                Dictionary<Guid, UserInfoData> userDetails = new Dictionary<Guid, UserInfoData>();
                response.Projects = new ProjectData[projects.Length];
                for (int i = 0; i < projects.Length; i++)
                {
                    var item = projects[i];
                    response.Projects[i] = new ProjectData(item, ModelInitializationType.Summary, userDetails);
                }

                response.UsersDetails = UserDetailsHelper.GetUserDetailsArray(userDetails);
            }

            return response;
        }
    }
}
