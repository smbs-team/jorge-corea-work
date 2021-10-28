using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCondocomplexPtasMediarepository
    {
        public Guid PtasCondocomplexPtasMediarepositoryid { get; set; }
        public Guid? PtasCondocomplexid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
