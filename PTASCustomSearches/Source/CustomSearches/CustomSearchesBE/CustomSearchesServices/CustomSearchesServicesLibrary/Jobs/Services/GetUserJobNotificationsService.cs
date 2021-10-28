namespace CustomSearchesServicesLibrary.Jobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.Jobs.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the user job notifications.
    /// </summary>
    public class GetUserJobNotificationsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserJobNotificationsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserJobNotificationsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the user notifications for jobs.
        /// </summary>
        /// <param name="maxJobs">The maximum jobs to return.</param>
        /// <param name="maxNotifications">The maximum notifications to return.</param>
        /// <returns>
        /// The job notifications and pending jobs.
        /// </returns>
        public async Task<GetUserJobNotificationsResponse> GetUserJobNotifications(int maxJobs, int maxNotifications)
        {
            var toReturn = new GetUserJobNotificationsResponse();

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            IEnumerable<WorkerJobQueue> pendingJobs = await DbTransientRetryPolicy.GetPendingWorkerJobsAsync(
               this.ServiceContext,
               userId,
               includePayload: true,
               maxPayloadSize: 1024 * 24, // Max 24k of payload per item.
               maxJobs);

            toReturn.PendingJobs = (from j in pendingJobs select new JobData(j)).ToArray();

            using (var dbContext = this.ServiceContext.DbContextFactory.Create())
            {
                var jobNotifications = await
                    (from jn in dbContext.UserJobNotification
                     where jn.Dismissed == false && jn.UserId == userId
                     orderby jn.CreatedTimestamp descending
                     select jn).Take(maxNotifications).ToListAsync();

                toReturn.JobNotifications = (from jn in jobNotifications select new JobNotificationData(jn)).ToArray();
            }

            return toReturn;
        }
    }
}
