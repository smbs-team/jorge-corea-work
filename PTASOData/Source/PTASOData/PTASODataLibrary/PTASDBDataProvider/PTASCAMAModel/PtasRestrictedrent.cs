using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasRestrictedrent
    {
        public Guid PtasRestrictedrentid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasSetaside100pct { get; set; }
        public decimal? PtasSetaside100pctBase { get; set; }
        public decimal? PtasSetaside120pct { get; set; }
        public decimal? PtasSetaside120pctBase { get; set; }
        public decimal? PtasSetaside20pct { get; set; }
        public decimal? PtasSetaside20pctBase { get; set; }
        public decimal? PtasSetaside30pct { get; set; }
        public decimal? PtasSetaside30pctBase { get; set; }
        public decimal? PtasSetaside35pct { get; set; }
        public decimal? PtasSetaside35pctBase { get; set; }
        public decimal? PtasSetaside40pct { get; set; }
        public decimal? PtasSetaside40pctBase { get; set; }
        public decimal? PtasSetaside45pct { get; set; }
        public decimal? PtasSetaside45pctBase { get; set; }
        public decimal? PtasSetaside50pct { get; set; }
        public decimal? PtasSetaside50pctBase { get; set; }
        public decimal? PtasSetaside60pct { get; set; }
        public decimal? PtasSetaside60pctBase { get; set; }
        public decimal? PtasSetaside70pct { get; set; }
        public decimal? PtasSetaside70pctBase { get; set; }
        public decimal? PtasSetaside80pct { get; set; }
        public decimal? PtasSetaside80pctBase { get; set; }
        public int? PtasUnittype { get; set; }
        public int? PtasUtilityprogram { get; set; }
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
        public Guid? PtasAssessmentyearValue { get; set; }
        public Guid? PtasLowincomehousingprogramValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearValueNavigation { get; set; }
        public virtual PtasHousingprogram PtasLowincomehousingprogramValueNavigation { get; set; }
    }
}
