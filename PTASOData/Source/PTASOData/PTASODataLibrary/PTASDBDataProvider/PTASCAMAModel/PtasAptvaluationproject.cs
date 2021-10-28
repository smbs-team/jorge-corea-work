using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptvaluationproject
    {
        public PtasAptvaluationproject()
        {
            PtasAptneighborhood = new HashSet<PtasAptneighborhood>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
        }

        public Guid PtasAptvaluationprojectid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasApartmentneighborhood { get; set; }
        public string PtasApartmentregressionprojecturl { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public bool? PtasComparablesalesapproachran { get; set; }
        public decimal? PtasComparablesalesweight { get; set; }
        public bool? PtasEmvapproachran { get; set; }
        public decimal? PtasEmvweight { get; set; }
        public bool? PtasIncomeapproachran { get; set; }
        public decimal? PtasIncomeweight { get; set; }
        public string PtasName { get; set; }
        public DateTimeOffset? PtasRentcompenddate { get; set; }
        public DateTimeOffset? PtasRentcompstartdate { get; set; }
        public DateTimeOffset? PtasRentregressionenddate { get; set; }
        public DateTimeOffset? PtasRentregressionstartdate { get; set; }
        public DateTimeOffset? PtasSalecompenddate { get; set; }
        public DateTimeOffset? PtasSalecompstartdate { get; set; }
        public DateTimeOffset? PtasSaleregressionenddate { get; set; }
        public DateTimeOffset? PtasSaleregressionstartdate { get; set; }
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
        public Guid? PtasAssessmentyearlookupidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearlookupidValueNavigation { get; set; }
        public virtual ICollection<PtasAptneighborhood> PtasAptneighborhood { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
    }
}
