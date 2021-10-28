// <copyright file="FileAttachmentMetadataManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Implementations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Parcel manager implementation.
    /// </summary>
    public class FileAttachmentMetadataManager : IFileAttachmentMetadataManager
    {
        private const string TableName = "ptas_fileattachmentmetadatas";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttachmentMetadataManager"/> class.
        /// </summary>
        /// <param name="crmWrapper">CRM wrapper from dependency.</param>
        public FileAttachmentMetadataManager(CRMWrapper crmWrapper)
        {
            this.CrmWrapper = crmWrapper;
        }

        /// <summary>
        /// Gets the current CRM Wrapper to process requests.
        /// </summary>
        private CRMWrapper CrmWrapper { get; }

        /// <inheritdoc/>
        public virtual async Task<List<DynamicsFileAttachmentMetadata>> GetFileAttchamentMetadataFromSEAppId(string sEAppId) =>
            await this.ExecuteQuery($"$filter=_ptas_seniorexemptionapplication_value eq '{sEAppId}'");

        private async Task<List<DynamicsFileAttachmentMetadata>> ExecuteQuery(string query)
        {
            var results = await this.CrmWrapper.ExecuteGet<DynamicsFileAttachmentMetadata>(TableName, query);
            List<DynamicsFileAttachmentMetadata> resultList = new List<DynamicsFileAttachmentMetadata>();

            if (results != null)
            {
                foreach (var item in results)
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }
    }
}
