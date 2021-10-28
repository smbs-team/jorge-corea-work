namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that gets the lookup values for a custom search parameter.
    /// </summary>
    public class GetCustomSearchParameterLookupValuesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomSearchParameterLookupValuesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetCustomSearchParameterLookupValuesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the  lookup values for a custom search parameter.
        /// </summary>
        /// <param name="customSearcDefinitionId">The column name.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="dependedParameterValues">The values for parameters that this parameter depends on.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The dataset column edit lookup values.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search expression was not found.</exception>
        /// <exception cref="CustomSearchesJsonException">Exception while evaluating json expression.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Exception while evaluating sql expression.</exception>
        /// <exception cref="InvalidExpressionResultException">Invalid expression results.</exception>
        public async Task<GetUserCustomSearchDataResponse> GetCustomSearchParameterLookupValuesAsync(
            int customSearcDefinitionId,
            string parameterName,
            CustomSearchParameterValueData[] dependedParameterValues,
            CustomSearchesDbContext dbContext)
        {
            var response = new GetUserCustomSearchDataResponse();
            var searchExpression = from cp in dbContext.CustomSearchParameter
                                   join ce in dbContext.CustomSearchExpression
                                   on cp.CustomSearchParameterId equals ce.CustomSearchParameterId
                                   where cp.ParameterName == parameterName &&
                                        cp.CustomSearchDefinitionId == customSearcDefinitionId &&
                                        ce.ExpressionRole == CustomSearchExpressionRoleType.LookupExpression.ToString()
                                   select ce;

            var customSearchExpression = await searchExpression.Include(ce => ce.CustomSearchParameter).FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(customSearchExpression, "Lookup expression", parameterName);

            var parameterValues = ParameterReplacementHelper.ParametersAsDictionary(
               dependedParameterValues,
               customSearchExpression.CustomSearchParameter.ParameterExtensions);

            response.Results = await CustomSearchExpressionEvaluator.EvaluateAsync<dynamic[], CustomSearchesDataDbContext>(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                customSearchExpression.ExpressionType,
                customSearchExpression.Script,
                parameterValues);

            CustomSearchExpressionValidator.AssertLookupExpressionResultsAreValid(response.Results);

            return response;
        }
    }
}
