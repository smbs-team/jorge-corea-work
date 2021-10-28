namespace PTASMapTileServicesLibrary.OverlapCalutionProvider
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using PTASMapTileServicesLibrary.OverlapCalutionProvider.Exception;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Provides Overlap calculation services.
    /// </summary>
    public class OverlapCalculationProvider
    {
        /// <summary>
        /// Provides SQL Server db context factory.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly IServiceTokenProvider tokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlapCalculationProvider" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The db context factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <exception cref="ArgumentNullException">dbContextFactory.</exception>
        public OverlapCalculationProvider(IFactory<TileFeatureDataDbContext> dbContextFactory, IServiceTokenProvider tokenProvider)
        {
            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
            this.tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Gets for a parcel the percentage of overlap to polygons in the target layer (the later need to be a SQL layer).
        /// </summary>
        /// <param name="parcelPins">The parcel identifiers.</param>
        /// <param name="layerid">ID of layer for calculate overlap with.</param>
        /// <param name="buffer">Distance value in meters around the poligon to consider as overlap.</param>
        /// <returns>Parcel coordinates (in EPSG4326) if the parcel was found.  Null otherwise.</returns>
        public async Task<List<OverlapData>> GetOverlapCalculation(List<string> parcelPins, long layerid, double buffer)
        {
            List<OverlapData> data = new List<OverlapData>();
            ParcelFeature feature = null;

            System.Text.StringBuilder parcelPinsList = new System.Text.StringBuilder();
            foreach (var item in parcelPins)
            {
                if (parcelPinsList.Length > 0)
                {
                    parcelPinsList.Append(",");
                }

                parcelPinsList.Append($"'{item}'");
            }

            try
            {
                // var conn = System.Configuration.ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
                using (var dbContext = this.dbContextFactory.Create())
                {
                    // feature = await query.FirstOrDefaultAsync();
                    using (var cnn = await this.GetConnectionFromContextAsync(dbContext))
                    {
                        // first get the layer table name and column name
                        using (System.Data.SqlClient.SqlCommand sqlCommandLysrc = new SqlCommand("SELECT TOP 1 [DbTableName],[LayerSourceName],[IsParcelSource], [IsVectorLayer] FROM[gis].[LayerSource] where LayerSourceId = @layerId", cnn))
                        {
                            sqlCommandLysrc.Parameters.AddWithValue("@layerId", layerid);
                            try
                            {
                                await cnn.OpenAsync();

                                var dbLayerTable = string.Empty;
                                using (System.Data.SqlClient.SqlDataReader reader = await sqlCommandLysrc.ExecuteReaderAsync())
                                {
                                    if (reader.Read())
                                    {
                                        dbLayerTable = reader.GetString(0);
                                    }
                                }

                                if (!string.IsNullOrEmpty(dbLayerTable))
                                {
                                    var dbLayerTabelSplit = dbLayerTable.Replace(")", string.Empty).Split('(', StringSplitOptions.RemoveEmptyEntries);
                                    if (dbLayerTabelSplit.Length == 2)
                                    {
                                        var tblName = dbLayerTabelSplit[0];
                                        var colName = dbLayerTabelSplit[1];

                                        using (System.Data.SqlClient.SqlCommand sqlCommand = new SqlCommand($"select parcel.PIN, parcel.Shape.STArea() as parcelArea, wt.[{colName}].STBuffer({buffer}).STIntersection(parcel.Shape).STArea() as overlapArea, wt.* from [gis].[PARCEL_GEOM_AREA] as parcel inner join [gis].[{tblName}]  as wt on wt.{colName}.STIntersects(parcel.[Shape]) = 1 where parcel.PIN in ({parcelPinsList.ToString()}) order by parcel.PIN", cnn))
                                        {
                                            var reader = await sqlCommand.ExecuteReaderAsync();
                                            Dictionary<string, int> foundParcels = new Dictionary<string, int>();
                                            while (reader.Read())
                                            {
                                                OverlapData item = new OverlapData();
                                                item.LayerID = layerid;
                                                item.ParcelPIN = reader.GetString(0);
                                                item.ParcelArea = reader.GetDouble(1);
                                                item.OverlapArea = reader.GetDouble(2);
                                                item.OverlapPercentage = item.OverlapArea / item.ParcelArea;
                                                bool concat = false;
                                                if (foundParcels.ContainsKey(item.ParcelPIN))
                                                {
                                                    concat = true;
                                                    var tmpItem = item;
                                                    item = data[foundParcels[tmpItem.ParcelPIN]];
                                                    item.OverlapArea += tmpItem.OverlapArea;
                                                }

                                                for (int i = 3; i < reader.FieldCount; i++)
                                                {
                                                    var fName = reader.GetName(i);
                                                    if (fName != colName)
                                                    {
                                                        if (item.AdditionalFields.ContainsKey(fName))
                                                        {
                                                            item.AdditionalFields[fName] = item.AdditionalFields[fName] + "," + reader.GetValue(i).ToString();
                                                        }
                                                        else
                                                        {
                                                            item.AdditionalFields.Add(fName, reader.GetValue(i).ToString());
                                                        }
                                                    }
                                                }

                                                if (!concat)
                                                {
                                                    data.Add(item);
                                                    foundParcels.Add(item.ParcelPIN, data.Count - 1);
                                                }
                                            }

                                            // fix overlap percentages.
                                            foreach (var item in data)
                                            {
                                                item.OverlapPercentage = item.OverlapArea / item.ParcelArea;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (SqlException sqlException)
                            {
                                string error = string.Format("Error trying to retrieve parcel geographic data from SQL Server.  Parcel Id: {0}", parcelPinsList.ToString());
                                throw new OverlapCalculationProviderException(OverlapCalculationProviderExceptionCategory.SqlServerError, error, sqlException);
                            }
                        }
                    }
                }
            }
            catch (System.Exception genException)
            {
                string error = string.Format("Error trying to calculate overlap data.  Parcel Id: {0}", parcelPinsList.ToString());
                throw new OverlapCalculationProviderException(OverlapCalculationProviderExceptionCategory.Unkown, error, genException);
            }

            if (data != null)
            {
                return data;
            }

            return null;
        }

        /// <summary>
        /// Gets the connection from context.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The Sql Connection.</returns>
        private async Task<SqlConnection> GetConnectionFromContextAsync(TileFeatureDataDbContext dbContext)
        {
            var currentConnection = dbContext.GetOpenConnection();
            string connectionString = currentConnection.ConnectionString;
            var connection = new SqlConnection(connectionString);

            // We only used bearer token when password is not provided
            if (!currentConnection.ConnectionString.Contains(TileFeatureDataDbContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
            {
                connection.AccessToken = await this.tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
            }

            return connection;
        }
    }
}