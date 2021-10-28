using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_homeimprovement
    {
        public ptas_homeimprovement()
        {
            ptas_fileattachmentmetadata = new HashSet<ptas_fileattachmentmetadata>();
            ptas_task = new HashSet<ptas_task>();
        }

        public Guid ptas_homeimprovementid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public string emailaddress { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public bool? ptas_applicationsignedbytaxpayer { get; set; }
        public int? ptas_applicationsource { get; set; }
        public string ptas_compositemailingaddress { get; set; }
        public DateTimeOffset? ptas_constructionbegindate { get; set; }
        public string ptas_constructionpropertyaddress { get; set; }
        public DateTimeOffset? ptas_dateapplicationreceived { get; set; }
        public DateTimeOffset? ptas_datepermitissued { get; set; }
        public string ptas_descriptionoftheimprovement { get; set; }
        public string ptas_emailaddress { get; set; }
        public DateTimeOffset? ptas_estimatedcompletiondate { get; set; }
        public decimal? ptas_estimatedconstructioncost { get; set; }
        public decimal? ptas_estimatedconstructioncost_base { get; set; }
        public decimal? ptas_exemptionamount { get; set; }
        public decimal? ptas_exemptionamount_base { get; set; }
        public int? ptas_exemptionnumber { get; set; }
        public bool? ptas_hipostcardsent { get; set; }
        public string ptas_name { get; set; }
        public string ptas_phonenumber { get; set; }
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
        public Guid? _ptas_buildingid_value { get; set; }
        public Guid? _ptas_exemptionbeginyearid_value { get; set; }
        public Guid? _ptas_exemptionendyearid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_permitid_value { get; set; }
        public Guid? _ptas_permitjurisdictionid_value { get; set; }
        public Guid? _ptas_portalcontactid_value { get; set; }
        public Guid? _ptas_taxaccountid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_buildingdetail _ptas_buildingid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_exemptionbeginyearid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_exemptionendyearid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual ptas_permit _ptas_permitid_valueNavigation { get; set; }
        public virtual ptas_jurisdiction _ptas_permitjurisdictionid_valueNavigation { get; set; }
        public virtual ptas_portalcontact _ptas_portalcontactid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_taxaccountid_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata { get; set; }
        public virtual ICollection<ptas_task> ptas_task { get; set; }
    }
}
