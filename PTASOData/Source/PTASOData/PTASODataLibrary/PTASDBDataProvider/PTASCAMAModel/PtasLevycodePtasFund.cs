using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLevycodePtasFund
    {
        public Guid PtasLevycodePtasFundid { get; set; }
        public Guid? PtasFundid { get; set; }
        public Guid? PtasLevycodeid { get; set; }
        public long? Versionnumber { get; set; }
    }
}
