using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTaxdistrictLevycode
    {
        public Guid PtasTaxdistrictLevycodeid { get; set; }
        public Guid? PtasLevycodeid { get; set; }
        public Guid? PtasTaxdistrictid { get; set; }
        public long? Versionnumber { get; set; }
    }
}
