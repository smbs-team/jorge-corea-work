// <copyright file="IFileAttachmentMetadataManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Generic FileAttachmentMetadata manager.
    /// </summary>
    public interface IFileAttachmentMetadataManager
    {
        /// <summary>
        /// Get the FileAttachmentMetadata Info that contains senior exemption application Id.
        /// </summary>
        /// <param name="sEAppId">Id to search.</param>
        /// <returns>File Attchaments Metadata look up data or null.</returns>
        Task<List<DynamicsFileAttachmentMetadata>> GetFileAttchamentMetadataFromSEAppId(string sEAppId);
    }
}