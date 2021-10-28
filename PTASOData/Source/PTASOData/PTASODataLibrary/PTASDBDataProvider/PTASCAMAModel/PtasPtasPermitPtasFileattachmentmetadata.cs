using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasPermitPtasFileattachmentmetadata
    {
        public Guid PtasPtasPermitPtasFileattachmentmetadataid { get; set; }
        public Guid? PtasFileattachmentmetadataid { get; set; }
        public Guid? PtasPermitid { get; set; }
        public long? Versionnumber { get; set; }
    }
}
