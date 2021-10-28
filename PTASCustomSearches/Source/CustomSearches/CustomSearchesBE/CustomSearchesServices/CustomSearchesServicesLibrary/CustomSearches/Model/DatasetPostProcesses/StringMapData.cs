namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using System;
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Model for the string map data.
    /// </summary>
    public class StringMapData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringMapData" /> class.
        /// </summary>
        /// <param name="stringMap">The string map.</param>
        public StringMapData(Stringmap stringMap)
        {
            this.StringMapId = stringMap.Stringmapid;
            this.AttributeName = stringMap.Attributename;
            this.AttributeValue = stringMap.Attributevalue;
            this.DisplayOrder = stringMap.Displayorder;
            this.LangId = stringMap.Langid;
            this.ObjectTypeCode = stringMap.Objecttypecode;
            this.Value = stringMap.Value;
            this.VersionNumber = stringMap.Versionnumber;
            this.OrganizationId = stringMap.Organizationid;
            this.ModifiedOn = stringMap.Modifiedon;
        }

        /// <summary>
        /// Gets or sets the string map id.
        /// </summary>
        public Guid StringMapId { get; set; }

        /// <summary>
        /// Gets or sets the attribute name.
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets or sets the attribute value.
        /// </summary>
        public long? AttributeValue { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int? LangId { get; set; }

        /// <summary>
        /// Gets or sets the object type code.
        /// </summary>
        public string ObjectTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public long? VersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        public DateTime ModifiedOn { get; set; }
    }
}
