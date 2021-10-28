namespace PTASMapTileServicesLibrary.Geography.Data
{
    /// <summary>
    /// Describes an spatial extent.
    /// </summary>
    public class Extent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Extent"/> class.
        /// </summary>
        /// <param name="minLon">The minimum x.</param>
        /// <param name="minLat">The minimum y.</param>
        /// <param name="maxLon">The maximum x.</param>
        /// <param name="maxLat">The maximum y.</param>
        public Extent(double minLon, double minLat, double maxLon, double maxLat)
        {
            this.Min = new GeoLocation(minLon, minLat);
            this.Max = new GeoLocation(maxLon, maxLat);
            this.Sort();
        }

        /// <summary>
        /// Gets or sets the min location of the extent.
        /// </summary>
        public GeoLocation Min { get; set; }

        /// <summary>
        /// Gets or sets the max location of the extent.
        /// </summary>
        public GeoLocation Max { get; set; }

        /// <summary>
        /// Sorts the min/max values.
        /// </summary>
        public void Sort()
        {
            if (this.Min.Lon > this.Max.Lon)
            {
                double tmp = this.Min.Lon;
                this.Min.Lon = this.Max.Lon;
                this.Max.Lon = tmp;
            }

            if (this.Min.Lat > this.Max.Lat)
            {
                double tmp = this.Min.Lat;
                this.Min.Lat = this.Max.Lat;
                this.Max.Lat = tmp;
            }
        }
    }
}
