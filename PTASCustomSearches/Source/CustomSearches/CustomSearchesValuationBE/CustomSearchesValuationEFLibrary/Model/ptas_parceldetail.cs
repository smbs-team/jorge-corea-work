using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_parceldetail
    {
        public ptas_parceldetail()
        {
            Inverse_ptas_masterparcelid_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_accessorydetail = new HashSet<ptas_accessorydetail>();
            ptas_addresschangehistory = new HashSet<ptas_addresschangehistory>();
            ptas_annexationparcelreview = new HashSet<ptas_annexationparcelreview>();
            ptas_aptavailablecomparablesale = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_aptlistedrent = new HashSet<ptas_aptlistedrent>();
            ptas_aptvaluation = new HashSet<ptas_aptvaluation>();
            ptas_bookmark = new HashSet<ptas_bookmark>();
            ptas_buildingdetail = new HashSet<ptas_buildingdetail>();
            ptas_condocomplex_ptas_associatedparcel2id_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condocomplex_ptas_associatedparcel3id_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condocomplex_ptas_associatedparcelid_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condocomplex_ptas_parcelid_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condounit = new HashSet<ptas_condounit>();
            ptas_contaminatedlandreduction = new HashSet<ptas_contaminatedlandreduction>();
            ptas_fileattachmentmetadata = new HashSet<ptas_fileattachmentmetadata>();
            ptas_homeimprovement = new HashSet<ptas_homeimprovement>();
            ptas_inspectionhistory = new HashSet<ptas_inspectionhistory>();
            ptas_landvaluebreakdown = new HashSet<ptas_landvaluebreakdown>();
            ptas_parceleconomicunit = new HashSet<ptas_parceleconomicunit>();
            ptas_permit_ptas_condounitid_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_ptas_parcelid_valueNavigation = new HashSet<ptas_permit>();
            ptas_permitinspectionhistory = new HashSet<ptas_permitinspectionhistory>();
            ptas_recentparcel = new HashSet<ptas_recentparcel>();
            ptas_sketch = new HashSet<ptas_sketch>();
            ptas_task = new HashSet<ptas_task>();
            ptas_taxaccount = new HashSet<ptas_taxaccount>();
        }

        public Guid ptas_parceldetailid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public int? ptas_accessorycount { get; set; }
        public int? ptas_accounttype { get; set; }
        public string ptas_acctnbr { get; set; }
        public string ptas_addr1_compositeaddress { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public int? ptas_addr1_directionprefix { get; set; }
        public int? ptas_addr1_directionsuffix { get; set; }
        public string ptas_addr1_line2 { get; set; }
        public string ptas_addr1_streetnumber { get; set; }
        public string ptas_addr1_streetnumberfraction { get; set; }
        public string ptas_address { get; set; }
        public string ptas_alternatekey { get; set; }
        public string ptas_applgroup { get; set; }
        public decimal? ptas_benefitacres { get; set; }
        public int? ptas_bldgnbr { get; set; }
        public bool? ptas_bothinspected { get; set; }
        public int? ptas_buildingcount { get; set; }
        public string ptas_changesource { get; set; }
        public int? ptas_commarea { get; set; }
        public int? ptas_commercialdistrict { get; set; }
        public int? ptas_commsubarea { get; set; }
        public int? ptas_condocommgridupdated { get; set; }
        public int? ptas_condocount { get; set; }
        public int? ptas_condoresgridupdated { get; set; }
        public int? ptas_currentuse { get; set; }
        public decimal? ptas_delinquenttaxesowed { get; set; }
        public decimal? ptas_delinquenttaxesowed_base { get; set; }
        public string ptas_directnavigation { get; set; }
        public string ptas_dirsuffix { get; set; }
        public string ptas_district { get; set; }
        public int? ptas_floatgridupdated { get; set; }
        public int? ptas_floatmobilecount { get; set; }
        public string ptas_folio { get; set; }
        public decimal? ptas_forestfireacres { get; set; }
        public int? ptas_formhasloaded { get; set; }
        public int? ptas_geoarea { get; set; }
        public int? ptas_geoneighborhood { get; set; }
        public string ptas_histguid { get; set; }
        public int? ptas_historicsite { get; set; }
        public int? ptas_histyear { get; set; }
        public int? ptas_holdoutreason { get; set; }
        public int? ptas_inspectionreason { get; set; }
        public bool? ptas_isgovernmentowned { get; set; }
        public bool? ptas_islipa { get; set; }
        public bool? ptas_ismfte { get; set; }
        public bool? ptas_istribalowned { get; set; }
        public string ptas_landalternatekey { get; set; }
        public bool? ptas_landinspected { get; set; }
        public DateTimeOffset? ptas_landinspecteddate { get; set; }
        public int? ptas_landtype { get; set; }
        public string ptas_landusecode { get; set; }
        public string ptas_landusedesc { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public string ptas_legaldescription { get; set; }
        public string ptas_levycode { get; set; }
        public string ptas_linktorecordersoffice { get; set; }
        public decimal? ptas_lotacreage { get; set; }
        public string ptas_major { get; set; }
        public bool? ptas_markfordelete { get; set; }
        public string ptas_mediaguid { get; set; }
        public int? ptas_mediatype { get; set; }
        public string ptas_migrationnote { get; set; }
        public string ptas_minor { get; set; }
        public int? ptas_mobilegridupdated { get; set; }
        public string ptas_name { get; set; }
        public string ptas_namesonaccount { get; set; }
        public string ptas_nbrfraction { get; set; }
        public int? ptas_nbrlivingunits { get; set; }
        public bool? ptas_needtorevisit { get; set; }
        public int? ptas_neighborhood { get; set; }
        public string ptas_newconstrval { get; set; }
        public int? ptas_notecount { get; set; }
        public int? ptas_numberofbuildings { get; set; }
        public string ptas_otherexemptions { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public bool? ptas_parcelinspected { get; set; }
        public DateTimeOffset? ptas_parcelinspecteddate { get; set; }
        public string ptas_parcelsnapshotnote { get; set; }
        public int? ptas_parceltype { get; set; }
        public int? ptas_permitcount { get; set; }
        public string ptas_platblock { get; set; }
        public string ptas_platlot { get; set; }
        public int? ptas_projectcount { get; set; }
        public string ptas_propertyname { get; set; }
        public string ptas_proptype { get; set; }
        public int? ptas_region { get; set; }
        public int? ptas_resarea { get; set; }
        public int? ptas_residentialdistrict { get; set; }
        public int? ptas_ressubarea { get; set; }
        public string ptas_rpaalternatekey { get; set; }
        public int? ptas_salecount { get; set; }
        public int? ptas_salesnotecount { get; set; }
        public bool? ptas_seesenior { get; set; }
        public bool? ptas_showbookmarks { get; set; }
        public bool? ptas_showcommercialfields { get; set; }
        public bool? ptas_showcommercialunits { get; set; }
        public bool? ptas_showexemptions { get; set; }
        public bool? ptas_showfloatunits { get; set; }
        public bool? ptas_showinspectionhistory { get; set; }
        public bool? ptas_showmobileunits { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public bool? ptas_showresidentialfields { get; set; }
        public bool? ptas_showresidentialunits { get; set; }
        public bool? ptas_showtasks { get; set; }
        public int? ptas_snapshotassessmentyear { get; set; }
        public int? ptas_snapshotcount { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public string ptas_snapshoterror { get; set; }
        public int? ptas_snapshotprogress { get; set; }
        public int? ptas_snapshottype { get; set; }
        public string ptas_spacenumber { get; set; }
        public int? ptas_specialtyarea { get; set; }
        public int? ptas_specialtyneighborhood { get; set; }
        public string ptas_splitcode { get; set; }
        public int? ptas_sqftlot { get; set; }
        public string ptas_streetname { get; set; }
        public string ptas_streetnbr { get; set; }
        public string ptas_streettype { get; set; }
        public int? ptas_supergroup { get; set; }
        public int? ptas_taxstatus { get; set; }
        public decimal? ptas_totalaccessoryvalue { get; set; }
        public decimal? ptas_totalaccessoryvalue_base { get; set; }
        public bool? ptas_vacantland { get; set; }
        public DateTimeOffset? ptas_whatifsyncdate { get; set; }
        public string ptas_zipcode { get; set; }
        public string ptas_zoning { get; set; }
        public string ptas_zoningdescription { get; set; }
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
        public Guid? _ptas_abstractparcelresultid_value { get; set; }
        public Guid? _ptas_addr1_cityid_value { get; set; }
        public Guid? _ptas_addr1_countryid_value { get; set; }
        public Guid? _ptas_addr1_stateid_value { get; set; }
        public Guid? _ptas_addr1_streetnameid_value { get; set; }
        public Guid? _ptas_addr1_streettypeid_value { get; set; }
        public Guid? _ptas_addr1_zipcodeid_value { get; set; }
        public Guid? _ptas_areaid_value { get; set; }
        public Guid? _ptas_assignedappraiserid_value { get; set; }
        public Guid? _ptas_districtid_value { get; set; }
        public Guid? _ptas_economicunit_value { get; set; }
        public Guid? _ptas_geoareaid_value { get; set; }
        public Guid? _ptas_geonbhdid_value { get; set; }
        public Guid? _ptas_jurisdiction_value { get; set; }
        public Guid? _ptas_landid_value { get; set; }
        public Guid? _ptas_landinspectedbyid_value { get; set; }
        public Guid? _ptas_levycodeid_value { get; set; }
        public Guid? _ptas_masterparcelid_value { get; set; }
        public Guid? _ptas_neighborhoodid_value { get; set; }
        public Guid? _ptas_parcelinspectedbyid_value { get; set; }
        public Guid? _ptas_propertytypeid_value { get; set; }
        public Guid? _ptas_qstrid_value { get; set; }
        public Guid? _ptas_responsibilityid_value { get; set; }
        public Guid? _ptas_saleid_value { get; set; }
        public Guid? _ptas_specialtyappraiserid_value { get; set; }
        public Guid? _ptas_specialtyareaid_value { get; set; }
        public Guid? _ptas_specialtynbhdid_value { get; set; }
        public Guid? _ptas_splitaccount1id_value { get; set; }
        public Guid? _ptas_splitaccount2id_value { get; set; }
        public Guid? _ptas_subareaid_value { get; set; }
        public Guid? _ptas_submarketid_value { get; set; }
        public Guid? _ptas_supergroupdid_value { get; set; }
        public Guid? _ptas_taxaccountid_value { get; set; }
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
        public virtual ptas_area _ptas_areaid_valueNavigation { get; set; }
        public virtual systemuser _ptas_assignedappraiserid_valueNavigation { get; set; }
        public virtual ptas_district _ptas_districtid_valueNavigation { get; set; }
        public virtual ptas_economicunit _ptas_economicunit_valueNavigation { get; set; }
        public virtual ptas_geoarea _ptas_geoareaid_valueNavigation { get; set; }
        public virtual ptas_geoneighborhood _ptas_geonbhdid_valueNavigation { get; set; }
        public virtual ptas_jurisdiction _ptas_jurisdiction_valueNavigation { get; set; }
        public virtual ptas_land _ptas_landid_valueNavigation { get; set; }
        public virtual systemuser _ptas_landinspectedbyid_valueNavigation { get; set; }
        public virtual ptas_levycode _ptas_levycodeid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_masterparcelid_valueNavigation { get; set; }
        public virtual ptas_neighborhood _ptas_neighborhoodid_valueNavigation { get; set; }
        public virtual systemuser _ptas_parcelinspectedbyid_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_propertytypeid_valueNavigation { get; set; }
        public virtual ptas_qstr _ptas_qstrid_valueNavigation { get; set; }
        public virtual ptas_responsibility _ptas_responsibilityid_valueNavigation { get; set; }
        public virtual ptas_sales _ptas_saleid_valueNavigation { get; set; }
        public virtual systemuser _ptas_specialtyappraiserid_valueNavigation { get; set; }
        public virtual ptas_specialtyarea _ptas_specialtyareaid_valueNavigation { get; set; }
        public virtual ptas_specialtyneighborhood _ptas_specialtynbhdid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_splitaccount1id_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_splitaccount2id_valueNavigation { get; set; }
        public virtual ptas_subarea _ptas_subareaid_valueNavigation { get; set; }
        public virtual ptas_submarket _ptas_submarketid_valueNavigation { get; set; }
        public virtual ptas_supergroup _ptas_supergroupdid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_taxaccountid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> Inverse_ptas_masterparcelid_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail { get; set; }
        public virtual ICollection<ptas_addresschangehistory> ptas_addresschangehistory { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview { get; set; }
        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale { get; set; }
        public virtual ICollection<ptas_aptlistedrent> ptas_aptlistedrent { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_ptas_associatedparcel2id_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_ptas_associatedparcel3id_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_ptas_associatedparcelid_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_ptas_parcelid_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_ptas_condounitid_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_ptas_parcelid_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory { get; set; }
        public virtual ICollection<ptas_recentparcel> ptas_recentparcel { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch { get; set; }
        public virtual ICollection<ptas_task> ptas_task { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount { get; set; }
    }
}
