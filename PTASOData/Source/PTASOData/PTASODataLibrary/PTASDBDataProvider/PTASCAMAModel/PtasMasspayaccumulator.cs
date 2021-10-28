using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasMasspayaccumulator
    {
        public Guid PtasMasspayaccumulatorid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAddordelete { get; set; }
        public string PtasCompanyname { get; set; }
        public string PtasCompanynumber { get; set; }
        public string PtasCountyid { get; set; }
        public bool? PtasError { get; set; }
        public string PtasErrorreason { get; set; }
        public string PtasFilename { get; set; }
        public string PtasLoannumber { get; set; }
        public DateTime? PtasLoanoriginationdate { get; set; }
        public string PtasName { get; set; }
        public bool? PtasPrimary { get; set; }
        public DateTime? PtasRundate { get; set; }
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
        public Guid? PtasAgentcodeidValue { get; set; }
        public Guid? PtasTaxaccountnumberidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasMasspayer PtasAgentcodeidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountnumberidValueNavigation { get; set; }
    }
}
