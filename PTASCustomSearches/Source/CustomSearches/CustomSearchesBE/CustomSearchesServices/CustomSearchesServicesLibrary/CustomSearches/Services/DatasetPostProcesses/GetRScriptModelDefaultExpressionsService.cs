namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the default custom expressions for the RScriptModel.
    /// </summary>
    public class GetRScriptModelDefaultExpressionsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRScriptModelDefaultExpressionsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetRScriptModelDefaultExpressionsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the  default custom expressions for a RScriptModel.
        /// </summary>
        /// <param name="rScriptModelId">RScriptModel id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The custom expressions for the RScriptModel.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Expressions for rscript model were not found.</exception>
        public async Task<GetRScriptModelDefaultExpressionsResponse> GetRScriptModelDefaultExpressionsAsync(
            int rScriptModelId,
            CustomSearchesDbContext dbContext)
        {
            GetRScriptModelDefaultExpressionsResponse response = new GetRScriptModelDefaultExpressionsResponse();

            var rScriptModel = await dbContext.RscriptModel
                .Where(m => m.RscriptModelId == rScriptModelId)
                .Include(m => m.CustomSearchExpression)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(rScriptModel, "RscriptModel", rScriptModelId);

            var expressions = rScriptModel.CustomSearchExpression.OrderBy(e => e.ExecutionOrder).ToArray();
            if (expressions.Count() > 0)
            {
                response.Expressions = new CustomSearchExpressionData[expressions.Length];
                for (int i = 0; i < expressions.Length; i++)
                {
                    var item = expressions[i];
                    response.Expressions[i] = new CustomSearchExpressionData(item, ModelInitializationType.Summary);
                }
            }

            return response;
        }
    }
}