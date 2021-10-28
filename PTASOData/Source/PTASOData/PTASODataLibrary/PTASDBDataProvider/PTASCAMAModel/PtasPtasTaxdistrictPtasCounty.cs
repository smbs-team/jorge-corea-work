using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasTaxdistrictPtasCounty
    {
        public Guid PtasPtasTaxdistrictPtasCountyid { get; set; }
        public Guid? PtasCountyid { get; set; }
        public Guid? PtasTaxdistrictid { get; set; }
        public long? Versionnumber { get; set; }
    }
}
