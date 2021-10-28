using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_ptasconfiguration
    {
        public Guid ptas_ptasconfigurationid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_assessorfirstname { get; set; }
        public string ptas_assessorlastname { get; set; }
        public string ptas_atc240lastbatchnumber { get; set; }
        public string ptas_atc240lastchangenumber { get; set; }
        public DateTimeOffset? ptas_atc240lastsenton { get; set; }
        public bool? ptas_atc240sendnotificationemail { get; set; }
        public string ptas_azureclientid { get; set; }
        public string ptas_customsearchurl { get; set; }
        public string ptas_edmsstorageurl { get; set; }
        public string ptas_inspectionreporturl { get; set; }
        public string ptas_mediabloburl { get; set; }
        public int? ptas_mincleansalepercentage { get; set; }
        public string ptas_name { get; set; }
        public string ptas_parcelmapurl { get; set; }
        public string ptas_portallist { get; set; }
        public DateTimeOffset? ptas_portalmaintenanceend { get; set; }
        public string ptas_portalmaintenancemessage { get; set; }
        public string ptas_portalurl { get; set; }
        public string ptas_redactionurl { get; set; }
        public int? ptas_sedefaultwaitindays { get; set; }
        public bool? ptas_sendsrexemptsyncemail { get; set; }
        public string ptas_sketchtoolurl { get; set; }
        public string ptas_srexemptsynclog { get; set; }
        public string ptas_storageurl { get; set; }
        public string ptas_webserviceurl { get; set; }
        public string ptas_whatifpeerurl { get; set; }
        public string ptas_whatifwebservice { get; set; }
        public int? ptas_yearsbeforerenewal { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _organizationid_value { get; set; }
        public Guid? _ptas_defaultsendfromid_value { get; set; }
        public Guid? _ptas_sendsrexemptsyncemailto_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _ptas_defaultsendfromid_valueNavigation { get; set; }
        public virtual systemuser _ptas_sendsrexemptsyncemailto_valueNavigation { get; set; }
    }
}
