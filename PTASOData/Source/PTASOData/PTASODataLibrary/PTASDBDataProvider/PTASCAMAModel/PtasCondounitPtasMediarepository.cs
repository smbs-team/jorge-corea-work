using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCondounitPtasMediarepository
    {
        public Guid PtasCondounitPtasMediarepositoryid { get; set; }
        public Guid? PtasCondounitid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
