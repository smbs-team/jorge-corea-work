namespace PTASODataLibrary.Api
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PTASODataLibrary.Helper;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;

    /// <summary>
    /// Encapsulates the business logic for OData API service.
    /// </summary>
    public static class PtasODataApi
    {
        /// <summary>
        /// Gets the OData payload for a given HttpRequest.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="log">The log.</param>
        /// <returns>OData payload for the requested resource.</returns>
        /// <exception cref="System.ArgumentNullException">resource.</exception>
        public static async Task<string> GetData(HttpRequest req, DbContext dbContext, string resource, ILogger log)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new System.ArgumentNullException(nameof(resource));
            }

            HttpRequest internalRequest = req.CreateInternalHttpContext().Request;
            IQueryable query = dbContext.GetODataQueryByResourceName(req, internalRequest, resource) as IQueryable;

            Type elementType = query.ElementType;
            MethodInfo method = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList));
            MethodInfo generic = method.MakeGenericMethod(elementType);
            object[] parmeters = { query };
            IList entities = (IList)generic.Invoke(null, parmeters);

            string toReturn = await req.SerializeIntoODataResponse(internalRequest, entities, query.ElementType);
            return toReturn;
        }

        /// <summary>
        /// Gets the meta data for the OData Service.
        /// </summary>
        /// <returns>OData meta-data in XML format.</returns>
        public static async Task<string> GetMetaData()
        {
            return await ODataExtensions.GetMetadata();
        }

        private static IList CretateGenericList(Type elementType)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(elementType);
            var instance = Activator.CreateInstance(constructedListType);
            return (IList)instance;
        }
    }
}
