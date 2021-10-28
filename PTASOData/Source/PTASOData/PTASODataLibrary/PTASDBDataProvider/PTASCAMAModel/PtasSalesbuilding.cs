using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSalesbuilding
    {
        public PtasSalesbuilding()
        {
            PtasSalesaccessory = new HashSet<PtasSalesaccessory>();
        }

        public Guid PtasSalesbuildingid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? Ptas12baths { get; set; }
        public int? Ptas1stflrSqft { get; set; }
        public int? Ptas2ndflrSqft { get; set; }
        public int? Ptas34baths { get; set; }
        public int? PtasAddlFireplace { get; set; }
        public decimal? PtasAddlFireplaceCost { get; set; }
        public decimal? PtasAddlFireplaceCostBase { get; set; }
        public int? PtasAttachedgarageSqft { get; set; }
        public int? PtasBasementgarageSqft { get; set; }
        public string PtasBasementgrade { get; set; }
        public int? PtasBedroomnbr { get; set; }
        public string PtasBrickStone { get; set; }
        public int? PtasBuilding { get; set; }
        public string PtasCondition { get; set; }
        public bool? PtasDaylightbasement { get; set; }
        public int? PtasDeckSqft { get; set; }
        public int? PtasFinbsmtSqft { get; set; }
        public int? PtasFrStdFireplace { get; set; }
        public int? PtasFullbathnbr { get; set; }
        public string PtasGrade { get; set; }
        public int? PtasHalfflrSqft { get; set; }
        public string PtasHeatsource { get; set; }
        public string PtasHeatsystem { get; set; }
        public int? PtasMultiFireplace { get; set; }
        public string PtasName { get; set; }
        public string PtasNbrfraction { get; set; }
        public string PtasNetcond { get; set; }
        public string PtasObsolescence { get; set; }
        public string PtasOpenEnclPorchSqft { get; set; }
        public int? PtasPercentcond { get; set; }
        public bool? PtasSalesaccessorycreated { get; set; }
        public int? PtasSingleFireplace { get; set; }
        public decimal? PtasStories { get; set; }
        public string PtasStreet { get; set; }
        public int? PtasStreetnbr { get; set; }
        public int? PtasTotalbsmtSqft { get; set; }
        public int? PtasTotallivingSqft { get; set; }
        public int? PtasUnfinishedFullSqft { get; set; }
        public int? PtasUnfinishedHalfSqft { get; set; }
        public string PtasUnitdescription { get; set; }
        public int? PtasUnits { get; set; }
        public int? PtasUpperflrSqft { get; set; }
        public string PtasVar { get; set; }
        public bool? PtasViewutil { get; set; }
        public string PtasZip { get; set; }
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
        public Guid? PtasBuildingdetailidValue { get; set; }
        public Guid? PtasSalesidValue { get; set; }
        public Guid? PtasSalesparcelidValue { get; set; }
        public Guid? PtasYearbuiltidValue { get; set; }
        public Guid? PtasYearrenovatedidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingdetailidValueNavigation { get; set; }
        public virtual PtasSales PtasSalesidValueNavigation { get; set; }
        public virtual PtasSalesparcel PtasSalesparcelidValueNavigation { get; set; }
        public virtual PtasYear PtasYearbuiltidValueNavigation { get; set; }
        public virtual PtasYear PtasYearrenovatedidValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessory { get; set; }
    }
}
