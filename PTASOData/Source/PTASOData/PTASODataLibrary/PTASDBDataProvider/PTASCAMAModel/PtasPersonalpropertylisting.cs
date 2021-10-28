using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPersonalpropertylisting
    {
        public PtasPersonalpropertylisting()
        {
            PtasPersonalpropertyasset = new HashSet<PtasPersonalpropertyasset>();
        }

        public Guid PtasPersonalpropertylistingid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasAutovalue { get; set; }
        public bool? PtasBypassminimumav { get; set; }
        public decimal? PtasEditedav { get; set; }
        public decimal? PtasEditedavBase { get; set; }
        public decimal? PtasEditedavsupplies { get; set; }
        public decimal? PtasEditedavsuppliesBase { get; set; }
        public decimal? PtasEditedcost { get; set; }
        public decimal? PtasEditedcostBase { get; set; }
        public decimal? PtasEditedsupplies { get; set; }
        public decimal? PtasEditedsuppliesBase { get; set; }
        public bool? PtasListingfiled { get; set; }
        public string PtasName { get; set; }
        public int? PtasPenalty { get; set; }
        public DateTimeOffset? PtasPosted { get; set; }
        public decimal? PtasPostedav { get; set; }
        public decimal? PtasPostedavBase { get; set; }
        public decimal? PtasPostedavsupplies { get; set; }
        public decimal? PtasPostedavsuppliesBase { get; set; }
        public decimal? PtasPostedcost { get; set; }
        public decimal? PtasPostedcostBase { get; set; }
        public decimal? PtasPostedsupplies { get; set; }
        public decimal? PtasPostedsuppliesBase { get; set; }
        public decimal? PtasPrioryearav { get; set; }
        public decimal? PtasPrioryearavBase { get; set; }
        public decimal? PtasPrioryearavsupplies { get; set; }
        public decimal? PtasPrioryearavsuppliesBase { get; set; }
        public decimal? PtasPrioryearcost { get; set; }
        public decimal? PtasPrioryearcostBase { get; set; }
        public decimal? PtasPrioryearsupplies { get; set; }
        public decimal? PtasPrioryearsuppliesBase { get; set; }
        public bool? PtasRevisedlisting { get; set; }
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
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyasset { get; set; }
    }
}
