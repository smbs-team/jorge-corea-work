using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptavailablecomparablesale
    {
        public PtasAptavailablecomparablesale()
        {
            PtasAptcomparablesale = new HashSet<PtasAptcomparablesale>();
        }

        public Guid PtasAptavailablecomparablesaleid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAirportnoiseadjustment { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public int? PtasAverageunitsize { get; set; }
        public int? PtasBuildingquality { get; set; }
        public decimal? PtasCaprate { get; set; }
        public int? PtasCondition { get; set; }
        public bool? PtasElevators { get; set; }
        public string PtasExcisetaxnumber { get; set; }
        public decimal? PtasGim { get; set; }
        public int? PtasGrosssqft { get; set; }
        public string PtasName { get; set; }
        public int? PtasNetsqft { get; set; }
        public int? PtasNumberofunits { get; set; }
        public int? PtasPercentcommercial { get; set; }
        public int? PtasPercentwithview { get; set; }
        public bool? PtasPool { get; set; }
        public string PtasPropertyaddress { get; set; }
        public string PtasPropertyname { get; set; }
        public DateTimeOffset? PtasSaledate { get; set; }
        public decimal? PtasSaleprice { get; set; }
        public decimal? PtasSalepriceBase { get; set; }
        public decimal? PtasSalepriceperunit { get; set; }
        public decimal? PtasSalepriceperunitBase { get; set; }
        public DateTimeOffset? PtasTrenddate { get; set; }
        public decimal? PtasTrendedsaleprice { get; set; }
        public decimal? PtasTrendedsalepriceBase { get; set; }
        public decimal? PtasTrendedsalepriceperunit { get; set; }
        public decimal? PtasTrendedsalepriceperunitBase { get; set; }
        public decimal? PtasViewrank { get; set; }
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
        public Guid? PtasApartmentneighborhoodidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasResponsibilityidValue { get; set; }
        public Guid? PtasSaleidValue { get; set; }
        public Guid? PtasSpecialtyareaidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAptneighborhood PtasApartmentneighborhoodidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasResponsibility PtasResponsibilityidValueNavigation { get; set; }
        public virtual PtasSales PtasSaleidValueNavigation { get; set; }
        public virtual PtasSpecialtyarea PtasSpecialtyareaidValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesale { get; set; }
    }
}
