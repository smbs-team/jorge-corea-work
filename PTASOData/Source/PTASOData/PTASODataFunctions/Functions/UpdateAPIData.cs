namespace PTASODataFunctions.Functions
{
    using System;
    using System.IO;
    using System.Linq;
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
    using Microsoft.OData;
    using Microsoft.OData.Edm;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PTASODataFunctions.Exception;
    using PTASODataLibrary.Helper;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the UpdateAPIData service.  This service updates data related to parcels.
    /// </summary>
    public class UpdateAPIData
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<DbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAPIData"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <exception cref=ArgumentNullException">If dbContextFactory parameter is null.</exception>
        public UpdateAPIData(IFactory<DbContext> dbContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Runs the UpdateData function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="resource">The resource being updated.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [FunctionName("UpdateData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "API/{resource}")] HttpRequest req, string resource, ILogger log)
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

                var body = await new StreamReader(req.Body).ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(body))
                {
                    throw new ArgumentNullException(nameof(body));
                }

                log.LogInformation(
                    "Processing OData Update Request for Resource: {0}.  Query String: {1}.  Host: {2}.  Path: {3}",
                    resource,
                    req.QueryString,
                    req.Host.ToString(),
                    req.Path.ToString());

                var edmModel = ODataExtensions.GetEdmModel();
                var edmEntity = edmModel.EntityContainer.FindEntitySet(resource);
                if (edmEntity == null)
                {
                    return new NotFoundObjectResult($"The resource '{resource}' was not found.");
                }

                string fullTypeName = edmEntity.Type.AsElementType().FullTypeName();
                Assembly entityAssembly = Assembly.GetAssembly(typeof(ODataExtensions));
                Type resourceType = entityAssembly.GetType(fullTypeName);

                object jsonObject = JsonConvert.DeserializeObject(body);
                if (!(jsonObject is JArray jsonArray))
                {
                    jsonArray = new JArray(jsonObject);
                }

                JsonSerializer jsonSerializer = new JsonSerializer() { MissingMemberHandling = MissingMemberHandling.Error };

                using (var dbContext = this.dbContextFactory.Create())
                {
                    var primaryKey = dbContext.Model.FindEntityType(resourceType).FindPrimaryKey();
                    if (primaryKey == null)
                    {
                        throw new ODataException("Keyless entities can't be updated");
                    }

                    var keyName = primaryKey.Properties.Select(x => x.Name).Single();

                    foreach (var item in jsonArray)
                    {
                        if (item[keyName] == null)
                        {
                            return new BadRequestObjectResult($"Entity does not contain the field key '{keyName}'.");
                        }

                        var entity = item.ToObject(resourceType, jsonSerializer);
                        var keyValue = resourceType.GetProperty(keyName).GetValue(entity);
                        var dbEntity = await dbContext.FindAsync(resourceType, keyValue);
                        if (dbEntity == null)
                        {
                            return new NotFoundObjectResult($" The entity '{keyValue}' was not found.");
                        }

                        JsonConvert.PopulateObject(JsonConvert.SerializeObject(item), dbEntity);
                    }

                    await dbContext.SaveChangesAsync();
                }

                return new NoContentResult();
            }
            catch (ArgumentNullException argNullEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleArgumentNullException(argNullEx, req, log);
            }
            catch (JsonException jsonEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleJsonException(jsonEx, req, log);
            }
            catch (DbUpdateException dbUpdateEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleDbUpdateException(dbUpdateEx, req, log);
            }
            catch (SqlException sqlEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleSqlException(sqlEx, req, log);
            }
            catch (ODataException odataEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleODataException(odataEx, req, log);
            }
            catch (Exception ex)
            {
                return PTASODataFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
