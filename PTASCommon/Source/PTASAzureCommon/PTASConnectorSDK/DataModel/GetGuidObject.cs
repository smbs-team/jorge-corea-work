using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTASConnectorSDK.DataModel
{
    public class GetGuidObject
    {
        public string kind { get; set; }

        public List<Dictionary<string, object>> keyFields { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGuidObject" /> class.
        /// </summary>
        public GetGuidObject()
        {
            keyFields = new List<Dictionary<string, object>>();
        }
    }
}
