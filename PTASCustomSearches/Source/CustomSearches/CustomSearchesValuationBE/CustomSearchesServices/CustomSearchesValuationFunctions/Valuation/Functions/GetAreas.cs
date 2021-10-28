namespace CustomSearchesValuationFunctions.Valuation.Functions
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesValuationEFLibrary;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class to get the areas.
    /// </summary>
    public class GetAreas
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<ValuationDbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAreas"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <exception cref="ArgumentNullException">If dbContextFactory parameter is null.</exception>
        public GetAreas(IFactory<ValuationDbContext> dbContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Gets the areas.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The areas.
        /// </returns>
        [FunctionName("GetAreas")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Valuation/GetAreas")] HttpRequest req,
            ILogger log)
        {
            using (ValuationDbContext dbContext = this.dbContextFactory.Create())
            {
                var response = await dbContext.ptas_area.ToArrayAsync();
                return new JsonResult(response);
            }
        }
    }
}
