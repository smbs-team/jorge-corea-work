using System.Collections.Generic;

namespace PTASSyncService.Models
{
    public class AssignToUserModel
    {
        public List<string> entityIdentifierList { get; set; }
        public string userName { get; set; }
        public int userId { get; set; }
    }
}
