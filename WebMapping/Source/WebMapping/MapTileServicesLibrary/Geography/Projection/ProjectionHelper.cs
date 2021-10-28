namespace PTASMapTileServicesLibrary.Geography.Projection
{
    using System;
    using System.Collections.Generic;
    using GeoAPI.CoordinateSystems;
    using GeoAPI.CoordinateSystems.Transformations;
    using GeoAPI.Geometries;
    using ProjNet.Converters.WellKnownText;
    using ProjNet.CoordinateSystems.Transformations;
    using PTASMapTileServicesLibrary.Geography.Data;

    /// <summary>
    /// Helps with geographic projection related operations.
    /// </summary>
    public class ProjectionHelper
    {
        /// <summary>
        /// Caches the coordinate systems so there is no need for constant parsing.
        /// </summary>
        private static readonly IDictionary<ProjectionCoordinateSystem, ICoordinateSystem> CoordinateSystems = new Dictionary<ProjectionCoordinateSystem, ICoordinateSystem>();

        /// <summary>
        /// Projects a location from one coordinate system to another.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="sourceProjection">The source projection.</param>
        /// <param name="targetProjection">The target projection.</param>
        /// <returns>The transformed coordinate.</returns>
        public static GeoLocation ProjectLocation(GeoLocation location, ProjectionCoordinateSystem sourceProjection, ProjectionCoordinateSystem targetProjection)
        {
            GeoLocation[] fromPoint = new GeoLocation[] { location };
            GeoLocation[] toPoint = ProjectionHelper.ProjectArray(fromPoint, sourceProjection, targetProjection);
            return toPoint[0];
        }

        /// <summary>
        /// Projects an extent from one coordinate system to another.
        /// </summary>
        /// <param name="extent">The extent.</param>
        /// <param name="sourceProjection">The source projection.</param>
        /// <param name="targetProjection">The target projection.</param>
        /// <returns>The transformed coordinate.</returns>
        public static Extent ProjectExtent(Extent extent, ProjectionCoordinateSystem sourceProjection, ProjectionCoordinateSystem targetProjection)
        {
            GeoLocation[] fromPoint = new GeoLocation[] { extent.Min, extent.Max };
            GeoLocation[] toPoint = ProjectionHelper.ProjectArray(fromPoint, sourceProjection, targetProjection);

            return new Extent(toPoint[0].Lon, toPoint[0].Lat, toPoint[1].Lon, toPoint[1].Lat);
        }

        /// <summary>
        /// Projects a location array from one coordinate system to another.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="sourceProjection">The source projection.</param>
        /// <param name="targetProjection">The target projection.</param>
        /// <returns>The transformed coordinate.</returns>
        public static GeoLocation[] ProjectArray(GeoLocation[] coordinates, ProjectionCoordinateSystem sourceProjection, ProjectionCoordinateSystem targetProjection)
        {
            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateSystem fromCs = ProjectionHelper.GetCoordinateSystem(sourceProjection);
            ICoordinateSystem toCs = ProjectionHelper.GetCoordinateSystem(targetProjection);
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(fromCs, toCs);
            var results = new List<GeoLocation>();
            foreach (var coord in coordinates)
            {
                double[] fromPoint = { coord.Lon, coord.Lat };
                double[] toPoint = trans.MathTransform.Transform(fromPoint);
                results.Add(new GeoLocation(toPoint[0], toPoint[1]));
            }

            return results.ToArray();
        }

        /// <summary>
        /// Gets the coordinate system.
        /// </summary>
        /// <param name="projection">The projection.</param>
        /// <returns>A coordinate system.</returns>
        private static ICoordinateSystem GetCoordinateSystem(ProjectionCoordinateSystem projection)
        {
            string wkt = null;

            if (!ProjectionHelper.CoordinateSystems.ContainsKey(projection))
                {
                switch (projection)
                {
                    case ProjectionCoordinateSystem.EPSG2926:
                        wkt =
                            @"PROJCS[""NAD83(HARN) / Washington North(ftUS)"",
                            GEOGCS[""NAD83(HARN)"",
                                DATUM[""NAD83_High_Accuracy_Reference_Network"",
                                    SPHEROID[""GRS 1980"", 6378137, 298.257222101,
                                        AUTHORITY[""EPSG"", ""7019""]],
                                    TOWGS84[0, 0, 0, 0, 0, 0, 0],
                                    AUTHORITY[""EPSG"", ""6152""]],
                                PRIMEM[""Greenwich"", 0,
                                    AUTHORITY[""EPSG"", ""8901""]],
                                UNIT[""degree"", 0.0174532925199433,
                                    AUTHORITY[""EPSG"", ""9122""]],
                                AUTHORITY[""EPSG"", ""4152""]],
                            PROJECTION[""Lambert_Conformal_Conic_2SP""],
                            PARAMETER[""standard_parallel_1"", 48.73333333333333],
                            PARAMETER[""standard_parallel_2"", 47.5],
                            PARAMETER[""latitude_of_origin"", 47],
                            PARAMETER[""central_meridian"", -120.8333333333333],
                            PARAMETER[""false_easting"", 1640416.667],
                            PARAMETER[""false_northing"", 0],
                            UNIT[""US survey foot"", 0.3048006096012192,
                                AUTHORITY[""EPSG"", ""9003""]],
                            AXIS[""X"", EAST],
                            AXIS[""Y"", NORTH],
                            AUTHORITY[""EPSG"", ""2926""]]";
                        break;
                    case ProjectionCoordinateSystem.EPSG4326:
                        wkt =
                            @"GEOGCS[""WGS 84"",
                            DATUM[""WGS_1984"",
                                SPHEROID[""WGS 84"", 6378137, 298.257223563,
                                    AUTHORITY[""EPSG"", ""7030""]],
                                AUTHORITY[""EPSG"", ""6326""]],
                            PRIMEM[""Greenwich"", 0,
                                AUTHORITY[""EPSG"", ""8901""]],
                            UNIT[""degree"", 0.0174532925199433,
                                AUTHORITY[""EPSG"", ""9122""]],
                            AUTHORITY[""EPSG"", ""4326""]]";
                        break;
                }

                lock (ProjectionHelper.CoordinateSystems)
                {
                    ICoordinateSystem cs =
                        CoordinateSystemWktReader.Parse(wkt, System.Text.Encoding.UTF8) as GeoAPI.CoordinateSystems.ICoordinateSystem;

                    ProjectionHelper.CoordinateSystems[projection] = cs;

                    return cs;
                }
            }

            return ProjectionHelper.CoordinateSystems[projection];
        }
    }
}
