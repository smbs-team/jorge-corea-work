using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAnnexationtracker
    {
        public PtasAnnexationtracker()
        {
            PtasAnnexationparcelreview = new HashSet<PtasAnnexationparcelreview>();
            PtasLevycodechangePtasAnnexationtrackeridValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasParcelidValueNavigation = new HashSet<PtasLevycodechange>();
        }

        public Guid PtasAnnexationtrackerid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAnnexationname { get; set; }
        public int? PtasAnnexationtype { get; set; }
        public string PtasBoundaryreviewboardnumber { get; set; }
        public DateTimeOffset? PtasBrbnoticeofintentdate { get; set; }
        public DateTimeOffset? PtasBrbnoticveofintentdate { get; set; }
        public bool? PtasChangelevycodes { get; set; }
        public DateTimeOffset? PtasCompletedlcreview { get; set; }
        public DateTimeOffset? PtasCompletedreceivedfromgis { get; set; }
        public DateTimeOffset? PtasCompletedsubmittogis { get; set; }
        public bool? PtasCreatelimit { get; set; }
        public bool? PtasCreatenewlevycodes { get; set; }
        public DateTimeOffset? PtasDoafirstreceived { get; set; }
        public DateTimeOffset? PtasDoaprocessed { get; set; }
        public DateTimeOffset? PtasDraftreceivedfromgis { get; set; }
        public DateTimeOffset? PtasDraftsubmittogis { get; set; }
        public DateTimeOffset? PtasEffectivedate { get; set; }
        public string PtasName { get; set; }
        public int? PtasNumberofparcels { get; set; }
        public int? PtasNumberofparcelsverified { get; set; }
        public string PtasResolutionordinance { get; set; }
        public DateTimeOffset? PtasSenttodor { get; set; }
        public DateTimeOffset? PtasSenttopersonalproperty { get; set; }
        public DateTimeOffset? PtasSenttorecordersoffice { get; set; }
        public DateTimeOffset? PtasSenttotreasury { get; set; }
        public decimal? PtasSignedandverifiedav { get; set; }
        public decimal? PtasSignedandverifiedavBase { get; set; }
        public decimal? PtasSignedandverifiedpercentage { get; set; }
        public decimal? PtasTotalassessedvalue { get; set; }
        public decimal? PtasTotalassessedvalueBase { get; set; }
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
        public Guid? PtasTaxingdistrictidValue { get; set; }
        public Guid? PtasTaxrollyeareffectiveidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTaxingdistrictidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxrollyeareffectiveidValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreview { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasAnnexationtrackeridValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasParcelidValueNavigation { get; set; }
    }
}
