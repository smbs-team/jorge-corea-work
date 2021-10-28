using System.Collections.Generic;

namespace PTASSyncService.Models
{
    public class UploadEntityModel
    {
        public string kind { get; set; }

        public List<Dictionary<string, object>> jsonEntities { get; set; }
    }
}
