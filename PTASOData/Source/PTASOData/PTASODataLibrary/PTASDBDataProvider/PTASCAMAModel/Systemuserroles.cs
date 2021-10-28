using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Systemuserroles
    {
        [Key]
        public Guid Systemuserroleid { get; set; }
        public Guid? Roleid { get; set; }
        public Guid? Systemuserid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
