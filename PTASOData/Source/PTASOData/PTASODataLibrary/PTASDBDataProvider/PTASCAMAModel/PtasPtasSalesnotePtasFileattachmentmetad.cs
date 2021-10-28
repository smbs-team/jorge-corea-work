using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasSalesnotePtasFileattachmentmetad
    {
        public Guid PtasPtasSalesnotePtasFileattachmentmetadid { get; set; }
        public Guid? PtasFileattachmentmetadataid { get; set; }
        public Guid? PtasSalesnoteid { get; set; }
        public long? Versionnumber { get; set; }
    }
}
