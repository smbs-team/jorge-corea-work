namespace CustomSearchesServicesLibrary.CustomSearches.Services.RScriptModel
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScriptModel;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets a rscript model.
    /// </summary>
    public class GetRScriptModelService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRScriptModelService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetRScriptModelService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the rscript model.
        /// </summary>
        /// <param name="rscriptModelId">The rscript model id.</param>
        /// <param name="includeLookupValues">Value indicating whether the results should include the lookup values.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The rscript model.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search expression was not found.</exception>
        /// <exception cref="CustomSearchesJsonException">Exception while evaluating json expression.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Exception while evaluating sql expression.</exception>
        /// <exception cref="InvalidExpressionResultException">Invalid expression results.</exception>
        public async Task<GetRScriptModelResponse> GetRScriptModelAsync(
            int rscriptModelId,
            bool includeLookupValues,
            CustomSearchesDbContext dbContext)
        {
            GetRScriptModelResponse response = new GetRScriptModelResponse();

            RscriptModel rscriptModel = await dbContext.RscriptModel
                .Where(r => r.RscriptModelId == rscriptModelId)
                .Include(r => r.CustomSearchExpression)
                .Include(r => r.CustomSearchParameter)
                .ThenInclude(p => p.CustomSearchExpression)
                .Include(r => r.DatasetPostProcess)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(rscriptModel, "RscriptModel", rscriptModelId);

            ModelInitializationType initializationType = ModelInitializationType.FullObject;

            RScriptModelData rscriptModelData = new RScriptModelData(rscriptModel, initializationType);

            if (rscriptModelData.Parameters != null)
            {
                foreach (var parameterData in rscriptModelData.Parameters)
                {
                    CustomSearchParameter parameter = rscriptModel.CustomSearchParameter.FirstOrDefault(p => p.CustomSearchParameterId == parameterData.Id);
                    parameterData.HasEditLookupExpression = parameter.CustomSearchExpression
                        .FirstOrDefault(c => c.ExpressionRole == CustomSearchExpressionRoleType.LookupExpression.ToString()) != null;
                    if (includeLookupValues && parameterData.HasEditLookupExpression)
                    {
                        parameterData.LookupValues = await this.GetCustomSearchParameterLookupValues(rscriptModelId, parameterData.Name, dbContext);
                    }
                }
            }

            response.RscriptModel = rscriptModelData;

            return response;
        }

        /// <summary>
        /// Gets the  lookup values for a custom search parameter.
        /// </summary>
        /// <param name="rscriptModelId">The rscript model id.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The dataset column edit lookup values.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search expression was not found.</exception>
        /// <exception cref="CustomSearchesJsonException">Exception while evaluating json expression.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Exception while evaluating sql expression.</exception>
        /// <exception cref="InvalidExpressionResultException">Invalid expression results.</exception>
        public async Task<object[]> GetCustomSearchParameterLookupValues(
            int rscriptModelId,
            string parameterName,
            CustomSearchesDbContext dbContext)
        {
            object[] results;
            var searchExpression = from cp in dbContext.CustomSearchParameter
                                   join ce in dbContext.CustomSearchExpression
                                   on cp.CustomSearchParameterId equals ce.CustomSearchParameterId
                                   where cp.ParameterName == parameterName &&
                                        cp.RscriptModelId == rscriptModelId &&
                                        ce.ExpressionRole == CustomSearchExpressionRoleType.LookupExpression.ToString()
                                   select ce;

            var customSearchExpression = await searchExpression.FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(customSearchExpression, "Lookup expression", parameterName);

            results = await CustomSearchExpressionEvaluator.EvaluateAsync<object[], CustomSearchesDataDbContext>(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                customSearchExpression.ExpressionType,
                customSearchExpression.Script,
                parameters: null);

            CustomSearchExpressionValidator.AssertLookupExpressionResultsAreValid(results);

            return results;
        }
    }
}
