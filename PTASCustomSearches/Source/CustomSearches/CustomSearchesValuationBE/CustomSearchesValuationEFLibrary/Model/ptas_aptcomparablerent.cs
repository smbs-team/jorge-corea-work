using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptcomparablerent
    {
        public Guid ptas_aptcomparablerentid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_absoluteadjustsment { get; set; }
        public decimal? ptas_adjustedrent { get; set; }
        public decimal? ptas_adjustedrent_base { get; set; }
        public decimal? ptas_ageadjustment { get; set; }
        public decimal? ptas_airportnoiseadjustment { get; set; }
        public int? ptas_assessmentyear { get; set; }
        public int? ptas_comp { get; set; }
        public decimal? ptas_conditionadjustment { get; set; }
        public decimal? ptas_distancemetriccombinedadjustment { get; set; }
        public decimal? ptas_locationadjustment { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_netadjustment { get; set; }
        public decimal? ptas_numberofbathroomsadjustment { get; set; }
        public decimal? ptas_numberofbedroomsadjustment { get; set; }
        public decimal? ptas_pooladjustment { get; set; }
        public decimal? ptas_proximityadjustment { get; set; }
        public int? ptas_proximitycode { get; set; }
        public decimal? ptas_qualityadjustment { get; set; }
        public decimal? ptas_unitsizeadjustment1 { get; set; }
        public decimal? ptas_unitsizeadjustment2 { get; set; }
        public decimal? ptas_unittypeadjustment { get; set; }
        public decimal? ptas_viewadjustment1 { get; set; }
        public decimal? ptas_viewadjustment2 { get; set; }
        public decimal? ptas_weightingdenominator { get; set; }
        public decimal? ptas_yearbuiltadjustment { get; set; }
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
        public Guid? _ptas_rentid_value { get; set; }
        public Guid? _ptas_rentsubjectid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_aptlistedrent _ptas_rentid_valueNavigation { get; set; }
        public virtual ptas_aptvaluation _ptas_rentsubjectid_valueNavigation { get; set; }
    }
}
