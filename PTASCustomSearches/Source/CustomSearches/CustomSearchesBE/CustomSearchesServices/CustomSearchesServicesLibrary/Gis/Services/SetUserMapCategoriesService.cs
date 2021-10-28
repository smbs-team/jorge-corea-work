namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that sets the user map categories.
    /// </summary>
    public class SetUserMapCategoriesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetUserMapCategoriesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public SetUserMapCategoriesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Sets the user map categories.
        /// </summary>
        /// <param name="userMapId">User map id.</param>
        /// <param name="setUserMapCategoriesData">User map categories to set.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task SetUserMapCategories(int userMapId, SetUserMapCategoriesData setUserMapCategoriesData, GisDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("SetUserMapCategories");

            UserMap userMap = await dbContext.UserMap
                .Where(m => m.UserMapId == userMapId)
                .Include(m => m.UserMapCategoryUserMap)
                    .ThenInclude(c => c.UserMapCategory)
                .FirstOrDefaultAsync();

            if (userMap == null)
            {
                throw new CustomSearchesEntityNotFoundException(
                    $"User map with id '{userMapId}' not found.",
                    null);
            }

            dbContext.UserMapCategoryUserMap.RemoveRange(userMap.UserMapCategoryUserMap);

            userMap.UserMapCategoryUserMap.Clear();

            if ((setUserMapCategoriesData != null) &&
                (setUserMapCategoriesData.UserMapCategories != null) &&
                (setUserMapCategoriesData.UserMapCategories.Length > 0))
            {
                var categories = await dbContext.UserMapCategory.ToDictionaryAsync(m => m.CategoryName.ToLower());

                HashSet<string> assignedCategories = new HashSet<string>();
                foreach (var categoryData in setUserMapCategoriesData.UserMapCategories)
                {
                    string categoryKey = categoryData.CategoryName.ToLower();

                    // Checks assigned categories to avoid repetitions.
                    if (!assignedCategories.Contains(categoryKey))
                    {
                        assignedCategories.Add(categoryKey);

                        UserMapCategory userMapCategory;
                        if (categories.ContainsKey(categoryKey))
                        {
                            userMapCategory = categories[categoryKey];
                        }
                        else
                        {
                            userMapCategory = new UserMapCategory()
                            {
                                CategoryName = categoryData.CategoryName,
                                CategoryDescription = categoryData.CategoryDescription
                            };
                        }

                        UserMapCategoryUserMap userMapCategoryUserMap = new UserMapCategoryUserMap();
                        userMapCategoryUserMap.UserMap = userMap;
                        userMapCategoryUserMap.UserMapCategory = userMapCategory;
                        dbContext.UserMapCategoryUserMap.Add(userMapCategoryUserMap);
                    }
                }
            }

            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
