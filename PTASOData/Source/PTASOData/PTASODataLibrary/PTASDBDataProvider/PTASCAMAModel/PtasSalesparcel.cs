using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSalesparcel
    {
        public PtasSalesparcel()
        {
            PtasSalesaccessory = new HashSet<PtasSalesaccessory>();
            PtasSalesbuilding = new HashSet<PtasSalesbuilding>();
        }

        public Guid PtasSalesparcelid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAcctnbr { get; set; }
        public string PtasAddress { get; set; }
        public int? PtasCommarea { get; set; }
        public int? PtasCommsubarea { get; set; }
        public string PtasDirsuffix { get; set; }
        public string PtasDistrict { get; set; }
        public string PtasFolio { get; set; }
        public string PtasLegaldescription { get; set; }
        public string PtasLevycode { get; set; }
        public bool? PtasMainparcel { get; set; }
        public string PtasMajor { get; set; }
        public string PtasMinor { get; set; }
        public string PtasName { get; set; }
        public string PtasNamesonaccount { get; set; }
        public string PtasNbrfraction { get; set; }
        public int? PtasNeighborhood { get; set; }
        public string PtasPlatblock { get; set; }
        public string PtasPlatlot { get; set; }
        public string PtasProptype { get; set; }
        public int? PtasResarea { get; set; }
        public int? PtasRessubarea { get; set; }
        public bool? PtasSalesaccessorycreated { get; set; }
        public bool? PtasSalesbuildingcreated { get; set; }
        public string PtasStreetname { get; set; }
        public string PtasStreetnbr { get; set; }
        public string PtasStreettype { get; set; }
        public string PtasZipcode { get; set; }
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
        public Guid? PtasSalesidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual PtasSales PtasSalesidValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessory { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuilding { get; set; }
    }
}
