using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasParcelmetadata
    {
        public Guid PtasParcelmetadataid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAccessorycount { get; set; }
        public int? PtasBuildingcount { get; set; }
        public int? PtasCommercialcondocount { get; set; }
        public int? PtasFloatinghomecount { get; set; }
        public string PtasHeadertext1 { get; set; }
        public string PtasHeadertext2 { get; set; }
        public int? PtasMobilehomecount { get; set; }
        public string PtasName { get; set; }
        public int? PtasNotecount { get; set; }
        public int? PtasPermitcount { get; set; }
        public int? PtasPersonalpropertycount { get; set; }
        public int? PtasProjectcount { get; set; }
        public int? PtasResidentialcondocount { get; set; }
        public int? PtasReviewcount { get; set; }
        public int? PtasSalecount { get; set; }
        public string PtasSitusaddress { get; set; }
        public string PtasSitusaddressoneline { get; set; }
        public int? PtasSnapshotcount { get; set; }
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
        public Guid? PtasParcelValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelValueNavigation { get; set; }
    }
}
