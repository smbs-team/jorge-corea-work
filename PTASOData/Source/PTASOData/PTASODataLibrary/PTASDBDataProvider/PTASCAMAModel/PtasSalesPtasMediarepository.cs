using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSalesPtasMediarepository
    {
        public Guid PtasSalesPtasMediarepositoryid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public Guid? PtasSalesid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
