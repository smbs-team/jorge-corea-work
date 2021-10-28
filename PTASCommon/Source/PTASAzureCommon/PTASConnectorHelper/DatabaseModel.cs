// <copyright file="DatabaseModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace ConnectorService
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;

    /// <summary>The database model class.</summary>
    public class DatabaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseModel" /> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
        public DatabaseModel(XElement xmlModel)
        {
            Debug.Assert(xmlModel != null, "The xmlModel should not be null");
            this.Entities = new List<EntityModel>();
            if (xmlModel.HasAttributes)
            {
                this.Name = xmlModel.Attribute("name") != null ? xmlModel.Attribute("name").Value : string.Empty;
                this.DatabaseVersion = xmlModel.Attribute("databaseVersion") != null ? xmlModel.Attribute("databaseVersion").Value : "1";
            }

            if (xmlModel.HasElements)
            {
                foreach (var ele in xmlModel.Elements("entity"))
                {
                    this.Entities.Add(new EntityModel(ele));
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
        /// Gets or sets the database version.
        /// </summary>
        /// <value>
        /// The database version.
        /// </value>
        public string DatabaseVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public List<EntityModel> Entities
        {
            get;
            set;
        }
    }
}
