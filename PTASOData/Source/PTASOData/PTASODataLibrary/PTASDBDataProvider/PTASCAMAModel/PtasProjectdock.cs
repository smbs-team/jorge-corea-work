using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasProjectdock
    {
        public PtasProjectdock()
        {
            InversePtasMasterdockidValueNavigation = new HashSet<PtasProjectdock>();
            PtasCondounit = new HashSet<PtasCondounit>();
        }

        public Guid PtasProjectdockid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAlternatekey { get; set; }
        public bool? PtasGated { get; set; }
        public string PtasName { get; set; }
        public string PtasSecuritycode { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
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
        public Guid? PtasCondocomplexidValue { get; set; }
        public Guid? PtasMasterdockidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasCondocomplexidValueNavigation { get; set; }
        public virtual PtasProjectdock PtasMasterdockidValueNavigation { get; set; }
        public virtual ICollection<PtasProjectdock> InversePtasMasterdockidValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
    }
}
