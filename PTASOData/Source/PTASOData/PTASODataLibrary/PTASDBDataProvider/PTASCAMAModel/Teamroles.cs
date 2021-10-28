using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Teamroles
    {
        [Key]
        public Guid Teamroleid { get; set; }
        public Guid? Roleid { get; set; }
        public Guid? Teamid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
