// <copyright file="MappingProfile.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Mappings
{
  using AutoMapper;
  using PTASDynamicsCrudHelperClasses.JSONMappings;

  /// <summary>
  /// Automapper profile.
  /// </summary>
  public class MappingProfile : Profile
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
      this.Map2Ways<DynamicsContact, FormContact>();
      this.Map2Ways<DynamicsSeniorExemptionApplication, FormSeniorExemptionApplication>();
      this.Map2Ways<DynamicsSeniorExemptionApplicationForSave, FormSeniorExemptionApplication>();
      this.Map2Ways<DynamicsFileAttachmentMetadata, FormFileAttachmentMetadata>();
      this.Map2Ways<DynamicsFileAttachmentMetadataForSave, FormFileAttachmentMetadata>();
      this.Map2Ways<DynamicsSeniorExemptionApplicationDetail, FormSeniorExemptionApplicationDetail>();
      this.Map2Ways<DynamicsSeniorExemptionApplicationDetailForSave, FormSeniorExemptionApplicationDetail>();
      this.Map2Ways<DynamicsSeniorExemptionApplicationFinancial, FormSeniorExemptionApplicationFinancial>();
      this.Map2Ways<DynamicsSeniorExemptionApplicationFinancialForSave, FormSeniorExemptionApplicationFinancial>();
      this.Map2Ways<DynamicsSEAppOccupant, FormSEAppOccupant>();
      this.Map2Ways<DynamicsSEAppOccupantForSave, FormSEAppOccupant>();
      this.Map2Ways<DynamicsSEAppOtherProp, FormSEAppOtherProp>();
      this.Map2Ways<DynamicsSEAppOtherPropForSave, FormSEAppOtherProp>();
      this.Map2Ways<DynamicsSEAppNote, FormSEAppNote>();
      this.Map2Ways<DynamicsParcelDetail, FormParcelDetail>();
      this.Map2Ways<DynamicsMedicarePlan, FormMedicarePlan>();
      this.Map2Ways<DynamicsSEAppPredefNote, FormSEAppPredefNote>();
      this.CreateMap<DynamicsCounty, OutgoingCounty>();
      this.CreateMap<DynamicsYear, OutgoingYear>();
      this.CreateMap<DynamicsCountry, OutgoingCountry>();
    }

    private void Map2Ways<T1, T2>()
    {
      this.CreateMap<T1, T2>();
      this.CreateMap<T2, T1>();
    }
  }
}
