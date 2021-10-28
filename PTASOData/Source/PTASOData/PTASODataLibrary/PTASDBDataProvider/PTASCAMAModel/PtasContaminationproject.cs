using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasContaminationproject
    {
        public PtasContaminationproject()
        {
            PtasAnnualcostdistribution = new HashSet<PtasAnnualcostdistribution>();
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
            PtasContaminatedlandreduction = new HashSet<PtasContaminatedlandreduction>();
        }

        public Guid PtasContaminationprojectid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? Ptas10yeartreasuryrate { get; set; }
        public int? PtasEstimatetimetocure { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasPresentcost { get; set; }
        public decimal? PtasPresentcostBase { get; set; }
        public decimal? PtasTotalremediationcost { get; set; }
        public decimal? PtasTotalremediationcostBase { get; set; }
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
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAnnualcostdistribution> PtasAnnualcostdistribution { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplex { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreduction { get; set; }
    }
}
