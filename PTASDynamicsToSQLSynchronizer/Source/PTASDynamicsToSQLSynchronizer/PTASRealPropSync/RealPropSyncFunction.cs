using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace PTASRealPropSync
{
    public static class RealPropSyncFunction
    {
        private static readonly Dictionary<string, string> DynamicsEntityMap
            = new Dictionary<string, string>
        {
            { "ResBldg","ptas_buildingdetail" },
        };

        private static readonly Dictionary<string, string> SQLEntityMap
            = new Dictionary<string, string>
        {
            { "RealPropApplHist","ptas.ptas_appraisalHistory" },
        };

        private static readonly Dictionary<string, string> DynamicsFieldMap = new Dictionary<string, string> {
                {"Id", "ptas_alternatekey"},
                {"Active", ""},
                {"AddnlCost", "ptas_additionalcost"},
                {"Bath3qtrCount", "ptas_34baths"},
                {"BathFullCount", "ptas_fullbathnbr"},
                {"BathHalfCount", "ptas_12baths"},
                {"Bedrooms", "ptas_bedroomnbr"},
                {"BldgGradeId", ""},
                {"BldgGradeItemId", "ptas_buildinggrade"},
                {"BldgGradeVar", ""},
                {"BldgGuid", "ptas_buildingdetailid"},
                {"BldgNbr", "ptas_buildingnbr"},
                {"BrickStone", "ptas_percentbrickorstone"},
                {"ConditionId", ""},
                {"ConditionItemId", "ptas_res_buildingcondition"},
                {"CreateDate", ""},
                {"DaylightBasement", "ptas_daylightbasement"},
                {"FinBasementGradeId", ""},
                {"FinBasementGradeItemId", "ptas_res_basementgrade"},
                {"FpAdditional", "ptas_addl_fireplace"},
                {"FpFreestanding", "ptas_fr_std_fireplace"},
                {"FpMultiStory", "ptas_multi_fireplace"},
                {"FpSingleStory", "ptas_single_fireplace"},
                {"HeatSourceId", ""},
                {"HeatSourceItemId", "ptas_res_heatsource"},
                {"HeatSystemId", ""},
                {"HeatSystemItemId", "ptas_residentialheatingsystem"},
                {"NbrLivingUnits", "ptas_units"},
                {"Obsolescence", "ptas_buildingobsolescence"},
                {"PcntComplete", "ptas_percentcomplete"},
                {"PcntNetCondition", "ptas_percentnetcondition"},
                {"RpGuid", "_ptas_parceldetailid_value"},
                {"SqFt1stFloor", "ptas_1stflr_sqft"},
                {"SqFt2ndFloor", "ptas_2ndflr_sqft"},
                {"SqFtDeck", "ptas_deck_sqft"},
                {"SqFtEnclosedPorch", "ptas_enclosedporch_sqft"},
                {"SqFtFinBasement", "ptas_finbsmt_sqft"},
                {"SqFtGarageAttached", "ptas_attachedgarage_sqft"},
                {"SqFtGarageBasement", "ptas_basementgarage_sqft"},
                {"SqFtHalfFloor", "ptas_halfflr_sqft"},
                {"SqFtOpenPorch", "ptas_openporch_sqft"},
                {"SqFtTotBasement", "ptas_totalbsmt_sqft"},
                {"SqFtTotLiving", "ptas_totalliving_sqft"},
                {"SqFtUnfinFull", "ptas_unfinished_full_sqft"},
                {"SqFtUnfinHalf", "ptas_unfinished_half_sqft"},
                {"SqFtUpperFloor", "ptas_upperflr_sqft"},
                {"Stories", "ptas_numberofstoriesdecimal"},
                {"UpdateDate", ""},
                {"UpdatedByGuid", ""},
                {"ViewUtilization", ""},
                {"YrBuilt", "ptas_yearbuilt"},
                {"YrRenovated", ""}
            };

        // TODO: add real field map.
        private static readonly Dictionary<string, string> SQLFieldMap = new Dictionary<string, string> {
                {"appraisalHistoryGuid", "appraisalHistoryGuid"},
                {"appraisedDate", "appraisedDate"},
                {"appraisserName", "appraisserName"},
                {"ApprMethodId", "ApprMethodId"},
                {"createdByName", "createdByName"},
                {"createdOn", "createdOn"},
                {"impsValue", "impsValue"},
                {"interfaceFlag", "interfaceFlag"},
                {"landId", "landId"},
                {"landValue", "landValue"},
                {"modifiedByName", "modifiedByName"},
                {"modifiedOn", "modifiedOn"},
                {"newConstrValue", "newConstrValue"},
                {"parcelGuid", "parcelGuid"},
                {"parcelIdName", "parcelIdName"},
                {"postDate", "postDate"},
                {"realPropId", "realPropId"},
                {"revalOrMaint", "revalOrMaint"},
                {"RollYear", "RollYear"}
            };

        [FunctionName("RealPropSyncFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //string name = req.Query["name"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var config = new RealPropConfig();
                var mdLoader = new D2SSyncHelpers.Services.DynamicsRelationsLoader(config);
                var data = JsonConvert.DeserializeObject<RealPropSyncPayload>(requestBody);

                if (data.IsOdata == 1)
                {
                    await SaveODataAsync(data, log);
                }
                else
                {
                    await SaveSQLAsync(data, log);
                }
                return new OkObjectResult(new
                {
                    result = "Ok",
                    data
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed on " + requestBody);
                throw;
            }
        }

        private static async Task SaveODataAsync(RealPropSyncPayload data, ILogger log)
        {
            data.Fields = data.Fields
                .Select(c =>
                    (Key: DynamicsFieldMap.TryGetValue(c.Key, out string value) ? value : $"", c.Value))
                .Where(t => !string.IsNullOrEmpty(t.Key))
                .ToDictionary(t => t.Key, t => t.Value);
            var entityName = DynamicsEntityMap[data.TableName];
            log.LogInformation($"OData: {data.TableName}=>{entityName}");
            RealPropConfig config = new RealPropConfig();
            var mdLoader = new D2SSyncHelpers.Services.DynamicsRelationsLoader(config);
            var md = await mdLoader.GetRelatedEntities(entityName);

            var renamedFields = data.Fields.Select(itm =>
            {
                var key = itm.Key;
                var value = itm.Value;
                if (key.EndsWith("_value"))
                {
                    var referencingAttribute = key.Replace("_value", "")[1..];
                    var foundReference = md.Where(fref => fref.ReferencingAttribute == referencingAttribute)
                        .FirstOrDefault() ?? throw new Exception(referencingAttribute + " not found.");
                    var plurarized = foundReference.ReferencedEntity + "s";
                    return new KeyValuePair<string, object>($"{referencingAttribute}@odata.bind", $"/{plurarized}({value})");
                };

                string strVal = value.ToString().ToUpper();
                if (strVal.Length == 1 && "YN".Contains(strVal))
                    return new KeyValuePair<string, object>(itm.Key, strVal == "Y");

                return new KeyValuePair<string, object>(itm.Key, itm.Value);
            }).ToDictionary(itm => itm.Key, itm => itm.Value);
            var asdas = new D2SSyncHelpers.Services.DynamicsLoader(config);
            await asdas.SaveEntity($"{entityName}s({data.PKValue})", renamedFields);
        }

        private static string CreateupsertScript(RealPropSyncPayload data)
        {
            var insertFieldNames = string.Join(
                ", ",
                data.Fields.Select(field => field.Key));
            var updateFields = string.Join(
                ",",
                data.Fields
                    .Where(f => f.Key != data.PKName)
                    .Select(f => $"{f.Key}={FieldValue(f)}"));

            var insertFieldReplace = string.Join(",", data.Fields.Select(field => FieldValue(field)));
            var initialQuery = @$"
            if (exists
                    (select * from {data.TableName}
                        where {data.PKName}='{data.PKValue}'))
            BEGIN
	            update {data.TableName} set {updateFields}
	            where {data.PKName}='{data.PKValue}'
            END ELSE BEGIN
	            insert into {data.TableName} ({insertFieldNames})
	            values ({insertFieldReplace})
            END
            ";
            return initialQuery;
        }

        private static string FieldValue(KeyValuePair<string, object> field)
        {
            return field.Value is null ? "null" : $"'{field.Value}'";
        }

        private static async Task SaveSQLAsync(RealPropSyncPayload data, ILogger log)
        {
            data.Fields = data.Fields
                .Select(f =>
                {
                    return (Key: SQLFieldMap[f.Key], Value: f.Value);
                })
                .Where(f => !string.IsNullOrEmpty(f.Key))
                .ToDictionary(i => i.Key, i => i.Value);
            data.Fields[data.PKName] = data.PKValue;
            data.TableName = SQLEntityMap[data.TableName];
            string query = CreateupsertScript(data);
            RealPropConfig config = new RealPropConfig();
            var x = new D2SSyncHelpers.Services.DataAccessLibrary(config);
            var result = await x.SaveData<object>(query, new { }, log);
            ////RealPropConfig config = new RealPropConfig();
            if (result < 1)
                throw new Exception("No data: " + query);
        }
    }

    public class RealPropSyncPayload
    {
        public Dictionary<string, object> Fields { get; set; }
        public int IsInsert { get; set; }
        public int IsOdata { get; set; }
        public string PKName { get; set; }
        public string PKValue { get; set; }
        public string TableName { get; set; }
    }
}