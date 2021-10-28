using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCurrentuseapplication
    {
        public PtasCurrentuseapplication()
        {
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentusefarmyieldhistory = new HashSet<PtasCurrentusefarmyieldhistory>();
            PtasCurrentuselanduse = new HashSet<PtasCurrentuselanduse>();
            PtasCurrentusenote = new HashSet<PtasCurrentusenote>();
            PtasCurrentusepbrsresource = new HashSet<PtasCurrentusepbrsresource>();
        }

        public Guid PtasCurrentuseapplicationid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAcresappliedfor { get; set; }
        public DateTimeOffset? PtasAdditionalinformationrequestedon { get; set; }
        public string PtasAdditionalinformationrequired { get; set; }
        public DateTimeOffset? PtasApplicationdate { get; set; }
        public DateTimeOffset? PtasApprovaldenialdate { get; set; }
        public string PtasCouncilfile { get; set; }
        public int? PtasCurrentusetype { get; set; }
        public bool? PtasEntireparcels { get; set; }
        public decimal? PtasFaAcreagecrops { get; set; }
        public decimal? PtasFaAcreageemployeehousing { get; set; }
        public decimal? PtasFaAcreageequestrianuses { get; set; }
        public decimal? PtasFaAcreagefarmbuildingsgreenhouses { get; set; }
        public decimal? PtasFaAcreagegrazing { get; set; }
        public decimal? PtasFaAcreagehorticulture { get; set; }
        public decimal? PtasFaAcreagelivestock { get; set; }
        public decimal? PtasFaAcreageother { get; set; }
        public decimal? PtasFaAcreageresidence { get; set; }
        public decimal? PtasFaAcreagewoodlotareas { get; set; }
        public decimal? PtasFaAcresusedtogrowplantsincontainers { get; set; }
        public decimal? PtasFaAcresusedtogrowplantsintheground { get; set; }
        public decimal? PtasFaAcresusedtostorepurchasedplants { get; set; }
        public decimal? PtasFaAverageincomeperacre { get; set; }
        public decimal? PtasFaAverageincomeperacreBase { get; set; }
        public decimal? PtasFaAverageinvestmentperacre { get; set; }
        public decimal? PtasFaAverageinvestmentperacreBase { get; set; }
        public decimal? PtasFaAveragerentalfeeperacre { get; set; }
        public decimal? PtasFaAveragerentalfeeperacreBase { get; set; }
        public decimal? PtasFaCropsdry { get; set; }
        public decimal? PtasFaCropsirrigated { get; set; }
        public string PtasFaDescriptionofhorticulturalactivity { get; set; }
        public string PtasFaDescriptionofland { get; set; }
        public string PtasFaDescriptionofotheracres { get; set; }
        public string PtasFaDetailsoflandspresentuse { get; set; }
        public string PtasFaDetailsofotherpermitteduses { get; set; }
        public bool? PtasFaGrazinglandcultivated { get; set; }
        public bool? PtasFaHorticulturelessthanfiveacres { get; set; }
        public string PtasFaImprovementsontheparcels { get; set; }
        public bool? PtasFaLandpermittedforanyotheruse { get; set; }
        public decimal? PtasFaPercentofacreageforretail { get; set; }
        public decimal? PtasFaPercentoflandinpavement { get; set; }
        public bool? PtasFaPrimaryresidenceofowneronparcel { get; set; }
        public string PtasFaResidenceinvolvementinagriculturalop { get; set; }
        public bool? PtasFaSellingplantsfromanothergrower { get; set; }
        public decimal? PtasFaTotalacreage { get; set; }
        public string PtasFaTypesofcrops { get; set; }
        public string PtasFaTypesofequestrianuses { get; set; }
        public string PtasFaTypesoflivestock { get; set; }
        public bool? PtasFaWoodlotforgrazinglivestock { get; set; }
        public bool? PtasHomesiteapproved { get; set; }
        public bool? PtasHomesitedenied { get; set; }
        public string PtasName { get; set; }
        public bool? PtasOsApplicantisother { get; set; }
        public string PtasOsApplicantisotherdetails { get; set; }
        public bool? PtasOsApplicantispropertyowner { get; set; }
        public bool? PtasOsApplicantpurchasingthroughcontract { get; set; }
        public string PtasOsCurrentpublicuse { get; set; }
        public string PtasOsExistingimprovementsonparcels { get; set; }
        public string PtasOsFutureimprovementsonparcels { get; set; }
        public bool? PtasOsIncurrentuseprogram { get; set; }
        public bool? PtasOsLimitinglandagreements { get; set; }
        public bool? PtasOsLocatedinincorporatedcity { get; set; }
        public string PtasOsOtherpermitteduses { get; set; }
        public string PtasOsPresentandproposedlanduses { get; set; }
        public string PtasOsProposedpublicuse { get; set; }
        public string PtasOsResourcecategoryjustification { get; set; }
        public string PtasOsRoadpropertyisaccessedfrom { get; set; }
        public int? PtasOsTotalbonuspoints { get; set; }
        public int? PtasOsTotalpoints { get; set; }
        public int? PtasOsTotalstandardpoints { get; set; }
        public DateTimeOffset? PtasOwnernotifieddate { get; set; }
        public string PtasParcelsappliedfor { get; set; }
        public bool? PtasPortionofparcels { get; set; }
        public decimal? PtasProcessingfeereceived { get; set; }
        public decimal? PtasProcessingfeereceivedBase { get; set; }
        public DateTimeOffset? PtasProcessingfeereceiveddate { get; set; }
        public DateTimeOffset? PtasProcessingfeesenttotreasury { get; set; }
        public string PtasTfDescribeimprovementsontheparcels { get; set; }
        public string PtasTfDescriptionofland { get; set; }
        public string PtasTfDescriptionoftimbermanagementplan { get; set; }
        public string PtasTfDetailsofotherpermitteduses { get; set; }
        public bool? PtasTfExistingtimbermanagementplan { get; set; }
        public bool? PtasTfLandpermittedforanyotheruse { get; set; }
        public bool? PtasTfLandsubdividedorplatfiled { get; set; }
        public bool? PtasTfLandsubjecttoassessmentinrcw7604610 { get; set; }
        public bool? PtasTfLandusedforgrazing { get; set; }
        public decimal? PtasTfNumberofgrazingacres { get; set; }
        public string PtasTfParcelsmanagedassingleoperation { get; set; }
        public string PtasTfRelationshipofparcelowners { get; set; }
        public string PtasTfSummaryofcurrentandpastexperience { get; set; }
        public bool? PtasTfTitle76rcwcompliant { get; set; }
        public string PtasTfWhynotcomplianttitle76rcw { get; set; }
        public string PtasTfWhynotsubjecttoassessmentrcw7604610 { get; set; }
        public decimal? PtasTotalnumberofacres { get; set; }
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
        public Guid? PtasCurrentusegroupValue { get; set; }
        public Guid? PtasOsCityValue { get; set; }
        public Guid? PtasPrimaryapplicantValue { get; set; }
        public Guid? PtasTaxcodeareaValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCurrentusegroup PtasCurrentusegroupValueNavigation { get; set; }
        public virtual PtasJurisdiction PtasOsCityValueNavigation { get; set; }
        public virtual Contact PtasPrimaryapplicantValueNavigation { get; set; }
        public virtual PtasArea PtasTaxcodeareaValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcel { get; set; }
        public virtual ICollection<PtasCurrentusefarmyieldhistory> PtasCurrentusefarmyieldhistory { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduse { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenote { get; set; }
        public virtual ICollection<PtasCurrentusepbrsresource> PtasCurrentusepbrsresource { get; set; }
    }
}
