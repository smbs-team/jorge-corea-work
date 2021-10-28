namespace CustomSearchesEFLibrary.RScripting
{
    using CustomSearchesEFLibrary.RScripting.Model;
    using Microsoft.EntityFrameworkCore;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Database context for tile storage jobs.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class RScriptJobDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RScriptJobDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="tokenProvider">The token provider.</param>
        public RScriptJobDbContext(DbContextOptions<RScriptJobDbContext> options, IServiceTokenProvider tokenProvider)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the RScriptJobQueue table.
        /// </summary>
        public virtual DbSet<RScriptJobQueue> RScriptJobQueue { get; set; }

        /// <summary>
        /// Gets or sets the RegisteredRScript table.
        /// </summary>
        public virtual DbSet<RegisteredRScript> RegisteredRScript { get; set; }

        /// <summary>
        /// Gets or sets the OutputType field.
        /// </summary>
        public int OutputType { get; set; }
    }
}