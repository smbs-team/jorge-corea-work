using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasPermitPtasMediarepository
    {
        public Guid PtasPtasPermitPtasMediarepositoryid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public Guid? PtasPermitid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
