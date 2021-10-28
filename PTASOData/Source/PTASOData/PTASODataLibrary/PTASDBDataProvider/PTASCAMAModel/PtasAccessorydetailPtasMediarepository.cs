using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAccessorydetailPtasMediarepository
    {
        public Guid PtasAccessorydetailPtasMediarepositoryid { get; set; }
        public Guid? PtasAccessorydetailid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
