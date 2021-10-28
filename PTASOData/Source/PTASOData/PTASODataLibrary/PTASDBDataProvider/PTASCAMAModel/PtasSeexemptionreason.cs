using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSeexemptionreason
    {
        public PtasSeexemptionreason()
        {
            PtasSeappdetail = new HashSet<PtasSeappdetail>();
            PtasSeapplication = new HashSet<PtasSeapplication>();
            PtasSefrozenvalue = new HashSet<PtasSefrozenvalue>();
        }

        public Guid PtasSeexemptionreasonid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAlternatekey { get; set; }
        public string PtasAlternatekeyname { get; set; }
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
        public virtual ICollection<PtasSeappdetail> PtasSeappdetail { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplication { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalue { get; set; }
    }
}
