using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTaxaccountSnapshot
    {
        [Key]
        public Guid PtasTaxaccountid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public int? PtasAccounttype { get; set; }
        public int? PtasAccounttypeCalc { get; set; }
        public string PtasAddr1City { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public string PtasAddr1IntlPostalcode { get; set; }
        public string PtasAddr1IntlStateprovince { get; set; }
        public string PtasAddr1StreetIntlAddress { get; set; }
        public bool? PtasAddressvalidated { get; set; }
        public string PtasAttentionname { get; set; }
        public string PtasChangesource { get; set; }
        public string PtasEmail { get; set; }
        public bool? PtasForeclosure { get; set; }
        public bool? PtasIsnonusaddress { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLegacyrpaccountid { get; set; }
        public string PtasLevycodeCalc { get; set; }
        public bool? PtasLockmailingaddress { get; set; }
        public decimal? PtasLotacreageCalc { get; set; }
        public string PtasMailingaddrfullline { get; set; }
        public string PtasName { get; set; }
        public bool? PtasNoxiousweedexempt { get; set; }
        public string PtasNoxiousweedreason { get; set; }
        public bool? PtasPaperless { get; set; }
        public string PtasPhone1 { get; set; }
        public int? PtasPropertytype { get; set; }
        public string PtasPropertytypeCalc { get; set; }
        public int? PtasRatetype { get; set; }
        public string PtasReason { get; set; }
        public int? PtasSeniorexemption { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public bool? PtasSoilfeeexempt { get; set; }
        public string PtasSoilfeereason { get; set; }
        public string PtasSplitcodeCalc { get; set; }
        public bool? PtasSubjecttoforeclosure { get; set; }
        public int? PtasTaxablestatus { get; set; }
        public string PtasTaxpayername { get; set; }
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
        public Guid? PtasAddr1CityidValue { get; set; }
        public Guid? PtasAddr1CountryidValue { get; set; }
        public Guid? PtasAddr1StateidValue { get; set; }
        public Guid? PtasAddr1ZipcodeidValue { get; set; }
        public Guid? PtasCondounitidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasMasspayeridValue { get; set; }
        public Guid? PtasMastertaxaccountidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasPersonalpropertyidValue { get; set; }
    }
}
