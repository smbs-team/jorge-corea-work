// <copyright file="DrawingMetadata.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    /// <summary>Holds visual CADD drawing metadata needed for queries.</summary>
    public class DrawingMetadata
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>Gets or sets the attribute value.</summary>
        /// <value>The attribute value.</value>
        public int AttributeValue { get; set; }

        /// <summary>Gets or sets the drawing identifier.</summary>
        /// <value>The drawing identifier.</value>
        public string DrawingId { get; set; }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public string Type { get; set; }
    }
}
