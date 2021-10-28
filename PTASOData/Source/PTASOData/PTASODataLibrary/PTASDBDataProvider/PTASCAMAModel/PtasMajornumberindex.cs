using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasMajornumberindex
    {
        public PtasMajornumberindex()
        {
            PtasAbstractprojectPtasCondomajornumberidValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectPtasPlatmajornumberidValueNavigation = new HashSet<PtasAbstractproject>();
            PtasMajornumberdetail = new HashSet<PtasMajornumberdetail>();
        }

        public Guid PtasMajornumberindexid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasIsreserved { get; set; }
        public int? PtasLegacyisn { get; set; }
        public string PtasName { get; set; }
        public string PtasPlatorcondoname { get; set; }
        public int? PtasProjecttype { get; set; }
        public string PtasReserveddescription { get; set; }
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
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectPtasCondomajornumberidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectPtasPlatmajornumberidValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberdetail> PtasMajornumberdetail { get; set; }
    }
}
