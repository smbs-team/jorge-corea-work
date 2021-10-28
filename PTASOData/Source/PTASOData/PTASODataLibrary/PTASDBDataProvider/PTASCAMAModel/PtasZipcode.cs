using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasZipcode
    {
        public PtasZipcode()
        {
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPersonalpropertyPtasAddr1BusinessZipcodeidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAddr1PreparerZipcodeidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAddr1TaxpayerZipcodeidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistoryPtasBusinesszipidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasPreparerzipidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasTaxpayerzipidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
        }

        public Guid PtasZipcodeid { get; set; }
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
        public virtual ICollection<PtasCondocomplex> PtasCondocomplex { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcel { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1BusinessZipcodeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1PreparerZipcodeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1TaxpayerZipcodeidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasBusinesszipidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasPreparerzipidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasTaxpayerzipidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
    }
}
