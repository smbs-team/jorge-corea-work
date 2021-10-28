// <copyright file="EntityResult.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp.Types
{
    using System.Collections.Generic;
    using PTASketchFileMigratorConsoleApp.Constants;

    /// <summary>Holds the result of searching the VCD file name in the database.</summary>
    public class EntityResult
    {
        /// <summary>Gets or sets the sketch identifier.</summary>
        /// <value>The sketch identifier.</value>
        public string SketchId { get; set; }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>Gets or sets the name of the entity.</summary>
        /// <value>The name of the entity.</value>
        public string EntityName { get; set; }

        /// <summary>Gets or sets a value indicating whether returns true if ... is valid.</summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid { get; set; }

        /// <summary>Builds the object needed to mark the sketch as completed.</summary>
        /// <returns>
        ///   <para>The object.</para>
        /// </returns>
        public Dictionary<string, string> GetData()
        {
            var entityId = string.Empty;
            switch (this.EntityName)
            {
                case EntityNames.Building:
                    entityId = "buildingId";
                    break;
                case EntityNames.Accessory:
                    entityId = "accesoryId";
                    break;
                case EntityNames.Unit:
                    entityId = "unitId";
                    break;
                default:
                    break;
            }

            var data = new Dictionary<string, string>
            {
                { "sketchId", this.SketchId },
                { entityId, this.Id },
            };

            return data;
        }
    }
}
