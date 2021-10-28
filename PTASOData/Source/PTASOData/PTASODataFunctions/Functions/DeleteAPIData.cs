namespace PTASODataFunctions.Functions
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.OData.Edm;
    using PTASODataFunctions.Exception;
    using PTASODataLibrary.Helper;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the DeleteAPIData service.  This service deletes data related to parcels.
    /// </summary>
    public class DeleteAPIData
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<DbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAPIData"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <exception cref="ArgumentNullException">If dbContextFactory parameter is null.</exception>
        public DeleteAPIData(IFactory<DbContext> dbContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Runs the DeleteData function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="resource">The resource being deleted.</param>
        /// <param name="resourceId">The id of the resource to delete.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [FunctionName("DeleteAPIData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "API/{resource}/{resourceId}")] HttpRequest req, string resource, Guid resourceId, ILogger log)
        {
            try
            {
                if (!ODataExtensions.AllowODataUpdates)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotImplemented);
                }

                if (string.IsNullOrWhiteSpace(resource))
                {
                    throw new ArgumentNullException(nameof(resource));
                }

                log.LogInformation(
                    "Processing OData Delete Request for Resource: {0}.  Query String: {1}.  Host: {2}.  Path: {3}",
                    resource,
                    req.QueryString,
                    req.Host.ToString(),
                    req.Path.ToString());

                Assembly entityAssembly = Assembly.GetAssembly(typeof(ODataExtensions));

                var edmModel = ODataExtensions.GetEdmModel();
                var edmEntity = edmModel.EntityContainer.FindEntitySet(resource);
                if (edmEntity == null)
                {
                    return new NotFoundObjectResult("Resource not found.");
                }

                string fullTypeName = edmEntity.Type.AsElementType().FullTypeName();

                using (var dbContext = this.dbContextFactory.Create())
                {
                    var entity = await dbContext.FindAsync(entityAssembly.GetType(fullTypeName), resourceId);
                    if (entity == null)
                    {
                        return new NotFoundObjectResult($" The entity '{resourceId}' was not found.");
                    }

                    dbContext.Remove(entity);
                    await dbContext.SaveChangesAsync();
                    return new NoContentResult();
                }
            }
            catch (ArgumentNullException argNullEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleArgumentNullException(argNullEx, req, log);
            }
            catch (DbUpdateException dbUpdateEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleDbUpdateException(dbUpdateEx, req, log);
            }
            catch (SqlException sqlEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleSqlException(sqlEx, req, log);
            }
            catch (Exception ex)
            {
                return PTASODataFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
