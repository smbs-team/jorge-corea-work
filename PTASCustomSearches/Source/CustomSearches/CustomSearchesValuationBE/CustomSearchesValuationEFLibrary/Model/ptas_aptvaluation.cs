using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptvaluation
    {
        public ptas_aptvaluation()
        {
            ptas_aptcommercialincomeexpense = new HashSet<ptas_aptcommercialincomeexpense>();
            ptas_aptcomparablerent = new HashSet<ptas_aptcomparablerent>();
            ptas_aptcomparablesale = new HashSet<ptas_aptcomparablesale>();
        }

        public Guid ptas_aptvaluationid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public int? ptas_airportnoise { get; set; }
        public decimal? ptas_annuallaundryincome { get; set; }
        public decimal? ptas_annuallaundryincome_base { get; set; }
        public decimal? ptas_annualmiscellaneousincome { get; set; }
        public decimal? ptas_annualmiscellaneousincome_base { get; set; }
        public decimal? ptas_annualmoorageincome { get; set; }
        public decimal? ptas_annualmoorageincome_base { get; set; }
        public decimal? ptas_annualparkingincome { get; set; }
        public decimal? ptas_annualparkingincome_base { get; set; }
        public decimal? ptas_apartmentexpense { get; set; }
        public decimal? ptas_apartmentexpense_base { get; set; }
        public decimal? ptas_apartmentgim { get; set; }
        public decimal? ptas_apartmentrentincomemonthly { get; set; }
        public decimal? ptas_apartmentrentincomemonthly_base { get; set; }
        public string ptas_appeals { get; set; }
        public DateTimeOffset? ptas_assessmentdate { get; set; }
        public bool? ptas_atmarket { get; set; }
        public int? ptas_averageunitsize { get; set; }
        public decimal? ptas_baselandvalue { get; set; }
        public decimal? ptas_baselandvalue_base { get; set; }
        public decimal? ptas_blendedcaprate { get; set; }
        public int? ptas_buildingnetsqft { get; set; }
        public decimal? ptas_commercialcaprate { get; set; }
        public decimal? ptas_commercialegi { get; set; }
        public decimal? ptas_commercialegi_base { get; set; }
        public decimal? ptas_commercialgim { get; set; }
        public decimal? ptas_commercialincomevalue { get; set; }
        public decimal? ptas_commercialincomevalue_base { get; set; }
        public int? ptas_commercialnetsqft { get; set; }
        public decimal? ptas_commercialnoi { get; set; }
        public decimal? ptas_commercialnoi_base { get; set; }
        public decimal? ptas_commercialoexfactor { get; set; }
        public decimal? ptas_commercialpgi { get; set; }
        public decimal? ptas_commercialpgi_base { get; set; }
        public decimal? ptas_commercialrentincomeannual { get; set; }
        public decimal? ptas_commercialrentincomeannual_base { get; set; }
        public decimal? ptas_commercialrentrate { get; set; }
        public decimal? ptas_commercialrentrate_base { get; set; }
        public decimal? ptas_commercialvcl { get; set; }
        public bool? ptas_commonlaundry { get; set; }
        public decimal? ptas_comparablesalesvalue { get; set; }
        public decimal? ptas_comparablesalesvalue_base { get; set; }
        public decimal? ptas_comparablesalesvalueminusvacantland { get; set; }
        public decimal? ptas_comparablesalesvalueminusvacantland_base { get; set; }
        public decimal? ptas_comparablesalesvalueperunit { get; set; }
        public decimal? ptas_comparablesalesvalueperunit_base { get; set; }
        public decimal? ptas_comparablesalesweight { get; set; }
        public int? ptas_condition { get; set; }
        public decimal? ptas_costvaluercnldplusland { get; set; }
        public decimal? ptas_costvaluercnldplusland_base { get; set; }
        public string ptas_dataproblems { get; set; }
        public string ptas_economicunitparcellist { get; set; }
        public int? ptas_economicunitvaluationdescription { get; set; }
        public int? ptas_economicunitvaluationmethod { get; set; }
        public decimal? ptas_effectivegrossincome { get; set; }
        public decimal? ptas_effectivegrossincome_base { get; set; }
        public int? ptas_effectiveyear { get; set; }
        public int? ptas_elevators { get; set; }
        public decimal? ptas_emvminusvacantland { get; set; }
        public decimal? ptas_emvminusvacantland_base { get; set; }
        public decimal? ptas_emvperunit { get; set; }
        public decimal? ptas_emvperunit_base { get; set; }
        public decimal? ptas_emvweight { get; set; }
        public decimal? ptas_estimatedmarketvalueemv { get; set; }
        public decimal? ptas_estimatedmarketvalueemv_base { get; set; }
        public string ptas_flag { get; set; }
        public decimal? ptas_gimblended { get; set; }
        public decimal? ptas_gimminusvacantland { get; set; }
        public decimal? ptas_gimminusvacantland_base { get; set; }
        public bool? ptas_governmentowned { get; set; }
        public decimal? ptas_grossincomemultiplervaluegim { get; set; }
        public decimal? ptas_grossincomemultiplervaluegim_base { get; set; }
        public bool? ptas_holdout { get; set; }
        public decimal? ptas_incomevalue { get; set; }
        public decimal? ptas_incomevalue_base { get; set; }
        public decimal? ptas_incomevalueminusvacantland { get; set; }
        public decimal? ptas_incomevalueminusvacantland_base { get; set; }
        public decimal? ptas_incomevalueperunit { get; set; }
        public decimal? ptas_incomevalueperunit_base { get; set; }
        public decimal? ptas_incomevaluetopreviousvalue { get; set; }
        public decimal? ptas_incomeweight { get; set; }
        public decimal? ptas_manualvalue { get; set; }
        public decimal? ptas_manualvalue_base { get; set; }
        public int? ptas_mooragecovered { get; set; }
        public int? ptas_moorageopen { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_neighborhoodrank { get; set; }
        public decimal? ptas_netincome { get; set; }
        public decimal? ptas_netincome_base { get; set; }
        public int? ptas_numberofparcelsinsale { get; set; }
        public int? ptas_numberofunits { get; set; }
        public decimal? ptas_ommercialoex { get; set; }
        public decimal? ptas_ommercialoex_base { get; set; }
        public decimal? ptas_otherincome { get; set; }
        public decimal? ptas_otherincome_base { get; set; }
        public int? ptas_parkingcoveredsecured { get; set; }
        public int? ptas_parkingcoveredunsecured { get; set; }
        public int? ptas_parkingopensecured { get; set; }
        public int? ptas_parkingopenunsecured { get; set; }
        public decimal? ptas_percentcommercial { get; set; }
        public decimal? ptas_percenttax { get; set; }
        public int? ptas_percentwithview { get; set; }
        public string ptas_permits { get; set; }
        public bool? ptas_pool { get; set; }
        public decimal? ptas_potentialgrossincome { get; set; }
        public decimal? ptas_potentialgrossincome_base { get; set; }
        public int? ptas_previousselectedmethod { get; set; }
        public decimal? ptas_previoustotalvalue { get; set; }
        public decimal? ptas_previoustotalvalue_base { get; set; }
        public string ptas_propertyname { get; set; }
        public int? ptas_recommendedselectmethod { get; set; }
        public decimal? ptas_recommendedvalue { get; set; }
        public decimal? ptas_recommendedvalue_base { get; set; }
        public decimal? ptas_recommendedvaluetopreviousvalue { get; set; }
        public int? ptas_region { get; set; }
        public decimal? ptas_rent_roommarketrent { get; set; }
        public decimal? ptas_rent_roommarketrent_base { get; set; }
        public int? ptas_rent_studiocount { get; set; }
        public decimal? ptas_rents_1bedroom1bath_marketrent { get; set; }
        public decimal? ptas_rents_1bedroom1bath_marketrent_base { get; set; }
        public int? ptas_rents_1bedroom1bathavgsqft { get; set; }
        public decimal? ptas_rents_1bedroom1bathcomprent { get; set; }
        public decimal? ptas_rents_1bedroom1bathcomprent_base { get; set; }
        public int? ptas_rents_1bedroom1bathcount { get; set; }
        public int? ptas_rents_2bedroom1bathavgsqft { get; set; }
        public decimal? ptas_rents_2bedroom1bathcomprent { get; set; }
        public decimal? ptas_rents_2bedroom1bathcomprent_base { get; set; }
        public int? ptas_rents_2bedroom1bathcount { get; set; }
        public decimal? ptas_rents_2bedroom1bathmarketrent { get; set; }
        public decimal? ptas_rents_2bedroom1bathmarketrent_base { get; set; }
        public int? ptas_rents_2bedroom2bathavgsqft { get; set; }
        public decimal? ptas_rents_2bedroom2bathcomprent { get; set; }
        public decimal? ptas_rents_2bedroom2bathcomprent_base { get; set; }
        public int? ptas_rents_2bedroom2bathcount { get; set; }
        public decimal? ptas_rents_2bedroom2bathmarketrent { get; set; }
        public decimal? ptas_rents_2bedroom2bathmarketrent_base { get; set; }
        public int? ptas_rents_3bedroom1bathavgsqft { get; set; }
        public decimal? ptas_rents_3bedroom1bathcomprent { get; set; }
        public decimal? ptas_rents_3bedroom1bathcomprent_base { get; set; }
        public int? ptas_rents_3bedroom1bathcount { get; set; }
        public decimal? ptas_rents_3bedroom1bathmarketrent { get; set; }
        public decimal? ptas_rents_3bedroom1bathmarketrent_base { get; set; }
        public int? ptas_rents_3bedroom2bathavgsqft { get; set; }
        public decimal? ptas_rents_3bedroom2bathcomprent { get; set; }
        public decimal? ptas_rents_3bedroom2bathcomprent_base { get; set; }
        public int? ptas_rents_3bedroom2bathcount { get; set; }
        public decimal? ptas_rents_3bedroom2bathmarketrent { get; set; }
        public decimal? ptas_rents_3bedroom2bathmarketrent_base { get; set; }
        public int? ptas_rents_3bedroom3bathavgsqft { get; set; }
        public decimal? ptas_rents_3bedroom3bathcomprent { get; set; }
        public decimal? ptas_rents_3bedroom3bathcomprent_base { get; set; }
        public int? ptas_rents_3bedroom3bathcount { get; set; }
        public decimal? ptas_rents_3bedroom3bathmarketrent { get; set; }
        public decimal? ptas_rents_3bedroom3bathmarketrent_base { get; set; }
        public int? ptas_rents_4bedroomavgsqft { get; set; }
        public decimal? ptas_rents_4bedroomcomprent { get; set; }
        public decimal? ptas_rents_4bedroomcomprent_base { get; set; }
        public int? ptas_rents_4bedroomcount { get; set; }
        public decimal? ptas_rents_4bedroommarketrent { get; set; }
        public decimal? ptas_rents_4bedroommarketrent_base { get; set; }
        public int? ptas_rents_5bedroomplusavgsqft { get; set; }
        public decimal? ptas_rents_5bedroompluscomprent { get; set; }
        public decimal? ptas_rents_5bedroompluscomprent_base { get; set; }
        public int? ptas_rents_5bedroompluscount { get; set; }
        public decimal? ptas_rents_5bedroomplusmarketrent { get; set; }
        public decimal? ptas_rents_5bedroomplusmarketrent_base { get; set; }
        public decimal? ptas_rents_marketrent { get; set; }
        public decimal? ptas_rents_marketrent_base { get; set; }
        public decimal? ptas_rents_open1bedroom_marketrent { get; set; }
        public decimal? ptas_rents_open1bedroom_marketrent_base { get; set; }
        public int? ptas_rents_open1bedroomavgsqft { get; set; }
        public decimal? ptas_rents_open1bedroomcomprent { get; set; }
        public decimal? ptas_rents_open1bedroomcomprent_base { get; set; }
        public int? ptas_rents_open1bedroomcount { get; set; }
        public int? ptas_rents_roomavgsqft { get; set; }
        public decimal? ptas_rents_roomcomprent { get; set; }
        public decimal? ptas_rents_roomcomprent_base { get; set; }
        public int? ptas_rents_roomcount { get; set; }
        public int? ptas_rents_studioavgsqft { get; set; }
        public decimal? ptas_rents_studiocomprent { get; set; }
        public decimal? ptas_rents_studiocomprent_base { get; set; }
        public decimal? ptas_rents_studiomarketrent { get; set; }
        public decimal? ptas_rents_studiomarketrent_base { get; set; }
        public DateTimeOffset? ptas_saleddate { get; set; }
        public decimal? ptas_saleprice { get; set; }
        public decimal? ptas_saleprice_base { get; set; }
        public bool? ptas_tenantpaidheat { get; set; }
        public decimal? ptas_totalapartmentmarketrent { get; set; }
        public decimal? ptas_totalapartmentmarketrent_base { get; set; }
        public decimal? ptas_totalexpenses { get; set; }
        public decimal? ptas_totalexpenses_base { get; set; }
        public decimal? ptas_totalexpensespercent { get; set; }
        public decimal? ptas_trendedprice { get; set; }
        public decimal? ptas_trendedprice_base { get; set; }
        public decimal? ptas_trendedpriceperunit { get; set; }
        public decimal? ptas_trendedpriceperunit_base { get; set; }
        public decimal? ptas_unitsizefactor { get; set; }
        public decimal? ptas_vacancyandcreditloss { get; set; }
        public int? ptas_valueestimatesupdated { get; set; }
        public DateTimeOffset? ptas_valueestimateupdatedate { get; set; }
        public decimal? ptas_viewrank { get; set; }
        public decimal? ptas_weightedvalue { get; set; }
        public decimal? ptas_weightedvalue_base { get; set; }
        public decimal? ptas_weightedvalueminusvacantland { get; set; }
        public decimal? ptas_weightedvalueminusvacantland_base { get; set; }
        public decimal? ptas_weightedvalueperunit { get; set; }
        public decimal? ptas_weightedvalueperunit_base { get; set; }
        public decimal? ptas_weightedvaluetopreviousvalue { get; set; }
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
        public Guid? _ptas_appraiserid_value { get; set; }
        public Guid? _ptas_aptneighborhoodid_value { get; set; }
        public Guid? _ptas_aptvaluationprojectid_value { get; set; }
        public Guid? _ptas_economicunitid_value { get; set; }
        public Guid? _ptas_geoareaid_value { get; set; }
        public Guid? _ptas_geoneighborhoodid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_projectid_value { get; set; }
        public Guid? _ptas_propertytypeid_value { get; set; }
        public Guid? _ptas_responsibilityapplgroup_value { get; set; }
        public Guid? _ptas_specialtyarea_value { get; set; }
        public Guid? _ptas_supergroup_value { get; set; }
        public Guid? _ptas_updatedbyid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual systemuser _ptas_appraiserid_valueNavigation { get; set; }
        public virtual ptas_aptneighborhood _ptas_aptneighborhoodid_valueNavigation { get; set; }
        public virtual ptas_aptvaluationproject _ptas_aptvaluationprojectid_valueNavigation { get; set; }
        public virtual ptas_economicunit _ptas_economicunitid_valueNavigation { get; set; }
        public virtual ptas_geoarea _ptas_geoareaid_valueNavigation { get; set; }
        public virtual ptas_geoneighborhood _ptas_geoneighborhoodid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual ptas_condocomplex _ptas_projectid_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_propertytypeid_valueNavigation { get; set; }
        public virtual ptas_responsibility _ptas_responsibilityapplgroup_valueNavigation { get; set; }
        public virtual ptas_specialtyarea _ptas_specialtyarea_valueNavigation { get; set; }
        public virtual ptas_supergroup _ptas_supergroup_valueNavigation { get; set; }
        public virtual systemuser _ptas_updatedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale { get; set; }
    }
}
