namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the custom search parameters.
    /// </summary>
    public class GetCustomSearchParametersService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomSearchParametersService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetCustomSearchParametersService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the custom search parameters.
        /// </summary>
        /// <param name="customSearchId">Custom search id.</param>
        /// <param name="includeLookupValues">Value indicating whether the results should include the lookup values.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The custom search parameters.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or expression was not found.</exception>
        /// <exception cref="CustomSearchesJsonException">Exception while evaluating json expression.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Exception while evaluating sql expression.</exception>
        /// <exception cref="InvalidExpressionResultException">Invalid expression results.</exception>
        public async Task<GetCustomSearchParametersResponse> GetCustomSearchParametersAsync(int customSearchId, bool includeLookupValues, CustomSearchesDbContext dbContext)
        {
            GetCustomSearchParametersResponse response = new GetCustomSearchParametersResponse();

            var query = dbContext.CustomSearchDefinition as IQueryable<CustomSearchDefinition>;
            var definition = await query.
                Where(c => c.CustomSearchDefinitionId == customSearchId)
                .Include(c => c.CustomSearchParameter)
                .ThenInclude(p => p.CustomSearchExpression)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(definition, "Custom search definition", customSearchId);

            var results = definition.CustomSearchParameter.OrderBy(p => p.DisplayOrder).ToArray();
            response.CustomSearchParameters = new CustomSearchParameterData[results.Length];

            var service = new GetCustomSearchParameterLookupValuesService(this.ServiceContext);

            for (int i = 0; i < results.Length; i++)
            {
                CustomSearchParameter customSearchParameter = results[i];
                CustomSearchParameterData customSearchParameterData = new CustomSearchParameterData(customSearchParameter, ModelInitializationType.FullObject);
                customSearchParameterData.HasEditLookupExpression = customSearchParameter.CustomSearchExpression
                    .FirstOrDefault(c => c.ExpressionRole == CustomSearchExpressionRoleType.LookupExpression.ToString()) != null;

                if (includeLookupValues && customSearchParameterData.HasEditLookupExpression)
                {
                    var lookupResponse = await service.GetCustomSearchParameterLookupValuesAsync(
                        customSearchId,
                        customSearchParameter.ParameterName,
                        dependedParameterValues: null,
                        dbContext);
                    customSearchParameterData.LookupValues = lookupResponse.Results;
                }

                response.CustomSearchParameters[i] = customSearchParameterData;
            }

            return response;
        }
    }
}
