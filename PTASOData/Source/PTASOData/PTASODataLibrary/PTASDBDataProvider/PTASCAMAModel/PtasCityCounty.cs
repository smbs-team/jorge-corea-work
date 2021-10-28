using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCityCounty
    {
        public Guid PtasCityCountyid { get; set; }
        public Guid? PtasCityid { get; set; }
        public Guid? PtasCountyid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
