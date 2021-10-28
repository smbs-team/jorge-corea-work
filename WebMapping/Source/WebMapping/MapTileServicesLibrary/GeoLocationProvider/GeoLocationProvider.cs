namespace PTASMapTileServicesLibrary.GeoLocationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.Geography.Projection;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Provides GeoLocation Services.
    /// </summary>
    public class GeoLocationProvider
    {
        /// <summary>
        /// Provides SQL Server db context factory.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocationProvider"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The db context factory.</param>
        public GeoLocationProvider(IFactory<TileFeatureDataDbContext> dbContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Gets the parcel coordinates for a specific parcel.
        /// </summary>
        /// <param name="parcelPin">The parcel identifier.</param>
        /// <returns>Parcel coordinates (in EPSG4326) if the parcel was found.  Null otherwise.</returns>
        public async Task<Extent> GetParcelCoordinates(string parcelPin)
        {
            ParcelFeature feature = null;
            try
            {
                using (var dbContext = this.dbContextFactory.Create())
                {
                    var query = from p in dbContext.Parcel
                                where p.Pin == parcelPin
                                select p;

                    feature = await query.FirstOrDefaultAsync();
                }
            }
            catch (SqlException sqlException)
            {
                string error = string.Format("Error trying to retrieve parcel geographic location from SQL Server.  Parcel Id: {0}", parcelPin);
                throw new GeoLocationProviderException(GeoLocationProviderExceptionCategory.SqlServerError, error, sqlException);
            }

            if (feature != null)
            {
                Extent sourceProjectionExtent =
                    new Extent(feature.Shape.EnvelopeInternal.MinX, feature.Shape.EnvelopeInternal.MinY, feature.Shape.EnvelopeInternal.MaxX, feature.Shape.EnvelopeInternal.MaxY);
                return ProjectionHelper.ProjectExtent(sourceProjectionExtent, ProjectionCoordinateSystem.EPSG2926, ProjectionCoordinateSystem.EPSG4326);
            }

            return null;
        }

        /// <summary>
        /// Gets the dataset coordinates for a specific parcel.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="filterParameter">Valid values: "selected"/"notSelected"/null.
        /// If null returns all the parcels.
        /// If "selected" returns all the selected parcels.
        /// It "notSelected" returns all the not selected parcels.</param>
        /// <returns>
        /// If the dataset is found, returns parcel coordinates for each parcel in the dataset.  Null otherwise.
        /// </returns>
        public async Task<FeatureDataResponse> GetDatasetCoordinates(Guid datasetId, string filterParameter = null)
        {
            try
            {
                using (var dbContext = this.dbContextFactory.Create())
                {
                    bool isGisMapData = datasetId.ToString() == SqlServerFeatureDataProvider.GisMapDatasetId;

                    string datasetTableName = SqlServerFeatureDataProvider.GetDatasetTableName(datasetId.ToString(), dbContext);
                    string parcelTableName = SqlServerFeatureDataProvider.GeoParcelTableName;
                    string selectionFilter = string.Empty;

                    if (filterParameter?.ToLower() == "selected")
                    {
                        selectionFilter = "WHERE d.Selection = 1 ";
                    }
                    else if (filterParameter?.ToLower() == "notselected")
                    {
                        selectionFilter = "WHERE d.Selection = 0 ";
                    }

                    string sqlStatement =
                        $"SELECT p.[Major], p.[Minor], p.[Lat], p.[Long], p.[InSurfaceLat], p.[InSurfaceLong] FROM {parcelTableName} p ";

                    if (!isGisMapData)
                    {
                        sqlStatement += $"INNER JOIN {datasetTableName} d ON d.[Major] = p.[Major] AND d.[Minor] = p.[Minor] ";
                    }

                    sqlStatement += selectionFilter;

                    FeatureDataResponse featureDataResponse = new FeatureDataResponse();
                    featureDataResponse.FeaturesDataCollections = new FeatureDataCollection[1];
                    featureDataResponse.FeaturesDataCollections[0] = new FeatureDataCollection();
                    featureDataResponse.FeaturesDataCollections[0].FeaturesData = (await SqlServerFeatureDataProvider.GetDynamicResultAsync(sqlStatement, dbContext)).ToArray();

                    var coordinates = new List<GeoLocation>();
                    var onSurfaceCoordinates = new List<GeoLocation>();

                    foreach (object featureData in featureDataResponse.FeaturesDataCollections[0].FeaturesData)
                    {
                        var featureAsDictionary = featureData as IDictionary<string, object>;
                        coordinates.Add(new GeoLocation((double)featureAsDictionary["Long"], (double)featureAsDictionary["Lat"]));
                        onSurfaceCoordinates.Add(new GeoLocation((double)featureAsDictionary["InSurfaceLong"], (double)featureAsDictionary["InSurfaceLat"]));
                    }

                    var projectedCoordinates = ProjectionHelper.ProjectArray(coordinates.ToArray(), ProjectionCoordinateSystem.EPSG2926, ProjectionCoordinateSystem.EPSG4326);
                    var projectedOnSurfaceCoordinates = ProjectionHelper.ProjectArray(onSurfaceCoordinates.ToArray(), ProjectionCoordinateSystem.EPSG2926, ProjectionCoordinateSystem.EPSG4326);

                    int i = 0;
                    foreach (object featureData in featureDataResponse.FeaturesDataCollections[0].FeaturesData)
                    {
                        var featureAsDictionary = featureData as IDictionary<string, object>;
                        featureAsDictionary["Long"] = projectedCoordinates[i].Lon;
                        featureAsDictionary["Lat"] = projectedCoordinates[i].Lat;
                        featureAsDictionary["InSurfaceLong"] = projectedOnSurfaceCoordinates[i].Lon;
                        featureAsDictionary["InSurfaceLat"] = projectedOnSurfaceCoordinates[i].Lat;

                        i++;
                    }

                    return featureDataResponse;
                }
            }
            catch (SqlException sqlException)
            {
                string error = string.Format("Error trying to retrieve dataset geographic location from SQL Server.  Dataset Id: {0}", datasetId);
                throw new GeoLocationProviderException(GeoLocationProviderExceptionCategory.SqlServerError, error, sqlException);
            }
        }
    }
}