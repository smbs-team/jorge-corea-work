using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class CustomSearchParameter
    {
        public CustomSearchParameter()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
        }

        public int CustomSearchParameterId { get; set; }
        public string OwnerType { get; set; }
        public int? CustomSearchDefinitionId { get; set; }
        public string ParameterGroupName { get; set; }
        public string ParameterName { get; set; }
        public string ParameterDescription { get; set; }
        public string ParameterDataType { get; set; }
        public string ParameterRangeType { get; set; }
        public int ParameterTypeLength { get; set; }
        public string ParameterDefaultValue { get; set; }
        public bool ParameterIsRequired { get; set; }
        public bool ForceEditLookupExpression { get; set; }
        public int? RscriptModelId { get; set; }
        public bool AllowMultipleSelection { get; set; }
        public int DisplayOrder { get; set; }
        public string DisplayName { get; set; }
        public string ParameterExtensions { get; set; }

        public virtual CustomSearchDefinition CustomSearchDefinition { get; set; }
        public virtual OwnerType OwnerTypeNavigation { get; set; }
        public virtual DataType ParameterDataTypeNavigation { get; set; }
        public virtual RangeType ParameterRangeTypeNavigation { get; set; }
        public virtual RscriptModel RscriptModel { get; set; }
        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
    }
}
