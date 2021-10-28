namespace CustomSearchesValuationFunctions.Valuation.Functions
{
    using System;
    using System.Collections.Generic;
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
    /// Azure function to start the Floating Home Valuation Process.
    /// </summary>
    public class FloatingHomeValuation
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<ValuationDbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingHomeValuation"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <exception cref="ArgumentNullException">If dbContextFactory parameter is null.</exception>
        public FloatingHomeValuation(IFactory<ValuationDbContext> dbContextFactory)
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
        [FunctionName("StartFHValuation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "API/Valuation/FloatingHomeValuation/StartFHValuation")]
            HttpRequest req,
            ILogger log)
        {
            using (ValuationDbContext dbContext = this.dbContextFactory.Create())
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                string[] parcels = null;
                try
                {
                    parcels = JsonConvert.DeserializeObject<string[]>(body);
                }
                catch (Newtonsoft.Json.JsonException jsonSerializationException)
                {
                    throw new Exception("Exception occurred while deserializing incoming body parameters, expecting JSON array of parcel ids.", jsonSerializationException);
                }

                List<Guid> fhVals = new List<Guid>();
                List<Guid> estHistRecs = new List<Guid>();

                // Dictionary of Project Id & Associated Parcel1 Id
                Dictionary<Guid, Guid> specProjs = new Dictionary<Guid, Guid>();

                var assessYearId = dbContext.ptas_year
                    .Where(w => w.ptas_assessmentyearstart <= DateTime.Now && w.ptas_assessmentyearend >= DateTime.Now)
                    .Select(s => s.ptas_yearid)
                    .Single();
                var rollYearId = dbContext.ptas_year
                    .Where(w => w.ptas_assessmentyearstart <= DateTime.Now.AddYears(1) && w.ptas_assessmentyearend >= DateTime.Now.AddYears(1))
                    .Select(s => s.ptas_yearid)
                    .Single();

                foreach (var parcelId in parcels)
                {
                    var gParcel = new Guid(parcelId);
                    var unitBs =
                        (from ub in dbContext.ptas_unitbreakdown
                         join fh in dbContext.ptas_condounit on ub._ptas_floatinghome_value equals fh._ptas_parcelid_value
                         join ut in dbContext.ptas_unitbreakdowntype on ub._ptas_unitbreakdowntypeid_value equals ut.ptas_unitbreakdowntypeid
                         where fh._ptas_parcelid_value.Value == gParcel && ut.ptas_name == "Slip"
                         select new { UB = ub, UNIT = fh })
                         .ToList();

                    foreach (var unitInfo in unitBs)
                    {
                        var unitB = unitInfo.UB;
                        var slipGrade = dbContext.ptas_floatingHomeSlipValues
                            .Where(w => w.assessmentYearGuid == assessYearId)
                            .Single();
                        ptas_floatingHomeValuation fhv = new ptas_floatingHomeValuation()
                        {
                            floatingHomeValuationId = Guid.NewGuid(),
                            floatingHomeUnitGuid = unitB._ptas_floatinghome_value,
                            slipGradeValue = this.GetSlipValue(unitB.ptas_slip_grade, slipGrade)
                        };
                        fhv.subjectParcelSlipValue = unitB.ptas_subjectparcel * fhv.slipGradeValue;
                        fhv.dnrSlipValue = unitB.ptas_dnrparcel * fhv.slipGradeValue;
                        fhv.citySlipValue = unitB.ptas_cityparcel * fhv.slipGradeValue;

                        // Real Property
                        if (unitInfo.UNIT.ptas_accounttype == 668020000)
                        {
                            fhv.parcelGuid = unitInfo.UNIT._ptas_parcelid_value;
                        }

                        var fhCond = dbContext.ptas_floatingHomeReplacementCostRate
                            .Where(w => w.assessmentYearGuid == assessYearId);
                        if (unitInfo.UNIT._ptas_specialtyareaid_value != null)
                        {
                            fhCond = fhCond.Where(w => w.specialityAreaGuid == unitInfo.UNIT._ptas_specialtyareaid_value);
                        }

                        if (unitInfo.UNIT._ptas_specialtynbhdid_value != null)
                        {
                            fhCond = fhCond.Where(w => w.specialityNeighborhoodGuid == unitInfo.UNIT._ptas_specialtynbhdid_value);
                        }

                        var fhCondRec = fhCond.FirstOrDefault();
                        if (fhCondRec == null)
                        {
                            fhCondRec = new ptas_floatingHomeReplacementCostRate() { gradeAverage = 0, gradeAverageMinus = 0, gradeAveragePlus = 0, gradeExcellent = 0, gradeExcellentMinus = 0, gradeExcellentPlus = 0, gradeGood = 0, gradeGoodMinus = 0, gradeGoodPlus = 0 };
                        }

                        switch (fhv.slipGradeValue)
                        {
                            case 1:
                                fhv.RCNperSqft = fhCondRec.gradeAverageMinus;
                                break;
                            case 2:
                                fhv.RCNperSqft = fhCondRec.gradeAverage;
                                break;
                            case 3:
                                fhv.RCNperSqft = fhCondRec.gradeAveragePlus;
                                break;
                            case 4:
                                fhv.RCNperSqft = fhCondRec.gradeGoodMinus;
                                break;
                            case 5:
                                fhv.RCNperSqft = fhCondRec.gradeGood;
                                break;
                            case 6:
                                fhv.RCNperSqft = fhCondRec.gradeGoodPlus;
                                break;
                            case 7:
                                fhv.RCNperSqft = fhCondRec.gradeExcellentMinus;
                                break;
                            case 8:
                                fhv.RCNperSqft = fhCondRec.gradeExcellent;
                                break;
                            case 9:
                                fhv.RCNperSqft = fhCondRec.gradeExcellentPlus;
                                break;
                        }

                        switch (unitInfo.UNIT.ptas_condounitcondition)
                        {
                            case 1: // Poor
                                fhv.RCNLDperSqft = fhv.RCNperSqft * new decimal(0.60);
                                break;
                            case 2: // Fair
                                fhv.RCNLDperSqft = fhv.RCNperSqft * new decimal(0.65);
                                break;
                            case 3: // Below Average
                                fhv.RCNLDperSqft = fhv.RCNperSqft * new decimal(0.70);
                                break;
                            case 4: // Average
                                fhv.RCNLDperSqft = fhv.RCNperSqft * new decimal(0.75);
                                break;
                            case 5: // Good
                                fhv.RCNLDperSqft = fhv.RCNperSqft * new decimal(0.80);
                                break;
                            case 6: // Very good
                                fhv.RCNLDperSqft = fhv.RCNperSqft * new decimal(0.85);
                                break;
                            case 7: // Excellent
                                fhv.RCNLDperSqft = fhv.RCNperSqft * new decimal(0.90);
                                break;
                        }

                        fhv.livingValue = unitInfo.UNIT.ptas_totalliving * fhv.RCNLDperSqft;
                        fhv.basementValue = unitInfo.UNIT.ptas_totalbasement * fhv.RCNLDperSqft * new decimal(0.5);
                        fhv.totalHomeValue = fhv.livingValue + fhv.basementValue;
                        if (unitInfo.UNIT.ptas_smallhomeadjustment > 0)
                        {
                            fhv.smallHomeAdjustmentValue = fhv.totalHomeValue * unitInfo.UNIT.ptas_smallhomeadjustment;
                        }

                        if (unitInfo.UNIT.ptas_percentnetcondition > 0)
                        {
                            if (fhv.smallHomeAdjustmentValue > 0)
                            {
                                fhv.pcntNetConditionValue = (fhv.smallHomeAdjustmentValue * unitInfo.UNIT.ptas_percentnetcondition) / 100;
                            }
                            else
                            {
                                fhv.pcntNetConditionValue = (fhv.totalHomeValue * unitInfo.UNIT.ptas_percentnetcondition) / 100;
                            }
                        }

                        if (fhv.pcntNetConditionValue > 0)
                        {
                            fhv.RCNLD = fhv.pcntNetConditionValue;
                        }
                        else if (fhv.smallHomeAdjustmentValue > 0)
                        {
                            fhv.RCNLD = fhv.smallHomeAdjustmentValue;
                        }
                        else
                        {
                            fhv.RCNLD = fhv.totalHomeValue;
                        }

                        var projInfo =
                            (from fhp in dbContext.ptas_condocomplex
                             where fhp.ptas_projecttype == 668020004 // Floating Home
                                    && fhp._ptas_parcelid_value == gParcel
                             select new { ProjId = fhp.ptas_condocomplexid, Parcel1Id = fhp._ptas_associatedparcelid_value })
                                    .Single();
                        fhv.floatingHomeProjectGuid = projInfo.ProjId;

                        // Save results of valuation
                        dbContext.ptas_floatingHomeValuation.Add(fhv);
                        fhVals.Add(fhv.floatingHomeValuationId);

                        // Create estimate history record to match floating home valuation
                        var estHistRec = new ptas_estimateHistory()
                        {
                            estimateHistoryGuid = Guid.NewGuid(),
                            rollYearGuid = rollYearId,
                            estimateType = "RCNLD",
                            estimateTypeId = 3,
                            assessmentYearGuid = assessYearId,
                            impsValue = Convert.ToDouble(fhv.RCNLD),
                            calculationDate = DateTime.Now
                        };

                        var parcelInfo = (
                            from p in dbContext.ptas_parceldetail
                            join s in dbContext.ptas_specialtyarea on p._ptas_specialtyareaid_value equals s.ptas_specialtyareaid into pNs
                            from specArea in pNs.DefaultIfEmpty()
                            join g in dbContext.ptas_geoarea on p._ptas_geoareaid_value equals g.ptas_geoareaid into pNg
                            from geoArea in pNg.DefaultIfEmpty()
                            where p.ptas_parceldetailid == gParcel
                            select new { PARCEL = p, SAName = specArea.ptas_name, GeoArea = geoArea.ptas_name })
                            .SingleOrDefault();

                        // Real property
                        if (unitInfo.UNIT.ptas_accounttype == 668020000)
                        {
                            if (parcelInfo.SAName == "730")
                            {
                                estHistRec.parcelGuid = parcelInfo.PARCEL.ptas_parceldetailid;
                            }

                            estHistRec.taxAccountId = parcelInfo.PARCEL._ptas_taxaccountid_value;

                            estHistRec.landValue = Convert.ToDouble(fhv.subjectParcelSlipValue);
                        }
                        else
                        {
                            estHistRec.ppTaxAccountId = unitInfo.UNIT._ptas_taxaccountid_value;
                        }

                        estHistRec.totalValue = estHistRec.landValue + estHistRec.impsValue;
                        dbContext.ptas_estimateHistory.Add(estHistRec);
                        estHistRecs.Add(estHistRec.estimateHistoryGuid);

                        // If in this specialty area or geo area, save for latter computation of total value
                        if (parcelInfo.SAName == "730" || parcelInfo.GeoArea == "015")
                        {
                            specProjs.Add(projInfo.ProjId, projInfo.Parcel1Id.Value);
                        }
                    }
                }

                // First save all valuation changes to db in this transaction
                dbContext.SaveChanges();

                // Compute the slip values for DNR parcels
                foreach (var specProj in specProjs)
                {
                    var slipValues = (from val in dbContext.ptas_floatingHomeValuation
                                      where val.assessmentYearGuid == assessYearId &&
                                            val.floatingHomeProjectGuid == specProj.Key
                                      select new { DSV = val.dnrSlipValue, SPSV = val.subjectParcelSlipValue, PARCELID = val.parcelGuid })
                                        .ToList();

                    var dnrSlipValue = slipValues.Where(w => w.DSV != null).Sum(s => s.DSV);
                    var sbjSlipValue = slipValues.Where(w => w.SPSV != null).Sum(s => s.SPSV);

                    // Get the land id for the parcel associated with 1 of project
                    var landId = dbContext.ptas_parceldetail
                        .Where(w => w.ptas_parceldetailid == specProj.Value)
                        .Select(s => s._ptas_landid_value).SingleOrDefault();
                    if (landId != null)
                    {
                        ptas_land landRec = new ptas_land()
                        {
                            ptas_landid = landId.Value,
                            ptas_baselandvalue = sbjSlipValue != 0 ? sbjSlipValue : dnrSlipValue
                        };

                        // The DNR or Slip value will only be populated based on project type, set base land value appropriately
                        if (sbjSlipValue != 0)
                        {
                            landRec.ptas_baselandvalue = sbjSlipValue;
                        }
                        else if (dnrSlipValue != 0)
                        {
                            landRec.ptas_baselandvalue = dnrSlipValue;
                        }

                        if (landRec.ptas_baselandvalue != null)
                        {
                            dbContext.ptas_land.Update(landRec);
                        }
                    }
                }

                // Finally, save all DNR changes in this transaction
                dbContext.SaveChanges();

                return new JsonResult(new
                {
                    FloatingHomeValuations = fhVals.ToArray(),
                    EstimateHistories = estHistRecs.ToArray()
                });
            }
        }

        private decimal? GetSlipValue(int? grade, ptas_floatingHomeSlipValues fhsv)
        {
            switch (grade)
            {
                case 1:
                    return fhsv.grade1;
                case 2:
                    return fhsv.grade2;
                case 3:
                    return fhsv.grade3;
                case 4:
                    return fhsv.grade4;
                case 5:
                    return fhsv.grade5;
                default:
                    return 0;
            }
        }
    }
}
