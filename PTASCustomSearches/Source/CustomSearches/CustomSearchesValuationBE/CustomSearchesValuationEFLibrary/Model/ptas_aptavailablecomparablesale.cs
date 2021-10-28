using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptavailablecomparablesale
    {
        public ptas_aptavailablecomparablesale()
        {
            ptas_aptcomparablesale = new HashSet<ptas_aptcomparablesale>();
        }

        public Guid ptas_aptavailablecomparablesaleid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_airportnoiseadjustment { get; set; }
        public int? ptas_assessmentyear { get; set; }
        public int? ptas_averageunitsize { get; set; }
        public int? ptas_buildingquality { get; set; }
        public decimal? ptas_caprate { get; set; }
        public int? ptas_condition { get; set; }
        public bool? ptas_elevators { get; set; }
        public string ptas_excisetaxnumber { get; set; }
        public decimal? ptas_gim { get; set; }
        public int? ptas_grosssqft { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_netsqft { get; set; }
        public int? ptas_numberofunits { get; set; }
        public int? ptas_percentcommercial { get; set; }
        public int? ptas_percentwithview { get; set; }
        public bool? ptas_pool { get; set; }
        public string ptas_propertyaddress { get; set; }
        public string ptas_propertyname { get; set; }
        public DateTimeOffset? ptas_saledate { get; set; }
        public decimal? ptas_saleprice { get; set; }
        public decimal? ptas_saleprice_base { get; set; }
        public decimal? ptas_salepriceperunit { get; set; }
        public decimal? ptas_salepriceperunit_base { get; set; }
        public DateTimeOffset? ptas_trenddate { get; set; }
        public decimal? ptas_trendedsaleprice { get; set; }
        public decimal? ptas_trendedsaleprice_base { get; set; }
        public decimal? ptas_trendedsalepriceperunit { get; set; }
        public decimal? ptas_trendedsalepriceperunit_base { get; set; }
        public decimal? ptas_viewrank { get; set; }
        public int? ptas_yearbuilt { get; set; }
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
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_propertytypeid_value { get; set; }
        public Guid? _ptas_responsibilityid_value { get; set; }
        public Guid? _ptas_saleid_value { get; set; }
        public Guid? _ptas_specialtyareaid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_propertytypeid_valueNavigation { get; set; }
        public virtual ptas_responsibility _ptas_responsibilityid_valueNavigation { get; set; }
        public virtual ptas_sales _ptas_saleid_valueNavigation { get; set; }
        public virtual ptas_specialtyarea _ptas_specialtyareaid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale { get; set; }
    }
}
