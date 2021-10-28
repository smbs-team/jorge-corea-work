// <copyright file="EntityAttributeModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace ConnectorService
{
    using System;
    using System.Diagnostics;
    using System.Xml.Linq;

    /// <summary>Creates an entity attribute model.</summary>
    public class EntityAttributeModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAttributeModel" /> class.
        /// </summary>
        /// <param name="xmlAttribute">The XML attribute.</param>
        public EntityAttributeModel(XElement xmlAttribute)
        {
            Debug.Assert(xmlAttribute != null, "The xmlAttribute should not be null");
            if (xmlAttribute.HasAttributes)
            {
                this.Name = xmlAttribute.Attribute("name") != null ? xmlAttribute.Attribute("name").Value : string.Empty;
                this.AttributeType = xmlAttribute.Attribute("attributeType") != null ? xmlAttribute.Attribute("attributeType").Value : string.Empty;
                this.IsClientKey = xmlAttribute.Attribute("isClientKey") != null ? Convert.ToBoolean(xmlAttribute.Attribute("isClientKey").Value) : false;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the attribute.
        /// </summary>
        /// <value>
        /// The type of the attribute.
        /// </value>
        public string AttributeType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is client key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is client key; otherwise, <c>false</c>.
        /// </value>
        public bool IsClientKey
        {
            get;
            set;
        }
    }
}
