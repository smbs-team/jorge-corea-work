﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_condounit
    {
        public ptas_condounit()
        {
            Inverse_ptas_masterunitid_valueNavigation = new HashSet<ptas_condounit>();
            ptas_buildingdetail_commercialuse = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_inspectionhistory = new HashSet<ptas_inspectionhistory>();
            ptas_sketch = new HashSet<ptas_sketch>();
            ptas_taxaccount = new HashSet<ptas_taxaccount>();
            ptas_unitbreakdown = new HashSet<ptas_unitbreakdown>();
        }

        public Guid ptas_condounitid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_accessoryflatvalue { get; set; }
        public decimal? ptas_accessoryflatvalue_base { get; set; }
        public int? ptas_accountstatus { get; set; }
        public int? ptas_accounttype { get; set; }
        public string ptas_addr1_compositeaddress { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public int? ptas_addr1_directionprefix { get; set; }
        public int? ptas_addr1_directionsuffix { get; set; }
        public string ptas_addr1_line2 { get; set; }
        public string ptas_addr1_streetnumber { get; set; }
        public string ptas_addr1_streetnumberfraction { get; set; }
        public int? ptas_addressusage { get; set; }
        public int? ptas_attic { get; set; }
        public int? ptas_buildingnbr { get; set; }
        public string ptas_buildingnumber { get; set; }
        public int? ptas_carportsqft { get; set; }
        public bool? ptas_condo { get; set; }
        public int? ptas_condounitcondition { get; set; }
        public int? ptas_decksqft { get; set; }
        public string ptas_description { get; set; }
        public int? ptas_effectiveyear { get; set; }
        public int? ptas_endporchsqft { get; set; }
        public bool? ptas_endunit { get; set; }
        public int? ptas_energyrating { get; set; }
        public int? ptas_finishedbasement { get; set; }
        public bool? ptas_fireplace { get; set; }
        public int? ptas_firstfloor { get; set; }
        public int? ptas_floatinghomecondition { get; set; }
        public int? ptas_floatinghomefinishedbasementgrade { get; set; }
        public int? ptas_floatinghomegrade { get; set; }
        public int? ptas_floatinghomeownershiptype { get; set; }
        public int? ptas_floatinghometype { get; set; }
        public string ptas_floornumber { get; set; }
        public int? ptas_flotationtype { get; set; }
        public string ptas_fullsitusaddress { get; set; }
        public int? ptas_heatingsystem { get; set; }
        public decimal? ptas_improvementsvalue { get; set; }
        public decimal? ptas_improvementsvalue_base { get; set; }
        public int? ptas_inspectionreason { get; set; }
        public decimal? ptas_landvalue { get; set; }
        public decimal? ptas_landvalue_base { get; set; }
        public int? ptas_leasingclass { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public int? ptas_legacyunitid { get; set; }
        public int? ptas_length { get; set; }
        public string ptas_licensenumber { get; set; }
        public int? ptas_linearft { get; set; }
        public string ptas_mailingaddresstaxaccount { get; set; }
        public string ptas_makeandmodel { get; set; }
        public string ptas_minornumber { get; set; }
        public int? ptas_mobilehomeclass { get; set; }
        public int? ptas_mobilehomecondition { get; set; }
        public int? ptas_mobilehomesize { get; set; }
        public int? ptas_mobilehometype { get; set; }
        public int? ptas_mooragetype { get; set; }
        public string ptas_name { get; set; }
        public bool? ptas_needtorevisit { get; set; }
        public decimal? ptas_newconstructionvalue { get; set; }
        public decimal? ptas_newconstructionvalue_base { get; set; }
        public int? ptas_numberof12baths { get; set; }
        public int? ptas_numberof34baths { get; set; }
        public int? ptas_numberofbasementparkingspaces { get; set; }
        public int? ptas_numberofbasementtandemspaces { get; set; }
        public int? ptas_numberofbedrooms { get; set; }
        public int? ptas_numberofcarportspaces { get; set; }
        public int? ptas_numberoffullbaths { get; set; }
        public int? ptas_numberofgarageparkingspaces { get; set; }
        public int? ptas_numberofgaragetandemspaces { get; set; }
        public int? ptas_numberofhydraulicparkingspaces { get; set; }
        public int? ptas_numberofopenparkingspaces { get; set; }
        public int? ptas_numberofstories { get; set; }
        public int? ptas_openporchsqft { get; set; }
        public string ptas_otherparking { get; set; }
        public int? ptas_otherrooms { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public int? ptas_percentcomplete { get; set; }
        public decimal? ptas_percentlandvaluedecimal { get; set; }
        public int? ptas_percentnetcondition { get; set; }
        public decimal? ptas_percentownershipdecimal { get; set; }
        public decimal? ptas_percenttotalvaluedecimal { get; set; }
        public int? ptas_regressionexclusionreason { get; set; }
        public int? ptas_roomadditionalsqft { get; set; }
        public string ptas_seattleid { get; set; }
        public int? ptas_secondfloor { get; set; }
        public DateTimeOffset? ptas_selectdate { get; set; }
        public int? ptas_selectmethod { get; set; }
        public int? ptas_selectreason { get; set; }
        public string ptas_serialnumbervin { get; set; }
        public bool? ptas_showinspectionhistory { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public string ptas_situsaddress { get; set; }
        public int? ptas_size { get; set; }
        public int? ptas_skirtlinearft { get; set; }
        public int? ptas_skirttype { get; set; }
        public int? ptas_sliplocation { get; set; }
        public decimal? ptas_smallhomeadjustment { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public bool? ptas_sprinklers { get; set; }
        public int? ptas_storyheight { get; set; }
        public string ptas_taxaccountowner { get; set; }
        public int? ptas_tipoutarea { get; set; }
        public bool? ptas_topfloor { get; set; }
        public int? ptas_totalbasement { get; set; }
        public int? ptas_totalliving { get; set; }
        public decimal? ptas_totalvalue { get; set; }
        public decimal? ptas_totalvalue_base { get; set; }
        public bool? ptas_unitinspected { get; set; }
        public DateTimeOffset? ptas_unitinspecteddate { get; set; }
        public int? ptas_unitlocation { get; set; }
        public string ptas_unitnumbertext { get; set; }
        public int? ptas_unitofmeasure { get; set; }
        public int? ptas_unitquality { get; set; }
        public int? ptas_unitqualityos { get; set; }
        public int? ptas_unittype { get; set; }
        public int? ptas_viewcityorterritorial { get; set; }
        public int? ptas_viewlakeorriver { get; set; }
        public int? ptas_viewlakewashingtonorlakesammamish { get; set; }
        public int? ptas_viewmountain { get; set; }
        public int? ptas_viewpugetsound { get; set; }
        public int? ptas_width { get; set; }
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
        public Guid? _ptas_addr1_cityid_value { get; set; }
        public Guid? _ptas_addr1_countryid_value { get; set; }
        public Guid? _ptas_addr1_stateid_value { get; set; }
        public Guid? _ptas_addr1_streetnameid_value { get; set; }
        public Guid? _ptas_addr1_streettypeid_value { get; set; }
        public Guid? _ptas_addr1_zipcodeid_value { get; set; }
        public Guid? _ptas_buildingid_value { get; set; }
        public Guid? _ptas_complexid_value { get; set; }
        public Guid? _ptas_dock_value { get; set; }
        public Guid? _ptas_masterunitid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_propertytypeid_value { get; set; }
        public Guid? _ptas_responsibilityid_value { get; set; }
        public Guid? _ptas_selectbyid_value { get; set; }
        public Guid? _ptas_sketchid_value { get; set; }
        public Guid? _ptas_specialtyareaid_value { get; set; }
        public Guid? _ptas_specialtynbhdid_value { get; set; }
        public Guid? _ptas_taxaccountid_value { get; set; }
        public Guid? _ptas_unitinspectedbyid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_city _ptas_addr1_cityid_valueNavigation { get; set; }
        public virtual ptas_country _ptas_addr1_countryid_valueNavigation { get; set; }
        public virtual ptas_stateorprovince _ptas_addr1_stateid_valueNavigation { get; set; }
        public virtual ptas_streetname _ptas_addr1_streetnameid_valueNavigation { get; set; }
        public virtual ptas_streettype _ptas_addr1_streettypeid_valueNavigation { get; set; }
        public virtual ptas_zipcode _ptas_addr1_zipcodeid_valueNavigation { get; set; }
        public virtual ptas_buildingdetail _ptas_buildingid_valueNavigation { get; set; }
        public virtual ptas_condocomplex _ptas_complexid_valueNavigation { get; set; }
        public virtual ptas_projectdock _ptas_dock_valueNavigation { get; set; }
        public virtual ptas_condounit _ptas_masterunitid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_propertytypeid_valueNavigation { get; set; }
        public virtual ptas_responsibility _ptas_responsibilityid_valueNavigation { get; set; }
        public virtual systemuser _ptas_selectbyid_valueNavigation { get; set; }
        public virtual ptas_sketch _ptas_sketchid_valueNavigation { get; set; }
        public virtual ptas_specialtyarea _ptas_specialtyareaid_valueNavigation { get; set; }
        public virtual ptas_specialtyneighborhood _ptas_specialtynbhdid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_taxaccountid_valueNavigation { get; set; }
        public virtual systemuser _ptas_unitinspectedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> Inverse_ptas_masterunitid_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown { get; set; }
    }
}
