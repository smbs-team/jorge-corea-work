using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTaxdistrict
    {
        public PtasTaxdistrict()
        {
            InversePtasFiredistrictidValueNavigation = new HashSet<PtasTaxdistrict>();
            InversePtasLibrarydistrictidValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasAnnexationtracker = new HashSet<PtasAnnexationtracker>();
            PtasFund = new HashSet<PtasFund>();
            PtasLevycode = new HashSet<PtasLevycode>();
            PtasLevycodechangePtasFromcemeteryidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromcitytownroadidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromemsidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromfireidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromfloodidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromhospitalidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromlibrarycapitalidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromlibraryidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromother1idValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromother2idValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromparkidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFrompiercecountylibrarysystemidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromregionalsoundtransitidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromschoolidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromseweridValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasFromwateridValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTocemeteryidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTocitytownroadidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToemsidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTofireidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTofloodidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTohospitalidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTolibrarycapitalidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTolibraryidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToother1idValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToother2idValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToparkidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTopiercecountylibrarysystemidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToregionalsoundtransitidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToschoolidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToseweridValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasTowateridValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevylidliftbond = new HashSet<PtasLevylidliftbond>();
            PtasLevyordinancerequest = new HashSet<PtasLevyordinancerequest>();
            PtasRatesheetdetail = new HashSet<PtasRatesheetdetail>();
            PtasZoning = new HashSet<PtasZoning>();
        }

        public Guid PtasTaxdistrictid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasAnnexedtolibrary { get; set; }
        public bool? PtasCountywidedistrict { get; set; }
        public bool? PtasIncludeconstitutionalcheck { get; set; }
        public bool? PtasIncludeincheck { get; set; }
        public int? PtasLevycharacteristics { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasOverridingvotedrate { get; set; }
        public bool? PtasPopulation10k { get; set; }
        public bool? PtasSenioreligible { get; set; }
        public bool? PtasSoilfeeexempt { get; set; }
        public decimal? PtasStatutorymaxrate { get; set; }
        public string PtasTaxdistrictidstring { get; set; }
        public int? PtasTaxdistricttype { get; set; }
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
        public Guid? PtasFireannexyearidValue { get; set; }
        public Guid? PtasFiredistrictidValue { get; set; }
        public Guid? PtasLeviedtaxyearidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasLibraryannexeffectiveyearidValue { get; set; }
        public Guid? PtasLibrarydistrictidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasFireannexyearidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFiredistrictidValueNavigation { get; set; }
        public virtual PtasYear PtasLeviedtaxyearidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasYear PtasLibraryannexeffectiveyearidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasLibrarydistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> InversePtasFiredistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> InversePtasLibrarydistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtracker { get; set; }
        public virtual ICollection<PtasFund> PtasFund { get; set; }
        public virtual ICollection<PtasLevycode> PtasLevycode { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromcemeteryidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromcitytownroadidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromemsidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromfireidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromfloodidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromhospitalidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromlibrarycapitalidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromlibraryidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromother1idValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromother2idValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromparkidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFrompiercecountylibrarysystemidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromregionalsoundtransitidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromschoolidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromseweridValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromwateridValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTocemeteryidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTocitytownroadidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToemsidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTofireidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTofloodidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTohospitalidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTolibrarycapitalidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTolibraryidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToother1idValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToother2idValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToparkidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTopiercecountylibrarysystemidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToregionalsoundtransitidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToschoolidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToseweridValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasTowateridValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbond { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequest { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetail { get; set; }
        public virtual ICollection<PtasZoning> PtasZoning { get; set; }
    }
}
