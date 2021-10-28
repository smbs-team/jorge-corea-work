using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_task
    {
        public Guid ptas_taskid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_accessoriesindestroyedproperty { get; set; }
        public string ptas_anticipatedrepairdates { get; set; }
        public decimal? ptas_appdeterredinval_assessedval_imps { get; set; }
        public decimal? ptas_appdeterredinval_assessedval_imps_base { get; set; }
        public decimal? ptas_appdeterredinval_assessedval_land { get; set; }
        public decimal? ptas_appdeterredinval_assessedval_land_base { get; set; }
        public DateTimeOffset? ptas_appdeterredinval_calendaryear { get; set; }
        public decimal? ptas_appdeterredinval_fullmarketvalue_imps { get; set; }
        public decimal? ptas_appdeterredinval_fullmarketvalue_imps_base { get; set; }
        public decimal? ptas_appdeterredinval_fullmarketvalue_land { get; set; }
        public decimal? ptas_appdeterredinval_fullmarketvalue_land_base { get; set; }
        public int? ptas_appdeterredinval_taxrollcollectionfor { get; set; }
        public decimal? ptas_appdeterredinval_totalamount_imps { get; set; }
        public decimal? ptas_appdeterredinval_totalamount_imps_base { get; set; }
        public decimal? ptas_appdeterredinval_totalamount_land { get; set; }
        public decimal? ptas_appdeterredinval_totalamount_land_base { get; set; }
        public decimal? ptas_appraisedimprovementincrease { get; set; }
        public decimal? ptas_appraisedimprovementincrease_base { get; set; }
        public decimal? ptas_appraisedimprovementvalue { get; set; }
        public decimal? ptas_appraisedimprovementvalue_base { get; set; }
        public decimal? ptas_appraisedlandvalue { get; set; }
        public decimal? ptas_appraisedlandvalue_base { get; set; }
        public decimal? ptas_appraisedtotalvalue { get; set; }
        public decimal? ptas_appraisedtotalvalue_base { get; set; }
        public string ptas_area { get; set; }
        public int? ptas_changereason { get; set; }
        public string ptas_citystatezip { get; set; }
        public string ptas_claimdisqualificationreason { get; set; }
        public int? ptas_claimqualificationreason { get; set; }
        public string ptas_comments { get; set; }
        public int? ptas_convertpropertytypefrom { get; set; }
        public int? ptas_convertpropertytypeto { get; set; }
        public DateTimeOffset? ptas_dateofdestruction { get; set; }
        public DateTimeOffset? ptas_datesigned { get; set; }
        public string ptas_descriptionofdestroyedproperty { get; set; }
        public string ptas_folionumber { get; set; }
        public int? ptas_hitasktype { get; set; }
        public string ptas_lossoccurringasaresultof { get; set; }
        public string ptas_mailingaddress { get; set; }
        public string ptas_mailingcitystatezip { get; set; }
        public string ptas_name { get; set; }
        public string ptas_othercomments { get; set; }
        public string ptas_permitissuedby { get; set; }
        public string ptas_phonenumber { get; set; }
        public int? ptas_postadditionalrecordtorollyear { get; set; }
        public string ptas_propertyaddress { get; set; }
        public string ptas_qstr { get; set; }
        public bool? ptas_repairdatesunknownatthistime { get; set; }
        public int? ptas_revaluenoticeflag { get; set; }
        public bool? ptas_reviewedbyacctsectsupervisor { get; set; }
        public bool? ptas_reviewedbycommsrappraiser { get; set; }
        public bool? ptas_reviewedbyressrappraiser { get; set; }
        public int? ptas_selectmethod { get; set; }
        public int? ptas_selectreason { get; set; }
        public int? ptas_signedby { get; set; }
        public string ptas_splitcode { get; set; }
        public string ptas_subarea { get; set; }
        public int? ptas_submissionsource { get; set; }
        public string ptas_taskdescription { get; set; }
        public int? ptas_tasktype { get; set; }
        public decimal? ptas_taxableimprovementvalue { get; set; }
        public decimal? ptas_taxableimprovementvalue_base { get; set; }
        public decimal? ptas_taxablelandvalue { get; set; }
        public decimal? ptas_taxablelandvalue_base { get; set; }
        public decimal? ptas_taxabletotalvalue { get; set; }
        public decimal? ptas_taxabletotalvalue_base { get; set; }
        public string ptas_taxpayername { get; set; }
        public int? ptas_taxrollyear { get; set; }
        public int? ptas_taxvaluereason { get; set; }
        public string ptas_zoning { get; set; }
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
        public Guid? _ptas_accountingsectionsupervisor_value { get; set; }
        public Guid? _ptas_appraiser_value { get; set; }
        public Guid? _ptas_commercialsrappraiser_value { get; set; }
        public Guid? _ptas_convertpropertytypefromid_value { get; set; }
        public Guid? _ptas_convertpropertytypetoid_value { get; set; }
        public Guid? _ptas_homeimprovementid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_portalcontact_value { get; set; }
        public Guid? _ptas_residentialsrappraiser_value { get; set; }
        public Guid? _ptas_responsibilityfrom_value { get; set; }
        public Guid? _ptas_responsibilityto_value { get; set; }
        public Guid? _ptas_salesid_value { get; set; }
        public Guid? _ptas_taxaccountnumber_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual systemuser _ptas_accountingsectionsupervisor_valueNavigation { get; set; }
        public virtual systemuser _ptas_appraiser_valueNavigation { get; set; }
        public virtual systemuser _ptas_commercialsrappraiser_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_convertpropertytypefromid_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_convertpropertytypetoid_valueNavigation { get; set; }
        public virtual ptas_homeimprovement _ptas_homeimprovementid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual ptas_portalcontact _ptas_portalcontact_valueNavigation { get; set; }
        public virtual systemuser _ptas_residentialsrappraiser_valueNavigation { get; set; }
        public virtual ptas_responsibility _ptas_responsibilityfrom_valueNavigation { get; set; }
        public virtual ptas_responsibility _ptas_responsibilityto_valueNavigation { get; set; }
        public virtual ptas_sales _ptas_salesid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_taxaccountnumber_valueNavigation { get; set; }
    }
}
