using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasStreetname
    {
        public PtasStreetname()
        {
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPersonalpropertyPtasBusinessStreetnameidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasPreparerStreetnameidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasTaxpayerStreetnameidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistoryPtasBusinessStreetnameidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasPreparerStreetnameidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasTaxpayerStreetnameidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
        }

        public Guid PtasStreetnameid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
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
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasBusinessStreetnameidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasPreparerStreetnameidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasTaxpayerStreetnameidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasBusinessStreetnameidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasPreparerStreetnameidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasTaxpayerStreetnameidValueNavigation { get; set; }
    }
}
