using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAbstractproject
    {
        public PtasAbstractproject()
        {
            PtasAbstractdocument = new HashSet<PtasAbstractdocument>();
            PtasAbstractprojectresultparcel = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAbstractprojectsourceparcel = new HashSet<PtasAbstractprojectsourceparcel>();
        }

        public Guid PtasAbstractprojectid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasCondoCreateparcelsforexistingunits { get; set; }
        public bool? PtasCreatenewdevelopmentrights { get; set; }
        public bool? PtasCreateparcelsandtaxaccounts { get; set; }
        public bool? PtasCreateresultparcels { get; set; }
        public int? PtasIncrementminornumbersby { get; set; }
        public DateTimeOffset? PtasMppgDatemappingworkcomplete { get; set; }
        public DateTimeOffset? PtasMppgDatesenttomapping { get; set; }
        public string PtasMppgMappingchangedescription { get; set; }
        public string PtasName { get; set; }
        public string PtasNpdBlock { get; set; }
        public string PtasNpdBuilding { get; set; }
        public string PtasNpdMiscellaneousdescription { get; set; }
        public string PtasNpdTaxpayername { get; set; }
        public int? PtasNumberofcondostocreate { get; set; }
        public bool? PtasOtherAretaxlots { get; set; }
        public int? PtasOtherNumberoflotstocreate { get; set; }
        public int? PtasOtherNumberoftractstocreate { get; set; }
        public bool? PtasPhasedcondo { get; set; }
        public int? PtasPlatNumberoflotstocreate { get; set; }
        public int? PtasPlatNumberoftractstocreate { get; set; }
        public int? PtasPlatStartinglotnumber { get; set; }
        public string PtasPlatname { get; set; }
        public DateTimeOffset? PtasProjecteffectivedate { get; set; }
        public int? PtasProjectphase { get; set; }
        public int? PtasProjectreason { get; set; }
        public DateTimeOffset? PtasProjectstartdate { get; set; }
        public int? PtasProjecttype { get; set; }
        public bool? PtasResultparcelscreated { get; set; }
        public bool? PtasRpAllocatevalues { get; set; }
        public int? PtasRpLandvalueallocationmethod { get; set; }
        public bool? PtasShowabstractdocuments { get; set; }
        public string PtasSkipminornumbersrangeend { get; set; }
        public string PtasSkipminornumbersrangestart { get; set; }
        public decimal? PtasSpTotalimprovementvalue { get; set; }
        public decimal? PtasSpTotalimprovementvalueBase { get; set; }
        public decimal? PtasSpTotallandvalue { get; set; }
        public decimal? PtasSpTotallandvalueBase { get; set; }
        public int? PtasSpTotalsquarefootage { get; set; }
        public bool? PtasSpValuesbeforemustequalafter { get; set; }
        public string PtasSpcCurrentexemptionsmulti { get; set; }
        public bool? PtasSpcDifferentlevycodes { get; set; }
        public bool? PtasSpcDifferentowners { get; set; }
        public bool? PtasSpcDifferenttaxstatus { get; set; }
        public bool? PtasSpcExemptionstobereadded { get; set; }
        public bool? PtasSpcForcesegregation { get; set; }
        public bool? PtasSpcForestfireacres { get; set; }
        public bool? PtasSpcSplitaccounts { get; set; }
        public bool? PtasSpcUnpaidtaxes { get; set; }
        public int? PtasStartingcondonumber { get; set; }
        public string PtasStartingminornumber { get; set; }
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
        public Guid? PtasCondocomplexidValue { get; set; }
        public Guid? PtasCondomajornumberidValue { get; set; }
        public Guid? PtasLegaltrailerValue { get; set; }
        public Guid? PtasMppgMappingchangemadebyidValue { get; set; }
        public Guid? PtasNpdAreaidValue { get; set; }
        public Guid? PtasNpdDrainagedistrictidValue { get; set; }
        public Guid? PtasNpdEndrollyearidValue { get; set; }
        public Guid? PtasNpdLevycodeidValue { get; set; }
        public Guid? PtasNpdNeighborhoodidValue { get; set; }
        public Guid? PtasNpdPropertytypeidValue { get; set; }
        public Guid? PtasNpdQstridValue { get; set; }
        public Guid? PtasNpdResponsibilityidValue { get; set; }
        public Guid? PtasNpdStartrollyearidValue { get; set; }
        public Guid? PtasNpdSubareaidValue { get; set; }
        public Guid? PtasNpdZoningidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? PtasPlatmajornumberidValue { get; set; }
        public Guid? PtasProjectrollyearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasCondocomplexidValueNavigation { get; set; }
        public virtual PtasMajornumberindex PtasCondomajornumberidValueNavigation { get; set; }
        public virtual PtasLegaldescriptions PtasLegaltrailerValueNavigation { get; set; }
        public virtual Systemuser PtasMppgMappingchangemadebyidValueNavigation { get; set; }
        public virtual PtasArea PtasNpdAreaidValueNavigation { get; set; }
        public virtual PtasDistrict PtasNpdDrainagedistrictidValueNavigation { get; set; }
        public virtual PtasYear PtasNpdEndrollyearidValueNavigation { get; set; }
        public virtual PtasLevycode PtasNpdLevycodeidValueNavigation { get; set; }
        public virtual PtasNeighborhood PtasNpdNeighborhoodidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasNpdPropertytypeidValueNavigation { get; set; }
        public virtual PtasQstr PtasNpdQstridValueNavigation { get; set; }
        public virtual PtasResponsibility PtasNpdResponsibilityidValueNavigation { get; set; }
        public virtual PtasYear PtasNpdStartrollyearidValueNavigation { get; set; }
        public virtual PtasSubarea PtasNpdSubareaidValueNavigation { get; set; }
        public virtual PtasZoning PtasNpdZoningidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual PtasMajornumberindex PtasPlatmajornumberidValueNavigation { get; set; }
        public virtual PtasYear PtasProjectrollyearidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocument { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcel { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcel { get; set; }
    }
}
