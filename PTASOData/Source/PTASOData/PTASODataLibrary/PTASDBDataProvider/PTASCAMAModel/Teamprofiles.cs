using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Teamprofiles
    {
        [Key]
        public Guid Teamprofileid { get; set; }
        public Guid? Fieldsecurityprofileid { get; set; }
        public Guid? Teamid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
