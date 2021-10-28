// <copyright file="OdataResponse.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    /// <summary>Odata get response object.</summary>
    public class OdataResponse
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public Sketch[] Value { get; set; }
    }
}
