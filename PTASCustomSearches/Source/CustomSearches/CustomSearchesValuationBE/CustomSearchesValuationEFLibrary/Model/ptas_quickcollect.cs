using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_quickcollect
    {
        public Guid ptas_quickcollectid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_assessmentvalue { get; set; }
        public decimal? ptas_assessmentvalue_base { get; set; }
        public string ptas_attention { get; set; }
        public string ptas_businessname { get; set; }
        public DateTimeOffset? ptas_closingdate { get; set; }
        public DateTimeOffset? ptas_date { get; set; }
        public string ptas_email { get; set; }
        public decimal? ptas_equipment { get; set; }
        public decimal? ptas_equipment_base { get; set; }
        public string ptas_futurestatus { get; set; }
        public decimal? ptas_intangibles { get; set; }
        public decimal? ptas_intangibles_base { get; set; }
        public decimal? ptas_leaseholdimprovements { get; set; }
        public decimal? ptas_leaseholdimprovements_base { get; set; }
        public string ptas_legalentity { get; set; }
        public string ptas_name { get; set; }
        public string ptas_newbusinessname { get; set; }
        public string ptas_newinformation_addr_city { get; set; }
        public string ptas_newinformation_addr_locationcity { get; set; }
        public string ptas_newinformation_addr_locationstreet1 { get; set; }
        public string ptas_newinformation_addr_locationstreet2 { get; set; }
        public string ptas_newinformation_addr_locationzip { get; set; }
        public string ptas_newinformation_addr_street1 { get; set; }
        public string ptas_newinformation_addr_street2 { get; set; }
        public string ptas_newinformation_addr_zip { get; set; }
        public string ptas_newowneremail { get; set; }
        public string ptas_newownername { get; set; }
        public bool? ptas_oktopost { get; set; }
        public decimal? ptas_other { get; set; }
        public decimal? ptas_other_base { get; set; }
        public string ptas_ownername { get; set; }
        public bool? ptas_paid { get; set; }
        public string ptas_personalpropinfo_addr_city { get; set; }
        public string ptas_personalpropinfo_addr_locationcity { get; set; }
        public string ptas_personalpropinfo_addr_locationstreet1 { get; set; }
        public string ptas_personalpropinfo_addr_locationstreet2 { get; set; }
        public string ptas_personalpropinfo_addr_locationzip { get; set; }
        public string ptas_personalpropinfo_addr_street1 { get; set; }
        public string ptas_personalpropinfo_addr_street2 { get; set; }
        public string ptas_personalpropinfo_addr_zip { get; set; }
        public string ptas_quickcollectnumber { get; set; }
        public int? ptas_reasonforrequest { get; set; }
        public string ptas_requestorinfo_addr_city { get; set; }
        public string ptas_requestorinfo_addr_street1 { get; set; }
        public string ptas_requestorinfo_addr_street2 { get; set; }
        public string ptas_requestorinfo_addr_telephone { get; set; }
        public string ptas_requestorinfo_addr_zip { get; set; }
        public string ptas_requestorinfo_businessname { get; set; }
        public string ptas_telephone { get; set; }
        public decimal? ptas_totalsalesprice { get; set; }
        public decimal? ptas_totalsalesprice_base { get; set; }
        public string ptas_ubinumber { get; set; }
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
        public Guid? _ptas_billofsale_fileattachementmetadataid_value { get; set; }
        public Guid? _ptas_newinformation_addr_locationstateid_value { get; set; }
        public Guid? _ptas_newinformation_addr_stateid_value { get; set; }
        public Guid? _ptas_personalpropertyid_value { get; set; }
        public Guid? _ptas_personalpropinfo_addr_locationstateid_value { get; set; }
        public Guid? _ptas_personalpropinfo_addr_stateid_value { get; set; }
        public Guid? _ptas_processeduserid_value { get; set; }
        public Guid? _ptas_quickcollect_personalpropertyid_value { get; set; }
        public Guid? _ptas_requestorinfo_addr_stateid_value { get; set; }
        public Guid? _ptas_yearid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_fileattachmentmetadata _ptas_billofsale_fileattachementmetadataid_valueNavigation { get; set; }
        public virtual ptas_stateorprovince _ptas_newinformation_addr_locationstateid_valueNavigation { get; set; }
        public virtual ptas_stateorprovince _ptas_newinformation_addr_stateid_valueNavigation { get; set; }
        public virtual ptas_stateorprovince _ptas_personalpropinfo_addr_locationstateid_valueNavigation { get; set; }
        public virtual ptas_stateorprovince _ptas_personalpropinfo_addr_stateid_valueNavigation { get; set; }
        public virtual systemuser _ptas_processeduserid_valueNavigation { get; set; }
        public virtual ptas_stateorprovince _ptas_requestorinfo_addr_stateid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_yearid_valueNavigation { get; set; }
    }
}
