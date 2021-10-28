using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasChangehistory
    {
        public int? ChngId { get; set; }
        public int DetailId { get; set; }
        public Guid? ChngGuid { get; set; }
        [Key]
        public Guid ChngDtlGuid { get; set; }
        public Guid? ParcelGuid { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public DateTime? EventDate { get; set; }
        public string PropStatus { get; set; }
        public string DocId { get; set; }
        public Guid ModifiedRecordPk { get; set; }
        public string ModifiedRecordName { get; set; }
        public string ModifiedRecordNewName { get; set; }
        public Guid? ParentRecordPk { get; set; }
        public string ParentRecordName { get; set; }
        public string EntityDispName { get; set; }
        public string EntitySchemaName { get; set; }
        public string AttribDispName { get; set; }
        public string AttribSchemaName { get; set; }
        public string DisplayValueNew { get; set; }
        public string DisplayValueNewMulti { get; set; }
        public string IdvalueNew { get; set; }
        public string DisplayValueOriginal { get; set; }
        public string DisplayValueOriginalMulti { get; set; }
        public string IdvalueOriginal { get; set; }
        public string LegacyId { get; set; }
        public string UpdatedBy { get; set; }
        public Guid? UpdatedByGuid { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string PtasName { get; set; }
        public short? DataTypeItemId { get; set; }
        public string DataTypeDesc { get; set; }
        public string EventTypeDesc { get; set; }
    }
}
