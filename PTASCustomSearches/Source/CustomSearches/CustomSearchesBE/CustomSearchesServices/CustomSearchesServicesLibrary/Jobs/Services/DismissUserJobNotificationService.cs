namespace CustomSearchesServicesLibrary.Jobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Jobs.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that dismisses the user job notifications.
    /// </summary>
    public class DismissUserJobNotificationService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DismissUserJobNotificationService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DismissUserJobNotificationService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Dismisses the user notifications for jobs.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        /// <returns>The async task.</returns>
        public async Task DismissUserJobNotification(int notificationId)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            if (notificationId >= 0)
            {
                using (var dbContext = this.ServiceContext.DbContextFactory.Create())
                {
                    var jobNotification = await
                        (from jn in dbContext.UserJobNotification
                            where jn.JobNotificationId == notificationId && jn.UserId == userId
                            select jn).FirstOrDefaultAsync();

                    InputValidationHelper.AssertEntityExists(jobNotification, "UserJobNotification", notificationId);
                    jobNotification.Dismissed = true;
                    await dbContext.SaveChangesWithRetriesAsync();
                }
            }
            else
            {
                var sqlParameter = new SqlParameter("UserId", userId.ToString());
                await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                    this.ServiceContext,
                    this.ServiceContext.DbContextFactory,
                    "UPDATE [dbo].[UserJobNotification] SET Dismissed = 1 WHERE Dismissed = 0 AND UserId = @UserId",
                    new SqlParameter[] { sqlParameter },
                    $"Database error while dismissing notifications for user '{userId}'.");
            }
        }
    }
}
