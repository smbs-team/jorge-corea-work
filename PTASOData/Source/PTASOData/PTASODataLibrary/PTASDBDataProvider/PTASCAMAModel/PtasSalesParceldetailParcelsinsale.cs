using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSalesParceldetailParcelsinsale
    {
        public Guid PtasSalesParceldetailParcelsinsaleid { get; set; }
        public Guid? PtasParceldetailid { get; set; }
        public Guid? PtasSalesid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
