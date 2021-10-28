using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasStreettype
    {
        public PtasStreettype()
        {
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPersonalpropertyPtasBusinessStreettypeidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasPreparerStreettypeidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasTaxpayerStreettypeidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistoryPtasBusinessStreettypeidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasPreparerStreettypeidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasTaxpayerStreettypeidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
        }

        public Guid PtasStreettypeid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAlternate1 { get; set; }
        public string PtasAlternate2 { get; set; }
        public string PtasAlternate3 { get; set; }
        public string PtasAlternate4 { get; set; }
        public string PtasAlternate5 { get; set; }
        public string PtasAlternate6 { get; set; }
        public string PtasAlternate7 { get; set; }
        public string PtasAlternate8 { get; set; }
        public string PtasDescription { get; set; }
        public string PtasName { get; set; }
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

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetail { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcel { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasBusinessStreettypeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasPreparerStreettypeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasTaxpayerStreettypeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasBusinessStreettypeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasPreparerStreettypeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasTaxpayerStreettypeidValueNavigation { get; set; }
    }
}
