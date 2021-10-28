using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCamanotes
    {
        public Guid PtasCamanotesid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAttachedentitydisplayname { get; set; }
        public string PtasAttachedentitypk { get; set; }
        public string PtasAttachedentityschemaname { get; set; }
        public string PtasExcisetaxnmbr { get; set; }
        public string PtasFullsitusaddress { get; set; }
        public bool? PtasHasmedia { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasMigratednoteid { get; set; }
        public string PtasMigratednotetype { get; set; }
        public string PtasMinornumber { get; set; }
        public string PtasName { get; set; }
        public string PtasNotetext { get; set; }
        public int? PtasNotetype { get; set; }
        public bool? PtasPintotop { get; set; }
        public string PtasSitusaddress { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }
        public Guid? PtasMinorparcelidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasPropertyreviewidValue { get; set; }
        public Guid? PtasValuationyearidValue { get; set; }
    }
}
