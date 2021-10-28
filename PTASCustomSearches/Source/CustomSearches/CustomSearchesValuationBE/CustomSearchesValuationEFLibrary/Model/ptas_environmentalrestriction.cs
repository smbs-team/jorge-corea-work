using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_environmentalrestriction
    {
        public Guid ptas_environmentalrestrictionid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public bool? ptas_contaminationspecialistneeded { get; set; }
        public bool? ptas_delineationstudy { get; set; }
        public decimal? ptas_dollaradjustment { get; set; }
        public decimal? ptas_dollaradjustment_base { get; set; }
        public decimal? ptas_dollarpersqft { get; set; }
        public decimal? ptas_dollarpersqft_base { get; set; }
        public int? ptas_environmentalrestrictionsource { get; set; }
        public int? ptas_legacyrplandid { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_percentadjustment { get; set; }
        public int? ptas_percentaffected { get; set; }
        public int? ptas_percentremediationcost { get; set; }
        public string ptas_projectname { get; set; }
        public decimal? ptas_sqft { get; set; }
        public int? ptas_valuemethodenvironment { get; set; }
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
        public Guid? _ptas_environmentalrestrictiontypeid_value { get; set; }
        public Guid? _ptas_landid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_land _ptas_landid_valueNavigation { get; set; }
    }
}
