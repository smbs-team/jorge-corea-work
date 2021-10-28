using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PTAS_Console_test
{
    internal class Program
    {
        private static string jSonPayload = @"{
	""TableName"": ""ResBldg"",
	""PKName"": ""BldgGuid"",
	""PKValue"": ""bee3af36-60d8-4e01-9a08-000007d83a70"",
	""isOdata"": 1,
	""isInsert"": 0,
	""Fields"": {
		""Id"": 81603,
		""BldgGuid"": ""bee3af36-60d8-4e01-9a08-000007d83a70"",
		""RpGuid"": ""7a674407-ab4d-4f2e-b5e0-b85221aff620"",
		""Active"": true,
		""BldgNbr"": 1,
		""NbrLivingUnits"": 1,
		""Stories"": 1,
		""BldgGradeId"": 82,
		""BldgGradeItemId"": 7,
		""BldgGradeVar"": 0,
		""SqFt1stFloor"": 880,
		""SqFtHalfFloor"": 0,
		""SqFt2ndFloor"": 0,
		""SqFtUpperFloor"": 0,
		""SqFtUnfinFull"": 0,
		""SqFtUnfinHalf"": 0,
		""SqFtTotLiving"": 1100,
		""SqFtTotBasement"": 450,
		""SqFtFinBasement"": 220,
		""FinBasementGradeId"": 82,
		""FinBasementGradeItemId"": 5,
		""SqFtGarageBasement"": 0,
		""SqFtGarageAttached"": 0,
		""DaylightBasement"": ""N"",
		""Bedrooms"": 3,
		""BathHalfCount"": 0,
		""Bath3qtrCount"": 0,
		""BathFullCount"": 1,
		""FpSingleStory"": 1,
		""FpMultiStory"": 0,
		""FpFreestanding"": 0,
		""FpAdditional"": 0,
		""YrBuilt"": 1907,
		""YrRenovated"": 0,
		""PcntComplete"": 0,
		""Obsolescence"": 0,
		""PcntNetCondition"": 0,
		""ConditionId"": 83,
		""ConditionItemId"": 4,
		""ViewUtilization"": ""N"",
		""SqFtOpenPorch"": 100,
		""SqFtEnclosedPorch"": 0,
		""SqFtDeck"": 0,
		""HeatSystemId"": 108,
		""HeatSystemItemId"": 5,
		""HeatSourceId"": 84,
		""HeatSourceItemId"": 1,
		""BrickStone"": 0,
		""AddnlCost"": 0,
		""CreateDate"": null,
		""UpdatedByGuid"": ""942d8774-857f-46cf-b163-ca65753798a5"",
		""UpdateDate"": ""12/28/2020 2:33:00 PM""
	}
}";

        private static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var httpContent = new StringContent(jSonPayload, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://realpropsyncfunction.azurewebsites.net/api/RealPropSyncFunction?code=1/ShtO8kT1/xrrMFWlsWSdR0P4SkHdJyg1I5mZL/bFHOAm6XvDm7/w==", httpContent);
            var resStr = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resStr);
        }
    }
}