// <copyright file="EntityModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace ConnectorService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;

    /// <summary>Creates an entity model.</summary>
    public class EntityModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityModel" /> class.
        /// </summary>
        /// <param name="xmlEntity">The XML entity.</param>
        public EntityModel(XElement xmlEntity)
        {
            Debug.Assert(xmlEntity != null, "The xmlEntity should not be null");
            this.Attributes = new List<EntityAttributeModel>();
            this.Relationships = new List<EntityRelationshipModel>();
            if (xmlEntity.HasAttributes)
            {
                this.Name = xmlEntity.Attribute("name") != null ? xmlEntity.Attribute("name").Value : string.Empty;
                this.Syncable = xmlEntity.Attribute("syncable") != null ? (xmlEntity.Attribute("syncable").Value.ToString().ToUpperInvariant().Equals("YES") ? true : false) : false;
                this.SyncOrder = xmlEntity.Attribute("syncOrder") != null ? Convert.ToInt32(xmlEntity.Attribute("syncOrder").Value) : 0;
                this.IsDefault = xmlEntity.Attribute("isDefault") != null ? Convert.ToBoolean(xmlEntity.Attribute("isDefault").Value) : false;
                this.IsMediaEntity = xmlEntity.Attribute("isMediaEntity") != null ? Convert.ToBoolean(xmlEntity.Attribute("isMediaEntity").Value) : false;
                this.FriendlyName = xmlEntity.Attribute("friendlyName") != null ? xmlEntity.Attribute("friendlyName").Value : string.Empty;
                this.IsRootRelated = xmlEntity.Attribute("isRootRelated") != null ? Convert.ToBoolean(xmlEntity.Attribute("isRootRelated").Value) : false;
                this.BackendQuery = xmlEntity.Attribute("backendQuery") != null ? xmlEntity.Attribute("backendQuery").Value : string.Empty;
            }

            if (xmlEntity.HasElements)
            {
                foreach (var ele in xmlEntity.Elements("attribute"))
                {
                    this.Attributes.Add(new EntityAttributeModel(ele));
                }

                foreach (var ele in xmlEntity.Elements("relationship"))
                {
                    this.Relationships.Add(new EntityRelationshipModel(ele));
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
        /// Gets or sets a value indicating whether this <see cref="EntityModel" /> is sync-able.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sync-able; otherwise, <c>false</c>.
        /// </value>
        public bool Syncable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sync order.
        /// </summary>
        /// <value>
        /// The sync order.
        /// </value>
        public int SyncOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is media entity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is media entity; otherwise, <c>false</c>.
        /// </value>
        public bool IsMediaEntity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is root related.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is root related; otherwise, <c>false</c>.
        /// </value>
        public bool IsRootRelated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the back-end query.
        /// </summary>
        /// <value>
        /// The back-end query.
        /// </value>
        public string BackendQuery
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public List<EntityAttributeModel> Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the relationships.
        /// </summary>
        /// <value>
        /// The relationships.
        /// </value>
        public List<EntityRelationshipModel> Relationships
        {
            get;
            set;
        }
    }
}
