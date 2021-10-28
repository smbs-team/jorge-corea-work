using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasSalesPtasFileattachmentmetadata
    {
        public Guid PtasPtasSalesPtasFileattachmentmetadataid { get; set; }
        public Guid? PtasFileattachmentmetadataid { get; set; }
        public Guid? PtasSalesid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
