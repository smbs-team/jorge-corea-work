using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptlistedrent
    {
        public PtasAptlistedrent()
        {
            PtasAptcomparablerent = new HashSet<PtasAptcomparablerent>();
        }

        public Guid PtasAptlistedrentid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAirportnoise { get; set; }
        public int? PtasBuildingquality { get; set; }
        public int? PtasCondition { get; set; }
        public string PtasDescription { get; set; }
        public string PtasDetailcomment { get; set; }
        public DateTimeOffset? PtasEffectivestartdate { get; set; }
        public bool? PtasHasview { get; set; }
        public string PtasInformationsource { get; set; }
        public string PtasName { get; set; }
        public int? PtasNetareasqft { get; set; }
        public int? PtasNumberofbathrooms { get; set; }
        public int? PtasNumberofbedrooms { get; set; }
        public int? PtasNumberofunits { get; set; }
        public int? PtasPercentwithview { get; set; }
        public bool? PtasPool { get; set; }
        public int? PtasRegion { get; set; }
        public string PtasRenttermperiod { get; set; }
        public string PtasRenttermunit { get; set; }
        public DateTimeOffset? PtasTrenddate { get; set; }
        public decimal? PtasTrendedrent { get; set; }
        public decimal? PtasTrendedrentBase { get; set; }
        public decimal? PtasTypicalrent { get; set; }
        public decimal? PtasTypicalrentBase { get; set; }
        public int? PtasUnitbreakdowntype { get; set; }
        public string PtasUnituse { get; set; }
        public int? PtasYearbuilt { get; set; }
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
        public Guid? PtasNeighborhoodidValue { get; set; }
        public Guid? PtasParceldValue { get; set; }
        public Guid? PtasProjectidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasNeighborhood PtasNeighborhoodidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasProjectidValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerent { get; set; }
    }
}
