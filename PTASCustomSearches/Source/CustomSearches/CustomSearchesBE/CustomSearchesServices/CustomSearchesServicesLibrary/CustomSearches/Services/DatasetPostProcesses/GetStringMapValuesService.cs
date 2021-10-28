namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the string map values.
    /// </summary>
    public class GetStringMapValuesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStringMapValuesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetStringMapValuesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the string map values.
        /// </summary>
        /// <param name="getStringMapValuesData">The string map properties to look for.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The string map values.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<GetStringMapValuesResponse> GetStringMapValuesAsync(GetStringMapValuesData getStringMapValuesData, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(getStringMapValuesData.StringMapProperties, nameof(getStringMapValuesData.StringMapProperties));

            IQueryable<Stringmap> stringMapQuery = null;
            foreach (var stringMapProperty in getStringMapValuesData.StringMapProperties)
            {
                InputValidationHelper.AssertNotEmpty(stringMapProperty.ObjectTypeCode, nameof(stringMapProperty.ObjectTypeCode));
                InputValidationHelper.AssertNotEmpty(stringMapProperty.AttributeName, nameof(stringMapProperty.AttributeName));

                var currentStringMapQuery =
                    from sm in dbContext.Stringmap
                    where sm.Objecttypecode.Trim().ToLower() == stringMapProperty.ObjectTypeCode.Trim().ToLower() &&
                          sm.Attributename.Trim().ToLower() == stringMapProperty.AttributeName.Trim().ToLower()
                    select sm;

                stringMapQuery = stringMapQuery == null ? currentStringMapQuery : stringMapQuery.Union(currentStringMapQuery);
            }

            GetStringMapValuesResponse response = new GetStringMapValuesResponse();
            response.StringMapValues = await stringMapQuery
                .OrderBy(sm => sm.Objecttypecode)
                .ThenBy(sm => sm.Attributename)
                .ThenBy(sm => sm.Displayorder)
                .Select(sm => new StringMapData(sm))
                .ToArrayAsync();

            return response;
        }
    }
}
