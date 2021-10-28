using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Stringmap
    {
        public Guid Stringmapid { get; set; }
        public string Attributename { get; set; }
        public long? Attributevalue { get; set; }
        public int? Displayorder { get; set; }
        public int? Langid { get; set; }
        public string Objecttypecode { get; set; }
        public string Value { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? Organizationid { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}
