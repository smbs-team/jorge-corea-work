namespace PTASMapTileServicesLibrary.Geography.Tile
{
    using System;
    using NetTopologySuite.Geometries;
    using PTASMapTileServicesLibrary.Geography.Data;

    /// <summary>
    /// Helps to convert coordinates between tiles and long/lat.
    /// </summary>
    public static class TileConversionHelper
    {
        /// <summary>
        /// The projection identifier that extent coordinates are expressed in.
        /// </summary>
        public const int ExtentProjectionId = 4326;

        /// <summary>
        /// Gets a tile location from a GeoLocation.
        /// </summary>
        /// <param name="location">The location in EPSG4326.</param>
        /// <param name="zoom">The zoom.</param>
        /// <returns>A tile location.</returns>
        public static TileLocation TileLocationFromGeoLocation(GeoLocation location, int zoom)
        {
            TileLocation p = new TileLocation();
            p.X = (int)Math.Floor((location.Lon + 180.0) / 360.0 * (1 << zoom));
            p.Y = (int)Math.Floor((1.0 - Math.Log(Math.Tan(location.Lat * Math.PI / 180.0) + 1.0 / Math.Cos(location.Lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));

            return p;
        }

        /// <summary>
        /// Gets a geo location from a tile location. (NW corner).
        /// </summary>
        /// <param name="tileLocation">The tile location.</param>
        /// <returns>A geo location in in EPSG4326.</returns>
        public static GeoLocation GeoLocationFromTileLocation(TileLocation tileLocation)
        {
            GeoLocation p = new GeoLocation();
            double n = Math.PI - ((2.0 * Math.PI * tileLocation.Y) / Math.Pow(2.0, tileLocation.Z));

            p.Lon = (float)((tileLocation.X / Math.Pow(2.0, tileLocation.Z) * 360.0) - 180.0);
            p.Lat = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            return p;
        }

        /// <summary>
        /// Gets the extent from a tile location.
        /// </summary>
        /// <param name="tileLocation">The tile location.</param>
        /// <returns>An extent in in EPSG4326.</returns>
        public static Extent ExtentFromTileLocation(TileLocation tileLocation)
        {
            TileLocation nextTile = new TileLocation(tileLocation.X, tileLocation.Y, tileLocation.Z);
            GeoLocation loc0 = TileConversionHelper.GeoLocationFromTileLocation(tileLocation);
            GeoLocation loc1 = TileConversionHelper.GeoLocationFromTileLocation(nextTile);

            return new Extent(loc0.Lon, loc0.Lat, loc1.Lon, loc1.Lat);
        }

        /// <summary>
        /// Gets the extent from a tile location in EPSG4326.
        /// </summary>
        /// <param name="tileExtent">The tile extent.</param>
        /// <returns>An extent.</returns>
        public static Extent ExtentFromTileExtent(TileExtent tileExtent)
        {
            TileLocation newMax = new TileLocation(tileExtent.Max.X + 1, tileExtent.Max.Y + 1, tileExtent.Max.Z);
            GeoLocation loc0 = TileConversionHelper.GeoLocationFromTileLocation(tileExtent.Min);
            GeoLocation loc1 = TileConversionHelper.GeoLocationFromTileLocation(newMax);

            return new Extent(loc0.Lon, loc0.Lat, loc1.Lon, loc1.Lat);
        }

        /// <summary>
        /// Gets the tile extent from a extent.
        /// </summary>
        /// <param name="extent">The tile extent in EPSG4326.</param>
        /// <param name="z">The zoom level.</param>
        /// <returns>A tile extent.</returns>
        public static TileExtent TileExtentFromExtent(Extent extent, int z)
        {
            TileLocation loc0 = TileConversionHelper.TileLocationFromGeoLocation(extent.Min, z);
            TileLocation loc1 = TileConversionHelper.TileLocationFromGeoLocation(extent.Max, z);

            return new TileExtent(loc0.X, loc0.Y, loc1.X, loc1.Y, z);
        }

        /// <summary>
        /// Gets a polygon from a extent.
        /// </summary>
        /// <param name="extent">The tile extent.</param>
        /// <returns>The polygon formed by the extent points.</returns>
        public static Polygon GetPolygonFromExtent(Extent extent)
        {
            PrecisionModel precisionModel = new PrecisionModel(PrecisionModels.Floating);
            GeometryFactory geometryFactory = new GeometryFactory(precisionModel, TileConversionHelper.ExtentProjectionId);
            Coordinate[] extentCoordinates = new Coordinate[5];
            extentCoordinates[0] = new Coordinate(extent.Min.Lon, extent.Min.Lat);
            extentCoordinates[1] = new Coordinate(extent.Min.Lon, extent.Max.Lat);
            extentCoordinates[2] = new Coordinate(extent.Max.Lon, extent.Max.Lat);
            extentCoordinates[3] = new Coordinate(extent.Max.Lon, extent.Min.Lat);
            extentCoordinates[4] = new Coordinate(extent.Min.Lon, extent.Min.Lat);

            return geometryFactory.CreatePolygon(extentCoordinates);
        }

        /// <summary>
        /// Gets a polygon from a tile extent.
        /// </summary>
        /// <param name="tileExtent">The tile extent.</param>
        /// <returns>The polygon formed by the extent points.</returns>
        public static Polygon GetPolygonFromTileExtent(TileExtent tileExtent)
        {
            Extent extent = TileConversionHelper.ExtentFromTileExtent(tileExtent);
            return TileConversionHelper.GetPolygonFromExtent(extent);
        }

        /// <summary>
        /// Changes the zoom level of a tile extent.
        /// </summary>
        /// <param name="tileExtent">The tile extent.</param>
        /// <param name="z">The zoom level.</param>
        /// <returns>A tile extent.</returns>
        public static TileExtent ChangeZoomLevel(TileExtent tileExtent, int z)
        {
            TileExtent toReturn = null;
            if (z != tileExtent.Min.Z)
            {
                Extent extent = TileConversionHelper.ExtentFromTileExtent(tileExtent);
                toReturn = TileConversionHelper.TileExtentFromExtent(extent, z);
            }
            else
            {
                toReturn = new TileExtent(tileExtent.Min.X, tileExtent.Min.Y, tileExtent.Max.X, tileExtent.Max.Y, tileExtent.Min.Z);
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the size of the extent.
        /// </summary>
        /// <param name="tileExtent">The tile extent.</param>
        /// <returns>A tile location with the size.</returns>
        public static TileLocation GetTileExtentSize(TileExtent tileExtent)
        {
            tileExtent.Sort();
            return new TileLocation(tileExtent.Max.X - tileExtent.Min.X + 1, tileExtent.Max.Y - tileExtent.Min.Y + 1, tileExtent.Min.Z);
        }
    }
}
