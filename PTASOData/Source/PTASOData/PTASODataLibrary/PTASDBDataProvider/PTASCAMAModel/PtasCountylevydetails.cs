using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCountylevydetails
    {
        public Guid PtasCountylevydetailsid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? Ptas100timberassessedval { get; set; }
        public decimal? Ptas100timberassessedvalBase { get; set; }
        public decimal? Ptas80timberassessedval { get; set; }
        public decimal? Ptas80timberassessedvalBase { get; set; }
        public decimal? PtasAnnexassessedval { get; set; }
        public decimal? PtasAnnexassessedvalBase { get; set; }
        public decimal? PtasCountypercenttaxdistrict { get; set; }
        public decimal? PtasExcessassessedval { get; set; }
        public decimal? PtasExcessassessedvalBase { get; set; }
        public decimal? PtasLocalnewcons { get; set; }
        public decimal? PtasLocalnewconsBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasRefundamt { get; set; }
        public decimal? PtasRefundamtBase { get; set; }
        public decimal? PtasReglevyassessedval { get; set; }
        public decimal? PtasReglevyassessedvalBase { get; set; }
        public decimal? PtasUtilityval { get; set; }
        public decimal? PtasUtilityvalBase { get; set; }
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
        public Guid? PtasCountyidValue { get; set; }
        public Guid? PtasLevylimitworksheetdetailidValue { get; set; }
        public Guid? PtasTaxdistrictidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }
    }
}
