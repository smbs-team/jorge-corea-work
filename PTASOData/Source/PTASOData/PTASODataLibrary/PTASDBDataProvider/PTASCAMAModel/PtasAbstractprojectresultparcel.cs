using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAbstractprojectresultparcel
    {
        public PtasAbstractprojectresultparcel()
        {
            PtasParceldetail = new HashSet<PtasParceldetail>();
        }

        public Guid PtasAbstractprojectresultparcelid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasAeCurrentuse { get; set; }
        public bool? PtasAeHistoricbuilding { get; set; }
        public bool? PtasAeHomeimprovement { get; set; }
        public bool? PtasAeMultifamily { get; set; }
        public bool? PtasAeNonprofit { get; set; }
        public bool? PtasAeSeniorordisability { get; set; }
        public decimal? PtasDetailsImprovementsvalue { get; set; }
        public decimal? PtasDetailsImprovementsvalueBase { get; set; }
        public decimal? PtasDetailsLandvalue { get; set; }
        public decimal? PtasDetailsLandvalueBase { get; set; }
        public decimal? PtasDetailsParcelacres { get; set; }
        public int? PtasDetailsParcelsquarefootage { get; set; }
        public string PtasInfoBlock { get; set; }
        public string PtasInfoBuilding { get; set; }
        public int? PtasInfoLot { get; set; }
        public string PtasInfoPropertyname { get; set; }
        public int? PtasInfoUnit { get; set; }
        public string PtasInfoUnitChar { get; set; }
        public string PtasLandfractionallocation { get; set; }
        public decimal? PtasLandpercentallocation { get; set; }
        public string PtasLegaldescription { get; set; }
        public string PtasMajor { get; set; }
        public string PtasMinor { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasOtherBenefitacres { get; set; }
        public decimal? PtasOtherForestfireacres { get; set; }
        public string PtasParcelnumber { get; set; }
        public int? PtasParceltype { get; set; }
        public decimal? PtasPercentundividedinterest { get; set; }
        public string PtasSitusaddress { get; set; }
        public int? PtasTaxstatus { get; set; }
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
        public Guid? PtasAbstractprojectidValue { get; set; }
        public Guid? PtasCondocomplexidValue { get; set; }
        public Guid? PtasInfoAreaidValue { get; set; }
        public Guid? PtasInfoLevycodeidValue { get; set; }
        public Guid? PtasInfoNeighborhoodidValue { get; set; }
        public Guid? PtasInfoPropertytypeidValue { get; set; }
        public Guid? PtasInfoQstridValue { get; set; }
        public Guid? PtasInfoResponsibilityidValue { get; set; }
        public Guid? PtasInfoSubareaidValue { get; set; }
        public Guid? PtasOtherDrainagedistrictidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAbstractproject PtasAbstractprojectidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasCondocomplexidValueNavigation { get; set; }
        public virtual PtasArea PtasInfoAreaidValueNavigation { get; set; }
        public virtual PtasLevycode PtasInfoLevycodeidValueNavigation { get; set; }
        public virtual PtasNeighborhood PtasInfoNeighborhoodidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasInfoPropertytypeidValueNavigation { get; set; }
        public virtual PtasQstr PtasInfoQstridValueNavigation { get; set; }
        public virtual PtasResponsibility PtasInfoResponsibilityidValueNavigation { get; set; }
        public virtual PtasSubarea PtasInfoSubareaidValueNavigation { get; set; }
        public virtual PtasDistrict PtasOtherDrainagedistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
    }
}
