namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using NetTopologySuite.Geometries;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.Geography.Projection;
    using PTASMapTileServicesLibrary.Geography.Tile;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.Telemetry;

    /// <summary>
    /// Provides for feature data associated to tiles.
    /// </summary>
    /// <seealso cref="PTASMapTileServicesLibrary.Providers.ITileProvider" />
    public class SqlServerFeatureDataProvider : BaseTileFeatureProvider, ITileFeatureDataProvider
    {
        /// <summary>
        /// The parcel table name.
        /// </summary>
        public const string GeoParcelTableName = "[gis].[PARCEL_GEOM_AREA]";

        /// <summary>
        /// The parcel details table name.
        /// </summary>
        public const string ParcelDetailsTableName = "[dynamics].[ptas_parceldetail]";

        /// <summary>
        /// Dataset for Gis Map data.
        /// </summary>
        public const string GisMapDatasetId = "5F6333E3-6ACB-48F0-9268-B865E521AA08";

        /// <summary>
        /// Name of the resolved view column.
        /// </summary>
        private const string ResolvedViewColumnName = "ResolvedViewName";

        /// <summary>
        /// Provides SQL Server db context factory.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// The telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerFeatureDataProvider" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The db context factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <exception cref="ArgumentNullException">dbContextFactory.</exception>
        public SqlServerFeatureDataProvider(
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            ILogger logger,
            TelemetryClient telemetryClient = null)
            : base(logger)
        {
            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Gets the name of a given dataset.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The table or view name for a dataset.
        /// </returns>
        /// <exception cref="TileFeatureDataProviderException">Dataset table not found for dataset {datasetId} - null.</exception>
        public static string GetDatasetTableName(string datasetId, TileFeatureDataDbContext dbContext)
        {
            datasetId = string.IsNullOrWhiteSpace(datasetId) ? SqlServerFeatureDataProvider.GisMapDatasetId : datasetId;

            string datasetTableName = null;
            string getDatasetViewSql = $"EXEC [cus].[SP_GetDatasetView] @DatasetId = N'{datasetId}'";
            List<dynamic> results = SqlServerFeatureDataProvider.GetDynamicResultAsync(getDatasetViewSql, dbContext).Result.ToList();
            if (results.Count > 0)
            {
                IDictionary<string, object> result = results[0] as IDictionary<string, object>;
                if (result.ContainsKey(SqlServerFeatureDataProvider.ResolvedViewColumnName))
                {
                    string resolvedViewName = result[SqlServerFeatureDataProvider.ResolvedViewColumnName].ToString();
                    if (!string.IsNullOrWhiteSpace(resolvedViewName))
                    {
                        datasetTableName = $"[cus].[{resolvedViewName}]";
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(datasetTableName))
            {
                throw new TileFeatureDataProviderException(
                    TileFeatureDataProviderExceptionCategory.DatasetNotFound,
                    typeof(SqlServerFeatureDataProvider),
                    $"Dataset table not found for dataset {datasetId}",
                    null);
            }

            return datasetTableName;
        }

        /// <summary>
        /// Gets the dynamic results asynchronously.  Note:  IEnumerable is resolved. May
        /// have performance issues with large datasets.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="dataDbContext">The data database context.</param>
        /// <returns>
        /// The dynamic results.
        /// </returns>
        public static async Task<IEnumerable<dynamic>> GetDynamicResultAsync(string commandText, TileFeatureDataDbContext dataDbContext)
        {
            IEnumerable<dynamic> results = SqlServerFeatureDataProvider.GetDynamicResult(commandText, dataDbContext);
            var toReturn = new List<dynamic>();
            var enumerator = results.GetEnumerator();
            bool next = true;
            while (next)
            {
                await Task.Run(() =>
                {
                    next = enumerator.MoveNext();
                    if (next)
                    {
                        toReturn.Add(enumerator.Current);
                    }
                });
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the dynamic results.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="dataDbContext">Data database context.</param>
        /// <returns>The dynamic results.</returns>
        public static IEnumerable<dynamic> GetDynamicResult(string commandText, TileFeatureDataDbContext dataDbContext)
        {
            using (var command = dataDbContext.GetOpenConnection().CreateCommand())
            {
                command.CommandTimeout = 3600;
                command.CommandText = commandText;

                using (var dataReader = command.ExecuteReader())
                {
                    // List for column names
                    var columnNames = new List<string>();

                    if (dataReader.HasRows)
                    {
                        // Add column names to list
                        for (var i = 0; i < dataReader.VisibleFieldCount; i++)
                        {
                            columnNames.Add(dataReader.GetName(i));
                        }

                        while (dataReader.Read())
                        {
                            // Create the dynamic result for each row
                            var result = new ExpandoObject() as IDictionary<string, object>;

                            foreach (var columnName in columnNames)
                            {
                                result.Add(columnName, dataReader[columnName]);
                            }

                            yield return result;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the feature data for a tile.
        /// </summary>
        /// <param name="extent">The extent in EPSG4326.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="layerId">The layer id.</param>
        /// <param name="columns">The columns to be returned.</param>
        /// <param name="datasetId">The dataset id for the dataset where the data is going to be gathered from.
        /// If NULL the default live dataset will be used.</param>
        /// <param name="filterDatasetId">The dataset identifier for a join operation.
        /// Results will be returned if they are both present in the dataset and filter dataset.</param>
        /// <returns>
        /// The tile feature data.
        /// </returns>
        public async Task<FeatureDataResponse> GetTileFeatureData(
            Extent extent,
            int z,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId)
        {
            Extent projectedExtent = ProjectionHelper.ProjectExtent(extent, ProjectionCoordinateSystem.EPSG4326, ProjectionCoordinateSystem.EPSG2926);
            Polygon extentPolygon = TileConversionHelper.GetPolygonFromExtent(projectedExtent);
            string extentPolygonAsText = extentPolygon.AsText();

            try
            {
                using (TileFeatureDataDbContext dbContext = this.dbContextFactory.Create())
                {
                    string datasetTableName = SqlServerFeatureDataProvider.GetDatasetTableName(datasetId, dbContext);
                    string datasetJoinStatement = string.Empty;

                    string parcelTableName = SqlServerFeatureDataProvider.GeoParcelTableName;
                    string parcelDetailsTableName = SqlServerFeatureDataProvider.ParcelDetailsTableName;
                    bool useFilterDataset = !string.IsNullOrWhiteSpace(filterDatasetId);

                    string selectStatement = this.BuildSelectStatement(columns, useFilterDataset);

                    string sqlStatement = $"{selectStatement} FROM {parcelTableName} p ";

                    if (!useFilterDataset)
                    {
                        sqlStatement += $"  LEFT JOIN {datasetTableName} d ON d.[Major] = p.[Major] AND d.[Minor] = p.[Minor]" +
                            $"  INNER JOIN {parcelDetailsTableName} pd ON d.[Major] = pd.[ptas_major] AND d.[Minor] = pd.[ptas_minor]";
                    }
                    else
                    {
                        string joinDatasetTableName = SqlServerFeatureDataProvider.GetDatasetTableName(filterDatasetId, dbContext);
                        sqlStatement += $"    LEFT JOIN( " +
                                        $"      SELECT di.*, jd.Selection AS FilteredDatasetSelection FROM {datasetTableName} di " +
                                        $"        INNER JOIN {joinDatasetTableName} jd ON di.[Major] = jd.[Major] AND di.[Minor] = jd.[Minor] ) d " +
                                        $"      ON d.[Major] = p.[Major] AND d.[Minor] = p.[Minor] " +
                                        $"  INNER JOIN {parcelDetailsTableName} pd ON d.[Major] = pd.[ptas_major] AND d.[Minor] = pd.[ptas_minor]";
                    }

                    sqlStatement += $"  WHERE pd.ptas_snapshottype IS NULL AND SHAPE.STIntersects(geometry::STGeomFromText('{extentPolygonAsText}', 2926)) = 1";

                    FeatureDataResponse featureDataResponse = new FeatureDataResponse();
                    featureDataResponse.LayerId = layerId;
                    featureDataResponse.FeaturesDataCollections = new FeatureDataCollection[1];
                    featureDataResponse.FeaturesDataCollections[0] = new FeatureDataCollection();

                    await TelemetryHelper.TrackPerformanceAsync(
                        "GetTileFeatureData_GetData",
                        async () =>
                        {
                            featureDataResponse.FeaturesDataCollections[0].FeaturesData =
                                await SqlServerFeatureDataProvider.GetDynamicResultAsync(sqlStatement, dbContext);

                            var metrics = new Dictionary<string, double>()
                            {
                                { "GetTileFeatureData_GetData:Rows", (double)featureDataResponse.FeaturesDataCollections[0].FeaturesData.Count() }
                            };

                            return metrics;
                        },
                        this.telemetryClient);

                    return featureDataResponse;
                }
            }
            catch (SqlException sqlException)
            {
                string error = string.Format("Error trying to retrieve feature data from SQL Server.  Layer Id: {0}", layerId);
                throw new TileFeatureDataProviderException(TileFeatureDataProviderExceptionCategory.SqlServerError, this.GetType(), error, sqlException);
            }
        }

        /// <summary>
        /// Loads the tile data asynchronously.
        /// </summary>
        /// <param name="featureDataResponse">The feature data response.</param>
        /// <param name="location">The location.</param>
        /// <param name="collectionIndex">Index of the collection.</param>
        /// <param name="queryExtent">The query extent.</param>
        /// <param name="layerId">The layer identifier.</param>
        /// <param name="columns">The columns to be returned.</param>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="filterDatasetId">The dataset identifier for a join operation.
        /// Results will be returned if they are both present in the dataset and filter dataset.</param>
        /// <returns>An async task.</returns>
        protected async override Task LoadTileData(
            FeatureDataResponse featureDataResponse,
            TileLocation location,
            int collectionIndex,
            Extent queryExtent,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId)
        {
            FeatureDataResponse tileData = await this.GetTileFeatureData(
                queryExtent,
                SqlServerFeatureDataProvider.DefaultSqlQueryZoomLevel,
                layerId,
                columns,
                datasetId,
                filterDatasetId);

            featureDataResponse.FeaturesDataCollections[collectionIndex].FeaturesData = tileData.FeaturesDataCollections[0].FeaturesData;
        }

        /// <summary>
        /// Builds the select statement from a table name and columns array.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns>
        /// The select statement.
        /// </returns>
        private string BuildSelectStatement(string[] columns, bool useFilteredDataset)
        {
            string toReturn = $"SELECT pd.ptas_parcelDetailId, ";

            if (columns == null || columns.Length == 0)
            {
                toReturn += "d.* ";
            }
            else
            {
                int i = 0;
                foreach (var column in columns)
                {
                    toReturn += $" d.{column} ";
                    i++;
                    if (i < columns.Length)
                    {
                        toReturn += ", ";
                    }
                }

                if (useFilteredDataset)
                {
                    toReturn += $", d.FilteredDatasetSelection ";
                }
            }

            return toReturn;
        }
    }
}