using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasCamanotesPtasFileattachmentmetad
    {
        public Guid PtasPtasCamanotesPtasFileattachmentmetadid { get; set; }
        public Guid? PtasCamanotesid { get; set; }
        public Guid? PtasFileattachmentmetadataid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
