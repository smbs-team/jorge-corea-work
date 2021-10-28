using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSketch
    {
        public PtasSketch()
        {
            InversePtasTemplateidValueNavigation = new HashSet<PtasSketch>();
            PtasAccessorydetail = new HashSet<PtasAccessorydetail>();
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasVisitedsketch = new HashSet<PtasVisitedsketch>();
        }

        public Guid PtasSketchid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasBloburi { get; set; }
        public DateTimeOffset? PtasDrawdate { get; set; }
        public bool? PtasIscomplete { get; set; }
        public bool? PtasIsofficial { get; set; }
        public bool? PtasLocked { get; set; }
        public string PtasName { get; set; }
        public string PtasTags { get; set; }
        public string PtasVersion { get; set; }
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
        public Guid? PtasAccessoryidValue { get; set; }
        public Guid? PtasBuildingidValue { get; set; }
        public Guid? PtasDrawauthoridValue { get; set; }
        public Guid? PtasLockedbyidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasTemplateidValue { get; set; }
        public Guid? PtasUnitidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAccessorydetail PtasAccessoryidValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingidValueNavigation { get; set; }
        public virtual Systemuser PtasDrawauthoridValueNavigation { get; set; }
        public virtual Systemuser PtasLockedbyidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasSketch PtasTemplateidValueNavigation { get; set; }
        public virtual PtasCondounit PtasUnitidValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> InversePtasTemplateidValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetail { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetail { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketch { get; set; }
    }
}
