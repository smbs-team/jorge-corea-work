using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptcomparablesale
    {
        public Guid ptas_aptcomparablesaleid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_absoluteadjustmentwithoutlocation { get; set; }
        public decimal? ptas_absoluteadjustsment { get; set; }
        public decimal? ptas_adjustedsaleprice { get; set; }
        public decimal? ptas_adjustedsaleprice_base { get; set; }
        public decimal? ptas_adjustedsalepriceperunit { get; set; }
        public decimal? ptas_adjustedsalepriceperunit_base { get; set; }
        public decimal? ptas_ageadjustment { get; set; }
        public decimal? ptas_aggregateabsoluteadjustment { get; set; }
        public decimal? ptas_airportnoiseadjustment { get; set; }
        public int? ptas_assessmentyear { get; set; }
        public decimal? ptas_averageunitsizeadjustment { get; set; }
        public decimal? ptas_buildingqualityadjustment { get; set; }
        public int? ptas_comp { get; set; }
        public decimal? ptas_complementofabsoluteadjustment { get; set; }
        public decimal? ptas_compweight { get; set; }
        public decimal? ptas_conditionadjustment { get; set; }
        public decimal? ptas_distancemetriccombinedadjustment { get; set; }
        public decimal? ptas_locationadjustment { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_netadjustment { get; set; }
        public decimal? ptas_netadjustmentwithoutlocation { get; set; }
        public decimal? ptas_numberofunitsadjustment { get; set; }
        public decimal? ptas_percentcommercialadjustment { get; set; }
        public decimal? ptas_pooladjustment { get; set; }
        public int? ptas_proximitycode { get; set; }
        public decimal? ptas_proximitycodeadjustment { get; set; }
        public decimal? ptas_reconciledcomparablevalueperunit { get; set; }
        public decimal? ptas_reconciledcomparablevalueperunit_base { get; set; }
        public decimal? ptas_saledateadjustment { get; set; }
        public decimal? ptas_trendedsalepriceperunit { get; set; }
        public decimal? ptas_trendedsalepriceperunit_base { get; set; }
        public decimal? ptas_viewadjustment { get; set; }
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
        public Guid? _ptas_comparablesalesvaluationsubjectid_value { get; set; }
        public Guid? _ptas_saleid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_aptvaluation _ptas_comparablesalesvaluationsubjectid_valueNavigation { get; set; }
        public virtual ptas_aptavailablecomparablesale _ptas_saleid_valueNavigation { get; set; }
    }
}
