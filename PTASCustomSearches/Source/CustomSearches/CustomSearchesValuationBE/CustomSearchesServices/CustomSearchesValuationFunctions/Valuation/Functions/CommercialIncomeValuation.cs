namespace CustomSearchesValuationFunctions.Valuation.Functions
{
    using System;
    using System.Collections.Generic;
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
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function to start the Floating Home Valuation Process.
    /// </summary>
    public class CommercialIncomeValuation
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<ValuationDbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommercialIncomeValuation"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <exception cref="ArgumentNullException">If dbContextFactory parameter is null.</exception>
        public CommercialIncomeValuation(IFactory<ValuationDbContext> dbContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// This method will kick off process of valuation this floating home.
        /// </summary>
        /// <param name="req">The request object.</param>
        /// <param name="log">The log object.</param>
        /// <returns>A status of the valuation process.</returns>
        [FunctionName("StartCIValuation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "API/Valuation/CommercialIncomeValuation/StartCIValuation")]
            HttpRequest req,
            ILogger log)
        {
            Guid gGeoArea, gGeoNbhd;
            using (ValuationDbContext dbContext = this.dbContextFactory.Create())
            {
                if (req.Query.ContainsKey("geoArea") && req.Query.ContainsKey("geoNbhd"))
                {
                    gGeoArea = new Guid(req.Query["geoArea"]);
                    gGeoNbhd = new Guid(req.Query["geoNbhd"]);
                }
                else
                {
                    throw new ArgumentNullException("Required parameters not provided.");
                }

                List<ptas_incomevaluation> incVals = new List<ptas_incomevaluation>();
                List<ptas_estimateHistory> estHVals = new List<ptas_estimateHistory>();

                // Cache the years and section uses for lookup use later
                var ptasYears = dbContext.ptas_year.Where(w => w.statecode == 1)
                    .ToDictionary(y => y.ptas_yearid, y => int.Parse(y.ptas_name));
                var ptasSectionUses = dbContext.ptas_buildingsectionuse.Where(w => w.statecode == 1)
                    .ToDictionary(s => s.ptas_buildingsectionuseid, s => s.ptas_abbreviation);
                Guid? rollYearId = null;
                foreach (var parcel in dbContext.ptas_parceldetail
                                        .Where(w => w._ptas_geoareaid_value == gGeoArea && w._ptas_geonbhdid_value == gGeoNbhd)
                                        .Include(p => p.ptas_buildingdetail)
                                        .ThenInclude(b => b.ptas_buildingdetail_commercialuse))
                {
                    foreach (var building in parcel.ptas_buildingdetail)
                    {
                        foreach (var sectionUse in building.ptas_buildingdetail_commercialuse)
                        {
                            var inValRec = new ptas_incomevaluation()
                            {
                                parcelId = parcel.ptas_parceldetailid,
                                buildingId = building.ptas_buildingdetailid,
                                sectionUseId = sectionUse.ptas_buildingdetail_commercialuseid
                            };

                            // Building in Pending status
                            if (building.statuscode == 591500003)
                            {
                                inValRec.exceptionCode = "Building is pending";
                                continue;
                            }

                            // Check building Sq Ft and set exception accordingly.
                            if (building.ptas_buildingnet_sqft == 0)
                            {
                                inValRec.exceptionCode = "NetSqFt = 0";
                                inValRec.indicatedValue = 0;
                                continue;
                            }

                            if (building._ptas_effectiveyearid_value == null)
                            {
                                inValRec.exceptionCode = "Section use not found";
                                inValRec.indicatedValue = 0;
                                continue;
                            }

                            // Query rate table for matching record
                            var rateRows = (
                                from imd in dbContext.ptas_incomemodeldetail
                                where imd.geoAreaId == gGeoArea && imd.geoNbhdId == gGeoNbhd
                                select imd)
                                .ToList();

                            var rateRecs = rateRows
                                    .Where(w => w.maxEffectiveYearId != null && w.minEffectiveYearId != null &&
                                      ptasYears[w.maxEffectiveYearId.Value] >= ptasYears[building._ptas_effectiveyearid_value.Value] &&
                                      ptasYears[w.minEffectiveYearId.Value] <= ptasYears[building._ptas_effectiveyearid_value.Value] &&
                                      building.ptas_buildingnet_sqft >= w.minSqFt &&
                                      building.ptas_buildingnet_sqft <= w.maxSqFt &&
                                      building.ptas_buildingquality == w.stratification)
                                .ToList();

                            if (rateRecs.Count > 0)
                            {
                                var rentFactor = rateRecs.Where(w => w.rateType == "Rent").SingleOrDefault();
                                var vacancyFactor = rateRecs.Where(w => w.rateType == "Vacancy").SingleOrDefault();
                                var opExpFactor = rateRecs.Where(w => w.rateType == "Operating Expenses").SingleOrDefault();
                                var capRateFactor = rateRecs.Where(w => w.rateType == "Capitalization").SingleOrDefault();

                                var grade = dbContext.ptas_gradestratificationmapping
                                    .Where(w => w.ptas_buildingquality == building.ptas_buildingquality)
                                    .Select(s => s.ptas_grade)
                                    .SingleOrDefault();

                                inValRec.potentialGrossIncome = this.GetFactor(grade, rentFactor) * building.ptas_buildingnet_sqft;
                                inValRec.effectiveGrossIncome = inValRec.potentialGrossIncome * (1 - (this.GetFactor(grade, vacancyFactor) * Convert.ToDecimal(0.01)));
                                inValRec.netOperatingIncome = inValRec.effectiveGrossIncome * (1 - (this.GetFactor(grade, opExpFactor) * Convert.ToDecimal(0.01)));
                                inValRec.indicatedValue = inValRec.netOperatingIncome / this.GetFactor(grade, capRateFactor);
                                inValRec.weightedCapAmt = inValRec.netOperatingIncome * this.GetFactor(grade, capRateFactor);
                                inValRec.dollarPerSqFt = inValRec.indicatedValue / building.ptas_buildingnet_sqft;
                                incVals.Add(inValRec);

                                rollYearId = rateRecs.First().assessmentYearId;
                            }
                        }
                    }

                    if (incVals.Count > 0)
                    {
                        ptas_estimateHistory estHistRec = new ptas_estimateHistory()
                        {
                            parcelGuid = incVals.First().parcelId,
                            buildingGuid = incVals.First().buildingId,
                            landId = parcel._ptas_landid_value,
                            rollYearGuid = rollYearId,
                            estimateType = "INCOME", // 8, // Income Valuation
                            taxAccountId = parcel._ptas_taxaccountid_value,
                            assessmentYearGuid = rollYearId,
                            totalValue = Convert.ToDouble(incVals.Select(s => s.indicatedValue).Sum())
                        };
                        var landVal = dbContext.ptas_land.SingleOrDefault(s => s.ptas_landid == parcel._ptas_landid_value).ptas_grosslandvalue;
                        estHistRec.landValue = Convert.ToDouble(landVal);

                        // estHistRec. =
                        //    (from l in dbContext.ptas_land
                        //     join p in dbContext.ptas_parceldetail on l.ptas_landid equals p._ptas_landid_value
                        //     where p._ptas_economicunit_value == parcel._ptas_economicunit_value
                        //     select l.ptas_grosslandvalue)
                        //     .Sum();

                        // estHistRec.ptas_economicunitindicatedvalue =
                        //    (from i in dbContext.ptas_incomevaluation
                        //     join p in dbContext.ptas_parceldetail on i._ptas_parcelid_value equals p.ptas_parceldetailid
                        //     where p._ptas_economicunit_value == p._ptas_economicunit_value
                        //     select i.ptas_indicatedvalue)
                        //     .Sum();
                        var parcelEcon = dbContext.ptas_parceleconomicunit
                            .Where(w => w._ptas_parcelid_value == parcel.ptas_parceldetailid &&
                                        w._ptas_economicunitid_value == parcel._ptas_economicunit_value) // .Select(s => s.ptas_type == 1) // Primary Parcel
                            .SingleOrDefault();

                        // Is this a primary parcel on economic unit
                        // if (parcelEcon != null && parcelEcon.ptas_type == 1)
                        // {
                        //    estHistRec.totalValue = estHistRec.ptas_economicunitindicatedvalue - estHistRec.ptas_economicunitsubsidiarylandvalue;
                        // }
                        // else
                        // {
                        //    estHistRec.ptas_totalvalue = estHistRec.ptas_economicunitindicatedvalue;
                        // }
                        estHistRec.impsValue = estHistRec.totalValue - estHistRec.landValue;
                        estHistRec.calculationDate = DateTime.Now;

                        estHVals.Add(estHistRec);
                    }
                }

                return new JsonResult(new { EstimateHistories = estHVals.ToArray(), IncomeValuations = incVals.ToArray() });
            }
        }

        private decimal GetFactor(int? grade, ptas_incomemodeldetail imDetail)
        {
            switch (grade ?? 0)
            {
                case 1:
                    return imDetail.grade1 ?? 0;
                case 2:
                    return imDetail.grade2 ?? 0;
                case 3:
                    return imDetail.grade3 ?? 0;
                case 4:
                    return imDetail.grade4 ?? 0;
                case 5:
                    return imDetail.grade5 ?? 0;
                case 6:
                    return imDetail.grade6 ?? 0;
                case 7:
                    return imDetail.grade7 ?? 0;
                default:
                    return 0;
            }
        }
    }
}
