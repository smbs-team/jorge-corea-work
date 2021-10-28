namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Custom search definition helper.
    /// </summary>
    public class CustomSearchDefinitionHelper
    {
        /// <summary>
        /// Gets the latest version of the custom search definition.
        /// </summary>
        /// <param name="customSearchDefinitionId">The custom search definition id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The latest version of the custom search definition.
        /// </returns>
        public static async Task<CustomSearchDefinition> GetCustomSearchDefinitionLatestVersion(int customSearchDefinitionId, CustomSearchesDbContext dbContext)
        {
            var customSearchDefinition = await dbContext.CustomSearchDefinition
                .Where(d => d.CustomSearchDefinitionId == customSearchDefinitionId)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(customSearchDefinition, nameof(CustomSearchDefinition), customSearchDefinitionId);

            return await CustomSearchDefinitionHelper.GetCustomSearchDefinitionLatestVersion(customSearchDefinition.CustomSearchName, dbContext);
        }

        /// <summary>
        /// Gets the latest version of the custom search definition.
        /// </summary>
        /// <param name="customSearchDefinitionName">The custom search definition name.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The latest version of the custom search definition.
        /// </returns>
        public static async Task<CustomSearchDefinition> GetCustomSearchDefinitionLatestVersion(string customSearchDefinitionName, CustomSearchesDbContext dbContext)
        {
            var customSearchDefinition = await dbContext.CustomSearchDefinition
                    .Where(d => d.CustomSearchName.Trim().ToLower() == customSearchDefinitionName.Trim().ToLower())
                    .OrderByDescending(d => d.Version)
                    .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(customSearchDefinition, nameof(CustomSearchDefinition), customSearchDefinitionName);

            return customSearchDefinition;
        }

        /// <summary>
        /// Gets a value indicating whether the version of the custom search definition is outdated.
        /// </summary>
        /// <param name="customSearchDefinitionId">The custom search definition id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// A value indicating whether the version of the custom search definition is outdated.
        /// </returns>
        public static async Task<bool> GetIsCustomSearchDefinitionOutdated(int customSearchDefinitionId, CustomSearchesDbContext dbContext)
        {
            var customSearchDefinition =
                await CustomSearchDefinitionHelper.GetCustomSearchDefinitionLatestVersion(customSearchDefinitionId, dbContext);

            return customSearchDefinition.CustomSearchDefinitionId != customSearchDefinitionId;
        }
    }
}