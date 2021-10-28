using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_changehistory
    {
        public int? chngID { get; set; }
        public int detailID { get; set; }
        public Guid? ChngGuid { get; set; }
        public Guid ChngDtlGuid { get; set; }
        public Guid? parcelGuid { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public DateTime? EventDate { get; set; }
        public string PropStatus { get; set; }
        public string DocId { get; set; }
        public Guid modifiedRecordPK { get; set; }
        public string modifiedRecordName { get; set; }
        public string modifiedRecordNewName { get; set; }
        public Guid? parentRecordPK { get; set; }
        public string parentRecordName { get; set; }
        public string entityDispName { get; set; }
        public string entitySchemaName { get; set; }
        public string attribDispName { get; set; }
        public string attribSchemaName { get; set; }
        public string displayValueNew { get; set; }
        public string displayValueNewMulti { get; set; }
        public string IDValueNew { get; set; }
        public string displayValueOriginal { get; set; }
        public string displayValueOriginalMulti { get; set; }
        public string IDValueOriginal { get; set; }
        public string LegacyId { get; set; }
        public string updatedBy { get; set; }
        public Guid? updatedByGuid { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ptasName { get; set; }
        public short? DataTypeItemId { get; set; }
        public string dataTypeDesc { get; set; }
    }
}
