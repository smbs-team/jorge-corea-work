using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBookmark
    {
        public Guid PtasBookmarkid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public DateTimeOffset? PtasBookmarkdate { get; set; }
        public string PtasBookmarknote { get; set; }
        public int? PtasBookmarktype { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasName { get; set; }
        public string PtasTags { get; set; }
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
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? PtasTag1Value { get; set; }
        public Guid? PtasTag2Value { get; set; }
        public Guid? PtasTag3Value { get; set; }
        public Guid? PtasTag4Value { get; set; }
        public Guid? PtasTag5Value { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual PtasBookmarktag PtasTag1ValueNavigation { get; set; }
        public virtual PtasBookmarktag PtasTag2ValueNavigation { get; set; }
        public virtual PtasBookmarktag PtasTag3ValueNavigation { get; set; }
        public virtual PtasBookmarktag PtasTag4ValueNavigation { get; set; }
        public virtual PtasBookmarktag PtasTag5ValueNavigation { get; set; }
    }
}
