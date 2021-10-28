using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCountyZipcode
    {
        public Guid PtasCountyZipcodeid { get; set; }
        public Guid? PtasCountyid { get; set; }
        public Guid? PtasZipcodeid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
