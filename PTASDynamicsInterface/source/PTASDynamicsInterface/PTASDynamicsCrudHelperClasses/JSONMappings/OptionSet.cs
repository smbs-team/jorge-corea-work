// <copyright file="OptionSet.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    /// <summary>
    /// Class for the representation of Option Sets.
    /// </summary>
    public class OptionSet
    {
        /// <summary>
        /// Gets or sets Value of the option set.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets Attributename of the option set.
        /// </summary>
        public string Attributename { get; set; }

        /// <summary>
        /// Gets or sets Versionnumber of the option set.
        /// </summary>
        public string Versionnumber { get; set; }

        /// <summary>
        /// Gets or sets Langid of the option set.
        /// </summary>
        public string Langid { get; set; }

        /// <summary>
        /// Gets or sets Objecttypecode of the option set.
        /// </summary>
        public string Objecttypecode { get; set; }

        /// <summary>
        /// Gets or sets Stringmapid of the option set.
        /// </summary>
        public string Stringmapid { get; set; }

        /// <summary>
        /// Gets or sets Organizationid of the option set.
        /// </summary>
        public string Organizationid { get; set; }

        /// <summary>
        /// Gets or sets Displayorder of the option set.
        /// </summary>
        public string Displayorder { get; set; }

        /// <summary>
        /// Gets or sets AttributeValue of the option set.
        /// </summary>
        public int? AttributeValue { get; set; }
    }
}
