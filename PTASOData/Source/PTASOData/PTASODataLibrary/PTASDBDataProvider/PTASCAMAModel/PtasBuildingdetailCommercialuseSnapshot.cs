using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBuildingdetailCommercialuseSnapshot
    {
        [Key]
        public Guid PtasBuildingdetailCommercialuseid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAnchorstore { get; set; }
        public string PtasDescription { get; set; }
        public string PtasFloornumbertxt { get; set; }
        public int? PtasGrosssqft { get; set; }
        public string PtasHistguid { get; set; }
        public int? PtasHistyear { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasName { get; set; }
        public int? PtasNetsqft { get; set; }
        public int? PtasNumberofstories { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public int? PtasStoryheight { get; set; }
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
        public Guid? PtasBuildingsectionuseidValue { get; set; }
        public Guid? PtasMastersectionuseidValue { get; set; }
        public Guid? PtasProjectidValue { get; set; }
        public Guid? PtasSpecialtyareaidValue { get; set; }
        public Guid? PtasSpecialtynbhdidValue { get; set; }
        public Guid? PtasUnitidValue { get; set; }
    }
}
