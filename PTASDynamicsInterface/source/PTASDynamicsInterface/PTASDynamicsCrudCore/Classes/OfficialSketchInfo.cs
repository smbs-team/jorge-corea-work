// <copyright file="OfficialSketchInfo.cs" company="King County.">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Classes
{
    using System;

    using PTASDynamicsCrudHelperClasses.Classes;

    /// <summary>
    /// Infor for the official sketch of the set.
    /// </summary>
    internal class OfficialSketchInfo
    {
        /// <summary>
        /// Gets or sets sketch Id.
        /// </summary>
        public Guid SketchId { get; set; }

        /// <summary>
        /// Gets or sets item id.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets entity.
        /// </summary>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets sketch info.
        /// </summary>
        public EntityChanges SketchInfo { get; set; }
    }
}