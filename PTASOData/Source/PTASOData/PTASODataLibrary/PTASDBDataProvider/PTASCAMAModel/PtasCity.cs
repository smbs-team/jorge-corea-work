using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCity
    {
        public PtasCity()
        {
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPersonalpropertyPtasAddr1BusinessCityidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAddr1PreparerCityidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAddr1TaxpayerCityidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistoryPtasBusinesscityidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasPreparercityidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasTaxpayercityidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
        }

        public Guid PtasCityid { get; set; }
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
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1BusinessCityidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1PreparerCityidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAddr1TaxpayerCityidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasBusinesscityidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasPreparercityidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasTaxpayercityidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
    }
}
