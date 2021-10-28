using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_taxaccount_snapshot
    {
        public Guid ptas_taxaccountid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_accountnumber { get; set; }
        public int? ptas_accounttype { get; set; }
        public int? ptas_accounttype_calc { get; set; }
        public string ptas_addr1_city { get; set; }
        public string ptas_addr1_compositeaddress { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public string ptas_addr1_intl_postalcode { get; set; }
        public string ptas_addr1_intl_stateprovince { get; set; }
        public string ptas_addr1_street_intl_address { get; set; }
        public bool? ptas_addressvalidated { get; set; }
        public string ptas_attentionname { get; set; }
        public string ptas_changesource { get; set; }
        public string ptas_email { get; set; }
        public bool? ptas_foreclosure { get; set; }
        public bool? ptas_isnonusaddress { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public int? ptas_legacyrpaccountid { get; set; }
        public string ptas_levycode_calc { get; set; }
        public bool? ptas_lockmailingaddress { get; set; }
        public decimal? ptas_lotacreage_calc { get; set; }
        public string ptas_mailingaddrfullline { get; set; }
        public string ptas_name { get; set; }
        public bool? ptas_noxiousweedexempt { get; set; }
        public string ptas_noxiousweedreason { get; set; }
        public string ptas_phone1 { get; set; }
        public int? ptas_propertytype { get; set; }
        public string ptas_propertytype_calc { get; set; }
        public int? ptas_ratetype { get; set; }
        public string ptas_reason { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public bool? ptas_soilfeeexempt { get; set; }
        public string ptas_soilfeereason { get; set; }
        public string ptas_splitcode_calc { get; set; }
        public bool? ptas_subjecttoforeclosure { get; set; }
        public int? ptas_taxablestatus { get; set; }
        public string ptas_taxpayername { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _ownerid_value { get; set; }
        public Guid? _owningbusinessunit_value { get; set; }
        public Guid? _owningteam_value { get; set; }
        public Guid? _owninguser_value { get; set; }
        public Guid? _ptas_addr1_cityid_value { get; set; }
        public Guid? _ptas_addr1_countryid_value { get; set; }
        public Guid? _ptas_addr1_stateid_value { get; set; }
        public Guid? _ptas_addr1_zipcodeid_value { get; set; }
        public Guid? _ptas_condounitid_value { get; set; }
        public Guid? _ptas_levycodeid_value { get; set; }
        public Guid? _ptas_masspayerid_value { get; set; }
        public Guid? _ptas_mastertaxaccountid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_personalpropertyid_value { get; set; }
    }
}
