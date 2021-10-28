using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSalesaccessory
    {
        public Guid PtasSalesaccessoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasLinenbr { get; set; }
        public string PtasName { get; set; }
        public int? PtasQuantity { get; set; }
        public int? PtasSize { get; set; }
        public string PtasType { get; set; }
        public int? PtasUnitnbr { get; set; }
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
        public Guid? PtasAccessorydetailidValue { get; set; }
        public Guid? PtasSalesbuildingidValue { get; set; }
        public Guid? PtasSalesidValue { get; set; }
        public Guid? PtasSalesparcelidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAccessorydetail PtasAccessorydetailidValueNavigation { get; set; }
        public virtual PtasSalesbuilding PtasSalesbuildingidValueNavigation { get; set; }
        public virtual PtasSales PtasSalesidValueNavigation { get; set; }
        public virtual PtasSalesparcel PtasSalesparcelidValueNavigation { get; set; }
    }
}
