﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_condocomplex_snapshot
    {
        public Guid ptas_condocomplexid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public bool? ptas_addmanualadjustment { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public string ptas_addr1_line1 { get; set; }
        public bool? ptas_addrestaurantincome { get; set; }
        public bool? ptas_addretailincome { get; set; }
        public int? ptas_amenitypackage { get; set; }
        public bool? ptas_apartmentconversion { get; set; }
        public int? ptas_averagesize { get; set; }
        public decimal? ptas_avnrasqft { get; set; }
        public decimal? ptas_avnrasqft_base { get; set; }
        public int? ptas_bathroomtype { get; set; }
        public decimal? ptas_bathtosleepingroomratio { get; set; }
        public bool? ptas_bikestorage { get; set; }
        public string ptas_bikestoragedescription { get; set; }
        public int? ptas_buildingcondition { get; set; }
        public int? ptas_buildingquality { get; set; }
        public bool? ptas_calculateparkingincome { get; set; }
        public int? ptas_chargingstalls { get; set; }
        public bool? ptas_cleaningincluded { get; set; }
        public string ptas_complexdescription { get; set; }
        public int? ptas_constructionclass { get; set; }
        public string ptas_contactdescription { get; set; }
        public string ptas_contactname { get; set; }
        public int? ptas_coveredoutdoorstoragelinearft { get; set; }
        public bool? ptas_deck { get; set; }
        public string ptas_deckdescription { get; set; }
        public int? ptas_dnrland { get; set; }
        public int? ptas_effectiveyear { get; set; }
        public bool? ptas_elevators { get; set; }
        public string ptas_email { get; set; }
        public int? ptas_energycertification { get; set; }
        public string ptas_fax { get; set; }
        public bool? ptas_fireplace { get; set; }
        public string ptas_fullsitusaddress { get; set; }
        public bool? ptas_groundleases { get; set; }
        public bool? ptas_gym { get; set; }
        public string ptas_gymdescription { get; set; }
        public bool? ptas_internetincluded { get; set; }
        public int? ptas_kitchensinunit { get; set; }
        public decimal? ptas_kitchentosleepingroomratio { get; set; }
        public int? ptas_landperunit { get; set; }
        public int? ptas_landtype { get; set; }
        public int? ptas_laundry { get; set; }
        public decimal? ptas_lbratio { get; set; }
        public int? ptas_leasingclass { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public int? ptas_legacyrpcomplexid { get; set; }
        public bool? ptas_lowincomehousing { get; set; }
        public int? ptas_manualadjustmentvalue { get; set; }
        public int? ptas_marinaexistinguse { get; set; }
        public int? ptas_marinasubtype { get; set; }
        public int? ptas_maxnumberofcondounits { get; set; }
        public bool? ptas_mfte { get; set; }
        public DateTimeOffset? ptas_mfteenddate { get; set; }
        public DateTimeOffset? ptas_mftestartdate { get; set; }
        public int? ptas_mooragecovered { get; set; }
        public int? ptas_moorageopen { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_nraperroom { get; set; }
        public int? ptas_numberofanchors { get; set; }
        public int? ptas_numberofbathrooms { get; set; }
        public int? ptas_numberofbuildings { get; set; }
        public int? ptas_numberofcommonkitchens { get; set; }
        public int? ptas_numberofcondounitsremaining { get; set; }
        public int? ptas_numberofjunioranchors { get; set; }
        public int? ptas_numberofstorageunits { get; set; }
        public int? ptas_numberofstories { get; set; }
        public int? ptas_numberofunits { get; set; }
        public string ptas_othersecuritydescription { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public decimal? ptas_parkingoperatingexpensepct { get; set; }
        public decimal? ptas_parkingratio { get; set; }
        public int? ptas_percentanchors { get; set; }
        public int? ptas_percentcomplete { get; set; }
        public decimal? ptas_percentlandvaluedecimal { get; set; }
        public int? ptas_percentoffinishedbuildout { get; set; }
        public decimal? ptas_percentownershipdecimal { get; set; }
        public int? ptas_percentremediationcost { get; set; }
        public decimal? ptas_percenttotalvaluedecimal { get; set; }
        public int? ptas_percentwithview { get; set; }
        public string ptas_phone1 { get; set; }
        public bool? ptas_pool { get; set; }
        public int? ptas_projectappeal { get; set; }
        public int? ptas_projectlocation { get; set; }
        public string ptas_projectnotes { get; set; }
        public int? ptas_projectsubtype { get; set; }
        public int? ptas_projecttype { get; set; }
        public int? ptas_projectunittype { get; set; }
        public int? ptas_proximitytolightrail { get; set; }
        public int? ptas_proximitytouw { get; set; }
        public bool? ptas_railspuraccess { get; set; }
        public int? ptas_rentalmethod { get; set; }
        public bool? ptas_securitysystem { get; set; }
        public int? ptas_securitytype { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public bool? ptas_showsectionuses { get; set; }
        public bool? ptas_singletenancy { get; set; }
        public string ptas_situsaddress { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public bool? ptas_storage { get; set; }
        public string ptas_storagedescription { get; set; }
        public int? ptas_temperaturecontrol { get; set; }
        public bool? ptas_tenantpaidheat { get; set; }
        public decimal? ptas_totalassessedvalue { get; set; }
        public decimal? ptas_totalassessedvalue_base { get; set; }
        public int? ptas_totalgrosssquarefeet { get; set; }
        public int? ptas_totallinearft { get; set; }
        public int? ptas_totalnetsquarefeet { get; set; }
        public int? ptas_totalsize { get; set; }
        public bool? ptas_usevalueaddcaprate { get; set; }
        public bool? ptas_utilitiesincluded { get; set; }
        public decimal? ptas_valaddcaprate { get; set; }
        public string ptas_valueadddescription { get; set; }
        public string ptas_valueadjustmentdescription { get; set; }
        public int? ptas_valuedistributionmethod { get; set; }
        public int? ptas_valuedofcoveredsecuredstalls { get; set; }
        public int? ptas_valuedofcoveredunsecuredstalls { get; set; }
        public int? ptas_valuedofdailystalls { get; set; }
        public int? ptas_valuedofmonthlystalls { get; set; }
        public int? ptas_valuedofopensecured { get; set; }
        public int? ptas_valuedofopenunsecured { get; set; }
        public int? ptas_valuedoftotalstalls { get; set; }
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
        public Guid? _ptas_accessoryid_value { get; set; }
        public Guid? _ptas_addr1_cityid_value { get; set; }
        public Guid? _ptas_addr1_countryid_value { get; set; }
        public Guid? _ptas_addr1_stateid_value { get; set; }
        public Guid? _ptas_addr1_zipcode_value { get; set; }
        public Guid? _ptas_associatedparcel2id_value { get; set; }
        public Guid? _ptas_associatedparcel3id_value { get; set; }
        public Guid? _ptas_associatedparcelid_value { get; set; }
        public Guid? _ptas_contaminationproject_value { get; set; }
        public Guid? _ptas_economicunitid_value { get; set; }
        public Guid? _ptas_majorcondocomplexid_value { get; set; }
        public Guid? _ptas_masterprojectid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_parkingdistrictid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }
    }
}
