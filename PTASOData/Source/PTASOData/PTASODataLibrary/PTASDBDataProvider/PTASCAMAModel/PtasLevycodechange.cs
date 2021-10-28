using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLevycodechange
    {
        public Guid PtasLevycodechangeid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasCreatecitytownlimit { get; set; }
        public bool? PtasCreatefirelimit { get; set; }
        public bool? PtasCreatelibrarylimit { get; set; }
        public string PtasName { get; set; }
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
        public Guid? PtasAnnexationtrackeridValue { get; set; }
        public Guid? PtasChangefromlevycodeidValue { get; set; }
        public Guid? PtasChangetolevycodeidValue { get; set; }
        public Guid? PtasFromcemeteryidValue { get; set; }
        public Guid? PtasFromcitytownroadidValue { get; set; }
        public Guid? PtasFromemsidValue { get; set; }
        public Guid? PtasFromfireidValue { get; set; }
        public Guid? PtasFromfloodidValue { get; set; }
        public Guid? PtasFromhospitalidValue { get; set; }
        public Guid? PtasFromlibrarycapitalidValue { get; set; }
        public Guid? PtasFromlibraryidValue { get; set; }
        public Guid? PtasFromother1idValue { get; set; }
        public Guid? PtasFromother2idValue { get; set; }
        public Guid? PtasFromparkidValue { get; set; }
        public Guid? PtasFrompiercecountylibrarysystemidValue { get; set; }
        public Guid? PtasFromregionalsoundtransitidValue { get; set; }
        public Guid? PtasFromschoolidValue { get; set; }
        public Guid? PtasFromseweridValue { get; set; }
        public Guid? PtasFromwateridValue { get; set; }
        public Guid? PtasFromyearidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasTocemeteryidValue { get; set; }
        public Guid? PtasTocitytownroadidValue { get; set; }
        public Guid? PtasToemsidValue { get; set; }
        public Guid? PtasTofireidValue { get; set; }
        public Guid? PtasTofloodidValue { get; set; }
        public Guid? PtasTohospitalidValue { get; set; }
        public Guid? PtasTolibrarycapitalidValue { get; set; }
        public Guid? PtasTolibraryidValue { get; set; }
        public Guid? PtasToother1idValue { get; set; }
        public Guid? PtasToother2idValue { get; set; }
        public Guid? PtasToparkidValue { get; set; }
        public Guid? PtasTopiercecountylibrarysystemidValue { get; set; }
        public Guid? PtasToregionalsoundtransitidValue { get; set; }
        public Guid? PtasToschoolidValue { get; set; }
        public Guid? PtasToseweridValue { get; set; }
        public Guid? PtasTowateridValue { get; set; }
        public Guid? PtasToyearidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAnnexationtracker PtasAnnexationtrackeridValueNavigation { get; set; }
        public virtual PtasLevycode PtasChangefromlevycodeidValueNavigation { get; set; }
        public virtual PtasLevycode PtasChangetolevycodeidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromcemeteryidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromcitytownroadidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromemsidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromfireidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromfloodidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromhospitalidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromlibrarycapitalidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromlibraryidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromother1idValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromother2idValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromparkidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFrompiercecountylibrarysystemidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromregionalsoundtransitidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromschoolidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromseweridValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFromwateridValueNavigation { get; set; }
        public virtual PtasYear PtasFromyearidValueNavigation { get; set; }
        public virtual PtasAnnexationtracker PtasParcelidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTocemeteryidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTocitytownroadidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasToemsidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTofireidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTofloodidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTohospitalidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTolibrarycapitalidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTolibraryidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasToother1idValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasToother2idValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasToparkidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTopiercecountylibrarysystemidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasToregionalsoundtransitidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasToschoolidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasToseweridValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTowateridValueNavigation { get; set; }
        public virtual PtasYear PtasToyearidValueNavigation { get; set; }
    }
}
