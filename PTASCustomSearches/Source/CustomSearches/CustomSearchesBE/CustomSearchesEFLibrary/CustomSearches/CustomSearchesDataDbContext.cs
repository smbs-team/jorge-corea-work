namespace CustomSearchesEFLibrary.CustomSearches
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Database context for the custom searches backend.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class CustomSearchesDataDbContext : DbContext
    {
        /// <summary>
        /// Gets the name of custom searches schema.
        /// </summary>
        public const string CustomSearchesSchema = "cus";

        /// <summary>
        /// Gets the string format for custom search result table name.
        /// </summary>
        public const string CustomSearchResultTableFormat = "Dataset_{0}";

        /// <summary>
        /// Gets the string format for custom search result table name.
        /// </summary>
        public const string CustomSearchResultUpdateTableFormat = "Dataset_{0}_Update";

        /// <summary>
        /// Gets the string format for custom search result table full name.
        /// </summary>
        public const string CustomSearchResultFullTableFormat = "[cus].[Dataset_{0}]";

        /// <summary>
        /// Gets the string format for custom search update table full name.
        /// </summary>
        public const string CustomSearchResultFullUpdateTableFormat = "[cus].[Dataset_{0}_Update]";

        /// <summary>
        /// Gets the string format for custom search result view full name.
        /// </summary>
        public const string CustomSearchResultFullViewFormat = "[cus].[Dataset_{0}_View]";

        /// <summary>
        /// Gets the string format for post process table name.
        /// </summary>
        public const string PostProcessTableFormat = "Dataset_{0}_PostProcess_{1}";

        /// <summary>
        /// Gets the string format for post process table full name.
        /// </summary>
        public const string PostProcessFullTableFormat = "[cus].[Dataset_{0}_PostProcess_{1}]";

        /// <summary>
        /// Gets the string format for post process view full name.
        /// </summary>
        public const string PostProcessFullViewFormat = "[cus].[Dataset_{0}_PostProcess_View]";

        /// <summary>
        /// The connection string password section.
        /// </summary>
        private const string ConnectionStringPasswordSection = "password";

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesDataDbContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="principalCredentials">The principal credentials.</param>
        /// <exception cref="System.ArgumentNullException">tokenProvider</exception>
        public CustomSearchesDataDbContext(DbContextOptions<CustomSearchesDataDbContext> options, IServiceTokenProvider tokenProvider, ClientCredential principalCredentials)
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
                // We only used bearer token when password is not provided
                if (!conn.ConnectionString.Contains(CustomSearchesDataDbContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
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
        /// Gets the dataset view full name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="usePostProcess">Value indicating whether the results should include the post process.</param>
        /// <returns>The dataset view full name.</returns>
        public static string GetDatasetViewFullName(Dataset dataset, bool usePostProcess)
        {
            string viewFormat = usePostProcess ? PostProcessFullViewFormat : CustomSearchResultFullViewFormat;
            return string.Format(viewFormat, dataset.DatasetId.ToString().Replace("-", "_"));
        }

        /// <summary>
        /// Gets the dataset update table full name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The dataset table full name.</returns>
        public static string GetDatasetUpdateTableFullName(Dataset dataset)
        {
            return CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset.DatasetId);
        }

        /// <summary>
        /// Gets the dataset update table full name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The dataset table full name.</returns>
        public static string GetDatasetUpdateTableName(Dataset dataset)
        {
            return CustomSearchesDataDbContext.GetDatasetUpdateTableName(dataset.DatasetId);
        }

        /// <summary>
        /// Gets the dataset update table full name.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <returns>The dataset table full name.</returns>
        public static string GetDatasetUpdateTableFullName(Guid datasetId)
        {
            return string.Format(CustomSearchResultFullUpdateTableFormat, datasetId.ToString().Replace("-", "_"));
        }

        /// <summary>
        /// Gets the dataset update table full name.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <returns>The dataset table full name.</returns>
        public static string GetDatasetUpdateTableName(Guid datasetId)
        {
            return string.Format(CustomSearchResultUpdateTableFormat, datasetId.ToString().Replace("-", "_"));
        }

        /// <summary>
        /// Gets the dataset table full name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The dataset table full name.</returns>
        public static string GetDatasetTableFullName(Dataset dataset)
        {
            Guid datasetId = (dataset.SourceDatasetId == null || dataset.SourceDatasetId == Guid.Empty) ? dataset.DatasetId : (Guid)dataset.SourceDatasetId;
            return GetDatasetTableFullName(datasetId);
        }

        /// <summary>
        /// Gets the dataset table full name.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <returns>The dataset table full name.</returns>
        public static string GetDatasetTableFullName(Guid datasetId)
        {
            return string.Format(CustomSearchResultFullTableFormat, datasetId.ToString().Replace("-", "_"));
        }

        /// <summary>
        /// Gets the dataset post process table full name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcessId">The dataset post process Id.</param>
        /// <returns>The dataset post process table full name.</returns>
        public static string GetDatasetPostProcessTableFullName(Dataset dataset, int datasetPostProcessId)
        {
            return CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(dataset.DatasetId, datasetPostProcessId);
        }

        /// <summary>
        /// Gets the dataset post process table name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcessId">The dataset post process Id.</param>
        /// <returns>The dataset post process table name.</returns>
        public static string GetDatasetPostProcessTableName(Dataset dataset, int datasetPostProcessId)
        {
            return CustomSearchesDataDbContext.GetDatasetPostProcessTableName(dataset.DatasetId, datasetPostProcessId);
        }

        /// <summary>
        /// Gets the dataset post process table full name.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>The dataset post process table full name.</returns>
        public static string GetDatasetPostProcessTableFullName(DatasetPostProcess datasetPostProcess)
        {
            return CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(datasetPostProcess.DatasetId, datasetPostProcess.DatasetPostProcessId);
        }

        /// <summary>
        /// Gets the dataset post process table name.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>The dataset post process table name.</returns>
        public static string GetDatasetPostProcessTableName(DatasetPostProcess datasetPostProcess)
        {
            return CustomSearchesDataDbContext.GetDatasetPostProcessTableName(datasetPostProcess.DatasetId, datasetPostProcess.DatasetPostProcessId);
        }

        /// <summary>
        /// Gets the dataset post process table full name.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="datasetPostProcessId">The dataset post process Id.</param>
        /// <returns>The dataset post process table full name.</returns>
        public static string GetDatasetPostProcessTableFullName(Guid datasetId, int datasetPostProcessId)
        {
            return string.Format(PostProcessFullTableFormat, datasetId.ToString().Replace("-", "_"), datasetPostProcessId);
        }

        /// <summary>
        /// Gets the dataset post process table name.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="datasetPostProcessId">The dataset post process Id.</param>
        /// <returns>The dataset post process table name.</returns>
        public static string GetDatasetPostProcessTableName(Guid datasetId, int datasetPostProcessId)
        {
            return string.Format(PostProcessTableFormat, datasetId.ToString().Replace("-", "_"), datasetPostProcessId);
        }

        /// <summary>
        /// Gets the dataset table name.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The dataset view full name.</returns>
        public static string GetDatasetTableName(Dataset dataset)
        {
            Guid datasetId = (dataset.SourceDatasetId == null || dataset.SourceDatasetId == Guid.Empty) ? dataset.DatasetId : (Guid)dataset.SourceDatasetId;
            return GetDatasetTableName(datasetId);
        }

        /// <summary>
        /// Gets the dataset table name.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <returns>The dataset view full name.</returns>
        public static string GetDatasetTableName(Guid datasetId)
        {
            return string.Format(CustomSearchResultTableFormat, datasetId.ToString().Replace("-", "_"));
        }
    }
}