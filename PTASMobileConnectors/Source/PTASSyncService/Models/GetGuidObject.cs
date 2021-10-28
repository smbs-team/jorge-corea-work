using System.Collections.Generic;

namespace PTASSyncService.Models
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
