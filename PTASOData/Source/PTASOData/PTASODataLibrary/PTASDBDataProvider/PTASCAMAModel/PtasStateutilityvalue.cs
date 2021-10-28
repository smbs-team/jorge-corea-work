using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasStateutilityvalue
    {
        public Guid PtasStateutilityvalueid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasCompanyaddressline1 { get; set; }
        public string PtasCompanyaddressline2 { get; set; }
        public string PtasCompanyaddressline3 { get; set; }
        public string PtasCompanydoingbusinessas { get; set; }
        public string PtasCompanyname { get; set; }
        public string PtasCompanynumber { get; set; }
        public int? PtasCompanytype { get; set; }
        public string PtasCompanyzipcode { get; set; }
        public string PtasContact { get; set; }
        public string PtasLevycode { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasPersonalequalizedvalue { get; set; }
        public decimal? PtasPersonalequalizedvalueBase { get; set; }
        public decimal? PtasRealequalizedvalue { get; set; }
        public decimal? PtasRealequalizedvalueBase { get; set; }
        public string PtasTcanumber { get; set; }
        public decimal? PtasTotalrealpersonalequalizedvalue { get; set; }
        public decimal? PtasTotalrealpersonalequalizedvalueBase { get; set; }
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
    }
}
