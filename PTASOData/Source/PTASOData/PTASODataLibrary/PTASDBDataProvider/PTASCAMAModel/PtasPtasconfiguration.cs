using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasconfiguration
    {
        public Guid PtasPtasconfigurationid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAssessorfirstname { get; set; }
        public string PtasAssessorlastname { get; set; }
        public string PtasAtc240lastbatchnumber { get; set; }
        public string PtasAtc240lastchangenumber { get; set; }
        public DateTimeOffset? PtasAtc240lastsenton { get; set; }
        public bool? PtasAtc240sendnotificationemail { get; set; }
        public string PtasAzureclientid { get; set; }
        public string PtasCustomsearchurl { get; set; }
        public string PtasEdmsstorageurl { get; set; }
        public string PtasInspectionreporturl { get; set; }
        public string PtasMediabloburl { get; set; }
        public int? PtasMincleansalepercentage { get; set; }
        public string PtasName { get; set; }
        public string PtasParcelmapurl { get; set; }
        public string PtasPortallist { get; set; }
        public DateTimeOffset? PtasPortalmaintenanceend { get; set; }
        public string PtasPortalmaintenancemessage { get; set; }
        public string PtasPortalurl { get; set; }
        public string PtasRedactionurl { get; set; }
        public int? PtasSedefaultwaitindays { get; set; }
        public bool? PtasSendsrexemptsyncemail { get; set; }
        public string PtasSketchtoolurl { get; set; }
        public string PtasSrexemptsynclog { get; set; }
        public string PtasStorageurl { get; set; }
        public string PtasWebserviceurl { get; set; }
        public string PtasWhatifpeerurl { get; set; }
        public string PtasWhatifwebservice { get; set; }
        public int? PtasYearsbeforerenewal { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OrganizationidValue { get; set; }
        public Guid? PtasDefaultsendfromidValue { get; set; }
        public Guid? PtasSendsrexemptsyncemailtoValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser PtasDefaultsendfromidValueNavigation { get; set; }
        public virtual Systemuser PtasSendsrexemptsyncemailtoValueNavigation { get; set; }
    }
}
