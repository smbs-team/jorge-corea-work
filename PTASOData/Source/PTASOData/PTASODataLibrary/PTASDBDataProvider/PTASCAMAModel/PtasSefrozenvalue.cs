using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSefrozenvalue
    {
        public PtasSefrozenvalue()
        {
            InversePtasExemptionremoval1idValueNavigation = new HashSet<PtasSefrozenvalue>();
            InversePtasExemptionremoval2idValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSeappdetail = new HashSet<PtasSeappdetail>();
        }

        public Guid PtasSefrozenvalueid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public int? PtasAlternatekey { get; set; }
        public int? PtasAtc001transaction { get; set; }
        public int? PtasAtc240transaction { get; set; }
        public string PtasAtcgeneratedlines { get; set; }
        public string PtasAutonumber { get; set; }
        public string PtasCheckdigit2 { get; set; }
        public string PtasCity2 { get; set; }
        public string PtasCountry2 { get; set; }
        public int? PtasCurrentexemptionlevelforatc1 { get; set; }
        public int? PtasCurrentexemptionlevelforatc2 { get; set; }
        public string PtasErrordetail { get; set; }
        public int? PtasFromexemptiontype { get; set; }
        public decimal? PtasFromimps { get; set; }
        public decimal? PtasFromimpsBase { get; set; }
        public decimal? PtasFromimpvalue1 { get; set; }
        public decimal? PtasFromimpvalue1Base { get; set; }
        public decimal? PtasFromimpvalue2 { get; set; }
        public decimal? PtasFromimpvalue2Base { get; set; }
        public decimal? PtasFromland { get; set; }
        public decimal? PtasFromlandBase { get; set; }
        public decimal? PtasFromlandvalue1 { get; set; }
        public decimal? PtasFromlandvalue1Base { get; set; }
        public decimal? PtasFromlandvalue2 { get; set; }
        public decimal? PtasFromlandvalue2Base { get; set; }
        public decimal? PtasFrozenimprovementvalue { get; set; }
        public decimal? PtasFrozenimprovementvalueBase { get; set; }
        public decimal? PtasFrozenlandvalue { get; set; }
        public decimal? PtasFrozenlandvalueBase { get; set; }
        public bool? PtasIspersonalproperty { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasName { get; set; }
        public string PtasNbrfraction2 { get; set; }
        public string PtasNextatc { get; set; }
        public string PtasOther { get; set; }
        public bool? PtasOverrideerrors { get; set; }
        public DateTimeOffset? PtasProcessedon { get; set; }
        public DateTimeOffset? PtasProcessedonMerge { get; set; }
        public DateTimeOffset? PtasProcessedonSplit { get; set; }
        public DateTimeOffset? PtasProcessedonStatuschange { get; set; }
        public DateTimeOffset? PtasProcessedonValuechange { get; set; }
        public string PtasProcessstatus { get; set; }
        public bool? PtasRecordqueued { get; set; }
        public int? PtasReprocessatc { get; set; }
        public bool? PtasSameaddress { get; set; }
        public int? PtasSplitcode2 { get; set; }
        public int? PtasSplitpercentage { get; set; }
        public string PtasSplitreason { get; set; }
        public string PtasStateprovince2 { get; set; }
        public string PtasStreetdirection2 { get; set; }
        public string PtasStreetname2 { get; set; }
        public string PtasStreetnumber2 { get; set; }
        public string PtasStreettype2 { get; set; }
        public bool? PtasSuppresstaxbill1 { get; set; }
        public bool? PtasSuppresstaxbill2 { get; set; }
        public string PtasTaxpayer2name { get; set; }
        public DateTimeOffset? PtasTerminationdate { get; set; }
        public int? PtasToexemptiontype { get; set; }
        public decimal? PtasToimpvalue2 { get; set; }
        public decimal? PtasToimpvalue2Base { get; set; }
        public decimal? PtasTolandvalue2 { get; set; }
        public decimal? PtasTolandvalue2Base { get; set; }
        public decimal? PtasTotalfrom { get; set; }
        public decimal? PtasTotalfromBase { get; set; }
        public decimal? PtasTotalto { get; set; }
        public decimal? PtasTotaltoBase { get; set; }
        public string PtasZippostal2 { get; set; }
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
        public Guid? PtasChangereasonidValue { get; set; }
        public Guid? PtasContactidValue { get; set; }
        public Guid? PtasExemptionremoval1idValue { get; set; }
        public Guid? PtasExemptionremoval2idValue { get; set; }
        public Guid? PtasFrozenyearidValue { get; set; }
        public Guid? PtasOriginationyearValue { get; set; }
        public Guid? PtasParcel2idValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasSeappdetailidValue { get; set; }
        public Guid? PtasSeapplicationidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasSeexemptionreason PtasChangereasonidValueNavigation { get; set; }
        public virtual Contact PtasContactidValueNavigation { get; set; }
        public virtual PtasSefrozenvalue PtasExemptionremoval1idValueNavigation { get; set; }
        public virtual PtasSefrozenvalue PtasExemptionremoval2idValueNavigation { get; set; }
        public virtual PtasYear PtasFrozenyearidValueNavigation { get; set; }
        public virtual PtasYear PtasOriginationyearValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcel2idValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasSeappdetail PtasSeappdetailidValueNavigation { get; set; }
        public virtual PtasSeapplication PtasSeapplicationidValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> InversePtasExemptionremoval1idValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> InversePtasExemptionremoval2idValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetail { get; set; }
    }
}
