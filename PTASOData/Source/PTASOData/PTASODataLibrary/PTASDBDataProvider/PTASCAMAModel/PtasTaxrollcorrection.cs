using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTaxrollcorrection
    {
        public PtasTaxrollcorrection()
        {
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
            PtasTaxrollcorrectionvalue = new HashSet<PtasTaxrollcorrectionvalue>();
        }

        public Guid PtasTaxrollcorrectionid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasChangereason { get; set; }
        public string PtasComments { get; set; }
        public DateTime? PtasDateoforder { get; set; }
        public DateTimeOffset? PtasDaterequested { get; set; }
        public bool? PtasExtendtonexttaxyear { get; set; }
        public bool? PtasManifesterror { get; set; }
        public DateTimeOffset? PtasManifesterrordiscoverydate { get; set; }
        public int? PtasManifesterrorreason { get; set; }
        public string PtasName { get; set; }
        public string PtasNumber { get; set; }
        public string PtasPropertyowner { get; set; }
        public string PtasStatusdetails { get; set; }
        public int? PtasTaxvaluereason { get; set; }
        public string PtasYearsofcorrection { get; set; }
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
        public Guid? PtasAreaidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasRequestedbyidValue { get; set; }
        public Guid? PtasReviewedbyidValue { get; set; }
        public Guid? PtasSeniorrevieweridValue { get; set; }
        public Guid? PtasSubareaidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasArea PtasAreaidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual Systemuser PtasRequestedbyidValueNavigation { get; set; }
        public virtual Systemuser PtasReviewedbyidValueNavigation { get; set; }
        public virtual Systemuser PtasSeniorrevieweridValueNavigation { get; set; }
        public virtual PtasSubarea PtasSubareaidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalue { get; set; }
    }
}
