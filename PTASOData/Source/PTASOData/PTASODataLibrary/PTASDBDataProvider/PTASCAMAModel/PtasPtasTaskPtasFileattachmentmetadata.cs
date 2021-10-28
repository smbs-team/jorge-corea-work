using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasTaskPtasFileattachmentmetadata
    {
        public Guid PtasPtasTaskPtasFileattachmentmetadataid { get; set; }
        public Guid? PtasFileattachmentmetadataid { get; set; }
        public Guid? PtasTaskid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
