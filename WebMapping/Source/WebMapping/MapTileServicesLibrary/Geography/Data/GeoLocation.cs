namespace PTASMapTileServicesLibrary.Geography.Data
{
    /// <summary>
    /// Describes a geographic location.
    /// </summary>
    public class GeoLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocation"/> class.
        /// </summary>
        /// <param name="lon">The lon.</param>
        /// <param name="lat">The lat.</param>
        public GeoLocation(double lon, double lat)
        {
            this.Lon = lon;
            this.Lat = lat;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocation"/> class.
        /// </summary>
        public GeoLocation()
        {
            this.Lon = 0;
            this.Lat = 0;
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Lon { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Lat { get; set; }
    }
}
