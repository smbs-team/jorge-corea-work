// <copyright file="EntityRelationshipModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
namespace ConnectorService
{
    using System.Diagnostics;
    using System.Xml.Linq;

    /// <summary>Creates an entity relationships model.</summary>
    public class EntityRelationshipModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRelationshipModel" /> class.
        /// </summary>
        /// <param name="xmlRelationship">The XML relationship.</param>
        public EntityRelationshipModel(XElement xmlRelationship)
        {
            Debug.Assert(xmlRelationship != null, "The xmlRelationship should not be null");
            if (xmlRelationship.HasAttributes)
            {
                this.Name = xmlRelationship.Attribute("name") != null ? xmlRelationship.Attribute("name").Value : string.Empty;
                this.DestinationEntity = xmlRelationship.Attribute("destinationEntity") != null ? xmlRelationship.Attribute("destinationEntity").Value : string.Empty;
                this.ToMany = xmlRelationship.Attribute("toMany") != null ? (xmlRelationship.Attribute("toMany").Value.ToString().ToUpperInvariant().Equals("YES") ? true : false) : false;
                this.InverseEntity = xmlRelationship.Attribute("inverseEntity") != null ? xmlRelationship.Attribute("inverseEntity").Value : string.Empty;
                this.InverseName = xmlRelationship.Attribute("inverseName") != null ? xmlRelationship.Attribute("inverseName").Value : string.Empty;

                foreach (var clientKey in xmlRelationship.Elements("clientKey"))
                {
                    this.SourceKey = clientKey.Attribute("sourceName").Value.ToString();
                    this.DestinationKey = clientKey.Attribute("destinationName").Value.ToString();
                }
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
        /// Gets or sets the destination entity.
        /// </summary>
        /// <value>
        /// The destination entity.
        /// </value>
        public string DestinationEntity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [to many].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [to many]; otherwise, <c>false</c>.
        /// </value>
        public bool ToMany
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source key.
        /// </summary>
        /// <value>
        /// The destination entity.
        /// </value>
        public string SourceKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the destination key.
        /// </summary>
        /// <value>
        /// The destination key.
        /// </value>
        public string DestinationKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inverse relationship name.
        /// </summary>
        /// <value>
        /// The inverse relationship name.
        /// </value>
        public string InverseName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inverse entity name.
        /// </summary>
        /// <value>
        /// The inverse entity name.
        /// </value>
        public string InverseEntity
        {
            get;
            set;
        }
    }
}
