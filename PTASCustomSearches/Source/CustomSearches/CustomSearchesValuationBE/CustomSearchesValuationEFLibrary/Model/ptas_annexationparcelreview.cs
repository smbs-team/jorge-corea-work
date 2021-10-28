using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_annexationparcelreview
    {
        public Guid ptas_annexationparcelreviewid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_assessedimprovements { get; set; }
        public decimal? ptas_assessedimprovements_base { get; set; }
        public decimal? ptas_assessedland { get; set; }
        public decimal? ptas_assessedland_base { get; set; }
        public string ptas_name { get; set; }
        public string ptas_propertyowner { get; set; }
        public bool? ptas_signedpetition { get; set; }
        public decimal? ptas_totalassessedvalue { get; set; }
        public decimal? ptas_totalassessedvalue_base { get; set; }
        public bool? ptas_verifiedpropertyowner { get; set; }
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
        public Guid? _ptas_annexationtrackerid_value { get; set; }
        public Guid? _ptas_levycodeid_value { get; set; }
        public Guid? _ptas_parcel_value { get; set; }
        public Guid? _ptas_taxrollyearforav_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_annexationtracker _ptas_annexationtrackerid_valueNavigation { get; set; }
        public virtual ptas_levycode _ptas_levycodeid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcel_valueNavigation { get; set; }
        public virtual ptas_year _ptas_taxrollyearforav_valueNavigation { get; set; }
    }
}
