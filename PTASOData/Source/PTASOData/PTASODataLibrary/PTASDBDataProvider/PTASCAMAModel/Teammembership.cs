using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Teammembership
    {
        [Key]
        public Guid Teammembershipid { get; set; }
        public Guid? Systemuserid { get; set; }
        public Guid? Teamid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
