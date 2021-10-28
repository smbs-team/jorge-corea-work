using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_jurisdiction
    {
        public ptas_jurisdiction()
        {
            ptas_homeimprovement = new HashSet<ptas_homeimprovement>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_permit_ptas_currentjurisdiction_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_ptas_issuedbyid_valueNavigation = new HashSet<ptas_permit>();
        }

        public Guid ptas_jurisdictionid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_abbreviation { get; set; }
        public int? ptas_advancenoticeforplansdays { get; set; }
        public string ptas_appraisalnotes { get; set; }
        public string ptas_description { get; set; }
        public int? ptas_id { get; set; }
        public string ptas_mainphonenumber { get; set; }
        public string ptas_name { get; set; }
        public string ptas_number { get; set; }
        public string ptas_planrequestemail { get; set; }
        public string ptas_planrequesturl { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _organizationid_value { get; set; }
        public Guid? _ptas_commercialcontactid_value { get; set; }
        public Guid? _ptas_permitwebsiteconfigid_value { get; set; }
        public Guid? _ptas_residentialcontactid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ptas_permitwebsiteconfig _ptas_permitwebsiteconfigid_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_ptas_currentjurisdiction_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_ptas_issuedbyid_valueNavigation { get; set; }
    }
}
