using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPerspropbannerannouncement
    {
        public Guid PtasPerspropbannerannouncementid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasBannertext { get; set; }
        public DateTimeOffset? PtasEnddate { get; set; }
        public bool? PtasFarming { get; set; }
        public bool? PtasGeneralbusiness { get; set; }
        public bool? PtasHomeoffice { get; set; }
        public bool? PtasImpsonexemptland { get; set; }
        public bool? PtasLeasingcompany { get; set; }
        public bool? PtasMall { get; set; }
        public bool? PtasMarijuanarelated { get; set; }
        public string PtasName { get; set; }
        public bool? PtasOilcompanies { get; set; }
        public bool? PtasSpecialty { get; set; }
        public DateTimeOffset? PtasStartdate { get; set; }
        public bool? PtasTimber { get; set; }
        public bool? PtasUnspecified { get; set; }
        public bool? PtasVendingequipment { get; set; }
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
    }
}
