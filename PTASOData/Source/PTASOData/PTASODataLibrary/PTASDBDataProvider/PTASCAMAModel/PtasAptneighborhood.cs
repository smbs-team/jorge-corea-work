using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptneighborhood
    {
        public PtasAptneighborhood()
        {
            PtasAptavailablecomparablesale = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptcloseproximityPtasSalerentneighborhoodidValueNavigation = new HashSet<PtasAptcloseproximity>();
            PtasAptcloseproximityPtasSubjectneighborhoodidValueNavigation = new HashSet<PtasAptcloseproximity>();
            PtasAptincomeexpense = new HashSet<PtasAptincomeexpense>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
        }

        public Guid PtasAptneighborhoodid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public decimal? PtasCommonlaundryincome { get; set; }
        public decimal? PtasCommonlaundryincomeBase { get; set; }
        public decimal? PtasMoorageeffectiverentmultiplier { get; set; }
        public decimal? PtasMoorageexpensemultiplier { get; set; }
        public decimal? PtasMooragerent { get; set; }
        public decimal? PtasMooragerentBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasParkingcoveredsecuredrent { get; set; }
        public decimal? PtasParkingcoveredsecuredrentBase { get; set; }
        public decimal? PtasParkingcoveredunsecuredrent { get; set; }
        public decimal? PtasParkingcoveredunsecuredrentBase { get; set; }
        public decimal? PtasParkingopensecuredrent { get; set; }
        public decimal? PtasParkingopensecuredrentBase { get; set; }
        public decimal? PtasParkingopenunsecuredrent { get; set; }
        public decimal? PtasParkingopenunsecuredrentBase { get; set; }
        public decimal? PtasRank { get; set; }
        public int? PtasRegion { get; set; }
        public decimal? PtasRentmultiplier { get; set; }
        public decimal? PtasSalemodelfactor { get; set; }
        public decimal? PtasVacancyandcreditloss { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OrganizationidValue { get; set; }
        public Guid? PtasAptvaluationprojectidValue { get; set; }
        public Guid? PtasAssessmentyearlookupidValue { get; set; }
        public Guid? PtasNeighborhoodidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasAptvaluationproject PtasAptvaluationprojectidValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearlookupidValueNavigation { get; set; }
        public virtual PtasNeighborhood PtasNeighborhoodidValueNavigation { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesale { get; set; }
        public virtual ICollection<PtasAptcloseproximity> PtasAptcloseproximityPtasSalerentneighborhoodidValueNavigation { get; set; }
        public virtual ICollection<PtasAptcloseproximity> PtasAptcloseproximityPtasSubjectneighborhoodidValueNavigation { get; set; }
        public virtual ICollection<PtasAptincomeexpense> PtasAptincomeexpense { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
    }
}
