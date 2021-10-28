using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCamanotesPtasMediarepository
    {
        public Guid PtasCamanotesPtasMediarepositoryid { get; set; }
        public Guid? PtasCamanotesid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
