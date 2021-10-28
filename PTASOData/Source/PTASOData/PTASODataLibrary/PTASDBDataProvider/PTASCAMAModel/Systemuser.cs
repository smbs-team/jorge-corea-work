using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Systemuser
    {
        public Systemuser()
        {
            ContactCreatedbyValueNavigation = new HashSet<Contact>();
            ContactCreatedonbehalfbyValueNavigation = new HashSet<Contact>();
            ContactModifiedbyValueNavigation = new HashSet<Contact>();
            ContactModifiedonbehalfbyValueNavigation = new HashSet<Contact>();
            ContactOwninguserValueNavigation = new HashSet<Contact>();
            ContactPreferredsystemuseridValueNavigation = new HashSet<Contact>();
            InverseCreatedbyValueNavigation = new HashSet<Systemuser>();
            InverseCreatedonbehalfbyValueNavigation = new HashSet<Systemuser>();
            InverseModifiedbyValueNavigation = new HashSet<Systemuser>();
            InverseModifiedonbehalfbyValueNavigation = new HashSet<Systemuser>();
            InverseParentsystemuseridValueNavigation = new HashSet<Systemuser>();
            PtasAbstractdocumentCreatedbyValueNavigation = new HashSet<PtasAbstractdocument>();
            PtasAbstractdocumentCreatedonbehalfbyValueNavigation = new HashSet<PtasAbstractdocument>();
            PtasAbstractdocumentModifiedbyValueNavigation = new HashSet<PtasAbstractdocument>();
            PtasAbstractdocumentModifiedonbehalfbyValueNavigation = new HashSet<PtasAbstractdocument>();
            PtasAbstractdocumentOwninguserValueNavigation = new HashSet<PtasAbstractdocument>();
            PtasAbstractprojectCreatedbyValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectCreatedonbehalfbyValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectModifiedbyValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectModifiedonbehalfbyValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectOwninguserValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectPtasMppgMappingchangemadebyidValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectresultparcelCreatedbyValueNavigation = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAbstractprojectresultparcelCreatedonbehalfbyValueNavigation = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAbstractprojectresultparcelModifiedbyValueNavigation = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAbstractprojectresultparcelModifiedonbehalfbyValueNavigation = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAbstractprojectresultparcelOwninguserValueNavigation = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAbstractprojectsourceparcelCreatedbyValueNavigation = new HashSet<PtasAbstractprojectsourceparcel>();
            PtasAbstractprojectsourceparcelCreatedonbehalfbyValueNavigation = new HashSet<PtasAbstractprojectsourceparcel>();
            PtasAbstractprojectsourceparcelModifiedbyValueNavigation = new HashSet<PtasAbstractprojectsourceparcel>();
            PtasAbstractprojectsourceparcelModifiedonbehalfbyValueNavigation = new HashSet<PtasAbstractprojectsourceparcel>();
            PtasAbstractprojectsourceparcelOwninguserValueNavigation = new HashSet<PtasAbstractprojectsourceparcel>();
            PtasAccessorydetailCreatedbyValueNavigation = new HashSet<PtasAccessorydetail>();
            PtasAccessorydetailCreatedonbehalfbyValueNavigation = new HashSet<PtasAccessorydetail>();
            PtasAccessorydetailModifiedbyValueNavigation = new HashSet<PtasAccessorydetail>();
            PtasAccessorydetailModifiedonbehalfbyValueNavigation = new HashSet<PtasAccessorydetail>();
            PtasAccessorydetailOwninguserValueNavigation = new HashSet<PtasAccessorydetail>();
            PtasAddresschangehistoryCreatedbyValueNavigation = new HashSet<PtasAddresschangehistory>();
            PtasAddresschangehistoryCreatedonbehalfbyValueNavigation = new HashSet<PtasAddresschangehistory>();
            PtasAddresschangehistoryModifiedbyValueNavigation = new HashSet<PtasAddresschangehistory>();
            PtasAddresschangehistoryModifiedonbehalfbyValueNavigation = new HashSet<PtasAddresschangehistory>();
            PtasAddresschangehistoryOwninguserValueNavigation = new HashSet<PtasAddresschangehistory>();
            PtasAgriculturalusetypeCreatedbyValueNavigation = new HashSet<PtasAgriculturalusetype>();
            PtasAgriculturalusetypeCreatedonbehalfbyValueNavigation = new HashSet<PtasAgriculturalusetype>();
            PtasAgriculturalusetypeModifiedbyValueNavigation = new HashSet<PtasAgriculturalusetype>();
            PtasAgriculturalusetypeModifiedonbehalfbyValueNavigation = new HashSet<PtasAgriculturalusetype>();
            PtasAnnexationparcelreviewCreatedbyValueNavigation = new HashSet<PtasAnnexationparcelreview>();
            PtasAnnexationparcelreviewCreatedonbehalfbyValueNavigation = new HashSet<PtasAnnexationparcelreview>();
            PtasAnnexationparcelreviewModifiedbyValueNavigation = new HashSet<PtasAnnexationparcelreview>();
            PtasAnnexationparcelreviewModifiedonbehalfbyValueNavigation = new HashSet<PtasAnnexationparcelreview>();
            PtasAnnexationparcelreviewOwninguserValueNavigation = new HashSet<PtasAnnexationparcelreview>();
            PtasAnnexationtrackerCreatedbyValueNavigation = new HashSet<PtasAnnexationtracker>();
            PtasAnnexationtrackerCreatedonbehalfbyValueNavigation = new HashSet<PtasAnnexationtracker>();
            PtasAnnexationtrackerModifiedbyValueNavigation = new HashSet<PtasAnnexationtracker>();
            PtasAnnexationtrackerModifiedonbehalfbyValueNavigation = new HashSet<PtasAnnexationtracker>();
            PtasAnnexationtrackerOwninguserValueNavigation = new HashSet<PtasAnnexationtracker>();
            PtasAnnualcostdistributionCreatedbyValueNavigation = new HashSet<PtasAnnualcostdistribution>();
            PtasAnnualcostdistributionCreatedonbehalfbyValueNavigation = new HashSet<PtasAnnualcostdistribution>();
            PtasAnnualcostdistributionModifiedbyValueNavigation = new HashSet<PtasAnnualcostdistribution>();
            PtasAnnualcostdistributionModifiedonbehalfbyValueNavigation = new HashSet<PtasAnnualcostdistribution>();
            PtasAnnualcostdistributionOwninguserValueNavigation = new HashSet<PtasAnnualcostdistribution>();
            PtasApartmentregionCreatedbyValueNavigation = new HashSet<PtasApartmentregion>();
            PtasApartmentregionCreatedonbehalfbyValueNavigation = new HashSet<PtasApartmentregion>();
            PtasApartmentregionModifiedbyValueNavigation = new HashSet<PtasApartmentregion>();
            PtasApartmentregionModifiedonbehalfbyValueNavigation = new HashSet<PtasApartmentregion>();
            PtasApartmentsupergroupCreatedbyValueNavigation = new HashSet<PtasApartmentsupergroup>();
            PtasApartmentsupergroupCreatedonbehalfbyValueNavigation = new HashSet<PtasApartmentsupergroup>();
            PtasApartmentsupergroupModifiedbyValueNavigation = new HashSet<PtasApartmentsupergroup>();
            PtasApartmentsupergroupModifiedonbehalfbyValueNavigation = new HashSet<PtasApartmentsupergroup>();
            PtasAppealCreatedbyValueNavigation = new HashSet<PtasAppeal>();
            PtasAppealCreatedonbehalfbyValueNavigation = new HashSet<PtasAppeal>();
            PtasAppealModifiedbyValueNavigation = new HashSet<PtasAppeal>();
            PtasAppealModifiedonbehalfbyValueNavigation = new HashSet<PtasAppeal>();
            PtasAppealOwninguserValueNavigation = new HashSet<PtasAppeal>();
            PtasAptadjustedlevyrateCreatedbyValueNavigation = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptadjustedlevyrateCreatedonbehalfbyValueNavigation = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptadjustedlevyrateModifiedbyValueNavigation = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptadjustedlevyrateModifiedonbehalfbyValueNavigation = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptadjustedlevyrateOwninguserValueNavigation = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptavailablecomparablesaleCreatedbyValueNavigation = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptavailablecomparablesaleCreatedonbehalfbyValueNavigation = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptavailablecomparablesaleModifiedbyValueNavigation = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptavailablecomparablesaleModifiedonbehalfbyValueNavigation = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptavailablecomparablesaleOwninguserValueNavigation = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptbuildingqualityadjustmentCreatedbyValueNavigation = new HashSet<PtasAptbuildingqualityadjustment>();
            PtasAptbuildingqualityadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptbuildingqualityadjustment>();
            PtasAptbuildingqualityadjustmentModifiedbyValueNavigation = new HashSet<PtasAptbuildingqualityadjustment>();
            PtasAptbuildingqualityadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptbuildingqualityadjustment>();
            PtasAptbuildingqualityadjustmentOwninguserValueNavigation = new HashSet<PtasAptbuildingqualityadjustment>();
            PtasAptcloseproximityCreatedbyValueNavigation = new HashSet<PtasAptcloseproximity>();
            PtasAptcloseproximityCreatedonbehalfbyValueNavigation = new HashSet<PtasAptcloseproximity>();
            PtasAptcloseproximityModifiedbyValueNavigation = new HashSet<PtasAptcloseproximity>();
            PtasAptcloseproximityModifiedonbehalfbyValueNavigation = new HashSet<PtasAptcloseproximity>();
            PtasAptcommercialincomeexpenseCreatedbyValueNavigation = new HashSet<PtasAptcommercialincomeexpense>();
            PtasAptcommercialincomeexpenseCreatedonbehalfbyValueNavigation = new HashSet<PtasAptcommercialincomeexpense>();
            PtasAptcommercialincomeexpenseModifiedbyValueNavigation = new HashSet<PtasAptcommercialincomeexpense>();
            PtasAptcommercialincomeexpenseModifiedonbehalfbyValueNavigation = new HashSet<PtasAptcommercialincomeexpense>();
            PtasAptcommercialincomeexpenseOwninguserValueNavigation = new HashSet<PtasAptcommercialincomeexpense>();
            PtasAptcomparablerentCreatedbyValueNavigation = new HashSet<PtasAptcomparablerent>();
            PtasAptcomparablerentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptcomparablerent>();
            PtasAptcomparablerentModifiedbyValueNavigation = new HashSet<PtasAptcomparablerent>();
            PtasAptcomparablerentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptcomparablerent>();
            PtasAptcomparablerentOwninguserValueNavigation = new HashSet<PtasAptcomparablerent>();
            PtasAptcomparablesaleCreatedbyValueNavigation = new HashSet<PtasAptcomparablesale>();
            PtasAptcomparablesaleCreatedonbehalfbyValueNavigation = new HashSet<PtasAptcomparablesale>();
            PtasAptcomparablesaleModifiedbyValueNavigation = new HashSet<PtasAptcomparablesale>();
            PtasAptcomparablesaleModifiedonbehalfbyValueNavigation = new HashSet<PtasAptcomparablesale>();
            PtasAptcomparablesaleOwninguserValueNavigation = new HashSet<PtasAptcomparablesale>();
            PtasAptconditionadjustmentCreatedbyValueNavigation = new HashSet<PtasAptconditionadjustment>();
            PtasAptconditionadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptconditionadjustment>();
            PtasAptconditionadjustmentModifiedbyValueNavigation = new HashSet<PtasAptconditionadjustment>();
            PtasAptconditionadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptconditionadjustment>();
            PtasAptconditionadjustmentOwninguserValueNavigation = new HashSet<PtasAptconditionadjustment>();
            PtasAptestimatedunitsqftCreatedbyValueNavigation = new HashSet<PtasAptestimatedunitsqft>();
            PtasAptestimatedunitsqftCreatedonbehalfbyValueNavigation = new HashSet<PtasAptestimatedunitsqft>();
            PtasAptestimatedunitsqftModifiedbyValueNavigation = new HashSet<PtasAptestimatedunitsqft>();
            PtasAptestimatedunitsqftModifiedonbehalfbyValueNavigation = new HashSet<PtasAptestimatedunitsqft>();
            PtasAptestimatedunitsqftOwninguserValueNavigation = new HashSet<PtasAptestimatedunitsqft>();
            PtasAptexpensehighendCreatedbyValueNavigation = new HashSet<PtasAptexpensehighend>();
            PtasAptexpensehighendCreatedonbehalfbyValueNavigation = new HashSet<PtasAptexpensehighend>();
            PtasAptexpensehighendModifiedbyValueNavigation = new HashSet<PtasAptexpensehighend>();
            PtasAptexpensehighendModifiedonbehalfbyValueNavigation = new HashSet<PtasAptexpensehighend>();
            PtasAptexpensehighendOwninguserValueNavigation = new HashSet<PtasAptexpensehighend>();
            PtasAptexpenseunitsizeCreatedbyValueNavigation = new HashSet<PtasAptexpenseunitsize>();
            PtasAptexpenseunitsizeCreatedonbehalfbyValueNavigation = new HashSet<PtasAptexpenseunitsize>();
            PtasAptexpenseunitsizeModifiedbyValueNavigation = new HashSet<PtasAptexpenseunitsize>();
            PtasAptexpenseunitsizeModifiedonbehalfbyValueNavigation = new HashSet<PtasAptexpenseunitsize>();
            PtasAptexpenseunitsizeOwninguserValueNavigation = new HashSet<PtasAptexpenseunitsize>();
            PtasAptincomeexpenseCreatedbyValueNavigation = new HashSet<PtasAptincomeexpense>();
            PtasAptincomeexpenseCreatedonbehalfbyValueNavigation = new HashSet<PtasAptincomeexpense>();
            PtasAptincomeexpenseModifiedbyValueNavigation = new HashSet<PtasAptincomeexpense>();
            PtasAptincomeexpenseModifiedonbehalfbyValueNavigation = new HashSet<PtasAptincomeexpense>();
            PtasAptlistedrentCreatedbyValueNavigation = new HashSet<PtasAptlistedrent>();
            PtasAptlistedrentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptlistedrent>();
            PtasAptlistedrentModifiedbyValueNavigation = new HashSet<PtasAptlistedrent>();
            PtasAptlistedrentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptlistedrent>();
            PtasAptlistedrentOwninguserValueNavigation = new HashSet<PtasAptlistedrent>();
            PtasAptneighborhoodCreatedbyValueNavigation = new HashSet<PtasAptneighborhood>();
            PtasAptneighborhoodCreatedonbehalfbyValueNavigation = new HashSet<PtasAptneighborhood>();
            PtasAptneighborhoodModifiedbyValueNavigation = new HashSet<PtasAptneighborhood>();
            PtasAptneighborhoodModifiedonbehalfbyValueNavigation = new HashSet<PtasAptneighborhood>();
            PtasAptnumberofunitsadjustmentCreatedbyValueNavigation = new HashSet<PtasAptnumberofunitsadjustment>();
            PtasAptnumberofunitsadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptnumberofunitsadjustment>();
            PtasAptnumberofunitsadjustmentModifiedbyValueNavigation = new HashSet<PtasAptnumberofunitsadjustment>();
            PtasAptnumberofunitsadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptnumberofunitsadjustment>();
            PtasAptnumberofunitsadjustmentOwninguserValueNavigation = new HashSet<PtasAptnumberofunitsadjustment>();
            PtasAptpoolandelevatorexpenseCreatedbyValueNavigation = new HashSet<PtasAptpoolandelevatorexpense>();
            PtasAptpoolandelevatorexpenseCreatedonbehalfbyValueNavigation = new HashSet<PtasAptpoolandelevatorexpense>();
            PtasAptpoolandelevatorexpenseModifiedbyValueNavigation = new HashSet<PtasAptpoolandelevatorexpense>();
            PtasAptpoolandelevatorexpenseModifiedonbehalfbyValueNavigation = new HashSet<PtasAptpoolandelevatorexpense>();
            PtasAptpoolandelevatorexpenseOwninguserValueNavigation = new HashSet<PtasAptpoolandelevatorexpense>();
            PtasAptrentmodelCreatedbyValueNavigation = new HashSet<PtasAptrentmodel>();
            PtasAptrentmodelCreatedonbehalfbyValueNavigation = new HashSet<PtasAptrentmodel>();
            PtasAptrentmodelModifiedbyValueNavigation = new HashSet<PtasAptrentmodel>();
            PtasAptrentmodelModifiedonbehalfbyValueNavigation = new HashSet<PtasAptrentmodel>();
            PtasAptrentmodelOwninguserValueNavigation = new HashSet<PtasAptrentmodel>();
            PtasAptsalesmodelCreatedbyValueNavigation = new HashSet<PtasAptsalesmodel>();
            PtasAptsalesmodelCreatedonbehalfbyValueNavigation = new HashSet<PtasAptsalesmodel>();
            PtasAptsalesmodelModifiedbyValueNavigation = new HashSet<PtasAptsalesmodel>();
            PtasAptsalesmodelModifiedonbehalfbyValueNavigation = new HashSet<PtasAptsalesmodel>();
            PtasAptsalesmodelOwninguserValueNavigation = new HashSet<PtasAptsalesmodel>();
            PtasApttrendingCreatedbyValueNavigation = new HashSet<PtasApttrending>();
            PtasApttrendingCreatedonbehalfbyValueNavigation = new HashSet<PtasApttrending>();
            PtasApttrendingModifiedbyValueNavigation = new HashSet<PtasApttrending>();
            PtasApttrendingModifiedonbehalfbyValueNavigation = new HashSet<PtasApttrending>();
            PtasApttrendingOwninguserValueNavigation = new HashSet<PtasApttrending>();
            PtasAptunittypeadjustmentCreatedbyValueNavigation = new HashSet<PtasAptunittypeadjustment>();
            PtasAptunittypeadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptunittypeadjustment>();
            PtasAptunittypeadjustmentModifiedbyValueNavigation = new HashSet<PtasAptunittypeadjustment>();
            PtasAptunittypeadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptunittypeadjustment>();
            PtasAptunittypeadjustmentOwninguserValueNavigation = new HashSet<PtasAptunittypeadjustment>();
            PtasAptvaluationCreatedbyValueNavigation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationCreatedonbehalfbyValueNavigation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationModifiedbyValueNavigation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationModifiedonbehalfbyValueNavigation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationOwninguserValueNavigation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationPtasAppraiseridValueNavigation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationPtasUpdatedbyidValueNavigation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationprojectCreatedbyValueNavigation = new HashSet<PtasAptvaluationproject>();
            PtasAptvaluationprojectCreatedonbehalfbyValueNavigation = new HashSet<PtasAptvaluationproject>();
            PtasAptvaluationprojectModifiedbyValueNavigation = new HashSet<PtasAptvaluationproject>();
            PtasAptvaluationprojectModifiedonbehalfbyValueNavigation = new HashSet<PtasAptvaluationproject>();
            PtasAptvaluationprojectOwninguserValueNavigation = new HashSet<PtasAptvaluationproject>();
            PtasAptviewqualityadjustmentCreatedbyValueNavigation = new HashSet<PtasAptviewqualityadjustment>();
            PtasAptviewqualityadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptviewqualityadjustment>();
            PtasAptviewqualityadjustmentModifiedbyValueNavigation = new HashSet<PtasAptviewqualityadjustment>();
            PtasAptviewqualityadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptviewqualityadjustment>();
            PtasAptviewrankadjustmentCreatedbyValueNavigation = new HashSet<PtasAptviewrankadjustment>();
            PtasAptviewrankadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptviewrankadjustment>();
            PtasAptviewrankadjustmentModifiedbyValueNavigation = new HashSet<PtasAptviewrankadjustment>();
            PtasAptviewrankadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptviewrankadjustment>();
            PtasAptviewrankadjustmentOwninguserValueNavigation = new HashSet<PtasAptviewrankadjustment>();
            PtasAptviewtypeadjustmentCreatedbyValueNavigation = new HashSet<PtasAptviewtypeadjustment>();
            PtasAptviewtypeadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptviewtypeadjustment>();
            PtasAptviewtypeadjustmentModifiedbyValueNavigation = new HashSet<PtasAptviewtypeadjustment>();
            PtasAptviewtypeadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptviewtypeadjustment>();
            PtasAptyearbuiltadjustmentCreatedbyValueNavigation = new HashSet<PtasAptyearbuiltadjustment>();
            PtasAptyearbuiltadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasAptyearbuiltadjustment>();
            PtasAptyearbuiltadjustmentModifiedbyValueNavigation = new HashSet<PtasAptyearbuiltadjustment>();
            PtasAptyearbuiltadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasAptyearbuiltadjustment>();
            PtasAptyearbuiltadjustmentOwninguserValueNavigation = new HashSet<PtasAptyearbuiltadjustment>();
            PtasArcreasoncodeCreatedbyValueNavigation = new HashSet<PtasArcreasoncode>();
            PtasArcreasoncodeCreatedonbehalfbyValueNavigation = new HashSet<PtasArcreasoncode>();
            PtasArcreasoncodeModifiedbyValueNavigation = new HashSet<PtasArcreasoncode>();
            PtasArcreasoncodeModifiedonbehalfbyValueNavigation = new HashSet<PtasArcreasoncode>();
            PtasArcreasoncodeOwninguserValueNavigation = new HashSet<PtasArcreasoncode>();
            PtasAreaCreatedbyValueNavigation = new HashSet<PtasArea>();
            PtasAreaCreatedonbehalfbyValueNavigation = new HashSet<PtasArea>();
            PtasAreaModifiedbyValueNavigation = new HashSet<PtasArea>();
            PtasAreaModifiedonbehalfbyValueNavigation = new HashSet<PtasArea>();
            PtasAreaPtasAppraiseridValueNavigation = new HashSet<PtasArea>();
            PtasAreaPtasSeniorappraiserValueNavigation = new HashSet<PtasArea>();
            PtasAssessmentrollcorrectionCreatedbyValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasAssessmentrollcorrectionCreatedonbehalfbyValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasAssessmentrollcorrectionModifiedbyValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasAssessmentrollcorrectionModifiedonbehalfbyValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasAssessmentrollcorrectionOwninguserValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasAssessmentrollcorrectionPtasApprovalappraiseridValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasAssessmentrollcorrectionPtasPostedbyidValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasAssessmentrollcorrectionPtasResponsibleappraiseridValueNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasBillingclassificationCreatedbyValueNavigation = new HashSet<PtasBillingclassification>();
            PtasBillingclassificationCreatedonbehalfbyValueNavigation = new HashSet<PtasBillingclassification>();
            PtasBillingclassificationModifiedbyValueNavigation = new HashSet<PtasBillingclassification>();
            PtasBillingclassificationModifiedonbehalfbyValueNavigation = new HashSet<PtasBillingclassification>();
            PtasBillingclassificationOwninguserValueNavigation = new HashSet<PtasBillingclassification>();
            PtasBillingcodeCreatedbyValueNavigation = new HashSet<PtasBillingcode>();
            PtasBillingcodeCreatedonbehalfbyValueNavigation = new HashSet<PtasBillingcode>();
            PtasBillingcodeModifiedbyValueNavigation = new HashSet<PtasBillingcode>();
            PtasBillingcodeModifiedonbehalfbyValueNavigation = new HashSet<PtasBillingcode>();
            PtasBillingcodeOwninguserValueNavigation = new HashSet<PtasBillingcode>();
            PtasBookmarkCreatedbyValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkCreatedonbehalfbyValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkModifiedbyValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkModifiedonbehalfbyValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarkOwninguserValueNavigation = new HashSet<PtasBookmark>();
            PtasBookmarktagCreatedbyValueNavigation = new HashSet<PtasBookmarktag>();
            PtasBookmarktagCreatedonbehalfbyValueNavigation = new HashSet<PtasBookmarktag>();
            PtasBookmarktagModifiedbyValueNavigation = new HashSet<PtasBookmarktag>();
            PtasBookmarktagModifiedonbehalfbyValueNavigation = new HashSet<PtasBookmarktag>();
            PtasBookmarktagOwninguserValueNavigation = new HashSet<PtasBookmarktag>();
            PtasBuildingdetailCommercialuseCreatedbyValueNavigation = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasBuildingdetailCommercialuseCreatedonbehalfbyValueNavigation = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasBuildingdetailCommercialuseModifiedbyValueNavigation = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasBuildingdetailCommercialuseModifiedonbehalfbyValueNavigation = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasBuildingdetailCommercialuseOwninguserValueNavigation = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasBuildingdetailCreatedbyValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasBuildingdetailCreatedonbehalfbyValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasBuildingdetailModifiedbyValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasBuildingdetailModifiedonbehalfbyValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasBuildingdetailOwninguserValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasBuildingsectionfeatureCreatedbyValueNavigation = new HashSet<PtasBuildingsectionfeature>();
            PtasBuildingsectionfeatureCreatedonbehalfbyValueNavigation = new HashSet<PtasBuildingsectionfeature>();
            PtasBuildingsectionfeatureModifiedbyValueNavigation = new HashSet<PtasBuildingsectionfeature>();
            PtasBuildingsectionfeatureModifiedonbehalfbyValueNavigation = new HashSet<PtasBuildingsectionfeature>();
            PtasBuildingsectionfeatureOwninguserValueNavigation = new HashSet<PtasBuildingsectionfeature>();
            PtasBuildingsectionuseCreatedbyValueNavigation = new HashSet<PtasBuildingsectionuse>();
            PtasBuildingsectionuseCreatedonbehalfbyValueNavigation = new HashSet<PtasBuildingsectionuse>();
            PtasBuildingsectionuseModifiedbyValueNavigation = new HashSet<PtasBuildingsectionuse>();
            PtasBuildingsectionuseModifiedonbehalfbyValueNavigation = new HashSet<PtasBuildingsectionuse>();
            PtasCaprateCreatedbyValueNavigation = new HashSet<PtasCaprate>();
            PtasCaprateCreatedonbehalfbyValueNavigation = new HashSet<PtasCaprate>();
            PtasCaprateModifiedbyValueNavigation = new HashSet<PtasCaprate>();
            PtasCaprateModifiedonbehalfbyValueNavigation = new HashSet<PtasCaprate>();
            PtasChangereasonCreatedbyValueNavigation = new HashSet<PtasChangereason>();
            PtasChangereasonCreatedonbehalfbyValueNavigation = new HashSet<PtasChangereason>();
            PtasChangereasonModifiedbyValueNavigation = new HashSet<PtasChangereason>();
            PtasChangereasonModifiedonbehalfbyValueNavigation = new HashSet<PtasChangereason>();
            PtasChangereasonOwninguserValueNavigation = new HashSet<PtasChangereason>();
            PtasCityCreatedbyValueNavigation = new HashSet<PtasCity>();
            PtasCityCreatedonbehalfbyValueNavigation = new HashSet<PtasCity>();
            PtasCityModifiedbyValueNavigation = new HashSet<PtasCity>();
            PtasCityModifiedonbehalfbyValueNavigation = new HashSet<PtasCity>();
            PtasCondocomplexCreatedbyValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondocomplexCreatedonbehalfbyValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondocomplexModifiedbyValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondocomplexModifiedonbehalfbyValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondocomplexOwninguserValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondounitCreatedbyValueNavigation = new HashSet<PtasCondounit>();
            PtasCondounitCreatedonbehalfbyValueNavigation = new HashSet<PtasCondounit>();
            PtasCondounitModifiedbyValueNavigation = new HashSet<PtasCondounit>();
            PtasCondounitModifiedonbehalfbyValueNavigation = new HashSet<PtasCondounit>();
            PtasCondounitOwninguserValueNavigation = new HashSet<PtasCondounit>();
            PtasCondounitPtasSelectbyidValueNavigation = new HashSet<PtasCondounit>();
            PtasCondounitPtasUnitinspectedbyidValueNavigation = new HashSet<PtasCondounit>();
            PtasContaminatedlandreductionCreatedbyValueNavigation = new HashSet<PtasContaminatedlandreduction>();
            PtasContaminatedlandreductionCreatedonbehalfbyValueNavigation = new HashSet<PtasContaminatedlandreduction>();
            PtasContaminatedlandreductionModifiedbyValueNavigation = new HashSet<PtasContaminatedlandreduction>();
            PtasContaminatedlandreductionModifiedonbehalfbyValueNavigation = new HashSet<PtasContaminatedlandreduction>();
            PtasContaminatedlandreductionOwninguserValueNavigation = new HashSet<PtasContaminatedlandreduction>();
            PtasContaminationprojectCreatedbyValueNavigation = new HashSet<PtasContaminationproject>();
            PtasContaminationprojectCreatedonbehalfbyValueNavigation = new HashSet<PtasContaminationproject>();
            PtasContaminationprojectModifiedbyValueNavigation = new HashSet<PtasContaminationproject>();
            PtasContaminationprojectModifiedonbehalfbyValueNavigation = new HashSet<PtasContaminationproject>();
            PtasContaminationprojectOwninguserValueNavigation = new HashSet<PtasContaminationproject>();
            PtasCountryCreatedbyValueNavigation = new HashSet<PtasCountry>();
            PtasCountryCreatedonbehalfbyValueNavigation = new HashSet<PtasCountry>();
            PtasCountryModifiedbyValueNavigation = new HashSet<PtasCountry>();
            PtasCountryModifiedonbehalfbyValueNavigation = new HashSet<PtasCountry>();
            PtasCountyCreatedbyValueNavigation = new HashSet<PtasCounty>();
            PtasCountyCreatedonbehalfbyValueNavigation = new HashSet<PtasCounty>();
            PtasCountyModifiedbyValueNavigation = new HashSet<PtasCounty>();
            PtasCountyModifiedonbehalfbyValueNavigation = new HashSet<PtasCounty>();
            PtasCurrentuseapplicationCreatedbyValueNavigation = new HashSet<PtasCurrentuseapplication>();
            PtasCurrentuseapplicationCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentuseapplication>();
            PtasCurrentuseapplicationModifiedbyValueNavigation = new HashSet<PtasCurrentuseapplication>();
            PtasCurrentuseapplicationModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentuseapplication>();
            PtasCurrentuseapplicationOwninguserValueNavigation = new HashSet<PtasCurrentuseapplication>();
            PtasCurrentuseapplicationparcelCreatedbyValueNavigation = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentuseapplicationparcelCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentuseapplicationparcelModifiedbyValueNavigation = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentuseapplicationparcelModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentuseapplicationparcelOwninguserValueNavigation = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentusebacktaxstatementCreatedbyValueNavigation = new HashSet<PtasCurrentusebacktaxstatement>();
            PtasCurrentusebacktaxstatementCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusebacktaxstatement>();
            PtasCurrentusebacktaxstatementModifiedbyValueNavigation = new HashSet<PtasCurrentusebacktaxstatement>();
            PtasCurrentusebacktaxstatementModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusebacktaxstatement>();
            PtasCurrentusebacktaxstatementOwninguserValueNavigation = new HashSet<PtasCurrentusebacktaxstatement>();
            PtasCurrentusebacktaxyearCreatedbyValueNavigation = new HashSet<PtasCurrentusebacktaxyear>();
            PtasCurrentusebacktaxyearCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusebacktaxyear>();
            PtasCurrentusebacktaxyearModifiedbyValueNavigation = new HashSet<PtasCurrentusebacktaxyear>();
            PtasCurrentusebacktaxyearModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusebacktaxyear>();
            PtasCurrentusebacktaxyearOwninguserValueNavigation = new HashSet<PtasCurrentusebacktaxyear>();
            PtasCurrentusefarmyieldhistoryCreatedbyValueNavigation = new HashSet<PtasCurrentusefarmyieldhistory>();
            PtasCurrentusefarmyieldhistoryCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusefarmyieldhistory>();
            PtasCurrentusefarmyieldhistoryModifiedbyValueNavigation = new HashSet<PtasCurrentusefarmyieldhistory>();
            PtasCurrentusefarmyieldhistoryModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusefarmyieldhistory>();
            PtasCurrentusegroupCreatedbyValueNavigation = new HashSet<PtasCurrentusegroup>();
            PtasCurrentusegroupCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusegroup>();
            PtasCurrentusegroupModifiedbyValueNavigation = new HashSet<PtasCurrentusegroup>();
            PtasCurrentusegroupModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusegroup>();
            PtasCurrentuselanduseCreatedbyValueNavigation = new HashSet<PtasCurrentuselanduse>();
            PtasCurrentuselanduseCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentuselanduse>();
            PtasCurrentuselanduseModifiedbyValueNavigation = new HashSet<PtasCurrentuselanduse>();
            PtasCurrentuselanduseModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentuselanduse>();
            PtasCurrentusenoteCreatedbyValueNavigation = new HashSet<PtasCurrentusenote>();
            PtasCurrentusenoteCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusenote>();
            PtasCurrentusenoteModifiedbyValueNavigation = new HashSet<PtasCurrentusenote>();
            PtasCurrentusenoteModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusenote>();
            PtasCurrentusenoteOwninguserValueNavigation = new HashSet<PtasCurrentusenote>();
            PtasCurrentuseparcel2CreatedbyValueNavigation = new HashSet<PtasCurrentuseparcel2>();
            PtasCurrentuseparcel2CreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentuseparcel2>();
            PtasCurrentuseparcel2ModifiedbyValueNavigation = new HashSet<PtasCurrentuseparcel2>();
            PtasCurrentuseparcel2ModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentuseparcel2>();
            PtasCurrentuseparcel2OwninguserValueNavigation = new HashSet<PtasCurrentuseparcel2>();
            PtasCurrentusepbrsresourceCreatedbyValueNavigation = new HashSet<PtasCurrentusepbrsresource>();
            PtasCurrentusepbrsresourceCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusepbrsresource>();
            PtasCurrentusepbrsresourceModifiedbyValueNavigation = new HashSet<PtasCurrentusepbrsresource>();
            PtasCurrentusepbrsresourceModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusepbrsresource>();
            PtasCurrentusetaskCreatedbyValueNavigation = new HashSet<PtasCurrentusetask>();
            PtasCurrentusetaskCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusetask>();
            PtasCurrentusetaskModifiedbyValueNavigation = new HashSet<PtasCurrentusetask>();
            PtasCurrentusetaskModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusetask>();
            PtasCurrentusetaskOwninguserValueNavigation = new HashSet<PtasCurrentusetask>();
            PtasCurrentusevaluehistoryCreatedbyValueNavigation = new HashSet<PtasCurrentusevaluehistory>();
            PtasCurrentusevaluehistoryCreatedonbehalfbyValueNavigation = new HashSet<PtasCurrentusevaluehistory>();
            PtasCurrentusevaluehistoryModifiedbyValueNavigation = new HashSet<PtasCurrentusevaluehistory>();
            PtasCurrentusevaluehistoryModifiedonbehalfbyValueNavigation = new HashSet<PtasCurrentusevaluehistory>();
            PtasCurrentusevaluehistoryOwninguserValueNavigation = new HashSet<PtasCurrentusevaluehistory>();
            PtasDepreciationtableCreatedbyValueNavigation = new HashSet<PtasDepreciationtable>();
            PtasDepreciationtableCreatedonbehalfbyValueNavigation = new HashSet<PtasDepreciationtable>();
            PtasDepreciationtableModifiedbyValueNavigation = new HashSet<PtasDepreciationtable>();
            PtasDepreciationtableModifiedonbehalfbyValueNavigation = new HashSet<PtasDepreciationtable>();
            PtasDepreciationtableOwninguserValueNavigation = new HashSet<PtasDepreciationtable>();
            PtasDesignationtypeCreatedbyValueNavigation = new HashSet<PtasDesignationtype>();
            PtasDesignationtypeCreatedonbehalfbyValueNavigation = new HashSet<PtasDesignationtype>();
            PtasDesignationtypeModifiedbyValueNavigation = new HashSet<PtasDesignationtype>();
            PtasDesignationtypeModifiedonbehalfbyValueNavigation = new HashSet<PtasDesignationtype>();
            PtasDesignationtypeOwninguserValueNavigation = new HashSet<PtasDesignationtype>();
            PtasDistrictCreatedbyValueNavigation = new HashSet<PtasDistrict>();
            PtasDistrictCreatedonbehalfbyValueNavigation = new HashSet<PtasDistrict>();
            PtasDistrictModifiedbyValueNavigation = new HashSet<PtasDistrict>();
            PtasDistrictModifiedonbehalfbyValueNavigation = new HashSet<PtasDistrict>();
            PtasEbscostcenterCreatedbyValueNavigation = new HashSet<PtasEbscostcenter>();
            PtasEbscostcenterCreatedonbehalfbyValueNavigation = new HashSet<PtasEbscostcenter>();
            PtasEbscostcenterModifiedbyValueNavigation = new HashSet<PtasEbscostcenter>();
            PtasEbscostcenterModifiedonbehalfbyValueNavigation = new HashSet<PtasEbscostcenter>();
            PtasEbsfundnumberCreatedbyValueNavigation = new HashSet<PtasEbsfundnumber>();
            PtasEbsfundnumberCreatedonbehalfbyValueNavigation = new HashSet<PtasEbsfundnumber>();
            PtasEbsfundnumberModifiedbyValueNavigation = new HashSet<PtasEbsfundnumber>();
            PtasEbsfundnumberModifiedonbehalfbyValueNavigation = new HashSet<PtasEbsfundnumber>();
            PtasEbsmainaccountCreatedbyValueNavigation = new HashSet<PtasEbsmainaccount>();
            PtasEbsmainaccountCreatedonbehalfbyValueNavigation = new HashSet<PtasEbsmainaccount>();
            PtasEbsmainaccountModifiedbyValueNavigation = new HashSet<PtasEbsmainaccount>();
            PtasEbsmainaccountModifiedonbehalfbyValueNavigation = new HashSet<PtasEbsmainaccount>();
            PtasEbsprojectCreatedbyValueNavigation = new HashSet<PtasEbsproject>();
            PtasEbsprojectCreatedonbehalfbyValueNavigation = new HashSet<PtasEbsproject>();
            PtasEbsprojectModifiedbyValueNavigation = new HashSet<PtasEbsproject>();
            PtasEbsprojectModifiedonbehalfbyValueNavigation = new HashSet<PtasEbsproject>();
            PtasEconomicunitCreatedbyValueNavigation = new HashSet<PtasEconomicunit>();
            PtasEconomicunitCreatedonbehalfbyValueNavigation = new HashSet<PtasEconomicunit>();
            PtasEconomicunitModifiedbyValueNavigation = new HashSet<PtasEconomicunit>();
            PtasEconomicunitModifiedonbehalfbyValueNavigation = new HashSet<PtasEconomicunit>();
            PtasEconomicunitOwninguserValueNavigation = new HashSet<PtasEconomicunit>();
            PtasEnvironmentalrestrictionCreatedbyValueNavigation = new HashSet<PtasEnvironmentalrestriction>();
            PtasEnvironmentalrestrictionCreatedonbehalfbyValueNavigation = new HashSet<PtasEnvironmentalrestriction>();
            PtasEnvironmentalrestrictionModifiedbyValueNavigation = new HashSet<PtasEnvironmentalrestriction>();
            PtasEnvironmentalrestrictionModifiedonbehalfbyValueNavigation = new HashSet<PtasEnvironmentalrestriction>();
            PtasEnvironmentalrestrictionOwninguserValueNavigation = new HashSet<PtasEnvironmentalrestriction>();
            PtasEnvironmentalrestrictiontypeCreatedbyValueNavigation = new HashSet<PtasEnvironmentalrestrictiontype>();
            PtasEnvironmentalrestrictiontypeCreatedonbehalfbyValueNavigation = new HashSet<PtasEnvironmentalrestrictiontype>();
            PtasEnvironmentalrestrictiontypeModifiedbyValueNavigation = new HashSet<PtasEnvironmentalrestrictiontype>();
            PtasEnvironmentalrestrictiontypeModifiedonbehalfbyValueNavigation = new HashSet<PtasEnvironmentalrestrictiontype>();
            PtasExemptionCreatedbyValueNavigation = new HashSet<PtasExemption>();
            PtasExemptionCreatedonbehalfbyValueNavigation = new HashSet<PtasExemption>();
            PtasExemptionModifiedbyValueNavigation = new HashSet<PtasExemption>();
            PtasExemptionModifiedonbehalfbyValueNavigation = new HashSet<PtasExemption>();
            PtasExemptionOwninguserValueNavigation = new HashSet<PtasExemption>();
            PtasFileattachmentmetadataCreatedbyValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFileattachmentmetadataCreatedonbehalfbyValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFileattachmentmetadataModifiedbyValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFileattachmentmetadataModifiedonbehalfbyValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFileattachmentmetadataOwninguserValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFileattachmentmetadataPtasLoadbyidValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFundCreatedbyValueNavigation = new HashSet<PtasFund>();
            PtasFundCreatedonbehalfbyValueNavigation = new HashSet<PtasFund>();
            PtasFundModifiedbyValueNavigation = new HashSet<PtasFund>();
            PtasFundModifiedonbehalfbyValueNavigation = new HashSet<PtasFund>();
            PtasFundOwninguserValueNavigation = new HashSet<PtasFund>();
            PtasFundallocationCreatedbyValueNavigation = new HashSet<PtasFundallocation>();
            PtasFundallocationCreatedonbehalfbyValueNavigation = new HashSet<PtasFundallocation>();
            PtasFundallocationModifiedbyValueNavigation = new HashSet<PtasFundallocation>();
            PtasFundallocationModifiedonbehalfbyValueNavigation = new HashSet<PtasFundallocation>();
            PtasFundallocationOwninguserValueNavigation = new HashSet<PtasFundallocation>();
            PtasFundfactordetailCreatedbyValueNavigation = new HashSet<PtasFundfactordetail>();
            PtasFundfactordetailCreatedonbehalfbyValueNavigation = new HashSet<PtasFundfactordetail>();
            PtasFundfactordetailModifiedbyValueNavigation = new HashSet<PtasFundfactordetail>();
            PtasFundfactordetailModifiedonbehalfbyValueNavigation = new HashSet<PtasFundfactordetail>();
            PtasFundfactordetailOwninguserValueNavigation = new HashSet<PtasFundfactordetail>();
            PtasFundtypeCreatedbyValueNavigation = new HashSet<PtasFundtype>();
            PtasFundtypeCreatedonbehalfbyValueNavigation = new HashSet<PtasFundtype>();
            PtasFundtypeModifiedbyValueNavigation = new HashSet<PtasFundtype>();
            PtasFundtypeModifiedonbehalfbyValueNavigation = new HashSet<PtasFundtype>();
            PtasFundtypeOwninguserValueNavigation = new HashSet<PtasFundtype>();
            PtasGeoareaCreatedbyValueNavigation = new HashSet<PtasGeoarea>();
            PtasGeoareaCreatedonbehalfbyValueNavigation = new HashSet<PtasGeoarea>();
            PtasGeoareaModifiedbyValueNavigation = new HashSet<PtasGeoarea>();
            PtasGeoareaModifiedonbehalfbyValueNavigation = new HashSet<PtasGeoarea>();
            PtasGeoareaPtasAppraiseridValueNavigation = new HashSet<PtasGeoarea>();
            PtasGeoareaPtasSeniorappraiseridValueNavigation = new HashSet<PtasGeoarea>();
            PtasGeoneighborhoodCreatedbyValueNavigation = new HashSet<PtasGeoneighborhood>();
            PtasGeoneighborhoodCreatedonbehalfbyValueNavigation = new HashSet<PtasGeoneighborhood>();
            PtasGeoneighborhoodModifiedbyValueNavigation = new HashSet<PtasGeoneighborhood>();
            PtasGeoneighborhoodModifiedonbehalfbyValueNavigation = new HashSet<PtasGeoneighborhood>();
            PtasGovtaxpayernameCreatedbyValueNavigation = new HashSet<PtasGovtaxpayername>();
            PtasGovtaxpayernameCreatedonbehalfbyValueNavigation = new HashSet<PtasGovtaxpayername>();
            PtasGovtaxpayernameModifiedbyValueNavigation = new HashSet<PtasGovtaxpayername>();
            PtasGovtaxpayernameModifiedonbehalfbyValueNavigation = new HashSet<PtasGovtaxpayername>();
            PtasGradestratificationmappingCreatedbyValueNavigation = new HashSet<PtasGradestratificationmapping>();
            PtasGradestratificationmappingCreatedonbehalfbyValueNavigation = new HashSet<PtasGradestratificationmapping>();
            PtasGradestratificationmappingModifiedbyValueNavigation = new HashSet<PtasGradestratificationmapping>();
            PtasGradestratificationmappingModifiedonbehalfbyValueNavigation = new HashSet<PtasGradestratificationmapping>();
            PtasHomeimprovementCreatedbyValueNavigation = new HashSet<PtasHomeimprovement>();
            PtasHomeimprovementCreatedonbehalfbyValueNavigation = new HashSet<PtasHomeimprovement>();
            PtasHomeimprovementModifiedbyValueNavigation = new HashSet<PtasHomeimprovement>();
            PtasHomeimprovementModifiedonbehalfbyValueNavigation = new HashSet<PtasHomeimprovement>();
            PtasHomeimprovementOwninguserValueNavigation = new HashSet<PtasHomeimprovement>();
            PtasHousingprogramCreatedbyValueNavigation = new HashSet<PtasHousingprogram>();
            PtasHousingprogramCreatedonbehalfbyValueNavigation = new HashSet<PtasHousingprogram>();
            PtasHousingprogramModifiedbyValueNavigation = new HashSet<PtasHousingprogram>();
            PtasHousingprogramModifiedonbehalfbyValueNavigation = new HashSet<PtasHousingprogram>();
            PtasIndustryCreatedbyValueNavigation = new HashSet<PtasIndustry>();
            PtasIndustryCreatedonbehalfbyValueNavigation = new HashSet<PtasIndustry>();
            PtasIndustryModifiedbyValueNavigation = new HashSet<PtasIndustry>();
            PtasIndustryModifiedonbehalfbyValueNavigation = new HashSet<PtasIndustry>();
            PtasIndustryOwninguserValueNavigation = new HashSet<PtasIndustry>();
            PtasInspectionhistoryCreatedbyValueNavigation = new HashSet<PtasInspectionhistory>();
            PtasInspectionhistoryCreatedonbehalfbyValueNavigation = new HashSet<PtasInspectionhistory>();
            PtasInspectionhistoryModifiedbyValueNavigation = new HashSet<PtasInspectionhistory>();
            PtasInspectionhistoryModifiedonbehalfbyValueNavigation = new HashSet<PtasInspectionhistory>();
            PtasInspectionhistoryOwninguserValueNavigation = new HashSet<PtasInspectionhistory>();
            PtasInspectionhistoryPtasInspectedbyidValueNavigation = new HashSet<PtasInspectionhistory>();
            PtasInspectionyearCreatedbyValueNavigation = new HashSet<PtasInspectionyear>();
            PtasInspectionyearCreatedonbehalfbyValueNavigation = new HashSet<PtasInspectionyear>();
            PtasInspectionyearModifiedbyValueNavigation = new HashSet<PtasInspectionyear>();
            PtasInspectionyearModifiedonbehalfbyValueNavigation = new HashSet<PtasInspectionyear>();
            PtasInspectionyearOwninguserValueNavigation = new HashSet<PtasInspectionyear>();
            PtasJurisdictionCreatedbyValueNavigation = new HashSet<PtasJurisdiction>();
            PtasJurisdictionCreatedonbehalfbyValueNavigation = new HashSet<PtasJurisdiction>();
            PtasJurisdictionModifiedbyValueNavigation = new HashSet<PtasJurisdiction>();
            PtasJurisdictionModifiedonbehalfbyValueNavigation = new HashSet<PtasJurisdiction>();
            PtasJurisdictioncontactCreatedbyValueNavigation = new HashSet<PtasJurisdictioncontact>();
            PtasJurisdictioncontactCreatedonbehalfbyValueNavigation = new HashSet<PtasJurisdictioncontact>();
            PtasJurisdictioncontactModifiedbyValueNavigation = new HashSet<PtasJurisdictioncontact>();
            PtasJurisdictioncontactModifiedonbehalfbyValueNavigation = new HashSet<PtasJurisdictioncontact>();
            PtasJurisdictioncontactOwninguserValueNavigation = new HashSet<PtasJurisdictioncontact>();
            PtasLandCreatedbyValueNavigation = new HashSet<PtasLand>();
            PtasLandCreatedonbehalfbyValueNavigation = new HashSet<PtasLand>();
            PtasLandModifiedbyValueNavigation = new HashSet<PtasLand>();
            PtasLandModifiedonbehalfbyValueNavigation = new HashSet<PtasLand>();
            PtasLandOwninguserValueNavigation = new HashSet<PtasLand>();
            PtasLanduseCreatedbyValueNavigation = new HashSet<PtasLanduse>();
            PtasLanduseCreatedonbehalfbyValueNavigation = new HashSet<PtasLanduse>();
            PtasLanduseModifiedbyValueNavigation = new HashSet<PtasLanduse>();
            PtasLanduseModifiedonbehalfbyValueNavigation = new HashSet<PtasLanduse>();
            PtasLandvaluebreakdownCreatedbyValueNavigation = new HashSet<PtasLandvaluebreakdown>();
            PtasLandvaluebreakdownCreatedonbehalfbyValueNavigation = new HashSet<PtasLandvaluebreakdown>();
            PtasLandvaluebreakdownModifiedbyValueNavigation = new HashSet<PtasLandvaluebreakdown>();
            PtasLandvaluebreakdownModifiedonbehalfbyValueNavigation = new HashSet<PtasLandvaluebreakdown>();
            PtasLandvaluebreakdownOwninguserValueNavigation = new HashSet<PtasLandvaluebreakdown>();
            PtasLandvaluecalculationCreatedbyValueNavigation = new HashSet<PtasLandvaluecalculation>();
            PtasLandvaluecalculationCreatedonbehalfbyValueNavigation = new HashSet<PtasLandvaluecalculation>();
            PtasLandvaluecalculationModifiedbyValueNavigation = new HashSet<PtasLandvaluecalculation>();
            PtasLandvaluecalculationModifiedonbehalfbyValueNavigation = new HashSet<PtasLandvaluecalculation>();
            PtasLandvaluecalculationOwninguserValueNavigation = new HashSet<PtasLandvaluecalculation>();
            PtasLevycodeCreatedbyValueNavigation = new HashSet<PtasLevycode>();
            PtasLevycodeCreatedonbehalfbyValueNavigation = new HashSet<PtasLevycode>();
            PtasLevycodeModifiedbyValueNavigation = new HashSet<PtasLevycode>();
            PtasLevycodeModifiedonbehalfbyValueNavigation = new HashSet<PtasLevycode>();
            PtasLevycodechangeCreatedbyValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangeCreatedonbehalfbyValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangeModifiedbyValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangeModifiedonbehalfbyValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangeOwninguserValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevylidliftbondCreatedbyValueNavigation = new HashSet<PtasLevylidliftbond>();
            PtasLevylidliftbondCreatedonbehalfbyValueNavigation = new HashSet<PtasLevylidliftbond>();
            PtasLevylidliftbondModifiedbyValueNavigation = new HashSet<PtasLevylidliftbond>();
            PtasLevylidliftbondModifiedonbehalfbyValueNavigation = new HashSet<PtasLevylidliftbond>();
            PtasLevylidliftbondOwninguserValueNavigation = new HashSet<PtasLevylidliftbond>();
            PtasLevyordinancerequestCreatedbyValueNavigation = new HashSet<PtasLevyordinancerequest>();
            PtasLevyordinancerequestCreatedonbehalfbyValueNavigation = new HashSet<PtasLevyordinancerequest>();
            PtasLevyordinancerequestModifiedbyValueNavigation = new HashSet<PtasLevyordinancerequest>();
            PtasLevyordinancerequestModifiedonbehalfbyValueNavigation = new HashSet<PtasLevyordinancerequest>();
            PtasLevyordinancerequestOwninguserValueNavigation = new HashSet<PtasLevyordinancerequest>();
            PtasLowincomehousingprogramCreatedbyValueNavigation = new HashSet<PtasLowincomehousingprogram>();
            PtasLowincomehousingprogramCreatedonbehalfbyValueNavigation = new HashSet<PtasLowincomehousingprogram>();
            PtasLowincomehousingprogramModifiedbyValueNavigation = new HashSet<PtasLowincomehousingprogram>();
            PtasLowincomehousingprogramModifiedonbehalfbyValueNavigation = new HashSet<PtasLowincomehousingprogram>();
            PtasLowincomehousingprogramOwninguserValueNavigation = new HashSet<PtasLowincomehousingprogram>();
            PtasMajornumberdetailCreatedbyValueNavigation = new HashSet<PtasMajornumberdetail>();
            PtasMajornumberdetailCreatedonbehalfbyValueNavigation = new HashSet<PtasMajornumberdetail>();
            PtasMajornumberdetailModifiedbyValueNavigation = new HashSet<PtasMajornumberdetail>();
            PtasMajornumberdetailModifiedonbehalfbyValueNavigation = new HashSet<PtasMajornumberdetail>();
            PtasMajornumberdetailOwninguserValueNavigation = new HashSet<PtasMajornumberdetail>();
            PtasMajornumberindexCreatedbyValueNavigation = new HashSet<PtasMajornumberindex>();
            PtasMajornumberindexCreatedonbehalfbyValueNavigation = new HashSet<PtasMajornumberindex>();
            PtasMajornumberindexModifiedbyValueNavigation = new HashSet<PtasMajornumberindex>();
            PtasMajornumberindexModifiedonbehalfbyValueNavigation = new HashSet<PtasMajornumberindex>();
            PtasMasspayaccumulatorCreatedbyValueNavigation = new HashSet<PtasMasspayaccumulator>();
            PtasMasspayaccumulatorCreatedonbehalfbyValueNavigation = new HashSet<PtasMasspayaccumulator>();
            PtasMasspayaccumulatorModifiedbyValueNavigation = new HashSet<PtasMasspayaccumulator>();
            PtasMasspayaccumulatorModifiedonbehalfbyValueNavigation = new HashSet<PtasMasspayaccumulator>();
            PtasMasspayaccumulatorOwninguserValueNavigation = new HashSet<PtasMasspayaccumulator>();
            PtasMasspayactionCreatedbyValueNavigation = new HashSet<PtasMasspayaction>();
            PtasMasspayactionCreatedonbehalfbyValueNavigation = new HashSet<PtasMasspayaction>();
            PtasMasspayactionModifiedbyValueNavigation = new HashSet<PtasMasspayaction>();
            PtasMasspayactionModifiedonbehalfbyValueNavigation = new HashSet<PtasMasspayaction>();
            PtasMasspayactionOwninguserValueNavigation = new HashSet<PtasMasspayaction>();
            PtasMasspayerCreatedbyValueNavigation = new HashSet<PtasMasspayer>();
            PtasMasspayerCreatedonbehalfbyValueNavigation = new HashSet<PtasMasspayer>();
            PtasMasspayerModifiedbyValueNavigation = new HashSet<PtasMasspayer>();
            PtasMasspayerModifiedonbehalfbyValueNavigation = new HashSet<PtasMasspayer>();
            PtasMasspayerOwninguserValueNavigation = new HashSet<PtasMasspayer>();
            PtasMediarepositoryCreatedbyValueNavigation = new HashSet<PtasMediarepository>();
            PtasMediarepositoryCreatedonbehalfbyValueNavigation = new HashSet<PtasMediarepository>();
            PtasMediarepositoryModifiedbyValueNavigation = new HashSet<PtasMediarepository>();
            PtasMediarepositoryModifiedonbehalfbyValueNavigation = new HashSet<PtasMediarepository>();
            PtasMediarepositoryOwninguserValueNavigation = new HashSet<PtasMediarepository>();
            PtasMedicareplanCreatedbyValueNavigation = new HashSet<PtasMedicareplan>();
            PtasMedicareplanCreatedonbehalfbyValueNavigation = new HashSet<PtasMedicareplan>();
            PtasMedicareplanModifiedbyValueNavigation = new HashSet<PtasMedicareplan>();
            PtasMedicareplanModifiedonbehalfbyValueNavigation = new HashSet<PtasMedicareplan>();
            PtasNaicscodeCreatedbyValueNavigation = new HashSet<PtasNaicscode>();
            PtasNaicscodeCreatedonbehalfbyValueNavigation = new HashSet<PtasNaicscode>();
            PtasNaicscodeModifiedbyValueNavigation = new HashSet<PtasNaicscode>();
            PtasNaicscodeModifiedonbehalfbyValueNavigation = new HashSet<PtasNaicscode>();
            PtasNeighborhoodCreatedbyValueNavigation = new HashSet<PtasNeighborhood>();
            PtasNeighborhoodCreatedonbehalfbyValueNavigation = new HashSet<PtasNeighborhood>();
            PtasNeighborhoodModifiedbyValueNavigation = new HashSet<PtasNeighborhood>();
            PtasNeighborhoodModifiedonbehalfbyValueNavigation = new HashSet<PtasNeighborhood>();
            PtasNotificationconfigurationCreatedbyValueNavigation = new HashSet<PtasNotificationconfiguration>();
            PtasNotificationconfigurationCreatedonbehalfbyValueNavigation = new HashSet<PtasNotificationconfiguration>();
            PtasNotificationconfigurationModifiedbyValueNavigation = new HashSet<PtasNotificationconfiguration>();
            PtasNotificationconfigurationModifiedonbehalfbyValueNavigation = new HashSet<PtasNotificationconfiguration>();
            PtasNotificationconfigurationOwninguserValueNavigation = new HashSet<PtasNotificationconfiguration>();
            PtasNuisancetypeCreatedbyValueNavigation = new HashSet<PtasNuisancetype>();
            PtasNuisancetypeCreatedonbehalfbyValueNavigation = new HashSet<PtasNuisancetype>();
            PtasNuisancetypeModifiedbyValueNavigation = new HashSet<PtasNuisancetype>();
            PtasNuisancetypeModifiedonbehalfbyValueNavigation = new HashSet<PtasNuisancetype>();
            PtasNuisancetypeOwninguserValueNavigation = new HashSet<PtasNuisancetype>();
            PtasOmitCreatedbyValueNavigation = new HashSet<PtasOmit>();
            PtasOmitCreatedonbehalfbyValueNavigation = new HashSet<PtasOmit>();
            PtasOmitModifiedbyValueNavigation = new HashSet<PtasOmit>();
            PtasOmitModifiedonbehalfbyValueNavigation = new HashSet<PtasOmit>();
            PtasOmitOwninguserValueNavigation = new HashSet<PtasOmit>();
            PtasParceldetailCreatedbyValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailCreatedonbehalfbyValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailModifiedbyValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailModifiedonbehalfbyValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailOwninguserValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailPtasAssignedappraiseridValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailPtasLandinspectedbyidValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailPtasParcelinspectedbyidValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceldetailPtasSpecialtyappraiseridValueNavigation = new HashSet<PtasParceldetail>();
            PtasParceleconomicunitCreatedbyValueNavigation = new HashSet<PtasParceleconomicunit>();
            PtasParceleconomicunitCreatedonbehalfbyValueNavigation = new HashSet<PtasParceleconomicunit>();
            PtasParceleconomicunitModifiedbyValueNavigation = new HashSet<PtasParceleconomicunit>();
            PtasParceleconomicunitModifiedonbehalfbyValueNavigation = new HashSet<PtasParceleconomicunit>();
            PtasParceleconomicunitOwninguserValueNavigation = new HashSet<PtasParceleconomicunit>();
            PtasParcelmetadataCreatedbyValueNavigation = new HashSet<PtasParcelmetadata>();
            PtasParcelmetadataCreatedonbehalfbyValueNavigation = new HashSet<PtasParcelmetadata>();
            PtasParcelmetadataModifiedbyValueNavigation = new HashSet<PtasParcelmetadata>();
            PtasParcelmetadataModifiedonbehalfbyValueNavigation = new HashSet<PtasParcelmetadata>();
            PtasParcelmetadataOwninguserValueNavigation = new HashSet<PtasParcelmetadata>();
            PtasParkingdistrictCreatedbyValueNavigation = new HashSet<PtasParkingdistrict>();
            PtasParkingdistrictCreatedonbehalfbyValueNavigation = new HashSet<PtasParkingdistrict>();
            PtasParkingdistrictModifiedbyValueNavigation = new HashSet<PtasParkingdistrict>();
            PtasParkingdistrictModifiedonbehalfbyValueNavigation = new HashSet<PtasParkingdistrict>();
            PtasParkingdistrictOwninguserValueNavigation = new HashSet<PtasParkingdistrict>();
            PtasPaymentreceiptCreatedbyValueNavigation = new HashSet<PtasPaymentreceipt>();
            PtasPaymentreceiptCreatedonbehalfbyValueNavigation = new HashSet<PtasPaymentreceipt>();
            PtasPaymentreceiptModifiedbyValueNavigation = new HashSet<PtasPaymentreceipt>();
            PtasPaymentreceiptModifiedonbehalfbyValueNavigation = new HashSet<PtasPaymentreceipt>();
            PtasPaymentreceiptOwninguserValueNavigation = new HashSet<PtasPaymentreceipt>();
            PtasPbrspointlevelCreatedbyValueNavigation = new HashSet<PtasPbrspointlevel>();
            PtasPbrspointlevelCreatedonbehalfbyValueNavigation = new HashSet<PtasPbrspointlevel>();
            PtasPbrspointlevelModifiedbyValueNavigation = new HashSet<PtasPbrspointlevel>();
            PtasPbrspointlevelModifiedonbehalfbyValueNavigation = new HashSet<PtasPbrspointlevel>();
            PtasPbrsresourcetypeCreatedbyValueNavigation = new HashSet<PtasPbrsresourcetype>();
            PtasPbrsresourcetypeCreatedonbehalfbyValueNavigation = new HashSet<PtasPbrsresourcetype>();
            PtasPbrsresourcetypeModifiedbyValueNavigation = new HashSet<PtasPbrsresourcetype>();
            PtasPbrsresourcetypeModifiedonbehalfbyValueNavigation = new HashSet<PtasPbrsresourcetype>();
            PtasPermitCreatedbyValueNavigation = new HashSet<PtasPermit>();
            PtasPermitCreatedonbehalfbyValueNavigation = new HashSet<PtasPermit>();
            PtasPermitModifiedbyValueNavigation = new HashSet<PtasPermit>();
            PtasPermitModifiedonbehalfbyValueNavigation = new HashSet<PtasPermit>();
            PtasPermitOwninguserValueNavigation = new HashSet<PtasPermit>();
            PtasPermitPtasReviewedbyidValueNavigation = new HashSet<PtasPermit>();
            PtasPermitPtasStatusupdatedbyidValueNavigation = new HashSet<PtasPermit>();
            PtasPermitinspectionhistoryCreatedbyValueNavigation = new HashSet<PtasPermitinspectionhistory>();
            PtasPermitinspectionhistoryCreatedonbehalfbyValueNavigation = new HashSet<PtasPermitinspectionhistory>();
            PtasPermitinspectionhistoryModifiedbyValueNavigation = new HashSet<PtasPermitinspectionhistory>();
            PtasPermitinspectionhistoryModifiedonbehalfbyValueNavigation = new HashSet<PtasPermitinspectionhistory>();
            PtasPermitinspectionhistoryOwninguserValueNavigation = new HashSet<PtasPermitinspectionhistory>();
            PtasPermitwebsiteconfigCreatedbyValueNavigation = new HashSet<PtasPermitwebsiteconfig>();
            PtasPermitwebsiteconfigCreatedonbehalfbyValueNavigation = new HashSet<PtasPermitwebsiteconfig>();
            PtasPermitwebsiteconfigModifiedbyValueNavigation = new HashSet<PtasPermitwebsiteconfig>();
            PtasPermitwebsiteconfigModifiedonbehalfbyValueNavigation = new HashSet<PtasPermitwebsiteconfig>();
            PtasPermitwebsiteconfigOwninguserValueNavigation = new HashSet<PtasPermitwebsiteconfig>();
            PtasPersonalpropertyCreatedbyValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyCreatedonbehalfbyValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyModifiedbyValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyModifiedonbehalfbyValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyOwninguserValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAccountcreatedbyidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAccountmanageridValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasAuditedbyidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasDiscoveredbyidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasDiscoveryauditbyidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyassetCreatedbyValueNavigation = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertyassetCreatedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertyassetModifiedbyValueNavigation = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertyassetModifiedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertyassetOwninguserValueNavigation = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertycategoryCreatedbyValueNavigation = new HashSet<PtasPersonalpropertycategory>();
            PtasPersonalpropertycategoryCreatedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertycategory>();
            PtasPersonalpropertycategoryModifiedbyValueNavigation = new HashSet<PtasPersonalpropertycategory>();
            PtasPersonalpropertycategoryModifiedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertycategory>();
            PtasPersonalpropertycategoryOwninguserValueNavigation = new HashSet<PtasPersonalpropertycategory>();
            PtasPersonalpropertyhistoryCreatedbyValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryCreatedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryModifiedbyValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryModifiedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryOwninguserValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasAccountmanageruseridValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasAuditedbyidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasAuditedbyuseridValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasDiscoveredbyValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasDiscoveredbyidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertylistingCreatedbyValueNavigation = new HashSet<PtasPersonalpropertylisting>();
            PtasPersonalpropertylistingCreatedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertylisting>();
            PtasPersonalpropertylistingModifiedbyValueNavigation = new HashSet<PtasPersonalpropertylisting>();
            PtasPersonalpropertylistingModifiedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertylisting>();
            PtasPersonalpropertylistingOwninguserValueNavigation = new HashSet<PtasPersonalpropertylisting>();
            PtasPersonalpropertynoteCreatedbyValueNavigation = new HashSet<PtasPersonalpropertynote>();
            PtasPersonalpropertynoteCreatedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertynote>();
            PtasPersonalpropertynoteModifiedbyValueNavigation = new HashSet<PtasPersonalpropertynote>();
            PtasPersonalpropertynoteModifiedonbehalfbyValueNavigation = new HashSet<PtasPersonalpropertynote>();
            PtasPersonalpropertynoteOwninguserValueNavigation = new HashSet<PtasPersonalpropertynote>();
            PtasPerspropbannerannouncementCreatedbyValueNavigation = new HashSet<PtasPerspropbannerannouncement>();
            PtasPerspropbannerannouncementCreatedonbehalfbyValueNavigation = new HashSet<PtasPerspropbannerannouncement>();
            PtasPerspropbannerannouncementModifiedbyValueNavigation = new HashSet<PtasPerspropbannerannouncement>();
            PtasPerspropbannerannouncementModifiedonbehalfbyValueNavigation = new HashSet<PtasPerspropbannerannouncement>();
            PtasPerspropbannerannouncementOwninguserValueNavigation = new HashSet<PtasPerspropbannerannouncement>();
            PtasPhonenumberCreatedbyValueNavigation = new HashSet<PtasPhonenumber>();
            PtasPhonenumberCreatedonbehalfbyValueNavigation = new HashSet<PtasPhonenumber>();
            PtasPhonenumberModifiedbyValueNavigation = new HashSet<PtasPhonenumber>();
            PtasPhonenumberModifiedonbehalfbyValueNavigation = new HashSet<PtasPhonenumber>();
            PtasPhonenumberOwninguserValueNavigation = new HashSet<PtasPhonenumber>();
            PtasPortalcontactCreatedbyValueNavigation = new HashSet<PtasPortalcontact>();
            PtasPortalcontactCreatedonbehalfbyValueNavigation = new HashSet<PtasPortalcontact>();
            PtasPortalcontactModifiedbyValueNavigation = new HashSet<PtasPortalcontact>();
            PtasPortalcontactModifiedonbehalfbyValueNavigation = new HashSet<PtasPortalcontact>();
            PtasPortalcontactOwninguserValueNavigation = new HashSet<PtasPortalcontact>();
            PtasPortalemailCreatedbyValueNavigation = new HashSet<PtasPortalemail>();
            PtasPortalemailCreatedonbehalfbyValueNavigation = new HashSet<PtasPortalemail>();
            PtasPortalemailModifiedbyValueNavigation = new HashSet<PtasPortalemail>();
            PtasPortalemailModifiedonbehalfbyValueNavigation = new HashSet<PtasPortalemail>();
            PtasPortalemailOwninguserValueNavigation = new HashSet<PtasPortalemail>();
            PtasProjectdockCreatedbyValueNavigation = new HashSet<PtasProjectdock>();
            PtasProjectdockCreatedonbehalfbyValueNavigation = new HashSet<PtasProjectdock>();
            PtasProjectdockModifiedbyValueNavigation = new HashSet<PtasProjectdock>();
            PtasProjectdockModifiedonbehalfbyValueNavigation = new HashSet<PtasProjectdock>();
            PtasPropertyreviewCreatedbyValueNavigation = new HashSet<PtasPropertyreview>();
            PtasPropertyreviewCreatedonbehalfbyValueNavigation = new HashSet<PtasPropertyreview>();
            PtasPropertyreviewModifiedbyValueNavigation = new HashSet<PtasPropertyreview>();
            PtasPropertyreviewModifiedonbehalfbyValueNavigation = new HashSet<PtasPropertyreview>();
            PtasPropertyreviewOwninguserValueNavigation = new HashSet<PtasPropertyreview>();
            PtasPropertytypeCreatedbyValueNavigation = new HashSet<PtasPropertytype>();
            PtasPropertytypeCreatedonbehalfbyValueNavigation = new HashSet<PtasPropertytype>();
            PtasPropertytypeModifiedbyValueNavigation = new HashSet<PtasPropertytype>();
            PtasPropertytypeModifiedonbehalfbyValueNavigation = new HashSet<PtasPropertytype>();
            PtasPtasconfigurationCreatedbyValueNavigation = new HashSet<PtasPtasconfiguration>();
            PtasPtasconfigurationCreatedonbehalfbyValueNavigation = new HashSet<PtasPtasconfiguration>();
            PtasPtasconfigurationModifiedbyValueNavigation = new HashSet<PtasPtasconfiguration>();
            PtasPtasconfigurationModifiedonbehalfbyValueNavigation = new HashSet<PtasPtasconfiguration>();
            PtasPtasconfigurationPtasDefaultsendfromidValueNavigation = new HashSet<PtasPtasconfiguration>();
            PtasPtasconfigurationPtasSendsrexemptsyncemailtoValueNavigation = new HashSet<PtasPtasconfiguration>();
            PtasPtassettingCreatedbyValueNavigation = new HashSet<PtasPtassetting>();
            PtasPtassettingCreatedonbehalfbyValueNavigation = new HashSet<PtasPtassetting>();
            PtasPtassettingModifiedbyValueNavigation = new HashSet<PtasPtassetting>();
            PtasPtassettingModifiedonbehalfbyValueNavigation = new HashSet<PtasPtassetting>();
            PtasPtassettingOwninguserValueNavigation = new HashSet<PtasPtassetting>();
            PtasQstrCreatedbyValueNavigation = new HashSet<PtasQstr>();
            PtasQstrCreatedonbehalfbyValueNavigation = new HashSet<PtasQstr>();
            PtasQstrModifiedbyValueNavigation = new HashSet<PtasQstr>();
            PtasQstrModifiedonbehalfbyValueNavigation = new HashSet<PtasQstr>();
            PtasQuickcollectCreatedbyValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectCreatedonbehalfbyValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectModifiedbyValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectModifiedonbehalfbyValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectOwninguserValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectPtasProcesseduseridValueNavigation = new HashSet<PtasQuickcollect>();
            PtasRatesheetdetailCreatedbyValueNavigation = new HashSet<PtasRatesheetdetail>();
            PtasRatesheetdetailCreatedonbehalfbyValueNavigation = new HashSet<PtasRatesheetdetail>();
            PtasRatesheetdetailModifiedbyValueNavigation = new HashSet<PtasRatesheetdetail>();
            PtasRatesheetdetailModifiedonbehalfbyValueNavigation = new HashSet<PtasRatesheetdetail>();
            PtasRatesheetdetailOwninguserValueNavigation = new HashSet<PtasRatesheetdetail>();
            PtasRecentparcelCreatedbyValueNavigation = new HashSet<PtasRecentparcel>();
            PtasRecentparcelCreatedonbehalfbyValueNavigation = new HashSet<PtasRecentparcel>();
            PtasRecentparcelModifiedbyValueNavigation = new HashSet<PtasRecentparcel>();
            PtasRecentparcelModifiedonbehalfbyValueNavigation = new HashSet<PtasRecentparcel>();
            PtasRecentparcelOwninguserValueNavigation = new HashSet<PtasRecentparcel>();
            PtasRefundpetitionCreatedbyValueNavigation = new HashSet<PtasRefundpetition>();
            PtasRefundpetitionCreatedonbehalfbyValueNavigation = new HashSet<PtasRefundpetition>();
            PtasRefundpetitionModifiedbyValueNavigation = new HashSet<PtasRefundpetition>();
            PtasRefundpetitionModifiedonbehalfbyValueNavigation = new HashSet<PtasRefundpetition>();
            PtasRefundpetitionOwninguserValueNavigation = new HashSet<PtasRefundpetition>();
            PtasRefundpetitionlevyrateCreatedbyValueNavigation = new HashSet<PtasRefundpetitionlevyrate>();
            PtasRefundpetitionlevyrateCreatedonbehalfbyValueNavigation = new HashSet<PtasRefundpetitionlevyrate>();
            PtasRefundpetitionlevyrateModifiedbyValueNavigation = new HashSet<PtasRefundpetitionlevyrate>();
            PtasRefundpetitionlevyrateModifiedonbehalfbyValueNavigation = new HashSet<PtasRefundpetitionlevyrate>();
            PtasRefundpetitionlevyrateOwninguserValueNavigation = new HashSet<PtasRefundpetitionlevyrate>();
            PtasResidentialappraiserteamCreatedbyValueNavigation = new HashSet<PtasResidentialappraiserteam>();
            PtasResidentialappraiserteamCreatedonbehalfbyValueNavigation = new HashSet<PtasResidentialappraiserteam>();
            PtasResidentialappraiserteamModifiedbyValueNavigation = new HashSet<PtasResidentialappraiserteam>();
            PtasResidentialappraiserteamModifiedonbehalfbyValueNavigation = new HashSet<PtasResidentialappraiserteam>();
            PtasResidentialappraiserteamOwninguserValueNavigation = new HashSet<PtasResidentialappraiserteam>();
            PtasResponsibilityCreatedbyValueNavigation = new HashSet<PtasResponsibility>();
            PtasResponsibilityCreatedonbehalfbyValueNavigation = new HashSet<PtasResponsibility>();
            PtasResponsibilityModifiedbyValueNavigation = new HashSet<PtasResponsibility>();
            PtasResponsibilityModifiedonbehalfbyValueNavigation = new HashSet<PtasResponsibility>();
            PtasRestrictedrentCreatedbyValueNavigation = new HashSet<PtasRestrictedrent>();
            PtasRestrictedrentCreatedonbehalfbyValueNavigation = new HashSet<PtasRestrictedrent>();
            PtasRestrictedrentModifiedbyValueNavigation = new HashSet<PtasRestrictedrent>();
            PtasRestrictedrentModifiedonbehalfbyValueNavigation = new HashSet<PtasRestrictedrent>();
            PtasRestrictedrentOwninguserValueNavigation = new HashSet<PtasRestrictedrent>();
            PtasSalepriceadjustmentCreatedbyValueNavigation = new HashSet<PtasSalepriceadjustment>();
            PtasSalepriceadjustmentCreatedonbehalfbyValueNavigation = new HashSet<PtasSalepriceadjustment>();
            PtasSalepriceadjustmentModifiedbyValueNavigation = new HashSet<PtasSalepriceadjustment>();
            PtasSalepriceadjustmentModifiedonbehalfbyValueNavigation = new HashSet<PtasSalepriceadjustment>();
            PtasSalepriceadjustmentOwninguserValueNavigation = new HashSet<PtasSalepriceadjustment>();
            PtasSalesaccessoryCreatedbyValueNavigation = new HashSet<PtasSalesaccessory>();
            PtasSalesaccessoryCreatedonbehalfbyValueNavigation = new HashSet<PtasSalesaccessory>();
            PtasSalesaccessoryModifiedbyValueNavigation = new HashSet<PtasSalesaccessory>();
            PtasSalesaccessoryModifiedonbehalfbyValueNavigation = new HashSet<PtasSalesaccessory>();
            PtasSalesaccessoryOwninguserValueNavigation = new HashSet<PtasSalesaccessory>();
            PtasSalesaggregateCreatedbyValueNavigation = new HashSet<PtasSalesaggregate>();
            PtasSalesaggregateCreatedonbehalfbyValueNavigation = new HashSet<PtasSalesaggregate>();
            PtasSalesaggregateModifiedbyValueNavigation = new HashSet<PtasSalesaggregate>();
            PtasSalesaggregateModifiedonbehalfbyValueNavigation = new HashSet<PtasSalesaggregate>();
            PtasSalesaggregateOwninguserValueNavigation = new HashSet<PtasSalesaggregate>();
            PtasSalesbuildingCreatedbyValueNavigation = new HashSet<PtasSalesbuilding>();
            PtasSalesbuildingCreatedonbehalfbyValueNavigation = new HashSet<PtasSalesbuilding>();
            PtasSalesbuildingModifiedbyValueNavigation = new HashSet<PtasSalesbuilding>();
            PtasSalesbuildingModifiedonbehalfbyValueNavigation = new HashSet<PtasSalesbuilding>();
            PtasSalesbuildingOwninguserValueNavigation = new HashSet<PtasSalesbuilding>();
            PtasSalesnoteCreatedbyValueNavigation = new HashSet<PtasSalesnote>();
            PtasSalesnoteCreatedonbehalfbyValueNavigation = new HashSet<PtasSalesnote>();
            PtasSalesnoteModifiedbyValueNavigation = new HashSet<PtasSalesnote>();
            PtasSalesnoteModifiedonbehalfbyValueNavigation = new HashSet<PtasSalesnote>();
            PtasSalesnoteOwninguserValueNavigation = new HashSet<PtasSalesnote>();
            PtasSalesparcelCreatedbyValueNavigation = new HashSet<PtasSalesparcel>();
            PtasSalesparcelCreatedonbehalfbyValueNavigation = new HashSet<PtasSalesparcel>();
            PtasSalesparcelModifiedbyValueNavigation = new HashSet<PtasSalesparcel>();
            PtasSalesparcelModifiedonbehalfbyValueNavigation = new HashSet<PtasSalesparcel>();
            PtasSalesparcelOwninguserValueNavigation = new HashSet<PtasSalesparcel>();
            PtasSaleswarningcodeCreatedbyValueNavigation = new HashSet<PtasSaleswarningcode>();
            PtasSaleswarningcodeCreatedonbehalfbyValueNavigation = new HashSet<PtasSaleswarningcode>();
            PtasSaleswarningcodeModifiedbyValueNavigation = new HashSet<PtasSaleswarningcode>();
            PtasSaleswarningcodeModifiedonbehalfbyValueNavigation = new HashSet<PtasSaleswarningcode>();
            PtasScheduledworkflowCreatedbyValueNavigation = new HashSet<PtasScheduledworkflow>();
            PtasScheduledworkflowCreatedonbehalfbyValueNavigation = new HashSet<PtasScheduledworkflow>();
            PtasScheduledworkflowModifiedbyValueNavigation = new HashSet<PtasScheduledworkflow>();
            PtasScheduledworkflowModifiedonbehalfbyValueNavigation = new HashSet<PtasScheduledworkflow>();
            PtasScheduledworkflowOwninguserValueNavigation = new HashSet<PtasScheduledworkflow>();
            PtasSeappdetailCreatedbyValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeappdetailCreatedonbehalfbyValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeappdetailModifiedbyValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeappdetailModifiedonbehalfbyValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeappdetailOwninguserValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeappdetailPtasCompletedbyidValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeapplicationCreatedbyValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationCreatedonbehalfbyValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationModifiedbyValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationModifiedonbehalfbyValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationOwninguserValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationPtasCompletedbyidValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationtaskCreatedbyValueNavigation = new HashSet<PtasSeapplicationtask>();
            PtasSeapplicationtaskCreatedonbehalfbyValueNavigation = new HashSet<PtasSeapplicationtask>();
            PtasSeapplicationtaskModifiedbyValueNavigation = new HashSet<PtasSeapplicationtask>();
            PtasSeapplicationtaskModifiedonbehalfbyValueNavigation = new HashSet<PtasSeapplicationtask>();
            PtasSeapplicationtaskOwninguserValueNavigation = new HashSet<PtasSeapplicationtask>();
            PtasSeappnoteCreatedbyValueNavigation = new HashSet<PtasSeappnote>();
            PtasSeappnoteCreatedonbehalfbyValueNavigation = new HashSet<PtasSeappnote>();
            PtasSeappnoteModifiedbyValueNavigation = new HashSet<PtasSeappnote>();
            PtasSeappnoteModifiedonbehalfbyValueNavigation = new HashSet<PtasSeappnote>();
            PtasSeappnoteOwninguserValueNavigation = new HashSet<PtasSeappnote>();
            PtasSeappoccupantCreatedbyValueNavigation = new HashSet<PtasSeappoccupant>();
            PtasSeappoccupantCreatedonbehalfbyValueNavigation = new HashSet<PtasSeappoccupant>();
            PtasSeappoccupantModifiedbyValueNavigation = new HashSet<PtasSeappoccupant>();
            PtasSeappoccupantModifiedonbehalfbyValueNavigation = new HashSet<PtasSeappoccupant>();
            PtasSeappoccupantOwninguserValueNavigation = new HashSet<PtasSeappoccupant>();
            PtasSeappotherpropCreatedbyValueNavigation = new HashSet<PtasSeappotherprop>();
            PtasSeappotherpropCreatedonbehalfbyValueNavigation = new HashSet<PtasSeappotherprop>();
            PtasSeappotherpropModifiedbyValueNavigation = new HashSet<PtasSeappotherprop>();
            PtasSeappotherpropModifiedonbehalfbyValueNavigation = new HashSet<PtasSeappotherprop>();
            PtasSeappotherpropOwninguserValueNavigation = new HashSet<PtasSeappotherprop>();
            PtasSectionusesqftCreatedbyValueNavigation = new HashSet<PtasSectionusesqft>();
            PtasSectionusesqftCreatedonbehalfbyValueNavigation = new HashSet<PtasSectionusesqft>();
            PtasSectionusesqftModifiedbyValueNavigation = new HashSet<PtasSectionusesqft>();
            PtasSectionusesqftModifiedonbehalfbyValueNavigation = new HashSet<PtasSectionusesqft>();
            PtasSectionusesqftOwninguserValueNavigation = new HashSet<PtasSectionusesqft>();
            PtasSeeligibilityCreatedbyValueNavigation = new HashSet<PtasSeeligibility>();
            PtasSeeligibilityCreatedonbehalfbyValueNavigation = new HashSet<PtasSeeligibility>();
            PtasSeeligibilityModifiedbyValueNavigation = new HashSet<PtasSeeligibility>();
            PtasSeeligibilityModifiedonbehalfbyValueNavigation = new HashSet<PtasSeeligibility>();
            PtasSeeligibilityOwninguserValueNavigation = new HashSet<PtasSeeligibility>();
            PtasSeeligibilitybracketCreatedbyValueNavigation = new HashSet<PtasSeeligibilitybracket>();
            PtasSeeligibilitybracketCreatedonbehalfbyValueNavigation = new HashSet<PtasSeeligibilitybracket>();
            PtasSeeligibilitybracketModifiedbyValueNavigation = new HashSet<PtasSeeligibilitybracket>();
            PtasSeeligibilitybracketModifiedonbehalfbyValueNavigation = new HashSet<PtasSeeligibilitybracket>();
            PtasSeeligibilitybracketOwninguserValueNavigation = new HashSet<PtasSeeligibilitybracket>();
            PtasSeexemptionreasonCreatedbyValueNavigation = new HashSet<PtasSeexemptionreason>();
            PtasSeexemptionreasonCreatedonbehalfbyValueNavigation = new HashSet<PtasSeexemptionreason>();
            PtasSeexemptionreasonModifiedbyValueNavigation = new HashSet<PtasSeexemptionreason>();
            PtasSeexemptionreasonModifiedonbehalfbyValueNavigation = new HashSet<PtasSeexemptionreason>();
            PtasSefrozenvalueCreatedbyValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSefrozenvalueCreatedonbehalfbyValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSefrozenvalueModifiedbyValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSefrozenvalueModifiedonbehalfbyValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSefrozenvalueOwninguserValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSketchCreatedbyValueNavigation = new HashSet<PtasSketch>();
            PtasSketchCreatedonbehalfbyValueNavigation = new HashSet<PtasSketch>();
            PtasSketchModifiedbyValueNavigation = new HashSet<PtasSketch>();
            PtasSketchModifiedonbehalfbyValueNavigation = new HashSet<PtasSketch>();
            PtasSketchOwninguserValueNavigation = new HashSet<PtasSketch>();
            PtasSketchPtasDrawauthoridValueNavigation = new HashSet<PtasSketch>();
            PtasSketchPtasLockedbyidValueNavigation = new HashSet<PtasSketch>();
            PtasSpecialtyareaCreatedbyValueNavigation = new HashSet<PtasSpecialtyarea>();
            PtasSpecialtyareaCreatedonbehalfbyValueNavigation = new HashSet<PtasSpecialtyarea>();
            PtasSpecialtyareaModifiedbyValueNavigation = new HashSet<PtasSpecialtyarea>();
            PtasSpecialtyareaModifiedonbehalfbyValueNavigation = new HashSet<PtasSpecialtyarea>();
            PtasSpecialtyareaPtasSeniorappraiseridValueNavigation = new HashSet<PtasSpecialtyarea>();
            PtasSpecialtyneighborhoodCreatedbyValueNavigation = new HashSet<PtasSpecialtyneighborhood>();
            PtasSpecialtyneighborhoodCreatedonbehalfbyValueNavigation = new HashSet<PtasSpecialtyneighborhood>();
            PtasSpecialtyneighborhoodModifiedbyValueNavigation = new HashSet<PtasSpecialtyneighborhood>();
            PtasSpecialtyneighborhoodModifiedonbehalfbyValueNavigation = new HashSet<PtasSpecialtyneighborhood>();
            PtasSpecialtyneighborhoodPtasAppraiseridValueNavigation = new HashSet<PtasSpecialtyneighborhood>();
            PtasStateorprovinceCreatedbyValueNavigation = new HashSet<PtasStateorprovince>();
            PtasStateorprovinceCreatedonbehalfbyValueNavigation = new HashSet<PtasStateorprovince>();
            PtasStateorprovinceModifiedbyValueNavigation = new HashSet<PtasStateorprovince>();
            PtasStateorprovinceModifiedonbehalfbyValueNavigation = new HashSet<PtasStateorprovince>();
            PtasStateutilityvalueCreatedbyValueNavigation = new HashSet<PtasStateutilityvalue>();
            PtasStateutilityvalueCreatedonbehalfbyValueNavigation = new HashSet<PtasStateutilityvalue>();
            PtasStateutilityvalueModifiedbyValueNavigation = new HashSet<PtasStateutilityvalue>();
            PtasStateutilityvalueModifiedonbehalfbyValueNavigation = new HashSet<PtasStateutilityvalue>();
            PtasStateutilityvalueOwninguserValueNavigation = new HashSet<PtasStateutilityvalue>();
            PtasStreetnameCreatedbyValueNavigation = new HashSet<PtasStreetname>();
            PtasStreetnameCreatedonbehalfbyValueNavigation = new HashSet<PtasStreetname>();
            PtasStreetnameModifiedbyValueNavigation = new HashSet<PtasStreetname>();
            PtasStreetnameModifiedonbehalfbyValueNavigation = new HashSet<PtasStreetname>();
            PtasStreettypeCreatedbyValueNavigation = new HashSet<PtasStreettype>();
            PtasStreettypeCreatedonbehalfbyValueNavigation = new HashSet<PtasStreettype>();
            PtasStreettypeModifiedbyValueNavigation = new HashSet<PtasStreettype>();
            PtasStreettypeModifiedonbehalfbyValueNavigation = new HashSet<PtasStreettype>();
            PtasSubareaCreatedbyValueNavigation = new HashSet<PtasSubarea>();
            PtasSubareaCreatedonbehalfbyValueNavigation = new HashSet<PtasSubarea>();
            PtasSubareaModifiedbyValueNavigation = new HashSet<PtasSubarea>();
            PtasSubareaModifiedonbehalfbyValueNavigation = new HashSet<PtasSubarea>();
            PtasSubmarketCreatedbyValueNavigation = new HashSet<PtasSubmarket>();
            PtasSubmarketCreatedonbehalfbyValueNavigation = new HashSet<PtasSubmarket>();
            PtasSubmarketModifiedbyValueNavigation = new HashSet<PtasSubmarket>();
            PtasSubmarketModifiedonbehalfbyValueNavigation = new HashSet<PtasSubmarket>();
            PtasSupergroupCreatedbyValueNavigation = new HashSet<PtasSupergroup>();
            PtasSupergroupCreatedonbehalfbyValueNavigation = new HashSet<PtasSupergroup>();
            PtasSupergroupModifiedbyValueNavigation = new HashSet<PtasSupergroup>();
            PtasSupergroupModifiedonbehalfbyValueNavigation = new HashSet<PtasSupergroup>();
            PtasTaskCreatedbyValueNavigation = new HashSet<PtasTask>();
            PtasTaskCreatedonbehalfbyValueNavigation = new HashSet<PtasTask>();
            PtasTaskModifiedbyValueNavigation = new HashSet<PtasTask>();
            PtasTaskModifiedonbehalfbyValueNavigation = new HashSet<PtasTask>();
            PtasTaskOwninguserValueNavigation = new HashSet<PtasTask>();
            PtasTaskPtasAccountingsectionsupervisorValueNavigation = new HashSet<PtasTask>();
            PtasTaskPtasAppraiserValueNavigation = new HashSet<PtasTask>();
            PtasTaskPtasCommercialsrappraiserValueNavigation = new HashSet<PtasTask>();
            PtasTaskPtasResidentialsrappraiserValueNavigation = new HashSet<PtasTask>();
            PtasTaxaccountCreatedbyValueNavigation = new HashSet<PtasTaxaccount>();
            PtasTaxaccountCreatedonbehalfbyValueNavigation = new HashSet<PtasTaxaccount>();
            PtasTaxaccountModifiedbyValueNavigation = new HashSet<PtasTaxaccount>();
            PtasTaxaccountModifiedonbehalfbyValueNavigation = new HashSet<PtasTaxaccount>();
            PtasTaxaccountOwninguserValueNavigation = new HashSet<PtasTaxaccount>();
            PtasTaxbillCreatedbyValueNavigation = new HashSet<PtasTaxbill>();
            PtasTaxbillCreatedonbehalfbyValueNavigation = new HashSet<PtasTaxbill>();
            PtasTaxbillModifiedbyValueNavigation = new HashSet<PtasTaxbill>();
            PtasTaxbillModifiedonbehalfbyValueNavigation = new HashSet<PtasTaxbill>();
            PtasTaxbillOwninguserValueNavigation = new HashSet<PtasTaxbill>();
            PtasTaxdistrictCreatedbyValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxdistrictCreatedonbehalfbyValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxdistrictModifiedbyValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxdistrictModifiedonbehalfbyValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxdistrictOwninguserValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxrollcorrectionCreatedbyValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionCreatedonbehalfbyValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionModifiedbyValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionModifiedonbehalfbyValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionOwninguserValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionPtasRequestedbyidValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionPtasReviewedbyidValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionPtasSeniorrevieweridValueNavigation = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionvalueCreatedbyValueNavigation = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasTaxrollcorrectionvalueCreatedonbehalfbyValueNavigation = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasTaxrollcorrectionvalueModifiedbyValueNavigation = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasTaxrollcorrectionvalueModifiedonbehalfbyValueNavigation = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasTaxrollcorrectionvalueOwninguserValueNavigation = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasTimberusetypeCreatedbyValueNavigation = new HashSet<PtasTimberusetype>();
            PtasTimberusetypeCreatedonbehalfbyValueNavigation = new HashSet<PtasTimberusetype>();
            PtasTimberusetypeModifiedbyValueNavigation = new HashSet<PtasTimberusetype>();
            PtasTimberusetypeModifiedonbehalfbyValueNavigation = new HashSet<PtasTimberusetype>();
            PtasTrendfactorCreatedbyValueNavigation = new HashSet<PtasTrendfactor>();
            PtasTrendfactorCreatedonbehalfbyValueNavigation = new HashSet<PtasTrendfactor>();
            PtasTrendfactorModifiedbyValueNavigation = new HashSet<PtasTrendfactor>();
            PtasTrendfactorModifiedonbehalfbyValueNavigation = new HashSet<PtasTrendfactor>();
            PtasTrendfactorOwninguserValueNavigation = new HashSet<PtasTrendfactor>();
            PtasUnitbreakdownCreatedbyValueNavigation = new HashSet<PtasUnitbreakdown>();
            PtasUnitbreakdownCreatedonbehalfbyValueNavigation = new HashSet<PtasUnitbreakdown>();
            PtasUnitbreakdownModifiedbyValueNavigation = new HashSet<PtasUnitbreakdown>();
            PtasUnitbreakdownModifiedonbehalfbyValueNavigation = new HashSet<PtasUnitbreakdown>();
            PtasUnitbreakdownOwninguserValueNavigation = new HashSet<PtasUnitbreakdown>();
            PtasUnitbreakdowntypeCreatedbyValueNavigation = new HashSet<PtasUnitbreakdowntype>();
            PtasUnitbreakdowntypeCreatedonbehalfbyValueNavigation = new HashSet<PtasUnitbreakdowntype>();
            PtasUnitbreakdowntypeModifiedbyValueNavigation = new HashSet<PtasUnitbreakdowntype>();
            PtasUnitbreakdowntypeModifiedonbehalfbyValueNavigation = new HashSet<PtasUnitbreakdowntype>();
            PtasValuehistoryCreatedbyValueNavigation = new HashSet<PtasValuehistory>();
            PtasValuehistoryCreatedonbehalfbyValueNavigation = new HashSet<PtasValuehistory>();
            PtasValuehistoryModifiedbyValueNavigation = new HashSet<PtasValuehistory>();
            PtasValuehistoryModifiedonbehalfbyValueNavigation = new HashSet<PtasValuehistory>();
            PtasValuehistoryOwninguserValueNavigation = new HashSet<PtasValuehistory>();
            PtasViewtypeCreatedbyValueNavigation = new HashSet<PtasViewtype>();
            PtasViewtypeCreatedonbehalfbyValueNavigation = new HashSet<PtasViewtype>();
            PtasViewtypeModifiedbyValueNavigation = new HashSet<PtasViewtype>();
            PtasViewtypeModifiedonbehalfbyValueNavigation = new HashSet<PtasViewtype>();
            PtasViewtypeOwninguserValueNavigation = new HashSet<PtasViewtype>();
            PtasVisitedsketchCreatedbyValueNavigation = new HashSet<PtasVisitedsketch>();
            PtasVisitedsketchCreatedonbehalfbyValueNavigation = new HashSet<PtasVisitedsketch>();
            PtasVisitedsketchModifiedbyValueNavigation = new HashSet<PtasVisitedsketch>();
            PtasVisitedsketchModifiedonbehalfbyValueNavigation = new HashSet<PtasVisitedsketch>();
            PtasVisitedsketchOwninguserValueNavigation = new HashSet<PtasVisitedsketch>();
            PtasVisitedsketchPtasVisitedbyidValueNavigation = new HashSet<PtasVisitedsketch>();
            PtasYearCreatedbyValueNavigation = new HashSet<PtasYear>();
            PtasYearCreatedonbehalfbyValueNavigation = new HashSet<PtasYear>();
            PtasYearModifiedbyValueNavigation = new HashSet<PtasYear>();
            PtasYearModifiedonbehalfbyValueNavigation = new HashSet<PtasYear>();
            PtasYearPtasRollovernotificationidValueNavigation = new HashSet<PtasYear>();
            PtasZipcodeCreatedbyValueNavigation = new HashSet<PtasZipcode>();
            PtasZipcodeCreatedonbehalfbyValueNavigation = new HashSet<PtasZipcode>();
            PtasZipcodeModifiedbyValueNavigation = new HashSet<PtasZipcode>();
            PtasZipcodeModifiedonbehalfbyValueNavigation = new HashSet<PtasZipcode>();
            PtasZoningCreatedbyValueNavigation = new HashSet<PtasZoning>();
            PtasZoningCreatedonbehalfbyValueNavigation = new HashSet<PtasZoning>();
            PtasZoningModifiedbyValueNavigation = new HashSet<PtasZoning>();
            PtasZoningModifiedonbehalfbyValueNavigation = new HashSet<PtasZoning>();
            RoleCreatedbyValueNavigation = new HashSet<Role>();
            RoleCreatedonbehalfbyValueNavigation = new HashSet<Role>();
            RoleModifiedbyValueNavigation = new HashSet<Role>();
            RoleModifiedonbehalfbyValueNavigation = new HashSet<Role>();
            TeamAdministratoridValueNavigation = new HashSet<Team>();
            TeamCreatedbyValueNavigation = new HashSet<Team>();
            TeamCreatedonbehalfbyValueNavigation = new HashSet<Team>();
            TeamModifiedbyValueNavigation = new HashSet<Team>();
            TeamModifiedonbehalfbyValueNavigation = new HashSet<Team>();
        }

        public int? Accessmode { get; set; }
        public Guid? Address1Addressid { get; set; }
        public int? Address1Addresstypecode { get; set; }
        public string Address1City { get; set; }
        public string Address1Composite { get; set; }
        public string Address1Country { get; set; }
        public string Address1County { get; set; }
        public string Address1Fax { get; set; }
        public double? Address1Latitude { get; set; }
        public string Address1Line1 { get; set; }
        public string Address1Line2 { get; set; }
        public string Address1Line3 { get; set; }
        public double? Address1Longitude { get; set; }
        public string Address1Name { get; set; }
        public string Address1Postalcode { get; set; }
        public string Address1Postofficebox { get; set; }
        public int? Address1Shippingmethodcode { get; set; }
        public string Address1Stateorprovince { get; set; }
        public string Address1Telephone1 { get; set; }
        public string Address1Telephone2 { get; set; }
        public string Address1Telephone3 { get; set; }
        public string Address1Upszone { get; set; }
        public int? Address1Utcoffset { get; set; }
        public Guid? Address2Addressid { get; set; }
        public int? Address2Addresstypecode { get; set; }
        public string Address2City { get; set; }
        public string Address2Composite { get; set; }
        public string Address2Country { get; set; }
        public string Address2County { get; set; }
        public string Address2Fax { get; set; }
        public double? Address2Latitude { get; set; }
        public string Address2Line1 { get; set; }
        public string Address2Line2 { get; set; }
        public string Address2Line3 { get; set; }
        public double? Address2Longitude { get; set; }
        public string Address2Name { get; set; }
        public string Address2Postalcode { get; set; }
        public string Address2Postofficebox { get; set; }
        public int? Address2Shippingmethodcode { get; set; }
        public string Address2Stateorprovince { get; set; }
        public string Address2Telephone1 { get; set; }
        public string Address2Telephone2 { get; set; }
        public string Address2Telephone3 { get; set; }
        public string Address2Upszone { get; set; }
        public int? Address2Utcoffset { get; set; }
        public Guid? Applicationid { get; set; }
        public string Applicationiduri { get; set; }
        public Guid? Azureactivedirectoryobjectid { get; set; }
        public int? Caltype { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public bool? Defaultfilterspopulated { get; set; }
        public string Defaultodbfoldername { get; set; }
        public string Disabledreason { get; set; }
        public bool? Displayinserviceviews { get; set; }
        public string Domainname { get; set; }
        public int? Emailrouteraccessapproval { get; set; }
        public string Employeeid { get; set; }
        public long? EntityimageTimestamp { get; set; }
        public string EntityimageUrl { get; set; }
        public Guid? Entityimageid { get; set; }
        public decimal? Exchangerate { get; set; }
        public string Firstname { get; set; }
        public string Fullname { get; set; }
        public string Governmentid { get; set; }
        public string Homephone { get; set; }
        public int? Identityid { get; set; }
        public int? Importsequencenumber { get; set; }
        public int? Incomingemaildeliverymethod { get; set; }
        public string Internalemailaddress { get; set; }
        public int? Invitestatuscode { get; set; }
        public bool? Isdisabled { get; set; }
        public bool? Isemailaddressapprovedbyo365admin { get; set; }
        public bool? Isintegrationuser { get; set; }
        public bool? Islicensed { get; set; }
        public bool? Issyncwithdirectory { get; set; }
        public string Jobtitle { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string Mobilealertemail { get; set; }
        public string Mobilephone { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public bool? MsdynGdproptout { get; set; }
        public string Nickname { get; set; }
        public Guid? Organizationid { get; set; }
        public int? Outgoingemaildeliverymethod { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? Passporthi { get; set; }
        public int? Passportlo { get; set; }
        public string Personalemailaddress { get; set; }
        public string Photourl { get; set; }
        public int? Preferredaddresscode { get; set; }
        public int? Preferredemailcode { get; set; }
        public int? Preferredphonecode { get; set; }
        public Guid? Processid { get; set; }
        public string PtasLegacyid { get; set; }
        public string Salutation { get; set; }
        public bool? Setupuser { get; set; }
        public string Sharepointemailaddress { get; set; }
        public string Skills { get; set; }
        public Guid? Stageid { get; set; }
        [Key]
        public Guid Systemuserid { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public string Title { get; set; }
        public string Traversedpath { get; set; }
        public int? Userlicensetype { get; set; }
        public string Userpuid { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public string Windowsliveid { get; set; }
        public string Yammeremailaddress { get; set; }
        public string Yammeruserid { get; set; }
        public string Yomifirstname { get; set; }
        public string Yomifullname { get; set; }
        public string Yomilastname { get; set; }
        public string Yomimiddlename { get; set; }
        public Guid? BusinessunitidValue { get; set; }
        public Guid? CalendaridValue { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? DefaultmailboxValue { get; set; }
        public Guid? MobileofflineprofileidValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? ParentsystemuseridValue { get; set; }
        public Guid? PositionidValue { get; set; }
        public Guid? QueueidValue { get; set; }
        public Guid? SiteidValue { get; set; }
        public Guid? TerritoryidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }
        public bool? PtasTaskassignmentnotification { get; set; }
        public bool? PtasTaskstatusnotification { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ParentsystemuseridValueNavigation { get; set; }
        public virtual ICollection<Contact> ContactCreatedbyValueNavigation { get; set; }
        public virtual ICollection<Contact> ContactCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Contact> ContactModifiedbyValueNavigation { get; set; }
        public virtual ICollection<Contact> ContactModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Contact> ContactOwninguserValueNavigation { get; set; }
        public virtual ICollection<Contact> ContactPreferredsystemuseridValueNavigation { get; set; }
        public virtual ICollection<Systemuser> InverseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<Systemuser> InverseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Systemuser> InverseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<Systemuser> InverseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Systemuser> InverseParentsystemuseridValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocumentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocumentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocumentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocumentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocumentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectPtasMppgMappingchangemadebyidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcelOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcelOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAddresschangehistory> PtasAddresschangehistoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAddresschangehistory> PtasAddresschangehistoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAddresschangehistory> PtasAddresschangehistoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAddresschangehistory> PtasAddresschangehistoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAddresschangehistory> PtasAddresschangehistoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAgriculturalusetype> PtasAgriculturalusetypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAgriculturalusetype> PtasAgriculturalusetypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAgriculturalusetype> PtasAgriculturalusetypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAgriculturalusetype> PtasAgriculturalusetypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreviewCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreviewCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreviewModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreviewModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreviewOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtrackerCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtrackerCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtrackerModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtrackerModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtrackerOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAnnualcostdistribution> PtasAnnualcostdistributionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnualcostdistribution> PtasAnnualcostdistributionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnualcostdistribution> PtasAnnualcostdistributionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnualcostdistribution> PtasAnnualcostdistributionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAnnualcostdistribution> PtasAnnualcostdistributionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentregion> PtasApartmentregionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentregion> PtasApartmentregionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentregion> PtasApartmentregionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentregion> PtasApartmentregionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentsupergroup> PtasApartmentsupergroupCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentsupergroup> PtasApartmentsupergroupCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentsupergroup> PtasApartmentsupergroupModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasApartmentsupergroup> PtasApartmentsupergroupModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAppeal> PtasAppealCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAppeal> PtasAppealCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAppeal> PtasAppealModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAppeal> PtasAppealModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAppeal> PtasAppealOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrateCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrateCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrateModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrateModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrateOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesaleCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesaleCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesaleModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesaleModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesaleOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptbuildingqualityadjustment> PtasAptbuildingqualityadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptbuildingqualityadjustment> PtasAptbuildingqualityadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptbuildingqualityadjustment> PtasAptbuildingqualityadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptbuildingqualityadjustment> PtasAptbuildingqualityadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptbuildingqualityadjustment> PtasAptbuildingqualityadjustmentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptcloseproximity> PtasAptcloseproximityCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcloseproximity> PtasAptcloseproximityCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcloseproximity> PtasAptcloseproximityModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcloseproximity> PtasAptcloseproximityModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpenseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpenseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpenseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpenseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpenseOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesaleCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesaleCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesaleModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesaleModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesaleOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptconditionadjustment> PtasAptconditionadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptconditionadjustment> PtasAptconditionadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptconditionadjustment> PtasAptconditionadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptconditionadjustment> PtasAptconditionadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptconditionadjustment> PtasAptconditionadjustmentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptestimatedunitsqft> PtasAptestimatedunitsqftCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptestimatedunitsqft> PtasAptestimatedunitsqftCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptestimatedunitsqft> PtasAptestimatedunitsqftModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptestimatedunitsqft> PtasAptestimatedunitsqftModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptestimatedunitsqft> PtasAptestimatedunitsqftOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpensehighend> PtasAptexpensehighendCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpensehighend> PtasAptexpensehighendCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpensehighend> PtasAptexpensehighendModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpensehighend> PtasAptexpensehighendModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpensehighend> PtasAptexpensehighendOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpenseunitsize> PtasAptexpenseunitsizeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpenseunitsize> PtasAptexpenseunitsizeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpenseunitsize> PtasAptexpenseunitsizeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpenseunitsize> PtasAptexpenseunitsizeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptexpenseunitsize> PtasAptexpenseunitsizeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptincomeexpense> PtasAptincomeexpenseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptincomeexpense> PtasAptincomeexpenseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptincomeexpense> PtasAptincomeexpenseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptincomeexpense> PtasAptincomeexpenseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptneighborhood> PtasAptneighborhoodCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptneighborhood> PtasAptneighborhoodCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptneighborhood> PtasAptneighborhoodModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptneighborhood> PtasAptneighborhoodModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptnumberofunitsadjustment> PtasAptnumberofunitsadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptnumberofunitsadjustment> PtasAptnumberofunitsadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptnumberofunitsadjustment> PtasAptnumberofunitsadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptnumberofunitsadjustment> PtasAptnumberofunitsadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptnumberofunitsadjustment> PtasAptnumberofunitsadjustmentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptpoolandelevatorexpense> PtasAptpoolandelevatorexpenseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptpoolandelevatorexpense> PtasAptpoolandelevatorexpenseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptpoolandelevatorexpense> PtasAptpoolandelevatorexpenseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptpoolandelevatorexpense> PtasAptpoolandelevatorexpenseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptpoolandelevatorexpense> PtasAptpoolandelevatorexpenseOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptrentmodel> PtasAptrentmodelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptrentmodel> PtasAptrentmodelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptrentmodel> PtasAptrentmodelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptrentmodel> PtasAptrentmodelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptrentmodel> PtasAptrentmodelOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptsalesmodel> PtasAptsalesmodelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptsalesmodel> PtasAptsalesmodelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptsalesmodel> PtasAptsalesmodelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptsalesmodel> PtasAptsalesmodelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptsalesmodel> PtasAptsalesmodelOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasApttrending> PtasApttrendingCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasApttrending> PtasApttrendingCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasApttrending> PtasApttrendingModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasApttrending> PtasApttrendingModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasApttrending> PtasApttrendingOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptunittypeadjustment> PtasAptunittypeadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptunittypeadjustment> PtasAptunittypeadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptunittypeadjustment> PtasAptunittypeadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptunittypeadjustment> PtasAptunittypeadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptunittypeadjustment> PtasAptunittypeadjustmentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluationOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluationPtasAppraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluationPtasUpdatedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluationproject> PtasAptvaluationprojectCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluationproject> PtasAptvaluationprojectCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluationproject> PtasAptvaluationprojectModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluationproject> PtasAptvaluationprojectModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluationproject> PtasAptvaluationprojectOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewqualityadjustment> PtasAptviewqualityadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewqualityadjustment> PtasAptviewqualityadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewqualityadjustment> PtasAptviewqualityadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewqualityadjustment> PtasAptviewqualityadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewrankadjustment> PtasAptviewrankadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewrankadjustment> PtasAptviewrankadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewrankadjustment> PtasAptviewrankadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewrankadjustment> PtasAptviewrankadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewrankadjustment> PtasAptviewrankadjustmentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewtypeadjustment> PtasAptviewtypeadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewtypeadjustment> PtasAptviewtypeadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewtypeadjustment> PtasAptviewtypeadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptviewtypeadjustment> PtasAptviewtypeadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptyearbuiltadjustment> PtasAptyearbuiltadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptyearbuiltadjustment> PtasAptyearbuiltadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptyearbuiltadjustment> PtasAptyearbuiltadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptyearbuiltadjustment> PtasAptyearbuiltadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptyearbuiltadjustment> PtasAptyearbuiltadjustmentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasArcreasoncode> PtasArcreasoncodeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasArcreasoncode> PtasArcreasoncodeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasArcreasoncode> PtasArcreasoncodeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasArcreasoncode> PtasArcreasoncodeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasArcreasoncode> PtasArcreasoncodeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasArea> PtasAreaCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasArea> PtasAreaCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasArea> PtasAreaModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasArea> PtasAreaModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasArea> PtasAreaPtasAppraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasArea> PtasAreaPtasSeniorappraiserValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionPtasApprovalappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionPtasPostedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionPtasResponsibleappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassificationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassificationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassificationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassificationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassificationOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcodeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcodeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcodeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcodeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcodeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmarkOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBookmarktag> PtasBookmarktagCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmarktag> PtasBookmarktagCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmarktag> PtasBookmarktagModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmarktag> PtasBookmarktagModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBookmarktag> PtasBookmarktagOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuseOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeatureCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeatureCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeatureModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeatureModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeatureOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionuse> PtasBuildingsectionuseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionuse> PtasBuildingsectionuseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionuse> PtasBuildingsectionuseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionuse> PtasBuildingsectionuseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCaprate> PtasCaprateCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCaprate> PtasCaprateCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCaprate> PtasCaprateModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCaprate> PtasCaprateModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasChangereason> PtasChangereasonCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasChangereason> PtasChangereasonCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasChangereason> PtasChangereasonModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasChangereason> PtasChangereasonModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasChangereason> PtasChangereasonOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCity> PtasCityCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCity> PtasCityCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCity> PtasCityModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCity> PtasCityModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounitCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounitCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounitModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounitModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounitOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounitPtasSelectbyidValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounitPtasUnitinspectedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreductionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreductionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreductionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreductionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreductionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasContaminationproject> PtasContaminationprojectCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminationproject> PtasContaminationprojectCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminationproject> PtasContaminationprojectModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminationproject> PtasContaminationprojectModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasContaminationproject> PtasContaminationprojectOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCountry> PtasCountryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCountry> PtasCountryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCountry> PtasCountryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCountry> PtasCountryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCounty> PtasCountyCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCounty> PtasCountyCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCounty> PtasCountyModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCounty> PtasCountyModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplicationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplicationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplicationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplicationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplicationOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcelOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxstatement> PtasCurrentusebacktaxstatementCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxstatement> PtasCurrentusebacktaxstatementCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxstatement> PtasCurrentusebacktaxstatementModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxstatement> PtasCurrentusebacktaxstatementModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxstatement> PtasCurrentusebacktaxstatementOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyearCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyearCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyearModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyearModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyearOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusefarmyieldhistory> PtasCurrentusefarmyieldhistoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusefarmyieldhistory> PtasCurrentusefarmyieldhistoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusefarmyieldhistory> PtasCurrentusefarmyieldhistoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusefarmyieldhistory> PtasCurrentusefarmyieldhistoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusegroup> PtasCurrentusegroupCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusegroup> PtasCurrentusegroupCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusegroup> PtasCurrentusegroupModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusegroup> PtasCurrentusegroupModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenoteCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenoteCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenoteModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenoteModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenoteOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseparcel2> PtasCurrentuseparcel2CreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseparcel2> PtasCurrentuseparcel2CreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseparcel2> PtasCurrentuseparcel2ModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseparcel2> PtasCurrentuseparcel2ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseparcel2> PtasCurrentuseparcel2OwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusepbrsresource> PtasCurrentusepbrsresourceCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusepbrsresource> PtasCurrentusepbrsresourceCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusepbrsresource> PtasCurrentusepbrsresourceModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusepbrsresource> PtasCurrentusepbrsresourceModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusetask> PtasCurrentusetaskCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusetask> PtasCurrentusetaskCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusetask> PtasCurrentusetaskModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusetask> PtasCurrentusetaskModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusetask> PtasCurrentusetaskOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasDepreciationtable> PtasDepreciationtableCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasDepreciationtable> PtasDepreciationtableCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasDepreciationtable> PtasDepreciationtableModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasDepreciationtable> PtasDepreciationtableModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasDepreciationtable> PtasDepreciationtableOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasDesignationtype> PtasDesignationtypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasDesignationtype> PtasDesignationtypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasDesignationtype> PtasDesignationtypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasDesignationtype> PtasDesignationtypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasDesignationtype> PtasDesignationtypeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasDistrict> PtasDistrictCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasDistrict> PtasDistrictCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasDistrict> PtasDistrictModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasDistrict> PtasDistrictModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbscostcenter> PtasEbscostcenterCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbscostcenter> PtasEbscostcenterCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbscostcenter> PtasEbscostcenterModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbscostcenter> PtasEbscostcenterModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsfundnumber> PtasEbsfundnumberCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsfundnumber> PtasEbsfundnumberCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsfundnumber> PtasEbsfundnumberModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsfundnumber> PtasEbsfundnumberModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsmainaccount> PtasEbsmainaccountCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsmainaccount> PtasEbsmainaccountCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsmainaccount> PtasEbsmainaccountModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsmainaccount> PtasEbsmainaccountModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsproject> PtasEbsprojectCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsproject> PtasEbsprojectCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsproject> PtasEbsprojectModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEbsproject> PtasEbsprojectModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEconomicunit> PtasEconomicunitCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEconomicunit> PtasEconomicunitCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEconomicunit> PtasEconomicunitModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEconomicunit> PtasEconomicunitModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEconomicunit> PtasEconomicunitOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestrictionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestrictionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestrictionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestrictionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestrictionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestrictiontype> PtasEnvironmentalrestrictiontypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestrictiontype> PtasEnvironmentalrestrictiontypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestrictiontype> PtasEnvironmentalrestrictiontypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestrictiontype> PtasEnvironmentalrestrictiontypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasExemption> PtasExemptionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasExemption> PtasExemptionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasExemption> PtasExemptionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasExemption> PtasExemptionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasExemption> PtasExemptionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataPtasLoadbyidValueNavigation { get; set; }
        public virtual ICollection<PtasFund> PtasFundCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFund> PtasFundCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFund> PtasFundModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFund> PtasFundModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFund> PtasFundOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasFundallocation> PtasFundallocationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundallocation> PtasFundallocationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundallocation> PtasFundallocationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundallocation> PtasFundallocationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundallocation> PtasFundallocationOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasFundtype> PtasFundtypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundtype> PtasFundtypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundtype> PtasFundtypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundtype> PtasFundtypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasFundtype> PtasFundtypeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasGeoarea> PtasGeoareaCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGeoarea> PtasGeoareaCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasGeoarea> PtasGeoareaModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGeoarea> PtasGeoareaModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasGeoarea> PtasGeoareaPtasAppraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasGeoarea> PtasGeoareaPtasSeniorappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasGeoneighborhood> PtasGeoneighborhoodCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGeoneighborhood> PtasGeoneighborhoodCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasGeoneighborhood> PtasGeoneighborhoodModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGeoneighborhood> PtasGeoneighborhoodModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasGovtaxpayername> PtasGovtaxpayernameCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGovtaxpayername> PtasGovtaxpayernameCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasGovtaxpayername> PtasGovtaxpayernameModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGovtaxpayername> PtasGovtaxpayernameModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasGradestratificationmapping> PtasGradestratificationmappingCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGradestratificationmapping> PtasGradestratificationmappingCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasGradestratificationmapping> PtasGradestratificationmappingModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasGradestratificationmapping> PtasGradestratificationmappingModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovementCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovementCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovementModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovementModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovementOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasHousingprogram> PtasHousingprogramCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasHousingprogram> PtasHousingprogramCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasHousingprogram> PtasHousingprogramModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasHousingprogram> PtasHousingprogramModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasIndustry> PtasIndustryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasIndustry> PtasIndustryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasIndustry> PtasIndustryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasIndustry> PtasIndustryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasIndustry> PtasIndustryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistoryPtasInspectedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyearCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyearCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyearModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyearModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyearOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdiction> PtasJurisdictionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdiction> PtasJurisdictionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdiction> PtasJurisdictionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdiction> PtasJurisdictionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdictioncontact> PtasJurisdictioncontactCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdictioncontact> PtasJurisdictioncontactCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdictioncontact> PtasJurisdictioncontactModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdictioncontact> PtasJurisdictioncontactModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdictioncontact> PtasJurisdictioncontactOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasLand> PtasLandCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLand> PtasLandCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLand> PtasLandModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLand> PtasLandModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLand> PtasLandOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasLanduse> PtasLanduseCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLanduse> PtasLanduseCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLanduse> PtasLanduseModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLanduse> PtasLanduseModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdownCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdownCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdownModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdownModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdownOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculationOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasLevycode> PtasLevycodeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycode> PtasLevycodeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycode> PtasLevycodeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycode> PtasLevycodeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbondCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbondCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbondModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbondModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbondOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequestCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequestCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequestModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequestModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequestOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasLowincomehousingprogram> PtasLowincomehousingprogramCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLowincomehousingprogram> PtasLowincomehousingprogramCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLowincomehousingprogram> PtasLowincomehousingprogramModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasLowincomehousingprogram> PtasLowincomehousingprogramModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasLowincomehousingprogram> PtasLowincomehousingprogramOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberdetail> PtasMajornumberdetailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberdetail> PtasMajornumberdetailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberdetail> PtasMajornumberdetailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberdetail> PtasMajornumberdetailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberdetail> PtasMajornumberdetailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberindex> PtasMajornumberindexCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberindex> PtasMajornumberindexCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberindex> PtasMajornumberindexModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMajornumberindex> PtasMajornumberindexModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaccumulator> PtasMasspayaccumulatorCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaccumulator> PtasMasspayaccumulatorCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaccumulator> PtasMasspayaccumulatorModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaccumulator> PtasMasspayaccumulatorModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaccumulator> PtasMasspayaccumulatorOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaction> PtasMasspayactionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaction> PtasMasspayactionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaction> PtasMasspayactionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaction> PtasMasspayactionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayaction> PtasMasspayactionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayer> PtasMasspayerCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayer> PtasMasspayerCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayer> PtasMasspayerModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayer> PtasMasspayerModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMasspayer> PtasMasspayerOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepositoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepositoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepositoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepositoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepositoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasMedicareplan> PtasMedicareplanCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMedicareplan> PtasMedicareplanCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasMedicareplan> PtasMedicareplanModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasMedicareplan> PtasMedicareplanModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNaicscode> PtasNaicscodeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNaicscode> PtasNaicscodeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNaicscode> PtasNaicscodeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNaicscode> PtasNaicscodeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNeighborhood> PtasNeighborhoodCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNeighborhood> PtasNeighborhoodCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNeighborhood> PtasNeighborhoodModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNeighborhood> PtasNeighborhoodModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNotificationconfiguration> PtasNotificationconfigurationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNotificationconfiguration> PtasNotificationconfigurationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNotificationconfiguration> PtasNotificationconfigurationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNotificationconfiguration> PtasNotificationconfigurationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNotificationconfiguration> PtasNotificationconfigurationOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasNuisancetype> PtasNuisancetypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNuisancetype> PtasNuisancetypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNuisancetype> PtasNuisancetypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasNuisancetype> PtasNuisancetypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasNuisancetype> PtasNuisancetypeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailPtasAssignedappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailPtasLandinspectedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailPtasParcelinspectedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetailPtasSpecialtyappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunitCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunitCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunitModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunitModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunitOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasParcelmetadata> PtasParcelmetadataCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParcelmetadata> PtasParcelmetadataCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParcelmetadata> PtasParcelmetadataModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParcelmetadata> PtasParcelmetadataModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParcelmetadata> PtasParcelmetadataOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasParkingdistrict> PtasParkingdistrictCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParkingdistrict> PtasParkingdistrictCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParkingdistrict> PtasParkingdistrictModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasParkingdistrict> PtasParkingdistrictModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasParkingdistrict> PtasParkingdistrictOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceiptCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceiptCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceiptModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceiptModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceiptOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPbrspointlevel> PtasPbrspointlevelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPbrspointlevel> PtasPbrspointlevelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPbrspointlevel> PtasPbrspointlevelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPbrspointlevel> PtasPbrspointlevelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPbrsresourcetype> PtasPbrsresourcetypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPbrsresourcetype> PtasPbrsresourcetypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPbrsresourcetype> PtasPbrsresourcetypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPbrsresourcetype> PtasPbrsresourcetypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitPtasReviewedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitPtasStatusupdatedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPermitwebsiteconfig> PtasPermitwebsiteconfigCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitwebsiteconfig> PtasPermitwebsiteconfigCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitwebsiteconfig> PtasPermitwebsiteconfigModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitwebsiteconfig> PtasPermitwebsiteconfigModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPermitwebsiteconfig> PtasPermitwebsiteconfigOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAccountcreatedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAccountmanageridValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasAuditedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasDiscoveredbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasDiscoveryauditbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyassetCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyassetCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyassetModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyassetModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyassetOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertycategory> PtasPersonalpropertycategoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertycategory> PtasPersonalpropertycategoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertycategory> PtasPersonalpropertycategoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertycategory> PtasPersonalpropertycategoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertycategory> PtasPersonalpropertycategoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasAccountmanageruseridValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasAuditedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasAuditedbyuseridValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasDiscoveredbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasDiscoveredbyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertylisting> PtasPersonalpropertylistingCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertylisting> PtasPersonalpropertylistingCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertylisting> PtasPersonalpropertylistingModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertylisting> PtasPersonalpropertylistingModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertylisting> PtasPersonalpropertylistingOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertynote> PtasPersonalpropertynoteCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertynote> PtasPersonalpropertynoteCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertynote> PtasPersonalpropertynoteModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertynote> PtasPersonalpropertynoteModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertynote> PtasPersonalpropertynoteOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPerspropbannerannouncement> PtasPerspropbannerannouncementCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPerspropbannerannouncement> PtasPerspropbannerannouncementCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPerspropbannerannouncement> PtasPerspropbannerannouncementModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPerspropbannerannouncement> PtasPerspropbannerannouncementModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPerspropbannerannouncement> PtasPerspropbannerannouncementOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPhonenumber> PtasPhonenumberCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPhonenumber> PtasPhonenumberCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPhonenumber> PtasPhonenumberModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPhonenumber> PtasPhonenumberModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPhonenumber> PtasPhonenumberOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPortalcontact> PtasPortalcontactCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalcontact> PtasPortalcontactCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalcontact> PtasPortalcontactModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalcontact> PtasPortalcontactModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalcontact> PtasPortalcontactOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPortalemail> PtasPortalemailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalemail> PtasPortalemailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalemail> PtasPortalemailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalemail> PtasPortalemailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPortalemail> PtasPortalemailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasProjectdock> PtasProjectdockCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasProjectdock> PtasProjectdockCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasProjectdock> PtasProjectdockModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasProjectdock> PtasProjectdockModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertyreview> PtasPropertyreviewCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertyreview> PtasPropertyreviewCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertyreview> PtasPropertyreviewModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertyreview> PtasPropertyreviewModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertyreview> PtasPropertyreviewOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasPropertytype> PtasPropertytypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertytype> PtasPropertytypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertytype> PtasPropertytypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPropertytype> PtasPropertytypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtasconfiguration> PtasPtasconfigurationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtasconfiguration> PtasPtasconfigurationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtasconfiguration> PtasPtasconfigurationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtasconfiguration> PtasPtasconfigurationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtasconfiguration> PtasPtasconfigurationPtasDefaultsendfromidValueNavigation { get; set; }
        public virtual ICollection<PtasPtasconfiguration> PtasPtasconfigurationPtasSendsrexemptsyncemailtoValueNavigation { get; set; }
        public virtual ICollection<PtasPtassetting> PtasPtassettingCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtassetting> PtasPtassettingCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtassetting> PtasPtassettingModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtassetting> PtasPtassettingModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasPtassetting> PtasPtassettingOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasQstr> PtasQstrCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasQstr> PtasQstrCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasQstr> PtasQstrModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasQstr> PtasQstrModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasProcesseduseridValueNavigation { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasRecentparcel> PtasRecentparcelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRecentparcel> PtasRecentparcelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRecentparcel> PtasRecentparcelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRecentparcel> PtasRecentparcelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRecentparcel> PtasRecentparcelOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetitionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetitionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetitionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetitionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetitionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetitionlevyrate> PtasRefundpetitionlevyrateCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetitionlevyrate> PtasRefundpetitionlevyrateCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetitionlevyrate> PtasRefundpetitionlevyrateModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetitionlevyrate> PtasRefundpetitionlevyrateModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetitionlevyrate> PtasRefundpetitionlevyrateOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasResidentialappraiserteam> PtasResidentialappraiserteamCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasResidentialappraiserteam> PtasResidentialappraiserteamCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasResidentialappraiserteam> PtasResidentialappraiserteamModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasResidentialappraiserteam> PtasResidentialappraiserteamModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasResidentialappraiserteam> PtasResidentialappraiserteamOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasResponsibility> PtasResponsibilityCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasResponsibility> PtasResponsibilityCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasResponsibility> PtasResponsibilityModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasResponsibility> PtasResponsibilityModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRestrictedrent> PtasRestrictedrentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRestrictedrent> PtasRestrictedrentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRestrictedrent> PtasRestrictedrentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasRestrictedrent> PtasRestrictedrentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasRestrictedrent> PtasRestrictedrentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSalepriceadjustment> PtasSalepriceadjustmentCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalepriceadjustment> PtasSalepriceadjustmentCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalepriceadjustment> PtasSalepriceadjustmentModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalepriceadjustment> PtasSalepriceadjustmentModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalepriceadjustment> PtasSalepriceadjustmentOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregateCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregateCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregateModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregateModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregateOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuildingCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuildingCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuildingModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuildingModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuildingOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSalesnote> PtasSalesnoteCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesnote> PtasSalesnoteCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesnote> PtasSalesnoteModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesnote> PtasSalesnoteModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesnote> PtasSalesnoteOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcelCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcelCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcelModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcelModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcelOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSaleswarningcode> PtasSaleswarningcodeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSaleswarningcode> PtasSaleswarningcodeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSaleswarningcode> PtasSaleswarningcodeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSaleswarningcode> PtasSaleswarningcodeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasScheduledworkflow> PtasScheduledworkflowCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasScheduledworkflow> PtasScheduledworkflowCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasScheduledworkflow> PtasScheduledworkflowModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasScheduledworkflow> PtasScheduledworkflowModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasScheduledworkflow> PtasScheduledworkflowOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailPtasCompletedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationPtasCompletedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtaskCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtaskCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtaskModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtaskModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtaskOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnoteCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnoteCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnoteModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnoteModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnoteOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeappoccupant> PtasSeappoccupantCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappoccupant> PtasSeappoccupantCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappoccupant> PtasSeappoccupantModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappoccupant> PtasSeappoccupantModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappoccupant> PtasSeappoccupantOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeappotherprop> PtasSeappotherpropCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappotherprop> PtasSeappotherpropCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappotherprop> PtasSeappotherpropModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappotherprop> PtasSeappotherpropModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeappotherprop> PtasSeappotherpropOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqftCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqftCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqftModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqftModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqftOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibility> PtasSeeligibilityCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibility> PtasSeeligibilityCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibility> PtasSeeligibilityModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibility> PtasSeeligibilityModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibility> PtasSeeligibilityOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibilitybracket> PtasSeeligibilitybracketCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibilitybracket> PtasSeeligibilitybracketCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibilitybracket> PtasSeeligibilitybracketModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibilitybracket> PtasSeeligibilitybracketModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibilitybracket> PtasSeeligibilitybracketOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSeexemptionreason> PtasSeexemptionreasonCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeexemptionreason> PtasSeexemptionreasonCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeexemptionreason> PtasSeexemptionreasonModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSeexemptionreason> PtasSeexemptionreasonModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalueCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalueCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalueModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalueModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalueOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketchCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketchCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketchModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketchModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketchOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketchPtasDrawauthoridValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketchPtasLockedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyarea> PtasSpecialtyareaCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyarea> PtasSpecialtyareaCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyarea> PtasSpecialtyareaModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyarea> PtasSpecialtyareaModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyarea> PtasSpecialtyareaPtasSeniorappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyneighborhood> PtasSpecialtyneighborhoodCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyneighborhood> PtasSpecialtyneighborhoodCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyneighborhood> PtasSpecialtyneighborhoodModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyneighborhood> PtasSpecialtyneighborhoodModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSpecialtyneighborhood> PtasSpecialtyneighborhoodPtasAppraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasStateorprovince> PtasStateorprovinceCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateorprovince> PtasStateorprovinceCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateorprovince> PtasStateorprovinceModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateorprovince> PtasStateorprovinceModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateutilityvalue> PtasStateutilityvalueCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateutilityvalue> PtasStateutilityvalueCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateutilityvalue> PtasStateutilityvalueModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateutilityvalue> PtasStateutilityvalueModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasStateutilityvalue> PtasStateutilityvalueOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasStreetname> PtasStreetnameCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStreetname> PtasStreetnameCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasStreetname> PtasStreetnameModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStreetname> PtasStreetnameModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasStreettype> PtasStreettypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStreettype> PtasStreettypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasStreettype> PtasStreettypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasStreettype> PtasStreettypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubarea> PtasSubareaCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubarea> PtasSubareaCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubarea> PtasSubareaModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubarea> PtasSubareaModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubmarket> PtasSubmarketCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubmarket> PtasSubmarketCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubmarket> PtasSubmarketModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSubmarket> PtasSubmarketModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSupergroup> PtasSupergroupCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSupergroup> PtasSupergroupCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasSupergroup> PtasSupergroupModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasSupergroup> PtasSupergroupModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskPtasAccountingsectionsupervisorValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskPtasAppraiserValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskPtasCommercialsrappraiserValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTaskPtasResidentialsrappraiserValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccountCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccountCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccountModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccountModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccountOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbillCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbillCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbillModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbillModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbillOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionPtasRequestedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionPtasReviewedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrectionPtasSeniorrevieweridValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalueCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalueCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalueModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalueModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalueOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasTimberusetype> PtasTimberusetypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTimberusetype> PtasTimberusetypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTimberusetype> PtasTimberusetypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTimberusetype> PtasTimberusetypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTrendfactor> PtasTrendfactorCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTrendfactor> PtasTrendfactorCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTrendfactor> PtasTrendfactorModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasTrendfactor> PtasTrendfactorModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasTrendfactor> PtasTrendfactorOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdownCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdownCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdownModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdownModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdownOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdowntype> PtasUnitbreakdowntypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdowntype> PtasUnitbreakdowntypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdowntype> PtasUnitbreakdowntypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdowntype> PtasUnitbreakdowntypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistoryCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistoryCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistoryModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistoryModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistoryOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasViewtype> PtasViewtypeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasViewtype> PtasViewtypeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasViewtype> PtasViewtypeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasViewtype> PtasViewtypeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasViewtype> PtasViewtypeOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketchCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketchCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketchModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketchModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketchOwninguserValueNavigation { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketchPtasVisitedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasYear> PtasYearCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasYear> PtasYearCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasYear> PtasYearModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasYear> PtasYearModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasYear> PtasYearPtasRollovernotificationidValueNavigation { get; set; }
        public virtual ICollection<PtasZipcode> PtasZipcodeCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasZipcode> PtasZipcodeCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasZipcode> PtasZipcodeModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasZipcode> PtasZipcodeModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasZoning> PtasZoningCreatedbyValueNavigation { get; set; }
        public virtual ICollection<PtasZoning> PtasZoningCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasZoning> PtasZoningModifiedbyValueNavigation { get; set; }
        public virtual ICollection<PtasZoning> PtasZoningModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Role> RoleCreatedbyValueNavigation { get; set; }
        public virtual ICollection<Role> RoleCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Role> RoleModifiedbyValueNavigation { get; set; }
        public virtual ICollection<Role> RoleModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Team> TeamAdministratoridValueNavigation { get; set; }
        public virtual ICollection<Team> TeamCreatedbyValueNavigation { get; set; }
        public virtual ICollection<Team> TeamCreatedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Team> TeamModifiedbyValueNavigation { get; set; }
        public virtual ICollection<Team> TeamModifiedonbehalfbyValueNavigation { get; set; }
    }
}
