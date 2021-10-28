namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Database context for tile data.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class TileFeatureDataDbContext : DbContext
    {
        public const string ConnectionStringPasswordSection = "password";

        /// <summary>
        /// Initializes a new instance of the <see cref="TileFeatureDataDbContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="tokenProvider">The token provider.</param>
        public TileFeatureDataDbContext(DbContextOptions<TileFeatureDataDbContext> options, IServiceTokenProvider tokenProvider)
            : base(options)
        {
            dynamic conn = this.Database?.GetDbConnection();
            if (conn != null)
            {
                // We only used bearer token when password is not provided
                if (!conn.ConnectionString.Contains(TileFeatureDataDbContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
                {
                    string token = Task.Run(async () =>
                    {
                        return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                    }).Result;

                    conn.AccessToken = token;
                }
            }
        }

        /// <summary>
        /// Gets or sets the parcel DbSet.
        /// </summary>
        public virtual DbSet<ParcelFeature> Parcel { get; set; }

        /// <summary>
        /// Gets or sets the parcel DbSet.
        /// </summary>
        public virtual DbSet<ParcelFeatureData> GisMapData2 { get; set; }

        /// <summary>
        /// Gets or sets the LayerSource DbSet.
        /// </summary>
        public virtual DbSet<LayerSource> LayerSource { get; set; }

        /// <summary>
        /// Gets the database connection and open the connection if isn't open.
        /// </summary>
        /// <returns>The database connection.</returns>
        public DbConnection GetOpenConnection()
        {
            var connection = this.Database.GetDbConnection();

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}