using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasEnvironmentalrestriction
    {
        public Guid PtasEnvironmentalrestrictionid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasContaminationspecialistneeded { get; set; }
        public bool? PtasDelineationstudy { get; set; }
        public decimal? PtasDollaradjustment { get; set; }
        public decimal? PtasDollaradjustmentBase { get; set; }
        public decimal? PtasDollarpersqft { get; set; }
        public decimal? PtasDollarpersqftBase { get; set; }
        public int? PtasEnvironmentalrestrictionsource { get; set; }
        public int? PtasLegacyrplandid { get; set; }
        public string PtasName { get; set; }
        public int? PtasPercentadjustment { get; set; }
        public int? PtasPercentaffected { get; set; }
        public int? PtasPercentremediationcost { get; set; }
        public string PtasProjectname { get; set; }
        public decimal? PtasSqft { get; set; }
        public int? PtasValuemethodenvironment { get; set; }
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
        public Guid? PtasEnvironmentalrestrictiontypeidValue { get; set; }
        public Guid? PtasLandidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasEnvironmentalrestrictiontype PtasEnvironmentalrestrictiontypeidValueNavigation { get; set; }
        public virtual PtasLand PtasLandidValueNavigation { get; set; }
    }
}
