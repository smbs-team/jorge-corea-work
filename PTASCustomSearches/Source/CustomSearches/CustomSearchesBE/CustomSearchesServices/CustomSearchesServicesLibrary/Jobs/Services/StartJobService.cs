namespace CustomSearchesServicesLibrary.Jobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Jobs.Enumeration;
    using CustomSearchesServicesLibrary.Jobs.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Newtonsoft.Json;

    /// <summary>
    /// Service that starts a new job.
    /// </summary>
    public class StartJobService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartJobService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public StartJobService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the job status.
        /// </summary>
        /// <param name="queue">The queue.</param>
        /// <param name="jobType">Type of the job.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// The job status.
        /// </returns>
        public async Task<int> StartJobAsync(string queue, string jobType, object payload, int timeout)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("StartJob");
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            return await this.ServiceContext.AddWorkerJobQueueAsync(queue, jobType, userId, payload, timeout);
        }
    }
}
