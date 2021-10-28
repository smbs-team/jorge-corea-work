namespace CustomSearchesEFLibrary.WorkerJob
{
    using System.Linq;
    using System.Threading.Tasks;

    using CustomSearchesEFLibrary.WorkerJob.Model;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Database context for tile storage jobs.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class WorkerJobDbContext : DbContext
    {
        /// <summary>
        /// The connection string password section.
        /// </summary>
        private const string ConnectionStringPasswordSection = "password";

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerJobDbContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="principalCredentials">The principal credentials.</param>
        /// <exception cref="System.ArgumentNullException">tokenProvider.</exception>
        public WorkerJobDbContext(DbContextOptions<WorkerJobDbContext> options, IServiceTokenProvider tokenProvider, ClientCredential principalCredentials)
            : base(options)
        {
            if (tokenProvider == null)
            {
                throw new System.ArgumentNullException(nameof(tokenProvider));
            }

            dynamic conn = null;
            try
            {
                conn = this.Database?.GetDbConnection();
            }
            catch (System.InvalidOperationException)
            {
                // System.InvalidOperationException is thrown when this is a memory database.  We consider the connection null in this case.
            }

            if (conn != null)
            {
                string connectionStr = conn.ConnectionString;

                // We only used bearer token when password is not provided
                if (!(connectionStr.Contains(ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase) || connectionStr.Contains("local")))
                {
                    string token = null;
                    if (principalCredentials == null)
                    {
                        token = Task.Run(async () =>
                        {
                            return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                        }).Result;
                    }
                    else
                    {
                        token = Task.Run(async () =>
                        {
                            return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantId, principalCredentials);
                        }).Result;
                    }

                    conn.AccessToken = token;
                }
            }
        }

        /// <summary>
        /// Gets or sets the RScriptJobQueue table.
        /// </summary>
        public virtual DbSet<WorkerJobQueue> WorkerJobQueue { get; set; }

        /// <summary>
        /// Pops the next storage job.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="timeThreshold">The time threshold.</param>
        /// <returns>
        /// The next pending storage job if there is any.
        /// </returns>
        public virtual WorkerJobQueue PopNextWorkerJob(string queueName, int timeThreshold)
        {
            if (timeThreshold == 0)
            {
                return null;
            }

            return this.WorkerJobQueue.FromSqlInterpolated($"exec [dbo].[SP_PopNextWorkerJobV2] {queueName}, {timeThreshold}").ToList().FirstOrDefault();
        }
    }
}