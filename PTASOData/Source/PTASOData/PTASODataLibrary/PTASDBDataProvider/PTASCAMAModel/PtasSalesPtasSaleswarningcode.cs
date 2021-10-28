using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSalesPtasSaleswarningcode
    {
        public Guid PtasSalesPtasSaleswarningcodeid { get; set; }
        public Guid? PtasSalesid { get; set; }
        public Guid? PtasSaleswarningcodeid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
