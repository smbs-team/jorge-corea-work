using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTaxrollhistory
    {
        public Guid PtasTaxrollhistoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAppraiserimpvalue { get; set; }
        public decimal? PtasAppraiserimpvalueBase { get; set; }
        public decimal? PtasAppraiserlandvalue { get; set; }
        public decimal? PtasAppraiserlandvalueBase { get; set; }
        public decimal? PtasAppraisertotalvalue { get; set; }
        public decimal? PtasAppraisertotalvalueBase { get; set; }
        public bool? PtasIscurrent { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNewconstruction { get; set; }
        public decimal? PtasNewconstructionBase { get; set; }
        public string PtasReceivabletype { get; set; }
        public decimal? PtasTaxableimpvalue { get; set; }
        public decimal? PtasTaxableimpvalueBase { get; set; }
        public decimal? PtasTaxablelandvalue { get; set; }
        public decimal? PtasTaxablelandvalueBase { get; set; }
        public decimal? PtasTaxabletotal { get; set; }
        public decimal? PtasTaxabletotalBase { get; set; }
        public string PtasTaxablevaluereason { get; set; }
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
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasOmityearidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasTaxrollcorrectionidValue { get; set; }
        public Guid? PtasTaxyearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasYear PtasOmityearidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual PtasTaxrollcorrection PtasTaxrollcorrectionidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxyearidValueNavigation { get; set; }
    }
}
