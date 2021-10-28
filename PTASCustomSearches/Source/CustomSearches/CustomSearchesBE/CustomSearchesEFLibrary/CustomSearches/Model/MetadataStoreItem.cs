using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class MetadataStoreItem
    {
        public int MetadataStoreItemId { get; set; }
        public int Version { get; set; }
        public string StoreType { get; set; }
        public string ItemName { get; set; }
        public string Value { get; set; }
    }
}
