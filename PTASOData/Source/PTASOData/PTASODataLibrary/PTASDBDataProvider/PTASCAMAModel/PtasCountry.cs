using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCountry
    {
        public PtasCountry()
        {
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPersonalpropertyPtasPreparercountryidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasTaxpayercountryidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistoryPtasPreparercountryidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasTaxpayercountryidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasSeapplicationPtasAddrcountryidValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationPtasCheckaddresscountryidValueNavigation = new HashSet<PtasSeapplication>();
            PtasStateorprovince = new HashSet<PtasStateorprovince>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
        }

        public Guid PtasCountryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAbbreviation { get; set; }
        public string PtasAlpha2 { get; set; }
        public string PtasName { get; set; }
        public string PtasNumericcode { get; set; }
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
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasPreparercountryidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasTaxpayercountryidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasPreparercountryidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasTaxpayercountryidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationPtasAddrcountryidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationPtasCheckaddresscountryidValueNavigation { get; set; }
        public virtual ICollection<PtasStateorprovince> PtasStateorprovince { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
    }
}
