namespace CustomSearchesServicesLibrary.CustomSearches.Services.RScriptModel
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScriptModel;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets a rscript models.
    /// </summary>
    public class GetRScriptModelsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRScriptModelsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetRScriptModelsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the rscript models.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="includeDeleted">if set to <c>true</c> includes the soft-deleted models.</param>
        /// <returns>
        /// The rscript models.
        /// </returns>
        public async Task<GetRScriptModelsResponse> GetRScriptModelsAsync(CustomSearchesDbContext dbContext, bool includeDeleted)
        {
            GetRScriptModelsResponse response = new GetRScriptModelsResponse();

            var query = from rm in dbContext.RscriptModel select rm;
            if (!includeDeleted)
            {
                query = query.Where(rm => rm.IsDeleted == false);
            }

            RscriptModel[] rscriptModels = await query.ToArrayAsync();

            if (rscriptModels != null)
            {
                response.RscriptModels = new RScriptModelData[rscriptModels.Length];
                for (int i = 0; i < rscriptModels.Length; i++)
                {
                    var rscriptModel = rscriptModels[i];
                    RScriptModelData rscriptModelData = new RScriptModelData(rscriptModel, ModelInitializationType.Summary);
                    response.RscriptModels[i] = rscriptModelData;
                }
            }

            return response;
        }
    }
}
