namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;

    /// <summary>
    /// Project helper.
    /// </summary>
    public class ProjectHelper
    {
        /// <summary>
        /// Gets the project with its dependencies.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The project with all its dependencies.
        /// </returns>
        public static async Task<UserProject> GetProjectWithDependencies(int projectId, CustomSearchesDbContext dbContext)
        {
            UserProject project = await dbContext.UserProject.
                    Where(p => p.UserProjectId == projectId).
                    Include(p => p.CreatedByNavigation).
                    Include(p => p.LastModifiedByNavigation).
                    Include(p => p.User).
                    Include(p => p.ProjectType).
                    Include(p => p.UserProjectDataset).
                    FirstOrDefaultAsync();

            var datasetIds = project.UserProjectDataset.Select(pd => pd.DatasetId).ToHashSet();

            await (from d in dbContext.Dataset where datasetIds.Contains(d.DatasetId) select d).LoadAsync();
            await (from p in dbContext.DatasetPostProcess where datasetIds.Contains(p.DatasetId) select p).LoadAsync();
            await (from c in dbContext.InteractiveChart where datasetIds.Contains(c.DatasetId) select c).LoadAsync();

            await (from u in dbContext.Systemuser
                   from c in dbContext.InteractiveChart
                   where u.Systemuserid == c.CreatedBy
                   || u.Systemuserid == c.LastModifiedBy
                   join d in dbContext.Dataset
                   on c.DatasetId equals d.DatasetId
                   join pd in dbContext.UserProjectDataset
                   on d.DatasetId equals pd.DatasetId
                   join p in dbContext.UserProject
                   on pd.UserProjectId equals p.UserProjectId
                   where pd.UserProjectId == project.UserProjectId
                   select u).
                Union(from u in dbContext.Systemuser
                      from pp in dbContext.DatasetPostProcess
                      where u.Systemuserid == pp.CreatedBy
                          || u.Systemuserid == pp.LastModifiedBy
                      join d in dbContext.Dataset
                          on pp.DatasetId equals d.DatasetId
                      join pd in dbContext.UserProjectDataset
                          on d.DatasetId equals pd.DatasetId
                      join p in dbContext.UserProject
                          on pd.UserProjectId equals p.UserProjectId
                      where pd.UserProjectId == project.UserProjectId
                      select u).
                    Union(from u in dbContext.Systemuser
                          from d in dbContext.Dataset
                          where u.Systemuserid == d.CreatedBy
                              || u.Systemuserid == d.LastModifiedBy
                              || u.Systemuserid == d.UserId
                          join pd in dbContext.UserProjectDataset
                              on d.DatasetId equals pd.DatasetId
                          join p in dbContext.UserProject
                              on pd.UserProjectId equals p.UserProjectId
                          where pd.UserProjectId == project.UserProjectId
                          select u)
                    .LoadAsync();

            return project;
        }
    }
}