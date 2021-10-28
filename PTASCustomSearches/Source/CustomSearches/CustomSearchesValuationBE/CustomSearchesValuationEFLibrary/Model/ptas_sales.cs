using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_sales
    {
        public ptas_sales()
        {
            ptas_aptavailablecomparablesale = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_mediarepository = new HashSet<ptas_mediarepository>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_salepriceadjustment = new HashSet<ptas_salepriceadjustment>();
            ptas_salesaggregate = new HashSet<ptas_salesaggregate>();
            ptas_salesnote = new HashSet<ptas_salesnote>();
            ptas_task = new HashSet<ptas_task>();
        }

        public Guid ptas_salesid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_addr1_compositeaddress { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public int? ptas_addr1_directionprefix { get; set; }
        public int? ptas_addr1_directionsuffix { get; set; }
        public string ptas_addr1_line2 { get; set; }
        public string ptas_addr1_streetnumber { get; set; }
        public string ptas_addr1_streetnumberfraction { get; set; }
        public decimal? ptas_adjustedsaleprice { get; set; }
        public decimal? ptas_adjustedsaleprice_base { get; set; }
        public int? ptas_affidavitpropertytype { get; set; }
        public int? ptas_agggrosssqft { get; set; }
        public decimal? ptas_agglandacres { get; set; }
        public int? ptas_agglandsft { get; set; }
        public int? ptas_aggnetsqft { get; set; }
        public int? ptas_aggnumberofunits { get; set; }
        public int? ptas_aggregatebuildingquality { get; set; }
        public int? ptas_aggregateconstructionclass { get; set; }
        public int? ptas_aggregateeffectiveyear { get; set; }
        public int? ptas_aggregatemaxfloors { get; set; }
        public int? ptas_aggregatenumberofbuildings { get; set; }
        public int? ptas_aggregateyearbuilt { get; set; }
        public bool? ptas_allparcelsmatch { get; set; }
        public decimal? ptas_appraiseradjustment { get; set; }
        public decimal? ptas_appraiseradjustment_base { get; set; }
        public string ptas_appraiseradjustmentdescription { get; set; }
        public string ptas_associatedmajornumbers { get; set; }
        public string ptas_buyeraddress { get; set; }
        public string ptas_buyercitystatezip { get; set; }
        public string ptas_confidentialnotes { get; set; }
        public bool? ptas_currentuse { get; set; }
        public DateTimeOffset? ptas_datelettersenttobuyer { get; set; }
        public DateTimeOffset? ptas_datelettersenttoseller { get; set; }
        public bool? ptas_developmentrights { get; set; }
        public int? ptas_documenttype { get; set; }
        public string ptas_documenttypestr { get; set; }
        public string ptas_exemptionremark { get; set; }
        public bool? ptas_exemptstatus { get; set; }
        public bool? ptas_forest { get; set; }
        public bool? ptas_frozenseniorcitizenexemption { get; set; }
        public string ptas_fullsitusaddress { get; set; }
        public string ptas_granteelastname { get; set; }
        public string ptas_grantorlastname { get; set; }
        public bool? ptas_historic { get; set; }
        public DateTimeOffset? ptas_identifiedbydate { get; set; }
        public int? ptas_instrument { get; set; }
        public string ptas_integrationsource { get; set; }
        public int? ptas_isatmarket { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public string ptas_legaldescription { get; set; }
        public int? ptas_levelofverification { get; set; }
        public string ptas_linktorecordingdocument { get; set; }
        public string ptas_linktoreeta { get; set; }
        public string ptas_migrationnote { get; set; }
        public bool? ptas_multiparcelsale { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_nbrparcels { get; set; }
        public bool? ptas_nonprofit { get; set; }
        public bool? ptas_nonrepresentativesale { get; set; }
        public bool? ptas_operatingproperty { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public bool? ptas_parcelseg { get; set; }
        public bool? ptas_partialsaleflag { get; set; }
        public int? ptas_reason { get; set; }
        public string ptas_recordingnumber { get; set; }
        public DateTimeOffset? ptas_reetachangedate { get; set; }
        public bool? ptas_salecomplete { get; set; }
        public DateTimeOffset? ptas_saledate { get; set; }
        public decimal? ptas_saleprice { get; set; }
        public decimal? ptas_saleprice_base { get; set; }
        public int? ptas_salepropertyclass { get; set; }
        public int? ptas_salesidreviewstatus { get; set; }
        public int? ptas_salesprincipleuse { get; set; }
        public int? ptas_saletype { get; set; }
        public string ptas_selleraddress { get; set; }
        public string ptas_sellercitystatezip { get; set; }
        public decimal? ptas_sqftlotgra { get; set; }
        public decimal? ptas_sqftlotnra { get; set; }
        public decimal? ptas_sqftlotunit { get; set; }
        public bool? ptas_syncnameaddress { get; set; }
        public decimal? ptas_taxablesellingprice { get; set; }
        public decimal? ptas_taxablesellingprice_base { get; set; }
        public string ptas_taxfirstname { get; set; }
        public string ptas_taxlastname { get; set; }
        public bool? ptas_undividedinterest { get; set; }
        public bool? ptas_vacantland { get; set; }
        public DateTimeOffset? ptas_verifiedbydate { get; set; }
        public decimal? ptas_vspgra { get; set; }
        public decimal? ptas_vspgra_base { get; set; }
        public decimal? ptas_vspnra { get; set; }
        public decimal? ptas_vspnra_base { get; set; }
        public decimal? ptas_vspsqftlot { get; set; }
        public decimal? ptas_vspsqftlot_base { get; set; }
        public decimal? ptas_vspunit { get; set; }
        public decimal? ptas_vspunit_base { get; set; }
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
        public Guid? _ptas_identifiedbyid_value { get; set; }
        public Guid? _ptas_nonrepresentativesale1id_value { get; set; }
        public Guid? _ptas_nonrepresentativesale2id_value { get; set; }
        public Guid? _ptas_primarybuildingid_value { get; set; }
        public Guid? _ptas_primaryparcelid_value { get; set; }
        public Guid? _ptas_taxaccountid_value { get; set; }
        public Guid? _ptas_unitid_value { get; set; }
        public Guid? _ptas_verifiedbyid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_salepriceadjustment> ptas_salepriceadjustment { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate { get; set; }
        public virtual ICollection<ptas_salesnote> ptas_salesnote { get; set; }
        public virtual ICollection<ptas_task> ptas_task { get; set; }
    }
}
