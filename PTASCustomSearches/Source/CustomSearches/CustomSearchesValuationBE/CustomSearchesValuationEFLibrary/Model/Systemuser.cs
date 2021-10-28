using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class systemuser
    {
        public systemuser()
        {
            Inverse_createdby_valueNavigation = new HashSet<systemuser>();
            Inverse_createdonbehalfby_valueNavigation = new HashSet<systemuser>();
            Inverse_modifiedby_valueNavigation = new HashSet<systemuser>();
            Inverse_modifiedonbehalfby_valueNavigation = new HashSet<systemuser>();
            Inverse_parentsystemuserid_valueNavigation = new HashSet<systemuser>();
            ptas_accessorydetail_createdby_valueNavigation = new HashSet<ptas_accessorydetail>();
            ptas_accessorydetail_createdonbehalfby_valueNavigation = new HashSet<ptas_accessorydetail>();
            ptas_accessorydetail_modifiedby_valueNavigation = new HashSet<ptas_accessorydetail>();
            ptas_accessorydetail_modifiedonbehalfby_valueNavigation = new HashSet<ptas_accessorydetail>();
            ptas_accessorydetail_owninguser_valueNavigation = new HashSet<ptas_accessorydetail>();
            ptas_addresschangehistory_createdby_valueNavigation = new HashSet<ptas_addresschangehistory>();
            ptas_addresschangehistory_createdonbehalfby_valueNavigation = new HashSet<ptas_addresschangehistory>();
            ptas_addresschangehistory_modifiedby_valueNavigation = new HashSet<ptas_addresschangehistory>();
            ptas_addresschangehistory_modifiedonbehalfby_valueNavigation = new HashSet<ptas_addresschangehistory>();
            ptas_addresschangehistory_owninguser_valueNavigation = new HashSet<ptas_addresschangehistory>();
            ptas_annexationparcelreview_createdby_valueNavigation = new HashSet<ptas_annexationparcelreview>();
            ptas_annexationparcelreview_createdonbehalfby_valueNavigation = new HashSet<ptas_annexationparcelreview>();
            ptas_annexationparcelreview_modifiedby_valueNavigation = new HashSet<ptas_annexationparcelreview>();
            ptas_annexationparcelreview_modifiedonbehalfby_valueNavigation = new HashSet<ptas_annexationparcelreview>();
            ptas_annexationparcelreview_owninguser_valueNavigation = new HashSet<ptas_annexationparcelreview>();
            ptas_annexationtracker_createdby_valueNavigation = new HashSet<ptas_annexationtracker>();
            ptas_annexationtracker_createdonbehalfby_valueNavigation = new HashSet<ptas_annexationtracker>();
            ptas_annexationtracker_modifiedby_valueNavigation = new HashSet<ptas_annexationtracker>();
            ptas_annexationtracker_modifiedonbehalfby_valueNavigation = new HashSet<ptas_annexationtracker>();
            ptas_annexationtracker_owninguser_valueNavigation = new HashSet<ptas_annexationtracker>();
            ptas_annualcostdistribution_createdby_valueNavigation = new HashSet<ptas_annualcostdistribution>();
            ptas_annualcostdistribution_createdonbehalfby_valueNavigation = new HashSet<ptas_annualcostdistribution>();
            ptas_annualcostdistribution_modifiedby_valueNavigation = new HashSet<ptas_annualcostdistribution>();
            ptas_annualcostdistribution_modifiedonbehalfby_valueNavigation = new HashSet<ptas_annualcostdistribution>();
            ptas_annualcostdistribution_owninguser_valueNavigation = new HashSet<ptas_annualcostdistribution>();
            ptas_apartmentregion_createdby_valueNavigation = new HashSet<ptas_apartmentregion>();
            ptas_apartmentregion_createdonbehalfby_valueNavigation = new HashSet<ptas_apartmentregion>();
            ptas_apartmentregion_modifiedby_valueNavigation = new HashSet<ptas_apartmentregion>();
            ptas_apartmentregion_modifiedonbehalfby_valueNavigation = new HashSet<ptas_apartmentregion>();
            ptas_apartmentsupergroup_createdby_valueNavigation = new HashSet<ptas_apartmentsupergroup>();
            ptas_apartmentsupergroup_createdonbehalfby_valueNavigation = new HashSet<ptas_apartmentsupergroup>();
            ptas_apartmentsupergroup_modifiedby_valueNavigation = new HashSet<ptas_apartmentsupergroup>();
            ptas_apartmentsupergroup_modifiedonbehalfby_valueNavigation = new HashSet<ptas_apartmentsupergroup>();
            ptas_aptadjustedlevyrate_createdby_valueNavigation = new HashSet<ptas_aptadjustedlevyrate>();
            ptas_aptadjustedlevyrate_createdonbehalfby_valueNavigation = new HashSet<ptas_aptadjustedlevyrate>();
            ptas_aptadjustedlevyrate_modifiedby_valueNavigation = new HashSet<ptas_aptadjustedlevyrate>();
            ptas_aptadjustedlevyrate_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptadjustedlevyrate>();
            ptas_aptadjustedlevyrate_owninguser_valueNavigation = new HashSet<ptas_aptadjustedlevyrate>();
            ptas_aptavailablecomparablesale_createdby_valueNavigation = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_aptavailablecomparablesale_createdonbehalfby_valueNavigation = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_aptavailablecomparablesale_modifiedby_valueNavigation = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_aptavailablecomparablesale_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_aptavailablecomparablesale_owninguser_valueNavigation = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_aptbuildingqualityadjustment_createdby_valueNavigation = new HashSet<ptas_aptbuildingqualityadjustment>();
            ptas_aptbuildingqualityadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptbuildingqualityadjustment>();
            ptas_aptbuildingqualityadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptbuildingqualityadjustment>();
            ptas_aptbuildingqualityadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptbuildingqualityadjustment>();
            ptas_aptbuildingqualityadjustment_owninguser_valueNavigation = new HashSet<ptas_aptbuildingqualityadjustment>();
            ptas_aptcloseproximity_createdby_valueNavigation = new HashSet<ptas_aptcloseproximity>();
            ptas_aptcloseproximity_createdonbehalfby_valueNavigation = new HashSet<ptas_aptcloseproximity>();
            ptas_aptcloseproximity_modifiedby_valueNavigation = new HashSet<ptas_aptcloseproximity>();
            ptas_aptcloseproximity_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptcloseproximity>();
            ptas_aptcommercialincomeexpense_createdby_valueNavigation = new HashSet<ptas_aptcommercialincomeexpense>();
            ptas_aptcommercialincomeexpense_createdonbehalfby_valueNavigation = new HashSet<ptas_aptcommercialincomeexpense>();
            ptas_aptcommercialincomeexpense_modifiedby_valueNavigation = new HashSet<ptas_aptcommercialincomeexpense>();
            ptas_aptcommercialincomeexpense_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptcommercialincomeexpense>();
            ptas_aptcommercialincomeexpense_owninguser_valueNavigation = new HashSet<ptas_aptcommercialincomeexpense>();
            ptas_aptcomparablerent_createdby_valueNavigation = new HashSet<ptas_aptcomparablerent>();
            ptas_aptcomparablerent_createdonbehalfby_valueNavigation = new HashSet<ptas_aptcomparablerent>();
            ptas_aptcomparablerent_modifiedby_valueNavigation = new HashSet<ptas_aptcomparablerent>();
            ptas_aptcomparablerent_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptcomparablerent>();
            ptas_aptcomparablerent_owninguser_valueNavigation = new HashSet<ptas_aptcomparablerent>();
            ptas_aptcomparablesale_createdby_valueNavigation = new HashSet<ptas_aptcomparablesale>();
            ptas_aptcomparablesale_createdonbehalfby_valueNavigation = new HashSet<ptas_aptcomparablesale>();
            ptas_aptcomparablesale_modifiedby_valueNavigation = new HashSet<ptas_aptcomparablesale>();
            ptas_aptcomparablesale_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptcomparablesale>();
            ptas_aptcomparablesale_owninguser_valueNavigation = new HashSet<ptas_aptcomparablesale>();
            ptas_aptconditionadjustment_createdby_valueNavigation = new HashSet<ptas_aptconditionadjustment>();
            ptas_aptconditionadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptconditionadjustment>();
            ptas_aptconditionadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptconditionadjustment>();
            ptas_aptconditionadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptconditionadjustment>();
            ptas_aptconditionadjustment_owninguser_valueNavigation = new HashSet<ptas_aptconditionadjustment>();
            ptas_aptestimatedunitsqft_createdby_valueNavigation = new HashSet<ptas_aptestimatedunitsqft>();
            ptas_aptestimatedunitsqft_createdonbehalfby_valueNavigation = new HashSet<ptas_aptestimatedunitsqft>();
            ptas_aptestimatedunitsqft_modifiedby_valueNavigation = new HashSet<ptas_aptestimatedunitsqft>();
            ptas_aptestimatedunitsqft_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptestimatedunitsqft>();
            ptas_aptestimatedunitsqft_owninguser_valueNavigation = new HashSet<ptas_aptestimatedunitsqft>();
            ptas_aptexpensehighend_createdby_valueNavigation = new HashSet<ptas_aptexpensehighend>();
            ptas_aptexpensehighend_createdonbehalfby_valueNavigation = new HashSet<ptas_aptexpensehighend>();
            ptas_aptexpensehighend_modifiedby_valueNavigation = new HashSet<ptas_aptexpensehighend>();
            ptas_aptexpensehighend_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptexpensehighend>();
            ptas_aptexpensehighend_owninguser_valueNavigation = new HashSet<ptas_aptexpensehighend>();
            ptas_aptexpenseunitsize_createdby_valueNavigation = new HashSet<ptas_aptexpenseunitsize>();
            ptas_aptexpenseunitsize_createdonbehalfby_valueNavigation = new HashSet<ptas_aptexpenseunitsize>();
            ptas_aptexpenseunitsize_modifiedby_valueNavigation = new HashSet<ptas_aptexpenseunitsize>();
            ptas_aptexpenseunitsize_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptexpenseunitsize>();
            ptas_aptexpenseunitsize_owninguser_valueNavigation = new HashSet<ptas_aptexpenseunitsize>();
            ptas_aptincomeexpense_createdby_valueNavigation = new HashSet<ptas_aptincomeexpense>();
            ptas_aptincomeexpense_createdonbehalfby_valueNavigation = new HashSet<ptas_aptincomeexpense>();
            ptas_aptincomeexpense_modifiedby_valueNavigation = new HashSet<ptas_aptincomeexpense>();
            ptas_aptincomeexpense_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptincomeexpense>();
            ptas_aptlistedrent_createdby_valueNavigation = new HashSet<ptas_aptlistedrent>();
            ptas_aptlistedrent_createdonbehalfby_valueNavigation = new HashSet<ptas_aptlistedrent>();
            ptas_aptlistedrent_modifiedby_valueNavigation = new HashSet<ptas_aptlistedrent>();
            ptas_aptlistedrent_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptlistedrent>();
            ptas_aptlistedrent_owninguser_valueNavigation = new HashSet<ptas_aptlistedrent>();
            ptas_aptneighborhood_createdby_valueNavigation = new HashSet<ptas_aptneighborhood>();
            ptas_aptneighborhood_createdonbehalfby_valueNavigation = new HashSet<ptas_aptneighborhood>();
            ptas_aptneighborhood_modifiedby_valueNavigation = new HashSet<ptas_aptneighborhood>();
            ptas_aptneighborhood_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptneighborhood>();
            ptas_aptnumberofunitsadjustment_createdby_valueNavigation = new HashSet<ptas_aptnumberofunitsadjustment>();
            ptas_aptnumberofunitsadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptnumberofunitsadjustment>();
            ptas_aptnumberofunitsadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptnumberofunitsadjustment>();
            ptas_aptnumberofunitsadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptnumberofunitsadjustment>();
            ptas_aptnumberofunitsadjustment_owninguser_valueNavigation = new HashSet<ptas_aptnumberofunitsadjustment>();
            ptas_aptpoolandelevatorexpense_createdby_valueNavigation = new HashSet<ptas_aptpoolandelevatorexpense>();
            ptas_aptpoolandelevatorexpense_createdonbehalfby_valueNavigation = new HashSet<ptas_aptpoolandelevatorexpense>();
            ptas_aptpoolandelevatorexpense_modifiedby_valueNavigation = new HashSet<ptas_aptpoolandelevatorexpense>();
            ptas_aptpoolandelevatorexpense_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptpoolandelevatorexpense>();
            ptas_aptpoolandelevatorexpense_owninguser_valueNavigation = new HashSet<ptas_aptpoolandelevatorexpense>();
            ptas_aptrentmodel_createdby_valueNavigation = new HashSet<ptas_aptrentmodel>();
            ptas_aptrentmodel_createdonbehalfby_valueNavigation = new HashSet<ptas_aptrentmodel>();
            ptas_aptrentmodel_modifiedby_valueNavigation = new HashSet<ptas_aptrentmodel>();
            ptas_aptrentmodel_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptrentmodel>();
            ptas_aptrentmodel_owninguser_valueNavigation = new HashSet<ptas_aptrentmodel>();
            ptas_aptsalesmodel_createdby_valueNavigation = new HashSet<ptas_aptsalesmodel>();
            ptas_aptsalesmodel_createdonbehalfby_valueNavigation = new HashSet<ptas_aptsalesmodel>();
            ptas_aptsalesmodel_modifiedby_valueNavigation = new HashSet<ptas_aptsalesmodel>();
            ptas_aptsalesmodel_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptsalesmodel>();
            ptas_aptsalesmodel_owninguser_valueNavigation = new HashSet<ptas_aptsalesmodel>();
            ptas_apttrending_createdby_valueNavigation = new HashSet<ptas_apttrending>();
            ptas_apttrending_createdonbehalfby_valueNavigation = new HashSet<ptas_apttrending>();
            ptas_apttrending_modifiedby_valueNavigation = new HashSet<ptas_apttrending>();
            ptas_apttrending_modifiedonbehalfby_valueNavigation = new HashSet<ptas_apttrending>();
            ptas_apttrending_owninguser_valueNavigation = new HashSet<ptas_apttrending>();
            ptas_aptunittypeadjustment_createdby_valueNavigation = new HashSet<ptas_aptunittypeadjustment>();
            ptas_aptunittypeadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptunittypeadjustment>();
            ptas_aptunittypeadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptunittypeadjustment>();
            ptas_aptunittypeadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptunittypeadjustment>();
            ptas_aptunittypeadjustment_owninguser_valueNavigation = new HashSet<ptas_aptunittypeadjustment>();
            ptas_aptvaluation_createdby_valueNavigation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluation_createdonbehalfby_valueNavigation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluation_modifiedby_valueNavigation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluation_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluation_owninguser_valueNavigation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluation_ptas_appraiserid_valueNavigation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluation_ptas_updatedbyid_valueNavigation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluationproject_createdby_valueNavigation = new HashSet<ptas_aptvaluationproject>();
            ptas_aptvaluationproject_createdonbehalfby_valueNavigation = new HashSet<ptas_aptvaluationproject>();
            ptas_aptvaluationproject_modifiedby_valueNavigation = new HashSet<ptas_aptvaluationproject>();
            ptas_aptvaluationproject_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptvaluationproject>();
            ptas_aptvaluationproject_owninguser_valueNavigation = new HashSet<ptas_aptvaluationproject>();
            ptas_aptviewqualityadjustment_createdby_valueNavigation = new HashSet<ptas_aptviewqualityadjustment>();
            ptas_aptviewqualityadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptviewqualityadjustment>();
            ptas_aptviewqualityadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptviewqualityadjustment>();
            ptas_aptviewqualityadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptviewqualityadjustment>();
            ptas_aptviewrankadjustment_createdby_valueNavigation = new HashSet<ptas_aptviewrankadjustment>();
            ptas_aptviewrankadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptviewrankadjustment>();
            ptas_aptviewrankadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptviewrankadjustment>();
            ptas_aptviewrankadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptviewrankadjustment>();
            ptas_aptviewrankadjustment_owninguser_valueNavigation = new HashSet<ptas_aptviewrankadjustment>();
            ptas_aptviewtypeadjustment_createdby_valueNavigation = new HashSet<ptas_aptviewtypeadjustment>();
            ptas_aptviewtypeadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptviewtypeadjustment>();
            ptas_aptviewtypeadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptviewtypeadjustment>();
            ptas_aptviewtypeadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptviewtypeadjustment>();
            ptas_aptyearbuiltadjustment_createdby_valueNavigation = new HashSet<ptas_aptyearbuiltadjustment>();
            ptas_aptyearbuiltadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_aptyearbuiltadjustment>();
            ptas_aptyearbuiltadjustment_modifiedby_valueNavigation = new HashSet<ptas_aptyearbuiltadjustment>();
            ptas_aptyearbuiltadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_aptyearbuiltadjustment>();
            ptas_aptyearbuiltadjustment_owninguser_valueNavigation = new HashSet<ptas_aptyearbuiltadjustment>();
            ptas_arcreasoncode_createdby_valueNavigation = new HashSet<ptas_arcreasoncode>();
            ptas_arcreasoncode_createdonbehalfby_valueNavigation = new HashSet<ptas_arcreasoncode>();
            ptas_arcreasoncode_modifiedby_valueNavigation = new HashSet<ptas_arcreasoncode>();
            ptas_arcreasoncode_modifiedonbehalfby_valueNavigation = new HashSet<ptas_arcreasoncode>();
            ptas_arcreasoncode_owninguser_valueNavigation = new HashSet<ptas_arcreasoncode>();
            ptas_area_createdby_valueNavigation = new HashSet<ptas_area>();
            ptas_area_createdonbehalfby_valueNavigation = new HashSet<ptas_area>();
            ptas_area_modifiedby_valueNavigation = new HashSet<ptas_area>();
            ptas_area_modifiedonbehalfby_valueNavigation = new HashSet<ptas_area>();
            ptas_area_ptas_appraiserid_valueNavigation = new HashSet<ptas_area>();
            ptas_area_ptas_seniorappraiser_valueNavigation = new HashSet<ptas_area>();
            ptas_assessmentrollcorrection_createdby_valueNavigation = new HashSet<ptas_assessmentrollcorrection>();
            ptas_assessmentrollcorrection_createdonbehalfby_valueNavigation = new HashSet<ptas_assessmentrollcorrection>();
            ptas_assessmentrollcorrection_modifiedby_valueNavigation = new HashSet<ptas_assessmentrollcorrection>();
            ptas_assessmentrollcorrection_modifiedonbehalfby_valueNavigation = new HashSet<ptas_assessmentrollcorrection>();
            ptas_assessmentrollcorrection_owninguser_valueNavigation = new HashSet<ptas_assessmentrollcorrection>();
            ptas_assessmentrollcorrection_ptas_approvalappraiserid_valueNavigation = new HashSet<ptas_assessmentrollcorrection>();
            ptas_assessmentrollcorrection_ptas_responsibleappraiserid_valueNavigation = new HashSet<ptas_assessmentrollcorrection>();
            ptas_bookmark_createdby_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_createdonbehalfby_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_modifiedby_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_modifiedonbehalfby_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_owninguser_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmarktag_createdby_valueNavigation = new HashSet<ptas_bookmarktag>();
            ptas_bookmarktag_createdonbehalfby_valueNavigation = new HashSet<ptas_bookmarktag>();
            ptas_bookmarktag_modifiedby_valueNavigation = new HashSet<ptas_bookmarktag>();
            ptas_bookmarktag_modifiedonbehalfby_valueNavigation = new HashSet<ptas_bookmarktag>();
            ptas_bookmarktag_owninguser_valueNavigation = new HashSet<ptas_bookmarktag>();
            ptas_buildingdetail_commercialuse_createdby_valueNavigation = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_buildingdetail_commercialuse_createdonbehalfby_valueNavigation = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_buildingdetail_commercialuse_modifiedby_valueNavigation = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_buildingdetail_commercialuse_modifiedonbehalfby_valueNavigation = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_buildingdetail_commercialuse_owninguser_valueNavigation = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_buildingdetail_createdby_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_buildingdetail_createdonbehalfby_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_buildingdetail_modifiedby_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_buildingdetail_modifiedonbehalfby_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_buildingdetail_owninguser_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_buildingsectionfeature_createdby_valueNavigation = new HashSet<ptas_buildingsectionfeature>();
            ptas_buildingsectionfeature_createdonbehalfby_valueNavigation = new HashSet<ptas_buildingsectionfeature>();
            ptas_buildingsectionfeature_modifiedby_valueNavigation = new HashSet<ptas_buildingsectionfeature>();
            ptas_buildingsectionfeature_modifiedonbehalfby_valueNavigation = new HashSet<ptas_buildingsectionfeature>();
            ptas_buildingsectionfeature_owninguser_valueNavigation = new HashSet<ptas_buildingsectionfeature>();
            ptas_buildingsectionuse_createdby_valueNavigation = new HashSet<ptas_buildingsectionuse>();
            ptas_buildingsectionuse_createdonbehalfby_valueNavigation = new HashSet<ptas_buildingsectionuse>();
            ptas_buildingsectionuse_modifiedby_valueNavigation = new HashSet<ptas_buildingsectionuse>();
            ptas_buildingsectionuse_modifiedonbehalfby_valueNavigation = new HashSet<ptas_buildingsectionuse>();
            ptas_changereason_createdby_valueNavigation = new HashSet<ptas_changereason>();
            ptas_changereason_createdonbehalfby_valueNavigation = new HashSet<ptas_changereason>();
            ptas_changereason_modifiedby_valueNavigation = new HashSet<ptas_changereason>();
            ptas_changereason_modifiedonbehalfby_valueNavigation = new HashSet<ptas_changereason>();
            ptas_changereason_owninguser_valueNavigation = new HashSet<ptas_changereason>();
            ptas_city_createdby_valueNavigation = new HashSet<ptas_city>();
            ptas_city_createdonbehalfby_valueNavigation = new HashSet<ptas_city>();
            ptas_city_modifiedby_valueNavigation = new HashSet<ptas_city>();
            ptas_city_modifiedonbehalfby_valueNavigation = new HashSet<ptas_city>();
            ptas_condocomplex_createdby_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condocomplex_createdonbehalfby_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condocomplex_modifiedby_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condocomplex_modifiedonbehalfby_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condocomplex_owninguser_valueNavigation = new HashSet<ptas_condocomplex>();
            ptas_condounit_createdby_valueNavigation = new HashSet<ptas_condounit>();
            ptas_condounit_createdonbehalfby_valueNavigation = new HashSet<ptas_condounit>();
            ptas_condounit_modifiedby_valueNavigation = new HashSet<ptas_condounit>();
            ptas_condounit_modifiedonbehalfby_valueNavigation = new HashSet<ptas_condounit>();
            ptas_condounit_owninguser_valueNavigation = new HashSet<ptas_condounit>();
            ptas_condounit_ptas_selectbyid_valueNavigation = new HashSet<ptas_condounit>();
            ptas_condounit_ptas_unitinspectedbyid_valueNavigation = new HashSet<ptas_condounit>();
            ptas_contaminatedlandreduction_createdby_valueNavigation = new HashSet<ptas_contaminatedlandreduction>();
            ptas_contaminatedlandreduction_createdonbehalfby_valueNavigation = new HashSet<ptas_contaminatedlandreduction>();
            ptas_contaminatedlandreduction_modifiedby_valueNavigation = new HashSet<ptas_contaminatedlandreduction>();
            ptas_contaminatedlandreduction_modifiedonbehalfby_valueNavigation = new HashSet<ptas_contaminatedlandreduction>();
            ptas_contaminatedlandreduction_owninguser_valueNavigation = new HashSet<ptas_contaminatedlandreduction>();
            ptas_contaminationproject_createdby_valueNavigation = new HashSet<ptas_contaminationproject>();
            ptas_contaminationproject_createdonbehalfby_valueNavigation = new HashSet<ptas_contaminationproject>();
            ptas_contaminationproject_modifiedby_valueNavigation = new HashSet<ptas_contaminationproject>();
            ptas_contaminationproject_modifiedonbehalfby_valueNavigation = new HashSet<ptas_contaminationproject>();
            ptas_contaminationproject_owninguser_valueNavigation = new HashSet<ptas_contaminationproject>();
            ptas_country_createdby_valueNavigation = new HashSet<ptas_country>();
            ptas_country_createdonbehalfby_valueNavigation = new HashSet<ptas_country>();
            ptas_country_modifiedby_valueNavigation = new HashSet<ptas_country>();
            ptas_country_modifiedonbehalfby_valueNavigation = new HashSet<ptas_country>();
            ptas_county_createdby_valueNavigation = new HashSet<ptas_county>();
            ptas_county_createdonbehalfby_valueNavigation = new HashSet<ptas_county>();
            ptas_county_modifiedby_valueNavigation = new HashSet<ptas_county>();
            ptas_county_modifiedonbehalfby_valueNavigation = new HashSet<ptas_county>();
            ptas_depreciationtable_createdby_valueNavigation = new HashSet<ptas_depreciationtable>();
            ptas_depreciationtable_createdonbehalfby_valueNavigation = new HashSet<ptas_depreciationtable>();
            ptas_depreciationtable_modifiedby_valueNavigation = new HashSet<ptas_depreciationtable>();
            ptas_depreciationtable_modifiedonbehalfby_valueNavigation = new HashSet<ptas_depreciationtable>();
            ptas_depreciationtable_owninguser_valueNavigation = new HashSet<ptas_depreciationtable>();
            ptas_district_createdby_valueNavigation = new HashSet<ptas_district>();
            ptas_district_createdonbehalfby_valueNavigation = new HashSet<ptas_district>();
            ptas_district_modifiedby_valueNavigation = new HashSet<ptas_district>();
            ptas_district_modifiedonbehalfby_valueNavigation = new HashSet<ptas_district>();
            ptas_ebscostcenter_createdby_valueNavigation = new HashSet<ptas_ebscostcenter>();
            ptas_ebscostcenter_createdonbehalfby_valueNavigation = new HashSet<ptas_ebscostcenter>();
            ptas_ebscostcenter_modifiedby_valueNavigation = new HashSet<ptas_ebscostcenter>();
            ptas_ebscostcenter_modifiedonbehalfby_valueNavigation = new HashSet<ptas_ebscostcenter>();
            ptas_ebsfundnumber_createdby_valueNavigation = new HashSet<ptas_ebsfundnumber>();
            ptas_ebsfundnumber_createdonbehalfby_valueNavigation = new HashSet<ptas_ebsfundnumber>();
            ptas_ebsfundnumber_modifiedby_valueNavigation = new HashSet<ptas_ebsfundnumber>();
            ptas_ebsfundnumber_modifiedonbehalfby_valueNavigation = new HashSet<ptas_ebsfundnumber>();
            ptas_ebsmainaccount_createdby_valueNavigation = new HashSet<ptas_ebsmainaccount>();
            ptas_ebsmainaccount_createdonbehalfby_valueNavigation = new HashSet<ptas_ebsmainaccount>();
            ptas_ebsmainaccount_modifiedby_valueNavigation = new HashSet<ptas_ebsmainaccount>();
            ptas_ebsmainaccount_modifiedonbehalfby_valueNavigation = new HashSet<ptas_ebsmainaccount>();
            ptas_ebsproject_createdby_valueNavigation = new HashSet<ptas_ebsproject>();
            ptas_ebsproject_createdonbehalfby_valueNavigation = new HashSet<ptas_ebsproject>();
            ptas_ebsproject_modifiedby_valueNavigation = new HashSet<ptas_ebsproject>();
            ptas_ebsproject_modifiedonbehalfby_valueNavigation = new HashSet<ptas_ebsproject>();
            ptas_economicunit_createdby_valueNavigation = new HashSet<ptas_economicunit>();
            ptas_economicunit_createdonbehalfby_valueNavigation = new HashSet<ptas_economicunit>();
            ptas_economicunit_modifiedby_valueNavigation = new HashSet<ptas_economicunit>();
            ptas_economicunit_modifiedonbehalfby_valueNavigation = new HashSet<ptas_economicunit>();
            ptas_economicunit_owninguser_valueNavigation = new HashSet<ptas_economicunit>();
            ptas_environmentalrestriction_createdby_valueNavigation = new HashSet<ptas_environmentalrestriction>();
            ptas_environmentalrestriction_createdonbehalfby_valueNavigation = new HashSet<ptas_environmentalrestriction>();
            ptas_environmentalrestriction_modifiedby_valueNavigation = new HashSet<ptas_environmentalrestriction>();
            ptas_environmentalrestriction_modifiedonbehalfby_valueNavigation = new HashSet<ptas_environmentalrestriction>();
            ptas_environmentalrestriction_owninguser_valueNavigation = new HashSet<ptas_environmentalrestriction>();
            ptas_fileattachmentmetadata_createdby_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_fileattachmentmetadata_createdonbehalfby_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_fileattachmentmetadata_modifiedby_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_fileattachmentmetadata_modifiedonbehalfby_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_fileattachmentmetadata_owninguser_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_fileattachmentmetadata_ptas_loadbyid_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_geoarea_createdby_valueNavigation = new HashSet<ptas_geoarea>();
            ptas_geoarea_createdonbehalfby_valueNavigation = new HashSet<ptas_geoarea>();
            ptas_geoarea_modifiedby_valueNavigation = new HashSet<ptas_geoarea>();
            ptas_geoarea_modifiedonbehalfby_valueNavigation = new HashSet<ptas_geoarea>();
            ptas_geoarea_ptas_appraiserid_valueNavigation = new HashSet<ptas_geoarea>();
            ptas_geoarea_ptas_seniorappraiserid_valueNavigation = new HashSet<ptas_geoarea>();
            ptas_geoneighborhood_createdby_valueNavigation = new HashSet<ptas_geoneighborhood>();
            ptas_geoneighborhood_createdonbehalfby_valueNavigation = new HashSet<ptas_geoneighborhood>();
            ptas_geoneighborhood_modifiedby_valueNavigation = new HashSet<ptas_geoneighborhood>();
            ptas_geoneighborhood_modifiedonbehalfby_valueNavigation = new HashSet<ptas_geoneighborhood>();
            ptas_govtaxpayername_createdby_valueNavigation = new HashSet<ptas_govtaxpayername>();
            ptas_govtaxpayername_createdonbehalfby_valueNavigation = new HashSet<ptas_govtaxpayername>();
            ptas_govtaxpayername_modifiedby_valueNavigation = new HashSet<ptas_govtaxpayername>();
            ptas_govtaxpayername_modifiedonbehalfby_valueNavigation = new HashSet<ptas_govtaxpayername>();
            ptas_gradestratificationmapping_createdby_valueNavigation = new HashSet<ptas_gradestratificationmapping>();
            ptas_gradestratificationmapping_createdonbehalfby_valueNavigation = new HashSet<ptas_gradestratificationmapping>();
            ptas_gradestratificationmapping_modifiedby_valueNavigation = new HashSet<ptas_gradestratificationmapping>();
            ptas_gradestratificationmapping_modifiedonbehalfby_valueNavigation = new HashSet<ptas_gradestratificationmapping>();
            ptas_homeimprovement_createdby_valueNavigation = new HashSet<ptas_homeimprovement>();
            ptas_homeimprovement_createdonbehalfby_valueNavigation = new HashSet<ptas_homeimprovement>();
            ptas_homeimprovement_modifiedby_valueNavigation = new HashSet<ptas_homeimprovement>();
            ptas_homeimprovement_modifiedonbehalfby_valueNavigation = new HashSet<ptas_homeimprovement>();
            ptas_homeimprovement_owninguser_valueNavigation = new HashSet<ptas_homeimprovement>();
            ptas_housingprogram_createdby_valueNavigation = new HashSet<ptas_housingprogram>();
            ptas_housingprogram_createdonbehalfby_valueNavigation = new HashSet<ptas_housingprogram>();
            ptas_housingprogram_modifiedby_valueNavigation = new HashSet<ptas_housingprogram>();
            ptas_housingprogram_modifiedonbehalfby_valueNavigation = new HashSet<ptas_housingprogram>();
            ptas_industry_createdby_valueNavigation = new HashSet<ptas_industry>();
            ptas_industry_createdonbehalfby_valueNavigation = new HashSet<ptas_industry>();
            ptas_industry_modifiedby_valueNavigation = new HashSet<ptas_industry>();
            ptas_industry_modifiedonbehalfby_valueNavigation = new HashSet<ptas_industry>();
            ptas_industry_owninguser_valueNavigation = new HashSet<ptas_industry>();
            ptas_inspectionhistory_createdby_valueNavigation = new HashSet<ptas_inspectionhistory>();
            ptas_inspectionhistory_createdonbehalfby_valueNavigation = new HashSet<ptas_inspectionhistory>();
            ptas_inspectionhistory_modifiedby_valueNavigation = new HashSet<ptas_inspectionhistory>();
            ptas_inspectionhistory_modifiedonbehalfby_valueNavigation = new HashSet<ptas_inspectionhistory>();
            ptas_inspectionhistory_owninguser_valueNavigation = new HashSet<ptas_inspectionhistory>();
            ptas_inspectionhistory_ptas_inspectedbyid_valueNavigation = new HashSet<ptas_inspectionhistory>();
            ptas_inspectionyear_createdby_valueNavigation = new HashSet<ptas_inspectionyear>();
            ptas_inspectionyear_createdonbehalfby_valueNavigation = new HashSet<ptas_inspectionyear>();
            ptas_inspectionyear_modifiedby_valueNavigation = new HashSet<ptas_inspectionyear>();
            ptas_inspectionyear_modifiedonbehalfby_valueNavigation = new HashSet<ptas_inspectionyear>();
            ptas_inspectionyear_owninguser_valueNavigation = new HashSet<ptas_inspectionyear>();
            ptas_jurisdiction_createdby_valueNavigation = new HashSet<ptas_jurisdiction>();
            ptas_jurisdiction_createdonbehalfby_valueNavigation = new HashSet<ptas_jurisdiction>();
            ptas_jurisdiction_modifiedby_valueNavigation = new HashSet<ptas_jurisdiction>();
            ptas_jurisdiction_modifiedonbehalfby_valueNavigation = new HashSet<ptas_jurisdiction>();
            ptas_land_createdby_valueNavigation = new HashSet<ptas_land>();
            ptas_land_createdonbehalfby_valueNavigation = new HashSet<ptas_land>();
            ptas_land_modifiedby_valueNavigation = new HashSet<ptas_land>();
            ptas_land_modifiedonbehalfby_valueNavigation = new HashSet<ptas_land>();
            ptas_land_owninguser_valueNavigation = new HashSet<ptas_land>();
            ptas_landuse_createdby_valueNavigation = new HashSet<ptas_landuse>();
            ptas_landuse_createdonbehalfby_valueNavigation = new HashSet<ptas_landuse>();
            ptas_landuse_modifiedby_valueNavigation = new HashSet<ptas_landuse>();
            ptas_landuse_modifiedonbehalfby_valueNavigation = new HashSet<ptas_landuse>();
            ptas_landvaluebreakdown_createdby_valueNavigation = new HashSet<ptas_landvaluebreakdown>();
            ptas_landvaluebreakdown_createdonbehalfby_valueNavigation = new HashSet<ptas_landvaluebreakdown>();
            ptas_landvaluebreakdown_modifiedby_valueNavigation = new HashSet<ptas_landvaluebreakdown>();
            ptas_landvaluebreakdown_modifiedonbehalfby_valueNavigation = new HashSet<ptas_landvaluebreakdown>();
            ptas_landvaluebreakdown_owninguser_valueNavigation = new HashSet<ptas_landvaluebreakdown>();
            ptas_landvaluecalculation_createdby_valueNavigation = new HashSet<ptas_landvaluecalculation>();
            ptas_landvaluecalculation_createdonbehalfby_valueNavigation = new HashSet<ptas_landvaluecalculation>();
            ptas_landvaluecalculation_modifiedby_valueNavigation = new HashSet<ptas_landvaluecalculation>();
            ptas_landvaluecalculation_modifiedonbehalfby_valueNavigation = new HashSet<ptas_landvaluecalculation>();
            ptas_landvaluecalculation_owninguser_valueNavigation = new HashSet<ptas_landvaluecalculation>();
            ptas_levycode_createdby_valueNavigation = new HashSet<ptas_levycode>();
            ptas_levycode_createdonbehalfby_valueNavigation = new HashSet<ptas_levycode>();
            ptas_levycode_modifiedby_valueNavigation = new HashSet<ptas_levycode>();
            ptas_levycode_modifiedonbehalfby_valueNavigation = new HashSet<ptas_levycode>();
            ptas_lowincomehousingprogram_createdby_valueNavigation = new HashSet<ptas_lowincomehousingprogram>();
            ptas_lowincomehousingprogram_createdonbehalfby_valueNavigation = new HashSet<ptas_lowincomehousingprogram>();
            ptas_lowincomehousingprogram_modifiedby_valueNavigation = new HashSet<ptas_lowincomehousingprogram>();
            ptas_lowincomehousingprogram_modifiedonbehalfby_valueNavigation = new HashSet<ptas_lowincomehousingprogram>();
            ptas_lowincomehousingprogram_owninguser_valueNavigation = new HashSet<ptas_lowincomehousingprogram>();
            ptas_masspayaccumulator_createdby_valueNavigation = new HashSet<ptas_masspayaccumulator>();
            ptas_masspayaccumulator_createdonbehalfby_valueNavigation = new HashSet<ptas_masspayaccumulator>();
            ptas_masspayaccumulator_modifiedby_valueNavigation = new HashSet<ptas_masspayaccumulator>();
            ptas_masspayaccumulator_modifiedonbehalfby_valueNavigation = new HashSet<ptas_masspayaccumulator>();
            ptas_masspayaccumulator_owninguser_valueNavigation = new HashSet<ptas_masspayaccumulator>();
            ptas_masspayaction_createdby_valueNavigation = new HashSet<ptas_masspayaction>();
            ptas_masspayaction_createdonbehalfby_valueNavigation = new HashSet<ptas_masspayaction>();
            ptas_masspayaction_modifiedby_valueNavigation = new HashSet<ptas_masspayaction>();
            ptas_masspayaction_modifiedonbehalfby_valueNavigation = new HashSet<ptas_masspayaction>();
            ptas_masspayaction_owninguser_valueNavigation = new HashSet<ptas_masspayaction>();
            ptas_masspayer_createdby_valueNavigation = new HashSet<ptas_masspayer>();
            ptas_masspayer_createdonbehalfby_valueNavigation = new HashSet<ptas_masspayer>();
            ptas_masspayer_modifiedby_valueNavigation = new HashSet<ptas_masspayer>();
            ptas_masspayer_modifiedonbehalfby_valueNavigation = new HashSet<ptas_masspayer>();
            ptas_masspayer_owninguser_valueNavigation = new HashSet<ptas_masspayer>();
            ptas_mediarepository_createdby_valueNavigation = new HashSet<ptas_mediarepository>();
            ptas_mediarepository_createdonbehalfby_valueNavigation = new HashSet<ptas_mediarepository>();
            ptas_mediarepository_modifiedby_valueNavigation = new HashSet<ptas_mediarepository>();
            ptas_mediarepository_modifiedonbehalfby_valueNavigation = new HashSet<ptas_mediarepository>();
            ptas_mediarepository_owninguser_valueNavigation = new HashSet<ptas_mediarepository>();
            ptas_naicscode_createdby_valueNavigation = new HashSet<ptas_naicscode>();
            ptas_naicscode_createdonbehalfby_valueNavigation = new HashSet<ptas_naicscode>();
            ptas_naicscode_modifiedby_valueNavigation = new HashSet<ptas_naicscode>();
            ptas_naicscode_modifiedonbehalfby_valueNavigation = new HashSet<ptas_naicscode>();
            ptas_neighborhood_createdby_valueNavigation = new HashSet<ptas_neighborhood>();
            ptas_neighborhood_createdonbehalfby_valueNavigation = new HashSet<ptas_neighborhood>();
            ptas_neighborhood_modifiedby_valueNavigation = new HashSet<ptas_neighborhood>();
            ptas_neighborhood_modifiedonbehalfby_valueNavigation = new HashSet<ptas_neighborhood>();
            ptas_notificationconfiguration_createdby_valueNavigation = new HashSet<ptas_notificationconfiguration>();
            ptas_notificationconfiguration_createdonbehalfby_valueNavigation = new HashSet<ptas_notificationconfiguration>();
            ptas_notificationconfiguration_modifiedby_valueNavigation = new HashSet<ptas_notificationconfiguration>();
            ptas_notificationconfiguration_modifiedonbehalfby_valueNavigation = new HashSet<ptas_notificationconfiguration>();
            ptas_notificationconfiguration_owninguser_valueNavigation = new HashSet<ptas_notificationconfiguration>();
            ptas_omit_createdby_valueNavigation = new HashSet<ptas_omit>();
            ptas_omit_createdonbehalfby_valueNavigation = new HashSet<ptas_omit>();
            ptas_omit_modifiedby_valueNavigation = new HashSet<ptas_omit>();
            ptas_omit_modifiedonbehalfby_valueNavigation = new HashSet<ptas_omit>();
            ptas_omit_owninguser_valueNavigation = new HashSet<ptas_omit>();
            ptas_parceldetail_createdby_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_createdonbehalfby_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_modifiedby_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_modifiedonbehalfby_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_owninguser_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_ptas_assignedappraiserid_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_ptas_landinspectedbyid_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_ptas_parcelinspectedbyid_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceldetail_ptas_specialtyappraiserid_valueNavigation = new HashSet<ptas_parceldetail>();
            ptas_parceleconomicunit_createdby_valueNavigation = new HashSet<ptas_parceleconomicunit>();
            ptas_parceleconomicunit_createdonbehalfby_valueNavigation = new HashSet<ptas_parceleconomicunit>();
            ptas_parceleconomicunit_modifiedby_valueNavigation = new HashSet<ptas_parceleconomicunit>();
            ptas_parceleconomicunit_modifiedonbehalfby_valueNavigation = new HashSet<ptas_parceleconomicunit>();
            ptas_parceleconomicunit_owninguser_valueNavigation = new HashSet<ptas_parceleconomicunit>();
            ptas_parkingdistrict_createdby_valueNavigation = new HashSet<ptas_parkingdistrict>();
            ptas_parkingdistrict_createdonbehalfby_valueNavigation = new HashSet<ptas_parkingdistrict>();
            ptas_parkingdistrict_modifiedby_valueNavigation = new HashSet<ptas_parkingdistrict>();
            ptas_parkingdistrict_modifiedonbehalfby_valueNavigation = new HashSet<ptas_parkingdistrict>();
            ptas_parkingdistrict_owninguser_valueNavigation = new HashSet<ptas_parkingdistrict>();
            ptas_permit_createdby_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_createdonbehalfby_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_modifiedby_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_modifiedonbehalfby_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_owninguser_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_ptas_reviewedbyid_valueNavigation = new HashSet<ptas_permit>();
            ptas_permit_ptas_statusupdatedbyid_valueNavigation = new HashSet<ptas_permit>();
            ptas_permitinspectionhistory_createdby_valueNavigation = new HashSet<ptas_permitinspectionhistory>();
            ptas_permitinspectionhistory_createdonbehalfby_valueNavigation = new HashSet<ptas_permitinspectionhistory>();
            ptas_permitinspectionhistory_modifiedby_valueNavigation = new HashSet<ptas_permitinspectionhistory>();
            ptas_permitinspectionhistory_modifiedonbehalfby_valueNavigation = new HashSet<ptas_permitinspectionhistory>();
            ptas_permitinspectionhistory_owninguser_valueNavigation = new HashSet<ptas_permitinspectionhistory>();
            ptas_permitwebsiteconfig_createdby_valueNavigation = new HashSet<ptas_permitwebsiteconfig>();
            ptas_permitwebsiteconfig_createdonbehalfby_valueNavigation = new HashSet<ptas_permitwebsiteconfig>();
            ptas_permitwebsiteconfig_modifiedby_valueNavigation = new HashSet<ptas_permitwebsiteconfig>();
            ptas_permitwebsiteconfig_modifiedonbehalfby_valueNavigation = new HashSet<ptas_permitwebsiteconfig>();
            ptas_permitwebsiteconfig_owninguser_valueNavigation = new HashSet<ptas_permitwebsiteconfig>();
            ptas_phonenumber_createdby_valueNavigation = new HashSet<ptas_phonenumber>();
            ptas_phonenumber_createdonbehalfby_valueNavigation = new HashSet<ptas_phonenumber>();
            ptas_phonenumber_modifiedby_valueNavigation = new HashSet<ptas_phonenumber>();
            ptas_phonenumber_modifiedonbehalfby_valueNavigation = new HashSet<ptas_phonenumber>();
            ptas_phonenumber_owninguser_valueNavigation = new HashSet<ptas_phonenumber>();
            ptas_portalcontact_createdby_valueNavigation = new HashSet<ptas_portalcontact>();
            ptas_portalcontact_createdonbehalfby_valueNavigation = new HashSet<ptas_portalcontact>();
            ptas_portalcontact_modifiedby_valueNavigation = new HashSet<ptas_portalcontact>();
            ptas_portalcontact_modifiedonbehalfby_valueNavigation = new HashSet<ptas_portalcontact>();
            ptas_portalcontact_owninguser_valueNavigation = new HashSet<ptas_portalcontact>();
            ptas_portalemail_createdby_valueNavigation = new HashSet<ptas_portalemail>();
            ptas_portalemail_createdonbehalfby_valueNavigation = new HashSet<ptas_portalemail>();
            ptas_portalemail_modifiedby_valueNavigation = new HashSet<ptas_portalemail>();
            ptas_portalemail_modifiedonbehalfby_valueNavigation = new HashSet<ptas_portalemail>();
            ptas_portalemail_owninguser_valueNavigation = new HashSet<ptas_portalemail>();
            ptas_projectdock_createdby_valueNavigation = new HashSet<ptas_projectdock>();
            ptas_projectdock_createdonbehalfby_valueNavigation = new HashSet<ptas_projectdock>();
            ptas_projectdock_modifiedby_valueNavigation = new HashSet<ptas_projectdock>();
            ptas_projectdock_modifiedonbehalfby_valueNavigation = new HashSet<ptas_projectdock>();
            ptas_propertytype_createdby_valueNavigation = new HashSet<ptas_propertytype>();
            ptas_propertytype_createdonbehalfby_valueNavigation = new HashSet<ptas_propertytype>();
            ptas_propertytype_modifiedby_valueNavigation = new HashSet<ptas_propertytype>();
            ptas_propertytype_modifiedonbehalfby_valueNavigation = new HashSet<ptas_propertytype>();
            ptas_ptasconfiguration_createdby_valueNavigation = new HashSet<ptas_ptasconfiguration>();
            ptas_ptasconfiguration_createdonbehalfby_valueNavigation = new HashSet<ptas_ptasconfiguration>();
            ptas_ptasconfiguration_modifiedby_valueNavigation = new HashSet<ptas_ptasconfiguration>();
            ptas_ptasconfiguration_modifiedonbehalfby_valueNavigation = new HashSet<ptas_ptasconfiguration>();
            ptas_ptasconfiguration_ptas_defaultsendfromid_valueNavigation = new HashSet<ptas_ptasconfiguration>();
            ptas_ptasconfiguration_ptas_sendsrexemptsyncemailto_valueNavigation = new HashSet<ptas_ptasconfiguration>();
            ptas_ptassetting_createdby_valueNavigation = new HashSet<ptas_ptassetting>();
            ptas_ptassetting_createdonbehalfby_valueNavigation = new HashSet<ptas_ptassetting>();
            ptas_ptassetting_modifiedby_valueNavigation = new HashSet<ptas_ptassetting>();
            ptas_ptassetting_modifiedonbehalfby_valueNavigation = new HashSet<ptas_ptassetting>();
            ptas_ptassetting_owninguser_valueNavigation = new HashSet<ptas_ptassetting>();
            ptas_qstr_createdby_valueNavigation = new HashSet<ptas_qstr>();
            ptas_qstr_createdonbehalfby_valueNavigation = new HashSet<ptas_qstr>();
            ptas_qstr_modifiedby_valueNavigation = new HashSet<ptas_qstr>();
            ptas_qstr_modifiedonbehalfby_valueNavigation = new HashSet<ptas_qstr>();
            ptas_quickcollect_createdby_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_createdonbehalfby_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_modifiedby_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_modifiedonbehalfby_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_owninguser_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_ptas_processeduserid_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_recentparcel_createdby_valueNavigation = new HashSet<ptas_recentparcel>();
            ptas_recentparcel_createdonbehalfby_valueNavigation = new HashSet<ptas_recentparcel>();
            ptas_recentparcel_modifiedby_valueNavigation = new HashSet<ptas_recentparcel>();
            ptas_recentparcel_modifiedonbehalfby_valueNavigation = new HashSet<ptas_recentparcel>();
            ptas_recentparcel_owninguser_valueNavigation = new HashSet<ptas_recentparcel>();
            ptas_residentialappraiserteam_createdby_valueNavigation = new HashSet<ptas_residentialappraiserteam>();
            ptas_residentialappraiserteam_createdonbehalfby_valueNavigation = new HashSet<ptas_residentialappraiserteam>();
            ptas_residentialappraiserteam_modifiedby_valueNavigation = new HashSet<ptas_residentialappraiserteam>();
            ptas_residentialappraiserteam_modifiedonbehalfby_valueNavigation = new HashSet<ptas_residentialappraiserteam>();
            ptas_residentialappraiserteam_owninguser_valueNavigation = new HashSet<ptas_residentialappraiserteam>();
            ptas_responsibility_createdby_valueNavigation = new HashSet<ptas_responsibility>();
            ptas_responsibility_createdonbehalfby_valueNavigation = new HashSet<ptas_responsibility>();
            ptas_responsibility_modifiedby_valueNavigation = new HashSet<ptas_responsibility>();
            ptas_responsibility_modifiedonbehalfby_valueNavigation = new HashSet<ptas_responsibility>();
            ptas_restrictedrent_createdby_valueNavigation = new HashSet<ptas_restrictedrent>();
            ptas_restrictedrent_createdonbehalfby_valueNavigation = new HashSet<ptas_restrictedrent>();
            ptas_restrictedrent_modifiedby_valueNavigation = new HashSet<ptas_restrictedrent>();
            ptas_restrictedrent_modifiedonbehalfby_valueNavigation = new HashSet<ptas_restrictedrent>();
            ptas_restrictedrent_owninguser_valueNavigation = new HashSet<ptas_restrictedrent>();
            ptas_salepriceadjustment_createdby_valueNavigation = new HashSet<ptas_salepriceadjustment>();
            ptas_salepriceadjustment_createdonbehalfby_valueNavigation = new HashSet<ptas_salepriceadjustment>();
            ptas_salepriceadjustment_modifiedby_valueNavigation = new HashSet<ptas_salepriceadjustment>();
            ptas_salepriceadjustment_modifiedonbehalfby_valueNavigation = new HashSet<ptas_salepriceadjustment>();
            ptas_salepriceadjustment_owninguser_valueNavigation = new HashSet<ptas_salepriceadjustment>();
            ptas_salesaggregate_createdby_valueNavigation = new HashSet<ptas_salesaggregate>();
            ptas_salesaggregate_createdonbehalfby_valueNavigation = new HashSet<ptas_salesaggregate>();
            ptas_salesaggregate_modifiedby_valueNavigation = new HashSet<ptas_salesaggregate>();
            ptas_salesaggregate_modifiedonbehalfby_valueNavigation = new HashSet<ptas_salesaggregate>();
            ptas_salesaggregate_owninguser_valueNavigation = new HashSet<ptas_salesaggregate>();
            ptas_salesnote_createdby_valueNavigation = new HashSet<ptas_salesnote>();
            ptas_salesnote_createdonbehalfby_valueNavigation = new HashSet<ptas_salesnote>();
            ptas_salesnote_modifiedby_valueNavigation = new HashSet<ptas_salesnote>();
            ptas_salesnote_modifiedonbehalfby_valueNavigation = new HashSet<ptas_salesnote>();
            ptas_salesnote_owninguser_valueNavigation = new HashSet<ptas_salesnote>();
            ptas_saleswarningcode_createdby_valueNavigation = new HashSet<ptas_saleswarningcode>();
            ptas_saleswarningcode_createdonbehalfby_valueNavigation = new HashSet<ptas_saleswarningcode>();
            ptas_saleswarningcode_modifiedby_valueNavigation = new HashSet<ptas_saleswarningcode>();
            ptas_saleswarningcode_modifiedonbehalfby_valueNavigation = new HashSet<ptas_saleswarningcode>();
            ptas_scheduledworkflow_createdby_valueNavigation = new HashSet<ptas_scheduledworkflow>();
            ptas_scheduledworkflow_createdonbehalfby_valueNavigation = new HashSet<ptas_scheduledworkflow>();
            ptas_scheduledworkflow_modifiedby_valueNavigation = new HashSet<ptas_scheduledworkflow>();
            ptas_scheduledworkflow_modifiedonbehalfby_valueNavigation = new HashSet<ptas_scheduledworkflow>();
            ptas_scheduledworkflow_owninguser_valueNavigation = new HashSet<ptas_scheduledworkflow>();
            ptas_sectionusesqft_createdby_valueNavigation = new HashSet<ptas_sectionusesqft>();
            ptas_sectionusesqft_createdonbehalfby_valueNavigation = new HashSet<ptas_sectionusesqft>();
            ptas_sectionusesqft_modifiedby_valueNavigation = new HashSet<ptas_sectionusesqft>();
            ptas_sectionusesqft_modifiedonbehalfby_valueNavigation = new HashSet<ptas_sectionusesqft>();
            ptas_sectionusesqft_owninguser_valueNavigation = new HashSet<ptas_sectionusesqft>();
            ptas_sketch_createdby_valueNavigation = new HashSet<ptas_sketch>();
            ptas_sketch_createdonbehalfby_valueNavigation = new HashSet<ptas_sketch>();
            ptas_sketch_modifiedby_valueNavigation = new HashSet<ptas_sketch>();
            ptas_sketch_modifiedonbehalfby_valueNavigation = new HashSet<ptas_sketch>();
            ptas_sketch_owninguser_valueNavigation = new HashSet<ptas_sketch>();
            ptas_sketch_ptas_drawauthorid_valueNavigation = new HashSet<ptas_sketch>();
            ptas_sketch_ptas_lockedbyid_valueNavigation = new HashSet<ptas_sketch>();
            ptas_specialtyarea_createdby_valueNavigation = new HashSet<ptas_specialtyarea>();
            ptas_specialtyarea_createdonbehalfby_valueNavigation = new HashSet<ptas_specialtyarea>();
            ptas_specialtyarea_modifiedby_valueNavigation = new HashSet<ptas_specialtyarea>();
            ptas_specialtyarea_modifiedonbehalfby_valueNavigation = new HashSet<ptas_specialtyarea>();
            ptas_specialtyarea_ptas_seniorappraiserid_valueNavigation = new HashSet<ptas_specialtyarea>();
            ptas_specialtyneighborhood_createdby_valueNavigation = new HashSet<ptas_specialtyneighborhood>();
            ptas_specialtyneighborhood_createdonbehalfby_valueNavigation = new HashSet<ptas_specialtyneighborhood>();
            ptas_specialtyneighborhood_modifiedby_valueNavigation = new HashSet<ptas_specialtyneighborhood>();
            ptas_specialtyneighborhood_modifiedonbehalfby_valueNavigation = new HashSet<ptas_specialtyneighborhood>();
            ptas_specialtyneighborhood_ptas_appraiserid_valueNavigation = new HashSet<ptas_specialtyneighborhood>();
            ptas_stateorprovince_createdby_valueNavigation = new HashSet<ptas_stateorprovince>();
            ptas_stateorprovince_createdonbehalfby_valueNavigation = new HashSet<ptas_stateorprovince>();
            ptas_stateorprovince_modifiedby_valueNavigation = new HashSet<ptas_stateorprovince>();
            ptas_stateorprovince_modifiedonbehalfby_valueNavigation = new HashSet<ptas_stateorprovince>();
            ptas_streetname_createdby_valueNavigation = new HashSet<ptas_streetname>();
            ptas_streetname_createdonbehalfby_valueNavigation = new HashSet<ptas_streetname>();
            ptas_streetname_modifiedby_valueNavigation = new HashSet<ptas_streetname>();
            ptas_streetname_modifiedonbehalfby_valueNavigation = new HashSet<ptas_streetname>();
            ptas_streettype_createdby_valueNavigation = new HashSet<ptas_streettype>();
            ptas_streettype_createdonbehalfby_valueNavigation = new HashSet<ptas_streettype>();
            ptas_streettype_modifiedby_valueNavigation = new HashSet<ptas_streettype>();
            ptas_streettype_modifiedonbehalfby_valueNavigation = new HashSet<ptas_streettype>();
            ptas_subarea_createdby_valueNavigation = new HashSet<ptas_subarea>();
            ptas_subarea_createdonbehalfby_valueNavigation = new HashSet<ptas_subarea>();
            ptas_subarea_modifiedby_valueNavigation = new HashSet<ptas_subarea>();
            ptas_subarea_modifiedonbehalfby_valueNavigation = new HashSet<ptas_subarea>();
            ptas_submarket_createdby_valueNavigation = new HashSet<ptas_submarket>();
            ptas_submarket_createdonbehalfby_valueNavigation = new HashSet<ptas_submarket>();
            ptas_submarket_modifiedby_valueNavigation = new HashSet<ptas_submarket>();
            ptas_submarket_modifiedonbehalfby_valueNavigation = new HashSet<ptas_submarket>();
            ptas_supergroup_createdby_valueNavigation = new HashSet<ptas_supergroup>();
            ptas_supergroup_createdonbehalfby_valueNavigation = new HashSet<ptas_supergroup>();
            ptas_supergroup_modifiedby_valueNavigation = new HashSet<ptas_supergroup>();
            ptas_supergroup_modifiedonbehalfby_valueNavigation = new HashSet<ptas_supergroup>();
            ptas_task_createdby_valueNavigation = new HashSet<ptas_task>();
            ptas_task_createdonbehalfby_valueNavigation = new HashSet<ptas_task>();
            ptas_task_modifiedby_valueNavigation = new HashSet<ptas_task>();
            ptas_task_modifiedonbehalfby_valueNavigation = new HashSet<ptas_task>();
            ptas_task_owninguser_valueNavigation = new HashSet<ptas_task>();
            ptas_task_ptas_accountingsectionsupervisor_valueNavigation = new HashSet<ptas_task>();
            ptas_task_ptas_appraiser_valueNavigation = new HashSet<ptas_task>();
            ptas_task_ptas_commercialsrappraiser_valueNavigation = new HashSet<ptas_task>();
            ptas_task_ptas_residentialsrappraiser_valueNavigation = new HashSet<ptas_task>();
            ptas_taxaccount_createdby_valueNavigation = new HashSet<ptas_taxaccount>();
            ptas_taxaccount_createdonbehalfby_valueNavigation = new HashSet<ptas_taxaccount>();
            ptas_taxaccount_modifiedby_valueNavigation = new HashSet<ptas_taxaccount>();
            ptas_taxaccount_modifiedonbehalfby_valueNavigation = new HashSet<ptas_taxaccount>();
            ptas_taxaccount_owninguser_valueNavigation = new HashSet<ptas_taxaccount>();
            ptas_trendfactor_createdby_valueNavigation = new HashSet<ptas_trendfactor>();
            ptas_trendfactor_createdonbehalfby_valueNavigation = new HashSet<ptas_trendfactor>();
            ptas_trendfactor_modifiedby_valueNavigation = new HashSet<ptas_trendfactor>();
            ptas_trendfactor_modifiedonbehalfby_valueNavigation = new HashSet<ptas_trendfactor>();
            ptas_trendfactor_owninguser_valueNavigation = new HashSet<ptas_trendfactor>();
            ptas_unitbreakdown_createdby_valueNavigation = new HashSet<ptas_unitbreakdown>();
            ptas_unitbreakdown_createdonbehalfby_valueNavigation = new HashSet<ptas_unitbreakdown>();
            ptas_unitbreakdown_modifiedby_valueNavigation = new HashSet<ptas_unitbreakdown>();
            ptas_unitbreakdown_modifiedonbehalfby_valueNavigation = new HashSet<ptas_unitbreakdown>();
            ptas_unitbreakdown_owninguser_valueNavigation = new HashSet<ptas_unitbreakdown>();
            ptas_unitbreakdowntype_createdby_valueNavigation = new HashSet<ptas_unitbreakdowntype>();
            ptas_unitbreakdowntype_createdonbehalfby_valueNavigation = new HashSet<ptas_unitbreakdowntype>();
            ptas_unitbreakdowntype_modifiedby_valueNavigation = new HashSet<ptas_unitbreakdowntype>();
            ptas_unitbreakdowntype_modifiedonbehalfby_valueNavigation = new HashSet<ptas_unitbreakdowntype>();
            ptas_visitedsketch_createdby_valueNavigation = new HashSet<ptas_visitedsketch>();
            ptas_visitedsketch_createdonbehalfby_valueNavigation = new HashSet<ptas_visitedsketch>();
            ptas_visitedsketch_modifiedby_valueNavigation = new HashSet<ptas_visitedsketch>();
            ptas_visitedsketch_modifiedonbehalfby_valueNavigation = new HashSet<ptas_visitedsketch>();
            ptas_visitedsketch_owninguser_valueNavigation = new HashSet<ptas_visitedsketch>();
            ptas_visitedsketch_ptas_visitedbyid_valueNavigation = new HashSet<ptas_visitedsketch>();
            ptas_year_createdby_valueNavigation = new HashSet<ptas_year>();
            ptas_year_createdonbehalfby_valueNavigation = new HashSet<ptas_year>();
            ptas_year_modifiedby_valueNavigation = new HashSet<ptas_year>();
            ptas_year_modifiedonbehalfby_valueNavigation = new HashSet<ptas_year>();
            ptas_year_ptas_rollovernotificationid_valueNavigation = new HashSet<ptas_year>();
            ptas_zipcode_createdby_valueNavigation = new HashSet<ptas_zipcode>();
            ptas_zipcode_createdonbehalfby_valueNavigation = new HashSet<ptas_zipcode>();
            ptas_zipcode_modifiedby_valueNavigation = new HashSet<ptas_zipcode>();
            ptas_zipcode_modifiedonbehalfby_valueNavigation = new HashSet<ptas_zipcode>();
            ptas_zoning_createdby_valueNavigation = new HashSet<ptas_zoning>();
            ptas_zoning_createdonbehalfby_valueNavigation = new HashSet<ptas_zoning>();
            ptas_zoning_modifiedby_valueNavigation = new HashSet<ptas_zoning>();
            ptas_zoning_modifiedonbehalfby_valueNavigation = new HashSet<ptas_zoning>();
            role_createdby_valueNavigation = new HashSet<role>();
            role_createdonbehalfby_valueNavigation = new HashSet<role>();
            role_modifiedby_valueNavigation = new HashSet<role>();
            role_modifiedonbehalfby_valueNavigation = new HashSet<role>();
            team_administratorid_valueNavigation = new HashSet<team>();
            team_createdby_valueNavigation = new HashSet<team>();
            team_createdonbehalfby_valueNavigation = new HashSet<team>();
            team_modifiedby_valueNavigation = new HashSet<team>();
            team_modifiedonbehalfby_valueNavigation = new HashSet<team>();
        }

        public int? accessmode { get; set; }
        public Guid? address1_addressid { get; set; }
        public int? address1_addresstypecode { get; set; }
        public string address1_city { get; set; }
        public string address1_composite { get; set; }
        public string address1_country { get; set; }
        public string address1_county { get; set; }
        public string address1_fax { get; set; }
        public double? address1_latitude { get; set; }
        public string address1_line1 { get; set; }
        public string address1_line2 { get; set; }
        public string address1_line3 { get; set; }
        public double? address1_longitude { get; set; }
        public string address1_name { get; set; }
        public string address1_postalcode { get; set; }
        public string address1_postofficebox { get; set; }
        public int? address1_shippingmethodcode { get; set; }
        public string address1_stateorprovince { get; set; }
        public string address1_telephone1 { get; set; }
        public string address1_telephone2 { get; set; }
        public string address1_telephone3 { get; set; }
        public string address1_upszone { get; set; }
        public int? address1_utcoffset { get; set; }
        public Guid? address2_addressid { get; set; }
        public int? address2_addresstypecode { get; set; }
        public string address2_city { get; set; }
        public string address2_composite { get; set; }
        public string address2_country { get; set; }
        public string address2_county { get; set; }
        public string address2_fax { get; set; }
        public double? address2_latitude { get; set; }
        public string address2_line1 { get; set; }
        public string address2_line2 { get; set; }
        public string address2_line3 { get; set; }
        public double? address2_longitude { get; set; }
        public string address2_name { get; set; }
        public string address2_postalcode { get; set; }
        public string address2_postofficebox { get; set; }
        public int? address2_shippingmethodcode { get; set; }
        public string address2_stateorprovince { get; set; }
        public string address2_telephone1 { get; set; }
        public string address2_telephone2 { get; set; }
        public string address2_telephone3 { get; set; }
        public string address2_upszone { get; set; }
        public int? address2_utcoffset { get; set; }
        public Guid? applicationid { get; set; }
        public string applicationiduri { get; set; }
        public Guid? azureactivedirectoryobjectid { get; set; }
        public int? caltype { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public bool? defaultfilterspopulated { get; set; }
        public string defaultodbfoldername { get; set; }
        public string disabledreason { get; set; }
        public bool? displayinserviceviews { get; set; }
        public string domainname { get; set; }
        public int? emailrouteraccessapproval { get; set; }
        public string employeeid { get; set; }
        public long? entityimage_timestamp { get; set; }
        public string entityimage_url { get; set; }
        public Guid? entityimageid { get; set; }
        public decimal? exchangerate { get; set; }
        public string firstname { get; set; }
        public string fullname { get; set; }
        public string governmentid { get; set; }
        public string homephone { get; set; }
        public int? identityid { get; set; }
        public int? importsequencenumber { get; set; }
        public int? incomingemaildeliverymethod { get; set; }
        public string internalemailaddress { get; set; }
        public int? invitestatuscode { get; set; }
        public bool? isdisabled { get; set; }
        public bool? isemailaddressapprovedbyo365admin { get; set; }
        public bool? isintegrationuser { get; set; }
        public bool? islicensed { get; set; }
        public bool? issyncwithdirectory { get; set; }
        public string jobtitle { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string mobilealertemail { get; set; }
        public string mobilephone { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public bool? msdyn_gdproptout { get; set; }
        public string nickname { get; set; }
        public Guid? organizationid { get; set; }
        public int? outgoingemaildeliverymethod { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public int? passporthi { get; set; }
        public int? passportlo { get; set; }
        public string personalemailaddress { get; set; }
        public string photourl { get; set; }
        public int? preferredaddresscode { get; set; }
        public int? preferredemailcode { get; set; }
        public int? preferredphonecode { get; set; }
        public Guid? processid { get; set; }
        public string ptas_legacyid { get; set; }
        public string salutation { get; set; }
        public bool? setupuser { get; set; }
        public string sharepointemailaddress { get; set; }
        public string skills { get; set; }
        public Guid? stageid { get; set; }
        public Guid systemuserid { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public string title { get; set; }
        public string traversedpath { get; set; }
        public int? userlicensetype { get; set; }
        public string userpuid { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public string windowsliveid { get; set; }
        public string yammeremailaddress { get; set; }
        public string yammeruserid { get; set; }
        public string yomifirstname { get; set; }
        public string yomifullname { get; set; }
        public string yomilastname { get; set; }
        public string yomimiddlename { get; set; }
        public Guid? _businessunitid_value { get; set; }
        public Guid? _calendarid_value { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _defaultmailbox_value { get; set; }
        public Guid? _mobileofflineprofileid_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _parentsystemuserid_value { get; set; }
        public Guid? _positionid_value { get; set; }
        public Guid? _queueid_value { get; set; }
        public Guid? _siteid_value { get; set; }
        public Guid? _territoryid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }
        public bool? ptas_taskassignmentnotification { get; set; }
        public bool? ptas_taskstatusnotification { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _parentsystemuserid_valueNavigation { get; set; }
        public virtual ICollection<systemuser> Inverse_createdby_valueNavigation { get; set; }
        public virtual ICollection<systemuser> Inverse_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<systemuser> Inverse_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<systemuser> Inverse_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<systemuser> Inverse_parentsystemuserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_addresschangehistory> ptas_addresschangehistory_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_addresschangehistory> ptas_addresschangehistory_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_addresschangehistory> ptas_addresschangehistory_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_addresschangehistory> ptas_addresschangehistory_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_addresschangehistory> ptas_addresschangehistory_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationtracker> ptas_annexationtracker_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationtracker> ptas_annexationtracker_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationtracker> ptas_annexationtracker_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationtracker> ptas_annexationtracker_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationtracker> ptas_annexationtracker_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_annualcostdistribution> ptas_annualcostdistribution_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annualcostdistribution> ptas_annualcostdistribution_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annualcostdistribution> ptas_annualcostdistribution_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annualcostdistribution> ptas_annualcostdistribution_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annualcostdistribution> ptas_annualcostdistribution_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentregion> ptas_apartmentregion_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentregion> ptas_apartmentregion_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentregion> ptas_apartmentregion_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentregion> ptas_apartmentregion_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentsupergroup> ptas_apartmentsupergroup_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentsupergroup> ptas_apartmentsupergroup_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentsupergroup> ptas_apartmentsupergroup_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apartmentsupergroup> ptas_apartmentsupergroup_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcloseproximity> ptas_aptcloseproximity_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcloseproximity> ptas_aptcloseproximity_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcloseproximity> ptas_aptcloseproximity_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcloseproximity> ptas_aptcloseproximity_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptconditionadjustment> ptas_aptconditionadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptconditionadjustment> ptas_aptconditionadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptconditionadjustment> ptas_aptconditionadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptconditionadjustment> ptas_aptconditionadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptconditionadjustment> ptas_aptconditionadjustment_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpensehighend> ptas_aptexpensehighend_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpensehighend> ptas_aptexpensehighend_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpensehighend> ptas_aptexpensehighend_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpensehighend> ptas_aptexpensehighend_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpensehighend> ptas_aptexpensehighend_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptincomeexpense> ptas_aptincomeexpense_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptincomeexpense> ptas_aptincomeexpense_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptincomeexpense> ptas_aptincomeexpense_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptincomeexpense> ptas_aptincomeexpense_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptlistedrent> ptas_aptlistedrent_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptlistedrent> ptas_aptlistedrent_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptlistedrent> ptas_aptlistedrent_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptlistedrent> ptas_aptlistedrent_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptlistedrent> ptas_aptlistedrent_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptneighborhood> ptas_aptneighborhood_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptneighborhood> ptas_aptneighborhood_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptneighborhood> ptas_aptneighborhood_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptneighborhood> ptas_aptneighborhood_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptrentmodel> ptas_aptrentmodel_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptrentmodel> ptas_aptrentmodel_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptrentmodel> ptas_aptrentmodel_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptrentmodel> ptas_aptrentmodel_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptrentmodel> ptas_aptrentmodel_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptsalesmodel> ptas_aptsalesmodel_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptsalesmodel> ptas_aptsalesmodel_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptsalesmodel> ptas_aptsalesmodel_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptsalesmodel> ptas_aptsalesmodel_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptsalesmodel> ptas_aptsalesmodel_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_apttrending> ptas_apttrending_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apttrending> ptas_apttrending_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apttrending> ptas_apttrending_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apttrending> ptas_apttrending_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_apttrending> ptas_apttrending_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation_ptas_appraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation_ptas_updatedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluationproject> ptas_aptvaluationproject_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluationproject> ptas_aptvaluationproject_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluationproject> ptas_aptvaluationproject_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluationproject> ptas_aptvaluationproject_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluationproject> ptas_aptvaluationproject_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewqualityadjustment> ptas_aptviewqualityadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewqualityadjustment> ptas_aptviewqualityadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewqualityadjustment> ptas_aptviewqualityadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewqualityadjustment> ptas_aptviewqualityadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewtypeadjustment> ptas_aptviewtypeadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewtypeadjustment> ptas_aptviewtypeadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewtypeadjustment> ptas_aptviewtypeadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptviewtypeadjustment> ptas_aptviewtypeadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_arcreasoncode> ptas_arcreasoncode_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_arcreasoncode> ptas_arcreasoncode_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_arcreasoncode> ptas_arcreasoncode_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_arcreasoncode> ptas_arcreasoncode_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_arcreasoncode> ptas_arcreasoncode_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_area> ptas_area_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_area> ptas_area_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_area> ptas_area_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_area> ptas_area_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_area> ptas_area_ptas_appraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_area> ptas_area_ptas_seniorappraiser_valueNavigation { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection_ptas_approvalappraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection_ptas_responsibleappraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmarktag> ptas_bookmarktag_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmarktag> ptas_bookmarktag_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmarktag> ptas_bookmarktag_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmarktag> ptas_bookmarktag_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmarktag> ptas_bookmarktag_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> ptas_buildingsectionfeature_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> ptas_buildingsectionfeature_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> ptas_buildingsectionfeature_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> ptas_buildingsectionfeature_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> ptas_buildingsectionfeature_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionuse> ptas_buildingsectionuse_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionuse> ptas_buildingsectionuse_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionuse> ptas_buildingsectionuse_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionuse> ptas_buildingsectionuse_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_changereason> ptas_changereason_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_changereason> ptas_changereason_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_changereason> ptas_changereason_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_changereason> ptas_changereason_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_changereason> ptas_changereason_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_city> ptas_city_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_city> ptas_city_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_city> ptas_city_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_city> ptas_city_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit_ptas_selectbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit_ptas_unitinspectedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminationproject> ptas_contaminationproject_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminationproject> ptas_contaminationproject_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminationproject> ptas_contaminationproject_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminationproject> ptas_contaminationproject_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminationproject> ptas_contaminationproject_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_country> ptas_country_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_country> ptas_country_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_country> ptas_country_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_country> ptas_country_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_county> ptas_county_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_county> ptas_county_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_county> ptas_county_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_county> ptas_county_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_depreciationtable> ptas_depreciationtable_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_depreciationtable> ptas_depreciationtable_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_depreciationtable> ptas_depreciationtable_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_depreciationtable> ptas_depreciationtable_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_depreciationtable> ptas_depreciationtable_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_district> ptas_district_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_district> ptas_district_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_district> ptas_district_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_district> ptas_district_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebscostcenter> ptas_ebscostcenter_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebscostcenter> ptas_ebscostcenter_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebscostcenter> ptas_ebscostcenter_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebscostcenter> ptas_ebscostcenter_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsfundnumber> ptas_ebsfundnumber_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsfundnumber> ptas_ebsfundnumber_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsfundnumber> ptas_ebsfundnumber_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsfundnumber> ptas_ebsfundnumber_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsmainaccount> ptas_ebsmainaccount_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsmainaccount> ptas_ebsmainaccount_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsmainaccount> ptas_ebsmainaccount_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsmainaccount> ptas_ebsmainaccount_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsproject> ptas_ebsproject_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsproject> ptas_ebsproject_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsproject> ptas_ebsproject_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ebsproject> ptas_ebsproject_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_economicunit> ptas_economicunit_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_economicunit> ptas_economicunit_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_economicunit> ptas_economicunit_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_economicunit> ptas_economicunit_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_economicunit> ptas_economicunit_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_environmentalrestriction> ptas_environmentalrestriction_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_environmentalrestriction> ptas_environmentalrestriction_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_environmentalrestriction> ptas_environmentalrestriction_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_environmentalrestriction> ptas_environmentalrestriction_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_environmentalrestriction> ptas_environmentalrestriction_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_ptas_loadbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoarea> ptas_geoarea_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoarea> ptas_geoarea_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoarea> ptas_geoarea_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoarea> ptas_geoarea_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoarea> ptas_geoarea_ptas_appraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoarea> ptas_geoarea_ptas_seniorappraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoneighborhood> ptas_geoneighborhood_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoneighborhood> ptas_geoneighborhood_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoneighborhood> ptas_geoneighborhood_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_geoneighborhood> ptas_geoneighborhood_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_govtaxpayername> ptas_govtaxpayername_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_govtaxpayername> ptas_govtaxpayername_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_govtaxpayername> ptas_govtaxpayername_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_govtaxpayername> ptas_govtaxpayername_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_gradestratificationmapping> ptas_gradestratificationmapping_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_gradestratificationmapping> ptas_gradestratificationmapping_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_gradestratificationmapping> ptas_gradestratificationmapping_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_gradestratificationmapping> ptas_gradestratificationmapping_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_housingprogram> ptas_housingprogram_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_housingprogram> ptas_housingprogram_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_housingprogram> ptas_housingprogram_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_housingprogram> ptas_housingprogram_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_industry> ptas_industry_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_industry> ptas_industry_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_industry> ptas_industry_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_industry> ptas_industry_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_industry> ptas_industry_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory_ptas_inspectedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_jurisdiction> ptas_jurisdiction_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_jurisdiction> ptas_jurisdiction_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_jurisdiction> ptas_jurisdiction_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_jurisdiction> ptas_jurisdiction_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_land> ptas_land_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_land> ptas_land_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_land> ptas_land_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_land> ptas_land_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_land> ptas_land_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_landuse> ptas_landuse_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landuse> ptas_landuse_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landuse> ptas_landuse_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landuse> ptas_landuse_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluecalculation> ptas_landvaluecalculation_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluecalculation> ptas_landvaluecalculation_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluecalculation> ptas_landvaluecalculation_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluecalculation> ptas_landvaluecalculation_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_landvaluecalculation> ptas_landvaluecalculation_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_levycode> ptas_levycode_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_levycode> ptas_levycode_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_levycode> ptas_levycode_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_levycode> ptas_levycode_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_lowincomehousingprogram> ptas_lowincomehousingprogram_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_lowincomehousingprogram> ptas_lowincomehousingprogram_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_lowincomehousingprogram> ptas_lowincomehousingprogram_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_lowincomehousingprogram> ptas_lowincomehousingprogram_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_lowincomehousingprogram> ptas_lowincomehousingprogram_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaccumulator> ptas_masspayaccumulator_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaccumulator> ptas_masspayaccumulator_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaccumulator> ptas_masspayaccumulator_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaccumulator> ptas_masspayaccumulator_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaccumulator> ptas_masspayaccumulator_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaction> ptas_masspayaction_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaction> ptas_masspayaction_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaction> ptas_masspayaction_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaction> ptas_masspayaction_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayaction> ptas_masspayaction_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayer> ptas_masspayer_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayer> ptas_masspayer_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayer> ptas_masspayer_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayer> ptas_masspayer_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_masspayer> ptas_masspayer_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_naicscode> ptas_naicscode_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_naicscode> ptas_naicscode_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_naicscode> ptas_naicscode_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_naicscode> ptas_naicscode_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_neighborhood> ptas_neighborhood_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_neighborhood> ptas_neighborhood_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_neighborhood> ptas_neighborhood_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_neighborhood> ptas_neighborhood_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_notificationconfiguration> ptas_notificationconfiguration_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_notificationconfiguration> ptas_notificationconfiguration_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_notificationconfiguration> ptas_notificationconfiguration_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_notificationconfiguration> ptas_notificationconfiguration_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_notificationconfiguration> ptas_notificationconfiguration_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_ptas_assignedappraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_ptas_landinspectedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_ptas_parcelinspectedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail_ptas_specialtyappraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_parkingdistrict> ptas_parkingdistrict_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parkingdistrict> ptas_parkingdistrict_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parkingdistrict> ptas_parkingdistrict_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parkingdistrict> ptas_parkingdistrict_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_parkingdistrict> ptas_parkingdistrict_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_ptas_reviewedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit_ptas_statusupdatedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitwebsiteconfig> ptas_permitwebsiteconfig_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitwebsiteconfig> ptas_permitwebsiteconfig_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitwebsiteconfig> ptas_permitwebsiteconfig_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitwebsiteconfig> ptas_permitwebsiteconfig_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_permitwebsiteconfig> ptas_permitwebsiteconfig_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_phonenumber> ptas_phonenumber_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_phonenumber> ptas_phonenumber_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_phonenumber> ptas_phonenumber_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_phonenumber> ptas_phonenumber_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_phonenumber> ptas_phonenumber_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalcontact> ptas_portalcontact_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalcontact> ptas_portalcontact_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalcontact> ptas_portalcontact_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalcontact> ptas_portalcontact_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalcontact> ptas_portalcontact_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalemail> ptas_portalemail_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalemail> ptas_portalemail_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalemail> ptas_portalemail_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalemail> ptas_portalemail_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_portalemail> ptas_portalemail_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_projectdock> ptas_projectdock_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_projectdock> ptas_projectdock_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_projectdock> ptas_projectdock_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_projectdock> ptas_projectdock_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_propertytype> ptas_propertytype_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_propertytype> ptas_propertytype_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_propertytype> ptas_propertytype_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_propertytype> ptas_propertytype_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptasconfiguration> ptas_ptasconfiguration_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptasconfiguration> ptas_ptasconfiguration_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptasconfiguration> ptas_ptasconfiguration_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptasconfiguration> ptas_ptasconfiguration_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptasconfiguration> ptas_ptasconfiguration_ptas_defaultsendfromid_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptasconfiguration> ptas_ptasconfiguration_ptas_sendsrexemptsyncemailto_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptassetting> ptas_ptassetting_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptassetting> ptas_ptassetting_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptassetting> ptas_ptassetting_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptassetting> ptas_ptassetting_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_ptassetting> ptas_ptassetting_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_qstr> ptas_qstr_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_qstr> ptas_qstr_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_qstr> ptas_qstr_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_qstr> ptas_qstr_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_ptas_processeduserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_recentparcel> ptas_recentparcel_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_recentparcel> ptas_recentparcel_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_recentparcel> ptas_recentparcel_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_recentparcel> ptas_recentparcel_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_recentparcel> ptas_recentparcel_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_residentialappraiserteam> ptas_residentialappraiserteam_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_residentialappraiserteam> ptas_residentialappraiserteam_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_residentialappraiserteam> ptas_residentialappraiserteam_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_residentialappraiserteam> ptas_residentialappraiserteam_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_residentialappraiserteam> ptas_residentialappraiserteam_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_responsibility> ptas_responsibility_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_responsibility> ptas_responsibility_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_responsibility> ptas_responsibility_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_responsibility> ptas_responsibility_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_restrictedrent> ptas_restrictedrent_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_restrictedrent> ptas_restrictedrent_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_restrictedrent> ptas_restrictedrent_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_restrictedrent> ptas_restrictedrent_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_restrictedrent> ptas_restrictedrent_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_salepriceadjustment> ptas_salepriceadjustment_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salepriceadjustment> ptas_salepriceadjustment_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salepriceadjustment> ptas_salepriceadjustment_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salepriceadjustment> ptas_salepriceadjustment_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salepriceadjustment> ptas_salepriceadjustment_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesnote> ptas_salesnote_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesnote> ptas_salesnote_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesnote> ptas_salesnote_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesnote> ptas_salesnote_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesnote> ptas_salesnote_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_saleswarningcode> ptas_saleswarningcode_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_saleswarningcode> ptas_saleswarningcode_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_saleswarningcode> ptas_saleswarningcode_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_saleswarningcode> ptas_saleswarningcode_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_scheduledworkflow> ptas_scheduledworkflow_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_scheduledworkflow> ptas_scheduledworkflow_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_scheduledworkflow> ptas_scheduledworkflow_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_scheduledworkflow> ptas_scheduledworkflow_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_scheduledworkflow> ptas_scheduledworkflow_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_sectionusesqft> ptas_sectionusesqft_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sectionusesqft> ptas_sectionusesqft_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sectionusesqft> ptas_sectionusesqft_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sectionusesqft> ptas_sectionusesqft_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sectionusesqft> ptas_sectionusesqft_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch_ptas_drawauthorid_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch_ptas_lockedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyarea> ptas_specialtyarea_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyarea> ptas_specialtyarea_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyarea> ptas_specialtyarea_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyarea> ptas_specialtyarea_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyarea> ptas_specialtyarea_ptas_seniorappraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyneighborhood> ptas_specialtyneighborhood_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyneighborhood> ptas_specialtyneighborhood_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyneighborhood> ptas_specialtyneighborhood_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyneighborhood> ptas_specialtyneighborhood_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_specialtyneighborhood> ptas_specialtyneighborhood_ptas_appraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_stateorprovince> ptas_stateorprovince_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_stateorprovince> ptas_stateorprovince_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_stateorprovince> ptas_stateorprovince_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_stateorprovince> ptas_stateorprovince_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streetname> ptas_streetname_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streetname> ptas_streetname_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streetname> ptas_streetname_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streetname> ptas_streetname_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streettype> ptas_streettype_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streettype> ptas_streettype_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streettype> ptas_streettype_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_streettype> ptas_streettype_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_subarea> ptas_subarea_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_subarea> ptas_subarea_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_subarea> ptas_subarea_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_subarea> ptas_subarea_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_submarket> ptas_submarket_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_submarket> ptas_submarket_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_submarket> ptas_submarket_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_submarket> ptas_submarket_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_supergroup> ptas_supergroup_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_supergroup> ptas_supergroup_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_supergroup> ptas_supergroup_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_supergroup> ptas_supergroup_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_ptas_accountingsectionsupervisor_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_ptas_appraiser_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_ptas_commercialsrappraiser_valueNavigation { get; set; }
        public virtual ICollection<ptas_task> ptas_task_ptas_residentialsrappraiser_valueNavigation { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_trendfactor> ptas_trendfactor_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_trendfactor> ptas_trendfactor_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_trendfactor> ptas_trendfactor_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_trendfactor> ptas_trendfactor_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_trendfactor> ptas_trendfactor_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdowntype> ptas_unitbreakdowntype_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdowntype> ptas_unitbreakdowntype_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdowntype> ptas_unitbreakdowntype_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_unitbreakdowntype> ptas_unitbreakdowntype_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch_owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch_ptas_visitedbyid_valueNavigation { get; set; }
        public virtual ICollection<ptas_year> ptas_year_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_year> ptas_year_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_year> ptas_year_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_year> ptas_year_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_year> ptas_year_ptas_rollovernotificationid_valueNavigation { get; set; }
        public virtual ICollection<ptas_zipcode> ptas_zipcode_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_zipcode> ptas_zipcode_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_zipcode> ptas_zipcode_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_zipcode> ptas_zipcode_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_zoning> ptas_zoning_createdby_valueNavigation { get; set; }
        public virtual ICollection<ptas_zoning> ptas_zoning_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_zoning> ptas_zoning_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<ptas_zoning> ptas_zoning_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<role> role_createdby_valueNavigation { get; set; }
        public virtual ICollection<role> role_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<role> role_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<role> role_modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<team> team_administratorid_valueNavigation { get; set; }
        public virtual ICollection<team> team_createdby_valueNavigation { get; set; }
        public virtual ICollection<team> team_createdonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<team> team_modifiedby_valueNavigation { get; set; }
        public virtual ICollection<team> team_modifiedonbehalfby_valueNavigation { get; set; }
    }
}
