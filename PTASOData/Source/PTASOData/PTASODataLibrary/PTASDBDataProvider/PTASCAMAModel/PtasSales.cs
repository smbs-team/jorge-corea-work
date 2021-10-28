using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSales
    {
        public PtasSales()
        {
            PtasAptavailablecomparablesale = new HashSet<PtasAptavailablecomparablesale>();
            PtasMediarepository = new HashSet<PtasMediarepository>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasSalepriceadjustment = new HashSet<PtasSalepriceadjustment>();
            PtasSalesaccessory = new HashSet<PtasSalesaccessory>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
            PtasSalesbuilding = new HashSet<PtasSalesbuilding>();
            PtasSalesnote = new HashSet<PtasSalesnote>();
            PtasSalesparcel = new HashSet<PtasSalesparcel>();
            PtasSeapplicationtask = new HashSet<PtasSeapplicationtask>();
            PtasTask = new HashSet<PtasTask>();
        }

        public Guid PtasSalesid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAddr1Directionprefix { get; set; }
        public int? PtasAddr1Directionsuffix { get; set; }
        public string PtasAddr1Line2 { get; set; }
        public string PtasAddr1Streetnumber { get; set; }
        public string PtasAddr1Streetnumberfraction { get; set; }
        public decimal? PtasAdjustedsaleprice { get; set; }
        public decimal? PtasAdjustedsalepriceBase { get; set; }
        public int? PtasAffidavitpropertytype { get; set; }
        public int? PtasAgggrosssqft { get; set; }
        public decimal? PtasAgglandacres { get; set; }
        public int? PtasAgglandsft { get; set; }
        public int? PtasAggnetsqft { get; set; }
        public int? PtasAggnumberofunits { get; set; }
        public int? PtasAggregatebuildingquality { get; set; }
        public int? PtasAggregateconstructionclass { get; set; }
        public int? PtasAggregateeffectiveyear { get; set; }
        public int? PtasAggregatemaxfloors { get; set; }
        public int? PtasAggregatenumberofbuildings { get; set; }
        public int? PtasAggregateyearbuilt { get; set; }
        public bool? PtasAllparcelsmatch { get; set; }
        public decimal? PtasAppraiseradjustment { get; set; }
        public decimal? PtasAppraiseradjustmentBase { get; set; }
        public string PtasAppraiseradjustmentdescription { get; set; }
        public string PtasAssociatedmajornumbers { get; set; }
        public string PtasBuyeraddress { get; set; }
        public string PtasBuyercitystatezip { get; set; }
        public string PtasConfidentialnotes { get; set; }
        public bool? PtasCurrentuse { get; set; }
        public DateTimeOffset? PtasDatelettersenttobuyer { get; set; }
        public DateTimeOffset? PtasDatelettersenttoseller { get; set; }
        public bool? PtasDevelopmentrights { get; set; }
        public int? PtasDocumenttype { get; set; }
        public string PtasDocumenttypestr { get; set; }
        public string PtasExemptionremark { get; set; }
        public bool? PtasExemptstatus { get; set; }
        public bool? PtasForest { get; set; }
        public bool? PtasFrozenseniorcitizenexemption { get; set; }
        public string PtasFullsitusaddress { get; set; }
        public string PtasGranteelastname { get; set; }
        public string PtasGrantorlastname { get; set; }
        public bool? PtasHistoric { get; set; }
        public DateTimeOffset? PtasIdentifiedbydate { get; set; }
        public int? PtasInstrument { get; set; }
        public string PtasIntegrationsource { get; set; }
        public int? PtasIsatmarket { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasLegaldescription { get; set; }
        public int? PtasLevelofverification { get; set; }
        public string PtasLinktorecordingdocument { get; set; }
        public string PtasLinktoreeta { get; set; }
        public string PtasMigrationnote { get; set; }
        public bool? PtasMultiparcelsale { get; set; }
        public string PtasName { get; set; }
        public int? PtasNbrparcels { get; set; }
        public bool? PtasNonprofit { get; set; }
        public bool? PtasNonrepresentativesale { get; set; }
        public bool? PtasOperatingproperty { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public bool? PtasParcelseg { get; set; }
        public bool? PtasPartialsaleflag { get; set; }
        public int? PtasReason { get; set; }
        public string PtasRecordingnumber { get; set; }
        public DateTimeOffset? PtasReetachangedate { get; set; }
        public bool? PtasSalecomplete { get; set; }
        public DateTimeOffset? PtasSaledate { get; set; }
        public decimal? PtasSaleprice { get; set; }
        public decimal? PtasSalepriceBase { get; set; }
        public int? PtasSalepropertyclass { get; set; }
        public int? PtasSalesidreviewstatus { get; set; }
        public int? PtasSalesprincipleuse { get; set; }
        public int? PtasSaletype { get; set; }
        public string PtasSelleraddress { get; set; }
        public string PtasSellercitystatezip { get; set; }
        public bool? PtasSetprimarybldgparcel { get; set; }
        public decimal? PtasSqftlotgra { get; set; }
        public decimal? PtasSqftlotnra { get; set; }
        public decimal? PtasSqftlotunit { get; set; }
        public bool? PtasSyncnameaddress { get; set; }
        public decimal? PtasTaxablesellingprice { get; set; }
        public decimal? PtasTaxablesellingpriceBase { get; set; }
        public string PtasTaxfirstname { get; set; }
        public string PtasTaxlastname { get; set; }
        public bool? PtasUndividedinterest { get; set; }
        public bool? PtasVacantland { get; set; }
        public DateTimeOffset? PtasVerifiedbydate { get; set; }
        public decimal? PtasVspgra { get; set; }
        public decimal? PtasVspgraBase { get; set; }
        public decimal? PtasVspnra { get; set; }
        public decimal? PtasVspnraBase { get; set; }
        public decimal? PtasVspsqftlot { get; set; }
        public decimal? PtasVspsqftlotBase { get; set; }
        public decimal? PtasVspunit { get; set; }
        public decimal? PtasVspunitBase { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }
        public Guid? PtasAddr1CityidValue { get; set; }
        public Guid? PtasAddr1CountryidValue { get; set; }
        public Guid? PtasAddr1StateidValue { get; set; }
        public Guid? PtasAddr1StreetnameidValue { get; set; }
        public Guid? PtasAddr1StreettypeidValue { get; set; }
        public Guid? PtasAddr1ZipcodeidValue { get; set; }
        public Guid? PtasIdentifiedbyidValue { get; set; }
        public Guid? PtasNonrepresentativesale1idValue { get; set; }
        public Guid? PtasNonrepresentativesale2idValue { get; set; }
        public Guid? PtasPrimarybuildingidValue { get; set; }
        public Guid? PtasPrimaryparcelidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasUnitidValue { get; set; }
        public Guid? PtasVerifiedbyidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesale { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepository { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasSalepriceadjustment> PtasSalepriceadjustment { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessory { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuilding { get; set; }
        public virtual ICollection<PtasSalesnote> PtasSalesnote { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcel { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtask { get; set; }
        public virtual ICollection<PtasTask> PtasTask { get; set; }
    }
}
