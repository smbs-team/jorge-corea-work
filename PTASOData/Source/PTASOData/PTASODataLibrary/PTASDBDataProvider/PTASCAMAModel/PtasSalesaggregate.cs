using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSalesaggregate
    {
        public Guid PtasSalesaggregateid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAcres { get; set; }
        public string PtasAddress { get; set; }
        public int? PtasAvgsqftofunits { get; set; }
        public int? PtasBuildinggrosssqft { get; set; }
        public int? PtasBuildingnetsqft { get; set; }
        public int? PtasBuildingquality { get; set; }
        public int? PtasConstructionclass { get; set; }
        public string PtasDescription { get; set; }
        public string PtasFolio { get; set; }
        public int? PtasLotsizesqft { get; set; }
        public string PtasMajor { get; set; }
        public int? PtasMaxfloors { get; set; }
        public string PtasMinor { get; set; }
        public string PtasName { get; set; }
        public int? PtasNoofunitssold { get; set; }
        public decimal? PtasPercentview { get; set; }
        public int? PtasPresentuse { get; set; }
        public string PtasPrimarybuilding { get; set; }
        public int? PtasProjectappeal { get; set; }
        public int? PtasProjectlocation { get; set; }
        public string PtasPropertyname { get; set; }
        public decimal? PtasSqftlotgrossbuildingarea { get; set; }
        public decimal? PtasSqftlotnetbuildingarea { get; set; }
        public decimal? PtasSqftlotunit { get; set; }
        public int? PtasTotalbuildings { get; set; }
        public bool? PtasVacantland { get; set; }
        public decimal? PtasVerifiedsalepricesqftlot { get; set; }
        public bool? PtasViews { get; set; }
        public decimal? PtasVsppergra { get; set; }
        public decimal? PtasVsppernra { get; set; }
        public decimal? PtasVspperunit { get; set; }
        public string PtasZoning { get; set; }
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
        public Guid? PtasBuildingsectionuseidValue { get; set; }
        public Guid? PtasDistrictidValue { get; set; }
        public Guid? PtasGeoareaidValue { get; set; }
        public Guid? PtasGeoneighborhoodidValue { get; set; }
        public Guid? PtasPresentuseidValue { get; set; }
        public Guid? PtasPrimarybuildingidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasQstridValue { get; set; }
        public Guid? PtasResponsibilityidValue { get; set; }
        public Guid? PtasSaleidValue { get; set; }
        public Guid? PtasSpecialtyareaidValue { get; set; }
        public Guid? PtasSpecialtyneighborhoodidValue { get; set; }
        public Guid? PtasYearbuiltidValue { get; set; }
        public Guid? PtasYeareffectiveidValue { get; set; }
        public Guid? PtasZoningidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBuildingsectionuse PtasBuildingsectionuseidValueNavigation { get; set; }
        public virtual PtasDistrict PtasDistrictidValueNavigation { get; set; }
        public virtual PtasGeoarea PtasGeoareaidValueNavigation { get; set; }
        public virtual PtasGeoneighborhood PtasGeoneighborhoodidValueNavigation { get; set; }
        public virtual PtasLanduse PtasPresentuseidValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasPrimarybuildingidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasQstr PtasQstridValueNavigation { get; set; }
        public virtual PtasResponsibility PtasResponsibilityidValueNavigation { get; set; }
        public virtual PtasSales PtasSaleidValueNavigation { get; set; }
        public virtual PtasSpecialtyarea PtasSpecialtyareaidValueNavigation { get; set; }
        public virtual PtasSpecialtyneighborhood PtasSpecialtyneighborhoodidValueNavigation { get; set; }
        public virtual PtasYear PtasYearbuiltidValueNavigation { get; set; }
        public virtual PtasYear PtasYeareffectiveidValueNavigation { get; set; }
        public virtual PtasZoning PtasZoningidValueNavigation { get; set; }
    }
}
