using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasParceldetailPtasMediarepository
    {
        public Guid PtasParceldetailPtasMediarepositoryid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public Guid? PtasParceldetailid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
