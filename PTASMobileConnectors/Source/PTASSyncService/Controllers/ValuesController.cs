using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Swashbuckle.AspNetCore.Annotations;
using PTASSyncService.Utilities;

namespace PTASSyncService.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public static IDbConnection OpenConnection(string connStr)
        {
            string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
            var conn = new SqlConnection(connStr);
            conn.AccessToken = sqlToken;
            conn.Open();
            return conn;
        }

        // GET api/values
        [HttpGet]
        [SwaggerOperation(OperationId = "ValuesGet")]
        public IEnumerable<int> Get()
        {
            return new int[] { 1,2 };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "ValuesIdGet")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [SwaggerOperation(OperationId = "ValuesPost")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [SwaggerOperation(OperationId = "ValuesPut")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "ValuesDelete")]
        public void Delete(int id)
        {
        }
    }
}
