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
    /// Service that gets the projects for user.
    /// </summary>
    public class GetUserProjectsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserProjectsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserProjectsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the projects for a user.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The projects for user.
        /// </returns>
        public async Task<GetUserProjectsResponse> GetUserProjectsAsync(Guid userId, CustomSearchesDbContext dbContext)
        {
            GetUserProjectsResponse response = new GetUserProjectsResponse();

            var projects = await (from p in dbContext.UserProject where p.UserId == userId select p).
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
