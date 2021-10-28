using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_permit
    {
        public ptas_permit()
        {
            ptas_homeimprovement = new HashSet<ptas_homeimprovement>();
            ptas_permitinspectionhistory = new HashSet<ptas_permitinspectionhistory>();
        }

        public Guid ptas_permitid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public bool? ptas_deactivatedviastatus { get; set; }
        public string ptas_description { get; set; }
        public int? ptas_errorreason { get; set; }
        public string ptas_integrationsource { get; set; }
        public DateTimeOffset? ptas_issueddate { get; set; }
        public int? ptas_jurisid { get; set; }
        public DateTimeOffset? ptas_latestpermitinspectiondate { get; set; }
        public int? ptas_latestpermitinspectiontype { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public string ptas_linktopermit { get; set; }
        public string ptas_minorifcondounit { get; set; }
        public string ptas_name { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public int? ptas_percentcomplete { get; set; }
        public int? ptas_permitsource { get; set; }
        public int? ptas_permitstatus { get; set; }
        public int? ptas_permittype { get; set; }
        public decimal? ptas_permitvalue { get; set; }
        public decimal? ptas_permitvalue_base { get; set; }
        public DateTimeOffset? ptas_planreadydate { get; set; }
        public int? ptas_planrequest { get; set; }
        public DateTimeOffset? ptas_planrequestdate { get; set; }
        public string ptas_projectaddress { get; set; }
        public int? ptas_projectdescriptionshortcut { get; set; }
        public string ptas_projectname { get; set; }
        public bool? ptas_qualifiesforhiex { get; set; }
        public DateTimeOffset? ptas_revieweddate { get; set; }
        public bool? ptas_showinspectionhistory { get; set; }
        public DateTimeOffset? ptas_statusdate { get; set; }
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
        public Guid? _ptas_condounitid_value { get; set; }
        public Guid? _ptas_currentjurisdiction_value { get; set; }
        public Guid? _ptas_issuedbyid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_reviewedbyid_value { get; set; }
        public Guid? _ptas_statusupdatedbyid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_condounitid_valueNavigation { get; set; }
        public virtual ptas_jurisdiction _ptas_currentjurisdiction_valueNavigation { get; set; }
        public virtual ptas_jurisdiction _ptas_issuedbyid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual systemuser _ptas_reviewedbyid_valueNavigation { get; set; }
        public virtual systemuser _ptas_statusupdatedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory { get; set; }
    }
}
