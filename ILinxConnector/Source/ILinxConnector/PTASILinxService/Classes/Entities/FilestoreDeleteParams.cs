// <copyright file="FilestoreDeleteParams.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.Entities
{
    /// <summary>
    /// Delete params.
    /// </summary>
    public class FilestoreDeleteParams
    {
        /// <summary>
        /// Gets or sets containerName.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets fileName.
        /// </summary>
        public string FileName { get; set; }
    }
}