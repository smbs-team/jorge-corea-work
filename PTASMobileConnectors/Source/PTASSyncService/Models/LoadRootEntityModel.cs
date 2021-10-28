using System.Collections.Generic;

namespace PTASSyncService.Models
{
    public class LoadRootEntityModel
    {
        public List<string> RootEntityList { get; set; }

        public LoadRootEntityModel()
        {
            this.RootEntityList = new List<string>();
        }
    }
}
