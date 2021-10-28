using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasEconomicunitAccessorydetail
    {
        public Guid PtasEconomicunitAccessorydetailid { get; set; }
        public Guid? PtasAccessorydetailid { get; set; }
        public Guid? PtasEconomicunitid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
