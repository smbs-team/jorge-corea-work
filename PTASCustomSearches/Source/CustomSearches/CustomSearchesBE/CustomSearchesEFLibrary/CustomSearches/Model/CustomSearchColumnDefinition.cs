using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class CustomSearchColumnDefinition
    {
        public CustomSearchColumnDefinition()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
        }

        public int CustomSearchColumnDefinitionId { get; set; }
        public int CustomSearchDefinitionId { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public int ColumnTypeLength { get; set; }
        public bool? CanBeUsedAsLookup { get; set; }
        public string ColumnCategory { get; set; }
        public bool IsEditable { get; set; }
        public string BackendEntityName { get; set; }
        public string BackendEntityFieldName { get; set; }
        public bool ForceEditLookupExpression { get; set; }
        public string ColumnEditRoles { get; set; }
        public string ColumDefinitionExtensions { get; set; }
        public string DependsOnColumn { get; set; }

        public virtual DataType ColumnTypeNavigation { get; set; }
        public virtual CustomSearchDefinition CustomSearchDefinition { get; set; }
        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
    }
}
