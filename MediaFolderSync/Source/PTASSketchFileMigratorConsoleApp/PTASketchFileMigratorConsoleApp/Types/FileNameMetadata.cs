// <copyright file="FileNameMetadata.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using MoreLinq.Extensions;
    using Newtonsoft.Json;
    using PTASketchFileMigratorConsoleApp.Constants;

    /// <summary>Defines a file metadata properties.</summary>
    public class FileNameMetadata
    {
        private readonly Dictionary<string, string> typeNames;
        private readonly string[] accessoryTypes;
        private readonly AccAttributes accAttributes;
        private readonly FileType identifiers;
        private readonly IFileSystem fileSystem;
        private string entityName;

        /// <summary>Initializes a new instance of the <see cref="FileNameMetadata"/> class.</summary>
        /// <param name="fileSystem">The file system.</param>
        public FileNameMetadata(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;

            try
            {
                this.accAttributes = JsonConvert.DeserializeObject<AccAttributes>(this.fileSystem.File.ReadAllText(@"./accAttributes.json"));
                this.identifiers = JsonConvert.DeserializeObject<FileType>(this.fileSystem.File.ReadAllText(@"./fileData.json"));
            }
            catch (Exception)
            {
                throw;
            }

            this.typeNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.identifiers.DrawingIds.ForEach(s => this.typeNames.Add(s.Key.Trim(), s.Value.Trim()));
            this.accessoryTypes = this.identifiers.AccessoryDrawingIDs.Select(s => s.ToLower().Trim()).ToArray();
        }

        /// <summary>Gets or sets the mayor.</summary>
        /// <value>The mayor.</value>
        public string Mayor { get; set; }

        /// <summary>Gets or sets the minor.</summary>
        /// <value>The minor.</value>
        public string Minor { get; set; }

        /// <summary>Gets or sets the number that is after the identifier.</summary>
        /// <value>The number.</value>
        public string Number { get; set; }

        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        public string Identifier { get; private set; }

        /// <summary>Gets the accessory attribute value.</summary>
        /// <value>The accessory  attribute value.</value>
        public int AccAttributeValue { get; private set; }

        /// <summary>Gets the name of the accessory  attribute.</summary>
        /// <value>The name of the accessory  attribute.</value>
        public string AccAttributeName { get; private set; }

        /// <summary>Gets the type of the accessory.</summary>
        /// <value>The type of the accessory.</value>
        public string AccType { get; private set; }

        /// <summary>Gets or sets the full name including identifier.</summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }

        /// <summary>Gets or sets the name, it excludes the identifier.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified date.</value>
        public DateTime ModifiedDate { get; set; }

        /// <summary>Gets or sets the entity name.</summary>
        /// <value>The entity name.</value>
        public string EntityName
        {
            get
            {
                return this.entityName;
            }

            set
            {
                var lowerValue = value.ToLower();
                this.Identifier = value.ToLower();

                if (string.IsNullOrEmpty(lowerValue))
                {
                    this.entityName = EntityNames.Building;
                    this.Identifier = "b";
                    return;
                }

                if (this.accessoryTypes.Contains(lowerValue))
                {
                    this.entityName = EntityNames.Accessory;
                    var accessory = this.accAttributes.Value.Where(a => a.DrawingId.ToLower() == lowerValue).FirstOrDefault();
                    if (accessory == null)
                    {
                        return;
                    }

                    this.AccAttributeName = accessory.Value;
                    this.AccAttributeValue = accessory.AttributeValue;
                    this.AccType = accessory.Type;
                    return;
                }

                if (this.typeNames.ContainsKey(lowerValue))
                {
                  this.entityName = this.typeNames[lowerValue];
                  return;
                }

                this.entityName = string.Empty;
            }
        }
    }
}
