using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasParkingdistrict
    {
        public PtasParkingdistrict()
        {
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
        }

        public Guid PtasParkingdistrictid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasCaprate { get; set; }
        public decimal? PtasDailyrate { get; set; }
        public decimal? PtasDailyrateBase { get; set; }
        public decimal? PtasMonthlyrate { get; set; }
        public decimal? PtasMonthlyrateBase { get; set; }
        public string PtasName { get; set; }
        public int? PtasOccupancyrate { get; set; }
        public decimal? PtasOperatingexpenses { get; set; }
        public decimal? PtasOperatingexpensesBase { get; set; }
        public int? PtasPercentofmonthlystalls { get; set; }
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
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplex { get; set; }
    }
}
