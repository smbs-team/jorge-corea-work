using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBookmarktag
    {
        public PtasBookmarktag()
        {
            PtasBookmarkPtasTag1ValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkPtasTag2ValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkPtasTag3ValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkPtasTag4ValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkPtasTag5ValueNavigation = new HashSet<PtasBookmark>();
        }

        public Guid PtasBookmarktagid { get; set; }
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
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkPtasTag1ValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkPtasTag2ValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkPtasTag3ValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkPtasTag4ValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkPtasTag5ValueNavigation { get; set; }
    }
}
