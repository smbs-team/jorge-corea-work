using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasBookmarkPtasBookmarktag
    {
        public Guid PtasPtasBookmarkPtasBookmarktagid { get; set; }
        public Guid? PtasBookmarkid { get; set; }
        public Guid? PtasBookmarktagid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
