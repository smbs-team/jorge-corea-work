using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasStateorprovince
    {
        public PtasStateorprovince()
        {
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPersonalpropertyPtasAddr1BusinessStateidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAddr1PreparerStateidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAddr1TaxpayerStateidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasStateincorporatedidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistoryPtasBusinessstateincorporatedValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasPreparerstateidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasTaxpayerstateidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasQuickcollectPtasNewinformationAddrLocationstateidValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectPtasNewinformationAddrStateidValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectPtasPersonalpropinfoAddrLocationstateidValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectPtasPersonalpropinfoAddrStateidValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectPtasRequestorinfoAddrStateidValueNavigation = new HashSet<PtasQuickcollect>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
        }

        public Guid PtasStateorprovinceid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAbbreviation { get; set; }
        public string PtasAlpha3 { get; set; }
        public string PtasName { get; set; }
        public string PtasUncode { get; set; }
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
        public Guid? PtasCountryidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasCountry PtasCountryidValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetail { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplex { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcel { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1BusinessStateidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1PreparerStateidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1TaxpayerStateidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasStateincorporatedidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasBusinessstateincorporatedValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasPreparerstateidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasTaxpayerstateidValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasNewinformationAddrLocationstateidValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasNewinformationAddrStateidValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasPersonalpropinfoAddrLocationstateidValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasPersonalpropinfoAddrStateidValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasRequestorinfoAddrStateidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
    }
}
