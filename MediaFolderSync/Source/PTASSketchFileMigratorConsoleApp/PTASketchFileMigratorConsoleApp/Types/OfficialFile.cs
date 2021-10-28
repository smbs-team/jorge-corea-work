// <copyright file="OfficialFile.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp.Types
{
    /// <summary>Data for object marked as official.</summary>
    public class OfficialFile
    {
        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>Gets or sets the name of the sketch.</summary>
        /// <value>The name of the sketch.</value>
        public string SketchName { get; set; }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>Gets or sets the entity result.</summary>
        /// <value>The entity result.</value>
        public EntityResult EntityResult { get; set; }
    }
}
