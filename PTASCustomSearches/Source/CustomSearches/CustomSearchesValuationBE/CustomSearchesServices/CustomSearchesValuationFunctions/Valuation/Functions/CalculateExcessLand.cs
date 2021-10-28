namespace CustomSearchesValuationFunctions.Valuation.Functions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesValuationEFLibrary;
    using CustomSearchesValuationEFLibrary.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class to get the areas.
    /// </summary>
    public class CalculateExcessLand
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<ValuationDbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateExcessLand"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <exception cref="ArgumentNullException">If dbContextFactory parameter is null.</exception>
        public CalculateExcessLand(IFactory<ValuationDbContext> dbContextFactory)
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
        [FunctionName("CalculateExcessLand")]
        public async Task<IActionResult> Run(
             [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "API/Valuation/CalculateExcessLand")]
             HttpRequest req,
             ILogger log)
        {
            using (ValuationDbContext dbContext = this.dbContextFactory.Create())
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                Guid[] estimateIds = null;
                try
                {
                    string[] estimates = JsonConvert.DeserializeObject<string[]>(body);
                    estimateIds = estimates.Select(s => new Guid(s)).ToArray();
                }
                catch (Newtonsoft.Json.JsonException jsonSerializationException)
                {
                    throw new Exception("Exception occurred while deserializing incoming body parameters, expecting JSON array of estimate history ids.", jsonSerializationException);
                }

                var dbData =
                    (from ids in estimateIds
                     join eh in dbContext.ptas_estimateHistory on ids equals eh.estimateHistoryGuid
                     join p in dbContext.ptas_parceldetail on eh.parcelGuid equals p.ptas_parceldetailid
                     join l in dbContext.ptas_land on p._ptas_landid_value equals l.ptas_landid
                     join pe in dbContext.ptas_parceleconomicunit on ids equals pe._ptas_parcelid_value
                     select new { EstHist = eh, Parcel = p, Land = l, ParcelToEconUnit = pe })
                    .ToList();

                foreach (var dbRow in dbData)
                {
                    if (dbRow.Land.ptas_calculateexcessland.GetValueOrDefault())
                    {
                        // Primary
                        if (dbRow.ParcelToEconUnit.ptas_type == 1)
                        {
                            var pBuildingSqft =
                                (from p in dbContext.ptas_parceldetail
                                 join b in dbContext.ptas_buildingdetail on p.ptas_parceldetailid equals b._ptas_parceldetailid_value
                                 where p.ptas_parceldetailid == dbRow.Parcel.ptas_parceldetailid
                                 select b.ptas_buildinggross_sqft)
                                 .Sum();
                            var euBaseLandValue =
                                (from pe in dbContext.ptas_parceleconomicunit
                                 join p in dbContext.ptas_parceldetail on pe._ptas_parcelid_value equals p.ptas_parceldetailid
                                 join l in dbContext.ptas_land on p._ptas_landid_value equals l.ptas_landid
                                 where pe._ptas_economicunitid_value == dbRow.ParcelToEconUnit._ptas_economicunitid_value
                                 select l.ptas_grosslandvalue)
                                 .Sum();
                            var requiredLandSqFt = dbRow.Land.ptas_requiredlbratio * pBuildingSqft;
                            var excessLandSqft = euBaseLandValue * requiredLandSqFt;

                            if (excessLandSqft > 0)
                            {
                                var landValuePerSqFt = euBaseLandValue / excessLandSqft;
                                var requiredLandVal = landValuePerSqFt * requiredLandSqFt;
                                var improvementVal = Convert.ToDecimal(dbRow.EstHist.totalValue) - requiredLandVal;
                                var excessLandVal = excessLandSqft * dbRow.Land.ptas_dollarspersquarefoot;
                                var excessLandWarning = false;
                                var totalEconUnitValue = Convert.ToDecimal(dbRow.EstHist.totalValue) + excessLandVal;
                            }
                            else
                            {
                                var landValuePerSqFt = 0;
                            }
                        }
                        else
                        {
                            // To-do
                        }
                    }
                    else if (dbRow.ParcelToEconUnit.ptas_type == 1)
                    {
                        // To-do
                    }
                }

                var response = await dbContext.ptas_area.ToArrayAsync();
                return new JsonResult(response);
            }
        }
    }
}
