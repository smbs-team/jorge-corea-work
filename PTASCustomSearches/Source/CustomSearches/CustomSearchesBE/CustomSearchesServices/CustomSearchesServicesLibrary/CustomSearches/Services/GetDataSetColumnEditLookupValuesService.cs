namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
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

    /// <summary>
    /// Service that gets the dataset column edit lookup values.
    /// </summary>
    public class GetDataSetColumnEditLookupValuesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDataSetColumnEditLookupValuesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDataSetColumnEditLookupValuesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the dataset column edit lookup values.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="dependedParameterValues">The depended parameter values.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The dataset column edit lookup values.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or expression was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset.</exception>
        /// <exception cref="CustomSearchesJsonException">Exception while evaluating json expression.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Exception while evaluating sql expression.</exception>
        public async Task<GetUserCustomSearchDataResponse> GetDataSetColumnEditLookupValuesAsync(
            Guid datasetId,
            string columnName,
            CustomSearchParameterValueData[] dependedParameterValues,
            CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(columnName, nameof(columnName));
            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            DatasetHelper.AssertCanReadFromDataset(dataset, usePostProcess: false);

            GetUserCustomSearchDataResponse response = new GetUserCustomSearchDataResponse();
            var query = dbContext.Dataset as IQueryable<Dataset>;
            var customSearchDefinitionQuery = from d in dbContext.Dataset join c in dbContext.CustomSearchDefinition on d.CustomSearchDefinitionId equals c.CustomSearchDefinitionId where d.DatasetId == datasetId select c;
            var columnDefinitionQuery = from c in customSearchDefinitionQuery join cd in dbContext.CustomSearchColumnDefinition on c.CustomSearchDefinitionId equals cd.CustomSearchDefinitionId where cd.ColumnName.ToLower() == columnName.ToLower() select cd;
            var searchExpression = from cd
                                   in columnDefinitionQuery
                                   join se
                                   in dbContext.CustomSearchExpression
                                   on cd.CustomSearchColumnDefinitionId equals se.CustomSearchColumnDefinitionId
                                   where se.ExpressionRole == CustomSearchExpressionRoleType.EditLookupExpression.ToString()
                select new { se, cd };
            var tuple = await searchExpression.FirstOrDefaultAsync();
            var customSearchExpression = tuple.se;
            var columnDefinition = tuple.cd;

            InputValidationHelper.AssertEntityExists(customSearchExpression, "Edit Lookup Expression", columnName);

            var parameterValues = ParameterReplacementHelper.ParametersAsDictionary(
                dependedParameterValues,
                columnDefinition.ColumDefinitionExtensions);

            response.Results = await CustomSearchExpressionEvaluator.EvaluateAsync<object[], CustomSearchesDataDbContext>(
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
